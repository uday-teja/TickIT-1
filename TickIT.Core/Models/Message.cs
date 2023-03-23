using Microsoft.Graph.Models;
using System;

namespace TockIT.Auth.Models
{
    public class Message
    {
        public string Id { get; set; }
        public string Subject { get; set; }
        public DateTimeOffset? SentDateTime { get; set; }
        public bool IsRead { get; set; }
        public EmailAddress From { get; set; }
    }
}
