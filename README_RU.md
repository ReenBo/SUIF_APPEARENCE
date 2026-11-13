# SUIF_APPEARENCE — Витрина и боевой полигон для SUIF и Unity 6 AI Workflow

[![Live WebGL Demo](https://img.shields.io/badge/Live%20Demo-WebGL%20on%20Cloudflare-00f0ff?style=flat&logo=cloudflare)](https://suif-showcase.covenant-rimless194.workers.dev/)
[![Unity](https://img.shields.io/badge/Unity-6000.6.7f1-black.svg?logo=unity)](https://unity.com/)
[![SUIF](https://img.shields.io/badge/SUIF-v1.0.3-purple.svg)](https://github.com/ReenBo/SUIF)
[![AI Workflow](https://img.shields.io/badge/AI%20Workflow-v1.4.3-blue.svg)](https://github.com/ReenBo/unity6-ai-workflow)
[![License: MIT](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE)

*English version: [README.md](README.md)*

**SUIF_APPEARENCE** — это интерактивная showcase-сцена и реальный полигон испытаний для двух связанных репозиториев:
- **[SUIF (`com.reenbo.suif`)](https://github.com/ReenBo/SUIF)** — масштабируемый MVVM UI Toolkit фреймворк без GC-аллокаций для Unity 6+.
- **[Unity 6 AI Workflow (`.agents`)](https://github.com/ReenBo/unity6-ai-workflow)** — система оркестрации AI-агентов (Antigravity), архитектурных ограничений, верификации через Unity CLI/MCP и кроссплатформенного CI/CD-аудита.

🌐 **Актуальная WebGL-сборка (автодеплой):** [https://suif-showcase.covenant-rimless194.workers.dev/](https://suif-showcase.covenant-rimless194.workers.dev/)

---

## 🎯 Какие проблемы решает экосистема

| Репозиторий | Какую проблему решает | Как это доказано в проекте |
| :--- | :--- | :--- |
| **[SUIF](https://github.com/ReenBo/SUIF)** | Связанный `MonoBehaviour`-спагетти в UI, аллокации GC в кадровом цикле, конфликты стилей USS, сложные модальные стеки и ручная обвязка `Addressables`. | Вся Cyberpunk FUI-витрина построена на **24 чистых POCO-классах (`0` `MonoBehaviour`-контроллеров)** с DI на VContainer, реактивностью R3, async на UniTask, `ZString` и загрузкой окон из Addressables `[UI]`. |
| **[Unity 6 AI Workflow](https://github.com/ReenBo/unity6-ai-workflow)** | Непроверенные правки от LLM, раздувание контекста чтением `Editor.log`, деградация архитектуры и визуальные баги UI без проверки кадра. | Внедрены уровни сложности `Tier 0–3`, live-компиляция и тёплые EditMode-тесты через Unity CLI/MCP, инспекция Draw Calls через `eval`, обязательная визуальная проверка `game_view`, pre-commit хуки и 3-уровневая память в [`.foragents/`](.foragents/PROJECT_MEMORY.md). |

---

## 🖥️ Модули витрины

Приложение представляет собой Cyberpunk FUI Workbench из 7 Addressables UXML-вьюх с live-телеметрией `ProfilerRecorder` (`FPS` / `[XX DC]` / память) и встроенным бенчмарком:

| Модуль | Что демонстрирует | Реализация |
| :--- | :--- | :--- |
| **Frame Hub & Standby** | Каркас навигации, активные состояния, закрытие окон (`[ X ]` и `STANDBY`), диагональный hover-wipe без stencil-буферов | [`ShowcaseFrameView`](Assets/Showcase/Scripts/Frame/ShowcaseFrameView.cs), [`DiagonalWipeEffect`](Assets/Showcase/Scripts/Common/DiagonalWipeEffect.cs) |
| **01 · Lifecycle** | Жизненный цикл окна (открытие, фокус, закрытие) | [`LifecycleWindowView`](Assets/Showcase/Scripts/Windows/01_Lifecycle/LifecycleWindowView.cs) |
| **02 · Reactive Counter** | Двусторонний MVVM data binding на реактивных свойствах R3 | [`ReactiveCounterView`](Assets/Showcase/Scripts/Windows/02_Counter/ReactiveCounterView.cs) |
| **03 · Recycled List** | Виртуализированный список с переиспользованием элементов без аллокаций | [`RecycledListWindowView`](Assets/Showcase/Scripts/Windows/03_RecycledList/RecycledListWindowView.cs) |
| **04 · Modals** | Полноэкранный backdrop, стек модальных окон и блокировка фона | [`ModalDialogView`](Assets/Showcase/Scripts/Windows/04_Modals/ModalDialogView.cs) |
| **05 · Inventory** | Привязка слотов инвентаря и реактивное обновление выбора | [`InventoryWindowView`](Assets/Showcase/Scripts/Windows/05_Inventory/InventoryWindowView.cs) |
| **06 · Theme Matrix** | Мгновенное переключение тем SMACSS/USS в рантайме без перезагрузки сцены | [`ThemeMatrixWindowView`](Assets/Showcase/Scripts/Windows/06_ThemeMatrix/ThemeMatrixWindowView.cs), [`ShowcaseThemeService`](Assets/Showcase/Scripts/Common/ShowcaseThemeService.cs) |
| **Telemetry & Benchmark** | Автоматический прогон модулей и WebGL-мост метрик (`ProfilerRecorder`) | [`BenchmarkRunnerService`](Assets/Showcase/Scripts/Benchmark/BenchmarkRunnerService.cs), [`BenchmarkTelemetryService`](Assets/Showcase/Scripts/Benchmark/BenchmarkTelemetryService.cs) |

---

## ⚡ Кейс: оптимизация Draw Calls без визуальных изменений

При профилировании сборки выяснилось, что диагональный `-18deg` hover-эффект кнопок через повёрнутые дочерние `VisualElement` внутри `overflow: hidden` вызывал выделение stencil-буферов UI Toolkit и разбиение батчей, а пустая UI-сцена дополнительно прогоняла лишние URP/Camera/SSAO пассы.

**Проделанная работа (при 100% сохранении визуала, цветов, угла `-18deg` и таймингов):**
1. **Геометрический клиппинг на дефолтном шейдере ([`DiagonalWipeEffect.cs`](Assets/Showcase/Scripts/Common/DiagonalWipeEffect.cs))**: убраны 14 wipe-элементов и `overflow: hidden` у `.fui-nav-btn`, `.fui-close-btn` и `.fui-viewport-frame`; эффект переведён на `MeshGenerationContext.Allocate` со стековым (`Span<Vector2>`) отсечением Сазерленда — Ходжмана (`0` вершин в idle/active состоянии).
2. **Очистка URP/Camera на UI-only сцене ([`SampleScene.unity`](Assets/Scenes/SampleScene.unity), [`PC_Renderer.asset`](Assets/Settings/PC_Renderer.asset))**: отключены `CullingMask`, HDR, MSAA, тени, depth/opaque копии, постобработка и `ScreenSpaceAmbientOcclusion`.

**Замеренный эффект:**
- **Unity Editor (DX12 PlayMode):** `14–31 DC` → **`5–8 DC`** (`7 / 7` EditMode-тестов в [`ShowcaseWindowCloseManagementTests`](Assets/Showcase/Tests/ShowcaseWindowCloseManagementTests.cs) пройдены).
- **Chrome WebGL2:** `76–118 DC` (`[72 DC]` в простое) → **`45–66 DC`** (снижение на ~41–44%).
- **Прозрачный трекинг:** дальнейшее профилирование разницы WebGL2 vs DX12 (`QualitySettings` `WebGL: 0` `Mobile_RPAsset`, live WebGL capture и явный `PanelTextSettings`) зафиксировано в [`.foragents/techdept.md`](.foragents/techdept.md), а обобщение `DiagonalWipeEffect` в `com.reenbo.suif` — в [`.foragents/backlog.md`](.foragents/backlog.md).

По итогам этого кейса также обновлена методология `unity6-ai-workflow` до **v1.4.3** (`.agents` `687d94c`): добавлены паттерн Default-Shader Geometry Injection, мультиплатформенный URP-чеклист для UI-сцен и аудит батчинга через MCP `eval`.

---

## 🛠️ Стек технологий

- **Движок:** Unity `6000.6.7f1` (`URP 17.6.0`, `UI Toolkit`, `Addressables 4.1.2`, `Input System 1.20.1`)
- **Архитектура и async:** `VContainer`, `UniTask`, `R3`, `ObservableCollections`, `ZString`, `NuGetForUnity`
- **UI-фреймворк:** `com.reenbo.suif` (`v1.0.3`, локальный пакет `file:../../SUIF`)
- **AI & Tooling:** `com.unity.pipeline` (`0.9.1-exp.1`), Unity CLI, сабмодуль `.agents` (`unity6-ai-workflow` `v1.4.3`)

---

## 🔄 CI/CD и автоматическая сборка WebGL

Каждый push или PR в `develop` / `main` (и теги `v*`) запускает пайплайн [`.github/workflows/webgl-pipeline.yml`](.github/workflows/webgl-pipeline.yml):

1. **`audit` (`windows-latest`)**: выполняет `.agents/ci/run-all-checks.ps1` (проверка VContainer POCO, GC-аллокаций в frame-loop и PII-утечек).
2. **`build-and-deploy` (`ubuntu-latest`)**: забирает `SUIF_APPEARENCE` (с сабмодулем `.agents`) и `ReenBo/SUIF`, кэширует `Library/`, собирает WebGL через `game-ci/unity-builder@v4` (`SUIF.Editor.Build.WebGLBuilder.BuildFromCommandLine`) и автоматически публикует `Builds/WebGL` в **Cloudflare Workers Static Assets** ([`wrangler.json`](wrangler.json) → [онлайн-демо](https://suif-showcase.covenant-rimless194.workers.dev/)). Теги `v*` дополнительно деплоятся на itch.io.

---

## 🚀 Локальный запуск

1. Клонировать с сабмодулями рядом с репозиторием `SUIF` ([`Packages/manifest.json`](Packages/manifest.json) ссылается на `file:../../SUIF`):
   ```bash
   git clone --recurse-submodules -b develop https://github.com/ReenBo/SUIF_APPEARENCE.git
   git clone -b develop https://github.com/ReenBo/SUIF.git
   ```
2. Открыть `SUIF_APPEARENCE` в **Unity `6000.6.7f1`**.
3. *(Опционально)* Установить локальные Git pre-commit хуки `.agents`:
   ```powershell
   .\.agents\hooks\install-hooks.ps1
   ```

---

## 📁 Структура проекта и документация

```text
SUIF_APPEARENCE/
├── .agents/                  # Сабмодуль unity6-ai-workflow (правила, скиллы, субагенты, CI)
├── .foragents/               # Память проекта, бэклог, техдолг и логи задач
│   ├── PROJECT_MEMORY.md     # Архитектурный статус и история задач
│   ├── backlog.md            # Отложенные улучшения и обобщения
│   ├── techdept.md           # Обязательный технический долг
│   └── logs/                 # Отчёты по задачам для PR и Code Review
├── .github/workflows/        # Автоаудит + сборка WebGL и деплой на Cloudflare
├── Assets/
│   ├── Editor/               # Точка входа CLI-сборки WebGLBuilder
│   ├── Settings/             # URP-ассеты (PC/Mobile), PanelSettings, Build Profiles
│   └── Showcase/             # POCO MVVM-вьюхи, ViewModel, UXML, USS и тесты
├── Packages/manifest.json    # Зависимости Golden Stack и com.reenbo.suif
└── wrangler.json             # Конфигурация Cloudflare Workers Static Assets
```

## 📄 Лицензия

MIT License — подробнее в [LICENSE](LICENSE).

