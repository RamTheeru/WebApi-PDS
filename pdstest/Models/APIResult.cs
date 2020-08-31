using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pdstest.Models
{
    public class APIResult
    {
        public List<RegisterEmployee> employees { get; set; }

        public List<UserType> Usertypes { get; set; }
        public List<Designation> Designations { get; set; } 
        public string EmployeeName { get; set; }
        public string CommandType { get; set; }
        public RegisterEmployee employee { get; set; }
        public bool Status { get; set; }
        public string Message { get; set; }
        public int Id { get; set; }
    }
}
