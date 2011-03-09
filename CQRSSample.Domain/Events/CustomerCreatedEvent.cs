using System;

namespace CQRSSample.Domain.Events
{
    [Serializable]
    public class CustomerCreatedEvent : DomainEvent
    {
        public readonly string CustomerName;
        public readonly string Street;
        public readonly string StreetNumber;
        public readonly string PostalCode;
        public readonly string City;
        public readonly string PhoneNumber;

        public CustomerCreatedEvent(Guid id, string customerName, string street, string streetNumber, string postalCode, string city, string phoneNumber)
        {
            AggregateId = id;
            CustomerName = customerName;
            Street = street;
            StreetNumber = streetNumber;
            PostalCode = postalCode;
            City = city;
            PhoneNumber = phoneNumber;
        }
    }
}