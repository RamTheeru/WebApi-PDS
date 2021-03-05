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


namespace pdstest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FinanceController : ControllerBase
    {
        private readonly IConfiguration configuration;
        //public EmployeeController(IConfiguration config) {
        //    this.configuration = config;

        //}
        private static IConnection conn;
        private BLLogic logic;
        public FinanceController(IConnection con, IConfiguration config)
        {

            conn = con;
            configuration = config;
            logic = new BLLogic(conn,configuration);
        }

        [HttpPost]
        [Route("InsertVoucher")]
        public IActionResult InsertVoucher(Voucher obj)
        {
            APIResult result = new APIResult();
            try
            {
                if (obj.StationId>0 && !(string.IsNullOrEmpty(obj.VoucherNumber)) && !(string.IsNullOrEmpty(obj.V_Date)))
                {
                    result = logic.InsertVoucher(obj);

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
        [HttpGet]
        [Route("VoucherDetails")]
        public IActionResult GetVoucherDetailsbyVoucherNumber(string voucherNumber)
        {
            APIResult result = new APIResult();
            try
            {
                if (!string.IsNullOrEmpty(voucherNumber))
                {
                    result = logic.GetVoucherDetailsbyVoucherNumber(voucherNumber);

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
                result.CommandType = "Select";
                result.Id = 0;
                result.EmployeeName = "";
                return StatusCode(StatusCodes.Status500InternalServerError, result);
            }
            return Ok(result);
            //return new CustomResult(result);

        }
        [HttpPost]
        [Route("UpdateVoucher")]
        public IActionResult UpdateVoucher(Voucher obj)
        {
            APIResult result = new APIResult();
            try
            {
                if (obj.StationId > 0 && obj.VoucherId > 0 && !(string.IsNullOrEmpty(obj.VoucherNumber)) && !(string.IsNullOrEmpty(obj.V_Date)))
                {
                    result = logic.UpdateVoucher(obj);

                }
                else
                {
                    result.Message = "Invalid Input!!!";
                    result.Status = false;
                    result.CommandType = "UPDATE";
                    result.Id = 0;
                    result.EmployeeName = "";
                    return StatusCode(StatusCodes.Status400BadRequest, result);

                }
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                result.Status = false;
                result.CommandType = "UPDATE";
                result.Id = 0;
                result.EmployeeName = "";
                return StatusCode(StatusCodes.Status500InternalServerError, result);
            }
            return Ok(result);
            //return new CustomResult(result);

        }
        [HttpGet]
        [Route("PreviousMonthCredit")]
        public IActionResult GetPreviouMonthCredit(int stationId)
        {
            APIResult result = new APIResult();
            try
            {
                if (stationId > 0)
                {
                    result = logic.GetPreviousCreditandDebitDetails(stationId);

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
                result.CommandType = "Select";
                result.Id = 0;
                result.EmployeeName = "";
                return StatusCode(StatusCodes.Status500InternalServerError, result);
            }
            return Ok(result);
            //return new CustomResult(result);

        }

        [HttpPost]
        [Route("InsertCredit")]
        public IActionResult InsertCredit(Ledger obj)
        {
            APIResult result = new APIResult();
            try
            {
                if (obj.StationId > 0 && obj.Credit>0 && !(string.IsNullOrEmpty(obj.Cred_Date)))
                {
                    result = logic.InsertLedger(obj);

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

        [HttpPost("Vouchers")]

        public IActionResult GetVouchers(APIInput input)
        {
            APIResult result = new APIResult();
            try
            {
                input.table = "voucher";
                if (input.status != null)
                    input.status = input.status.CleanString();
                if (input.vEndDate == "string")
                    input.vEndDate = null;
                if (input.stationId == 0 || string.IsNullOrEmpty(input.vstartDate) || string.IsNullOrEmpty(input.status))
                {
                    result.Message = "Invalid Input!!!";
                    result.Status = false;
                    result.CommandType = "SELECT";
                    result.EmployeeName = "";
                    return StatusCode(StatusCodes.Status400BadRequest, result);

                }
                else if (input.status == "string" || input.vstartDate == "string")
                {
                    result.Message = "Invalid Input!!!";
                    result.Status = false;
                    result.CommandType = "SELECT";
                    result.EmployeeName = "";
                    return StatusCode(StatusCodes.Status400BadRequest, result);

                }
                result = logic.GetPagnationRecords(input);

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
        [HttpPost("Ledgers")]

        public IActionResult GetLedgers(APIInput input)
        {
            APIResult result = new APIResult();
            try
            {
                input.table = "ledger";
                if (input.status != null)
                    input.status = input.status.CleanString();
                if (input.stationId == 0 || input.vstartDate == null)
                {
                    result.Message = "Invalid Input!!!";
                    result.Status = false;
                    result.CommandType = "SELECT";
                    result.EmployeeName = "";
                    return StatusCode(StatusCodes.Status400BadRequest, result);

                }
                result = logic.GetPagnationRecords(input);

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
    }
}
