#!/usr/bin/env bash
set -euo pipefail

# ============================================================================
# build.sh — Restore / Build
# ============================================================================
# Usage:
#   ./scripts/build.sh restore
#   ./scripts/build.sh build [Debug|Release]
#   ./scripts/build.sh build-release
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

cmd_restore() {
    log "Restoring packages..."
    dotnet restore "$ROOT_DIR/$SOLUTION" --verbosity "$VERBOSITY"
    ok "Packages restored."
}

cmd_build() {
    local config="${1:-Debug}"
    log "Building solution ($config)..."
    dotnet build "$ROOT_DIR/$SOLUTION" \
        --configuration "$config" \
        --verbosity "$VERBOSITY"
    ok "Build completed ($config)."
}

cmd_build_release() {
    cmd_build Release
}

# ---------------------------------------------------------------------------
# Dispatch
# ---------------------------------------------------------------------------
command="${1:-help}"
shift || true

case "$command" in
    restore)       cmd_restore ;;
    build)         cmd_build "$@" ;;
    build-release) cmd_build_release ;;
    *)
        echo "Usage: $0 {restore|build|build-release}"
        exit 1
        ;;
esac
