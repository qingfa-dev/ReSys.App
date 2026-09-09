#!/usr/bin/env bash
set -euo pipefail

# ============================================================================
# test.sh — Test runners
# ============================================================================
# Usage:
#   ./scripts/test.sh all [config]
#   ./scripts/test.sh unit [config]
#   ./scripts/test.sh int [config]
#   ./scripts/test.sh arch [config]
#   ./scripts/test.sh filter <expression> [config]
# ============================================================================

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
ROOT_DIR="$(cd "$SCRIPT_DIR/.." && pwd)"

CYAN='\033[36m'
GREEN='\033[32m'
RED='\033[31m'
RESET='\033[0m'

SOLUTION="ReSys.App.slnx"
VERBOSITY="quiet"

log()  { printf "${CYAN}▶ %s${RESET}\n" "$1"; }
ok()   { printf "${GREEN}✔ %s${RESET}\n" "$1"; }
fail() { printf "${RED}✘ %s${RESET}\n" "$1"; exit 1; }

run_test() {
    local project="$1"
    local config="${2:-Debug}"
    log "Running $project..."
    dotnet test "$project" \
        --configuration "$config" \
        --verbosity "$VERBOSITY" \
        --no-build
}

# ---------------------------------------------------------------------------
# Commands
# ---------------------------------------------------------------------------

cmd_all() {
    local config="${1:-Debug}"
    dotnet test "$ROOT_DIR/$SOLUTION" \
        --configuration "$config" \
        --verbosity "$VERBOSITY" \
        --no-build
    ok "All tests passed."
}

cmd_unit() {
    local config="${1:-Debug}"
    local projects
    projects=$(find "$ROOT_DIR/tests" -maxdepth 2 -name "*.UnitTests.csproj" -o -name "*.Tests.csproj" 2>/dev/null || true)
    if [[ -z "$projects" ]]; then
        fail "No unit test projects found."
    fi
    while IFS= read -r proj; do
        run_test "$proj" "$config"
    done <<< "$projects"
    ok "Unit tests passed."
}

cmd_int() {
    local config="${1:-Debug}"
    local projects
    projects=$(find "$ROOT_DIR/tests" -maxdepth 2 -name "*.IntegrationTests.csproj" 2>/dev/null || true)
    if [[ -z "$projects" ]]; then
        fail "No integration test projects found."
    fi
    while IFS= read -r proj; do
        run_test "$proj" "$config"
    done <<< "$projects"
    ok "Integration tests passed."
}

cmd_arch() {
    local config="${1:-Debug}"
    dotnet test "$ROOT_DIR/$SOLUTION" \
        --configuration "$config" \
        --verbosity "$VERBOSITY" \
        --no-build \
        --filter "Category=Architecture"
    ok "Architecture tests passed."
}

cmd_filter() {
    local expression="${1:?Usage: $0 filter <expression> [config]}"
    local config="${2:-Debug}"
    dotnet test "$ROOT_DIR/$SOLUTION" \
        --configuration "$config" \
        --verbosity "$VERBOSITY" \
        --no-build \
        --filter "$expression"
    ok "Filtered tests passed."
}

# ---------------------------------------------------------------------------
# Dispatch
# ---------------------------------------------------------------------------
command="${1:-help}"
shift || true

case "$command" in
    all)    cmd_all "$@" ;;
    unit)   cmd_unit "$@" ;;
    int)    cmd_int "$@" ;;
    arch)   cmd_arch "$@" ;;
    filter) cmd_filter "$@" ;;
    *)
        echo "Usage: $0 {all|unit|int|arch|filter <expr>} [config]"
        exit 1
        ;;
esac
