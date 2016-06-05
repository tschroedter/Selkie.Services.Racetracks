using System.Diagnostics.CodeAnalysis;
using System.ServiceProcess;
using System.Threading;
using Castle.Windsor;
using JetBrains.Annotations;
using Selkie.EasyNetQ;
using Selkie.Services.Common.Messages;
using Selkie.Windsor;

namespace Selkie.Services.Racetracks.Windows.Service
{
    [ExcludeFromCodeCoverage]
    public partial class Service : ServiceBase
    {
        public Service()
        {
            InitializeComponent();

            m_Thread = CreateWorkerThread();
        }

        private readonly RacetracksServiceMain m_Main = new RacetracksServiceMain();
        private readonly ManualResetEvent m_ShutDownEvent = new ManualResetEvent(false);
        private readonly Thread m_Thread;

        private ISelkieBus m_Bus;
        private IWindsorContainer m_Container;
        private ISelkieLogger m_Logger;

        public void Start()
        {
            OnStart(new string[0]);
        }

        protected override void OnStart(string[] args)
        {
            m_Thread.Start();
        }

        protected override void OnStop()
        {
            var message = new StopServiceRequestMessage
                          {
                              ServiceName = Racetracks.Service.ServiceName
                          };

            m_Bus.PublishAsync(message);

            m_Thread.Abort();
        }

        private static IWindsorContainer CreateWindsorContainer()
        {
            var container = new WindsorContainer();
            var installer = new Installer();

            container.Install(installer);
            return container;
        }

        private Thread CreateWorkerThread()
        {
            var thread = new Thread(() => WorkerThreadFunc(m_Main))
                         {
                             Name = "Selkie Service Aco Worker Thread",
                             IsBackground = true
                         };

            return thread;
        }

        private void InitializeAcoServiceRelatedFields()
        {
            m_Container = CreateWindsorContainer();
            m_Logger = m_Container.Resolve <ISelkieLogger>();
            m_Bus = m_Container.Resolve <ISelkieBus>();
        }

        private void WaitForShutDownEvent()
        {
            while ( !m_ShutDownEvent.WaitOne() )
            {
                m_Logger.Info("Service received shut down event...");
            }
        }

        private void WorkerThreadFunc([NotNull] RacetracksServiceMain serviceMain)
        {
            InitializeAcoServiceRelatedFields();

            serviceMain.Run();

            WaitForShutDownEvent();
        }
    }
}