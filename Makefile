# ============================================================================
# ReSys.App - Build & Test Automation
# ============================================================================
#
# Usage:
#   make help          Show available targets
#   make build         Build solution (Debug)
#   make test          Run all tests
#   make ci            Full CI pipeline
#
# Coverage reports land in each test project's coverage/ directory.
# Generate HTML with: `make coverage-html`
# ============================================================================

SHELL := /bin/bash
.DEFAULT_GOAL := help

# ---------------------------------------------------------------------------
# Configuration
# ---------------------------------------------------------------------------
SOLUTION       := ReSys.App.slnx
CONFIGURATION  ?= Debug
DOTNET         := dotnet
VERBOSITY      ?= quiet

# Test projects (discovered by convention)
UNIT_TEST_PROJECTS   := $(wildcard tests/*.UnitTests/*.csproj)
INT_TEST_PROJECTS    := $(wildcard tests/*.IntegrationTests/*.csproj)
ALL_TEST_PROJECTS    := $(wildcard tests/**/*.csproj)

# Coverage settings
COVERAGE_FORMAT      := cobertura,json
INCLUDE_BRANCH_COVERAGE ?= true

# Report output
COVERAGE_REPORT_DIR  := coverage-report

# ---------------------------------------------------------------------------
# Color helpers
# ---------------------------------------------------------------------------
CYAN  := \033[36m
GREEN := \033[32m
RESET := \033[0m

# ============================================================================
# GENERAL
# ============================================================================

.PHONY: help
help: ## Show this help
	@printf "\n$(CYAN)ReSys.App$(RESET) — make targets:\n\n"
	@grep -E '^[a-zA-Z_-]+:.*?## .*$$' $(MAKEFILE_LIST) | \
		awk 'BEGIN {FS = ":.*?## "}; {printf "  $(GREEN)%-24s$(RESET) %s\n", $$1, $$2}'
	@printf "\n"

.PHONY: restore
restore: ## Restore NuGet packages
	$(DOTNET) restore $(SOLUTION) --verbosity $(VERBOSITY)

.PHONY: build
build: ## Build solution (Debug)
	$(DOTNET) build $(SOLUTION) --configuration $(CONFIGURATION) --verbosity $(VERBOSITY)

.PHONY: build-release
build-release: ## Build solution (Release)
	$(DOTNET) build $(SOLUTION) --configuration Release --verbosity $(VERBOSITY)

.PHONY: clean
clean: ## Remove build artifacts and coverage data
	$(DOTNET) clean $(SOLUTION) --verbosity $(VERBOSITY) 2>/dev/null || true
	rm -rf test-results/
	rm -rf $(COVERAGE_REPORT_DIR)/
	find tests/ -name coverage -type d -exec rm -rf {} + 2>/dev/null || true
	find tests/ -name "*.cobertura.xml" -delete 2>/dev/null || true
	find tests/ -name "*.json" -path "*/coverage/*" -delete 2>/dev/null || true
	@printf "$(GREEN)Cleaned.$(RESET)\n"

.PHONY: clean-all
clean-all: clean ## Remove all artifacts including obj/bin
	find . -type d -name obj -exec rm -rf {} + 2>/dev/null || true
	find . -type d -name bin -exec rm -rf {} + 2>/dev/null || true
	@printf "$(GREEN)Deep cleaned.$(RESET)\n"

# ============================================================================
# TESTING
# ============================================================================

.PHONY: test
test: ## Run all tests
	$(DOTNET) test $(SOLUTION) \
		--configuration $(CONFIGURATION) \
		--verbosity $(VERBOSITY) \
		--no-build

.PHONY: test-quick
test-quick: build ## Build then run all tests (no coverage)
	$(DOTNET) test $(SOLUTION) \
		--configuration $(CONFIGURATION) \
		--verbosity $(VERBOSITY)

.PHONY: test-unit
test-unit: build ## Run unit tests only
	@for proj in $(UNIT_TEST_PROJECTS); do \
		printf "\n$(CYAN)▶ Running $$proj$(RESET)\n"; \
		$(DOTNET) test "$$proj" \
			--configuration $(CONFIGURATION) \
			--verbosity $(VERBOSITY) \
			--no-build; \
	done

.PHONY: test-int
test-int: build ## Run integration tests only
	@for proj in $(INT_TEST_PROJECTS); do \
		printf "\n$(CYAN)▶ Running $$proj$(RESET)\n"; \
		$(DOTNET) test "$$proj" \
			--configuration $(CONFIGURATION) \
			--verbosity $(VERBOSITY) \
			--no-build; \
	done

.PHONY: test-arch
test-arch: build ## Run architecture tests (NetArchTest)
	$(DOTNET) test $(SOLUTION) \
		--configuration $(CONFIGURATION) \
		--verbosity $(VERBOSITY) \
		--no-build \
		--filter "Category=Architecture"

.PHONY: test-filter
test-filter: build ## Run tests matching a filter (usage: make test-filter FILTER="ClassName=HealthTests")
	$(DOTNET) test $(SOLUTION) \
		--configuration $(CONFIGURATION) \
		--verbosity $(VERBOSITY) \
		--no-build \
		--filter "$(FILTER)"

# ============================================================================
# CODE COVERAGE
# ============================================================================

.PHONY: coverage
coverage: build ## Run ALL tests with line + branch coverage
	$(DOTNET) test $(SOLUTION) \
		--configuration $(CONFIGURATION) \
		--verbosity $(VERBOSITY) \
		--no-build \
		/p:CollectCoverage=true \
		/p:CoverletOutputFormat=$(COVERAGE_FORMAT) \
		/p:IncludeBranchCoverage=$(INCLUDE_BRANCH_COVERAGE) \
		/p:CoverletOutput="$(MSBuildProjectDirectory)/coverage/"

.PHONY: coverage-unit
coverage-unit: build ## Run unit tests with coverage
	@for proj in $(UNIT_TEST_PROJECTS); do \
		printf "\n$(CYAN)▶ Coverage: $$proj$(RESET)\n"; \
		$(DOTNET) test "$$proj" \
			--configuration $(CONFIGURATION) \
			--verbosity $(VERBOSITY) \
			--no-build \
			/p:CollectCoverage=true \
			/p:CoverletOutputFormat=$(COVERAGE_FORMAT) \
			/p:IncludeBranchCoverage=$(INCLUDE_BRANCH_COVERAGE) \
			/p:CoverletOutput="$$(dirname "$$proj")/coverage/"; \
	done

.PHONY: coverage-int
coverage-int: build ## Run integration tests with coverage
	@for proj in $(INT_TEST_PROJECTS); do \
		printf "\n$(CYAN)▶ Coverage: $$proj$(RESET)\n"; \
		$(DOTNET) test "$$proj" \
			--configuration $(CONFIGURATION) \
			--verbosity $(VERBOSITY) \
			--no-build \
			/p:CollectCoverage=true \
			/p:CoverletOutputFormat=$(COVERAGE_FORMAT) \
			/p:IncludeBranchCoverage=$(INCLUDE_BRANCH_COVERAGE) \
			/p:CoverletOutput="$$(dirname "$$proj")/coverage/"; \
	done

.PHONY: coverage-html
coverage-html: coverage ## Generate HTML coverage report (installs tool if missing)
	@mkdir -p $(COVERAGE_REPORT_DIR)/html
	$(DOTNET) tool list -g | grep -q reportgenerator || \
		$(DOTNET) tool install -g dotnet-reportgenerator-globaltool
	reportgenerator \
		"-reports:tests/**/coverage.cobertura.xml" \
		"-targetdir:$(COVERAGE_REPORT_DIR)/html" \
		"-reporttypes:HtmlInline_AzurePipelines;MarkdownSummaryGithub;Cobertura;JsonSummary"
	@printf "$(GREEN)✔ HTML report → $(COVERAGE_REPORT_DIR)/html/index.html$(RESET)\n"

.PHONY: coverage-enforce
coverage-enforce: build ## Run tests + fail if below threshold (usage: make coverage-enforce THRESHOLD=80)
	$(DOTNET) test $(SOLUTION) \
		--configuration $(CONFIGURATION) \
		--verbosity $(VERBOSITY) \
		--no-build \
		/p:CollectCoverage=true \
		/p:CoverletOutputFormat=$(COVERAGE_FORMAT) \
		/p:IncludeBranchCoverage=$(INCLUDE_BRANCH_COVERAGE) \
		/p:CoverletThreshold=$(THRESHOLD) \
		/p:CoverletThresholdType=line \
		/p:CoverletOutput="$(MSBuildProjectDirectory)/coverage/"

# ============================================================================
# CODE QUALITY
# ============================================================================

.PHONY: lint
lint: restore ## Run Roslyn analyzers
	$(DOTNET) build $(SOLUTION) \
		--configuration $(CONFIGURATION) \
		--verbosity $(VERBOSITY) \
		-p:RunAnalyzers=true \
		-p:RunAnalyzersDuringBuild=true

.PHONY: format
format: ## Check code formatting (.editorconfig)
	$(DOTNET) format $(SOLUTION) \
		--verify-no-changes \
		--verbosity diagnostic

.PHONY: format-fix
format-fix: ## Auto-fix code formatting
	$(DOTNET) format $(SOLUTION) \
		--verbosity normal

.PHONY: format-styled
format-styled: ## Format with explicit style analysis
	$(DOTNET) format $(SOLUTION) \
		--verify-no-changes \
		--include \
		--severity info \
		--verbosity diagnostic

# ============================================================================
# CI PIPELINES
# ============================================================================

.PHONY: ci
ci: restore build coverage coverage-html ## Full CI: restore → build → test+coverage → HTML report
	@printf "\n$(GREEN)✔ CI pipeline completed.$(RESET)\n"

.PHONY: ci-fast
ci-fast: restore build test ## Fast CI: restore → build → test (no coverage)
	@printf "\n$(GREEN)✔ Fast CI completed.$(RESET)\n"

.PHONY: ci-strict
ci-strict: restore build coverage-enforce coverage-html lint ## Strict CI: build + enforce coverage + HTML + lint
	@printf "\n$(GREEN)✔ Strict CI completed (threshold: $(COVERAGE_THRESHOLD)%%).$(RESET)\n"

.PHONY: ci-local
ci-local: ## Simulate full CI locally (uses Release config)
	$(MAKE) clean-all
	CI=true $(MAKE) ci CONFIGURATION=Release
	@printf "\n$(GREEN)✔ Local CI simulation completed.$(RESET)\n"

.PHONY: ci-unit
ci-unit: restore build coverage-unit ## CI for unit tests only
	@printf "\n$(GREEN)✔ Unit CI completed.$(RESET)\n"

.PHONY: ci-int
ci-int: restore build coverage-int ## CI for integration tests only
	@printf "\n$(GREEN)✔ Integration CI completed.$(RESET)\n"

# ============================================================================
# UTILITIES
# ============================================================================

.PHONY: info
info: ## Show project and tool info
	@echo "Solution:    $(SOLUTION)"
	@echo "SDK:         $(shell $(DOTNET) --version 2>/dev/null || echo 'not found')"
	@echo "Config:      $(CONFIGURATION)"
	@echo "Unit tests:  $(UNIT_TEST_PROJECTS)"
	@echo "Int tests:   $(INT_TEST_PROJECTS)"
	@echo "Coverage:    $(COVERAGE_FORMAT) (branch=$(INCLUDE_BRANCH_COVERAGE))"

.PHONY: watch
watch: ## Watch for file changes and rebuild
	$(DOTNET) watch build --project src/Api/Api.csproj

.PHONY: run
run: build ## Run the API project
	$(DOTNET) run --project src/Api/Api.csproj

.PHONY: run-aspire
run-aspire: ## Run with Aspire orchestrator
	$(DOTNET) run --project src/AppHost/AppHost.csproj

.PHONY: outdated
outdated: ## Check for outdated NuGet packages
	$(DOTNET) list $(SOLUTION) package --outdated

.PHONY: lock
lock: ## Regenerate package lock files
	$(DOTNET) restore $(SOLUTION) --force-evaluate

.PHONY: tool-restore
tool-restore: ## Restore dotnet tools (if .config/dotnet-tools.json exists)
	@test -f .config/dotnet-tools.json && $(DOTNET) tool restore || echo "No tool manifest found."
