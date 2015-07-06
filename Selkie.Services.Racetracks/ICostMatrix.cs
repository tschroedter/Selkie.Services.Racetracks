using System.Collections.Generic;
using JetBrains.Annotations;
using Selkie.Geometry.Shapes;
using Selkie.Racetrack;

namespace Selkie.Services.Racetracks
{
    public interface ICostMatrix
    {
        [NotNull]
        IEnumerable <ILine> Lines { get; }

        [NotNull]
        double[][] Matrix { get; }

        IRacetracks Racetracks // todo testing
        { get; }
    }
}