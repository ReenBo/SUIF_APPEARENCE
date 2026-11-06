# Task Execution Protocol: Button Wipe Top-Left Corner & Re-Hover Artifact Fix

- **Date**: 2026-11-06
- **Branch**: `develop`
- **Subagents / Roles**: Primary Agent (Fast-Path Tier 0 Hotfix)

---

## 1. Objective

Resolve two visual defects reported on Cyberpunk button animations:
1. **Top-Left Corner Clipping**: A black triangular cut appeared in the top-left corner of active buttons due to rotated quad boundaries intersecting the button frame.
2. **Re-Hover Flash Glitch**: Hovering over an already selected/active button momentarily flashed a cyan artifact in the top-left corner during transition re-triggering.

---

## 2. Root Cause Analysis

1. **Rotation Geometry Bounds (Defect 1)**:
   - The `.fui-btn-wipe-fill` and `.fui-btn-wipe-edge` quads previously had dimensions of `width: 480px; height: 130px; top: -40px;`.
   - When rotated by `-18deg`, the top edge of the quad tilted downwards towards the left, crossing $x=0$ at $y \approx 25px$ (halfway down the 48px button height). This left the upper-left quadrant $[0..70px, 0..25px]$ completely uncovered, exposing the black button background.

2. **Active State Hover Re-Triggering (Defect 2)**:
   - When `.fui-nav-btn.is-active` was hovered, the hover rule `.fui-nav-btn:hover .fui-btn-wipe-edge { translate: 0 0; }` re-triggered on the underlying cyan edge quad.
   - Because the top-left corner was uncovered by the fill quad, the cyan edge swept through the void on re-hover, creating a momentary cyan flash.

---

## 3. Key Accomplishments

1. **Expanded Wipe Quad Bounds with Vertical Centering (`CyberpunkTheme.uss`)**:
   - Resized `.fui-btn-wipe-edge` and `.fui-btn-wipe-fill` to `width: 560px; height: 280px; top: -116px; left: -105px / -120px;`.
   - Perfectly centered vertically at $y_{center} = -116 + 140 = 24px$ (exact vertical center of 48px button).
   - At $-18deg$ rotation, the top edge of the quad is at $y \approx -64px$ at $x=0$, providing a **64px margin of safety** above the button border. The entire button area is 100% solid, with zero clipped corners.
   - Similarly resized close button wipe quads to `height: 180px; top: -73px; width: 240px;` with >60px margin of safety.

2. **Locked Active Button Wipe Layers**:
   - Added locked selectors for `.is-active` and `.is-active:hover`:
     - `.fui-btn-wipe-fill`: locked at `translate: 0 0; transition-duration: 0s;`.
     - `.fui-btn-wipe-edge`: explicitly set to `display: none;` on active buttons.
   - Applied identical locked rules to `.is-active-modal`.
   - Hovering over an active button now has zero transition replay, completely eliminating any artifact flash.

---

## 4. Verification

- Verified geometry calculations: $Y_{top} = 24 - (140 - 52) = -64px < 0$, guaranteeing 100% fill coverage across all four corners.
- Pre-commit audit passed cleanly (18 POCO classes, 0 MonoBehaviour controllers, zero GC allocations in frame loops).

