﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using pdstest.Models;
using MySql.Data.MySqlClient;
using System.Data;
using pdstest.services;
using Renci.SshNet.Security.Cryptography;

namespace pdstest.DAL
{
    public class MySQLDBOperations 
    {

        public static string connectionString = DBConnection.GetDBConnection(false);


        public DataBaseResult RegisterEmployee(RegisterEmployee input)
        {
            string insertQuery = "";
            DataBaseResult dbr = new DataBaseResult();
            MySqlCommand cmd = new MySqlCommand();
            MySqlParameter param;

            try
            {
                dbr.CommandType = "Insert";
                insertQuery = DBConnection.GetInsertQuery(true);

                if (string.IsNullOrEmpty(insertQuery) || string.IsNullOrEmpty(connectionString))
                {
                    dbr.Id = 0;
                    dbr.Message = "Something Wrong with getting DB Commands!!";
                    dbr.EmployeeName = "";
                    dbr.Status = false;

                }
                else
                {
                    using (MySqlConnection conn = new MySqlConnection(connectionString))
                    {
                        cmd.CommandText = insertQuery;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = conn;

                        param = new MySqlParameter("@FirstName", input.FirstName);
                        param.Direction = ParameterDirection.Input;
                        param.MySqlDbType = MySqlDbType.VarChar;
                        param.Size = 50;
                        cmd.Parameters.Add(param);

                        param = new MySqlParameter("@MiddleName", input.MiddleName);
                        param.Direction = ParameterDirection.Input;
                        param.MySqlDbType = MySqlDbType.VarChar;
                        param.Size = 50;
                        cmd.Parameters.Add(param);

                        param = new MySqlParameter("@LastName", input.LastName);
                        param.Direction = ParameterDirection.Input;
                        param.MySqlDbType = MySqlDbType.VarChar;
                        param.Size = 50;
                        cmd.Parameters.Add(param);

                        param = new MySqlParameter("@DOB", input.DOB);
                        param.Direction = ParameterDirection.Input;
                        param.MySqlDbType = MySqlDbType.VarChar;
                        param.Size = 30;
                        cmd.Parameters.Add(param);

                        param = new MySqlParameter("@Age", input.Age);
                        param.Direction = ParameterDirection.Input;
                        param.MySqlDbType = MySqlDbType.Int32;
                        cmd.Parameters.Add(param);

                        param = new MySqlParameter("@BloodGroup", input.BloodGroup);
                        param.Direction = ParameterDirection.Input;
                        param.MySqlDbType = MySqlDbType.VarChar;
                        param.Size = 5;
                        cmd.Parameters.Add(param);

                        param = new MySqlParameter("@Gender", input.Gender);
                        param.Direction = ParameterDirection.Input;
                        param.MySqlDbType = MySqlDbType.VarChar;
                        param.Size = 5;
                        cmd.Parameters.Add(param);

                        param = new MySqlParameter("@MaritalStatus", input.MaritalStatus);
                        param.Direction = ParameterDirection.Input;
                        param.MySqlDbType = MySqlDbType.Bit;
                        cmd.Parameters.Add(param);

                        param = new MySqlParameter("@Phone", input.Phone);
                        param.Direction = ParameterDirection.Input;
                        param.MySqlDbType = MySqlDbType.VarChar;
                        param.Size = 12;
                        cmd.Parameters.Add(param);

                        param = new MySqlParameter("@Address1", input.Address1);
                        param.Direction = ParameterDirection.Input;
                        param.MySqlDbType = MySqlDbType.LongText;
                        param.Size = -1;
                        cmd.Parameters.Add(param);

                        param = new MySqlParameter("@Address2", input.Address2);
                        param.Direction = ParameterDirection.Input;
                        param.MySqlDbType = MySqlDbType.LongText;
                        param.Size = -1;
                        cmd.Parameters.Add(param);


                        param = new MySqlParameter("@Place", input.Place);
                        param.Direction = ParameterDirection.Input;
                        param.MySqlDbType = MySqlDbType.VarChar;
                        param.Size = 50;
                        cmd.Parameters.Add(param);

                        param = new MySqlParameter("@StateName", input.State);
                        param.Direction = ParameterDirection.Input;
                        param.MySqlDbType = MySqlDbType.VarChar;
                        param.Size = 100;
                        cmd.Parameters.Add(param);

                        param = new MySqlParameter("@PostalCode", input.PostalCode);
                        param.Direction = ParameterDirection.Input;
                        param.MySqlDbType = MySqlDbType.VarChar;
                        param.Size = 10;
                        cmd.Parameters.Add(param);

                        param = new MySqlParameter("@AadharNumber", input.AadharNumber);
                        param.Direction = ParameterDirection.Input;
                        param.MySqlDbType = MySqlDbType.VarChar;
                        param.Size = 20;
                        cmd.Parameters.Add(param);


                        param = new MySqlParameter("@PAN", input.PANNumber);
                        param.Direction = ParameterDirection.Input;
                        param.MySqlDbType = MySqlDbType.VarChar;
                        param.Size = 20;
                        cmd.Parameters.Add(param);

                        param = new MySqlParameter("@IsPermanent", input.IsPermanent);
                        param.Direction = ParameterDirection.Input;
                        param.MySqlDbType = MySqlDbType.Bit;
                        cmd.Parameters.Add(param);
                        input.EmployeeType = (input.IsPermanent == true) ? "Permanent" : "Contract";
                        param = new MySqlParameter("@EmployeeType", input.EmployeeType);
                        param.Direction = ParameterDirection.Input;
                        param.MySqlDbType = MySqlDbType.VarChar;
                        param.Size = 50;
                        cmd.Parameters.Add(param);


                        param = new MySqlParameter("@Gaurd_firstname", input.Gaurd_firstname);
                        param.Direction = ParameterDirection.Input;
                        param.MySqlDbType = MySqlDbType.VarChar;
                        param.Size = 50;
                        cmd.Parameters.Add(param);


                        param = new MySqlParameter("@Gaurd_lastname", input.Gaurd_lastname);
                        param.Direction = ParameterDirection.Input;
                        param.MySqlDbType = MySqlDbType.VarChar;
                        param.Size = 50;
                        cmd.Parameters.Add(param);


                        param = new MySqlParameter("@Gaurd_middlename", input.Gaurd_middlename);
                        param.Direction = ParameterDirection.Input;
                        param.MySqlDbType = MySqlDbType.VarChar;
                        param.Size = 50;
                        cmd.Parameters.Add(param);

                        param = new MySqlParameter("@Gaurd_Phone", input.Gaurd_PhoneNumber);
                        param.Direction = ParameterDirection.Input;
                        param.MySqlDbType = MySqlDbType.VarChar;
                        param.Size = 12;
                        cmd.Parameters.Add(param);

                        param = new MySqlParameter("@DOJ", input.DOJ);
                        param.Direction = ParameterDirection.Input;
                        param.MySqlDbType = MySqlDbType.VarChar;
                        param.Size = 30;
                        cmd.Parameters.Add(param);

                        param = new MySqlParameter("@LoginType", input.LoginType);
                        param.Direction = ParameterDirection.Input;
                        param.MySqlDbType = MySqlDbType.VarChar;
                        param.Size = 50;
                        cmd.Parameters.Add(param);

                        param = new MySqlParameter("@Designation", input.Designation);
                        param.Direction = ParameterDirection.Input;
                        param.MySqlDbType = MySqlDbType.VarChar;
                        param.Size = 50;
                        cmd.Parameters.Add(param);

                        param = new MySqlParameter("@StateCode", input.StationCode);
                        param.Direction = ParameterDirection.Input;
                        param.MySqlDbType = MySqlDbType.VarChar;
                        param.Size = 30;
                        cmd.Parameters.Add(param);

                        param = new MySqlParameter("@LocationName", input.LocationName);
                        param.Direction = ParameterDirection.Input;
                        param.MySqlDbType = MySqlDbType.VarChar;
                        param.Size = 50;
                        cmd.Parameters.Add(param);

                        param = new MySqlParameter("@IsActive", false);
                        param.Direction = ParameterDirection.Input;
                        param.MySqlDbType = MySqlDbType.Bit;
                        cmd.Parameters.Add(param);

                        param = new MySqlParameter("@UsrTypeId", input.UserTypeId);
                        param.Direction = ParameterDirection.Input;
                        param.MySqlDbType = MySqlDbType.Int32;
                        cmd.Parameters.Add(param); 

                        param = new MySqlParameter("@StationId", input.StationId);
                        param.Direction = ParameterDirection.Input;
                        param.MySqlDbType = MySqlDbType.Int32;
                        cmd.Parameters.Add(param);

                        MySqlParameter output = new MySqlParameter();
                        output.ParameterName = "@OutRegisterId";
                        output.MySqlDbType = MySqlDbType.Int32;
                        output.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(output);

                        MySqlParameter output2 = new MySqlParameter();
                        output2.ParameterName = "@EmpName";
                        output2.MySqlDbType = MySqlDbType.VarChar;
                        output2.Size = 50;
                        output2.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(output2);

                        conn.Open();
                        cmd.ExecuteNonQuery();

                        string registerId = output.Value.ToString();

                        string empName = output2.Value.ToString();
                        conn.Close();
                        dbr.Id = string.IsNullOrEmpty(registerId) ? 0 : Convert.ToInt32(registerId);
                        if (dbr.Id > 0)
                        {
                            dbr.EmployeeName = empName;
                            dbr.Status = true;
                            dbr.Message = "Employee Registered Successfully!!!";
                        }
                        else
                        {
                            dbr.Id = 0;
                            dbr.EmployeeName = "";
                            dbr.Status = false;
                            dbr.Message = "Process went well but Something wrong with database Connection!! ";

                        }

                    }

                }



            }
            catch (MySqlException e)
            {
                dbr.Id = 0;
                dbr.EmployeeName = "";
                dbr.Status = false;
                dbr.Message = "Something wrong with database : " + e.Message;
                throw e;

            }
            catch (Exception e)
            {
                dbr.Id = 0;
                dbr.Message = e.Message;
                dbr.EmployeeName = "";
                dbr.Status = false;
                throw e;

            }
            finally
            {
                cmd.Dispose();


            }
            return dbr;
        }

