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
        private readonly IPdfFile _pdf;
        private readonly IGeneratePdf _generatePdf;

        public WeatherForecastController(ILogger<WeatherForecastController> logger,IPdfFile pdf,IGeneratePdf generatePdf)
        {
            _logger = logger;
            _pdf = pdf;
            _generatePdf = generatePdf;
        }
        [HttpGet("pdfget")]
        public IActionResult GetPdf()
        {
            string contentType = "application/pdf";
            MemoryStream stream = new MemoryStream();
            //Define the file name.
            stream = _pdf.CreatePdfFilewthTable();
            string fileName = "Output.pdf";
            return File(stream, contentType, fileName);
        }
        [HttpGet("pdffile")]
        public async Task<IActionResult> GetPdffile()
        {
            PDFLayout fil = new PDFLayout();
            fil.HeaderText = "PennaDeliveryServices";
            fil.BodyText = "This is for test purpose";
            byte[] ct=  await _generatePdf.GetByteArray("pdflayout/pdfformat.cshtml", fil);
            string fileName = "PdsSample.pdf";
            string contentType = "application/pdf";
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
    }
}
