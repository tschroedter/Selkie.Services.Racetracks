using JetBrains.Annotations;
using Selkie.Geometry.Shapes;
using Selkie.Services.Common.Dto;

namespace Selkie.Services.Racetracks.Converters.Dtos
{
    public class LineToLineSegmentDtoConverter : ILineToLineSegmentDtoConverter
    {
        private readonly IPointToPointDtoConverter m_PointToPointDto;
        private LineSegmentDto m_Dto = new LineSegmentDto();
        private ILine m_Line = Geometry.Shapes.Line.Unknown;

        public LineToLineSegmentDtoConverter([NotNull] IPointToPointDtoConverter pointToPointDto)
        {
            m_PointToPointDto = pointToPointDto;
        }

        public ILine Line
        {
            get
            {
                return m_Line;
            }
            set
            {
                m_Line = value;
            }
        }

        public LineSegmentDto Dto
        {
            get
            {
                return m_Dto;
            }
        }

        public void Convert()
        {
            m_Dto = new LineSegmentDto
                    {
                        IsUnknown = m_Line.IsUnknown,
                        StartPoint = CreatePointDto(m_Line.StartPoint),
                        EndPoint = CreatePointDto(m_Line.EndPoint),
                        RunDirection = m_Line.RunDirection.ToString()
                    };
        }

        private PointDto CreatePointDto([NotNull] Point point)
        {
            m_PointToPointDto.Point = point;
            m_PointToPointDto.Convert();

            return m_PointToPointDto.Dto;
        }
    }
}