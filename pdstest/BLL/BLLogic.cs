using pdstest.DAL;
using pdstest.Models;
using pdstest.services;
using System;
using System.Collections.Generic;
using System.Linq;
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
                result.Designations = new List<Designation>();
                result.Usertypes = new List<UserType>();
                dbr = ops.GetConstants();
                List<DropDown> dds = new List<DropDown>();
                int count = 0;
                count = dbr.ds.Tables[0].Rows.Count;
                if (count > 0)
                {
 
                    for (int i = 0; i < count; i++)
                    {
                        DropDown dd = new DropDown();
                         dd.Category = dbr.ds.Tables[0].Rows[i]["Category"].ToString();
                         dd.ConstantId = Convert.ToInt32(dbr.ds.Tables[0].Rows[i]["ConstantId"]);
                         dd.ConstantName = dbr.ds.Tables[0].Rows[i]["ConstantName"].ToString();
                        dds.Add(dd);

                    }
                    if (dds.Count > 0)
                    {
                        int designationCount = dds.Where(x => x.Category.ToLower() == "designation").Count();
                        int userTypeCount = dds.Where(x => x.Category.ToLower() == "logintype").Count();
                        if (designationCount > 0)
                        {
                            result.Designations = dds.Where(x => x.Category.ToLower() == "designation").Select(
                                a => new Designation { DesignationId = a.ConstantId, DesginationName = a.ConstantName }
                                ).ToList();
                        }
                        if (userTypeCount > 0)
                        {
                            result.Usertypes = dds.Where(x => x.Category.ToLower() == "logintype").Select(
                            a => new UserType { UserTypeId = a.ConstantId, User = a.ConstantName }
                            ).ToList();


                        }
                        if (designationCount > 0 && userTypeCount > 0)
                        {
                            result.Message = "For both UserTypes and Designations : " + dbr.Message;

                        }
                        else if (designationCount > 0 && userTypeCount == 0)
                        {
                            result.Message = "For Designations : " + dbr.Message;
                        }
                        else if (designationCount == 0 && userTypeCount > 0)
                        {
                            result.Message = "For UserTypes : " + dbr.Message;
                        }
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

        public APIResult GetRegisteredUsers(string stationCode = "")
        {
            APIResult result = new APIResult();
            DataBaseResult dbr = new DataBaseResult();
            try 
            {
                result.registerEmployees = new List<RegisterEmployee>();
                dbr = ops.GetRegisteredUsers(stationCode);
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
                
                result.userInfo = new UserType();
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
                            vouch.PartyName = dbr.ds.Tables[0].Rows[i]["PartyName"].ToString();
                            vouch.PurposeOfPayment = dbr.ds.Tables[0].Rows[i]["PurposeOfPayment"].ToString();
                            vouchs.Add(vouch);

                        }
                        result.vouchers = vouchs;


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

        public APIResult GetLoginUserInfo(string username,string password)
        {
            APIResult result = new APIResult();
            DataBaseResult dbr = new DataBaseResult();
            try
            {
                result.userInfo = new UserType();
                dbr = ops.GetLoginUserInfo(username,password);
                UserType user = new UserType();
                int count = 0;
                count = dbr.ds.Tables[0].Rows.Count;
                if (count > 0)
                {
                    for (int i = 0; i < count; i++)
                    {
                        int userTypeid = 0;
                        string usertype = dbr.ds.Tables[0].Rows[i]["UserTypeId"].ToString();
                        bool  success = int.TryParse(usertype, out userTypeid);
                        userTypeid = (success==true)? userTypeid : 0;
                        user.UserTypeId = userTypeid;
                        user.Role = dbr.ds.Tables[0].Rows[i]["LoginType"].ToString();
                        user.User = dbr.ds.Tables[0].Rows[i]["FirstName"].ToString();
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
        public APIResult ApproveUser(int registerId)
        {
            APIResult result = new APIResult();
            DataBaseResult dbr = new DataBaseResult();
            try
            {
                dbr = ops.ApproveUser(registerId);
                result.Status = dbr.Status;
                result.Message = dbr.Message;
                result.Id = dbr.Id;
                result.CommandType = dbr.CommandType;
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                result.Status = false;
                result.CommandType = "UPDATE";
                result.Id = registerId;
                throw e;

            }

            return result;
        }

        public APIResult GetEmployees(string stationCode = "")
        {
            APIResult result = new APIResult();
            DataBaseResult dbr = new DataBaseResult();
            try
            {
                result.employees = new List<Employee>();
                dbr = ops.GetEmployees(stationCode);
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
                dbr = ops.InsertVoucher(input);
                result.Message = dbr.Message;
                result.Status = dbr.Status;
                result.Id = dbr.Id;
                result.VoucerNumber = dbr.VoucherNumber;
                result.CommandType = dbr.CommandType;

            }
            catch (Exception e)
            {
                result.Message = e.Message;
                result.Status = false;
                result.CommandType = "INSERT";
                result.Id = 0;
                result.VoucerNumber = "";
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

        public APIResult CreateEmployee(Employee input)
        {
            APIResult result = new APIResult();
            DataBaseResult dbr = new DataBaseResult();
            try 
            {
                dbr = ops.CreateEmployee(input);
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
        public APIResult RegisterEmployee(RegisterEmployee input)
        {
            APIResult result = new APIResult();
            DataBaseResult dbr = new DataBaseResult();
            try
            {
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
