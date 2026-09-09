#!/usr/bin/env bash
set -euo pipefail

# ============================================================================
# info.sh — Project info and utilities
# ============================================================================
# Usage:
#   ./scripts/info.sh              Show project and tool info
#   ./scripts/info.sh outdated     Check for outdated NuGet packages
#   ./scripts/info.sh lock         Regenerate package lock files
#   ./scripts/info.sh tool-restore Restore dotnet tools
# ============================================================================

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
ROOT_DIR="$(cd "$SCRIPT_DIR/.." && pwd)"

CYAN='\033[36m'
GREEN='\033[32m'
RESET='\033[0m'

SOLUTION="ReSys.App.slnx"

log()  { printf "${CYAN}▶ %s${RESET}\n" "$1"; }
ok()   { printf "${GREEN}✔ %s${RESET}\n" "$1"; }

# ---------------------------------------------------------------------------
# Commands
# ---------------------------------------------------------------------------

cmd_info() {
    local sdk_version
    sdk_version=$(dotnet --version 2>/dev/null || echo "not found")

    local unit_tests
    unit_tests=$(find "$ROOT_DIR/tests" -maxdepth 2 -name "*.UnitTests.csproj" -o -name "*.Tests.csproj" 2>/dev/null | wc -l)

    local int_tests
    int_tests=$(find "$ROOT_DIR/tests" -maxdepth 2 -name "*.IntegrationTests.csproj" 2>/dev/null | wc -l)

    echo ""
    echo "  Solution:    $SOLUTION"
    echo "  SDK:         $sdk_version"
    echo "  Unit tests:  $unit_tests project(s)"
    echo "  Int tests:   $int_tests project(s)"
    echo "  Coverage:    cobertura,json (branch=true)"
    echo ""
}

cmd_outdated() {
    log "Checking outdated packages..."
    dotnet list "$ROOT_DIR/$SOLUTION" package --outdated
}

cmd_lock() {
    log "Regenerating lock files..."
    dotnet restore "$ROOT_DIR/$SOLUTION" --force-evaluate
    ok "Lock files regenerated."
}

cmd_tool_restore() {
    if [[ -f "$ROOT_DIR/.config/dotnet-tools.json" ]]; then
        log "Restoring dotnet tools..."
        dotnet tool restore
        ok "Tools restored."
    else
        echo "No .config/dotnet-tools.json found."
    fi
}

# ---------------------------------------------------------------------------
# Dispatch
# ---------------------------------------------------------------------------
command="${1:-help}"
shift || true

case "$command" in
    info)        cmd_info "$@" ;;
    outdated)    cmd_outdated "$@" ;;
    lock)        cmd_lock "$@" ;;
    tool-restore) cmd_tool_restore "$@" ;;
    *)
        echo "Usage: $0 {info|outdated|lock|tool-restore}"
        exit 1
        ;;
esac
