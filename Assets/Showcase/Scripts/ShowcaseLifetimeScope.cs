using SUIF.Addressables;
using SUIF.API;
using SUIF.Showcase.Benchmark;
using SUIF.Showcase.Common;
using SUIF.Showcase.Data;
using SUIF.Showcase.Formatting;
using SUIF.Showcase.Frame;
using SUIF.Showcase.Services;
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
        [SerializeField] private ShowcaseInventoryCatalog _inventoryCatalog;

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

            builder.Register<BenchmarkTelemetryService>(Lifetime.Singleton)
                .AsImplementedInterfaces()
                .AsSelf();

            builder.Register<BenchmarkRunnerService>(Lifetime.Singleton)
                .AsImplementedInterfaces()
                .AsSelf();

            base.Configure(builder);
        }

        protected override void ConfigureWindows(IContainerBuilder builder)
        {
            builder.Register<ShowcaseTextFormatter>(Lifetime.Singleton).As<IShowcaseTextFormatter>();
            builder.Register<ShowcaseThemeService>(Lifetime.Singleton);
            builder.Register<LifecycleMonitorService>(Lifetime.Singleton).As<ILifecycleMonitorService>();
            builder.Register<CounterService>(Lifetime.Singleton).As<ICounterService>();
            builder.Register<ModalStateService>(Lifetime.Singleton).As<IModalStateService>();
            builder.Register<InventoryService>(Lifetime.Singleton).WithParameter(_inventoryCatalog).AsImplementedInterfaces();
            builder.Register<RecycledListService>(Lifetime.Singleton).AsImplementedInterfaces();

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
