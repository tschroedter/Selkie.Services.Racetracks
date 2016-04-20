using JetBrains.Annotations;
using Selkie.Geometry.Shapes;
using Selkie.Services.Common.Dto;

namespace Selkie.Services.Racetracks.Interfaces.Converters.Dtos
{
    public interface ILineToLineSegmentDtoConverter : IConverter
    {
        [NotNull]
        ILine Line { get; set; }

        [NotNull]
        LineSegmentDto Dto { get; }
    }
}