        public DataBaseResult CreateEmployee(Employee input,bool isemployee=false)
        {
            string insertQuery = "";
            DataBaseResult dbr = new DataBaseResult();
            MySqlCommand cmd = new MySqlCommand();
            MySqlParameter param;

            try
            {
                dbr.CommandType = "Insert";
                insertQuery = DBConnection.GetInsertQuery(false);

                if (string.IsNullOrEmpty(insertQuery) || string.IsNullOrEmpty(connectionString))
                {
                    dbr.Id = 0;
                    dbr.Message = "Something Wrong with getting DB Commands!!";
                    dbr.EmployeeName = "";
                    dbr.Status = false;

                }
                else
                {
                    using (MySqlConnection conn = new MySqlConnection(connectionString))
                    {
                        cmd.CommandText = insertQuery;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = conn;

                        param = new MySqlParameter("@FirstName", input.FirstName);
                        param.Direction = ParameterDirection.Input;
                        param.MySqlDbType = MySqlDbType.VarChar;
                        param.Size = 50;
                        cmd.Parameters.Add(param);

                        param = new MySqlParameter("@MiddleName", input.MiddleName);
                        param.Direction = ParameterDirection.Input;
                        param.MySqlDbType = MySqlDbType.VarChar;
                        param.Size = 50;
                        cmd.Parameters.Add(param);

                        param = new MySqlParameter("@LastName", input.LastName);
                        param.Direction = ParameterDirection.Input;
                        param.MySqlDbType = MySqlDbType.VarChar;
                        param.Size = 50;
                        cmd.Parameters.Add(param);

                        param = new MySqlParameter("@DOB", input.DOB);
                        param.Direction = ParameterDirection.Input;
                        param.MySqlDbType = MySqlDbType.VarChar;
                        param.Size = 30;
                        cmd.Parameters.Add(param);

                        param = new MySqlParameter("@Age", input.Age);
                        param.Direction = ParameterDirection.Input;
                        param.MySqlDbType = MySqlDbType.Int32;
                        cmd.Parameters.Add(param);

                        param = new MySqlParameter("@BloodGroup", input.BloodGroup);
                        param.Direction = ParameterDirection.Input;
                        param.MySqlDbType = MySqlDbType.VarChar;
                        param.Size = 5;
                        cmd.Parameters.Add(param);

                        param = new MySqlParameter("@Gender", input.Gender);
                        param.Direction = ParameterDirection.Input;
                        param.MySqlDbType = MySqlDbType.VarChar;
                        param.Size = 5;
                        cmd.Parameters.Add(param);

                        param = new MySqlParameter("@MaritalStatus", input.MaritalStatus);
                        param.Direction = ParameterDirection.Input;
                        param.MySqlDbType = MySqlDbType.Bit;
                        cmd.Parameters.Add(param);

                        param = new MySqlParameter("@Phone", input.Phone);
                        param.Direction = ParameterDirection.Input;
                        param.MySqlDbType = MySqlDbType.VarChar;
                        param.Size = 12;
                        cmd.Parameters.Add(param);

                        param = new MySqlParameter("@Address1", input.Address1);
                        param.Direction = ParameterDirection.Input;
                        param.MySqlDbType = MySqlDbType.LongText;
                        param.Size = -1;
                        cmd.Parameters.Add(param);

                        param = new MySqlParameter("@Address2", input.Address2);
                        param.Direction = ParameterDirection.Input;
                        param.MySqlDbType = MySqlDbType.LongText;
                        param.Size = -1;
                        cmd.Parameters.Add(param);


                        param = new MySqlParameter("@Place", input.Place);
                        param.Direction = ParameterDirection.Input;
                        param.MySqlDbType = MySqlDbType.VarChar;
                        param.Size = 50;
                        cmd.Parameters.Add(param);

                        param = new MySqlParameter("@StateName", input.State);
                        param.Direction = ParameterDirection.Input;
                        param.MySqlDbType = MySqlDbType.VarChar;
                        param.Size = 100;
                        cmd.Parameters.Add(param);

                        param = new MySqlParameter("@PostalCode", input.PostalCode);
                        param.Direction = ParameterDirection.Input;
                        param.MySqlDbType = MySqlDbType.VarChar;
                        param.Size = 10;
                        cmd.Parameters.Add(param);

                        param = new MySqlParameter("@AadharNumber", input.AadharNumber);
                        param.Direction = ParameterDirection.Input;
                        param.MySqlDbType = MySqlDbType.VarChar;
                        param.Size = 20;
                        cmd.Parameters.Add(param);


                        param = new MySqlParameter("@PAN", input.PANNumber);
                        param.Direction = ParameterDirection.Input;
                        param.MySqlDbType = MySqlDbType.VarChar;
                        param.Size = 20;
                        cmd.Parameters.Add(param);

                        param = new MySqlParameter("@IsPermanent", input.IsPermanent);
                        param.Direction = ParameterDirection.Input;
                        param.MySqlDbType = MySqlDbType.Bit;
                        cmd.Parameters.Add(param);
                        input.EmployeeType = (input.IsPermanent == true) ? "Permanent" : "Contract";
                        param = new MySqlParameter("@EmployeeType", input.EmployeeType);
                        param.Direction = ParameterDirection.Input;
                        param.MySqlDbType = MySqlDbType.VarChar;
                        param.Size = 50;
                        cmd.Parameters.Add(param);

                        string lastname = input.Gaurd_lastname == null ? "" : input.Gaurd_lastname;
                        string midname = input.Gaurd_middlename == null ? "" : input.Gaurd_middlename;
                        string fullname = input.Gaurd_firstname + ' ' + midname + ' ' + lastname;
                        param = new MySqlParameter("@Gaurd_fullname",fullname);
                        param.Direction = ParameterDirection.Input;
                        param.MySqlDbType = MySqlDbType.VarChar;
                        param.Size = 50;
                        cmd.Parameters.Add(param);


                        //param = new MySqlParameter("@Gaurd_lastname", input.Gaurd_lastname);
                        //param.Direction = ParameterDirection.Input;
                        //param.MySqlDbType = MySqlDbType.VarChar;
                        //param.Size = 50;
                        //cmd.Parameters.Add(param);


                        //param = new MySqlParameter("@Gaurd_middlename", input.Gaurd_middlename);
                        //param.Direction = ParameterDirection.Input;
                        //param.MySqlDbType = MySqlDbType.VarChar;
                        //param.Size = 50;
                        //cmd.Parameters.Add(param);

                        param = new MySqlParameter("@Gaurd_Phone", input.Gaurd_PhoneNumber);
                        param.Direction = ParameterDirection.Input;
                        param.MySqlDbType = MySqlDbType.VarChar;
                        param.Size = 12;
                        cmd.Parameters.Add(param);

                        param = new MySqlParameter("@DOJ", input.DOJ);
                        param.Direction = ParameterDirection.Input;
                        param.MySqlDbType = MySqlDbType.VarChar;
                        param.Size = 30;
                        cmd.Parameters.Add(param);

                        param = new MySqlParameter("@LoginType", input.LoginType);
                        param.Direction = ParameterDirection.Input;
                        param.MySqlDbType = MySqlDbType.VarChar;
                        param.Size = 50;
                        cmd.Parameters.Add(param);

                        param = new MySqlParameter("@Designation", input.Designation);
                        param.Direction = ParameterDirection.Input;
                        param.MySqlDbType = MySqlDbType.VarChar;
                        param.Size = 50;
                        cmd.Parameters.Add(param);

                        param = new MySqlParameter("@StateCode", input.StationCode);
                        param.Direction = ParameterDirection.Input;
                        param.MySqlDbType = MySqlDbType.VarChar;
                        param.Size = 30;
                        cmd.Parameters.Add(param);

                        param = new MySqlParameter("@LocationName", input.LocationName);
                        param.Direction = ParameterDirection.Input;
                        param.MySqlDbType = MySqlDbType.VarChar;
                        param.Size = 50;
                        cmd.Parameters.Add(param);

                        param = new MySqlParameter("@IsActive", false);
                        param.Direction = ParameterDirection.Input;
                        param.MySqlDbType = MySqlDbType.Bit;
                        cmd.Parameters.Add(param);

                        param = new MySqlParameter("@IsEmployee", isemployee);
                        param.Direction = ParameterDirection.Input;
                        param.MySqlDbType = MySqlDbType.Bit;
                        cmd.Parameters.Add(param);
                        
                        param = new MySqlParameter("@UsrTypeId", input.UserTypeId);
                        param.Direction = ParameterDirection.Input;
                        param.MySqlDbType = MySqlDbType.Int32;
                        cmd.Parameters.Add(param);

                        param = new MySqlParameter("@DLLRStatus", input.DLLRStatus);
                        param.Direction = ParameterDirection.Input;
                        param.MySqlDbType = MySqlDbType.VarChar;
                        param.Size = 30;
                        cmd.Parameters.Add(param);

                        param = new MySqlParameter("@DLLRNumber", input.DLLRNumber);
                        param.Direction = ParameterDirection.Input;
                        param.MySqlDbType = MySqlDbType.VarChar;
                        param.Size = 30;
                        cmd.Parameters.Add(param);


                        param = new MySqlParameter("@VehicleNumber", input.VehicleNumber);
                        param.Direction = ParameterDirection.Input;
                        param.MySqlDbType = MySqlDbType.VarChar;
                        param.Size = 30;
                        cmd.Parameters.Add(param);


                        param = new MySqlParameter("@BankAccountNumber", input.BankAccountNumber);
                        param.Direction = ParameterDirection.Input;
                        param.MySqlDbType = MySqlDbType.VarChar;
                        param.Size = 30;
                        cmd.Parameters.Add(param);

                        param = new MySqlParameter("@BankName", input.BankName);
                        param.Direction = ParameterDirection.Input;
                        param.MySqlDbType = MySqlDbType.VarChar;
                        param.Size = 30;
                        cmd.Parameters.Add(param);

                        param = new MySqlParameter("@BranchName", input.BranchName);
                        param.Direction = ParameterDirection.Input;
                        param.MySqlDbType = MySqlDbType.VarChar;
                        param.Size = 30;
                        cmd.Parameters.Add(param);

                        param = new MySqlParameter("@IFSCCode", input.IFSCCode);
                        param.Direction = ParameterDirection.Input;
                        param.MySqlDbType = MySqlDbType.VarChar;
                        param.Size = 30;
                        cmd.Parameters.Add(param);

                        param = new MySqlParameter("@StationId", input.StationId);
                        param.Direction = ParameterDirection.Input;
                        param.MySqlDbType = MySqlDbType.Int32;
                        cmd.Parameters.Add(param);

                        MySqlParameter output = new MySqlParameter();
                        output.ParameterName = "@EmpId";
                        output.MySqlDbType = MySqlDbType.Int32;
                        output.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(output);

                        MySqlParameter output2 = new MySqlParameter();
                        output2.ParameterName = "@EmpName";
                        output2.MySqlDbType = MySqlDbType.VarChar;
                        output2.Size = 50;
                        output2.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(output2);

                        conn.Open();
                        cmd.ExecuteNonQuery();

                        string employeeId = output.Value.ToString();

                        string empName = output2.Value.ToString();
                        conn.Close();
                        dbr.Id = string.IsNullOrEmpty(employeeId) ? 0 : Convert.ToInt32(employeeId);
                        if (dbr.Id > 0)
                        {
                            dbr.EmployeeName = empName;
                            dbr.Status = true;
                            dbr.Message = "Employee Added Successfully!!!";
                        }
                        else
                        {
                            dbr.Id = 0;
                            dbr.EmployeeName = "";
                            dbr.Status = false;
                            dbr.Message = "Process went well but Something wrong with database Connection!! ";

                        }

                    }

                }



            }
            catch (MySqlException e)
            {
                dbr.Id = 0;
                dbr.EmployeeName = "";
                dbr.Status = false;
                dbr.Message = "Something wrong with database : " + e.Message;
                throw e;

            }
            catch (Exception e)
            {
                dbr.Id = 0;
                dbr.Message = e.Message;
                dbr.EmployeeName = "";
                dbr.Status = false;
                throw e;

            }
            finally
            {
                cmd.Dispose();


            }
            return dbr;
        }

