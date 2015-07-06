using System.Linq;
using JetBrains.Annotations;
using Selkie.Geometry.Shapes;
using Selkie.Services.Racetracks.Common.Dto;

namespace Selkie.Services.Racetracks.Converters.Dtos
{
    public class PolylineToPolylineDtoConverter : IPolylineToPolylineDtoConverter
    {
        private readonly ISegmentToSegmentDtoConverter m_SegmentToSegmentDto;
        private PolylineDto m_Dto = new PolylineDto();
        private IPolyline m_Polyline = new Polyline(); // todo Polyline.Unknown

        public PolylineToPolylineDtoConverter([NotNull] ISegmentToSegmentDtoConverter segmentToSegmentDto)
        {
            m_SegmentToSegmentDto = segmentToSegmentDto;
        }

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

        private SegmentDto CreateSegmentDto([NotNull] IPolylineSegment segment)
        {
            m_SegmentToSegmentDto.Segment = segment;
            m_SegmentToSegmentDto.Convert();

            return m_SegmentToSegmentDto.Dto;
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
    }
}