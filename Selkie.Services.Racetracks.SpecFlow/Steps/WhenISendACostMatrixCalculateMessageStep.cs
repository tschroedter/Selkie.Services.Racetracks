using JetBrains.Annotations;
using Selkie.Services.Common.Dto;
using Selkie.Services.Racetracks.Common.Messages;
using Selkie.Services.Racetracks.SpecFlow.Steps.Common;
using TechTalk.SpecFlow;

namespace Selkie.Services.Racetracks.SpecFlow.Steps
{
    public class WhenISendACostMatrixCalculateMessageStep : BaseStep
    {
        [When(@"I send a CostMatrixCalculateMessage")]
        public override void Do()
        {
            SurveyFeatureDto[] lineDtos = CreateSurveyFeatureDtos();

            var message = new CostMatrixCalculateMessage
                          {
                              SurveyFeatureDtos = lineDtos,
                              TurnRadiusForPort = 100.0,
                              TurnRadiusForStarboard = 200.0,
                              IsPortTurnAllowed = true,
                              IsStarboardTurnAllowed = true
                          };

            Bus.PublishAsync(message);
        }

        [NotNull]
        private SurveyFeatureDto[] CreateSurveyFeatureDtos()
        {
            var dto = new SurveyFeatureDto
                      {
                          Id = 0,
                          RunDirection = "Forward",
                          IsUnknown = false,
                          StartPoint = new PointDto
                                       {
                                           X = 0.0,
                                           Y = 0.0
                                       },
                          EndPoint = new PointDto
                                     {
                                         X = 0.0,
                                         Y = 100.0
                                     },
                          AngleToXAxisAtStartPoint = 90.0,
                          AngleToXAxisAtEndPoint = 90.0,
                          Length = 100.0
                      };

            var two = new SurveyFeatureDto
                      {
                          Id = 1,
                          RunDirection = "Forward",
                          IsUnknown = false,
                          StartPoint = new PointDto
                                       {
                                           X = 100.0,
                                           Y = 0.0
                                       },
                          EndPoint = new PointDto
                                     {
                                         X = 100.0,
                                         Y = 100.0
                                     },
                          AngleToXAxisAtStartPoint = 270.0,
                          AngleToXAxisAtEndPoint = 270.0,
                          Length = 100.0
                      };

            SurveyFeatureDto[] dtos =
            {
                dto,
                two
            };

            return dtos;
        }
    }
}