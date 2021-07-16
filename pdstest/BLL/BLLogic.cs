using Microsoft.Extensions.Configuration;
using pdstest.DAL;
using pdstest.Models;
using pdstest.services;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExcelDataReader;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Reflection;

namespace pdstest.BLL
{
    public class BLLogic
    {
         //DBOperations ops = new DBOperations();
        private readonly IConnection ops;
        private readonly IConfiguration configuration;
        public BLLogic(IConnection conn, IConfiguration config)
        {
            ops = conn;
            configuration = config;
        }

        public APIResult GetConstants()
        {
            APIResult result = new APIResult();
            DataBaseResult dbr = new DataBaseResult();
            try 
            {
                dbr.ds = new System.Data.DataSet();
                result.Designations = new List<Designation>();
                result.Usertypes = new List<UserType>();
                result.stations = new List<Station>();
                result.professions = new List<Profession>();
                result.uploadStatus = new UploadStatus();
                result.uploadStatus.columns = new List<string>();
                dbr = ops.GetConstants();
                List<DropDown> dds = new List<DropDown>();
                int count = 0;
                StringBuilder build = new StringBuilder();
                count = dbr.ds.Tables[0].Rows.Count;
                if (count > 0)
                {
 
                    for (int i = 0; i < count; i++)
                    {
                        DropDown dd = new DropDown();
                         dd.Category = dbr.ds.Tables[0].Rows[i]["Category"].ToString();
                         dd.ConstantId = Convert.ToInt32(dbr.ds.Tables[0].Rows[i]["ConstantId"]);
                         dd.ConstantName = dbr.ds.Tables[0].Rows[i]["ConstantName"].ToString();
                        dd.ConstantValue = dbr.ds.Tables[0].Rows[i]["ConstantValue"].ToString();
                        dds.Add(dd);

                    }
                    if (dds.Count > 0)
                    {
                        int designationCount = dds.Where(x => x.Category.ToLower() == "designation").Count();
                        int userTypeCount = dds.Where(x => x.Category.ToLower() == "logintype").Count();
                        int stationCount = dds.Where(x => x.Category.ToLower() == "station").Count();
                        int professionCount = dds.Where(x => x.Category.ToLower() == "profession").Count(); 
                        if (designationCount > 0)
                        {
                            result.Designations = dds.Where(x => x.Category.ToLower() == "designation").Select(
                                a => new Designation { DesignationId = a.ConstantId, DesginationName = a.ConstantName }
                                ).ToList();
                            build.Append("Designations,");
                        }
                        if (userTypeCount > 0)
                        {
                            result.Usertypes = dds.Where(x => x.Category.ToLower() == "logintype").Select(
                            a => new UserType { UserTypeId = a.ConstantId, User = a.ConstantName,Role = a.ConstantValue }
                            ).ToList();
                            build.Append("UserTypes,");

                        }
                        if (professionCount > 0)
                        {
                            result.professions = dds.Where(x => x.Category.ToLower() == "profession").Select(
                            a => new Profession { Pid = a.ConstantId, ProfessionName = a.ConstantName }
                            ).ToList();
                            build.Append("Professions,");

                        }
                        if (stationCount>0)
                        {
                            result.stations = dds.Where(x => x.Category.ToLower() == "station").Select(
                                     a => new Station { StationId = a.ConstantId, StationName = a.ConstantName,StationCode = a.ConstantValue }
                                     ).ToList();
                            build.Append("Stations");
                        }
                        result.Message = build.ToString() + " : "+ dbr.Message;
                        //string category = item.Category;
                        //if (category.ToLower() == "logintype")
                        //{

                        //    UserType ut = new UserType();
                        //    ut.UserTypeId = item.ConstantId;
                        //    ut.User
                        //}
                        result.Status = dbr.Status;
                        result.CommandType = dbr.CommandType;

                    }
                    else 
                    {
                        result.Message = dbr.Message;
                        result.Status = dbr.Status;
                        result.CommandType = dbr.CommandType;

                    }
                    

                    

                }
                else if(count == 0)
                {
                    result.Message = dbr.Message;
                    result.Status = dbr.Status;
                    result.CommandType = dbr.CommandType;

                }
                List<Tuple<string, bool>> cols = new List<Tuple<string, bool>>();
                cols = this.GetColumnsForExcel();
                result.uploadStatus.columns = (from c in cols
                                           //where c.Item2 == true
                                       select c.Item1).ToList<string>();

            }
            catch(Exception e)
            {
                result.Message = e.Message;
                result.Status = false;
                result.CommandType = "Select";
                throw e;

            }
            return result;
        
        }

        public APIResult GetRegisteredUsers(int stationId)
        {
            APIResult result = new APIResult();
            DataBaseResult dbr = new DataBaseResult();
            try 
            {
                dbr.ds = new System.Data.DataSet();
                result.registerEmployees = new List<RegisterEmployee>();
                dbr = ops.GetRegisteredUsers(stationId);
                List<RegisterEmployee> regs = new List<RegisterEmployee>();
                int count = 0;
                count = dbr.ds.Tables[0].Rows.Count;
                if (count > 0)
                {
                    for (int i = 0; i < count; i++)
                    {
                        RegisterEmployee reg = new RegisterEmployee();
                        reg.RegisterId = Convert.ToInt32(dbr.ds.Tables[0].Rows[i]["RegisterId"]);
                        reg.FirstName = dbr.ds.Tables[0].Rows[i]["FirstName"].ToString();
                        reg.Phone =  dbr.ds.Tables[0].Rows[i]["Phone"].ToString();
                        reg.LoginType = dbr.ds.Tables[0].Rows[i]["LoginType"].ToString();
                        reg.Designation = dbr.ds.Tables[0].Rows[i]["Designation"].ToString();
                        reg.State = dbr.ds.Tables[0].Rows[i]["StateCode"].ToString();
                        reg.LocationName= dbr.ds.Tables[0].Rows[i]["LocationName"].ToString();
                        regs.Add(reg);

                    }
                    result.registerEmployees = regs;
                    result.Message = dbr.Message;
                    result.Status = dbr.Status;
                    result.CommandType = dbr.CommandType;
                }
                else 
                {
                    result.Message = dbr.Message;
                    result.Status = dbr.Status;
                    result.CommandType = dbr.CommandType;


                }



            }
            catch (Exception e)
            {
                result.Message = e.Message;
                result.Status = false;
                result.CommandType = "Select";
                throw e;

            }

            return result;
        }

        public Employee GetRegisteredUser(int empId)
        {
            //APIResult result = new APIResult();
            DataBaseResult dbr = new DataBaseResult();
            Employee reg = new Employee();
            try
            {
                dbr.ds = new System.Data.DataSet();
              //  result.registerEmployee = new RegisterEmployee();
                dbr = ops.GetRegisteredUser(empId);
               
                int count = 0;
                count = dbr.ds.Tables[0].Rows.Count;
                if (count > 0)
                {
                    for (int i = 0; i < count; i++)
                    {
                        string eId = dbr.ds.Tables[0].Rows[i]["EmployeeId"].ToString();
                        reg.EmployeeId = this.HandleStringtoInt(eId);
                        reg.FirstName = dbr.ds.Tables[0].Rows[i]["FirstName"].ToString();
                        reg.Phone = dbr.ds.Tables[0].Rows[i]["Phone"].ToString();
                        reg.LoginType = dbr.ds.Tables[0].Rows[i]["LoginType"].ToString();
                        //reg.Designation = dbr.ds.Tables[0].Rows[i]["Designation"].ToString();
                        reg.State = dbr.ds.Tables[0].Rows[i]["StateCode"].ToString();
                        reg.LocationName = dbr.ds.Tables[0].Rows[i]["LocationName"].ToString();
                        reg.Email = dbr.ds.Tables[0].Rows[i]["Email"].ToString();
                        // regs.Add(reg);

                    }
                    //result.registerEmployee = reg;
                    //result.Message = dbr.Message;
                    //result.Status = dbr.Status;
                    //result.CommandType = dbr.CommandType;
                }
                else
                {
                    //result.Message = dbr.Message;
                    //result.Status = dbr.Status;
                    //result.CommandType = dbr.CommandType;

                    reg = new Employee();
                }



            }
            catch 
            {
                reg = new Employee();
               // throw e;

            }

            return reg;
        }

