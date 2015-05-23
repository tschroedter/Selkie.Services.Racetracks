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
        private readonly object m_PadlockSource = new object();
        private ICostMatrix m_Source = CostMatrix.Unkown;

        public CostMatrixSourceManager([NotNull] IBus bus,
                                       [NotNull] ILogger logger,
                                       [NotNull] ICostMatrixFactory factory)
        {
            m_Bus = bus;
            m_Logger = logger;
            m_Factory = factory;

            m_Bus.SubscribeHandlerAsync <CostMatrixCalculateMessage>(logger,
                                                                     GetType().FullName,
                                                                     CostMatrixCalculateHandler);

            m_Bus.SubscribeHandlerAsync <CostMatrixGetMessage>(logger,
                                                               GetType().FullName,
                                                               CostMatrixGetHandler);
        }

        public ICostMatrix Source
        {
            get
            {
                ICostMatrix source;

                lock ( m_PadlockSource )
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

        internal void UpdateSource()
        {
            lock ( this ) // todo warning nested locks
            {
                ICostMatrix costMatrix = m_Factory.Create();

                lock ( m_PadlockSource )
                {
                    ICostMatrix oldSource = m_Source;

                    m_Source = costMatrix;

                    if ( oldSource != null )
                    {
                        m_Factory.Release(oldSource);
                    }
                }

                m_Bus.PublishAsync(new CostMatrixChangedMessage
                                   {
                                       Matrix = costMatrix.Matrix
                                   });
            }
        }

        internal void CostMatrixGetHandler([NotNull] CostMatrixGetMessage message)
        {
            ICostMatrix costMatrix;

            lock ( m_PadlockSource )
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