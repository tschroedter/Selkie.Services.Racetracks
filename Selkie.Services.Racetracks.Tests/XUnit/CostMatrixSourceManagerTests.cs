using System;
using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;
using NSubstitute;
using Ploeh.AutoFixture.Xunit;
using Selkie.EasyNetQ;
using Selkie.Services.Racetracks.Common.Messages;
using Selkie.Services.Racetracks.TypedFactories;
using Selkie.Windsor;
using Selkie.XUnit.Extensions;
using Xunit;
using Xunit.Extensions;

namespace Selkie.Services.Racetracks.Tests.XUnit
{
    //ncrunch: no coverRacetracksGetMessageage start
    [ExcludeFromCodeCoverage]
    public sealed class CostMatrixSourceManagerTests
    {
        [Theory]
        [AutoNSubstituteData]
        public void SubscribesToCostMatrixGetMessage([NotNull] [Frozen] ISelkieBus bus,
                                                     [NotNull] CostMatrixSourceManager sut)
        {
            // assemble
            // act
            string subscriptionId = sut.GetType().FullName;

            // assert
            bus.Received().SubscribeAsync(subscriptionId,
                                          Arg.Any <Action <CostMatrixGetMessage>>());
        }

        [Theory]
        [AutoNSubstituteData]
        public void SubscribesToRacetracksChangedMessage([NotNull] [Frozen] ISelkieBus bus,
                                                         [NotNull] CostMatrixSourceManager sut)
        {
            // assemble
            // act
            string subscriptionId = sut.GetType().FullName;

            // assert
            bus.Received().SubscribeAsync(subscriptionId,
                                          Arg.Any <Action <RacetracksChangedMessage>>());
        }

        [Theory]
        [AutoNSubstituteData]
        public void ConstructorSendsRacetrackSettingsGetMessage([NotNull] [Frozen] ISelkieBus bus,
                                                                [NotNull] CostMatrixSourceManager sut)
        {
            // assemble
            // act
            sut.UpdateSource();

            // assert
            bus.Received().PublishAsync(Arg.Any <RacetrackSettingsGetMessage>());
        }

        [Theory]
        [AutoNSubstituteData]
        public void SourceDefault([NotNull] [Frozen] ISelkieBus bus,
                                  [NotNull] CostMatrixSourceManager sut)
        {
            Assert.Equal(CostMatrix.Unkown,
                         sut.Source);
        }

        [Theory]
        [AutoNSubstituteData]
        public void UpdateSourceSetsSource([NotNull] ISelkieBus bus,
                                           [NotNull] ISelkieLogger logger,
                                           [NotNull] IRacetracksToDtoConverter converter,
                                           [NotNull] ICostMatrix costMatrix)
        {
            // assemble
            var factory = Substitute.For <ICostMatrixFactory>();

            factory.Create().Returns(costMatrix);

            var manager = new CostMatrixSourceManager(bus,
                                                      logger,
                                                      factory);

            // act
            manager.UpdateSource();

            // assert
            Assert.Equal(costMatrix,
                         manager.Source);
        }

        [Theory]
        [AutoNSubstituteData]
        public void UpdateSourceSendsCostMatrixChangedMessage([NotNull] ISelkieBus bus,
                                                              [NotNull] ISelkieLogger logger,
                                                              [NotNull] ICostMatrix costMatrix)
        {
            // assemble
            var factory = Substitute.For <ICostMatrixFactory>();
            factory.Create().Returns(costMatrix);

            CostMatrixSourceManager sut = ConfigureManagerWithtMatrix(bus,
                                                                      logger,
                                                                      costMatrix);


            // act
            sut.UpdateSource();

            // assert
            bus.Received().PublishAsync(Arg.Is <CostMatrixChangedMessage>(x => x.Matrix == costMatrix.Matrix));
        }

