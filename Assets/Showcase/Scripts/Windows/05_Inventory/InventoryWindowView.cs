using SUIF.Attributes;
using SUIF.Binding;
using SUIF.Controls;
using SUIF.R3.Base;
using SUIF.R3.Extensions;
using SUIF.Showcase.Contracts;
using UnityEngine.UIElements;

namespace SUIF.Showcase.Windows.Inventory
{
    [UIView("InventoryWindowView", SUIF.API.UILayer.Windows, UILoadingMode.Cached)]
    public sealed class InventoryWindowView : BaseReactiveDataView<InventoryWindowViewModel, InventoryViewDto>
    {
        private const string SelectedClass = "is-selected";

        [UQuery("inventory-slots-container")] private VisualElement _slotsContainer;
        [UQuery("inventory-inspector-name")] private Label _inspectorName;
        [UQuery("inventory-inspector-rarity")] private Label _inspectorRarity;
        [UQuery("inventory-inspector-power")] private Label _inspectorPower;
        [UQuery("inventory-inspector-desc")] private Label _inspectorDesc;
        [UQuery("btn-inventory-modal-inspect")] private Button _btnModalInspect;
        [UQuery("btn-window-close")] private Button _btnClose;

        private FixedTextSlot _inspectorNameSlot;
        private FixedTextSlot _inspectorRaritySlot;
        private FixedTextSlot _inspectorPowerSlot;
        private FixedTextSlot _inspectorDescSlot;
        private FixedElementPool<Button> _slotPool;
        private int _lastSelectedIndex = -1;

        public InventoryWindowView(VisualElement root) : base(root)
        {
        }

        protected override void OnBind()
        {
            if (_btnModalInspect != null) Binder.Bind(_btnModalInspect).ToClick(ViewModel.InspectInModal);
            if (_btnClose != null) Binder.Bind(_btnClose).ToClick(ViewModel.Close);

            _inspectorNameSlot = Binder.FixedSlot(_inspectorName, 24);
            _inspectorRaritySlot = Binder.FixedSlot(_inspectorRarity, 16);
            _inspectorPowerSlot = Binder.FixedSlot(_inspectorPower, 16);
            _inspectorDescSlot = Binder.FixedSlot(_inspectorDesc, 96);

            if (_slotsContainer != null)
            {
                _slotsContainer.Clear();
                var items = ViewModel.Items;
                _slotPool = new FixedElementPool<Button>(_slotsContainer, items.Length, CreateInventorySlot);
                AddDisposable(_slotPool);
                _slotPool.Render(items.Length, this, static (slot, index, view) => view.BindInventorySlot(slot, index));
            }
        }

        protected override void RenderData(InventoryViewDto dto)
        {
            var formatter = ViewModel.Formatter;
            var item = ViewModel.GetCurrentItem();
            _inspectorNameSlot?.SetWritten(item.Code, formatter.InventoryName);
            _inspectorRaritySlot?.SetWritten(item.Rarity, formatter.InventoryRarityBadge);
            _inspectorPowerSlot?.SetWritten(item, formatter.InventoryStat);
            _inspectorDescSlot?.SetWritten(item.Code, formatter.InventoryDescription);

            if (_slotPool != null && _lastSelectedIndex != dto.SelectedIndex)
            {
                if (_lastSelectedIndex >= 0 && _lastSelectedIndex < _slotPool.Capacity)
                {
                    _slotPool.GetSlot(_lastSelectedIndex).RemoveFromClassList(SelectedClass);
                }

                if (dto.SelectedIndex >= 0 && dto.SelectedIndex < _slotPool.Capacity)
                {
                    _slotPool.GetSlot(dto.SelectedIndex).AddToClassList(SelectedClass);
                }

                _lastSelectedIndex = dto.SelectedIndex;
            }
        }

        private Button CreateInventorySlot(int index)
        {
            var slot = new Button(() => ViewModel.SelectSlot(index));
            slot.AddToClassList("fui-inventory-slot");

            var header = new VisualElement();
            header.AddToClassList("fui-inventory-slot__header");

            var codeLabel = CreateSlotLabel(10, "fui-inventory-slot__code");
            var weightLabel = CreateSlotLabel(10, "fui-inventory-slot__weight");
            header.Add(codeLabel);
            header.Add(weightLabel);

            var nameLabel = CreateSlotLabel(24, "fui-inventory-slot__name");
            var typeLabel = CreateSlotLabel(14, "fui-inventory-slot__type");

            slot.Add(header);
            slot.Add(nameLabel);
            slot.Add(typeLabel);

            return slot;
        }

        private FixedSlotLabel CreateSlotLabel(int capacity, string className)
        {
            var label = new FixedSlotLabel { Capacity = capacity };
            label.AddToClassList(className);
            Binder.AddDisposable(label);
            return label;
        }

        private void BindInventorySlot(Button slot, int index)
        {
            var formatter = ViewModel.Formatter;
            var item = ViewModel.Items[index];
            var header = slot[0];
            ((FixedSlotLabel)header[0]).SetFixedWritten(item.Code, formatter.InventoryCode);
            ((FixedSlotLabel)header[1]).SetFixedWritten(item.WeightKg, formatter.InventoryWeight);
            ((FixedSlotLabel)slot[1]).SetFixedWritten(item.Code, formatter.InventoryName);
            ((FixedSlotLabel)slot[2]).SetFixedWritten(item.Category, formatter.InventoryCategory);
        }
    }
}
