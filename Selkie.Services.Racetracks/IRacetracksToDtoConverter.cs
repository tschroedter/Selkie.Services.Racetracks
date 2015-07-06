using Selkie.Racetrack;
using Selkie.Services.Racetracks.Common.Dto;

namespace Selkie.Services.Racetracks
{
    public interface IRacetracksToDtoConverter
    {
        RacetracksDto ConvertPaths(IRacetracks racetracks);
    }
}