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
- [ ] **Next**: Ready for next feature / task.

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

* **Date**: 2026-11-06
* **Role**: Primary Agent / Orchestrator
* **Summary**: Implemented v1.4.0 Token Economy & Profiler upgrade across unity6-ai-workflow: added Task Complexity Tiers (Tier 0-3), strict subagent blocking on Tier 0 (Fast-Path), mandatory Pre-Push Cost Summary widget (.agents/scripts/get-task-cost.ps1), Token & Cost Profiler subagent and skill with 25k budget alert triggers (.agents/skills/token-cost-profiler/).
