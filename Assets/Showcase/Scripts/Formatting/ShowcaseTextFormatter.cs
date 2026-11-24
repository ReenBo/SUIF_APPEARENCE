using Cysharp.Text;
using SUIF.Binding;
using SUIF.Showcase.Contracts;

namespace SUIF.Showcase.Formatting
{
    public sealed class ShowcaseTextFormatter : IShowcaseTextFormatter
    {
        public ShowcaseThemeId[] SupportedThemes { get; }

        public FixedTextWriter<ShowcaseModuleId> ModuleTitle { get; }
        public FixedTextWriter<ShowcaseModuleId> ModuleBaseReadout { get; }
        public FixedTextWriter<ShowcaseModuleId> ViewportIdBadge { get; }
        public FixedTextWriter<ShowcaseModuleId> ViewportSubBadge { get; }
        public FixedTextWriter<FrameNavigationDto> FrameOpenState { get; }
        public FixedTextWriter<FrameNavigationDto> CloseButtonHint { get; }
        public FixedTextWriter<FrameNavigationDto> ModalTelemetry { get; }
        public FixedTextWriter<FrameNavigationDto> LayerTag { get; }
        public FixedTextWriter<FrameNavigationDto> ViewportCoords { get; }
        public FixedTextWriter<FrameNavigationDto> FooterStatus { get; }
        public FixedTextWriter<bool> BenchmarkButtonText { get; }
        public FixedTextWriter<FrameTelemetryDto> TelemetryFps { get; }
        public FixedTextWriter<FrameTelemetryDto> TelemetryDrawCalls { get; }
        public FixedTextWriter<LifecycleLogEntryDto> LifecycleLogEntry { get; }
        public FixedTextWriter<int> CounterValue { get; }
        public FixedTextWriter<int> RecycledNodeIndex { get; }
        public FixedTextWriter<byte> RecycledNodePool { get; }
        public FixedTextWriter<uint> RecycledNodeAddress { get; }
        public FixedTextWriter<RecycledNodeStatus> RecycledNodeStatusWriter { get; }
        public FixedTextWriter<float> RecycledNodeLatency { get; }
        public FixedTextWriter<int> ModalDepth { get; }
        public FixedTextWriter<ModalPresetId> ModalTitle { get; }
        public FixedTextWriter<ModalPresetId> ModalBody { get; }
        public FixedTextWriter<InventoryItemCode> InventoryCode { get; }
        public FixedTextWriter<InventoryItemCode> InventoryName { get; }
        public FixedTextWriter<InventoryItemCategory> InventoryCategory { get; }
        public FixedTextWriter<float> InventoryWeight { get; }
        public FixedTextWriter<InventoryRarity> InventoryRarityBadge { get; }
        public FixedTextWriter<InventoryItemDto> InventoryStat { get; }
        public FixedTextWriter<InventoryItemCode> InventoryDescription { get; }
        public FixedTextWriter<int> LoadingSpinnerGlyph { get; }
        public FixedTextWriter<LoadingStateDto> LoadingStatus { get; }
        public FixedTextWriter<FutureSlotId> HubToastText { get; }
        public FixedTextWriter<TahoeDesktopStateDto> TahoeStageHeader { get; }
        public FixedTextWriter<FrameTelemetryDto> TahoeFpsReadout { get; }
        public FixedTextWriter<FrameTelemetryDto> TahoeDcReadout { get; }
        public FixedTextWriter<bool> TahoeBenchmarkBadge { get; }
        public FixedTextWriter<LifecycleStageId> TahoeLifecycleStage { get; }
        public FixedTextWriter<int> TahoeCounterValue { get; }
        public FixedTextWriter<int> TahoeInv100SlotCode { get; }
        public FixedTextWriter<int> TahoeInv100Title { get; }
        public FixedTextWriter<int> TahoeInv100Meta { get; }
        public FixedTextWriter<int> TahoeInv100Rarity { get; }
        public FixedTextWriter<int> TahoeListRowIndex { get; }
        public FixedTextWriter<int> TahoeListRowTitle { get; }
        public FixedTextWriter<int> TahoeListRowSlot { get; }

