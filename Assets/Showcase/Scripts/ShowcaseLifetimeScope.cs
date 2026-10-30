using SUIF.API;
using SUIF.VContainer;
using SUIF.ViewSystems;
using UnityEngine;
using UnityEngine.UIElements;
using VContainer;
using VContainer.Unity;

namespace SUIF.Showcase
{
    public sealed class ShowcaseLifetimeScope : SUIFLifetimeScope
    {
        [SerializeField] private UIRoot _sceneUiRoot;
        [SerializeField] private VisualTreeAsset _showcaseHubAsset;

        protected override void Configure(IContainerBuilder builder)
        {
            if (_sceneUiRoot != null)
            {
                builder.RegisterComponent(_sceneUiRoot).As<IUIRoot>();
            }
            else
            {
                var found = FindFirstObjectByType<UIRoot>();
                if (found != null)
                {
                    builder.RegisterComponent(found).As<IUIRoot>();
                }
            }

            builder.RegisterDirectAssetProvider();

            base.Configure(builder);
        }

        protected override void ConfigureWindows(IContainerBuilder builder)
        {
            builder.Register<ShowcaseHubViewModel>(Lifetime.Transient);

            if (_showcaseHubAsset != null)
            {
                builder.Register<ShowcaseEntryPoint>(Lifetime.Singleton)
                    .WithParameter(_showcaseHubAsset)
                    .AsImplementedInterfaces()
                    .AsSelf();
            }
            else
            {
                builder.Register<ShowcaseEntryPoint>(Lifetime.Singleton)
                    .AsImplementedInterfaces()
                    .AsSelf();
            }
        }
    }
}
