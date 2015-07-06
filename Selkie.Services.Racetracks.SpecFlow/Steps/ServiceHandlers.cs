using System;
using JetBrains.Annotations;
using Selkie.EasyNetQ.Extensions;
using Selkie.Services.Racetracks.Common.Messages;
using TechTalk.SpecFlow;

// ReSharper disable once CheckNamespace

namespace Selkie.Services.Racetracks.SpecFlow.Steps.Common
{
    public partial class ServiceHandlers
    {
        public void SubscribeOther()
        {
            m_Bus.SubscribeHandlerAsync <LinesChangedMessage>(m_Logger,
                                                              GetType().FullName,
                                                              LinesChangedHandler);

            m_Bus.SubscribeHandlerAsync <RacetrackSettingsChangedMessage>(m_Logger,
                                                                          GetType().FullName,
                                                                          RacetrackSettingsChangedHandler);

            m_Bus.SubscribeHandlerAsync <CostMatrixChangedMessage>(m_Logger,
                                                                   GetType().FullName,
                                                                   CostMatrixChangedHandler);

            m_Bus.SubscribeHandlerAsync <RacetracksChangedMessage>(m_Logger,
                                                                   GetType().FullName,
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
            if (IsRacetracksValid(message))
            {
                Console.WriteLine("Received 'empty' RacetracksChangedMessage!");
                return;
            }

            ScenarioContext.Current["IsReceivedRacetracksChangedMessage"] = true;
            ScenarioContext.Current["ReceivedRacetracks"] = message.Racetracks;
        }

        private static bool IsRacetracksValid(RacetracksChangedMessage message)
        {
            // todo currently just a rough test of the racetrack content, maybe better to check more details
            return message.Racetracks.ForwardToForward.Length != 2 ||
                   message.Racetracks.ForwardToForward[0].Length != 2;
        }
    }
}