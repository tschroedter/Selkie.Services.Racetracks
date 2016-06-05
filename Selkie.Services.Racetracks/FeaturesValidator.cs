using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Selkie.Geometry.Surveying;
using Selkie.Services.Common.Dto;
using Selkie.Services.Racetracks.Interfaces;
using Selkie.Windsor;

namespace Selkie.Services.Racetracks
{
    [ProjectComponent(Lifestyle.Transient)]
    public class FeaturesValidator : IFeaturesValidator // todo testing
    {
        public bool ValidateDtos(IEnumerable <SurveyFeatureDto> dtos)
        {
            IEnumerable <int> array = dtos.Select(x => x.Id);

            return ValidateIds(array);
        }

        public bool ValidateFeatures(IEnumerable <ISurveyFeature> lines)
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