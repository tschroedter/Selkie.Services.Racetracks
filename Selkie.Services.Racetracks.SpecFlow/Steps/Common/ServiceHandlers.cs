using Castle.Core.Logging;
using EasyNetQ;
using JetBrains.Annotations;
using Selkie.EasyNetQ.Extensions;
using Selkie.Services.Common.Messages;
using TechTalk.SpecFlow;

namespace Selkie.Services.Racetracks.SpecFlow.Steps.Common
{
    public partial class ServiceHandlers
    {
        private readonly IBus m_Bus;
        private readonly ILogger m_Logger;

        public ServiceHandlers()
        {
            m_Logger = ( ILogger ) ScenarioContext.Current [ "ILogger" ];
            m_Bus = ( IBus ) ScenarioContext.Current [ "IBus" ];
        }

        public void Subscribe()
        {
            m_Bus.SubscribeHandlerAsync <PingResponseMessage>(m_Logger,
                                                              GetType().FullName,
                                                              PingResponseHandler);

            m_Bus.SubscribeHandlerAsync <ServiceStartedResponseMessage>(m_Logger,
                                                                        GetType().FullName,
                                                                        ServiceStartedResponseHandler);

            m_Bus.SubscribeHandlerAsync <ServiceStoppedResponseMessage>(m_Logger,
                                                                        GetType().FullName,
                                                                        ServiceStoppedResponseHandler);

            SubscribeOther();
        }

        private void ServiceStoppedResponseHandler([NotNull] ServiceStoppedResponseMessage message)
        {
            if ( message.ServiceName == Helper.ServiceName )
            {
                ScenarioContext.Current [ "IsReceivedServiceStoppedResponse" ] = true;
            }
        }

        private void ServiceStartedResponseHandler([NotNull] ServiceStartedResponseMessage message)
        {
            if ( message.ServiceName == Helper.ServiceName )
            {
                ScenarioContext.Current [ "IsReceivedServiceStartedResponse" ] = true;
            }
        }

        private void PingResponseHandler([NotNull] PingResponseMessage message)
        {
            ScenarioContext.Current [ "IsReceivedPingResponse" ] = true;
        }
    }
}