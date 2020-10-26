using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace pdstest.DAL
{
    public class DataBaseResult
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public int Id { get; set; }

        public string EmployeeName { get; set; }
        public DataSet ds { get; set; }

        public DataTable dt { get; set; }
        public string VoucherNumber { get; set; }
        public int QueryTotalCount { get; set; }

        public string CommandType { get; set; }
    }
}
