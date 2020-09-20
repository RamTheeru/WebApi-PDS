using pdstest.DAL;
using pdstest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace pdstest.BLL
{
    public class BLLogic
    {
        // DBOperations ops = new DBOperations();
        MySQLDBOperations ops = new MySQLDBOperations();
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
