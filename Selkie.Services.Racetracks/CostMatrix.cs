using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.Core;
using JetBrains.Annotations;
using Selkie.Aop.Aspects;
using Selkie.Geometry.Surveying;
using Selkie.Racetrack.Interfaces;
using Selkie.Services.Racetracks.Interfaces;
using Selkie.Services.Racetracks.Interfaces.Converters;
using Selkie.Services.Racetracks.TypedFactories;
using Selkie.Windsor;
using Selkie.Windsor.Extensions;

namespace Selkie.Services.Racetracks
{
    [Interceptor(typeof( LogAspect ))]
    [ProjectComponent(Lifestyle.Transient)]
    public sealed class CostMatrix : ICostMatrix
    {
        private CostMatrix()
        {
        }

        public CostMatrix([NotNull] ISelkieLogger logger,
                          [NotNull] ISurveyFeaturesSourceManager surveyFeaturesSourceManager,
                          [NotNull] IRacetracksSourceManager racetracksSourceManager,
                          [NotNull] IConverterFactory converterFactory)
        {
            m_Logger = logger;
            m_SurveyFeaturesSourceManager = surveyFeaturesSourceManager;
            m_RacetracksSourceManager = racetracksSourceManager;
            m_ConverterFactory = converterFactory;

            Initialize();

            logger.Info("Created new CostMatrix!");
        }

        public static readonly ICostMatrix Unkown = new CostMatrix();
        public static readonly double CostToMyself = 0.0;
        private readonly IConverterFactory m_ConverterFactory;
        private readonly ISelkieLogger m_Logger;
        private readonly IRacetracksSourceManager m_RacetracksSourceManager;
        private readonly ISurveyFeaturesSourceManager m_SurveyFeaturesSourceManager;
        private ISurveyFeature[] m_Features = new ISurveyFeature[0];
        private double[][] m_Matrix = new double[0][];
        private IRacetracks m_Racetracks = Converters.Dtos.Racetracks.Unknown;

        public IEnumerable <ISurveyFeature> Features
        {
            get
            {
                return m_Features;
            }
        }

        public double[][] Matrix
        {
            get
            {
                return m_Matrix;
            }
        }

        public IRacetracks Racetracks
        {
            get
            {
                return m_Racetracks;
            }
        }

        public Guid ColonyId { get; private set; } // todo were to get it from

        public void Initialize()
        {
            // todo move into calculate method
            IEnumerable <ISurveyFeature> features = m_SurveyFeaturesSourceManager.Features.ToArray();

            m_Logger.Debug(FeaturesToString(features));

            m_Features = features.ToArray();
            m_Matrix = CreateMatrix(m_Features);

            m_Racetracks = m_RacetracksSourceManager.Racetracks;
            ColonyId = m_RacetracksSourceManager.ColonyId; // todo testing
        }

        public override string ToString()
        {
            var builder = new StringBuilder();

            builder.AppendLine("Matrix:");

            for ( var i = 0 ; i < m_Matrix.Length ; i++ )
            {
                builder.Append("[{0}]".Inject(i));

                for ( var j = 0 ; j < m_Matrix.Length ; j++ )
                {
                    builder.Append(" {0:F2}".Inject(m_Matrix [ i ] [ j ]));
                }

                builder.AppendLine();
            }

            return builder.ToString();
        }

        [NotNull]
        internal ISurveyFeatureToSurveyFeaturesConverter[] CreateFeaturesToFeatures([NotNull] ISurveyFeature[] features)
        {
            int size = features.Length;
            var featuresToFeatures = new ISurveyFeatureToSurveyFeaturesConverter[size];

            for ( var i = 0 ; i < features.Length ; i++ )
            {
                var featureToFeatures = m_ConverterFactory.Create <ISurveyFeatureToSurveyFeaturesConverter>();
                featureToFeatures.Racetracks = m_RacetracksSourceManager.Racetracks;
                featureToFeatures.Features = features;
                featureToFeatures.Feature = features [ i ];
                featureToFeatures.Convert();

                featuresToFeatures [ i ] = featureToFeatures;
            }

            return featuresToFeatures;
        }