        public APIResult GetPagnationRecords(APIInput input)
        {
            APIResult result = new APIResult();
            DataBaseResult dbr = new DataBaseResult();
            try
            {
                dbr.ds = new System.Data.DataSet();
                result.userInfo = new UserType();
                int ps = input.pagesize == null || input.pagesize == 0 ? 5 : Convert.ToInt32(input.pagesize);
                int p = input.page == null || input.page == 0 ? 1 : Convert.ToInt32(input.page);
                if(input.table.ToLower()== "ledger")
                {
                    dbr = ops.GetPaginationRecords(input.stationId, input.table, input.vstartDate, input.vEndDate, input.page, input.pagesize, input.status,false,input.currentmonth);
                }
                else
                    dbr = ops.GetPaginationRecords(input.stationId, input.table, input.vstartDate, input.vEndDate, input.page, input.pagesize, input.status);
                //if (input.page == 0)
                //    input.page = 1;
                //if (input.pagesize == 0)
                //    input.pagesize = 5;


                int count = 0;
                count = dbr.ds.Tables[0].Rows.Count;
                if (count > 0)
                {
                    if (input.table.ToLower() == "voucher")
                    {
                        result.vouchers = new List<Voucher>();
                        List<Voucher> vouchs = new List<Voucher>();
                        for (int i = 0; i < count; i++)
                        {
                            Voucher vouch = new Voucher();
                            vouch.IsApproved = Convert.ToBoolean(dbr.ds.Tables[0].Rows[i]["IsApproved"]);
                            int stationid = 0;
                            string sId = dbr.ds.Tables[0].Rows[i]["StationId"].ToString();
                            bool success = int.TryParse(sId, out stationid);
                            vouch.StationId = (success == true) ? stationid : 0;
                            int total = 0;
                            string tot = dbr.ds.Tables[0].Rows[i]["TotalAmount"].ToString();
                            success = int.TryParse(tot, out total);
                            vouch.TotalAmount = (success == true) ? total : 0;
                            int tax = 0;
                            string tx = dbr.ds.Tables[0].Rows[i]["TaxAmount"].ToString();
                            success = int.TryParse(tx, out tax);
                            vouch.TaxAmount = (success == true) ? tax : 0;
                            int net = 0;
                            string nt = dbr.ds.Tables[0].Rows[i]["NetAmount"].ToString();
                            success = int.TryParse(nt, out net);
                            vouch.NetAmount = (success == true) ? net : 0;
                            vouch.VoucherNumber = dbr.ds.Tables[0].Rows[i]["VoucherNumber"].ToString();
                            vouch.VoucherStatus = dbr.ds.Tables[0].Rows[i]["VoucherStatus"].ToString();
                            int voucherid = 0;
                            string vId = dbr.ds.Tables[0].Rows[i]["VoucherId"].ToString();
                            success = int.TryParse(vId, out voucherid);
                            vouch.VoucherId = (success == true) ? voucherid : 0;
                            DateTime vouch_Date;
                            string vDate = dbr.ds.Tables[0].Rows[i]["VoucherDate"].ToString();
                            success = DateTime.TryParse(vDate, out vouch_Date);
                            vouch.VoucherDate = (success == true) ? vouch_Date : new DateTime();
                            vouch.V_Date = vouch.VoucherDate.DateTimetoStringforView();
                            string part = dbr.ds.Tables[0].Rows[i]["PartyName"].ToString();
                            vouch.PartyName = string.IsNullOrEmpty(part) ? "" : part;
                            string pur = dbr.ds.Tables[0].Rows[i]["PurposeOfPayment"].ToString();
                            vouch.PurposeOfPayment = string.IsNullOrEmpty(pur) ? "" : pur;
                            vouchs.Add(vouch);

                        }
                        result.vouchers = vouchs;
                        result.QueryTotalCount = dbr.QueryTotalCount;
 
                    }
                    else if (input.table.ToLower() == "ledger" || input.table.ToLower() == "ledgerreport")
                    {
                        result.ledgers = new List<Ledger>();
                        List<Ledger> ledgs = new List<Ledger>();
                        for (int i = 0; i < count; i++)
                        {
                            Ledger ledg = new Ledger();
                           // ledg.IsApproved = Convert.ToBoolean(dbr.ds.Tables[0].Rows[i]["IsApproved"]);
                           // int stationid = 0;
                            string sId = dbr.ds.Tables[0].Rows[i]["StationId"].ToString();
                            //bool success = int.TryParse(sId, out stationid);
                            //ledg.StationId = (success == true) ? stationid : 0;
                            ledg.StationId = this.HandleStringtoInt(sId);                           
                            if(ledg.StationId > 0)
                            {
                                Tuple<string, string> st = System.Tuple.Create("", "");
                                st = ops.GetStationNameByStationId(ledg.StationId);
                                ledg.StationName = st.Item1;
                            }
                            else
                            {
                                ledg.StationName = "";
                            }
                          //  int debit = 0;
                            string deb = dbr.ds.Tables[0].Rows[i]["Debit"].ToString();
                            //success = int.TryParse(deb, out debit);
                            //ledg.Debit = (success == true) ? debit : 0;
                            ledg.Debit = this.HandleStringtoInt(deb);
                           // int credit = 0;
                            string cred = dbr.ds.Tables[0].Rows[i]["Credit"].ToString();
                            //success = int.TryParse(cred, out credit);
                            //ledg.Credit = (success == true) ? credit : 0;
                            ledg.Credit = this.HandleStringtoInt(cred);
                            string bal = dbr.ds.Tables[0].Rows[i]["Balance"].ToString();
                            ledg.Balance = this.HandleStringtoInt(bal);
                            string vn = dbr.ds.Tables[0].Rows[i]["VoucherNumber"].ToString();
                            ledg.VoucherNumber = string.IsNullOrEmpty(vn) ? "--" : vn;
                            string vs = dbr.ds.Tables[0].Rows[i]["VoucherStatus"].ToString();
                            ledg.VoucherStatus = string.IsNullOrEmpty(vs) ? "--" : vs;
                            string par = dbr.ds.Tables[0].Rows[i]["PurposeOfPayment"].ToString();
                            ledg.Particulars = string.IsNullOrEmpty(par) ? "--" : par;
                            //int ledgerid = 0;
                            string lId = dbr.ds.Tables[0].Rows[i]["Id"].ToString();
                            //success = int.TryParse(lId, out ledgerid);
                            //ledg.Id = (success == true) ? ledgerid : 0;
                            ledg.Id = this.HandleStringtoInt(lId);
                            DateTime vouch_Date;
                            string vDate = dbr.ds.Tables[0].Rows[i]["VoucherDate"].ToString();
                            bool success = DateTime.TryParse(vDate, out vouch_Date);
                            ledg.VoucherDate = (success == true) ? vouch_Date : new DateTime();
                            ledg.V_Date = (success == true) ? ledg.VoucherDate.DateTimetoStringforView():"--";
                            DateTime cred_Date;
                            string cDate = dbr.ds.Tables[0].Rows[i]["CreditDate"].ToString();
                             success = DateTime.TryParse(cDate, out cred_Date);
                            ledg.CreditDate = (success == true) ? cred_Date : new DateTime();                       
                            ledg.Cred_Date = (success == true) ? ledg.CreditDate.DateTimetoStringforView():"--";
                            string isac = dbr.ds.Tables[0].Rows[i]["IsActive"].ToString();
                            ledg.IsActive = isac == "1";                           
                            ledgs.Add(ledg);

                        }
                        result.ledgers = ledgs;
                        result.QueryTotalCount = dbr.QueryTotalCount;
                    }
                    else if(input.table.ToLower() == "register")
                    {
                        result.registerEmployees = new List<RegisterEmployee>();
                        List<RegisterEmployee> regs = new List<RegisterEmployee>();
                        for (int i = 0; i < count; i++)
                        {
                            RegisterEmployee reg = new RegisterEmployee();
                            int stationid = 0;
                            string sId = dbr.ds.Tables[0].Rows[i]["StationId"].ToString();
                            bool success = int.TryParse(sId, out stationid);
                            reg.StationId = (success == true) ? stationid : 0;
                            reg.RegisterId = Convert.ToInt32(dbr.ds.Tables[0].Rows[i]["RegisterId"]);
                            reg.FirstName= dbr.ds.Tables[0].Rows[i]["FirstName"].ToString();
                            reg.UserName = dbr.ds.Tables[0].Rows[i]["UserName"].ToString();
                            reg.LoginType = dbr.ds.Tables[0].Rows[i]["LoginType"].ToString();
                            reg.Phone = dbr.ds.Tables[0].Rows[i]["Phone"].ToString();
                            reg.StationCode = dbr.ds.Tables[0].Rows[i]["StateCode"].ToString();
                            reg.LocationName = dbr.ds.Tables[0].Rows[i]["LocationName"].ToString();

                            regs.Add(reg);

                        }
                        result.registerEmployees = regs;
                        result.QueryTotalCount = dbr.QueryTotalCount;
                    }
                    else if (input.table.ToLower() == "daemployees")
                    {
                        result.employees = new List<Employee>();
                        List<Employee> emps = new List<Employee>();
                        for (int i = 0; i < count; i++)
                        {
                            Employee emp = new Employee();
                            int stationid = 0;
                            string sId = dbr.ds.Tables[0].Rows[i]["StationId"].ToString();
                            bool success = int.TryParse(sId, out stationid);
                            emp.StationId = (success == true) ? stationid : 0;
                            int empid = 0;
                            string eId = dbr.ds.Tables[0].Rows[i]["EmployeeId"].ToString();
                            bool success2 = int.TryParse(eId, out empid);
                            emp.EmployeeId = (success2 == true) ? empid : 0;
                            emp.EmpCode = dbr.ds.Tables[0].Rows[i]["CDACode"].ToString();
                            emp.FirstName = dbr.ds.Tables[0].Rows[i]["FirstName"].ToString();
                            emp.LastName = dbr.ds.Tables[0].Rows[i]["LastName"].ToString();
                            emp.Guard_FullName = dbr.ds.Tables[0].Rows[i]["Gaurd_fullname"].ToString();
                            emp.Phone = dbr.ds.Tables[0].Rows[i]["Phone"].ToString();
                            emp.Guard_Phone = dbr.ds.Tables[0].Rows[i]["Gaurd_Phone"].ToString();
                            string db = dbr.ds.Tables[0].Rows[i]["DOB"].ToString();
                            string dj = dbr.ds.Tables[0].Rows[i]["DOJ"].ToString();
                            emp.DOB = db.ShowDatetimeView();
                            emp.DOJ = dj.ShowDatetimeView();
                            emp.BloodGroup = dbr.ds.Tables[0].Rows[i]["BloodGroup"].ToString();
                            emp.StationCode = dbr.ds.Tables[0].Rows[i]["StateCode"].ToString();
                            emp.EmpAge = dbr.ds.Tables[0].Rows[i]["Age"].ToString();
                            emp.BloodGroup = dbr.ds.Tables[0].Rows[i]["BloodGroup"].ToString();
                            string mstat = dbr.ds.Tables[0].Rows[i]["MaritalStatus"].ToString();
                            emp.MaritalStatus = mstat=="1" ;
                            emp.IsMarrired = dbr.ds.Tables[0].Rows[i]["IsMarrired"].ToString();
                            emp.PANStatus = dbr.ds.Tables[0].Rows[i]["PANStatus"].ToString();
                            // emp.Designation = dbr.ds.Tables[0].Rows[i]["Designation"].ToString();
                            emp.Place = dbr.ds.Tables[0].Rows[i]["Place"].ToString();
                            emp.AadharNumber = dbr.ds.Tables[0].Rows[i]["AadharNumber"].ToString();
                            emp.PANNumber = dbr.ds.Tables[0].Rows[i]["PAN"].ToString();
                            emp.Address1 = dbr.ds.Tables[0].Rows[i]["HouseNo"].ToString();
                            emp.Address1 = emp.Address1 +", "+ dbr.ds.Tables[0].Rows[i]["StreetName"].ToString();
                            emp.District = dbr.ds.Tables[0].Rows[i]["District"].ToString();
                            emp.EmployeeNameasperBank = dbr.ds.Tables[0].Rows[i]["EmployeeNameasperBank"].ToString();
                            emp.Address2 = dbr.ds.Tables[0].Rows[i]["VillageorTown"].ToString();
                            emp.EmployeeType = dbr.ds.Tables[0].Rows[i]["EmployeeType"].ToString();
                            emp.BankAccountNumber = dbr.ds.Tables[0].Rows[i]["BankAccountNumber"].ToString();
                            emp.BranchName = dbr.ds.Tables[0].Rows[i]["BranchName"].ToString();
                            emp.IFSCCode = dbr.ds.Tables[0].Rows[i]["IFSCCode"].ToString();
                            emp.VehicleNumber = dbr.ds.Tables[0].Rows[i]["VehicleNumber"].ToString();
                            emp.DLLRNumber = dbr.ds.Tables[0].Rows[i]["DLLRNumber"].ToString();
                            emp.DLLRStatus = dbr.ds.Tables[0].Rows[i]["DLLRStatus"].ToString();
                            emp.State = dbr.ds.Tables[0].Rows[i]["StateName"].ToString();
                            emp.LocationName = dbr.ds.Tables[0].Rows[i]["LocationName"].ToString();
                            emps.Add(emp);

                        }
                        result.employees = emps;
                        result.QueryTotalCount = dbr.QueryTotalCount;
                    }
                    else if(input.table.ToLower()== "getcdadel")
                    {
                        result.employees = new List<Employee>();
                        List<Employee> emps = new List<Employee>();
                        for (int i = 0; i < count; i++)
                        {
                            Employee emp = new Employee();
                            int stationid = 0;
                            string sId = dbr.ds.Tables[0].Rows[i]["StationId"].ToString();
                            bool success = int.TryParse(sId, out stationid);
                            emp.StationId = (success == true) ? stationid : 0;
                            int empid = 0;
                            string eId = dbr.ds.Tables[0].Rows[i]["EmployeeId"].ToString();
                            bool success2 = int.TryParse(eId, out empid);
                            emp.EmployeeId = (success2 == true) ? empid : 0;
                            emp.EmpCode = dbr.ds.Tables[0].Rows[i]["CDACode"].ToString();
                            emp.FirstName = dbr.ds.Tables[0].Rows[i]["FirstName"].ToString();
                            emp.delivery = new DeliveryDetails();
                            if (input.currentmonth > 0)
                            {
                                DeliveryDetails dd = new DeliveryDetails();
                                dd.CurrentMonth = input.currentmonth;
                                dd.EmployeeId = emp.EmployeeId;
                                dd.StationId = emp.StationId;
                                dd.EmployeeCode = emp.EmpCode;
                                dd = this.GetCDADeliveryDetailsbyMonth(dd.EmployeeId, dd.StationId, dd.CurrentMonth);
                                Tuple<string, string> sta = System.Tuple.Create("", "");
                                sta = ops.GetStationNameByStationId(emp.StationId);
                                result.EmployeeName=sta.Item2 + this.GetMonth(input.currentmonth) + "-" + dd.CreateDt.Year.ToString();
                                emp.delivery = dd;
                            }
                            emps.Add(emp);
                        }
                        if(string.IsNullOrEmpty(input.status))
                            emps = emps.Where(x => x.delivery.DeliveryRate > 0).ToList();
                        result.employees = emps;
                        result.QueryTotalCount = dbr.QueryTotalCount;
                    }
                    else if (input.table.ToLower() == "logins")
                    {
                        result.employees = new List<Employee>();
                        List<Employee> emps = new List<Employee>();
                        for (int i = 0; i < count; i++)
                        {
                            Employee emp = new Employee();
                            int stationid = 0;
                            string sId = dbr.ds.Tables[0].Rows[i]["StationId"].ToString();
                            bool success = int.TryParse(sId, out stationid);
                            emp.StationId = (success == true) ? stationid : 0;
                            emp.EmpCode = dbr.ds.Tables[0].Rows[i]["EmpCode"].ToString();
                            emp.FirstName = dbr.ds.Tables[0].Rows[i]["FirstName"].ToString();
                            emp.LoginType = dbr.ds.Tables[0].Rows[i]["LoginType"].ToString();
                            emp.Phone = dbr.ds.Tables[0].Rows[i]["Phone"].ToString();
                            emp.StationCode = dbr.ds.Tables[0].Rows[i]["StateCode"].ToString();
                            string act = dbr.ds.Tables[0].Rows[i]["IsActive"].ToString();
                            emp.IsActive = act=="1";

                            emps.Add(emp);

                        }
                        result.employees = emps;
                        result.QueryTotalCount = dbr.QueryTotalCount;
                    }
                    if (result.QueryTotalCount > 0)
                    {
                        if (p >= 1 && result.QueryTotalCount > ps)
                        {
                            double n1 = (double)result.QueryTotalCount;
                            double n2 = (double)ps;
                            double pages = 0.0;
                             pages =   n1 / n2;//Convert.ToDouble(( / ps));
                            ///result.QueryPages = (int)Math.Round(pages, MidpointRounding.AwayFromZero);
                            result.QueryPages = (int)Math.Ceiling(pages);
                        }
                        else 
                        {
                            result.QueryPages = 1;
                        }
                    }
                    

                    result.Message = dbr.Message;
                    result.Status = dbr.Status;
                    result.CommandType = dbr.CommandType;
                }
                else
                {
                    result.Message = dbr.Message;
                    result.Status = dbr.Status;
                    result.CommandType = dbr.CommandType;


                }



            }
            catch (Exception e)
            {
                result.Message = e.Message;
                result.Status = false;
                result.CommandType = "Select";
                throw e;

            }

            return result;
        }
        public int HandleStringtoInt(string str)
        {
            int result = 0;
            if (!string.IsNullOrEmpty(str))
            {
                bool success = int.TryParse(str, out result);
                result = (success == true) ? result : 0;
            }
            return result;
        }
        public APIResult GetCDADeliveryDetailsbyStation(int stationId)
        {
            APIResult result = new APIResult();
            DataBaseResult dbr = new DataBaseResult();
            CommercialConstant cc = new CommercialConstant();
            try
            {
                result.commercialConstant = new CommercialConstant();
                    int c = 0;

                dbr.ds = new System.Data.DataSet();
                dbr = ops.GetDeliveryRatesbyStation(stationId);
                c = dbr.ds.Tables[0].Rows.Count;
                if (c > 0)
                {
                    string sId = dbr.ds.Tables[0].Rows[0]["StationId"].ToString();
                    string dr2 = dbr.ds.Tables[0].Rows[0]["DeliveryRate"].ToString();
                    string petr2 = dbr.ds.Tables[0].Rows[0]["PetrolAllowance"].ToString();
                    string inc2 = dbr.ds.Tables[0].Rows[0]["Incentives"].ToString();
                    cc.DeliveryRate = this.HandleStringtoInt(dr2);
                    cc.PetrolAllowance = this.HandleStringtoInt(petr2);
                    cc.StationId = this.HandleStringtoInt(sId);
                    cc.Incentives = this.HandleStringtoInt(inc2);
                    // dd.Incentive = this.HandleStringtoInt(inc2);
                }
                result.Message = dbr.Message;
                result.Status = dbr.Status;

                result.commercialConstant = cc;
            }
            catch(Exception e)
            {
                cc.DeliveryRate = 0;
                //dd.DeliveryRate = 0;
                cc.PetrolAllowance = 0;
                //throw e;
                result.Status = false;
                result.Message = e.Message;
                result.commercialConstant = cc;
                throw e;
            }
            return result;

        }
        public DeliveryDetails GetCDADeliveryDetailsbyMonth(int employeeId,int stationId,int currentMonth)
        {
            DeliveryDetails dd = new DeliveryDetails();
            DataBaseResult dbr = new DataBaseResult();
            try
            {
                dbr.ds = new System.Data.DataSet();
                dbr = ops.GetCDADeliveryDetails(employeeId, stationId, currentMonth);
                int count = 0;
                count = dbr.ds.Tables[0].Rows.Count;
                if(count > 0)
                {
                    string dc = dbr.ds.Tables[0].Rows[0]["DeliveryCount"].ToString();
                    string dr = dbr.ds.Tables[0].Rows[0]["DeliveryRate"].ToString();
                    string petrl = dbr.ds.Tables[0].Rows[0]["PetrolAllowanceRate"].ToString();
                    string inc = dbr.ds.Tables[0].Rows[0]["Incentives"].ToString();
                    string total = dbr.ds.Tables[0].Rows[0]["TotalAmount"].ToString(); 
                  //  string stat = dbr.ds.Tables[0].Rows[0]["StationId"].ToString();
                    string cd = dbr.ds.Tables[0].Rows[0]["CreatedDate"].ToString();
                    DateTime cdDate = cd.StringtoDateTime();
                    dd.CreateDt = cdDate;
                   // dd.StationId = this.HandleStringtoInt(stat);
                    dd.DeliveryCount = this.HandleStringtoInt(dc);
                     dd.DeliveryRate = this.HandleStringtoInt(dr);
                    dd.PetrolAllowance = this.HandleStringtoInt(petrl);
                    dd.Incentive = this.HandleStringtoInt(inc);
                    dd.TotalAmount = this.HandleStringtoInt(total);
                  
        
                }
                else
                {
                    dd.DeliveryCount = 0;
                    //dd.DeliveryRate = 0;
                    dd.CurrentMonth = currentMonth;
                    dd.Incentive = 0;
                    dd.TotalAmount = 0;
                }
            }
            catch(Exception e)
            {
                dd.DeliveryCount = 0;
                //dd.DeliveryRate = 0;
                dd.CurrentMonth = currentMonth;
                dd.Incentive = 0;
                dd.TotalAmount = 0;
                throw e;
            }
            return dd;
        }

