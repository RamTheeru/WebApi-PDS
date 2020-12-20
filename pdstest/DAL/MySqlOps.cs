using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using pdstest.Models;
using pdstest.services;

namespace pdstest.DAL
{
    public class MySqlOps : IConnection
    {
        private MySQLDBOperations _ops = new MySQLDBOperations();

        //public MySqlOps(MySQLDBOperations ops)
        //{
        //    _ops = ops;
        //}
        public DataBaseResult CreateEmployee(Employee input,bool isEmployee=false)
        {
           return _ops.CreateEmployee(input,isEmployee);
        }

        public DataBaseResult GetConstants()
        {
            return _ops.GetConstants();
        }

        public DataBaseResult GetEmployees(string stationCode = "", bool isEmployee = false)
        {
            return _ops.GetEmployees(stationCode,isEmployee);
        }

        public DataBaseResult GetRegisteredUsers(string stationCode = "")
        {
            return _ops.GetRegisteredUsers(stationCode);
        }

        public DataBaseResult RegisterEmployee(RegisterEmployee input)
        {
            return _ops.RegisterEmployee(input);
        }
        public DataBaseResult CheckUserExists(string userName)
        {
            return _ops.CheckUserExists(userName);
        }
        public DataBaseResult ApproveUser(int registerId)
        {
            return _ops.ApproveUser(registerId);
        }
        public DataBaseResult GetLoginUserInfo(string username, string password)
        {
            return _ops.GetLoginUserInfo(username,password);
        }
        public DataBaseResult GetPaginationRecords(int stationId, string table, string vstartDate, string vEndDate = "", int? page = 1, int? pagesize = 5, string status = "", bool isEmployee = false)
        {
            return _ops.GetPaginationRecords(stationId, table, vstartDate, vEndDate, page, pagesize, status,isEmployee);
        }

        public DataBaseResult InsertVoucher(Voucher input)
        {
            return _ops.InsertVoucher(input);
        }
        public DataBaseResult InsertLedger(Ledger input)
        {
            return _ops.InsertLedger(input);
        }

        public DataBaseResult CreateSession(UserType input)
        {
            return _ops.CreateSession(input);
        }

    }
}
