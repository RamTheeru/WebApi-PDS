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
        public DataBaseResult CreateEmployee(Employee input)
        {
           return _ops.CreateEmployee(input);
        }

        public DataBaseResult GetConstants()
        {
            return _ops.GetConstants();
        }

        public DataBaseResult GetEmployees(string stationCode = "")
        {
            return _ops.GetEmployees(stationCode);
        }

        public DataBaseResult GetRegisteredUsers(string stationCode = "")
        {
            return _ops.GetRegisteredUsers(stationCode);
        }

        public DataBaseResult RegisterEmployee(RegisterEmployee input)
        {
            return _ops.RegisterEmployee(input);
        }
    }
}
