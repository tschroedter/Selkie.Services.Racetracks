using Selkie.Racetrack;

namespace Selkie.Services.Racetracks.Converters
{
    public class CostStartToEndCalculator
        : BaseCostCalculator,
          ICostStartToEndCalculator
    {
        internal override double CalculateRacetrackCost(int fromLineId,
                                                        int toLineId)
        {
            IPath path = Racetracks.ReverseToReverse [ fromLineId ] [ toLineId ];

            return path.Distance.Length;
        }
    }
}