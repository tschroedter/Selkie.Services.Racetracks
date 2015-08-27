using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using NSubstitute;
using NUnit.Framework;
using Selkie.EasyNetQ;
using Selkie.Geometry.Primitives;
using Selkie.Geometry.Shapes;
using Selkie.Racetrack;
using Selkie.Racetrack.Calculators;
using Selkie.Services.Racetracks.Common.Dto;
using Selkie.Services.Racetracks.Common.Messages;
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
            m_RacetrackSettingsSource.TurnRadius.Returns(new Distance(30.0));

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
        public void CalculateCallsCalculateTest()
        {
            m_Manager.Update();

            m_RacetracksCalculator.Received().Calculate();
        }

        [Test]
        public void CalculateCallsCreateTest()
        {
            m_Manager.Update();

            m_Factory.Received().Create <IRacetracksCalculator>();
        }

        [Test]
        public void CalculateCallsReleaseTest()
        {
            // assemble
            m_Manager.Update();

            // act
            m_Manager.Update();

            // assert
            m_Factory.Received().Release(m_RacetracksCalculator);
        }

        [Test]
        public void CalculateSendsMessageTest()
        {
            m_Manager.Update();

            m_Bus.Received().PublishAsync(Arg.Any <RacetracksChangedMessage>());
        }

        [Test]
        public void CalculateSetsLinesTest()
        {
            m_Manager.Update();

            Assert.AreEqual(m_LinesSourceManager.Lines,
                            m_RacetracksCalculator.Lines);
        }

        [Test]
        public void CalculateSetsRadiusTest()
        {
            m_Manager.Update();

            Assert.AreEqual(m_RacetrackSettingsSourceManager.Source.TurnRadius,
                            m_RacetracksCalculator.Radius);
        }

        [Test]
        public void ConstructorSetsLinesTest()
        {
            Assert.AreEqual(m_Lines,
                            m_RacetracksCalculator.Lines);
        }

        [Test]
        public void DisposeCallsReleaseTest()
        {
            // assemble
            RacetracksSourceManager manager = CreateSut();
            manager.Update();

            // act
            manager.Dispose();

            // assert
            m_Factory.Received().Release(m_RacetracksCalculator);
        }

        [Test]
        public void RacetrackSettingsChangedMessageCallsUpdateTest()
        {
            // assemble
            RacetracksSourceManager sut = CreateSut();
            var message = new RacetrackSettingsChangedMessage();

            // act
            sut.RacetrackSettingsChangedHandler(message);

            // assert
            m_RacetracksCalculator.Received().Calculate();
        }

        [Test]
        public void RacetracksGetHandlerCallsConverterTest()
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
            m_Bus.Received().PublishAsync(Arg.Any <RacetracksChangedMessage>());
        }

        [Test]
        public void RacetracksTest()
        {
            // assemble
            // act
            m_Manager.Update();

            // assert
            Assert.AreEqual(m_RacetracksCalculator,
                            m_Manager.Racetracks);
        }

        [Test]
        public void SendRacetracksChangedMessageSendsMessageTest()
        {
            // assemble
            RacetracksSourceManager sut = CreateSut();

            // act
            sut.SendRacetracksChangedMessage(Substitute.For <IRacetracks>());

            // assert
            m_Bus.Received().PublishAsync(Arg.Is <RacetracksChangedMessage>(x => x.Racetracks == m_RacetracksDto));
        }

        [Test]
        public void SendsRacetrackSettingsGetMessageTest()
        {
            // assemble
            // ReSharper disable once UnusedVariable
            RacetracksSourceManager sut = CreateSut();

            // act
            // assert
            m_Bus.Received().PublishAsync(Arg.Any <RacetrackSettingsGetMessage>());
        }

        [Test]
        public void SubscribesToRacetrackSettingsChangedMessageTest()
        {
            // assemble
            RacetracksSourceManager sut = CreateSut();

            // act
            string subscriptionId = sut.GetType().FullName;

            // assert
            m_Bus.Received().SubscribeAsync(subscriptionId,
                                            Arg.Any <Action <RacetrackSettingsChangedMessage>>());
        }

        [Test]
        public void SubscribesToRacetracksGetMessageTest()
        {
            // assemble
            RacetracksSourceManager sut = CreateSut();

            // act
            string subscriptionId = sut.GetType().FullName;

            // assert
            m_Bus.Received().SubscribeAsync(subscriptionId,
                                            Arg.Any <Action <RacetracksGetMessage>>());
        }
    }
}