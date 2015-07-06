using Selkie.Services.Racetracks.Common.Messages;
using Selkie.Services.Racetracks.SpecFlow.Steps.Common;
using TechTalk.SpecFlow;

namespace Selkie.Services.Racetracks.SpecFlow.Steps
{
    public class WhenISendARacetracksGetMessage : BaseStep
    {
        [When(@"I send a RacetracksGetMessage")]
        public override void Do()
        {
            Bus.PublishAsync(new RacetracksGetMessage());
        }
    }
}