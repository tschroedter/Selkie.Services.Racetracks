using JetBrains.Annotations;
using Selkie.Geometry.Shapes;
using Selkie.Services.Common.Dto;
using Selkie.Services.Racetracks.Interfaces.Converters.Dtos;

namespace Selkie.Services.Racetracks.Converters.Dtos
{
    public class CircleToCircleDtoConverter : ICircleToCircleDtoConverter
    {
        public CircleToCircleDtoConverter([NotNull] IPointToPointDtoConverter pointToPointDto)
        {
            m_PointToPointDto = pointToPointDto;
        }

        private readonly IPointToPointDtoConverter m_PointToPointDto;
        private ICircle m_Circle = Geometry.Shapes.Circle.Unknown;
        private CircleDto m_Dto = new CircleDto();

        [NotNull]
        public ICircle Circle
        {
            get
            {
                return m_Circle;
            }
            set
            {
                m_Circle = value;
            }
        }

        public CircleDto Dto
        {
            get
            {
                return m_Dto;
            }
        }

        public void Convert()
        {
            m_PointToPointDto.Point = Circle.CentrePoint;
            m_PointToPointDto.Convert();

            m_Dto = new CircleDto
                    {
                        IsUnknown = Circle.IsUnknown,
                        CentrePoint = m_PointToPointDto.Dto,
                        Radius = Circle.Radius
                    };
        }
    }
}