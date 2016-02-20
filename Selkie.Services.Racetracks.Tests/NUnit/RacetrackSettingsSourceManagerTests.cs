using System;
using System.Diagnostics.CodeAnalysis;
using NSubstitute;
using NUnit.Framework;
using Selkie.EasyNetQ;
using Selkie.NUnit.Extensions;
using Selkie.Services.Racetracks.Common.Messages;
using Selkie.Windsor;

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
            m_Logger = Substitute.For <ISelkieLogger>();
            m_Bus = Substitute.For <ISelkieBus>();

            m_Manager = new RacetrackSettingsSourceManager(m_Logger,
                                                           m_Bus);
        }

        private ISelkieBus m_Bus;
        private ISelkieLogger m_Logger;
        private RacetrackSettingsSourceManager m_Manager;

        [Test]
        public void DefaultSourceTest()
        {
            Assert.NotNull(m_Manager.Source);
        }

        [Test]
        public void RacetrackSettingsGetHandlerSendsMessageTest()
        {
            var message = new RacetrackSettingsGetMessage();

            m_Manager.RacetrackSettingsGetHandler(message);

            m_Bus.Received().PublishAsync(Arg.Any <RacetrackSettingsChangedMessage>());
        }

        [Test]
        public void RacetrackSettingsSetHandlerCreatesNewSourceTest()
        {
            var message = new RacetrackSettingsSetMessage
                          {
                              TurnRadiusForPort = 100.0,
                              TurnRadiusForStarboard = 200.0,
                              IsPortTurnAllowed = true,
                              IsStarboardTurnAllowed = true
                          };

            m_Manager.RacetrackSettingsSetHandler(message);

            IRacetrackSettingsSource actual = m_Manager.Source;

            NUnitHelper.AssertIsEquivalent(100.0,
                                           actual.TurnRadiusForPort);
        }

        [Test]
        public void RacetrackSettingsSetHandlerSendsMessageTest()
        {
            var message = new RacetrackSettingsSetMessage
                          {
                              TurnRadiusForPort = 100.0,
                              TurnRadiusForStarboard = 200.0,
                              IsPortTurnAllowed = true,
                              IsStarboardTurnAllowed = true
                          };

            m_Manager.RacetrackSettingsSetHandler(message);

            m_Bus.Received().PublishAsync(Arg.Any <RacetrackSettingsChangedMessage>());
        }

        [Test]
        public void RacetrackSettingsSetHandlerThrowsForTurnRadiusForPortIsNegativeTest()
        {
            var message = new RacetrackSettingsSetMessage
                          {
                              TurnRadiusForPort = -1.0,
                              TurnRadiusForStarboard = 1.0,
                              IsPortTurnAllowed = true,
                              IsStarboardTurnAllowed = true
                          };

            Assert.Throws <ArgumentException>(() => m_Manager.RacetrackSettingsSetHandler(message));
        }

        [Test]
        public void RacetrackSettingsSetHandlerThrowsForTurnRadiusForPortIsZeroTest()
        {
            var message = new RacetrackSettingsSetMessage
                          {
                              TurnRadiusForPort = 0.0,
                              TurnRadiusForStarboard = 1.0,
                              IsPortTurnAllowed = true,
                              IsStarboardTurnAllowed = true
                          };

            Assert.Throws <ArgumentException>(() => m_Manager.RacetrackSettingsSetHandler(message));
        }

        [Test]
        public void RacetrackSettingsSetHandlerThrowsForTurnRadiusForStarboardIsNegativeTest()
        {
            var message = new RacetrackSettingsSetMessage
                          {
                              TurnRadiusForPort = 1.0,
                              TurnRadiusForStarboard = -1.0,
                              IsPortTurnAllowed = true,
                              IsStarboardTurnAllowed = true
                          };

            Assert.Throws <ArgumentException>(() => m_Manager.RacetrackSettingsSetHandler(message));
        }

        [Test]
        public void RacetrackSettingsSetHandlerThrowsForTurnRadiusForStarboardIsZeroTest()
        {
            var message = new RacetrackSettingsSetMessage
                          {
                              TurnRadiusForPort = 1.0,
                              TurnRadiusForStarboard = 0.0,
                              IsPortTurnAllowed = true,
                              IsStarboardTurnAllowed = true
                          };

            Assert.Throws <ArgumentException>(() => m_Manager.RacetrackSettingsSetHandler(message));
        }

        [Test]
        public void SendRacetrackSettingsChangedMessageSendsMessageTest()
        {
            var source = Substitute.For <IRacetrackSettingsSource>();
            source.TurnRadiusForPort.Returns(1.0);
            source.TurnRadiusForStarboard.Returns(2.0);
            source.IsPortTurnAllowed.Returns(true);
            source.IsStarboardTurnAllowed.Returns(true);

            m_Manager.SendRacetrackSettingsChangedMessage(source);

            m_Bus.Received()
                 .PublishAsync(
                               Arg.Is <RacetrackSettingsChangedMessage>(
                                                                        x =>
                                                                        Math.Abs(x.TurnRadiusForPort - 1.0) <
                                                                        0.1 &&
                                                                        Math.Abs(x.TurnRadiusForStarboard - 2.0) <
                                                                        0.1 &&
                                                                        x.IsPortTurnAllowed &&
                                                                        x.IsStarboardTurnAllowed));
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
        public void SubscribesToRacetrackSettingsGetMessageTest()
        {
            m_Bus.Received().SubscribeAsync(m_Manager.GetType().ToString(),
                                            Arg.Any <Action <RacetrackSettingsGetMessage>>());
        }

        [Test]
        public void SubscribesToRacetrackSettingsSetMessageTest()
        {
            m_Bus.Received().SubscribeAsync(m_Manager.GetType().ToString(),
                                            Arg.Any <Action <RacetrackSettingsSetMessage>>());
        }
    }
}