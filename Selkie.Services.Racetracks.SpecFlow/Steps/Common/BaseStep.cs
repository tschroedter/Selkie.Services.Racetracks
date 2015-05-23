using System;
using System.Threading;
using EasyNetQ;
using JetBrains.Annotations;
using TechTalk.SpecFlow;

namespace Selkie.Services.Racetracks.SpecFlow.Steps.Common
{
    [Binding]
    public abstract class BaseStep
    {
        private static readonly TimeSpan SleepTime = TimeSpan.FromSeconds(1.0);
        private readonly IBus m_Bus;

        protected BaseStep()
        {
            m_Bus = ( IBus ) ScenarioContext.Current [ "IBus" ];
        }

        protected IBus Bus
        {
            get
            {
                return m_Bus;
            }
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

        public void DoNothing()
        {
        }

        public abstract void Do();
    }
}