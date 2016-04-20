using System.Diagnostics.CodeAnalysis;
using NSubstitute;
using NUnit.Framework;
using Selkie.Geometry.Shapes;
using Selkie.NUnit.Extensions;
using Selkie.Racetrack.Interfaces;
using Selkie.Services.Racetracks.Interfaces;
using Selkie.Services.Racetracks.Interfaces.Converters;
using Selkie.Services.Racetracks.TypedFactories;
using Selkie.Windsor;
using Constants = Selkie.Common.Constants;

namespace Selkie.Services.Racetracks.Tests.NUnit
{
    // ReSharper disable ClassTooBig
    [TestFixture]
    [ExcludeFromCodeCoverage]
    internal sealed class CostMatrixTests
    {
        private IConverterFactory m_ConverterFactory;
        private CostMatrix m_CostMatrix;
        private Line m_Line1;
        private Line m_Line2;
        private ILine[] m_Lines;
        private ILinesSourceManager m_LinesSourceManager;
        private ILineToLinesConverter m_LineToLinesConverter;
        private IRacetracksSourceManager m_RacetracksSourceManager;

        [Test]
        public void CreateJaggedCostMatrixForMultipleLineTest()
        {
            double[][] actual = m_CostMatrix.CreateJaggedCostMatrix(m_Lines);

            Assert.AreEqual(4,
                            actual.Length,
                            "Length");
            Assert.AreEqual(4,
                            actual.GetLength(0),
                            "GetLength(0)");
        }

        [Test]
        public void CreateJaggedCostMatrixForSingleLineTest()
        {
            double[][] actual = m_CostMatrix.CreateJaggedCostMatrix(new ILine[]
                                                                    {
                                                                        m_Line1
                                                                    });

            Assert.AreEqual(2,
                            actual.Length,
                            "Length");
            Assert.AreEqual(2,
                            actual.GetLength(0),
                            "GetLength(0)");
        }

        [Test]
        public void CreateLinesToLinesCallsConvertTest()
        {
            m_CostMatrix.CreateLinesToLines(m_Lines);

            m_LineToLinesConverter.Received(4).Convert();
        }

        [Test]
        public void CreateLinesToLinesLengthTest()
        {
            ILineToLinesConverter[] actual = m_CostMatrix.CreateLinesToLines(m_Lines);

            Assert.AreEqual(2,
                            actual.Length,
                            "Length");
        }

        [Test]
        public void CreateLinesToLinesSetsLinesTest()
        {
            ILine[] lines =
            {
                m_Line1
            };

            m_CostMatrix.CreateLinesToLines(lines);

            Assert.AreEqual(lines,
                            m_LineToLinesConverter.Lines);
        }

        [Test]
        public void CreateLinesToLinesSetsLineTest()
        {
            ILine[] lines =
            {
                m_Line1
            };

            m_CostMatrix.CreateLinesToLines(lines);

            Assert.AreEqual(m_Line1,
                            m_LineToLinesConverter.Line);
        }

        [Test]
        public void CreateLinesToLinesSetsRacetracksTest()
        {
            ILine[] lines =
            {
                m_Line1
            };

            m_CostMatrix.CreateLinesToLines(lines);

            IRacetracks expected = m_RacetracksSourceManager.Racetracks;
            IRacetracks actual = m_LineToLinesConverter.Racetracks;

            Assert.AreEqual(expected,
                            actual);
        }

        [Test]
        public void CreateLinesToLinesTest()
        {
            ILineToLinesConverter[] actual = m_CostMatrix.CreateLinesToLines(m_Lines);

            Assert.NotNull(actual [ 0 ],
                           "actual[0]");
            Assert.NotNull(actual [ 1 ],
                           "actual[1]");
        }

        [Test]
        public void CreateMatrixForMultipleLineTest()
        {
            double[][] actual = m_CostMatrix.CreateMatrix(m_Lines);

            Assert.AreEqual(4,
                            actual.Length,
                            "Length");
            Assert.AreEqual(4,
                            actual.GetLength(0),
                            "GetLength(0)");
        }

