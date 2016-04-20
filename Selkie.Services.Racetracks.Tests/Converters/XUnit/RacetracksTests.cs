using System.Diagnostics.CodeAnalysis;
using Selkie.Racetrack.Interfaces;
using Xunit;

namespace Selkie.Services.Racetracks.Tests.Converters.XUnit
{
    [ExcludeFromCodeCoverage]
    //ncrunch: no coverage start
    public sealed class RacetracksTests
    {
        [Fact]
        public void IsUnknownTest()
        {
            bool actual = Racetracks.Converters.Dtos.Racetracks.Unknown.IsUnknown;

            Assert.True(actual);
        }

        [Fact]
        public void ForwardToForwardDefaultTest()
        {
            IPath[][] actual = Racetracks.Converters.Dtos.Racetracks.Unknown.ForwardToForward;

            Assert.Equal(0,
                         actual.GetLength(0));
        }

        [Fact]
        public void ForwardToReverseDefaultTest()
        {
            IPath[][] actual = Racetracks.Converters.Dtos.Racetracks.Unknown.ForwardToReverse;

            Assert.Equal(0,
                         actual.GetLength(0));
        }

        [Fact]
        public void ReverseToForwardDefaultTest()
        {
            IPath[][] actual = Racetracks.Converters.Dtos.Racetracks.Unknown.ReverseToForward;

            Assert.Equal(0,
                         actual.GetLength(0));
        }

        [Fact]
        public void ReverseToReverseDefaultTest()
        {
            IPath[][] actual = Racetracks.Converters.Dtos.Racetracks.Unknown.ReverseToReverse;

            Assert.Equal(0,
                         actual.GetLength(0));
        }
    }
}