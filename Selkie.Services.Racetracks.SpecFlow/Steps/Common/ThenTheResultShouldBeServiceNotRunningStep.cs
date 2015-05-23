using System.Diagnostics;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace Selkie.Services.Racetracks.SpecFlow.Steps.Common
{
    public class ThenTheResultShouldBeServiceNotRunningStep : BaseStep
    {
        [Then(@"the result should be service not running")]
        public override void Do()
        {
            SleepWaitAndDo(() => ( ( Process ) ScenarioContext.Current [ "ExeProcess" ] ).HasExited,
                           DoNothing);

            Assert.True(( ( Process ) ScenarioContext.Current [ "ExeProcess" ] ).HasExited,
                        "Process didn't exited!");
        }
    }
}