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
using Wkhtmltopdf.NetCore;
using Wkhtmltopdf.NetCore.Options;

namespace pdstest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [CustomAuthorization]
    public class FinanceController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly IGeneratePdf _generatePdf;
        public static string Zone = "Finance Controller";
        //public EmployeeController(IConfiguration config) {
        //    this.configuration = config;

        //}
        private static IConnection conn;
        private BLLogic logic;
        public FinanceController(IConnection con, IConfiguration config, IGeneratePdf generatePdf)
        {

            conn = con;
            configuration = config;
            logic = new BLLogic(conn, configuration);
            _generatePdf = generatePdf;
        }

        [HttpPost]
        [Route("InsertVoucher")]
        public IActionResult InsertVoucher(Voucher obj)
        {
            APIResult result = new APIResult();
            try
            {
                if (obj.StationId > 0 && !(string.IsNullOrEmpty(obj.VoucherNumber)) && !(string.IsNullOrEmpty(obj.V_Date)))
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
                ErrorLogTrack err = new ErrorLogTrack("InsertVoucher", e.TargetSite.ReflectedType.FullName, result.CommandType, e.Message, Zone);
                string r = logic.TraceError(err);
                result.Message = result.Message + " and " + r ?? "";
                return StatusCode(StatusCodes.Status500InternalServerError, result);
            }
            return Ok(result);
            //return new CustomResult(result);

        }
        [HttpGet]
        [Route("VoucherDetails")]
        public IActionResult GetVoucherDetailsbyVoucherNumber(int voucherId)
        {
            APIResult result = new APIResult();
            try
            {
                if (voucherId > 0)
                {
                    result = logic.GetVoucherDetailsbyVoucherNumber(voucherId);

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
                ErrorLogTrack err = new ErrorLogTrack("VoucherDetails", e.TargetSite.ReflectedType.FullName, result.CommandType, e.Message, Zone);
                string r = logic.TraceError(err);
                result.Message = result.Message + " and " + r ?? "";
                return StatusCode(StatusCodes.Status500InternalServerError, result);
            }
            return Ok(result);
            //return new CustomResult(result);

        }
        [HttpPost]
        [Route("ApproveVoucher")]
        public IActionResult ApproveVoucher(List<Voucher> vouchers)
        {
            APIResult result = new APIResult();
            try
            {
                if (vouchers.Count > 0)
                {
                    result = logic.UpdateVoucherDetails(vouchers, "A");

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
                ErrorLogTrack err = new ErrorLogTrack("ApproveVoucher", e.TargetSite.ReflectedType.FullName, result.CommandType, e.Message, Zone);
                string r = logic.TraceError(err);
                result.Message = result.Message + " and " + r ?? "";
                return StatusCode(StatusCodes.Status500InternalServerError, result);
            }
            return Ok(result);
            //return new CustomResult(result);

        }
        [HttpPost]
        [Route("RejectVoucher")]
        public IActionResult RejectVoucher(List<Voucher> vouchers)
        {
            APIResult result = new APIResult();
            try
            {
                if (vouchers.Count > 0)
                {
                    result = logic.UpdateVoucherDetails(vouchers, "R");

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
                ErrorLogTrack err = new ErrorLogTrack("RejectVoucher", e.TargetSite.ReflectedType.FullName, result.CommandType, e.Message, Zone);
                string r = logic.TraceError(err);
                result.Message = result.Message + " and " + r ?? "";
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
                ErrorLogTrack err = new ErrorLogTrack("UpdateVoucher", e.TargetSite.ReflectedType.FullName, result.CommandType, e.Message, Zone);
                string r = logic.TraceError(err);
                result.Message = result.Message + " and " + r ?? "";
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
                ErrorLogTrack err = new ErrorLogTrack("PreviousMonthCredit", e.TargetSite.ReflectedType.FullName, result.CommandType, e.Message, Zone);
                string r = logic.TraceError(err);
                result.Message = result.Message + " and " + r ?? "";
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
                //  obj.Cred_Date = DateTime.Now.ToShortDateString();
                DateTime dtt = DateTime.Now.GetIndianDateTimeNow();
                obj.Cred_Date = dtt.DateTimetoString();
                if (obj.StationId > 0 && obj.Credit > 0 && !(string.IsNullOrEmpty(obj.Cred_Date)))
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
                ErrorLogTrack err = new ErrorLogTrack("InsertCredit", e.TargetSite.ReflectedType.FullName, result.CommandType, e.Message, Zone);
                string r = logic.TraceError(err);
                result.Message = result.Message + " and " + r ?? "";
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
                if (input.stationId == 0 || string.IsNullOrEmpty(input.vEndDate))
                {
                    result.Message = "Invalid Input!!!";
                    result.Status = false;
                    result.CommandType = "SELECT";
                    result.EmployeeName = "";
                    return StatusCode(StatusCodes.Status400BadRequest, result);

                }
                //else if (input.status == "string" || input.vstartDate == "string")
                //{
                //    result.Message = "Invalid Input!!!";
                //    result.Status = false;
                //    result.CommandType = "SELECT";
                //    result.EmployeeName = "";
                //    return StatusCode(StatusCodes.Status400BadRequest, result);

                //}
                result = logic.GetPagnationRecords(input);

            }
            catch (Exception e)
            {
                result.Message = e.Message;
                result.Status = false;
                result.CommandType = "Select";
                ErrorLogTrack err = new ErrorLogTrack("Vouchers", e.TargetSite.ReflectedType.FullName, result.CommandType, e.Message, Zone);
                string r = logic.TraceError(err);
                result.Message = result.Message + " and " + r ?? "";
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
                if (input.stationId == 0)
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
                ErrorLogTrack err = new ErrorLogTrack("Ledgers", e.TargetSite.ReflectedType.FullName, result.CommandType, e.Message, Zone);
                string r = logic.TraceError(err);
                result.Message = result.Message + " and " + r ?? "";
                return StatusCode(StatusCodes.Status500InternalServerError, result);
            }
            return Ok(result);
            //return new CustomResult(result);

        }

        [HttpPost]
        [Route("DownloadLedgerDetails")]
        [CustomAuthorization]
        public async Task<IActionResult> DownloadLedgerDetails(APIInput input)
        {
            APIResult result = new APIResult();
            List<InMemoryFile> files = new List<InMemoryFile>();
            string contentType = "application/pdf";
            string fileName = "";
            try
            {
                input.table = "ledgerreport";
                if (input.status != null)
                    input.status = input.status.CleanString();
                if (input.stationId == 0 || string.IsNullOrEmpty(input.vEndDate) || string.IsNullOrEmpty(input.vstartDate))
                {
                    result.Message = "Invalid Input!!!";
                    result.Status = false;
                    result.CommandType = "SELECT";
                    result.EmployeeName = "";
                    return StatusCode(StatusCodes.Status400BadRequest, result);

                }
                string[] dtcontents = input.vEndDate.Split("-");
                if (dtcontents.Length > 0)
                {
                    string cmonth = dtcontents[1];
                    input.currentmonth = logic.HandleStringtoInt(cmonth);
                    result.ledgers = new List<Ledger>();
                    result = logic.GetPagnationRecords(input);
                    if (result.ledgers.Count > 0)
                    {
                        string stationname = "";
                        string currentYear = "";
                        Ledger ledg = result.ledgers.FirstOrDefault(x => x.StationId == input.stationId);
                        if (ledg != null)
                        {
                            stationname = ledg.StationName;
                            currentYear = DateTime.Now.GetIndianDateTimeNow().Year.ToString();
                        }

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
                        byte[] ct = await _generatePdf.GetByteArray("pdflayout/LedgerLayout.cshtml", result);
                        //var pdfStream = new System.IO.MemoryStream();
                        //pdfStream.Write(ct, 0, ct.Length);
                        //pdfStream.Position = 0;
                        fileName = "LedgerBalance-" + logic.GetMonth(input.currentmonth) + "-" + currentYear + ".pdf";
                        return File(ct, contentType, fileName);
                    }
                    result.Status = false;
                    result.Message = "No data to create file";
                    return StatusCode(StatusCodes.Status400BadRequest, result);
                }
                else
                {
                    result.Status = false;
                    result.Message = "Invalid Input dates to pickup data";
                    return StatusCode(StatusCodes.Status400BadRequest, result);
                }
                //  return File(zipFileContent, contentType, "CDAInvoice" + result.employee.StationCode + result.pdfLayout.BillingPeriod);
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                result.Status = false;
                result.CommandType = "Download";
                result.Id = 0;
                result.EmployeeName = "";
                ErrorLogTrack err = new ErrorLogTrack("DownloadLedgerDetails", e.TargetSite.ReflectedType.FullName, result.CommandType, e.Message, Zone);
                string r = logic.TraceError(err);
                result.Message = result.Message + " and " + r ?? "";
                return StatusCode(StatusCodes.Status500InternalServerError, result);

            }
        }
    }
}
