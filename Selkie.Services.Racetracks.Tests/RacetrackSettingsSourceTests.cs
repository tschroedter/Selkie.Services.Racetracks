using System.Diagnostics.CodeAnalysis;
using Selkie.XUnit.Extensions;
using Xunit;

namespace Selkie.Services.Racetracks.Tests
{
    [ExcludeFromCodeCoverage]
    internal sealed class RacetrackSettingsSourceTests
    {
        public RacetrackSettingsSourceTests()
        {
            m_Source = new RacetrackSettingsSource(100.0,
                                                   200.0,
                                                   true,
                                                   true);
        }

        private readonly RacetrackSettingsSource m_Source;

        [Fact]
        public void DefaultTest()
        {
            RacetrackSettingsSource actual = RacetrackSettingsSource.Default;

            XUnitHelper.AssertIsEquivalent(RacetrackSettingsSource.DefaultRadius,
                                           actual.TurnRadiusForPort,
                                           "TurnRadiusForPortTurnInMetres");
            XUnitHelper.AssertIsEquivalent(RacetrackSettingsSource.DefaultRadius,
                                           actual.TurnRadiusForStarboard,
                                           "TurnRadiusForStarboard");
            Assert.True(m_Source.IsPortTurnAllowed,
                        "IsPortTurnAllowed");
            Assert.True(m_Source.IsStarboardTurnAllowed,
                        "IsStarboardTurnAllowed");
        }

        [Fact]
        public void IsPortTurnAllowedTest()
        {
            Assert.True(m_Source.IsPortTurnAllowed);
        }

        [Fact]
        public void IsStarboardTurnAllowedTest()
        {
            Assert.True(m_Source.IsStarboardTurnAllowed);
        }

        [Fact]
        public void TurnRadiusForPortTurnInMetresTest()
        {
            XUnitHelper.AssertIsEquivalent(100.0,
                                           m_Source.TurnRadiusForPort);
        }

        [Fact]
        public void TurnRadiusForStarboardTurnInMetresTest()
        {
            XUnitHelper.AssertIsEquivalent(200.0,
                                           m_Source.TurnRadiusForStarboard);
        }
    }
}