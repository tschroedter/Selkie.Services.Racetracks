using System;
using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;
using Selkie.NUnit.Extensions;

namespace Selkie.Services.Racetracks.Tests
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    internal sealed class RacetrackSettingsSourceTests
    {
        public RacetrackSettingsSourceTests()
        {
            m_ColonyId = Guid.Parse("00000000-0000-0000-0000-000000000001");

            m_Sut =
                new RacetrackSettingsSource(
                    m_ColonyId,
                    100.0,
                    200.0,
                    true,
                    true);
        }

        private readonly Guid m_ColonyId;

        private readonly RacetrackSettingsSource m_Sut;

        [Test]
        public void ColonyIdTest()
        {
            Assert.AreEqual(m_ColonyId,
                            m_Sut.ColonyId);
        }

        [Test]
        public void DefaultTest()
        {
            RacetrackSettingsSource sut = RacetrackSettingsSource.Default;

            Assert.AreEqual(Guid.Empty,
                            sut.ColonyId,
                            "ColonyId");
            NUnitHelper.AssertIsEquivalent(RacetrackSettingsSource.DefaultRadius,
                                           sut.TurnRadiusForPort,
                                           "TurnRadiusForPortTurnInMetres");
            NUnitHelper.AssertIsEquivalent(RacetrackSettingsSource.DefaultRadius,
                                           sut.TurnRadiusForStarboard,
                                           "TurnRadiusForStarboard");
            Assert.True(sut.IsPortTurnAllowed,
                        "IsPortTurnAllowed");
            Assert.True(sut.IsStarboardTurnAllowed,
                        "IsStarboardTurnAllowed");
        }

        [Test]
        public void IsPortTurnAllowedTest()
        {
            Assert.True(m_Sut.IsPortTurnAllowed);
        }

        [Test]
        public void IsStarboardTurnAllowedTest()
        {
            Assert.True(m_Sut.IsStarboardTurnAllowed);
        }

        [Test]
        public void TurnRadiusForPortTurnInMetresTest()
        {
            NUnitHelper.AssertIsEquivalent(100.0,
                                           m_Sut.TurnRadiusForPort);
        }

        [Test]
        public void TurnRadiusForStarboardTurnInMetresTest()
        {
            NUnitHelper.AssertIsEquivalent(200.0,
                                           m_Sut.TurnRadiusForStarboard);
        }
    }
}