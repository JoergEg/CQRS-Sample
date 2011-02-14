using System;
using System.Collections.Generic;
using System.Linq;
using CQRSSample.Domain;
using CQRSSample.Events;
using Raven.Client;

namespace CQRSSample.Infrastructure
{
    public class RavenDbEventStore : IEventStore
    {
        private readonly IDocumentStore _documentStore;
        private readonly IBus _bus;

        public RavenDbEventStore(IDocumentStore documentStore, IBus bus)
        {
            _documentStore = documentStore;
            _bus = bus;
        }

        public void SaveEvents(Guid aggregateId, IEnumerable<DomainEvent> events, int expectedVersion)
        {
            var i = expectedVersion;

            using(var session = _documentStore.OpenSession())
            {
                foreach (var @event in events)
                {
                    i++;
                    @event.Version = i;

                    var descriptor = new EventDescriptor(aggregateId, @event, i);
                    
                    //TODO: Überprüfung Concurrency Exception

                    session.Store(descriptor);
                    //_documentStore.DatabaseCommands.Put("events/" + aggregateId + "/" + i, null, JObject.FromObject(descriptor), new JObject());  
                    
                    _bus.Publish(@event);
                }
                
                session.SaveChanges();
            }
        }

        public IList<DomainEvent> GetEventsForAggregate(Guid aggregateId)
        {
            IList<EventDescriptor> events;
            using (var session = _documentStore.OpenSession())
            {
                events = session.Query<EventDescriptor>()
                   .Customize(x => x.WaitForNonStaleResults())
                   .Where(x => x.Id == aggregateId)
                   .OrderBy(x => x.Version)
                   .ToList();
            }

            return events.Select(x => x.EventData).ToList();
        }
    }
}