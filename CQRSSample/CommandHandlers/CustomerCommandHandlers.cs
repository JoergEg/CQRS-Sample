using CQRSSample.Commands;
using CQRSSample.Domain;
using CommonDomain.Persistence;

namespace CQRSSample.CommandHandlers
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
            var customer = _repository.GetById<Customer>(command.Id, command.Version);
            customer.RelocateCustomer(command.Street, command.Streetnumber, command.PostalCode, command.City);
            _repository.Save(customer, customer.Version);
        }
    }
}