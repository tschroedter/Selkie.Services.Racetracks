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
    public sealed class CostEndToEndCalculatorTests
    {
        public CostEndToEndCalculatorTests()
        {
            var pathOne = Substitute.For <IPath>();
            pathOne.Distance.Returns(new Distance(100.0));
            var pathTwo = Substitute.For <IPath>();
            pathTwo.Distance.Returns(new Distance(200.0));

            m_Paths = new[]
                      {
                          new[]
                          {
                              pathOne,
                              pathTwo
                          },
                          new[]
                          {
                              pathOne,
                              pathTwo
                          }
                      };

            m_Racetracks = Substitute.For <IRacetracks>();

            m_Calculator = new CostEndToEndCalculator(Substitute.For <ISelkieLogger>())
                           {
                               Racetracks = m_Racetracks
                           };
        }

        private readonly CostEndToEndCalculator m_Calculator;
        private readonly IPath[][] m_Paths;
        private readonly IRacetracks m_Racetracks;

        [Fact]
        public void CalculateRacetrackCostTest()
        {
            m_Racetracks.ForwardToReverse.Returns(m_Paths);

            const double expected = 200.0;
            double actual = m_Calculator.CalculateRacetrackCost(0,
                                                                1);

            XUnitHelper.AssertIsEquivalent(expected,
                                           actual,
                                           Constants.EpsilonDistance,
                                           "Racetrack length is wrong!");
        }
    }
}