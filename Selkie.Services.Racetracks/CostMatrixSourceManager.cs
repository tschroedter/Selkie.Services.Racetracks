using Castle.Core;
using Castle.Core.Logging;
using EasyNetQ;
using JetBrains.Annotations;
using Selkie.EasyNetQ.Extensions;
using Selkie.Services.Racetracks.Common.Messages;
using Selkie.Services.Racetracks.TypedFactories;
using Selkie.Windsor;
using Selkie.Windsor.Extensions;

namespace Selkie.Services.Racetracks
{
    [ProjectComponent(Lifestyle.Singleton)]
    public class CostMatrixSourceManager
        : ICostMatrixSourceManager,
          IStartable
    {
        private readonly IBus m_Bus;
        private readonly ICostMatrixFactory m_Factory;
        private readonly ILogger m_Logger;
        private readonly object m_Padlock = new object();
        private ICostMatrix m_Source = CostMatrix.Unkown;

        public CostMatrixSourceManager([NotNull] IBus bus,
                                       [NotNull] ILogger logger,
                                       [NotNull] ICostMatrixFactory factory)
        {
            m_Bus = bus;
            m_Logger = logger;
            m_Factory = factory;

            string subscriptionId = GetType().FullName;

            m_Bus.SubscribeHandlerAsync <CostMatrixCalculateMessage>(logger,
                                                                     subscriptionId,
                                                                     CostMatrixCalculateHandler);

            m_Bus.SubscribeHandlerAsync <CostMatrixGetMessage>(logger,
                                                               subscriptionId,
                                                               CostMatrixGetHandler);

            m_Bus.SubscribeHandlerAsync <RacetracksChangedMessage>(logger,
                                                                   subscriptionId,
                                                                   RacetracksChangedHandler);

            m_Bus.PublishAsync(new RacetrackSettingsGetMessage());
        }

        public ICostMatrix Source
        {
            get
            {
                ICostMatrix source;

                lock ( m_Padlock )
                {
                    source = m_Source;
                }

                return source;
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
            UpdateSource();
        }

        internal void RacetracksChangedHandler(RacetracksChangedMessage message)
        {
            UpdateSource();
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

                SendCostMatrixChangedMessage(costMatrix);
            }
        }

        private void SendCostMatrixChangedMessage(ICostMatrix costMatrix)
        {
            m_Bus.PublishAsync(new CostMatrixChangedMessage
                               {
                                   Matrix = costMatrix.Matrix
                               });
        }

        internal void CostMatrixGetHandler([NotNull] CostMatrixGetMessage message)
        {
            ICostMatrix costMatrix;

            lock ( m_Padlock )
            {
                costMatrix = m_Source;
            }

            var response = new CostMatrixChangedMessage
                           {
                               Matrix = costMatrix.Matrix
                           };

            m_Bus.PublishAsync(response);
        }
    }
}