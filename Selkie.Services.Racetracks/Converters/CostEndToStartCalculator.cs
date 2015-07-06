using Castle.Core.Logging;
using JetBrains.Annotations;
using Selkie.Racetrack;

namespace Selkie.Services.Racetracks.Converters
{
    public class CostEndToStartCalculator
        : BaseCostCalculator,
          ICostEndToStartCalculator
    {
        public CostEndToStartCalculator([NotNull] ILogger logger)
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