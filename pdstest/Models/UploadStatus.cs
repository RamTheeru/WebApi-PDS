using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pdstest.Models
{
    public class UploadStatus
    {
        public string SuccessMessage { get; set; }
        public string ErrorMessage { get; set; }
        public bool uploadStatus { get; set; }
        public List<PDSEmployee> employees { get; set; }

        public List<string> columns { get; set; }
        public List<string> headers { get; set; }

        public List<EmpUploadStatus> stats { get; set; }

    }
    public class EmpUploadStatus
    {
        public string Message { get; set; }
        public bool status { get; set; }
        public string EmpCode { get; set; }
    }
}
