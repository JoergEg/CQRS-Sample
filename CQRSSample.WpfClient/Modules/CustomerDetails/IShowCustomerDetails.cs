using System;

namespace CQRSSample.WpfClient.Modules.CustomerDetails
{
    public interface IShowCustomerDetails
    {
        void WithCustomer(string customerId);

        Guid GetCustomerId();
    }
}