# SUIF_APPEARENCE — Showcase & Proving Ground for SUIF and Unity 6 AI Workflow

[![Live WebGL Demo](https://img.shields.io/badge/Live%20Demo-WebGL%20on%20Cloudflare-00f0ff?style=flat&logo=cloudflare)](https://suif-showcase.covenant-rimless194.workers.dev/)
[![Unity](https://img.shields.io/badge/Unity-6000.6.7f1-black.svg?logo=unity)](https://unity.com/)
[![SUIF](https://img.shields.io/badge/SUIF-v1.0.3-purple.svg)](https://github.com/ReenBo/SUIF)
[![AI Workflow](https://img.shields.io/badge/AI%20Workflow-v1.4.3-blue.svg)](https://github.com/ReenBo/unity6-ai-workflow)
[![License: MIT](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE)

*Read in Russian: [README_RU.md](README_RU.md)*

**SUIF_APPEARENCE** is the live showcase application and engineering proving ground for two companion repositories:
- **[SUIF (`com.reenbo.suif`)](https://github.com/ReenBo/SUIF)** — Scalable, zero-allocation MVVM UI Toolkit framework for Unity 6+.
- **[Unity 6 AI Workflow (`.agents`)](https://github.com/ReenBo/unity6-ai-workflow)** — Autonomous Antigravity agent orchestration, architectural guardrails, Unity CLI/MCP verification, and cross-platform CI/CD audit system.

🌐 **Live Auto-Deployed WebGL Showcase:** [https://suif-showcase.covenant-rimless194.workers.dev/](https://suif-showcase.covenant-rimless194.workers.dev/)

---

## 🎯 What Problems This Ecosystem Solves

| System | Problems Addressed | How It Is Proven Here |
| :--- | :--- | :--- |
| **[SUIF](https://github.com/ReenBo/SUIF)** | Coupled `MonoBehaviour` UI scripts, per-frame GC spikes from string/binding updates, USS style collisions across screens, clunky modal stacking, and manual `Addressables` boilerplate. | Entire Cyberpunk FUI hub runs on **24 pure POCO classes (`0` `MonoBehaviour` UI controllers)** with VContainer DI, R3 reactivity, UniTask async lifecycle, ZString formatting, and Addressables `[UI]` streaming. |
| **[Unity 6 AI Workflow](https://github.com/ReenBo/unity6-ai-workflow)** | Unverified AI code edits, `Editor.log` token bloat, accidental `MonoBehaviour`/LINQ/comment regressions, and uninspected visual UI glitches. | Enforces tiered task routing (`Tier 0–3`), live Unity MCP `recompile`, warm EditMode tests, `eval` Draw Call introspection, `game_view` visual checks, pre-commit hooks, and 3-tier project memory in [`.foragents/`](.foragents/PROJECT_MEMORY.md). |

---

## 🖥️ Showcase Modules & Capabilities

The application presents a Cyberpunk FUI Workbench built with 7 Addressables-loaded UXML views, live `ProfilerRecorder` telemetry (`FPS` / `[XX DC]` / memory), and an automated benchmark runner:

| Module | Demonstrated Capabilities | Implementation |
| :--- | :--- | :--- |
| **Frame Hub & Standby** | Persistent navigation chrome, active window indicators, `[ X ]` & global `STANDBY` close management, zero-stencil diagonal hover wipes | [`ShowcaseFrameView`](Assets/Showcase/Scripts/Frame/ShowcaseFrameView.cs), [`DiagonalWipeEffect`](Assets/Showcase/Scripts/Common/DiagonalWipeEffect.cs) |
| **01 · Lifecycle** | Window open/close/focus lifecycle and deterministic teardown | [`LifecycleWindowView`](Assets/Showcase/Scripts/Windows/01_Lifecycle/LifecycleWindowView.cs) |
| **02 · Reactive Counter** | Declarative MVVM two-way bindings via R3 reactive properties | [`ReactiveCounterView`](Assets/Showcase/Scripts/Windows/02_Counter/ReactiveCounterView.cs) |
| **03 · Recycled List** | High-throughput virtualized list recycling with zero GC churn | [`RecycledListWindowView`](Assets/Showcase/Scripts/Windows/03_RecycledList/RecycledListWindowView.cs) |
| **04 · Modals** | Full-screen overlay backdrop, modal stacking, and input blocking | [`ModalDialogView`](Assets/Showcase/Scripts/Windows/04_Modals/ModalDialogView.cs) |
| **05 · Inventory** | Multi-slot UI binding and reactive selection updates | [`InventoryWindowView`](Assets/Showcase/Scripts/Windows/05_Inventory/InventoryWindowView.cs) |
| **06 · Theme Matrix** | Runtime SMACSS/USS token theme switching without scene reload | [`ThemeMatrixWindowView`](Assets/Showcase/Scripts/Windows/06_ThemeMatrix/ThemeMatrixWindowView.cs), [`ShowcaseThemeService`](Assets/Showcase/Scripts/Common/ShowcaseThemeService.cs) |
| **Telemetry & Benchmark** | Automated cross-module benchmark & WebGL bridge (`ProfilerRecorder`) | [`BenchmarkRunnerService`](Assets/Showcase/Scripts/Benchmark/BenchmarkRunnerService.cs), [`BenchmarkTelemetryService`](Assets/Showcase/Scripts/Benchmark/BenchmarkTelemetryService.cs) |

---

## ⚡ Engineering Highlight: Zero-Visual-Change Draw Call Optimization

During real-world WebGL profiling, angled `-18deg` button hover wipes built with rotated child `VisualElement`s inside `overflow: hidden` forced UI Toolkit stencil-buffer allocations and Transform Group batch breaks, alongside unused URP/camera passes on the UI-only `SampleScene`.

**What we changed (preserving identical visuals, colors, `-18deg` sweep, and timings):**
1. **Default-Shader Geometry Clipping ([`DiagonalWipeEffect.cs`](Assets/Showcase/Scripts/Common/DiagonalWipeEffect.cs))**: Replaced 14 wipe DOM elements and `overflow: hidden` across `.fui-nav-btn`, `.fui-close-btn`, and `.fui-viewport-frame` with a pure POCO `: IDisposable` effect using `MeshGenerationContext.Allocate` on the default UITK shader and stack-only (`Span<Vector2>`) Sutherland-Hodgman clipping (`0` vertices allocated when idle or locked active).
2. **UI-Only Scene URP Cleanup ([`SampleScene.unity`](Assets/Scenes/SampleScene.unity), [`PC_Renderer.asset`](Assets/Settings/PC_Renderer.asset))**: Disabled unused `Main Camera` culling (`m_CullingMask: 0`), HDR, MSAA, shadows, depth/opaque copies, post-processing, and `ScreenSpaceAmbientOcclusion`.

**Measured Results:**
- **Unity Editor (DX12 PlayMode):** `14–31 DC` → **`5–8 DC`** (`7 / 7` EditMode tests passing in [`ShowcaseWindowCloseManagementTests`](Assets/Showcase/Tests/ShowcaseWindowCloseManagementTests.cs)).
- **Chrome WebGL2:** `76–118 DC` (`[72 DC]` idle) → **`45–66 DC`** (~41–44% reduction).
- **Transparent Tracking:** Remaining WebGL2 baseline vs DX12 delta (`QualitySettings` `WebGL: 0` `Mobile_RPAsset`, live WebGL frame capture, and explicit `PanelTextSettings`) is tracked in [`.foragents/techdept.md`](.foragents/techdept.md); generalizing `DiagonalWipeEffect` into `com.reenbo.suif` is tracked in [`.foragents/backlog.md`](.foragents/backlog.md).

This case study also directly evolved `unity6-ai-workflow` to **v1.4.3** (`.agents` commit `687d94c`), adding the Default-Shader Geometry Injection rule, multi-platform URP UI-only checklist, and MCP `eval` `GatherDrawStatistics()` audit standard.

---

## 🛠️ Tech Stack

- **Engine:** Unity `6000.6.7f1` (`URP 17.6.0`, `UI Toolkit`, `Addressables 4.1.2`, `Input System 1.20.1`)
- **Architecture & Async:** `VContainer`, `UniTask`, `R3`, `ObservableCollections`, `ZString`, `NuGetForUnity`
- **Framework:** `com.reenbo.suif` (`v1.0.3`, connected via local sibling package `file:../../SUIF`)
- **Tooling & AI Workflow:** `com.unity.pipeline` (`0.9.1-exp.1`), Unity CLI, `.agents` submodule (`unity6-ai-workflow` `v1.4.3`)

---

## 🔄 CI/CD & Auto-Deployed WebGL Pipeline

Every push or pull request to `develop` / `main` (and `v*` tags) triggers [`.github/workflows/webgl-pipeline.yml`](.github/workflows/webgl-pipeline.yml):

1. **`audit` (`windows-latest`)**: Runs `.agents/ci/run-all-checks.ps1` (VContainer POCO audit, GC allocation audit, Security PII scan).
2. **`build-and-deploy` (`ubuntu-latest`)**: Checks out `SUIF_APPEARENCE` (with `.agents` submodule) + `ReenBo/SUIF`, caches `Library/`, builds WebGL via `game-ci/unity-builder@v4` (`SUIF.Editor.Build.WebGLBuilder.BuildFromCommandLine`), and deploys `Builds/WebGL` to **Cloudflare Workers Static Assets** ([`wrangler.json`](wrangler.json) → [live demo](https://suif-showcase.covenant-rimless194.workers.dev/)). Release tags `v*` also publish to itch.io when configured.

---

## 🚀 Local Setup

1. Clone with submodules alongside the sibling `SUIF` repository (since [`Packages/manifest.json`](Packages/manifest.json) references `file:../../SUIF`):
   ```bash
   git clone --recurse-submodules -b develop https://github.com/ReenBo/SUIF_APPEARENCE.git
   git clone -b develop https://github.com/ReenBo/SUIF.git
   ```
2. Open `SUIF_APPEARENCE` in **Unity `6000.6.7f1`**.
3. *(Optional)* Install local `.agents` Git pre-commit hooks:
   ```powershell
   .\.agents\hooks\install-hooks.ps1
   ```

---

## 📁 Project Structure & Documentation

```text
SUIF_APPEARENCE/
├── .agents/                  # unity6-ai-workflow submodule (rules, skills, subagents, CI)
├── .foragents/               # Project memory, backlog, tech debt & task execution logs
│   ├── PROJECT_MEMORY.md     # High-level architectural state & history
│   ├── backlog.md            # Deferred enhancements & cross-repo generalizations
│   ├── techdept.md           # Mandatory technical debt items
│   └── logs/                 # Per-task execution protocols for PRs & review
├── .github/workflows/        # Automated audit + WebGL build & Cloudflare deploy
├── Assets/
│   ├── Editor/               # WebGLBuilder CLI entry point
│   ├── Settings/             # URP assets (PC/Mobile), PanelSettings, Build Profiles
│   └── Showcase/             # Pure POCO MVVM views, ViewModels, UXML, USS, Tests
├── Packages/manifest.json    # Golden Stack & com.reenbo.suif dependencies
└── wrangler.json             # Cloudflare Workers Static Assets deployment config
```

## 📄 License

MIT License — see [LICENSE](LICENSE).

