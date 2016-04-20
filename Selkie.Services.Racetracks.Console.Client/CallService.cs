using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Threading;
using JetBrains.Annotations;
using Selkie.EasyNetQ;
using Selkie.Services.Common;
using Selkie.Services.Common.Dto;
using Selkie.Services.Racetracks.Common.Messages;
using Selkie.Windsor.Extensions;

namespace Selkie.Services.Racetracks.Console.Client
{
    [ExcludeFromCodeCoverage]
    //ncrunch: no coverage start
    public class CallService
    {
        private static readonly TimeSpan SleepTime = TimeSpan.FromSeconds(1.0);
        private readonly ISelkieBus m_Bus;
        private readonly ISelkieConsole m_SelkieConsole;
        private bool m_IsReceivedCostMatrixResponseMessage;
        private bool m_IsReceivedRacetrackSettingsResponseMessage;
        private bool m_IsReceivedRacetracksResponseMessage;
        private double[][] m_Matrix;
        private RacetracksDto m_Racetracks;

        public CallService([NotNull] ISelkieBus bus,
                           [NotNull] ISelkieConsole selkieConsole)
        {
            m_Bus = bus;
            m_SelkieConsole = selkieConsole;

            m_Bus.SubscribeAsync <CostMatrixResponseMessage>(GetType().FullName,
                                                             CostMatrixResponseHandler);

            m_Bus.SubscribeAsync <RacetracksResponseMessage>(GetType().FullName,
                                                             RacetracksResponseHandler);
        }

        private void RacetracksResponseHandler(RacetracksResponseMessage message)
        {
            m_IsReceivedRacetracksResponseMessage = true;
            m_Racetracks = message.Racetracks;
        }

        private void CostMatrixResponseHandler([NotNull] CostMatrixResponseMessage message)
        {
            m_IsReceivedCostMatrixResponseMessage = true;
            m_Matrix = message.Matrix;
        }

        public void Do()
        {
            SendCostMatrixCalculateMessage();
            WaitForCostMatrixResponseMessage();
            SendRacetracksGetMessage();
            WaitForRacetracksResponseMessage();
        }

        public void WaitForRacetrackSettingsResponseMessage()
        {
            m_IsReceivedRacetrackSettingsResponseMessage = false;
            SleepWaitAndDo(() => m_IsReceivedRacetrackSettingsResponseMessage,
                           DoNothing);

            if ( !m_IsReceivedRacetrackSettingsResponseMessage )
            {
                throw new Exception("Did not receive RacetrackSettingsResponseMessage!");
            }

            m_SelkieConsole.WriteLine("Received RacetrackSettingsResponseMessage!");
        }

        public void SendCostMatrixCalculateMessage()
        {
            m_SelkieConsole.WriteLine("Sending CostMatrixCalculateMessage!");

            var message = new CostMatrixCalculateMessage
                          {
                              LineDtos = CreateLineDtos(),
                              IsPortTurnAllowed = true,
                              IsStarboardTurnAllowed = true,
                              TurnRadiusForPort = 100.0,
                              TurnRadiusForStarboard = 100.0
                          };

            m_Bus.PublishAsync(message);
        }

        public void WaitForCostMatrixResponseMessage()
        {
            SleepWaitAndDo(() => m_IsReceivedCostMatrixResponseMessage,
                           DoNothing);

            if ( !m_IsReceivedCostMatrixResponseMessage )
            {
                throw new Exception("Did not receive CostMatrixResponseMessage!");
            }

            m_SelkieConsole.WriteLine(
                                      "Received CostMatrixResponseMessage! - m_Matrix.GetLength(0): {0}".Inject(
                                                                                                                m_Matrix
                                                                                                                    .GetLength
                                                                                                                    (0)));
            m_SelkieConsole.WriteLine(CostMatrixToString(m_Matrix));
        }

        public void SendRacetracksGetMessage()
        {
            m_SelkieConsole.WriteLine("Sending RacetracksGetMessage!");

            m_Bus.PublishAsync(new RacetracksGetMessage());
        }

        public void WaitForRacetracksResponseMessage()
        {
            SleepWaitAndDo(() => m_IsReceivedRacetracksResponseMessage,
                           DoNothing);

            if ( !m_IsReceivedRacetracksResponseMessage )
            {
                throw new Exception("Did not receive RacetracksResponseMessage!");
            }

            if ( m_Racetracks == null )
            {
                throw new Exception("Did not receive Racetracks!");
            }

            m_SelkieConsole.WriteLine("Received RacetracksResponseMessage! - ForwardToForward: {0} " +
                                      "ForwardToReverse: {1} ReverseToForward: {2} ReverseToReverse: {3}"
                                          .Inject(m_Racetracks.ForwardToForward.Length,
                                                  m_Racetracks.ForwardToReverse.Length,
                                                  m_Racetracks.ReverseToForward.Length,
                                                  m_Racetracks.ReverseToReverse.Length));
            m_SelkieConsole.WriteLine(CostMatrixToString(m_Matrix));
        }

        [NotNull]
        private LineDto[] CreateLineDtos()
        {
            var lineOne = new LineDto
                          {
                              Id = 0,
                              RunDirection = "Forward",
                              IsUnknown = false,
                              X1 = 0.0,
                              Y1 = 0.0,
                              X2 = 0.0,
                              Y2 = 100.0
                          };

            var lineTwo = new LineDto
                          {
                              Id = 1,
                              RunDirection = "Forward",
                              IsUnknown = false,
                              X1 = 100.0,
                              Y1 = 0.0,
                              X2 = 100.0,
                              Y2 = 100.0
                          };

            LineDto[] dtos =
            {
                lineOne,
                lineTwo
            };

            return dtos;
        }

        private void DoNothing()
        {
        }

        public void SleepWaitAndDo([NotNull] Func <bool> breakIfTrue,
                                   [NotNull] Action doSomething)
        {
            for ( var i = 0 ; i < 10 ; i++ )
            {
                Thread.Sleep(SleepTime);

                if ( breakIfTrue() )
                {
                    break;
                }

                doSomething();
            }
        }

        public string CostMatrixToString([NotNull] double[][] costMatrix)
        {
            var builder = new StringBuilder();

            builder.AppendLine("CostMatrix:");

            for ( var i = 0 ; i < costMatrix.Length ; i++ )
            {
                builder.Append("[{0}]".Inject(i));

                for ( var j = 0 ; j < costMatrix.Length ; j++ )
                {
                    builder.Append(" {0:F2}".Inject(costMatrix [ i ] [ j ]));
                }

                builder.AppendLine();
            }

            return builder.ToString();
        }
    }
}