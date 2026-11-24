using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using NUnit.Framework;
using SUIF.API;
using SUIF.Binding;
using SUIF.Showcase.Contracts;
using SUIF.Showcase.Formatting;
using SUIF.Showcase.Frame;
using SUIF.Showcase.Hub;
using SUIF.Showcase.Services;
using SUIF.Showcase.Tahoe;
using SUIF.Showcase.Windows.Counter;
using SUIF.Showcase.Windows.Inventory;
using SUIF.Showcase.Windows.Lifecycle;
using SUIF.Showcase.Windows.Modals;
using SUIF.Showcase.Windows.RecycledList;
using SUIF.Showcase.Windows.ThemeMatrix;

namespace SUIF.Showcase.Tests
{
    [TestFixture]
    public sealed class ShowcaseHubAndTahoeTests
    {
        private MockHubFlow _flow;
        private ShowcaseTextFormatter _formatter;
        private HubNavigationService _hubNavigation;
        private CounterService _counterService;
        private LifecycleMonitorService _lifecycleService;
        private InventoryService _inventoryService;
        private TahoeDesktopService _tahoeService;

        [SetUp]
        public void SetUp()
        {
            _flow = new MockHubFlow();
            _formatter = new ShowcaseTextFormatter();
            _hubNavigation = new HubNavigationService(_flow);
            _counterService = new CounterService();
            _lifecycleService = new LifecycleMonitorService();
            _inventoryService = new InventoryService();
            _tahoeService = new TahoeDesktopService(_counterService);
        }

        [TearDown]
        public void TearDown()
        {
            _tahoeService.Dispose();
            _inventoryService.Dispose();
            _lifecycleService.Dispose();
            _counterService.Dispose();
            _hubNavigation.Dispose();
        }

        [Test]
        public void HubNavigation_OpenAndReturnToHub_UpdatesScreenAndFlow()
        {
            _hubNavigation.OpenHubAsync().GetAwaiter().GetResult();
            Assert.That(_hubNavigation.ActiveScreen.CurrentValue, Is.EqualTo(HubScreenId.Hub));
            Assert.That(_flow.OpenedTypes, Contains.Item(typeof(WelcomeHubView)));

            _hubNavigation.OpenStyleAsync(HubStyleType.CyberPunkStyle).GetAwaiter().GetResult();
            Assert.That(_hubNavigation.ActiveScreen.CurrentValue, Is.EqualTo(HubScreenId.CyberPunkStyle));
            Assert.That(_flow.OpenedTypes, Contains.Item(typeof(ShowcaseFrameView)));
            Assert.That(_flow.ClosedTypes, Contains.Item(typeof(WelcomeHubView)));

            _hubNavigation.ReturnToHubAsync().GetAwaiter().GetResult();
            Assert.That(_hubNavigation.ActiveScreen.CurrentValue, Is.EqualTo(HubScreenId.Hub));
            Assert.That(_flow.ClosedTypes, Contains.Item(typeof(ShowcaseFrameView)));
            Assert.That(_flow.ClosedTypes, Contains.Item(typeof(ModalDialogView)));
        }

        [Test]
        public void HubNavigation_OpenMacOSTahoe_SwitchesScreenAndReturns()
        {
            _hubNavigation.OpenStyleAsync(HubStyleType.MacOSTahoe).GetAwaiter().GetResult();
            Assert.That(_hubNavigation.ActiveScreen.CurrentValue, Is.EqualTo(HubScreenId.MacOSTahoe));
            Assert.That(_flow.OpenedTypes, Contains.Item(typeof(MacOSTahoeView)));

            _hubNavigation.ReturnToHubAsync().GetAwaiter().GetResult();
            Assert.That(_hubNavigation.ActiveScreen.CurrentValue, Is.EqualTo(HubScreenId.Hub));
            Assert.That(_flow.ClosedTypes, Contains.Item(typeof(MacOSTahoeView)));
        }

