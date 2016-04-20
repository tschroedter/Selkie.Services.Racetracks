using System.Collections.Generic;
using JetBrains.Annotations;
using Selkie.Geometry.Shapes;
using Selkie.Racetrack.Interfaces;
using Selkie.Racetrack.Interfaces.Calculators;

namespace Selkie.Services.Racetracks.Interfaces.Converters
{
    public interface IBaseCostCalculator : ICalculator
    {
        [NotNull]
        Dictionary <int, double> Costs { get; }

        [NotNull]
        ILine Line { get; set; }

        [NotNull]
        IEnumerable <ILine> Lines { get; set; }

        [NotNull]
        IRacetracks Racetracks { get; set; }
    }
}