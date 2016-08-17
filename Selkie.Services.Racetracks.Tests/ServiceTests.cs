using System;
using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;
using NSubstitute;
using NUnit.Framework;
using Ploeh.AutoFixture.NUnit3;
using Selkie.EasyNetQ;
using Selkie.NUnit.Extensions;
using Selkie.Services.Common.Messages;
using Selkie.Windsor;

namespace Selkie.Services.Racetracks.Tests
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    internal sealed class ServiceTests
    {
        [Theory]
        [AutoNSubstituteData]
        public void Constructor_ReturnsInstance_WhenCalled([NotNull] [Frozen] ISelkieBus bus,
                                                           [NotNull] Service service)
        {
            // assemble
            // act
            var sut = new Service(Substitute.For <ISelkieBus>(),
                                  Substitute.For <ISelkieLogger>(),
                                  Substitute.For <ISelkieManagementClient>());

            // assert
            Assert.NotNull(sut);
        }

        [Theory]
        [AutoNSubstituteData]
        public void ServiceInitializeSubscribesToPingRequestMessageTest([NotNull] [Frozen] ISelkieBus bus,
                                                                        [NotNull] Service service)
        {
            // assemble
            // act
            service.Initialize();

            string subscriptionId = service.GetType().ToString();

            // assert
            bus.Received().SubscribeAsync(subscriptionId,
                                          Arg.Any <Action <PingRequestMessage>>());
        }

        [Theory]
        [AutoNSubstituteData]
        public void ServiceStartSendsMessageTest([NotNull] [Frozen] ISelkieBus bus,
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
        public void ServiceStopSendsMessageTest([NotNull] [Frozen] ISelkieBus bus,
                                                [NotNull] Service service)
        {
            // assemble
            // act
            service.Stop();

            // assert
            bus.Received().Publish(Arg.Is <ServiceStoppedResponseMessage>(x => x.ServiceName == Service.ServiceName));
        }
    }
}