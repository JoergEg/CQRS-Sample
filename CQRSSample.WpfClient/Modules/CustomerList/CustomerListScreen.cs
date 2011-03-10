using System.Collections.Generic;
using Caliburn.Micro;
using CQRSSample.ReadModel;
using System.Linq;

namespace CQRSSample.WpfClient.Modules.CustomerList
{
    public class CustomerListViewModel : Screen
    {
        private readonly IReadRepository _repository;

        public CustomerListViewModel(IReadRepository repository)
        {
            _repository = repository;
        }

        private string _searchText;
        public string SearchText
        {
            get { return _searchText; }
            set
            {
                _searchText = value;
                NotifyOfPropertyChange(() => SearchText);
                NotifyOfPropertyChange(() => CanSearch);
            }
        }

        public bool CanSearch
        {
            get { return !string.IsNullOrWhiteSpace(SearchText); }
        }

        public void Search()
        {
            NotifyOfPropertyChange(() => ViewModel);
        }

        public IEnumerable<CustomerListDto> ViewModel 
        { 
            get
            {
                //Todo: Paging
                return _repository.GetAll<CustomerListDto>().Where(x => x.Name == SearchText);
            } 
        }
    }
}