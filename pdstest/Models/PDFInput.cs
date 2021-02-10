using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pdstest.Models
{
    public class PDFInput
    {
        public List<int> emps { get; set; }
        public int currentmonth { get; set; }
        public bool forall { get; set; }
        public int stationId { get; set; }
    }
}
