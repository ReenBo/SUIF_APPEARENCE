using System;
using global::R3;
using ObservableCollections;
using SUIF.Base;
using SUIF.R3.Base;
using SUIF.Showcase.Contracts;
using SUIF.Showcase.Formatting;
using SUIF.Showcase.Frame;
using SUIF.Showcase.Services;

namespace SUIF.Showcase.Windows.Counter
{
    public sealed class ReactiveCounterViewModel : BaseViewModel, IReactiveDataViewModel<CounterViewDto>
    {
        private readonly ShowcaseFrameViewModel _frameViewModel;
        private readonly ICounterService _counterService;

        public IShowcaseTextFormatter Formatter { get; }
        public ReadOnlyReactiveProperty<CounterViewDto> State => _counterService.State;
        public Observable<CounterViewDto> Data => _counterService.State;
        public ObservableList<int> CounterHistory => _counterService.History;

        public ReactiveCounterViewModel(
            ShowcaseFrameViewModel frameViewModel,
            ICounterService counterService,
            IShowcaseTextFormatter formatter)
        {
            _frameViewModel = frameViewModel ?? throw new ArgumentNullException(nameof(frameViewModel));
            _counterService = counterService ?? throw new ArgumentNullException(nameof(counterService));
            Formatter = formatter ?? throw new ArgumentNullException(nameof(formatter));
        }

        public void UpdateCounter(int delta)
        {
            _counterService.UpdateCounter(delta);
        }

        public void ResetCounter()
        {
            _counterService.ResetCounter();
        }

        public void Close()
        {
            _frameViewModel.CloseBaseWindow();
        }
    }
}
