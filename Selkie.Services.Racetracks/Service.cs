using Castle.Core;
using JetBrains.Annotations;
using Selkie.Aop.Aspects;
using Selkie.EasyNetQ;
using Selkie.Services.Common;
using Selkie.Services.Common.Messages;
using Selkie.Windsor;
using IService = Selkie.Services.Racetracks.Interfaces.IService;

namespace Selkie.Services.Racetracks
{
    [Interceptor(typeof( MessageHandlerAspect ))]
    [ProjectComponent(Lifestyle.Singleton)]
    public class Service
        : BaseService,
          IService
    {
        public Service([NotNull] ISelkieBus bus,
                       [NotNull] ISelkieLogger logger,
                       [NotNull] ISelkieManagementClient client)
            : base(bus,
                   logger,
                   client,
                   ServiceName)
        {
        }

        public const string ServiceName = "Racetracks Service";

        protected override void ServiceInitialize()
        {
        }

        protected override void ServiceStart()
        {
            var message = new ServiceStartedResponseMessage
                          {
                              ServiceName = ServiceName
                          };

            Bus.Publish(message);
        }

        protected override void ServiceStop()
        {
            var message = new ServiceStoppedResponseMessage
                          {
                              ServiceName = ServiceName
                          };

            Bus.Publish(message);
        }
    }
}