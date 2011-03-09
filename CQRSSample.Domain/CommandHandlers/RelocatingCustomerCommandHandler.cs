using System;
using CommonDomain.Persistence;
using CQRSSample.Domain.Commands;
using CQRSSample.Domain.Domain;

namespace CQRSSample.Domain.CommandHandlers
{
    public class RelocatingCustomerCommandHandler : Handles<RelocatingCustomerCommand>
    {
        private readonly IRepository _repository;

        public RelocatingCustomerCommandHandler(IRepository repository)
        {
            _repository = repository;
        }

        public void Handle(RelocatingCustomerCommand command)
        {
            const int version = 0;
            var customer = _repository.GetById<Customer>(command.Id, version);
            customer.RelocateCustomer(command.Street, command.Streetnumber, command.PostalCode, command.City);
            _repository.Save(customer, Guid.NewGuid(), null);
        }
    }
}