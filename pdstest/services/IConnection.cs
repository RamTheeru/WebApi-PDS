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
        DataBaseResult GetRegisteredUser(int registerId);
        DataBaseResult GetEmployees(string stationCode = "", bool isEmployee = false);
        DataBaseResult GetLoginUserInfo(string username, string password);
        DataBaseResult GetLoginUserInfo(int usertypeId, int employeeId);
        DataBaseResult ApproveUser(int registerId,string status,string empCode="",int pId = 0);
        DataBaseResult GetPaginationRecords(int stationId, string table, string vstartDate, string vEndDate = "", int? page = 1, int? pagesize = 5, string status = "", bool isEmployee = false, int currentMonth = 0);
        DataBaseResult InsertVoucher(Voucher input);
        DataBaseResult GetVoucherDetailsbyVoucherNumber(int voucherId);
        DataBaseResult UpdateVoucher(Voucher input);
        DataBaseResult GetPreviousCreditandDebitDetails(int stationId);
        DataBaseResult InsertLedger(Ledger input);
        DataBaseResult CreateSession(UserType input);
        DataBaseResult CheckUserExists(string userName);
        DataBaseResult CheckEmpCodeExists(string empCode,bool isEmployee);
        bool CheckIfSessionExists(string userName, int employeeId, int userTypeId);
        DataBaseResult UpdateSession(UserType usr);
        DataBaseResult DeleteSession(string userName,  int employeeId, int userTypeId);
        DataBaseResult ResetPassword(int employeeId, string password);
        DataBaseResult GetAdminDetails();
        DataBaseResult CreateCommercialConsant(CommercialConstant constant);
        Tuple<string, string> GetStationNameByStationId(int stationId);
        DataBaseResult UpdateDeliveryDetails(List<DeliveryDetails> cdds);
        DataBaseResult GetCDADeliveryDetails(int empId, int stationId, int currentMonth);
        DataBaseResult GetDeliveryRatesbyStation(int stationId);
        DataBaseResult GetAllEmpsDeliveryDetailsforPDF(int stationId, int currentMonth);
        DataBaseResult GetEmpDataforPDF(int employeeId);
        DataBaseResult GetEmpDeliveryDetailsforPDF(int employeeId, int stationId, int currentMonth);
        DataBaseResult RestoreDB(string file);
    }
}
