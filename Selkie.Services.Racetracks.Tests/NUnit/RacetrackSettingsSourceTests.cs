using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;
using Selkie.Geometry.Primitives;

namespace Selkie.Services.Racetracks.Tests.NUnit
{
    //ncrunch: no coverage start
    [TestFixture]
    [ExcludeFromCodeCoverage]
    internal sealed class RacetrackSettingsSourceTests
    {
        [SetUp]
        public void Setup()
        {
            m_Source = new RacetrackSettingsSource(new Distance(100.0),
                                                   true,
                                                   true);
        }

        private RacetrackSettingsSource m_Source;

        [Test]
        public void DefaultTest()
        {
            RacetrackSettingsSource actual = RacetrackSettingsSource.Default;

            Assert.AreEqual(RacetrackSettingsSource.DefaultRadius,
                            actual.TurnRadius,
                            "TurnRadiusInMeters");
            Assert.True(m_Source.IsPortTurnAllowed,
                        "IsPortTurnAllowed");
            Assert.True(m_Source.IsStarboardTurnAllowed,
                        "IsStarboardTurnAllowed");
        }

        [Test]
        public void IsPortTurnAllowedTest()
        {
            Assert.True(m_Source.IsPortTurnAllowed);
        }

        [Test]
        public void IsStarboardTurnAllowedTest()
        {
            Assert.True(m_Source.IsStarboardTurnAllowed);
        }

        [Test]
        public void TurnRadiusTest()
        {
            Assert.AreEqual(100.0,
                            m_Source.TurnRadius.Length);
        }
    }
}