
using Caliburn.Micro;
using static TickIT.Models.Enums;
using System.Collections.Generic;
using System.Threading;
using System;
using TickIT.Models.Models;
using TickIT.App.Common;
using System.Linq;
using TickIT.App.Messages;

namespace TickIT.App.ViewModels
{
    public class CreateTicketViewModel : Screen,IHandle<CreateEditTicketMessage>
    {
        #region Fields
        private IEventAggregator _eventAggregator;
        private Ticket _ticket;
        private string _submitBtnContent;
        private UserRole _userRole;
        #endregion

        #region Properties

        public UserRole UserRole
        {
            get { return _userRole; }
            set
            {
                _userRole = value;
                if (value == UserRole.Create)
                {
                    SubmitBtnContent = Constant.Create;
                }
                else
                {
                    SubmitBtnContent = Constant.Update;
                }
            }
        }

        public Ticket InputTicket
        {
            get { return _ticket; }
            set { _ticket = value; NotifyOfPropertyChange(nameof(InputTicket)); }
        }

        public IEnumerable<Status> StatusOptions
        {
            get
            {
                return Enum.GetValues(typeof(Status)).Cast<Status>();
            }
        }

        public string SubmitBtnContent
        {
            get { return _submitBtnContent; }
            set { _submitBtnContent = value; NotifyOfPropertyChange(nameof(SubmitBtnContent)); }
        }

        #endregion

        #region Constructors

        public CreateTicketViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            UserRole = UserRole.Create;
            InputTicket = new()
            {
                Name = string.Empty,
                Description = string.Empty,
                Status = Status.New,
                Priority = Priority.Low,
                DueDate = DateTime.Now,
                Category = Category.NewFeature

            };
            _eventAggregator.SubscribeOnUIThread(this);

        }

        #endregion

        #region Methods

        public void CreateOrUpdateTicket()
        {
            InputTicket.Name = InputTicket.Name.Trim();
            InputTicket.Description = InputTicket.Description.Trim();
            if (UserRole == UserRole.Create)
            {
                CreateTicket();
            }
            else
            {
                UpdateTicket();
            }
            ResetInputControls();
        }

        public void CreateTicket()
        {
            if (InputTicket != null && !string.IsNullOrWhiteSpace(InputTicket.Name))
            {
                _eventAggregator.PublishOnUIThreadAsync(new CreateEditTicketMessage() { Sender = this, Ticket = InputTicket, OperationType = OperationType.Create });
            }

        }

        public void UpdateTicket()
        {
            if (InputTicket != null && !string.IsNullOrWhiteSpace(InputTicket.Name))
            {
                _eventAggregator.PublishOnUIThreadAsync(new CreateEditTicketMessage() { Sender = this, Ticket = InputTicket, OperationType = OperationType.Update });
            }
        }

        public void ResetInputControls()
        {
            InputTicket = new();
            UserRole = UserRole.Create;
            SubmitBtnContent = Constant.Create;
        }

        public void Cancel()
        {
            ResetInputControls();
        }

        #endregion

        #region EventHandlers

        public System.Threading.Tasks.Task HandleAsync(CreateEditTicketMessage message, CancellationToken cancellationToken)
        {
            if (message.Sender != this && message.OperationType == OperationType.Display)
            {
                UserRole = UserRole.Edit;
                InputTicket = new(message.Ticket);
            }
            return System.Threading.Tasks.Task.CompletedTask;
        }

        #endregion

        #region Overrides

        public override object GetView(object context = null)
        {
            return null;
        }

        #endregion
    }
}
