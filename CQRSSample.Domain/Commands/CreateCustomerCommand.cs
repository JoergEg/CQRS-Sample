using System;

namespace CQRSSample.Domain.Commands
{
    [Serializable]
    public class CreateCustomerCommand : Command
    {
        public readonly string ClientName;
        public readonly string Street;
        public readonly string StreetNumber;
        public readonly string PostalCode;
        public readonly string City;
        public readonly string PhoneNumber;

        public CreateCustomerCommand(Guid id, string clientName, string street, string streetNumber, string postalCode, string city, string phoneNumber)
            : base(id)
        {
            ClientName = clientName;
            Street = street;
            StreetNumber = streetNumber;
            PostalCode = postalCode;
            City = city;
            PhoneNumber = phoneNumber;
        }
    }
}