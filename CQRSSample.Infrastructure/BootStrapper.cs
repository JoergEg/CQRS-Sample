using System;
using System.Linq;
using System.Reflection;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.Windsor.Installer;
using CQRSSample.Domain.Events;
using CQRSSample.ReadModel;
using Raven.Client;

namespace CQRSSample.Infrastructure
{
    public class BootStrapper
    {
        public static readonly string RavenDbConnectionStringName = "RavenDB";

        public static IWindsorContainer BootStrap(IDocumentStore store)
        {
            var container = new WindsorContainer();

            container.Register(Component.For<IDocumentStore>().Instance(store));
            container.Register(Component.For<IWindsorContainer>().Instance(container));

            // adds and configures all components using WindsorInstallers from executing assembly  
            container.Install(FromAssembly.This());

            SetupDomainEventHandlers(container.Resolve<IBus>(), container.Resolve<IDocumentStore>());
            //RegisterEventHandlersInBus.BootStrap(container);

            return container;
        }

        private static void SetupDomainEventHandlers(IBus bus, IDocumentStore documentStore)
        {
            var view = new CustomerListView(documentStore);
            bus.RegisterHandler<CustomerCreatedEvent>(view.Handle);
            bus.RegisterHandler<CustomerRelocatedEvent>(view.Handle);
        }
    }

    public class RegisterEventHandlersInBus
    {
        private static MethodInfo _createPublishActionMethod;
        private static MethodInfo _registerMethod;

        public static void BootStrap(IWindsorContainer container)
        {
            new RegisterEventHandlersInBus().RegisterEventHandlers(container);
        }

        private void RegisterEventHandlers(IWindsorContainer container)
        {
            var bus = container.Resolve<IBus>();

            _createPublishActionMethod = GetType().GetMethod("CreatePublishAction");
            _registerMethod = bus.GetType().GetMethod("RegisterHandler");

            var handlers = typeof(CustomerListView)
                .Assembly
                .GetExportedTypes()
                .Where(x => x.GetInterfaces().Any(y => y.IsGenericType && y.GetGenericTypeDefinition() == typeof(HandlesEvent<>)))
                .ToList();

            foreach (var handlerType in handlers)
            {

                var handleEventTypes = handlerType.GetInterfaces().Where(
                    x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(HandlesEvent<>));

                foreach (var handleEventType in handleEventTypes)
                {
                    var eventHandler = container.Resolve(handleEventType);
                    var action = CreateTheProperAction(handleEventType, eventHandler);
                    RegisterTheCreatedAction(bus, handleEventType, action);
                }
            }
        }

        public Action<TMessage> CreatePublishAction<TMessage, TMessageHandler>(TMessageHandler messageHandler)
            where TMessage : DomainEvent
            where TMessageHandler : HandlesEvent<TMessage>
        {
            return messageHandler.Handle;
        }

        private void RegisterTheCreatedAction(IBus bus, Type handleEventType, object action)
        {
            _registerMethod.MakeGenericMethod(handleEventType).Invoke(bus, new[] { action });
        }

        private object CreateTheProperAction(Type eventType, object eventHandler)
        {
            return _createPublishActionMethod.MakeGenericMethod(eventType, eventHandler.GetType()).Invoke(this, new[] { eventHandler });
        }
    }
}