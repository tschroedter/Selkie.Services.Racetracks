using System;
using JetBrains.Annotations;
using Selkie.Racetrack.Interfaces;

namespace Selkie.Services.Racetracks.Interfaces
{
    public interface IRacetracksSourceManager
    {
        [NotNull]
        IRacetracks Racetracks { get; }

        Guid ColonyId { get; }

        void CalculateRacetracks();
    }
}