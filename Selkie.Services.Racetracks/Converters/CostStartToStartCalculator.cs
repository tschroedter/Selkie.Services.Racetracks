using JetBrains.Annotations;
using Selkie.Racetrack;
using Selkie.Windsor;

namespace Selkie.Services.Racetracks.Converters
{
    public class CostStartToStartCalculator
        : BaseCostCalculator,
          ICostStartToStartCalculator
    {
        public CostStartToStartCalculator([NotNull] ISelkieLogger logger)
            : base(logger)
        {
        }

        internal override double CalculateRacetrackCost(int fromLineId,
                                                        int toLineId)
        {
            IPath path = Racetracks.ReverseToForward [ fromLineId ] [ toLineId ];

            return path.Distance.Length;
        }
    }
}