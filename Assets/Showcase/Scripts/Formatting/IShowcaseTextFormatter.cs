using Cysharp.Text;
using SUIF.Binding;
using SUIF.Showcase.Contracts;

namespace SUIF.Showcase.Formatting
{
    public interface IShowcaseTextFormatter
    {
        ShowcaseThemeId[] SupportedThemes { get; }

        FixedTextWriter<ShowcaseModuleId> ModuleTitle { get; }
        FixedTextWriter<ShowcaseModuleId> ModuleBaseReadout { get; }
        FixedTextWriter<ShowcaseModuleId> ViewportIdBadge { get; }
        FixedTextWriter<ShowcaseModuleId> ViewportSubBadge { get; }
        FixedTextWriter<FrameNavigationDto> FrameOpenState { get; }
        FixedTextWriter<FrameNavigationDto> CloseButtonHint { get; }
        FixedTextWriter<FrameNavigationDto> ModalTelemetry { get; }
        FixedTextWriter<FrameNavigationDto> LayerTag { get; }
        FixedTextWriter<FrameNavigationDto> ViewportCoords { get; }
        FixedTextWriter<FrameNavigationDto> FooterStatus { get; }
        FixedTextWriter<bool> BenchmarkButtonText { get; }
        FixedTextWriter<FrameTelemetryDto> TelemetryFps { get; }
        FixedTextWriter<FrameTelemetryDto> TelemetryDrawCalls { get; }
        FixedTextWriter<LifecycleLogEntryDto> LifecycleLogEntry { get; }
        FixedTextWriter<int> CounterValue { get; }
        FixedTextWriter<int> RecycledNodeIndex { get; }
        FixedTextWriter<byte> RecycledNodePool { get; }
        FixedTextWriter<uint> RecycledNodeAddress { get; }
        FixedTextWriter<RecycledNodeStatus> RecycledNodeStatusWriter { get; }
        FixedTextWriter<float> RecycledNodeLatency { get; }
        FixedTextWriter<int> ModalDepth { get; }
        FixedTextWriter<ModalPresetId> ModalTitle { get; }
        FixedTextWriter<ModalPresetId> ModalBody { get; }
        FixedTextWriter<InventoryItemCode> InventoryCode { get; }
        FixedTextWriter<InventoryItemCode> InventoryName { get; }
        FixedTextWriter<InventoryItemCategory> InventoryCategory { get; }
        FixedTextWriter<float> InventoryWeight { get; }
        FixedTextWriter<InventoryRarity> InventoryRarityBadge { get; }
        FixedTextWriter<InventoryItemDto> InventoryStat { get; }
        FixedTextWriter<InventoryItemCode> InventoryDescription { get; }
        FixedTextWriter<int> LoadingSpinnerGlyph { get; }
        FixedTextWriter<LoadingStateDto> LoadingStatus { get; }
        FixedTextWriter<FutureSlotId> HubToastText { get; }
        FixedTextWriter<TahoeDesktopStateDto> TahoeStageHeader { get; }
        FixedTextWriter<FrameTelemetryDto> TahoeFpsReadout { get; }
        FixedTextWriter<FrameTelemetryDto> TahoeDcReadout { get; }
        FixedTextWriter<bool> TahoeBenchmarkBadge { get; }
        FixedTextWriter<LifecycleStageId> TahoeLifecycleStage { get; }
        FixedTextWriter<int> TahoeCounterValue { get; }
        FixedTextWriter<int> TahoeInv100SlotCode { get; }
        FixedTextWriter<int> TahoeInv100Title { get; }
        FixedTextWriter<int> TahoeInv100Meta { get; }
        FixedTextWriter<int> TahoeInv100Rarity { get; }
        FixedTextWriter<int> TahoeListRowIndex { get; }
        FixedTextWriter<int> TahoeListRowTitle { get; }
        FixedTextWriter<int> TahoeListRowSlot { get; }

        string GetThemeCssClass(ShowcaseThemeId themeId);
        string GetTahoeThemeCssClass(TahoeThemeId themeId);
        int GetModuleNavNumber(ShowcaseModuleId module);

