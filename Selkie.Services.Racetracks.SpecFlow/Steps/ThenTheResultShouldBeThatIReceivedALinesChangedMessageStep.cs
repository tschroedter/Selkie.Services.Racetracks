using NUnit.Framework;
using Selkie.Services.Racetracks.SpecFlow.Steps.Common;
using TechTalk.SpecFlow;

namespace Selkie.Services.Racetracks.SpecFlow.Steps
{
    public class ThenTheResultShouldBeThatIReceivedALinesChangedMessageStep : BaseStep
    {
        [Then(@"the result should be that I received a LinesChangedMessage")]
        public override void Do()
        {
            SleepWaitAndDo(() => ( bool ) ScenarioContext.Current [ "IsReceivedLinesChangedMessage" ],
                           DoNothing);

            Assert.True(( bool ) ScenarioContext.Current [ "IsReceivedLinesChangedMessage" ],
                        "Did not receive LinesChangedMessage!");
        }
    }
}