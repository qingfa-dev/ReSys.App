#!/usr/bin/env bash
set -euo pipefail

# ============================================================================
# format.sh — Linting and code formatting
# ============================================================================
# Usage:
#   ./scripts/format.sh lint          Run Roslyn analyzers
#   ./scripts/format.sh check         Verify .editorconfig formatting
#   ./scripts/format.sh fix           Auto-fix formatting
#   ./scripts/format.sh styled        Style analysis with severity info
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

# ---------------------------------------------------------------------------
# Commands
# ---------------------------------------------------------------------------

cmd_lint() {
    log "Running Roslyn analyzers..."
    dotnet build "$ROOT_DIR/$SOLUTION" \
        --configuration Debug \
        --verbosity "$VERBOSITY" \
        -p:RunAnalyzers=true \
        -p:RunAnalyzersDuringBuild=true
    ok "Lint passed."
}

cmd_check() {
    log "Checking code formatting..."
    dotnet format "$ROOT_DIR/$SOLUTION" \
        --verify-no-changes \
        --verbosity diagnostic
    ok "Format check passed."
}

cmd_fix() {
    log "Auto-fixing code formatting..."
    dotnet format "$ROOT_DIR/$SOLUTION" \
        --verbosity normal
    ok "Formatting fixed."
}

cmd_styled() {
    log "Running style analysis..."
    dotnet format "$ROOT_DIR/$SOLUTION" \
        --verify-no-changes \
        --include \
        --severity info \
        --verbosity diagnostic
    ok "Style check passed."
}

# ---------------------------------------------------------------------------
# Dispatch
# ---------------------------------------------------------------------------
command="${1:-help}"
shift || true

case "$command" in
    lint)   cmd_lint "$@" ;;
    check)  cmd_check "$@" ;;
    fix)    cmd_fix "$@" ;;
    styled) cmd_styled "$@" ;;
    *)
        echo "Usage: $0 {lint|check|fix|styled}"
        exit 1
        ;;
esac
