using Caliburn.Micro;
using MahApps.Metro.Controls.Dialogs;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using TickIT.Models.Models;
using static TickIT.Models.Enums;
using Microsoft.Toolkit.Uwp.Notifications;
using Constant = TickIT.App.Common.Constant;
using TickIT.App.Helpers;
using TickIT.App.Messages;
using TickIT.Auth;

namespace TickIT.App.ViewModels
{
    public class HomeViewModel : Conductor<Screen>, IHandle<CreateEditTicketMessage>
    {
        #region Fields

        private bool _isListViewEnabled;
        private bool _isCardViewEnabled;
        private readonly SimpleContainer _container;
        private readonly IEventAggregator _eventAggregator;
        private CreateTicketViewModel _createTicketView;
        private ListViewModel _listViewModel;
        private BindableCollection<Ticket> _newTickets;
        private BindableCollection<Ticket> _inProgressTickets;
        private BindableCollection<Ticket> _completedTickets;
        private static int _activeHomeViewModelId;
        private bool isCreateFormOpen;

        #endregion
        #region Properties

        public bool IsCreateFormOpen
        {
            get { return isCreateFormOpen; }
            set
            {
                isCreateFormOpen = value;
                NotifyOfPropertyChange(() => IsCreateFormOpen);
            }
        }

        public BindableCollection<Ticket> NewTickets
        {
            get
            {
                return _newTickets;
            }
            set
            {
                _newTickets = value;
                NotifyOfPropertyChange(nameof(NewTickets));
            }

        }

        public BindableCollection<Ticket> InProgressTickets
        {
            get
            {
                return _inProgressTickets;
            }
            set
            {
                _inProgressTickets = value;
                NotifyOfPropertyChange(nameof(InProgressTickets));
            }
        }

        public BindableCollection<Ticket> CompletedTickets
        {
            get
            {
                return _completedTickets;
            }
            set
            {
                _completedTickets = value;
                NotifyOfPropertyChange(nameof(CompletedTickets));
            }
        }

        public bool CanSwitchToListView
        {
            get
            {
                return IsCardViewEnabled;
            }
        }

        public bool CanSwitchToCardView
        {
            get
            {
                return IsListViewEnabled;
            }
        }

        public bool IsListViewEnabled
        {
            get
            {
                return _isListViewEnabled;
            }
            set
            {
                _isListViewEnabled = value;
                NotifyOfPropertyChange(nameof(IsListViewEnabled));
                NotifyOfPropertyChange(nameof(CanSwitchToCardView));
                NotifyOfPropertyChange(nameof(CanSwitchToListView));
            }
        }

