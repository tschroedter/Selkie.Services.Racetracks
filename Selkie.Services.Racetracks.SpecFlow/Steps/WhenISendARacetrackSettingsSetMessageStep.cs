using Selkie.Services.Racetracks.Common.Messages;
using Selkie.Services.Racetracks.SpecFlow.Steps.Common;
using TechTalk.SpecFlow;

namespace Selkie.Services.Racetracks.SpecFlow.Steps
{
    public class WhenISendARacetrackSettingsSetMessageStep : BaseStep
    {
        [When(@"I send a RacetrackSettingsSetMessage")]
        public override void Do()
        {
            var message = new RacetrackSettingsSetMessage
                          {
                              TurnRadiusForPort = 100.0,
                              TurnRadiusForStarboard = 200.0,
                              IsPortTurnAllowed = true,
                              IsStarboardTurnAllowed = true
                          };

            Bus.PublishAsync(message);
        }
    }
}