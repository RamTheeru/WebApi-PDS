using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pdstest.Models
{
    public class MySqlParamDescription
    {
        public string ParamName { get; set; }
        public string ParamDataType { get; set; }
        public string ParamDirection { get; set; }
        public int ParamLength { get; set; }
    }
}
