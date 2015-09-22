using System;
using Castle.Core;
using JetBrains.Annotations;
using Selkie.Aop.Aspects;
using Selkie.EasyNetQ;
using Selkie.Geometry.Primitives;
using Selkie.Services.Racetracks.Common.Messages;
using Selkie.Windsor;
using Selkie.Windsor.Extensions;

namespace Selkie.Services.Racetracks
{
    // todo move message handlers into separate classes, check all other classes as well
    [Interceptor(typeof(MessageHandlerAspect))]
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

        internal void RacetrackSettingsGetHandler(RacetrackSettingsGetMessage obj)
        {
            SendRacetrackSettingsChangedMessage(m_Source);
        }

        internal void RacetrackSettingsSetHandler([NotNull] RacetrackSettingsSetMessage message)
        {
            double turnRadiusInMetres = message.TurnRadiusInMetres;

            if ( message.TurnRadiusInMetres <= 0.0 )
            {
                string text = "Turn radius in meters is '{0}' but it can't be 0 or negative!".Inject(turnRadiusInMetres);

                throw new ArgumentException(text,
                                            "message");
            }

            HandleValidRacetrackSettingsMessage(message);
        }

        private void HandleValidRacetrackSettingsMessage([NotNull] RacetrackSettingsSetMessage message)
        {
            var turnRadius = new Distance(message.TurnRadiusInMetres);

            m_Source = new RacetrackSettingsSource(turnRadius,
                                                   message.IsPortTurnAllowed,
                                                   message.IsStarboardTurnAllowed);

            LogRacetrackSettings(m_Source);

            SendRacetrackSettingsChangedMessage(m_Source);
        }

        internal void SendRacetrackSettingsChangedMessage([NotNull] IRacetrackSettingsSource source)
        {
            m_Bus.PublishAsync(new RacetrackSettingsChangedMessage
                               {
                                   TurnRadiusInMetres = source.TurnRadius.Length,
                                   IsPortTurnAllowed = source.IsPortTurnAllowed,
                                   IsStarboardTurnAllowed = source.IsStarboardTurnAllowed
                               });
        }

        private void LogRacetrackSettings([NotNull] IRacetrackSettingsSource source)
        {
            string text = "[RacetrackSettingsSourceManager] " +
                          "Racetrack Settings: TurnRadius = {0} " +
                          "IsPortTurnAllowed = {1} " +
                          "IsStarboardTurnAllowed = {2}";

            m_Logger.Info(text.Inject(source.TurnRadius,
                                      source.IsPortTurnAllowed,
                                      source.IsStarboardTurnAllowed));
        }
    }
}