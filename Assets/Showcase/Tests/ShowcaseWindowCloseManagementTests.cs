using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using NUnit.Framework;
using SUIF.API;
using SUIF.Showcase.Common;
using SUIF.Showcase.Frame;
using SUIF.Showcase.Windows.Counter;
using SUIF.Showcase.Windows.Inventory;
using SUIF.Showcase.Windows.Lifecycle;
using SUIF.Showcase.Windows.Modals;
using SUIF.Showcase.Windows.RecycledList;
using SUIF.Showcase.Windows.ThemeMatrix;

namespace SUIF.Showcase.Tests
{
    [TestFixture]
    public sealed class ShowcaseWindowCloseManagementTests
    {
        private MockUIFlow _flow;
        private ShowcaseThemeService _themeService;
        private ShowcaseFrameViewModel _viewModel;

        [SetUp]
        public void SetUp()
        {
            _flow = new MockUIFlow();
            _themeService = new ShowcaseThemeService();
            _viewModel = new ShowcaseFrameViewModel(_flow, _themeService);
        }

        [TearDown]
        public void TearDown()
        {
            _viewModel.Dispose();
            _themeService.Dispose();
        }

        [Test]
        public void InitialState_StartsInStandbyWithNoActiveWindows()
        {
            Assert.That(_viewModel.ActiveNavIndex.Value, Is.EqualTo(0));
            Assert.That(_viewModel.ActiveTitle.Value, Is.EqualTo("[ NO_ACTIVE_VIEW // IDLE ]"));
            Assert.That(_viewModel.IsModalOpen.Value, Is.False);
            Assert.That(_viewModel.FooterStatus.Value, Is.EqualTo("STATE_SYNC // READY"));
        }

        [Test]
        public void OpenLifecycleWindow_SetsActiveNavIndexAndOpensView()
        {
            _viewModel.OpenLifecycleWindow();

            Assert.That(_viewModel.ActiveNavIndex.Value, Is.EqualTo(1));
            Assert.That(_viewModel.ActiveTitle.Value, Is.EqualTo("Basic Window Lifecycle"));
            Assert.That(_flow.OpenedTypes, Contains.Item(typeof(LifecycleWindowView)));
        }

        [Test]
        public void SwitchingBetweenWindows_ClosesPreviousBaseWindows()
        {
            _viewModel.OpenLifecycleWindow();
            _viewModel.OpenCounterWindow();

            Assert.That(_viewModel.ActiveNavIndex.Value, Is.EqualTo(2));
            Assert.That(_viewModel.ActiveTitle.Value, Is.EqualTo("MVVM & R3 Reactive Counter"));
            Assert.That(_flow.ClosedTypes, Contains.Item(typeof(LifecycleWindowView)));
            Assert.That(_flow.OpenedTypes, Contains.Item(typeof(ReactiveCounterView)));
        }

        [Test]
        public void SwitchingRapidlyAcrossAllModules_EnsuresNoOrphanedBaseWindows()
        {
            _viewModel.OpenLifecycleWindow();
            _viewModel.OpenCounterWindow();
            _viewModel.OpenRecycledListWindow();
            _viewModel.OpenInventoryWindow();
            _viewModel.OpenThemeMatrixWindow();

            Assert.That(_viewModel.ActiveNavIndex.Value, Is.EqualTo(6));
            Assert.That(_flow.ClosedTypes, Contains.Item(typeof(LifecycleWindowView)));
            Assert.That(_flow.ClosedTypes, Contains.Item(typeof(ReactiveCounterView)));
            Assert.That(_flow.ClosedTypes, Contains.Item(typeof(RecycledListWindowView)));
            Assert.That(_flow.ClosedTypes, Contains.Item(typeof(InventoryWindowView)));

            _viewModel.CloseActiveWindow();

            Assert.That(_viewModel.ActiveNavIndex.Value, Is.EqualTo(0));
            Assert.That(_viewModel.ActiveTitle.Value, Is.EqualTo("[ NO_ACTIVE_VIEW // IDLE ]"));
            Assert.That(_flow.ClosedTypes, Contains.Item(typeof(ThemeMatrixWindowView)));
        }

        [Test]
        public void CloseBaseWindow_ClosesAllBaseWindowsAndRevertsToStandby()
        {
            _viewModel.OpenRecycledListWindow();
            _viewModel.CloseBaseWindow();

            Assert.That(_viewModel.ActiveNavIndex.Value, Is.EqualTo(0));
            Assert.That(_viewModel.ActiveTitle.Value, Is.EqualTo("[ NO_ACTIVE_VIEW // IDLE ]"));
            Assert.That(_viewModel.FooterStatus.Value, Is.EqualTo("CLOSED // BASE_WINDOW"));
            Assert.That(_flow.ClosedTypes, Contains.Item(typeof(RecycledListWindowView)));
        }

        [Test]
        public void CloseActiveWindow_WithModalOpen_ClosesModalFirst()
        {
            _viewModel.OpenLifecycleWindow();
            _viewModel.ToggleModal();

            Assert.That(_viewModel.IsModalOpen.Value, Is.True);

            _viewModel.CloseActiveWindow();

            Assert.That(_viewModel.IsModalOpen.Value, Is.False);
            Assert.That(_viewModel.ActiveNavIndex.Value, Is.EqualTo(1));
            Assert.That(_flow.ClosedTypes, Contains.Item(typeof(ModalDialogView)));

            _viewModel.CloseActiveWindow();

            Assert.That(_viewModel.ActiveNavIndex.Value, Is.EqualTo(0));
            Assert.That(_flow.ClosedTypes, Contains.Item(typeof(LifecycleWindowView)));
        }

        [Test]
        public void CloseBaseWindow_WithModalOpen_DirectlyClosesBaseWindow()
        {
            _viewModel.OpenLifecycleWindow();
            _viewModel.ToggleModal();

            _viewModel.CloseBaseWindow();

            Assert.That(_viewModel.ActiveNavIndex.Value, Is.EqualTo(0));
            Assert.That(_flow.ClosedTypes, Contains.Item(typeof(LifecycleWindowView)));
        }

        private sealed class MockUIFlow : IUIFlow
        {
            public List<Type> OpenedTypes { get; } = new();
            public List<Type> ClosedTypes { get; } = new();

            public UniTask<TView> OpenAsync<TView>(CancellationToken ct = default) where TView : class, IView
            {
                OpenedTypes.Add(typeof(TView));
                return UniTask.FromResult<TView>(null);
            }

            public UniTask<TView> OpenViewAsync<TView>(CancellationToken ct = default) where TView : class, IView
            {
                return OpenAsync<TView>(ct);
            }

            public void Close<TView>() where TView : class, IView
            {
                ClosedTypes.Add(typeof(TView));
            }

            public UniTask CloseViewAsync<TView>(CancellationToken ct = default) where TView : class, IView
            {
                Close<TView>();
                return UniTask.CompletedTask;
            }

            public void CloseAll()
            {
            }
        }
    }
}
