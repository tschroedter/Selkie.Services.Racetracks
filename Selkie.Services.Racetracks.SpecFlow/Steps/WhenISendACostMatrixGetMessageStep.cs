using Selkie.Services.Racetracks.Common.Messages;
using Selkie.Services.Racetracks.SpecFlow.Steps.Common;
using TechTalk.SpecFlow;

namespace Selkie.Services.Racetracks.SpecFlow.Steps
{
    public class WhenISendACostMatrixRequestMessageStep : BaseStep
    {
        [When(@"I send a CostMatrixRequestMessage")]
        public override void Do()
        {
            Bus.PublishAsync(new CostMatrixRequestMessage());
        }
    }
}