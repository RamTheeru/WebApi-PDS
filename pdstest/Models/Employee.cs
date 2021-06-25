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
        
      
      
      

       
        public DeliveryDetails delivery { get; set; }

    }
}