        public APIResult UpdateDeliveryRates(List<DeliveryDetails> cdds)
        {
            APIResult result = new APIResult();
            DataBaseResult dbr = new DataBaseResult();
            try
            {
                dbr = ops.UpdateDeliveryDetails(cdds);
                result.CommandType = dbr.CommandType;
                result.Status = dbr.Status;
                result.Message = dbr.Message;

            }
            catch(Exception e)
            {
                result.Message = e.Message;
                result.Status = false;
                result.CommandType = "Insert";
                throw e;
            }
            return result;
        }
        public APIResult DeleteSession(string userName,int employeeId,int userTypeId)
        {
            APIResult result = new APIResult();
            DataBaseResult dbr = new DataBaseResult();
            try
            {
                dbr = ops.DeleteSession(userName, employeeId, userTypeId);
                result.CommandType = dbr.CommandType;
                result.Message = dbr.Message;
                result.Status = dbr.Status;
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                result.Status = false;
                result.CommandType = "Select";
                throw e;

            }

            return result;
        }
        public APIResult CreateCommercialConstants(CommercialConstant constant)
        {
            APIResult result = new APIResult();
            DataBaseResult dbr = new DataBaseResult();
            try
            {
                dbr = ops.CreateCommercialConsant(constant);
                result.CommandType = dbr.CommandType;
                result.Message = dbr.Message;
                result.Status = dbr.Status;
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                result.Status = false;
                result.CommandType = "Select";
                throw e;

            }

            return result;
        }
        public APIResult CreateSession(UserType usr)
        {
            APIResult result = new APIResult();
            DataBaseResult dbr = new DataBaseResult();
            try
            {
                dbr.ds = new System.Data.DataSet();
                result.userInfo = new UserType();
                dbr = ops.CreateSession(usr);
                if(dbr.IsExists)
                {
                    result.Message = dbr.Message;
                    result.Status = false;
                    result.CommandType = "Session Insert";

                }
                else
                {
                    UserType user = new UserType();
                    int count = 0;
                    if (dbr.ds.Tables.Count <= 0)
                        count = 0;
                    else
                        count = dbr.ds.Tables[0].Rows.Count;
                    if (count > 0)
                    {
                        for (int i = 0; i < count; i++)
                        {
                            int userTypeid = 0;
                            int employeeid = 0;
                            int stationid = 0;
                            string eCode = dbr.ds.Tables[0].Rows[i]["EmpCode"].ToString();
                            user.EmpCode = eCode;
                            string empId = dbr.ds.Tables[0].Rows[i]["EmployeeId"].ToString();
                            bool succ = int.TryParse(empId, out employeeid);
                            string usertype = dbr.ds.Tables[0].Rows[i]["UserType"].ToString();
                            bool success = int.TryParse(usertype, out userTypeid);
                            string statId = dbr.ds.Tables[0].Rows[i]["StationId"].ToString();
                            bool succstat = int.TryParse(statId, out stationid);
                            userTypeid = (success == true) ? userTypeid : 0;
                            employeeid = (succ == true) ? employeeid : 0;
                            stationid = (succstat == true) ? stationid : 0;
                            user.EmployeeId = employeeid;
                            user.UserTypeId = userTypeid;
                            user.StationId = stationid;
                            if (user.UserTypeId > 0)
                            {
                                APIResult res = new APIResult();
                                res = GetConstants();
                                if (res.Status)
                                {
                                    if (res.Usertypes.Count > 0)
                                    {
                                        user.Screen = res.Usertypes.FirstOrDefault(x => x.UserTypeId == user.UserTypeId)?.Role;
                                    }
                                }

                            }
                            user.Token = dbr.ds.Tables[0].Rows[i]["UserToken"].ToString();
                            user.User = dbr.ds.Tables[0].Rows[i]["UserName"].ToString();
                            user.IsAlreadySession = Convert.ToBoolean(dbr.ds.Tables[0].Rows[i]["IsAlreadySession"].ToString());
                            user.Valid = true;
                            user.Role = usr.Role;

                        }
                        result.userInfo = user;

                        result.Status = dbr.Status;
                        result.CommandType = dbr.CommandType;
                    }
                    else
                    {
                        result.Status =false;
                        result.CommandType = dbr.CommandType;
                        result.Message = dbr.Message;
                    }
                    if (user.IsAlreadySession)
                        result.Message = "Session exists already!!! Please proceed to another page or wait for ten minutes to end this session!!!";
                    else
                        result.Message = dbr.Message;
                }


            }
            catch (Exception e)
            {
                result.Message = e.Message;
                result.Status = false;
                result.CommandType = "Session Insert";
                throw e;

            }

            return result;
        }
        public APIResult GetLoginUserInfo(string username, string password)
        {
            APIResult result = new APIResult();
            DataBaseResult dbr = new DataBaseResult();
            try
            {
                dbr.ds = new System.Data.DataSet();
                result.userInfo = new UserType();
                dbr = ops.GetLoginUserInfo(username, password);
                UserType user = new UserType();
                int count = 0;
                count = dbr.ds.Tables[0].Rows.Count;
                if (count > 0)
                {
                    for (int i = 0; i < count; i++)
                    {
                        int userTypeid = 0;
                        int employeeid = 0;
                        int stationid = 0;
                        string eCode = dbr.ds.Tables[0].Rows[i]["EmpCode"].ToString();
                        user.EmpCode = eCode;
                        string empId = dbr.ds.Tables[0].Rows[i]["EmployeeId"].ToString();
                        bool succ = int.TryParse(empId, out employeeid);
                        string usertype = dbr.ds.Tables[0].Rows[i]["UserTypeId"].ToString();
                        bool success = int.TryParse(usertype, out userTypeid);
                        string statId = dbr.ds.Tables[0].Rows[i]["StationId"].ToString();
                        bool succstat = int.TryParse(statId, out stationid);
                        userTypeid = (success == true) ? userTypeid : 0;
                        employeeid = (succ == true) ? employeeid : 0;
                        stationid = (succstat == true) ? stationid : 0;
                        user.EmployeeId = employeeid;
                        user.UserTypeId = userTypeid;
                        user.StationId = stationid;
                        if(user.UserTypeId>0)
                        {
                            APIResult res = new APIResult();
                            res = GetConstants();
                            if (res.Status)
                            {
                                if(res.Usertypes.Count>0)
                                {
                                    user.Screen = res.Usertypes.FirstOrDefault(x => x.UserTypeId == user.UserTypeId)?.Role;
                                }
                            }

                        }
                        user.Role = dbr.ds.Tables[0].Rows[i]["LoginType"].ToString();
                        user.User = dbr.ds.Tables[0].Rows[i]["UserName"].ToString();
                        user.Valid = true;

                    }
                    result.userInfo = user;
                    result.Message = dbr.Message;
                    result.Status = dbr.Status;
                    result.CommandType = dbr.CommandType;
                }
                else
                {
                    result.Message = dbr.Message;
                    result.Status = dbr.Status;
                    result.CommandType = dbr.CommandType;


                }



            }
            catch (Exception e)
            {
                result.Message = e.Message;
                result.Status = false;
                result.CommandType = "Select";
                throw e;

            }

            return result;
        }
        public APIResult GetLoginUserInfo(int userTypeId, int employeeId)
        {
            APIResult result = new APIResult();
            DataBaseResult dbr = new DataBaseResult();
            try
            {
                dbr.ds = new System.Data.DataSet();
                result.userInfo = new UserType();
                dbr = ops.GetLoginUserInfo(userTypeId, employeeId);
                UserType user = new UserType();
                int count = 0;
                count = dbr.ds.Tables[0].Rows.Count;
                if (count > 0)
                {
                    for (int i = 0; i < count; i++)
                    {
                        int userTypeid = 0;
                        int employeeid = 0;
                        int stationid = 0;
                        string empId = dbr.ds.Tables[0].Rows[i]["EmployeeId"].ToString();
                        bool succ = int.TryParse(empId, out employeeid);
                        string usertype = dbr.ds.Tables[0].Rows[i]["UserTypeId"].ToString();
                        bool success = int.TryParse(usertype, out userTypeid);
                        string statId = dbr.ds.Tables[0].Rows[i]["StationId"].ToString();
                        bool succstat = int.TryParse(statId, out stationid);
                        userTypeid = (success == true) ? userTypeid : 0;
                        employeeid = (succ == true) ? employeeid : 0;
                        stationid = (succstat == true) ? stationid : 0;
                        user.EmployeeId = employeeid;
                        user.UserTypeId = userTypeid;
                        user.StationId = stationid;
                        if (user.UserTypeId > 0)
                        {
                            APIResult res = new APIResult();
                            res = GetConstants();
                            if (res.Status)
                            {
                                if (res.Usertypes.Count > 0)
                                {
                                    user.Screen = res.Usertypes.FirstOrDefault(x => x.UserTypeId == user.UserTypeId)?.Role;
                                }
                            }

                        }
                        user.Role = dbr.ds.Tables[0].Rows[i]["LoginType"].ToString();
                        user.User = dbr.ds.Tables[0].Rows[i]["UserName"].ToString();
                        user.Valid = true;

                    }
                    result.userInfo = user;
                    result.Message = dbr.Message;
                    result.Status = dbr.Status;
                    result.CommandType = dbr.CommandType;
                }
                else
                {
                    result.Message = dbr.Message;
                    result.Status = dbr.Status;
                    result.CommandType = dbr.CommandType;


                }



            }
            catch (Exception e)
            {
                result.Message = e.Message;
                result.Status = false;
                result.CommandType = "Select";
                throw e;

            }

            return result;
        }
        public APIResult CheckIfSessionExists(UserType user)
        {
            APIResult result = new APIResult();
            bool isExists = false;
            try
            {
                isExists = ops.CheckIfSessionExists(user.User, user.EmployeeId, user.UserTypeId);
                if(isExists)
                {
                    result.Message = "Session already exists for this user!!";
                    result.Status = true;
                    result.CommandType = "Insert";
                    result.userInfo = user;
                    result.EmployeeName = user.User;
                }
                else
                {
                    result.Status = false;
                    result.userInfo = user;
                    result.CommandType = "Insert";
                    result.EmployeeName = user.User;
                    result.Message = "Session Expired or terminated, Please login again!!";
                }

            }
            catch (Exception e)
            {
                result.Message = e.Message;
                result.Status = false;
                result.CommandType = "Select";
                throw e;

            }

            return result;

        }

