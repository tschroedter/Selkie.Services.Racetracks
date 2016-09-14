using System;
using Castle.Core;
using JetBrains.Annotations;
using Selkie.Aop.Aspects;
using Selkie.Services.Racetracks.Interfaces;
using Selkie.Windsor;
using Selkie.Windsor.Extensions;

namespace Selkie.Services.Racetracks
{
    // todo move message handlers into separate classes, check all other classes as well
    [Interceptor(typeof( MessageHandlerAspect ))]
    [ProjectComponent(Lifestyle.Singleton)]
    public class RacetrackSettingsSourceManager
        : IRacetrackSettingsSourceManager,
          IStartable
    {
        public RacetrackSettingsSourceManager([NotNull] ISelkieLogger logger)
        {
            m_Logger = logger;
        }

        private readonly ISelkieLogger m_Logger;
        private IRacetrackSettingsSource m_Source = RacetrackSettingsSource.Default;

        public IRacetrackSettingsSource Source
        {
            get
            {
                return m_Source;
            }
        }

        public void SetSettings(RacetrackSettings settings)
        {
            ValidateSettings(settings);

            HandleValidRacetrackSettingsMessage(settings);
        }

        public void Start()
        {
            m_Logger.Info("Started '{0}'!".Inject(GetType().FullName));
        }

        public void Stop()
        {
            m_Logger.Info("Stopped '{0}'!".Inject(GetType().FullName));
        }

        private static void ValidateSettings(RacetrackSettings settings)
        {
            if ( settings.ColonyId == Guid.Empty )
            {
                string text = "ColonyId '{0}' can't be Guid.Empty!".Inject(settings.ColonyId);

                throw new ArgumentException(text,
                                            "settings");
            }

            if ( settings.TurnRadiusForPort <= 0.0 )
            {
                string text = "Turn radius for port turn in meters is '{0}' " +
                              "but it can't be 0 or negative!".Inject(settings.TurnRadiusForPort);

                throw new ArgumentException(text,
                                            "settings");
            }

            if ( !( settings.TurnRadiusForStarboard <= 0.0 ) )
            {
                return;
            }

            string message = "Turn radius for starboard turn in meters is '{0}' " +
                             "but it can't be 0 or negative!".Inject(settings.TurnRadiusForStarboard);

            throw new ArgumentException(message,
                                        "settings");
        }

        private void HandleValidRacetrackSettingsMessage([NotNull] RacetrackSettings settings)
        {
            m_Source = new RacetrackSettingsSource(settings.ColonyId,
                                                   settings.TurnRadiusForPort,
                                                   settings.TurnRadiusForStarboard,
                                                   settings.IsPortTurnAllowed,
                                                   settings.IsStarboardTurnAllowed);

            LogRacetrackSettings(m_Source);
        }

        private void LogRacetrackSettings([NotNull] IRacetrackSettingsSource source)
        {
            const string text = "[RacetrackSettingsSourceManager] " +
                                "ColonyId: {0}" +
                                "Racetrack Settings: TurnRadiusForPort = {1} " +
                                "TurnRadiusForStarboard = {2} " +
                                "IsPortTurnAllowed = {3} " +
                                "IsStarboardTurnAllowed = {4}";

            m_Logger.Info(text.Inject(source.ColonyId,
                                      source.TurnRadiusForPort,
                                      source.TurnRadiusForStarboard,
                                      source.IsPortTurnAllowed,
                                      source.IsStarboardTurnAllowed));
        }
    }
}