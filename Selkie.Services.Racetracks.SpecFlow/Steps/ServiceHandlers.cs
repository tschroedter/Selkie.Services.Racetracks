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
            m_Bus.SubscribeAsync <CostMatrixResponseMessage>(GetType().FullName,
                                                             CostMatrixResponseHandler);

            m_Bus.SubscribeAsync <RacetracksResponseMessage>(GetType().FullName,
                                                             RacetracksResponseHandler);
        }

        private void CostMatrixResponseHandler([NotNull] CostMatrixResponseMessage message)
        {
            ScenarioContext.Current [ "IsReceivedCostMatrixResponseMessage" ] = true;
            ScenarioContext.Current [ "Matrix" ] = message.Matrix;
        }

        private void RacetracksResponseHandler([NotNull] RacetracksResponseMessage message)
        {
            if ( IsRacetracksValid(message) )
            {
                Console.WriteLine("Received 'empty' RacetracksResponseMessage!");
                return;
            }

            ScenarioContext.Current [ "IsReceivedRacetracksResponseMessage" ] = true;
            ScenarioContext.Current [ "ReceivedRacetracks" ] = message.Racetracks;
        }

        private static bool IsRacetracksValid(RacetracksResponseMessage message)
        {
            // todo currently just a rough test of the racetrack content, maybe better to check more details
            return message.Racetracks.ForwardToForward.Length != 2 ||
                   message.Racetracks.ForwardToForward [ 0 ].Length != 2;
        }
    }
}