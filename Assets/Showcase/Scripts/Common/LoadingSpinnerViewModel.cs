using System;
using global::R3;
using SUIF.Base;
using SUIF.R3.Base;
using SUIF.Showcase.Contracts;
using SUIF.Showcase.Formatting;
using SUIF.Showcase.Services;

namespace SUIF.Showcase.Common
{
    public sealed class LoadingSpinnerViewModel : BaseViewModel, IReactiveDataViewModel<LoadingStateDto>
    {
        private readonly IShowcaseDataLoader _dataLoader;

        public IShowcaseTextFormatter Formatter { get; }
        public ReadOnlyReactiveProperty<LoadingStateDto> State => _dataLoader.State;
        public Observable<LoadingStateDto> Data => _dataLoader.State;

        public LoadingSpinnerViewModel(IShowcaseDataLoader dataLoader, IShowcaseTextFormatter formatter)
        {
            _dataLoader = dataLoader ?? throw new ArgumentNullException(nameof(dataLoader));
            Formatter = formatter ?? throw new ArgumentNullException(nameof(formatter));
        }
    }
}
