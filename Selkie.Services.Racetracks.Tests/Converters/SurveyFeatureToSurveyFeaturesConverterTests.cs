using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;
using NSubstitute;
using NUnit.Framework;
using Selkie.Geometry.Primitives;
using Selkie.Geometry.Shapes;
using Selkie.Geometry.Surveying;
using Selkie.NUnit.Extensions;
using Selkie.Racetrack.Interfaces;
using Selkie.Services.Racetracks.Converters;
using Selkie.Windsor;
using Constants = Selkie.Geometry.Constants;

namespace Selkie.Services.Racetracks.Tests.Converters
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    internal sealed class SurveyFeatureToSurveyFeaturesConverterTests
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
                                           Constants.LineDirection.Forward,
                                           10.0);

            m_Feature2 = new SurveyFeature(1,
                                           new Point(10.0,
                                                     20.0),
                                           new Point(20.0,
                                                     20.0),
                                           Angle.For45Degrees,
                                           Angle.For45Degrees,
                                           Constants.LineDirection.Forward,
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

        [Test]
        public void BaseCostDefaultTest()
        {
            m_Converter.Feature = m_Feature1;

            Assert.AreEqual(m_Feature1.Length,
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
        public void CostEndToEndForFeature1Test()
        {
            NUnitHelper.AssertIsEquivalent(double.MaxValue,
                                           m_Converter.CostEndToEnd(m_Feature1),
                                           Constants.EpsilonDistance,
                                           "actual[1]");
        }

        [Test]
        public void CostEndToEndForFeature2Test()
        {
            NUnitHelper.AssertIsEquivalent(70.0,
                                           m_Converter.CostEndToEnd(m_Feature2),
                                           Constants.EpsilonDistance,
                                           "actual[1]");
        }

        [Test]
        public void CostEndToStartdForFeature1Test()
        {
            NUnitHelper.AssertIsEquivalent(double.MaxValue,
                                           m_Converter.CostEndToStart(m_Feature1),
                                           Constants.EpsilonDistance,
                                           "actual[1]");
        }

        [Test]
        public void CostEndToStartForFeature2Test()
        {
            NUnitHelper.AssertIsEquivalent(30.0,
                                           m_Converter.CostEndToStart(m_Feature2),
                                           Constants.EpsilonDistance,
                                           "actual[1]");
        }

        [Test]
        public void CostForwardForwardForFeature1Test()
        {
            NUnitHelper.AssertIsEquivalent(double.MaxValue,
                                           m_Converter.CostForwardForward(m_Feature1),
                                           Constants.EpsilonDistance,
                                           "actual[1]");
        }

        [Test]
        public void CostForwardForwardForFeature2Test()
        {
            NUnitHelper.AssertIsEquivalent(30.0,
                                           m_Converter.CostForwardForward(m_Feature2),
                                           Constants.EpsilonDistance,
                                           "actual[1]");
        }

        [Test]
        public void CostForwardReverseForFeature1Test()
        {
            NUnitHelper.AssertIsEquivalent(double.MaxValue,
                                           m_Converter.CostForwardReverse(m_Feature1),
                                           Constants.EpsilonDistance,
                                           "actual[1]");
        }

        [Test]
        public void CostForwardReverseForFeature2Test()
        {
            NUnitHelper.AssertIsEquivalent(70.0,
                                           m_Converter.CostForwardReverse(m_Feature2),
                                           Constants.EpsilonDistance,
                                           "actual[1]");
        }

        [Test]
        public void CostReverseForwardForFeature1Test()
        {
            NUnitHelper.AssertIsEquivalent(double.MaxValue,
                                           m_Converter.CostReverseForward(m_Feature1),
                                           Constants.EpsilonDistance,
                                           "actual[1]");
        }

        [Test]
        public void CostReverseForwardForFeature2Test()
        {
            NUnitHelper.AssertIsEquivalent(110.0,
                                           m_Converter.CostReverseForward(m_Feature2),
                                           Constants.EpsilonDistance,
                                           "actual[1]");
        }

        [Test]
        public void CostReverseReverseForFeature1Test()
        {
            NUnitHelper.AssertIsEquivalent(double.MaxValue,
                                           m_Converter.CostReverseReverse(m_Feature1),
                                           Constants.EpsilonDistance,
                                           "actual[1]");
        }

        [Test]
        public void CostReverseReverseForFeature2Test()
        {
            NUnitHelper.AssertIsEquivalent(150.0,
                                           m_Converter.CostReverseReverse(m_Feature2),
                                           Constants.EpsilonDistance,
                                           "actual[1]");
        }

        [Test]
        public void CostStartToEndForFeature1Test()
        {
            NUnitHelper.AssertIsEquivalent(double.MaxValue,
                                           m_Converter.CostStartToEnd(m_Feature1),
                                           Constants.EpsilonDistance,
                                           "actual[1]");
        }

        [Test]
        public void CostStartToEndForFeature2Test()
        {
            NUnitHelper.AssertIsEquivalent(150.0,
                                           m_Converter.CostStartToEnd(m_Feature2),
                                           Constants.EpsilonDistance,
                                           "actual[1]");
        }

        [Test]
        public void CostStartToEndReturnsTotalCostTest()
        {
            const double expected = 100.0;
            double actual = m_Converter.CostStartToEnd(m_Feature1);

            Assert.False(NUnitHelper.IsEquivalent(expected,
                                                  actual));
        }

        [Test]
        public void CostStartToStartForFeature1Test()
        {
            NUnitHelper.AssertIsEquivalent(double.MaxValue,
                                           m_Converter.CostStartToStart(m_Feature1),
                                           Constants.EpsilonDistance,
                                           "actual[1]");
        }

        [Test]
        public void CostStartToStartForFeature2Test()
        {
            NUnitHelper.AssertIsEquivalent(110.0,
                                           m_Converter.CostStartToStart(m_Feature2),
                                           Constants.EpsilonDistance,
                                           "actual[1]");
        }

        [Test]
        public void FeatureRoundtripTest()
        {
            m_Converter.Feature = m_Feature1;
            Assert.AreEqual(m_Feature1,
                            m_Converter.Feature);

            m_Converter.Feature = m_Feature2;
            Assert.AreEqual(m_Feature2,
                            m_Converter.Feature);
        }

        [Test]
        public void FeaturesRoundtripTest()
        {
            m_Converter.Features = m_Features1;
            Assert.AreEqual(m_Features1,
                            m_Converter.Features);

            m_Converter.Features = m_Features2;
            Assert.AreEqual(m_Features2,
                            m_Converter.Features);
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
        public void RacetracksDefaultTest()
        {
            var converter = new SurveyFeatureToSurveyFeaturesConverter(new CostStartToStartCalculator(m_Logger),
                                                                       new CostStartToEndCalculator(m_Logger),
                                                                       new CostEndToStartCalculator(m_Logger),
                                                                       new CostEndToEndCalculator(m_Logger));

            Assert.True(Racetracks.Converters.Dtos.Racetracks.Unknown == converter.Racetracks);
        }
    }
}