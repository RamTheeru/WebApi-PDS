using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using pdstest.services;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Drawing;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Syncfusion.Pdf.Grid;

namespace pdstest.Models
{
    public class CustomPdfFile : IPdfFile
    {
        public void CreatePdfFilewithData()
        {
            //Create a new PDF document
            PdfDocument document = new PdfDocument();

            //Add a page to the document
            PdfPage page = document.Pages.Add();

            //Create PDF graphics for the page
            PdfGraphics graphics = page.Graphics;

            //Set the standard font
            PdfFont font = new PdfStandardFont(PdfFontFamily.Helvetica, 20);

            //Draw the text
            graphics.DrawString("Hello World!!!", font, PdfBrushes.Black, new PointF(0, 0));

            //Saving the PDF to the MemoryStream
            MemoryStream stream = new MemoryStream();

            document.Save(stream);

            //Set the position as '0'.
            stream.Position = 0;

            //Download the PDF document in the browser
            FileStreamResult fileStreamResult = new FileStreamResult(stream, "application/pdf");

            fileStreamResult.FileDownloadName = "Sample.pdf";

           // return fileStreamResult;
        }

        public void CreatePdfFilewithImage()
        {
            //Create a new PDF document.
            PdfDocument doc = new PdfDocument();
            //Add a page to the document.
            PdfPage page = doc.Pages.Add();
            //Create PDF graphics for the page
            PdfGraphics graphics = page.Graphics;
            //Load the image as stream.
            FileStream imageStream = new FileStream("Autumn Leaves.jpg", FileMode.Open, FileAccess.Read);
            PdfBitmap image = new PdfBitmap(imageStream);
            //Draw the image
            graphics.DrawImage(image, 0, 0);
            //Save the PDF document to stream
            MemoryStream stream = new MemoryStream();
            doc.Save(stream);
            //If the position is not set to '0' then the PDF will be empty.
            stream.Position = 0;
            //Close the document.
            doc.Close(true);
            //Defining the ContentType for pdf file.
           // string contentType = "application/pdf";
            //Define the file name.
           // string fileName = "Output.pdf";
            //Creates a FileContentResult object by using the file contents, content type, and file name.
           // return File(stream, contentType, fileName);
        }

        public MemoryStream CreatePdfFilewthTable()
        {
            //Create a new PDF document.
            PdfDocument doc = new PdfDocument();
            //Add a page.
            PdfPage page = doc.Pages.Add();
            //Create a PdfGrid.
            PdfGrid pdfGrid = new PdfGrid();
            //Add values to list
            List<object> data = new List<object>();
            Object row1 = new { ID = "E01", Name = "Clay" };
            Object row2 = new { ID = "E02", Name = "Thomas" };
            Object row3 = new { ID = "E03", Name = "Andrew" };
            Object row4 = new { ID = "E04", Name = "Paul" };
            Object row5 = new { ID = "E05", Name = "Gray" };
            data.Add(row1);
            data.Add(row2);
            data.Add(row3);
            data.Add(row4);
            data.Add(row5);
            //Add list to IEnumerable
            IEnumerable<object> dataTable = data;
            //Assign data source.
            pdfGrid.DataSource = dataTable;
            //Draw grid to the page of PDF document.
            pdfGrid.Draw(page, new Syncfusion.Drawing.PointF(10, 10));
            //Save the PDF document to stream
            MemoryStream stream = new MemoryStream();
            doc.Save(stream);
            //If the position is not set to '0' then the PDF will be empty.
            stream.Position = 0;
            //Close the document.
            doc.Close(true);
            //Defining the ContentType for pdf file.
            //string contentType = "application/pdf";
            //Define the file name.
           // string fileName = "Output.pdf";
            //Creates a FileContentResult object by using the file contents, content type, and file name.
            // return File(stream, contentType, fileName);
            return stream;
        }
    }
}
