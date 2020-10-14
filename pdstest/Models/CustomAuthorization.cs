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
            var check = context.HttpContext.Items["userallow"];
            var msg = context.HttpContext.Items["msg"];
            if (check == null)
            {
                result.Message = msg != null ?"Error occured : "+msg : "Not Authorized to process this request!!";
                result.Status = false;
                // not logged in
                context.Result = new JsonResult(result) { StatusCode = StatusCodes.Status401Unauthorized };
            }
        }
    }
}
