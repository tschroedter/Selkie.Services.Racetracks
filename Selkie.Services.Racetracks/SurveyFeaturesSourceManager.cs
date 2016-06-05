using System.Collections.Generic;
using System.Linq;
using Castle.Core;
using JetBrains.Annotations;
using Selkie.Aop.Aspects;
using Selkie.Geometry.Surveying;
using Selkie.Services.Common.Dto;
using Selkie.Services.Racetracks.Interfaces;
using Selkie.Windsor;
using Selkie.Windsor.Extensions;

namespace Selkie.Services.Racetracks
{
    [Interceptor(typeof( MessageHandlerAspect ))]
    [ProjectComponent(Lifestyle.Singleton)]
    public class SurveyFeaturesSourceManager
        : ISurveyFeaturesSourceManager,
          IStartable
    {
        public SurveyFeaturesSourceManager([NotNull] ISelkieLogger logger,
                                           [NotNull] IFeaturesValidator validator)
        {
            m_Logger = logger;
            m_Validator = validator;
            Features = new ISurveyFeature[0];
        }

        private readonly ISelkieLogger m_Logger;
        private readonly IFeaturesValidator m_Validator;

        public void Start()
        {
            m_Logger.Info("Started '{0}'!".Inject(GetType().FullName));
        }

        public void Stop()
        {
            m_Logger.Info("Stopped '{0}'!".Inject(GetType().FullName));
        }

        public IEnumerable <ISurveyFeature> Features { get; private set; }

        public void SetSurveyFeaturesIfValid(IEnumerable <SurveyFeatureDto> surveyFeatureDtos)
        {
            surveyFeatureDtos = GetAndValidateSurveyFeatureDtos(surveyFeatureDtos);

            UpdateSurveyFeaturesIfValid(surveyFeatureDtos);
        }

        [NotNull]
        private IEnumerable <SurveyFeatureDto> GetAndValidateSurveyFeatureDtos(
            [CanBeNull] IEnumerable <SurveyFeatureDto> dtos)
        {
            if ( dtos != null )
            {
                return dtos;
            }

            m_Logger.Error("Received SurveyFeatureDto are null!");
            dtos = new SurveyFeatureDto[0];
            return dtos;
        }

        private void UpdateSurveyFeaturesIfValid([NotNull] IEnumerable <SurveyFeatureDto> dtos)
        {
            SurveyFeatureDto[] arrayDtos = dtos.ToArray();

            if ( !m_Validator.ValidateDtos(arrayDtos) )
            {
                return;
            }

            IEnumerable <ISurveyFeature> features =
                arrayDtos.Select(SurveyFeatureToSurveyFeatureDtoConverter.ConvertToSurveyFeature);

            Features = features.ToArray();
        }
    }
}