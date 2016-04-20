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

        internal override double CalculateRacetrackCost(int fromLineId,
                                                        int toLineId)
        {
            IPath path = Racetracks.ForwardToReverse [ fromLineId ] [ toLineId ];

            return path.Distance.Length;
        }
    }
}