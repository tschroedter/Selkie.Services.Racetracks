using Castle.Core;
using JetBrains.Annotations;
using Selkie.Aop.Aspects;
using Selkie.EasyNetQ;
using Selkie.Services.Racetracks.Common.Messages;
using Selkie.Services.Racetracks.Interfaces;
using Selkie.Services.Racetracks.TypedFactories;
using Selkie.Windsor;
using Selkie.Windsor.Extensions;

namespace Selkie.Services.Racetracks
{
    [Interceptor(typeof( MessageHandlerAspect ))]
    [ProjectComponent(Lifestyle.Singleton)]
    public class CostMatrixSourceManager
        : ICostMatrixSourceManager,
          IStartable
    {
        public CostMatrixSourceManager([NotNull] ISelkieBus bus,
                                       [NotNull] ISelkieLogger logger,
                                       [NotNull] ISurveyFeaturesSourceManager surveyFeaturesSourceManager,
                                       [NotNull] IRacetrackSettingsSourceManager racetrackSettingsSourceManager,
                                       [NotNull] IRacetracksSourceManager racetracksSourceManager,
                                       [NotNull] ICostMatrixFactory factory)
        {
            m_Bus = bus;
            m_Logger = logger;
            m_SurveyFeaturesSourceManager = surveyFeaturesSourceManager;
            m_RacetrackSettingsSourceManager = racetrackSettingsSourceManager;
            m_RacetracksSourceManager = racetracksSourceManager;
            m_Factory = factory;

            string subscriptionId = GetType().FullName;

            m_Bus.SubscribeAsync <CostMatrixCalculateMessage>(subscriptionId,
                                                              CostMatrixCalculateHandler);

            m_Bus.SubscribeAsync <CostMatrixRequestMessage>(subscriptionId,
                                                            CostMatrixGetHandler);
        }

        private readonly ISelkieBus m_Bus;
        private readonly ICostMatrixFactory m_Factory;
        private readonly ISelkieLogger m_Logger;
        private readonly object m_Padlock = new object();
        private readonly IRacetrackSettingsSourceManager m_RacetrackSettingsSourceManager;
        private readonly IRacetracksSourceManager m_RacetracksSourceManager;
        private readonly ISurveyFeaturesSourceManager m_SurveyFeaturesSourceManager;
        private ICostMatrix m_Source = CostMatrix.Unkown;

        public ICostMatrix Source
        {
            get
            {
                lock ( m_Padlock )
                {
                    return m_Source;
                }
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

        internal void CostMatrixCalculateHandler([NotNull] CostMatrixCalculateMessage message)
        {
            PreUpdateSource(message);

            UpdateSource();
        }

        internal void CostMatrixGetHandler([NotNull] CostMatrixRequestMessage message)
        {
            ICostMatrix costMatrix;

            lock ( m_Padlock )
            {
                costMatrix = m_Source;
            }

            var response = new CostMatrixResponseMessage
                           {
                               Matrix = costMatrix.Matrix
                           };

            m_Bus.PublishAsync(response);
        }

        internal void PreUpdateSource(CostMatrixCalculateMessage message)
        {
            m_SurveyFeaturesSourceManager.SetSurveyFeaturesIfValid(message.SurveyFeatureDtos);

            var settings = new RacetrackSettings
                           {
                               IsPortTurnAllowed = message.IsPortTurnAllowed,
                               IsStarboardTurnAllowed = message.IsStarboardTurnAllowed,
                               TurnRadiusForPort = message.TurnRadiusForPort,
                               TurnRadiusForStarboard = message.TurnRadiusForStarboard
                           };

            m_RacetrackSettingsSourceManager.SetSettings(settings);

            m_RacetracksSourceManager.CalculateRacetracks();
        }

        internal void UpdateSource()
        {
            lock ( m_Padlock )
            {
                ICostMatrix costMatrix = m_Factory.Create();
                ICostMatrix oldSource = m_Source;

                m_Source = costMatrix;

                if ( oldSource != null )
                {
                    m_Factory.Release(oldSource);
                }

                SendCostMatrixResponseMessage(costMatrix);
            }
        }

        private void SendCostMatrixResponseMessage(ICostMatrix costMatrix)
        {
            m_Bus.PublishAsync(new CostMatrixResponseMessage
                               {
                                   Matrix = costMatrix.Matrix
                               });
        }
    }
}