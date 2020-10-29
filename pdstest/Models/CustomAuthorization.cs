using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
//using WebApi.Entities;

namespace pdstest.Models
{


    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class CustomAuthorization : Attribute, IAuthorizationFilter
    {
        APIResult result = new APIResult();
        public void OnAuthorization(AuthorizationFilterContext context)
        {
           var userRole =  context.HttpContext.Items["userrole"] ;
           var user =  context.HttpContext.Items["user"] ;
           var userType =  context.HttpContext.Items["usertype"];
            var msg = context.HttpContext.Items["msg"];
            if (userRole != null && user != null && userType != null && msg == null)
            {
                result.Message = "You are not Authorized to process this request. Please try login again!!!";
                result.Status = false;
                // not logged in
                context.Result = new JsonResult(result) { StatusCode = StatusCodes.Status401Unauthorized };
            }
            else 
            {
                result.Message = "Something went wrong while authorizing your request!! " + Environment.NewLine + " Reason :" + msg;
                result.Status = false;
                // not logged in
                context.Result = new JsonResult(result) { StatusCode = StatusCodes.Status500InternalServerError };


            }
        }
    }
}
