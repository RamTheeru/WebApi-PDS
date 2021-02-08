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
using System.Text;

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
            sqllib["GetUserTypes"] = "select ConstantId, ConstantName,Category,ConstantValue from constants where IsActive = 1";
            sqllib["InsertEmpStoredProc"] = "usp_InsertEmployee";
            sqllib["InsertCDAEmpStoredProc"] = "usp_InsertCDAEmployee";
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
        public static List<Dictionary<string, string>> GetAdminDetails()
        {
            List<Dictionary<string, string>> result = new List<Dictionary<string, string>>();

            try
            {
                Dictionary<string, string> text = new Dictionary<string, string>();
                //var builder = new ConfigurationBuilder().SetBasePath(path).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                text["loginrequests"] = "SELECT COUNT(*) FROM register WHERE IsActive = 0";
                result.Add(text);

            }
            catch (Exception e)
            {
                string msg = e.Message;
                result = new List<Dictionary<string, string>>();

            }
            return result;
        }

        public static string GetRegisteredUsers(int stationId)
        {
            string text = "";

            try
            {
                //var builder = new ConfigurationBuilder().SetBasePath(path).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                if (stationId > 0)
                    text = string.Format("select * from register where IsActive = 0 and StationId = {0}", stationId);
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
                    else if (table.ToLower() == "daemployees")
                    {
                        text.Add("main", string.Format("SELECT * FROM CDAEmployees where StationId = {0} AND PID = {1} AND IsActive = 1 LIMIT {2},{3};", stationId, 3, range, ps));
                        text.Add("count", string.Format("SELECT COUNT(*) FROM CDAEmployees where StationId = {0} AND PID = {1} AND IsActive = 1 ;", stationId, 3));

                    }
                    else if (table.ToLower() == "getcdadel")
                    {
                        text.Add("main", string.Format("SELECT * FROM CDAEmployees where StationId = {0} AND PID = {1} AND IsActive = 1 LIMIT {2},{3};", stationId, 3, range, ps));
                        text.Add("count", string.Format("SELECT COUNT(*) FROM CDAEmployees where StationId = {0} AND PID = {1} AND IsActive = 1 ;", stationId, 3));

                    }                   
                    else if (table.ToLower() == "logins")
                    {
                        text.Add("main", string.Format("SELECT * FROM employees where StationId = {0} AND LoginType IS NOT NULL AND UpdateByTrigger = 1 LIMIT {1},{2};", stationId, range, ps));
                        text.Add("count", string.Format("SELECT COUNT(*) FROM employees where StationId = {0} AND LoginType IS NOT NULL AND UpdateByTrigger = 1 ;", stationId, isEmployee));

                    }
                    else if (table.ToLower() == "register")
                    {
                        text.Add("main", string.Format("SELECT * FROM register where StationId = {0}  AND IsActive = 0 LIMIT {2},{3};", stationId, isEmployee, range, ps));
                        text.Add("count", string.Format("SELECT COUNT(*) FROM register where StationId = {0}  AND IsActive = 0 ;", stationId, isEmployee));

                    }

                }
           

            }
            catch (Exception e)
            {
                string msg = e.Message;
                text = new Dictionary<string, string>();
                text.Add("main", "");
                text.Add("count", "");

            }
            return text;
        }
        public static string ClearInactiveSessions()
        {
            string text = "";
            try
            {
                text = "Select COUNT(*) from UserSessions where  Now()  not between StartDate and EndDate;";

            }
            catch (Exception e)
            {
                string msg = e.Message;
                text = "";

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
        public static string CheckEmpCodeExists(string empCode,bool isEmployee)
        {
            string text = "";

            try
            {
                if(isEmployee)
                    text = string.Format("select COUNT(*) from employees where EmpCode = '{0}';", empCode);
                else
                    text = string.Format("select COUNT(*) from CDAEmployees where CDACode = '{0}';", empCode);
            }
            catch (Exception e)
            {
                string msg = e.Message;
                text = "";

            }
            return text;
        }
        public static string GetSessionDetails(string userName,int employeeId, int userTypeId)
        {
            string text = "";

            try
            {
                text = string.Format("SELECT COUNT(*) FROM UserSessions where EmployeeId = {0} AND UserTypeId = {1} AND UserName='{2}' AND IsActive=1 LIMIT 1;", employeeId, userTypeId,userName);

            }
            catch (Exception e)
            {
                string msg = e.Message;
                text = "";

            }
            return text;
        }
        public static string DeleteSession(string userName, int employeeId, int userTypeId)
        {
            string text = "";

            try
            {
                text = string.Format("DELETE FROM UserSessions where EmployeeId = {0} AND UserTypeId = {1} AND UserName='{2}' AND IsActive=1;", employeeId, userTypeId, userName);

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
                    text = string.Format("SELECT EmployeeId,UserTypeId,LoginType,FirstName,UserName,StationId FROM employees where UserName = '{0}' AND Passwrd = '{1}' AND IsActive=1 LIMIT 1;", username,password);

            }
            catch (Exception e)
            {
                string msg = e.Message;
                text = "";

            }
            return text;
        }
        public static string ApproveUser(int registerId,string status)
        {
            string text = "";

            try
            {
                //var builder = new ConfigurationBuilder().SetBasePath(path).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                if(status.ToLower() == "a")
                    text = string.Format("UPDATE register SET IsActive = 1 WHERE RegisterId = {0}", registerId);
                else
                    text = string.Format("DELETE FROM register WHERE RegisterId = {0}", registerId);

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
        public static string GetDeleteDeliveryDetailCDAforEmployeeCurrentMonth(DeliveryDetails dd)
        {
            string text = "";
            try
            {
                text = string.Format("DELETE from CDADelivery Where StationId = {0} AND CurrentMonth = {1} AND EmployeeId={2} AND IsActive=1", dd.StationId,dd.CurrentMonth,dd.EmployeeId);
            }
            catch
            {
                text = "";
            }
            return text;
        }
        public static string GetCHECKDeliveryDetailCDAforEmployeeCurrentMonth(DeliveryDetails dd)
        {
            string text = "";
            try
            {
                text = string.Format("SELECT COUNT(*) from CDADelivery Where StationId = {0} AND CurrentMonth = {1} AND EmployeeId={2} AND IsActive=1", dd.StationId, dd.CurrentMonth, dd.EmployeeId);
            }
            catch
            {
                text = "";
            }
            return text;
        }
        public static string GetEmployeedatabyEmpId(int employeeId)
        {
            string text = "";
            try
            {
                text = string.Format("SELECT FirstName,LastName,Phone,Address1,Address2,CDACode,PAN from CDAEmployees WHERE EmployeeId={0}", employeeId);
            }
            catch
            {
                text = "";
            }
            return text;
        }
        public static string GetEmployeedeliverydetailsbyEmpId(int employeeId,int currentMonth,int stationId)
        {
            string text = "";
            try
            {
                text = string.Format("SELECT * from CDADelivery WHERE EmployeeId={0} AND CurrentMonth={1} AND StationId={2}", employeeId,currentMonth,stationId);
            }
            catch
            {
                text = "";
            }
            return text;
        }
        public static string GetUpdateDeiverydetailInsertQuery(DeliveryDetails cdd)
        {
            string cmdText = "";
            try
            {
                DateTime d = DateTime.Now;
                StringBuilder insertCmd = new StringBuilder();
                insertCmd.Append("Insert into CDADelivery(StationId,CurrentMonth,DeliveryCount,DeliveryRate,PetrolAllowanceRate,EmployeeId,Incentives,TotalAmount,IsActive) ");
                insertCmd.AppendLine(" VALUES(");
                insertCmd.Append(cdd.StationId.ToString() + ",");
                insertCmd.Append(cdd.CurrentMonth.ToString() + ",");
                insertCmd.Append(cdd.DeliveryCount.ToString() + ",");
                insertCmd.Append(cdd.DeliveryRate.ToString() + ",");
                insertCmd.Append(cdd.PetrolAllowance.ToString() + ",");
                insertCmd.Append(cdd.EmployeeId.ToString() + ",");
                insertCmd.Append(cdd.Incentive.ToString() + ",");
                insertCmd.Append(cdd.TotalAmount.ToString() + ",");
                insertCmd.Append("1 )");
                cmdText = insertCmd.ToString();

            }
            catch
            {
                cmdText = "";
            }
            return cmdText;
        }
        public static string GetDeliveryDetailCDAbyStation(int StationId)
        {
            string text = "";
            try
            {
                text = string.Format("Select * from CommercialConstants Where StationId = {0} AND IsActive=1", StationId);
            }
            catch 
            {
                text = "";
            }
            return text;
        }
        public static string GetDeliveryDetailCDAbyMonth(int employeeId,int StationId,int currentMonth)
        {
            string text = "";
            try
            {
                text = string.Format("Select * from CDADelivery Where StationId = {0} AND CurrentMonth = {1} AND EmployeeId ={2} AND IsActive=1", StationId, currentMonth, employeeId);
            }
            catch 
            {
                text = "";
            }
            return text;
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

        public static string GetCDAInsertQuery()
        {
            string text = "";
            //string path = "";
            try
            {
                //path = Directory.GetCurrentDirectory();
                //var builder = new ConfigurationBuilder().SetBasePath(path).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                //text = builder.Build().GetSection("SQLProcs").GetSection("StoredProc").Value;
               
                 text = _dbQueries["InsertCDAEmpStoredProc"]; 


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
