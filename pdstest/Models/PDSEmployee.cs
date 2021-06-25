using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pdstest.Models
{
    public class PDSEmployee : Employee
    {
        public string ReportingManager { get; set; }
        public string ReportingManagerEmpCode { get; set; }
        public bool IsLwfApplicable { get; set; }
        public bool IsPfOption { get; set; }
        public string PfFundName { get; set; }
      
        public string PfDOJFund { get; set; }
        
        public bool IsEsicApplicable { get; set; }
        public string ESICFundId { get; set; }
        public string ESICMemberShipNumber { get; set; }
        public string ESICDOJFund { get; set; }
        public string EmployeeCategory { get; set; }
        public string ShiftDetails { get; set; }
        public string LeavePolicy { get; set; }

        public string AttendancePolicy { get; set; }
        public string OverTimeLogic { get; set; }
        public string OverTimeLogicPayout { get; set; }
        public string LateInEarlyOut { get; set; }
        public string WeeklyOff { get; set; }
        public string DayOff { get; set; }
        public string HolidayList { get; set; }

        public string AttendanceCycle { get; set; }
        public string ApprovalHierarchy { get; set; }
        public string ClosingLeaveBalances { get; set; }
        public string DAPeopleSoftId { get; set; }
        public string PSC { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string EmployeeStatus { get; set; }
        public string ICPaymentMethod { get; set; }
        public string PaymentPerUnit { get; set; }
        public string PayFrequency { get; set; }
        public string RegularPayRateDesc { get; set; }
        public string BlockARate { get; set; }
        public string BlockBRate { get; set; }
        public string PackagesDelivered { get; set; }
        public string ApplicablePayRate { get; set; }



    }
}
