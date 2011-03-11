using System;

namespace CQRSSample.Domain.Commands
{
    [Serializable]
    public class CreateCustomerCommand : Command
    {
        public string CustomerName { get; set; }
        public string Street { get; set; }
        public string StreetNumber { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public string PhoneNumber { get; set; }

        public CreateCustomerCommand(Guid id) : base(id){}

        public CreateCustomerCommand(Guid id, string customerName, string street, string streetNumber, string postalCode, string city, string phoneNumber)
            : base(id)
        {
            CustomerName = customerName;
            Street = street;
            StreetNumber = streetNumber;
            PostalCode = postalCode;
            City = city;
            PhoneNumber = phoneNumber;
        }
    }
}