        public DataBaseResult InsertVoucher(Voucher input)
        {
            string insertQuery = "";
            DataBaseResult dbr = new DataBaseResult();
            MySqlCommand cmd = new MySqlCommand();
            MySqlParameter param;

            try
            {
                dbr.CommandType = "Insert";
                insertQuery = DBConnection.GetVoucherInsertQuery();

                if (string.IsNullOrEmpty(insertQuery) || string.IsNullOrEmpty(connectionString))
                {
                    dbr.Id = 0;
                    dbr.Message = "Something Wrong with getting DB Commands!!";
                    dbr.EmployeeName = "";
                    dbr.Status = false;

                }
                else
                {
                    using (MySqlConnection conn = new MySqlConnection(connectionString))
                    {
                        input.VoucherNumber = input.VoucherNumber.CleanString();
                        string cmdT = string.Format("SELECT * FROM Voucher where VoucherNumber = '{0}'",input.VoucherNumber);
                        bool isExists = false;
                        isExists = new BasicDBOps().CheckRecordCountExistsOrNot(connectionString, cmdT);
                        if (isExists) 
                        {
                            dbr.Id = 0;
                            dbr.VoucherNumber = input.VoucherNumber;
                            dbr.Status = false;
                            dbr.Message = "This Voucher already exists..Please Create another Voucher!! ";

                        }
                        else 
                        {
                            cmd.CommandText = insertQuery;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Connection = conn;

                            param = new MySqlParameter("@VoucherNumber", input.VoucherNumber);
                            param.Direction = ParameterDirection.Input;
                            param.MySqlDbType = MySqlDbType.VarChar;
                            param.Size = 50;
                            cmd.Parameters.Add(param);

                            param = new MySqlParameter("@VoucherDate", input.VoucherDate);
                            param.Direction = ParameterDirection.Input;
                            param.MySqlDbType = MySqlDbType.DateTime;
                            cmd.Parameters.Add(param);

                            param = new MySqlParameter("@PurposeOfPayment", input.PurposeOfPayment);
                            param.Direction = ParameterDirection.Input;
                            param.MySqlDbType = MySqlDbType.VarChar;
                            param.Size = 50;
                            cmd.Parameters.Add(param);

                            param = new MySqlParameter("@PartyName", input.PartyName);
                            param.Direction = ParameterDirection.Input;
                            param.MySqlDbType = MySqlDbType.VarChar;
                            param.Size = 30;
                            cmd.Parameters.Add(param);

                            param = new MySqlParameter("@NetAmount", input.NetAmount);
                            param.Direction = ParameterDirection.Input;
                            param.MySqlDbType = MySqlDbType.Int32;
                            cmd.Parameters.Add(param);

                            param = new MySqlParameter("@TotalAmount", input.TotalAmount);
                            param.Direction = ParameterDirection.Input;
                            param.MySqlDbType = MySqlDbType.Int32;
                            cmd.Parameters.Add(param);

                            param = new MySqlParameter("@TaxAmount", input.TaxAmount);
                            param.Direction = ParameterDirection.Input;
                            param.MySqlDbType = MySqlDbType.Int32;
                            cmd.Parameters.Add(param);

                            param = new MySqlParameter("@StationId", input.StationId);
                            param.Direction = ParameterDirection.Input;
                            param.MySqlDbType = MySqlDbType.Int32;
                            cmd.Parameters.Add(param);


                            MySqlParameter output = new MySqlParameter();
                            output.ParameterName = "@VId";
                            output.MySqlDbType = MySqlDbType.Int32;
                            output.Direction = ParameterDirection.Output;
                            cmd.Parameters.Add(output);

                            MySqlParameter output2 = new MySqlParameter();
                            output2.ParameterName = "@VoucherNum";
                            output2.MySqlDbType = MySqlDbType.VarChar;
                            output2.Size = 50;
                            output2.Direction = ParameterDirection.Output;
                            cmd.Parameters.Add(output2);

                            conn.Open();
                            cmd.ExecuteNonQuery();

                            string vId = output.Value.ToString();

                            string vNum = output2.Value.ToString();
                            conn.Close();
                            dbr.Id = string.IsNullOrEmpty(vId) ? 0 : Convert.ToInt32(vId);
                            if (dbr.Id > 0)
                            {
                                dbr.VoucherNumber = vNum;
                                dbr.Status = true;
                                dbr.Message = "Voucher Added Successfully!!!";
                            }
                            else
                            {
                                dbr.Id = 0;
                                dbr.VoucherNumber = "";
                                dbr.Status = false;
                                dbr.Message = "Process went well but Something wrong with database Connection!! ";

                            }
                        }

                        }

                }



            }
            catch (MySqlException e)
            {
                dbr.Id = 0;
                dbr.EmployeeName = "";
                dbr.Status = false;
                dbr.Message = "Something wrong with database : " + e.Message;
                throw e;

            }
            catch (Exception e)
            {
                dbr.Id = 0;
                dbr.Message = e.Message;
                dbr.EmployeeName = "";
                dbr.Status = false;
                throw e;

            }
            finally
            {
                cmd.Dispose();


            }
            return dbr;
        }

