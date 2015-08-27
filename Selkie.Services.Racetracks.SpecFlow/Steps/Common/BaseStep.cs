using System;
using JetBrains.Annotations;
using Selkie.EasyNetQ;
using TechTalk.SpecFlow;

namespace Selkie.Services.Racetracks.SpecFlow.Steps.Common
{
    [Binding]
    public abstract class BaseStep
    {
        private readonly ISelkieBus m_Bus;

        protected BaseStep()
        {
            m_Bus = ( ISelkieBus ) ScenarioContext.Current [ "ISelkieBus" ];
        }

        protected ISelkieBus Bus
        {
            get
            {
                return m_Bus;
            }
        }

        public void SleepWaitAndDo([NotNull] Func <bool> breakIfTrue,
                                   [NotNull] Action doSomething)
        {
            Helper.SleepWaitAndDo(breakIfTrue,
                                  doSomething);
        }

        public void DoNothing()
        {
        }

        public abstract void Do();
    }
}