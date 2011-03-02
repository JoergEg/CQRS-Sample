using System.Collections.Generic;
using System.Linq;
using CQRSSample.Events;
using EventStore;
using EventStore.Dispatcher;

namespace CQRSSample.Infrastructure
{
    public class FakeBus : IPublishMessages
    {
        readonly ICollection<DomainEvent> domainEvents;

        public FakeBus(ICollection<DomainEvent> domainEvents)
        {
            this.domainEvents = domainEvents;
        }

        public void Dispose()
        {
        }

        public void Publish(Commit commit)
        {
            commit.Events.ToList().ForEach(e => this.domainEvents.Add(e.Body as DomainEvent));
        }
    }
}