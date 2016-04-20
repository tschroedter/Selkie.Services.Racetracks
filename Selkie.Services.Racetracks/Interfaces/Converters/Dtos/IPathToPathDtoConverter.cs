using Selkie.Racetrack.Interfaces;
using Selkie.Services.Common.Dto;

namespace Selkie.Services.Racetracks.Interfaces.Converters.Dtos
{
    public interface IPathToPathDtoConverter : IConverter
    {
        IPath Path { get; set; }
        PathDto Dto { get; }
    }
}