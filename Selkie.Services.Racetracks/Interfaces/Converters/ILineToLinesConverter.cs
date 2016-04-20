using System.Collections.Generic;
using JetBrains.Annotations;
using Selkie.Geometry.Shapes;
using Selkie.Racetrack.Interfaces;

namespace Selkie.Services.Racetracks.Interfaces.Converters
{
    public interface ILineToLinesConverter : IConverter
    {
        [NotNull]
        ILine Line { get; set; }

        [NotNull]
        IEnumerable <ILine> Lines { get; set; }

        double BaseCost { get; }

        [NotNull]
        IRacetracks Racetracks { get; set; }

        double CostForwardForward([NotNull] ILine other);
        double CostForwardReverse([NotNull] ILine other);
        double CostReverseForward([NotNull] ILine to);
        double CostReverseReverse([NotNull] ILine to);
        double CostStartToStart([NotNull] ILine other);
        double CostEndToStart([NotNull] ILine other);
        double CostEndToEnd([NotNull] ILine other);
        double CostStartToEnd([NotNull] ILine to);
    }
}