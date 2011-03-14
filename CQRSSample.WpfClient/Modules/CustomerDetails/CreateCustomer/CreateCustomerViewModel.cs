using System;
using Caliburn.Micro;
using CQRSSample.Commands;
using CQRSSample.Infrastructure;

namespace CQRSSample.WpfClient.Modules.CustomerDetails.CreateCustomer
{
    public class CreateCustomerViewModel : Screen
    {
        private readonly IBus _bus;
        private readonly IEventAggregator _eventAggregator;

        public CreateCustomerViewModel(IBus bus, IEventAggregator eventAggregator)
        {
            _bus = bus;
            _eventAggregator = eventAggregator;
            Command = new CreateCustomerCommand(Guid.NewGuid());
        }

        public CreateCustomerCommand Command{ get; private set; }

        public void Save()
        {
            //important: send command over bus
            _bus.Send(Command);
            
            //signal for UI - change view
            _eventAggregator.Publish(new CreateCustomerSavedEvent());
        }
    }

    public class CreateCustomerSavedEvent
    {
    }
}