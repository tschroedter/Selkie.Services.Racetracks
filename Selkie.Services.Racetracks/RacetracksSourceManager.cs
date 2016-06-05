using System;
using Castle.Core;
using JetBrains.Annotations;
using Selkie.Aop.Aspects;
using Selkie.EasyNetQ;
using Selkie.Geometry.Primitives;
using Selkie.Racetrack.Interfaces;
using Selkie.Racetrack.Interfaces.Calculators;
using Selkie.Services.Common.Dto;
using Selkie.Services.Racetracks.Common.Messages;
using Selkie.Services.Racetracks.Interfaces;
using Selkie.Windsor;
using Selkie.Windsor.Extensions;

namespace Selkie.Services.Racetracks
{
    // todo discovered problem when turn circle is 40 and distance between lines is 30 will not find a path
    //      -=> at the moment the algorithm can only handle circles, circle-line-circle, but not circle-line-circle ->line-> circle-line-circle
    [Interceptor(typeof( MessageHandlerAspect ))]
    [ProjectComponent(Lifestyle.Singleton)]
    public sealed class RacetracksSourceManager
        : IRacetracksSourceManager,
          IDisposable
    {
        public RacetracksSourceManager([NotNull] ISelkieLogger logger,
                                       [NotNull] ISelkieBus bus,
                                       [NotNull] ISurveyFeaturesSourceManager surveyFeaturesSourceManager,
                                       [NotNull] IRacetrackSettingsSourceManager racetrackSettingsSourceManager,
                                       [NotNull] ICalculatorFactory factory,
                                       [NotNull] IRacetracksToDtoConverter converter)
        {
            m_Logger = logger;
            m_Bus = bus;
            m_SurveyFeaturesSourceManager = surveyFeaturesSourceManager;
            m_RacetrackSettingsSourceManager = racetrackSettingsSourceManager;
            m_Factory = factory;
            m_Converter = converter;
            m_RacetracksCalculator = m_Factory.Create<IRacetracksCalculator>();

            string subscriptionId = GetType().FullName;

            m_Bus.SubscribeAsync <RacetracksGetMessage>(subscriptionId,
                                                        RacetracksGetHandler);
        }

        private readonly ISelkieBus m_Bus;
        private readonly IRacetracksToDtoConverter m_Converter;
        private readonly ICalculatorFactory m_Factory;
        private readonly ISelkieLogger m_Logger;
        private readonly IRacetrackSettingsSourceManager m_RacetrackSettingsSourceManager;
        private readonly ISurveyFeaturesSourceManager m_SurveyFeaturesSourceManager;
        private IRacetracksCalculator m_RacetracksCalculator;

        public void Dispose()
        {
            m_Factory.Release(m_RacetracksCalculator);
        }

        public IRacetracks Racetracks
        {
            get
            {
                return m_RacetracksCalculator;
            }
        }

        public void CalculateRacetracks()
        {
            Calculate();

            SendRacetracksResponseMessage(Racetracks);
        }

        internal void Calculate()
        {
            m_Factory.Release(m_RacetracksCalculator);

            IRacetrackSettingsSource source = m_RacetrackSettingsSourceManager.Source;

            LogRacetrackSettings(source);

            m_RacetracksCalculator = m_Factory.Create <IRacetracksCalculator>();
            m_RacetracksCalculator.Features = m_SurveyFeaturesSourceManager.Features; // todo test
            m_RacetracksCalculator.TurnRadiusForPort = new Distance(source.TurnRadiusForPort);
            m_RacetracksCalculator.TurnRadiusForStarboard = new Distance(source.TurnRadiusForStarboard);
            m_RacetracksCalculator.IsPortTurnAllowed = source.IsPortTurnAllowed;
            m_RacetracksCalculator.IsStarboardTurnAllowed = source.IsStarboardTurnAllowed;
            m_RacetracksCalculator.Calculate();
        }

        internal void RacetracksGetHandler(RacetracksGetMessage message)
        {
            SendRacetracksResponseMessage(Racetracks);
        }

        internal void SendRacetracksResponseMessage([NotNull] IRacetracks racetracks)
        {
            RacetracksDto racetracksDto = m_Converter.ConvertPaths(racetracks);

            var response = new RacetracksResponseMessage
                           {
                               Racetracks = racetracksDto
                           };

            m_Bus.PublishAsync(response);
        }

        private void LogRacetrackSettings([NotNull] IRacetrackSettingsSource source)
        {
            const string text = "[RacetracksSourceManager] " +
                                "Racetrack Settings: TurnRadius = {0} " +
                                "IsPortTurnAllowed = {1} " +
                                "IsStarboardTurnAllowed = {2}";

            m_Logger.Info(text.Inject(source.TurnRadiusForPort,
                                      source.TurnRadiusForStarboard,
                                      source.IsPortTurnAllowed,
                                      source.IsStarboardTurnAllowed));
        }
    }
}