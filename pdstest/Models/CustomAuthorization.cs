using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using pdstest.BLL;
using pdstest.services;
using pdstest.DAL;
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
                result.userInfo = new UserType();
                UserType usr = new UserType();
                usr.User = user != null ? (string)user : "";
                usr.UserTypeId = userType != null ? (int)userType : 0;
                usr.Role = userRole != null ? (string)userRole : "";
                usr.EmployeeId = empId != null ? (int)empId : 0;
                result = new MySQLDBOperations().GetLoginUserSessionInfo(usr);
                if (!result.userInfo.Valid || msg == null)
                {
                    result.Message = result.Message;
                    result.Status = result.Status;
                    // not logged in
                    context.Result = new JsonResult(result) { StatusCode = StatusCodes.Status401Unauthorized };
                }
                else if (msg != null)
                {
                    result.Message = "Something went wrong while authorizing your request!! " + Environment.NewLine + " Reason :" + msg;
                    result.Status = false;
                    // not logged in
                    context.Result = new JsonResult(result) { StatusCode = StatusCodes.Status500InternalServerError };


                }
            }
            catch (Exception e)
            {
                result.userInfo.Valid = false;
                result.Status = false;
                result.Message = e.Message;
                context.Result = new JsonResult(result) { StatusCode = StatusCodes.Status500InternalServerError };

            }
        }
    }
}
