using JetBrains.Annotations;
using Selkie.Racetrack.Interfaces;
using Selkie.Services.Racetracks.Interfaces.Converters;
using Selkie.Windsor;

namespace Selkie.Services.Racetracks.Converters
{
    public class CostEndToEndCalculator
        : BaseCostCalculator,
          ICostEndToEndCalculator
    {
        public CostEndToEndCalculator([NotNull] ISelkieLogger logger)
            : base(logger)
        {
        }

        internal override double CalculateRacetrackCost(int fromFeatureId,
                                                        int toFeatureId)
        {
            IPath path = Racetracks.ForwardToReverse [ fromFeatureId ] [ toFeatureId ];

            return path.Distance.Length;
        }
    }
}