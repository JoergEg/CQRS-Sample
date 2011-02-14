using CQRSSample.Commands;
using CQRSSample.Domain;

namespace CQRSSample.CommandHandlers
{
    public class RelocatingCustomerCommandHandler : Handles<RelocatingCustomerCommand>
    {
        private readonly IRepository<Customer> _repository;

        public RelocatingCustomerCommandHandler(IRepository<Customer> repository)
        {
            _repository = repository;
        }

        public void Handle(RelocatingCustomerCommand command)
        {
            var customer = _repository.GetById(command.Id);
            customer.RelocateCustomer(command.Street, command.Streetnumber, command.PostalCode, command.City);
            _repository.Save(customer, customer.Version);
        }
    }
}