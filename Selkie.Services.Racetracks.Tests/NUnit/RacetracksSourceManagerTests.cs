using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using NSubstitute;
using NUnit.Framework;
using Selkie.EasyNetQ;
using Selkie.Geometry.Shapes;
using Selkie.Racetrack.Interfaces;
using Selkie.Racetrack.Interfaces.Calculators;
using Selkie.Services.Common.Dto;
using Selkie.Services.Racetracks.Common.Messages;
using Selkie.Services.Racetracks.Interfaces;
using Selkie.Windsor;

namespace Selkie.Services.Racetracks.Tests.NUnit
{
    [SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable")]
    [TestFixture]
    //ncrunch: no coverage start
    [ExcludeFromCodeCoverage]
    internal sealed class RacetracksSourceManagerTests
    {
        [SetUp]
        public void Setup()
        {
            m_Bus = Substitute.For <ISelkieBus>();
            m_Bus = Substitute.For <ISelkieBus>();
            m_RacetracksDto = new RacetracksDto();
            m_Converter = Substitute.For <IRacetracksToDtoConverter>();
            m_Converter.ConvertPaths(null).ReturnsForAnyArgs(m_RacetracksDto);

            m_RacetrackSettingsSource = Substitute.For <IRacetrackSettingsSource>();
            m_RacetrackSettingsSource.TurnRadiusForPort.Returns(30.0);
            m_RacetrackSettingsSource.TurnRadiusForStarboard.Returns(30.0);

            m_Lines = new ILine[]
                      {
                      };

            m_LinesSourceManager = Substitute.For <ILinesSourceManager>();
            IEnumerable <ILine> lines = m_LinesSourceManager.Lines;
            lines.Returns(m_Lines);

            m_RacetrackSettingsSourceManager = Substitute.For <IRacetrackSettingsSourceManager>();
            m_RacetrackSettingsSourceManager.Source.Returns(m_RacetrackSettingsSource);

            m_RacetracksCalculator = Substitute.For <IRacetracksCalculator>();
            m_Factory = Substitute.For <ICalculatorFactory>();
            m_Factory.Create <IRacetracksCalculator>().Returns(m_RacetracksCalculator);

            m_Manager = CreateSut();
        }

        [TearDown]
        public void TearDown()
        {
            m_Manager.Dispose();
        }

        private ICalculatorFactory m_Factory;
        private ILine[] m_Lines;
        private ILinesSourceManager m_LinesSourceManager;
        private RacetracksSourceManager m_Manager;
        private IRacetracksCalculator m_RacetracksCalculator;
        private IRacetrackSettingsSource m_RacetrackSettingsSource;
        private IRacetrackSettingsSourceManager m_RacetrackSettingsSourceManager;
        private ISelkieBus m_Bus;
        private IRacetracksToDtoConverter m_Converter;
        private RacetracksDto m_RacetracksDto;

        private RacetracksSourceManager CreateSut()
        {
            var manager = new RacetracksSourceManager(Substitute.For <ISelkieLogger>(),
                                                      m_Bus,
                                                      m_LinesSourceManager,
                                                      m_RacetrackSettingsSourceManager,
                                                      m_Factory,
                                                      m_Converter);
            return manager;
        }

        [Test]
        public void CalculateRacetracks_CallsCalculate_WhenCalled()
        {
            m_Manager.CalculateRacetracks();

            m_RacetracksCalculator.Received().Calculate();
        }

        [Test]
        public void CalculateRacetracks_CallsCreate_WhenCalled()
        {
            m_Manager.CalculateRacetracks();

            m_Factory.Received().Create <IRacetracksCalculator>();
        }

        [Test]
        public void CalculateRacetracks_CallsRelease_WhenCalled()
        {
            // assemble
            m_Manager.CalculateRacetracks();

            // act
            m_Manager.CalculateRacetracks();

            // assert
            m_Factory.Received().Release(m_RacetracksCalculator);
        }

        [Test]
        public void CalculateRacetracks_SendsMessage_WhenCalled()
        {
            m_Manager.CalculateRacetracks();

            m_Bus.Received().PublishAsync(Arg.Any <RacetracksResponseMessage>());
        }

        [Test]
        public void CalculateRacetracks_SetsLines_WhenCalled()
        {
            m_Manager.CalculateRacetracks();

            Assert.AreEqual(m_LinesSourceManager.Lines,
                            m_RacetracksCalculator.Lines);
        }

        [Test]
        public void CalculateRacetracks_SetsTurnRadius_ForPort()
        {
            m_Manager.CalculateRacetracks();

            Assert.AreEqual(m_RacetrackSettingsSourceManager.Source.TurnRadiusForPort,
                            m_RacetracksCalculator.TurnRadiusForPort.Length);
        }

        [Test]
        public void CalculateRacetracks_SetsTurnRadius_ForStarboard()
        {
            m_Manager.CalculateRacetracks();

            Assert.AreEqual(m_RacetrackSettingsSourceManager.Source.TurnRadiusForStarboard,
                            m_RacetracksCalculator.TurnRadiusForStarboard.Length);
        }

        [Test]
        public void Constructor_SetsLines_WhenCalled()
        {
            Assert.AreEqual(m_Lines,
                            m_RacetracksCalculator.Lines);
        }

        [Test]
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

        [Test]
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

        [Test]
        public void Racetracks_ReturnsRacetracks_WhenCalled()
        {
            // assemble
            // act
            m_Manager.CalculateRacetracks();

            // assert
            Assert.AreEqual(m_RacetracksCalculator,
                            m_Manager.Racetracks);
        }

        [Test]
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

        [Test]
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

        [Test]
        public void SendRacetracksResponseMessage_SendsMessage_WhenCalled()
        {
            // assemble
            RacetracksSourceManager sut = CreateSut();

            // act
            sut.SendRacetracksResponseMessage(Substitute.For <IRacetracks>());

            // assert
            m_Bus.Received().PublishAsync(Arg.Is <RacetracksResponseMessage>(x => x.Racetracks == m_RacetracksDto));
        }
    }
}