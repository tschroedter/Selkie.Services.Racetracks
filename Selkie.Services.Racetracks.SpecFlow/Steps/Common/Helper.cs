using System;
using System.Threading;
using JetBrains.Annotations;

namespace Selkie.Services.Racetracks.SpecFlow.Steps.Common
{
    public static class Helper
    {
        public const string ServiceName = "Racetracks Service";

        public const string WorkingFolder =
            @"C:\Development\Selkie\Services\Racetracks\Selkie.Services.Racetracks.Console\bin\Debug\";

        public const string FilenName = WorkingFolder + "Selkie.Services.Racetracks.Console.exe";
        private static readonly TimeSpan SleepTime = TimeSpan.FromSeconds(1.0);

        public static void SleepWaitAndDo([NotNull] Func <bool> breakIfTrue,
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
    }
}