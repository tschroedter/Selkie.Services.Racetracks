using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Castle.Windsor;
using Castle.Windsor.Installer;
using JetBrains.Annotations;
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

        [AfterScenario]
        public void AfterScenario()
        {
            bool isExited = GetBoolValueForScenarioContext("IsExited");

            if ( !isExited )
            {
                m_SpecFlowService.KillAndWaitForExit();
            }
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

        [Given(@"Service is running")]
        public void Do()
        {
            ScenarioContext.Current [ "IsReceivedPingResponse" ] = false;

            Helper.SleepWaitAndDo(() => GetBoolValueForScenarioContext("IsReceivedPingResponse"),
                                  WhenISendAPingMessage);

            Assert.True(GetBoolValueForScenarioContext("IsReceivedPingResponse"),
                        "Didn't receive ping response!");
        }

        private static bool GetBoolValueForScenarioContext([NotNull] string key)
        {
            if ( !ScenarioContext.Current.Keys.Contains(key) )
            {
                return false;
            }

            var result = ( bool ) ScenarioContext.Current [ key ];

            return result;
        }

        private void WhenISendAPingMessage()
        {
            var bus = ( ISelkieBus ) ScenarioContext.Current [ "ISelkieBus" ];

            bus.PublishAsync(new PingRequestMessage());
        }
    }
}