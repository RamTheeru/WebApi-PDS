using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pdstest.Models
{
    public class Ledger : Voucher
    {
        public int Id { get; set; }
        public int CurrentCreditAmount { get; set; }
        public int Credit { get; set; }
        public int Debit { get; set; }
        public int Balance { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreditDate { get; set; }
        public string Cred_Date { get; set; }
        public string Particulars { get; set; }
        public string Remarks { get; set; }

    }
}
