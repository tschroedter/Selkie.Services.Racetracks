using Selkie.Services.Racetracks.SpecFlow.Steps.Common;
using TechTalk.SpecFlow;

namespace Selkie.Services.Racetracks.SpecFlow.Steps
{
    public class GivenDidNotReceiveCostMatrixResponseMessageStep : BaseStep
    {
        [Given(@"Did not receive CostMatrixResponseMessage")]
        public override void Do()
        {
            ScenarioContext.Current [ "IsReceivedCostMatrixResponseMessage" ] = false;
        }
    }
}