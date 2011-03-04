using System;
using System.Collections.Generic;
using Castle.Windsor;
using CQRSSample.CommandHandlers;
using CQRSSample.Commands;
using CQRSSample.Events;
using EventStore;
using EventStore.Dispatcher;

namespace CQRSSample.Infrastructure
{
    public interface IBus
    {
        void Send<T>(T command) where T : Command;
        void RegisterHandler<T>(Action<T> handler) where T : DomainEvent;
    }

    public class InProcessBus : IBus, IPublishMessages
    {
        private readonly IWindsorContainer _container;
        private readonly Dictionary<Type, List<Action<DomainEvent>>> _routes = new Dictionary<Type, List<Action<DomainEvent>>>();

        public InProcessBus(IWindsorContainer container)
        {
            _container = container;
        }

        public void Send<T>(T command) where T : Command
        {
            var transactionHandler = new TransactionHandler();
            transactionHandler.Execute(command, GetCommandHandlerForCommand<T>());
        }

        public void RegisterHandler<T>(Action<T> handler) where T : DomainEvent
        {
            List<Action<DomainEvent>> handlers;
            if (!_routes.TryGetValue(typeof(T), out handlers))
            {
                handlers = new List<Action<DomainEvent>>();
                _routes.Add(typeof(T), handlers);
            }
            handlers.Add(DelegateAdjuster.CastArgument<DomainEvent, T>(x => handler(x)));
        }

        private Handles<T> GetCommandHandlerForCommand<T>() where T : Command
        {
            return _container.Resolve<Handles<T>>();
        }

        public void Publish(Commit commit)
        {
            foreach (var @event in commit.Events)
            {
                List<Action<DomainEvent>> handlers;

                if (!_routes.TryGetValue(@event.Body.GetType(), out handlers)) return;
                foreach (var handler in handlers)
                {
                    //dispatch on thread pool for added awesomeness
                    //var handler1 = handler;
                    //ThreadPool.QueueUserWorkItem(x => handler1(@event));
                    handler((DomainEvent)@event.Body);
                }
            }
        }

        public void Dispose()
        {
        }
    }
}