using Caliburn.Micro;

namespace CQRSSample.WpfClient.Modules.CustomerDetails.WhatsNext
{
    public class WhatsNextViewModel : Screen
    {
        private readonly IEventAggregator _eventAggregator;

        public WhatsNextViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
        }

        public void SearchCustomer()
        {
            _eventAggregator.Publish(new ShowSearchCustomerEvent());
        }

        public void AddCustomer()
        {
            _eventAggregator.Publish(new ShowAddNewCustomerEvent());
        }
    }

    public class ShowAddNewCustomerEvent
    {
    }

    public class ShowSearchCustomerEvent
    {
    }
}