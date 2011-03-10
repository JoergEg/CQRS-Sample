using Caliburn.Micro;
using Castle.Core;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace CQRSSample.WpfClient.Infrastructure.Installers
{
    public class ViewModelInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<IWindowManager>().ImplementedBy<WindowManager>().LifeStyle.Is(LifestyleType.Singleton));
            container.Register(
                Component.For<IEventAggregator>().ImplementedBy<EventAggregator>().LifeStyle.Is(LifestyleType.Singleton));

            container.Register(AllTypes.FromThisAssembly().Pick());
        }
    }
}