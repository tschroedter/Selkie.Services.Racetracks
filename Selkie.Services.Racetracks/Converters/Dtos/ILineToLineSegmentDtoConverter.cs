using JetBrains.Annotations;
using Selkie.Geometry.Shapes;
using Selkie.Services.Racetracks.Common.Dto;

namespace Selkie.Services.Racetracks.Converters.Dtos
{
    public interface ILineToLineSegmentDtoConverter : IConverter
    {
        [NotNull]
        ILine Line { get; set; }

        [NotNull]
        LineSegmentDto Dto { get; }
    }
}