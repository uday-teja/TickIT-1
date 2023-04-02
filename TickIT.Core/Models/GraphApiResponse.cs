using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TickIT.Auth.Models
{
    public class GraphApiResponse<T>
    {
        [JsonPropertyName("value")]

        public List<T> Value { get; set; }
    }

}
