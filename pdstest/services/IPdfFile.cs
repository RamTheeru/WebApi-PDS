using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

namespace pdstest.services
{
   public interface IPdfFile
    {
         void CreatePdfFilewithData();
        void CreatePdfFilewithImage();
        MemoryStream CreatePdfFilewthTable();
    }
}
