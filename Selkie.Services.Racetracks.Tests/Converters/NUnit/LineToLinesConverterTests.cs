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
    // ReSharper disable once ClassTooBig
    [TestFixture]
    [ExcludeFromCodeCoverage]
    internal sealed class LineToLinesConverterTests
    {
        [SetUp]
        public void Setup()
        {
            ComfigureLines();
            ConfigureRacetracks();

            m_Logger = Substitute.For <ISelkieLogger>();

            m_Converter = new LineToLinesConverter(new CostStartToStartCalculator(m_Logger),
                                                   new CostStartToEndCalculator(m_Logger),
                                                   new CostEndToStartCalculator(m_Logger),
                                                   new CostEndToEndCalculator(m_Logger))
                          {
                              Racetracks = m_Racetracks,
                              Line = m_Line1,
                              Lines = m_Lines1
                          };

            m_Converter.Convert();
        }

        private LineToLinesConverter m_Converter;
        private IPath[][] m_ForwardForwardPaths;
        private IPath[][] m_ForwardReversePaths;
        private Line m_Line1;
        private Line m_Line2;
        private Line[] m_Lines1;
        private Line[] m_Lines2;
        private IRacetracks m_Racetracks;
        private IPath[][] m_ReverseForwardPaths;
        private IPath[][] m_ReverseReversePaths;
        private ISelkieLogger m_Logger;

        private void ComfigureLines()
        {
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
            m_Lines2 = new[]
                       {
                           m_Line2,
                           m_Line1
                       };
        }

        private void ConfigureRacetracks()
        {
            m_ForwardForwardPaths = CreateForwardForwardPaths();
            m_ForwardReversePaths = CreateForwardReversePaths();
            m_ReverseForwardPaths = CreateReverseForwardPaths();
            m_ReverseReversePaths = CreateReverseReversePaths();

            m_Racetracks = Substitute.For <IRacetracks>();
            m_Racetracks.ForwardToForward.Returns(m_ForwardForwardPaths);
            m_Racetracks.ForwardToReverse.Returns(m_ForwardReversePaths);
            m_Racetracks.ReverseToForward.Returns(m_ReverseForwardPaths);
            m_Racetracks.ReverseToReverse.Returns(m_ReverseReversePaths);
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

        [Test]
        public void BaseCostDefaultTest()
        {
            m_Converter.Line = m_Line1;

            Assert.AreEqual(m_Line1.Length,
                            m_Converter.BaseCost);
        }

        [Test]
        public void CalculateTotalCostReturnsMaxValueForCostToMySelfTest()
        {
            const double expected = double.MaxValue;
            double actual = m_Converter.CalculateTotalCost(10.0,
                                                           CostMatrix.CostToMyself);

            Assert.AreEqual(expected,
                            actual);
        }

        [Test]
        public void CalculateTotalCostReturnsMaxValueForLessZeroTest()
        {
            const double expected = double.MaxValue;
            double actual = m_Converter.CalculateTotalCost(10.0,
                                                           -10.0);

            Assert.AreEqual(expected,
                            actual);
        }

        [Test]
        public void CalculateTotalCostReturnsMaxValueForZeroTest()
        {
            const double expected = double.MaxValue;
            double actual = m_Converter.CalculateTotalCost(10.0,
                                                           0.0);

            Assert.AreEqual(expected,
                            actual);
        }

        [Test]
        public void CalculateTotalCostReturnsValueForGreaterZeroTest()
        {
            const double expected = 30.0;
            double actual = m_Converter.CalculateTotalCost(10.0,
                                                           20.0);

            Assert.AreEqual(expected,
                            actual);
        }

        [Test]
        public void CostEndToEndForLine1Test()
        {
            NUnitHelper.AssertIsEquivalent(double.MaxValue,
                                           m_Converter.CostEndToEnd(m_Line1),
                                           Constants.EpsilonDistance,
                                           "actual[1]");
        }

        [Test]
        public void CostEndToEndForLine2Test()
        {
            NUnitHelper.AssertIsEquivalent(70.0,
                                           m_Converter.CostEndToEnd(m_Line2),
                                           Constants.EpsilonDistance,
                                           "actual[1]");
        }

        [Test]
        public void CostEndToStartdForLine1Test()
        {
            NUnitHelper.AssertIsEquivalent(double.MaxValue,
                                           m_Converter.CostEndToStart(m_Line1),
                                           Constants.EpsilonDistance,
                                           "actual[1]");
        }

        [Test]
        public void CostEndToStartForLine2Test()
        {
            NUnitHelper.AssertIsEquivalent(30.0,
                                           m_Converter.CostEndToStart(m_Line2),
                                           Constants.EpsilonDistance,
                                           "actual[1]");
        }

        [Test]
        public void CostForwardForwardForLine1Test()
        {
            NUnitHelper.AssertIsEquivalent(double.MaxValue,
                                           m_Converter.CostForwardForward(m_Line1),
                                           Constants.EpsilonDistance,
                                           "actual[1]");
        }

        [Test]
        public void CostForwardForwardForLine2Test()
        {
            NUnitHelper.AssertIsEquivalent(30.0,
                                           m_Converter.CostForwardForward(m_Line2),
                                           Constants.EpsilonDistance,
                                           "actual[1]");
        }

        [Test]
        public void CostForwardReverseForLine1Test()
        {
            NUnitHelper.AssertIsEquivalent(double.MaxValue,
                                           m_Converter.CostForwardReverse(m_Line1),
                                           Constants.EpsilonDistance,
                                           "actual[1]");
        }

        [Test]
        public void CostForwardReverseForLine2Test()
        {
            NUnitHelper.AssertIsEquivalent(70.0,
                                           m_Converter.CostForwardReverse(m_Line2),
                                           Constants.EpsilonDistance,
                                           "actual[1]");
        }

        [Test]
        public void CostReverseForwardForLine1Test()
        {
            NUnitHelper.AssertIsEquivalent(double.MaxValue,
                                           m_Converter.CostReverseForward(m_Line1),
                                           Constants.EpsilonDistance,
                                           "actual[1]");
        }

        [Test]
        public void CostReverseForwardForLine2Test()
        {
            NUnitHelper.AssertIsEquivalent(110.0,
                                           m_Converter.CostReverseForward(m_Line2),
                                           Constants.EpsilonDistance,
                                           "actual[1]");
        }

        [Test]
        public void CostReverseReverseForLine1Test()
        {
            NUnitHelper.AssertIsEquivalent(double.MaxValue,
                                           m_Converter.CostReverseReverse(m_Line1),
                                           Constants.EpsilonDistance,
                                           "actual[1]");
        }

        [Test]
        public void CostReverseReverseForLine2Test()
        {
            NUnitHelper.AssertIsEquivalent(150.0,
                                           m_Converter.CostReverseReverse(m_Line2),
                                           Constants.EpsilonDistance,
                                           "actual[1]");
        }

        [Test]
        public void CostStartToEndForLine1Test()
        {
            NUnitHelper.AssertIsEquivalent(double.MaxValue,
                                           m_Converter.CostStartToEnd(m_Line1),
                                           Constants.EpsilonDistance,
                                           "actual[1]");
        }

        [Test]
        public void CostStartToEndForLine2Test()
        {
            NUnitHelper.AssertIsEquivalent(150.0,
                                           m_Converter.CostStartToEnd(m_Line2),
                                           Constants.EpsilonDistance,
                                           "actual[1]");
        }

        [Test]
        public void CostStartToEndReturnsTotalCostTest()
        {
            const double expected = 100.0;
            double actual = m_Converter.CostStartToEnd(m_Line1);

            Assert.False(NUnitHelper.IsEquivalent(expected,
                                                  actual));
        }

        [Test]
        public void CostStartToStartForLine1Test()
        {
            NUnitHelper.AssertIsEquivalent(double.MaxValue,
                                           m_Converter.CostStartToStart(m_Line1),
                                           Constants.EpsilonDistance,
                                           "actual[1]");
        }

        [Test]
        public void CostStartToStartForLine2Test()
        {
            NUnitHelper.AssertIsEquivalent(110.0,
                                           m_Converter.CostStartToStart(m_Line2),
                                           Constants.EpsilonDistance,
                                           "actual[1]");
        }

        [Test]
        public void IsCostToMySelfReturnsFalseForPositiveTest()
        {
            Assert.False(m_Converter.IsCostToMySelf(100.0));
        }

        [Test]
        public void IsCostToMySelfReturnsTrueForCostToMySelfTest()
        {
            Assert.True(m_Converter.IsCostToMySelf(CostMatrix.CostToMyself));
        }

        [Test]
        public void IsCostToMySelfReturnsTrueForNegativeTest()
        {
            Assert.False(m_Converter.IsCostToMySelf(-100.0));
        }

        [Test]
        public void IsCostValidReturnsFalseForCostToMySelfTest()
        {
            Assert.False(m_Converter.IsCostValid(CostMatrix.CostToMyself));
        }

        [Test]
        public void IsCostValidReturnsFalseForLessZeroTest()
        {
            Assert.False(m_Converter.IsCostValid(-1.0));
        }

        [Test]
        public void IsCostValidReturnsTrueForGreaterZeroTest()
        {
            Assert.True(m_Converter.IsCostValid(1.0));
        }

        [Test]
        public void LineRoundtripTest()
        {
            m_Converter.Line = m_Line1;
            Assert.AreEqual(m_Line1,
                            m_Converter.Line);

            m_Converter.Line = m_Line2;
            Assert.AreEqual(m_Line2,
                            m_Converter.Line);
        }

        [Test]
        public void LinesRoundtripTest()
        {
            m_Converter.Lines = m_Lines1;
            Assert.AreEqual(m_Lines1,
                            m_Converter.Lines);

            m_Converter.Lines = m_Lines2;
            Assert.AreEqual(m_Lines2,
                            m_Converter.Lines);
        }

        [Test]
        public void RacetracksDefaultTest()
        {
            var converter = new LineToLinesConverter(new CostStartToStartCalculator(m_Logger),
                                                     new CostStartToEndCalculator(m_Logger),
                                                     new CostEndToStartCalculator(m_Logger),
                                                     new CostEndToEndCalculator(m_Logger));

            Assert.True(Racetracks.Converters.Dtos.Racetracks.Unknown == converter.Racetracks);
        }
    }
}