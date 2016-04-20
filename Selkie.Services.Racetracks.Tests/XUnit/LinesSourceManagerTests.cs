using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using JetBrains.Annotations;
using NSubstitute;
using Ploeh.AutoFixture.Xunit;
using Selkie.Geometry.Shapes;
using Selkie.Services.Common.Dto;
using Selkie.Windsor;
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
        public void SetLinesIfValid_ReplacesNullWithEmpty_ForLineDtos(
            [NotNull] LinesSourceManager manager)
        {
            // assemble
            // act
            // ReSharper disable once AssignNullToNotNullAttribute
            manager.SetLinesIfValid(null);

            // assert
            Assert.Equal(0,
                         manager.Lines.Count());
        }

        [Theory]
        [AutoNSubstituteData]
        public void SetLinesIfValid_LogsError_ForLineDtosIsNull(
            [NotNull] [Frozen] ISelkieLogger logger,
            [NotNull] LinesSourceManager manager)
        {
            // assemble
            // act
            // ReSharper disable once AssignNullToNotNullAttribute
            manager.SetLinesIfValid(null);

            // assert
            logger.Received().Error(Arg.Any <string>());
        }

        [Theory]
        [AutoNSubstituteData]
        public void SetLinesIfValid_UpdatesSourceCount_ForLineDtos(
            [NotNull] LinesSourceManager manager)
        {
            // assemble
            LineDto[] lineDtos = CreateLineDtos();

            // act
            manager.SetLinesIfValid(lineDtos);

            // assert
            IEnumerable <ILine> actual = manager.Lines;

            Assert.Equal(lineDtos.Length,
                         actual.Count());
        }

        [Theory]
        [AutoNSubstituteData]
        public void StartCallsLoggerTest([NotNull] [Frozen] ISelkieLogger logger,
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
        public void StopCallsLoggerTest([NotNull] [Frozen] ISelkieLogger logger,
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
    }
}