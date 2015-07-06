using System;
using JetBrains.Annotations;
using Selkie.Geometry.Shapes;
using Selkie.Services.Racetracks.Common.Dto;

namespace Selkie.Services.Racetracks.Converters.Dtos
{
    public class SegmentToSegmentDtoConverter : ISegmentToSegmentDtoConverter
    {
        private readonly IArcSegmentToArcSergmentDtoConverter m_ArcSegmentToArcSergmentDto;
        private readonly ILineToLineSegmentDtoConverter m_LineToLineSegmentDto;
        private SegmentDto m_Dto = new SegmentDto();
        private IPolylineSegment m_Segment = Line.Unknown;

        public SegmentToSegmentDtoConverter([NotNull] IArcSegmentToArcSergmentDtoConverter arcSegmentToArcSergmentDto,
                                            [NotNull] ILineToLineSegmentDtoConverter lineToLineSegmentDto)
        {
            m_ArcSegmentToArcSergmentDto = arcSegmentToArcSergmentDto;
            m_LineToLineSegmentDto = lineToLineSegmentDto;
        }

        public IPolylineSegment Segment
        {
            get
            {
                return m_Segment;
            }
            set
            {
                m_Segment = value;
            }
        }

        public SegmentDto Dto
        {
            get
            {
                return m_Dto;
            }
        }

        public void Convert()
        {
            var arcSegment = m_Segment as IArcSegment;

            if ( arcSegment != null )
            {
                m_Dto = CreateArcSergmentDto(arcSegment);
                return;
            }

            var lineSegment = m_Segment as ILine;

            if ( lineSegment != null )
            {
                m_Dto = CreateLineSegmentDto(lineSegment);
                return;
            }

            throw new ArgumentException("Unknown IPolylineSegment: " + m_Segment.GetType().FullName);
        }

        private LineSegmentDto CreateLineSegmentDto(ILine line)
        {
            m_LineToLineSegmentDto.Line = line;
            m_LineToLineSegmentDto.Convert();

            return m_LineToLineSegmentDto.Dto;
        }

        private ArcSegmentDto CreateArcSergmentDto(IArcSegment arcSegment)
        {
            m_ArcSegmentToArcSergmentDto.ArcSegment = arcSegment;
            m_ArcSegmentToArcSergmentDto.Convert();

            return m_ArcSegmentToArcSergmentDto.Dto;
        }
    }
}