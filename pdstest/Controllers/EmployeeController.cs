using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using pdstest.Models;
using pdstest.BLL;
using Microsoft.Extensions.Configuration;

namespace pdstest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        //private readonly IConfiguration configuration;
        //public EmployeeController(IConfiguration config) {
        //    this.configuration = config;
        
        //}
        BLLogic logic = new BLLogic();

        [HttpGet("Constants")]

        public IActionResult GetConstants()        
        {
            APIResult result = new APIResult();
            try 
            {
                
                result = logic.GetConstants();

            }
            catch (Exception e)
            {
                result.Message = e.Message;
                result.Status = false;
                result.CommandType = "Select";

            }
            return new CustomResult(result);

        }

        [HttpPost]
        [Route("CreateEmployee")]
        public IActionResult CreateEmployee(RegisterEmployee obj)
        {
            APIResult result = new APIResult();
            try
            {
                if (!(string.IsNullOrEmpty(obj.FirstName)) || !(string.IsNullOrEmpty(obj.UserType)))
                {
                    result = logic.CreateEmployee(obj);

                }
                else
                {
                    result.Message = "Invalid Input!!!";
                    result.Status = false;
                    result.CommandType = "INSERT";
                    result.Id = 0;
                    result.EmployeeName = "";


                }
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                result.Status = false;
                result.CommandType = "INSERT";
                result.Id = 0;
                result.EmployeeName = "";

            }
            return new CustomResult(result);

        }

    }
}
