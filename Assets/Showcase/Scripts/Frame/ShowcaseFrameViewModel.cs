using System;
using Cysharp.Threading.Tasks;
using global::R3;
using SUIF.API;
using SUIF.Base;
using SUIF.Showcase.Common;
using SUIF.Showcase.Windows.Counter;
using SUIF.Showcase.Windows.Inventory;
using SUIF.Showcase.Windows.Lifecycle;
using SUIF.Showcase.Windows.Modals;
using SUIF.Showcase.Windows.RecycledList;
using SUIF.Showcase.Windows.ThemeMatrix;

namespace SUIF.Showcase.Frame
{
    public sealed class ShowcaseFrameViewModel : BaseViewModel
    {
        private readonly IUIFlow _uiFlow;
        private readonly ShowcaseThemeService _themeService;

        public ReactiveProperty<int> ActiveNavIndex { get; } = new(0);
        public ReactiveProperty<string> ActiveTitle { get; } = new("[ NO_ACTIVE_VIEW // IDLE ]");
        public ReactiveProperty<bool> IsModalOpen { get; } = new(false);
        public ReactiveProperty<string> FooterStatus { get; } = new("STATE_SYNC // READY");

        public ReadOnlyReactiveProperty<string> CurrentTheme => _themeService.CurrentTheme;

        public ShowcaseFrameViewModel(IUIFlow uiFlow, ShowcaseThemeService themeService)
        {
            _uiFlow = uiFlow ?? throw new ArgumentNullException(nameof(uiFlow));
            _themeService = themeService ?? throw new ArgumentNullException(nameof(themeService));

            AddDisposable(ActiveNavIndex);
            AddDisposable(ActiveTitle);
            AddDisposable(IsModalOpen);
            AddDisposable(FooterStatus);
        }

        public void OpenLifecycleWindow()
        {
            ActiveNavIndex.Value = 1;
            ActiveTitle.Value = "Basic Window Lifecycle";
            FooterStatus.Value = "OPENED // LIFECYCLE_WINDOW";
            _uiFlow.OpenAsync<LifecycleWindowView>().Forget();
        }

        public void OpenCounterWindow()
        {
            ActiveNavIndex.Value = 2;
            ActiveTitle.Value = "MVVM & R3 Reactive Counter";
            FooterStatus.Value = "OPENED // COUNTER_WINDOW";
            _uiFlow.OpenAsync<ReactiveCounterView>().Forget();
        }

        public void OpenRecycledListWindow()
        {
            ActiveNavIndex.Value = 3;
            ActiveTitle.Value = "Virtualized Recycled List";
            FooterStatus.Value = "OPENED // RECYCLED_LIST_WINDOW";
            _uiFlow.OpenAsync<RecycledListWindowView>().Forget();
        }

        public void ToggleModal()
        {
            if (IsModalOpen.Value)
            {
                IsModalOpen.Value = false;
                FooterStatus.Value = "UNMOUNTED // MODAL_OVERLAY";
                _uiFlow.Close<ModalDialogView>();
            }
            else
            {
                IsModalOpen.Value = true;
                FooterStatus.Value = "MOUNTED // MODAL_OVERLAY";
                _uiFlow.OpenAsync<ModalDialogView>().Forget();
            }
        }

        public void OpenInventoryWindow()
        {
            ActiveNavIndex.Value = 5;
            ActiveTitle.Value = "INVENTORY";
            FooterStatus.Value = "OPENED // INVENTORY_WINDOW";
            _uiFlow.OpenAsync<InventoryWindowView>().Forget();
        }

        public void OpenThemeMatrixWindow()
        {
            ActiveNavIndex.Value = 6;
            ActiveTitle.Value = "Dynamic Theme Switcher";
            FooterStatus.Value = "OPENED // THEME_MATRIX_WINDOW";
            _uiFlow.OpenAsync<ThemeMatrixWindowView>().Forget();
        }

        public void CloseActiveWindow()
        {
            if (IsModalOpen.Value)
            {
                IsModalOpen.Value = false;
                FooterStatus.Value = "CLOSED // MODAL_OVERLAY";
                _uiFlow.Close<ModalDialogView>();
                return;
            }

            var currentIndex = ActiveNavIndex.Value;
            ActiveNavIndex.Value = 0;
            ActiveTitle.Value = "[ NO_ACTIVE_VIEW // IDLE ]";
            FooterStatus.Value = "CLOSED // BASE_WINDOW";

            switch (currentIndex)
            {
                case 1: _uiFlow.Close<LifecycleWindowView>(); break;
                case 2: _uiFlow.Close<ReactiveCounterView>(); break;
                case 3: _uiFlow.Close<RecycledListWindowView>(); break;
                case 5: _uiFlow.Close<InventoryWindowView>(); break;
                case 6: _uiFlow.Close<ThemeMatrixWindowView>(); break;
            }
        }

        public void SetModalState(bool isOpen)
        {
            IsModalOpen.Value = isOpen;
        }
    }
}

