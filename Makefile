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

SCRIPTS := $(shell pwd)/scripts

# ---------------------------------------------------------------------------
# Configuration (override via: make build CONFIGURATION=Release)
# ---------------------------------------------------------------------------
CONFIGURATION ?= Debug
THRESHOLD     ?= 80
FILTER        ?=

# ============================================================================
# GENERAL
# ============================================================================

.PHONY: help
help: ## Show this help
	@printf "\n\033[36mReSys.App\033[0m — make targets:\n\n"
	@grep -E '^[a-zA-Z_-]+:.*?## .*$$' $(MAKEFILE_LIST) | \
		awk 'BEGIN {FS = ":.*?## "}; {printf "  \033[32m%-24s\033[0m %s\n", $$1, $$2}'
	@printf "\n"

.PHONY: restore
restore: ## Restore NuGet packages
	$(SCRIPTS)/build.sh restore

.PHONY: build
build: ## Build solution (Debug)
	$(SCRIPTS)/build.sh build $(CONFIGURATION)

.PHONY: build-release
build-release: ## Build solution (Release)
	$(SCRIPTS)/build.sh build Release

.PHONY: clean
clean: ## Remove build artifacts and coverage data
	$(SCRIPTS)/clean.sh

.PHONY: clean-all
clean-all: ## Remove all artifacts including obj/bin
	$(SCRIPTS)/clean.sh --all

# ============================================================================
# TESTING
# ============================================================================

.PHONY: test
test: ## Run all tests
	$(SCRIPTS)/test.sh all $(CONFIGURATION)

.PHONY: test-quick
test-quick: build ## Build then run all tests (no coverage)
	$(SCRIPTS)/test.sh all $(CONFIGURATION)

.PHONY: test-unit
test-unit: build ## Run unit tests only
	$(SCRIPTS)/test.sh unit $(CONFIGURATION)

.PHONY: test-int
test-int: build ## Run integration tests only
	$(SCRIPTS)/test.sh int $(CONFIGURATION)

.PHONY: test-arch
test-arch: build ## Run architecture tests (NetArchTest)
	$(SCRIPTS)/test.sh arch $(CONFIGURATION)

.PHONY: test-filter
test-filter: build ## Run tests matching a filter (usage: make test-filter FILTER="ClassName=HealthTests")
	$(SCRIPTS)/test.sh filter "$(FILTER)" $(CONFIGURATION)

# ============================================================================
# CODE COVERAGE
# ============================================================================

.PHONY: coverage
coverage: build ## Run ALL tests with line + branch coverage
	$(SCRIPTS)/coverage.sh all $(CONFIGURATION)

.PHONY: coverage-unit
coverage-unit: build ## Run unit tests with coverage
	$(SCRIPTS)/coverage.sh unit $(CONFIGURATION)

.PHONY: coverage-int
coverage-int: build ## Run integration tests with coverage
	$(SCRIPTS)/coverage.sh int $(CONFIGURATION)

.PHONY: coverage-html
coverage-html: coverage ## Generate HTML coverage report (installs tool if missing)
	$(SCRIPTS)/coverage.sh html

.PHONY: coverage-enforce
coverage-enforce: build ## Run tests + fail if below threshold (usage: make coverage-enforce THRESHOLD=80)
	$(SCRIPTS)/coverage.sh enforce $(THRESHOLD) $(CONFIGURATION)

# ============================================================================
# CODE QUALITY
# ============================================================================

.PHONY: lint
lint: restore ## Run Roslyn analyzers
	$(SCRIPTS)/format.sh lint

.PHONY: format
format: ## Check code formatting (.editorconfig)
	$(SCRIPTS)/format.sh check

.PHONY: format-fix
format-fix: ## Auto-fix code formatting
	$(SCRIPTS)/format.sh fix

.PHONY: format-styled
format-styled: ## Format with explicit style analysis
	$(SCRIPTS)/format.sh styled

# ============================================================================
# CI PIPELINES
# ============================================================================

.PHONY: ci
ci: restore build coverage coverage-html ## Full CI: restore → build → test+coverage → HTML report
	@printf "\n\033[32m✔ CI pipeline completed.\033[0m\n"

.PHONY: ci-fast
ci-fast: restore build test ## Fast CI: restore → build → test (no coverage)
	@printf "\n\033[32m✔ Fast CI completed.\033[0m\n"

.PHONY: ci-strict
ci-strict: restore build coverage-enforce coverage-html lint ## Strict CI: build + enforce coverage + HTML + lint
	@printf "\n\033[32m✔ Strict CI completed.\033[0m\n"

.PHONY: ci-local
ci-local: ## Simulate full CI locally (uses Release config)
	$(SCRIPTS)/ci.sh local

.PHONY: ci-unit
ci-unit: restore build coverage-unit ## CI for unit tests only
	@printf "\n\033[32m✔ Unit CI completed.\033[0m\n"

.PHONY: ci-int
ci-int: restore build coverage-int ## CI for integration tests only
	@printf "\n\033[32m✔ Integration CI completed.\033[0m\n"

# ============================================================================
# UTILITIES
# ============================================================================

.PHONY: info
info: ## Show project and tool info
	$(SCRIPTS)/info.sh info

.PHONY: watch
watch: ## Watch for file changes and rebuild
	$(SCRIPTS)/run.sh watch

.PHONY: run
run: build ## Run the API project
	$(SCRIPTS)/run.sh api

.PHONY: run-aspire
run-aspire: ## Run with Aspire orchestrator
	$(SCRIPTS)/run.sh aspire

.PHONY: outdated
outdated: ## Check for outdated NuGet packages
	$(SCRIPTS)/info.sh outdated

.PHONY: lock
lock: ## Regenerate package lock files
	$(SCRIPTS)/info.sh lock

.PHONY: tool-restore
tool-restore: ## Restore dotnet tools (if .config/dotnet-tools.json exists)
	$(SCRIPTS)/info.sh tool-restore
