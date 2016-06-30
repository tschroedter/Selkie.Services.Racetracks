using System;
using System.Linq;
using JetBrains.Annotations;
using Selkie.EasyNetQ;
using TechTalk.SpecFlow;

namespace Selkie.Services.Racetracks.SpecFlow.Steps.Common
{
    [Binding]
    public abstract class BaseStep
    {
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

        private readonly ISelkieBus m_Bus;

        public static bool GetBoolValueForScenarioContext([NotNull] string key)
        {
            if ( !ScenarioContext.Current.Keys.Contains(key) )
            {
                return false;
            }

            var result = ( bool ) ScenarioContext.Current [ key ];

            return result;
        }

        public abstract void Do();

        public void DoNothing()
        {
        }

        public void SleepWaitAndDo([NotNull] Func <bool> breakIfTrue,
                                   [NotNull] Action doSomething)
        {
            Helper.SleepWaitAndDo(breakIfTrue,
                                  doSomething);
        }
    }
}