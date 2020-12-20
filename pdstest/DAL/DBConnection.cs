using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using pdstest.Models;

namespace pdstest.DAL
{
    public class DBConnection
    {
        public static Dictionary<string, string> _dbQueries = DBConnection.StoreQuiries();
        public static Dictionary<string, string> StoreQuiries()
        {
            Dictionary<string, string> sqllib = new Dictionary<string, string>();
            sqllib["LocalDB"] = @"server=localhost;database=PDS;userid=sa;password=12345;";                ////"Data Source=.;Initial Catalog=PDS;Integrated Security=True";
            sqllib["AWSDB"] = "";
            sqllib["GetUserTypes"] = "select ConstantId, ConstantName,Category from constants where IsActive = 1";
            sqllib["InsertEmpStoredProc"] = "usp_InsertEmployee";
            sqllib["RegisterEmpStoredProc"] = "usp_RegisterEmployee";
            sqllib["VoucherInsertProc"] = "usp_InsertVoucher";
            sqllib["LedgerInsertProc"] = "usp_InsertLedger";
            sqllib["CreateSessionProc"] = "usp_CreateSession";
            return sqllib;
        }

        
        public static string GetConstants()
        {
            string text = "";

            try
            {
                //var builder = new ConfigurationBuilder().SetBasePath(path).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                text = _dbQueries["GetUserTypes"];

            }
            catch (Exception e)
            {
                string msg = e.Message;
                text = "";

            }
            return text;
        }

