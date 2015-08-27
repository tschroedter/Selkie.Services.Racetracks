using System.Collections.Generic;
using JetBrains.Annotations;
using Selkie.Geometry.Shapes;
using Selkie.Racetrack;
using Selkie.Windsor;
using Selkie.Windsor.Extensions;

namespace Selkie.Services.Racetracks.Converters
{
    // todo use Fody!!!!
    public abstract class BaseCostCalculator : IBaseCostCalculator
    {
        private readonly ISelkieLogger m_Logger;
        private readonly object m_Padlock = new object();
        private Dictionary <int, double> m_Costs = new Dictionary <int, double>();
        private ILine m_Line = Geometry.Shapes.Line.Unknown;

        private IEnumerable <ILine> m_Lines = new ILine[]
                                              {
                                              };

        private IRacetracks m_Racetracks = Dtos.Racetracks.Unknown;

        protected BaseCostCalculator([NotNull] ISelkieLogger logger)
        {
            m_Logger = logger;
        }

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
            lock ( m_Padlock )
            {
                m_Costs = CalculateCost();
            }
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
                    int fromLineId = Line.Id;
                    int toLineId = otherLine.Id;

                    cost = CheckAndCalculate(fromLineId,
                                             toLineId);
                }

                costs.Add(otherLine.Id,
                          cost);
            }

            return costs;
        }

        internal double CheckAndCalculate(int fromLineId,
                                          int toLineId)
        {
            if ( !IsValidLineId(fromLineId) )
            {
                string message = "fromLineId = {0} paths[{1}][]".Inject(fromLineId,
                                                                        m_Racetracks.ForwardToForward.Length);
                m_Logger.Warn(message);

                return 0.0;
            }

            if ( !IsValidLineId(toLineId) )
            {
                string message = "toLineId = {0} paths[{1}][]".Inject(fromLineId,
                                                                      m_Racetracks.ForwardToForward.Length);
                m_Logger.Warn(message);

                return 0.0;
            }

            return CalculateRacetrackCost(fromLineId,
                                          toLineId);
        }

        private bool IsValidLineId(int lineId)
        {
            int length = m_Racetracks.ForwardToForward.Length;

            if ( length <= 0 )
            {
                return false;
            }

            if ( lineId < 0 ||
                 lineId >= length )
            {
                return false;
            }

            return true;
        }

        internal abstract double CalculateRacetrackCost(int fromLineId,
                                                        int toLineId);
    }
}