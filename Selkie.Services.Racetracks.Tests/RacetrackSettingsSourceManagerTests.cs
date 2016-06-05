using System;
using System.Diagnostics.CodeAnalysis;
using NSubstitute;
using Selkie.Services.Racetracks.Interfaces;
using Selkie.Windsor;
using Selkie.XUnit.Extensions;
using Xunit;

namespace Selkie.Services.Racetracks.Tests
{
    [ExcludeFromCodeCoverage]
    public sealed class RacetrackSettingsSourceManagerTests
    {
        public RacetrackSettingsSourceManagerTests()
        {
            m_Logger = Substitute.For <ISelkieLogger>();

            m_Manager = new RacetrackSettingsSourceManager(m_Logger);
        }

        private readonly ISelkieLogger m_Logger;
        private readonly RacetrackSettingsSourceManager m_Manager;

        [Fact]
        public void SetSettings_SetsTurnRadiusForPort_WhenCalled()
        {
            var settings = new RacetrackSettings
                           {
                               TurnRadiusForPort = 100.0,
                               TurnRadiusForStarboard = 200.0,
                               IsPortTurnAllowed = true,
                               IsStarboardTurnAllowed = true
                           };

            m_Manager.SetSettings(settings);

            IRacetrackSettingsSource actual = m_Manager.Source;

            XUnitHelper.AssertIsEquivalent(100.0,
                                           actual.TurnRadiusForPort);
        }

        [Fact]
        public void SetSettings_ThrowsException_ForTurnRadiusForPortIsNegative()
        {
            var settings = new RacetrackSettings
                           {
                               TurnRadiusForPort = -1.0,
                               TurnRadiusForStarboard = 1.0,
                               IsPortTurnAllowed = true,
                               IsStarboardTurnAllowed = true
                           };

            Assert.Throws <ArgumentException>(() => m_Manager.SetSettings(settings));
        }

        [Fact]
        public void SetSettings_ThrowsException_ForTurnRadiusForPortIsZero()
        {
            var settings = new RacetrackSettings
                           {
                               TurnRadiusForPort = 0.0,
                               TurnRadiusForStarboard = 1.0,
                               IsPortTurnAllowed = true,
                               IsStarboardTurnAllowed = true
                           };

            Assert.Throws <ArgumentException>(() => m_Manager.SetSettings(settings));
        }

        [Fact]
        public void SetSettings_ThrowsException_ForTurnRadiusForStarboardIsNegative()
        {
            var settings = new RacetrackSettings
                           {
                               TurnRadiusForPort = 1.0,
                               TurnRadiusForStarboard = -1.0,
                               IsPortTurnAllowed = true,
                               IsStarboardTurnAllowed = true
                           };

            Assert.Throws <ArgumentException>(() => m_Manager.SetSettings(settings));
        }

        [Fact]
        public void SetSettings_ThrowsException_ForTurnRadiusForStarboardIsZero()
        {
            var settings = new RacetrackSettings
                           {
                               TurnRadiusForPort = 1.0,
                               TurnRadiusForStarboard = 0.0,
                               IsPortTurnAllowed = true,
                               IsStarboardTurnAllowed = true
                           };

            Assert.Throws <ArgumentException>(() => m_Manager.SetSettings(settings));
        }

        [Fact]
        public void Source_ReturnsDefault_WhenCalled()
        {
            Assert.NotNull(m_Manager.Source);
        }

        [Fact]
        public void Start_CallsLogger_WhenCalled()
        {
            m_Manager.Start();

            m_Logger.Received().Info(Arg.Is <string>(x => x.StartsWith("Started")));
        }

        [Fact]
        public void Stop_CallsLogger_WhenCalled()
        {
            m_Manager.Stop();

            m_Logger.Received().Info(Arg.Is <string>(x => x.StartsWith("Stopped")));
        }
    }
}