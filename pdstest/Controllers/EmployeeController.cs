using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using pdstest.Models;
using pdstest.BLL;
using Microsoft.Extensions.Configuration;
using pdstest.services;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;

namespace pdstest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IConfiguration configuration;
        //public EmployeeController(IConfiguration config) {
        //    this.configuration = config;

        //}
        private static IConnection conn;
        private BLLogic logic;
        public EmployeeController(IConnection con,IConfiguration config)
        {

            conn = con;
            configuration = config;
            logic = new BLLogic(conn);
        }
        [HttpGet("Login")]
        public IActionResult Login(string username,string password)
        {
            APIResult result = new APIResult();
            string token = "";
            result.userInfo = new UserType();
            try
            {
                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                {
                    result.Message = "Invalid Input!!!";
                    result.Status = false;
                    result.CommandType = "SELECT";
                    result.EmployeeName = "";
                    return StatusCode(StatusCodes.Status400BadRequest, result);

                }
                if (username != null)
                    username = username.Trim('"'); ;
                if (password != null)
                    password = password.Trim('"'); ;
                result = logic.GetLoginUserInfo(username, password);
                if (!string.IsNullOrEmpty(result.userInfo.User))
                {
                    if (result.userInfo.Valid)
                    {
                        token = GenerateJSONWebToken(result.userInfo);
                    }
                    else
                    {
                        result.Message = "User Authentication failed!!!";
                        result.Status = false;
                        result.CommandType = "SELECT";
                        result.EmployeeName = username;

                    }
                    if (string.IsNullOrEmpty(token))
                    {
                        result.Message = "Something Went Wrong : Session cant be generated for this user";
                        result.Status = false;
                        result.CommandType = "SELECT";
                        result.EmployeeName = username;

                    }
                    result.Token = token;
                    result.Status = true;
                    result.CommandType = "SELECT";
                    result.EmployeeName = username;
                    result.Message = "User Authenticated Sucessfully with Session!!!!";
                }
                else 
                {
                    result.Message = "No User Found : User Authentication failed!!!";
                    result.Status = false;
                    result.CommandType = "SELECT";
                    result.EmployeeName = username;

                }

            }
            catch (Exception e)
            {
                result.Message = e.Message;
                result.Status = false;
                result.CommandType = "Select";
                return StatusCode(StatusCodes.Status500InternalServerError, result);
            }
            return Ok(result);
        
        }

        private string GenerateJSONWebToken(UserType user)
        {
            string tkn = "";
            try
            {
                var key = Encoding.UTF8.GetBytes(configuration["Jwt:Key"]);
                var securityKey = new SymmetricSecurityKey(key);
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
                var claims = new[]
                {
                new Claim(JwtRegisteredClaimNames.Sub,user.User),
                new Claim(JwtRegisteredClaimNames.Email,user.User),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
             };
                var token = new JwtSecurityToken(
                    issuer: configuration["Jwt:Issuer"],
                    audience: configuration["Jwt:Issuer"],
                    claims,
                    expires: DateTime.Now.AddMinutes(5),
                    signingCredentials: credentials);

                var encodeToken = new JwtSecurityTokenHandler().WriteToken(token);
                tkn = encodeToken;
            }
            catch (Exception e)
            {
                tkn = "";
            }
            return tkn;
        }


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
                return StatusCode(StatusCodes.Status500InternalServerError, result);
            }
            //return new CustomResult(result);
            return Ok(result);

        }

      
        [HttpGet("RegisteredUsers")]
        //[Authorize(AuthenticationSchemes = "Bearer")]
        [CustomAuthorization]
        public IActionResult GetRegisteredUsers(string stationCode="")
        {
            APIResult result = new APIResult();

            try
            {
   
                    if (stationCode != null)
                        stationCode = stationCode.Replace(@"\", "");
                    result = logic.GetRegisteredUsers(stationCode);
                

            }
            catch (Exception e)
            {
                result.Message = e.Message;
                result.Status = false;
                result.CommandType = "Select";
                return StatusCode(StatusCodes.Status500InternalServerError, result);

            }
            return Ok(result);
            //return new CustomResult(result);

        }
        [HttpPut("ApproveUser")]
        public IActionResult ApproveUser(string registerId)
        {
            APIResult result = new APIResult();
            int regId = 0;
            bool success = false;
            try
            {
                success = int.TryParse(registerId, out regId);
                if (string.IsNullOrEmpty(registerId) || regId == 0 || !success)
                {
                    result.Message = "Invalid Input!!!";
                    result.Status = false;
                    result.CommandType = "UPDATE";
                    result.Id = regId;
                    result.EmployeeName = "";
                    return StatusCode(StatusCodes.Status400BadRequest, result);

                }
                else 
                {
                    result = logic.ApproveUser(regId);
                }  

            }
            catch (Exception e)
            {
                result.Message = e.Message;
                result.Status = false;
                result.CommandType = "UPDATE";
                return StatusCode(StatusCodes.Status500InternalServerError, result);
            }
            return Ok(result);
            //return new CustomResult(result);

        }

        [HttpGet("Employees")]

        public IActionResult GetEmployees(string stationCode = "")
        {
            APIResult result = new APIResult();
            try
            {
                if(stationCode != null)
                    stationCode = stationCode.Replace(@"\", "");
                result = logic.GetEmployees(stationCode);

            }
            catch (Exception e)
            {
                result.Message = e.Message;
                result.Status = false;
                result.CommandType = "Select";
                return StatusCode(StatusCodes.Status500InternalServerError, result);
            }
            return Ok(result);
            //return new CustomResult(result);

        }

        [HttpPost]
        [Route("RegisterEmployee")]
        public IActionResult RegisterEmployee(RegisterEmployee obj)
        {
            APIResult result = new APIResult();
            try
            {
                if (!(string.IsNullOrEmpty(obj.FirstName)) || !(string.IsNullOrEmpty(obj.UserType)))
                {
                    obj.IsRegister = true;
                    result = logic.RegisterEmployee(obj);

                }
                else
                {
                    result.Message = "Invalid Input!!!";
                    result.Status = false;
                    result.CommandType = "INSERT";
                    result.Id = 0;
                    result.EmployeeName = "";
                    return StatusCode(StatusCodes.Status400BadRequest, result);

                }
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                result.Status = false;
                result.CommandType = "INSERT";
                result.Id = 0;
                result.EmployeeName = "";
                return StatusCode(StatusCodes.Status500InternalServerError, result);
            }
            return Ok(result);
            //return new CustomResult(result);

        }

        [HttpPost]
        [Route("CreateEmployee")]
        public IActionResult CreateEmployee(Employee obj)
        {
            APIResult result = new APIResult();
            try
            {
                if (!(string.IsNullOrEmpty(obj.FirstName)) || !(string.IsNullOrEmpty(obj.UserType)))
                {
                    obj.IsRegister = false;
                    result = logic.CreateEmployee(obj);

                }
                else
                {
                    result.Message = "Invalid Input!!!";
                    result.Status = false;
                    result.CommandType = "INSERT";
                    result.Id = 0;
                    result.EmployeeName = "";
                    return StatusCode(StatusCodes.Status400BadRequest, result);

                }
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                result.Status = false;
                result.CommandType = "INSERT";
                result.Id = 0;
                result.EmployeeName = "";
                return  StatusCode(StatusCodes.Status500InternalServerError,result);

            }
            return Ok(result);
           // return new CustomResult(result);

        }

    }
}
