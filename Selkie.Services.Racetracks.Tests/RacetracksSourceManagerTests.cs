using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using NSubstitute;
using Selkie.EasyNetQ;
using Selkie.Geometry.Surveying;
using Selkie.Racetrack.Interfaces;
using Selkie.Racetrack.Interfaces.Calculators;
using Selkie.Services.Common.Dto;
using Selkie.Services.Racetracks.Common.Messages;
using Selkie.Services.Racetracks.Interfaces;
using Selkie.Windsor;
using Xunit;

namespace Selkie.Services.Racetracks.Tests
{
    [SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable")]
    [ExcludeFromCodeCoverage]
    public sealed class RacetracksSourceManagerTests
    {
        public RacetracksSourceManagerTests()
        {
            m_Bus = Substitute.For <ISelkieBus>();
            m_Bus = Substitute.For <ISelkieBus>();
            m_RacetracksDto = new RacetracksDto();
            m_Converter = Substitute.For <IRacetracksToDtoConverter>();
            m_Converter.ConvertPaths(null).ReturnsForAnyArgs(m_RacetracksDto);

            m_RacetrackSettingsSource = Substitute.For <IRacetrackSettingsSource>();
            m_RacetrackSettingsSource.TurnRadiusForPort.Returns(30.0);
            m_RacetrackSettingsSource.TurnRadiusForStarboard.Returns(30.0);

            m_Features = new ISurveyFeature[]
                         {
                         };

            m_SurveyFeaturesSourceManager = Substitute.For <ISurveyFeaturesSourceManager>();
            IEnumerable <ISurveyFeature> features = m_SurveyFeaturesSourceManager.Features;
            features.Returns(m_Features);

            m_RacetrackSettingsSourceManager = Substitute.For <IRacetrackSettingsSourceManager>();
            m_RacetrackSettingsSourceManager.Source.Returns(m_RacetrackSettingsSource);

            m_RacetracksCalculator = Substitute.For <IRacetracksCalculator>();
            m_Factory = Substitute.For <ICalculatorFactory>();
            m_Factory.Create <IRacetracksCalculator>().Returns(m_RacetracksCalculator);

            m_Manager = CreateSut();
        }

        private readonly ISelkieBus m_Bus;
        private readonly IRacetracksToDtoConverter m_Converter;

        private readonly ICalculatorFactory m_Factory;
        private readonly ISurveyFeature[] m_Features;
        private readonly RacetracksSourceManager m_Manager;
        private readonly IRacetracksCalculator m_RacetracksCalculator;
        private readonly RacetracksDto m_RacetracksDto;
        private readonly IRacetrackSettingsSource m_RacetrackSettingsSource;
        private readonly IRacetrackSettingsSourceManager m_RacetrackSettingsSourceManager;
        private readonly ISurveyFeaturesSourceManager m_SurveyFeaturesSourceManager;

        [Fact]
        public void CalculateRacetracks_CallsCalculate_WhenCalled()
        {
            m_Manager.CalculateRacetracks();

            m_RacetracksCalculator.Received().Calculate();
        }

        [Fact]
        public void CalculateRacetracks_CallsCreate_WhenCalled()
        {
            m_Manager.CalculateRacetracks();

            m_Factory.Received().Create <IRacetracksCalculator>();
        }

        [Fact]
        public void CalculateRacetracks_CallsRelease_WhenCalled()
        {
            // assemble
            m_Manager.CalculateRacetracks();

            // act
            m_Manager.CalculateRacetracks();

            // assert
            m_Factory.Received().Release(m_RacetracksCalculator);
        }

        [Fact]
        public void CalculateRacetracks_SendsMessage_WhenCalled()
        {
            m_Manager.CalculateRacetracks();

            m_Bus.Received().PublishAsync(Arg.Any <RacetracksResponseMessage>());
        }

        [Fact]
        public void CalculateRacetracks_SetsFeatures_WhenCalled()
        {
            m_Manager.CalculateRacetracks();

            Assert.Equal(m_SurveyFeaturesSourceManager.Features,
                         m_RacetracksCalculator.Features);
        }

        [Fact]
        public void CalculateRacetracks_SetsTurnRadius_ForPort()
        {
            m_Manager.CalculateRacetracks();

            Assert.Equal(m_RacetrackSettingsSourceManager.Source.TurnRadiusForPort,
                         m_RacetracksCalculator.TurnRadiusForPort.Length);
        }

        [Fact]
        public void CalculateRacetracks_SetsTurnRadius_ForStarboard()
        {
            m_Manager.CalculateRacetracks();

            Assert.Equal(m_RacetrackSettingsSourceManager.Source.TurnRadiusForStarboard,
                         m_RacetracksCalculator.TurnRadiusForStarboard.Length);
        }

        [Fact]
        public void Constructor_SetsFeatures_WhenCalled()
        {
            Assert.Equal(m_Features,
                         m_RacetracksCalculator.Features);
        }

        [Fact]
        public void Constructor_SubscribesToRacetracksGetMessage_WhenCalled()
        {
            // assemble
            RacetracksSourceManager sut = CreateSut();

            // act
            string subscriptionId = sut.GetType().FullName;

            // assert
            m_Bus.Received().SubscribeAsync(subscriptionId,
                                            Arg.Any <Action <RacetracksGetMessage>>());
        }

        [Fact]
        public void Dispose_CallsRelease_WhenCalled()
        {
            // assemble
            RacetracksSourceManager manager = CreateSut();
            manager.CalculateRacetracks();

            // act
            manager.Dispose();

            // assert
            m_Factory.Received().Release(m_RacetracksCalculator);
        }

        [Fact]
        public void Racetracks_ReturnsDefault_WhenCalled()
        {
            Assert.NotNull(m_Manager.Racetracks);
        }

        [Fact]
        public void Racetracks_ReturnsRacetracks_WhenCalled()
        {
            // assemble
            // act
            m_Manager.CalculateRacetracks();

            // assert
            Assert.Equal(m_RacetracksCalculator,
                         m_Manager.Racetracks);
        }

        [Fact]
        public void RacetracksGetHandler_CallsConverter_WhenCalledt()
        {
            // assemble
            RacetracksSourceManager sut = CreateSut();
            var message = new RacetracksGetMessage();

            // act
            sut.RacetracksGetHandler(message);

            // assert
            m_Converter.Received().ConvertPaths(Arg.Any <IRacetracks>());
        }

        [Fact]
        public void RacetracksGetHandlerSendsMessageTest()
        {
            // assemble
            RacetracksSourceManager sut = CreateSut();
            var message = new RacetracksGetMessage();

            // act
            sut.RacetracksGetHandler(message);

            // assert
            m_Bus.Received().PublishAsync(Arg.Any <RacetracksResponseMessage>());
        }

        [Fact]
        public void SendRacetracksResponseMessage_SendsMessage_WhenCalled()
        {
            // assemble
            RacetracksSourceManager sut = CreateSut();

            // act
            sut.SendRacetracksResponseMessage(Substitute.For <IRacetracks>());

            // assert
            m_Bus.Received().PublishAsync(Arg.Is <RacetracksResponseMessage>(x => x.Racetracks == m_RacetracksDto));
        }

        private RacetracksSourceManager CreateSut()
        {
            var manager = new RacetracksSourceManager(Substitute.For <ISelkieLogger>(),
                                                      m_Bus,
                                                      m_SurveyFeaturesSourceManager,
                                                      m_RacetrackSettingsSourceManager,
                                                      m_Factory,
                                                      m_Converter);
            return manager;
        }
    }
}