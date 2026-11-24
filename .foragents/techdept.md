# Technical Debt Registry: SUIF_APPEARENCE

> **Purpose:** Mandatory technical debt and platform gaps that must be resolved in upcoming engineering cycles. Optional enhancements are tracked in [backlog.md](file:///e:/GIT/SUIF_APPEARENCE/.foragents/backlog.md).

---

## Active Technical Debt

### 1. `[debt/webgl2-draw-calls-profiling]` Profile & Reduce Chrome WebGL2 `45–66 DC` Floor vs Editor DX12 `5–8 DC`
- **Severity:** High (WebGL2 showcase performance parity)
- **Logged:** `2026-11-13`
- **Current State:**
  - Commits `ddc7bc9`, `483f661`, `47a6944` reduced Chrome WebGL2 telemetry from `76–118 DC` (`[72 DC]` idle) down to **`45–66 DC`**, and Editor DX12 PlayMode from `14–31 DC` down to **`5–8 DC`** (`01_Lifecycle`: `drawCommandCount = 129`, `drawRangeCallCount = 2..4`, `UnityStats.drawCalls = 5–7`).
  - `QualitySettings` uses level `0` ([Mobile_RPAsset.asset](file:///e:/GIT/SUIF_APPEARENCE/Assets/Settings/Mobile_RPAsset.asset), `renderScale: 0.8`, `supportsHDR: 1`, `Mobile_Renderer.asset` `m_RendererFeatures: []`) for `WebGL`, though running `Quality = 0` in Editor DX12 still produces `5–7 DC`.
  - Inspection of `WebGLSupport` `UnityEngine.UIElementsModule.dll` confirmed `EvaluateChain` batching matches Editor (`m_TextureSlotCount = Eight`), and simulating `1` texture slot on `01_Lifecycle` gives `6` UI batches (`2–8` slots give `4`).
  - Live Chrome profiling via `browser` subagent was blocked because `chrome_devtools` MCP failed to launch (`"npx" is not installed or not on PATH`).
- **Required Resolution:**
  - Enable `npx` on `PATH` for `chrome_devtools` MCP or capture a WebGL2 frame using Spector.js / a Development WebGL build attached to Unity Profiler to pinpoint the exact source of the remaining `+40–58 DC` in WebGL2.
  - Also clean up unused `Mobile_RPAsset.asset` passes (`renderScale = 1`, `supportsHDR = 0`, shadows off) for the UI-only WebGL target once live profiling confirms URP vs WebGL2 UITK breakdown.

### 2. `[debt/ui-font-textsettings]` [RESOLVED] Assign Explicit `PanelTextSettings` & Embedded Font Asset for WebGL Determinism
- **Severity:** Medium (Cross-platform UI rendering & single-atlas text batching) -> **Resolved (`2026-11-24`, `perf/uitk-font-batching`)**
- **Logged:** `2026-11-13` (Audited & resolved `2026-11-24` on `perf/uitk-font-batching`)
- **Resolution Verified:**
  - Created [ShowcasePanelTextSettings.asset](file:///e:/GIT/SUIF_APPEARENCE/Assets/Settings/ShowcasePanelTextSettings.asset) (`defaultFontAsset = Schiffbauer-Regular SDF`, `fallbackFontAssets = []`, `enableEmojiSupport = false`, `clearDynamicDataOnBuild = false`) and wired it into [ShowcasePanelSettings.asset](file:///e:/GIT/SUIF_APPEARENCE/Assets/Settings/ShowcasePanelSettings.asset).
  - Configured `.unity-text-element` in [CyberpunkTheme.uss](file:///e:/GIT/SUIF_APPEARENCE/Assets/Showcase/UI/Styles/CyberpunkTheme.uss) with `-unity-font: none` and explicit `-unity-font-definition` pointing to `Schiffbauer-Regular SDF.asset`.
  - Replaced 9 unbaked Unicode symbols (`●`, `↻`, `■`, `┼`, `▲`, `▼`, `═`, `◢`, `◣`) in Showcase UXML (`fe5c7e5`) so all remaining symbols (`×`, `»`, `›`, `–`) fit cleanly in `Schiffbauer-Regular SDF`.
  - Re-baked `Schiffbauer-Regular SDF.asset` (`7f0bf53` in `SUIF`) for Unity 6.6 Advanced Text Generator (ATG): **`AtlasPopulationMode.Dynamic`** (ATG rejects `Static` assets), pre-populated **`165` glyphs** (printable ASCII + Cyrillic + `× » › –`), `sourceFontFile = Schiffbauer-Regular`, `GlyphRenderMode.SDFAA`, `1024x1024`, **`atlasPadding = 6`**, `isMultiAtlasTexturesEnabled = false`.
  - Play Mode verification (`05_Inventory`, `01_Lifecycle`, `04_Modals`): **`Schiffbauer-Regular SDF Atlas`** (`1024x1024`, `refCount = 102..104`), **`0` OS fallback atlases** (`Arial` / `Microsoft YaHei`), `US.dc = 2` (settled Inventory/Lifecycle), `0` console warnings/errors, `16/16` EditMode tests passing.
