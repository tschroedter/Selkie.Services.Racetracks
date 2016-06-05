using System.Collections.Generic;
using JetBrains.Annotations;
using Selkie.Geometry.Surveying;
using Selkie.Racetrack.Interfaces;

namespace Selkie.Services.Racetracks.Interfaces
{
    public interface ICostMatrix
    {
        [NotNull]
        IEnumerable <ISurveyFeature> Features { get; }

        [NotNull]
        double[][] Matrix { get; }

        IRacetracks Racetracks { get; }
    }
}