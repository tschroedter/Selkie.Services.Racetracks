using Selkie.Racetrack;

namespace Selkie.Services.Racetracks.Converters
{
    public class CostEndToEndCalculator
        : BaseCostCalculator,
          ICostEndToEndCalculator
    {
        internal override double CalculateRacetrackCost(int fromLineId,
                                                        int toLineId)
        {
            IPath path = Racetracks.ForwardToReverse [ fromLineId ] [ toLineId ];

            return path.Distance.Length;
        }
    }
}