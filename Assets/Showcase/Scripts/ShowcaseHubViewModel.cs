using System;
using System.Collections.Generic;
using Cysharp.Text;
using global::R3;
using ObservableCollections;
using SUIF.Base;

namespace SUIF.Showcase
{
    public sealed class ShowcaseHubViewModel : BaseViewModel, IDisposable
    {
        public static readonly InventoryItem[] InventoryItems =
        {
            new("CYB_01", "MANTIS_BLADE_MK4", "CYBERWARE", "4.2 KG", "ICONIC", "940 DPS", "High-frequency thermal monowire edge with neural reflex link."),
            new("OPT_02", "KIROSHI_ORACLE_V3", "OPTICS", "0.4 KG", "SPEC_OPS", "16X ZOOM", "Threat-highlighting FUI target reticle overlay with wall-penetrating sonar."),
            new("DECK_03", "NETWATCH_RIPPLER", "CYBERDECK", "1.1 KG", "LEGENDARY", "32 RAM", "Spreads quickhack daemons across 4 adjacent subnet nodes."),
            new("MOD_04", "R3_KERNEL_BOOSTER", "CHIPWARE", "0.1 KG", "RARE", "+45% TICK", "Accelerates MVVM reactive property bindings with zero GC spikes."),
            new("STIM_05", "MAXDOC_INHALER", "CONSUMABLE", "0.2 KG", "COMMON", "+80% HP", "Instant nanite tissue reconstruction compound."),
            new("ARM_06", "SUBDERMAL_TITANIUM", "ARMOR", "6.8 KG", "HEAVY", "+320 ARM", "Ballistic weave plated directly beneath epidermis."),
            new("EMP_07", "VOLT_GRENADE_X", "ORDNANCE", "0.5 KG", "TACTICAL", "450 EMP", "Disables robotic actuators and HUD overlays within 8m radius."),
            new("KEY_08", "ARASAKA_AUTH_SHARD", "KEY_ITEM", "0.0 KG", "ENCRYPTED", "LVL_5", "Biometric clearance token for orbital relay terminal.")
        };

        public static readonly ModalPreset[] ModalPresets =
        {
            new("DANGER // POPUP_INTERCEPT", "This modal layer is non-destructively mounted over the active base window. Underlying reactive streams and lifecycle states remain preserved in memory."),
            new("CONFIRM // MEMORY_PURGE_0x04", "Verify zero-allocation pool flush before unmounting active SUIF container. Modal stack isolates user input until resolved."),
            new("SYSTEM_ALERT // OVERLAY_PRIORITY", "High-priority FUI notification injected at Z-Layer 999. Base window continues rendering beneath the scanline backdrop.")
        };

        public static readonly RecycledNodeItem[] RecycledNodes =
        {
            new(1, "NODE_01", "0x00F410", "HOT_SWAP", "0.12ms"),
            new(2, "NODE_02", "0x00F411", "RECYCLED", "0.15ms"),
            new(3, "NODE_03", "0x00F412", "RECYCLED", "0.18ms"),
            new(4, "NODE_04", "0x00F413", "RECYCLED", "0.21ms"),
            new(5, "NODE_05", "0x00F414", "HOT_SWAP", "0.12ms"),
            new(6, "NODE_06", "0x00F415", "RECYCLED", "0.15ms"),
            new(7, "NODE_01", "0x00F416", "RECYCLED", "0.18ms"),
            new(8, "NODE_02", "0x00F417", "RECYCLED", "0.21ms"),
            new(9, "NODE_03", "0x00F418", "HOT_SWAP", "0.12ms"),
            new(10, "NODE_04", "0x00F419", "RECYCLED", "0.15ms"),
            new(11, "NODE_05", "0x00F41A", "RECYCLED", "0.18ms"),
            new(12, "NODE_06", "0x00F41B", "RECYCLED", "0.21ms"),
            new(13, "NODE_01", "0x00F41C", "HOT_SWAP", "0.12ms"),
            new(14, "NODE_02", "0x00F41D", "RECYCLED", "0.15ms"),
            new(15, "NODE_03", "0x00F41E", "RECYCLED", "0.18ms"),
            new(16, "NODE_04", "0x00F41F", "RECYCLED", "0.21ms"),
            new(17, "NODE_05", "0x00F420", "HOT_SWAP", "0.12ms"),
            new(18, "NODE_06", "0x00F421", "RECYCLED", "0.15ms")
        };

        private readonly CompositeDisposable _disposables = new();

        public ReactiveProperty<ShowcaseViewType> ActiveBaseView { get; } = new(ShowcaseViewType.Lifecycle);
        public ReactiveProperty<bool> IsModalOpen { get; } = new(true);
        public ReactiveProperty<int> ModalDepth { get; } = new(1);
        public ReactiveProperty<int> ModalPresetIndex { get; } = new(0);

        public ReactiveProperty<string> LifecycleStage { get; } = new("OnShow");
        public ObservableList<string> LifecycleLogs { get; } = new();

        public ReactiveProperty<int> CounterValue { get; } = new(42);
        public ObservableList<int> CounterHistory { get; } = new();

        public ReactiveProperty<int> SelectedInventoryIndex { get; } = new(0);
        public ReactiveProperty<string> CurrentTheme { get; } = new("mono");
        public ReactiveProperty<string> FooterStatusLog { get; } = new("STATE_SYNC // READY");

