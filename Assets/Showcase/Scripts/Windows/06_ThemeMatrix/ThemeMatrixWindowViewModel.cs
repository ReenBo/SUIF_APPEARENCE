using System;
using global::R3;
using SUIF.Base;
using SUIF.Showcase.Common;
using SUIF.Showcase.Frame;

namespace SUIF.Showcase.Windows.ThemeMatrix
{
    public sealed class ThemeMatrixWindowViewModel : BaseViewModel
    {
        private readonly ShowcaseThemeService _themeService;
        private readonly ShowcaseFrameViewModel _frameViewModel;

        public ReadOnlyReactiveProperty<string> CurrentTheme => _themeService.CurrentTheme;

        public ThemeMatrixWindowViewModel(ShowcaseThemeService themeService, ShowcaseFrameViewModel frameViewModel)
        {
            _themeService = themeService ?? throw new ArgumentNullException(nameof(themeService));
            _frameViewModel = frameViewModel ?? throw new ArgumentNullException(nameof(frameViewModel));
        }

        public void ApplyTheme(string themeId)
        {
            _themeService.ApplyTheme(themeId);
        }

        public void Close()
        {
            _frameViewModel.CloseActiveWindow();
        }
    }
}

