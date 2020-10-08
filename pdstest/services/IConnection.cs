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

        DataBaseResult ApproveUser(int registerId);
    }
}
