using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pdstest.Models
{
    public class Voucher
    {
      public int VoucherId { get; set; }
        public DateTime VoucherDate { get; set; }
        public string V_Date { get; set; }
        public string VoucherNumber { get; set; }
        public string PurposeOfPayment { get; set; }
        public string PartyName { get; set; }
        public int NetAmount { get; set; }
        public int TotalAmount { get; set; }
        public int TaxAmount { get; set; }
        public string VoucherStatus { get; set; }
        public int StationId { get; set; }
        public string StationName { get; set; }
        public bool IsApproved { get; set; }
    }
}
