using SUIF.Attributes;
using SUIF.Base;
using SUIF.Controls;
using SUIF.R3.Extensions;
using SUIF.Showcase.Contracts;
using UnityEngine.UIElements;

namespace SUIF.Showcase.Windows.RecycledList
{
    [UIView("RecycledListWindowView", SUIF.API.UILayer.Windows, UILoadingMode.Cached)]
    public sealed class RecycledListWindowView : BaseView<RecycledListWindowViewModel>
    {
        [UQuery("list-nodes-container")] private ScrollView _listContainer;
        [UQuery("btn-window-close")] private Button _btnClose;

        public RecycledListWindowView(VisualElement root) : base(root)
        {
        }

        protected override void Bind()
        {
            if (_btnClose != null) Binder.Bind(_btnClose).ToClick(ViewModel.Close);
            if (_listContainer == null) return;

            _listContainer.Clear();
            var nodes = ViewModel.Nodes;
            for (var i = 0; i < nodes.Length; i++)
            {
                _listContainer.Add(CreateNodeRow(nodes[i]));
            }
        }

        private VisualElement CreateNodeRow(RecycledNodeDto node)
        {
            var formatter = ViewModel.Formatter;

            var row = new VisualElement();
            row.AddToClassList("fui-recycled-row");

            var left = new VisualElement();
            left.AddToClassList("fui-recycled-row__left");

            var indexLabel = CreateFixedLabel(8, "fui-recycled-row__index");
            indexLabel.SetFixedWritten(node.Index, formatter.RecycledNodeIndex);

            var nameLabel = CreateFixedLabel(10, "fui-recycled-row__name");
            nameLabel.SetFixedWritten(node.PoolSlot, formatter.RecycledNodePool);

            var addrLabel = CreateFixedLabel(16, "fui-recycled-row__addr");
            addrLabel.SetFixedWritten(node.MemoryAddress, formatter.RecycledNodeAddress);

            left.Add(indexLabel);
            left.Add(nameLabel);
            left.Add(addrLabel);

            var right = new VisualElement();
            right.AddToClassList("fui-recycled-row__right");

            var statusLabel = CreateFixedLabel(10, "fui-recycled-row__status");
            if (node.Status == RecycledNodeStatus.HotSwap)
            {
                statusLabel.AddToClassList("is-hot");
            }
            statusLabel.SetFixedWritten(node.Status, formatter.RecycledNodeStatusWriter);

            var latLabel = CreateFixedLabel(8, "fui-recycled-row__latency");
            latLabel.SetFixedWritten(node.LatencyMs, formatter.RecycledNodeLatency);

            right.Add(statusLabel);
            right.Add(latLabel);

            row.Add(left);
            row.Add(right);
            return row;
        }

        private FixedSlotLabel CreateFixedLabel(int capacity, string className)
        {
            var label = new FixedSlotLabel { Capacity = capacity };
            label.AddToClassList(className);
            Binder.AddDisposable(label);
            return label;
        }
    }
}
