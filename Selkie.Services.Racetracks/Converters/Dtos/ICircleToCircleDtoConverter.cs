using Selkie.Geometry.Shapes;
using Selkie.Services.Racetracks.Common.Dto;

namespace Selkie.Services.Racetracks.Converters.Dtos
{
    public interface ICircleToCircleDtoConverter : IConverter
    {
        ICircle Circle { get; set; }
        CircleDto Dto { get; }
    }
}