using NUnit.Framework;
using Selkie.Services.Racetracks.SpecFlow.Steps.Common;
using TechTalk.SpecFlow;

namespace Selkie.Services.Racetracks.SpecFlow.Steps
{
    public class ThenTheResultShouldBeThatIReceivedARacetrackSettingsChangedMessageStep : BaseStep
    {
        [Then(@"the result should be that I received a RacetrackSettingsChangedMessage")]
        public override void Do()
        {
            SleepWaitAndDo(() => ( bool ) ScenarioContext.Current [ "IsReceivedRacetrackSettingsChangedMessage" ],
                           DoNothing);

            Assert.True(( bool ) ScenarioContext.Current [ "IsReceivedRacetrackSettingsChangedMessage" ],
                        "Did not receive RacetrackSettingsChangedMessage!");
        }
    }
}