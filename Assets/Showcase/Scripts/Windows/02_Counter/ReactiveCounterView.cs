using SUIF.Attributes;
using SUIF.Binding;
using SUIF.Controls;
using SUIF.R3.Base;
using SUIF.R3.Extensions;
using SUIF.Showcase.Contracts;
using SUIF.Showcase.Formatting;
using UnityEngine.UIElements;
using VContainer;

namespace SUIF.Showcase.Windows.Counter
{
    [UIView("ReactiveCounterView", SUIF.API.UILayer.Windows, UILoadingMode.Cached)]
    public sealed class ReactiveCounterView : BaseReactiveDataView<ReactiveCounterViewModel, CounterViewDto>
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
        private IShowcaseTextFormatter _formatter;
        private FixedElementPool<VisualElement> _marblePool;
        private FixedTextSlot _valueSlot;

        public ReactiveCounterView(VisualElement root) : base(root)
        {
        }

        [Inject]
        public void Construct(IShowcaseTextFormatter formatter)
        {
            _formatter = formatter;
        }

        protected override void OnBind()
        {
            _formatter ??= new ShowcaseTextFormatter();

            if (_btnDec != null) Binder.Bind(_btnDec).ToClick(() => ViewModel.UpdateCounter(-1));
            if (_btnInc != null) Binder.Bind(_btnInc).ToClick(() => ViewModel.UpdateCounter(1));
            if (_btnBoost != null) Binder.Bind(_btnBoost).ToClick(() => ViewModel.UpdateCounter(10));
            if (_btnReset != null) Binder.Bind(_btnReset).ToClick(ViewModel.ResetCounter);
            if (_btnClose != null) Binder.Bind(_btnClose).ToClick(ViewModel.Close);

            _valueSlot = Binder.FixedSlot(_labelValue, 8);

            if (_marblesContainer != null)
            {
                _marblesContainer.Clear();
                _marblePool = new FixedElementPool<VisualElement>(
                    _marblesContainer,
                    MaxMarbleSlots,
                    CreateMarbleSlot);
                AddDisposable(_marblePool);
            }
        }

        protected override void RenderData(CounterViewDto dto)
        {
            _valueSlot?.SetWritten(dto.CurrentValue, _formatter.CounterValue);
            _marblePool?.Render(dto.HistoryCount, this, static (slot, index, view) => view.BindMarbleSlot(index));
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
            Binder.AddDisposable(pill);
            _marblePills[index] = pill;
            row.Add(pill);

            var arrow = new Label("->");
            arrow.AddToClassList(MarbleArrowClass);
            _marbleArrows[index] = arrow;
            row.Add(arrow);

            return row;
        }

        private void BindMarbleSlot(int index)
        {
            var history = ViewModel.CounterHistory;
            var pill = _marblePills[index];
            var arrow = _marbleArrows[index];

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
