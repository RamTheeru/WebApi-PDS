using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pdstest.Models
{
    public class UserType
    {
        public int UserTypeId { get; set; }
        public string User { get; set; }
        public string Role { get { return Role; } set { if (UserTypeId == 1) Role = "Admin";
                else if (UserTypeId == 2) Role = "HRLE";
                else if (UserTypeId == 3) Role = "HRHE";
                else if (UserTypeId == 4) Role = "FLE";
                else if (UserTypeId == 5) Role = "FHE";
                else if (UserTypeId == 6) Role = "ELE";
                else if (UserTypeId == 7) Role = "EHE";
            } }

        public bool Valid { get; set; }
    }
}
