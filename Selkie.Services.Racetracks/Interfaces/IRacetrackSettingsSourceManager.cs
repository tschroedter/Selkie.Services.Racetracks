using JetBrains.Annotations;

namespace Selkie.Services.Racetracks.Interfaces
{
    public interface IRacetrackSettingsSourceManager
    {
        [NotNull]
        IRacetrackSettingsSource Source { get; }

        void SetSettings([NotNull] RacetrackSettings settings);
    }
}