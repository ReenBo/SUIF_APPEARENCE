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
- **Logged:** `2026-11-13`
- **Current State:**
  - [ShowcasePanelSettings.asset](file:///e:/GIT/SUIF_APPEARENCE/Assets/Settings/ShowcasePanelSettings.asset) has `textSettings: {fileID: 0}` and [CyberpunkTheme.uss](file:///e:/GIT/SUIF_APPEARENCE/Assets/Showcase/UI/Styles/CyberpunkTheme.uss) does not specify an embedded `FontAsset` (`unity-font-definition`).
  - In Editor PlayMode, `TextureRegistry` allocated a secondary fallback font atlas (`Microsoft YaHei - Regular Atlas` alongside `Arial - Regular Atlas`), while Linux CI WebGL builds (`ubuntu-latest`) lack Windows OS fallback fonts.
- **Required Resolution:**
  - Create and assign a dedicated `PanelTextSettings` and embedded SDF/Font asset covering all ASCII and FUI symbol glyphs used across `SUIF_APPEARENCE` so text renders identically in WebGL and stays consolidated in a single font atlas.
