using System;
using System.Collections.Generic;
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
    public class CallService
    {
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

        private static readonly TimeSpan SleepTime = TimeSpan.FromSeconds(1.0);
        private readonly ISelkieBus m_Bus;
        private readonly ISelkieConsole m_SelkieConsole;
        private bool m_IsReceivedCostMatrixResponseMessage;
        private bool m_IsReceivedRacetracksResponseMessage;
        private double[][] m_Matrix;
        private RacetracksDto m_Racetracks;

        public void Do()
        {
            SendCostMatrixCalculateMessage();
            WaitForCostMatrixResponseMessage();
            SendRacetracksGetMessage();
            WaitForRacetracksResponseMessage();
        }

        private static string CostMatrixToString([NotNull] IReadOnlyList <double[]> costMatrix)
        {
            var builder = new StringBuilder();

            builder.AppendLine("CostMatrix:");

            for ( var i = 0 ; i < costMatrix.Count ; i++ )
            {
                builder.Append("[{0}]".Inject(i));

                for ( var j = 0 ; j < costMatrix.Count ; j++ )
                {
                    builder.Append(" {0:F2}".Inject(costMatrix [ i ] [ j ]));
                }

                builder.AppendLine();
            }

            return builder.ToString();
        }

        private void CostMatrixResponseHandler([NotNull] CostMatrixResponseMessage message)
        {
            m_IsReceivedCostMatrixResponseMessage = true;
            m_Matrix = message.Matrix;
        }

        [NotNull]
        private SurveyFeatureDto[] CreateSurveyFeatureDtos()
        {
            var dto = new SurveyFeatureDto
                      {
                          Id = 0,
                          RunDirection = "Forward",
                          IsUnknown = false,
                          StartPoint = new PointDto
                                       {
                                           X = 0.0,
                                           Y = 0.0
                                       },
                          EndPoint = new PointDto
                                     {
                                         X = 0.0,
                                         Y = 100.0
                                     },
                          AngleToXAxisAtStartPoint = 90.0,
                          AngleToXAxisAtEndPoint = 90.0,
                          Length = 100.0
                      };

            var two = new SurveyFeatureDto
                      {
                          Id = 1,
                          RunDirection = "Forward",
                          IsUnknown = false,
                          StartPoint = new PointDto
                                       {
                                           X = 100.0,
                                           Y = 0.0
                                       },
                          EndPoint = new PointDto
                                     {
                                         X = 100.0,
                                         Y = 100.0
                                     },
                          AngleToXAxisAtStartPoint = 270.0,
                          AngleToXAxisAtEndPoint = 270.0,
                          Length = 100.0
                      };

            SurveyFeatureDto[] dtos =
            {
                dto,
                two
            };

            return dtos;
        }

        private void DoNothing()
        {
        }

        private void RacetracksResponseHandler(RacetracksResponseMessage message)
        {
            m_IsReceivedRacetracksResponseMessage = true;
            m_Racetracks = message.Racetracks;
        }

        private void SendCostMatrixCalculateMessage()
        {
            m_SelkieConsole.WriteLine("Sending CostMatrixCalculateMessage!");

            var message = new CostMatrixCalculateMessage
                          {
                              SurveyFeatureDtos = CreateSurveyFeatureDtos(),
                              IsPortTurnAllowed = true,
                              IsStarboardTurnAllowed = true,
                              TurnRadiusForPort = 100.0,
                              TurnRadiusForStarboard = 100.0
                          };

            m_Bus.PublishAsync(message);
        }

        private void SendRacetracksGetMessage()
        {
            m_SelkieConsole.WriteLine("Sending RacetracksGetMessage!");

            m_Bus.PublishAsync(new RacetracksGetMessage());
        }

        private void SleepWaitAndDo([NotNull] Func <bool> breakIfTrue,
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

        private void WaitForCostMatrixResponseMessage()
        {
            SleepWaitAndDo(() => m_IsReceivedCostMatrixResponseMessage,
                           DoNothing);

            if ( !m_IsReceivedCostMatrixResponseMessage )
            {
                throw new Exception("Did not receive CostMatrixResponseMessage!");
            }

            m_SelkieConsole.WriteLine("Received CostMatrixResponseMessage! - m_Matrix.GetLength(0): {0}".Inject(
                                                                                                                m_Matrix
                                                                                                                    .GetLength
                                                                                                                    (0)));
            m_SelkieConsole.WriteLine(CostMatrixToString(m_Matrix));
        }

        private void WaitForRacetracksResponseMessage()
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
    }
}