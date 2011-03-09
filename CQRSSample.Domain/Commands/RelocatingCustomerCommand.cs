using System;

namespace CQRSSample.Domain.Commands
{
    [Serializable]
    public class RelocatingCustomerCommand : Command
    {
        public readonly string Street;
        public readonly string Streetnumber;
        public readonly string PostalCode;
        public readonly string City;

        public RelocatingCustomerCommand(Guid id, string street, string streetNumber, string postalCode, string city) : base(id)
        {
            Street = street;
            Streetnumber = streetNumber;
            PostalCode = postalCode;
            City = city;
        }
    }
}
