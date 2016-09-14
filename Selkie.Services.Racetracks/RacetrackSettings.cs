using System;

namespace Selkie.Services.Racetracks
{
    public class RacetrackSettings
    {
        public Guid ColonyId { get; set; }
        public bool IsPortTurnAllowed;
        public bool IsStarboardTurnAllowed;
        public double TurnRadiusForPort;
        public double TurnRadiusForStarboard;
    }
}