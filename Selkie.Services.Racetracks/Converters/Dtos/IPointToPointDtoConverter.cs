using JetBrains.Annotations;
using Selkie.Geometry.Shapes;
using Selkie.Services.Racetracks.Common.Dto;

namespace Selkie.Services.Racetracks.Converters.Dtos
{
    public interface IPointToPointDtoConverter : IConverter
    {
        [NotNull]
        Point Point { get; set; }

        [NotNull]
        PointDto Dto { get; }
    }
}