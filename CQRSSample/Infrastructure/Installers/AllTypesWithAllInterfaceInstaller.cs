using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace CQRSSample.Infrastructure.Installers
{
    public class AllTypesWithAllInterfaceInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IBus>().ImplementedBy<InProcessBus>().LifeStyle.Singleton);

            container.Register(AllTypes.FromThisAssembly().Pick().WithService.AllInterfaces());
        }
    }
}