        public DataBaseResult InsertLedger(Ledger input)
        {
            string insertQuery = "";
            DataBaseResult dbr = new DataBaseResult();
            MySqlCommand cmd = new MySqlCommand();
            MySqlParameter param;

            try
            {
                dbr.CommandType = "Insert";
                insertQuery = DBConnection.GetLedgerInsertQuery();

                if (string.IsNullOrEmpty(insertQuery) || string.IsNullOrEmpty(connectionString))
                {
                    dbr.Id = 0;
                    dbr.Message = "Something Wrong with getting DB Commands!!";
                    dbr.EmployeeName = "";
                    dbr.Status = false;

                }
                else
                {
                    using (MySqlConnection conn = new MySqlConnection(connectionString))
                    {
                        cmd.CommandText = insertQuery;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = conn;

                        param = new MySqlParameter("@CreditDate", input.CreditDate);
                        param.Direction = ParameterDirection.Input;
                        param.MySqlDbType = MySqlDbType.DateTime;
                        cmd.Parameters.Add(param);


                        param = new MySqlParameter("@Credit", input.Credit);
                        param.Direction = ParameterDirection.Input;
                        param.MySqlDbType = MySqlDbType.Int32;
                        cmd.Parameters.Add(param);


                        param = new MySqlParameter("@StationId", input.StationId);
                        param.Direction = ParameterDirection.Input;
                        param.MySqlDbType = MySqlDbType.Int32;
                        cmd.Parameters.Add(param);


                        MySqlParameter output = new MySqlParameter();
                        output.ParameterName = "@CreditId";
                        output.MySqlDbType = MySqlDbType.Int32;
                        output.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(output);

                        conn.Open();
                        cmd.ExecuteNonQuery();

                        string lId = output.Value.ToString();

                        conn.Close();
                        dbr.Id = string.IsNullOrEmpty(lId) ? 0 : Convert.ToInt32(lId);
                        if (dbr.Id > 0)
                        {
                            dbr.Status = true;
                            dbr.Message = "Credit Added Successfully!!!";
                        }
                        else
                        {
                            dbr.Id = 0;
                            dbr.EmployeeName = "";
                            dbr.Status = false;
                            dbr.Message = "Process went well but Something wrong with database Connection!! ";

                        }

                    }

                }



            }
            catch (MySqlException e)
            {
                dbr.Id = 0;
                dbr.EmployeeName = "";
                dbr.Status = false;
                dbr.Message = "Something wrong with database : " + e.Message;
                throw e;

            }
            catch (Exception e)
            {
                dbr.Id = 0;
                dbr.Message = e.Message;
                dbr.EmployeeName = "";
                dbr.Status = false;
                throw e;

            }
            finally
            {
                cmd.Dispose();


            }
            return dbr;
        }

