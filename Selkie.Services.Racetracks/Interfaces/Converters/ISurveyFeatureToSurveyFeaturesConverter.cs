using System.Collections.Generic;
using JetBrains.Annotations;
using Selkie.Geometry.Surveying;
using Selkie.Racetrack.Interfaces;

namespace Selkie.Services.Racetracks.Interfaces.Converters
{
    public interface ISurveyFeatureToSurveyFeaturesConverter : IConverter
    {
        [NotNull]
        ISurveyFeature Feature { get; set; }

        [NotNull]
        IEnumerable <ISurveyFeature> Features { get; set; }

        double BaseCost { get; }

        [NotNull]
        IRacetracks Racetracks { get; set; }

        double CostEndToEnd([NotNull] ISurveyFeature other);
        double CostEndToStart([NotNull] ISurveyFeature other);

        double CostForwardForward([NotNull] ISurveyFeature other);
        double CostForwardReverse([NotNull] ISurveyFeature other);
        double CostReverseForward([NotNull] ISurveyFeature to);
        double CostReverseReverse([NotNull] ISurveyFeature to);
        double CostStartToEnd([NotNull] ISurveyFeature to);
        double CostStartToStart([NotNull] ISurveyFeature other);
    }
}