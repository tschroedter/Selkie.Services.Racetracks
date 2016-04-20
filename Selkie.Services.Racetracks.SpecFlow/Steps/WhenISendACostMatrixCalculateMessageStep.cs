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
            LineDto[] lineDtos = CreateLineDtos();

            var message = new CostMatrixCalculateMessage
                          {
                              LineDtos = lineDtos,
                              TurnRadiusForPort = 100.0,
                              TurnRadiusForStarboard = 200.0,
                              IsPortTurnAllowed = true,
                              IsStarboardTurnAllowed = true
                          };

            Bus.PublishAsync(message);
        }

        [NotNull]
        private LineDto[] CreateLineDtos()
        {
            var lineOne = new LineDto
                          {
                              Id = 0,
                              RunDirection = "Forward",
                              IsUnknown = false,
                              X1 = 0.0,
                              Y1 = 0.0,
                              X2 = 0.0,
                              Y2 = 100.0
                          };

            var lineTwo = new LineDto
                          {
                              Id = 1,
                              RunDirection = "Forward",
                              IsUnknown = false,
                              X1 = 100.0,
                              Y1 = 0.0,
                              X2 = 100.0,
                              Y2 = 100.0
                          };

            LineDto[] dtos =
            {
                lineOne,
                lineTwo
            };

            return dtos;
        }
    }
}