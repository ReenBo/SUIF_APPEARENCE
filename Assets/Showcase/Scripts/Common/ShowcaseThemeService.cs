using System;
using global::R3;

namespace SUIF.Showcase.Common
{
    public sealed class ShowcaseThemeService : IDisposable
    {
        public ReactiveProperty<string> CurrentTheme { get; } = new("mono");

        public void ApplyTheme(string themeId)
        {
            CurrentTheme.Value = themeId;
        }

        public void Dispose()
        {
            CurrentTheme.Dispose();
        }
    }
}