        public ShowcaseTextFormatter()
        {
            SupportedThemes = new[]
            {
                ShowcaseThemeId.Mono,
                ShowcaseThemeId.Cyan,
                ShowcaseThemeId.Crimson,
                ShowcaseThemeId.Amber
            };

            ModuleTitle = WriteModuleTitle;
            ModuleBaseReadout = WriteModuleBaseReadout;
            ViewportIdBadge = WriteViewportIdBadge;
            ViewportSubBadge = WriteViewportSubBadge;
            FrameOpenState = WriteFrameOpenState;
            CloseButtonHint = WriteCloseButtonHint;
            ModalTelemetry = WriteModalTelemetry;
            LayerTag = WriteLayerTag;
            ViewportCoords = WriteViewportCoords;
            FooterStatus = WriteFooterStatus;
            BenchmarkButtonText = WriteBenchmarkButtonText;
            TelemetryFps = WriteTelemetryFps;
            TelemetryDrawCalls = WriteTelemetryDrawCalls;
            LifecycleLogEntry = WriteLifecycleLogEntry;
            CounterValue = WriteCounterValue;
            RecycledNodeIndex = WriteRecycledNodeIndex;
            RecycledNodePool = WriteRecycledNodePool;
            RecycledNodeAddress = WriteRecycledNodeAddress;
            RecycledNodeStatusWriter = WriteRecycledNodeStatus;
            RecycledNodeLatency = WriteRecycledNodeLatency;
            ModalDepth = WriteModalDepth;
            ModalTitle = WriteModalTitle;
            ModalBody = WriteModalBody;
            InventoryCode = WriteInventoryCode;
            InventoryName = WriteInventoryName;
            InventoryCategory = WriteInventoryCategory;
            InventoryWeight = WriteInventoryWeight;
            InventoryRarityBadge = WriteInventoryRarityBadge;
            InventoryStat = WriteInventoryStat;
            InventoryDescription = WriteInventoryDescription;
            LoadingSpinnerGlyph = WriteLoadingSpinnerGlyph;
            LoadingStatus = WriteLoadingStatus;
            HubToastText = WriteHubToastText;
            TahoeStageHeader = WriteTahoeStageHeader;
            TahoeFpsReadout = WriteTahoeFpsReadout;
            TahoeDcReadout = WriteTahoeDcReadout;
            TahoeBenchmarkBadge = WriteTahoeBenchmarkBadge;
            TahoeLifecycleStage = WriteTahoeLifecycleStage;
            TahoeCounterValue = WriteTahoeCounterValue;
            TahoeInv100SlotCode = WriteTahoeInv100SlotCode;
            TahoeInv100Title = WriteTahoeInv100Title;
            TahoeInv100Meta = WriteTahoeInv100Meta;
            TahoeInv100Rarity = WriteTahoeInv100Rarity;
            TahoeListRowIndex = WriteTahoeListRowIndex;
            TahoeListRowTitle = WriteTahoeListRowTitle;
            TahoeListRowSlot = WriteTahoeListRowSlot;
        }

        public string GetTahoeThemeCssClass(TahoeThemeId themeId)
        {
            return themeId switch
            {
                TahoeThemeId.White => "theme-tahoe-white",
                TahoeThemeId.Indigo => "theme-tahoe-indigo",
                TahoeThemeId.Crimson => "theme-tahoe-crimson",
                _ => "theme-tahoe-dark"
            };
        }

        public string GetThemeCssClass(ShowcaseThemeId themeId)
        {
            return themeId switch
            {
                ShowcaseThemeId.Cyan => "theme-cyan",
                ShowcaseThemeId.Crimson => "theme-crimson",
                ShowcaseThemeId.Amber => "theme-amber",
                _ => "theme-mono"
            };
        }

        public void WriteModuleTitle(ref Utf16ValueStringBuilder sb, ShowcaseModuleId module)
        {
            sb.Append(module switch
            {
                ShowcaseModuleId.Lifecycle => "Basic Window Lifecycle",
                ShowcaseModuleId.Counter => "MVVM & R3 Reactive Counter",
                ShowcaseModuleId.RecycledList => "Virtualized Recycled List",
                ShowcaseModuleId.Inventory => "INVENTORY",
                ShowcaseModuleId.ThemeMatrix => "Dynamic Theme Switcher",
                _ => "[ NO_ACTIVE_VIEW // IDLE ]"
            });
        }