        [NotNull]
        internal double[][] CreateJaggedCostMatrix([NotNull] ISurveyFeature[] features)
        {
            double[][] matrix = features.Length == 1
                                    ? MatrixForOneFeature()
                                    : MatrixForMultipleFeatures(features);

            return matrix;
        }

        [NotNull]
        internal double[][] CreateMatrix([NotNull] ISurveyFeature[] features)
        {
            double[][] matrix = CreateJaggedCostMatrix(features);

            return matrix;
        }

        [NotNull]
        internal double[] MatrixFeatureForForward([NotNull] ISurveyFeatureToSurveyFeaturesConverter from,
                                                  [NotNull] ICollection <ISurveyFeature> features)
        {
            var matrixRow = new double[features.Count * 2];

            var matrixColumn = 0;

            foreach ( ISurveyFeature toFeature in features )
            {
                double costEndToStart = from.CostEndToStart(toFeature);
                matrixRow [ matrixColumn++ ] = costEndToStart;

                double costEndToEnd = from.CostEndToEnd(toFeature);
                matrixRow [ matrixColumn++ ] = costEndToEnd;
            }

            return matrixRow;
        }

        [NotNull]
        internal double[] MatrixFeatureForRevers([NotNull] ISurveyFeatureToSurveyFeaturesConverter from,
                                                 [NotNull] ICollection <ISurveyFeature> features)
        {
            var matrixRow = new double[features.Count * 2];

            var matrixColumn = 0;

            foreach ( ISurveyFeature toFeature in features )
            {
                double costStartToStart = from.CostStartToStart(toFeature);
                matrixRow [ matrixColumn++ ] = costStartToStart;

                double costStartToEnd = from.CostStartToEnd(toFeature);
                matrixRow [ matrixColumn++ ] = costStartToEnd;
            }

            return matrixRow;
        }

        [NotNull]
        internal double[][] MatrixForMultipleFeatures([NotNull] ISurveyFeature[] features)
        {
            int size = features.Length * 2;
            var matrix = new double[size][];
            ISurveyFeatureToSurveyFeaturesConverter[] converters = CreateFeaturesToFeatures(features);

            var matrixLine = 0;

            for ( var i = 0 ; i < features.Length ; i++ )
            {
                ISurveyFeatureToSurveyFeaturesConverter fromSurveyFeatureToSurveyFeatures = converters [ i ];

                matrix [ matrixLine++ ] = MatrixFeatureForForward(fromSurveyFeatureToSurveyFeatures,
                                                                  features);
                matrix [ matrixLine++ ] = MatrixFeatureForRevers(fromSurveyFeatureToSurveyFeatures,
                                                                 features);
            }

            foreach ( ISurveyFeatureToSurveyFeaturesConverter converter in converters )
            {
                m_ConverterFactory.Release(converter);
            }

            return matrix;
        }

        [NotNull]
        internal double[][] MatrixForOneFeature()
        {
            var matrix = new double[2][];

            matrix [ 0 ] = new[]
                           {
                               CostToMyself,
                               CostToMyself
                           };
            matrix [ 1 ] = new[]
                           {
                               CostToMyself,
                               CostToMyself
                           };

            return matrix;
        }

        private string FeaturesToString([NotNull] IEnumerable <ISurveyFeature> features)
        {
            // todo testing
            IEnumerable <ISurveyFeature> enumerable = features as ISurveyFeature[] ?? features.ToArray();

            var builder = new StringBuilder();
            builder.AppendLine("Feature count: {0}".Inject(enumerable.Count()));

            foreach ( ISurveyFeature feature in enumerable )
            {
                builder.AppendLine(feature.ToString());
            }

            return builder.ToString();
        }
    }
}