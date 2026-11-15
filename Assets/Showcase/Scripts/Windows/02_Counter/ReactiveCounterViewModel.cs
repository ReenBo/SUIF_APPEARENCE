using System;
using global::R3;
using ObservableCollections;
using SUIF.Base;
using SUIF.Showcase.Contracts;
using SUIF.Showcase.Frame;

namespace SUIF.Showcase.Windows.Counter
{
    public sealed class ReactiveCounterViewModel : BaseViewModel
    {
        private const int MaxHistory = 7;

        private readonly ShowcaseFrameViewModel _frameViewModel;

        public ReactiveProperty<CounterViewDto> State { get; }
        public ObservableList<int> CounterHistory { get; } = new();

        public ReactiveCounterViewModel(ShowcaseFrameViewModel frameViewModel)
        {
            _frameViewModel = frameViewModel ?? throw new ArgumentNullException(nameof(frameViewModel));

            CounterHistory.Add(38);
            CounterHistory.Add(39);
            CounterHistory.Add(40);
            CounterHistory.Add(41);
            CounterHistory.Add(42);

            State = new ReactiveProperty<CounterViewDto>(new CounterViewDto(42, CounterHistory.Count));
            AddDisposable(State);
        }

        public void UpdateCounter(int delta)
        {
            int nextValue = State.Value.CurrentValue + delta;
            CounterHistory.Add(nextValue);
            if (CounterHistory.Count > MaxHistory)
            {
                CounterHistory.RemoveAt(0);
            }

            State.Value = new CounterViewDto(nextValue, CounterHistory.Count);
        }

        public void ResetCounter()
        {
            CounterHistory.Clear();
            CounterHistory.Add(0);
            State.Value = new CounterViewDto(0, CounterHistory.Count);
        }

        public void Close()
        {
            _frameViewModel.CloseBaseWindow();
        }
    }
}
