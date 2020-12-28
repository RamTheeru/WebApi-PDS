using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pdstest.Models
{
    public class CommercialConstant
    {
      public int CId { get; set; }
        public int StationId { get; set; }
        public string StationName { get; set; }
        public int DeliveryRate { get; set; }
        public int PetrolAllowance { get; set; }
        public int Incentives { get; set; }
        public bool IsActive { get; set; }

    }
}