        public DataBaseResult CreateSession(UserType input)
        {
            string insertQuery = "";
            DataBaseResult dbr = new DataBaseResult();
            MySqlCommand cmd = new MySqlCommand();
            MySqlParameter param;

            try
            {
                dbr.CommandType = "Insert";
                insertQuery = DBConnection.GetCreateSessionQuery();
                dbr.CommandType = "Session Insert";
                if (string.IsNullOrEmpty(insertQuery) || string.IsNullOrEmpty(connectionString))
                {
                    dbr.Id = 0;
                    dbr.Message = "Something Wrong with getting DB Commands!!";
                    dbr.EmployeeName = "";
                    dbr.Status = false;

                }
                else
                {
                    using (MySqlConnection conn = new MySqlConnection(connectionString))
                    {
                        cmd.CommandText = insertQuery;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = conn;

                        param = new MySqlParameter("@UserName", input.User);
                        param.Direction = ParameterDirection.Input;
                        param.MySqlDbType = MySqlDbType.VarChar;
                        param.Size = 100;
                        cmd.Parameters.Add(param);


                        param = new MySqlParameter("@UserTypeId", input.UserTypeId);
                        param.Direction = ParameterDirection.Input;
                        param.MySqlDbType = MySqlDbType.Int32;
                        cmd.Parameters.Add(param);

                        input.SessionStartDate = DateTime.Now.DateTimetoString();
                        input.SessionEndDate = DateTime.Now.AddMinutes(5).DateTimetoString();

                        input.StartDate = input.SessionStartDate.StringtoDateTime();
                        input.EndDate = input.SessionEndDate.StringtoDateTime();

                        param = new MySqlParameter("@StartDate", input.StartDate);
                        param.Direction = ParameterDirection.Input;
                        param.MySqlDbType = MySqlDbType.DateTime;
                        cmd.Parameters.Add(param);


                        param = new MySqlParameter("@EndDate", input.EndDate);
                        param.Direction = ParameterDirection.Input;
                        param.MySqlDbType = MySqlDbType.DateTime;
                        cmd.Parameters.Add(param);

                        

                        param = new MySqlParameter("@Token", input.Token);
                        param.Direction = ParameterDirection.Input;
                        param.MySqlDbType = MySqlDbType.LongText;
                        param.Size = -1;
                        cmd.Parameters.Add(param);

                        param = new MySqlParameter("@EmployeeId", input.EmployeeId);
                        param.Direction = ParameterDirection.Input;
                        param.MySqlDbType = MySqlDbType.Int32;
                        cmd.Parameters.Add(param);

                        MySqlParameter output = new MySqlParameter();
                        output.ParameterName = "@SId";
                        output.MySqlDbType = MySqlDbType.Int32;
                        output.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(output);


                        MySqlParameter output2 = new MySqlParameter();
                        output2.ParameterName = "@Usrtype";
                        output2.MySqlDbType = MySqlDbType.Int32;
                        output2.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(output2);

                        MySqlParameter output3 = new MySqlParameter();
                        output3.ParameterName = "@EmpId";
                        output3.MySqlDbType = MySqlDbType.Int32;
                        output3.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(output3);

                        MySqlParameter output4 = new MySqlParameter();
                        output4.ParameterName = "@UsrToken";
                        output4.MySqlDbType = MySqlDbType.LongText;
                        output4.Size = -1;
                        output4.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(output4);

                        MySqlParameter output5 = new MySqlParameter();
                        output5.ParameterName = "@Usrname";
                        output5.MySqlDbType = MySqlDbType.VarChar;
                        output5.Size = 100;
                        output5.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(output5);

                        MySqlParameter output6 = new MySqlParameter();
                        output6.ParameterName = "@IsAlreadySession";
                        output6.MySqlDbType = MySqlDbType.Bit;
                        output6.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(output6);

                        conn.Open();
                        cmd.ExecuteNonQuery();

                        dbr.ds = new DataSet();

                        string sId = output.Value.ToString();
                        string utId = output2.Value.ToString();
                        string eId = output3.Value.ToString();
                        string utkn = output4.Value.ToString();
                        string usrnm = output5.Value.ToString();
                        string isssn = output6.Value.ToString();

                        conn.Close();
                        dbr.Id = string.IsNullOrEmpty(sId) ? 0 : Convert.ToInt32(sId);

                        if (dbr.Id > 0)
                        {
                            int usrType = string.IsNullOrEmpty(utId) ? 0 : Convert.ToInt32(utId);
                            int empId = string.IsNullOrEmpty(eId) ? 0 : Convert.ToInt32(eId);
                            bool isSession = string.IsNullOrEmpty(isssn) ? false : isssn == "0" ? false :true ;
                           
                            dbr.dt = new DataTable();
                            dbr.dt.Clear();
                            dbr.dt.Columns.Add("UserType");
                            dbr.dt.Columns.Add("EmployeeId");
                            dbr.dt.Columns.Add("UserToken");
                            dbr.dt.Columns.Add("UserName");
                            dbr.dt.Columns.Add("IsAlreadySession");
                            DataRow dr = dbr.dt.NewRow();
                            dr["UserType"] = usrType;
                            dr["EmployeeId"] = empId;
                            dr["UserToken"] = utkn;
                            dr["UserName"] = usrnm;
                            dr["IsAlreadySession"] = isSession;
                            dbr.dt.Rows.Add(dr);
                            dbr.ds.Tables.Add(dbr.dt);
                            dbr.Status = true;
                           
                            dbr.Message = "User Authenticated Sucessfully with Session!!!!!!!";
                        }
                        else
                        {
                            dbr.Id = 0;
                            dbr.EmployeeName = "";
                            dbr.Status = false;
                            dbr.Message = "Unable to Create the Session for this user, Please contact support team!! ";

                        }

                    }

                }



            }
            catch (MySqlException e)
            {
                dbr.Id = 0;
                dbr.EmployeeName = "";
                dbr.Status = false;
                dbr.Message = "Something wrong with database : " + e.Message;
                throw e;

            }
            catch (Exception e)
            {
                dbr.Id = 0;
                dbr.Message = e.Message;
                dbr.EmployeeName = "";
                dbr.Status = false;
                throw e;

            }
            finally
            {
                cmd.Dispose();


            }
            return dbr;
        }

