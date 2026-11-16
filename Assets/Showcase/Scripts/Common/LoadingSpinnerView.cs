using SUIF.Attributes;
using SUIF.Binding;
using SUIF.R3.Base;
using SUIF.Showcase.Contracts;
using SUIF.Showcase.Formatting;
using UnityEngine.UIElements;
using VContainer;

namespace SUIF.Showcase.Common
{
    [UIView("LoadingSpinnerView", SUIF.API.UILayer.Topmost, UILoadingMode.Cached)]
    public sealed class LoadingSpinnerView : BaseReactiveDataView<LoadingSpinnerViewModel, LoadingStateDto>
    {
        [UQuery("loading-spinner-glyph")] private Label _spinnerGlyph;
        [UQuery("loading-status-label")] private Label _statusLabel;

        private IShowcaseTextFormatter _formatter;
        private FixedTextSlot _glyphSlot;
        private FixedTextSlot _statusSlot;

        public LoadingSpinnerView(VisualElement root) : base(root)
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
            _glyphSlot = Binder.FixedSlot(_spinnerGlyph, 6);
            _statusSlot = Binder.FixedSlot(_statusLabel, 24);
        }

        protected override void RenderData(LoadingStateDto dto)
        {
            _glyphSlot?.SetWritten(dto.SpinnerStep, _formatter.LoadingSpinnerGlyph);
            _statusSlot?.SetWritten(dto, _formatter.LoadingStatus);
        }
    }
}
