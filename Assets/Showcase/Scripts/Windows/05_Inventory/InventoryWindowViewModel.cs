using System;
using Cysharp.Threading.Tasks;
using global::R3;
using SUIF.API;
using SUIF.Base;
using SUIF.Showcase.Frame;
using SUIF.Showcase.Windows.Modals;

namespace SUIF.Showcase.Windows.Inventory
{
    public readonly struct InventoryItemData
    {
        public readonly string Code;
        public readonly string Name;
        public readonly string Type;
        public readonly string Weight;
        public readonly string Rarity;
        public readonly string Power;
        public readonly string Description;

        public InventoryItemData(string code, string name, string type, string weight, string rarity, string power, string description)
        {
            Code = code;
            Name = name;
            Type = type;
            Weight = weight;
            Rarity = rarity;
            Power = power;
            Description = description;
        }
    }

    public sealed class InventoryWindowViewModel : BaseViewModel
    {
        public static readonly InventoryItemData[] Items =
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

        private readonly IUIFlow _uiFlow;
        private readonly ShowcaseFrameViewModel _frameViewModel;

        public ReactiveProperty<int> SelectedIndex { get; } = new(0);

        public InventoryWindowViewModel(IUIFlow uiFlow, ShowcaseFrameViewModel frameViewModel)
        {
            _uiFlow = uiFlow ?? throw new ArgumentNullException(nameof(uiFlow));
            _frameViewModel = frameViewModel ?? throw new ArgumentNullException(nameof(frameViewModel));

            AddDisposable(SelectedIndex);
        }

        public void SelectSlot(int index)
        {
            if (index >= 0 && index < Items.Length)
            {
                SelectedIndex.Value = index;
            }
        }

        public void InspectInModal()
        {
            _frameViewModel.SetModalState(true);
            _uiFlow.OpenAsync<ModalDialogView>().Forget();
        }

        public void Close()
        {
            _frameViewModel.CloseActiveWindow();
        }

        public InventoryItemData GetCurrentItem()
        {
            return Items[SelectedIndex.Value % Items.Length];
        }
    }
}

