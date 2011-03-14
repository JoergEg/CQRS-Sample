using System;
using CommonDomain.Persistence;
using CQRSSample.Commands;
using CQRSSample.Domain.Domain;

namespace CQRSSample.Domain.CommandHandlers
{
    public class CreateCustomerCommandHandler : Handles<CreateCustomerCommand>
    {
        private readonly IRepository _repository;

        public CreateCustomerCommandHandler(IRepository repository)
        {
            _repository = repository;
        }

        public void Handle(CreateCustomerCommand command)
        {
            var client = Customer.CreateNew(command.Id, new CustomerName(command.CustomerName),
                                          new Address(command.Street, command.StreetNumber,
                                                      command.PostalCode, command.City),
                                          new PhoneNumber(command.PhoneNumber));
            _repository.Save(client, Guid.NewGuid(), null);
        }
    }
}