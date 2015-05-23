using System;
using System.Diagnostics;
using System.Management.Instrumentation;
using EasyNetQ.Management.Client;
using EasyNetQ.Management.Client.Model;
using JetBrains.Annotations;
using TechTalk.SpecFlow;

namespace Selkie.Services.Racetracks.SpecFlow.Steps.Common
{
    public sealed class SpecFlowService : IDisposable
    {
        private Process m_ExeProcess;

        public void Dispose()
        {
            m_ExeProcess.Exited -= ExeProcessOnExited;
        }

        public void Run()
        {
            m_ExeProcess = StartService();

            if ( m_ExeProcess == null )
            {
                throw new InstanceNotFoundException("Could not start service!");
            }

            ScenarioContext.Current [ "ExeProcess" ] = m_ExeProcess;
            ScenarioContext.Current [ "IsExited" ] = false;

            m_ExeProcess.Exited += ExeProcessOnExited;
        }

        [CanBeNull]
        private Process StartService()
        {
            var startInfo = new ProcessStartInfo
                            {
                                WorkingDirectory = Helper.WorkingFolder,
                                CreateNoWindow = false,
                                UseShellExecute = false,
                                FileName = Helper.FilenName,
                                WindowStyle = ProcessWindowStyle.Normal,
                                Arguments = ""
                            };

            Process lineService = Process.Start(startInfo);

            return lineService;
        }

        private void ExeProcessOnExited([NotNull] object sender,
                                        [NotNull] EventArgs eventArgs)
        {
            ScenarioContext.Current [ "IsExited" ] = true;
        }

        public void KillAndWaitForExit()
        {
            try
            {
                var exeProcess = ( Process ) ScenarioContext.Current [ "ExeProcess" ];

                exeProcess.Kill();
                exeProcess.WaitForExit(2000);
            }
            catch ( InvalidOperationException exception )
            {
                // process already exited, so we can ignore the exception
                Console.WriteLine("Couldn't stop service! - {0}",
                                  exception.Message);
                Console.WriteLine(exception.StackTrace);
            }
        }

        public void DeleteQueues()
        {
            var client = new ManagementClient("http://localhost",
                                              "selkieAdmin",
                                              "selkieAdmin");

            foreach ( Queue queue in client.GetQueues() )
            {
                if ( queue.Vhost == "selkie" )
                {
                    client.DeleteQueue(queue);
                }
            }
        }
    }
}