using System;
using JetBrains.Annotations;
using Selkie.Services.Racetracks.Common.Messages;
using TechTalk.SpecFlow;

// ReSharper disable once CheckNamespace

namespace Selkie.Services.Racetracks.SpecFlow.Steps.Common
{
    public partial class ServiceHandlers
    {
        public void SubscribeOther()
        {
            m_Bus.SubscribeAsync <LinesChangedMessage>(GetType().FullName,
                                                       LinesChangedHandler);

            m_Bus.SubscribeAsync <RacetrackSettingsChangedMessage>(GetType().FullName,
                                                                   RacetrackSettingsChangedHandler);

            m_Bus.SubscribeAsync <CostMatrixChangedMessage>(GetType().FullName,
                                                            CostMatrixChangedHandler);

            m_Bus.SubscribeAsync <RacetracksChangedMessage>(GetType().FullName,
                                                            RacetracksChangedHandler);
        }

        private void LinesChangedHandler([NotNull] LinesChangedMessage message)
        {
            ScenarioContext.Current [ "IsReceivedLinesChangedMessage" ] = true;
        }

        private void CostMatrixChangedHandler([NotNull] CostMatrixChangedMessage message)
        {
            ScenarioContext.Current [ "IsReceivedCostMatrixChangedMessage" ] = true;
            ScenarioContext.Current [ "Matrix" ] = message.Matrix;
        }

        private void RacetrackSettingsChangedHandler([NotNull] RacetrackSettingsChangedMessage message)
        {
            ScenarioContext.Current [ "IsReceivedRacetrackSettingsChangedMessage" ] = true;
        }

        private void RacetracksChangedHandler([NotNull] RacetracksChangedMessage message)
        {
            if ( IsRacetracksValid(message) )
            {
                Console.WriteLine("Received 'empty' RacetracksChangedMessage!");
                return;
            }

            ScenarioContext.Current [ "IsReceivedRacetracksChangedMessage" ] = true;
            ScenarioContext.Current [ "ReceivedRacetracks" ] = message.Racetracks;
        }

        private static bool IsRacetracksValid(RacetracksChangedMessage message)
        {
            // todo currently just a rough test of the racetrack content, maybe better to check more details
            return message.Racetracks.ForwardToForward.Length != 2 ||
                   message.Racetracks.ForwardToForward [ 0 ].Length != 2;
        }
    }
}