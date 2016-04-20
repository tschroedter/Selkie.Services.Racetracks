using NUnit.Framework;
using Selkie.Services.Racetracks.SpecFlow.Steps.Common;
using TechTalk.SpecFlow;

namespace Selkie.Services.Racetracks.SpecFlow.Steps
{
    public class ThenTheCostMatrixResponseMessageContainsTheRacetracksStep : BaseStep
    {
        [Then(@"the CostMatrixResponseMessage contains the racetracks")]
        public override void Do()
        {
            SleepWaitAndDo(IsReceived,
                           DoNothing);

            var actual = ( double[][] ) ScenarioContext.Current [ "Matrix" ];

            Assert.AreEqual(4,
                            actual.GetLength(0),
                            "actual.GetLength(0)");
            Assert.AreEqual(4,
                            actual [ 0 ].Length,
                            "actual[0].Length");
            Assert.AreEqual(4,
                            actual [ 1 ].Length,
                            "actual[1].Length");
            Assert.AreEqual(4,
                            actual [ 2 ].Length,
                            "actual[2].Length");
            Assert.AreEqual(4,
                            actual [ 3 ].Length,
                            "actual[3].Length");
        }

        private static bool IsReceived()
        {
            var isReceived = ( bool ) ScenarioContext.Current [ "IsReceivedCostMatrixResponseMessage" ];
            var matrix = ( double[][] ) ScenarioContext.Current [ "Matrix" ];

            bool received = isReceived && matrix.GetLength(0) == 4;

            return received;
        }
    }
}