﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using pdstest.Models;
using pdstest.services;

namespace pdstest.DAL
{
    public class MySqlOps : IConnection
    {
        private MySQLDBOperations _ops = new MySQLDBOperations();

        //public MySqlOps(MySQLDBOperations ops)
        //{
        //    _ops = ops;
        //}
        public DataBaseResult CreateEmployee(Employee input,bool isEmployee=false)
        {
           return _ops.CreateEmployee(input,isEmployee);
        }
        public DataBaseResult CreateCDAEmployee(Employee input)
        {
            return _ops.CreateCDAEmployee(input);
        }
        public DataBaseResult GetConstants()
        {
            return _ops.GetConstants();
        }

        public DataBaseResult GetEmployees(string stationCode = "", bool isEmployee = false)
        {
            return _ops.GetEmployees(stationCode,isEmployee);
        }

        public DataBaseResult GetRegisteredUsers(int stationId)
        {
            return _ops.GetRegisteredUsers(stationId);
        }

        public DataBaseResult RegisterEmployee(RegisterEmployee input)
        {
            return _ops.RegisterEmployee(input);
        }
        public DataBaseResult CheckUserExists(string userName)
        {
            return _ops.CheckUserExists(userName);
        }
        public DataBaseResult ApproveUser(int registerId,string status,string empCode="",int pId=0)
        {
            return _ops.ApproveUser(registerId,status,empCode,pId);
        }
        public DataBaseResult GetLoginUserInfo(string username, string password)
        {
            return _ops.GetLoginUserInfo(username,password);
        }
        public DataBaseResult GetPaginationRecords(int stationId, string table, string vstartDate, string vEndDate = "", int? page = 1, int? pagesize = 5, string status = "", bool isEmployee = false)
        {
            return _ops.GetPaginationRecords(stationId, table, vstartDate, vEndDate, page, pagesize, status,isEmployee);
        }
        public bool CheckIfSessionExists(string userName, int employeeId, int userTypeId)
        {
            return _ops.CheckIfSessionExists(userName, employeeId, userTypeId);
        }
        public DataBaseResult InsertVoucher(Voucher input)
        {
            return _ops.InsertVoucher(input);
        }
        public DataBaseResult InsertLedger(Ledger input)
        {
            return _ops.InsertLedger(input);
        }

        public DataBaseResult CreateSession(UserType input)
        {
            return _ops.CreateSession(input);
        }
        public DataBaseResult DeleteSession(string userName,  int employeeId, int userTypeId)
        {
            return _ops.DeleteSession(userName,employeeId, userTypeId);
        }
        public DataBaseResult GetAdminDetails()
        {
            return _ops.GetAdminDetails();
        }
        public DataBaseResult CreateCommercialConsant(CommercialConstant constant)
        {
            return _ops.CreateCommercialConsant(constant);
        }
        public Dictionary<string, string> GetStationNameByStationId(int stationId)
        {
            return _ops.GetStationNameByStationId(stationId);
        }
        public DataBaseResult UpdateDeliveryDetails(List<DeliveryDetails> cdds)
        {
            return _ops.UpdateDeliveryDetails(cdds);
        }
        public DataBaseResult GetCDADeliveryDetails(int empId, int stationId, int currentMonth)
        {
            return _ops.GetCDADeliveryDetails(empId, stationId, currentMonth);
        }
        public DataBaseResult GetDeliveryRatesbyStation(int stationId)
        {
            return _ops.GetDeliveryRatesbyStation(stationId);
        }
        public DataBaseResult CheckEmpCodeExists(string empCode,bool isEmployee)
        {
            return _ops.CheckEmpCodeExists(empCode,isEmployee);
        }
        public DataBaseResult GetEmpDataforPDF(int employeeId)
        {
            return _ops.GetEmpDataforPDF(employeeId);
        }
        public DataBaseResult GetEmpDeliveryDetailsforPDF(int employeeId, int stationId, int currentMonth)
        {
            return _ops.GetEmpDeliveryDetailsforPDF(employeeId, stationId, currentMonth);
        }

    }
}
