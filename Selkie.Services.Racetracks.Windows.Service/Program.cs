using System;
using System.Diagnostics.CodeAnalysis;
using System.ServiceProcess;
using System.Threading;

namespace Selkie.Services.Racetracks.Windows.Service
{
    [ExcludeFromCodeCoverage]
    internal static class Program
    {
        private static void Main()
        {
            if ( Environment.UserInteractive )
            {
                RunAsApplication();
            }
            else
            {
                RunAsService();
            }
        }

        private static void RunAsApplication()
        {
            Console.WriteLine("Starting service as application...");

            var service = new Service();
            service.Start();

            var waitForever = new ManualResetEvent(false);
            waitForever.WaitOne();
        }

        private static void RunAsService()
        {
            var servicesToRun = new ServiceBase[]
                                {
                                    new Service()
                                };

            ServiceBase.Run(servicesToRun);
        }
    }
}