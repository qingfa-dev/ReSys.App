#!/usr/bin/env bash
set -euo pipefail

# ============================================================================
# run.sh — Run the application
# ============================================================================
# Usage:
#   ./scripts/run.sh api              Run the API project
#   ./scripts/run.sh aspire           Run with Aspire orchestrator
#   ./scripts/run.sh watch            Watch + rebuild on change
# ============================================================================

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
ROOT_DIR="$(cd "$SCRIPT_DIR/.." && pwd)"

CYAN='\033[36m'
GREEN='\033[32m'
RED='\033[31m'
RESET='\033[0m'

log()  { printf "${CYAN}▶ %s${RESET}\n" "$1"; }
ok()   { printf "${GREEN}✔ %s${RESET}\n" "$1"; }
fail() { printf "${RED}✘ %s${RESET}\n" "$1"; exit 1; }

# ---------------------------------------------------------------------------
# Commands
# ---------------------------------------------------------------------------

cmd_api() {
    log "Running API..."
    dotnet run --project "$ROOT_DIR/src/Api/Api.csproj"
}

cmd_aspire() {
    log "Running Aspire orchestrator..."
    dotnet run --project "$ROOT_DIR/src/AppHost/AppHost.csproj"
}

cmd_watch() {
    log "Watching API for changes..."
    dotnet watch build --project "$ROOT_DIR/src/Api/Api.csproj"
}

# ---------------------------------------------------------------------------
# Dispatch
# ---------------------------------------------------------------------------
command="${1:-help}"
shift || true

case "$command" in
    api)    cmd_api "$@" ;;
    aspire) cmd_aspire "$@" ;;
    watch)  cmd_watch "$@" ;;
    *)
        echo "Usage: $0 {api|aspire|watch}"
        exit 1
        ;;
esac
