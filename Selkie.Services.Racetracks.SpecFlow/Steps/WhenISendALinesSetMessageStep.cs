using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Selkie.Services.Common.Dto;
using Selkie.Services.Racetracks.Common.Messages;
using Selkie.Services.Racetracks.SpecFlow.Steps.Common;
using TechTalk.SpecFlow;

namespace Selkie.Services.Racetracks.SpecFlow.Steps
{
    public class WhenISendALinesSetMessageStep : BaseStep
    {
        [When(@"I send a LinesSetMessage")]
        public override void Do()
        {
            IEnumerable <LineDto> dtos = CreateLineDtos();
            var linesSetMessage = new LinesSetMessage
                                  {
                                      LineDtos = dtos.ToArray()
                                  };

            Bus.PublishAsync(linesSetMessage);
        }

        [NotNull]
        private IEnumerable <LineDto> CreateLineDtos()
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