using System.Collections.Generic;

namespace TockIT.Auth.Models
{
    public class GraphApiResponse<T>
    {
        public List<T> Value { get; set; }
    }

}