        public void WriteModuleBaseReadout(ref Utf16ValueStringBuilder sb, ShowcaseModuleId module)
        {
            sb.Append(module switch
            {
                ShowcaseModuleId.Lifecycle => "01_LIFECYCLE",
                ShowcaseModuleId.Counter => "02_R3_MVVM",
                ShowcaseModuleId.RecycledList => "03_RECYCLED_LIST",
                ShowcaseModuleId.Inventory => "05_INVENTORY",
                ShowcaseModuleId.ThemeMatrix => "06_THEME_MATRIX",
                _ => "NONE // IDLE"
            });
        }

        public void WriteViewportIdBadge(ref Utf16ValueStringBuilder sb, ShowcaseModuleId module)
        {
            int navIndex = GetModuleNavNumber(module);
            sb.Append("VIEWPORT_0");
            sb.Append(navIndex);
        }

        public void WriteViewportSubBadge(ref Utf16ValueStringBuilder sb, ShowcaseModuleId module)
        {
            sb.Append(module switch
            {
                ShowcaseModuleId.Lifecycle => "// LIFECYCLE_STATE_MACHINE",
                ShowcaseModuleId.Counter => "// REACTIVE_PROPERTY_STREAM",
                ShowcaseModuleId.RecycledList => "// ZERO_ALLOC_SCROLL_POOL",
                ShowcaseModuleId.Inventory => "// TACTICAL_GRID_STORAGE",
                ShowcaseModuleId.ThemeMatrix => "// GLOBAL_SHADER_PALETTE",
                _ => "// STANDBY_BUFFER"
            });
        }

        public void WriteFrameOpenState(ref Utf16ValueStringBuilder sb, FrameNavigationDto dto)
        {
            bool isOpen = dto.ActiveModule != ShowcaseModuleId.Standby || dto.IsModalOpen;
            sb.Append(isOpen ? "open" : "closed");
        }

        public void WriteCloseButtonHint(ref Utf16ValueStringBuilder sb, FrameNavigationDto dto)
        {
            sb.Append(dto.IsModalOpen ? "CLS // OVR" : "CLS // WIN");
        }

        public void WriteModalTelemetry(ref Utf16ValueStringBuilder sb, FrameNavigationDto dto)
        {
            sb.Append(dto.IsModalOpen ? "MOUNTED [1]" : "STANDBY [0]");
        }

        public void WriteLayerTag(ref Utf16ValueStringBuilder sb, FrameNavigationDto dto)
        {
            int navIndex = GetModuleNavNumber(dto.ActiveModule);
            sb.Append("LAYER_Z // ");
            sb.Append(navIndex);
            sb.Append(dto.IsModalOpen ? "_BASE + MODAL_999" : "_BASE_100");
        }

        public void WriteViewportCoords(ref Utf16ValueStringBuilder sb, FrameNavigationDto dto)
        {
            if (dto.IsModalOpen)
            {
                sb.Append("BASE: ");
                sb.Append(GetModuleNavNumber(dto.ActiveModule));
                sb.Append(" // OVERLAY: MODALS_04");
                return;
            }

            sb.Append(dto.ActiveModule == ShowcaseModuleId.Standby ? "BASE_STATE: STANDBY" : "BASE_STATE: ACTIVE");
        }

        public void WriteFooterStatus(ref Utf16ValueStringBuilder sb, FrameNavigationDto dto)
        {
            switch (dto.LastStatus)
            {
                case FrameActionStatus.ModuleOpened:
                    sb.Append("OPENED // ");
                    sb.Append(dto.ActiveModule switch
                    {
                        ShowcaseModuleId.Lifecycle => "LIFECYCLE_WINDOW",
                        ShowcaseModuleId.Counter => "COUNTER_WINDOW",
                        ShowcaseModuleId.RecycledList => "RECYCLED_LIST_WINDOW",
                        ShowcaseModuleId.Inventory => "INVENTORY_WINDOW",
                        ShowcaseModuleId.ThemeMatrix => "THEME_MATRIX_WINDOW",
                        _ => "BASE_WINDOW"
                    });
                    break;
                case FrameActionStatus.ModuleClosed:
                    sb.Append("CLOSED // BASE_WINDOW");
                    break;
                case FrameActionStatus.ModalMounted:
                case FrameActionStatus.ModalUnmounted:
                    sb.Append(dto.IsModalOpen ? "MOUNTED // MODAL_OVERLAY" : "UNMOUNTED // MODAL_OVERLAY");
                    break;
                case FrameActionStatus.BenchmarkRunning:
                    sb.Append("BENCHMARK // RECORDING");
                    break;
                case FrameActionStatus.BenchmarkCompleted:
                    sb.Append("BENCHMARK // COMPLETED");
                    break;
                default:
                    sb.Append("STATE_SYNC // READY");
                    break;
            }
        }

