using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;
using NSubstitute;
using NUnit.Framework;
using Selkie.Geometry.Primitives;
using Selkie.Geometry.Shapes;
using Selkie.NUnit.Extensions;
using Selkie.Racetrack;
using Selkie.Services.Racetracks.Converters;
using Selkie.Windsor;
using Constants = Selkie.Common.Constants;

namespace Selkie.Services.Racetracks.Tests.Converters.NUnit
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    //ncrunch: no coverage start
    internal sealed class BaseCostCalculatorTests
    {
        [SetUp]
        public void Setup()
        {
            var path = Substitute.For <IPath>();
            path.Distance.Returns(new Distance(100.0));

            m_Line1 = new Line(0,
                               10.0,
                               10.0,
                               20.0,
                               10.0);
            m_Line2 = new Line(1,
                               10.0,
                               20.0,
                               20.0,
                               20.0);

            m_Lines1 = new[]
                       {
                           m_Line1,
                           m_Line2
                       };

            CreatePaths();

            CreateRacetracks();

            m_Logger = Substitute.For <ISelkieLogger>();

            m_Calculator = new TestBaseCostCalculator(m_Logger);
        }

        private TestBaseCostCalculator m_Calculator;
        private IPath[][] m_ForwardForwardPaths;
        private IPath[][] m_ForwardReversePaths;
        private Line m_Line1;
        private Line m_Line2;
        private Line[] m_Lines1;
        private IRacetracks m_Racetracks;
        private IPath[][] m_ReverseForwardPaths;
        private IPath[][] m_ReverseReversePaths;
        private ISelkieLogger m_Logger;

        private void CreateRacetracks()
        {
            m_Racetracks = Substitute.For <IRacetracks>();
            m_Racetracks.ForwardToForward.Returns(m_ForwardForwardPaths);
            m_Racetracks.ForwardToReverse.Returns(m_ForwardReversePaths);
            m_Racetracks.ReverseToForward.Returns(m_ReverseForwardPaths);
            m_Racetracks.ReverseToReverse.Returns(m_ReverseReversePaths);
        }

        private void CreatePaths()
        {
            m_ForwardForwardPaths = CreateForwardForwardPaths();
            m_ForwardReversePaths = CreateForwardReversePaths();
            m_ReverseForwardPaths = CreateReverseForwardPaths();
            m_ReverseReversePaths = CreateReverseReversePaths();
        }

        [NotNull]
        private static IPath[][] CreateForwardForwardPaths()
        {
            return CreatePaths(new[]
                               {
                                   10.0,
                                   20.0,
                                   30.0,
                                   40.0
                               });
        }

        [NotNull]
        private static IPath[][] CreateForwardReversePaths()
        {
            return CreatePaths(new[]
                               {
                                   50.0,
                                   60.0,
                                   70.0,
                                   80.0
                               });
        }

        [NotNull]
        private static IPath[][] CreateReverseForwardPaths()
        {
            return CreatePaths(new[]
                               {
                                   90.0,
                                   100.0,
                                   110.0,
                                   120.0
                               });
        }

        [NotNull]
        private static IPath[][] CreateReverseReversePaths()
        {
            return CreatePaths(new[]
                               {
                                   130.0,
                                   140.0,
                                   150.0,
                                   160.0
                               });
        }

        [NotNull]
        private static IPath[][] CreatePaths([NotNull] double[] distances)
        {
            var distance0 = new Distance(distances [ 0 ]);
            var distance1 = new Distance(distances [ 1 ]);

            var path1 = Substitute.For <IPath>();
            path1.Distance.Returns(distance0);
            var path2 = Substitute.For <IPath>();
            path2.Distance.Returns(distance1);

            IPath[][] paths =
            {
                new[]
                {
                    path1,
                    path2
                },
                new[]
                {
                    path1,
                    path2
                }
            };

            return paths;
        }

        private class TestBaseCostCalculator : BaseCostCalculator
        {
            public TestBaseCostCalculator([NotNull] ISelkieLogger logger)
                : base(logger)
            {
            }

            internal override double CalculateRacetrackCost(int fromLineId,
                                                            int toLineId)
            {
                return 100.0;
            }
        }

        [Test]
        public void CalculateRacetrackCostShouldLogsMessageForFromLineIdToBigIdTest()
        {
            m_Racetracks.ForwardToReverse.Returns(m_ForwardForwardPaths);

            m_Calculator.CheckAndCalculate(2,
                                           1);

            m_Logger.Received().Warn(Arg.Any <string>());
        }

        [Test]
        public void CalculateRacetrackCostShouldLogsMessageForNegativeFromLineIdTest()
        {
            m_Racetracks.ForwardToReverse.Returns(m_ForwardForwardPaths);

            m_Calculator.CheckAndCalculate(-1,
                                           1);

            m_Logger.Received().Warn(Arg.Any <string>());
        }

        [Test]
        public void CalculateRacetrackCostShouldLogsMessageForNegativeToLineIdTest()
        {
            m_Racetracks.ForwardToReverse.Returns(m_ForwardForwardPaths);

            m_Calculator.CheckAndCalculate(0,
                                           -1);

            m_Logger.Received().Warn(Arg.Any <string>());
        }

        [Test]
        public void CalculateRacetrackCostShouldLogsMessageForPathIsEmptyTest()
        {
            m_Racetracks.ForwardToReverse.Returns(new IPath[0][]);

            m_Calculator.CheckAndCalculate(0,
                                           2);

            m_Logger.Received().Warn(Arg.Any <string>());
        }

        [Test]
        public void CalculateRacetrackCostShouldLogsMessageForToLineIdToBigIdTest()
        {
            m_Racetracks.ForwardToForward.Returns(m_ForwardForwardPaths);
            m_Calculator.Racetracks = m_Racetracks;

            m_Calculator.CheckAndCalculate(1,
                                           2);

            m_Logger.Received().Warn(Arg.Any <string>());
        }

        [Test]
        public void CalculateRacetrackCostShouldLogsMesssageForToLineIdToBigIdTest()
        {
            m_Racetracks.ForwardToReverse.Returns(m_ForwardForwardPaths);

            m_Calculator.CheckAndCalculate(0,
                                           2);

            m_Logger.Received().Warn(Arg.Any <string>());
        }

        [Test]
        public void CalculateTest()
        {
            m_Calculator.Lines = m_Lines1;
            m_Calculator.Line = m_Line1;
            m_Calculator.Racetracks = m_Racetracks;
            m_Calculator.Calculate();

            Dictionary <int, double> actual = m_Calculator.Costs;

            Assert.AreEqual(2,
                            actual.Keys.Count,
                            "Count");
            Assert.AreEqual(CostMatrix.CostToMyself,
                            actual [ 0 ]);
            NUnitHelper.AssertIsEquivalent(100.0,
                                           actual [ 1 ],
                                           Constants.EpsilonDistance,
                                           "actual[1]");
        }

        [Test]
        public void DefaultCostsTest()
        {
            Assert.NotNull(m_Calculator.Costs);
        }

        [Test]
        public void DefaultLinesTest()
        {
            Assert.NotNull(m_Calculator.Lines);
        }

        [Test]
        public void DefaultLineTest()
        {
            Assert.NotNull(m_Calculator.Line,
                           "Line");
            Assert.True(m_Calculator.Line.IsUnknown,
                        "IsUnknown");
        }

        [Test]
        public void DefaultRacetracksest()
        {
            Assert.True(m_Calculator.Racetracks == Racetracks.Converters.Dtos.Racetracks.Unknown);
        }

        [Test]
        public void RoundtripLinesTest()
        {
            var line = Substitute.For <ILine>();
            ILine[] lines =
            {
                line
            };

            m_Calculator.Lines = lines;

            Assert.AreEqual(lines,
                            m_Calculator.Lines);
        }

        [Test]
        public void RoundtripLineTest()
        {
            var line = Substitute.For <ILine>();

            m_Calculator.Line = line;

            Assert.AreEqual(line,
                            m_Calculator.Line);
        }

        [Test]
        public void RoundtripRacetracksTest()
        {
            var racetracks = Substitute.For <IRacetracks>();

            m_Calculator.Racetracks = racetracks;

            Assert.AreEqual(racetracks,
                            m_Calculator.Racetracks);
        }
    }
}