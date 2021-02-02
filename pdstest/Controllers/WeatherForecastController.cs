using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using pdstest.Models;
using pdstest.services;
using Wkhtmltopdf.NetCore;
using Wkhtmltopdf.NetCore.Interfaces;
using Wkhtmltopdf;
using Wkhtmltopdf.NetCore.Options;

namespace pdstest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        //private readonly IPdfFile _pdf;
        private readonly IGeneratePdf _generatePdf;

        public WeatherForecastController(ILogger<WeatherForecastController> logger,IGeneratePdf generatePdf)
        {
            _logger = logger;
            //_pdf = pdf;
            _generatePdf = generatePdf;
        }
        //[HttpGet("pdfget")]
        //public IActionResult GetPdf()
        //{
        //    string contentType = "application/pdf";
        //    MemoryStream stream = new MemoryStream();
        //    //Define the file name.
        //    stream = _pdf.CreatePdfFilewithData();
        //    string fileName = "Output.pdf";
        //    return File(stream, contentType, fileName);
        //}
        [HttpGet("pdffile")]
        public async Task<IActionResult> GetPdffile()
        {
            PDFLayout fil = new PDFLayout();
            fil.Title = "PennaDeliveryServices";
            //fil.BodyText = "This is for test purpose";
            fil = this.GetPdfContent();
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
            byte[] ct=  await _generatePdf.GetByteArray("pdflayout/pdfformat.cshtml", fil);
            //var pdfStream = new System.IO.MemoryStream();
            //pdfStream.Write(ct, 0, ct.Length);
            //pdfStream.Position = 0;
            string fileName = "CDAInvoiceFormat.pdf";
            string contentType = "application/pdf";
           //return new FileStreamResult(pdfStream,contentType);
            return File(ct, contentType, fileName);
        }
        [HttpGet("excelfile")]
        public IActionResult Get()
        {
            var rng = new Random();
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            string fileName = "authors.xlsx";
            List<WeatherForecast> list =  Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToList();
            // var workbook = new XLWorkbook();
            try
            {
                using (var workbook = new XLWorkbook())
                {
                    IXLWorksheet worksheet =
                    workbook.Worksheets.Add("tests");
                    worksheet.Cell(1, 1).Value = "Date";
                    worksheet.Cell(1, 2).Value = "TemperatureC";
                    worksheet.Cell(1, 3).Value = "Summary";
                    for (int index = 1; index <= list.Count; index++)
                    {
                        worksheet.Cell(index + 1, 1).Value =
                        list[index - 1].Date;
                        worksheet.Cell(index + 1, 2).Value =
                        list[index - 1].TemperatureC;
                        worksheet.Cell(index + 1, 3).Value =
                        list[index - 1].Summary;
                    }
                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        var content = stream.ToArray();
                        return File(content, contentType, fileName);
                    }
                }
            }
            catch 
            {
                return BadRequest();
            }

            //return Ok(); 
        }
        [NonAction]
        public PDFLayout GetPdfContent()
        {
            PDFLayout pdfC = new PDFLayout();
            pdfC.Name = "Ram Theertha";
            pdfC.Address = "Thapovanam, " + Environment.NewLine + "Anantapur, " + Environment.NewLine + "PINCODE : 500004";
            pdfC.MobileNumber = "95796706668";
            pdfC.BillingPeriod = "JAN-2021";
            pdfC.VendorCode = "V-ATP";
            pdfC.InvoiceDate = DateTime.Now.ToShortDateString();
            pdfC.PANDetails = "AJR476TDH8U";
            pdfC.BillTo = "Billing to someone";
            pdfC.WorkPerformed = "Transport and delivery 50 packges";
            pdfC.tbContents = new List<PdfTbContent>();
            List<PdfTbContent> tbs = new List<PdfTbContent>();
            PdfTbContent tb = new PdfTbContent();
            tb.Price = "200";
            tb.Description = "Delivery Rate";
            tb.Quantity = "1";
            int r = 0;
            r = Convert.ToInt32(tb.Price) * Convert.ToInt32(tb.Quantity);
            tb.FinalAmount = r;
            tb.Amount = r.ToString() ;
            tbs.Add(tb);
             tb = new PdfTbContent();
            tb.Price = "100";
            tb.Description = "Allowances";
            tb.Quantity = "2";
             r = 0;
            r = Convert.ToInt32(tb.Price) * Convert.ToInt32(tb.Quantity);
            tb.FinalAmount = r;
            tb.Amount = r.ToString();
            tbs.Add(tb);
             tb = new PdfTbContent();
            tb.Price = "25";
            tb.Description = "Incentives";
            tb.Quantity = "4";
             r = 0;
            r = Convert.ToInt32(tb.Price) * Convert.ToInt32(tb.Quantity);
            tb.FinalAmount = r;
            tb.Amount = r.ToString();
            tbs.Add(tb);
            pdfC.tbContents = tbs;
            pdfC.TDS = "20";
            int tot = pdfC.tbContents.Sum(x => x.FinalAmount) + Convert.ToInt32(pdfC.TDS);
            pdfC.Total = tot.ToString() ;
            pdfC.AmountInWords = "Five Hundred And Twenty Only";
            pdfC.Comments = "No Comments as of now";
            pdfC.Sign1 = "Signature of Branch Incharge";
            pdfC.Sign2 = "Signature of Signing Authority";
            pdfC.Sign3 = "Signature of Delivery Associate";
            return pdfC;
        }

    }
}
