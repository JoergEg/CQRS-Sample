using CQRSSample.Commands;
using CQRSSample.Domain;

namespace CQRSSample.CommandHandlers
{
    public class CreateCustomerCommandHandler : Handles<CreateCustomerCommand>
    {
        private readonly IRepository<Customer> _repository;

        public CreateCustomerCommandHandler(IRepository<Customer> repository)
        {
            _repository = repository;
        }

        public void Handle(CreateCustomerCommand command)
        {
            var client = Customer.CreateNew(command.Id, new CustomerName(command.ClientName),
                                          new Address(command.Street, command.StreetNumber,
                                                      command.PostalCode, command.City),
                                          new PhoneNumber(command.PhoneNumber));
            _repository.Save(client, -1);
        }
    }
}