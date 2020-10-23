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
        DataBaseResult CreateEmployee(Employee input);
        DataBaseResult GetConstants();
        DataBaseResult GetRegisteredUsers(string stationCode = "");
        DataBaseResult GetEmployees(string stationCode = "");
        DataBaseResult GetLoginUserInfo(string username, string password);
        DataBaseResult ApproveUser(int registerId);
        DataBaseResult GetPaginationRecords(int stationId, string table, string vstartDate, string vEndDate = "", int page = 1, int pagesize = 5, string status = "");
        DataBaseResult InsertVoucher(Voucher input);
        DataBaseResult InsertLedger(Ledger input);
    }
}
