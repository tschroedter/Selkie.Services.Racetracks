using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;
using NSubstitute;
using Selkie.Geometry;
using Selkie.Geometry.Primitives;
using Selkie.Geometry.Shapes;
using Selkie.Geometry.Surveying;
using Selkie.Racetrack.Interfaces;
using Selkie.Services.Racetracks.Converters;
using Selkie.Windsor;
using Selkie.XUnit.Extensions;
using Xunit;

namespace Selkie.Services.Racetracks.Tests.Converters
{
    [ExcludeFromCodeCoverage]
    public sealed class BaseCostCalculatorTests
    {
        public BaseCostCalculatorTests()
        {
            var path = Substitute.For <IPath>();
            path.Distance.Returns(new Distance(100.0));

            m_Feature1 = new SurveyFeature(0,
                                           new Point(10.0,
                                                     10.0),
                                           new Point(20.0,
                                                     10.0),
                                           Angle.For45Degrees,
                                           Angle.For180Degrees,
                                           Constants.LineDirection.Forward,
                                           123.0);
            m_Feature2 = new SurveyFeature(1,
                                           new Point(10.0,
                                                     20.0),
                                           new Point(20.0,
                                                     20.0),
                                           Angle.For45Degrees,
                                           Angle.For180Degrees,
                                           Constants.LineDirection.Forward,
                                           123.0);


            m_Features = new[]
                         {
                             m_Feature1,
                             m_Feature2
                         };

            CreatePaths();

            CreateRacetracks();

            m_Logger = Substitute.For <ISelkieLogger>();

            m_Calculator = new TestBaseCostCalculator(m_Logger);
        }

        private readonly TestBaseCostCalculator m_Calculator;
        private readonly ISurveyFeature m_Feature1;
        private readonly ISurveyFeature m_Feature2;
        private readonly ISurveyFeature[] m_Features;
        private readonly ISelkieLogger m_Logger;
        private IPath[][] m_ForwardForwardPaths;
        private IPath[][] m_ForwardReversePaths;
        private IRacetracks m_Racetracks;
        private IPath[][] m_ReverseForwardPaths;
        private IPath[][] m_ReverseReversePaths;

        [Fact]
        public void CalculateRacetrackCostShouldLogsMessageForFromFeatureIdToBigIdTest()
        {
            m_Racetracks.ForwardToReverse.Returns(m_ForwardForwardPaths);

            m_Calculator.CheckAndCalculate(2,
                                           1);

            m_Logger.Received().Warn(Arg.Any <string>());
        }

        [Fact]
        public void CalculateRacetrackCostShouldLogsMessageForNegativeFromFeatureIdTest()
        {
            m_Racetracks.ForwardToReverse.Returns(m_ForwardForwardPaths);

            m_Calculator.CheckAndCalculate(-1,
                                           1);

            m_Logger.Received().Warn(Arg.Any <string>());
        }

        [Fact]
        public void CalculateRacetrackCostShouldLogsMessageForNegativeToFeatureIdTest()
        {
            m_Racetracks.ForwardToReverse.Returns(m_ForwardForwardPaths);

            m_Calculator.CheckAndCalculate(0,
                                           -1);

            m_Logger.Received().Warn(Arg.Any <string>());
        }

        [Fact]
        public void CalculateRacetrackCostShouldLogsMessageForPathIsEmptyTest()
        {
            m_Racetracks.ForwardToReverse.Returns(new IPath[0][]);

            m_Calculator.CheckAndCalculate(0,
                                           2);

            m_Logger.Received().Warn(Arg.Any <string>());
        }

        [Fact]
        public void CalculateRacetrackCostShouldLogsMessageForToFeatureIdToBigIdTest()
        {
            m_Racetracks.ForwardToForward.Returns(m_ForwardForwardPaths);
            m_Calculator.Racetracks = m_Racetracks;

            m_Calculator.CheckAndCalculate(1,
                                           2);

            m_Logger.Received().Warn(Arg.Any <string>());
        }

        [Fact]
        public void CalculateRacetrackCostShouldLogsMesssageForToFeatureIdToBigIdTest()
        {
            m_Racetracks.ForwardToReverse.Returns(m_ForwardForwardPaths);

            m_Calculator.CheckAndCalculate(0,
                                           2);

            m_Logger.Received().Warn(Arg.Any <string>());
        }

        [Fact]
        public void CalculateTest()
        {
            m_Calculator.Features = m_Features;
            m_Calculator.Feature = m_Feature1;
            m_Calculator.Racetracks = m_Racetracks;
            m_Calculator.Calculate();

            Dictionary <int, double> actual = m_Calculator.Costs;

            XUnitHelper.AssertIsEqual(2,
                                      actual.Keys.Count,
                                      "Count");
            XUnitHelper.AssertIsEqual(CostMatrix.CostToMyself,
                                      actual [ 0 ],
                                      "actual [ 0 ]");
            XUnitHelper.AssertIsEquivalent(100.0,
                                           actual [ 1 ],
                                           Selkie.Common.Constants.EpsilonDistance,
                                           "actual[1]");
        }

        [Fact]
        public void DefaultCostsTest()
        {
            Assert.NotNull(m_Calculator.Costs);
        }

        [Fact]
        public void DefaultFeaturesTest()
        {
            Assert.NotNull(m_Calculator.Features);
        }

        [Fact]
        public void DefaultFeatureTest()
        {
            Assert.NotNull(m_Calculator.Feature);
            Assert.True(m_Calculator.Feature.IsUnknown,
                        "IsUnknown");
        }

        [Fact]
        public void DefaultRacetracksest()
        {
            Assert.True(m_Calculator.Racetracks == Racetracks.Converters.Dtos.Racetracks.Unknown);
        }

        [Fact]
        public void RoundtripFeaturesTest()
        {
            var feature = Substitute.For <ISurveyFeature>();
            ISurveyFeature[] features =
            {
                feature
            };

            m_Calculator.Features = features;

            Assert.Equal(features,
                         m_Calculator.Features);
        }

        [Fact]
        public void RoundtripFeatureTest()
        {
            var feature = Substitute.For <ISurveyFeature>();

            m_Calculator.Feature = feature;

            Assert.Equal(feature,
                         m_Calculator.Feature);
        }

        [Fact]
        public void RoundtripRacetracksTest()
        {
            var racetracks = Substitute.For <IRacetracks>();

            m_Calculator.Racetracks = racetracks;

            Assert.Equal(racetracks,
                         m_Calculator.Racetracks);
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

        private void CreatePaths()
        {
            m_ForwardForwardPaths = CreateForwardForwardPaths();
            m_ForwardReversePaths = CreateForwardReversePaths();
            m_ReverseForwardPaths = CreateReverseForwardPaths();
            m_ReverseReversePaths = CreateReverseReversePaths();
        }

        private void CreateRacetracks()
        {
            m_Racetracks = Substitute.For <IRacetracks>();
            m_Racetracks.ForwardToForward.Returns(m_ForwardForwardPaths);
            m_Racetracks.ForwardToReverse.Returns(m_ForwardReversePaths);
            m_Racetracks.ReverseToForward.Returns(m_ReverseForwardPaths);
            m_Racetracks.ReverseToReverse.Returns(m_ReverseReversePaths);
        }

        private class TestBaseCostCalculator : BaseCostCalculator
        {
            public TestBaseCostCalculator([NotNull] ISelkieLogger logger)
                : base(logger)
            {
            }

            internal override double CalculateRacetrackCost(int fromFeatureId,
                                                            int toFeatureId)
            {
                return 100.0;
            }
        }
    }
}