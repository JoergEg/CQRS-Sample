using System;
using System.Collections.Generic;
using Caliburn.Micro;
using CQRSSample.ReadModel;
using System.Linq;

namespace CQRSSample.WpfClient.Modules.CustomerList
{
    public class CustomerListViewModel : Screen
    {
        private readonly IReadRepository _repository;
        private readonly IEventAggregator _eventAggregator;

        public CustomerListViewModel(IReadRepository repository, IEventAggregator eventAggregator)
        {
            _repository = repository;
            _eventAggregator = eventAggregator;
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
            //Todo: Paging
            ViewModel = _repository.GetAll<CustomerListDto>().Where(x => x.Name == SearchText);
        }

        private IEnumerable<CustomerListDto> _viewModel;
        public IEnumerable<CustomerListDto> ViewModel
        {
            get { return _viewModel; }
            private set
            {
                _viewModel = value;
                NotifyOfPropertyChange(() => ViewModel);
            }
        }

        public void ShowCustomerDetails(CustomerListDto dto)
        {
            _eventAggregator.Publish(new ShowCustomerDetailsEvent(dto.Id));
        }
    }

    public class ShowCustomerDetailsEvent
    {
        public readonly Guid Id;

        public ShowCustomerDetailsEvent(Guid id)
        {
            Id = id;
        }
    }
}