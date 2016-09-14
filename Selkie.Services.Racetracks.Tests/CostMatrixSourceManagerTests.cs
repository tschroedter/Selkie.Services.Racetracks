using System;
using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;
using NSubstitute;
using NUnit.Framework;
using Ploeh.AutoFixture.NUnit3;
using Selkie.EasyNetQ;
using Selkie.NUnit.Extensions;
using Selkie.Services.Racetracks.Common.Messages;
using Selkie.Services.Racetracks.Interfaces;
using Selkie.Services.Racetracks.TypedFactories;
using Selkie.Windsor;

namespace Selkie.Services.Racetracks.Tests
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    internal sealed class CostMatrixSourceManagerTests
    {
        private const double Tolerance = 0.00001;

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
        public void CostMatrixRequestHandlerSendMessage(
            [NotNull] ISelkieBus bus,
            [NotNull] ISelkieLogger logger,
            [NotNull] IRacetracksToDtoConverter converter,
            [NotNull] ISurveyFeaturesSourceManager surveyFeaturesSourceManager,
            [NotNull] IRacetrackSettingsSourceManager racetrackSettingsSourceManager,
            [NotNull] IRacetracksSourceManager racetracksSourceManager,
            [NotNull] ICostMatrix costMatrix)
        {
            // assemble
            CostMatrixSourceManager sut = ConfigureManagerWithtMatrix(bus,
                                                                      logger,
                                                                      surveyFeaturesSourceManager,
                                                                      racetrackSettingsSourceManager,
                                                                      racetracksSourceManager,
                                                                      costMatrix);

            bus.ClearReceivedCalls();

            var message = new CostMatrixRequestMessage
                          {
                              ColonyId = costMatrix.ColonyId
                          };

            // act
            sut.CostMatrixRequestHandler(message);

            // assert
            bus.Received().PublishAsync(Arg.Is <CostMatrixResponseMessage>(x => x.ColonyId == costMatrix.ColonyId &&
                                                                                x.Matrix == costMatrix.Matrix));
        }

        [Theory]
        [AutoNSubstituteData]
        public void CostMatrixRequestHandlerThrowsExceptionForNotMatchingColonyIds(
            [NotNull] ISelkieBus bus,
            [NotNull] ISelkieLogger logger,
            [NotNull] IRacetracksToDtoConverter converter,
            [NotNull] ISurveyFeaturesSourceManager surveyFeaturesSourceManager,
            [NotNull] IRacetrackSettingsSourceManager racetrackSettingsSourceManager,
            [NotNull] IRacetracksSourceManager racetracksSourceManager,
            [NotNull] ICostMatrix costMatrix)
        {
            // assemble
            CostMatrixSourceManager sut = ConfigureManagerWithtMatrix(bus,
                                                                      logger,
                                                                      surveyFeaturesSourceManager,
                                                                      racetrackSettingsSourceManager,
                                                                      racetracksSourceManager,
                                                                      costMatrix);

            bus.ClearReceivedCalls();

            var message = new CostMatrixRequestMessage
                          {
                              ColonyId = Guid.Empty
                          };

            // act
            // assert
            Assert.Throws <ArgumentException>(() =>
                                              {
                                                  sut.CostMatrixRequestHandler(message);
                                              });
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

        [Theory]
        [AutoNSubstituteData]
        public void PreUpdateSource_SetsLines_WhenCalled(
            [NotNull] CostMatrixCalculateMessage message,
            [NotNull] [Frozen] ISurveyFeaturesSourceManager manager,
            [NotNull] CostMatrixSourceManager sut)
        {
            // assemble

            // act
            sut.PreUpdateSource(message);

            // assert
            manager.Received().SetSurveyFeaturesIfValid(message.SurveyFeatureDtos);
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
                   .SetSettings(Arg.Is <RacetrackSettings>(x => x.ColonyId == message.ColonyId &&
                                                                x.IsPortTurnAllowed == message.IsPortTurnAllowed &&
                                                                x.IsStarboardTurnAllowed ==
                                                                message.IsStarboardTurnAllowed &&
                                                                Math.Abs(x.TurnRadiusForPort - message.TurnRadiusForPort) <
                                                                Tolerance &&
                                                                Math.Abs(x.TurnRadiusForStarboard -
                                                                         message.TurnRadiusForStarboard) < Tolerance));
        }

        [Theory]
        [AutoNSubstituteData]
        public void SourceDefault([NotNull] [Frozen] ISelkieBus bus,
                                  [NotNull] CostMatrixSourceManager sut)
        {
            Assert.AreEqual(CostMatrix.Unkown,
                            sut.Source);
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
        public void UpdateSourceReleasesOldSource([NotNull] ISelkieBus bus,
                                                  [NotNull] ISelkieLogger logger,
                                                  [NotNull] IRacetracksToDtoConverter converter,
                                                  [NotNull] IRacetrackSettingsSourceManager
                                                      racetrackSettingsSourceManager,
                                                  [NotNull] ISurveyFeaturesSourceManager surveyFeaturesSourceManager,
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
                                                      surveyFeaturesSourceManager,
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
        public void UpdateSourceSendsCostMatrixResponseMessage(
            [NotNull] ISelkieBus bus,
            [NotNull] ISelkieLogger logger,
            [NotNull] ISurveyFeaturesSourceManager surveyFeaturesSourceManager,
            [NotNull] IRacetrackSettingsSourceManager racetrackSettingsSourceManager,
            [NotNull] IRacetracksSourceManager racetracksSourceManager,
            [NotNull] ICostMatrix costMatrix)
        {
            // assemble
            var factory = Substitute.For <ICostMatrixFactory>();
            factory.Create().Returns(costMatrix);

            CostMatrixSourceManager sut = ConfigureManagerWithtMatrix(bus,
                                                                      logger,
                                                                      surveyFeaturesSourceManager,
                                                                      racetrackSettingsSourceManager,
                                                                      racetracksSourceManager,
                                                                      costMatrix);


            // act
            sut.UpdateSource();

            // assert
            bus.Received().PublishAsync(Arg.Is <CostMatrixResponseMessage>(x => x.ColonyId == costMatrix.ColonyId &&
                                                                                x.Matrix == costMatrix.Matrix));
        }

        [Theory]
        [AutoNSubstituteData]
        public void UpdateSourceSetsSource([NotNull] ISelkieBus bus,
                                           [NotNull] ISelkieLogger logger,
                                           [NotNull] IRacetracksToDtoConverter converter,
                                           [NotNull] ISurveyFeaturesSourceManager surveyFeaturesSourceManager,
                                           [NotNull] IRacetrackSettingsSourceManager racetrackSettingsSourceManager,
                                           [NotNull] IRacetracksSourceManager racetracksSourceManager,
                                           [NotNull] ICostMatrix costMatrix)
        {
            // assemble
            var factory = Substitute.For <ICostMatrixFactory>();

            factory.Create().Returns(costMatrix);

            var manager = new CostMatrixSourceManager(bus,
                                                      logger,
                                                      surveyFeaturesSourceManager,
                                                      racetrackSettingsSourceManager,
                                                      racetracksSourceManager,
                                                      factory);

            // act
            manager.UpdateSource();

            // assert
            Assert.AreEqual(costMatrix,
                            manager.Source);
        }

        [NotNull]
        private static CostMatrixSourceManager ConfigureManagerWithtMatrix(
            [NotNull] ISelkieBus bus,
            [NotNull] ISelkieLogger logger,
            [NotNull] ISurveyFeaturesSourceManager surveyFeaturesSourceManager,
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
                                                      surveyFeaturesSourceManager,
                                                      racetrackSettingsSourceManager,
                                                      racetracksSourceManager,
                                                      factory);

            manager.UpdateSource();
            return manager;
        }
    }
}