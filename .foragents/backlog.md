# Deferred Tasks & Backlog: SUIF_APPEARENCE

> **Purpose:** Non-blocking improvement ideas and generalizations intentionally deferred to future iterations. Mandatory fixes and obligations are tracked separately in [techdept.md](file:///e:/GIT/SUIF_APPEARENCE/.foragents/techdept.md).

---

## Pending Backlog Items

### 1. `[backlog/suif-batched-effects]` Generalize `DiagonalWipeEffect` into `com.reenbo.suif`
- **Status:** Deferred (planned after upcoming animation tasks and tests).
- **Context:** Currently [DiagonalWipeEffect.cs](file:///e:/GIT/SUIF_APPEARENCE/Assets/Showcase/Scripts/Common/DiagonalWipeEffect.cs) lives inside `SUIF_APPEARENCE` (`Assets/Showcase/Scripts/Common/`), while `SUIF_APPEARENCE` serves as the showcase demo for `com.reenbo.suif` (`e:\GIT\SUIF`).
- **Proposed Scope:**
  - Extract a parameterized zero-stencil effect / `ViewBinder` extension into `com.reenbo.suif` with configurable wipe angle, enter/exit durations, CSS custom property names, and active-state predicate.
  - Keep only Cyberpunk theme tokens and bindings inside `SUIF_APPEARENCE`.

### 2. `[backlog/workflow-evolution]` Evolve `unity6-ai-workflow` (`.agents`) Rules & Checklists
- **Status:** Deferred (to be applied in `e:\GIT\unity6-ai-workflow`, branch `develop`, and synced via `.agents` submodule).
- **Context:** Draw calls optimization and live MCP introspection surfaced gaps in `.agents` rules and subagent checklists.
- **Proposed Scope:**
  - Update [suif-effects-and-animations.md](file:///e:/GIT/SUIF_APPEARENCE/.agents/rules/suif-effects-and-animations.md) to note that custom `Material` (`unityMaterial`) breaks batching against default `Internal-UIRDefault` elements, document the default-shader `MeshGenerationContext.Allocate` + stack-only polygon clipping pattern (`0` stencil buffers, `0` `overflow: hidden`, `0` idle vertices), add a UI-only URP/Camera checklist across all `QualitySettings` platforms (`PC` + `WebGL`), and remove `//` comments from snippets to comply with [code-style-no-comments.md](file:///e:/GIT/SUIF_APPEARENCE/.agents/rules/code-style-no-comments.md).
  - Update [unity-cli-standard.md](file:///e:/GIT/SUIF_APPEARENCE/.agents/rules/unity-cli-standard.md) and [code-reviewer.md](file:///e:/GIT/SUIF_APPEARENCE/.agents/subagents/code-reviewer.md) with UI Toolkit MCP `eval` batching inspection (`GatherDrawStatistics()`, `RenderTree.m_FirstCommand`, `TextureRegistry`), method-body compilation scope (no top-level `using`), `scratch/` screenshot path discipline (never inside `Assets/`), and checklist alignment.