        public static string GetRegisteredUsers(string stationCode="")
        {
            string text = "";

            try
            {
                //var builder = new ConfigurationBuilder().SetBasePath(path).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                if (!string.IsNullOrEmpty(stationCode))
                    text = string.Format("select * from register where IsActive = 0 and StateCode = '{0}'",stationCode);
                else
                    text = "select * from register where IsActive = 0";

            }
            catch (Exception e)
            {
                string msg = e.Message;
                text = "";

            }
            return text;
        }
        public  static Dictionary<string, string> GetRecordsforPagination(int stationId,string table,string vstartDate,string vEndDate="", int? page=1, int? pagesize=5, string status="",bool isEmployee=false)
        {
            /*SELECT* FROM Constants
                        LIMIT 10/*range , 5/*pagesize ;
            event_date BETWEEN '2018-01-01 12:00:00' AND '2018-01-01 23:30:00';*/
            DateTime dt = DateTime.Now;
            DateTime dt2 = DateTime.Now;
            Dictionary<string, string> text = new Dictionary<string, string>();
            int range = 0;
            int ps = pagesize == null || pagesize==0 ? 5 :Convert.ToInt32(pagesize);
            int p = page == null || page == 0 ? 1 : Convert.ToInt32(page);
            try
            {

                range = (ps * p) - ps;
                if (!string.IsNullOrEmpty(vstartDate))
                {
                    dt = vstartDate.StringtoDateTime();
                    vstartDate = dt.DateTimetoString();
                }
                if (!string.IsNullOrEmpty(vEndDate))
                {
                    dt2 = vEndDate.StringtoDateTime();
                    vEndDate = dt2.DateTimetoString();
                }
                if (!string.IsNullOrEmpty(table))
                {
                    if (table.ToLower() == "voucher" && !string.IsNullOrEmpty(vEndDate) && !string.IsNullOrEmpty(status))
                    {
                        text.Add("main", string.Format("SELECT * FROM Voucher where StationId = {0} AND (VoucherDate " +
                            "BETWEEN '{1}' AND '{2}') AND VoucherStatus = '{3}' LIMIT {4},{5};", stationId, vstartDate, vEndDate, status, range, ps));
                        text.Add("count", string.Format("SELECT COUNT(*) FROM Voucher where StationId = {0} AND (VoucherDate " +
                            "BETWEEN '{1}' AND '{2}') AND VoucherStatus = '{3}';", stationId, vstartDate, vEndDate, status));
                    }
                    else if (table.ToLower() == "voucher" && string.IsNullOrEmpty(vEndDate) && !string.IsNullOrEmpty(status))
                    {
                        text.Add("main", string.Format("SELECT * FROM Voucher where StationId = {0} AND (VoucherDate " +
                          "<= '{1}')  AND VoucherStatus = '{2}' LIMIT {3},{4};", stationId, vstartDate, status, range, ps));
                        text.Add("count", string.Format("SELECT COUNT(*) FROM Voucher where StationId = {0} AND (VoucherDate " +
                         "<= '{1}')  AND VoucherStatus = '{2}';", stationId, vstartDate, status));
                    }
                    else if (table.ToLower() == "voucher" && !string.IsNullOrEmpty(vEndDate))
                    {
                        text.Add("main", string.Format("SELECT * FROM Voucher where StationId = {0} AND (VoucherDate " +
                        "BETWEEN '{1}' AND '{2}') LIMIT {3},{4};", stationId, vstartDate, vEndDate, range, ps));
                        text.Add("count", string.Format("SELECT COUNT(*) FROM Voucher where StationId = {0} AND (VoucherDate " +
                        "BETWEEN '{1}' AND '{2}');", stationId, vstartDate, vEndDate));
                    }
                    else if (table.ToLower() == "voucher" && string.IsNullOrEmpty(vEndDate))
                    {
                        text.Add("main", string.Format("SELECT * FROM Voucher where StationId = {0} AND (VoucherDate " +
                         "<= '{1}')  LIMIT {2},{3};", stationId, vstartDate, range, ps));
                        text.Add("count", string.Format("SELECT COUNT(*) FROM Voucher where StationId = {0} AND (VoucherDate " +
                        "<= '{1}');", stationId, vstartDate));

                    }
                    else if (table.ToLower() == "ledger" && !string.IsNullOrEmpty(vEndDate))
                    {
                        text.Add("main", string.Format("SELECT * FROM FinanceLedger where StationId = {0} AND (VoucherDate " +
                              "BETWEEN '{1}' AND '{2}') AND Credit IS NOT NULL AND IsActive = 1 LIMIT {3},{4};", stationId, vstartDate, vEndDate, range, ps));
                        text.Add("count", string.Format("SELECT COUNT(*) FROM FinanceLedger where StationId = {0} AND (VoucherDate " +
                          "BETWEEN '{1}' AND '{2}') AND Credit IS NOT NULL AND IsActive = 1 ;", stationId, vstartDate, vEndDate));
                    }
                    else if (table.ToLower() == "employees")
                    {
                        text.Add("main", string.Format("SELECT * FROM employees where StationId = {0} AND IsEmployee = {1} AND IsActive = 1 LIMIT {2},{3};", stationId, isEmployee, range, ps));
                        text.Add("count", string.Format("SELECT COUNT(*) FROM employees where StationId = {0} AND IsEmployee = {1} AND IsActive = 1 ;", stationId, isEmployee));

                    }


                }
           

            }
            catch (Exception e)
            {
                string msg = e.Message;
                text = new Dictionary<string, string>();

            }
            return text;
        }

