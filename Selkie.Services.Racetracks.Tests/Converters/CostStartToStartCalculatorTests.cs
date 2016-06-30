using System.Diagnostics.CodeAnalysis;
using NSubstitute;
using NUnit.Framework;
using Selkie.Geometry.Primitives;
using Selkie.NUnit.Extensions;
using Selkie.Racetrack.Interfaces;
using Selkie.Services.Racetracks.Converters;
using Selkie.Windsor;

namespace Selkie.Services.Racetracks.Tests.Converters
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    internal sealed class CostStartToStartCalculatorTests
    {
        public CostStartToStartCalculatorTests()
        {
            var path = Substitute.For <IPath>();
            path.Distance.Returns(new Distance(100.0));
            m_Paths = new[]
                      {
                          new[]
                          {
                              null,
                              path
                          }
                      };

            m_Racetracks = Substitute.For <IRacetracks>();

            m_Calculator = new CostStartToStartCalculator(Substitute.For <ISelkieLogger>());
        }

        private readonly CostStartToStartCalculator m_Calculator;
        private readonly IPath[][] m_Paths;
        private readonly IRacetracks m_Racetracks;

        [Test]
        public void CalculateRacetrackCostTest()
        {
            m_Racetracks.ReverseToForward.Returns(m_Paths);
            m_Calculator.Racetracks = m_Racetracks;

            const double expected = 100.0;
            double actual = m_Calculator.CalculateRacetrackCost(0,
                                                                1);

            NUnitHelper.AssertIsEquivalent(expected,
                                           actual,
                                           Constants.EpsilonDistance,
                                           "Racetrack length is wrong!");
        }
    }
}