using pdstest.DAL;
using pdstest.Models;
using pdstest.services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace pdstest.BLL
{
    public class BLLogic
    {
         //DBOperations ops = new DBOperations();
        private  IConnection ops;
        public BLLogic(IConnection conn)
        {
            ops = conn;
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
                //if (input.page == 0)
                //    input.page = 1;
                //if (input.pagesize == 0)
                //    input.pagesize = 5;
                dbr = ops.GetPaginationRecords(input.stationId, input.table, input.vstartDate, input.vEndDate, input.page, input.pagesize, input.status);
                
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
                            vouch.V_Date = vouch.VoucherDate.DateTimetoString();
                            vouch.PartyName = dbr.ds.Tables[0].Rows[i]["PartyName"].ToString();
                            vouch.PurposeOfPayment = dbr.ds.Tables[0].Rows[i]["PurposeOfPayment"].ToString();
                            vouchs.Add(vouch);

                        }
                        result.vouchers = vouchs;
                        result.QueryTotalCount = dbr.QueryTotalCount;
 
                    }
                    else if (input.table.ToLower() == "ledger")
                    {
                        result.ledgers = new List<Ledger>();
                        List<Ledger> ledgs = new List<Ledger>();
                        for (int i = 0; i < count; i++)
                        {
                            Ledger ledg = new Ledger();
                            ledg.IsApproved = Convert.ToBoolean(dbr.ds.Tables[0].Rows[i]["IsApproved"]);
                            int stationid = 0;
                            string sId = dbr.ds.Tables[0].Rows[i]["StationId"].ToString();
                            bool success = int.TryParse(sId, out stationid);
                            ledg.StationId = (success == true) ? stationid : 0;
                            int debit = 0;
                            string deb = dbr.ds.Tables[0].Rows[i]["Debit"].ToString();
                            success = int.TryParse(deb, out debit);
                            ledg.Debit = (success == true) ? debit : 0;
                            int credit = 0;
                            string cred = dbr.ds.Tables[0].Rows[i]["Credit"].ToString();
                            success = int.TryParse(cred, out credit);
                            ledg.Credit = (success == true) ? credit : 0;
                            ledg.VoucherNumber = dbr.ds.Tables[0].Rows[i]["VoucherNumber"].ToString();
                            ledg.VoucherStatus = dbr.ds.Tables[0].Rows[i]["VoucherStatus"].ToString();
                            int ledgerid = 0;
                            string lId = dbr.ds.Tables[0].Rows[i]["Id"].ToString();
                            success = int.TryParse(lId, out ledgerid);
                            ledg.Id = (success == true) ? ledgerid : 0;
                            DateTime vouch_Date;
                            string vDate = dbr.ds.Tables[0].Rows[i]["VoucherDate"].ToString();
                            success = DateTime.TryParse(vDate, out vouch_Date);
                            ledg.VoucherDate = (success == true) ? vouch_Date : new DateTime();
                            ledg.IsActive = Convert.ToBoolean(dbr.ds.Tables[0].Rows[i]["IsActive"]);
                            ledg.Particulars = dbr.ds.Tables[0].Rows[i]["PurposeOfPayment"].ToString();
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
                        if (p > 1)
                        {
                            double pages = Convert.ToDouble((result.QueryTotalCount / ps));
                            result.QueryPages = (int)Math.Round(pages, MidpointRounding.AwayFromZero);
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
                    count = dbr.ds.Tables[0].Rows.Count;
                    if (count > 0)
                    {
                        for (int i = 0; i < count; i++)
                        {
                            int userTypeid = 0;
                            int employeeid = 0;
                            string empId = dbr.ds.Tables[0].Rows[i]["EmployeeId"].ToString();
                            bool succ = int.TryParse(empId, out employeeid);
                            string usertype = dbr.ds.Tables[0].Rows[i]["UserType"].ToString();
                            bool success = int.TryParse(usertype, out userTypeid);
                            userTypeid = (success == true) ? userTypeid : 0;
                            employeeid = (succ == true) ? employeeid : 0;
                            user.EmployeeId = employeeid;
                            user.UserTypeId = userTypeid;
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
                        string empId = dbr.ds.Tables[0].Rows[i]["EmployeeId"].ToString();
                        bool succ = int.TryParse(empId, out employeeid);
                        string usertype = dbr.ds.Tables[0].Rows[i]["UserTypeId"].ToString();
                        bool success = int.TryParse(usertype, out userTypeid);
                        userTypeid = (success == true) ? userTypeid : 0;
                        employeeid = (succ == true) ? employeeid : 0;
                        user.EmployeeId = employeeid;
                        user.UserTypeId = userTypeid;
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
        public APIResult ApproveUser(int registerId,string status,string empCode="",int pId=0)
        {
            APIResult result = new APIResult();
            DataBaseResult dbr = new DataBaseResult();
            try
            {
                dbr.ds = new System.Data.DataSet();
                dbr = ops.ApproveUser(registerId,status,empCode,pId);
                result.Status = dbr.Status;
                result.Message = dbr.Message;
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
                dbr = ops.GetPaginationRecords(input.stationId,"daemployees",string.Empty,string.Empty,input.page,input.pagesize,string.Empty,isEmployee);
                List<Employee> emps = new List<Employee>();
                int count = 0;
                count = dbr.ds.Tables[0].Rows.Count;
                if (count > 0)
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

        public APIResult CreateEmployee(Employee input,bool isEmployee=false)
        {
            APIResult result = new APIResult();
            DataBaseResult dbr = new DataBaseResult();
            try 
            {
                dbr.ds = new System.Data.DataSet();
                dbr = ops.CreateEmployee(input,isEmployee);
                result.Message = dbr.Message;
                result.Status = dbr.Status;
                result.Id = dbr.Id;
                result.EmployeeName = dbr.EmployeeName;
                result.CommandType = dbr.CommandType;
            
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
        public APIResult RegisterEmployee(RegisterEmployee input)
        {
            APIResult result = new APIResult();
            DataBaseResult dbr = new DataBaseResult();
            try
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
    }
}
