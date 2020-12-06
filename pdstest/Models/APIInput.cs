using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace pdstest.Models
{
    public class APIInput
    {
       public int stationId { get; set; }
        [JsonIgnore]
        public string table { get; set; }
        public string vstartDate { get; set; }
        public string vEndDate { get; set; }
        public int? page { get; set; }
        public int? pagesize { get; set; }
        public string status { get; set; }
    }
}