        [Theory]
        [AutoNSubstituteData]
        public void UpdateSourceReleasesOldSource([NotNull] ISelkieBus bus,
                                                  [NotNull] ISelkieLogger logger,
                                                  [NotNull] IRacetracksToDtoConverter converter)
        {
            // assemble
            var factory = Substitute.For <ICostMatrixFactory>();

            var costMatrixOne = Substitute.For <ICostMatrix>();
            var costMatrixTwo = Substitute.For <ICostMatrix>();

            factory.Create().Returns(costMatrixOne,
                                     costMatrixTwo);

            var manager = new CostMatrixSourceManager(bus,
                                                      logger,
                                                      factory);

            // act
            manager.UpdateSource();
            manager.UpdateSource();

            // assert
            factory.Received().Release(costMatrixOne);
        }

        [Theory]
        [AutoNSubstituteData]
        public void CostMatrixCalculateHandlerCallsUpdateSource([NotNull] [Frozen] ISelkieBus bus,
                                                                [NotNull] CostMatrixSourceManager sut)
        {
            // assemble
            var message = new CostMatrixCalculateMessage();

            // act
            sut.CostMatrixCalculateHandler(message);

            // assert
            bus.Received().PublishAsync(Arg.Any <CostMatrixChangedMessage>());
        }

        [Theory]
        [AutoNSubstituteData]
        public void RacetracksChangedHandlerCallsUpdateSource([NotNull] [Frozen] ISelkieBus bus,
                                                              [NotNull] CostMatrixSourceManager sut)
        {
            // assemble
            var message = new RacetracksChangedMessage();

            // act
            sut.RacetracksChangedHandler(message);

            // assert
            bus.Received().PublishAsync(Arg.Any <CostMatrixChangedMessage>());
        }

        [Theory]
        [AutoNSubstituteData]
        public void CostMatrixGetHandlerSendMessage([NotNull] ISelkieBus bus,
                                                    [NotNull] ISelkieLogger logger,
                                                    [NotNull] IRacetracksToDtoConverter converter,
                                                    [NotNull] ICostMatrix costMatrix)
        {
            // assemble
            CostMatrixSourceManager sut = ConfigureManagerWithtMatrix(bus,
                                                                      logger,
                                                                      costMatrix);

            bus.ClearReceivedCalls();

            // act
            sut.CostMatrixGetHandler(new CostMatrixGetMessage());

            // assert
            bus.Received().PublishAsync(Arg.Is <CostMatrixChangedMessage>(x => x.Matrix == costMatrix.Matrix));
        }

        [NotNull]
        private static CostMatrixSourceManager ConfigureManagerWithtMatrix([NotNull] ISelkieBus bus,
                                                                           [NotNull] ISelkieLogger logger,
                                                                           [NotNull] ICostMatrix costMatrix)
        {
            costMatrix.Matrix.Returns(new[]
                                      {
                                          new[]
                                          {
                                              1.0
                                          }
                                      });

            var factory = Substitute.For <ICostMatrixFactory>();
            factory.Create().Returns(costMatrix);

            var manager = new CostMatrixSourceManager(bus,
                                                      logger,
                                                      factory);

            manager.UpdateSource();
            return manager;
        }

        [Theory]
        [AutoNSubstituteData]
        public void StartCallsLoggerTest([NotNull] [Frozen] ISelkieLogger logger,
                                         [NotNull] CostMatrixSourceManager sut)
        {
            // assemble
            // act
            sut.Start();

            // assert
            logger.Received().Info(Arg.Is <string>(x => x.StartsWith("Started")));
        }

        [Theory]
        [AutoNSubstituteData]
        public void StopCallsLoggerTest([NotNull] [Frozen] ISelkieLogger logger,
                                        [NotNull] CostMatrixSourceManager sut)
        {
            // assemble
            // act
            sut.Stop();

            // assert
            logger.Received().Info(Arg.Is <string>(x => x.StartsWith("Stopped")));
        }
    }
}