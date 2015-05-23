using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Castle.Core.Logging;
using EasyNetQ;
using NSubstitute;
using NUnit.Framework;
using Selkie.Geometry.Primitives;
using Selkie.Services.Racetracks.Common.Messages;

namespace Selkie.Services.Racetracks.Tests.NUnit
{
    //ncrunch: no coverage start
    [TestFixture]
    [ExcludeFromCodeCoverage]
    internal sealed class RacetrackSettingsSourceManagerTests
    {
        [SetUp]
        public void Setup()
        {
            m_Logger = Substitute.For <ILogger>();
            m_Bus = Substitute.For <IBus>();

            m_Manager = new RacetrackSettingsSourceManager(m_Logger,
                                                           m_Bus);
        }

        private IBus m_Bus;
        private ILogger m_Logger;
        private RacetrackSettingsSourceManager m_Manager;

        [Test]
        public void DefaultSourceTest()
        {
            Assert.NotNull(m_Manager.Source);
        }

        [Test]
        public void RacetrackSettingsSetMessageHandlerCreatesNewSourceTest()
        {
            var turnRadius = new Distance(100.0);
            var message = new RacetrackSettingsSetMessage
                          {
                              TurnRadiusInMetres = turnRadius.Length,
                              IsPortTurnAllowed = true,
                              IsStarboardTurnAllowed = true
                          };

            m_Manager.RacetrackSettingsSetMessageHandler(message);

            IRacetrackSettingsSource actual = m_Manager.Source;

            Assert.AreEqual(turnRadius,
                            actual.TurnRadius);
        }

        [Test]
        public void RacetrackSettingsSetMessageHandlerSendsMessageTest()
        {
            var message = new RacetrackSettingsSetMessage
                          {
                              TurnRadiusInMetres = 100.0,
                              IsPortTurnAllowed = true,
                              IsStarboardTurnAllowed = true
                          };

            m_Manager.RacetrackSettingsSetMessageHandler(message);

            m_Bus.Received().PublishAsync(Arg.Any <RacetrackSettingsChangedMessage>());
        }

        [Test]
        public void RacetrackSettingsSetMessageHandlerThrowsForTurnRadiusInMetresIsNegativeTest()
        {
            var turnRadius = new Distance(-1.0);
            var message = new RacetrackSettingsSetMessage
                          {
                              TurnRadiusInMetres = turnRadius.Length,
                              IsPortTurnAllowed = true,
                              IsStarboardTurnAllowed = true
                          };

            Assert.Throws <ArgumentException>(() => m_Manager.RacetrackSettingsSetMessageHandler(message));
        }

        [Test]
        public void RacetrackSettingsSetMessageHandlerThrowsForTurnRadiusInMetresIsZeroTest()
        {
            var turnRadius = new Distance(0.0);
            var message = new RacetrackSettingsSetMessage
                          {
                              TurnRadiusInMetres = turnRadius.Length,
                              IsPortTurnAllowed = true,
                              IsStarboardTurnAllowed = true
                          };

            Assert.Throws <ArgumentException>(() => m_Manager.RacetrackSettingsSetMessageHandler(message));
        }

        [Test]
        public void StartCallsLoggerTest()
        {
            m_Manager.Start();

            m_Logger.Received().Info(Arg.Is <string>(x => x.StartsWith("Started")));
        }

        [Test]
        public void StopCallsLoggerTest()
        {
            m_Manager.Stop();

            m_Logger.Received().Info(Arg.Is <string>(x => x.StartsWith("Stopped")));
        }

        [Test]
        public void SubscribesToPRacetrackSettingsSetMessageTest()
        {
            m_Bus.Received().SubscribeAsync(m_Manager.GetType().ToString(),
                                            Arg.Any <Func <RacetrackSettingsSetMessage, Task>>());
        }
    }
}