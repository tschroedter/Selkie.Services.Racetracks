using System.Diagnostics.CodeAnalysis;
using NSubstitute;
using NUnit.Framework;
using Selkie.Geometry.Primitives;
using Selkie.NUnit.Extensions;
using Selkie.Racetrack;
using Selkie.Services.Racetracks.Converters;
using Constants = Selkie.Common.Constants;

namespace Selkie.Services.Racetracks.Tests.Converters.NUnit
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    internal sealed class CostEndToEndCalculatorTests
    {
        [SetUp]
        public void Setup()
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

            m_Calculator = new CostEndToEndCalculator
                           {
                               Racetracks = m_Racetracks
                           };
        }

        private CostEndToEndCalculator m_Calculator;
        private IPath[][] m_Paths;
        private IRacetracks m_Racetracks;

        [Test]
        public void CalculateRacetrackCostTest()
        {
            m_Racetracks.ForwardToReverse.Returns(m_Paths);

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