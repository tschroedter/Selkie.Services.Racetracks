using JetBrains.Annotations;
using Selkie.Geometry.Shapes;
using Selkie.Racetrack.Interfaces;
using Selkie.Services.Common.Dto;
using Selkie.Services.Racetracks.Interfaces.Converters.Dtos;

namespace Selkie.Services.Racetracks.Converters.Dtos
{
    public class PathToPathDtoConverter : IPathToPathDtoConverter
    {
        public PathToPathDtoConverter([NotNull] IPointToPointDtoConverter pointToPointDto,
                                      [NotNull] IPolylineToPolylineDtoConverter polylineToPolylineDto)
        {
            m_PointToPointDto = pointToPointDto;
            m_PolylineToPolylineDto = polylineToPolylineDto;
        }

        private readonly IPointToPointDtoConverter m_PointToPointDto;
        private readonly IPolylineToPolylineDtoConverter m_PolylineToPolylineDto;
        private PathDto m_Dto = new PathDto();
        private IPath m_Path = Racetrack.Path.Unknown;

        [NotNull]
        public IPath Path
        {
            get
            {
                return m_Path;
            }
            set
            {
                m_Path = value;
            }
        }

        public PathDto Dto
        {
            get
            {
                return m_Dto;
            }
        }

        public void Convert()
        {
            m_Dto = new PathDto
                    {
                        IsUnknown = m_Path.IsUnknown,
                        StartPoint = CreatePointDto(m_Path.StartPoint),
                        EndPoint = CreatePointDto(m_Path.EndPoint),
                        Polyline = CreatePolylineDto(m_Path.Polyline)
                    };
        }

        private PointDto CreatePointDto([NotNull] Point point)
        {
            m_PointToPointDto.Point = point;
            m_PointToPointDto.Convert();

            return m_PointToPointDto.Dto;
        }

        private PolylineDto CreatePolylineDto(IPolyline polyline)
        {
            m_PolylineToPolylineDto.Polyline = polyline;
            m_PolylineToPolylineDto.Convert();

            return m_PolylineToPolylineDto.Dto;
        }
    }
}