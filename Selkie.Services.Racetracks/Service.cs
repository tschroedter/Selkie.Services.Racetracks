using Castle.Core;
using JetBrains.Annotations;
using Selkie.Aop.Aspects;
using Selkie.EasyNetQ;
using Selkie.Services.Common;
using Selkie.Services.Common.Messages;
using Selkie.Windsor;

namespace Selkie.Services.Racetracks
{
    [Interceptor(typeof(MessageHandlerAspect))]
    [ProjectComponent(Lifestyle.Singleton)]
    public class Service
        : BaseService,
          IService
    {
        public const string ServiceName = "Racetracks Service";

        public Service([NotNull] ISelkieBus bus,
                       [NotNull] ISelkieLogger logger,
                       [NotNull] ISelkieManagementClient client)
            : base(bus,
                   logger,
                   client,
                   ServiceName)
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

        protected override void ServiceInitialize()
        {
        }
    }
}