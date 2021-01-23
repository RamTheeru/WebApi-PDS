using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pdstest.Models
{
    public class Employee : RegisterEmployee
    {
       public string Guard_FullName { get; set; }

        public string Guard_Phone { get; set; }
        public string DLLRStatus { get; set; }
        public string DLLRNumber { get; set; }
        public string VehicleNumber { get; set; }
        public string BankAccountNumber { get; set; }
        public string BankName { get; set; }
        public string BranchName { get; set; }
        public string IFSCCode { get; set; }
        public DeliveryDetails delivery { get; set; }

    }
}
