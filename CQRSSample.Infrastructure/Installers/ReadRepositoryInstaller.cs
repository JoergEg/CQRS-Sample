using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using CQRSSample.ReadModel;

namespace CQRSSample.Infrastructure.Installers
{
    public class ReadRepositoryInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(AllTypes.FromAssemblyContaining(typeof(IReadRepository)).Where(x => x.GetInterface(typeof(IReadRepository).Name) != null).WithService.AllInterfaces());
        }
    }
}