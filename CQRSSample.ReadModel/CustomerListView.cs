using System;
using CQRSSample.Domain.Events;
using Raven.Client;

namespace CQRSSample.ReadModel
{
    public class CustomerListView : HandlesEvent<CustomerCreatedEvent>, HandlesEvent<CustomerRelocatedEvent>
    {
        private readonly IDocumentStore _documentStore;

        public CustomerListView(IDocumentStore documentStore)
        {
            _documentStore = documentStore;
        }

        public void Handle(CustomerRelocatedEvent @event)
        {
            using (var session = _documentStore.OpenSession())
            {
                var dto = session.Load<CustomerListDto>(@event.AggregateId.ToString());
                dto.City = @event.City;
                session.SaveChanges();
            }
        }

        public void Handle(CustomerCreatedEvent @event)
        {
            using(var session = _documentStore.OpenSession())
            {
                var dto = new CustomerListDto { Id = @event.AggregateId, City = @event.City, Name = @event.CustomerName };
                session.Store(dto);
                session.SaveChanges();
            }
        }
    }

    public class CustomerListDto
    {
        public Guid Id { get; set; }

        public string City { get; set; }

        public string Name { get; set; }
    }
}