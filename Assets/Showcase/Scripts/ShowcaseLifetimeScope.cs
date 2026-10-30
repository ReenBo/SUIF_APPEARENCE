using SUIF.Addressables;
using SUIF.API;
using SUIF.Showcase.Common;
using SUIF.Showcase.Frame;
using SUIF.Showcase.Windows.Counter;
using SUIF.Showcase.Windows.Inventory;
using SUIF.Showcase.Windows.Lifecycle;
using SUIF.Showcase.Windows.Modals;
using SUIF.Showcase.Windows.RecycledList;
using SUIF.Showcase.Windows.ThemeMatrix;
using SUIF.VContainer;
using SUIF.ViewSystems;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace SUIF.Showcase
{
    public sealed class ShowcaseLifetimeScope : SUIFLifetimeScope
    {
        [SerializeField] private UIRoot _sceneUiRoot;

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

            builder.Register<AddressablesUIAssetProvider>(Lifetime.Singleton).As<IUIAssetProvider>();

            base.Configure(builder);
        }

        protected override void ConfigureWindows(IContainerBuilder builder)
        {
            builder.Register<ShowcaseThemeService>(Lifetime.Singleton);

            builder.Register<ShowcaseFrameViewModel>(Lifetime.Singleton);
            builder.Register<LifecycleWindowViewModel>(Lifetime.Transient);
            builder.Register<ReactiveCounterViewModel>(Lifetime.Transient);
            builder.Register<RecycledListWindowViewModel>(Lifetime.Transient);
            builder.Register<ModalDialogViewModel>(Lifetime.Transient);
            builder.Register<InventoryWindowViewModel>(Lifetime.Transient);
            builder.Register<ThemeMatrixWindowViewModel>(Lifetime.Transient);

            builder.Register<AppEntryPoint>(Lifetime.Singleton)
                .AsImplementedInterfaces()
                .AsSelf();
        }
    }
}
