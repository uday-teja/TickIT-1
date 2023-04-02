using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TickIT.Core.Models
{
    public class Message
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("subject")]
        public string Subject { get; set; }
        [JsonPropertyName("sentDateTime")]
        public DateTimeOffset? SentDateTime { get; set; }
        [JsonPropertyName("isRead")]
        public bool IsRead { get; set; }
        [JsonPropertyName("from")]
        public EmailAddress From { get; set; }
        [JsonPropertyName("webLink")]
        public string WebLink { get; set; }
    }
}
