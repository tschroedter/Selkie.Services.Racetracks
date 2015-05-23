using System.Diagnostics.CodeAnalysis;
using Selkie.Racetrack;
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
            bool actual = Racetracks.Converters.Racetracks.Unknown.IsUnknown;

            Assert.True(actual);
        }

        [Fact]
        public void ForwardToForwardDefaultTest()
        {
            IPath[][] actual = Racetracks.Converters.Racetracks.Unknown.ForwardToForward;

            Assert.Equal(0,
                         actual.GetLength(0));
        }

        [Fact]
        public void ForwardToReverseDefaultTest()
        {
            IPath[][] actual = Racetracks.Converters.Racetracks.Unknown.ForwardToReverse;

            Assert.Equal(0,
                         actual.GetLength(0));
        }

        [Fact]
        public void ReverseToForwardDefaultTest()
        {
            IPath[][] actual = Racetracks.Converters.Racetracks.Unknown.ReverseToForward;

            Assert.Equal(0,
                         actual.GetLength(0));
        }

        [Fact]
        public void ReverseToReverseDefaultTest()
        {
            IPath[][] actual = Racetracks.Converters.Racetracks.Unknown.ReverseToReverse;

            Assert.Equal(0,
                         actual.GetLength(0));
        }
    }
}