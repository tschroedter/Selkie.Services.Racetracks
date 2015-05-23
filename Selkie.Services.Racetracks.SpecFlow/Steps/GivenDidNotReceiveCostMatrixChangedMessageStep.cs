using Selkie.Services.Racetracks.SpecFlow.Steps.Common;
using TechTalk.SpecFlow;

namespace Selkie.Services.Racetracks.SpecFlow.Steps
{
    public class GivenDidNotReceiveCostMatrixChangedMessageStep : BaseStep
    {
        [Given(@"Did not receive CostMatrixChangedMessage")]
        public override void Do()
        {
            ScenarioContext.Current [ "IsReceivedCostMatrixChangedMessage" ] = false;
        }
    }
}