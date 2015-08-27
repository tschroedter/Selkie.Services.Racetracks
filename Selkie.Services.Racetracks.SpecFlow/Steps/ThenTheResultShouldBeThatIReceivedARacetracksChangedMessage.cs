using NUnit.Framework;
using Selkie.Services.Racetracks.SpecFlow.Steps.Common;
using TechTalk.SpecFlow;

namespace Selkie.Services.Racetracks.SpecFlow.Steps
{
    public class ThenTheResultShouldBeThatIReceivedARacetracksChangedMessage : BaseStep
    {
        [Then(@"the result should be that I received a RacetracksChangedMessage")]
        public override void Do()
        {
            SleepWaitAndDo(() => ( bool ) ScenarioContext.Current [ "IsReceivedRacetracksChangedMessage" ],
                           DoNothing);

            Assert.True(( bool ) ScenarioContext.Current [ "IsReceivedRacetracksChangedMessage" ],
                        "Did not receive RacetracksChangedMessage!");
        }
    }
}