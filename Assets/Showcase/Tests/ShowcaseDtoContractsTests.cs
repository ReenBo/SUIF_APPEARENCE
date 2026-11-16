using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Text;
using Cysharp.Threading.Tasks;
using NUnit.Framework;
using SUIF.API;
using SUIF.Binding;
using SUIF.Showcase.Common;
using SUIF.Showcase.Contracts;
using SUIF.Showcase.Formatting;
using SUIF.Showcase.Frame;
using SUIF.Showcase.Services;
using SUIF.Showcase.Windows.Counter;
using SUIF.Showcase.Windows.Inventory;
using SUIF.Showcase.Windows.Lifecycle;
using SUIF.Showcase.Windows.Modals;
using SUIF.Showcase.Windows.RecycledList;

namespace SUIF.Showcase.Tests
{
    [TestFixture]
    public sealed class ShowcaseDtoContractsTests
    {
        private MockFlow _flow;
        private ShowcaseThemeService _themeService;
        private ShowcaseFrameViewModel _frameVm;

        [SetUp]
        public void SetUp()
        {
            _flow = new MockFlow();
            _themeService = new ShowcaseThemeService();
            _frameVm = new ShowcaseFrameViewModel(_flow, _themeService);
        }

        [TearDown]
        public void TearDown()
        {
            _frameVm.Dispose();
            _themeService.Dispose();
        }

        [Test]
        public void ThemeService_UpdatesTypedDtoAndCssClass()
        {
            Assert.That(_themeService.CurrentTheme.Value.Id, Is.EqualTo(ShowcaseThemeId.Mono));
            Assert.That(ShowcaseTextFormatter.GetThemeCssClass(_themeService.CurrentTheme.Value.Id), Is.EqualTo("theme-mono"));

            _themeService.ApplyTheme(ShowcaseThemeId.Crimson);

            Assert.That(_themeService.CurrentTheme.Value.Id, Is.EqualTo(ShowcaseThemeId.Crimson));
            Assert.That(ShowcaseTextFormatter.GetThemeCssClass(_themeService.CurrentTheme.Value.Id), Is.EqualTo("theme-crimson"));
        }

        [Test]
        public void FrameFormatter_WritesExpectedModuleTitlesAndTelemetryFromDto()
        {
            var dto = new FrameNavigationDto(ShowcaseModuleId.Inventory, true, FrameActionStatus.ModalMounted);

            Assert.That(Format(dto.ActiveModule, ShowcaseTextFormatter.WriteModuleTitle), Is.EqualTo("INVENTORY"));
            Assert.That(Format(dto.ActiveModule, ShowcaseTextFormatter.WriteModuleBaseReadout), Is.EqualTo("05_INVENTORY"));
            Assert.That(Format(dto, ShowcaseTextFormatter.WriteModalTelemetry), Is.EqualTo("MOUNTED [1]"));
            Assert.That(Format(dto, ShowcaseTextFormatter.WriteLayerTag), Is.EqualTo("LAYER_Z // 5_BASE + MODAL_999"));
            Assert.That(Format(dto, ShowcaseTextFormatter.WriteFooterStatus), Is.EqualTo("MOUNTED // MODAL_OVERLAY"));
        }

        [Test]
        public void LifecycleViewModel_TriggersTypedStageEntriesWithoutRawStrings()
        {
            using var service = new LifecycleMonitorService();
            using var vm = new LifecycleWindowViewModel(_flow, _frameVm, service);
            Assert.That(vm.State.CurrentValue.CurrentStage, Is.EqualTo(LifecycleStageId.Initialize));
            Assert.That(vm.State.CurrentValue.LogCount, Is.EqualTo(3));

            vm.TriggerStage(LifecycleStageId.Show);

            Assert.That(vm.State.CurrentValue.CurrentStage, Is.EqualTo(LifecycleStageId.Show));
            Assert.That(vm.State.CurrentValue.LogCount, Is.EqualTo(4));
            Assert.That(vm.LifecycleLogs[0].Stage, Is.EqualTo(LifecycleStageId.Show));
            Assert.That(vm.LifecycleLogs[0].IsInitialBootEntry, Is.False);

            var formatted = Format(vm.LifecycleLogs[0], ShowcaseTextFormatter.WriteLifecycleLogEntry);
            Assert.That(formatted, Does.Contain("OnShow() -> State transition executed"));
        }

