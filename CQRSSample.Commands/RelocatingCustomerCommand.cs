using System;
using FluentValidation;

namespace CQRSSample.Commands
{
    [Serializable]
    public class RelocatingCustomerCommand : Command
    {
        public string Street { get; set; }
        public string Streetnumber { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }

        public RelocatingCustomerCommand(Guid id) : base(id){}

        public RelocatingCustomerCommand(Guid id, string street, string streetNumber, string postalCode, string city) : base(id)
        {
            Street = street;
            Streetnumber = streetNumber;
            PostalCode = postalCode;
            City = city;
        }
    }

    public class RelocatingCustomerValidator : AbstractValidator<RelocatingCustomerCommand>
    {
        public RelocatingCustomerValidator()
        {
            RuleFor(command => command.City).NotEmpty().NotNull();
        }
    }
}
