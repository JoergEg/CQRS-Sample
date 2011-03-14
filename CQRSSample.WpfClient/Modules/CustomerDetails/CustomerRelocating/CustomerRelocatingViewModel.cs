using System;
using Caliburn.Micro;
using CQRSSample.Commands;
using CQRSSample.Infrastructure;
using CQRSSample.ReadModel;
using CQRSSample.WpfClient.ApplicationFramework;

namespace CQRSSample.WpfClient.Modules.CustomerDetails.CustomerRelocating
{
    public class CustomerRelocatingViewModel : ScreenWithValidatingCommand<RelocatingCustomerCommand>
    {
        private readonly IBus _bus;
        private readonly IEventAggregator _eventAggregator;
        private readonly IReadRepository _readRepository;

        public CustomerRelocatingViewModel(IBus bus, IEventAggregator eventAggregator, IReadRepository readRepository)
        {
            _bus = bus;
            _eventAggregator = eventAggregator;
            _readRepository = readRepository;

            Validator = new RelocatingCustomerValidator();
        }

        public void WithCustomer(Guid customerId)
        {
            ViewModel = _readRepository.GetById<CustomerListDto>(customerId);
            Command = new ValidatingCommand<RelocatingCustomerCommand>(new RelocatingCustomerCommand(ViewModel.Id), Validator);
        }

        public CustomerListDto ViewModel { get; private set; }

        public void Save()
        {
            if(!Validate().IsValid)
                return;

            //important: send command over bus
            _bus.Send(Command.InnerCommand);

            //signal for UI - change view
            _eventAggregator.Publish(new CustomerRelocatingSavedEvent());
        }
    }


    public class CustomerRelocatingSavedEvent
    {
    }
}