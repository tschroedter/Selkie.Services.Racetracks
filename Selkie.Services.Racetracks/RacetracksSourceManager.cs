using System;
using JetBrains.Annotations;
using Selkie.Racetrack;
using Selkie.Racetrack.Calculators;
using Selkie.Windsor;

namespace Selkie.Services.Racetracks
{
    // todo discovered problem when turn circle is 40 and distance between lines is 30 will not find a path
    //      -=> at the moment the algorithm can only handle circles, circle-line-circle, but not circle-line-circle ->line-> circle-line-circle
    [ProjectComponent(Lifestyle.Singleton)]
    public sealed class RacetracksSourceManager
        : IRacetracksSourceManager,
          IDisposable
    {
        private readonly ICalculatorFactory m_Factory;
        private readonly ILinesSourceManager m_LinesSourceManager;
        private readonly IRacetrackSettingsSourceManager m_RacetrackSettingsSourceManager;
        private IRacetracksCalculator m_RacetracksCalculator;

        public RacetracksSourceManager([NotNull] ILinesSourceManager linesSourceManager,
                                       [NotNull] IRacetrackSettingsSourceManager racetrackSettingsSourceManager,
                                       [NotNull] ICalculatorFactory factory)
        {
            m_LinesSourceManager = linesSourceManager;
            m_RacetrackSettingsSourceManager = racetrackSettingsSourceManager;
            m_Factory = factory;

            Update();
        }

        public void Dispose()
        {
            m_Factory.Release(m_RacetracksCalculator);
        }

        #region IRacetracksSourceManager Members

        public IRacetracks Racetracks
        {
            get
            {
                return m_RacetracksCalculator;
            }
        }

        #endregion

        // todo maybe do on different thread/async here or in CostMatrix
        internal void Update()
        {
            m_Factory.Release(m_RacetracksCalculator);

            IRacetrackSettingsSource settingsSource = m_RacetrackSettingsSourceManager.Source;

            m_RacetracksCalculator = m_Factory.Create <IRacetracksCalculator>();
            m_RacetracksCalculator.Lines = m_LinesSourceManager.Lines;
            m_RacetracksCalculator.Radius = settingsSource.TurnRadius;
            m_RacetracksCalculator.IsPortTurnAllowed = settingsSource.IsPortTurnAllowed;
            m_RacetracksCalculator.IsStarboardTurnAllowed = settingsSource.IsStarboardTurnAllowed;
            m_RacetracksCalculator.Calculate();
        }
    }
}