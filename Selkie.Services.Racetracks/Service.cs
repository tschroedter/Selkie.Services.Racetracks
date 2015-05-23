using Castle.Core.Logging;
using EasyNetQ;
using JetBrains.Annotations;
using Selkie.Services.Common;
using Selkie.Services.Common.Messages;
using Selkie.Windsor;

namespace Selkie.Services.Racetracks
{
    [ProjectComponent(Lifestyle.Singleton)]
    public class Service
        : BaseService,
          IService
    {
        public const string ServiceName = "Racetracks Service";

        public Service([NotNull] IBus bus,
                       [NotNull] ILogger logger,
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