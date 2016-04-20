using System;
using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;
using NSubstitute;
using Ploeh.AutoFixture.Xunit;
using Selkie.EasyNetQ;
using Selkie.Services.Racetracks.Common.Messages;
using Selkie.Services.Racetracks.Interfaces;
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
        private const double Tolerance = 0.00001;

        [Theory]
        [AutoNSubstituteData]
        public void SubscribesToCostMatrixRequestMessage([NotNull] [Frozen] ISelkieBus bus,
                                                         [NotNull] CostMatrixSourceManager sut)
        {
            // assemble
            // act
            string subscriptionId = sut.GetType().FullName;

            // assert
            bus.Received().SubscribeAsync(subscriptionId,
                                          Arg.Any <Action <CostMatrixRequestMessage>>());
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
                                           [NotNull] ILinesSourceManager linesSourceManager,
                                           [NotNull] IRacetrackSettingsSourceManager racetrackSettingsSourceManager,
                                           [NotNull] IRacetracksSourceManager racetracksSourceManager,
                                           [NotNull] ICostMatrix costMatrix)
        {
            // assemble
            var factory = Substitute.For <ICostMatrixFactory>();

            factory.Create().Returns(costMatrix);

            var manager = new CostMatrixSourceManager(bus,
                                                      logger,
                                                      linesSourceManager,
                                                      racetrackSettingsSourceManager,
                                                      racetracksSourceManager,
                                                      factory);

            // act
            manager.UpdateSource();

            // assert
            Assert.Equal(costMatrix,
                         manager.Source);
        }

        [Theory]
        [AutoNSubstituteData]
        public void UpdateSourceSendsCostMatrixResponseMessage(
            [NotNull] ISelkieBus bus,
            [NotNull] ISelkieLogger logger,
            [NotNull] ILinesSourceManager linesSourceManager,
            [NotNull] IRacetrackSettingsSourceManager racetrackSettingsSourceManager,
            [NotNull] IRacetracksSourceManager racetracksSourceManager,
            [NotNull] ICostMatrix costMatrix)
        {
            // assemble
            var factory = Substitute.For <ICostMatrixFactory>();
            factory.Create().Returns(costMatrix);

            CostMatrixSourceManager sut = ConfigureManagerWithtMatrix(bus,
                                                                      logger,
                                                                      linesSourceManager,
                                                                      racetrackSettingsSourceManager,
                                                                      racetracksSourceManager,
                                                                      costMatrix);


            // act
            sut.UpdateSource();

            // assert
            bus.Received().PublishAsync(Arg.Is <CostMatrixResponseMessage>(x => x.Matrix == costMatrix.Matrix));
        }

        [Theory]
        [AutoNSubstituteData]
        public void UpdateSourceReleasesOldSource([NotNull] ISelkieBus bus,
                                                  [NotNull] ISelkieLogger logger,
                                                  [NotNull] IRacetracksToDtoConverter converter,
                                                  [NotNull] IRacetrackSettingsSourceManager
                                                      racetrackSettingsSourceManager,
                                                  [NotNull] ILinesSourceManager linesSourceManager,
                                                  [NotNull] IRacetracksSourceManager racetracksSourceManager)
        {
            // assemble
            var factory = Substitute.For <ICostMatrixFactory>();

            var costMatrixOne = Substitute.For <ICostMatrix>();
            var costMatrixTwo = Substitute.For <ICostMatrix>();

            factory.Create().Returns(costMatrixOne,
                                     costMatrixTwo);

            var manager = new CostMatrixSourceManager(bus,
                                                      logger,
                                                      linesSourceManager,
                                                      racetrackSettingsSourceManager,
                                                      racetracksSourceManager,
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
            bus.Received().PublishAsync(Arg.Any <CostMatrixResponseMessage>());
        }

        [Theory]
        [AutoNSubstituteData]
        public void CostMatrixGetHandlerSendMessage(
            [NotNull] ISelkieBus bus,
            [NotNull] ISelkieLogger logger,
            [NotNull] IRacetracksToDtoConverter converter,
            [NotNull] ILinesSourceManager linesSourceManager,
            [NotNull] IRacetrackSettingsSourceManager racetrackSettingsSourceManager,
            [NotNull] IRacetracksSourceManager racetracksSourceManager,
            [NotNull] ICostMatrix costMatrix)
        {
            // assemble
            CostMatrixSourceManager sut = ConfigureManagerWithtMatrix(bus,
                                                                      logger,
                                                                      linesSourceManager,
                                                                      racetrackSettingsSourceManager,
                                                                      racetracksSourceManager,
                                                                      costMatrix);

            bus.ClearReceivedCalls();

            // act
            sut.CostMatrixGetHandler(new CostMatrixRequestMessage());

            // assert
            bus.Received().PublishAsync(Arg.Is <CostMatrixResponseMessage>(x => x.Matrix == costMatrix.Matrix));
        }

        [NotNull]
        private static CostMatrixSourceManager ConfigureManagerWithtMatrix(
            [NotNull] ISelkieBus bus,
            [NotNull] ISelkieLogger logger,
            [NotNull] ILinesSourceManager linesSourceManager,
            [NotNull] IRacetrackSettingsSourceManager racetrackSettingsSourceManager,
            [NotNull] IRacetracksSourceManager racetracksSourceManager,
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
                                                      linesSourceManager,
                                                      racetrackSettingsSourceManager,
                                                      racetracksSourceManager,
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

        [Theory]
        [AutoNSubstituteData]
        public void PreUpdateSource_SetsLines_WhenCalled(
            [NotNull] CostMatrixCalculateMessage message,
            [NotNull] [Frozen] ILinesSourceManager manager,
            [NotNull] CostMatrixSourceManager sut)
        {
            // assemble

            // act
            sut.PreUpdateSource(message);

            // assert
            manager.Received().SetLinesIfValid(message.LineDtos);
        }

        [Theory]
        [AutoNSubstituteData]
        public void PreUpdateSource_SetsRacetrackSettings_WhenCalled(
            [NotNull] CostMatrixCalculateMessage message,
            [NotNull] [Frozen] IRacetrackSettingsSourceManager manager,
            [NotNull] CostMatrixSourceManager sut)
        {
            // assemble

            // act
            sut.PreUpdateSource(message);

            // assert
            manager.Received()
                   .SetSettings(Arg.Is <RacetrackSettings>(x => x.IsPortTurnAllowed == message.IsPortTurnAllowed &&
                                                                x.IsStarboardTurnAllowed ==
                                                                message.IsStarboardTurnAllowed &&
                                                                Math.Abs(x.TurnRadiusForPort - message.TurnRadiusForPort) <
                                                                Tolerance &&
                                                                Math.Abs(x.TurnRadiusForStarboard -
                                                                         message.TurnRadiusForStarboard) < Tolerance));
        }

        [Theory]
        [AutoNSubstituteData]
        public void PreUpdateSource_CalculateRacetracks_WhenCalled(
            [NotNull] CostMatrixCalculateMessage message,
            [NotNull] [Frozen] IRacetracksSourceManager manager,
            [NotNull] CostMatrixSourceManager sut)
        {
            // assemble

            // act
            sut.PreUpdateSource(message);

            // assert
            manager.Received().CalculateRacetracks();
        }
    }
}