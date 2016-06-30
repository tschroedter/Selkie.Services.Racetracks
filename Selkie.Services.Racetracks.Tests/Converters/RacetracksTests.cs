using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;
using Selkie.Racetrack.Interfaces;

namespace Selkie.Services.Racetracks.Tests.Converters
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    internal sealed class RacetracksTests
    {
        [Test]
        public void ForwardToForwardDefaultTest()
        {
            IPath[][] actual = Racetracks.Converters.Dtos.Racetracks.Unknown.ForwardToForward;

            Assert.AreEqual(0,
                            actual.GetLength(0));
        }

        [Test]
        public void ForwardToReverseDefaultTest()
        {
            IPath[][] actual = Racetracks.Converters.Dtos.Racetracks.Unknown.ForwardToReverse;

            Assert.AreEqual(0,
                            actual.GetLength(0));
        }

        [Test]
        public void IsUnknownTest()
        {
            bool actual = Racetracks.Converters.Dtos.Racetracks.Unknown.IsUnknown;

            Assert.True(actual);
        }

        [Test]
        public void ReverseToForwardDefaultTest()
        {
            IPath[][] actual = Racetracks.Converters.Dtos.Racetracks.Unknown.ReverseToForward;

            Assert.AreEqual(0,
                            actual.GetLength(0));
        }

        [Test]
        public void ReverseToReverseDefaultTest()
        {
            IPath[][] actual = Racetracks.Converters.Dtos.Racetracks.Unknown.ReverseToReverse;

            Assert.AreEqual(0,
                            actual.GetLength(0));
        }
    }
}