        [Test]
        public void CreateMatrixForSingleLineTest()
        {
            double[][] actual = m_CostMatrix.CreateMatrix(new ILine[]
                                                          {
                                                              m_Line1
                                                          });

            Assert.AreEqual(2,
                            actual.Length,
                            "Length");
            Assert.AreEqual(2,
                            actual.GetLength(0),
                            "GetLength(0)");
        }

        [Test]
        public void LinesTest()
        {
            Assert.AreEqual(m_Lines,
                            m_CostMatrix.Lines);
        }

        [Test]
        public void MatrixForMultipleLinesLengthTest()
        {
            double[][] actual = m_CostMatrix.MatrixForMultipleLines(m_Lines);

            Assert.AreEqual(4,
                            actual.Length,
                            "Length");
            Assert.AreEqual(4,
                            actual.GetLength(0),
                            "GetLength(0)");
        }

        [Test]
        public void MatrixForMultipleLinesLevelOneTest()
        {
            double[][] multple = m_CostMatrix.MatrixForMultipleLines(m_Lines);
            double[] actual = multple [ 1 ];

            Assert.AreEqual(4,
                            actual.Length,
                            "Length");
            NUnitHelper.AssertIsEquivalent(10.0,
                                           actual [ 0 ],
                                           Constants.EpsilonDistance,
                                           "actual[0]");
            NUnitHelper.AssertIsEquivalent(20.0,
                                           actual [ 1 ],
                                           Constants.EpsilonDistance,
                                           "actual[1]");
            NUnitHelper.AssertIsEquivalent(50.0,
                                           actual [ 2 ],
                                           Constants.EpsilonDistance,
                                           "actual[2]");
            NUnitHelper.AssertIsEquivalent(60.0,
                                           actual [ 3 ],
                                           Constants.EpsilonDistance,
                                           "actual[3]");
        }

        [Test]
        public void MatrixForMultipleLinesLevelThreeTest()
        {
            double[][] multple = m_CostMatrix.MatrixForMultipleLines(m_Lines);
            double[] actual = multple [ 3 ];

            Assert.AreEqual(4,
                            actual.Length,
                            "Length");
            NUnitHelper.AssertIsEquivalent(10.0,
                                           actual [ 0 ],
                                           Constants.EpsilonDistance,
                                           "actual[0]");
            NUnitHelper.AssertIsEquivalent(20.0,
                                           actual [ 1 ],
                                           Constants.EpsilonDistance,
                                           "actual[1]");
            NUnitHelper.AssertIsEquivalent(50.0,
                                           actual [ 2 ],
                                           Constants.EpsilonDistance,
                                           "actual[2]");
            NUnitHelper.AssertIsEquivalent(60.0,
                                           actual [ 3 ],
                                           Constants.EpsilonDistance,
                                           "actual[3]");
        }

        [Test]
        public void MatrixForMultipleLinesLevelTwoTest()
        {
            double[][] multple = m_CostMatrix.MatrixForMultipleLines(m_Lines);
            double[] actual = multple [ 2 ];

            Assert.AreEqual(4,
                            actual.Length,
                            "Length");
            NUnitHelper.AssertIsEquivalent(30.0,
                                           actual [ 0 ],
                                           Constants.EpsilonDistance,
                                           "actual[0]");
            NUnitHelper.AssertIsEquivalent(40.0,
                                           actual [ 1 ],
                                           Constants.EpsilonDistance,
                                           "actual[1]");
            NUnitHelper.AssertIsEquivalent(70.0,
                                           actual [ 2 ],
                                           Constants.EpsilonDistance,
                                           "actual[2]");
            NUnitHelper.AssertIsEquivalent(80.0,
                                           actual [ 3 ],
                                           Constants.EpsilonDistance,
                                           "actual[3]");
        }

        [Test]
        public void MatrixForMultipleLinesLevelZeroTest()
        {
            double[][] multple = m_CostMatrix.MatrixForMultipleLines(m_Lines);
            double[] actual = multple [ 0 ];

            Assert.AreEqual(4,
                            actual.Length,
                            "Length");
            NUnitHelper.AssertIsEquivalent(30.0,
                                           actual [ 0 ],
                                           Constants.EpsilonDistance,
                                           "actual[0]");
            NUnitHelper.AssertIsEquivalent(40.0,
                                           actual [ 1 ],
                                           Constants.EpsilonDistance,
                                           "actual[1]");
            NUnitHelper.AssertIsEquivalent(70.0,
                                           actual [ 2 ],
                                           Constants.EpsilonDistance,
                                           "actual[2]");
            NUnitHelper.AssertIsEquivalent(80.0,
                                           actual [ 3 ],
                                           Constants.EpsilonDistance,
                                           "actual[3]");
        }

