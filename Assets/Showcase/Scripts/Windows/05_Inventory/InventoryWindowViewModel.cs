using System;
using Cysharp.Threading.Tasks;
using global::R3;
using SUIF.API;
using SUIF.Base;
using SUIF.Showcase.Contracts;
using SUIF.Showcase.Frame;
using SUIF.Showcase.Windows.Modals;

namespace SUIF.Showcase.Windows.Inventory
{
    public sealed class InventoryWindowViewModel : BaseViewModel
    {
        public static readonly InventoryItemDto[] Items =
        {
            new(InventoryItemCode.Cyb01, InventoryItemCategory.Cyberware, 4.2f, InventoryRarity.Iconic, InventoryStatKind.Dps, 940),
            new(InventoryItemCode.Opt02, InventoryItemCategory.Optics, 0.4f, InventoryRarity.SpecOps, InventoryStatKind.Zoom, 16),
            new(InventoryItemCode.Deck03, InventoryItemCategory.Cyberdeck, 1.1f, InventoryRarity.Legendary, InventoryStatKind.Ram, 32),
            new(InventoryItemCode.Mod04, InventoryItemCategory.Chipware, 0.1f, InventoryRarity.Rare, InventoryStatKind.TickBonusPercent, 45),
            new(InventoryItemCode.Stim05, InventoryItemCategory.Consumable, 0.2f, InventoryRarity.Common, InventoryStatKind.HealPercent, 80),
            new(InventoryItemCode.Arm06, InventoryItemCategory.Armor, 6.8f, InventoryRarity.Heavy, InventoryStatKind.Armor, 320),
            new(InventoryItemCode.Emp07, InventoryItemCategory.Ordnance, 0.5f, InventoryRarity.Tactical, InventoryStatKind.Emp, 450),
            new(InventoryItemCode.Key08, InventoryItemCategory.KeyItem, 0.0f, InventoryRarity.Encrypted, InventoryStatKind.ClearanceLevel, 5)
        };

        private readonly IUIFlow _uiFlow;
        private readonly ShowcaseFrameViewModel _frameViewModel;
        private readonly float _totalWeightKg;

        public ReactiveProperty<InventoryViewDto> State { get; }

        public InventoryWindowViewModel(IUIFlow uiFlow, ShowcaseFrameViewModel frameViewModel)
        {
            _uiFlow = uiFlow ?? throw new ArgumentNullException(nameof(uiFlow));
            _frameViewModel = frameViewModel ?? throw new ArgumentNullException(nameof(frameViewModel));

            float sum = 0f;
            for (int i = 0; i < Items.Length; i++)
            {
                sum += Items[i].WeightKg;
            }
            _totalWeightKg = sum;

            State = new ReactiveProperty<InventoryViewDto>(new InventoryViewDto(0, _totalWeightKg));
            AddDisposable(State);
        }

        public void SelectSlot(int index)
        {
            if (index >= 0 && index < Items.Length)
            {
                State.Value = new InventoryViewDto(index, _totalWeightKg);
            }
        }

        public void InspectInModal()
        {
            _frameViewModel.SetModalState(true);
            _uiFlow.OpenAsync<ModalDialogView>().Forget();
        }

        public void Close()
        {
            _frameViewModel.CloseBaseWindow();
        }

        public InventoryItemDto GetCurrentItem()
        {
            return Items[State.Value.SelectedIndex % Items.Length];
        }
    }
}
