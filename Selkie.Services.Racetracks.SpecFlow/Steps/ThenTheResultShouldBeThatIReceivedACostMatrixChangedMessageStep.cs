using NUnit.Framework;
using Selkie.Services.Racetracks.Common.Messages;
using Selkie.Services.Racetracks.SpecFlow.Steps.Common;
using TechTalk.SpecFlow;

namespace Selkie.Services.Racetracks.SpecFlow.Steps
{
    public class ThenTheResultShouldBeThatIReceivedACostMatrixChangedMessageStep : BaseStep
    {
        [Then(@"the result should be that I received a CostMatrixChangedMessage")]
        public override void Do()
        {
            SleepWaitAndDo(() => ( bool ) ScenarioContext.Current [ "IsReceivedCostMatrixChangedMessage" ],
                           SendMessages);

            Assert.True(( bool ) ScenarioContext.Current [ "IsReceivedCostMatrixChangedMessage" ],
                        "Did not receive CostMatrixChangedMessage!");
        }

        private void SendMessages()
        {
            Bus.PublishAsync(new CostMatrixCalculateMessage());
        }
    }
}