        public void WriteBenchmarkButtonText(ref Utf16ValueStringBuilder sb, bool isRecording)
        {
            sb.Append(isRecording ? "TESTING..." : "RUN BENCHMARK");
        }

        public void WriteTelemetryFps(ref Utf16ValueStringBuilder sb, FrameTelemetryDto dto)
        {
            sb.AppendFormat("{0:F1}", dto.Fps);
            sb.Append(" FPS");
        }

        public void WriteTelemetryDrawCalls(ref Utf16ValueStringBuilder sb, FrameTelemetryDto dto)
        {
            sb.Append('[');
            sb.Append(dto.DrawCalls);
            sb.Append(" DC]");
        }

        public void WriteLifecycleLogEntry(ref Utf16ValueStringBuilder sb, LifecycleLogEntryDto entry)
        {
            sb.Append("> [");
            AppendTwoDigits(ref sb, entry.Hour);
            sb.Append(':');
            AppendTwoDigits(ref sb, entry.Minute);
            sb.Append(entry.IsInitialBootEntry ? '.' : ':');
            AppendTwoDigits(ref sb, entry.Second);
            if (!entry.IsInitialBootEntry)
            {
                sb.Append('.');
                AppendTwoDigits(ref sb, entry.Centisecond);
            }
            sb.Append("] ");

            sb.Append(entry.Stage switch
            {
                LifecycleStageId.Initialize => entry.IsInitialBootEntry
                    ? "OnInitialize() -> Memory blocks pre-warmed"
                    : "OnInitialize() -> State transition executed",
                LifecycleStageId.Open => entry.IsInitialBootEntry
                    ? "OnOpen()       -> View hierarchy attached to SUIF Root"
                    : "OnOpen() -> State transition executed",
                LifecycleStageId.Show => entry.IsInitialBootEntry
                    ? "OnShow()       -> Transition animation completed [ACTIVE]"
                    : "OnShow() -> State transition executed",
                LifecycleStageId.Hide => "OnHide() -> State transition executed",
                LifecycleStageId.Close => "OnClose() -> State transition executed",
                _ => "Unknown() -> State transition executed"
            });
        }

        public void WriteCounterValue(ref Utf16ValueStringBuilder sb, int value)
        {
            sb.AppendFormat("{0:D3}", value);
        }

        public void WriteRecycledNodeIndex(ref Utf16ValueStringBuilder sb, int index)
        {
            sb.Append('#');
            sb.AppendFormat("{0:D4}", index);
        }

        public void WriteRecycledNodePool(ref Utf16ValueStringBuilder sb, byte slot)
        {
            sb.Append("NODE_");
            AppendTwoDigits(ref sb, slot);
        }

        public void WriteRecycledNodeAddress(ref Utf16ValueStringBuilder sb, uint address)
        {
            sb.Append("MEM: 0x");
            sb.AppendFormat("{0:X6}", address);
        }

        public void WriteRecycledNodeStatus(ref Utf16ValueStringBuilder sb, RecycledNodeStatus status)
        {
            sb.Append(status == RecycledNodeStatus.HotSwap ? "HOT_SWAP" : "RECYCLED");
        }

        public void WriteRecycledNodeLatency(ref Utf16ValueStringBuilder sb, float latencyMs)
        {
            sb.AppendFormat("{0:F2}ms", latencyMs);
        }

        public void WriteModalDepth(ref Utf16ValueStringBuilder sb, int depth)
        {
            sb.Append("DEPTH: ");
            sb.Append(depth);
        }

