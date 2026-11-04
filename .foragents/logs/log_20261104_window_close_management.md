# Task Execution Protocol: Base Window Close Management & Zero-Leak State Machine

- **Date**: 2026-11-04
- **Branch**: `fix/window-close-management`
- **Subagents / Roles**: Task Analyzer, Code Architect, Code Writer, Test Engineer, Code Reviewer, Execution Logger

---

## 1. Objective

Resolve intermittent base window closing failures across the Showcase Hub where base windows (01 Lifecycle, 02 Counter, 03 Recycled List, 05 Inventory, 06 Theme Matrix) could become stuck on screen or fail to close when switching between modules or clicking header close buttons. Ensure that all windows can be reliably closed, returning to Standby while leaving the main menu shell (`ShowcaseFrameView`) and modal dialog overlay intact.

---

## 2. Root Cause Analysis

1. **Orphaned Views on Module Switching**: Navigating between modules (e.g. 01 -> 02 -> 03) previously invoked `_uiFlow.OpenAsync<T>()` without closing preceding active views. This resulted in multiple views co-existing on `Layer-Windows`.
2. **Narrow Closing Switch**: `CloseActiveWindow()` previously evaluated a `switch (ActiveNavIndex.Value)`. If multiple views had been opened or if navigation state became unaligned, only the most recent view was closed, permanently stranding earlier views in the hierarchy.
3. **Modal Precedence Confusion**: Window-level `Close()` callbacks previously redirected to `CloseActiveWindow()`, which prioritized closing the modal overlay if open, rather than closing the specific base window whose `[ X ]` button was clicked.

---

## 3. Key Accomplishments

1. **State Machine Architecture (`ShowcaseFrameViewModel`)**:
   - Added `CloseAllBaseWindows()`: Unconditionally invokes `_uiFlow.Close<T>()` across all 5 base window types (`LifecycleWindowView`, `ReactiveCounterView`, `RecycledListWindowView`, `InventoryWindowView`, `ThemeMatrixWindowView`).
   - Added `CloseBaseWindow()`: Resets `ActiveNavIndex = 0`, updates title to `[ NO_ACTIVE_VIEW // IDLE ]`, logs `CLOSED // BASE_WINDOW`, and calls `CloseAllBaseWindows()`.
   - Updated Navigation Openers: `OpenLifecycleWindow()`, `OpenCounterWindow()`, `OpenRecycledListWindow()`, `OpenInventoryWindow()`, and `OpenThemeMatrixWindow()` now invoke `CloseAllBaseWindows()` before opening new views, preventing multiple views from ever co-existing.
   - Updated View-Level Close Handlers: `LifecycleWindowViewModel.Close()`, `ReactiveCounterViewModel.Close()`, `RecycledListWindowViewModel.Close()`, `InventoryWindowViewModel.Close()`, and `ThemeMatrixWindowViewModel.Close()` now route to `CloseBaseWindow()`.

2. **Automated Unit & Integration Test Suite (`Showcase.Tests`)**:
   - Configured `Assets/Showcase/Tests/Showcase.Tests.asmdef` with NUnit, R3, UniTask, and VContainer bindings.
   - Implemented `ShowcaseWindowCloseManagementTests.cs` covering 7 test scenarios:
     - `InitialState_StartsInStandbyWithNoActiveWindows`
     - `OpenLifecycleWindow_SetsActiveNavIndexAndOpensView`
     - `SwitchingBetweenWindows_ClosesPreviousBaseWindows`
     - `SwitchingRapidlyAcrossAllModules_EnsuresNoOrphanedBaseWindows`
     - `CloseBaseWindow_ClosesAllBaseWindowsAndRevertsToStandby`
     - `CloseActiveWindow_WithModalOpen_ClosesModalFirst`
     - `CloseBaseWindow_WithModalOpen_DirectlyClosesBaseWindow`
   - Verified 100% pass rate (`7/7 passed in 2.79 seconds`) via warm editor `unity cmd test run`.

3. **Live PlayMode & MCP `game_view` Visual Verification**:
   - Executed live PlayMode interaction: tested rapid switching across modules, closing via window header `[ X ]`, and verified clean unmounting of all views with instant return to `ALL BASE WINDOWS CLOSED // STANDBY`.
   - Captured visual proof `window_closed_standby.png` via MCP `game_view`.

---

## 4. Audit & Compliance

- **VContainer & POCO**: 100% POCO classes, 0 prohibited `MonoBehaviour` controllers.
- **GC & Performance**: Zero allocations in state transitions.
- **Code Style**: Strictly 0 inline/block comments in source code per `rules/code-style-no-comments.md`.
- **Pre-Commit Checks**: Passed cleanly.