        [Test]
        public void HubNavigation_TriggerFutureSlot_ShowsToastWithSlot()
        {
            _hubNavigation.TriggerFutureSlot(FutureSlotId.Future2);
            var toast = _hubNavigation.ToastState.CurrentValue;

            Assert.That(toast.IsVisible, Is.True);
            Assert.That(toast.Slot, Is.EqualTo(FutureSlotId.Future2));

            var sb = Cysharp.Text.ZString.CreateStringBuilder();
            try
            {
                _formatter.HubToastText(ref sb, toast.Slot);
                Assert.That(sb.ToString(), Is.EqualTo("Future2 — Coming Soon"));
            }
            finally
            {
                sb.Dispose();
            }
        }

        [Test]
        public void TahoeDesktopService_OpenFocusCloseAndAllOperations_WorkCorrectly()
        {
            var initial = _tahoeService.State.CurrentValue;
            Assert.That(initial.IsWindowOpen(TahoeWindowId.RecycledList), Is.True);
            Assert.That(initial.HasFocusedWindow, Is.True);
            Assert.That(initial.FocusedWindow, Is.EqualTo(TahoeWindowId.RecycledList));

            _tahoeService.OpenOrFocusWindow(TahoeWindowId.Counter);
            Assert.That(_tahoeService.State.CurrentValue.IsWindowOpen(TahoeWindowId.Counter), Is.True);
            Assert.That(_tahoeService.State.CurrentValue.FocusedWindow, Is.EqualTo(TahoeWindowId.Counter));

            _tahoeService.CloseWindow(TahoeWindowId.Counter);
            Assert.That(_tahoeService.State.CurrentValue.IsWindowOpen(TahoeWindowId.Counter), Is.False);
            Assert.That(_tahoeService.State.CurrentValue.HasFocusedWindow, Is.True);

            _tahoeService.OpenAllWindows();
            for (int i = 0; i < 7; i++)
            {
                Assert.That(_tahoeService.State.CurrentValue.IsWindowOpen((TahoeWindowId)i), Is.True);
            }

            _tahoeService.CloseAllWindows();
            Assert.That(_tahoeService.State.CurrentValue.OpenMask, Is.EqualTo(0));
            Assert.That(_tahoeService.State.CurrentValue.HasFocusedWindow, Is.False);
        }

        [Test]
        public void TahoeDesktopService_ThemesAndModalSheet_UpdateState()
        {
            _tahoeService.SetTheme(TahoeThemeId.Indigo);
            Assert.That(_tahoeService.State.CurrentValue.Theme, Is.EqualTo(TahoeThemeId.Indigo));
            Assert.That(_formatter.GetTahoeThemeCssClass(TahoeThemeId.Indigo), Is.EqualTo("theme-tahoe-indigo"));

            _tahoeService.ToggleModalSheet(true);
            Assert.That(_tahoeService.State.CurrentValue.IsModalSheetOpen, Is.True);

            _tahoeService.SelectInventory100(42);
            Assert.That(_tahoeService.State.CurrentValue.Inventory100SelectedIndex, Is.EqualTo(42));
        }

        [Test]
        public void MacOSTahoeViewModel_ForwardsCommandsToServices()
        {
            using var vm = new MacOSTahoeViewModel(
                _tahoeService,
                _hubNavigation,
                _lifecycleService,
                _counterService,
                _inventoryService,
                _formatter);

            vm.StepCounter(5);
            Assert.That(vm.CounterState.CurrentValue.CurrentValue, Is.EqualTo(47));

            vm.ResetCounter();
            Assert.That(vm.CounterState.CurrentValue.CurrentValue, Is.EqualTo(0));

            vm.TriggerLifecycle(LifecycleStageId.Hide);
            Assert.That(vm.LifecycleState.CurrentValue.CurrentStage, Is.EqualTo(LifecycleStageId.Hide));

            vm.SelectInv6(3);
            Assert.That(vm.Inventory6State.CurrentValue.SelectedIndex, Is.EqualTo(3));
        }

        private sealed class MockHubFlow : IUIFlow
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
                ClosedTypes.Add(typeof(ShowcaseFrameView));
            }
        }
    }
}
