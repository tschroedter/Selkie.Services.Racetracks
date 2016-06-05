using JetBrains.Annotations;
using Selkie.Racetrack.Interfaces;
using Selkie.Services.Racetracks.Interfaces.Converters;
using Selkie.Windsor;

namespace Selkie.Services.Racetracks.Converters
{
    public class CostStartToEndCalculator
        : BaseCostCalculator,
          ICostStartToEndCalculator
    {
        public CostStartToEndCalculator([NotNull] ISelkieLogger logger)
            : base(logger)
        {
        }

        internal override double CalculateRacetrackCost(int fromFeatureId,
                                                        int toFeatureId)
        {
            IPath path = Racetracks.ReverseToReverse [ fromFeatureId ] [ toFeatureId ];

            return path.Distance.Length;
        }
    }
}