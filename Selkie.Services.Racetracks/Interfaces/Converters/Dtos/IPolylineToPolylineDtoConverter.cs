using Selkie.Geometry.Shapes;
using Selkie.Services.Common.Dto;

namespace Selkie.Services.Racetracks.Interfaces.Converters.Dtos
{
    public interface IPolylineToPolylineDtoConverter : IConverter
    {
        IPolyline Polyline { get; set; }
        PolylineDto Dto { get; }
    }
}