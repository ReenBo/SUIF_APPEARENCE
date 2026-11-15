using SUIF.Attributes;
using SUIF.Base;
using SUIF.R3.Extensions;
using SUIF.Showcase.Contracts;
using UnityEngine.UIElements;

namespace SUIF.Showcase.Windows.ThemeMatrix
{
    [UIView("ThemeMatrixWindowView", SUIF.API.UILayer.Windows, UILoadingMode.Cached)]
    public sealed class ThemeMatrixWindowView : BaseView<ThemeMatrixWindowViewModel>
    {
        [UQuery("btn-theme-mono")] private Button _btnMono;
        [UQuery("btn-theme-cyan")] private Button _btnCyan;
        [UQuery("btn-theme-crimson")] private Button _btnCrimson;
        [UQuery("btn-theme-amber")] private Button _btnAmber;
        [UQuery("btn-window-close")] private Button _btnClose;

        public ThemeMatrixWindowView(VisualElement root) : base(root)
        {
        }

        protected override void Bind()
        {
            if (_btnMono != null) Binder.Bind(_btnMono).ToClick(() => ViewModel.ApplyTheme(ShowcaseThemeId.Mono));
            if (_btnCyan != null) Binder.Bind(_btnCyan).ToClick(() => ViewModel.ApplyTheme(ShowcaseThemeId.Cyan));
            if (_btnCrimson != null) Binder.Bind(_btnCrimson).ToClick(() => ViewModel.ApplyTheme(ShowcaseThemeId.Crimson));
            if (_btnAmber != null) Binder.Bind(_btnAmber).ToClick(() => ViewModel.ApplyTheme(ShowcaseThemeId.Amber));
            if (_btnClose != null) Binder.Bind(_btnClose).ToClick(ViewModel.Close);
        }
    }
}
