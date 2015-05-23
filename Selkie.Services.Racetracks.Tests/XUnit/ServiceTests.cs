﻿using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using EasyNetQ;
using JetBrains.Annotations;
using NSubstitute;
using Ploeh.AutoFixture.Xunit;
using Selkie.Services.Common.Messages;
using Selkie.XUnit.Extensions;
using Xunit.Extensions;

namespace Selkie.Services.Racetracks.Tests.XUnit
{
    //ncrunch: no coverage start
    [ExcludeFromCodeCoverage]
    public sealed class ServiceTests
    {
        [Theory]
        [AutoNSubstituteData]
        public void ServiceStartSendsMessageTest([NotNull] [Frozen] IBus bus,
                                                 [NotNull] Service service)
        {
            // assemble
            // act
            service.Start();

            // assert
            bus.Received().Publish(Arg.Is <ServiceStartedResponseMessage>(x => x.ServiceName == Service.ServiceName));
        }

        [Theory]
        [AutoNSubstituteData]
        public void ServiceStopSendsMessageTest([NotNull] [Frozen] IBus bus,
                                                [NotNull] Service service)
        {
            // assemble
            // act
            service.Stop();

            // assert
            bus.Received().Publish(Arg.Is <ServiceStoppedResponseMessage>(x => x.ServiceName == Service.ServiceName));
        }

        [Theory]
        [AutoNSubstituteData]
        public void ServiceInitializeSubscribesToPingRequestMessageTest([NotNull] [Frozen] IBus bus,
                                                                        [NotNull] Service service)
        {
            // assemble
            // act
            service.Initialize();

            string subscriptionId = service.GetType().ToString();

            // assert
            bus.Received().SubscribeAsync(subscriptionId,
                                          Arg.Any <Func <PingRequestMessage, Task>>());
        }
    }
}