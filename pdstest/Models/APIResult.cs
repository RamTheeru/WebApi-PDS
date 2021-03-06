﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace pdstest.Models
{
    public class APIResult
    {
        public List<RegisterEmployee> registerEmployees { get; set; }
        public List<Employee> employees { get; set; }
        public UserType userInfo { get; set; }
        public List<UserType> Usertypes { get; set; }
        public List<Designation> Designations { get; set; } 
        public List<Voucher> vouchers { get; set; }
        public List<CommercialConstant> commercialConstants { get; set; }
        public CommercialConstant commercialConstant { get; set; }
        public List<Ledger> ledgers { get; set; }
        public List<Profession> professions { get; set; }
        public List<Station> stations { get; set; }
        public PDFLayout pdfLayout { get; set; }
        public Voucher voucher { get; set; }
        public Ledger ledger { get; set; }
        public string EmployeeName { get; set; }
        public int QueryTotalCount { get; set; }
        public int QueryPages { get; set; }
        public string VoucherNumber { get; set; }
        public string CommandType { get; set; }
        public Employee employee { get; set; }
        public RegisterEmployee registerEmployee { get; set; }
        public bool Status { get; set; }
        public string Message { get; set; }
        public int Id { get; set; }
        public List<HeaderDescription> headers { get; set; }
        public List<RequestDetail> requests { get; set; }
        public List<DbBackupInfo> dbBackups { get; set; }
        public string Host { get; set; }
        public string Path { get; set; }
        public HttpContext Context { get; set; }
        public UploadStatus uploadStatus { get; set; } 

    }
}
