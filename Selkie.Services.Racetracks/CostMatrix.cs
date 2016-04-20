using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.Core;
using JetBrains.Annotations;
using Selkie.Aop.Aspects;
using Selkie.Geometry.Shapes;
using Selkie.Racetrack.Interfaces;
using Selkie.Services.Racetracks.Interfaces;
using Selkie.Services.Racetracks.Interfaces.Converters;
using Selkie.Services.Racetracks.TypedFactories;
using Selkie.Windsor;
using Selkie.Windsor.Extensions;

namespace Selkie.Services.Racetracks
{
    [Interceptor(typeof ( LogAspect ))]
    [ProjectComponent(Lifestyle.Transient)]
    public sealed class CostMatrix : ICostMatrix
    {
        public static readonly ICostMatrix Unkown = new CostMatrix();
        public static readonly double CostToMyself = 0.0;
        private readonly IConverterFactory m_ConverterFactory;
        private readonly ILinesSourceManager m_LinesSourceManager;
        private readonly ISelkieLogger m_Logger;
        private readonly IRacetracksSourceManager m_RacetracksSourceManager;
        private ILine[] m_Lines = new ILine[0];
        private double[][] m_Matrix = new double[0][];
        private IRacetracks m_Racetracks = Converters.Dtos.Racetracks.Unknown;

        private CostMatrix()
        {
        }

        public CostMatrix([NotNull] ISelkieLogger logger,
                          [NotNull] ILinesSourceManager linesSourceManager,
                          [NotNull] IRacetracksSourceManager racetracksSourceManager,
                          [NotNull] IConverterFactory converterFactory)
        {
            m_Logger = logger;
            m_LinesSourceManager = linesSourceManager;
            m_RacetracksSourceManager = racetracksSourceManager;
            m_ConverterFactory = converterFactory;

            Initialize();

            logger.Info("Created new CostMatrix!");
        }

        public IEnumerable <ILine> Lines
        {
            get
            {
                return m_Lines;
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

        public void Initialize()
        {
            // todo move into calculate method
            IEnumerable <ILine> sourceLines = m_LinesSourceManager.Lines.ToArray();

            m_Logger.Debug(LinesToString(sourceLines));

            m_Lines = sourceLines.ToArray();
            m_Matrix = CreateMatrix(m_Lines);

            m_Racetracks = m_RacetracksSourceManager.Racetracks;
        }

        [NotNull]
        internal double[][] CreateMatrix([NotNull] ILine[] lines)
        {
            double[][] matrix = CreateJaggedCostMatrix(lines);

            return matrix;
        }

        [NotNull]
        internal double[][] CreateJaggedCostMatrix([NotNull] ILine[] lines)
        {
            double[][] matrix = lines.Length == 1
                                    ? MatrixForOneLine()
                                    : MatrixForMultipleLines(lines);

            return matrix;
        }

        [NotNull]
        internal double[][] MatrixForOneLine()
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

        [NotNull]
        internal ILineToLinesConverter[] CreateLinesToLines([NotNull] ILine[] lines)
        {
            int size = lines.Length;
            var linesToLines = new ILineToLinesConverter[size];

            for ( var i = 0 ; i < lines.Length ; i++ )
            {
                var lineToLines = m_ConverterFactory.Create <ILineToLinesConverter>();
                lineToLines.Racetracks = m_RacetracksSourceManager.Racetracks;
                lineToLines.Lines = lines;
                lineToLines.Line = lines [ i ];
                lineToLines.Convert();

                linesToLines [ i ] = lineToLines;
            }

            return linesToLines;
        }

        [NotNull]
        internal double[][] MatrixForMultipleLines([NotNull] ILine[] lines)
        {
            int size = lines.Length * 2;
            var matrix = new double[size][];
            ILineToLinesConverter[] converters = CreateLinesToLines(lines);

            var matrixLine = 0;

            for ( var i = 0 ; i < lines.Length ; i++ )
            {
                ILineToLinesConverter fromLineToLines = converters [ i ];

                matrix [ matrixLine++ ] = MatrixLineForForward(fromLineToLines,
                                                               lines);
                matrix [ matrixLine++ ] = MatrixLineForRevers(fromLineToLines,
                                                              lines);
            }

            foreach ( ILineToLinesConverter converter in converters )
            {
                m_ConverterFactory.Release(converter);
            }

            return matrix;
        }

        [NotNull]
        internal double[] MatrixLineForRevers([NotNull] ILineToLinesConverter from,
                                              [NotNull] ICollection <ILine> lines)
        {
            var matrixRow = new double[lines.Count * 2];

            var matrixColumn = 0;

            foreach ( ILine toLine in lines )
            {
                double costStartToStart = from.CostStartToStart(toLine);
                matrixRow [ matrixColumn++ ] = costStartToStart;

                double costStartToEnd = from.CostStartToEnd(toLine);
                matrixRow [ matrixColumn++ ] = costStartToEnd;
            }

            return matrixRow;
        }

        [NotNull]
        internal double[] MatrixLineForForward([NotNull] ILineToLinesConverter from,
                                               [NotNull] ICollection <ILine> lines)
        {
            var matrixRow = new double[lines.Count * 2];

            var matrixColumn = 0;

            foreach ( ILine toLine in lines )
            {
                double costEndToStart = from.CostEndToStart(toLine);
                matrixRow [ matrixColumn++ ] = costEndToStart;

                double costEndToEnd = from.CostEndToEnd(toLine);
                matrixRow [ matrixColumn++ ] = costEndToEnd;
            }

            return matrixRow;
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

        private string LinesToString([NotNull] IEnumerable <ILine> lines)
        {
            // todo testing
            IEnumerable <ILine> enumerable = lines as ILine[] ?? lines.ToArray();

            var builder = new StringBuilder();
            builder.AppendLine("Line count: {0}".Inject(enumerable.Count()));

            foreach ( ILine line in enumerable )
            {
                builder.AppendLine(line.ToString());
            }

            return builder.ToString();
        }
    }
}