        public void WriteModalTitle(ref Utf16ValueStringBuilder sb, ModalPresetId preset)
        {
            sb.Append(preset switch
            {
                ModalPresetId.PopupIntercept => "DANGER // POPUP_INTERCEPT",
                ModalPresetId.MemoryPurge => "CONFIRM // MEMORY_PURGE_0x04",
                _ => "SYSTEM_ALERT // OVERLAY_PRIORITY"
            });
        }

        public void WriteModalBody(ref Utf16ValueStringBuilder sb, ModalPresetId preset)
        {
            sb.Append(preset switch
            {
                ModalPresetId.PopupIntercept => "This modal layer is non-destructively mounted over the active base window. Underlying reactive streams and lifecycle states remain preserved in memory.",
                ModalPresetId.MemoryPurge => "Verify zero-allocation pool flush before unmounting active SUIF container. Modal stack isolates user input until resolved.",
                _ => "High-priority FUI notification injected at Z-Layer 999. Base window continues rendering beneath the scanline backdrop."
            });
        }

        public void WriteInventoryCode(ref Utf16ValueStringBuilder sb, InventoryItemCode code)
        {
            sb.Append(code switch
            {
                InventoryItemCode.Cyb01 => "CYB_01",
                InventoryItemCode.Opt02 => "OPT_02",
                InventoryItemCode.Deck03 => "DECK_03",
                InventoryItemCode.Mod04 => "MOD_04",
                InventoryItemCode.Stim05 => "STIM_05",
                InventoryItemCode.Arm06 => "ARM_06",
                InventoryItemCode.Emp07 => "EMP_07",
                InventoryItemCode.Key08 => "KEY_08",
                _ => "UNK_00"
            });
        }

        public void WriteInventoryName(ref Utf16ValueStringBuilder sb, InventoryItemCode code)
        {
            sb.Append(code switch
            {
                InventoryItemCode.Cyb01 => "MANTIS_BLADE_MK4",
                InventoryItemCode.Opt02 => "KIROSHI_ORACLE_V3",
                InventoryItemCode.Deck03 => "NETWATCH_RIPPLER",
                InventoryItemCode.Mod04 => "R3_KERNEL_BOOSTER",
                InventoryItemCode.Stim05 => "MAXDOC_INHALER",
                InventoryItemCode.Arm06 => "SUBDERMAL_TITANIUM",
                InventoryItemCode.Emp07 => "VOLT_GRENADE_X",
                InventoryItemCode.Key08 => "ARASAKA_AUTH_SHARD",
                _ => "UNKNOWN_ITEM"
            });
        }

        public void WriteInventoryCategory(ref Utf16ValueStringBuilder sb, InventoryItemCategory category)
        {
            sb.Append(category switch
            {
                InventoryItemCategory.Cyberware => "CYBERWARE",
                InventoryItemCategory.Optics => "OPTICS",
                InventoryItemCategory.Cyberdeck => "CYBERDECK",
                InventoryItemCategory.Chipware => "CHIPWARE",
                InventoryItemCategory.Consumable => "CONSUMABLE",
                InventoryItemCategory.Armor => "ARMOR",
                InventoryItemCategory.Ordnance => "ORDNANCE",
                InventoryItemCategory.KeyItem => "KEY_ITEM",
                _ => "MISC"
            });
        }

        public void WriteInventoryWeight(ref Utf16ValueStringBuilder sb, float weightKg)
        {
            sb.AppendFormat("{0:F1} KG", weightKg);
        }

        public void WriteInventoryRarityBadge(ref Utf16ValueStringBuilder sb, InventoryRarity rarity)
        {
            sb.Append('[');
            sb.Append(rarity switch
            {
                InventoryRarity.Common => "COMMON",
                InventoryRarity.Rare => "RARE",
                InventoryRarity.Tactical => "TACTICAL",
                InventoryRarity.Heavy => "HEAVY",
                InventoryRarity.Encrypted => "ENCRYPTED",
                InventoryRarity.SpecOps => "SPEC_OPS",
                InventoryRarity.Legendary => "LEGENDARY",
                InventoryRarity.Iconic => "ICONIC",
                _ => "STANDARD"
            });
            sb.Append(']');
        }

