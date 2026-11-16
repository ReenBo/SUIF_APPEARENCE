using System;
using global::R3;
using SUIF.Base;
using SUIF.R3.Base;
using SUIF.Showcase.Contracts;
using SUIF.Showcase.Services;

namespace SUIF.Showcase.Common
{
    public sealed class LoadingSpinnerViewModel : BaseViewModel, IReactiveDataViewModel<LoadingStateDto>
    {
        private readonly IShowcaseDataLoader _dataLoader;

        public ReadOnlyReactiveProperty<LoadingStateDto> State => _dataLoader.State;
        public Observable<LoadingStateDto> Data => _dataLoader.State;

        public LoadingSpinnerViewModel(IShowcaseDataLoader dataLoader)
        {
            _dataLoader = dataLoader ?? throw new ArgumentNullException(nameof(dataLoader));
        }
    }
}
