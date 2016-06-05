using System;
using System.IO;
using System.Linq;
using System.Threading;
using JetBrains.Annotations;

namespace Selkie.Services.Racetracks.SpecFlow.Steps.Common
{
    public static class Helper
    {
        public const string ServiceName = "Racetracks Service";
        public const string ExeFilenName = "Selkie.Services.Racetracks.Console.exe";
        private static readonly TimeSpan SleepTime = TimeSpan.FromSeconds(1.0);

        [NotNull]
        public static string FullnameForServiceName(
            [NotNull] string directory,
            [NotNull] string serviceName)
        {
            string[] allFiles = Directory.GetFiles(directory,
                                                   serviceName,
                                                   SearchOption.AllDirectories);

            if ( allFiles.Length == 0 )
            {
                throw new ArgumentException("Couldn't find file '" + serviceName + "'!");
            }

            if ( allFiles.Length > 1 )
            {
                throw new ArgumentException("Found multiple locations for file '" + serviceName + "'!");
            }

            return allFiles.First();
        }

        public static string GetDirectoryName()
        {
            string directoryName = AppDomain.CurrentDomain.BaseDirectory;

            if ( directoryName == null )
            {
                throw new NullReferenceException("Can't find entry assembly!");
            }
            return directoryName;
        }

        public static string GetWorkingFolder(string fullName)
        {
            string workingFolder = Path.GetDirectoryName(fullName);

            if ( workingFolder == null )
            {
                throw new NullReferenceException("Can't get path from fullname '" + fullName + "'!");
            }
            return workingFolder;
        }

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