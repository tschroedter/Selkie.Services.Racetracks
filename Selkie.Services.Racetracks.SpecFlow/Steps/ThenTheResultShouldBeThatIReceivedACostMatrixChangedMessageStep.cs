using NUnit.Framework;
using Selkie.Services.Racetracks.Common.Messages;
using Selkie.Services.Racetracks.SpecFlow.Steps.Common;
using TechTalk.SpecFlow;

namespace Selkie.Services.Racetracks.SpecFlow.Steps
{
    public class ThenTheResultShouldBeThatIReceivedACostMatrixResponseMessageStep : BaseStep
    {
        [Then(@"the result should be that I received a CostMatrixResponseMessage")]
        public override void Do()
        {
            SleepWaitAndDo(() => GetBoolValueForScenarioContext("IsReceivedCostMatrixResponseMessage"),
                           SendMessages);

            Assert.True(GetBoolValueForScenarioContext("IsReceivedCostMatrixResponseMessage"),
                        "Did not receive CostMatrixResponseMessage!");
        }

        private void SendMessages()
        {
            Bus.PublishAsync(new CostMatrixCalculateMessage());
        }
    }
}