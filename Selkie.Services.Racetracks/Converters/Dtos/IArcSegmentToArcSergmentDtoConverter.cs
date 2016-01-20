using JetBrains.Annotations;
using Selkie.Geometry.Shapes;
using Selkie.Services.Common.Dto;

namespace Selkie.Services.Racetracks.Converters.Dtos
{
    public interface IArcSegmentToArcSergmentDtoConverter : IConverter
    {
        [NotNull]
        IArcSegment ArcSegment { get; set; }

        [NotNull]
        ArcSegmentDto Dto { get; }
    }
}