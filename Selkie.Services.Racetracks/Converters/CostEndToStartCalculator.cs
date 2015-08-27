using JetBrains.Annotations;
using Selkie.Racetrack;
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

        internal override double CalculateRacetrackCost(int fromLineId,
                                                        int toLineId)
        {
            IPath path = Racetracks.ForwardToForward [ fromLineId ] [ toLineId ];

            return path.Distance.Length;
        }
    }
}