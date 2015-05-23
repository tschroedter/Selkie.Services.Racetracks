using NUnit.Framework;
using TechTalk.SpecFlow;

namespace Selkie.Services.Racetracks.SpecFlow.Steps.Common
{
    public class ThenTheResultShouldBeAPingResponseMessageStep : BaseStep
    {
        [Then(@"the result should be a ping response message")]
        public override void Do()
        {
            SleepWaitAndDo(() => ( bool ) ScenarioContext.Current [ "IsReceivedPingResponse" ],
                           DoNothing);

            Assert.True(( bool ) ScenarioContext.Current [ "IsReceivedPingResponse" ],
                        "Didn't receive ping response");
        }
    }
}