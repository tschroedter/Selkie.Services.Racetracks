using JetBrains.Annotations;
using Selkie.Geometry.Primitives;

namespace Selkie.Services.Racetracks
{
    public interface IRacetrackSettingsSource
    {
        [NotNull]
        Distance TurnRadius { get; }

        bool IsPortTurnAllowed { get; }
        bool IsStarboardTurnAllowed { get; }
    }
}