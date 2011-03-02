using System;
using System.Collections.Generic;
using System.Linq;
using CQRSSample.Events;
using EventStore;

namespace CQRSSample.Domain
{
    public interface IRepository<T> where T : AggregateRoot, new()
    {
        void Save(T aggregate, int expectedVersion);
        T GetById(Guid id);
    }

    public class Repository<T> : IRepository<T> where T : AggregateRoot, new()
    {
        private readonly IStoreEvents _eventStore;

        public Repository(IStoreEvents eventStore)
        {
            _eventStore = eventStore;
        }

        public void Save(T aggregate, int expectedVersion)
        {
            AppendToStream(aggregate);
            
            aggregate.MarkChangesAsCommitted();
        }

        private void AppendToStream(T aggregate)
        {
            var latestSnapshot = _eventStore.GetSnapshot(aggregate.Id, int.MaxValue);

            using (var stream = latestSnapshot == null ? _eventStore.OpenStream(aggregate.Id, int.MinValue, int.MaxValue) : _eventStore.OpenStream(latestSnapshot, int.MaxValue))
            {
                var version = stream.StreamRevision;
                foreach (var @event in aggregate.GetChanges())
                {
                    stream.Add(new EventMessage { Body = @event });
                    version++;
                }
                stream.CommitChanges(Guid.NewGuid());
                
                TakeSnapshot(aggregate, version);
            }
        }

        private void TakeSnapshot(T aggregate, int streamRevision)
        {
            _eventStore.AddSnapshot(new Snapshot(aggregate.Id, streamRevision, aggregate));
        }

        public T GetById(Guid id)
        {
            var obj = new T();
            var events = LoadFromSnapshotForward(id);
            obj.LoadFromHistory(events.Select(x => x.Body as DomainEvent));
            
            return obj;
        }

        private IEnumerable<EventMessage> LoadFromSnapshotForward(Guid streamId)
        {
            var latestSnapshot = _eventStore.GetSnapshot(streamId, int.MaxValue);
            
            using (var stream = _eventStore.OpenStream(latestSnapshot, int.MaxValue))
            {
                return stream.CommittedEvents;
            }
        }
    }
}