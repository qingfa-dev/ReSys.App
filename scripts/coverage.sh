#!/usr/bin/env bash
set -euo pipefail

# ============================================================================
# coverage.sh — Code coverage collection and reporting
# ============================================================================
# Usage:
#   ./scripts/coverage.sh all   [config]        All tests + branch coverage
#   ./scripts/coverage.sh unit  [config]        Unit tests only
#   ./scripts/coverage.sh int   [config]        Integration tests only
#   ./scripts/coverage.sh html                    Generate HTML report
#   ./scripts/coverage.sh enforce <threshold> [config]  Fail below threshold %
# ============================================================================

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
ROOT_DIR="$(cd "$SCRIPT_DIR/.." && pwd)"

CYAN='\033[36m'
GREEN='\033[32m'
RED='\033[31m'
RESET='\033[0m'

SOLUTION="ReSys.App.slnx"
VERBOSITY="quiet"
FORMAT="cobertura,json"
BRANCH="true"
REPORT_DIR="coverage-report"

log()  { printf "${CYAN}▶ %s${RESET}\n" "$1"; }
ok()   { printf "${GREEN}✔ %s${RESET}\n" "$1"; }
fail() { printf "${RED}✘ %s${RESET}\n" "$1"; exit 1; }

cov_props() {
    local out_dir="$1"
    printf "/p:CollectCoverage=true /p:CoverletOutputFormat=%s /p:IncludeBranchCoverage=%s /p:CoverletOutput=%s/coverage/" \
        "$FORMAT" "$BRANCH" "$out_dir"
}

# ---------------------------------------------------------------------------
# Commands
# ---------------------------------------------------------------------------

cmd_all() {
    local config="${1:-Debug}"
    log "Running all tests with coverage..."
    # shellcheck disable=SC2046
    dotnet test "$ROOT_DIR/$SOLUTION" \
        --configuration "$config" \
        --verbosity "$VERBOSITY" \
        --no-build \
        /p:CollectCoverage=true \
        /p:CoverletOutputFormat="$FORMAT" \
        /p:IncludeBranchCoverage="$BRANCH" \
        /p:CoverletOutput="$ROOT_DIR/coverage/"
    ok "Coverage collected → tests/*/coverage/"
}

cmd_unit() {
    local config="${1:-Debug}"
    local projects
    projects=$(find "$ROOT_DIR/tests" -maxdepth 2 -name "*.UnitTests.csproj" -o -name "*.Tests.csproj" 2>/dev/null || true)
    if [[ -z "$projects" ]]; then
        fail "No unit test projects found."
    fi
    while IFS= read -r proj; do
        local dir
        dir=$(dirname "$proj")
        log "Coverage: $proj"
        dotnet test "$proj" \
            --configuration "$config" \
            --verbosity "$VERBOSITY" \
            --no-build \
            /p:CollectCoverage=true \
            /p:CoverletOutputFormat="$FORMAT" \
            /p:IncludeBranchCoverage="$BRANCH" \
            /p:CoverletOutput="$dir/coverage/"
    done <<< "$projects"
    ok "Unit coverage collected."
}

cmd_int() {
    local config="${1:-Debug}"
    local projects
    projects=$(find "$ROOT_DIR/tests" -maxdepth 2 -name "*.IntegrationTests.csproj" 2>/dev/null || true)
    if [[ -z "$projects" ]]; then
        fail "No integration test projects found."
    fi
    while IFS= read -r proj; do
        local dir
        dir=$(dirname "$proj")
        log "Coverage: $proj"
        dotnet test "$proj" \
            --configuration "$config" \
            --verbosity "$VERBOSITY" \
            --no-build \
            /p:CollectCoverage=true \
            /p:CoverletOutputFormat="$FORMAT" \
            /p:IncludeBranchCoverage="$BRANCH" \
            /p:CoverletOutput="$dir/coverage/"
    done <<< "$projects"
    ok "Integration coverage collected."
}

cmd_html() {
    log "Generating HTML coverage report..."
    mkdir -p "$ROOT_DIR/$REPORT_DIR/html"

    # Install reportgenerator if missing
    if ! dotnet tool list -g 2>/dev/null | grep -q reportgenerator; then
        log "Installing dotnet-reportgenerator-globaltool..."
        dotnet tool install -g dotnet-reportgenerator-globaltool
    fi

    reportgenerator \
        "-reports:$ROOT_DIR/tests/**/coverage.cobertura.xml" \
        "-targetdir:$ROOT_DIR/$REPORT_DIR/html" \
        "-reporttypes:HtmlInline_AzurePipelines;MarkdownSummaryGithub;Cobertura;JsonSummary"

    ok "HTML report → $REPORT_DIR/html/index.html"
}

cmd_enforce() {
    local threshold="${1:?Usage: $0 enforce <threshold-percent> [config]}"
    local config="${2:-Debug}"
    log "Running tests with coverage (min ${threshold}%)..."
    dotnet test "$ROOT_DIR/$SOLUTION" \
        --configuration "$config" \
        --verbosity "$VERBOSITY" \
        --no-build \
        /p:CollectCoverage=true \
        /p:CoverletOutputFormat="$FORMAT" \
        /p:IncludeBranchCoverage="$BRANCH" \
        /p:CoverletThreshold="$threshold" \
        /p:CoverletThresholdType=line \
        /p:CoverletOutput="$ROOT_DIR/coverage/"
    ok "Coverage enforced (≥ ${threshold}%)."
}

# ---------------------------------------------------------------------------
# Dispatch
# ---------------------------------------------------------------------------
command="${1:-help}"
shift || true

case "$command" in
    all)     cmd_all "$@" ;;
    unit)    cmd_unit "$@" ;;
    int)     cmd_int "$@" ;;
    html)    cmd_html "$@" ;;
    enforce) cmd_enforce "$@" ;;
    *)
        echo "Usage: $0 {all|unit|int|html|enforce <threshold>}"
        exit 1
        ;;
esac