        public APIResult UpdateSession(UserType usr)
        {
            APIResult result = new APIResult();
            DataBaseResult dbr = new DataBaseResult();
            try
            {
                result.userInfo = new UserType();
                dbr.ds = new System.Data.DataSet();
                dbr = ops.UpdateSession(usr);
                result.userInfo = usr;
                result.Status = dbr.Status;
                result.Message = dbr.Message;
                result.CommandType = dbr.CommandType;
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                result.Status = false;
                result.CommandType ="Session update";
                //result.Id = registerId;
                throw e;

            }

            return result;
        }
        public APIResult ResetPassword(int employeeId,string password)
        {
            APIResult result = new APIResult();
            DataBaseResult dbr = new DataBaseResult();
            try
            {
                result.userInfo = new UserType();
                dbr.ds = new System.Data.DataSet();
                dbr = ops.ResetPassword(employeeId,password);
                result.Status = dbr.Status;
                result.Message = dbr.Message;
                result.CommandType = dbr.CommandType;
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                result.Status = false;
                result.CommandType = "RESET";
                //result.Id = registerId;
                throw e;

            }

            return result;
        }
        public APIResult ApproveUser(int registerId,string status,string empCode="",int pId=0)
        {
            APIResult result = new APIResult();
            DataBaseResult dbr = new DataBaseResult();
            Tuple<bool, string> verify = System.Tuple.Create(false, "");
           // bool verify = false;
            try
            {
                dbr.ds = new System.Data.DataSet();
                dbr = ops.ApproveUser(registerId,status,empCode,pId);
                result.Message = dbr.Message;
                if (status == "a" && dbr.Status == true && dbr.Id > 0)
                {
                    result.employee = new Employee();
                    result.employee = this.GetRegisteredUser(dbr.Id);
                    if(result.employee != null)
                    {
                        if(!string.IsNullOrEmpty(result.employee.Email) && result.employee.EmployeeId > 0)
                        {
                            try
                            {
                                // string p = "C:\Users\Public\home.html");
                                string mpath = configuration["mailpath"];
                                StreamReader reader = new StreamReader(mpath);
                                string readFile = reader.ReadToEnd();
                                string myString = "";
                                myString = readFile;
                                myString = myString.Replace("https://www.kleenandshine.com/#/ResetPassword/<rid>", "https://www.kleenandshine.com/#/ResetPassword/" + result.employee.EmployeeId);
                                //myString = myString.Replace("$$CompanyName$$", "Dasari Group");
                                //myString = myString.Replace("$$Email$$", "suresh@gmail.com");
                                //myString = myString.Replace("$$Website$$", "http://www.aspdotnet-suresh.com");
                                verify = EMAIL.SendEmail("theeru999@gmail.com", result.employee.Email, myString, "USER REQUEST APPROVED");
                            } 
                            catch(Exception e)
                            {
                                result.Message = result.Message + "; Error occured while collecting data to send mail  due to "+e.Message+"  !!  Unable to send Email to this user.";
                            }

                        }
                    }
                    result.Message = result.Message +"; " + verify.Item2;
                    //if (verify.Item1)
                    //{
                    //    result.Message = result.Message + "; Mail sent to this user.";
                    //}
                    //else
                    //{
                    //    result.Message = result.Message + "; Unable to send Email to this user.";
                    //}
                }
                result.Status = dbr.Status;
                result.Id = dbr.Id;
                result.CommandType = dbr.CommandType;
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                result.Status = false;
                result.CommandType =status=="a"? "UPDATE":"DELETE";
                result.Id = registerId;
                throw e;

            }

            return result;
        }

        public APIResult GetAdminDetails()
        {
            APIResult result = new APIResult();
            DataBaseResult dbr = new DataBaseResult();
            try
            {
                dbr.ds = new System.Data.DataSet();
                result.requests = new List<RequestDetail>();
                dbr = ops.GetAdminDetails();
                List<RequestDetail> details = new List<RequestDetail>();
                int count = 0;
                if (dbr.ds.Tables.Count > 0)
                {
                    count = dbr.ds.Tables[0].Rows.Count;
                    if (count > 0)
                    {
                        for (int i = 0; i < count; i++)
                        {
                            RequestDetail req = new RequestDetail();
                            req.Count = Convert.ToInt32(dbr.ds.Tables[0].Rows[i]["Count"]);
                            req.Detail = dbr.ds.Tables[0].Rows[i]["Detail"].ToString();
                            details.Add(req);

                        }
                        result.requests = details;
                        result.Message = dbr.Message;
                        result.Status = dbr.Status;
                        result.CommandType = dbr.CommandType;
                    }
                    else
                    {
                        result.Message = dbr.Message;
                        result.Status = dbr.Status;
                        result.CommandType = dbr.CommandType;


                    }
                }
                else
                {
                    result.Message = dbr.Message;
                    result.Status = dbr.Status;
                    result.CommandType = dbr.CommandType;


                }



            }
            catch (Exception e)
            {
                result.Message = e.Message;
                result.Status = false;
                result.CommandType = "Select";
                throw e;

            }

            return result;
        }

        public APIResult GetEmployees(APIInput input,bool isEmployee=false)
        {
            APIResult result = new APIResult();
            DataBaseResult dbr = new DataBaseResult();
            try
            {
                dbr.ds = new System.Data.DataSet();
                result.employees = new List<Employee>();
                dbr = ops.GetPaginationRecords(input.stationId,input.table,string.Empty,string.Empty,input.page,input.pagesize,string.Empty,isEmployee);
                List<Employee> emps = new List<Employee>();
                int count = 0;
                count = dbr.ds.Tables[0].Rows.Count;
                if (count > 0)
                {
                    if(input.table.ToLower()=="daemployees")
                    {
                        for (int i = 0; i < count; i++)
                        {
                            Employee emp = new Employee();
                            emp.EmployeeId = Convert.ToInt32(dbr.ds.Tables[0].Rows[i]["EmployeeId"]);
                            emp.FirstName = dbr.ds.Tables[0].Rows[i]["FirstName"].ToString();
                            emp.Phone = dbr.ds.Tables[0].Rows[i]["Phone"].ToString();
                            emp.LastName = dbr.ds.Tables[0].Rows[i]["LastName"].ToString();
                            emp.Guard_FullName = dbr.ds.Tables[0].Rows[i]["Gaurd_fullname"].ToString();
                            emp.DOB = dbr.ds.Tables[0].Rows[i]["DOB"].ToString();
                            emp.DOJ = dbr.ds.Tables[0].Rows[i]["DOJ"].ToString();
                            emp.Guard_Phone = dbr.ds.Tables[0].Rows[i]["Gaurd_Phone"].ToString();
                            emp.StationCode = dbr.ds.Tables[0].Rows[i]["StateCode"].ToString();
                            emp.EmpAge = dbr.ds.Tables[0].Rows[i]["Age"].ToString();
                            emp.BloodGroup = dbr.ds.Tables[0].Rows[i]["BloodGroup"].ToString();
                            emp.MaritalStatus = Convert.ToBoolean(dbr.ds.Tables[0].Rows[i]["MaritalStatus"].ToString());
                            // emp.Designation = dbr.ds.Tables[0].Rows[i]["Designation"].ToString();
                            emp.Place = dbr.ds.Tables[0].Rows[i]["Place"].ToString();
                            emp.AadharNumber = dbr.ds.Tables[0].Rows[i]["AadharNumber"].ToString();
                            emp.PANNumber = dbr.ds.Tables[0].Rows[i]["PAN"].ToString();
                            emp.Address1 = dbr.ds.Tables[0].Rows[i]["Address1"].ToString();
                            emp.Address2 = dbr.ds.Tables[0].Rows[i]["Address2"].ToString();
                            emp.EmployeeType = dbr.ds.Tables[0].Rows[i]["EmployeeType"].ToString();
                            emp.BankAccountNumber = dbr.ds.Tables[0].Rows[i]["BankAccountNumber"].ToString();
                            emp.BranchName = dbr.ds.Tables[0].Rows[i]["BranchName"].ToString();
                            emp.IFSCCode = dbr.ds.Tables[0].Rows[i]["IFSCCode"].ToString();
                            emp.VehicleNumber = dbr.ds.Tables[0].Rows[i]["VehicleNumber"].ToString();
                            emp.DLLRNumber = dbr.ds.Tables[0].Rows[i]["DLLRNumber"].ToString();
                            emp.DLLRStatus = dbr.ds.Tables[0].Rows[i]["DLLRStatus"].ToString();
                            emp.State = dbr.ds.Tables[0].Rows[i]["StateName"].ToString();
                            emp.LocationName = dbr.ds.Tables[0].Rows[i]["LocationName"].ToString();
                            emps.Add(emp);

                        }

                    }
                    else if (input.table.ToLower() == "employees")
                    {
                        for (int i = 0; i < count; i++)
                        {
                            Employee emp = new Employee();
                            emp.EmployeeId = Convert.ToInt32(dbr.ds.Tables[0].Rows[i]["EmployeeId"]);
                            emp.FirstName = dbr.ds.Tables[0].Rows[i]["FirstName"].ToString();
                            emp.Phone = dbr.ds.Tables[0].Rows[i]["Phone"].ToString();
                            emp.LoginType = dbr.ds.Tables[0].Rows[i]["LoginType"].ToString();
                            emp.Designation = dbr.ds.Tables[0].Rows[i]["Designation"].ToString();
                            emp.State = dbr.ds.Tables[0].Rows[i]["StateCode"].ToString();
                            emp.LocationName = dbr.ds.Tables[0].Rows[i]["LocationName"].ToString();
                            emps.Add(emp);

                        }

                    }
                    else if (input.table.ToLower() == "pdsemployees" || input.table.ToLower() ==  "pdsunemployees")
                    {
                        for (int i = 0; i < count; i++)
                        {
                            Employee emp = new Employee();
                            emp.EmployeeId = Convert.ToInt32(dbr.ds.Tables[0].Rows[i]["EmployeeId"]);
                            emp.FirstName = dbr.ds.Tables[0].Rows[i]["FirstName"].ToString();
                            emp.Phone = dbr.ds.Tables[0].Rows[i]["Phone"].ToString();
                            emp.LastName = dbr.ds.Tables[0].Rows[i]["LastName"].ToString();
                            emp.Guard_FullName = dbr.ds.Tables[0].Rows[i]["Gaurd_fullname"].ToString();
                            emp.DOB = dbr.ds.Tables[0].Rows[i]["DOB"].ToString();
                            emp.DOJ = dbr.ds.Tables[0].Rows[i]["DOJ"].ToString();
                            emp.Guard_Phone = dbr.ds.Tables[0].Rows[i]["Gaurd_Phone"].ToString();
                            emp.StationCode = dbr.ds.Tables[0].Rows[i]["StateCode"].ToString();
                            emp.EmpAge = dbr.ds.Tables[0].Rows[i]["Age"].ToString();
                            emp.BloodGroup = dbr.ds.Tables[0].Rows[i]["BloodGroup"].ToString();
                            emp.MaritalStatus = Convert.ToBoolean(dbr.ds.Tables[0].Rows[i]["MaritalStatus"].ToString());
                            // emp.Designation = dbr.ds.Tables[0].Rows[i]["Designation"].ToString();
                            emp.Place = dbr.ds.Tables[0].Rows[i]["Place"].ToString();
                            emp.AadharNumber = dbr.ds.Tables[0].Rows[i]["AadharNumber"].ToString();
                            emp.PANNumber = dbr.ds.Tables[0].Rows[i]["PAN"].ToString();
                            emp.Address1 = dbr.ds.Tables[0].Rows[i]["Address1"].ToString();
                            emp.Address2 = dbr.ds.Tables[0].Rows[i]["Address2"].ToString();
                            emp.EmployeeType = dbr.ds.Tables[0].Rows[i]["EmployeeType"].ToString();
                            emp.BankAccountNumber = dbr.ds.Tables[0].Rows[i]["BankAccountNumber"].ToString();
                            emp.BranchName = dbr.ds.Tables[0].Rows[i]["BranchName"].ToString();
                            emp.IFSCCode = dbr.ds.Tables[0].Rows[i]["IFSCCode"].ToString();
                            emp.VehicleNumber = dbr.ds.Tables[0].Rows[i]["VehicleNumber"].ToString();
                            emp.DLLRNumber = dbr.ds.Tables[0].Rows[i]["DLLRNumber"].ToString();
                            emp.DLLRStatus = dbr.ds.Tables[0].Rows[i]["DLLRStatus"].ToString();
                            emp.State = dbr.ds.Tables[0].Rows[i]["StateName"].ToString();
                            emp.LocationName = dbr.ds.Tables[0].Rows[i]["LocationName"].ToString();
                            emps.Add(emp);

                        }

                    }

                    result.employees = emps;
                    result.Message = dbr.Message;
                    result.Status = dbr.Status;
                    result.CommandType = dbr.CommandType;
                }
                else
                {
                    result.Message = dbr.Message;
                    result.Status = dbr.Status;
                    result.CommandType = dbr.CommandType;


                }



            }
            catch (Exception e)
            {
                result.Message = e.Message;
                result.Status = false;
                result.CommandType = "Select";
                throw e;

            }

            return result;
        }

