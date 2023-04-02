using Microsoft.Graph.Models;
using System;
using System.Text.Json.Serialization;

namespace TickIT.Auth.Models
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
