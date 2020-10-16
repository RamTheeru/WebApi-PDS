using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pdstest.Models
{
    public class APIResult
    {
        public List<RegisterEmployee> registerEmployees { get; set; }
        public List<Employee> employees { get; set; }
        public UserType userInfo { get; set; }
        public List<UserType> Usertypes { get; set; }
        public List<Designation> Designations { get; set; } 
        public string EmployeeName { get; set; }
        public string CommandType { get; set; }
        public Employee employee { get; set; }
        public RegisterEmployee registerEmployee { get; set; }
        public bool Status { get; set; }
        public string Message { get; set; }
        public int Id { get; set; }
        public string Token { get; set; }
    }
}
