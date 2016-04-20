using NUnit.Framework;
using Selkie.Services.Racetracks.SpecFlow.Steps.Common;
using TechTalk.SpecFlow;

namespace Selkie.Services.Racetracks.SpecFlow.Steps
{
    public class ThenTheResultShouldBeThatIReceivedARacetracksResponseMessage : BaseStep
    {
        [Then(@"the result should be that I received a RacetracksResponseMessage")]
        public override void Do()
        {
            SleepWaitAndDo(() => ( bool ) ScenarioContext.Current [ "IsReceivedRacetracksResponseMessage" ],
                           DoNothing);

            Assert.True(( bool ) ScenarioContext.Current [ "IsReceivedRacetracksResponseMessage" ],
                        "Did not receive RacetracksResponseMessage!");
        }
    }
}