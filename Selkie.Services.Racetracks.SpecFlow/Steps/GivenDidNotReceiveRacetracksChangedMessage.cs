using Selkie.Services.Racetracks.SpecFlow.Steps.Common;
using TechTalk.SpecFlow;

namespace Selkie.Services.Racetracks.SpecFlow.Steps
{
    public class GivenDidNotReceiveRacetracksResponseMessage : BaseStep
    {
        [Given(@"Did not receive RacetracksResponseMessage")]
        public override void Do()
        {
            ScenarioContext.Current [ "IsReceivedRacetracksResponseMessage" ] = false;
        }
    }
}