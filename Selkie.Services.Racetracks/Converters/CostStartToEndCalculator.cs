using Castle.Core.Logging;
using JetBrains.Annotations;
using Selkie.Racetrack;

namespace Selkie.Services.Racetracks.Converters
{
    public class CostStartToEndCalculator
        : BaseCostCalculator,
          ICostStartToEndCalculator
    {
        public CostStartToEndCalculator([NotNull] ILogger logger)
            : base(logger)
        {
        }

        internal override double CalculateRacetrackCost(int fromLineId,
                                                        int toLineId)
        {
            IPath path = Racetracks.ReverseToReverse [ fromLineId ] [ toLineId ];

            return path.Distance.Length;
        }
    }
}