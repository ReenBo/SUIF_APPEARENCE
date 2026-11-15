using System;
using global::R3;
using SUIF.Showcase.Contracts;

namespace SUIF.Showcase.Common
{
    public sealed class ShowcaseThemeService : IDisposable
    {
        public ReactiveProperty<ShowcaseThemeDto> CurrentTheme { get; } = new(new ShowcaseThemeDto(ShowcaseThemeId.Mono));

        public void ApplyTheme(ShowcaseThemeId themeId)
        {
            CurrentTheme.Value = new ShowcaseThemeDto(themeId);
        }

        public void Dispose()
        {
            CurrentTheme.Dispose();
        }
    }
}
