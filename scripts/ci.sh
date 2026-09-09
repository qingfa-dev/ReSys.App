#!/usr/bin/env bash
set -euo pipefail

# ============================================================================
# ci.sh — CI pipeline orchestration
# ============================================================================
# Usage:
#   ./scripts/ci.sh              Full CI:  restore → build → coverage → html
#   ./scripts/ci.sh fast         Fast CI:  restore → build → test
#   ./scripts/ci.sh strict       Strict:   build → enforce → html → lint
#   ./scripts/ci.sh local        Clean + full CI in Release config
#   ./scripts/ci.sh unit         Unit tests only
#   ./scripts/ci.sh int          Integration tests only
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

cmd_full() {
    log "=== Full CI Pipeline ==="
    "$SCRIPT_DIR/build.sh" restore
    "$SCRIPT_DIR/build.sh" build Debug
    "$SCRIPT_DIR/coverage.sh" all Debug
    "$SCRIPT_DIR/coverage.sh" html
    ok "CI pipeline completed."
}

cmd_fast() {
    log "=== Fast CI Pipeline ==="
    "$SCRIPT_DIR/build.sh" restore
    "$SCRIPT_DIR/build.sh" build Debug
    "$SCRIPT_DIR/test.sh" all Debug
    ok "Fast CI completed."
}

cmd_strict() {
    local threshold="${1:-80}"
    log "=== Strict CI Pipeline (threshold: ${threshold}%) ==="
    "$SCRIPT_DIR/build.sh" restore
    "$SCRIPT_DIR/build.sh" build Debug
    "$SCRIPT_DIR/coverage.sh" enforce "$threshold" Debug
    "$SCRIPT_DIR/coverage.sh" html
    "$SCRIPT_DIR/format.sh" lint
    ok "Strict CI completed."
}

cmd_local() {
    log "=== Local CI Simulation (Release) ==="
    "$SCRIPT_DIR/clean.sh" --all
    CI=true "$SCRIPT_DIR/build.sh" restore
    CI=true "$SCRIPT_DIR/build.sh" build Release
    CI=true "$SCRIPT_DIR/coverage.sh" all Release
    CI=true "$SCRIPT_DIR/coverage.sh" html
    ok "Local CI simulation completed."
}

cmd_unit() {
    log "=== Unit CI ==="
    "$SCRIPT_DIR/build.sh" restore
    "$SCRIPT_DIR/build.sh" build Debug
    "$SCRIPT_DIR/coverage.sh" unit Debug
    ok "Unit CI completed."
}

cmd_int() {
    log "=== Integration CI ==="
    "$SCRIPT_DIR/build.sh" restore
    "$SCRIPT_DIR/build.sh" build Debug
    "$SCRIPT_DIR/coverage.sh" int Debug
    ok "Integration CI completed."
}

# ---------------------------------------------------------------------------
# Dispatch
# ---------------------------------------------------------------------------
command="${1:-help}"
shift || true

case "$command" in
    full)   cmd_full "$@" ;;
    fast)   cmd_fast "$@" ;;
    strict) cmd_strict "$@" ;;
    local)  cmd_local "$@" ;;
    unit)   cmd_unit "$@" ;;
    int)    cmd_int "$@" ;;
    *)
        echo "Usage: $0 {full|fast|strict|local|unit|int}"
        exit 1
        ;;
esac
