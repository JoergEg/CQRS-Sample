using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace CQRSSample.Infrastructure.Installers
{
    public class AllTypesWithFirstInterfaceInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IBus>().ImplementedBy<InProcessBus>().LifeStyle.Singleton);

            container.Register(AllTypes.FromThisAssembly().Pick().WithService.FirstInterface());

            //container.Register(AllTypes.FromThisAssembly()  
            //    .BasedOn<IController>()  
            //    .If(Component.IsInSameNamespaceAs<HomeController>())  
            //    .If(t => t.Name.EndsWith("Controller"))  
            //    .Configure((ConfigureDelegate) (c => c.Named(c.ServiceType.Name)  
            //    .LifeStyle.Transient)));  
        }
    }
}