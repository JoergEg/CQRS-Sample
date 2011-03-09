using System;

namespace CQRSSample.Domain.Events
{
    [Serializable]
    public class CustomerRelocatedEvent : DomainEvent
    {
        public readonly string Street;
        public readonly string StreetNumber;
        public readonly string PostalCode;
        public readonly string City;

        public CustomerRelocatedEvent(Guid id, string street, string streetNumber, string postalCode, string city)
        {
            AggregateId = id;
            Street = street;
            StreetNumber = streetNumber;
            PostalCode = postalCode;
            City = city;
        }
    }
}