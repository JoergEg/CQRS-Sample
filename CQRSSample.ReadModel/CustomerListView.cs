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
                var dto = session.Load<CustomerListDto>(Dto.GetDtoIdOf<CustomerListDto>(@event.AggregateId));
                dto.City = @event.City;
                session.SaveChanges();
            }
        }

        public void Handle(CustomerCreatedEvent @event)
        {
            using(var session = _documentStore.OpenSession())
            {
                var dto = new CustomerListDto { AggregateRootId = @event.AggregateId, City = @event.City, Name = @event.CustomerName };
                session.Store(dto);
                session.SaveChanges();
            }
        }
    }

    public class CustomerListDto : Dto
    {
        public string City { get; set; }

        public string Name { get; set; }
    }

    public abstract class Dto
    {
        public string Id
        {
            get { return GetDtoIdOf(AggregateRootId, GetType()); }
        }

        public Guid AggregateRootId { get; set; }

        public static string GetDtoIdOf<T>(Guid id) where T : Dto
        {
            return GetDtoIdOf(id, typeof (T));
        }

        public static string GetDtoIdOf(Guid id, Type type)
        {
            return type.Name + "/" + id;
        }
    }
}