using System;
using global::R3;
using SUIF.Base;
using SUIF.Showcase.Common;

namespace SUIF.Showcase.Windows.ThemeMatrix
{
    public sealed class ThemeMatrixWindowViewModel : BaseViewModel
    {
        private readonly ShowcaseThemeService _themeService;

        public ReadOnlyReactiveProperty<string> CurrentTheme => _themeService.CurrentTheme;

        public ThemeMatrixWindowViewModel(ShowcaseThemeService themeService)
        {
            _themeService = themeService ?? throw new ArgumentNullException(nameof(themeService));
        }

        public void ApplyTheme(string themeId)
        {
            _themeService.ApplyTheme(themeId);
        }
    }
}

