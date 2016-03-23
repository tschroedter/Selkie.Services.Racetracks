using System.Diagnostics.CodeAnalysis;
using Castle.Windsor.Installer;
using Selkie.Services.Common;

namespace Selkie.Services.Racetracks.Windows.Service
{
    [ExcludeFromCodeCoverage]
    public class RacetracksServiceMain : ServiceMain
    {
        public void Run()
        {
            StartServiceAndRunForever(FromAssembly.This(),
                                      Racetracks.Service.ServiceName);
        }
    }
}