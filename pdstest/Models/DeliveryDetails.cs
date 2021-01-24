using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pdstest.Models
{
    public class DeliveryDetails
    {
       public int  EmployeeId { get; set; }
        public int StationId { get; set; }
        public int CurrentMonth { get; set; }
       public string  EmployeeCode { get; set; }
       public string EmployeeName { get; set; }
        public int DeliveryCount { get; set; }
        public int  PetrolAllowance { get; set; }
        public string StandardRate { get; set; }
        public int DeliveryRate { get; set; }
        public int Incentive { get; set; }
        public int  TotalAmount { get; set; }
    }
}
