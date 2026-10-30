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
- [x] **[feature/showcase-hub-foundation]**: Implemented Cyberpunk FUI Showcase Hub 1-to-1 matching HTML prototype (ShowcaseHubView, UXML, USS, POCO ViewModels, R3 reactive bindings, 6 modules, non-destructive modal overlay).
- [x] Verified live runtime execution in warm Unity Editor via Unity CLI (0 errors, 0 warnings, 1920x1080 GameView capture confirmed pixel-perfect).
- [ ] **Next**: User review and merging `feature/showcase-hub-foundation` into `develop`.

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

* **Date**: 2026-10-30
* **Role**: Primary Agent / Orchestrator
* **Summary**: Configured full Unity CLI (`1.0.0-beta.13`) and `com.unity.pipeline` workflow integration, configured Antigravity MCP server, updated `.agents` submodule to 1.3.0 with Dual-Mode test runner (`unity cmd test run`), resolved all 417 compilation errors with NuGet restoration, and synchronized `develop` branch across `SUIF_APPEARENCE`, `SUIF`, and `unity6-ai-workflow`.
