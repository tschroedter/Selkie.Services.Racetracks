using System.Collections.Generic;
using JetBrains.Annotations;
using Selkie.Geometry.Surveying;
using Selkie.Services.Common.Dto;

namespace Selkie.Services.Racetracks.Interfaces
{
    public interface IFeaturesValidator
    {
        bool ValidateDtos([NotNull] IEnumerable <SurveyFeatureDto> dtos);
        bool ValidateFeatures([NotNull] IEnumerable <ISurveyFeature> lines);
    }
}