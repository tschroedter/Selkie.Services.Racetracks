using Selkie.Services.Racetracks.Interfaces;

namespace Selkie.Services.Racetracks
{
    public class RacetrackSettingsSource : IRacetrackSettingsSource
    {
        internal static readonly double DefaultRadius = 60.0;

        public static readonly RacetrackSettingsSource Default = new RacetrackSettingsSource(DefaultRadius,
                                                                                             DefaultRadius,
                                                                                             true,
                                                                                             true);

        public RacetrackSettingsSource(double turnRadiusForPort,
                                       double turnRadiusForStarboard,
                                       bool isPortTurnAllowed,
                                       bool isStarboardTurnAllowed)
        {
            TurnRadiusForPort = turnRadiusForPort;
            TurnRadiusForStarboard = turnRadiusForStarboard;
            IsPortTurnAllowed = isPortTurnAllowed;
            IsStarboardTurnAllowed = isStarboardTurnAllowed;
        }

        public double TurnRadiusForStarboard { get; private set; }

        public double TurnRadiusForPort { get; private set; }

        public bool IsPortTurnAllowed { get; private set; }

        public bool IsStarboardTurnAllowed { get; private set; }
    }
}