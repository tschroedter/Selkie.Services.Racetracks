using System.Collections.Generic;
using JetBrains.Annotations;
using Selkie.Geometry.Shapes;

namespace Selkie.Services.Racetracks.Interfaces
{
    public interface ILinesSource
    {
        [NotNull]
        IEnumerable <ILine> Lines { get; }
    }
}