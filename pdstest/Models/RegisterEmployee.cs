using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pdstest.Models
{
    public class RegisterEmployee
    {

        public int RegisterId { get; set; }
        public int EmployeeId { get; set; }
        public string EmpCode { get; set; }
        public int Pid { get; set; }
        public string Professionname { get; set; }
        public int StationId { get; set; }
        public string EmpID { get; set; }
        public bool IsRegister { get; set; }
        public string FirstName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string LastName { get;set; }
        public string MiddleName { get; set; }
        public bool IsActive { get; set; }
        public string DOB { get; set; }

        public int Age { get; set; }
        public string EmpAge { get; set; }
        public string BloodGroup { get; set; }
        public string Gender { get; set; }
        public bool MaritalStatus { get; set; }

        public string Phone { get; set; }

        public string Address1 { get; set; }

        public string Address2 { get; set; }

        public string Place { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string AadharNumber { get; set; }
        public string PANNumber { get; set; }

        public bool IsPermanent { get; set; }
        public string EmployeeType { get; set; }
        public string Gaurd_firstname { get; set; }

        public string Gaurd_lastname { get; set; }
        public string Gaurd_middlename { get; set; }
        public string Gaurd_PhoneNumber { get; set; }

        public string DOJ { get; set; }

        public string LoginType { get; set; }

        public string Designation { get; set; }

        public string StationCode { get; set; }

        public string LocationName { get; set; }  
            
        public int UserTypeId { get; set; }

        public string UserType { get; set; }

        public string EmpImage { get; set; }


    }
}
