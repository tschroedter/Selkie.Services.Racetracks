using System.Collections.Generic;
using JetBrains.Annotations;
using Selkie.Geometry.Shapes;

namespace Selkie.Services.Racetracks
{
    public interface ICostMatrix
    {
        [NotNull]
        IEnumerable <ILine> Lines { get; }

        [NotNull]
        double[][] Matrix { get; }
    }
}