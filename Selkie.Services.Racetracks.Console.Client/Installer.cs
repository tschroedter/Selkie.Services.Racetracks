﻿using System.Diagnostics.CodeAnalysis;
using Castle.MicroKernel.Registration;
using Selkie.Windsor;

namespace Selkie.Services.Racetracks.Console.Client
{
    [ExcludeFromCodeCoverage]
    //ncrunch: no coverage start
    public class Installer
        : BasicConsoleInstaller,
          IWindsorInstaller
    {
    }
}