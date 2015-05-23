using Selkie.Services.Racetracks.Common.Messages;
using Selkie.Services.Racetracks.SpecFlow.Steps.Common;
using TechTalk.SpecFlow;

namespace Selkie.Services.Racetracks.SpecFlow.Steps
{
    public class WhenISendACostMatrixCalculateMessageStep : BaseStep
    {
        [When(@"I send a CostMatrixCalculateMessage")]
        public override void Do()
        {
            Bus.PublishAsync(new CostMatrixCalculateMessage());
        }
    }
}