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

### 2. `[debt/ui-font-textsettings]` Assign Explicit `PanelTextSettings` & Embedded Font Asset for WebGL Determinism
- **Severity:** Medium (Cross-platform UI rendering & single-atlas text batching)
- **Logged:** `2026-11-13` (Audited in detail `2026-11-24` on `perf/uitk-font-batching`)
- **Current State:**
  - `Project PanelTextSettings count: 0`: [ShowcasePanelSettings.asset](file:///e:/GIT/SUIF_APPEARENCE/Assets/Settings/ShowcasePanelSettings.asset) has `textSettings: NULL` (`themeStyleSheet: UnityDefaultRuntimeTheme`, `dynamicAtlasMaxSize: 4096`, `maxSubTextureSize: 64`), and [CyberpunkTheme.uss](file:///e:/GIT/SUIF_APPEARENCE/Assets/Showcase/UI/Styles/CyberpunkTheme.uss) only specifies `font-size` / `-unity-font-style` without `-unity-font-definition`.
  - [Schiffbauer-Regular SDF.asset](file:///e:/GIT/SUIF/Runtime/Core/Styles/5_Themes/Typography/Fonts/Schiffbauer-Regular%20SDF.asset) in `SUIF` has `atlasPopulationMode = Static`, `atlasRenderMode = SDFAA`, `512x512`, **`pad = 0` (`atlasPadding = 0`)**, **`isMultiAtlasTexturesEnabled = true`**, `chars = 96`, `glyphs = 96`, `fallbacks = 0`.
  - Showcase UXML contains **13 non-ASCII Unicode glyphs** (`●` `U+25CF`, `↻` `U+21BB`, `×` `U+00D7`, `■` `U+25A0`, `┼` `U+253C`, `»` `U+00BB`, `›` `U+203A`, `▲` `U+25B2`, `▼` `U+25BC`, `–` `U+2013`, `═` `U+2550`, `◢` `U+25E2`, `◣` `U+25E3`) across `LifecycleWindowView.uxml`, `ModalDialogView.uxml`, and `ShowcaseFrameView.uxml`, triggering Editor OS fallback (`Microsoft YaHei - Regular Atlas` alongside `Arial - Regular Atlas`), while Linux WebGL CI (`ubuntu-latest`) lacks Windows OS fonts.
- **Required Resolution:**
  - Create and assign a dedicated `PanelTextSettings` asset and embedded `Static` `SDFAA` (`atlasPadding` `5–9` px, `isMultiAtlasTexturesEnabled = false`) `FontAsset` via `-unity-font-definition`, fix `Schiffbauer-Regular SDF.asset` padding/multi-atlas flags, and either bake or replace the 13 non-ASCII glyphs so text consolidates into 1 font atlas deterministically on WebGL.
