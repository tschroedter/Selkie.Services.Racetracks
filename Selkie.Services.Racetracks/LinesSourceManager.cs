using System.Collections.Generic;
using System.Linq;
using Castle.Core;
using JetBrains.Annotations;
using Selkie.EasyNetQ;
using Selkie.Geometry.Shapes;
using Selkie.Services.Racetracks.Common.Dto;
using Selkie.Services.Racetracks.Common.Messages;
using Selkie.Windsor;
using Selkie.Windsor.Extensions;

namespace Selkie.Services.Racetracks
{
    [ProjectComponent(Lifestyle.Singleton)]
    public class LinesSourceManager
        : ILinesSourceManager,
          IStartable
    {
        private readonly ISelkieBus m_Bus;
        private readonly ISelkieLogger m_Logger;
        private readonly ILinesValidator m_Validator;
        private IEnumerable <ILine> m_Lines = new Line[0];

        public LinesSourceManager([NotNull] ISelkieLogger logger,
                                  [NotNull] ISelkieBus bus,
                                  [NotNull] ILinesValidator validator)
        {
            m_Logger = logger;
            m_Bus = bus;
            m_Validator = validator;

            m_Bus.SubscribeAsync <LinesSetMessage>(GetType().FullName,
                                                   LinesSetMessageHandler);
        }

        public IEnumerable <ILine> Lines
        {
            get
            {
                return m_Lines;
            }
        }

        public void Start()
        {
            m_Logger.Info("Started '{0}'!".Inject(GetType().FullName));
        }

        public void Stop()
        {
            m_Logger.Info("Stopped '{0}'!".Inject(GetType().FullName));
        }

        internal void LinesSetMessageHandler([NotNull] LinesSetMessage message)
        {
            IEnumerable <LineDto> lineDtos = message.LineDtos;

            lineDtos = GetAndValidateLineDtos(lineDtos);

            UpdateLinesIfValid(lineDtos);

            SendResponse(m_Lines);
        }

        [NotNull]
        private IEnumerable <LineDto> GetAndValidateLineDtos([CanBeNull] IEnumerable <LineDto> lineDtos)
        {
            if ( lineDtos == null )
            {
                m_Logger.Error("Received LinesSetMessage with LineDtos are null!");
                lineDtos = new LineDto[0];
            }
            return lineDtos;
        }

        private void SendResponse([NotNull] IEnumerable <ILine> lines)
        {
            IEnumerable <LineDto> dtos = lines.Select(LineToLineDtoConverter.ConvertFrom);

            var response = new LinesChangedMessage
                           {
                               LineDtos = dtos.ToArray()
                           };

            m_Bus.PublishAsync(response);
        }

        private void UpdateLinesIfValid([NotNull] IEnumerable <LineDto> lineDtos)
        {
            LineDto[] arrayDtos = lineDtos.ToArray();

            if ( m_Validator.ValidateDtos(arrayDtos) )
            {
                IEnumerable <ILine> lines = arrayDtos.Select(LineToLineDtoConverter.ConvertToLine);

                m_Lines = lines.ToArray();
            }
        }
    }
}