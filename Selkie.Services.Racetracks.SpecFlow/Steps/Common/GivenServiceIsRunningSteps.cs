using System;
using System.Diagnostics.CodeAnalysis;
using Castle.Windsor;
using Castle.Windsor.Installer;
using NUnit.Framework;
using Selkie.EasyNetQ;
using Selkie.Services.Common.Messages;
using Selkie.Windsor;
using TechTalk.SpecFlow;

namespace Selkie.Services.Racetracks.SpecFlow.Steps.Common
{
    [Binding]
    [ExcludeFromCodeCoverage]
    public sealed class GivenServiceIsRunningStep : IDisposable
    {
        private IWindsorContainer m_Container;
        private ServiceHandlers m_ServiceHandlers;
        private SpecFlowService m_SpecFlowService;

        public void Dispose()
        {
            m_SpecFlowService.Dispose();
            m_Container.Dispose();
        }

        [BeforeScenario]
        public void BeforeScenario()
        {
            m_Container = new WindsorContainer();
            m_Container.Install(FromAssembly.This());

            ScenarioContext.Current [ "ISelkieLogger" ] = m_Container.Resolve <ISelkieLogger>();
            ScenarioContext.Current [ "ISelkieBus" ] = m_Container.Resolve <ISelkieBus>();

            m_SpecFlowService = new SpecFlowService();
            m_SpecFlowService.DeleteQueues();
            m_SpecFlowService.Run();

            m_ServiceHandlers = new ServiceHandlers();
            m_ServiceHandlers.Subscribe();
        }

        [AfterScenario]
        public void AfterScenario()
        {
            var isExited = ( bool ) ScenarioContext.Current [ "IsExited" ];

            if ( !isExited )
            {
                m_SpecFlowService.KillAndWaitForExit();
            }
        }

        [Given(@"Service is running")]
        public void Do()
        {
            ScenarioContext.Current [ "IsReceivedPingResponse" ] = false;

            Helper.SleepWaitAndDo(() => ( bool ) ScenarioContext.Current [ "IsReceivedPingResponse" ],
                                  WhenISendAPingMessage);

            Assert.True(( bool ) ScenarioContext.Current [ "IsReceivedPingResponse" ],
                        "Didn't receive ping response!");
        }

        private void WhenISendAPingMessage()
        {
            var bus = ( ISelkieBus ) ScenarioContext.Current [ "ISelkieBus" ];

            bus.PublishAsync(new PingRequestMessage());
        }
    }
}