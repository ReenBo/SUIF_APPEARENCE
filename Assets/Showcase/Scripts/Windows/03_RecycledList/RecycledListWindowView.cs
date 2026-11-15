using SUIF.Attributes;
using SUIF.Base;
using SUIF.Controls;
using SUIF.R3.Extensions;
using SUIF.Showcase.Contracts;
using SUIF.Showcase.Formatting;
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
            var nodes = RecycledListWindowViewModel.Nodes;
            for (var i = 0; i < nodes.Length; i++)
            {
                _listContainer.Add(CreateNodeRow(nodes[i]));
            }
        }

        private static VisualElement CreateNodeRow(RecycledNodeDto node)
        {
            var row = new VisualElement();
            row.AddToClassList("fui-recycled-row");

            var left = new VisualElement();
            left.AddToClassList("fui-recycled-row__left");

            var indexLabel = new FixedSlotLabel { Capacity = 8 };
            indexLabel.AddToClassList("fui-recycled-row__index");
            indexLabel.SetFixedWritten(node.Index, ShowcaseTextFormatter.WriteRecycledNodeIndex);

            var nameLabel = new FixedSlotLabel { Capacity = 10 };
            nameLabel.AddToClassList("fui-recycled-row__name");
            nameLabel.SetFixedWritten(node.PoolSlot, ShowcaseTextFormatter.WriteRecycledNodePool);

            var addrLabel = new FixedSlotLabel { Capacity = 16 };
            addrLabel.AddToClassList("fui-recycled-row__addr");
            addrLabel.SetFixedWritten(node.MemoryAddress, ShowcaseTextFormatter.WriteRecycledNodeAddress);

            left.Add(indexLabel);
            left.Add(nameLabel);
            left.Add(addrLabel);

            var right = new VisualElement();
            right.AddToClassList("fui-recycled-row__right");

            var statusLabel = new FixedSlotLabel { Capacity = 10 };
            statusLabel.AddToClassList("fui-recycled-row__status");
            if (node.Status == RecycledNodeStatus.HotSwap)
            {
                statusLabel.AddToClassList("is-hot");
            }
            statusLabel.SetFixedWritten(node.Status, ShowcaseTextFormatter.WriteRecycledNodeStatus);

            var latLabel = new FixedSlotLabel { Capacity = 8 };
            latLabel.AddToClassList("fui-recycled-row__latency");
            latLabel.SetFixedWritten(node.LatencyMs, ShowcaseTextFormatter.WriteRecycledNodeLatency);

            right.Add(statusLabel);
            right.Add(latLabel);

            row.Add(left);
            row.Add(right);
            return row;
        }
    }
}
