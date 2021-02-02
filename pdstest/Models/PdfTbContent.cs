using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wkhtmltopdf.NetCore.Interfaces;

namespace pdstest.Models
{
    public class PdfTbContent
    {
        public string Description { get; set; }
        public string Quantity { get; set; }
        public string Price { get; set; }
        public string Amount { get; set; }
        public int FinalAmount { get; set; }
    }
    public class MyOptions : IConvertOptions
    {
        public string GetConvertOptions()
        {
            //var options = new CustomOptions
            //{
            //    HeaderHtml = "http://localhost/header.html",
            //    PageOrientation = Wkhtmltopdf.NetCore.Options.Orientation.Landscape
            //};
            throw new NotImplementedException();
        }
    }
}
