using JetBrains.Annotations;

namespace Selkie.Services.Racetracks
{
    public interface IRacetrackSettingsSourceManager
    {
        [NotNull]
        IRacetrackSettingsSource Source { get; }
    }
}