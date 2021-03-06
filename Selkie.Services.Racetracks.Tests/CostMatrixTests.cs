﻿using System.Diagnostics.CodeAnalysis;
using NSubstitute;
using NUnit.Framework;
using Selkie.Geometry.Primitives;
using Selkie.Geometry.Shapes;
using Selkie.Geometry.Surveying;
using Selkie.NUnit.Extensions;
using Selkie.Racetrack.Interfaces;
using Selkie.Services.Racetracks.Interfaces;
using Selkie.Services.Racetracks.Interfaces.Converters;
using Selkie.Services.Racetracks.TypedFactories;
using Selkie.Windsor;
using Constants = Selkie.Geometry.Constants;

namespace Selkie.Services.Racetracks.Tests
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    internal sealed class CostMatrixTests
    {
        public CostMatrixTests()
        {
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

            ConfigureFeatureToFeaturesConverter();

            m_SurveyFeaturesSourceManager = Substitute.For <ISurveyFeaturesSourceManager>();
            m_SurveyFeaturesSourceManager.Features.Returns(m_Features);

            m_ConverterFactory = Substitute.For <IConverterFactory>();
            m_ConverterFactory.Create <ISurveyFeatureToSurveyFeaturesConverter>()
                              .Returns(m_SurveyFeatureToSurveyFeaturesConverter);

            m_RacetracksSourceManager = Substitute.For <IRacetracksSourceManager>();

            m_CostMatrix = new CostMatrix(Substitute.For <ISelkieLogger>(),
                                          m_SurveyFeaturesSourceManager,
                                          m_RacetracksSourceManager,
                                          m_ConverterFactory);
        }

        private readonly IConverterFactory m_ConverterFactory;
        private readonly CostMatrix m_CostMatrix;
        private readonly ISurveyFeature m_Feature1;
        private readonly ISurveyFeature m_Feature2;
        private readonly ISurveyFeature[] m_Features;
        private readonly IRacetracksSourceManager m_RacetracksSourceManager;
        private readonly ISurveyFeaturesSourceManager m_SurveyFeaturesSourceManager;
        private ISurveyFeatureToSurveyFeaturesConverter m_SurveyFeatureToSurveyFeaturesConverter;

        private void ConfigureFeatureToFeaturesConverter()
        {
            m_SurveyFeatureToSurveyFeaturesConverter = Substitute.For <ISurveyFeatureToSurveyFeaturesConverter>();
            m_SurveyFeatureToSurveyFeaturesConverter.CostStartToStart(m_Feature1).Returns(10.0);
            m_SurveyFeatureToSurveyFeaturesConverter.CostStartToEnd(m_Feature1).Returns(20.0);
            m_SurveyFeatureToSurveyFeaturesConverter.CostEndToStart(m_Feature1).Returns(30.0);
            m_SurveyFeatureToSurveyFeaturesConverter.CostEndToEnd(m_Feature1).Returns(40.0);

            m_SurveyFeatureToSurveyFeaturesConverter.CostStartToStart(m_Feature2).Returns(50.0);
            m_SurveyFeatureToSurveyFeaturesConverter.CostStartToEnd(m_Feature2).Returns(60.0);
            m_SurveyFeatureToSurveyFeaturesConverter.CostEndToStart(m_Feature2).Returns(70.0);
            m_SurveyFeatureToSurveyFeaturesConverter.CostEndToEnd(m_Feature2).Returns(80.0);
        }

        [Test]
        public void CreateFeaturesToFeaturesCallsConvertTest()
        {
            m_CostMatrix.CreateFeaturesToFeatures(m_Features);

            m_SurveyFeatureToSurveyFeaturesConverter.Received(4).Convert();
        }

        [Test]
        public void CreateFeaturesToFeaturesIndexOneTest()
        {
            ISurveyFeatureToSurveyFeaturesConverter[] actual = m_CostMatrix.CreateFeaturesToFeatures(m_Features);

            Assert.NotNull(actual [ 1 ]);
        }

        [Test]
        public void CreateFeaturesToFeaturesIndexZeroTest()
        {
            ISurveyFeatureToSurveyFeaturesConverter[] actual = m_CostMatrix.CreateFeaturesToFeatures(m_Features);

            Assert.NotNull(actual [ 0 ]);
        }

        [Test]
        public void CreateFeaturesToFeaturesLengthTest()
        {
            ISurveyFeatureToSurveyFeaturesConverter[] actual = m_CostMatrix.CreateFeaturesToFeatures(m_Features);

            Assert.AreEqual(2,
                            actual.Length,
                            "Length");
        }

        [Test]
        public void CreateFeaturesToFeaturesSetsFeaturesTest()
        {
            ISurveyFeature[] features =
            {
                m_Feature1
            };

            m_CostMatrix.CreateFeaturesToFeatures(features);

            Assert.AreEqual(features,
                            m_SurveyFeatureToSurveyFeaturesConverter.Features);
        }

        [Test]
        public void CreateFeaturesToFeaturesSetsFeatureTest()
        {
            ISurveyFeature[] features =
            {
                m_Feature1
            };

            m_CostMatrix.CreateFeaturesToFeatures(features);

            Assert.AreEqual(m_Feature1,
                            m_SurveyFeatureToSurveyFeaturesConverter.Feature);
        }

        [Test]
        public void CreateFeaturesToFeaturesSetsRacetracksTest()
        {
            ISurveyFeature[] features =
            {
                m_Feature1
            };

            m_CostMatrix.CreateFeaturesToFeatures(features);

            IRacetracks expected = m_RacetracksSourceManager.Racetracks;
            IRacetracks actual = m_SurveyFeatureToSurveyFeaturesConverter.Racetracks;

            Assert.AreEqual(expected,
                            actual);
        }

        [Test]
        public void CreateJaggedCostMatrixForMultipleFeatureTest()
        {
            double[][] actual = m_CostMatrix.CreateJaggedCostMatrix(m_Features);

            Assert.AreEqual(4,
                            actual.Length,
                            "Length");
            Assert.AreEqual(4,
                            actual.GetLength(0),
                            "GetLength(0)");
        }

        [Test]
        public void CreateJaggedCostMatrixForSingleFeatureTest()
        {
            double[][] actual = m_CostMatrix.CreateJaggedCostMatrix(new[]
                                                                    {
                                                                        m_Feature1
                                                                    });

            Assert.AreEqual(2,
                            actual.Length,
                            "Length");
            Assert.AreEqual(2,
                            actual.GetLength(0),
                            "GetLength(0)");
        }

        [Test]
        public void CreateMatrixForMultipleFeatureTest()
        {
            double[][] actual = m_CostMatrix.CreateMatrix(m_Features);

            Assert.AreEqual(4,
                            actual.Length,
                            "Length");
            Assert.AreEqual(4,
                            actual.GetLength(0),
                            "GetLength(0)");
        }

        [Test]
        public void CreateMatrixForSingleFeatureTest()
        {
            double[][] actual = m_CostMatrix.CreateMatrix(new[]
                                                          {
                                                              m_Feature1
                                                          });

            Assert.AreEqual(2,
                            actual.Length,
                            "Length");
            Assert.AreEqual(2,
                            actual.GetLength(0),
                            "GetLength(0)");
        }

        [Test]
        public void FeaturesTest()
        {
            Assert.AreEqual(m_Features,
                            m_CostMatrix.Features);
        }

        [Test]
        public void MatrixFeatureForForwardTest()
        {
            double[] actual = m_CostMatrix.MatrixFeatureForForward(m_SurveyFeatureToSurveyFeaturesConverter,
                                                                   m_Features);

            Assert.AreEqual(4,
                            actual.Length,
                            "Length");
            NUnitHelper.AssertIsEquivalent(30.0,
                                           actual [ 0 ],
                                           Selkie.Common.Constants.EpsilonDistance,
                                           "actual[0]");
            NUnitHelper.AssertIsEquivalent(40.0,
                                           actual [ 1 ],
                                           Selkie.Common.Constants.EpsilonDistance,
                                           "actual[1]");
            NUnitHelper.AssertIsEquivalent(70.0,
                                           actual [ 2 ],
                                           Selkie.Common.Constants.EpsilonDistance,
                                           "actual[2]");
            NUnitHelper.AssertIsEquivalent(80.0,
                                           actual [ 3 ],
                                           Selkie.Common.Constants.EpsilonDistance,
                                           "actual[3]");
        }

        [Test]
        public void MatrixFeatureForReversTest()
        {
            double[] actual = m_CostMatrix.MatrixFeatureForRevers(m_SurveyFeatureToSurveyFeaturesConverter,
                                                                  m_Features);

            Assert.AreEqual(4,
                            actual.Length,
                            "Length");
            NUnitHelper.AssertIsEquivalent(10.0,
                                           actual [ 0 ],
                                           Selkie.Common.Constants.EpsilonDistance,
                                           "actual[0]");
            NUnitHelper.AssertIsEquivalent(20.0,
                                           actual [ 1 ],
                                           Selkie.Common.Constants.EpsilonDistance,
                                           "actual[1]");
            NUnitHelper.AssertIsEquivalent(50.0,
                                           actual [ 2 ],
                                           Selkie.Common.Constants.EpsilonDistance,
                                           "actual[2]");
            NUnitHelper.AssertIsEquivalent(60.0,
                                           actual [ 3 ],
                                           Selkie.Common.Constants.EpsilonDistance,
                                           "actual[3]");
        }

        [Test]
        public void MatrixForMultipleFeaturesLengthTest()
        {
            double[][] actual = m_CostMatrix.MatrixForMultipleFeatures(m_Features);

            Assert.AreEqual(4,
                            actual.Length,
                            "Length");
            Assert.AreEqual(4,
                            actual.GetLength(0),
                            "GetLength(0)");
        }

        [Test]
        public void MatrixForMultipleFeaturesLevelOneTest()
        {
            double[][] multple = m_CostMatrix.MatrixForMultipleFeatures(m_Features);
            double[] actual = multple [ 1 ];

            Assert.AreEqual(4,
                            actual.Length,
                            "Length");
            NUnitHelper.AssertIsEquivalent(10.0,
                                           actual [ 0 ],
                                           Selkie.Common.Constants.EpsilonDistance,
                                           "actual[0]");
            NUnitHelper.AssertIsEquivalent(20.0,
                                           actual [ 1 ],
                                           Selkie.Common.Constants.EpsilonDistance,
                                           "actual[1]");
            NUnitHelper.AssertIsEquivalent(50.0,
                                           actual [ 2 ],
                                           Selkie.Common.Constants.EpsilonDistance,
                                           "actual[2]");
            NUnitHelper.AssertIsEquivalent(60.0,
                                           actual [ 3 ],
                                           Selkie.Common.Constants.EpsilonDistance,
                                           "actual[3]");
        }

        [Test]
        public void MatrixForMultipleFeaturesLevelThreeTest()
        {
            double[][] multple = m_CostMatrix.MatrixForMultipleFeatures(m_Features);
            double[] actual = multple [ 3 ];

            Assert.AreEqual(4,
                            actual.Length,
                            "Length");
            NUnitHelper.AssertIsEquivalent(10.0,
                                           actual [ 0 ],
                                           Selkie.Common.Constants.EpsilonDistance,
                                           "actual[0]");
            NUnitHelper.AssertIsEquivalent(20.0,
                                           actual [ 1 ],
                                           Selkie.Common.Constants.EpsilonDistance,
                                           "actual[1]");
            NUnitHelper.AssertIsEquivalent(50.0,
                                           actual [ 2 ],
                                           Selkie.Common.Constants.EpsilonDistance,
                                           "actual[2]");
            NUnitHelper.AssertIsEquivalent(60.0,
                                           actual [ 3 ],
                                           Selkie.Common.Constants.EpsilonDistance,
                                           "actual[3]");
        }

        [Test]
        public void MatrixForMultipleFeaturesLevelTwoTest()
        {
            double[][] multple = m_CostMatrix.MatrixForMultipleFeatures(m_Features);
            double[] actual = multple [ 2 ];

            Assert.AreEqual(4,
                            actual.Length,
                            "Length");
            NUnitHelper.AssertIsEquivalent(30.0,
                                           actual [ 0 ],
                                           Selkie.Common.Constants.EpsilonDistance,
                                           "actual[0]");
            NUnitHelper.AssertIsEquivalent(40.0,
                                           actual [ 1 ],
                                           Selkie.Common.Constants.EpsilonDistance,
                                           "actual[1]");
            NUnitHelper.AssertIsEquivalent(70.0,
                                           actual [ 2 ],
                                           Selkie.Common.Constants.EpsilonDistance,
                                           "actual[2]");
            NUnitHelper.AssertIsEquivalent(80.0,
                                           actual [ 3 ],
                                           Selkie.Common.Constants.EpsilonDistance,
                                           "actual[3]");
        }

        [Test]
        public void MatrixForMultipleFeaturesLevelZeroTest()
        {
            double[][] multple = m_CostMatrix.MatrixForMultipleFeatures(m_Features);
            double[] actual = multple [ 0 ];

            Assert.AreEqual(4,
                            actual.Length,
                            "Length");
            NUnitHelper.AssertIsEquivalent(30.0,
                                           actual [ 0 ],
                                           Selkie.Common.Constants.EpsilonDistance,
                                           "actual[0]");
            NUnitHelper.AssertIsEquivalent(40.0,
                                           actual [ 1 ],
                                           Selkie.Common.Constants.EpsilonDistance,
                                           "actual[1]");
            NUnitHelper.AssertIsEquivalent(70.0,
                                           actual [ 2 ],
                                           Selkie.Common.Constants.EpsilonDistance,
                                           "actual[2]");
            NUnitHelper.AssertIsEquivalent(80.0,
                                           actual [ 3 ],
                                           Selkie.Common.Constants.EpsilonDistance,
                                           "actual[3]");
        }

        [Test]
        public void MatrixForOneFeatureLengthTest()
        {
            double[][] actual = m_CostMatrix.MatrixForOneFeature();

            Assert.AreEqual(2,
                            actual.Length,
                            "Length");
            Assert.AreEqual(2,
                            actual.GetLength(0),
                            "GetLength(0)");
        }

        [Test]
        public void MatrixForOneFeatureLevelOneTest()
        {
            double[][] multiple = m_CostMatrix.MatrixForOneFeature();
            double[] actual = multiple [ 1 ];

            Assert.AreEqual(CostMatrix.CostToMyself,
                            actual [ 0 ],
                            "actual[0]");
            Assert.AreEqual(CostMatrix.CostToMyself,
                            actual [ 1 ],
                            "actual[1]");
        }

        [Test]
        public void MatrixForOneFeatureLevelZeroTest()
        {
            double[][] multiple = m_CostMatrix.MatrixForOneFeature();
            double[] actual = multiple [ 0 ];

            Assert.AreEqual(CostMatrix.CostToMyself,
                            actual [ 0 ],
                            "actual[0]");
            Assert.AreEqual(CostMatrix.CostToMyself,
                            actual [ 1 ],
                            "actual[1]");
        }

        [Test]
        public void MatrixLengthTest()
        {
            double[][] actual = m_CostMatrix.Matrix;

            Assert.AreEqual(4,
                            actual.Length,
                            "Length");
            Assert.AreEqual(4,
                            actual.GetLength(0),
                            "GetLength(0)");
        }

        [Test]
        public void MatrixLevelOneTest()
        {
            NUnitHelper.AssertIsEquivalent(10.0,
                                           m_CostMatrix.Matrix [ 1 ] [ 0 ],
                                           Selkie.Common.Constants.EpsilonDistance,
                                           "actual[0]");
            NUnitHelper.AssertIsEquivalent(20.0,
                                           m_CostMatrix.Matrix [ 1 ] [ 1 ],
                                           Selkie.Common.Constants.EpsilonDistance,
                                           "actual[1]");
            NUnitHelper.AssertIsEquivalent(50.0,
                                           m_CostMatrix.Matrix [ 1 ] [ 2 ],
                                           Selkie.Common.Constants.EpsilonDistance,
                                           "actual[2]");
            NUnitHelper.AssertIsEquivalent(60.0,
                                           m_CostMatrix.Matrix [ 1 ] [ 3 ],
                                           Selkie.Common.Constants.EpsilonDistance,
                                           "actual[3]");
        }

        [Test]
        public void MatrixLevelThreeTest()
        {
            NUnitHelper.AssertIsEquivalent(10.0,
                                           m_CostMatrix.Matrix [ 3 ] [ 0 ],
                                           Selkie.Common.Constants.EpsilonDistance,
                                           "actual[0]");
            NUnitHelper.AssertIsEquivalent(20.0,
                                           m_CostMatrix.Matrix [ 3 ] [ 1 ],
                                           Selkie.Common.Constants.EpsilonDistance,
                                           "actual[1]");
            NUnitHelper.AssertIsEquivalent(50.0,
                                           m_CostMatrix.Matrix [ 3 ] [ 2 ],
                                           Selkie.Common.Constants.EpsilonDistance,
                                           "actual[2]");
            NUnitHelper.AssertIsEquivalent(60.0,
                                           m_CostMatrix.Matrix [ 3 ] [ 3 ],
                                           Selkie.Common.Constants.EpsilonDistance,
                                           "actual[3]");
        }

        [Test]
        public void MatrixLevelTwoTest()
        {
            NUnitHelper.AssertIsEquivalent(30.0,
                                           m_CostMatrix.Matrix [ 2 ] [ 0 ],
                                           Selkie.Common.Constants.EpsilonDistance,
                                           "actual[0]");
            NUnitHelper.AssertIsEquivalent(40.0,
                                           m_CostMatrix.Matrix [ 2 ] [ 1 ],
                                           Selkie.Common.Constants.EpsilonDistance,
                                           "actual[1]");
            NUnitHelper.AssertIsEquivalent(70.0,
                                           m_CostMatrix.Matrix [ 2 ] [ 2 ],
                                           Selkie.Common.Constants.EpsilonDistance,
                                           "actual[2]");
            NUnitHelper.AssertIsEquivalent(80.0,
                                           m_CostMatrix.Matrix [ 2 ] [ 3 ],
                                           Selkie.Common.Constants.EpsilonDistance,
                                           "actual[3]");
        }

        [Test]
        public void MatrixLevelZeroTest()
        {
            NUnitHelper.AssertIsEquivalent(30.0,
                                           m_CostMatrix.Matrix [ 0 ] [ 0 ],
                                           Selkie.Common.Constants.EpsilonDistance,
                                           "actual[0]");
            NUnitHelper.AssertIsEquivalent(40.0,
                                           m_CostMatrix.Matrix [ 0 ] [ 1 ],
                                           Selkie.Common.Constants.EpsilonDistance,
                                           "actual[1]");
            NUnitHelper.AssertIsEquivalent(70.0,
                                           m_CostMatrix.Matrix [ 0 ] [ 2 ],
                                           Selkie.Common.Constants.EpsilonDistance,
                                           "actual[2]");
            NUnitHelper.AssertIsEquivalent(80.0,
                                           m_CostMatrix.Matrix [ 0 ] [ 3 ],
                                           Selkie.Common.Constants.EpsilonDistance,
                                           "actual[3]");
        }

        [Test]
        public void ToStringTest()
        {
            const string expected =
                "Matrix:\r\n[0] 30.00 40.00 70.00 80.00\r\n[1] 10.00 20.00 50.00 60.00\r\n[2] 30.00 40.00 70.00 80.00\r\n[3] 10.00 20.00 50.00 60.00\r\n";
            string actual = m_CostMatrix.ToString();

            Assert.AreEqual(expected,
                            actual);
        }

        [Test]
        public void UnkownTest()
        {
            double[][] matrix = CostMatrix.Unkown.Matrix;

            Assert.True(0 == matrix.GetLength(0));
        }
    }
}