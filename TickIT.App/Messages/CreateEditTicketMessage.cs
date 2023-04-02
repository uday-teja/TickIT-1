
using TickIT.Models.Models;
using static TickIT.Models.Enums;

namespace TickIT.App.Messages
{
    public class CreateEditTicketMessage
    {
        public Ticket Ticket { get; set; }

        public OperationType OperationType { get; set; }

        public object Sender { get; set; }
    }
}
