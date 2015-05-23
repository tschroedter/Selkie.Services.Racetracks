using JetBrains.Annotations;

namespace Selkie.Services.Racetracks
{
    public interface ICostMatrixSourceManager
    {
        [NotNull]
        ICostMatrix Source { get; }
    }
}