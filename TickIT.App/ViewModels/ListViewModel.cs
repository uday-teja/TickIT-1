using Caliburn.Micro;
using MahApps.Metro.Controls.Dialogs;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System.Windows;
using System;
using TickIT.Models.Models;
using static TickIT.Models.Enums;
using TickIT.App.Common;
using TickIT.App.Helpers;
using TickIT.App.Messages;

namespace TickIT.App.ViewModels
{
    public class ListViewModel : Screen,IHandle<CreateEditTicketMessage>
    {
        #region Fields
        private bool _isGroupingEnabled;
        private int _currentPageNumber;
        //private ITicketRepository _repository;
        private readonly SimpleContainer _container;
        private readonly IEventAggregator _eventAggregator;
        private string _searchKeyword;
        private int _totalPagesCount;
        private int _itemsPerPage = 10;
        private CreateTicketViewModel _createTicketView;
        private bool _isTicketFormEnabled;
        private BindableCollection<Ticket> _tickets;
        private static int _activeListViewModelId;
        private BindableCollection<Ticket> _filteredTickets;
        #endregion

        #region Properties
        public BindableCollection<Ticket> Tickets
        {
            get
            {
                return _tickets;
            }
            set
            {
                _tickets = value;
                NotifyOfPropertyChange(nameof(Tickets));
            }
        }

        public BindableCollection<Ticket> FilteredTickets
        {
            get
            {
                return _filteredTickets;
            }
            set
            {
                _filteredTickets = value;
                NotifyOfPropertyChange(nameof(FilteredTickets));
            }
        }

        public int TotalPagesCount
        {
            get
            {
                return _totalPagesCount;
            }
            set
            {
                _totalPagesCount = value;
                NotifyOfPropertyChange(nameof(TotalPagesCount));
                NotifyOfPropertyChange(nameof(CanNavigateNext));
                NotifyOfPropertyChange(nameof(CanNavigatePrevious));
            }
        }

        public int ItemsPerPage
        {
            get { return _itemsPerPage; }
            set
            {
                if (int.TryParse(value.ToString(), out int intValue))
                {
                    _itemsPerPage = intValue;
                    NotifyOfPropertyChange(nameof(ItemsPerPage));
                    NotifyOfPropertyChange(nameof(CanNavigateNext));
                    NotifyOfPropertyChange(nameof(CanNavigatePrevious));
                    InitializeTicketLists();
                }
                else
                {
                    _itemsPerPage = 10;
                }
            }
        }

        public int CurrentPageNumber
        {
            get { return _currentPageNumber; }
            set
            {
                _currentPageNumber = value;
                NotifyOfPropertyChange(nameof(CurrentPageNumber));
                NotifyOfPropertyChange(nameof(CanNavigateNext));
                NotifyOfPropertyChange(nameof(CanNavigatePrevious));
            }
        }

        public bool CanNavigateNext
        {
            get
            {
                return CurrentPageNumber < TotalPagesCount;
            }
        }

        public string SearchKeyword
        {
            get
            {
                return _searchKeyword;
            }
            set
            {
                _searchKeyword = value;
                NotifyOfPropertyChange(nameof(SearchKeyword));
            }
        }

        public bool CanNavigatePrevious
        {
            get
            {
                return CurrentPageNumber > 1;
            }
        }

        public CreateTicketViewModel CreateTicketView
        {
            get
            {
                return _createTicketView;
            }
            set
            {
                _createTicketView = value;
                NotifyOfPropertyChange(nameof(CreateTicketView));
            }
        }

        public bool IsTicketFormEnabled
        {
            get
            {
                return _isTicketFormEnabled;
            }
            set
            {
                _isTicketFormEnabled = value;
                NotifyOfPropertyChange(nameof(IsTicketFormEnabled));
            }
        }

        public bool IsGroupingEnabled
        {
            get
            {
                return _isGroupingEnabled;
            }
            set
            {
                _isGroupingEnabled = value;
                NotifyOfPropertyChange(nameof(IsGroupingEnabled));
            }
        }

        public static int ActiveListViewModelId
        {
            get
            {
                return _activeListViewModelId;
            }
            set
            {
                _activeListViewModelId = value;
            }
        }
        #endregion

        public ListViewModel(SimpleContainer container, IEventAggregator eventAggregator, CreateTicketViewModel CreateTicketViewModel)
        {
            _container = container;
            CreateTicketView = CreateTicketViewModel;
            _eventAggregator = eventAggregator;
            _eventAggregator.SubscribeOnUIThread(this);
            //_repository = DbContextFactory.TaskRepository;
            InitializeTicketLists();
        }

