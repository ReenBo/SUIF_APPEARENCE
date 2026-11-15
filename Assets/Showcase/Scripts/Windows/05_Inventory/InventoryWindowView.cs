using Cysharp.Text;
using global::R3;
using SUIF.Attributes;
using SUIF.Base;
using SUIF.Binding;
using SUIF.R3.Extensions;
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

            AddDisposable(ViewModel.SelectedIndex.Subscribe(OnSelectionChanged));
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

            var codeLabel = new Label();
            codeLabel.AddToClassList("fui-inventory-slot__code");
            var weightLabel = new Label();
            weightLabel.AddToClassList("fui-inventory-slot__weight");
            header.Add(codeLabel);
            header.Add(weightLabel);

            var nameLabel = new Label();
            nameLabel.AddToClassList("fui-inventory-slot__name");

            var typeLabel = new Label();
            typeLabel.AddToClassList("fui-inventory-slot__type");

            slot.Add(header);
            slot.Add(nameLabel);
            slot.Add(typeLabel);

            return slot;
        }

        private static void BindInventorySlot(Button slot, int index, InventoryItemData[] items)
        {
            var item = items[index];
            var header = slot[0];
            ((Label)header[0]).text = item.Code;
            ((Label)header[1]).text = item.Weight;
            ((Label)slot[1]).text = item.Name;
            ((Label)slot[2]).text = item.Type;
        }

        private void OnSelectionChanged(int index)
        {
            var item = ViewModel.GetCurrentItem();
            _inspectorNameSlot?.SetText(item.Name);
            if (_inspectorRaritySlot != null)
            {
                using var rarityBuilder = ZString.CreateStringBuilder(true);
                rarityBuilder.Append('[');
                rarityBuilder.Append(item.Rarity);
                rarityBuilder.Append(']');
                _inspectorRaritySlot.SetText(rarityBuilder.AsSpan());
            }
            _inspectorPowerSlot?.SetText(item.Power);
            _inspectorDescSlot?.SetText(item.Description);

            if (_slotPool != null)
            {
                for (var i = 0; i < _slotPool.Capacity; i++)
                {
                    var child = _slotPool.GetSlot(i);
                    if (i == index) child.AddToClassList(SelectedClass);
                    else child.RemoveFromClassList(SelectedClass);
                }
            }
        }
    }
}
