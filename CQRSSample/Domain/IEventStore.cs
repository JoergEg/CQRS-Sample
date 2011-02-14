using System;
using System.Collections.Generic;
using CQRSSample.Events;

namespace CQRSSample.Domain
{
    public interface IEventStore
    {
        void SaveEvents(Guid aggregateId, IEnumerable<DomainEvent> events, int expectedVersion);
        IList<DomainEvent> GetEventsForAggregate(Guid aggregateId);
    }
}