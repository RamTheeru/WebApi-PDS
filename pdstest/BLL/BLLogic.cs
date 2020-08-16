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
        public APIResult GetuserTypes()
        {
            APIResult result = new APIResult();
            DataBaseResult dbr = new DataBaseResult();
            try 
            {
                dbr = ops.GetUserTypes();
                List<UserType> usertypes = new List<UserType>();
                int count = 0;
                count = dbr.ds.Tables[0].Rows.Count;
                if (count > 0)
                {

                    for (int i = 0; i < count; i++)
                    {
                        UserType ut = new UserType();
                        ut.UserTypeId = Convert.ToInt32(dbr.ds.Tables[0].Rows[i]["UserTypeId"]);
                        ut.User = dbr.ds.Tables[0].Rows[i]["Username"].ToString();
                        usertypes.Add(ut);
                    }
                    result.Message = dbr.Message;
                    result.Status = dbr.Status;
                    result.CommandType = dbr.CommandType;
                    result.usersTypes = usertypes;

                }
                else if(count == 0)
                {
                    result.Message = dbr.Message;
                    result.Status = dbr.Status;
                    result.CommandType = dbr.CommandType;
                    result.usersTypes = usertypes;

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

        public APIResult CreateEmployee(RegisterEmployee input)
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
    }
}
