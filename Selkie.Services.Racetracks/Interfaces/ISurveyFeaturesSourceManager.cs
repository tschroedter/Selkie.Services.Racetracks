using System.Collections.Generic;
using JetBrains.Annotations;
using Selkie.Geometry.Surveying;
using Selkie.Services.Common.Dto;

namespace Selkie.Services.Racetracks.Interfaces
{
    public interface ISurveyFeaturesSourceManager
    {
        [NotNull]
        IEnumerable <ISurveyFeature> Features { get; }

        void SetSurveyFeaturesIfValid([NotNull] IEnumerable <SurveyFeatureDto> surveyFeatureDtos);
    }
}