using JetBrains.Annotations;
using Selkie.Geometry.Primitives;

namespace Selkie.Services.Racetracks
{
    public class RacetrackSettingsSource : IRacetrackSettingsSource
    {
        internal static readonly Distance DefaultRadius = new Distance(60.0);

        public static readonly RacetrackSettingsSource Default = new RacetrackSettingsSource(DefaultRadius,
                                                                                             true,
                                                                                             true);

        private readonly bool m_IsPortTurnAllowed;
        private readonly bool m_IsStarboardTurnAllowed;
        private readonly Distance m_TurnRadius;

        public RacetrackSettingsSource([NotNull] Distance turnRadius,
                                       bool isPortTurnAllowed,
                                       bool isStarboardTurnAllowed)
        {
            m_TurnRadius = turnRadius;
            m_IsPortTurnAllowed = isPortTurnAllowed;
            m_IsStarboardTurnAllowed = isStarboardTurnAllowed;
        }

        public Distance TurnRadius
        {
            get
            {
                return m_TurnRadius;
            }
        }

        public bool IsPortTurnAllowed
        {
            get
            {
                return m_IsPortTurnAllowed;
            }
        }

        public bool IsStarboardTurnAllowed
        {
            get
            {
                return m_IsStarboardTurnAllowed;
            }
        }
    }
}