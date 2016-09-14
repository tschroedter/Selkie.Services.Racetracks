using System;
using Selkie.Services.Racetracks.Interfaces;

namespace Selkie.Services.Racetracks
{
    public class RacetrackSettingsSource : IRacetrackSettingsSource
    {
        public RacetrackSettingsSource(
            Guid colonyId,
            double turnRadiusForPort,
            double turnRadiusForStarboard,
            bool isPortTurnAllowed,
            bool isStarboardTurnAllowed)
        {
            ColonyId = colonyId;
            TurnRadiusForPort = turnRadiusForPort;
            TurnRadiusForStarboard = turnRadiusForStarboard;
            IsPortTurnAllowed = isPortTurnAllowed;
            IsStarboardTurnAllowed = isStarboardTurnAllowed;
        }

        internal static readonly double DefaultRadius = 60.0;

        public static readonly RacetrackSettingsSource Default =
            new RacetrackSettingsSource(
                Guid.Empty,
                DefaultRadius,
                DefaultRadius,
                true,
                true);

        public Guid ColonyId { get; private set; }

        public double TurnRadiusForStarboard { get; private set; }

        public double TurnRadiusForPort { get; private set; }

        public bool IsPortTurnAllowed { get; private set; }

        public bool IsStarboardTurnAllowed { get; private set; }
    }
}