        public void WriteInventoryStat(ref Utf16ValueStringBuilder sb, InventoryItemDto item)
        {
            switch (item.StatKind)
            {
                case InventoryStatKind.Dps:
                    sb.Append(item.StatValue);
                    sb.Append(" DPS");
                    break;
                case InventoryStatKind.Zoom:
                    sb.Append(item.StatValue);
                    sb.Append("X ZOOM");
                    break;
                case InventoryStatKind.Ram:
                    sb.Append(item.StatValue);
                    sb.Append(" RAM");
                    break;
                case InventoryStatKind.TickBonusPercent:
                    sb.Append('+');
                    sb.Append(item.StatValue);
                    sb.Append("% TICK");
                    break;
                case InventoryStatKind.HealPercent:
                    sb.Append('+');
                    sb.Append(item.StatValue);
                    sb.Append("% HP");
                    break;
                case InventoryStatKind.Armor:
                    sb.Append('+');
                    sb.Append(item.StatValue);
                    sb.Append(" ARM");
                    break;
                case InventoryStatKind.Emp:
                    sb.Append(item.StatValue);
                    sb.Append(" EMP");
                    break;
                case InventoryStatKind.ClearanceLevel:
                    sb.Append("LVL_");
                    sb.Append(item.StatValue);
                    break;
            }
        }

        public void WriteInventoryDescription(ref Utf16ValueStringBuilder sb, InventoryItemCode code)
        {
            sb.Append(code switch
            {
                InventoryItemCode.Cyb01 => "High-frequency thermal monowire edge with neural reflex link.",
                InventoryItemCode.Opt02 => "Threat-highlighting FUI target reticle overlay with wall-penetrating sonar.",
                InventoryItemCode.Deck03 => "Spreads quickhack daemons across 4 adjacent subnet nodes.",
                InventoryItemCode.Mod04 => "Accelerates MVVM reactive property bindings with zero GC spikes.",
                InventoryItemCode.Stim05 => "Instant nanite tissue reconstruction compound.",
                InventoryItemCode.Arm06 => "Ballistic weave plated directly beneath epidermis.",
                InventoryItemCode.Emp07 => "Disables robotic actuators and HUD overlays within 8m radius.",
                InventoryItemCode.Key08 => "Biometric clearance token for orbital relay terminal.",
                _ => string.Empty
            });
        }

        public void WriteLoadingSpinnerGlyph(ref Utf16ValueStringBuilder sb, int step)
        {
            sb.Append((step & 3) switch
            {
                0 => "[|]",
                1 => "[/]",
                2 => "[-]",
                _ => "[\\]"
            });
        }

        public void WriteLoadingStatus(ref Utf16ValueStringBuilder sb, LoadingStateDto dto)
        {
            sb.Append(dto.Phase switch
            {
                LoadingPhase.Preloading => "PRELOADING // ",
                LoadingPhase.Completed => "READY // ",
                _ => "STANDBY // "
            });
            sb.Append(dto.CompletedTasks);
            sb.Append('/');
            sb.Append(dto.TotalTasks);
        }

        public int GetModuleNavNumber(ShowcaseModuleId module)
        {
            return module switch
            {
                ShowcaseModuleId.Lifecycle => 1,
                ShowcaseModuleId.Counter => 2,
                ShowcaseModuleId.RecycledList => 3,
                ShowcaseModuleId.Modals => 4,
                ShowcaseModuleId.Inventory => 5,
                ShowcaseModuleId.ThemeMatrix => 6,
                _ => 0
            };
        }

        private static void AppendTwoDigits(ref Utf16ValueStringBuilder sb, int value)
        {
            int clamped = value < 0 ? 0 : value % 100;
            sb.Append((char)('0' + clamped / 10));
            sb.Append((char)('0' + clamped % 10));
        }

        private static void AppendThreeDigits(ref Utf16ValueStringBuilder sb, int value)
        {
            int clamped = value < 0 ? 0 : value % 1000;
            sb.Append((char)('0' + clamped / 100));
            sb.Append((char)('0' + (clamped / 10) % 10));
            sb.Append((char)('0' + clamped % 10));
        }