        public ShowcaseHubViewModel()
        {
            _disposables.Add(ActiveBaseView);
            _disposables.Add(IsModalOpen);
            _disposables.Add(ModalDepth);
            _disposables.Add(ModalPresetIndex);
            _disposables.Add(LifecycleStage);
            _disposables.Add(CounterValue);
            _disposables.Add(SelectedInventoryIndex);
            _disposables.Add(CurrentTheme);
            _disposables.Add(FooterStatusLog);

            LifecycleLogs.Add("[00:00.02] OnInitialize() -> Memory blocks pre-warmed");
            LifecycleLogs.Add("[00:00.05] OnOpen()       -> View hierarchy attached to SUIF Root");
            LifecycleLogs.Add("[00:00.09] OnShow()       -> Transition animation completed [ACTIVE]");

            CounterHistory.Add(38);
            CounterHistory.Add(39);
            CounterHistory.Add(40);
            CounterHistory.Add(41);
            CounterHistory.Add(42);
        }

        public void SelectBaseView(ShowcaseViewType viewType)
        {
            ActiveBaseView.Value = viewType;
            if (IsModalOpen.Value)
            {
                IsModalOpen.Value = false;
            }
            FooterStatusLog.Value = ZString.Concat("OPENED BASE_VIEW // ", viewType);
        }

        public void ToggleModalOverlay()
        {
            if (!IsModalOpen.Value)
            {
                IsModalOpen.Value = true;
                ModalDepth.Value = 1;
                FooterStatusLog.Value = ZString.Concat("MOUNTED OVERLAY // MODALS_POPUPS ON TOP OF [", ActiveBaseView.Value, "]");
            }
            else
            {
                IsModalOpen.Value = false;
                FooterStatusLog.Value = "UNMOUNTED OVERLAY // MODALS_POPUPS CLOSED";
            }
        }

        public void CloseModalOverlay()
        {
            IsModalOpen.Value = false;
            FooterStatusLog.Value = "MODAL_OVERLAY DISMISSED // BASE WINDOW RESTORED";
        }

        public void HandleMainCloseButton()
        {
            if (IsModalOpen.Value)
            {
                CloseModalOverlay();
                return;
            }

            if (ActiveBaseView.Value != ShowcaseViewType.None)
            {
                var previous = ActiveBaseView.Value;
                ActiveBaseView.Value = ShowcaseViewType.None;
                FooterStatusLog.Value = ZString.Concat("CLOSED BASE_VIEW // ", previous, " -> [STANDBY]");
            }
        }

        public void TriggerLifecycleStage(string stageName)
        {
            LifecycleStage.Value = stageName;
            var timeStamp = DateTime.UtcNow.ToString("HH:mm:ss.ff");
            LifecycleLogs.Insert(0, ZString.Concat("[", timeStamp, "] ", stageName, "() -> State transition executed"));
            if (LifecycleLogs.Count > 10)
            {
                LifecycleLogs.RemoveAt(LifecycleLogs.Count - 1);
            }
            FooterStatusLog.Value = ZString.Concat("LIFECYCLE_HOOK // ", stageName, "() EXECUTED");
        }

        public void UpdateCounter(int delta)
        {
            CounterValue.Value += delta;
            CounterHistory.Add(CounterValue.Value);
            if (CounterHistory.Count > 7)
            {
                CounterHistory.RemoveAt(0);
            }
            FooterStatusLog.Value = ZString.Concat("R3_COUNTER // VALUE UPDATED TO ", CounterValue.Value);
        }

        public void ResetCounter()
        {
            CounterValue.Value = 0;
            CounterHistory.Clear();
            CounterHistory.Add(0);
            FooterStatusLog.Value = "R3_COUNTER // STREAM RESET TO 0";
        }

        public void SelectInventorySlot(int index)
        {
            if (index < 0 || index >= InventoryItems.Length)
            {
                return;
            }
            SelectedInventoryIndex.Value = index;
            FooterStatusLog.Value = ZString.Concat("INVENTORY // SELECTED [", InventoryItems[index].Name, "]");
        }

        public void ApplyTheme(string themeId)
        {
            CurrentTheme.Value = themeId;
            FooterStatusLog.Value = ZString.Concat("GLOBAL_THEME // SWITCHED TO [", themeId.ToUpperInvariant(), "]");
        }

        public void SpawnSubPopup()
        {
            ModalDepth.Value++;
            ModalPresetIndex.Value = (ModalPresetIndex.Value + 1) % ModalPresets.Length;
            FooterStatusLog.Value = ZString.Concat("MODAL_STACK // PUSHED NESTED POPUP [DEPTH: ", ModalDepth.Value, "]");
        }

        public void CycleModalPreset()
        {
            ModalPresetIndex.Value = (ModalPresetIndex.Value + 1) % ModalPresets.Length;
            FooterStatusLog.Value = "MODAL_OVERLAY // SWITCHED PAYLOAD TEMPLATE";
        }

        public ModalPreset GetCurrentModalPreset()
        {
            return ModalPresets[ModalPresetIndex.Value % ModalPresets.Length];
        }

        public InventoryItem GetCurrentInventoryItem()
        {
            return InventoryItems[SelectedInventoryIndex.Value % InventoryItems.Length];
        }

        public override void Dispose()
        {
            _disposables.Dispose();
            base.Dispose();
        }
    }
}
