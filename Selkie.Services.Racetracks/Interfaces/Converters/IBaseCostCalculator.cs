using System.Collections.Generic;
using JetBrains.Annotations;
using Selkie.Geometry.Surveying;
using Selkie.Racetrack.Interfaces;
using Selkie.Racetrack.Interfaces.Calculators;

namespace Selkie.Services.Racetracks.Interfaces.Converters
{
    public interface IBaseCostCalculator : ICalculator
    {
        [NotNull]
        Dictionary <int, double> Costs { get; }

        [NotNull]
        ISurveyFeature Feature { get; set; }

        [NotNull]
        IEnumerable <ISurveyFeature> Features { get; set; }

        [NotNull]
        IRacetracks Racetracks { get; set; }
    }
}