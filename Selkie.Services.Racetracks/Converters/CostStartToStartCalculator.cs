using Castle.Core.Logging;
using JetBrains.Annotations;
using Selkie.Racetrack;

namespace Selkie.Services.Racetracks.Converters
{
    public class CostStartToStartCalculator
        : BaseCostCalculator,
          ICostStartToStartCalculator
    {
        public CostStartToStartCalculator([NotNull] ILogger logger)
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