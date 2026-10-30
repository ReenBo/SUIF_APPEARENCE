using Cysharp.Text;
using global::R3;
using SUIF.Attributes;
using SUIF.Base;
using SUIF.R3.Extensions;
using UnityEngine.UIElements;

namespace SUIF.Showcase.Windows.Inventory
{
    [UIView("InventoryWindowView", SUIF.API.UILayer.Windows)]
    public sealed class InventoryWindowView : BaseView<InventoryWindowViewModel>
    {
        private const string SelectedClass = "is-selected";

        [UQuery("inventory-slots-container")] private VisualElement _slotsContainer;
        [UQuery("inventory-inspector-name")] private Label _inspectorName;
        [UQuery("inventory-inspector-rarity")] private Label _inspectorRarity;
        [UQuery("inventory-inspector-power")] private Label _inspectorPower;
        [UQuery("inventory-inspector-desc")] private Label _inspectorDesc;
        [UQuery("btn-inventory-modal-inspect")] private Button _btnModalInspect;

        public InventoryWindowView(VisualElement root) : base(root)
        {
        }

        protected override void Bind()
        {
            if (_btnModalInspect != null)
            {
                Binder.Bind(_btnModalInspect).ToClick(ViewModel.InspectInModal);
            }

            AddDisposable(ViewModel.SelectedIndex.Subscribe(OnSelectionChanged));
            PopulateSlots();
        }

        private void OnSelectionChanged(int index)
        {
            var item = ViewModel.GetCurrentItem();
            if (_inspectorName != null) _inspectorName.text = item.Name;
            if (_inspectorRarity != null) _inspectorRarity.text = ZString.Concat("[", item.Rarity, "]");
            if (_inspectorPower != null) _inspectorPower.text = item.Power;
            if (_inspectorDesc != null) _inspectorDesc.text = item.Description;

            if (_slotsContainer != null)
            {
                for (var i = 0; i < _slotsContainer.childCount; i++)
                {
                    var child = _slotsContainer[i];
                    if (i == index) child.AddToClassList(SelectedClass);
                    else child.RemoveFromClassList(SelectedClass);
                }
            }
        }

        private void PopulateSlots()
        {
            if (_slotsContainer == null) return;
            _slotsContainer.Clear();
            var items = InventoryWindowViewModel.Items;
            for (var i = 0; i < items.Length; i++)
            {
                var slotIndex = i;
                var item = items[i];

                var slot = new Button(() => ViewModel.SelectSlot(slotIndex));
                slot.AddToClassList("fui-inventory-slot");
                if (i == ViewModel.SelectedIndex.Value)
                {
                    slot.AddToClassList(SelectedClass);
                }

                var header = new VisualElement();
                header.AddToClassList("fui-inventory-slot__header");
                var codeLabel = new Label(item.Code);
                codeLabel.AddToClassList("fui-inventory-slot__code");
                var weightLabel = new Label(item.Weight);
                weightLabel.AddToClassList("fui-inventory-slot__weight");
                header.Add(codeLabel);
                header.Add(weightLabel);

                var nameLabel = new Label(item.Name);
                nameLabel.AddToClassList("fui-inventory-slot__name");

                var typeLabel = new Label(item.Type);
                typeLabel.AddToClassList("fui-inventory-slot__type");

                slot.Add(header);
                slot.Add(nameLabel);
                slot.Add(typeLabel);

                _slotsContainer.Add(slot);
            }
        }
    }
}
