using NUnit.Framework;
using TechTalk.SpecFlow;

namespace Selkie.Services.Racetracks.SpecFlow.Steps.Common
{
    public class ThenTheResultShouldBeThatIReceivedAServiceStoppedMessageStep : BaseStep
    {
        [Then(@"the result should be that I received a ServiceStoppedMessage")]
        public override void Do()
        {
            SleepWaitAndDo(() => ( bool ) ScenarioContext.Current [ "IsReceivedServiceStoppedResponse" ],
                           DoNothing);

            Assert.True(( bool ) ScenarioContext.Current [ "IsReceivedServiceStoppedResponse" ],
                        "Didn't receive service stopped response!");
        }
    }
}