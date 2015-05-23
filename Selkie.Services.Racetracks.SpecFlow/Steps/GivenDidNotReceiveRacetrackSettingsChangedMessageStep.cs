using Selkie.Services.Racetracks.SpecFlow.Steps.Common;
using TechTalk.SpecFlow;

namespace Selkie.Services.Racetracks.SpecFlow.Steps
{
    public class GivenDidNotReceiveRacetrackSettingsChangedMessageStep : BaseStep
    {
        [Given(@"Did not receive RacetrackSettingsChangedMessage")]
        public override void Do()
        {
            ScenarioContext.Current [ "IsReceivedRacetrackSettingsChangedMessage" ] = false;
        }
    }
}