using System;
using Caliburn.Micro;
using CQRSSample.ReadModel;

namespace CQRSSample.WpfClient.Modules.CustomerDetails.CustomerDetailsOverview
{
    public class CustomerDetailsOverviewViewModel : Screen
    {
        private readonly IReadRepository _readRepository;
        private readonly IEventAggregator _eventAggregator;

        public CustomerDetailsOverviewViewModel(IReadRepository readRepository, IEventAggregator eventAggregator)
        {
            _readRepository = readRepository;
            _eventAggregator = eventAggregator;
        }

        public void WithCustomer(Guid customerId)
        {
            ViewModel = _readRepository.GetById<CustomerListDto>(customerId);
        }

        //TODO: Change CustomerListDto to something specific for this screen
        public CustomerListDto ViewModel { get; private set; }

        public void RelocateCustomer()
        {
            _eventAggregator.Publish(new ShowCustomerRelocatingEvent(ViewModel.Id));
        }
    }

    public class ShowCustomerRelocatingEvent
    {
        public readonly Guid CustomerId;

        public ShowCustomerRelocatingEvent(Guid customerId)
        {
            CustomerId = customerId;
        }
    }
}