using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using CQRSSample.Domain.CommandHandlers;

namespace CQRSSample.Infrastructure.Installers
{
    public class CommandHandlerInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(AllTypes.FromAssemblyContaining(typeof(CreateCustomerCommandHandler)).Where(x => x.GetInterface(typeof(Handles).Name) != null).WithService.AllInterfaces());
        }
    }
}