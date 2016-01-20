using JetBrains.Annotations;
using NSubstitute;
using Selkie.Racetrack;
using Selkie.Services.Common.Dto;
using Selkie.Services.Racetracks.Converters.Dtos;
using Selkie.XUnit.Extensions;
using Xunit;
using Xunit.Extensions;

namespace Selkie.Services.Racetracks.Tests.Converters.Dtos.XUnit
{
    public class RacetracksToDtoConverterTests
    {
        [Theory]
        [AutoNSubstituteData]
        public void Convert_ReturnsDto_ForPRacetracks([NotNull] IPathToPathDtoConverter converter)
        {
            // Arrange
            var sut = new RacetracksToDtoConverter(converter);

            // Act
            RacetracksDto actual = sut.ConvertPaths(CreateRacetracks());

            // Assert
            Assert.True(actual.ForwardToForward.Length == 2,
                        "ForwardToForward");
            Assert.True(actual.ForwardToReverse.Length == 2,
                        "ForwardToReverse");
            Assert.True(actual.ReverseToForward.Length == 2,
                        "ReverseToForward");
            Assert.True(actual.ReverseToReverse.Length == 2,
                        "ReverseToReverse");
        }

        private IRacetracks CreateRacetracks()
        {
            var racetracks = Substitute.For <IRacetracks>();

            racetracks.ForwardToForward.Returns(CreatePathArrays());
            racetracks.ForwardToReverse.Returns(CreatePathArrays());
            racetracks.ReverseToForward.Returns(CreatePathArrays());
            racetracks.ReverseToReverse.Returns(CreatePathArrays());

            return racetracks;
        }

        [Theory]
        [AutoNSubstituteData]
        public void ConvertPathsConvert_ReturnsDto_ForPathArrays([NotNull] IPathToPathDtoConverter converter)
        {
            // Arrange
            var sut = new RacetracksToDtoConverter(converter);

            // Act
            PathDto[][] actual = sut.ConvertPaths(CreatePathArrays());

            // Assert
            Assert.True(actual.Length == 2,
                        "Length");
            Assert.True(actual [ 0 ].Length == 2,
                        "[0] Length");
        }

        [Theory]
        [AutoNSubstituteData]
        public void ConvertPathsConvert_CallsConverter_ForPathArrays([NotNull] IPathToPathDtoConverter converter)
        {
            // Arrange
            var sut = new RacetracksToDtoConverter(converter);

            // Act
            sut.ConvertPaths(CreatePathArrays());

            // Assert
            converter.ReceivedWithAnyArgs(4).Convert();
        }

        private IPath[][] CreatePathArrays()
        {
            IPath[][] pathArrays =
            {
                CreatePathArray(),
                CreatePathArray()
            };

            return pathArrays;
        }

        private IPath[] CreatePathArray()
        {
            var paths = new[]
                        {
                            Substitute.For <IPath>(),
                            Substitute.For <IPath>()
                        };

            return paths;
        }
    }
}