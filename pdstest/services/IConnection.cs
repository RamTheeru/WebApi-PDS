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
        DataBaseResult GetConstants();
        DataBaseResult GetRegisteredUsers(string stationCode = "");
        DataBaseResult GetEmployees(string stationCode = "", bool isEmployee = false);
        DataBaseResult GetLoginUserInfo(string username, string password);
        DataBaseResult ApproveUser(int registerId);
        DataBaseResult GetPaginationRecords(int stationId, string table, string vstartDate, string vEndDate = "", int? page = 1, int? pagesize = 5, string status = "", bool isEmployee = false);
        DataBaseResult InsertVoucher(Voucher input);
        DataBaseResult InsertLedger(Ledger input);
        DataBaseResult CreateSession(UserType input);
    }
}
