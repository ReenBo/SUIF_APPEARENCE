namespace SUIF.Showcase.Contracts
{
    public enum ShowcaseThemeId : byte
    {
        Mono,
        Cyan,
        Crimson,
        Amber
    }

    public enum ShowcaseModuleId : byte
    {
        Standby,
        Lifecycle,
        Counter,
        RecycledList,
        Modals,
        Inventory,
        ThemeMatrix
    }

    public enum FrameActionStatus : byte
    {
        Ready,
        ModuleOpened,
        ModuleClosed,
        ModalMounted,
        ModalUnmounted,
        BenchmarkRunning,
        BenchmarkCompleted
    }

    public readonly struct ShowcaseThemeDto
    {
        public readonly ShowcaseThemeId Id;

        public ShowcaseThemeDto(ShowcaseThemeId id)
        {
            Id = id;
        }
    }

    public readonly struct FrameNavigationDto
    {
        public readonly ShowcaseModuleId ActiveModule;
        public readonly bool IsModalOpen;
        public readonly FrameActionStatus LastStatus;

        public FrameNavigationDto(ShowcaseModuleId activeModule, bool isModalOpen, FrameActionStatus lastStatus)
        {
            ActiveModule = activeModule;
            IsModalOpen = isModalOpen;
            LastStatus = lastStatus;
        }
    }

    public readonly struct FrameTelemetryDto
    {
        public readonly float Fps;
        public readonly long DrawCalls;
        public readonly long Batches;
        public readonly float MemoryMb;
        public readonly bool IsRecording;

        public FrameTelemetryDto(float fps, long drawCalls, long batches, float memoryMb, bool isRecording)
        {
            Fps = fps;
            DrawCalls = drawCalls;
            Batches = batches;
            MemoryMb = memoryMb;
            IsRecording = isRecording;
        }
    }

    public enum LifecycleStageId : byte
    {
        Initialize,
        Open,
        Show,
        Hide,
        Close
    }

    public readonly struct LifecycleLogEntryDto
    {
        public readonly int Hour;
        public readonly int Minute;
        public readonly int Second;
        public readonly int Centisecond;
        public readonly LifecycleStageId Stage;
        public readonly bool IsInitialBootEntry;

        public LifecycleLogEntryDto(
            int hour,
            int minute,
            int second,
            int centisecond,
            LifecycleStageId stage,
            bool isInitialBootEntry = false)
        {
            Hour = hour;
            Minute = minute;
            Second = second;
            Centisecond = centisecond;
            Stage = stage;
            IsInitialBootEntry = isInitialBootEntry;
        }
    }

    public readonly struct LifecycleViewDto
    {
        public readonly LifecycleStageId CurrentStage;
        public readonly int LogCount;

        public LifecycleViewDto(LifecycleStageId currentStage, int logCount)
        {
            CurrentStage = currentStage;
            LogCount = logCount;
        }
    }

    public readonly struct CounterViewDto
    {
        public readonly int CurrentValue;
        public readonly int HistoryCount;

        public CounterViewDto(int currentValue, int historyCount)
        {
            CurrentValue = currentValue;
            HistoryCount = historyCount;
        }
    }

    public enum RecycledNodeStatus : byte
    {
        Recycled,
        HotSwap
    }

    public readonly struct RecycledNodeDto
    {
        public readonly int Index;
        public readonly byte PoolSlot;
        public readonly uint MemoryAddress;
        public readonly RecycledNodeStatus Status;
        public readonly float LatencyMs;

        public RecycledNodeDto(
            int index,
            byte poolSlot,
            uint memoryAddress,
            RecycledNodeStatus status,
            float latencyMs)
        {
            Index = index;
            PoolSlot = poolSlot;
            MemoryAddress = memoryAddress;
            Status = status;
            LatencyMs = latencyMs;
        }
    }

    public enum ModalPresetId : byte
    {
        PopupIntercept,
        MemoryPurge,
        OverlayPriority
    }

    public readonly struct ModalViewDto
    {
        public readonly int Depth;
        public readonly ModalPresetId Preset;

        public ModalViewDto(int depth, ModalPresetId preset)
        {
            Depth = depth;
            Preset = preset;
        }
    }

    public enum InventoryItemCode : byte
    {
        Cyb01,
        Opt02,
        Deck03,
        Mod04,
        Stim05,
        Arm06,
        Emp07,
        Key08
    }

    public enum InventoryItemCategory : byte
    {
        Cyberware,
        Optics,
        Cyberdeck,
        Chipware,
        Consumable,
        Armor,
        Ordnance,
        KeyItem
    }

    public enum InventoryRarity : byte
    {
        Common,
        Rare,
        Tactical,
        Heavy,
        Encrypted,
        SpecOps,
        Legendary,
        Iconic
    }

    public enum InventoryStatKind : byte
    {
        Dps,
        Zoom,
        Ram,
        TickBonusPercent,
        HealPercent,
        Armor,
        Emp,
        ClearanceLevel
    }

    public readonly struct InventoryItemDto
    {
        public readonly InventoryItemCode Code;
        public readonly InventoryItemCategory Category;
        public readonly float WeightKg;
        public readonly InventoryRarity Rarity;
        public readonly InventoryStatKind StatKind;
        public readonly int StatValue;

        public InventoryItemDto(
            InventoryItemCode code,
            InventoryItemCategory category,
            float weightKg,
            InventoryRarity rarity,
            InventoryStatKind statKind,
            int statValue)
        {
            Code = code;
            Category = category;
            WeightKg = weightKg;
            Rarity = rarity;
            StatKind = statKind;
            StatValue = statValue;
        }
    }

    public readonly struct InventoryViewDto
    {
        public readonly int SelectedIndex;
        public readonly float TotalWeightKg;

        public InventoryViewDto(int selectedIndex, float totalWeightKg)
        {
            SelectedIndex = selectedIndex;
            TotalWeightKg = totalWeightKg;
        }
    }

    public readonly struct ThemeMatrixViewDto
    {
        public readonly ShowcaseThemeId ActiveTheme;

        public ThemeMatrixViewDto(ShowcaseThemeId activeTheme)
        {
            ActiveTheme = activeTheme;
        }
    }

    public enum LoadingPhase : byte
    {
        Idle,
        Preloading,
        Completed
    }

    public readonly struct LoadingStateDto
    {
        public readonly LoadingPhase Phase;
        public readonly int CompletedTasks;
        public readonly int TotalTasks;
        public readonly int SpinnerStep;

        public LoadingStateDto(LoadingPhase phase, int completedTasks, int totalTasks, int spinnerStep)
        {
            Phase = phase;
            CompletedTasks = completedTasks;
            TotalTasks = totalTasks;
            SpinnerStep = spinnerStep;
        }
    }

    public enum HubStyleType : byte
    {
        CyberPunkStyle,
        MacOSTahoe
    }

    public enum FutureSlotId : byte
    {
        Future1,
        Future2
    }

    public enum HubScreenId : byte
    {
        Hub,
        CyberPunkStyle,
        MacOSTahoe
    }

    public readonly struct HubToastDto
    {
        public readonly bool IsVisible;
        public readonly FutureSlotId Slot;
        public readonly int TriggerId;

        public HubToastDto(bool isVisible, FutureSlotId slot, int triggerId)
        {
            IsVisible = isVisible;
            Slot = slot;
            TriggerId = triggerId;
        }
    }

    public enum TahoeThemeId : byte
    {
        Dark,
        White,
        Indigo,
        Crimson
    }

    public enum TahoeWindowId : byte
    {
        Lifecycle = 0,
        Counter = 1,
        RecycledList = 2,
        Modals = 3,
        Inventory6 = 4,
        Inventory100 = 5,
        Themes = 6
    }

    public readonly struct TahoeDesktopStateDto
    {
        public readonly TahoeThemeId Theme;
        public readonly bool HasFocusedWindow;
        public readonly TahoeWindowId FocusedWindow;
        public readonly int OpenMask;
        public readonly bool IsModalSheetOpen;
        public readonly bool IsBenchmarkRunning;
        public readonly bool IsBenchmarkCompleted;
        public readonly int Inventory100SelectedIndex;
        public readonly int ListScrollStep;
        public readonly int Inv100ScrollStep;

        public TahoeDesktopStateDto(
            TahoeThemeId theme,
            bool hasFocusedWindow,
            TahoeWindowId focusedWindow,
            int openMask,
            bool isModalSheetOpen,
            bool isBenchmarkRunning,
            bool isBenchmarkCompleted,
            int inventory100SelectedIndex,
            int listScrollStep,
            int inv100ScrollStep)
        {
            Theme = theme;
            HasFocusedWindow = hasFocusedWindow;
            FocusedWindow = focusedWindow;
            OpenMask = openMask;
            IsModalSheetOpen = isModalSheetOpen;
            IsBenchmarkRunning = isBenchmarkRunning;
            IsBenchmarkCompleted = isBenchmarkCompleted;
            Inventory100SelectedIndex = inventory100SelectedIndex;
            ListScrollStep = listScrollStep;
            Inv100ScrollStep = inv100ScrollStep;
        }

        public bool IsWindowOpen(TahoeWindowId windowId)
        {
            return (OpenMask & (1 << (int)windowId)) != 0;
        }
    }
}
