using System.Diagnostics.CodeAnalysis;
using NSubstitute;
using Selkie.Common;
using Selkie.Geometry.Primitives;
using Selkie.Racetrack.Interfaces;
using Selkie.Services.Racetracks.Converters;
using Selkie.Windsor;
using Selkie.XUnit.Extensions;
using Xunit;

namespace Selkie.Services.Racetracks.Tests.Converters
{
    [ExcludeFromCodeCoverage]
    public sealed class CostStartToEndCalculatorTests
    {
        public CostStartToEndCalculatorTests()
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

            m_Calculator = new CostStartToEndCalculator(Substitute.For <ISelkieLogger>())
                           {
                               Racetracks = m_Racetracks
                           };
        }

        private readonly CostStartToEndCalculator m_Calculator;
        private readonly IPath[][] m_Paths;
        private readonly IRacetracks m_Racetracks;

        [Fact]
        public void CalculateRacetrackCostTest()
        {
            m_Racetracks.ReverseToReverse.Returns(m_Paths);

            const double expected = 100.0;
            double actual = m_Calculator.CalculateRacetrackCost(0,
                                                                1);

            XUnitHelper.AssertIsEquivalent(expected,
                                           actual,
                                           Constants.EpsilonDistance,
                                           "Racetrack length is wrong!");
        }
    }
}