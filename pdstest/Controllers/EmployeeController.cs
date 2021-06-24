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
using System.IO;
using Wkhtmltopdf.NetCore;
using Wkhtmltopdf.NetCore.Interfaces;
using Wkhtmltopdf;
using Wkhtmltopdf.NetCore.Options;
using ExcelDataReader;

namespace pdstest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly IGeneratePdf _generatePdf;
        //public EmployeeController(IConfiguration config) {
        //    this.configuration = config;

        //}
        private static IConnection conn;
        private BLLogic logic;
        public EmployeeController(IConnection con,IConfiguration config, IGeneratePdf generatePdf)
        {

            conn = con;
            configuration = config;
            logic = new BLLogic(conn,configuration);
            _generatePdf = generatePdf;
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
                    username = username.CleanString(); 
                if (password != null)
                    password = password.CleanString(); 
                result = logic.GetLoginUserInfo(username, password);
                if (!string.IsNullOrEmpty(result.userInfo.User))
                {
                    if (result.userInfo.Valid && result.userInfo.UserTypeId>0)
                    {
                        result = logic.CheckIfSessionExists(result.userInfo);
                        if(result.Status)
                        {
                            result.Message = result.Message;
                            result.Status = false;
                            result.CommandType = "Insert";
                            result.EmployeeName = username;
                        }
                        else 
                        {

                            token = GenerateJSONWebToken(result.userInfo);
                            if (!string.IsNullOrEmpty(token))
                            {
                                result.userInfo.Token = token;
                                result = logic.CreateSession(result.userInfo);
                                if (result.Message == "Session already exists for this user!!")
                                {

                                    result.Message = result.Message;
                                    result.Status = false;
                                    result.CommandType = "Insert";
                                    result.EmployeeName = username;
                                }

                            }
                            else
                            {
                                result.Message = "Something Went Wrong : Session cant be generated for this user";
                                result.Status = false;
                                result.CommandType = "SELECT";
                                result.EmployeeName = username;

                            }

                        }


                    }
                    else
                    {
                        result.Message = "User Authentication failed!!!";
                        result.Status = false;
                        result.CommandType = "SELECT";
                        result.EmployeeName = username;

                    }

                    if (result.Message != "Session already exists for this user!!" && result.Status==true)
                    {
                        result.Status = true;
                        result.CommandType = result.CommandType;
                        result.EmployeeName = username;
                        result.Message = result.Message;
                    }
                    else
                    {
                        result.Status = false;
                        result.CommandType = result.CommandType;
                        result.EmployeeName = username;
                        result.Message = result.Message;
                    }
                }
                else 
                {
                    result.Message = "No User Found : User Authentication failed!!!";
                    result.Status = false;
                    result.CommandType = "Session Insert";
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
        [HttpGet("SessionUpdate")]
        public IActionResult SessionUpdate(int usertypeId, int employeeId)
        {
            APIResult result = new APIResult();
            result.userInfo = new UserType();
            try
            {
                if (usertypeId > 0 && employeeId > 0)
                {
                    result = logic.GetLoginUserInfo(usertypeId, employeeId);
                    if (!string.IsNullOrEmpty(result.userInfo.User))
                    {
                        if (result.userInfo.Valid && result.userInfo.UserTypeId > 0)
                        {
                            result = logic.CheckIfSessionExists(result.userInfo);
                            if (!result.Status)
                            {
                                result.Message = result.Message;
                                result.Status = false;
                                result.CommandType = "Session Update";
                                return StatusCode(StatusCodes.Status400BadRequest, result);
                            }
                            else
                            {
                                string token = GenerateJSONWebToken(result.userInfo);
                                if (!string.IsNullOrEmpty(token))
                                {
                                    result.userInfo.Token = token;
                                    result = logic.UpdateSession(result.userInfo);
                                    //if (result.Status)
                                    //    result.userInfo.Token = token;

                                }
                                else
                                {
                                    result.Message = "Something Went Wrong : Session cant be updated for this user.Please login again";
                                    result.Status = false;
                                    result.CommandType = "Session Update";
                                    return StatusCode(StatusCodes.Status400BadRequest, result);

                                }
                            }

                        }
                        else
                        {
                            result.Message = "User Authentication failed.Please login again!!!";
                            result.Status = false;
                            result.CommandType = "Session Update";
                            return StatusCode(StatusCodes.Status400BadRequest, result);

                        }
                    }
                    else
                    {
                        result.Message = "No User Found : User Authentication failed.Please login again!!!";
                        result.Status = false;
                        result.CommandType = "Session Update";
                        return StatusCode(StatusCodes.Status400BadRequest, result);

                    }
                }
                else
                {
                    result.Message = "Invalid Input!!!";
                    result.Status = false;
                    result.CommandType = "Session Update";
                    result.EmployeeName = "";
                    return StatusCode(StatusCodes.Status400BadRequest, result);
                }

            }
            catch (Exception e)
            {
                result.Message = e.Message;
                result.Status = false;
                result.CommandType = "Session Update";
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
                new Claim(JwtRegisteredClaimNames.Sub,user.Role),
                new Claim(JwtRegisteredClaimNames.Email,user.User),
                   new Claim(JwtRegisteredClaimNames.NameId,user.EmployeeId.ToString()),
                 new Claim(JwtRegisteredClaimNames.Typ,user.UserTypeId.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
             };
                DateTime dtt = DateTime.Now;
                dtt = dtt.GetIndianDateTimeNow();
                var token = new JwtSecurityToken(
                    issuer: configuration["Jwt:Issuer"],
                    audience: configuration["Jwt:Issuer"],
                    claims,
                    expires: dtt.AddMinutes(20),
                    signingCredentials: credentials);

                var encodeToken = new JwtSecurityTokenHandler().WriteToken(token);
                tkn = encodeToken;
            }
            catch (Exception e)
            {
                tkn = "";
                throw e;
            }
            return tkn;
        }


        [HttpGet("Constants")]

        public IActionResult GetConstants()        
        {
            APIResult result = new APIResult();
            try 
            {
                var p = "";
                HttpContext.GetCloudEnvironment(out p);
               
               result = logic.GetConstants();
                result.Path = p;
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
        [HttpGet("ResetPassword")]
        public IActionResult ResetPassword(int employeeId,string password)
        {
            APIResult result = new APIResult();
            try
            {
                if (!string.IsNullOrEmpty(password))
                    password = password.CleanString();
                if (employeeId == 0 || string.IsNullOrEmpty(password))
                {
                    result.Message = "Invalid Input!!!";
                    result.Status = false;
                    result.CommandType = "RESET";
                    result.EmployeeName = "";
                    return StatusCode(StatusCodes.Status400BadRequest, result);
                }
                else
                {
                    result = logic.ResetPassword(employeeId, password);
                }
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                result.Status = false;
                result.CommandType = "RESET";
                return StatusCode(StatusCodes.Status500InternalServerError, result);
            }
            return Ok(result);


        }

        [HttpGet("AdminDetails")]
        [CustomAuthorization]
        public IActionResult GetAdminDetails()
        {
            APIResult result = new APIResult();
            try
            {

                result = logic.GetAdminDetails();

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

        [HttpGet("DeleteSession")]

        public IActionResult DeleteSession(string userName,int employeeId,int userTypeId)
        {
            APIResult result = new APIResult();
            try
            {
                if (string.IsNullOrEmpty(userName) || employeeId==0||userTypeId==0)
                {
                    result.Message = "Invalid Input!!!";
                    result.Status = false;
                    result.CommandType = "DELETE";
                    result.EmployeeName = "";
                    return StatusCode(StatusCodes.Status400BadRequest, result);

                }
                if (!string.IsNullOrEmpty(userName))
                    userName = userName.CleanString();
                result = logic.DeleteSession(userName,employeeId,userTypeId);

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

        [HttpPost("Logins")]
        //[Authorize(AuthenticationSchemes = "Bearer")]
        [CustomAuthorization]
        public IActionResult GetEmployeeLogins(APIInput input)
        {
            APIResult result = new APIResult();

            try
            {
                if (input.stationId > 0)
                {
                    input.table = "logins";
                    result = logic.GetPagnationRecords(input);
                }
                else
                {
                    result.Message = "Invalid Input!!!";
                    result.Status = false;
                    result.CommandType = "Select";
                    return StatusCode(StatusCodes.Status400BadRequest, result);

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
            //return new CustomResult(result);

        }

        [HttpPost("RegisteredUsers")]
        //[Authorize(AuthenticationSchemes = "Bearer")]
        [CustomAuthorization]
        public IActionResult GetRegisteredUsers(APIInput input)
        {
            APIResult result = new APIResult();

            try
            {
                if (input.stationId > 0)
                {
                    input.table = "register";
                    result = logic.GetPagnationRecords(input);
                }
                else
                {
                    result.Message = "Invalid Input!!!";
                    result.Status = false;
                    result.CommandType = "Select";
                    return StatusCode(StatusCodes.Status400BadRequest, result);

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
            //return new CustomResult(result);

        }
       
        [HttpGet("ApproveUser")]
        [CustomAuthorization]
        public IActionResult ApproveUser(string registerId,string status,int pId,string empCode)
        {
            APIResult result = new APIResult();
            int regId = 0;
            bool success = false;
            try
            {
                success = int.TryParse(registerId, out regId);
                if (string.IsNullOrEmpty(registerId) || regId == 0 || !success||string.IsNullOrEmpty(status))
                {
                    result.Message = "Invalid Input!!!";
                    result.Status = false;
                    result.CommandType = "UPDATE";
                    result.Id = regId;
                    result.EmployeeName = "";
                    return StatusCode(StatusCodes.Status400BadRequest, result);

                }
                else if (status=="a" && (string.IsNullOrEmpty(empCode) || pId==0))
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
                    if (!string.IsNullOrEmpty(status))
                        status = status.CleanString();
                    result = logic.ApproveUser(regId,status,empCode,pId);
                }  

            }
            catch (Exception e)
            {
                result.Message = e.Message;
                result.Status = false;
                result.CommandType = status=="a"? "UPDATE":"DELETE";
                return StatusCode(StatusCodes.Status500InternalServerError, result);
            }
            return Ok(result);
            //return new CustomResult(result);

        }

        [HttpPost("Employees")]
        [CustomAuthorization]
        public IActionResult GetEmployees(APIInput input)
        {
            APIResult result = new APIResult();
            try
            {
                //if(stationCode != null)
                //    stationCode = stationCode.Replace(@"\", "");
                result = result = logic.GetEmployees(input, true);

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

        [HttpPost("PDSEmployees")]
        [CustomAuthorization]
        public IActionResult GetPDSEmployees(APIInput input)
        {
            APIResult result = new APIResult();
            try
            {
                input.table = "pdsemployees";
                //if(stationCode != null)
                //    stationCode = stationCode.Replace(@"\", "");
                result = result = logic.GetEmployees(input,false);

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
        [HttpPost("PDSUnApprovedEmployees")]
        [CustomAuthorization]
        public IActionResult GetUnApprovedPDSEmployees(APIInput input)
        {
            APIResult result = new APIResult();
            try
            {
                input.table = "pdsunemployees";
                //if(stationCode != null)
                //    stationCode = stationCode.Replace(@"\", "");
                result = result = logic.GetEmployees(input, false);

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
        [HttpPost("DAEmployees")]
        [CustomAuthorization]

        public IActionResult GetDAEmployees(APIInput input)
        {
            APIResult result = new APIResult();
            try
            {
                if (input.stationId > 0)
                {
                    input.table = "daemployees";
                    result = logic.GetPagnationRecords(input);
                }
                else
                {
                    result.Message = "Invalid Input!!!";
                    result.Status = false;
                    result.CommandType = "Select";
                    return StatusCode(StatusCodes.Status400BadRequest, result);

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
            //return new CustomResult(result);

        }

        [HttpGet("CheckUserName")]

        public IActionResult CheckUserName(string userName)
        {
            APIResult result = new APIResult();
            try
            {
                if (!string.IsNullOrEmpty(userName))
                    userName = userName.CleanString();
                else 
                {
                    result.Message = "Invalid Input!!!";
                    result.Status = false;
                    result.CommandType = "SELECT";
                    return StatusCode(StatusCodes.Status400BadRequest, result);

                }
                result = logic.CheckUserExists(userName);

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

        [HttpGet("CheckMainEmpCode")]

        public IActionResult CheckMainEmpCode(string empCode)
        {
            APIResult result = new APIResult();
            try
            {
                if (!string.IsNullOrEmpty(empCode))
                    empCode = empCode.CleanString();
                else
                {
                    result.Message = "Invalid Input!!!";
                    result.Status = false;
                    result.CommandType = "SELECT";
                    return StatusCode(StatusCodes.Status400BadRequest, result);

                }
                result = logic.CheckMainEmpCodeExists(empCode);

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

        [HttpGet("CheckEmpCode")]

        public IActionResult CheckEmpCode(string empCode)
        {
            APIResult result = new APIResult();
            try
            {
                if (!string.IsNullOrEmpty(empCode))
                    empCode = empCode.CleanString();
                else
                {
                    result.Message = "Invalid Input!!!";
                    result.Status = false;
                    result.CommandType = "SELECT";
                    return StatusCode(StatusCodes.Status400BadRequest, result);

                }
                result = logic.CheckEmpCodeExists(empCode,true);

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
        [HttpGet("CheckCDACode")]

        public IActionResult CheckCDACode(string cdaCode)
        {
            APIResult result = new APIResult();
            try
            {
                if (!string.IsNullOrEmpty(cdaCode))
                    cdaCode = cdaCode.CleanString();
                else
                {
                    result.Message = "Invalid Input!!!";
                    result.Status = false;
                    result.CommandType = "SELECT";
                    return StatusCode(StatusCodes.Status400BadRequest, result);

                }
                result = logic.CheckEmpCodeExists(cdaCode, false);

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
                if (!(string.IsNullOrEmpty(obj.FirstName)) && !(string.IsNullOrEmpty(obj.Phone)) && !(string.IsNullOrEmpty(obj.UserName))  && !(string.IsNullOrEmpty(obj.Email)))
                {
                    obj.IsRegister = true;
                    obj.Age = string.IsNullOrEmpty(obj.EmpAge) ? 0 : Convert.ToInt32(obj.EmpAge);
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
        [Route("CreatePDSEmployee")]
        public IActionResult CreatePDSEmployee(PDSEmployee obj)
        {
            APIResult result = new APIResult();
            try
            {
                if (!(string.IsNullOrEmpty(obj.FirstName)) || !(string.IsNullOrEmpty(obj.EmpCode)) || obj.StationId > 0 || obj.Pid > 0)
                {
                    result = logic.CreateMainEmployee(obj);

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
            // return new CustomResult(result);

        }
        [HttpPost]
        [Route("CreateEmployee")]
        public IActionResult CreateEmployee(Employee obj)
        {
            APIResult result = new APIResult();
            try
            {
                if (!(string.IsNullOrEmpty(obj.FirstName))||obj.StationId>0||obj.Pid>0)
                {
                    obj.IsRegister = false;
                    result = logic.CreateEmployee(obj,true);

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

        [HttpPost]
        [Route("CreateDAEmployee")]
        [CustomAuthorization]
        public IActionResult CreateDAEmployee(Employee obj)
        {
            APIResult result = new APIResult();
            try
            {
                if (!(string.IsNullOrEmpty(obj.FirstName))||obj.StationId>0)
                {
                    obj.IsRegister = false;
                    result = logic.CreateEmployee(obj,false);

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
            // return new CustomResult(result);

        }

        [HttpPost]
        [Route("CreateCC")]
        [CustomAuthorization]
        public IActionResult CreateCommercialConstant(CommercialConstant obj)
        {
            APIResult result = new APIResult();
            try
            {
                if (obj.StationId > 0 && obj.PetrolAllowance > 0 && obj.DeliveryRate > 0)
                {
                    result = logic.CreateCommercialConstants(obj);

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
            // return new CustomResult(result);

        }
        [HttpGet]
        [Route("CDAStationDeiveryDetails")]
        [CustomAuthorization]
        public IActionResult GetCDAStationDeliveryDetails(int stationId)
        {
            APIResult result = new APIResult();
            try
            {
                if (stationId > 0 )
                {
                    
                    result = logic.GetCDADeliveryDetailsbyStation(stationId);

                }
                else
                {
                    result.Message = "Invalid Input!!!";
                    result.Status = false;
                    result.CommandType = "Select";
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
            // return new CustomResult(result);

        }
        [HttpPost]
        [Route("CDAGetDeiveryDetails")]
        [CustomAuthorization]
        public IActionResult GetCDADeliveryDetails(APIInput input)
        {
            APIResult result = new APIResult();
            try
            {
                if (input.stationId > 0 && input.currentmonth > 0)
                {
                    input.table = "getcdadel";
                    result = logic.GetPagnationRecords(input);

                }
                else
                {
                    result.Message = "Invalid Input!!!";
                    result.Status = false;
                    result.CommandType = "Select";
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
            // return new CustomResult(result);

        }

        [HttpPost]
        [Route("CDAUpdateDeiveryDetails")]
        [CustomAuthorization]
        public IActionResult CDAUpdateDeliveryDetails(List<DeliveryDetails> cdds)
        {
            APIResult result = new APIResult();
           // List<DeliveryDetails> inputs = new List<DeliveryDetails>();
            try
            {
                if (cdds.Count>0)
                {
                    //foreach(var item in cdds)
                    //{
                    //    DeliveryDetails d = new DeliveryDetails();
                        


                    //}
                    result = logic.UpdateDeliveryRates(cdds);


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
            // return new CustomResult(result);

        }

        #region FILEDOWNLOAD

        [HttpPost]
        [Route("DownloadCDADeiveryDetails")]
        [CustomAuthorization]
        public async Task<IActionResult> CDADownloadDeliveryDetails(PDFInput input)
        {
            APIResult result = new APIResult();
            List<InMemoryFile> files = new List<InMemoryFile>();
            string contentType = "application/zip";
            string fileName = "";
            // List<DeliveryDetails> inputs = new List<DeliveryDetails>();
            try
            {

                if(input.forall && input.emps.Count == 0)
                {
                    if(input.stationId > 0 && input.currentmonth > 0)
                        input.emps = logic.GetAllEmpIdsforPDF(input.currentmonth, input.stationId);
                    else
                    {
                        result.Message = "Invalid Input!!!";
                        result.Status = false;
                        result.CommandType = "Download";
                        result.Id = 0;
                        result.EmployeeName = "";
                        return StatusCode(StatusCodes.Status400BadRequest, result);
                    }
                }
                if (input.emps.Count > 0)
                {
                    foreach (var item in input.emps)
                    {
                        if ((input.currentmonth > 0) && item > 0)
                        {
                            PDFLayout fil = new PDFLayout();
                            result = logic.GetEmpDataforPDFFile(item, input.currentmonth);
                            if (!result.Status)
                            {
                                result.Message = "Something went wrong!!  Reason : " + result.Message;
                                return StatusCode(StatusCodes.Status500InternalServerError, result);
                            }
                            fil = result.pdfLayout;
                            ConvertOptions opts = new ConvertOptions();
                            //opts.PageWidth = 800;
                            //opts.PageHeight = 508;
                            opts.PageSize = Size.A4;
                            opts.PageMargins = new Margins();
                            ///opts.PageOrientation = Wkhtmltopdf.NetCore.Options.Orientation.Portrait;
                            Margins mrgns = new Margins();
                            mrgns.Left = 0;
                            mrgns.Right = 0;
                            opts.PageMargins = mrgns;
                            _generatePdf.SetConvertOptions(opts);
                            byte[] ct = await _generatePdf.GetByteArray("pdflayout/pdfformat.cshtml", fil);
                            //var pdfStream = new System.IO.MemoryStream();
                            //pdfStream.Write(ct, 0, ct.Length);
                            //pdfStream.Position = 0;
                             fileName = fil.VendorCode + ".pdf"; //"CDAInvoiceFormat.pdf";

                            InMemoryFile file = new InMemoryFile();
                            file.FileName = fileName;
                            file.Content = ct;
                            files.Add(file);
                            //return new FileStreamResult(pdfStream,contentType);
                            //return File(ct, contentType, fileName);
                        }
                        else
                        {
                            result.Message = "Invalid Input!!!";
                            result.Status = false;
                            result.CommandType = "Download";
                            result.Id = 0;
                            result.EmployeeName = "";
                            return StatusCode(StatusCodes.Status400BadRequest, result);
                        }



                    }
                    if(files.Count>0 && files.Count == input.emps.Count)
                    {
                        byte[] zipFileContent = await logic.GetZipArchive(files);
                        if(zipFileContent != null)
                            return File(zipFileContent, contentType, "CDAInvoice"+result.employee.StationCode+result.pdfLayout.BillingPeriod);
                        else
                        {
                            result.Status = false;
                            result.CommandType = "Download";
                            result.Message = "Something went wrong,No file created!!  Reason :  Error occurred while compressing the files";
                            return StatusCode(StatusCodes.Status500InternalServerError, result);
                        }
                    }
                    else
                    {
                        result.Status = false;
                        result.CommandType = "Download";
                        result.Message = "Something went wrong,No file created!!  Reason : " + result.Message;
                        return StatusCode(StatusCodes.Status500InternalServerError, result);

                    }
                    // result = logic.UpdateDeliveryRates(cdds);

                }
                else
                {
                    result.Message = "Invalid Input!!!";
                    result.Status = false;
                    result.CommandType = "Download";
                    result.Id = 0;
                    result.EmployeeName = "";
                    return StatusCode(StatusCodes.Status400BadRequest, result);

                }
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                result.Status = false;
                result.CommandType = "Download";
                result.Id = 0;
                result.EmployeeName = "";
                return StatusCode(StatusCodes.Status500InternalServerError, result);

            }
            //return Ok(result);
            // return new CustomResult(result);

        }


        #endregion
        #region AttendanceFILEDownload

        [HttpPost]
        [Route("DownloadAttendance")]
        ///[CustomAuthorization]
        public async Task<IActionResult> DownloadAttendance(APIInput input)
        {
            APIResult result = new APIResult();
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            string fileName = "";
            try
            {
                if (input.currentYear == 0 || input.currentYear == 2)
                    input.currentYear = DateTime.Now.GetIndianDateTimeNow().Year;
                else if (input.currentYear == 1)
                    input.currentYear = DateTime.Now.GetIndianDateTimeNow().AddYears(-1).Year;
                if (input.stationId > 0 && input.currentmonth > 0)
                {
                    result = logic.GetConstants();
                    var stations = result.stations;
                    var dt = new DateTime(input.currentYear, input.currentmonth, 1);
                    var month = dt.getMonthAbbreviatedName();
                    var station = stations.FirstOrDefault(s => s.StationId == input.stationId);
                    var extension = ".xlsx";
                    if (station != null)
                    {

                        var pathBuilt = "";
                        HttpContext.GetCloudEnvironment(out pathBuilt);
                        if (pathBuilt.Contains("v2") == true)
                        {
                            pathBuilt = configuration["pattendancepath"];
                        }
                        else
                        {
                            pathBuilt = configuration["attendancepath"];
                        }
                        var stationCode = station.StationCode;
                        var stationPath = Path.Combine(pathBuilt, stationCode);
                        fileName = stationCode + "-" + month + "-" + input.currentYear + extension;
                        if (!Directory.Exists(stationPath))
                        {
                            Directory.CreateDirectory(stationPath);
                        }
                        var path = Path.Combine(stationPath, fileName);
                        if (System.IO.File.Exists(path))
                        {
                            byte[] fileBytes = await System.IO.File.ReadAllBytesAsync(path);

                            return File(fileBytes,contentType, fileName);
                            //using (StreamReader sr = System.IO.File.OpenText(path)) "application/force-download"
                            //{
                            //    return File(sr.BaseStream, contentType, fileName);
                            //}
                        }
                        extension = ".xls";
                        fileName = stationCode + "-" + month + "-" + input.currentYear + extension;
                        path = Path.Combine(pathBuilt, fileName);
                        if (System.IO.File.Exists(path))
                        {

                            byte[] fileBytes = await System.IO.File.ReadAllBytesAsync(path);

                            return File(fileBytes, contentType, fileName);
                            //using (StreamReader sr = System.IO.File.OpenText(path))
                            //{
                            //    Stream str = null;
                            //    await sr.BaseStream.CopyToAsync(str);
                            //    return File(str, contentType, fileName);
                            //}
                        }
                        else
                        {
                            result.Message = "No file uploaded yet for this Station!!!";
                            result.Status = true;
                            result.CommandType = "Download";
                            result.Id = 0;
                            result.EmployeeName = "";
                            return StatusCode(StatusCodes.Status400BadRequest, result);
                        }
                    }
                    else
                    {
                        result.Message = "Invalid Input,No file found for this Station!!!";
                        result.Status = false;
                        result.CommandType = "Download";
                        result.Id = 0;
                        result.EmployeeName = "";
                        return StatusCode(StatusCodes.Status400BadRequest, result);
                    }
                }
                else
                {
                    result.Message = "Invalid Input!!!";
                    result.Status = false;
                    result.CommandType = "Download";
                    result.Id = 0;
                    result.EmployeeName = "";
                    return StatusCode(StatusCodes.Status400BadRequest, result);
                }
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                result.Status = false;
                result.CommandType = "Download";
                result.Id = 0;
                result.EmployeeName = "";
                return StatusCode(StatusCodes.Status500InternalServerError, result);

            }
        }
            #endregion
            #region AttendanceFILEUPLOAD
            [HttpPost("upload", Name = "upload")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            APIResult result = new APIResult();
            try
            {
                result.CommandType = "file upload";
                if (CheckIfExcelFile(file))
                {
                    Tuple<bool, string> t = await WriteFile(file);
                    bool isSuccess = t.Item1;

                    if (isSuccess)
                    {
                        result.Status = true;

                    }
                    else
                    {
                        result.Status = false;

                    }
                    result.Message = t.Item2;
                    return Ok(result);
                }
                else
                {
                    result.Status = false;
                    result.Message = "Invalid file extension";
                    return StatusCode(StatusCodes.Status400BadRequest, result);
                }
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                result.Status = false;
                result.Id = 0;
                return StatusCode(StatusCodes.Status500InternalServerError, result);
            }

        }
        [NonAction]
        private bool CheckIfExcelFile(IFormFile file)
        {
            var extension = "." + file.FileName.Split('.')[file.FileName.Split('.').Length - 1];
            return (extension == ".xlsx" || extension == ".xls"); // Change the extension based on your need
        }
        [NonAction]
        private async Task<Tuple<bool, string>> WriteFile(IFormFile file)
        {
            Tuple<bool, string> t;
            bool isSaveSuccess = false;
            string fileName;
            try
            {
                var month = DateTime.Now.GetIndianDateTimeNow().getMonthAbbreviatedName();
                string[] fileprefix = file.FileName.Split('.');
                if (fileprefix[0].Contains("-"))
                {
                    string[] filepre = fileprefix[0].Split('-');
                    string station = filepre[0];
                    if (!string.IsNullOrEmpty(station))
                    {
                        APIResult resstat = new APIResult();
                        resstat = logic.GetConstants();
                        var stations = resstat.stations;
                        var validate = stations.FirstOrDefault(x => x.StationCode == station);
                        if (validate != null)
                        {
                            var statCode = validate.StationCode;
                            var pathBuilt = "";
                            HttpContext.GetCloudEnvironment(out pathBuilt);
                            if (pathBuilt.Contains("v2") == true)
                            {
                                pathBuilt = configuration["pattendancepath"];
                            }
                            else
                            {
                                pathBuilt = configuration["attendancepath"];
                            }
                           // var pathBuilt = configuration["attendancepath"];//Path.Combine(Directory.GetCurrentDirectory(), "Upload\\files");
                            var stationPath = Path.Combine(pathBuilt, statCode);
                            if (!Directory.Exists(stationPath))
                            {
                                Directory.CreateDirectory(stationPath);
                            }
                            var extension = "." + file.FileName.Split('.')[file.FileName.Split('.').Length - 1];
                            fileName = statCode + "-" + month + "-" + DateTime.Now.Year + extension; //Create a new Name for the file due to security reasons.

                            

                            //if (!Directory.Exists(pathBuilt))
                            //{
                            //    Directory.CreateDirectory(pathBuilt);
                            //}

                            var path = Path.Combine(stationPath, fileName);

                            using (var stream = new FileStream(path, FileMode.Create))
                            {
                                await file.CopyToAsync(stream);
                            }

                            isSaveSuccess = true;
                            t = Tuple.Create(isSaveSuccess, "file uploaded successfully");
                        }
                        else
                        {
                            isSaveSuccess = false;
                            t = Tuple.Create(isSaveSuccess, "StationCode mentioned in FileName is not in correct format.Please check and upload again.Format should be in (StationCode-MonthName.xlsx)");
                        }
                    }
                    else
                    {
                        isSaveSuccess = false;
                        t = Tuple.Create(isSaveSuccess, "file naming is not in correct format.Please check and upload again.Format should be in (StationCode-MonthName.xlsx)");
                    }
                }
                else
                {
                    isSaveSuccess = false;
                    t = Tuple.Create(isSaveSuccess, "file naming is not in correct format.Please check and upload again.Format should be in (StationCode-MonthName.xlsx)");
                }
            }
            catch (Exception e)
            {
                isSaveSuccess = false;
                t = Tuple.Create(isSaveSuccess, e.Message);

            }

            return t;
        }
        #endregion
        //#region CDAEmployeeExcelFileTODB
        //[HttpPost("uploadcdaexcel", Name = "uploadcdaexcel")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        //public  Task<IActionResult> UploadCDAEmpExcelFile(IFormFile file)
        //{
        //    APIResult result = new APIResult();
        //    try
        //    {
        //        result.CommandType = "file upload";
        //        if (CheckIfExcelFile(file))
        //        {
        //            Tuple<bool, string> t =  ReadFile(file);
        //            bool isSuccess = t.Item1;

        //            if (isSuccess)
        //            {
        //                result.Status = true;

        //            }
        //            else
        //            {
        //                result.Status = false;

        //            }
        //            result.Message = t.Item2;
        //            return Ok(result);
        //        }
        //        else
        //        {
        //            result.Status = false;
        //            result.Message = "Invalid file extension";
        //            return StatusCode(StatusCodes.Status400BadRequest, result);
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        result.Message = e.Message;
        //        result.Status = false;
        //        result.Id = 0;
        //        return StatusCode(StatusCodes.Status500InternalServerError, result);
        //    }

        //}
        //private  void ReadFile(IFormFile file)
        //{
        //    Tuple<bool, string> t;
        //    bool isSaveSuccess = false;
        //    string fileName;
        //    try
        //    {
        //        fileName = file.FileName;
        //        // For .net core, the next line requires the NuGet package, 
        //        // System.Text.Encoding.CodePages
        //        System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
        //        using (var stream = System.IO.File.Open(fileName, FileMode.Open, FileAccess.Read))
        //        {
        //            using (var reader = ExcelReaderFactory.CreateReader(stream))
        //            {
        //                t = Tuple.Create(true, "");
        //                while (reader.Read()) //Each row of the file
        //                {
        //                    //users.Add(new UserModel
        //                    //{
        //                    //    Name = reader.GetValue(0).ToString(),
        //                    //    Email = reader.GetValue(1).ToString(),
        //                    //    Phone = reader.GetValue(2).ToString()
        //                    //});

        //                }
        //            }
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        isSaveSuccess = false;
        //        t = Tuple.Create(isSaveSuccess, e.Message);

        //    }

        //  //  return t;
        //}

        //#endregion
        [HttpGet("Backups")]
        [CustomAuthorization]
        public IActionResult BackupList()
        {
            APIResult result = new APIResult();
            bool isProduction = false;
            try
            {
                var pathBuilt = "";
                HttpContext.GetCloudEnvironment(out pathBuilt);
                if (pathBuilt.Contains("v2") == true)
                {
                    isProduction = true;
                }
                result = logic.BackupList(isProduction);

            }
            catch (Exception e)
            {
                result.Message = e.Message;
                result.Status = false;
                result.CommandType = "Backup";
                return StatusCode(StatusCodes.Status500InternalServerError, result);
            }
            return Ok(result);
            //return new CustomResult(result);

        }
        [HttpGet("RestoreDb")]
        [CustomAuthorization]
        public IActionResult RestoreDb(string file)
        {
            APIResult result = new APIResult();
            try
            {
                bool isProdction = false;
                if (string.IsNullOrEmpty(file))
                {
                    result.Message = "Invalid Input!!!";
                    result.Status = false;
                    result.CommandType = "Restore";
                    return StatusCode(StatusCodes.Status400BadRequest, result);

                }
                else
                {
                    if (!string.IsNullOrEmpty(file))
                        file = file.CleanString();
                    var pathBuilt = "";
                    HttpContext.GetCloudEnvironment(out pathBuilt);
                    if (pathBuilt.Contains("v2") == true)
                    {
                        isProdction = true;
                    }
                    result = logic.RestoreDatabase(file,isProdction);
                }

            }
            catch (Exception e)
            {
                result.Message = e.Message;
                result.Status = false;
                result.CommandType = "Restore";
                return StatusCode(StatusCodes.Status500InternalServerError, result);
            }
            return Ok(result);
            //return new CustomResult(result);

        }
    }
}
