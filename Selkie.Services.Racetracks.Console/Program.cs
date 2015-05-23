using System;
using System.Diagnostics.CodeAnalysis;
using Castle.Windsor.Installer;
using Selkie.Services.Common;

namespace Selkie.Services.Racetracks.Console
{
    [ExcludeFromCodeCoverage]
    //ncrunch: no coverage start
    public class Program : ServiceMain
    {
        public static void Main()
        {
            StartServiceAndRunForever(FromAssembly.This(),
                                      Service.ServiceName);

            Environment.Exit(0);
        }
    }
}