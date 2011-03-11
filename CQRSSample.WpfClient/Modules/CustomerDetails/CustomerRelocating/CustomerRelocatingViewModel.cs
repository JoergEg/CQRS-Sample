using System;
using Caliburn.Micro;
using CQRSSample.Domain.Commands;
using CQRSSample.Infrastructure;
using CQRSSample.ReadModel;

namespace CQRSSample.WpfClient.Modules.CustomerDetails.CustomerRelocating
{
    public class CustomerRelocatingViewModel : Screen
    {
        private readonly IBus _bus;
        private readonly IEventAggregator _eventAggregator;
        private readonly IReadRepository _readRepository;

        public CustomerRelocatingViewModel(IBus bus, IEventAggregator eventAggregator, IReadRepository readRepository)
        {
            _bus = bus;
            _eventAggregator = eventAggregator;
            _readRepository = readRepository;
        }

        public void WithCustomer(Guid customerId)
        {
            ViewModel = _readRepository.GetById<CustomerListDto>(customerId);
            Command = new RelocatingCustomerCommand(ViewModel.Id);
        }

        public CustomerListDto ViewModel { get; private set; }

        public RelocatingCustomerCommand Command { get; private set; }

        public void Save()
        {
            //TODO: Validation

            //important: send command over bus
            _bus.Send(Command);

            //signal for UI - change view
            _eventAggregator.Publish(new CustomerRelocatingSavedEvent());
        }
    }

    public class CustomerRelocatingSavedEvent
    {
    }
}