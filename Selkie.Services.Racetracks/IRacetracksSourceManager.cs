using JetBrains.Annotations;
using Selkie.Racetrack;

namespace Selkie.Services.Racetracks
{
    public interface IRacetracksSourceManager
    {
        [NotNull]
        IRacetracks Racetracks { get; }
    }
}