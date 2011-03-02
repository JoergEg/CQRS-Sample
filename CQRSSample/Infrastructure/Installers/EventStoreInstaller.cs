using System;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using EventStore;
using EventStore.Dispatcher;
using EventStore.Persistence;
using EventStore.Persistence.SqlPersistence;
using EventStore.Serialization;
using Newtonsoft.Json;
using Raven.Client;

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
            container.Register(Component.For<IStoreEvents>().Instance(GetInitializedEventStore(container.Resolve<IDocumentStore>())));
        }

        private IStoreEvents GetInitializedEventStore(IDocumentStore store)
        {
            var persistence = BuildPersistenceEngine(store);
            persistence.Initialize();

            var dispatcher = BuildDispatcher(persistence);
            return new OptimisticEventStore(persistence, dispatcher);
        }

        private IPersistStreams BuildPersistenceEngine(IDocumentStore store)
        {
            //return new SqlPersistenceFactory("EventStore", BuildSerializer()).Build();
            return new RavenInMemoryPersistenceFactory(store, BuildSerializer()).Build();
        }

        private ISerialize BuildSerializer()
        {
            var serializer = new JsonSerializer() as ISerialize;
            serializer = new GzipSerializer(serializer);
            return new RijndaelSerializer(serializer, _encryptionKey);
        }

        private IDispatchCommits BuildDispatcher(IPersistStreams persistence)
        {
            return new AsynchronousDispatcher(
                new DelegateMessagePublisher(DispatchCommit),
                persistence,
                OnDispatchError);
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