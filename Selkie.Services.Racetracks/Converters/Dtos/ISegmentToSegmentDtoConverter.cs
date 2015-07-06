using JetBrains.Annotations;
using Selkie.Geometry.Shapes;
using Selkie.Services.Racetracks.Common.Dto;

namespace Selkie.Services.Racetracks.Converters.Dtos
{
    public interface ISegmentToSegmentDtoConverter : IConverter
    {
        [NotNull]
        IPolylineSegment Segment { get; set; }

        [NotNull]
        SegmentDto Dto { get; }
    }
}