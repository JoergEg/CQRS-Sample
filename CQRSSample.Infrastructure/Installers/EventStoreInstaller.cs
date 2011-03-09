using System;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using CommonDomain.Core;
using CommonDomain.Persistence;
using CommonDomain.Persistence.EventStore;
using EventStore;
using EventStore.Dispatcher;
using EventStore.Persistence;
using EventStore.Persistence.RavenPersistence;
using EventStore.Serialization;

namespace CQRSSample.Infrastructure.Installers
{
    public class EventStoreInstaller : IWindsorInstaller
    {
        private readonly byte[] _encryptionKey = new byte[]
		{
			0x0, 0x1, 0x2, 0x3, 0x4, 0x5, 0x6, 0x7, 0x8, 0x9, 0xa, 0xb, 0xc, 0xd, 0xe, 0xf
		};

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            //Bus
            var bus = new InProcessBus(container);
            //container.Register(Component.For<IBus>().ImplementedBy<InProcessBus>().LifeStyle.Singleton);
            container.Register(Component.For<IBus>().Instance(bus));
            //container.Register(Component.For<IPublishMessages>().Instance(bus));


            var eventStore = GetInitializedEventStore(bus);
            var repository = new EventStoreRepository(eventStore, new AggregateFactory(), new ConflictDetector());
            
            container.Register(Component.For<IStoreEvents>().Instance(eventStore));
            container.Register(Component.For<IRepository>().Instance(repository));
        }

        private IStoreEvents GetInitializedEventStore(IPublishMessages bus)
        {
            var persistence = BuildPersistenceEngine();
            persistence.Initialize();

            var dispatcher = BuildDispatcher(bus, persistence);
            return new OptimisticEventStore(persistence, dispatcher);
        }

        private IPersistStreams BuildPersistenceEngine()
        {
            //return new SqlPersistenceFactory("EventStore", BuildSerializer()).Build();
            return new RavenPersistenceFactory(BootStrapper.RavenDbConnectionStringName, BuildSerializer()).Build();
            
            //return new InMemoryPersistenceEngine();
        }

        private ISerialize BuildSerializer()
        {
            var serializer = new JsonSerializer() as ISerialize;
            serializer = new GzipSerializer(serializer);
            return new RijndaelSerializer(serializer, _encryptionKey);
        }

        private IDispatchCommits BuildDispatcher(IPublishMessages bus, IPersistStreams persistence)
        {
            //return new AsynchronousDispatcher(
            //    new DelegateMessagePublisher(DispatchCommit),
            //    persistence,
            //    OnDispatchError);

            return new SynchronousDispatcher(bus, persistence);
        }

        private void DispatchCommit(Commit commit)
        {
            // this is where we'd hook into our messaging infrastructure, e.g. NServiceBus.
            Console.WriteLine("Messages from commit have been published.");
        }
        private static void OnDispatchError(Commit commit, Exception exception)
        {
            // if for some reason our messaging infrastructure couldn't dispatch the messages we've committed
            // we would be alerted here.
            Console.WriteLine("Exception while publishing message");
        }
    }
}