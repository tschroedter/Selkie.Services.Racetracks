using System.Collections.Generic;
using System.Linq;
using Castle.Core;
using JetBrains.Annotations;
using Selkie.Aop.Aspects;
using Selkie.Geometry.Shapes;
using Selkie.Services.Common.Dto;
using Selkie.Services.Racetracks.Interfaces;
using Selkie.Windsor;
using Selkie.Windsor.Extensions;

namespace Selkie.Services.Racetracks
{
    [Interceptor(typeof ( MessageHandlerAspect ))]
    [ProjectComponent(Lifestyle.Singleton)]
    public class LinesSourceManager
        : ILinesSourceManager,
          IStartable
    {
        private readonly ISelkieLogger m_Logger;
        private readonly ILinesValidator m_Validator;
        private IEnumerable <ILine> m_Lines = new Line[0];

        public LinesSourceManager([NotNull] ISelkieLogger logger,
                                  [NotNull] ILinesValidator validator)
        {
            m_Logger = logger;
            m_Validator = validator;
        }

        public IEnumerable <ILine> Lines
        {
            get
            {
                return m_Lines;
            }
        }

        public void SetLinesIfValid(IEnumerable <LineDto> lineDtos)
        {
            lineDtos = GetAndValidateLineDtos(lineDtos);

            UpdateLinesIfValid(lineDtos);
        }

        public void Start()
        {
            m_Logger.Info("Started '{0}'!".Inject(GetType().FullName));
        }

        public void Stop()
        {
            m_Logger.Info("Stopped '{0}'!".Inject(GetType().FullName));
        }

        [NotNull]
        private IEnumerable <LineDto> GetAndValidateLineDtos([CanBeNull] IEnumerable <LineDto> lineDtos)
        {
            if ( lineDtos == null )
            {
                m_Logger.Error("Received LineDtos are null!");
                lineDtos = new LineDto[0];
            }
            return lineDtos;
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