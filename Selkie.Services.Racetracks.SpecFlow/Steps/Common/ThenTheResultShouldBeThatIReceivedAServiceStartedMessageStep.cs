using NUnit.Framework;
using TechTalk.SpecFlow;

namespace Selkie.Services.Racetracks.SpecFlow.Steps.Common
{
    public class ThenTheResultShouldBeThatIReceivedAServiceStartedMessageStep : BaseStep
    {
        [Then(@"the result should be that I received a ServiceStartedMessage")]
        public override void Do()
        {
            SleepWaitAndDo(() => ( bool ) ScenarioContext.Current [ "IsReceivedServiceStartedResponse" ],
                           DoNothing);

            Assert.True(( bool ) ScenarioContext.Current [ "IsReceivedServiceStartedResponse" ],
                        "Didn't receive service started response!");
        }
    }
}