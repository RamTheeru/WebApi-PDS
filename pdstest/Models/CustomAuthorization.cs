using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using pdstest.BLL;
using pdstest.services;
using pdstest.DAL;
using System.Text;
using System.Security.Claims;
//using WebApi.Entities;

namespace pdstest.Models
{


    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class CustomAuthorization : Attribute, IAuthorizationFilter
    {
    
        APIResult result = new APIResult();
   

          
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            try
            {
                var userRole = context.HttpContext.Items["userrole"];
                var user = context.HttpContext.Items["user"];
                var userType = context.HttpContext.Items["usertype"];
                var msg = context.HttpContext.Items["msg"];
                var empId = context.HttpContext.Items["empId"];
                var token = context.HttpContext.Items["userToken"];
                string tk = token != null ? token.ToString() : "";
                string errmsg = msg != null ? msg.ToString() : "";
                result.userInfo = new UserType();
                UserType usr = new UserType();
                usr.User = user != null ? user.ToString() : "";
                usr.UserTypeId = userType != null ? Convert.ToInt32(userType) : 0;
                usr.Role = userRole != null ? userRole.ToString() : "";
                usr.EmployeeId = empId != null ? Convert.ToInt32(empId) : 0;
                if (string.IsNullOrEmpty(tk))
                {

                    result.Message = "Unauthorized request !!!!";
                    result.Status = false;
                    // not logged in
                    context.Result = new JsonResult(result) { StatusCode = StatusCodes.Status401Unauthorized };
                }
                else if (!string.IsNullOrEmpty(errmsg))
                {
                    StringBuilder st = new StringBuilder();
                    string m = !string.IsNullOrEmpty(errmsg) ? "; Reason : " + errmsg : string.Empty;
                    string msgage = "Invalid Token for this request ";
                    if (m.Contains("expired"))
                        msgage = msgage + "" + "Token Expired!!!,Please try Login again!!";
                    else
                        msgage = msgage + string.Empty + m;
                    st.Append(msgage);
                    result = new APIResult();
                    result = new MySQLDBOperations().DeleteSession(usr, tk, true);
                    st.Append(" Action : ");
                    st.Append(result.Message);
                    result.Message = st.ToString();
                    result.Status = false;

                    // not logged in
                    context.Result = new JsonResult(result) { StatusCode = StatusCodes.Status401Unauthorized };

                }
                else if (usr.EmployeeId == 0 || usr.UserTypeId == 0 || string.IsNullOrEmpty(usr.User))
                {
                    result = new MySQLDBOperations().GetLoginUserSessionInfoByToken(tk);
                    if (result.userInfo != null)
                    {
                        usr = result.userInfo;
                    }
                    else
                    {
                        result.Message = result.Message;
                        result.Status = result.Status;
                        // not logged in
                        context.Result = new JsonResult(result) { StatusCode = StatusCodes.Status401Unauthorized };
                    }
                }
                else if(usr.EmployeeId != 0 && usr.UserTypeId != 0 && !string.IsNullOrEmpty(usr.User))
                { 
                        result = new MySQLDBOperations().ValidateLoginUserSession(usr);
                        if (!result.userInfo.Valid)
                        {
                            result.Message = result.Message;
                            result.Status = result.Status;
                            // not logged in
                            context.Result = new JsonResult(result) { StatusCode = StatusCodes.Status401Unauthorized };
                        }
                
                }
                else
                {
                    result.Message = "Something went wrong while authorizing your request!! ";
                    result.Status = false;
                    // not logged in
                    context.Result = new JsonResult(result) { StatusCode = StatusCodes.Status401Unauthorized };


                }
            }
            catch (Exception e)
            {
                //result.userInfo.Valid = false;
                result.Status = false;
                result.Message = e.Message;
                context.Result = new JsonResult(result) { StatusCode = StatusCodes.Status401Unauthorized };

            }
        }
    }
}
