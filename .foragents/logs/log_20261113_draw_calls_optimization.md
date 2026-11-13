# Task Execution Protocol: Zero-Visual-Change Draw Calls Optimization

- **Date**: 2026-11-13
- **Branch**: `develop`
- **Commits**: `ddc7bc9`, `483f661`, `47a6944`
- **Subagents / Roles**: Primary Agent (Direct Pipeline Optimization)

---

## 1. Objective

Reduce WebGL/Player frame Draw Calls (`72–118 DC` observed in Chrome and live telemetry `[72 DC]`) without altering any UI View visuals, colors, hover wipe angles (`-18deg`), timing, or layout.

---

## 2. Root Cause Analysis

1. **UI Toolkit Stencil & Transform Batch Breaking (`ShowcaseFrameView.uxml` / `CyberpunkTheme.uss`)**:
   - Six `.fui-nav-btn` elements and `.fui-close-btn` wrapped two child `VisualElement` nodes with `rotate: -18deg` and `translate: -120% 0` inside `overflow: hidden`.
   - `overflow: hidden` around transformed children forced UI Toolkit `ClipMethod.Stencil` push/pop pairs plus per-element Transform Groups, splitting adjacent UI batches across every button.
   - `.fui-viewport-frame` also declared unnecessary `overflow: hidden`.

2. **Empty-Scene URP & Camera Overhead (`SampleScene.unity` / `PC_Renderer.asset`)**:
   - `Main Camera` had `m_CullingMask` set to all layers, `HDR`, `MSAA`, `Shadows`, `PostProcessing` (Bloom + Vignette), and `UsePipelineSettings` (`RequireDepthTexture: 1`, `RequireOpaqueTexture: 1`).
   - `PC_Renderer.asset` executed active `ScreenSpaceAmbientOcclusion` despite `SampleScene` containing zero 3D renderers beneath the opaque `#050507` `.fui-root` UI.

---

## 3. Key Accomplishments

1. **Zero-Stencil Diagonal Wipe (`DiagonalWipeEffect.cs`, commit `ddc7bc9`)**:
   - Added pure POCO `DiagonalWipeEffect : IDisposable` using `MeshGenerationContext.Allocate` and stack-only (`Span<Vector2>`) Sutherland-Hodgman diagonal clipping at `tan(18deg)`.
   - Renders on the default UI Toolkit shader (zero custom shader/material switches), emits `0` vertices when idle or `.is-active` / `.is-active-modal`, animates only during hover transitions via `IVisualElementScheduledItem`, and tracks theme variables (`--fg-primary`, `--cyber-cyan`, `--cyber-red`) through `CustomStyleResolvedEvent`.

2. **Frame View & Style Cleanup (`ShowcaseFrameView.cs`, `ShowcaseFrameView.uxml`, `CyberpunkTheme.uss`, commit `483f661`)**:
   - Attached `DiagonalWipeEffect` to all 6 nav buttons and `_btnMainClose`, disposing cleanly via `AddDisposable`.
   - Removed 14 `.fui-*-wipe-*` DOM elements, removed `overflow: hidden` from `.fui-nav-btn`, `.fui-viewport-frame`, `.fui-close-btn`, and set `.fui-nav-btn.is-active-modal` background directly to `var(--cyber-cyan)`.

3. **Camera & URP Pass Elimination (`SampleScene.unity`, `PC_Renderer.asset`, commit `47a6944`)**:
   - Set `Main Camera` `m_CullingMask: 0`, disabled `HDR`, `MSAA`, `RenderShadows`, `RequiresDepthTexture`, `RequiresOpaqueTexture`, `RenderPostProcessing`, `AllowXRRendering`, `AllowHDROutput`, and matched `m_BackGroundColor` to `#050507`.
   - Disabled `ScreenSpaceAmbientOcclusion` (`m_Active: 0`) in `PC_Renderer.asset`.

---

## 4. Verification

- **Compilation**: Unity MCP `recompile` passed (`failed: false`, `compilationFailed: false`).
- **Automated Tests**: `ShowcaseWindowCloseManagementTests` (`EditMode`) — `7 / 7` Passed.
- **PlayMode & `game_view` Visual Inspection (Editor DX12)**:
  - `01_Lifecycle`: `DrawCalls=7`, `SetPass=8`, `[7 DC]` (`DrawStats.drawCommandCount=129`, `drawRangeCallCount=2..4`).
  - `03_RecycledList`: `RecycledList_DC=8`, `SetPass=8`.
  - `05_Inventory` + `MODAL_04`: `InventoryModal_DC=5`, `SetPass=8`, `[5 DC]`.
  - Pre-commit audits passed cleanly (24 POCO classes, 0 MonoBehaviour controllers, 0 GC allocations in frame loops).
- **Chrome WebGL2 Verification**:
  - Reduced Chrome WebGL2 telemetry from `76–118 DC` (`[72 DC]` at idle) down to `45–66 DC`.
  - Follow-up investigation confirmed `QualitySettings` `WebGL: 0` uses `Mobile_RPAsset` (`renderScale=0.8`, `supportsHDR=True`) and `WebGLSupport` `UnityEngine.UIElementsModule.dll` C# `EvaluateChain` has no WebGL-specific batch-break override (even 1-slot simulation on `01_Lifecycle` produces 6 UI batches). Inspecting the live deployed WebGL frame via `browser` subagent was blocked because `chrome_devtools` MCP requires `npx` on `PATH`.

