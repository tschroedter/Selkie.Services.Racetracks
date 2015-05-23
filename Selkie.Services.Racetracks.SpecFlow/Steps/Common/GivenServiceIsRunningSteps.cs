using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using Castle.Core.Logging;
using Castle.Windsor;
using Castle.Windsor.Installer;
using EasyNetQ;
using JetBrains.Annotations;
using NUnit.Framework;
using Selkie.Services.Common.Messages;
using TechTalk.SpecFlow;

namespace Selkie.Services.Racetracks.SpecFlow.Steps.Common
{
    [Binding]
    [ExcludeFromCodeCoverage]
    public sealed class GivenServiceIsRunningStep : IDisposable
    {
        private static readonly TimeSpan SleepTime = TimeSpan.FromSeconds(1.0);
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

            ScenarioContext.Current [ "ILogger" ] = m_Container.Resolve <ILogger>();
            ScenarioContext.Current [ "IBus" ] = m_Container.Resolve <IBus>(); // todo create ISelkieBus

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

            SleepWaitAndDo(() => ( bool ) ScenarioContext.Current [ "IsReceivedPingResponse" ],
                           WhenISendAPingMessage);

            Assert.True(( bool ) ScenarioContext.Current [ "IsReceivedPingResponse" ],
                        "Didn't receive ping response!");
        }

        private void WhenISendAPingMessage()
        {
            var bus = ( IBus ) ScenarioContext.Current [ "IBus" ];

            bus.PublishAsync(new PingRequestMessage());
        }

        // todo duplicated code in BaseStep
        public void SleepWaitAndDo([NotNull] Func <bool> breakIfTrue,
                                   [NotNull] Action doSomething)
        {
            for ( var i = 0 ; i < 10 ; i++ )
            {
                Thread.Sleep(SleepTime);

                if ( breakIfTrue() )
                {
                    break;
                }

                doSomething();
            }
        }
    }
}