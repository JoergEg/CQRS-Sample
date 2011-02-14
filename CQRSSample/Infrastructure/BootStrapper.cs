using System;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.Windsor.Installer;
using CQRSSample.Events;
using CQRSSample.ReadModel;
using Raven.Client;
using Raven.Client.Document;

namespace CQRSSample.Infrastructure
{
    public class BootStrapper
    {
        public static IWindsorContainer BootStrap(DocumentStore store)
        {
            var container = new WindsorContainer();

            container.Register(Component.For<IDocumentStore>().Instance(store));
            container.Register(Component.For<IWindsorContainer>().Instance(container));

            // adds and configures all components using WindsorInstallers from executing assembly  
            container.Install(FromAssembly.This());

            SetupDomainEventHandlers(container.Resolve<IBus>(), container.Resolve<IDocumentStore>());

            return container;
        }

        private static void SetupDomainEventHandlers(IBus bus, IDocumentStore documentStore)
        {
            var view = new CustomerListView(documentStore);
            bus.RegisterHandler<CustomerCreatedEvent>(view.Handle);
            bus.RegisterHandler<CustomerRelocatedEvent>(view.Handle);
        }
    }
}