        public static string GetLoginSessionInfo(UserType info)
        {
            string text = "";

            try
            {
                string currentDate = DateTime.Now.DateTimetoString();
                ///DateTime nw = currentDate.StringtoDateTime();
                text = string.Format("SELECT EmployeeId,UserTypeId,UserName,Token FROM UserSessions where UserName = '{0}' AND UserTypeId = {1} AND IsActive=1" +
                    " AND EmployeeId = {2}  AND StartDate <= '{3}' AND  EndDate >= '{3}' LIMIT 1;", info.User, info.UserTypeId,info.EmployeeId,currentDate);

            }
            catch (Exception e)
            {
                string msg = e.Message;
                text = "";

            }
            return text;
        }
        public static string GetLoginSessionInfoByToken(string token)
        {
            string text = "";

            try
            {
                text = string.Format("select * from UserSessions where Token = '{0}';", token);

            }
            catch (Exception e)
            {
                string msg = e.Message;
                text = "";

            }
            return text;
        }
        public static string CheckUserNameExists(string username)
        {
            string text = "";

            try
            {
                text = string.Format("select COUNT(*) from Register where UserName = '{0}';", username);

            }
            catch (Exception e)
            {
                string msg = e.Message;
                text = "";

            }
            return text;
        }
        public static string GetLoginUserInfo(string username,string password)
        {
            string text = "";

            try
            {
                    text = string.Format("SELECT EmployeeId,UserTypeId,LoginType,FirstName FROM employees where FirstName = '{0}' AND Passwrd = '{1}' AND IsActive=1 LIMIT 1;", username,password);

            }
            catch (Exception e)
            {
                string msg = e.Message;
                text = "";

            }
            return text;
        }
        public static string ApproveUser(int registerId)
        {
            string text = "";

            try
            {
                //var builder = new ConfigurationBuilder().SetBasePath(path).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                
                text = string.Format("UPDATE register SET IsActive = 1 WHERE RegisterId = {0}", registerId);

            }
            catch (Exception e)
            {
                string msg = e.Message;
                text = "";

            }
            return text;
        }
        public static string GetEmployees(string stationCode = "")
        {
            string text = "";

            try
            {
                //var builder = new ConfigurationBuilder().SetBasePath(path).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                if (!string.IsNullOrEmpty(stationCode))
                    text = string.Format("select * from employees where IsActive = 1 and UserTypeId <> 1 and StateCode = '{0}'", stationCode);
                else
                    text = "select * from employees where IsActive = 1";

            }
            catch (Exception e)
            {
                string msg = e.Message;
                text = "";

            }
            return text;
        }

        public static string GetDBConnection(bool isCloud)
        {
            string path = "";
            string connection = "";
            string env = "";
            try
            {
                path = Directory.GetCurrentDirectory();
                if (isCloud)
                {
                    env = "AWSDB";
                }
                else {
                    env = "LocalDB";


                }
                //var builder = new ConfigurationBuilder().SetBasePath(path).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                //connection = builder.Build().GetSection("ConnectionStrings").GetSection(env).Value;
                connection = _dbQueries[env];
            }
            catch (Exception e)
            {
                string msg = e.Message;
                connection = "";
                
            }

            return connection;
        
        }

        public static string GetInsertQuery(bool isRegistered)
        {
            string text = "";
            //string path = "";
            try 
            {
                //path = Directory.GetCurrentDirectory();
                //var builder = new ConfigurationBuilder().SetBasePath(path).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                //text = builder.Build().GetSection("SQLProcs").GetSection("StoredProc").Value;
                if (isRegistered) { text = _dbQueries["RegisterEmpStoredProc"]; }
                else { text = _dbQueries["InsertEmpStoredProc"]; }
                

            }
            catch (Exception e)
            {
                string msg = e.Message;
                text = "";

            }
            return text;
        }

        public static string GetVoucherInsertQuery()
        {
            string text = "";
            //string path = "";
            try
            {

                text = _dbQueries["VoucherInsertProc"]; 

            }
            catch (Exception e)
            {
                string msg = e.Message;
                text = "";

            }
            return text;
        }
        public static string GetCreateSessionQuery()
        {
            string text = "";
            //string path = "";
            try
            {

                text = _dbQueries["CreateSessionProc"];

            }
            catch (Exception e)
            {
                string msg = e.Message;
                text = "";

            }
            return text;
        }

        public static string GetLedgerInsertQuery()
        {
            string text = "";
            //string path = "";
            try
            {

                text = _dbQueries["LedgerInsertProc"];

            }
            catch (Exception e)
            {
                string msg = e.Message;
                text = "";

            }
            return text;
        }




    }
}
