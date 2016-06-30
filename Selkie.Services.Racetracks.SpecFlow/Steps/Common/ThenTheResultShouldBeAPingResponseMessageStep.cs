using NUnit.Framework;
using TechTalk.SpecFlow;

namespace Selkie.Services.Racetracks.SpecFlow.Steps.Common
{
    public class ThenTheResultShouldBeAPingResponseMessageStep : BaseStep
    {
        [Then(@"the result should be a ping response message")]
        public override void Do()
        {
            SleepWaitAndDo(() => GetBoolValueForScenarioContext("IsReceivedPingResponse"),
                           DoNothing);

            Assert.True(GetBoolValueForScenarioContext("IsReceivedPingResponse"),
                        "Didn't receive ping response");
        }
    }
}