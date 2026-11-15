using System;
using Cysharp.Threading.Tasks;
using global::R3;
using ObservableCollections;
using SUIF.API;
using SUIF.Base;
using SUIF.Showcase.Contracts;
using SUIF.Showcase.Frame;
using SUIF.Showcase.Windows.Modals;

namespace SUIF.Showcase.Windows.Lifecycle
{
    public sealed class LifecycleWindowViewModel : BaseViewModel
    {
        private const int MaxLogs = 10;

        private readonly IUIFlow _uiFlow;
        private readonly ShowcaseFrameViewModel _frameViewModel;

        public ReactiveProperty<LifecycleViewDto> State { get; }
        public ObservableList<LifecycleLogEntryDto> LifecycleLogs { get; } = new();

        public LifecycleWindowViewModel(IUIFlow uiFlow, ShowcaseFrameViewModel frameViewModel)
        {
            _uiFlow = uiFlow ?? throw new ArgumentNullException(nameof(uiFlow));
            _frameViewModel = frameViewModel ?? throw new ArgumentNullException(nameof(frameViewModel));

            LifecycleLogs.Add(new LifecycleLogEntryDto(0, 0, 2, 0, LifecycleStageId.Initialize, true));
            LifecycleLogs.Add(new LifecycleLogEntryDto(0, 0, 5, 0, LifecycleStageId.Open, true));
            LifecycleLogs.Add(new LifecycleLogEntryDto(0, 0, 9, 0, LifecycleStageId.Show, true));

            State = new ReactiveProperty<LifecycleViewDto>(new LifecycleViewDto(LifecycleStageId.Initialize, LifecycleLogs.Count));
            AddDisposable(State);
        }

        public void TriggerStage(LifecycleStageId stage)
        {
            var now = DateTime.UtcNow;
            LifecycleLogs.Insert(0, new LifecycleLogEntryDto(
                now.Hour,
                now.Minute,
                now.Second,
                now.Millisecond / 10,
                stage));

            if (LifecycleLogs.Count > MaxLogs)
            {
                LifecycleLogs.RemoveAt(LifecycleLogs.Count - 1);
            }

            State.Value = new LifecycleViewDto(stage, LifecycleLogs.Count);
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