        public APIResult InsertVoucher(Voucher input)
        {
            APIResult result = new APIResult();
            DataBaseResult dbr = new DataBaseResult();
            try
            {
                input.VoucherDate = input.V_Date.StringtoDateTime();
                dbr.ds = new System.Data.DataSet();
                dbr = ops.InsertVoucher(input);
                result.Message = dbr.Message;
                result.Status = dbr.Status;
                result.Id = dbr.Id;
                result.VoucherNumber = dbr.VoucherNumber;
                result.CommandType = dbr.CommandType;

            }
            catch (Exception e)
            {
                result.Message = e.Message;
                result.Status = false;
                result.CommandType = "INSERT";
                result.Id = 0;
                result.VoucherNumber = "";
                throw e;


            }
            return result;

        }
        public APIResult UpdateVoucher(Voucher input)
        {
            APIResult result = new APIResult();
            DataBaseResult dbr = new DataBaseResult();
            try
            {
                input.VoucherDate = input.V_Date.StringtoDateTime();
                dbr.ds = new System.Data.DataSet();
                dbr = ops.UpdateVoucher(input);
                result.Message = dbr.Message;
                result.Status = dbr.Status;
                result.Id = dbr.Id;
                result.VoucherNumber = dbr.VoucherNumber;
                result.CommandType = dbr.CommandType;

            }
            catch (Exception e)
            {
                result.Message = e.Message;
                result.Status = false;
                result.CommandType = "UPDATE";
                result.Id = 0;
                result.VoucherNumber = "";
                throw e;


            }
            return result;

        }
        public APIResult ApproveVoucher(int voucherId,string status)
        {
            APIResult result = new APIResult();
            DataBaseResult dbr = new DataBaseResult();
            try
            {
                dbr = ops.ApproveVoucher(voucherId,status);
                result.Message = dbr.Message;
                result.Status = dbr.Status;
                result.CommandType = dbr.CommandType;

            }
            catch (Exception e)
            {
                result.Message = e.Message;
                result.Status = false;
                result.CommandType = "UPDATE";
                result.Id = 0;
                result.VoucherNumber = "";
                throw e;


            }
            return result;

        }
        public APIResult UpdateVoucherDetails(List<Voucher> VIds,string status)
        {
            APIResult result = new APIResult();
            DataBaseResult dbr = new DataBaseResult();
            List<Voucher> vouchers = new List<Voucher>();
            try
            {
                if (VIds.Count > 0)
                {
                   
                    foreach (var item in VIds)
                    {
                        int i = item.VoucherId;
                        Voucher v = new Voucher();
                        if (i > 0)
                        {
                            result.voucher = new Voucher();
                            result = this.GetVoucherDetailsbyVoucherNumber(i);
                            if(result.voucher != null)
                            {
                                v = result.voucher;
                                vouchers.Add(v);
                            }
                        }
                        else
                        {
                            result.Status = false;
                            result.Message = "Unable to get details,no Input data to process";
                            break;
                        }

                    }
                    if(vouchers.Count > 0)
                    {
                        dbr = ops.UpdateVoucherDetails(vouchers,status);
                        result = new APIResult();
                        result.CommandType = dbr.CommandType;
                        result.Status = dbr.Status;
                        result.Message = dbr.Message;
                    }
                    else
                    {
                        result.Status = false;
                        result.Message = "No Input data to process";

                    }

                }
                else
                {
                    result.Status = false;
                    result.Message = "No Input data to process";

                }

            }
            catch (Exception e)
            {

                result.Status = false;
                result.Message = e.Message;
                dbr.CommandType = "Update";
                throw e;
            }
            return result;
        }
        public APIResult GetVoucherDetailsbyVoucherNumber(int voucherId)
        {
            APIResult result = new APIResult();
            DataBaseResult dbr = new DataBaseResult();
            Voucher v = new Voucher();
            try
            {
                result.voucher = new Voucher();
                int c = 0;

                dbr.ds = new System.Data.DataSet();
                dbr = ops.GetVoucherDetailsbyVoucherNumber(voucherId);
                if (dbr.ds.Tables.Count > 0)
                {
                    v.VoucherId = voucherId;
                    c = dbr.ds.Tables[0].Rows.Count;
                    if (c > 0)
                    {
                        string vno = dbr.ds.Tables[0].Rows[0]["VoucherNumber"].ToString();
                        string sId = dbr.ds.Tables[0].Rows[0]["StationId"].ToString();
                       // string vdate = dbr.ds.Tables[0].Rows[0]["VoucherDate"].ToString();
                        string purpose = dbr.ds.Tables[0].Rows[0]["PurposeOfPayment"].ToString();
                        string party = dbr.ds.Tables[0].Rows[0]["PartyName"].ToString();
                        string namnt = dbr.ds.Tables[0].Rows[0]["NetAmount"].ToString();
                        string totamnt = dbr.ds.Tables[0].Rows[0]["TotalAmount"].ToString();
                        string taxamnt = dbr.ds.Tables[0].Rows[0]["TaxAmount"].ToString();
                        string status = dbr.ds.Tables[0].Rows[0]["VoucherStatus"].ToString();
                        DateTime vouch_Date;
                        string vDate = dbr.ds.Tables[0].Rows[0]["VoucherDate"].ToString();
                        bool success = DateTime.TryParse(vDate, out vouch_Date);
                        v.VoucherDate = (success == true) ? vouch_Date : new DateTime();
                        v.V_Date = (success == true) ? v.VoucherDate.DateTimetoStringforView() : "";
                        v.VoucherNumber = string.IsNullOrEmpty(vno) ? "" : vno;
                      //  v.V_Date = vdate.StringDateTimetoStringView();
                        v.PartyName = string.IsNullOrEmpty(party) ? "" : party; 
                        v.PurposeOfPayment = string.IsNullOrEmpty(purpose) ? "" : purpose;
                        v.StationId = this.HandleStringtoInt(sId);
                        v.TotalAmount = this.HandleStringtoInt(totamnt);
                        v.TaxAmount = this.HandleStringtoInt(taxamnt);
                        v.NetAmount = this.HandleStringtoInt(namnt);
                        v.VoucherStatus = status;
                        // dd.Incentive = this.HandleStringtoInt(inc2);
                    }
                    result.Message = dbr.Message;
                    result.Status = dbr.Status;
                    
                }
                
                result.voucher = v;

            }
            catch (Exception e)
            {
               
                result.Status = false;
                result.Message = e.Message;
                result.voucher = v;
                throw e;
            }
            return result;

        }
        public APIResult GetPreviousCreditandDebitDetails(int stationId)
        {
            APIResult result = new APIResult();
            DataBaseResult dbr = new DataBaseResult();
            Ledger ld = new Ledger();
            try
            {
                result.ledger = new Ledger();
                ld.StationId = stationId;
                dbr.ds = new System.Data.DataSet();
                dbr = ops.GetPreviousCreditandDebitDetails(stationId);
                int count = 0;
                if (dbr.ds.Tables.Count > 0)
                {
                    count = dbr.ds.Tables[0].Rows.Count;
                    if (count > 0)
                    {
                           
                            string c_a = dbr.ds.Tables[0].Rows[0]["CreditAmount"].ToString();
                            string d_a = dbr.ds.Tables[0].Rows[0]["DebitAmount"].ToString();
                        string curr_cred = dbr.ds.Tables[0].Rows[0]["CurrentCreditAmount"].ToString();
                        string curr_debt = dbr.ds.Tables[0].Rows[0]["CurrentDebitAmount"].ToString();
                        string curr_bal = dbr.ds.Tables[0].Rows[0]["CurrentBalanceAmount"].ToString();
                        ld.Credit = this.HandleStringtoInt(c_a);
                            ld.Debit = this.HandleStringtoInt(d_a);
                        ld.Id = 0;
                        ld.CurrentCreditAmount = this.HandleStringtoInt(curr_cred);
                        ld.CurrentDebitAmount = this.HandleStringtoInt(curr_debt);
                        ld.CurrentBalanceAmount = this.HandleStringtoInt(curr_bal);
                        if (ld.Credit > 0)
                            {
                                if (ld.Credit > ld.Debit)
                                {
                                    ld.Balance = ld.Credit - ld.Debit;
                                }
                                else
                                {
                                    ld.Balance = 0;
                                }
                            }
                            else
                                ld.Balance = 0;
                            

                        
                    }
                    else
                    {
                        ld.Credit = 0;
                        ld.Debit = 0;
                        ld.Balance = 0;
                    }
                }
                else
                {
                    ld.Credit = 0;
                    ld.Debit = 0;
                    ld.Balance = 0;
                }
                result.ledger = ld;
                result.CommandType = dbr.CommandType;
                result.Status = dbr.Status;
                result.Message = dbr.Message;

            }
            catch (Exception e)
            {
                result.Message = e.Message;
                result.Status = false;
                result.CommandType = "SELECT";
                result.Id = 0;
                result.VoucherNumber = "";
                throw e;


            }
            return result;
        }

        public APIResult InsertLedger(Ledger input)
        {
            APIResult result = new APIResult();
            DataBaseResult dbr = new DataBaseResult();
            try
            {
                dbr.ds = new System.Data.DataSet();
                dbr = ops.InsertLedger(input);
                result.Message = dbr.Message;
                result.Status = dbr.Status;
                result.Id = dbr.Id;
                result.CommandType = dbr.CommandType;

            }
            catch (Exception e)
            {
                result.Message = e.Message;
                result.Status = false;
                result.CommandType = "INSERT";
                result.Id = 0;
                throw e;


            }
            return result;

        }

        #region Download PDF file

