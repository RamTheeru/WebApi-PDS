﻿using System;
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

                    if (result.Message != "Session already exists for this user!!")
                    {
                        result.Status = true;
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
                var token = new JwtSecurityToken(
                    issuer: configuration["Jwt:Issuer"],
                    audience: configuration["Jwt:Issuer"],
                    claims,
                    expires: DateTime.Now.AddMinutes(10),
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
        [HttpGet("AdminDetails")]

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
       
        [HttpPut("ApproveUser")]
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
                if (!(string.IsNullOrEmpty(obj.FirstName))|| !(string.IsNullOrEmpty(obj.Phone)) || !(string.IsNullOrEmpty(obj.UserName)) || !(string.IsNullOrEmpty(obj.UserType)))
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
        #region FILEUPLOAD
        ////[HttpPost("upload", Name = "upload")]
        ////[ProducesResponseType(StatusCodes.Status200OK)]
        ////[ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        ////public async Task<IActionResult> UploadFile(IFormFile file)
        ////{
        ////    APIResult result = new APIResult();
        ////    try
        ////    {
        ////        result.CommandType = "file upload";
        ////        if (CheckIfExcelFile(file))
        ////        {
        ////            Tuple<bool,string> t = await WriteFile(file);
        ////            bool isSuccess = t.Item1;

        ////            if (isSuccess)
        ////            {
        ////                result.Status = true;

        ////            }
        ////            else 
        ////            {
        ////                result.Status = false;

        ////            }
        ////            result.Message = t.Item2;
        ////            return Ok(result);
        ////        }
        ////        else
        ////        {
        ////            result.Status = false;
        ////            result.Message = "Invalid file extension";
        ////            return StatusCode(StatusCodes.Status400BadRequest, result);
        ////        }
        ////    }
        ////    catch (Exception e)
        ////    {
        ////        result.Message = e.Message;
        ////        result.Status = false;
        ////        result.Id = 0;
        ////        return StatusCode(StatusCodes.Status500InternalServerError, result);
        ////    }

        ////}
        ////private bool CheckIfExcelFile(IFormFile file)
        ////{
        ////    var extension = "." + file.FileName.Split('.')[file.FileName.Split('.').Length - 1];
        ////    return (extension == ".xlsx" || extension == ".xls"); // Change the extension based on your need
        ////}

        ////private async Task<Tuple<bool,string>> WriteFile(IFormFile file)
        ////{
        ////    Tuple<bool, string> t;
        ////    bool isSaveSuccess = false;
        ////    string fileName;
        ////    try
        ////    {
        ////        var month = DateTime.Now.getMonthAbbreviatedName();
        ////        var extension = "." + file.FileName.Split('.')[file.FileName.Split('.').Length - 1];
        ////        fileName = month+"-"+ DateTime.Now.Year + extension; //Create a new Name for the file due to security reasons.

        ////        var pathBuilt = Path.Combine(Directory.GetCurrentDirectory(), "Upload\\files");

        ////        if (!Directory.Exists(pathBuilt))
        ////        {
        ////            Directory.CreateDirectory(pathBuilt);
        ////        }

        ////        var path = Path.Combine(Directory.GetCurrentDirectory(), "Upload\\files",
        ////           fileName);

        ////        using (var stream = new FileStream(path, FileMode.Create))
        ////        {
        ////            await file.CopyToAsync(stream);
        ////        }

        ////        isSaveSuccess = true;
        ////        t = Tuple.Create(isSaveSuccess, "file uploaded successfully");
        ////    }
        ////    catch(Exception e)
        ////    {
        ////        isSaveSuccess = false;
        ////        t = Tuple.Create(isSaveSuccess, e.Message);

        ////    }

        ////    return t;
        ////}
        #endregion

    }
}
