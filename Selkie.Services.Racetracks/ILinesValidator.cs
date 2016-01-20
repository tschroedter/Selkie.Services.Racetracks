using System.Collections.Generic;
using JetBrains.Annotations;
using Selkie.Geometry.Shapes;
using Selkie.Services.Common.Dto;

namespace Selkie.Services.Racetracks
{
    public interface ILinesValidator
    {
        bool ValidateDtos([NotNull] IEnumerable <LineDto> lineDtos);
        bool ValidateLines([NotNull] IEnumerable <ILine> lines);
    }
}