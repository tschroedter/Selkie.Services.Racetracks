using Selkie.Racetrack.Interfaces;
using Selkie.Services.Common.Dto;

namespace Selkie.Services.Racetracks.Interfaces
{
    public interface IRacetracksToDtoConverter
    {
        RacetracksDto ConvertPaths(IRacetracks racetracks);
    }
}