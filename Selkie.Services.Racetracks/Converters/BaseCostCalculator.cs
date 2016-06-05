using System.Collections.Generic;
using JetBrains.Annotations;
using Selkie.Geometry.Surveying;
using Selkie.Racetrack.Interfaces;
using Selkie.Services.Racetracks.Interfaces.Converters;
using Selkie.Windsor;
using Selkie.Windsor.Extensions;

namespace Selkie.Services.Racetracks.Converters
{
    public abstract class BaseCostCalculator : IBaseCostCalculator
    {
        protected BaseCostCalculator([NotNull] ISelkieLogger logger)
        {
            Logger = logger;
        }

        protected readonly ISelkieLogger Logger;
        private readonly object m_Padlock = new object();
        private Dictionary <int, double> m_Costs = new Dictionary <int, double>();
        private ISurveyFeature m_Feature = SurveyFeature.Unknown;

        private IEnumerable <ISurveyFeature> m_Features = new ISurveyFeature[0];

        private IRacetracks m_Racetracks = Dtos.Racetracks.Unknown;

        public Dictionary <int, double> Costs
        {
            get
            {
                return m_Costs;
            }
        }

        public ISurveyFeature Feature
        {
            get
            {
                return m_Feature;
            }
            set
            {
                m_Feature = value;
            }
        }

        public IEnumerable <ISurveyFeature> Features
        {
            get
            {
                return m_Features;
            }
            set
            {
                m_Features = value;
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

        internal abstract double CalculateRacetrackCost(int fromFeatureId,
                                                        int toFeatureId);

        internal double CheckAndCalculate(int fromFeatureId,
                                          int toFeatureId)
        {
            if ( !IsValidFeatureId(fromFeatureId) )
            {
                string message = "fromFeatureId = {0} paths[{1}][]".Inject(fromFeatureId,
                                                                           m_Racetracks.ForwardToForward.Length);
                Logger.Warn(message);

                return 0.0;
            }

            if ( IsValidFeatureId(toFeatureId) )
            {
                return CalculateRacetrackCost(fromFeatureId,
                                              toFeatureId);
            }

            string text = "toFeatureId = {0} paths[{1}][]".Inject(fromFeatureId,
                                                                  m_Racetracks.ForwardToForward.Length);
            Logger.Warn(text);

            return 0.0;
        }

        [NotNull]
        private Dictionary <int, double> CalculateCost()
        {
            var costs = new Dictionary <int, double>();

            foreach ( ISurveyFeature otherFeature in Features )
            {
                double cost = CostMatrix.CostToMyself;

                if ( Feature.Id != otherFeature.Id )
                {
                    int fromFeatureId = Feature.Id;
                    int toFeatureId = otherFeature.Id;

                    cost = CheckAndCalculate(fromFeatureId,
                                             toFeatureId);
                }

                costs.Add(otherFeature.Id,
                          cost);
            }

            return costs;
        }

        private bool IsValidFeatureId(int lineId)
        {
            int length = m_Racetracks.ForwardToForward.Length;

            if ( length <= 0 )
            {
                return false;
            }

            return lineId >= 0 && lineId < length;
        }
    }
}