        public List<int> GetAllEmpIdsforPDF(int currentmonth,int stationId)
        {
            DataBaseResult dbr = new DataBaseResult();
            List<int> empIds = new List<int>();
            try
            {
                if (currentmonth == 0)
                {
                    DateTime dtt = DateTime.Now;
                    currentmonth = dtt.GetIndianDateTimeNow().Month;
                }
                dbr.ds = new System.Data.DataSet();
                dbr = ops.GetAllEmpsDeliveryDetailsforPDF(stationId,currentmonth);
                int count = 0;
                count = dbr.ds.Tables[0].Rows.Count;
                if (count > 0)
                {
                    for (int i = 0; i < count; i++)
                    {
                        int empid = 0;
                        string eId = dbr.ds.Tables[0].Rows[i]["EmployeeId"].ToString();
                        bool success2 = int.TryParse(eId, out empid);
                        empIds.Add(empid);
                    }
                }
            }
            catch 
            {
                empIds = new List<int>();
            }
            return empIds;
        }
        public APIResult GetEmpDataforPDFFile(int empId,int currentMonth)
        {
            APIResult result = new APIResult();
           // PDFLayout layout = new PDFLayout();
            DataBaseResult dbr = new DataBaseResult();
            try
            {
                Employee emp = new Employee();
                if (currentMonth == 0)
                {
                    DateTime dtt = DateTime.Now;
                    currentMonth = dtt.GetIndianDateTimeNow().Month;
                }
                dbr.ds = new System.Data.DataSet();
                result.employee = new Employee();
                result.pdfLayout = new PDFLayout();
                dbr = ops.GetEmpDataforPDF(empId);
                 int count = 0;
                count = dbr.ds.Tables[0].Rows.Count;
                if (count > 0)
                {
                    for (int i = 0; i < count; i++)
                    {
                      
                        int stationid = 0;
                        string sId = dbr.ds.Tables[0].Rows[i]["StationId"].ToString();
                        bool success = int.TryParse(sId, out stationid);
                        emp.StationId = (success == true) ? stationid : 0;
                        int empid = 0;
                        string eId = dbr.ds.Tables[0].Rows[i]["EmployeeId"].ToString();
                        bool success2 = int.TryParse(eId, out empid);
                        emp.EmployeeId = (success2 == true) ? empid : 0;
                        emp.EmpCode = dbr.ds.Tables[0].Rows[i]["CDACode"].ToString();
                        emp.FirstName = dbr.ds.Tables[0].Rows[i]["FirstName"].ToString();
                        emp.LastName = dbr.ds.Tables[0].Rows[i]["LastName"].ToString();
                        emp.Address1 = dbr.ds.Tables[0].Rows[i]["Address1"].ToString();
                        emp.Address2 = dbr.ds.Tables[0].Rows[i]["Address2"].ToString();
                        emp.PANNumber = dbr.ds.Tables[0].Rows[i]["PAN"].ToString();
                        emp.Phone = dbr.ds.Tables[0].Rows[i]["Phone"].ToString();
                        Tuple<string, string> sta = System.Tuple.Create("", "");
                        sta = ops.GetStationNameByStationId(emp.StationId);
                        emp.StationCode = sta.Item2;
                        dbr.ds = new System.Data.DataSet();
                        dbr = ops.GetEmpDeliveryDetailsforPDF(emp.EmployeeId, emp.StationId, currentMonth);
                        count = dbr.ds.Tables[0].Rows.Count;
                        if (count > 0)
                        {
                            emp.delivery = new DeliveryDetails();
                            for (int k = 0; k < count; k++)
                            {
                                emp.delivery.StandardRate = dbr.ds.Tables[0].Rows[k]["DeliveryRate"].ToString();
                                string ptrallw = dbr.ds.Tables[0].Rows[k]["PetrolAllowanceRate"].ToString();
                                emp.delivery.PetrolAllowance = this.HandleStringtoInt(ptrallw);
                                string dc = dbr.ds.Tables[0].Rows[k]["DeliveryCount"].ToString();
                                emp.delivery.DeliveryCount = this.HandleStringtoInt(dc);
                                string cd = dbr.ds.Tables[0].Rows[k]["CreatedDate"].ToString();
                                DateTime cdDate = cd.StringtoDateTime();
                                emp.delivery.CreateDt = cdDate;
                                string inc = dbr.ds.Tables[0].Rows[k]["Incentives"].ToString();
                                emp.delivery.Incentive = this.HandleStringtoInt(inc);
                                emp.delivery.CurrentMonth = currentMonth;
                            }
                            result.employee = emp;
                            result.pdfLayout = this.GetPdfContent(result.employee);
                            result.Status = true;
                        }
                        else
                        {
                            result.Message = "No Delivery Details found for this month for this employee.";
                            result.Status = false;
                            result.CommandType = "Download";
                            result.Id = 0;
                            result.EmployeeName = "";
                        }

                    }

                }
                else
                {
                    result.Message = "No Employee Found.";
                    result.Status = false;
                    result.CommandType = "Download";
                    result.Id = 0;
                    result.EmployeeName = "";
                }

            }
            catch (Exception e)
            {
                result.Message = e.Message;
                result.Status = false;
                result.CommandType = "Download";
                result.Id = 0;
                result.EmployeeName = "";
                throw e;


            }
            return result;
        }
        public PDFLayout GetPdfContent(Employee emp)
        {
            PDFLayout pdfC = new PDFLayout();
            try
            {
                pdfC.Title = "CDAInvoice";
                pdfC.Name = emp.FirstName +" "+emp.LastName;
                pdfC.Address = emp.Address1 +" , " + Environment.NewLine +emp.Address2;
                pdfC.MobileNumber = emp.Phone;
                pdfC.BillingPeriod = this.GetMonth(emp.delivery.CurrentMonth)+"-"+emp.delivery.CreateDt.Year.ToString();//"JAN-2021";
                pdfC.VendorCode = emp.EmpCode;
                DateTime dtt = DateTime.Now;
                dtt = dtt.GetIndianDateTimeNow();
                pdfC.InvoiceDate = dtt.ToShortDateString();
                pdfC.PANDetails = emp.PANNumber;
                pdfC.BillTo = "Penna Delivery Services";
                pdfC.WorkPerformed = "Deliveries did till "+pdfC.BillingPeriod;
                pdfC.tbContents = new List<PdfTbContent>();
                List<PdfTbContent> tbs = new List<PdfTbContent>();
                PdfTbContent tb = new PdfTbContent();
                tb.Price = emp.delivery.StandardRate;
                tb.Description = "Delivery Rate";
                tb.Quantity =emp.delivery.DeliveryCount.ToString();
                int r = 0;
                r = Convert.ToInt32(tb.Price) * Convert.ToInt32(tb.Quantity);
                tb.FinalAmount = r;
                tb.Amount = r.ToString();
                tbs.Add(tb);
                tb = new PdfTbContent();
                tb.Price = emp.delivery.PetrolAllowance.ToString();
                tb.Description = "Allowances";
                tb.Quantity = "1";
                r = 0;
                r = Convert.ToInt32(tb.Price) * Convert.ToInt32(tb.Quantity);
                tb.FinalAmount = r;
                tb.Amount = r.ToString();
                tbs.Add(tb);
                tb = new PdfTbContent();
                tb.Price = emp.delivery.Incentive.ToString();
                tb.Description = "Incentives";
                tb.Quantity = "1";
                r = 0;
                r = Convert.ToInt32(tb.Price) * Convert.ToInt32(tb.Quantity);
                tb.FinalAmount = r;
                tb.Amount = r.ToString();
                tbs.Add(tb);
                pdfC.tbContents = tbs;
                pdfC.TDS = "0";
                int tot = pdfC.tbContents.Sum(x => x.FinalAmount) + Convert.ToInt32(pdfC.TDS);
                pdfC.Total = tot.ToString();
                pdfC.AmountInWords = NumberToWords.ConvertAmount(double.Parse(pdfC.Total)) ;//"Five Hundred And Twenty Only";
                pdfC.Comments = "--";
                pdfC.Sign1 = "Signature of Branch Incharge";
                pdfC.Sign2 = "Signature of Signing Authority";
                pdfC.Sign3 = "Signature of Delivery Associate";
            }
            catch(Exception e)
            {
                pdfC = new PDFLayout();
                throw e;
            }
            return pdfC;
        }
        public async Task<byte[]> GetZipArchive(List<InMemoryFile> files)
        {
            byte[] archiveFile;
            try
            {
                using (var archiveStream = new MemoryStream())
                {
                    using (var archive = new ZipArchive(archiveStream, ZipArchiveMode.Create, true))
                    {
                        foreach (var file in files)
                        {
                            var zipArchiveEntry = archive.CreateEntry(file.FileName, CompressionLevel.Fastest);
                            using (var zipStream = zipArchiveEntry.Open())
                                await zipStream.WriteAsync(file.Content, 0, file.Content.Length);
                        }
                        //Parallel.ForEach(files, async file =>
                        // {
                        //     var zipArchiveEntry = archive.CreateEntry(file.FileName, CompressionLevel.Fastest);
                        //     using (var zipStream = zipArchiveEntry.Open())
                        //         await zipStream.WriteAsync(file.Content, 0, file.Content.Length);
                        // }
                        //    );
                    }

                    archiveFile = archiveStream.ToArray();
                }
            }
            catch(Exception e)
            {
                archiveFile = null;//new byte[];
                throw e;
            }

            return archiveFile;
        }

        public string GetMonth(int i)
        {
            string month = "";
                switch (i)
                {
                    case 1:month = "JAN";
                        break;
                    case 2:
                        month = "FEB";
                        break;
                    case 3:
                        month = "MAR";
                        break;
                    case 4:
                        month = "APR";
                        break;
                    case 5:
                        month = "MAY";
                        break;
                    case 6:
                        month = "JUN";
                        break;
                    case 7:
                        month = "JUL";
                        break;
                    case 8:
                        month = "Aug";
                        break;
                    case 9:
                        month = "SEP";
                        break;
                    case 10:
                        month = "OCT";
                        break;
                    case 11:
                        month = "NOV";
                        break;
                    case 12:
                        month = "DEC";
                        break;
                    default:
                        month = "";
                        break;
            }
            return month;

        }

        #endregion
        public APIResult CreateEmployee(PDSEmployee input,bool isEmployee=false)
        {
            APIResult result = new APIResult();
            DataBaseResult dbr = new DataBaseResult();
            try 
            {
                int age = 0;
                string empage = input.EmpAge;
                bool success = int.TryParse(empage, out age);
                input.Age = (success == true) ? age : 0;
                if (!string.IsNullOrEmpty(input.PANNumber))
                    input.PANStatus = "Yes";
                else
                    input.PANStatus = "No";
                Tuple<string, bool> vald = System.Tuple.Create("", false);
                vald = this.ValidateEmpModel(input);
                bool isValid = vald.Item2;
                if (isValid)
                {
                    dbr.ds = new System.Data.DataSet();
                    if (isEmployee)
                    {
                        dbr = ops.CheckEmpCodeExists(input.EmpCode, true);
                        if (!dbr.IsExists)
                        {
                            dbr = new DataBaseResult();
                            dbr.ds = new System.Data.DataSet();
                            dbr = ops.CreateEmployee(input, isEmployee);
                            result.EmployeeName = dbr.EmployeeName;
                            result.Id = dbr.Id;
                        }
                        result.Message = dbr.Message;
                        result.Status = dbr.Status;
                       // result.CommandType = "INSERT";

                    }
                    else
                    {
                        dbr = ops.CheckEmpCodeExists(input.EmpCode, false);
                        if (!dbr.IsExists)
                        {
                            dbr = new DataBaseResult();
                            dbr.ds = new System.Data.DataSet();
                            dbr = ops.CreateCDAEmployee(input);
                            result.Id = dbr.Id;
                            result.EmployeeName = dbr.EmployeeName;
                            result.Message = dbr.Message;
                            result.Status = dbr.Status;

                        }
                        else
                        {
                            result.Message = dbr.Message;
                            result.Status = false;
                        }
                       
                    }
                }
                else
                {
                    result.Message = vald.Item1;
                    result.Status = false;
                }
                result.CommandType = "INSERT";


            }
            catch (Exception e)
            {
                result.Message = e.Message;
                result.Status = false;
                result.CommandType = "INSERT";
                result.Id = 0;
                result.EmployeeName = "";
                throw e;


            }
            return result;

        }
        public APIResult CreateMainEmployee(PDSEmployee input)
        {
            APIResult result = new APIResult();
            DataBaseResult dbr = new DataBaseResult();
            try
            {
                int age = 0;
                string empage = input.EmpAge;
                bool success = int.TryParse(empage, out age);
                input.Age = (success == true) ? age : 0;
                dbr.ds = new System.Data.DataSet();
                dbr = ops.CheckMainEmpCodeExists(input.EmpCode);
                if (!dbr.IsExists)
                {
                    dbr = new DataBaseResult();
                    dbr.ds = new System.Data.DataSet();
                    dbr = ops.CreateMainEmployee(input);
                    result.EmployeeName = dbr.EmployeeName;
                    result.Id = dbr.Id;
                }
                result.Message = dbr.Message;
                result.Status = dbr.Status;
                result.CommandType = "INSERT";

            }
            catch (Exception e)
            {
                result.Message = e.Message;
                result.Status = false;
                result.CommandType = "INSERT";
                result.Id = 0;
                result.EmployeeName = "";
                throw e;


            }
            return result;

        }
        public APIResult CheckUserExists(string userName)
        {
            APIResult result = new APIResult();
            DataBaseResult dbr = new DataBaseResult();
            try
            {
                dbr.ds = new System.Data.DataSet();
                dbr = ops.CheckUserExists(userName);
                result.Message = dbr.Message;
                result.Status = dbr.Status;
                result.CommandType = dbr.CommandType;

            }
            catch (Exception e)
            {
                result.Message = e.Message;
                result.Status = false;
                result.CommandType = "SELECT";
                result.Id = 0;
                result.EmployeeName = "";
                throw e;
            }
            return result;
        }
        public APIResult CheckMainEmpCodeExists(string empCode)
        {
            APIResult result = new APIResult();
            DataBaseResult dbr = new DataBaseResult();
            try
            {
                dbr.ds = new System.Data.DataSet();
                dbr = ops.CheckMainEmpCodeExists(empCode);
                result.Message = dbr.Message;
                result.Status = dbr.Status;
                result.CommandType = dbr.CommandType;

            }
            catch (Exception e)
            {
                result.Message = e.Message;
                result.Status = false;
                result.CommandType = "SELECT";
                result.Id = 0;
                result.EmployeeName = "";
                throw e;
            }
            return result;
        }