        public bool IsCardViewEnabled
        {
            get
            {
                return _isCardViewEnabled;
            }
            set
            {
                _isCardViewEnabled = value;
                NotifyOfPropertyChange(nameof(IsCardViewEnabled));
                NotifyOfPropertyChange(nameof(CanSwitchToCardView));
                NotifyOfPropertyChange(nameof(CanSwitchToListView));
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

        public ListViewModel ListViewModel
        {
            get
            {
                return _listViewModel;
            }
            set
            {
                _listViewModel = value;
                NotifyOfPropertyChange(nameof(ListView));
            }
        }

        public static int ActiveHomeViewModelId
        {
            get { return _activeHomeViewModelId; }
            set { _activeHomeViewModelId = value; }
        }

        #endregion
        public HomeViewModel(IEventAggregator eventAggregator, SimpleContainer container)
        {
            _container = container;
            InitializeTicketLists();
            CreateTicketView = _container.GetInstance<CreateTicketViewModel>();
            ListViewModel = _container.GetInstance<ListViewModel>();
            IsCardViewEnabled = true;
            _eventAggregator = eventAggregator;
            _eventAggregator.SubscribeOnUIThread(this);
            InitializeNotificationTimer();
        }
        #region Methods

        private void InitializeTicketLists()
        {
            var tickets = new List<Ticket>();
            tickets = DataGenerator.CreateTickets(5);
            var mails = HostService.GetMailsFromJson();
            NewTickets = new(tickets.Where(tsk => tsk.Status == Status.New));
            InProgressTickets = new(tickets.Where(tsk => tsk.Status == Status.InProgress));
            CompletedTickets = new(tickets.Where(tsk => tsk.Status == Status.Completed));
        }

        public void CreateTicket(Ticket inputTicket)
        {
            //_repository.CreateTicket(inputTicket);
            AddTicketToUI(inputTicket);
        }

        public void UpdateTicket(Ticket inputTicket)
        {
            Ticket? oldTicket = GetTicketFromUI(inputTicket.Id);
            if (oldTicket != null)
            {
                AddTicketToUI(inputTicket);
                RemoveTicketFromUI(oldTicket.Id, oldTicket.Status);
                //_repository.UpdateTicket(inputTicket);
            }
            else
            {
                MessageBox.Show(Constant.UpdateFailed, Constant.ErrorOccured);
            }
        }

        private Ticket? GetTicketFromUI(Guid id)
        {
            return new[] { NewTickets, InProgressTickets, CompletedTickets }.SelectMany(tsk => tsk).FirstOrDefault(tsk => tsk.Id == id);
        }
        private void AddTicketToUI(Ticket Ticket)
        {
            switch (Ticket.Status)
            {
                case Status.New:
                    NewTickets.Add(Ticket);
                    break;
                case Status.InProgress:
                    InProgressTickets.Add(Ticket);
                    break;
                case Status.Completed:
                    CompletedTickets.Add(Ticket);
                    break;
            }
        }

        public void DisplayTicketById(Guid id)
        {
            Ticket selectedTicket = null;
            if (selectedTicket != null)
            {
                _eventAggregator.PublishOnUIThreadAsync(new CreateEditTicketMessage() { Sender = this, Ticket = selectedTicket, OperationType = OperationType.Display });
            }
        }

        /// <summary>
        /// isUiUpdate is true when the delete is raised through an event from listview.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <param name="isUiUpdate"></param>
        public async void DeleteById(Guid id, Status status, bool isUiUpdate = false)
        {

            if (!isUiUpdate)
            {
                if (await DialogHelper.ShowMessageDialog(Constant.ConfirmDeleteWinTitle, Constant.ConfirmDeleteMsg, MessageDialogStyle.AffirmativeAndNegative))
                {
                    await _eventAggregator.PublishOnUIThreadAsync(new CreateEditTicketMessage() { Sender = this, OperationType = OperationType.Delete, Ticket = new() { Id = id } });
                    //_repository.DeleteTicket(id);
                }
                else
                {
                    return;
                }
            }
            RemoveTicketFromUI(id, status);

        }

        private void RemoveTicketFromUI(Ticket Ticket)
        {
            switch (Ticket.Status)
            {
                case Status.New:
                    NewTickets.Remove(Ticket);
                    break;
                case Status.InProgress:
                    InProgressTickets.Remove(Ticket);
                    break;
                case Status.Completed:
                    CompletedTickets.Remove(Ticket);
                    break;
            }
        }

        private void RemoveTicketFromUI(Guid id, Status status)
        {
            try
            {
                switch (status)
                {
                    case Status.New:
                        NewTickets.Remove(NewTickets.FirstOrDefault(tsk => tsk.Id == id));
                        break;
                    case Status.InProgress:
                        InProgressTickets.Remove(InProgressTickets.FirstOrDefault(tsk => tsk.Id == id));
                        break;
                    case Status.Completed:
                        CompletedTickets.Remove(CompletedTickets.FirstOrDefault(tsk => tsk.Id == id));
                        break;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, Constant.ErrorOccured);
            }
        }

        public void SwitchToListView()
        {
            IsListViewEnabled = true;
            IsCardViewEnabled = false;
        }

        public void SwitchToCardView()
        {
            IsListViewEnabled = false;
            IsCardViewEnabled = true;
        }

        public void ShowTicketForm()
        {
            IsCreateFormOpen = !IsCreateFormOpen;
        }

        #endregion

        #region Drag and Drop helpers

        public void MouseMoveHandler(MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && e.Source is ListBox lbox && lbox.SelectedItem != null)
            {
                DragDrop.DoDragDrop(lbox, lbox.SelectedItem, DragDropEffects.Move);
            }
        }

        public void DropOnNewTickets(DragEventArgs e)
        {
            if (e.Data.GetData(typeof(Ticket)) is Ticket Ticket && Ticket.Status != Status.New)
            {
                RemoveTicketFromUI(Ticket);
                Ticket.Status = Status.New;
                //_repository.UpdateTicket(Ticket);
                NewTickets.Add(Ticket);
            }
        }

        public void DropOnInProgressTickets(DragEventArgs e)
        {
            if (e.Data.GetData(typeof(Ticket)) is Ticket Ticket && Ticket.Status != Status.InProgress)
            {
                RemoveTicketFromUI(Ticket);
                Ticket.Status = Status.InProgress;
                //_repository.UpdateTicket(Ticket);
                InProgressTickets.Add(Ticket);
            }
        }
        //TODO: remove showmessageasync
        public async void DropOnCompletedTickets(DragEventArgs e)
        {
            if (await DialogHelper.ShowMessageDialog(Constant.TicketCompletedWinTitle, Constant.TicketCompletedMsg, MessageDialogStyle.AffirmativeAndNegative) && e.Data.GetData(typeof(Ticket)) is Ticket ticket && ticket.Status != Status.Completed)
            {
                RemoveTicketFromUI(ticket);
                ticket.Status = Status.Completed;
                //_repository.UpdateTicket(Ticket);
                CompletedTickets.Add(ticket);
            }
        }

        #endregion

        #region Notification Helper methods

        private void InitializeNotificationTimer()
        {
            DispatcherTimer timer = new();
            timer.Tick += Timer_Tick;
            timer.Interval = TimeSpan.FromHours(1);
            timer.Start();
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            List<Ticket> Tickets = NewTickets.Where((tsk => tsk.DueDate <= DateTime.Now.AddHours(1) && tsk.DueDate >= DateTime.Now.Subtract(TimeSpan.FromHours(1)))).ToList();
            Tickets.AddRange(InProgressTickets.Where((tsk => tsk.DueDate <= DateTime.Now.AddHours(1) && tsk.DueDate >= DateTime.Now.Subtract(TimeSpan.FromHours(1)))));
            string message;
            switch (Tickets.Count)
            {
                case 0: break;
                case 1:
                    message = $"{Tickets[0].Name} is nearing due time";
                    RaiseToastNotification(Constant.TicketDue, message);
                    break;
                default:
                    message = $"You have {Tickets.Count} Tickets nearing due time";
                    RaiseToastNotification(Constant.TicketDue, message);
                    break;
            }
        }

        private void RaiseToastNotification(string title, string message, ToastScenario toastScenario = ToastScenario.Reminder, ToastDuration toastDuration = ToastDuration.Long)
        {
            new ToastContentBuilder()
                .AddAppLogoOverride(Constant.IconPath)
                .SetToastScenario(toastScenario)
                .SetToastDuration(toastDuration)
                .AddText(title)
                .AddText(message);

        }

        #endregion

        #region EventHandlers

        public Task HandleAsync(CreateEditTicketMessage message, CancellationToken cancellationToken)
        {
            //publisher shouldn't handle itself the event it published && handling instance should be the currently activatedVM instance
            if (message.Sender.GetHashCode() != this.GetHashCode() && this.GetHashCode() == ActiveHomeViewModelId)
            {
                if (message.OperationType == OperationType.Display)
                {
                    return Task.CompletedTask;
                }
                else if (message.OperationType == OperationType.Create)
                {
                    CreateTicket(message.Ticket);
                }
                else if (message.OperationType == OperationType.Update)
                {
                    UpdateTicket(message.Ticket);
                }
                else if (message.OperationType == OperationType.Delete)
                {
                    DeleteById(message.Ticket.Id, message.Ticket.Status, true);
                }
            }
            return Task.CompletedTask;
        }

        #endregion

        #region Overriden Methods

        protected override Task OnDeactivateAsync(bool close, CancellationToken cancellationToken)
        {
            _eventAggregator.Unsubscribe(this);
            DeactivateItemAsync(ListViewModel, close, cancellationToken);
            return base.OnDeactivateAsync(close, cancellationToken);
        }

        protected override Task OnActivateAsync(CancellationToken cancellationToken)
        {
            ListViewModel = _container.GetInstance<ListViewModel>();
            ActivateItemAsync(ListViewModel);
            ListViewModel.ActiveListViewModelId = ListViewModel.GetHashCode();
            return base.OnActivateAsync(cancellationToken);
        }

        #endregion
    }
}