        [Test]
        public void CounterViewModel_EmitsTypedStateAndFormatsZeroPaddedValue()
        {
            using var service = new CounterService();
            using var vm = new ReactiveCounterViewModel(_frameVm, service);
            Assert.That(vm.State.CurrentValue.CurrentValue, Is.EqualTo(42));
            Assert.That(vm.State.CurrentValue.HistoryCount, Is.EqualTo(5));

            vm.UpdateCounter(10);

            Assert.That(vm.State.CurrentValue.CurrentValue, Is.EqualTo(52));
            Assert.That(vm.State.CurrentValue.HistoryCount, Is.EqualTo(6));
            Assert.That(Format(vm.State.CurrentValue.CurrentValue, ShowcaseTextFormatter.WriteCounterValue), Is.EqualTo("052"));

            vm.ResetCounter();
            Assert.That(vm.State.CurrentValue.CurrentValue, Is.EqualTo(0));
            Assert.That(vm.State.CurrentValue.HistoryCount, Is.EqualTo(1));
        }

        [Test]
        public void ModalViewModel_CyclesTypedPresetsAndDepthDto()
        {
            using var service = new ModalStateService();
            using var vm = new ModalDialogViewModel(_flow, _frameVm, service);
            Assert.That(vm.State.CurrentValue.Depth, Is.EqualTo(1));
            Assert.That(vm.State.CurrentValue.Preset, Is.EqualTo(ModalPresetId.PopupIntercept));

            vm.PushSubPopup();

            Assert.That(vm.State.CurrentValue.Depth, Is.EqualTo(2));
            Assert.That(vm.State.CurrentValue.Preset, Is.EqualTo(ModalPresetId.MemoryPurge));
            Assert.That(Format(vm.State.CurrentValue.Depth, ShowcaseTextFormatter.WriteModalDepth), Is.EqualTo("DEPTH: 2"));
            Assert.That(Format(vm.State.CurrentValue.Preset, ShowcaseTextFormatter.WriteModalTitle), Is.EqualTo("CONFIRM // MEMORY_PURGE_0x04"));
        }

        [Test]
        public void InventoryViewModel_SelectsTypedItemAndFormatsStatsDto()
        {
            using var service = new InventoryService();
            using var vm = new InventoryWindowViewModel(_flow, _frameVm, service);
            Assert.That(vm.State.CurrentValue.SelectedIndex, Is.EqualTo(0));

            vm.SelectSlot(5);
            var item = vm.GetCurrentItem();

            Assert.That(vm.State.CurrentValue.SelectedIndex, Is.EqualTo(5));
            Assert.That(item.Code, Is.EqualTo(InventoryItemCode.Arm06));
            Assert.That(Format(item.Code, ShowcaseTextFormatter.WriteInventoryName), Is.EqualTo("SUBDERMAL_TITANIUM"));
            Assert.That(Format(item.Rarity, ShowcaseTextFormatter.WriteInventoryRarityBadge), Is.EqualTo("[HEAVY]"));
            Assert.That(Format(item, ShowcaseTextFormatter.WriteInventoryStat), Is.EqualTo("+320 ARM"));
        }

        [Test]
        public void RecycledNodes_UseTypedDtoAndFormatAddressHex()
        {
            var service = new RecycledListService();
            using var vm = new RecycledListWindowViewModel(_frameVm, service);
            var first = vm.Nodes[0];
            Assert.That(first.Status, Is.EqualTo(RecycledNodeStatus.HotSwap));
            Assert.That(Format(first.Index, ShowcaseTextFormatter.WriteRecycledNodeIndex), Is.EqualTo("#0001"));
            Assert.That(Format(first.PoolSlot, ShowcaseTextFormatter.WriteRecycledNodePool), Is.EqualTo("NODE_01"));
            Assert.That(Format(first.MemoryAddress, ShowcaseTextFormatter.WriteRecycledNodeAddress), Is.EqualTo("MEM: 0x00F410"));
            Assert.That(Format(first.LatencyMs, ShowcaseTextFormatter.WriteRecycledNodeLatency), Does.EndWith("ms"));
        }

        private static string Format<TState>(TState state, FixedTextWriter<TState> writer)
        {
            var builder = ZString.CreateStringBuilder(true);
            try
            {
                writer(ref builder, state);
                return builder.ToString();
            }
            finally
            {
                builder.Dispose();
            }
        }

        private sealed class MockFlow : IUIFlow
        {
            private readonly List<Type> _opened = new();

            public UniTask<TView> OpenAsync<TView>(CancellationToken ct = default) where TView : class, IView
            {
                _opened.Add(typeof(TView));
                return UniTask.FromResult<TView>(null);
            }

            public UniTask<TView> OpenViewAsync<TView>(CancellationToken ct = default) where TView : class, IView
            {
                return OpenAsync<TView>(ct);
            }

            public void Close<TView>() where TView : class, IView
            {
            }

            public UniTask CloseViewAsync<TView>(CancellationToken ct = default) where TView : class, IView
            {
                return UniTask.CompletedTask;
            }

            public void CloseAll()
            {
            }
        }
    }
}
