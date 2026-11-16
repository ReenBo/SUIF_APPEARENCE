using System;
using Cysharp.Threading.Tasks;
using global::R3;
using ObservableCollections;
using SUIF.API;
using SUIF.Base;
using SUIF.R3.Base;
using SUIF.Showcase.Contracts;
using SUIF.Showcase.Formatting;
using SUIF.Showcase.Frame;
using SUIF.Showcase.Services;
using SUIF.Showcase.Windows.Modals;

namespace SUIF.Showcase.Windows.Lifecycle
{
    public sealed class LifecycleWindowViewModel : BaseViewModel, IReactiveDataViewModel<LifecycleViewDto>
    {
        private readonly IUIFlow _uiFlow;
        private readonly ShowcaseFrameViewModel _frameViewModel;
        private readonly ILifecycleMonitorService _monitorService;

        public IShowcaseTextFormatter Formatter { get; }
        public ReadOnlyReactiveProperty<LifecycleViewDto> State => _monitorService.State;
        public Observable<LifecycleViewDto> Data => _monitorService.State;
        public ObservableList<LifecycleLogEntryDto> LifecycleLogs => _monitorService.Logs;

        public LifecycleWindowViewModel(
            IUIFlow uiFlow,
            ShowcaseFrameViewModel frameViewModel,
            ILifecycleMonitorService monitorService,
            IShowcaseTextFormatter formatter)
        {
            _uiFlow = uiFlow ?? throw new ArgumentNullException(nameof(uiFlow));
            _frameViewModel = frameViewModel ?? throw new ArgumentNullException(nameof(frameViewModel));
            _monitorService = monitorService ?? throw new ArgumentNullException(nameof(monitorService));
            Formatter = formatter ?? throw new ArgumentNullException(nameof(formatter));
        }

        public void TriggerStage(LifecycleStageId stage)
        {
            _monitorService.TriggerStage(stage);
        }

        public void OpenModal()
        {
            _frameViewModel.SetModalState(true);
            _uiFlow.OpenAsync<ModalDialogView>().Forget();
        }

        public void Close()
        {
            _frameViewModel.CloseBaseWindow();
        }
    }
}
