using SUIF.Attributes;
using SUIF.Base;
using SUIF.Binding;
using SUIF.Controls;
using SUIF.R3.Extensions;
using SUIF.Showcase.Contracts;
using SUIF.Showcase.Formatting;
using UnityEngine.UIElements;

namespace SUIF.Showcase.Windows.Inventory
{
    [UIView("InventoryWindowView", SUIF.API.UILayer.Windows, UILoadingMode.Cached)]
    public sealed class InventoryWindowView : BaseView<InventoryWindowViewModel>
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

        public InventoryWindowView(VisualElement root) : base(root)
        {
        }

        protected override void Bind()
        {
            if (_btnModalInspect != null)
            {
                Binder.Bind(_btnModalInspect).ToClick(ViewModel.InspectInModal);
            }
            if (_btnClose != null)
            {
                Binder.Bind(_btnClose).ToClick(ViewModel.Close);
            }

            _inspectorNameSlot = RegisterFixedSlot(_inspectorName, 24);
            _inspectorRaritySlot = RegisterFixedSlot(_inspectorRarity, 16);
            _inspectorPowerSlot = RegisterFixedSlot(_inspectorPower, 16);
            _inspectorDescSlot = RegisterFixedSlot(_inspectorDesc, 96);

            if (_slotsContainer != null)
            {
                _slotsContainer.Clear();
                var items = InventoryWindowViewModel.Items;
                _slotPool = new FixedElementPool<Button>(_slotsContainer, items.Length, CreateInventorySlot);
                AddDisposable(_slotPool);
                _slotPool.Render(items.Length, items, BindInventorySlot);
            }

            Binder.ToDto(ViewModel.State, this, static (dto, view) => view.ApplyState(dto));
        }

        private FixedTextSlot RegisterFixedSlot(TextElement element, int capacity)
        {
            if (element == null)
            {
                return null;
            }

            var slot = new FixedTextSlot(element, capacity);
            AddDisposable(slot);
            return slot;
        }

        private Button CreateInventorySlot(int index)
        {
            var slot = new Button(() => ViewModel.SelectSlot(index));
            slot.AddToClassList("fui-inventory-slot");

            var header = new VisualElement();
            header.AddToClassList("fui-inventory-slot__header");

            var codeLabel = new FixedSlotLabel { Capacity = 10 };
            codeLabel.AddToClassList("fui-inventory-slot__code");
            var weightLabel = new FixedSlotLabel { Capacity = 10 };
            weightLabel.AddToClassList("fui-inventory-slot__weight");
            header.Add(codeLabel);
            header.Add(weightLabel);

            var nameLabel = new FixedSlotLabel { Capacity = 24 };
            nameLabel.AddToClassList("fui-inventory-slot__name");

            var typeLabel = new FixedSlotLabel { Capacity = 14 };
            typeLabel.AddToClassList("fui-inventory-slot__type");

            slot.Add(header);
            slot.Add(nameLabel);
            slot.Add(typeLabel);

            return slot;
        }

        private static void BindInventorySlot(Button slot, int index, InventoryItemDto[] items)
        {
            var item = items[index];
            var header = slot[0];
            ((FixedSlotLabel)header[0]).SetFixedWritten(item.Code, ShowcaseTextFormatter.WriteInventoryCode);
            ((FixedSlotLabel)header[1]).SetFixedWritten(item.WeightKg, ShowcaseTextFormatter.WriteInventoryWeight);
            ((FixedSlotLabel)slot[1]).SetFixedWritten(item.Code, ShowcaseTextFormatter.WriteInventoryName);
            ((FixedSlotLabel)slot[2]).SetFixedWritten(item.Category, ShowcaseTextFormatter.WriteInventoryCategory);
        }

        private void ApplyState(InventoryViewDto dto)
        {
            var item = ViewModel.GetCurrentItem();
            _inspectorNameSlot?.SetWritten(item.Code, ShowcaseTextFormatter.WriteInventoryName);
            _inspectorRaritySlot?.SetWritten(item.Rarity, ShowcaseTextFormatter.WriteInventoryRarityBadge);
            _inspectorPowerSlot?.SetWritten(item, ShowcaseTextFormatter.WriteInventoryStat);
            _inspectorDescSlot?.SetWritten(item.Code, ShowcaseTextFormatter.WriteInventoryDescription);

            if (_slotPool != null)
            {
                for (var i = 0; i < _slotPool.Capacity; i++)
                {
                    var child = _slotPool.GetSlot(i);
                    if (i == dto.SelectedIndex) child.AddToClassList(SelectedClass);
                    else child.RemoveFromClassList(SelectedClass);
                }
            }
        }
    }
}
