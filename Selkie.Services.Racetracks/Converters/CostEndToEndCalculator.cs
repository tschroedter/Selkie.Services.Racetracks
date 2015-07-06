using Castle.Core.Logging;
using JetBrains.Annotations;
using Selkie.Racetrack;

namespace Selkie.Services.Racetracks.Converters
{
    public class CostEndToEndCalculator
        : BaseCostCalculator,
          ICostEndToEndCalculator
    {
        public CostEndToEndCalculator([NotNull] ILogger logger)
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