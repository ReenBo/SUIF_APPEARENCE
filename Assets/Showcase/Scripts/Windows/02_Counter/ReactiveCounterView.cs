using SUIF.Attributes;
using SUIF.Base;
using SUIF.Binding;
using SUIF.Controls;
using SUIF.R3.Extensions;
using SUIF.Showcase.Contracts;
using SUIF.Showcase.Formatting;
using UnityEngine.UIElements;

namespace SUIF.Showcase.Windows.Counter
{
    [UIView("ReactiveCounterView", SUIF.API.UILayer.Windows, UILoadingMode.Cached)]
    public sealed class ReactiveCounterView : BaseView<ReactiveCounterViewModel>
    {
        private const string ActiveClass = "is-active";
        private const string MarblePillClass = "fui-marble-pill";
        private const string MarbleArrowClass = "fui-marble-arrow";
        private const int MaxMarbleSlots = 7;
        private const int MarbleCharCapacity = 8;

        [UQuery("label-counter-value")] private Label _labelValue;
        [UQuery("btn-counter-dec")] private Button _btnDec;
        [UQuery("btn-counter-inc")] private Button _btnInc;
        [UQuery("btn-counter-boost")] private Button _btnBoost;
        [UQuery("btn-counter-reset")] private Button _btnReset;
        [UQuery("btn-window-close")] private Button _btnClose;
        [UQuery("counter-marbles-container")] private VisualElement _marblesContainer;

        private readonly FixedSlotLabel[] _marblePills = new FixedSlotLabel[MaxMarbleSlots];
        private readonly Label[] _marbleArrows = new Label[MaxMarbleSlots];
        private FixedElementPool<VisualElement> _marblePool;
        private FixedTextSlot _valueSlot;

        public ReactiveCounterView(VisualElement root) : base(root)
        {
        }

        protected override void Bind()
        {
            if (_btnDec != null) Binder.Bind(_btnDec).ToClick(() => ViewModel.UpdateCounter(-1));
            if (_btnInc != null) Binder.Bind(_btnInc).ToClick(() => ViewModel.UpdateCounter(1));
            if (_btnBoost != null) Binder.Bind(_btnBoost).ToClick(() => ViewModel.UpdateCounter(10));
            if (_btnReset != null) Binder.Bind(_btnReset).ToClick(ViewModel.ResetCounter);
            if (_btnClose != null) Binder.Bind(_btnClose).ToClick(ViewModel.Close);

            if (_labelValue != null)
            {
                _valueSlot = new FixedTextSlot(_labelValue, 8);
                AddDisposable(_valueSlot);
            }

            if (_marblesContainer != null)
            {
                _marblesContainer.Clear();
                _marblePool = new FixedElementPool<VisualElement>(
                    _marblesContainer,
                    MaxMarbleSlots,
                    CreateMarbleSlot);
                AddDisposable(_marblePool);
            }

            Binder.ToDto(ViewModel.State, this, static (dto, view) => view.ApplyState(dto));
        }

        private void ApplyState(CounterViewDto dto)
        {
            _valueSlot?.SetWritten(dto.CurrentValue, ShowcaseTextFormatter.WriteCounterValue);
            _marblePool?.Render(dto.HistoryCount, this, BindMarbleSlot);
        }

        private VisualElement CreateMarbleSlot(int index)
        {
            var row = new VisualElement();
            row.style.flexDirection = FlexDirection.Row;
            row.style.alignItems = Align.Center;

            var pill = new FixedSlotLabel
            {
                Capacity = MarbleCharCapacity
            };
            pill.AddToClassList(MarblePillClass);
            _marblePills[index] = pill;
            row.Add(pill);

            var arrow = new Label("->");
            arrow.AddToClassList(MarbleArrowClass);
            _marbleArrows[index] = arrow;
            row.Add(arrow);

            return row;
        }

        private static void BindMarbleSlot(VisualElement slot, int index, ReactiveCounterView view)
        {
            var history = view.ViewModel.CounterHistory;
            var pill = view._marblePills[index];
            var arrow = view._marbleArrows[index];

            pill.SetFixedFormat("{0}", history[index]);

            bool isLast = index == history.Count - 1;
            if (isLast)
            {
                pill.AddToClassList(ActiveClass);
            }
            else
            {
                pill.RemoveFromClassList(ActiveClass);
            }

            arrow.style.display = isLast ? DisplayStyle.None : StyleKeyword.Null;
        }
    }
}
