using Selkie.Racetrack.Interfaces;

namespace Selkie.Services.Racetracks.Converters.Dtos
{
    public class Racetracks : IRacetracks
    {
        private Racetracks()
        {
        }

        public static readonly IRacetracks Unknown = new Racetracks();
        private readonly IPath[][] m_ForwardToForward = new IPath[0][];
        private readonly IPath[][] m_ForwardToReverse = new IPath[0][];
        private readonly IPath[][] m_ReverseToForward = new IPath[0][];
        private readonly IPath[][] m_ReverseToReverse = new IPath[0][];

        public bool IsUnknown
        {
            get
            {
                return true;
            }
        }

        public IPath[][] ForwardToForward
        {
            get
            {
                return m_ForwardToForward;
            }
        }

        public IPath[][] ForwardToReverse
        {
            get
            {
                return m_ForwardToReverse;
            }
        }

        public IPath[][] ReverseToForward
        {
            get
            {
                return m_ReverseToForward;
            }
        }

        public IPath[][] ReverseToReverse
        {
            get
            {
                return m_ReverseToReverse;
            }
        }
    }
}