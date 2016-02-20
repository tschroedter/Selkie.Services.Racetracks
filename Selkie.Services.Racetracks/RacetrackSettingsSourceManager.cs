using System;
using Castle.Core;
using JetBrains.Annotations;
using Selkie.Aop.Aspects;
using Selkie.EasyNetQ;
using Selkie.Services.Racetracks.Common.Messages;
using Selkie.Windsor;
using Selkie.Windsor.Extensions;

namespace Selkie.Services.Racetracks
{
    // todo move message handlers into separate classes, check all other classes as well
    [Interceptor(typeof ( MessageHandlerAspect ))]
    [ProjectComponent(Lifestyle.Singleton)]
    public class RacetrackSettingsSourceManager
        : IRacetrackSettingsSourceManager,
          IStartable
    {
        private readonly ISelkieBus m_Bus;
        private readonly ISelkieLogger m_Logger;
        private IRacetrackSettingsSource m_Source = RacetrackSettingsSource.Default;

        public RacetrackSettingsSourceManager([NotNull] ISelkieLogger logger,
                                              [NotNull] ISelkieBus bus)
        {
            m_Logger = logger;
            m_Bus = bus;

            string subscriptionId = GetType().ToString();

            bus.SubscribeAsync <RacetrackSettingsSetMessage>(subscriptionId,
                                                             RacetrackSettingsSetHandler);

            bus.SubscribeAsync <RacetrackSettingsGetMessage>(subscriptionId,
                                                             RacetrackSettingsGetHandler);
        }

        public IRacetrackSettingsSource Source
        {
            get
            {
                return m_Source;
            }
        }

        public void Start()
        {
            m_Logger.Info("Started '{0}'!".Inject(GetType().FullName));
        }

        public void Stop()
        {
            m_Logger.Info("Stopped '{0}'!".Inject(GetType().FullName));
        }

        internal void RacetrackSettingsGetHandler([NotNull] RacetrackSettingsGetMessage message)
        {
            SendRacetrackSettingsChangedMessage(m_Source);
        }

        internal void RacetrackSettingsSetHandler([NotNull] RacetrackSettingsSetMessage message)
        {
            if ( message.TurnRadiusForPort <= 0.0 )
            {
                string text = "Turn radius for port turn in meters is '{0}' " +
                              "but it can't be 0 or negative!".Inject(message.TurnRadiusForPort);

                throw new ArgumentException(text,
                                            "message");
            }

            if ( message.TurnRadiusForStarboard <= 0.0 )
            {
                string text = "Turn radius for starboard turn in meters is '{0}' " +
                              "but it can't be 0 or negative!".Inject(message.TurnRadiusForStarboard);

                throw new ArgumentException(text,
                                            "message");
            }

            HandleValidRacetrackSettingsMessage(message);
        }

        private void HandleValidRacetrackSettingsMessage([NotNull] RacetrackSettingsSetMessage message)
        {
            m_Source = new RacetrackSettingsSource(message.TurnRadiusForPort,
                                                   message.TurnRadiusForStarboard,
                                                   message.IsPortTurnAllowed,
                                                   message.IsStarboardTurnAllowed);

            LogRacetrackSettings(m_Source);

            SendRacetrackSettingsChangedMessage(m_Source);
        }

        internal void SendRacetrackSettingsChangedMessage([NotNull] IRacetrackSettingsSource source)
        {
            m_Bus.PublishAsync(new RacetrackSettingsChangedMessage
                               {
                                   TurnRadiusForPort = source.TurnRadiusForPort,
                                   TurnRadiusForStarboard = source.TurnRadiusForStarboard,
                                   IsPortTurnAllowed = source.IsPortTurnAllowed,
                                   IsStarboardTurnAllowed = source.IsStarboardTurnAllowed
                               });
        }

        private void LogRacetrackSettings([NotNull] IRacetrackSettingsSource source)
        {
            const string text = "[RacetrackSettingsSourceManager] " +
                                "Racetrack Settings: TurnRadiusForPort = {0} " +
                                "TurnRadiusForStarboard = {1} " +
                                "IsPortTurnAllowed = {2} " +
                                "IsStarboardTurnAllowed = {3}";

            m_Logger.Info(text.Inject(source.TurnRadiusForPort,
                                      source.TurnRadiusForStarboard,
                                      source.IsPortTurnAllowed,
                                      source.IsStarboardTurnAllowed));
        }
    }
}