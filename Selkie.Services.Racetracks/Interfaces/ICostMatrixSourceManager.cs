using JetBrains.Annotations;

namespace Selkie.Services.Racetracks.Interfaces
{
    public interface ICostMatrixSourceManager
    {
        [NotNull]
        ICostMatrix Source { get; }
    }
}