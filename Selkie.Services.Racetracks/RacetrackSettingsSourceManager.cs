using System;
using Castle.Core;
using Castle.Core.Logging;
using EasyNetQ;
using JetBrains.Annotations;
using Selkie.EasyNetQ.Extensions;
using Selkie.Geometry.Primitives;
using Selkie.Services.Racetracks.Common.Messages;
using Selkie.Windsor;
using Selkie.Windsor.Extensions;

namespace Selkie.Services.Racetracks
{
    [ProjectComponent(Lifestyle.Singleton)]
    public class RacetrackSettingsSourceManager
        : IRacetrackSettingsSourceManager,
          IStartable
    {
        private readonly IBus m_Bus;
        private readonly ILogger m_Logger;
        private IRacetrackSettingsSource m_Source = RacetrackSettingsSource.Default;

        public RacetrackSettingsSourceManager([NotNull] ILogger logger,
                                              [NotNull] IBus bus)
        {
            m_Logger = logger;
            m_Bus = bus;

            bus.SubscribeHandlerAsync <RacetrackSettingsSetMessage>(logger,
                                                                    GetType().ToString(),
                                                                    RacetrackSettingsSetMessageHandler);
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

        internal void RacetrackSettingsSetMessageHandler([NotNull] RacetrackSettingsSetMessage message)
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

            m_Bus.PublishAsync(new RacetrackSettingsChangedMessage
                               {
                                   TurnRadiusInMetres = m_Source.TurnRadius.Length,
                                   IsPortTurnAllowed = m_Source.IsPortTurnAllowed,
                                   IsStarboardTurnAllowed = m_Source.IsStarboardTurnAllowed
                               });
        }
    }
}