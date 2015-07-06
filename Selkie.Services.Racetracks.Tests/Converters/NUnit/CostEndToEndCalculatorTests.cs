using System.Diagnostics.CodeAnalysis;
using Castle.Core.Logging;
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

            m_Calculator = new CostEndToEndCalculator(Substitute.For <ILogger>())
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

            const double expected = 200.0;
            double actual = m_Calculator.CalculateRacetrackCost(0,
                                                                1);

            NUnitHelper.AssertIsEquivalent(expected,
                                           actual,
                                           Constants.EpsilonDistance,
                                           "Racetrack length is wrong!");
        }
    }
}