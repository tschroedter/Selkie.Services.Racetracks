using Castle.Core;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Selkie.Racetrack.Calculators;
using Selkie.Services.Racetracks.Converters;
using Selkie.Windsor;

namespace Selkie.Services.Racetracks
{
    public class Installer : BaseInstaller <Installer>
    {
        // ReSharper disable once CodeAnnotationAnalyzer
        protected override void InstallComponents(IWindsorContainer container,
                                                  IConfigurationStore store)
        {
            // ReSharper disable MaximumChainedReferences
            container.Register(
                               Classes.FromThisAssembly()
                                      .BasedOn <Services.Common.IService>()
                                      .WithServiceFromInterface(typeof ( Services.Common.IService ))
                                      .Configure(c => c.LifeStyle.Is(LifestyleType.Transient)));

            container.Register(
                               Classes.FromThisAssembly()
                                      .BasedOn <IConverter>()
                                      .WithServiceFromInterface(typeof ( IConverter ))
                                      .Configure(c => c.LifeStyle.Is(LifestyleType.Transient)));

            container.Register(
                               Classes.FromThisAssembly()
                                      .BasedOn <ICalculator>()
                                      .WithServiceFromInterface(typeof ( ICalculator ))
                                      .Configure(c => c.LifeStyle.Is(LifestyleType.Transient)));
            // ReSharper restore MaximumChainedReferences
        }
    }
}