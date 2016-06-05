using System.Linq;
using JetBrains.Annotations;
using Selkie.Geometry.Shapes;
using Selkie.Services.Common.Dto;
using Selkie.Services.Racetracks.Interfaces.Converters.Dtos;

namespace Selkie.Services.Racetracks.Converters.Dtos
{
    public class PolylineToPolylineDtoConverter : IPolylineToPolylineDtoConverter
    {
        public PolylineToPolylineDtoConverter([NotNull] ISegmentToSegmentDtoConverter segmentToSegmentDto)
        {
            m_SegmentToSegmentDto = segmentToSegmentDto;
        }

        private readonly ISegmentToSegmentDtoConverter m_SegmentToSegmentDto;
        private PolylineDto m_Dto = new PolylineDto(); // todo add Id to DTO check for direction
        private IPolyline m_Polyline = Geometry.Shapes.Polyline.Unknown;

        [NotNull]
        public IPolyline Polyline
        {
            get
            {
                return m_Polyline;
            }
            set
            {
                m_Polyline = value;
            }
        }

        public PolylineDto Dto
        {
            get
            {
                return m_Dto;
            }
        }

        public void Convert()
        {
            m_Dto = new PolylineDto
                    {
                        Segments = m_Polyline.Segments.Select(CreateSegmentDto).ToArray()
                    };
        }

        internal bool IsPolylineAUturn([NotNull] IPolyline polyline)
        {
            if ( !polyline.Segments.Any() ||
                 polyline.Segments.Count() != 3 )
            {
                return false;
            }

            return polyline.Segments.ElementAt(1) is IArcSegment;
        }

        private SegmentDto CreateSegmentDto([NotNull] IPolylineSegment segment)
        {
            m_SegmentToSegmentDto.Segment = segment;
            m_SegmentToSegmentDto.Convert();

            return m_SegmentToSegmentDto.Dto;
        }
    }
}