        private void InitializeTicketLists()
        {
            Tickets = new BindableCollection<Ticket>();//new(_repository.GetTasks());
            //Tasks = new(DataGenerator.CreateTasks(100));
            FilteredTickets = new BindableCollection<Ticket>();
            Tickets = new(Tickets.Take(ItemsPerPage));
            TotalPagesCount = (Tickets.Count + ItemsPerPage - 1) / ItemsPerPage;
            CurrentPageNumber = 1;
        }

        public async Task SearchTickets()
        {
            await Task.Run(() =>
            {
                if (!string.IsNullOrWhiteSpace(SearchKeyword))
                    FilteredTickets = Tickets.Count > 0 ? new(Tickets.Where(tsk => tsk.Name.Contains(SearchKeyword, StringComparison.OrdinalIgnoreCase))) : new();
                else
                    LoadCurrentPage();
            });
        }

        public async Task DeleteTicketById(string id)
        {
            if (await DialogHelper.ShowMessageDialog(Constant.ConfirmDeleteWinTitle, Constant.ConfirmDeleteMsg, MessageDialogStyle.AffirmativeAndNegative))
            {
                try
                {
                    Ticket ticket = Tickets.FirstOrDefault(tsk => tsk.Id == id);
                    _eventAggregator.PublishOnUIThreadAsync(new CreateEditTicketMessage() { Sender = this, OperationType = OperationType.Delete, Ticket = ticket });
                    RemoveTicketFromUI(id);
                    //_repository.DeleteTask(id);
                }
                catch (Exception)
                {
                    MessageBox.Show(Constant.DeleteFailedMsg, Constant.DeleteFailedWinTitle);
                }
            }
        }

        public void AddTicketToUI(Ticket ticket)
        {
            Tickets.Add(ticket);
            FilteredTickets.Add(ticket);
        }

        public void RemoveTicketFromUI(string id)
        {
            try
            {
                FilteredTickets.Remove(Tickets.FirstOrDefault(tsk => tsk.Id == id));
                Tickets.Remove(Tickets.FirstOrDefault(tsk => tsk.Id == id));
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        public void DisplayTicketById(Guid id)
        {
            Ticket selectedTicket = new Ticket();
            if (selectedTicket != null)
            {
                _eventAggregator.PublishOnUIThreadAsync(new CreateEditTicketMessage() { Sender = this, Ticket = selectedTicket, OperationType = OperationType.Display });
                IsTicketFormEnabled = true;
            }
        }

        private void UpdateTicket(Ticket ticket)
        {
            RemoveTicketFromUI(ticket.Id);    
            AddTicketToUI(ticket);
            //_repository.UpdateTicket(ticket);
        }

        public void CloseTicketForm()
        {
            IsTicketFormEnabled = false;
        }

        #region Pagination Helper Methods

        public void NavigateNext()
        {

            FilteredTickets = new(Tickets.Skip(CurrentPageNumber * ItemsPerPage).Take(ItemsPerPage));
            CurrentPageNumber += 1;
        }

        public void NavigatePrevious()
        {
            FilteredTickets = new(Tickets.Skip((CurrentPageNumber - 2) * ItemsPerPage).Take(ItemsPerPage));
            CurrentPageNumber -= 1;
        }

        public void LoadCurrentPage()
        {
            FilteredTickets = new(Tickets.Skip((CurrentPageNumber - 1) * ItemsPerPage).Take(ItemsPerPage));
        }



        #endregion

        #region EventHandlers
        public Task HandleAsync(CreateEditTicketMessage message, CancellationToken cancellationToken)
        {
            if (message != null && message.Sender.GetHashCode() != this.GetHashCode() && this.GetHashCode() == ActiveListViewModelId)
            {
                switch (message.OperationType)
                {
                    case OperationType.Create:
                        AddTicketToUI(message.Ticket);
                        break;
                    case OperationType.Update:
                        UpdateTicket(message.Ticket);
                        break;
                    case OperationType.Delete:
                        RemoveTicketFromUI(message.Ticket.Id);
                        break;
                }
                CloseTicketForm();
            }
            return Task.CompletedTask;
        }
        #endregion

        #region overriden methods
        protected override Task OnDeactivateAsync(bool close, CancellationToken cancellationToken)
        {
            _eventAggregator.Unsubscribe(this);
            return base.OnDeactivateAsync(close, cancellationToken);
        }

        #endregion
    }
}
