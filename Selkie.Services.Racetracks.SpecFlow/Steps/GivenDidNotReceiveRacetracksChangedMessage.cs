using Selkie.Services.Racetracks.SpecFlow.Steps.Common;
using TechTalk.SpecFlow;

namespace Selkie.Services.Racetracks.SpecFlow.Steps
{
    public class GivenDidNotReceiveRacetracksChangedMessage : BaseStep
    {
        [Given(@"Did not receive RacetracksChangedMessage")]
        public override void Do()
        {
            ScenarioContext.Current [ "IsReceivedRacetracksChangedMessage" ] = false;
        }
    }
}