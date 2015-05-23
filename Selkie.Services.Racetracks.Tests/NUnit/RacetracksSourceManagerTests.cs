using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using NSubstitute;
using NUnit.Framework;
using Selkie.Geometry.Primitives;
using Selkie.Geometry.Shapes;
using Selkie.Racetrack.Calculators;

namespace Selkie.Services.Racetracks.Tests.NUnit
{
    [SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable")]
    [TestFixture]
    //ncrunch: no coverage start
    [ExcludeFromCodeCoverage]
    internal sealed class RacetracksSourceManagerTests
    {
        [SetUp]
        public void Setup()
        {
            m_RacetrackSettingsSource = Substitute.For <IRacetrackSettingsSource>();
            m_RacetrackSettingsSource.TurnRadius.Returns(new Distance(30.0));

            m_Lines = new ILine[]
                      {
                      };

            m_LinesSourceManager = Substitute.For <ILinesSourceManager>();
            IEnumerable <ILine> lines = m_LinesSourceManager.Lines;
            lines.Returns(m_Lines);

            m_RacetrackSettingsSourceManager = Substitute.For <IRacetrackSettingsSourceManager>();
            m_RacetrackSettingsSourceManager.Source.Returns(m_RacetrackSettingsSource);

            m_RacetracksCalculator = Substitute.For <IRacetracksCalculator>();
            m_Factory = Substitute.For <ICalculatorFactory>();
            m_Factory.Create <IRacetracksCalculator>().Returns(m_RacetracksCalculator);

            m_Manager = new RacetracksSourceManager(m_LinesSourceManager,
                                                    m_RacetrackSettingsSourceManager,
                                                    m_Factory);
        }

        [TearDown]
        public void TearDown()
        {
            m_Manager.Dispose();
        }

        private ICalculatorFactory m_Factory;
        private ILine[] m_Lines;
        private ILinesSourceManager m_LinesSourceManager;
        private RacetracksSourceManager m_Manager;
        private IRacetracksCalculator m_RacetracksCalculator;
        private IRacetrackSettingsSource m_RacetrackSettingsSource;
        private IRacetrackSettingsSourceManager m_RacetrackSettingsSourceManager;

        [Test]
        public void CalculateCallsCalculateTest()
        {
            m_Manager.Update();

            m_RacetracksCalculator.Received().Calculate();
        }

        [Test]
        public void CalculateCallsCreateTest()
        {
            m_Manager.Update();

            m_Factory.Received().Create <IRacetracksCalculator>();
        }

        [Test]
        public void CalculateCallsReleaseTest()
        {
            m_Manager.Update();

            m_Factory.Received().Release(m_RacetracksCalculator);
        }

        [Test]
        public void CalculateSetsLinesTest()
        {
            m_Manager.Update();

            Assert.AreEqual(m_LinesSourceManager.Lines,
                            m_RacetracksCalculator.Lines);
        }

        [Test]
        public void CalculateSetsRadiusTest()
        {
            m_Manager.Update();

            Assert.AreEqual(m_RacetrackSettingsSourceManager.Source.TurnRadius,
                            m_RacetracksCalculator.Radius);
        }

        [Test]
        public void ConstructorCallsCalculateTest()
        {
            m_RacetracksCalculator.Received().Calculate();
        }

        [Test]
        public void ConstructorSetsLinesTest()
        {
            Assert.AreEqual(m_Lines,
                            m_RacetracksCalculator.Lines);
        }

        [Test]
        public void ConstructorSetsRadiusTest()
        {
            Assert.AreEqual(new Distance(30.0),
                            m_RacetracksCalculator.Radius);
        }

        [Test]
        public void DisposeCallsReleaseTest()
        {
            var manager = new RacetracksSourceManager(m_LinesSourceManager,
                                                      m_RacetrackSettingsSourceManager,
                                                      m_Factory);

            manager.Dispose();

            m_Factory.Received().Release(m_RacetracksCalculator);
        }

        [Test]
        public void RacetracksTest()
        {
            Assert.AreEqual(m_RacetracksCalculator,
                            m_Manager.Racetracks);
        }
    }
}