        public APIResult CheckEmpCodeExists(string empCode,bool isEmployee)
        {
            APIResult result = new APIResult();
            DataBaseResult dbr = new DataBaseResult();
            try
            {
                dbr.ds = new System.Data.DataSet();
                dbr = ops.CheckEmpCodeExists(empCode,isEmployee);
                result.Message = dbr.Message;
                result.Status = dbr.Status;
                result.CommandType = dbr.CommandType;

            }
            catch (Exception e)
            {
                result.Message = e.Message;
                result.Status = false;
                result.CommandType = "SELECT";
                result.Id = 0;
                result.EmployeeName = "";
                throw e;
            }
            return result;
        }
        public APIResult RegisterEmployee(PDSEmployee input)
        {
            APIResult result = new APIResult();
            DataBaseResult dbr = new DataBaseResult();
            try
            {
                Tuple<string, bool> vald = System.Tuple.Create("", false);
                vald = this.ValidateEmpModel(input,true);
                bool isValid = vald.Item2;
                if (isValid)
                {
                    // input.Password = input.Phone.Substring(0, 4)+input.DOB.Substring((input.DOB.Length - 4), 4);
                    input.Password = input.Phone.Substring(0, 4) + input.DOB.Substring(0, 4);
                    dbr.ds = new System.Data.DataSet();
                    dbr = ops.RegisterEmployee(input);
                    result.Message = dbr.Message;
                    result.Status = dbr.Status;
                    result.Id = dbr.Id;
                    result.EmployeeName = dbr.EmployeeName;
                    result.CommandType = dbr.CommandType;
                }
                else
                {
                    result.Message = vald.Item1;
                    result.Status = false;
                }

            }
            catch (Exception e)
            {
                result.Message = e.Message;
                result.Status = false;
                result.CommandType = "INSERT";
                result.Id = 0;
                result.EmployeeName = "";
                throw e;


            }
            result.CommandType = "INSERT";
            return result;

        }

        public APIResult BackupList(bool isProduction)
        {
            APIResult result = new APIResult();
            List<DbBackupInfo> backupInfos = new List<DbBackupInfo>();
            result.dbBackups = new List<DbBackupInfo>();
            string pathbackup = "";
            try
            {
                if(!isProduction)
                    pathbackup = configuration["backuppath"];
                else
                    pathbackup = configuration["pbackuppath"];
                DirectoryInfo d = new DirectoryInfo(pathbackup);//Assuming Test is your Folder
                FileInfo[] Files = d.GetFiles("*.sql"); //Getting Text files
                foreach (FileInfo file in Files)
                {
                    DbBackupInfo info = new DbBackupInfo();
                    info.fileName = file.Name;
                    info.FilePath = file.FullName;
                    string dstring = this.Between(info.fileName, "Backup_", ".sql");
                    if(!string.IsNullOrEmpty(dstring))
                    {
                        string year = dstring.Substring(0, 4);
                        string month = dstring.Substring(4, 2);
                        string date = dstring.Substring(6, 2);
                        string cd = month + "/" + date + "/" + year;
                        info.CreatedDate = cd;
                    }
                    
                    //info.CreatedDate = 
                    backupInfos.Add(info);
                }
                
                if (backupInfos.Count > 0)
                {
                    result.dbBackups = backupInfos;
                    result.CommandType = "Backup";
                    result.Message = "Info retreived Successfully!!!";
                    result.Status = true;
                }
                else
                {
                    result.CommandType = "Backup";
                    result.Message = "NO file found!!!";
                    result.Status = false;

                }

            }
            catch (Exception e)
            {
                result.CommandType = "Backup";
                result.Message = e.Message;
                result.Status = false;
                throw e;
            }
            return result;
        }
        public string Between(string STR, string FirstString, string LastString)
        {
            string FinalString;
             if(!string.IsNullOrEmpty(STR)&& !string.IsNullOrEmpty(STR)&& !string.IsNullOrEmpty(STR))
            {
                int Pos1 = STR.IndexOf(FirstString) + FirstString.Length;
                int Pos2 = STR.IndexOf(LastString);
                FinalString = STR.Substring(Pos1, Pos2 - Pos1);
            }
            else
            {
                FinalString = "";
            }

            return FinalString;
        }

