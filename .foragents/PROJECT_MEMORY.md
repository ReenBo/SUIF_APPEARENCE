# 🧠 Project Memory & Active Status: SUIF_APPEARENCE

> **Note:** This file is read by Antigravity agents first. It stores a dry summary of the project state, architectural decisions, and active tasks, eliminating the need to spend tokens scanning source files.

---

## 📌 1. Active Status & Task Tracker

- [x] Committed and pushed initial Unity 6 (`6000.6.7f1`) template to `main`.
- [x] Created `develop` branch as default active branch.
- [x] Cloned `unity6-ai-workflow` (`e:\GIT\unity6-ai-workflow`) and connected `.agents` submodule (`develop` branch).
- [x] Initialized `.foragents/` project memory infrastructure.
- [x] Connected `com.reenbo.suif` (`file:../../SUIF`) and Golden Stack (`UniTask`, `VContainer`, `R3`, `ObservableCollections`, `ZString`, `Addressables`, `NuGetForUnity`) in `Packages/manifest.json`.
- [x] Resolved Unity 6000 compilation: restored NuGet assemblies for R3/ObservableCollections, updated Addressables to 4.1.2, fixed SUIF.R3 assembly definition and USS properties.
- [x] Integrated official Unity CLI (`1.0.0-beta.13`) & `com.unity.pipeline` for live editor introspection, configured Antigravity MCP server (`unity mcp configure antigravity`), and verified 2.5s live EditMode tests.
- [x] **[feature/showcase-hub-foundation]**: Implemented Decoupled Cyberpunk FUI Showcase Hub strictly matching HTML prototype:
  - Addressables group `[UI]` configured with all 7 modular views (`ShowcaseFrameView`, `LifecycleWindowView`, `ReactiveCounterView`, `RecycledListWindowView`, `ModalDialogView`, `InventoryWindowView`, `ThemeMatrixWindowView`).
  - Strict SOLID & Open/Closed Principle: decomposed monolithic view into independent POCO services, ViewModels, and UXML templates.
  - SUIF `AddressablesUIAssetProvider` robust handle caching & error handling.
  - Verified live runtime execution in warm Unity 6 Editor via Unity CLI (0 compilation errors, clean transitions across all 6 navigation modules and modal popup overlay).
  - Resolved window close defect: added `[ X ]` header buttons + `btn-main-close` integration + official Standby state (`ALL BASE WINDOWS CLOSED // STANDBY`).
  - Resolved modal text overflow defect: applied `white-space: normal;` with flex-shrink constraints.
- [x] Merged `feature/showcase-hub-foundation` into `develop` and pushed to remote `origin/develop`.
- [x] Formalized Unity CLI & MCP Tools Standard (v1.3.1) in unity6-ai-workflow with mandatory game_view scene verification, authored architectural whitepaper (docs/unity_cli_mcp_developer_whitepaper.md), and updated .agents submodule.
- [x] **[fix/window-close-management]**: Resolved base window closing defects across all 5 window modules (added `CloseAllBaseWindows()` and `CloseBaseWindow()`); fixed modal overlay backdrop to stretch across full screen (100% width and height); created automated test suite (8/8 passed via warm editor `unity cmd test run`); verified live with MCP `game_view`.
- [x] **[workflow/token-economy-and-profiler]**: Upgraded `unity6-ai-workflow` to v1.4.0:
  - Implemented 4-Tier Task Complexity Architecture (`Tier 0: Micro-Fix`, `Tier 1: Prototype / Spike`, `Tier 2: Standard`, `Tier 3: Mission-Critical`).
  - Added strict subagent blocking in Tier 0 (Fast-Path) for styles and minor fixes, reducing token usage by up to 90%.
  - Added `Task Analyzer` Auto Mode classifier with deterministic keyword and file type detection.
  - Implemented mandatory Pre-Push Cost Summary widget (`.agents/scripts/get-task-cost.ps1`) tracking uncached tokens, prompt cache read, cache hit %, duration, and estimated USD cost.
  - Added `token-cost-profiler` subagent and skill (`.agents/skills/token-cost-profiler/`) with automated background trigger on single-request/task token usage exceeding 25,000 tokens.
- [x] **[rules/suif-effects-and-animations]**: Standardized SUIF UI Effects & Animation Architecture (v1.4.1):
  - Formulated GPU-Driven Vertex Injection Pattern for 1-Draw-Call dynamic batching across animated chamfers and wipes.
  - Prohibited DOM `overflow: hidden` + rotated pseudo-elements due to Stencil/Scissor batch destruction.
  - Documented subpixel anti-aliased mathematical fragment equations and zero-alloc struct lerpers.
- [x] **[feature/cyberpunk-button-animations]**: Implemented dual-layer diagonal wipe button animations in SUIF_APPEARENCE strictly matching HTML prototype:
  - Added `.fui-btn-wipe-edge` (Cyan `#00f0ff`, 0.32s) and `.fui-btn-wipe-fill` (`#f4f4f6`, 0.36s) at `-18deg` across all 6 navigation buttons.
  - Added tactile content shift (`translate: 3px 0;` at 0.24s) and instant color inversion (`#050507` at 0.18s).
  - Added press bounce scale (`scale: 0.985;` at 0.12s) on `:active`.
  - Added close button red wipe (`#ff2a54`) with scale `1.08`.