        [Test]
        public void MatrixForOneLineLengthTest()
        {
            double[][] actual = m_CostMatrix.MatrixForOneLine();

            Assert.AreEqual(2,
                            actual.Length,
                            "Length");
            Assert.AreEqual(2,
                            actual.GetLength(0),
                            "GetLength(0)");
        }

        [Test]
        public void MatrixForOneLineLevelOneTest()
        {
            double[][] multiple = m_CostMatrix.MatrixForOneLine();
            double[] actual = multiple [ 1 ];

            Assert.AreEqual(CostMatrix.CostToMyself,
                            actual [ 0 ],
                            "actual[0]");
            Assert.AreEqual(CostMatrix.CostToMyself,
                            actual [ 1 ],
                            "actual[1]");
        }

        [Test]
        public void MatrixForOneLineLevelZeroTest()
        {
            double[][] multiple = m_CostMatrix.MatrixForOneLine();
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
                                           Constants.EpsilonDistance,
                                           "actual[0]");
            NUnitHelper.AssertIsEquivalent(20.0,
                                           m_CostMatrix.Matrix [ 1 ] [ 1 ],
                                           Constants.EpsilonDistance,
                                           "actual[1]");
            NUnitHelper.AssertIsEquivalent(50.0,
                                           m_CostMatrix.Matrix [ 1 ] [ 2 ],
                                           Constants.EpsilonDistance,
                                           "actual[2]");
            NUnitHelper.AssertIsEquivalent(60.0,
                                           m_CostMatrix.Matrix [ 1 ] [ 3 ],
                                           Constants.EpsilonDistance,
                                           "actual[3]");
        }

        [Test]
        public void MatrixLevelThreeTest()
        {
            NUnitHelper.AssertIsEquivalent(10.0,
                                           m_CostMatrix.Matrix [ 3 ] [ 0 ],
                                           Constants.EpsilonDistance,
                                           "actual[0]");
            NUnitHelper.AssertIsEquivalent(20.0,
                                           m_CostMatrix.Matrix [ 3 ] [ 1 ],
                                           Constants.EpsilonDistance,
                                           "actual[1]");
            NUnitHelper.AssertIsEquivalent(50.0,
                                           m_CostMatrix.Matrix [ 3 ] [ 2 ],
                                           Constants.EpsilonDistance,
                                           "actual[2]");
            NUnitHelper.AssertIsEquivalent(60.0,
                                           m_CostMatrix.Matrix [ 3 ] [ 3 ],
                                           Constants.EpsilonDistance,
                                           "actual[3]");
        }

        [Test]
        public void MatrixLevelTwoTest()
        {
            NUnitHelper.AssertIsEquivalent(30.0,
                                           m_CostMatrix.Matrix [ 2 ] [ 0 ],
                                           Constants.EpsilonDistance,
                                           "actual[0]");
            NUnitHelper.AssertIsEquivalent(40.0,
                                           m_CostMatrix.Matrix [ 2 ] [ 1 ],
                                           Constants.EpsilonDistance,
                                           "actual[1]");
            NUnitHelper.AssertIsEquivalent(70.0,
                                           m_CostMatrix.Matrix [ 2 ] [ 2 ],
                                           Constants.EpsilonDistance,
                                           "actual[2]");
            NUnitHelper.AssertIsEquivalent(80.0,
                                           m_CostMatrix.Matrix [ 2 ] [ 3 ],
                                           Constants.EpsilonDistance,
                                           "actual[3]");
        }

