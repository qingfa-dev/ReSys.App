#!/usr/bin/env bash
set -euo pipefail

# ============================================================================
# clean.sh — Remove build artifacts and coverage data
# ============================================================================
# Usage:
#   ./scripts/clean.sh          Remove test-results, coverage
#   ./scripts/clean.sh --all    Also remove obj/bin directories
# ============================================================================

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
ROOT_DIR="$(cd "$SCRIPT_DIR/.." && pwd)"

GREEN='\033[32m'
RESET='\033[0m'

SOLUTION="ReSys.App.slnx"
VERBOSITY="quiet"

ok() { printf "${GREEN}✔ %s${RESET}\n" "$1"; }

# ---------------------------------------------------------------------------
# Commands
# ---------------------------------------------------------------------------

cmd_clean() {
    dotnet clean "$ROOT_DIR/$SOLUTION" --verbosity "$VERBOSITY" 2>/dev/null || true
    rm -rf "$ROOT_DIR/test-results/"
    rm -rf "$ROOT_DIR/coverage-report/"
    find "$ROOT_DIR/tests/" -name coverage -type d -exec rm -rf {} + 2>/dev/null || true
    find "$ROOT_DIR/tests/" -name "*.cobertura.xml" -delete 2>/dev/null || true
    find "$ROOT_DIR/tests/" -name "*.json" -path "*/coverage/*" -delete 2>/dev/null || true
    ok "Cleaned."
}

cmd_clean_all() {
    cmd_clean
    find "$ROOT_DIR" -type d -name obj -exec rm -rf {} + 2>/dev/null || true
    find "$ROOT_DIR" -type d -name bin -exec rm -rf {} + 2>/dev/null || true
    ok "Deep cleaned (obj/bin removed)."
}

# ---------------------------------------------------------------------------
# Dispatch
# ---------------------------------------------------------------------------
command="${1:-}"

case "$command" in
    --all|-a)  cmd_clean_all ;;
    *)         cmd_clean ;;
esac