- [x] **[fix/button-wipe-artifacts]**: Resolved top-left triangular corner clipping and active-button re-hover flash:
  - Expanded wipe quad bounds from 130px to 280px (`height: 280px; top: -116px; width: 560px;`) with vertical centering ($y_{center}=24px$), providing >60px margin of safety to eliminate top-left rotation cutoff.
  - Locked active wipe state (`display: none` on `.fui-btn-wipe-edge`, `transition-duration: 0s` on `.fui-btn-wipe-fill` for `.is-active` and `.is-active-modal`), completely eliminating the re-hover flash glitch.
- [x] **[perf/draw-calls-optimization]**: Eliminated UI Toolkit stencil/transform batch breakers and empty-scene URP overhead without visual changes (`ddc7bc9`, `483f661`, `47a6944`):
  - Replaced 14 rotated/translated child wipe elements + `overflow: hidden` across `.fui-nav-btn`, `.fui-close-btn`, and `.fui-viewport-frame` with zero-stencil `DiagonalWipeEffect` (`MeshGenerationContext.Allocate` + stack-only Sutherland-Hodgman clipping on default UITK shader; `0` vertices in idle/active states).
  - Disabled unused `Main Camera` culling, HDR, MSAA, shadows, depth/opaque copies, PostProcessing, and `PC_Renderer` SSAO on the UI-only `SampleScene`.
  - Reduced Editor DX12 PlayMode Draw Calls from `14–31 DC` down to `5–8 DC` (`7 / 7` EditMode tests passed, `game_view` verified) and Chrome WebGL2 Draw Calls from `76–118 DC` down to `45–66 DC`.
- [x] **[workflow/effects-and-cli-v1.4.3]**: Evolved `.agents` (`unity6-ai-workflow` `develop`, `v1.4.3`) with default-shader `MeshGenerationContext.Allocate` batching pattern, multi-platform UI-only URP checklist, MCP `eval` `GatherDrawStatistics()` audit, comment-free snippet cleanup, and expanded `code-reviewer.md` checklist.
- [ ] **[backlog/suif-batched-effects]**: Tracked in [backlog.md](file:///e:/GIT/SUIF_APPEARENCE/.foragents/backlog.md) (generalize `DiagonalWipeEffect` into `com.reenbo.suif` after upcoming animation tasks and tests).
- [ ] **[debt/webgl2-draw-calls-profiling]** & **[debt/ui-font-textsettings]**: Tracked in [techdept.md](file:///e:/GIT/SUIF_APPEARENCE/.foragents/techdept.md) (profile remaining WebGL2 `45–66 DC` floor vs DX12 `5–8 DC` once `npx`/Spector.js or Development WebGL Profiler is available; assign explicit `PanelTextSettings` + embedded font asset).
- [ ] **Next**: Proceed to next animation feature/test tasks or address [techdept.md](file:///e:/GIT/SUIF_APPEARENCE/.foragents/techdept.md) items.

---

## 📐 2. Architectural Decisions Record (ADR)

* **3-Repo Co-Development**:
  * `e:\GIT\SUIF_APPEARENCE` — Unity 6 showcase project demonstrating all `SUIF` capabilities.
  * `e:\GIT\SUIF` — `com.reenbo.suif` UPM package linked locally via `"file:../../SUIF"`.
  * `e:\GIT\unity6-ai-workflow` — AI workflow rules/subagents linked via `.agents` submodule (`develop`).
* **Golden Technology Stack**:
  * **UI Framework**: `SUIF` (Declarative MVVM + UI Toolkit + SMACSS 5-layer styling).
  * **DI Framework**: **VContainer** (`jp.hadashikick.vcontainer`) — strictly POCO ViewModels (`Lifetime.Transient`) and services (`Lifetime.Singleton`).
  * **Async Engine**: **UniTask** (`com.cysharp.unitask`) — struct-based value-type async/await with mandatory `CancellationToken`.
  * **Reactivity**: **R3** (`com.cysharp.r3`) + **ObservableCollections** (`com.cysharp.observablecollections`).
  * **Zero-Alloc Strings**: **ZString** (`com.cysharp.zstring`).
  * **Asset Streaming**: **Addressables** (`com.unity.addressables`).
  * **Automation & Diagnostics**: **Unity CLI** (`unity` + `com.unity.pipeline`) — mandatory for live recompile and warm test execution.
  * **UI Effects & Batching Standard**: Single static Material Asset per shader type; per-element animation parameters injected via `Vertex.tint`; DOM `overflow: hidden` + rotated pseudo-elements strictly prohibited in hot lists/buttons.
* **Git Policy**:
  * `develop` is the default active branch; all commits and feature work target `develop`.
  * **Mandatory Pre-Push Rule**: Always ask the user `"Push? Да или нет?"` before executing any `git push`.

---

## ⚠️ 3. Known Gotchas & Technical Debt

* *Token Budgeting*: Task logs in `.foragents/logs/` must be written in English and not exceed 200 lines per report.
* *Code Style*: Strictly no inline or block comments (`//`, `/* ... */`) in C# source files per `.agents` rules.
* *UI Toolkit Gotchas*: `TemplateContainer` 0x0 collapse requires `.l-layer > * { flex-grow: 1; }`; `picking-mode="Ignore"` must be set in UXML, not USS.

---

## 📜 4. Last Session Log

* **Date**: 2026-11-13
* **Role**: Primary Agent / Orchestrator
* **Summary**: Eliminated UI Toolkit stencil/transform batch breakers (`DiagonalWipeEffect` using zero-stencil `MeshGenerationContext.Allocate` clipping) and empty-scene URP/SSAO/PostProcessing camera passes (`ddc7bc9`, `483f661`, `47a6944`), reducing Editor DX12 PlayMode Draw Calls from `14–31` down to `5–8 DC` and Chrome WebGL2 from `76–118` down to `45–66 DC` without visual changes.
