using System.Collections.Generic;
using JetBrains.Annotations;
using Selkie.Geometry.Shapes;
using Selkie.Services.Common.Dto;

namespace Selkie.Services.Racetracks.Interfaces
{
    public interface ILinesSourceManager
    {
        [NotNull]
        IEnumerable <ILine> Lines { get; }

        void SetLinesIfValid([NotNull] IEnumerable <LineDto> lineDtos);
    }
}