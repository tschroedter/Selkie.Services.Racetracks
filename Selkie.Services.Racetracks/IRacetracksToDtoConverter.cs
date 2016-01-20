using Selkie.Racetrack;
using Selkie.Services.Common.Dto;

namespace Selkie.Services.Racetracks
{
    public interface IRacetracksToDtoConverter
    {
        RacetracksDto ConvertPaths(IRacetracks racetracks);
    }
}