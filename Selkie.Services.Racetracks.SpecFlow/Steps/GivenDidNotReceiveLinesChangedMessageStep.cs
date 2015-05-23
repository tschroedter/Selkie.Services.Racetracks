using Selkie.Services.Racetracks.SpecFlow.Steps.Common;
using TechTalk.SpecFlow;

namespace Selkie.Services.Racetracks.SpecFlow.Steps
{
    public class GivenDidNotReceiveLinesChangedMessageStep : BaseStep
    {
        [Given(@"Did not receive LinesChangedMessage")]
        public override void Do()
        {
            ScenarioContext.Current [ "IsReceivedLinesChangedMessage" ] = false;
        }
    }
}