using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pdstest.Models
{
    public class ErrorLogTrack
    {
        public ErrorLogTrack(string sname,string method, string cmdtype, string reason,string zone)
        {
            ServiceName = sname;
            MethodName = method;
            CommandType = cmdtype;
            Reason = reason;
            Zone = zone;
        }
        public int ErrorId { get; set; }
        public string ServiceName { get; set; }
        public string MethodName { get; set; }
        public string CommandType { get; set; }
        public string Reason { get; set; }
        public string Zone { get; set; }
        public string c_Date { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
