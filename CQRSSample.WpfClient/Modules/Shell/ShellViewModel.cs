using System.Windows;
using Caliburn.Micro;
using CQRSSample.ReadModel;
using CQRSSample.WpfClient.Modules.CustomerList;

namespace CQRSSample.WpfClient.Modules.Shell
{
    public class ShellViewModel : Conductor<object>
    {
        public ShellViewModel(IReadRepository repository)
        {
            ActivateItem(new CustomerListViewModel(repository));
        }
    }

}