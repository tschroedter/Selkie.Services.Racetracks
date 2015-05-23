using System.Collections.Generic;
using JetBrains.Annotations;
using Selkie.Geometry.Shapes;

namespace Selkie.Services.Racetracks
{
    public interface ILinesSourceManager
    {
        [NotNull]
        IEnumerable <ILine> Lines { get; }
    }
}