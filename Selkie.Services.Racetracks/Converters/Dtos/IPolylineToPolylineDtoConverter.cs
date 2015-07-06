using Selkie.Geometry.Shapes;
using Selkie.Services.Racetracks.Common.Dto;

namespace Selkie.Services.Racetracks.Converters.Dtos
{
    public interface IPolylineToPolylineDtoConverter : IConverter
    {
        IPolyline Polyline { get; set; }
        PolylineDto Dto { get; }
    }
}