        public DataBaseResult GetConstants()
        {
            string getUserTypes = "";
            DataBaseResult dbr = new DataBaseResult();
            MySqlCommand cmd = new MySqlCommand();
            MySqlDataAdapter sda;
            try
            {
                dbr.CommandType = "Select";
                getUserTypes = DBConnection.GetConstants();

                if (string.IsNullOrEmpty(getUserTypes) || string.IsNullOrEmpty(connectionString))
                {
                    dbr.Id = 0;
                    dbr.Message = "Something Wrong with getting DB Commands!!";
                    dbr.EmployeeName = "";
                    dbr.Status = false;
                    dbr.dt = new DataTable();
                    dbr.ds = new DataSet();
                }
                else
                {
                    //using (MySqlConnection conn = new MySqlConnection(connectionString))
                    //{
                        DataSet ds = new DataSet();
                        dbr.ds = new DataSet();
                        //DataTable dt = new DataTable();
                        //sda = new MySqlDataAdapter(getUserTypes, conn);
                        //sda.SelectCommand.CommandType = CommandType.Text;
                        //sda.Fill(ds);

                        ds = new BasicDBOps().GetMultipleRecords(connectionString, getUserTypes);
                        int count = 0;
                        count = ds.Tables[0].Rows.Count;
                        if (ds.Tables.Count > 0 && count > 0)
                        {
                            //foreach (DataRow dr in dt.Rows)
                            //{
                            //    Console.WriteLine(string.Format("user_id = {0}", dr["user_id"].ToString()));
                            //}
                            dbr.ds = ds;
                            dbr.Message = "Records retreived Successfully!!!";
                            dbr.Status = true;

                        }
                        else if (count == 0)
                        {
                            dbr.ds = ds;
                            dbr.Message = "No Records Found for this request!!";
                            dbr.Status = true;


                        }

                   // }




                }


            }
            catch (MySqlException e)
            {

                dbr.Status = false;
                dbr.Message = "Something wrong with database : " + e.Message;
                throw e;

            }
            catch (Exception e)
            {
                dbr.Message = e.Message;
                dbr.Status = false;
                throw e;
            }
            finally
            {
                cmd.Dispose();


            }
            return dbr;


        }
        public DataBaseResult GetPaginationRecords(int stationId, string table, string vstartDate, string vEndDate = "", int? page = 1, int? pagesize = 5, string status = "", bool isEmployee = false)
        {
            Dictionary<string, string> getSelectQuery = new Dictionary<string, string>();
            DataBaseResult dbr = new DataBaseResult();
            MySqlCommand cmd = new MySqlCommand();
           // MySqlDataAdapter sda;
            try
            {
                dbr.CommandType = "Select";
                getSelectQuery = DBConnection.GetRecordsforPagination(stationId, table,vstartDate,vEndDate,page,pagesize,status,isEmployee);

                if (string.IsNullOrEmpty(getSelectQuery["main"]) || string.IsNullOrEmpty(connectionString))
                {
                    dbr.Id = 0;
                    dbr.Message = "Something Wrong with getting DB Commands!!";
                    dbr.EmployeeName = "";
                    dbr.Status = false;
                    dbr.dt = new DataTable();
                    dbr.ds = new DataSet();
                }
                else
                {
                    using (MySqlConnection conn = new MySqlConnection(connectionString))
                    {
                        DataSet ds = new DataSet();
                        dbr.ds = new DataSet();
                        // sda = new MySqlDataAdapter(getUserInfo, conn);
                        //sda.SelectCommand.CommandType = CommandType.Text;
                        //sda.Fill(ds);
                        //cmd = new MySqlCommand(getSelectQuery["main"], conn);
                        //DataTable temp = new DataTable();
                        //MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                        //adapter.Fill(temp);
                        ds= new BasicDBOps().GetMultipleRecords(connectionString, getSelectQuery["main"]);
                        dbr.ds = ds;
                       // cmd = new MySqlCommand(getSelectQuery["count"], conn);
                        int count =  0;
                        count = new BasicDBOps().GetTotalCountOfQuery(connectionString, getSelectQuery["count"]);
                        if (ds.Tables.Count > 0 && count > 0)
                        {
                            //foreach (DataRow dr in dt.Rows)
                            //{
                            //    Console.WriteLine(string.Format("user_id = {0}", dr["user_id"].ToString()));
                            //}
                            dbr.QueryTotalCount = count;
                            dbr.ds = ds;
                            dbr.Message = "Record(s) retreived Successfully!!!";
                            dbr.Status = true;

                        }
                        else if (count == 0)
                        {
                            dbr.ds = ds;
                            dbr.Message = "No Records Found for this request!!";
                            dbr.Status = true;


                        }

                    }




                }


            }
            catch (MySqlException e)
            {

                dbr.Status = false;
                dbr.Message = "Something wrong with database : " + e.Message;
                throw e;

            }
            catch (Exception e)
            {
                dbr.Message = e.Message;
                dbr.Status = false;
                throw e;
            }
            finally
            {
                cmd.Dispose();


            }
            return dbr;


        }
        public DataBaseResult GetLoginUserInfo(string username,string password)
        {
            string getUserInfo = "";
            DataBaseResult dbr = new DataBaseResult();
            MySqlCommand cmd = new MySqlCommand();
            try
            {
                dbr.CommandType = "Select";
                getUserInfo = DBConnection.GetLoginUserInfo(username,password);

                if (string.IsNullOrEmpty(getUserInfo) || string.IsNullOrEmpty(connectionString))
                {
                    dbr.Id = 0;
                    dbr.Message = "Something Wrong with getting DB Commands!!";
                    dbr.EmployeeName = "";
                    dbr.Status = false;
                    dbr.dt = new DataTable();
                    dbr.ds = new DataSet();
                }
                else
                {
                    //using (MySqlConnection conn = new MySqlConnection(connectionString))
                    //{
                        DataSet ds = new DataSet();
                        dbr.ds = new DataSet();
                    // sda = new MySqlDataAdapter(getUserInfo, conn);
                    //sda.SelectCommand.CommandType = CommandType.Text;
                    //sda.Fill(ds);
                    //cmd = new MySqlCommand(getUserInfo, conn);
                    //DataTable temp = new DataTable();
                    //MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    //adapter.Fill(ds);

                    //ds.Tables.Add(temp);
                    ds = new BasicDBOps().GetMultipleRecords(connectionString,getUserInfo);
                        int count = 0;
                        count = ds.Tables[0].Rows.Count;
                        if (ds.Tables.Count > 0 && count > 0)
                        {
                            //foreach (DataRow dr in dt.Rows)
                            //{
                            //    Console.WriteLine(string.Format("user_id = {0}", dr["user_id"].ToString()));
                            //}
                            dbr.ds = ds;
                            dbr.Message = "Record(s) retreived Successfully!!!";
                            dbr.Status = true;

                        }
                        else if (count == 0)
                        {
                            dbr.ds = ds;
                            dbr.Message = "No Records Found for this request!!";
                            dbr.Status = true;


                        }

                    //}




                }


            }
            catch (MySqlException e)
            {

                dbr.Status = false;
                dbr.Message = "Something wrong with database : " + e.Message;
                throw e;

            }
            catch (Exception e)
            {
                dbr.Message = e.Message;
                dbr.Status = false;
                throw e;
            }
            finally
            {
                cmd.Dispose();


            }
            return dbr;


        }

        public DataBaseResult GetLoginSessionDetails(UserType usr)
        {
            string getUserSessionInfo = "";
            DataBaseResult dbr = new DataBaseResult();
            MySqlCommand cmd = new MySqlCommand();
            try
            {
                dbr.CommandType = "Select";
                getUserSessionInfo = DBConnection.GetLoginSessionInfo(usr);

                if (string.IsNullOrEmpty(getUserSessionInfo) || string.IsNullOrEmpty(getUserSessionInfo))
                {
                    dbr.Id = 0;
                    dbr.Message = "Something Wrong with getting DB Commands!!";
                    dbr.EmployeeName = "";
                    dbr.Status = false;
                    dbr.dt = new DataTable();
                    dbr.ds = new DataSet();
                }
                else
                {
                    //using (MySqlConnection conn = new MySqlConnection(connectionString))
                    //{
                        DataSet ds = new DataSet();
                        dbr.ds = new DataSet();
                    // sda = new MySqlDataAdapter(getUserInfo, conn);
                    //sda.SelectCommand.CommandType = CommandType.Text;
                    //sda.Fill(ds);
                    //cmd = new MySqlCommand(getUserSessionInfo, conn);
                    //DataTable temp = new DataTable();
                    //MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    //adapter.Fill(temp);
                    ds = new BasicDBOps().GetMultipleRecords(connectionString, getUserSessionInfo);
                        //ds.Tables.Add(temp);
                        int count = 0;
                        count = ds.Tables[0].Rows.Count;
                        if (ds.Tables.Count > 0 && count > 0)
                        {
                            //foreach (DataRow dr in dt.Rows)
                            //{
                            //    Console.WriteLine(string.Format("user_id = {0}", dr["user_id"].ToString()));
                            //}
                            dbr.ds = ds;
                            dbr.Message = "Record(s) retreived Successfully!!!";
                            dbr.Status = true;

                        }
                        else if (count == 0)
                        {
                            dbr.ds = ds;
                            dbr.Message = "No Records Found for this request!!";
                            dbr.Status = true;


                        }

                   // }




                }


            }
            catch (MySqlException e)
            {

                dbr.Status = false;
                dbr.Message = "Something wrong with database : " + e.Message;
                throw e;

            }
            catch (Exception e)
            {
                dbr.Message = e.Message;
                dbr.Status = false;
                throw e;
            }
            finally
            {
                cmd.Dispose();


            }
            return dbr;


        }
        public DataBaseResult GetLoginUserInfoByToken(string userToken)
        {
            string getUserInfo = "";
            DataBaseResult dbr = new DataBaseResult();
            MySqlCommand cmd = new MySqlCommand();
            try
            {
                dbr.CommandType = "Select";
                getUserInfo = DBConnection.GetLoginSessionInfoByToken(userToken);

                if (string.IsNullOrEmpty(getUserInfo) || string.IsNullOrEmpty(connectionString))
                {
                    dbr.Id = 0;
                    dbr.Message = "Something Wrong with getting DB Commands!!";
                    dbr.EmployeeName = "";
                    dbr.Status = false;
                    dbr.dt = new DataTable();
                    dbr.ds = new DataSet();
                }
                else
                {
                    //using (MySqlConnection conn = new MySqlConnection(connectionString))
                    //{
                        DataSet ds = new DataSet();
                        dbr.ds = new DataSet();
                        // sda = new MySqlDataAdapter(getUserInfo, conn);
                        //sda.SelectCommand.CommandType = CommandType.Text;
                        //sda.Fill(ds);
                        //cmd = new MySqlCommand(getUserInfo, conn);
                        //DataTable temp = new DataTable();
                        //MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                        //adapter.Fill(temp);

                        //ds.Tables.Add(temp);
                        ds = new BasicDBOps().GetMultipleRecords(connectionString, getUserInfo);
                        int count = 0;
                        count = ds.Tables[0].Rows.Count;
                        if (ds.Tables.Count > 0 && count > 0)
                        {
                            //foreach (DataRow dr in dt.Rows)
                            //{
                            //    Console.WriteLine(string.Format("user_id = {0}", dr["user_id"].ToString()));
                            //}
                            dbr.ds = ds;
                            dbr.Message = "Record(s) retreived Successfully!!!";
                            dbr.Status = true;

                        }
                        else if (count == 0)
                        {
                            dbr.ds = ds;
                            dbr.Message = "No Records Found for this request!!";
                            dbr.Status = true;


                        }

                  //  }




                }


            }
            catch (MySqlException e)
            {

                dbr.Status = false;
                dbr.Message = "Something wrong with database : " + e.Message;
                throw e;

            }
            catch (Exception e)
            {
                dbr.Message = e.Message;
                dbr.Status = false;
                throw e;
            }
            finally
            {
                cmd.Dispose();


            }
            return dbr;


        }