        public string TraceError(ErrorLogTrack log)
        {
            string result = "";
            try
            {
                int changes = 0;
                changes = ops.TraceError(log);
                if(changes > 0)
                {
                    result = "Error Traced Successfully!!";
                }
            }
            catch(Exception e)
            {
                result = "Error not traced for reason : "  +e.Message+" !!!";
            }
            return result;
        }
        public APIResult RestoreDatabase(string file,bool isProduction)
        {
            APIResult result = new APIResult();
            DataBaseResult dbr = new DataBaseResult();
            try
            {
                string connection = DBConnection.GetDBConnection(isProduction);
                dbr = ops.RestoreDB(file,connection);
                result.CommandType = dbr.CommandType;
                result.Message = dbr.Message;
                result.Status = dbr.Status;
            }
            catch(Exception e)
            {
                result.CommandType = "Restore";
                result.Message = e.Message;
                result.Status = false;
                throw e;
            }
            return result;
        }
        public UploadStatus ReadExcelFileToDataSet(string path)
        {
            UploadStatus status = new UploadStatus();
            List<string> columnNames = new List<string>();
            status.ds = new System.Data.DataSet();
            try
            {
                System.Data.DataTable dt = new System.Data.DataTable();
                dt.Clear();
               // dt.Columns.Add("UserType");
                //Lets open the existing excel file and read through its content . Open the excel using openxml sdk
                using (SpreadsheetDocument doc = SpreadsheetDocument.Open(path, false))
                {
                    //create the object for workbook part  
                    WorkbookPart workbookPart = doc.WorkbookPart;
                    Sheets thesheetcollection = workbookPart.Workbook.GetFirstChild<Sheets>();
                    int i = 0;
                    //using for each loop to get the sheet from the sheetcollection  
                    foreach (Sheet thesheet in thesheetcollection)
                    {
                        if (i > 0)
                            break;
                        i = 1;
                        //statement to get the worksheet object by using the sheet id  
                        Worksheet theWorksheet = ((WorksheetPart)workbookPart.GetPartById(thesheet.Id)).Worksheet;
                        SheetData thesheetdata = (SheetData)theWorksheet.GetFirstChild<SheetData>();
                        status.employees = new List<PDSEmployee>();
                        foreach (Row thecurrentrow in thesheetdata)
                        {
                            System.Data.DataRow dr = dt.NewRow();
                            PDSEmployee emp = new PDSEmployee();
                            foreach (Cell thecurrentcell in thecurrentrow)
                            {
                                //statement to take the integer value  
                                string currentcellvalue = string.Empty;
                                if (thecurrentcell.DataType != null)
                                {
                                    if (thecurrentcell.DataType == CellValues.SharedString)
                                    {
                                        int id;
                                        if (Int32.TryParse(thecurrentcell.InnerText, out id))
                                        {
                                            SharedStringItem item = workbookPart.SharedStringTablePart.SharedStringTable.Elements<SharedStringItem>().ElementAt(id);
                                            if (item.Text != null)
                                            {
                                                currentcellvalue = item.Text.Text;
                                                //code to take the string value  
                                                //excelResult.Append(item.Text.Text + " ");
                                            }
                                            else if (item.InnerText != null)
                                            {
                                                currentcellvalue = item.InnerText;
                                            }
                                            else if (item.InnerXml != null)
                                            {
                                                currentcellvalue = item.InnerXml;
                                            }
                                        }
                                    }
                                    if (currentcellvalue.ToLower().Contains("sno") || currentcellvalue.ToLower().Contains("s.no"))
                                        continue;
                                    dt.Columns.Add(currentcellvalue.ToLower().Trim());
                                }
                                else
                                {
                                    if(columnNames.Count == 0)
                                        columnNames = dt.Columns.Cast<System.Data.DataColumn>().Select(x => x.ColumnName).ToList<string>();                                   
                                    foreach(var item in columnNames)
                                    {
                                        try
                                        {
                                            dr[item] = thecurrentcell.InnerText;
                                            columnNames.Remove(item);
                                            break;
                                        }
                                        catch(Exception e)
                                        {
                                            string msg = e.Message;
                                            throw;
                                        }
                                    }
                                    if (columnNames.Count == 0)
                                        break;
                                    //dr["UserType"] = usrType;
                                    //excelResult.Append(Convert.ToInt16(thecurrentcell.InnerText) + " ");
                                }

                            }
                           
                            // excelResult.AppendLine();
                        }
                        status.ds.Tables.Add(dt);
                        //excelResult.Append("");
                    }
                }
                status.uploadStatus = true;
            }
            catch (Exception e)
            {
                status.ErrorMessage = e.Message;
                status.uploadStatus = false;
            }
            return status;
        }
        public UploadStatus ReadExceltoDataSet(string path, List<string> headers,bool getHeaders=false)      
        {
            UploadStatus u = new UploadStatus();
            u.headers = new List<string>();
            u.ds = new System.Data.DataSet();
            try
            {
                System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                using (var stream = File.Open(path, FileMode.Open, FileAccess.Read))
                {
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        System.Data.DataTable dt = new System.Data.DataTable();
                        dt.Clear();
                        List<string> columnNames = new List<string>();
                        int i = 0;
                        int j = 0;
                        do
                        {
                            while (reader.Read() && (i==0 || !getHeaders)) //Each ROW
                            {
                                i = 1;
                                System.Data.DataRow dr = dt.NewRow();

                                for (int column = 0; column < reader.FieldCount; column++)
                                {
                                    try
                                    {
                                        if (getHeaders)
                                        {
                                            object msg = reader.GetValue(column);
                                            Type celltype = msg.GetType();
                                            string currentcellvalue = msg.ToString();
                                            if (currentcellvalue.ToLower().Contains("sno") || currentcellvalue.ToLower().Contains("s.no"))
                                                continue;
                                            u.headers.Add(currentcellvalue);
                                        }
                                        else
                                        {
                                           
                                            if (headers.Count > 0)
                                            {
                                                foreach(var item in headers)
                                                {
                                                    dt.Columns.Add(item);
                                                }
                                                headers.Clear();
                                                 break;
                                            }
                                            else
                                            {
                                               
                                                if (j == 0)
                                                {
                                                    j = 1;
                                                    continue;
                                                }
                                                if(columnNames.Count == 0)
                                                    columnNames = dt.Columns.Cast<System.Data.DataColumn>().Select(x => x.ColumnName).ToList<string>();
                                                object msg = reader.GetValue(column);
                                                Type celltype = msg.GetType();

                                                foreach (var item in columnNames)
                                                {
                                                    if (celltype.FullName.ToLower().Contains("string"))
                                                    {
                                                        string currentcellvalue = msg.ToString();
                                                        dr[item] = currentcellvalue;
                                                        columnNames.Remove(item);
                                                        break;
                                                    }
                                                    else if (celltype.FullName.ToLower().Contains("int"))
                                                    {
                                                        int val = 0;
                                                        bool success = int.TryParse(msg.ToString(), out val);
                                                        if (success)
                                                            dr[item] = val;
                                                        else
                                                            dr[item] = 0;
                                                        columnNames.Remove(item);
                                                        break;

                                                    }
                                                    else if (celltype.FullName.ToLower().Contains("date"))
                                                    {
                                                        DateTime val = new DateTime();
                                                        bool success = DateTime.TryParse(msg.ToString(), out val);
                                                        if (success)
                                                            dr[item] = val;
                                                        else
                                                            dr[item] = 0;
                                                        columnNames.Remove(item);
                                                        break;

                                                    }
                                                    else if (celltype.FullName.ToLower().Contains("double"))
                                                    {
                                                        double val = 0;
                                                        bool success = double.TryParse(msg.ToString(), out val);
                                                        if (success)
                                                            dr[item] = val;
                                                        else
                                                            dr[item] = 0;
                                                        columnNames.Remove(item);
                                                        break;

                                                    }
                                                    else if (celltype.FullName.ToLower().Contains("bool"))
                                                    {
                                                        bool val = false;
                                                        bool success = bool.TryParse(msg.ToString(), out val);
                                                        if (success)
                                                            dr[item] = val;
                                                        else
                                                            dr[item] = false;
                                                        columnNames.Remove(item);
                                                        break;

                                                    }
                                                    else
                                                    {
                                                        dr[item] = null;
                                                        columnNames.Remove(item);
                                                        break;
                                                    }
                                                        
                                                    
                                                }
                                                if (columnNames.Count == 0)
                                                {
                                                    dt.Rows.Add(dr);
                                                    break;
                                                }
                                            }

                                        }
                                     }
                                    catch(Exception e)
                                    {
                                        string m = e.Message;
                                        throw;
                                    }
                                }
                                if (getHeaders)
                                {
                                    u.uploadStatus = true;
                                    break;
                                }

                            }
                            if (!getHeaders)
                            {
                                u.ds.Tables.Add(dt);
                                u.uploadStatus = true;
                                break;
                            }
                        } while (reader.NextResult() && i==0); //Move to NEXT SHEET
                        
                    }
                }
            }
            catch(Exception e)
            {
                u.uploadStatus = false;
                string msg = e.Message;
                u.ds = new System.Data.DataSet();
            }
            return u;
        }
        public UploadStatus ReadExcelFile(string path, bool getHeaders = false)
        {
            UploadStatus status = new UploadStatus();
            APIResult result = new APIResult();
            result.uploadStatus = new UploadStatus();
            result.uploadStatus.headers = new List<string>();
            try
            {
                status.headers = new List<string>();
                result = this.GetConstants();
                //Lets open the existing excel file and read through its content . Open the excel using openxml sdk
                using (SpreadsheetDocument doc = SpreadsheetDocument.Open(path, false))
                {
                    //create the object for workbook part  
                    WorkbookPart workbookPart = doc.WorkbookPart;
                    Sheets thesheetcollection = workbookPart.Workbook.GetFirstChild<Sheets>();
                    int i = 0;
                    //using for each loop to get the sheet from the sheetcollection  
                    foreach (Sheet thesheet in thesheetcollection)
                    {
                        if (i > 0)
                            break;
                        i = 1;
                        //statement to get the worksheet object by using the sheet id  
                        Worksheet theWorksheet = ((WorksheetPart)workbookPart.GetPartById(thesheet.Id)).Worksheet;
                        SheetData thesheetdata = (SheetData)theWorksheet.GetFirstChild<SheetData>();
                        status.employees = new List<PDSEmployee>();
                        foreach (Row thecurrentrow in thesheetdata)
                        {
                            PDSEmployee emp = new PDSEmployee();
                            foreach (Cell thecurrentcell in thecurrentrow)
                            {
                                bool valueset = false;
                                //statement to take the integer value  
                                string currentcellvalue = string.Empty;
                                if (thecurrentcell.DataType != null)
                                {
                                    if (thecurrentcell.DataType == CellValues.SharedString)
                                    {
                                        int id;
                                        if (Int32.TryParse(thecurrentcell.InnerText, out id))
                                        {
                                            SharedStringItem item = workbookPart.SharedStringTablePart.SharedStringTable.Elements<SharedStringItem>().ElementAt(id);
                                            if (item.Text != null)
                                            {
                                                currentcellvalue = item.Text.Text;
                                                //code to take the string value  
                                                //excelResult.Append(item.Text.Text + " ");
                                            }
                                            else if (item.InnerText != null)
                                            {
                                                currentcellvalue = item.InnerText;
                                            }
                                            else if (item.InnerXml != null)
                                            {
                                                currentcellvalue = item.InnerXml;
                                            }
                                        }
                                    }
                                    if (currentcellvalue.ToLower().Contains("sno") || currentcellvalue.ToLower().Contains("s.no") || currentcellvalue.ToLower().EndsWith("no"))
                                        continue;

                                    status.headers.Add(currentcellvalue.ToLower().Trim());
                                }
                                else
                                {
                                    if (!getHeaders)
                                    {
                                       
                                        if (result.uploadStatus.columns.Count > 0)
                                        {
                                            foreach (var item in result.uploadStatus.columns)
                                            {
                                                valueset = false;
                                                Type type = emp.GetType();
                                                PropertyInfo[] props = type.GetProperties();
                                                foreach (var prop in props)
                                                {
                                                    try
                                                    {
                                                        if (item == prop.Name.ToLower().Trim())
                                                        {
                                                            if (prop.PropertyType == typeof(string))
                                                            {                                                                
                                                                prop.SetValue(emp, thecurrentcell.InnerText??"");
                                                                valueset = true;
                                                                break;
                                                            }
                                                            else if (prop.PropertyType == typeof(int))
                                                            {
                                                                int val = 0;
                                                                bool success = int.TryParse(thecurrentcell.InnerText, out val);
                                                                if (success == true)
                                                                    prop.SetValue(emp, val);
                                                                else
                                                                    prop.SetValue(emp, 0);
                                                                valueset = true;
                                                                break;
                                                            }
                                                            else if (prop.PropertyType == typeof(bool))
                                                            {
                                                                bool val = false;
                                                                bool success = bool.TryParse(thecurrentcell.InnerText, out val);
                                                                if (success == true)
                                                                    prop.SetValue(emp, val);
                                                                else
                                                                    prop.SetValue(emp, false);
                                                                valueset = true;
                                                                break;
                                                            }

                                                        }
                                                            
                                                    }
                                                    catch(Exception e)
                                                    {
                                                        valueset = false;
                                                        string msg = e.Message;
                                                        throw;
                                                    }
                                                }
                                                if (valueset) 
                                                {
                                                   // result.uploadStatus.columns.Remove(item);
                                                    break; 
                                                }
                                            }
                                            status.uploadStatus = true;
                                        }
                                        else
                                        {
                                            status.uploadStatus = false;
                                            status.ErrorMessage = "unable to get columns";
                                        }
                                    }
                                    //excelResult.Append(Convert.ToInt16(thecurrentcell.InnerText) + " ");
                                }
                                
                            }
                            if (getHeaders)
                                break;
                            if (!getHeaders && status.uploadStatus)
                                status.employees.Add(emp);

                            // excelResult.AppendLine();
                        }
                        //excelResult.Append("");
                    }
                }
                status.uploadStatus = true;
            }
            catch (Exception e)
            {
                status.ErrorMessage = e.Message;
                status.uploadStatus = false;
            }
            return status;
        }
        public List<Tuple<string, bool>> GetColumnsForExcel()
        {
            List<Tuple<string, bool>> columns = new List<Tuple<string, bool>>();
            DataBaseResult dbr = new DataBaseResult();
            try
            {
                dbr = ops.GetHeadersforExcel();
                int count = 0;
                if (dbr.ds.Tables.Count > 0)
                {
                    count = dbr.ds.Tables[0].Rows.Count;
                    if (count > 0)
                    {

                        for (int i = 0; i < count; i++)
                        {
                            Tuple<string, bool> column = System.Tuple.Create("", false);
                            string col = dbr.ds.Tables[0].Rows[i]["ColumnName"].ToString();
                            bool mandatory = Convert.ToBoolean(dbr.ds.Tables[0].Rows[i]["isMandatory"]);
                            column = System.Tuple.Create(col.ToLower().Trim(), mandatory);
                            columns.Add(column);
                        }
                    }
                    else
                    {
                        columns = new List<Tuple<string, bool>>();
                    }
                }
                else
                {
                    columns = new List<Tuple<string, bool>>();
                }
            }
            catch(Exception e)
            {
                string ms = e.Message;
                columns = new List<Tuple<string, bool>>();
            }
            return columns;
        }
        public  List<PDSEmployee> GetPDSEmployeesFromDataset(System.Data.DataSet ds)
        {
            List<PDSEmployee> employees = new List<PDSEmployee>();
            try
            {
                if(ds.Tables.Count>0)
                {
                    if(ds.Tables[0].Rows.Count>0)
                    {
                        try
                        {
                            List<string> columnNames = ds.Tables[0].Columns.Cast<System.Data.DataColumn>().Select(x => x.ColumnName).ToList<string>();
                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                PDSEmployee emp = new PDSEmployee();
                                Type type = emp.GetType();
                                PropertyInfo[] props = type.GetProperties();
                                //bool valueSet = false;
                                foreach (var prop in props)
                                {
                                    foreach(var item in columnNames)
                                    {
                                        try
                                        {
                                            if (item.ToLower().Trim() == prop.Name.ToLower().Trim())
                                            {
                                                var valu = ds.Tables[0].Rows[i][item].ToString();
                                                if (prop.PropertyType == typeof(string))
                                                {
                                                    prop.SetValue(emp, valu ?? "");
                                                    //valueSet = true;
                                                    columnNames.Remove(item);
                                                    break;
                                                }
                                                else if (prop.PropertyType == typeof(int))
                                                {
                                                    int val = 0;
                                                    bool success = int.TryParse(valu, out val);
                                                    if (success == true)
                                                        prop.SetValue(emp, val);
                                                    else
                                                        prop.SetValue(emp, 0);
                                                    // valueSet = true;
                                                    columnNames.Remove(item);
                                                    break;
                                                }
                                                else if (prop.PropertyType == typeof(bool))
                                                {
                                                    bool val = false;
                                                    bool success = bool.TryParse(valu, out val);
                                                    if (success == true)
                                                        prop.SetValue(emp, val);
                                                    else
                                                        prop.SetValue(emp, false);
                                                    // valueSet = true;
                                                    columnNames.Remove(item);
                                                    break;
                                                }
                                            }
                                        }
                                        catch(Exception e)
                                        {
                                            string msg = e.Message;
                                            throw;
                                        }
                                    }
                                    if (columnNames.Count == 0)
                                        break;
                                    //if (valueSet)
                                    //    break;
                                }
                                employees.Add(emp);
                            }

                        }
                        catch (Exception e)
                        {
                            string msg = e.Message;
                            throw;
                        }

                    }
                    else
                    {
                        employees = new List<PDSEmployee>();
                    }
                }
                else
                {
                    employees = new List<PDSEmployee>();
                }

            }
            catch (Exception e)
            {
                employees = new List<PDSEmployee>();
            }
            return employees;
        }
        public Tuple<string,bool> ValidateEmpModel(PDSEmployee emp,bool isRegister=false)
        {
            Tuple<string, bool> valid = System.Tuple.Create("", false) ;
            string ErrMsg =  "";
            List<HeaderDescription> headers = new List<HeaderDescription>();
            try
            {
                List<Tuple<string, bool>> tuples = new List<Tuple<string, bool>>();
                tuples = this.GetColumnsForExcel();
                foreach (var item in tuples)
                {
                    HeaderDescription head = new HeaderDescription();
                    head.ColumnName = item.Item1;
                    head.IsMandatory = item.Item2;
                    headers.Add(head);
                }
                Type type = emp.GetType();
                PropertyInfo[] props = type.GetProperties();
                var mandateHeaders = headers.Where(x => x.IsMandatory == true).ToList<HeaderDescription>();
                foreach (var prop in props)
                {
                    foreach (var item in mandateHeaders)
                    {
                        if (item.ColumnName.ToLower().Trim() == prop.Name.ToLower().Trim())
                        {
                            if ((prop.Name.ToLower().Trim() == "empcode" || prop.Name.ToLower().Trim().StartsWith("desig") || prop.Name.ToLower().Trim().StartsWith("ref")) && isRegister)
                                continue;
                            ErrMsg = string.Format("Invalid Input!!! {0} Field is required", prop.Name);
                            var val = prop.GetValue(emp);
                            if (prop.PropertyType == typeof(string))
                            {                                
                                string v = val?.ToString();
                                if (string.IsNullOrEmpty(v))
                                    throw new Exception(ErrMsg);
                                else
                                    valid = System.Tuple.Create("", true);
                            }
                            else if (prop.PropertyType == typeof(int))
                            {
                                int v = 0;
                                bool success = int.TryParse(val.ToString(), out v);
                                if (!success)
                                    throw new Exception(ErrMsg);
                                else if (v == 0)
                                    throw new Exception(ErrMsg);
                                else
                                    valid = System.Tuple.Create("", true);

                            }
                            else if (prop.PropertyType == typeof(bool))
                            {
                                bool v = false;
                                bool success = bool.TryParse(val.ToString(), out v);
                                if (!success)
                                    throw new Exception(ErrMsg);
                                else
                                    valid = System.Tuple.Create("", true);

                            }

                        }
                    }
                }
            }
            catch (Exception e)
            {
                //string msg = e.Message;
                valid = System.Tuple.Create(e.Message, false); ;
            }
            return valid;
        }
        public bool ListComparer(List<string> list1,List<string> list2,out List<string> result)
        {
            var firstNotSecond = list1.Except(list2).ToList();
            var secondNotFirst = list2.Except(list1).ToList();
            result = firstNotSecond;
            return !firstNotSecond.Any() && !secondNotFirst.Any() && list1.Count == list2.Count;
        }
    }
}
