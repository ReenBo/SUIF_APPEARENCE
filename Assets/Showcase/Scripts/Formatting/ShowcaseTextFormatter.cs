using Cysharp.Text;
using SUIF.Showcase.Contracts;

namespace SUIF.Showcase.Formatting
{
    public static class ShowcaseTextFormatter
    {
        public static string GetThemeCssClass(ShowcaseThemeId themeId)
        {
            return themeId switch
            {
                ShowcaseThemeId.Cyan => "theme-cyan",
                ShowcaseThemeId.Crimson => "theme-crimson",
                ShowcaseThemeId.Amber => "theme-amber",
                _ => "theme-mono"
            };
        }

        public static void WriteModuleTitle(ref Utf16ValueStringBuilder sb, ShowcaseModuleId module)
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

        public static void WriteModuleBaseReadout(ref Utf16ValueStringBuilder sb, ShowcaseModuleId module)
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

        public static void WriteViewportIdBadge(ref Utf16ValueStringBuilder sb, ShowcaseModuleId module)
        {
            int navIndex = GetModuleNavNumber(module);
            sb.Append("VIEWPORT_0");
            sb.Append(navIndex);
        }

        public static void WriteViewportSubBadge(ref Utf16ValueStringBuilder sb, ShowcaseModuleId module)
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

        public static void WriteFrameOpenState(ref Utf16ValueStringBuilder sb, FrameNavigationDto dto)
        {
            bool isOpen = dto.ActiveModule != ShowcaseModuleId.Standby || dto.IsModalOpen;
            sb.Append(isOpen ? "open" : "closed");
        }

        public static void WriteCloseButtonHint(ref Utf16ValueStringBuilder sb, FrameNavigationDto dto)
        {
            sb.Append(dto.IsModalOpen ? "CLS // OVR" : "CLS // WIN");
        }

        public static void WriteModalTelemetry(ref Utf16ValueStringBuilder sb, FrameNavigationDto dto)
        {
            sb.Append(dto.IsModalOpen ? "MOUNTED [1]" : "STANDBY [0]");
        }

        public static void WriteLayerTag(ref Utf16ValueStringBuilder sb, FrameNavigationDto dto)
        {
            int navIndex = GetModuleNavNumber(dto.ActiveModule);
            sb.Append("LAYER_Z // ");
            sb.Append(navIndex);
            sb.Append(dto.IsModalOpen ? "_BASE + MODAL_999" : "_BASE_100");
        }

        public static void WriteViewportCoords(ref Utf16ValueStringBuilder sb, FrameNavigationDto dto)
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

        public static void WriteFooterStatus(ref Utf16ValueStringBuilder sb, FrameNavigationDto dto)
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
                        _ => "STANDBY"
                    });
                    break;
                case FrameActionStatus.ModuleClosed:
                    sb.Append("CLOSED // BASE_WINDOW");
                    break;
                case FrameActionStatus.ModalMounted:
                    sb.Append("MOUNTED // MODAL_OVERLAY");
                    break;
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

        public static void WriteBenchmarkButtonText(ref Utf16ValueStringBuilder sb, bool isRecording)
        {
            sb.Append(isRecording ? "TESTING..." : "RUN BENCHMARK");
        }

        public static void WriteTelemetryFps(ref Utf16ValueStringBuilder sb, FrameTelemetryDto dto)
        {
            sb.AppendFormat("{0:F1}", dto.Fps);
            sb.Append(" FPS");
        }

        public static void WriteTelemetryDrawCalls(ref Utf16ValueStringBuilder sb, FrameTelemetryDto dto)
        {
            sb.Append('[');
            sb.Append(dto.DrawCalls);
            sb.Append(" DC]");
        }

        public static void WriteLifecycleLogEntry(ref Utf16ValueStringBuilder sb, LifecycleLogEntryDto entry)
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

        public static void WriteCounterValue(ref Utf16ValueStringBuilder sb, int value)
        {
            sb.AppendFormat("{0:D3}", value);
        }

        public static void WriteRecycledNodeIndex(ref Utf16ValueStringBuilder sb, int index)
        {
            sb.Append('#');
            sb.AppendFormat("{0:D4}", index);
        }

        public static void WriteRecycledNodePool(ref Utf16ValueStringBuilder sb, byte slot)
        {
            sb.Append("NODE_");
            AppendTwoDigits(ref sb, slot);
        }

        public static void WriteRecycledNodeAddress(ref Utf16ValueStringBuilder sb, uint address)
        {
            sb.Append("MEM: 0x");
            sb.AppendFormat("{0:X6}", address);
        }

        public static void WriteRecycledNodeStatus(ref Utf16ValueStringBuilder sb, RecycledNodeStatus status)
        {
            sb.Append(status == RecycledNodeStatus.HotSwap ? "HOT_SWAP" : "RECYCLED");
        }

        public static void WriteRecycledNodeLatency(ref Utf16ValueStringBuilder sb, float latencyMs)
        {
            sb.AppendFormat("{0:F2}ms", latencyMs);
        }

        public static void WriteModalDepth(ref Utf16ValueStringBuilder sb, int depth)
        {
            sb.Append("DEPTH: ");
            sb.Append(depth);
        }

        public static void WriteModalTitle(ref Utf16ValueStringBuilder sb, ModalPresetId preset)
        {
            sb.Append(preset switch
            {
                ModalPresetId.PopupIntercept => "DANGER // POPUP_INTERCEPT",
                ModalPresetId.MemoryPurge => "CONFIRM // MEMORY_PURGE_0x04",
                _ => "SYSTEM_ALERT // OVERLAY_PRIORITY"
            });
        }

        public static void WriteModalBody(ref Utf16ValueStringBuilder sb, ModalPresetId preset)
        {
            sb.Append(preset switch
            {
                ModalPresetId.PopupIntercept => "This modal layer is non-destructively mounted over the active base window. Underlying reactive streams and lifecycle states remain preserved in memory.",
                ModalPresetId.MemoryPurge => "Verify zero-allocation pool flush before unmounting active SUIF container. Modal stack isolates user input until resolved.",
                _ => "High-priority FUI notification injected at Z-Layer 999. Base window continues rendering beneath the scanline backdrop."
            });
        }

        public static void WriteInventoryCode(ref Utf16ValueStringBuilder sb, InventoryItemCode code)
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

        public static void WriteInventoryName(ref Utf16ValueStringBuilder sb, InventoryItemCode code)
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

        public static void WriteInventoryCategory(ref Utf16ValueStringBuilder sb, InventoryItemCategory category)
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

        public static void WriteInventoryWeight(ref Utf16ValueStringBuilder sb, float weightKg)
        {
            sb.AppendFormat("{0:F1} KG", weightKg);
        }

        public static void WriteInventoryRarityBadge(ref Utf16ValueStringBuilder sb, InventoryRarity rarity)
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

        public static void WriteInventoryStat(ref Utf16ValueStringBuilder sb, InventoryItemDto item)
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

        public static void WriteInventoryDescription(ref Utf16ValueStringBuilder sb, InventoryItemCode code)
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

        public static int GetModuleNavNumber(ShowcaseModuleId module)
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
    }
}
