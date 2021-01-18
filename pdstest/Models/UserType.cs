using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pdstest.Models
{
    public class UserType
    {
        public int UserTypeId { get; set; }
        public string User { get; set; }
        public int EmployeeId { get; set; }
        public string Role { get; set; }
        public string Token { get; set; }
        public bool Valid { get; set; }
        public int StationId { get; set; }
        public string Screen { get; set; }
        public DateTime StartDate { get; set; }
        public string SessionStartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string SessionEndDate { get; set; }
        public bool IsAlreadySession { get; set; }
    }
}
