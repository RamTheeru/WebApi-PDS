using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wkhtmltopdf.NetCore;
using Wkhtmltopdf.NetCore.Interfaces;
using Wkhtmltopdf;
using Wkhtmltopdf.NetCore.Options;

namespace pdstest.Models
{
    public class PDFLayout
    {
        public string Title { get; set; }
        public string Name { get; set; }
        public string HeaderText { get; set; }
        public string BodyText { get; set; }
        public string  Address { get; set; }
        public string MobileNumber { get; set; }
        public string BillingPeriod { get; set; }
        public string VendorCode { get; set; }
        public string InvoiceDate { get; set; }
        public string PANDetails { get; set; }
        public string BillTo { get; set; }
        public string WorkPerformed { get; set; }
        public string TDS { get; set; }
        public string AmountInWords { get; set; }
        public string Comments { get; set; }
        public string Total { get; set; }
        public string Sign1 { get; set; }
        public string Sign2 { get; set; }
        public string Sign3 { get; set; }
        public List<PdfTbContent> tbContents { get; set; }
    }
    public class GenerateMyPdf : IGeneratePdf
    {
        public void AddView(string path, string viewHTML)
        {
            throw new NotImplementedException();
        }

        public bool ExistsView(string path)
        {
            throw new NotImplementedException();
        }

        public Task<byte[]> GetByteArray<T>(string View, T model)
        {
            throw new NotImplementedException();
        }

        public Task<byte[]> GetByteArray(string View)
        {
            throw new NotImplementedException();
        }

        public Task<byte[]> GetByteArrayViewInHtml<T>(string ViewInHtml, T model)
        {
            throw new NotImplementedException();
        }

        public Task<byte[]> GetByteArrayViewInHtml(string ViewInHtml)
        {
            throw new NotImplementedException();
        }

        public Task<IActionResult> GetPdf<T>(string View, T model)
        {
            throw new NotImplementedException();
        }

        public Task<IActionResult> GetPdf(string View)
        {
            throw new NotImplementedException();
        }

        public byte[] GetPDF(string html)
        {
            throw new NotImplementedException();
        }

        public Task<IActionResult> GetPdfViewInHtml<T>(string ViewInHtml, T model)
        {
            throw new NotImplementedException();
        }

        public Task<IActionResult> GetPdfViewInHtml(string ViewInHtml)
        {
            throw new NotImplementedException();
        }

        public void SetConvertOptions(IConvertOptions options)
        {
            ConvertOptions optons = new ConvertOptions();
            optons.PageWidth = 1032;
            optons.PageHeight = 508;
            throw new NotImplementedException();
        }

        public void UpdateView(string path, string viewHTML)
        {
            throw new NotImplementedException();
        }
    }
}
