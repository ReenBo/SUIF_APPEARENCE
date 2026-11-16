using SUIF.Attributes;
using SUIF.Binding;
using SUIF.R3.Base;
using SUIF.Showcase.Contracts;
using UnityEngine.UIElements;

namespace SUIF.Showcase.Common
{
    [UIView("LoadingSpinnerView", SUIF.API.UILayer.Topmost, UILoadingMode.Cached)]
    public sealed class LoadingSpinnerView : BaseReactiveDataView<LoadingSpinnerViewModel, LoadingStateDto>
    {
        [UQuery("loading-spinner-glyph")] private Label _spinnerGlyph;
        [UQuery("loading-status-label")] private Label _statusLabel;

        private FixedTextSlot _glyphSlot;
        private FixedTextSlot _statusSlot;

        public LoadingSpinnerView(VisualElement root) : base(root)
        {
        }

        protected override void OnBind()
        {
            _glyphSlot = Binder.FixedSlot(_spinnerGlyph, 6);
            _statusSlot = Binder.FixedSlot(_statusLabel, 24);
        }

        protected override void RenderData(LoadingStateDto dto)
        {
            var formatter = ViewModel.Formatter;
            _glyphSlot?.SetWritten(dto.SpinnerStep, formatter.LoadingSpinnerGlyph);
            _statusSlot?.SetWritten(dto, formatter.LoadingStatus);
        }
    }
}
