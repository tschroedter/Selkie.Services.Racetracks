﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Castle.Core.Logging;
using EasyNetQ;
using JetBrains.Annotations;
using NSubstitute;
using Ploeh.AutoFixture.Xunit;
using Selkie.Geometry.Shapes;
using Selkie.Services.Racetracks.Common.Dto;
using Selkie.Services.Racetracks.Common.Messages;
using Selkie.XUnit.Extensions;
using Xunit;
using Xunit.Extensions;

namespace Selkie.Services.Racetracks.Tests.XUnit
{
    //ncrunch: no coverage start
    // ReSharper disable once MaximumChainedReferences
    [ExcludeFromCodeCoverage]
    public sealed class LinesSourceManagerTests
    {
        [Theory]
        [AutoNSubstituteData]
        public void SubscribesToLinesSetMessageTest([NotNull] [Frozen] IBus bus,
                                                    [NotNull] LinesSourceManager manager)
        {
            // assemble
            // act
            // assert
            string subscriptionId = manager.GetType().FullName;

            bus.Received().SubscribeAsync(subscriptionId,
                                          Arg.Any <Func <LinesSetMessage, Task>>());
        }

        [Theory]
        [AutoNSubstituteData]
        public void LinesSetMessageHandlerReplacesNullForLineDtosWithEmptyTest([NotNull] LinesSourceManager manager)
        {
            // assemble
            var message = new LinesSetMessage();

            // act
            manager.LinesSetMessageHandler(message);

            // assert
            IEnumerable <ILine> actual = manager.Lines;

            Assert.Equal(0,
                         actual.Count());
        }

        [Theory]
        [AutoNSubstituteData]
        public void LinesSetMessageHandlerLogsErrorForLineDtosIsNullTest([NotNull] [Frozen] ILogger logger,
                                                                         [NotNull] LinesSourceManager manager)
        {
            // assemble
            var message = new LinesSetMessage();

            // act
            manager.LinesSetMessageHandler(message);

            // assert
            logger.Received().Error(Arg.Any <string>());
        }

        [Theory]
        [AutoNSubstituteData]
        public void LinesSetMessageHandlerUpdatesSourceCountTest([NotNull] LinesSourceManager manager)
        {
            // assemble
            LineDto[] dtos = CreateLineDtos();
            var message = new LinesSetMessage
                          {
                              LineDtos = dtos
                          };

            // act
            manager.LinesSetMessageHandler(message);

            // assert
            IEnumerable <ILine> actual = manager.Lines;

            Assert.Equal(dtos.Count(),
                         actual.Count());
        }

        [Theory]
        [AutoNSubstituteData]
        public void LinesSetMessageHandlerSendsResponseTest([NotNull] [Frozen] IBus bus,
                                                            [NotNull] [Frozen] ILinesValidator validator,
                                                            [NotNull] LinesSourceManager manager)
        {
            // assemble
            validator.ValidateDtos(Arg.Any <IEnumerable <LineDto>>()).Returns(true);

            LineDto[] dtos = CreateLineDtos();
            var message = new LinesSetMessage
                          {
                              LineDtos = dtos
                          };

            // act
            manager.LinesSetMessageHandler(message);

            // assert
            bus.Received().PublishAsync(Arg.Is <LinesChangedMessage>(m => m.LineDtos.Count() == dtos.Count()));
        }

        [Theory]
        [AutoNSubstituteData]
        public void LinesSetMessageHandlerIgnorsSetForInvalidLineDtoTest([NotNull] [Frozen] IBus bus,
                                                                         [NotNull] LinesSourceManager manager)
        {
            // assemble
            LineDto[] valid = CreateLineDtos();
            var messageValid = new LinesSetMessage
                               {
                                   LineDtos = valid
                               };
            manager.LinesSetMessageHandler(messageValid);

            IEnumerable <LineDto> invalid = CreateInvalidLineDtosOnlyOneLineDto();
            var messageInvalid = new LinesSetMessage
                                 {
                                     LineDtos = invalid.ToArray()
                                 };

            // act
            manager.LinesSetMessageHandler(messageInvalid);

            // assert
            bus.Received().PublishAsync(Arg.Is <LinesChangedMessage>(m => m.LineDtos.Count() == valid.Count()));
        }

        [Theory]
        [AutoNSubstituteData]
        public void StartCallsLoggerTest([NotNull] [Frozen] ILogger logger,
                                         [NotNull] LinesSourceManager manager)
        {
            // assemble
            // act
            manager.Start();

            // assert
            logger.Received().Info(Arg.Is <string>(x => x.StartsWith("Started")));
        }

        [Theory]
        [AutoNSubstituteData]
        public void StopCallsLoggerTest([NotNull] [Frozen] ILogger logger,
                                        [NotNull] LinesSourceManager manager)
        {
            // assemble
            // act
            manager.Stop();

            // assert
            logger.Received().Info(Arg.Is <string>(x => x.StartsWith("Stopped")));
        }

        [NotNull]
        private static LineDto[] CreateLineDtos()
        {
            var lineOne = Substitute.For <ILine>();
            lineOne.Id.Returns(0);

            var lineTwo = Substitute.For <ILine>();
            lineTwo.Id.Returns(1);

            LineDto lineDtoOne = LineToLineDtoConverter.ConvertFrom(lineOne);
            LineDto lineDtoTwo = LineToLineDtoConverter.ConvertFrom(lineOne);

            LineDto[] lineDtos =
            {
                lineDtoOne,
                lineDtoTwo
            };

            return lineDtos;
        }

        [NotNull]
        private static IEnumerable <LineDto> CreateInvalidLineDtosOnlyOneLineDto()
        {
            var lineOne = Substitute.For <ILine>();
            lineOne.Id.Returns(0);

            LineDto lineDtoOne = LineToLineDtoConverter.ConvertFrom(lineOne);

            LineDto[] lineDtos =
            {
                lineDtoOne
            };

            return lineDtos;
        }
    }
}