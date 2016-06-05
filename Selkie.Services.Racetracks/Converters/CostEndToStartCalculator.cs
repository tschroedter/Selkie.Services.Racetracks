using JetBrains.Annotations;
using Selkie.Racetrack.Interfaces;
using Selkie.Services.Racetracks.Interfaces.Converters;
using Selkie.Windsor;

namespace Selkie.Services.Racetracks.Converters
{
    public class CostEndToStartCalculator
        : BaseCostCalculator,
          ICostEndToStartCalculator
    {
        public CostEndToStartCalculator([NotNull] ISelkieLogger logger)
            : base(logger)
        {
        }

        internal override double CalculateRacetrackCost(int fromFeatureId,
                                                        int toFeatureId)
        {
            IPath path = Racetracks.ForwardToForward [ fromFeatureId ] [ toFeatureId ];

            return path.Distance.Length;
        }
    }
}