using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;
using NSubstitute;
using Selkie.Common;
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
    public sealed class SurveyFeatureToSurveyFeaturesConverterTests
    {
        public SurveyFeatureToSurveyFeaturesConverterTests()
        {
            ConfigureSurveyFeatures();
            ConfigureRacetracks();

            m_Logger = Substitute.For <ISelkieLogger>();

            m_Converter = new SurveyFeatureToSurveyFeaturesConverter(new CostStartToStartCalculator(m_Logger),
                                                                     new CostStartToEndCalculator(m_Logger),
                                                                     new CostEndToStartCalculator(m_Logger),
                                                                     new CostEndToEndCalculator(m_Logger))
                          {
                              Racetracks = m_Racetracks,
                              Feature = m_Feature1,
                              Features = m_Features1
                          };

            m_Converter.Convert();
        }

        private readonly SurveyFeatureToSurveyFeaturesConverter m_Converter;
        private readonly ISelkieLogger m_Logger;
        private ISurveyFeature m_Feature1;
        private ISurveyFeature m_Feature2;
        private ISurveyFeature[] m_Features1;
        private ISurveyFeature[] m_Features2;
        private IPath[][] m_ForwardForwardPaths;
        private IPath[][] m_ForwardReversePaths;
        private IRacetracks m_Racetracks;
        private IPath[][] m_ReverseForwardPaths;
        private IPath[][] m_ReverseReversePaths;

        [Fact]
        public void BaseCostDefaultTest()
        {
            m_Converter.Feature = m_Feature1;

            Assert.Equal(m_Feature1.Length,
                         m_Converter.BaseCost);
        }

        [Fact]
        public void CalculateTotalCostReturnsMaxValueForCostToMySelfTest()
        {
            const double expected = double.MaxValue;
            double actual = m_Converter.CalculateTotalCost(10.0,
                                                           CostMatrix.CostToMyself);

            Assert.Equal(expected,
                         actual);
        }

        [Fact]
        public void CalculateTotalCostReturnsMaxValueForLessZeroTest()
        {
            const double expected = double.MaxValue;
            double actual = m_Converter.CalculateTotalCost(10.0,
                                                           -10.0);

            Assert.Equal(expected,
                         actual);
        }

        [Fact]
        public void CalculateTotalCostReturnsMaxValueForZeroTest()
        {
            const double expected = double.MaxValue;
            double actual = m_Converter.CalculateTotalCost(10.0,
                                                           0.0);

            Assert.Equal(expected,
                         actual);
        }

        [Fact]
        public void CalculateTotalCostReturnsValueForGreaterZeroTest()
        {
            const double expected = 30.0;
            double actual = m_Converter.CalculateTotalCost(10.0,
                                                           20.0);

            Assert.Equal(expected,
                         actual);
        }

        [Fact]
        public void CostEndToEndForFeature1Test()
        {
            XUnitHelper.AssertIsEquivalent(double.MaxValue,
                                           m_Converter.CostEndToEnd(m_Feature1),
                                           Constants.EpsilonDistance,
                                           "actual[1]");
        }

        [Fact]
        public void CostEndToEndForFeature2Test()
        {
            XUnitHelper.AssertIsEquivalent(70.0,
                                           m_Converter.CostEndToEnd(m_Feature2),
                                           Constants.EpsilonDistance,
                                           "actual[1]");
        }

        [Fact]
        public void CostEndToStartdForFeature1Test()
        {
            XUnitHelper.AssertIsEquivalent(double.MaxValue,
                                           m_Converter.CostEndToStart(m_Feature1),
                                           Constants.EpsilonDistance,
                                           "actual[1]");
        }

        [Fact]
        public void CostEndToStartForFeature2Test()
        {
            XUnitHelper.AssertIsEquivalent(30.0,
                                           m_Converter.CostEndToStart(m_Feature2),
                                           Constants.EpsilonDistance,
                                           "actual[1]");
        }

        [Fact]
        public void CostForwardForwardForFeature1Test()
        {
            XUnitHelper.AssertIsEquivalent(double.MaxValue,
                                           m_Converter.CostForwardForward(m_Feature1),
                                           Constants.EpsilonDistance,
                                           "actual[1]");
        }

        [Fact]
        public void CostForwardForwardForFeature2Test()
        {
            XUnitHelper.AssertIsEquivalent(30.0,
                                           m_Converter.CostForwardForward(m_Feature2),
                                           Constants.EpsilonDistance,
                                           "actual[1]");
        }

        [Fact]
        public void CostForwardReverseForFeature1Test()
        {
            XUnitHelper.AssertIsEquivalent(double.MaxValue,
                                           m_Converter.CostForwardReverse(m_Feature1),
                                           Constants.EpsilonDistance,
                                           "actual[1]");
        }

        [Fact]
        public void CostForwardReverseForFeature2Test()
        {
            XUnitHelper.AssertIsEquivalent(70.0,
                                           m_Converter.CostForwardReverse(m_Feature2),
                                           Constants.EpsilonDistance,
                                           "actual[1]");
        }

        [Fact]
        public void CostReverseForwardForFeature1Test()
        {
            XUnitHelper.AssertIsEquivalent(double.MaxValue,
                                           m_Converter.CostReverseForward(m_Feature1),
                                           Constants.EpsilonDistance,
                                           "actual[1]");
        }

        [Fact]
        public void CostReverseForwardForFeature2Test()
        {
            XUnitHelper.AssertIsEquivalent(110.0,
                                           m_Converter.CostReverseForward(m_Feature2),
                                           Constants.EpsilonDistance,
                                           "actual[1]");
        }

        [Fact]
        public void CostReverseReverseForFeature1Test()
        {
            XUnitHelper.AssertIsEquivalent(double.MaxValue,
                                           m_Converter.CostReverseReverse(m_Feature1),
                                           Constants.EpsilonDistance,
                                           "actual[1]");
        }

        [Fact]
        public void CostReverseReverseForFeature2Test()
        {
            XUnitHelper.AssertIsEquivalent(150.0,
                                           m_Converter.CostReverseReverse(m_Feature2),
                                           Constants.EpsilonDistance,
                                           "actual[1]");
        }

        [Fact]
        public void CostStartToEndForFeature1Test()
        {
            XUnitHelper.AssertIsEquivalent(double.MaxValue,
                                           m_Converter.CostStartToEnd(m_Feature1),
                                           Constants.EpsilonDistance,
                                           "actual[1]");
        }

        [Fact]
        public void CostStartToEndForFeature2Test()
        {
            XUnitHelper.AssertIsEquivalent(150.0,
                                           m_Converter.CostStartToEnd(m_Feature2),
                                           Constants.EpsilonDistance,
                                           "actual[1]");
        }

        [Fact]
        public void CostStartToEndReturnsTotalCostTest()
        {
            const double expected = 100.0;
            double actual = m_Converter.CostStartToEnd(m_Feature1);

            Assert.False(XUnitHelper.IsEquivalent(expected,
                                                  actual));
        }

        [Fact]
        public void CostStartToStartForFeature1Test()
        {
            XUnitHelper.AssertIsEquivalent(double.MaxValue,
                                           m_Converter.CostStartToStart(m_Feature1),
                                           Constants.EpsilonDistance,
                                           "actual[1]");
        }

        [Fact]
        public void CostStartToStartForFeature2Test()
        {
            XUnitHelper.AssertIsEquivalent(110.0,
                                           m_Converter.CostStartToStart(m_Feature2),
                                           Constants.EpsilonDistance,
                                           "actual[1]");
        }

        [Fact]
        public void FeatureRoundtripTest()
        {
            m_Converter.Feature = m_Feature1;
            Assert.Equal(m_Feature1,
                         m_Converter.Feature);

            m_Converter.Feature = m_Feature2;
            Assert.Equal(m_Feature2,
                         m_Converter.Feature);
        }

        [Fact]
        public void FeaturesRoundtripTest()
        {
            m_Converter.Features = m_Features1;
            Assert.Equal(m_Features1,
                         m_Converter.Features);

            m_Converter.Features = m_Features2;
            Assert.Equal(m_Features2,
                         m_Converter.Features);
        }

        [Fact]
        public void IsCostToMySelfReturnsFalseForPositiveTest()
        {
            Assert.False(m_Converter.IsCostToMySelf(100.0));
        }

        [Fact]
        public void IsCostToMySelfReturnsTrueForCostToMySelfTest()
        {
            Assert.True(m_Converter.IsCostToMySelf(CostMatrix.CostToMyself));
        }

        [Fact]
        public void IsCostToMySelfReturnsTrueForNegativeTest()
        {
            Assert.False(m_Converter.IsCostToMySelf(-100.0));
        }

        [Fact]
        public void IsCostValidReturnsFalseForCostToMySelfTest()
        {
            Assert.False(m_Converter.IsCostValid(CostMatrix.CostToMyself));
        }

        [Fact]
        public void IsCostValidReturnsFalseForLessZeroTest()
        {
            Assert.False(m_Converter.IsCostValid(-1.0));
        }

        [Fact]
        public void IsCostValidReturnsTrueForGreaterZeroTest()
        {
            Assert.True(m_Converter.IsCostValid(1.0));
        }

        [Fact]
        public void RacetracksDefaultTest()
        {
            var converter = new SurveyFeatureToSurveyFeaturesConverter(new CostStartToStartCalculator(m_Logger),
                                                                       new CostStartToEndCalculator(m_Logger),
                                                                       new CostEndToStartCalculator(m_Logger),
                                                                       new CostEndToEndCalculator(m_Logger));

            Assert.True(Racetracks.Converters.Dtos.Racetracks.Unknown == converter.Racetracks);
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

        private void ConfigureSurveyFeatures()
        {
            m_Feature1 = new SurveyFeature(0,
                                           new Point(10.0,
                                                     10.0),
                                           new Point(20.0,
                                                     10.0),
                                           Angle.For45Degrees,
                                           Angle.For45Degrees,
                                           Geometry.Constants.LineDirection.Forward,
                                           10.0);

            m_Feature2 = new SurveyFeature(1,
                                           new Point(10.0,
                                                     20.0),
                                           new Point(20.0,
                                                     20.0),
                                           Angle.For45Degrees,
                                           Angle.For45Degrees,
                                           Geometry.Constants.LineDirection.Forward,
                                           10.0);

            m_Features1 = new[]
                          {
                              m_Feature1,
                              m_Feature2
                          };
            m_Features2 = new[]
                          {
                              m_Feature2,
                              m_Feature1
                          };
        }
    }
}