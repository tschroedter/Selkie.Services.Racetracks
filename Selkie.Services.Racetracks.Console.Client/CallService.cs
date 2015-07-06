using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading;
using Castle.Core.Logging;
using EasyNetQ;
using JetBrains.Annotations;
using Selkie.EasyNetQ.Extensions;
using Selkie.Services.Common;
using Selkie.Services.Racetracks.Common.Dto;
using Selkie.Services.Racetracks.Common.Messages;
using Selkie.Windsor.Extensions;

namespace Selkie.Services.Racetracks.Console.Client
{
    [ExcludeFromCodeCoverage]
    //ncrunch: no coverage start
    public class CallService
    {
        private static readonly TimeSpan SleepTime = TimeSpan.FromSeconds(1.0);
        private readonly IBus m_Bus;
        private readonly ISelkieConsole m_SelkieConsole;
        private bool m_IsReceivedCostMatrixChangedMessage;
        private bool m_IsReceivedLinesChangedMessage;
        private bool m_IsReceivedRacetracksChangedMessage;
        private bool m_IsReceivedRacetrackSettingsChangedMessage;
        private double[][] m_Matrix;
        private RacetracksDto m_Racetracks;

        public CallService([NotNull] IBus bus,
                           [NotNull] ILogger logger,
                           [NotNull] ISelkieConsole selkieConsole)
        {
            m_Bus = bus;
            m_SelkieConsole = selkieConsole;
            ILogger logger1 = logger;

            m_Bus.SubscribeHandlerAsync <LinesChangedMessage>(logger1,
                                                              GetType().FullName,
                                                              LinesChangedHandler);

            m_Bus.SubscribeHandlerAsync <RacetrackSettingsChangedMessage>(logger1,
                                                                          GetType().FullName,
                                                                          RacetrackSettingsChangedHandler);

            m_Bus.SubscribeHandlerAsync <CostMatrixChangedMessage>(logger1,
                                                                   GetType().FullName,
                                                                   CostMatrixChangedHandler);

            m_Bus.SubscribeHandlerAsync <RacetracksChangedMessage>(logger1,
                                                                   GetType().FullName,
                                                                   RacetracksChangedHandler);
        }

        private void RacetracksChangedHandler(RacetracksChangedMessage message)
        {
            m_IsReceivedRacetracksChangedMessage = true;
            m_Racetracks = message.Racetracks;
        }

        private void LinesChangedHandler([NotNull] LinesChangedMessage message)
        {
            m_IsReceivedLinesChangedMessage = true;
        }

        private void CostMatrixChangedHandler([NotNull] CostMatrixChangedMessage message)
        {
            m_IsReceivedCostMatrixChangedMessage = true;
            m_Matrix = message.Matrix;
        }

        private void RacetrackSettingsChangedHandler([NotNull] RacetrackSettingsChangedMessage message)
        {
            m_IsReceivedRacetrackSettingsChangedMessage = true;
        }

        public void Do()
        {
            SendLinesSetMessage();
            WaitForLinesChangedMessage();
            SendRacetrackSettingsSetMessage();
            WaitForRacetrackSettingsChangedMessage();
            WaitForRacetracksChangedMessage();
            SendCostMatrixCalculateMessage();
            WaitForCostMatrixChangedMessage();
            SendRacetracksGetMessage();
            WaitForRacetracksChangedMessage();
        }

        public void SendRacetrackSettingsSetMessage()
        {
            m_SelkieConsole.WriteLine("Sending RacetrackSettingsSetMessage!");

            var message = new RacetrackSettingsSetMessage
                          {
                              TurnRadiusInMetres = 100.0,
                              IsPortTurnAllowed = true,
                              IsStarboardTurnAllowed = true
                          };

            m_Bus.PublishAsync(message);
        }

        public void WaitForRacetrackSettingsChangedMessage()
        {
            m_IsReceivedRacetrackSettingsChangedMessage = false;
            SleepWaitAndDo(() => m_IsReceivedRacetrackSettingsChangedMessage,
                           DoNothing);

            if ( !m_IsReceivedRacetrackSettingsChangedMessage )
            {
                throw new Exception("Did not receive RacetrackSettingsChangedMessage!");
            }

            m_SelkieConsole.WriteLine("Received RacetrackSettingsChangedMessage!");
        }

        public void SendLinesSetMessage()
        {
            m_SelkieConsole.WriteLine("Sending LinesSetMessage!");

            IEnumerable <LineDto> dtos = CreateLineDtos();
            var linesSetMessage = new LinesSetMessage
                                  {
                                      LineDtos = dtos.ToArray()
                                  };

            m_Bus.PublishAsync(linesSetMessage);
        }

        public void WaitForLinesChangedMessage()
        {
            SleepWaitAndDo(() => m_IsReceivedLinesChangedMessage,
                           DoNothing);

            if ( !m_IsReceivedLinesChangedMessage )
            {
                throw new Exception("Did not receive LinesChangedMessage!");
            }

            m_SelkieConsole.WriteLine("Received LinesChangedMessage!");
        }

        public void SendCostMatrixCalculateMessage()
        {
            m_SelkieConsole.WriteLine("Sending CostMatrixCalculateMessage!");

            m_Bus.PublishAsync(new CostMatrixCalculateMessage());
        }

        public void WaitForCostMatrixChangedMessage()
        {
            SleepWaitAndDo(() => m_IsReceivedCostMatrixChangedMessage,
                           DoNothing);

            if ( !m_IsReceivedCostMatrixChangedMessage )
            {
                throw new Exception("Did not receive CostMatrixChangedMessage!");
            }

            m_SelkieConsole.WriteLine(
                                      "Received CostMatrixChangedMessage! - m_Matrix.GetLength(0): {0}".Inject(
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

        public void WaitForRacetracksChangedMessage()
        {
            SleepWaitAndDo(() => m_IsReceivedRacetracksChangedMessage,
                           DoNothing);

            if ( !m_IsReceivedCostMatrixChangedMessage )
            {
                throw new Exception("Did not receive RacetracksChangedMessage!");
            }

            if ( m_Racetracks == null )
            {
                throw new Exception("Did not receive Racetracks!");
            }

            m_SelkieConsole.WriteLine(
                                      "Received RacetracksChangedMessage! - ForwardToForward: {0} ForwardToReverse: {1} ReverseToForward: {2} ReverseToReverse: {3}"
                                          .Inject(m_Racetracks.ForwardToForward.Length,
                                                  m_Racetracks.ForwardToReverse.Length,
                                                  m_Racetracks.ReverseToForward.Length,
                                                  m_Racetracks.ReverseToReverse.Length));
            m_SelkieConsole.WriteLine(CostMatrixToString(m_Matrix));
        }

        [NotNull]
        private IEnumerable <LineDto> CreateLineDtos()
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