        #region BLL
        public APIResult GetLoginUserSessionInfoByToken(string userToken)
        {
            APIResult result = new APIResult();
            DataBaseResult dbr = new DataBaseResult();
            try
            {

                
                dbr = new MySQLDBOperations().GetLoginUserInfoByToken(userToken);
                UserType user = new UserType();
                int count = 0;
                count = dbr.ds.Tables[0].Rows.Count;
                if (count > 0)
                {
                    result.userInfo = new UserType();
                    for (int i = 0; i < count; i++)
                    {
                        int userTypeid = 0;
                        int employeeid = 0;
                        string empId = dbr.ds.Tables[0].Rows[i]["EmployeeId"].ToString();
                        bool succ = int.TryParse(empId, out employeeid);
                        string usertype = dbr.ds.Tables[0].Rows[i]["UserTypeId"].ToString();
                        bool success = int.TryParse(usertype, out userTypeid);
                        userTypeid = (success == true) ? userTypeid : 0;
                        employeeid = (succ == true) ? employeeid : 0;
                        user.EmployeeId = employeeid;
                        user.UserTypeId = userTypeid;
                        user.Token = dbr.ds.Tables[0].Rows[i]["Token"].ToString();
                        user.User = dbr.ds.Tables[0].Rows[i]["UserName"].ToString();
                        user.Valid = true;

                    }
                    result.userInfo = user;
                    result.Message = "Session Details retrieved Successfully!!!";
                    result.Status = true;
                }
                else {
              
                    result.Message = "Invalid token, try Login again!!!";
                    result.Status = false;

                }
                //result.userInfo.Valid = false;



            }
            catch (Exception e)
            {
               // result.userInfo.Valid = false;
                result.Status = false;
                result.Message = e.Message;
                throw e;

            }

            return result;

        }
        public APIResult DeleteSession(UserType info, string userToken = "", bool isToken = false)
        {
            APIResult result = new APIResult();
            // MySqlCommand cmd = new MySqlCommand();
            string cmd = "";
            try
            {
                //using (MySqlConnection conn = new MySqlConnection(connectionString))
                //{
                   // conn.Open();
                    if (!string.IsNullOrEmpty(userToken) && isToken)
                        cmd = string.Format("Delete From UserSessions WHERE Token = '{0}';", userToken);
                    else
                        cmd = string.Format("Delete From UserSessions  where UserName = '{0}' AND UserTypeId = {1} AND IsActive=1" +
                    " AND EmployeeId = {2};", info.User, info.UserTypeId, info.EmployeeId);
                //cmd.CommandType = CommandType.Text;
                //cmd.Connection = conn;
                int res = new BasicDBOps().ExceuteCommand(connectionString, cmd);
                   // int res = cmd.ExecuteNonQuery();
                    if (res > 0 && isToken)
                    {

                        result.Message = "User Session Terminated along with token, Please try login again!!!!!";
                        result.Status = false;

                    }
                    else if (res > 0 && !isToken)
                    {
                        result.Message = "Current Session terminated for this user, Please try login again!!!!!";
                        result.Status = false;

                    }
                    else if (isToken)
                    {
                        //using (MySqlConnection conn2 = new MySqlConnection(connectionString))
                        //{
                           // conn2.Open();
                            //cmd = new MySqlCommand();
                            cmd = string.Format("select COUNT(*) From UserSessions WHERE Token = '{0}';", userToken);
                    //cmd.CommandType = CommandType.Text;
                    //cmd.Connection = conn2;
                    // MySqlDataReader reader = cmd.ExecuteReader();
                    //int res1 = 0;
                    //while(reader.Read())
                    //    res1 = reader.GetInt32(0);
                    bool isExists = new BasicDBOps().CheckRecordCountExistsOrNot(connectionString, cmd);
                            if (isExists)
                            {
                                result.Message = "Unable to terminate Session with invalid token or Session already terminated, Contact Support Team or try login again!!";
                                result.Status = false;
                            }
                            else {
                                result.Message = "No token avalilable for this session, try login again!!";
                                result.Status = false;
                            }
                            //conn2.Close();
                       // }
                    }
                    else if (!isToken)
                    {
                        //using (MySqlConnection conn3 = new MySqlConnection(connectionString))
                        //{
                               // conn3.Open();
                               // cmd = new MySqlCommand();
                            cmd = string.Format("select COUNT(*) From UserSessions where UserName = '{0}' AND UserTypeId = {1} AND IsActive=1" +
                        " AND EmployeeId = {2};", info.User, info.UserTypeId, info.EmployeeId);
                            //cmd.CommandType = CommandType.Text;
                            //cmd.Connection = conn3;
                            //MySqlDataReader reader = cmd.ExecuteReader();
                           // int res2 = 0;
                            //while (reader.Read())
                            //    res2 = reader.GetInt32(0);
                            bool isExists = new BasicDBOps().CheckRecordCountExistsOrNot(connectionString, cmd);
                            if (isExists)
                            {
                                result.Message = "Unable to terminate Session for this user , Contact Support Team!!";
                                result.Status = false;
                            }
                            else
                            {
                                result.Message = "No session avalilable for this user, try login again!!";
                                result.Status = false;
                            }
                          //  conn3.Close();
                      //  }
                    }
                  //  conn.Close();
              //  }


            }
            catch (MySqlException e)
            {

                result.Status = false;
                result.Message = "Something wrong with database : " + e.Message;
                throw e;

            }
            catch (Exception e)
            {
               // result.userInfo.Valid = false;
                result.Status = false;
                result.Message = e.Message;
                throw e;

            }
            //finally { cmd.Dispose(); }

            return result;

        }
        public APIResult ValidateLoginUserSession(UserType usr)
        {
            APIResult result = new APIResult();
            DataBaseResult dbr = new DataBaseResult();
            try
            {
                result.userInfo = new UserType();
                dbr =  new MySQLDBOperations().GetLoginSessionDetails(usr);
                UserType user = new UserType();
                int count = 0;
                count = dbr.ds.Tables[0].Rows.Count;
                if (count > 0)
                {
                    for (int i = 0; i < count; i++)
                    {
                        int userTypeid = 0;
                        int employeeid = 0;
                        string empId = dbr.ds.Tables[0].Rows[i]["EmployeeId"].ToString();
                        bool succ = int.TryParse(empId, out employeeid);
                        string usertype = dbr.ds.Tables[0].Rows[i]["UserTypeId"].ToString();
                        bool success = int.TryParse(usertype, out userTypeid);
                        userTypeid = (success == true) ? userTypeid : 0;
                        employeeid = (succ == true) ? employeeid : 0;
                        user.EmployeeId = employeeid;
                        user.UserTypeId = userTypeid;
                        user.Token = dbr.ds.Tables[0].Rows[i]["Token"].ToString();
                        user.User = dbr.ds.Tables[0].Rows[i]["UserName"].ToString();
                        user.Valid = true;

                    }
                    result.userInfo = user;
                    result.Message = "Session Details retrieved Successfully!!!";
                    result.Status = true;
                }
                else 
                {
                    //using (MySqlConnection conn = new MySqlConnection(connectionString))
                    //{
                        string txt = string.Format("SELECT COUNT(*) from UserSessions WHERE UserName='{0}'" +
                            " AND UserTypeId={1} AND EmployeeId = {2} AND IsActive=1;",usr.User,usr.UserTypeId,usr.EmployeeId);
                        //MySqlCommand cmd = new MySqlCommand();
                        //cmd.CommandText = txt;
                        //cmd.CommandType = CommandType.Text;

                        //conn.Open();
                        //cmd.Connection = conn;
                        //MySqlDataReader reader = cmd.ExecuteReader();
                        //int res =0;
                        //while (reader.Read())
                        //    res = reader.GetInt32(0);
                        bool isExists = new BasicDBOps().CheckRecordCountExistsOrNot(connectionString, txt);
                        if (isExists)
                        {
                            //using (MySqlConnection conn2 = new MySqlConnection(connectionString))
                            //{
                                //MySqlCommand cmd2 = new MySqlCommand();
                                //conn2.Open();
                               string cmdText = string.Format("DELETE from UserSessions WHERE UserName='{0}'" +
                            " AND UserTypeId={1} AND EmployeeId = {2} AND IsActive=1;", usr.User, usr.UserTypeId, usr.EmployeeId);
                            //cmd2.CommandType = CommandType.Text;
                            //cmd2.Connection = conn2;
                            //int i = cmd2.ExecuteNonQuery();
                            int i = new BasicDBOps().ExceuteCommand(connectionString,cmdText);
                                if (i > 0)
                                {
                                    result.Message = "Existing Session Expired,  Try login again!!!";
                                    result.Status = false;
                                    //result.userInfo.Valid = false;
                                }
                                else {
                                    result.Message = "Unable to terminate Session for this user , Contact Support Team!!";
                                    result.Status = false;
                                    //result.userInfo.Valid = false;
                                }
                             //   cmd2.Dispose();
                               // conn2.Close();
                           // }
                                

                        }
                        //cmd.Dispose();
                        //conn.Close();
                  //  }


                }
               


            }
            catch (MySqlException e)
            {

                result.Status = false;
                result.Message = "Something wrong with database : " + e.Message;
                throw e;

            }
            catch (Exception e)
            {
                //result.userInfo.Valid = false;
                result.Status = false;
                result.Message = e.Message;
                throw e;

            }

            return result;
        }

