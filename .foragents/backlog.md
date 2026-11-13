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

---

## Completed from Backlog
- [x] **`[backlog/workflow-evolution]`** (`2026-11-13`): Updated `unity6-ai-workflow` (`.agents` `v1.4.3`) with default-shader `MeshGenerationContext.Allocate` clipping rules, multi-platform URP UI-only checklist, MCP `eval` batching audit, and `code-reviewer.md` checklist alignment.

