using System.Linq;
using JetBrains.Annotations;
using Selkie.Racetrack.Interfaces;
using Selkie.Services.Common.Dto;
using Selkie.Services.Racetracks.Interfaces;
using Selkie.Services.Racetracks.Interfaces.Converters.Dtos;
using Selkie.Windsor;

namespace Selkie.Services.Racetracks.Converters.Dtos
{
    [ProjectComponent(Lifestyle.Transient)]
    public class RacetracksToDtoConverter : IRacetracksToDtoConverter
    {
        public RacetracksToDtoConverter([NotNull] IPathToPathDtoConverter pathToPathDto)
        {
            m_PathToPathDto = pathToPathDto;
        }

        private readonly IPathToPathDtoConverter m_PathToPathDto;

        public RacetracksDto ConvertPaths(IRacetracks racetracks)
        {
            PathDto[][] forwardToForward = ConvertPaths(racetracks.ForwardToForward);
            PathDto[][] forwardToReverse = ConvertPaths(racetracks.ForwardToReverse);
            PathDto[][] reverseToForward = ConvertPaths(racetracks.ReverseToForward);
            PathDto[][] reverseToReverse = ConvertPaths(racetracks.ReverseToReverse);

            var dto = new RacetracksDto
                      {
                          IsUnknown = racetracks.IsUnknown,
                          ForwardToForward = forwardToForward,
                          ForwardToReverse = forwardToReverse,
                          ReverseToForward = reverseToForward,
                          ReverseToReverse = reverseToReverse
                      };

            return dto;
        }

        [NotNull]
        public PathDto[][] ConvertPaths([NotNull] IPath[][] racetracks)
        {
            var dtos = new PathDto[racetracks.Length][];
            var i = 0;

            foreach ( IPath[] racetrack in racetracks )
            {
                dtos [ i++ ] = racetrack.Select(ConvertPath).ToArray();
            }

            return dtos;
        }

        private PathDto ConvertPath(IPath path)
        {
            m_PathToPathDto.Path = path;
            m_PathToPathDto.Convert();

            return m_PathToPathDto.Dto;
        }
    }
}