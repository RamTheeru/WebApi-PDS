using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace pdstest.Models
{
    public class RegisterEmployee
    {

        public int RegisterId { get; set; }

        //outputs
        public int OutRegisterId { get; set; }
        public string EmpName { get; set; }
        public string EmpId { get; set; }


        public int EmployeeId { get; set; }
        public string EmpCode { get; set; }
        public string CDACode { get; set; }
        public int Pid { get; set; }
        public string Email { get; set; }
        public string Professionname { get; set; }
        public int StationId { get; set; }
       
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
        public string IsMarrired { get; set; }
        public bool MaritalStatus { get; set; }

        public string Phone { get; set; }

        public string Address1 { get; set; }

        public string Address2 { get; set; }
        public string EmployeeNameasperBank { get; set; }
        public string FatherName { get; set; }
        public string MotherName { get; set; }
        public string District { get; set; }
        public string Place { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string AadharNumber { get; set; }
        public string PANNumber { get; set; }
        public bool IsPAN { get; set; }
        public string PANStatus { get; set; } 
        public string HouseNo { get; set; }
        public string StreetName { get; set; }
        public string VillageorTown { get; set; }
        public string LandMark { get; set; }
        public bool IsPermanent { get; set; }
        public string EmployeeType { get; set; }
        public string Gaurd_firstname { get; set; }

        public string Gaurd_lastname { get; set; }
        public string Gaurd_middlename { get; set; }
        public string Gaurd_PhoneNumber { get; set; }

        public string DOJ { get; set; }
        public string ContractEndDate { get; set; }

        public string LoginType { get; set; }
        public string Indemunity_Bond { get; set; }
        public string Designation { get; set; }

        public string StationCode { get; set; }

        public string LocationName { get; set; }  
            
        public int UserTypeId { get; set; }

        public string UserType { get; set; }

        public string EmpImage { get; set; }

        public string BankAccountNumber { get; set; }

        public string IFSCCode { get; set; }
        public string BankName { get; set; }
        public string BranchName { get; set; }
        public string DLLRStatus { get; set; }
        public bool IsDllr { get; set; }
        public string DLLRNumber { get; set; }
        public string IssuedDate { get; set; }
        public string ExpiryDate { get; set; }
        public string VehicleNumber { get; set; }
        public string GPAPolicyNumber { get; set; }
        public string GPAPolicyInsurer { get; set; }
        public string GPAPolicyExpiryDate { get; set; }
        public string GMCPolicyNumber { get; set; }
        public string GMCPolicyInsurer { get; set; }
        public string GMCPolicyExpiryDate { get; set; }
        public string HealthCardStatus { get; set; }
        public bool IsHealthCard { get; set; }
        public string HealthCardNumber { get; set; }
        public string ESICNo { get; set; }
        public string ESICCardNumber { get; set; }
        public string PfMembershipNumber { get; set; }
        public string UAN { get; set; }

        public bool IsReference { get; set; }
        public string ReferenceStatus { get; set; }
        public string RefName { get; set; }
        public string RefContactNumber { get; set; }
        public bool IsPhysicallyHandicapped { get; set; }
        public string PhysicallyHandicapStatus { get; set; }
        public string AddedBY { get; set; }
    }
}
