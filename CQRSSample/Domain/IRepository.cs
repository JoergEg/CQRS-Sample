using System;

namespace CQRSSample.Domain
{
    public interface IRepository<T> where T : AggregateRoot, new()
    {
        void Save(T aggregate, int expectedVersion);
        T GetById(Guid id);
    }

    public class Repository<T> : IRepository<T> where T : AggregateRoot, new()
    {
        private readonly IEventStore _storage;

        public Repository(IEventStore storage)
        {
            _storage = storage;
        }

        public void Save(T aggregate, int expectedVersion)
        {
            _storage.SaveEvents(aggregate.Id, aggregate.GetChanges(), expectedVersion);
            
            aggregate.MarkChangesAsCommitted();
        }

        public T GetById(Guid id)
        {
            var obj = new T();
            var e = _storage.GetEventsForAggregate(id);
            obj.LoadFromHistory(e);
            return obj;
        }
    }
}