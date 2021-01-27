using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using pdstest.Models;
using pdstest.DAL;

namespace pdstest.services
{
    public interface IConnection
    {
        DataBaseResult RegisterEmployee(RegisterEmployee input);
        DataBaseResult CreateEmployee(Employee input,bool isEmployee=false);
        DataBaseResult CreateCDAEmployee(Employee input);
        DataBaseResult GetConstants();
        DataBaseResult GetRegisteredUsers(int stationId);
        DataBaseResult GetEmployees(string stationCode = "", bool isEmployee = false);
        DataBaseResult GetLoginUserInfo(string username, string password);
        DataBaseResult ApproveUser(int registerId,string status,string empCode="",int pId = 0);
        DataBaseResult GetPaginationRecords(int stationId, string table, string vstartDate, string vEndDate = "", int? page = 1, int? pagesize = 5, string status = "", bool isEmployee = false);
        DataBaseResult InsertVoucher(Voucher input);
        DataBaseResult InsertLedger(Ledger input);
        DataBaseResult CreateSession(UserType input);
        DataBaseResult CheckUserExists(string userName);
        DataBaseResult CheckEmpCodeExists(string empCode,bool isEmployee);
        bool CheckIfSessionExists(string userName, int employeeId, int userTypeId);
        DataBaseResult DeleteSession(string userName,  int employeeId, int userTypeId);
        DataBaseResult GetAdminDetails();
        DataBaseResult CreateCommercialConsant(CommercialConstant constant);
        Dictionary<string, string> GetStationNameByStationId(int stationId);
        DataBaseResult UpdateDeliveryDetails(List<DeliveryDetails> cdds);
        DataBaseResult GetCDADeliveryDetails(int empId, int stationId, int currentMonth);
        DataBaseResult GetDeliveryRatesbyStation(int stationId);
    }
}
