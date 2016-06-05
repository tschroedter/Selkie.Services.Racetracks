using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Selkie.Geometry.Surveying;
using Selkie.Racetrack.Interfaces;
using Selkie.Services.Racetracks.Interfaces.Converters;

namespace Selkie.Services.Racetracks.Converters
{
    public class SurveyFeatureToSurveyFeaturesConverter : ISurveyFeatureToSurveyFeaturesConverter
    {
        public SurveyFeatureToSurveyFeaturesConverter([NotNull] ICostStartToStartCalculator costStartToStartCalculator,
                                                      [NotNull] ICostStartToEndCalculator costStartToEndCalculator,
                                                      [NotNull] ICostEndToStartCalculator costEndToStartCalculator,
                                                      [NotNull] ICostEndToEndCalculator costEndToEndCalculator)
        {
            m_CostStartToStartCalculator = costStartToStartCalculator;
            m_CostStartToEndCalculator = costStartToEndCalculator;
            m_CostEndToStartCalculator = costEndToStartCalculator;
            m_CostEndToEndCalculator = costEndToEndCalculator;

            m_CostCalculators = new IBaseCostCalculator[]
                                {
                                    m_CostStartToStartCalculator,
                                    m_CostStartToEndCalculator,
                                    m_CostEndToStartCalculator,
                                    m_CostEndToEndCalculator
                                };
        }

        private readonly IBaseCostCalculator[] m_CostCalculators;
        private readonly ICostEndToEndCalculator m_CostEndToEndCalculator;
        private readonly ICostEndToStartCalculator m_CostEndToStartCalculator;
        private readonly ICostStartToEndCalculator m_CostStartToEndCalculator;
        private readonly ICostStartToStartCalculator m_CostStartToStartCalculator;
        private ISurveyFeature m_Feature = SurveyFeature.Unknown;

        private IEnumerable <ISurveyFeature> m_Features = new ISurveyFeature[0];

        private IRacetracks m_Racetracks = Dtos.Racetracks.Unknown;

        #region ISurveyFeatureToSurveyFeaturesConverter Members

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

        public double BaseCost
        {
            get
            {
                return m_Feature.Length;
            }
        }

        public double CostForwardForward(ISurveyFeature other)
        {
            return CostEndToStart(other);
        }

        public double CostForwardReverse(ISurveyFeature other)
        {
            return CostEndToEnd(other);
        }

        public double CostReverseForward(ISurveyFeature to)
        {
            return CostStartToStart(to);
        }

        public double CostReverseReverse(ISurveyFeature to)
        {
            return CostStartToEnd(to);
        }

        public double CostStartToStart(ISurveyFeature to)
        {
            double costToOther = m_CostStartToStartCalculator.Costs [ to.Id ];

            return CalculateTotalCost(m_Feature.Length,
                                      costToOther);
        }

        public double CostStartToEnd(ISurveyFeature to)
        {
            double costToOther = m_CostStartToEndCalculator.Costs [ to.Id ];

            return CalculateTotalCost(m_Feature.Length,
                                      costToOther);
        }

        public double CostEndToStart(ISurveyFeature other)
        {
            double costToOther = m_CostEndToStartCalculator.Costs [ other.Id ];

            return CalculateTotalCost(m_Feature.Length,
                                      costToOther);
        }

        public double CostEndToEnd(ISurveyFeature other)
        {
            double costToOther = m_CostEndToEndCalculator.Costs [ other.Id ];

            return CalculateTotalCost(m_Feature.Length,
                                      costToOther);
        }

        public void Convert()
        {
            foreach ( IBaseCostCalculator baseCostCalculator in m_CostCalculators )
            {
                baseCostCalculator.Feature = m_Feature;
                baseCostCalculator.Features = m_Features;
                baseCostCalculator.Racetracks = m_Racetracks;
                baseCostCalculator.Calculate();
            }
        }

        internal double CalculateTotalCost(double lineLength,
                                           double costToOther)
        {
            if ( !IsCostValid(costToOther) )
            {
                return double.MaxValue;
            }

            double cost = lineLength + costToOther;

            return cost;
        }

        internal bool IsCostValid(double costToOther)
        {
            return costToOther >= 0.1 && !IsCostToMySelf(costToOther);
        }

        internal bool IsCostToMySelf(double costToOther)
        {
            return Math.Abs(costToOther - CostMatrix.CostToMyself) < 0.1;
        }

        #endregion
    }
}