using EventStore.Persistence.RavenPersistence;
using EventStore.Serialization;
using Raven.Client;

namespace CQRSSample.Infrastructure
{
    public class RavenInMemoryPersistenceFactory : RavenPersistenceFactory
    {
        private readonly IDocumentStore _store;

        protected RavenInMemoryPersistenceFactory(string connectionName, ISerialize serializer) : base(connectionName, serializer)
        {
        }

        protected RavenInMemoryPersistenceFactory(string connectionName, ISerialize serializer, bool consistentQueries) : base(connectionName, serializer, consistentQueries)
        {
        }

        public RavenInMemoryPersistenceFactory(IDocumentStore store, ISerialize serializer) : this("", serializer)
        {
            _store = store;
        }

        protected override IDocumentStore GetStore()
        {
            _store.Initialize();

            return _store;
        }
    }
}