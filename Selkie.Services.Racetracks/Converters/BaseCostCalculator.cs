using System.Collections.Generic;
using JetBrains.Annotations;
using Selkie.Geometry.Shapes;
using Selkie.Racetrack;

namespace Selkie.Services.Racetracks.Converters
{
    public abstract class BaseCostCalculator : IBaseCostCalculator
    {
        private Dictionary <int, double> m_Costs = new Dictionary <int, double>();
        private ILine m_Line = Geometry.Shapes.Line.Unknown;

        private IEnumerable <ILine> m_Lines = new ILine[]
                                              {
                                              };

        private IRacetracks m_Racetracks = Converters.Racetracks.Unknown;

        public Dictionary <int, double> Costs
        {
            get
            {
                return m_Costs;
            }
        }

        public ILine Line
        {
            get
            {
                return m_Line;
            }
            set
            {
                m_Line = value;
            }
        }

        public IEnumerable <ILine> Lines
        {
            get
            {
                return m_Lines;
            }
            set
            {
                m_Lines = value;
            }
        }

        public IRacetracks Racetracks
        {
            get
            {
                return m_Racetracks;
            }
            set
            {
                m_Racetracks = value;
            }
        }

        public void Calculate()
        {
            m_Costs = CalculateCost();
        }

        [NotNull]
        private Dictionary <int, double> CalculateCost()
        {
            var costs = new Dictionary <int, double>();

            foreach ( ILine otherLine in Lines )
            {
                double cost = CostMatrix.CostToMyself;

                if ( Line.Id != otherLine.Id )
                {
                    cost = CalculateRacetrackCost(Line.Id,
                                                  otherLine.Id);
                }

                costs.Add(otherLine.Id,
                          cost);
            }

            return costs;
        }

        internal abstract double CalculateRacetrackCost(int fromLineId,
                                                        int toLineId);
    }
}