        public void WriteHubToastText(ref Utf16ValueStringBuilder sb, FutureSlotId slot)
        {
            sb.Append(slot == FutureSlotId.Future1 ? "Future1 — Coming Soon" : "Future2 — Coming Soon");
        }

        public void WriteTahoeStageHeader(ref Utf16ValueStringBuilder sb, TahoeDesktopStateDto state)
        {
            if (!state.HasFocusedWindow)
            {
                sb.Append("IDLE -> NONE");
                return;
            }

            sb.Append("OPEN -> ");
            sb.Append(state.FocusedWindow switch
            {
                TahoeWindowId.Lifecycle => "BASIC WINDOW LIFECYCLE",
                TahoeWindowId.Counter => "MVVM & R3 REACTIVE COUNTER",
                TahoeWindowId.RecycledList => "VIRTUALIZED RECYCLED LIST",
                TahoeWindowId.Modals => "MODALS/POPUPS",
                TahoeWindowId.Inventory6 => "INVENTORY (6 ITEMS)",
                TahoeWindowId.Inventory100 => "INVENTORY (100 ITEMS)",
                TahoeWindowId.Themes => "DYNAMIC THEME SWITCHER",
                _ => "NONE"
            });
        }

        public void WriteTahoeFpsReadout(ref Utf16ValueStringBuilder sb, FrameTelemetryDto dto)
        {
            sb.Append("FPS: ");
            sb.Append((int)dto.Fps);
        }

        public void WriteTahoeDcReadout(ref Utf16ValueStringBuilder sb, FrameTelemetryDto dto)
        {
            sb.Append("DC: ");
            sb.Append(dto.DrawCalls);
        }

        public void WriteTahoeBenchmarkBadge(ref Utf16ValueStringBuilder sb, bool isRunning)
        {
            sb.Append(isRunning ? "Running Stress Benchmark..." : "Benchmark Complete: 0 GC Alloc");
        }

        public void WriteTahoeLifecycleStage(ref Utf16ValueStringBuilder sb, LifecycleStageId stage)
        {
            sb.Append(stage switch
            {
                LifecycleStageId.Initialize => "OnInitialize()",
                LifecycleStageId.Open => "OnOpen()",
                LifecycleStageId.Show => "OnShow()",
                LifecycleStageId.Hide => "OnHide()",
                LifecycleStageId.Close => "OnClose()",
                _ => "OnShow()"
            });
        }

        public void WriteTahoeCounterValue(ref Utf16ValueStringBuilder sb, int value)
        {
            sb.Append(value);
        }

        public void WriteTahoeInv100SlotCode(ref Utf16ValueStringBuilder sb, int index)
        {
            sb.Append('#');
            AppendThreeDigits(ref sb, index + 1);
        }

        public void WriteTahoeInv100Title(ref Utf16ValueStringBuilder sb, int index)
        {
            sb.Append("SUIF Module #");
            AppendThreeDigits(ref sb, index + 1);
        }

        public void WriteTahoeInv100Meta(ref Utf16ValueStringBuilder sb, int index)
        {
            int qty = ((index * 3) % 64) + 1;
            sb.Append("Slot #");
            AppendThreeDigits(ref sb, index + 1);
            sb.Append(" · Qty x");
            sb.Append(qty);
        }

        public void WriteTahoeInv100Rarity(ref Utf16ValueStringBuilder sb, int index)
        {
            int tier = (index * 7 + 3) % 4;
            sb.Append(tier switch
            {
                0 => "Common",
                1 => "Rare",
                2 => "Epic",
                _ => "Mythic"
            });
        }

        public void WriteTahoeListRowIndex(ref Utf16ValueStringBuilder sb, int index)
        {
            sb.Append('#');
            AppendThreeDigits(ref sb, index + 1);
        }

        public void WriteTahoeListRowTitle(ref Utf16ValueStringBuilder sb, int index)
        {
            sb.Append("Recycled Row Item #");
            AppendThreeDigits(ref sb, index + 1);
        }

        public void WriteTahoeListRowSlot(ref Utf16ValueStringBuilder sb, int index)
        {
            sb.Append("VIEW_0");
            sb.Append((index % 6) + 1);
        }
    }
}