        #endregion

        public DataBaseResult GetRegisteredUsers(string stationCode = "")
        {
            string getRegisteredUsers = "";
            DataBaseResult dbr = new DataBaseResult();
            MySqlCommand cmd = new MySqlCommand();
            MySqlDataAdapter sda;
            try
            {
                dbr.CommandType = "Select";
                getRegisteredUsers = DBConnection.GetRegisteredUsers(stationCode);

                if (string.IsNullOrEmpty(getRegisteredUsers) || string.IsNullOrEmpty(connectionString))
                {
                    dbr.Id = 0;
                    dbr.Message = "Something Wrong with getting DB Commands!!";
                    dbr.EmployeeName = "";
                    dbr.Status = false;
                    dbr.dt = new DataTable();
                    dbr.ds = new DataSet();
                }
                else
                {
                    //using (MySqlConnection conn = new MySqlConnection(connectionString))
                    //{
                        DataSet ds = new DataSet();
                        dbr.ds = new DataSet();
                    //DataTable dt = new DataTable();
                    //sda = new MySqlDataAdapter(getRegisteredUsers, conn);
                    //sda.SelectCommand.CommandType = CommandType.Text;
                    //sda.Fill(ds);
                    ds = new BasicDBOps().GetMultipleRecords(connectionString, getRegisteredUsers);
                        int count = 0;
                        count = ds.Tables[0].Rows.Count;
                        if (ds.Tables.Count > 0 && count > 0)
                        {
                            //foreach (DataRow dr in dt.Rows)
                            //{
                            //    Console.WriteLine(string.Format("user_id = {0}", dr["user_id"].ToString()));
                            //}
                            dbr.ds = ds;
                            dbr.Message = "Records retreived Successfully!!!";
                            dbr.Status = true;

                        }
                        else if (count == 0)
                        {
                            dbr.ds = ds;
                            dbr.Message = "No Records Found for this request!!";
                            dbr.Status = true;


                        }

                  //  }




                }


            }
            catch (MySqlException e)
            {

                dbr.Status = false;
                dbr.Message = "Something wrong with database : " + e.Message;
                throw e;

            }
            catch (Exception e)
            {
                dbr.Message = e.Message;
                dbr.Status = false;
                throw e;
            }
            finally
            {
                cmd.Dispose();


            }
            return dbr;


        }

        public DataBaseResult ApproveUser(int registerId)
        {
            string getApproveUser = "";
            DataBaseResult dbr = new DataBaseResult();
           // MySqlCommand cmd = new MySqlCommand();
            try
            {
                dbr.Id = registerId;
                dbr.CommandType = "UPDATE";
                getApproveUser = DBConnection.ApproveUser(registerId);

                if (string.IsNullOrEmpty(getApproveUser) || string.IsNullOrEmpty(connectionString))
                {
                    dbr.Id = 0;
                    dbr.Message = "Something Wrong with getting DB Commands!!";
                    dbr.EmployeeName = "";
                    dbr.Status = false;
                    dbr.dt = new DataTable();
                    dbr.ds = new DataSet();
                }
                else
                {
                    //using (MySqlConnection conn = new MySqlConnection(connectionString))
                    //{
                    //cmd.CommandText = getApproveUser;
                    //cmd.CommandType = CommandType.Text;
                    //int res = cmd.ExecuteNonQuery();
                    int res = new BasicDBOps().ExceuteCommand(connectionString, getApproveUser);
                        if (res > 0)
                        {
                            
                            dbr.Message = "User Approved Successfully!!!";
                            dbr.Status = true;

                        }
                        else
                        {
                            
                            dbr.Message = "User not approved for this request!!";
                            dbr.Status = false;


                        }

                   // }




                }


            }
            catch (MySqlException e)
            {

                dbr.Status = false;
                dbr.Message = "Something wrong with database : " + e.Message;
                throw e;

            }
            catch (Exception e)
            {
                dbr.Message = e.Message;
                dbr.Status = false;
                throw e;
            }
            finally
            {
                
              //  cmd.Dispose();


            }
            return dbr;


        }

        public DataBaseResult GetEmployees(string stationCode = "", bool isEmployee = false)
        {
            string getEmployees = "";
            DataBaseResult dbr = new DataBaseResult();
            //MySqlCommand cmd = new MySqlCommand();
            //MySqlDataAdapter sda;
            try
            {
                dbr.CommandType = "Select";
                getEmployees = DBConnection.GetEmployees(stationCode);

                if (string.IsNullOrEmpty(getEmployees) || string.IsNullOrEmpty(connectionString))
                {
                    dbr.Id = 0;
                    dbr.Message = "Something Wrong with getting DB Commands!!";
                    dbr.EmployeeName = "";
                    dbr.Status = false;
                    dbr.dt = new DataTable();
                    dbr.ds = new DataSet();
                }
                else
                {
                    //using (MySqlConnection conn = new MySqlConnection(connectionString))
                    //{
                        DataSet ds = new DataSet();
                        dbr.ds = new DataSet();
                    //DataTable dt = new DataTable();
                    //sda = new MySqlDataAdapter(getEmployees, conn);
                    //sda.SelectCommand.CommandType = CommandType.Text;
                    //sda.Fill(ds);
                    ds = new BasicDBOps().GetMultipleRecords(connectionString, getEmployees);
                        int count = 0;
                        count = ds.Tables[0].Rows.Count;
                        if (ds.Tables.Count > 0 && count > 0)
                        {
                            //foreach (DataRow dr in dt.Rows)
                            //{
                            //    Console.WriteLine(string.Format("user_id = {0}", dr["user_id"].ToString()));
                            //}
                            dbr.ds = ds;
                            dbr.Message = "Records retreived Successfully!!!";
                            dbr.Status = true;

                        }
                        else if (count == 0)
                        {
                            dbr.ds = ds;
                            dbr.Message = "No Records Found for this request!!";
                            dbr.Status = true;


                        }

                  //  }




                }

            }
            catch (MySqlException e)
            {

                dbr.Status = false;
                dbr.Message = "Something wrong with database : " + e.Message;
                throw e;

            }
            catch (Exception e)
            {
                dbr.Message = e.Message;
                dbr.Status = false;
                throw e;
            }
            finally
            {
               // cmd.Dispose();


            }
            return dbr;


         }

     }


}
