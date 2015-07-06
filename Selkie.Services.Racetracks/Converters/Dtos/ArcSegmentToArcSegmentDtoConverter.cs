using JetBrains.Annotations;
using Selkie.Geometry.Shapes;
using Selkie.Services.Racetracks.Common.Dto;

namespace Selkie.Services.Racetracks.Converters.Dtos
{
    public class ArcSegmentToArcSegmentDtoConverter : IArcSegmentToArcSergmentDtoConverter
    {
        private readonly ICircleToCircleDtoConverter m_CircleToCircleDto;
        private readonly IPointToPointDtoConverter m_PointToPointDto;
        private IArcSegment m_ArcSegment = Geometry.Shapes.ArcSegment.Unknown;
        private ArcSegmentDto m_Dto = new ArcSegmentDto();

        public ArcSegmentToArcSegmentDtoConverter([NotNull] IPointToPointDtoConverter pointToPointDto,
                                                  [NotNull] ICircleToCircleDtoConverter circleToCircleDto)
        {
            m_PointToPointDto = pointToPointDto;
            m_CircleToCircleDto = circleToCircleDto;
        }

        public IArcSegment ArcSegment
        {
            get
            {
                return m_ArcSegment;
            }
            set
            {
                m_ArcSegment = value;
            }
        }

        public ArcSegmentDto Dto
        {
            get
            {
                return m_Dto;
            }
        }

        public void Convert()
        {
            m_Dto = new ArcSegmentDto
                    {
                        IsUnknown = m_ArcSegment.IsUnknown,
                        StartPoint = CreatePointDto(m_ArcSegment.StartPoint),
                        EndPoint = CreatePointDto(m_ArcSegment.EndPoint),
                        Circle = CreateCircleDto(new Circle(m_ArcSegment.CentrePoint,
                                                            m_ArcSegment.Radius)),
                        TurnDirection = m_ArcSegment.TurnDirection.ToString()
                    };
        }

        private PointDto CreatePointDto([NotNull] Point point)
        {
            m_PointToPointDto.Point = point;
            m_PointToPointDto.Convert();

            return m_PointToPointDto.Dto;
        }

        private CircleDto CreateCircleDto([NotNull] ICircle circle)
        {
            m_CircleToCircleDto.Circle = circle;
            m_CircleToCircleDto.Convert();

            return m_CircleToCircleDto.Dto;
        }
    }
}