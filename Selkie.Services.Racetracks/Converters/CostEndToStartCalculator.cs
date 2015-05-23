using Selkie.Racetrack;

namespace Selkie.Services.Racetracks.Converters
{
    public class CostEndToStartCalculator
        : BaseCostCalculator,
          ICostEndToStartCalculator
    {
        internal override double CalculateRacetrackCost(int fromLineId,
                                                        int toLineId)
        {
            IPath path = Racetracks.ForwardToForward [ fromLineId ] [ toLineId ];

            return path.Distance.Length;
        }
    }
}