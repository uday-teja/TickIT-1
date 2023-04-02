using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TickIT.Core.Models
{
    public class GraphApiResponse<T>
    {
        [JsonPropertyName("value")]
        public List<T> Value { get; set; }
    }
}
