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
