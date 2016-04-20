using System.Collections.Generic;
using JetBrains.Annotations;
using Selkie.Geometry.Shapes;
using Selkie.Racetrack.Interfaces;

namespace Selkie.Services.Racetracks.Interfaces
{
    public interface ICostMatrix
    {
        [NotNull]
        IEnumerable <ILine> Lines { get; }

        [NotNull]
        double[][] Matrix { get; }

        IRacetracks Racetracks { get; }
    }
}