using Selkie.Services.Racetracks.Common.Messages;
using Selkie.Services.Racetracks.SpecFlow.Steps.Common;
using TechTalk.SpecFlow;

namespace Selkie.Services.Racetracks.SpecFlow.Steps
{
    public class WhenISendACostMatrixGetMessageStep : BaseStep
    {
        [When(@"I send a CostMatrixGetMessage")]
        public override void Do()
        {
            Bus.PublishAsync(new CostMatrixGetMessage());
        }
    }
}