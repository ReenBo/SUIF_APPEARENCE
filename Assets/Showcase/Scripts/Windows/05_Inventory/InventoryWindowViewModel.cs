using System;
using Cysharp.Threading.Tasks;
using global::R3;
using SUIF.API;
using SUIF.Base;
using SUIF.R3.Base;
using SUIF.Showcase.Contracts;
using SUIF.Showcase.Formatting;
using SUIF.Showcase.Frame;
using SUIF.Showcase.Services;
using SUIF.Showcase.Windows.Modals;

namespace SUIF.Showcase.Windows.Inventory
{
    public sealed class InventoryWindowViewModel : BaseViewModel, IReactiveDataViewModel<InventoryViewDto>
    {
        private readonly IUIFlow _uiFlow;
        private readonly ShowcaseFrameViewModel _frameViewModel;
        private readonly IInventoryService _inventoryService;

        public IShowcaseTextFormatter Formatter { get; }
        public InventoryItemDto[] Items => _inventoryService.Items;
        public ReadOnlyReactiveProperty<InventoryViewDto> State => _inventoryService.State;
        public Observable<InventoryViewDto> Data => _inventoryService.State;

        public InventoryWindowViewModel(
            IUIFlow uiFlow,
            ShowcaseFrameViewModel frameViewModel,
            IInventoryService inventoryService,
            IShowcaseTextFormatter formatter)
        {
            _uiFlow = uiFlow ?? throw new ArgumentNullException(nameof(uiFlow));
            _frameViewModel = frameViewModel ?? throw new ArgumentNullException(nameof(frameViewModel));
            _inventoryService = inventoryService ?? throw new ArgumentNullException(nameof(inventoryService));
            Formatter = formatter ?? throw new ArgumentNullException(nameof(formatter));
        }

        public void SelectSlot(int index)
        {
            _inventoryService.SelectSlot(index);
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
            return _inventoryService.GetCurrentItem();
        }
    }
}
