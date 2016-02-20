using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;
using Selkie.NUnit.Extensions;

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
            m_Source = new RacetrackSettingsSource(100.0,
                                                   200.0,
                                                   true,
                                                   true);
        }

        private RacetrackSettingsSource m_Source;

        [Test]
        public void DefaultTest()
        {
            RacetrackSettingsSource actual = RacetrackSettingsSource.Default;

            NUnitHelper.AssertIsEquivalent(RacetrackSettingsSource.DefaultRadius,
                                           actual.TurnRadiusForPort,
                                           "TurnRadiusForPortTurnInMetres");
            NUnitHelper.AssertIsEquivalent(RacetrackSettingsSource.DefaultRadius,
                                           actual.TurnRadiusForStarboard,
                                           "TurnRadiusForStarboard");
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
        public void TurnRadiusForPortTurnInMetresTest()
        {
            NUnitHelper.AssertIsEquivalent(100.0,
                                           m_Source.TurnRadiusForPort);
        }

        [Test]
        public void TurnRadiusForStarboardTurnInMetresTest()
        {
            NUnitHelper.AssertIsEquivalent(200.0,
                                           m_Source.TurnRadiusForStarboard);
        }
    }
}