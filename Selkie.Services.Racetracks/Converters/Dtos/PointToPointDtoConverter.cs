using Selkie.Geometry.Shapes;
using Selkie.Services.Common.Dto;

namespace Selkie.Services.Racetracks.Converters.Dtos
{
    public class PointToPointDtoConverter : IPointToPointDtoConverter
    {
        private PointDto m_Dto = new PointDto();
        private Point m_Point = Point.Unknown;

        public Point Point
        {
            get
            {
                return m_Point;
            }
            set
            {
                m_Point = value;
            }
        }

        public PointDto Dto
        {
            get
            {
                return m_Dto;
            }
        }

        public void Convert()
        {
            m_Dto = new PointDto
                    {
                        X = Point.X,
                        Y = Point.Y
                    };
        }
    }
}