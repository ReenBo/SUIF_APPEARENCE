# Task Execution Protocol: Token Economy, Fast-Path Tiers & Cost Profiler Upgrade

- **Date**: 2026-11-06
- **Branch**: `develop` (submodule `.agents` updated to v1.4.0)
- **Subagents / Roles**: Task Analyzer, Code Architect, Code Writer, Token & Cost Profiler, Execution Logger

---

## 1. Objective

Upgrade the `unity6-ai-workflow` ecosystem (v1.4.0) with comprehensive token economy mechanisms:
1. Implement a **Pre-Push Cost Summary widget** displaying hardware model token metrics, prompt cache efficiency, and estimated spend before remote pushes.
2. Build an autonomous **Token & Cost Profiler** subagent and skill with automated >25k token budget alert triggers to isolate context bloat and tool payload bottlenecks.
3. Introduce **Task Complexity Tiers (Tier 0 to Tier 3)** with an intelligent **Auto Mode Classifier** and strict subagent blocking on Tier 0 (Fast-Path) to eliminate multi-agent dispatch overhead on minor fixes and prototypes.

---

## 2. Key Accomplishments

1. **Pre-Push Token & Cost Calculator (`.agents/scripts/get-task-cost.ps1`)**:
   - Parses `transcript.jsonl` in the Antigravity session directory.
   - Extracts hardware model metrics: uncached input tokens, prompt cache read tokens, output tokens, planner call counts, and execution duration.
   - Computes Cache Hit Ratio (`cache_read_tokens / total_context * 100`).
   - Calculates estimated USD cost across Gemini Flash, Gemini Pro, and Claude pricing models.
   - Evaluates configurable token budget thresholds (default: 25,000 tokens) and formats a clean ASCII dashboard widget.

2. **Token & Cost Profiler Subagent & Skill (`.agents/skills/token-cost-profiler/`)**:
   - Profile: `.agents/subagents/token-cost-profiler.md`
   - Skill: `.agents/skills/token-cost-profiler/SKILL.md`
   - Analytical Engine: `.agents/skills/token-cost-profiler/scripts/analyze-token-bottlenecks.ps1`
   - Evaluates 5 modern metrics: Prompt Cache Hit Ratio, Tool Payload Bloat Index, Subagent Overhead, Fix Loop Penalty, and Code Efficiency.
   - Operates in dual-mode: on-demand command (`/profile-tokens`) or autonomous background execution triggered by single-request/task token usage exceeding 25,000 tokens.

3. **Task Complexity Tiers & Adaptive Routing (`AGENTS.md` & `task-analyzer.md`)**:
   - **Tier 0 (Micro-Fix)**: Styles/USS/UXML or 1-line changes. **Subagents strictly prohibited.** Executed directly by Primary Agent via `edit_file` + `unity recompile`. Saves up to 90% tokens.
   - **Tier 1 (Prototype / Spike)**: Raw exploratory features. `Code Architect` and `Test Engineer` disabled. Direct drafting via Primary/Writer + Reviewer. Saves up to 60% tokens.
   - **Tier 2 (Standard Feature)**: Full standard pipeline with NUnit tests and architectural contracts.
   - **Tier 3 (Mission-Critical)**: Full pipeline with all audits (PII, GC, Security, Warm Tests, MCP `game_view`).
   - Added `(Recommended) Auto mode` as first choice in the developer interview with deterministic classification rules.

4. **Integration & Documentation**:
   - Updated `.agents/package.json` to version `1.4.0` with `task-cost` and `profile-tokens` npm scripts.
   - Updated `.agents/CHANGELOG.md` documenting all additions.
   - Updated `.foragents/PROJECT_MEMORY.md` with active feature tracking.

---

## 3. Verification & Metrics

- Tested `.agents/scripts/get-task-cost.ps1` against live active session transcript: successfully parsed 30+ planner calls, evaluated ~90% prompt cache efficiency, and generated real-time ASCII table.
- Tested `.agents/skills/token-cost-profiler/scripts/analyze-token-bottlenecks.ps1`: successfully isolated top model spikes, heavy file read payloads, and generated actionable recommendations.
- Verified fast-path announcement string formatting:
  `⚡ [Auto Mode] Обнаружена микро-правка стилей/конфигурации. Включаю Fast-Path (Tier 0). 🚫 Использование субагентов заблокировано для экономии токенов и времени.`