        void WriteModuleTitle(ref Utf16ValueStringBuilder sb, ShowcaseModuleId module);
        void WriteModuleBaseReadout(ref Utf16ValueStringBuilder sb, ShowcaseModuleId module);
        void WriteViewportIdBadge(ref Utf16ValueStringBuilder sb, ShowcaseModuleId module);
        void WriteViewportSubBadge(ref Utf16ValueStringBuilder sb, ShowcaseModuleId module);
        void WriteFrameOpenState(ref Utf16ValueStringBuilder sb, FrameNavigationDto dto);
        void WriteCloseButtonHint(ref Utf16ValueStringBuilder sb, FrameNavigationDto dto);
        void WriteModalTelemetry(ref Utf16ValueStringBuilder sb, FrameNavigationDto dto);
        void WriteLayerTag(ref Utf16ValueStringBuilder sb, FrameNavigationDto dto);
        void WriteViewportCoords(ref Utf16ValueStringBuilder sb, FrameNavigationDto dto);
        void WriteFooterStatus(ref Utf16ValueStringBuilder sb, FrameNavigationDto dto);
        void WriteBenchmarkButtonText(ref Utf16ValueStringBuilder sb, bool isRecording);
        void WriteTelemetryFps(ref Utf16ValueStringBuilder sb, FrameTelemetryDto dto);
        void WriteTelemetryDrawCalls(ref Utf16ValueStringBuilder sb, FrameTelemetryDto dto);
        void WriteLifecycleLogEntry(ref Utf16ValueStringBuilder sb, LifecycleLogEntryDto entry);
        void WriteCounterValue(ref Utf16ValueStringBuilder sb, int value);
        void WriteRecycledNodeIndex(ref Utf16ValueStringBuilder sb, int index);
        void WriteRecycledNodePool(ref Utf16ValueStringBuilder sb, byte slot);
        void WriteRecycledNodeAddress(ref Utf16ValueStringBuilder sb, uint address);
        void WriteRecycledNodeStatus(ref Utf16ValueStringBuilder sb, RecycledNodeStatus status);
        void WriteRecycledNodeLatency(ref Utf16ValueStringBuilder sb, float latencyMs);
        void WriteModalDepth(ref Utf16ValueStringBuilder sb, int depth);
        void WriteModalTitle(ref Utf16ValueStringBuilder sb, ModalPresetId preset);
        void WriteModalBody(ref Utf16ValueStringBuilder sb, ModalPresetId preset);
        void WriteInventoryCode(ref Utf16ValueStringBuilder sb, InventoryItemCode code);
        void WriteInventoryName(ref Utf16ValueStringBuilder sb, InventoryItemCode code);
        void WriteInventoryCategory(ref Utf16ValueStringBuilder sb, InventoryItemCategory category);
        void WriteInventoryWeight(ref Utf16ValueStringBuilder sb, float weightKg);
        void WriteInventoryRarityBadge(ref Utf16ValueStringBuilder sb, InventoryRarity rarity);
        void WriteInventoryStat(ref Utf16ValueStringBuilder sb, InventoryItemDto item);
        void WriteInventoryDescription(ref Utf16ValueStringBuilder sb, InventoryItemCode code);
        void WriteLoadingSpinnerGlyph(ref Utf16ValueStringBuilder sb, int step);
        void WriteLoadingStatus(ref Utf16ValueStringBuilder sb, LoadingStateDto dto);
        void WriteHubToastText(ref Utf16ValueStringBuilder sb, FutureSlotId slot);
        void WriteTahoeStageHeader(ref Utf16ValueStringBuilder sb, TahoeDesktopStateDto state);
        void WriteTahoeFpsReadout(ref Utf16ValueStringBuilder sb, FrameTelemetryDto dto);
        void WriteTahoeDcReadout(ref Utf16ValueStringBuilder sb, FrameTelemetryDto dto);
        void WriteTahoeBenchmarkBadge(ref Utf16ValueStringBuilder sb, bool isRunning);
        void WriteTahoeLifecycleStage(ref Utf16ValueStringBuilder sb, LifecycleStageId stage);
        void WriteTahoeCounterValue(ref Utf16ValueStringBuilder sb, int value);
        void WriteTahoeInv100SlotCode(ref Utf16ValueStringBuilder sb, int index);
        void WriteTahoeInv100Title(ref Utf16ValueStringBuilder sb, int index);
        void WriteTahoeInv100Meta(ref Utf16ValueStringBuilder sb, int index);
        void WriteTahoeInv100Rarity(ref Utf16ValueStringBuilder sb, int index);
        void WriteTahoeListRowIndex(ref Utf16ValueStringBuilder sb, int index);
        void WriteTahoeListRowTitle(ref Utf16ValueStringBuilder sb, int index);
        void WriteTahoeListRowSlot(ref Utf16ValueStringBuilder sb, int index);
    }
}
