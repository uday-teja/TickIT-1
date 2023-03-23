using static TickIT.Models.Enums;

namespace TickIT.Models.Models
{
    public class CreateEditTicketMessage
    {
        public Ticket Ticket { get; set; }

        public OperationType OperationType { get; set; }

        public object Sender { get; set; }
    }
}
