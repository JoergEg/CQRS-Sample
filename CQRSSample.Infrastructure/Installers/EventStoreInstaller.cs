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
            container.Register(Component.For<IBus>().Instance(bus));

            var eventStore = GetInitializedEventStore(bus);
            var repository = new EventStoreRepository(eventStore, new AggregateFactory(), new ConflictDetector());
            
            container.Register(Component.For<IStoreEvents>().Instance(eventStore));
            container.Register(Component.For<IRepository>().Instance(repository));
        }

        private IStoreEvents GetInitializedEventStore(IPublishMessages bus)
        {
            return Wireup.Init()
                //.UsingRavenPersistence(BootStrapper.RavenDbConnectionStringName, new ByteStreamDocumentSerializer(BuildSerializer()))
                .UsingRavenPersistence(BootStrapper.RavenDbConnectionStringName, new NullDocumentSerializer())
                .UsingSynchronousDispatcher(bus)
                .Build();
        }

        private ISerialize BuildSerializer()
        {
            var serializer = new JsonSerializer() as ISerialize;
            serializer = new GzipSerializer(serializer);
            return new RijndaelSerializer(serializer, _encryptionKey);
        }
    }
}