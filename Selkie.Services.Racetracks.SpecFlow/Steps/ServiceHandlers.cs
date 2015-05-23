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
    }
}