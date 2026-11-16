using System.Threading;
using Cysharp.Threading.Tasks;

namespace SUIF.Showcase.Services
{
    public interface IShowcasePreloadable
    {
        UniTask PreloadAsync(CancellationToken ct = default);
    }
}