        [Test]
        public void MatrixLevelZeroTest()
        {
            NUnitHelper.AssertIsEquivalent(30.0,
                                           m_CostMatrix.Matrix [ 0 ] [ 0 ],
                                           Constants.EpsilonDistance,
                                           "actual[0]");
            NUnitHelper.AssertIsEquivalent(40.0,
                                           m_CostMatrix.Matrix [ 0 ] [ 1 ],
                                           Constants.EpsilonDistance,
                                           "actual[1]");
            NUnitHelper.AssertIsEquivalent(70.0,
                                           m_CostMatrix.Matrix [ 0 ] [ 2 ],
                                           Constants.EpsilonDistance,
                                           "actual[2]");
            NUnitHelper.AssertIsEquivalent(80.0,
                                           m_CostMatrix.Matrix [ 0 ] [ 3 ],
                                           Constants.EpsilonDistance,
                                           "actual[3]");
        }

        [Test]
        public void MatrixLineForForwardTest()
        {
            double[] actual = m_CostMatrix.MatrixLineForForward(m_LineToLinesConverter,
                                                                m_Lines);

            Assert.AreEqual(4,
                            actual.Length,
                            "Length");
            NUnitHelper.AssertIsEquivalent(30.0,
                                           actual [ 0 ],
                                           Constants.EpsilonDistance,
                                           "actual[0]");
            NUnitHelper.AssertIsEquivalent(40.0,
                                           actual [ 1 ],
                                           Constants.EpsilonDistance,
                                           "actual[1]");
            NUnitHelper.AssertIsEquivalent(70.0,
                                           actual [ 2 ],
                                           Constants.EpsilonDistance,
                                           "actual[2]");
            NUnitHelper.AssertIsEquivalent(80.0,
                                           actual [ 3 ],
                                           Constants.EpsilonDistance,
                                           "actual[3]");
        }

        [Test]
        public void MatrixLineForReversTest()
        {
            double[] actual = m_CostMatrix.MatrixLineForRevers(m_LineToLinesConverter,
                                                               m_Lines);

            Assert.AreEqual(4,
                            actual.Length,
                            "Length");
            NUnitHelper.AssertIsEquivalent(10.0,
                                           actual [ 0 ],
                                           Constants.EpsilonDistance,
                                           "actual[0]");
            NUnitHelper.AssertIsEquivalent(20.0,
                                           actual [ 1 ],
                                           Constants.EpsilonDistance,
                                           "actual[1]");
            NUnitHelper.AssertIsEquivalent(50.0,
                                           actual [ 2 ],
                                           Constants.EpsilonDistance,
                                           "actual[2]");
            NUnitHelper.AssertIsEquivalent(60.0,
                                           actual [ 3 ],
                                           Constants.EpsilonDistance,
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

        // ReSharper disable MaximumChainedReferences
        [SetUp]
        public void Setup()
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

            m_Lines = new ILine[]
                      {
                          m_Line1,
                          m_Line2
                      };

            ConfigureLineToLinesConverter();

            m_LinesSourceManager = Substitute.For <ILinesSourceManager>();
            m_LinesSourceManager.Lines.Returns(m_Lines);

            m_ConverterFactory = Substitute.For <IConverterFactory>();
            m_ConverterFactory.Create <ILineToLinesConverter>().Returns(m_LineToLinesConverter);

            m_RacetracksSourceManager = Substitute.For <IRacetracksSourceManager>();

            m_CostMatrix = new CostMatrix(Substitute.For <ISelkieLogger>(),
                                          m_LinesSourceManager,
                                          m_RacetracksSourceManager,
                                          m_ConverterFactory);
        }

        private void ConfigureLineToLinesConverter()
        {
            m_LineToLinesConverter = Substitute.For <ILineToLinesConverter>();
            m_LineToLinesConverter.CostStartToStart(m_Line1).Returns(10.0);
            m_LineToLinesConverter.CostStartToEnd(m_Line1).Returns(20.0);
            m_LineToLinesConverter.CostEndToStart(m_Line1).Returns(30.0);
            m_LineToLinesConverter.CostEndToEnd(m_Line1).Returns(40.0);

            m_LineToLinesConverter.CostStartToStart(m_Line2).Returns(50.0);
            m_LineToLinesConverter.CostStartToEnd(m_Line2).Returns(60.0);
            m_LineToLinesConverter.CostEndToStart(m_Line2).Returns(70.0);
            m_LineToLinesConverter.CostEndToEnd(m_Line2).Returns(80.0);
        }

        // ReSharper restore MaximumChainedReferences
    }

    // ReSharper restore ClassTooBig
}