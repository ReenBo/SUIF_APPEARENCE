using Cysharp.Text;
using SUIF.Attributes;
using SUIF.Base;
using SUIF.R3.Extensions;
using UnityEngine.UIElements;

namespace SUIF.Showcase.Windows.RecycledList
{
    [UIView("RecycledListWindowView", SUIF.API.UILayer.Windows)]
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
                var node = nodes[i];
                var row = new VisualElement();
                row.AddToClassList("fui-recycled-row");

                var left = new VisualElement();
                left.AddToClassList("fui-recycled-row__left");

                var indexLabel = new Label(ZString.Concat("#", node.Index.ToString("D4")));
                indexLabel.AddToClassList("fui-recycled-row__index");
                var nameLabel = new Label(node.PoolNode);
                nameLabel.AddToClassList("fui-recycled-row__name");
                var addrLabel = new Label(ZString.Concat("MEM: ", node.Address));
                addrLabel.AddToClassList("fui-recycled-row__addr");

                left.Add(indexLabel);
                left.Add(nameLabel);
                left.Add(addrLabel);

                var right = new VisualElement();
                right.AddToClassList("fui-recycled-row__right");

                var statusLabel = new Label(node.Status);
                statusLabel.AddToClassList("fui-recycled-row__status");
                if (node.Status == "HOT_SWAP")
                {
                    statusLabel.AddToClassList("is-hot");
                }
                var latLabel = new Label(node.Latency);
                latLabel.AddToClassList("fui-recycled-row__latency");

                right.Add(statusLabel);
                right.Add(latLabel);

                row.Add(left);
                row.Add(right);
                _listContainer.Add(row);
            }
        }
    }
}

