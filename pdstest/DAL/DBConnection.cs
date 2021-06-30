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
            // sqllib["LocalDB"] = @"server=localhost;database=PDS;userid=sa;password=12345;";                ////"Data Source=.;Initial Catalog=PDS;Integrated Security=True";
            sqllib["LocalDB"] = @"server=localhost;database=PDS;userid=root;password=12345;";
            sqllib["ProdDB"] = @"server=localhost;database=pdsmain;userid=root;password=12345;";
            sqllib["GetUserTypes"] = "select ConstantId, ConstantName,Category,ConstantValue from constants where IsActive = 1";
            sqllib["InsertEmpStoredProc"] = "usp_InsertEmployee";
            sqllib["InsertCDAEmpStoredProc"] = "usp_InsertCDAEmployee";
            sqllib["InsertMainEmpStoredProc"] = "usp_InsertMainEmployee";
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
        public static string GetParametersFromStoreProc(string procName)
        {
            string text = "";

            try
            {
                //var builder = new ConfigurationBuilder().SetBasePath(path).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                text = string.Format("SELECT PARAMETER_MODE, PARAMETER_NAME, DATA_TYPE, CHARACTER_MAXIMUM_LENGTH " +
                                     " FROM information_schema.parameters WHERE SPECIFIC_NAME = '{0}';", procName);

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

        public static string GetRegisteredUser(int empId)
        {
            string text = "";

            try
            {
                //var builder = new ConfigurationBuilder().SetBasePath(path).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
   
                    text = string.Format("select * from Employees where IsActive = 1 and EmployeeId = {0}", empId);

            }
            catch (Exception e)
            {
                string msg = e.Message;
                text = "";

            }
            return text;
        }
        public  static Dictionary<string, string> GetRecordsforPagination(int stationId,string table,string vstartDate,string vEndDate="", int? page=1, int? pagesize=5, string status="",bool isEmployee=false,int currentMonth=0)
        {
            /*SELECT* FROM Constants
                        LIMIT 10/*range , 5/*pagesize ;
            event_date BETWEEN '2018-01-01 12:00:00' AND '2018-01-01 23:30:00';*/
            DateTime dt = DateTime.Now.GetIndianDateTimeNow();
            DateTime dt2 = DateTime.Now.GetIndianDateTimeNow();
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
                    else if (table.ToLower() == "ledger" && currentMonth > 0)
                    {
   //                     text.Add("main", string.Format("SELECT * FROM FinanceLedger where StationId = {0} AND ((MONTH(CreditDate) = MONTH('{1}') " +
   //" AND YEAR(CreditDate) = YEAR('{1}')) OR (MONTH(VoucherDate) = MONTH('{1}') AND YEAR(VoucherDate) = YEAR('{1}'))) AND IsActive = 1 LIMIT {2},{3};", stationId, cdat, range, ps));
                        DateTime dtt = DateTime.Now;
                        dtt = dtt.GetIndianDateTimeNow();
                        DateTime dtvalue = new DateTime(dtt.Year, currentMonth, 1);
                        string cdat = dtvalue.DateTimetoString();
                        text.Add("main", string.Format("SELECT * FROM FinanceLedger where StationId = {0} AND ((MONTH(CreditDate) = MONTH('{1}') " +
                    " AND YEAR(CreditDate) = YEAR('{1}')) OR (MONTH(VoucherDate) = MONTH('{1}') AND YEAR(VoucherDate) = YEAR('{1}'))) AND IsActive = 1 LIMIT {2},{3};", stationId, cdat, range, ps));
                        text.Add("count", string.Format("SELECT COUNT(*) FROM FinanceLedger where StationId = {0} AND ((MONTH(CreditDate) = MONTH('{1}') " +
                    " AND YEAR(CreditDate) = YEAR('{1}')) OR (MONTH(VoucherDate) = MONTH('{1}') AND YEAR(VoucherDate) = YEAR('{1}'))) AND IsActive = 1 ;", stationId, cdat));
                    }
                    else if (table.ToLower() == "ledgerreport")
                    {
                        //                     text.Add("main", string.Format("SELECT * FROM FinanceLedger where StationId = {0} AND ((MONTH(CreditDate) = MONTH('{1}') " +
                        //" AND YEAR(CreditDate) = YEAR('{1}')) OR (MONTH(VoucherDate) = MONTH('{1}') AND YEAR(VoucherDate) = YEAR('{1}'))) AND IsActive = 1 LIMIT {2},{3};", stationId, cdat, range, ps));
                        string cdtxt = string.Format("select * FROM FinanceLedger where StationId = {0} " +
                                           "AND( (VoucherDate BETWEEN '{1}' AND '{2}') OR " +
                                           " (MONTH(CreditDate) = MONTH('{1}') AND YEAR(CreditDate) = YEAR('{1}') ) " +
                                           " ) AND IsActive = 1; ", stationId, vstartDate, vEndDate);
                        string countText = string.Format("select COUNT(*) FROM FinanceLedger where StationId = {0} " +
                  "AND( (VoucherDate BETWEEN '{1}' AND '{2}') OR " +
                  " (MONTH(CreditDate) = MONTH('{1}') AND YEAR(CreditDate) = YEAR('{1}') ) " +
                  " ) AND IsActive = 1; ", stationId, vstartDate, vEndDate);
                        text.Add("main", cdtxt);
                        text.Add("count", countText);
                    }
                    else if (table.ToLower() == "ledgerreport" && currentMonth > 0)
                    {
                        //                     text.Add("main", string.Format("SELECT * FROM FinanceLedger where StationId = {0} AND ((MONTH(CreditDate) = MONTH('{1}') " +
                        //" AND YEAR(CreditDate) = YEAR('{1}')) OR (MONTH(VoucherDate) = MONTH('{1}') AND YEAR(VoucherDate) = YEAR('{1}'))) AND IsActive = 1 LIMIT {2},{3};", stationId, cdat, range, ps));
                        DateTime dtt = DateTime.Now;
                        dtt = dtt.GetIndianDateTimeNow();
                        DateTime dtvalue = new DateTime(dtt.Year, currentMonth, 1);
                        string cdat = dtvalue.DateTimetoString();
                        text.Add("main", string.Format("SELECT * FROM FinanceLedger where StationId = {0} AND ((MONTH(CreditDate) = MONTH('{1}') " +
                    " AND YEAR(CreditDate) = YEAR('{1}')) OR (MONTH(VoucherDate) = MONTH('{1}') AND YEAR(VoucherDate) = YEAR('{1}'))) AND IsActive = 1;", stationId, cdat));
                        text.Add("count", string.Format("SELECT COUNT(*) FROM FinanceLedger where StationId = {0} AND ((MONTH(CreditDate) = MONTH('{1}') " +
                    " AND YEAR(CreditDate) = YEAR('{1}')) OR (MONTH(VoucherDate) = MONTH('{1}') AND YEAR(VoucherDate) = YEAR('{1}'))) AND IsActive = 1 ;", stationId, cdat));
                    }
                    else if (table.ToLower() == "ledger" && !string.IsNullOrEmpty(vstartDate) && !string.IsNullOrEmpty(vEndDate))
                    {
                        string cdtxt = string.Format("select * FROM FinanceLedger where StationId = {0} " +
                    "AND( (VoucherDate BETWEEN '{1}' AND '{2}') OR " +
                    " (MONTH(CreditDate) = MONTH('{1}') AND YEAR(CreditDate) = YEAR('{1}') ) " +
                    " ) AND IsActive = 1 LIMIT {3},{4}; ", stationId,vstartDate,vEndDate,range, ps);
                        string countText = string.Format("select COUNT(*) FROM FinanceLedger where StationId = {0} " +
                  "AND( (VoucherDate BETWEEN '{1}' AND '{2}') OR " +
                  " (MONTH(CreditDate) = MONTH('{1}') AND YEAR(CreditDate) = YEAR('{1}') ) " +
                  " ) AND IsActive = 1; ", stationId, vstartDate, vEndDate);
                        text.Add("main",cdtxt);
                        text.Add("count",countText);
                    }
                    else if (table.ToLower() == "daemployees")
                    {
                        text.Add("main", string.Format("SELECT * FROM CDAEmployees where StationId = {0} AND PID = {1} AND IsActive = 1 LIMIT {2},{3};", stationId, 3, range, ps));
                        text.Add("count", string.Format("SELECT COUNT(*) FROM CDAEmployees where StationId = {0} AND PID = {1} AND IsActive = 1 ;", stationId, 3));

                    }                   
                    else if (table.ToLower() == "pdsemployees")
                    {
                        text.Add("main", string.Format("SELECT * FROM PDSEmployees where StationId = {0}  AND IsActive = 1 LIMIT {1},{2};", stationId, range, ps));
                        text.Add("count", string.Format("SELECT COUNT(*) FROM PDSEmployees where StationId = {0}  AND IsActive = 1 ;", stationId));

                    }
                    else if (table.ToLower() == "pdsunemployees")
                    {
                        text.Add("main", string.Format("SELECT * FROM PDSEmployees where StationId = {0}  AND IsActive = 0 LIMIT {1},{2};", stationId, range, ps));
                        text.Add("count", string.Format("SELECT COUNT(*) FROM PDSEmployees where StationId = {0}  AND IsActive = 0 ;", stationId));

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
        public static string CheckUserforRegistration(RegisterEmployee emp)
        {
            string text = "";

            try
            {
                text = text = string.Format("select COUNT(*) from register where FirstName='{0}' and Phone = '{1}' and StationId = {2} and UserTypeId = {3};", emp.FirstName,emp.Phone,emp.StationId, emp.UserTypeId);

            }
            catch (Exception e)
            {
                string msg = e.Message;
                text = "";

            }
            return text;
        }
        public static string ClearInactiveSessions()
        {
            string text = "";
            DateTime dt = DateTime.Now;
            try
            {
                dt = dt.GetIndianDateTimeNow();
               string d = dt.DateTimetoString();
                text = string.Format("Select COUNT(*) from UserSessions where  '{0}'  not between StartDate and EndDate;",d);

            }
            catch (Exception e)
            {
                string msg = e.Message;
                text = "";

            }
            return text;
        }
        public static string GetColumnsForExcelfile()
        {
            string text = "";
            try
            {
                text = string.Format("Select * from EmpExcelTemplate where  IsActive = 1;");

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
                DateTime dtt = DateTime.Now;
                dtt = dtt.GetIndianDateTimeNow();
                string currentDate = dtt.DateTimetoString();
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
        public static string ApproveVoucher(int voucherId, string status)
        {
            string text = "";

            try
            {
                //var builder = new ConfigurationBuilder().SetBasePath(path).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                if(status.ToLower() == "r")
                    text = string.Format("update  Voucher SET IsApproved=0 ,VoucherStatus='R' where  VoucherId = {0}", voucherId);
                else
                    text = string.Format("update  Voucher SET IsApproved=1 ,VoucherStatus='A' where  VoucherId = {0}", voucherId);

            }
            catch (Exception e)
            {
                string msg = e.Message;
                text = "";

            }
            return text;
        }
        public static string GetTotalDebitamountinPreviousMonthForVoucher(int stationId)
        {
            string text = "";

            try
            {
                text = string.Format("select 0 as c,coalesce(SUM(Debit),0) as DebitAmount from FinanceLedger " +
                    " WHERE StationId = {0} AND MONTH(VoucherDate) = MONTH(CURRENT_DATE - INTERVAL 1 MONTH) " +
                    " AND YEAR(VoucherDate) = YEAR(CURRENT_DATE - INTERVAL 1 MONTH) AND Debit IS NOT NULL " +
                    " AND VoucherDate IS NOT NULL  AND IsActive = 1 Group BY StationId; ", stationId);

            }
            catch (Exception e)
            {
                string msg = e.Message;
                text = "";

            }
            return text;
        }
        public static string GetTotalCreditamountinPreviousMonthForVoucher(int stationId)
        {
            string text = "";

            try
            {
                text = string.Format("select count(Credit),ifnull(Credit, 0) as CreditAmount from FinanceLedger " +
                    " WHERE StationId = {0} AND MONTH(CreditDate) = MONTH(CURRENT_DATE - INTERVAL 1 MONTH) " +
                    " AND YEAR(CreditDate) = YEAR(CURRENT_DATE - INTERVAL 1 MONTH) AND Credit IS NOT NULL " +
                    " AND CreditDate IS NOT NULL  AND IsActive = 1; ", stationId);

            }
            catch (Exception e)
            {
                string msg = e.Message;
                text = "";

            }
            return text;
        }
        public static string GetCreditamountinCurrentMonthForVoucher(int stationId,string voucherDate)
        {
            string text = "";

            try
            {
                text = string.Format("select count(Credit),ifnull(Credit, 0) as CreditAmount from FinanceLedger " +
                    " WHERE StationId = {0} AND MONTH(CreditDate) = MONTH('{1}') "+
                    " AND YEAR(CreditDate) = YEAR('{1}') AND Credit IS NOT NULL " +
                    " AND CreditDate IS NOT NULL AND IsActive = 1; ", stationId,voucherDate);

            }
            catch (Exception e)
            {
                string msg = e.Message;
                text = "";

            }
            return text;
        }
        public static string GetTotalDebitamountinCurrentMonthForVoucher(int stationId, string voucherDate)
        {
            string text = "";

            try
            {
                text = string.Format("select 0 as c,coalesce(SUM(Debit),0) as DebitAmount from FinanceLedger " +
                    " WHERE StationId = {0} AND MONTH(VoucherDate) = MONTH('{1}') " +
                    " AND YEAR(VoucherDate) = YEAR('{1}') AND VoucherDate <= '{1}' AND Debit IS NOT NULL " +
                    " AND VoucherDate IS NOT NULL AND IsActive = 1 Group BY StationId; ", stationId, voucherDate);

            }
            catch (Exception e)
            {
                string msg = e.Message;
                text = "";

            }
            return text;
        }
        public static string CheckforCreditintoStation(int stationId, string date)
        {
            string text = "";

            try
            {
                text = text = string.Format("select  COUNT(*) from FinanceLedger WHERE StationId = {0} AND MONTH(CreditDate) = MONTH('{1}') AND YEAR(CreditDate) = YEAR('{1}') AND Credit IS NOT NULL AND IsActive = 1;", stationId, date);

            }
            catch (Exception e)
            {
                string msg = e.Message;
                text = "";

            }
            return text;
        }
        public static string CheckVoucherExists(string voucherNumber,bool isCreditValidate=false,int stationId=0,string voucherDate="")
        {
            string text = "";

            try
            {
                if (isCreditValidate)
                {
                    text = string.Format("select  COUNT(*) from FinanceLedger WHERE StationId = {0} AND MONTH(CreditDate) = MONTH('{1}') AND YEAR(CreditDate) = YEAR('{1}') AND Credit IS NOT NULL AND CreditDate IS NOT NULL AND IsActive = 1;", stationId, voucherDate);
                }
                else
                {
                    if (stationId == 0) //AND MONTH(CreditDate) = MONTH(NEW.VoucherDate)
                        text = string.Format("select COUNT(*) from Voucher where VoucherNumber = '{0}';", voucherNumber);
                    else if (!string.IsNullOrEmpty(voucherDate))
                    {
                        //DateTime d = voucherDate.StringtoDateTime();
                        text = string.Format("select COUNT(*) from Voucher where MONTH(VoucherDate) = MONTH('{0}') AND YEAR(VoucherDate) = YEAR('{0}') AND StationId = {1} AND IsApproved = 0;", voucherDate, stationId);
                    }
                }
                        
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
        public static string CheckConstantforStation(int stationId)
        {
            string text = "";

            try
            {
                text = string.Format("select COUNT(*) from commercialconstants where StationId = {0};", stationId);

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
        public static string CheckMainEmpCodeExists(string empCode)
        {
            string text = "";

            try
            {
                    text = string.Format("select COUNT(*) from PDSEmployees where EmpCode = '{0}';", empCode);
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
        public static string GetLoginUserInfo(int usertypeId, int employeeId)
        {
            string text = "";

            try
            {
                text = string.Format("SELECT EmployeeId,UserTypeId,LoginType,FirstName,UserName,StationId FROM employees where UserTypeId = {0} AND EmployeeId = {1} AND IsActive=1 LIMIT 1;", usertypeId, employeeId);

            }
            catch (Exception e)
            {
                string msg = e.Message;
                text = "";

            }
            return text;
        }
        public static string ResetPassword(string  password,int employeeId)
        {
            string text = "";

            try
            {

                //DateTime StartDate = SessionStartDate.StringtoDateTime();
                //DateTime EndDate = SessionEndDate.StringtoDateTime();

                text = string.Format("UPDATE employees Set Passwrd='{0}'  where  EmployeeId = {1} AND IsActive=1;", password, employeeId);

            }
            catch (Exception e)
            {
                string msg = e.Message;
                text = "";

            }
            return text;
        }
        public static string SessionUpdate(UserType usr)
        {
            string text = "";

            try
            {
                DateTime dtt = DateTime.Now;
                dtt = dtt.GetIndianDateTimeNow();
                string SessionStartDate = dtt.DateTimetoString();
                string SessionEndDate = dtt.AddMinutes(20).DateTimetoString();

                //DateTime StartDate = SessionStartDate.StringtoDateTime();
                //DateTime EndDate = SessionEndDate.StringtoDateTime();

                text = string.Format("UPDATE UserSessions Set Token='{0}',StartDate='{1}',EndDate='{2}'   where UserTypeId = {3} AND EmployeeId = {4} AND IsActive=1;", usr.Token, SessionStartDate, SessionEndDate, usr.UserTypeId, usr.EmployeeId);

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
                    env = "ProdDB";
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
                text = string.Format("DELETE from CDADelivery Where StationId = {0} AND CurrentMonth = {1} AND EmployeeId={2} AND IsActive=1;", dd.StationId,dd.CurrentMonth,dd.EmployeeId);
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
                text = string.Format("SELECT COUNT(*) from CDADelivery Where StationId = {0} AND CurrentMonth = {1} AND EmployeeId={2} AND IsActive=1;", dd.StationId, dd.CurrentMonth, dd.EmployeeId);
            }
            catch
            {
                text = "";
            }
            return text;
        }
        public static string GetEmpIds(int currentMonth, int stationId)
        {
            string text = "";
            try
            {
                text = string.Format("SELECT * from CDADelivery WHERE CurrentMonth={0} AND StationId={1};", currentMonth, stationId);
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
                text = string.Format("SELECT StationId,EmployeeId,FirstName,LastName,Phone,Address1,Address2,CDACode,PAN from CDAEmployees WHERE EmployeeId={0};", employeeId);
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
                text = string.Format("SELECT * from CDADelivery WHERE EmployeeId={0} AND CurrentMonth={1} AND StationId={2};", employeeId,currentMonth,stationId);
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
                d = d.GetIndianDateTimeNow();
                string idate = d.DateTimetoString();
                StringBuilder insertCmd = new StringBuilder();
                insertCmd.Append("Insert into CDADelivery(StationId,CurrentMonth,DeliveryCount,DeliveryRate,PetrolAllowanceRate,EmployeeId,Incentives,TotalAmount,CreatedDate,IsActive) ");
                insertCmd.AppendLine(" VALUES(");
                insertCmd.Append(cdd.StationId + ",");
                insertCmd.Append(cdd.CurrentMonth + ",");
                insertCmd.Append(cdd.DeliveryCount + ",");
                insertCmd.Append(cdd.DeliveryRate + ",");
                insertCmd.Append(cdd.PetrolAllowance + ",");
                insertCmd.Append(cdd.EmployeeId + ",");
                insertCmd.Append(cdd.Incentive + ",");
                insertCmd.Append(cdd.TotalAmount + ","); 
                insertCmd.Append("'"+idate + "',");
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
                text = string.Format("Select * from CommercialConstants Where StationId = {0} AND IsActive=1;", StationId);
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
                text = string.Format("Select * from CDADelivery Where StationId = {0} AND CurrentMonth = {1} AND EmployeeId ={2} AND IsActive=1;", StationId, currentMonth, employeeId);
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
        
        public static string GetMainEmployeeInsertQuery()
        {
            string text = "";
            //string path = "";
            try
            {
                //path = Directory.GetCurrentDirectory();
                //var builder = new ConfigurationBuilder().SetBasePath(path).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                //text = builder.Build().GetSection("SQLProcs").GetSection("StoredProc").Value;

                text = _dbQueries["InsertMainEmpStoredProc"];


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
        public static string GetVoucherDetailsbyVoucherNumberQuery(int voucherId)
        {
            string text = "";
            //string path = "";
            try
            {

                text = string.Format("Select * from Voucher where VoucherId = {0}; ", voucherId);

            }
            catch (Exception e)
            {
                string msg = e.Message;
                text = "";

            }
            return text;

        }
        public static string GetVoucherUpdateQuery(Voucher vouch)
        {
            string text = "";
            //string path = "";
            try
            {

                text = string.Format("UPDATE voucher SET PurposeOfPayment = '{0}',PartyName='{1}',VoucherDate='{2}',NetAmount={3} " +
                    " , TotalAmount={4},TaxAmount={5},VoucherStatus='P',IsApproved=0  WHERE StationId = {6} AND VoucherNumber = '{7}'; "
                    ,vouch.PurposeOfPayment,vouch.PartyName,vouch.V_Date,vouch.NetAmount,vouch.TotalAmount,vouch.TaxAmount,vouch.StationId,vouch.VoucherNumber);

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

        public static List<Tuple<string,string>> GetProcParameters()
        {
            List<Tuple<string, string>> procparams = new List<Tuple<string, string>>();
            try
            {
                Tuple<string, string> tup = Tuple.Create("@ReportingManager", "50");
                procparams.Add(tup);
                tup = Tuple.Create("ReportingManagerEmpCode", "50");
                procparams.Add(tup);
                tup = Tuple.Create("PfFundName", "50");
                procparams.Add(tup);
                tup = Tuple.Create("PfMembershipNumber", "50");
                procparams.Add(tup);
                tup = Tuple.Create("PfDOJFund", "50");
                procparams.Add(tup);
                tup = Tuple.Create("UAN", "50");
                procparams.Add(tup);
                tup = Tuple.Create("ESICFundId", "50");
                procparams.Add(tup);
                tup = Tuple.Create("ESICMemberShipNumber", "50");
                procparams.Add(tup);
                tup = Tuple.Create("ESICDOJFund", "50");
                procparams.Add(tup);
                tup = Tuple.Create("EmployeeCategory", "50");
                procparams.Add(tup);
                tup = Tuple.Create("ShiftDetails", "50");
                procparams.Add(tup);
                tup = Tuple.Create("LeavePolicy", "50");
                procparams.Add(tup);
                tup = Tuple.Create("AttendancePolicy", "50");
                procparams.Add(tup);
                tup = Tuple.Create("OverTimeLogic", "50");
                procparams.Add(tup);
                tup = Tuple.Create("OverTimeLogicPayout", "50");
                procparams.Add(tup);
                tup = Tuple.Create("LateInEarlyOut", "50");
                procparams.Add(tup);
                tup = Tuple.Create("WeeklyOff", "50");
                procparams.Add(tup);
                tup = Tuple.Create("DayOff", "50");
                procparams.Add(tup);
                tup = Tuple.Create("HolidayList", "50");
                procparams.Add(tup);
                tup = Tuple.Create("AttendanceCycle", "50");
                procparams.Add(tup);
                tup = Tuple.Create("ApprovalHierarchy", "50");
                procparams.Add(tup);
                tup = Tuple.Create("ClosingLeaveBalances", "50");
                procparams.Add(tup);
                tup = Tuple.Create("DAPeopleSoftId", "50");
                procparams.Add(tup);
                tup = Tuple.Create("PSC", "50");
                procparams.Add(tup);
                tup = Tuple.Create("FromDate", "50");
                procparams.Add(tup);
                tup = Tuple.Create("ToDate", "50");
                procparams.Add(tup);
                tup = Tuple.Create("EmployeeStatus", "50");
                procparams.Add(tup);
                tup = Tuple.Create("ICPaymentMethod", "50");
                procparams.Add(tup);
                tup = Tuple.Create("PaymentPerUnit", "50");
                procparams.Add(tup);
                tup = Tuple.Create("PayFrequency", "50");
                procparams.Add(tup);
                tup = Tuple.Create("RegularPayRateDesc", "50");
                procparams.Add(tup);
                tup = Tuple.Create("BlockARate", "50");
                procparams.Add(tup);
                tup = Tuple.Create("BlockBRate", "50");
                procparams.Add(tup);
                tup = Tuple.Create("PackagesDelivered", "50");
                procparams.Add(tup);
                tup = Tuple.Create("ApplicablePayRate", "50");
                procparams.Add(tup);
            }
            catch 
            {
               // string msg = e.Message;
               // text = "";

            }
            return procparams;


        }

        public static string GetErrorLogInsertQuery(ErrorLogTrack err)
        {
            string cmdText = "";
            try
            {
                DateTime d = DateTime.Now;
                d = d.GetIndianDateTimeNow();
                string idate = d.DateTimetoString();
                StringBuilder insertCmd = new StringBuilder();
                insertCmd.Append("Insert into ErrorLog(ServiceName,MethodName,Reason,CommandType,Zone,CreatedDate) ");
                insertCmd.AppendLine(" VALUES(");
                insertCmd.Append("'" + err.ServiceName.ToString() + "',");
                insertCmd.Append("'" + err.MethodName.ToString() + "',");
                insertCmd.Append("'" + err.Reason.ToString() + "',");
                insertCmd.Append("'" + err.CommandType.ToString() + "',");
                insertCmd.Append("'" + err.Zone.ToString() + "',");
                insertCmd.Append("'" + idate + "')");
                cmdText = insertCmd.ToString();

            }
            catch
            {
                cmdText = "";
            }
            return cmdText;
        }

    }
}
