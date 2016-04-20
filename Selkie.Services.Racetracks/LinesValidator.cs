using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Selkie.Geometry.Shapes;
using Selkie.Services.Common.Dto;
using Selkie.Services.Racetracks.Interfaces;
using Selkie.Windsor;

namespace Selkie.Services.Racetracks
{
    [ProjectComponent(Lifestyle.Transient)]
    public class LinesValidator : ILinesValidator
    {
        public bool ValidateDtos(IEnumerable <LineDto> lineDtos)
        {
            IEnumerable <int> array = lineDtos.Select(x => x.Id);

            return ValidateIds(array);
        }

        public bool ValidateLines(IEnumerable <ILine> lines)
        {
            IEnumerable <int> array = lines.Select(x => x.Id);

            return ValidateIds(array);
        }

        private static bool ValidateIds([NotNull] IEnumerable <int> ids)
        {
            int[] array = ids.ToArray();

            if ( array.Length <= 1 )
            {
                return false;
            }

            var expectedId = 0;

            return array.All(id => expectedId++ == id);
        }
    }
}