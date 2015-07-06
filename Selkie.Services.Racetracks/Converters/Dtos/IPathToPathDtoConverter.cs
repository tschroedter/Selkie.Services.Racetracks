using Selkie.Racetrack;
using Selkie.Services.Racetracks.Common.Dto;

namespace Selkie.Services.Racetracks.Converters.Dtos
{
    public interface IPathToPathDtoConverter : IConverter
    {
        IPath Path { get; set; }
        PathDto Dto { get; }
    }
}