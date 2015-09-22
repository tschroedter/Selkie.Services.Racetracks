using System;
using Castle.Core;
using JetBrains.Annotations;
using Selkie.Aop.Aspects;
using Selkie.EasyNetQ;
using Selkie.Racetrack;
using Selkie.Racetrack.Calculators;
using Selkie.Services.Racetracks.Common.Dto;
using Selkie.Services.Racetracks.Common.Messages;
using Selkie.Windsor;
using Selkie.Windsor.Extensions;

namespace Selkie.Services.Racetracks
{
    // todo discovered problem when turn circle is 40 and distance between lines is 30 will not find a path
    //      -=> at the moment the algorithm can only handle circles, circle-line-circle, but not circle-line-circle ->line-> circle-line-circle
    [Interceptor(typeof(MessageHandlerAspect))]
    [ProjectComponent(Lifestyle.Singleton)]
    public sealed class RacetracksSourceManager
        : IRacetracksSourceManager,
          IDisposable
    {
        private readonly ISelkieBus m_Bus;
        private readonly IRacetracksToDtoConverter m_Converter;
        private readonly ICalculatorFactory m_Factory;
        private readonly ILinesSourceManager m_LinesSourceManager;
        private readonly ISelkieLogger m_Logger;
        private readonly IRacetrackSettingsSourceManager m_RacetrackSettingsSourceManager;
        private IRacetracksCalculator m_RacetracksCalculator;

        public RacetracksSourceManager([NotNull] ISelkieLogger logger,
                                       [NotNull] ISelkieBus bus,
                                       [NotNull] ILinesSourceManager linesSourceManager,
                                       [NotNull] IRacetrackSettingsSourceManager racetrackSettingsSourceManager,
                                       [NotNull] ICalculatorFactory factory,
                                       [NotNull] IRacetracksToDtoConverter converter)
        {
            m_Logger = logger;
            m_Bus = bus;
            m_LinesSourceManager = linesSourceManager;
            m_RacetrackSettingsSourceManager = racetrackSettingsSourceManager;
            m_Factory = factory;
            m_Converter = converter;

            string subscriptionId = GetType().FullName;
            m_Bus.SubscribeAsync <RacetrackSettingsChangedMessage>(subscriptionId,
                                                                   RacetrackSettingsChangedHandler);

            m_Bus.SubscribeAsync <RacetracksGetMessage>(subscriptionId,
                                                        RacetracksGetHandler);

            m_Bus.PublishAsync(new RacetrackSettingsGetMessage());
        }

        public void Dispose()
        {
            m_Factory.Release(m_RacetracksCalculator);
        }

        #region IRacetracksSourceManager Members

        public IRacetracks Racetracks
        {
            get
            {
                return m_RacetracksCalculator;
            }
        }

        #endregion

        internal void RacetrackSettingsChangedHandler(RacetrackSettingsChangedMessage message)
        {
            Update();
        }

        internal void Update()
        {
            m_Factory.Release(m_RacetracksCalculator);

            IRacetrackSettingsSource source = m_RacetrackSettingsSourceManager.Source;

            LogRacetrackSettings(source);

            m_RacetracksCalculator = m_Factory.Create <IRacetracksCalculator>();
            m_RacetracksCalculator.Lines = m_LinesSourceManager.Lines;
            m_RacetracksCalculator.Radius = source.TurnRadius;
            m_RacetracksCalculator.IsPortTurnAllowed = source.IsPortTurnAllowed;
            m_RacetracksCalculator.IsStarboardTurnAllowed = source.IsStarboardTurnAllowed;
            m_RacetracksCalculator.Calculate();

            SendRacetracksChangedMessage(Racetracks);
        }

        private void LogRacetrackSettings([NotNull] IRacetrackSettingsSource source)
        {
            string text = "[RacetracksSourceManager] " +
                          "Racetrack Settings: TurnRadius = {0} " +
                          "IsPortTurnAllowed = {1} " +
                          "IsStarboardTurnAllowed = {2}";

            m_Logger.Info(text.Inject(source.TurnRadius,
                                      source.IsPortTurnAllowed,
                                      source.IsStarboardTurnAllowed));
        }

        internal void RacetracksGetHandler(RacetracksGetMessage message)
        {
            SendRacetracksChangedMessage(Racetracks);
        }

        internal void SendRacetracksChangedMessage([NotNull] IRacetracks racetracks)
        {
            RacetracksDto racetracksDto = m_Converter.ConvertPaths(racetracks);

            var response = new RacetracksChangedMessage
                           {
                               Racetracks = racetracksDto
                           };

            m_Bus.PublishAsync(response);
        }
    }
}