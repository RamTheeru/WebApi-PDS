using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using pdstest.Models;
using MySql.Data.MySqlClient;
using System.Data;
using pdstest.services;
using Renci.SshNet.Security.Cryptography;
using System.Text;

namespace pdstest.DAL
{
    public class MySQLDBOperations 
    {

        public static string connectionString = DBConnection.GetDBConnection(false);
        public static string connectionString2 = DBConnection.GetDBConnection(true);

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
                    string cmdtxt = "";
                    cmdtxt = DBConnection.CheckUserforRegistration(input);
                    bool isExists = new BasicDBOps().CheckRecordCountExistsOrNot(connectionString, cmdtxt);
                    if(isExists)
                    {
                        dbr.Id = 0;
                        dbr.EmployeeName = "";
                        dbr.Status = false;
                        dbr.Message = "Already register with this details for this station.";
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

                            param = new MySqlParameter("@Email", input.Email);
                            param.Direction = ParameterDirection.Input;
                            param.MySqlDbType = MySqlDbType.VarChar;
                            param.Size = 50;
                            cmd.Parameters.Add(param);

                            param = new MySqlParameter("@LastName", input.LastName);
                            param.Direction = ParameterDirection.Input;
                            param.MySqlDbType = MySqlDbType.VarChar;
                            param.Size = 50;
                            cmd.Parameters.Add(param);

                            param = new MySqlParameter("@Usr", input.UserName);
                            param.Direction = ParameterDirection.Input;
                            param.MySqlDbType = MySqlDbType.VarChar;
                            param.Size = 50;
                            cmd.Parameters.Add(param);

                            param = new MySqlParameter("@Passwrd", input.Password);
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

        public DataBaseResult CreateEmployee(Employee input, bool isemployee = false)
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

                        //param = new MySqlParameter("@PID", input.Pid);
                        //param.Direction = ParameterDirection.Input;
                        //param.MySqlDbType = MySqlDbType.Int32;
                        //cmd.Parameters.Add(param);

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

                        param = new MySqlParameter("@EmpCode", input.EmpCode);
                        param.Direction = ParameterDirection.Input;
                        param.MySqlDbType = MySqlDbType.VarChar;
                        param.Size = 50;
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
                        param = new MySqlParameter("@Gaurd_fullname", fullname);
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

                        param = new MySqlParameter("@IsActive", 1);
                        param.Direction = ParameterDirection.Input;
                        param.MySqlDbType = MySqlDbType.Bit;
                        cmd.Parameters.Add(param);

                        param = new MySqlParameter("@PID", input.Pid);
                        param.Direction = ParameterDirection.Input;
                        param.MySqlDbType = MySqlDbType.Int32;
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

        public DataBaseResult CreateCDAEmployee(Employee input)
        {
            string insertQuery = "";
            DataBaseResult dbr = new DataBaseResult();
            MySqlCommand cmd = new MySqlCommand();
            MySqlParameter param;

            try
            {
                dbr.CommandType = "Insert";
                insertQuery = DBConnection.GetCDAInsertQuery();

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

                        param = new MySqlParameter("@CDACode", input.EmpCode);
                        param.Direction = ParameterDirection.Input;
                        param.MySqlDbType = MySqlDbType.VarChar;
                        param.Size = 50;
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

                        param = new MySqlParameter("@IsActive", 1);
                        param.Direction = ParameterDirection.Input;
                        param.MySqlDbType = MySqlDbType.Bit;
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
                            dbr.Message = "CDA Employee Added Successfully!!!";
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




        #region Voucher related
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
                        string cmdT = string.Format("SELECT * FROM Voucher where VoucherNumber = '{0}'", input.VoucherNumber);
                        bool isExists = false;
                        isExists = new BasicDBOps().CheckRecordCountExistsOrNot(connectionString, cmdT);
                        string otherVoucher = DBConnection.CheckVoucherExists(input.VoucherNumber, false, input.StationId, input.V_Date);
                        bool otherExists = false;
                        otherExists = new BasicDBOps().CheckRecordCountExistsOrNot(connectionString, otherVoucher);
                        string creditValid = DBConnection.CheckVoucherExists(input.VoucherNumber, true, input.StationId, input.V_Date);
                        bool validateCredit = true;
                        validateCredit = new BasicDBOps().CheckRecordCountExistsOrNot(connectionString, creditValid);
                        bool validatebalance = false;
                        int creditamont = 0;
                        string creditcmd = DBConnection.GetCreditamountinCurrentMonthForVoucher(input.StationId, input.V_Date);
                        creditamont = this.GetBalanceAmountForVoucherCreation("", creditcmd, "CreditAmount");
                        int debitamt = 0;
                        string debitcmd = DBConnection.GetTotalDebitamountinCurrentMonthForVoucher(input.StationId, input.V_Date);
                        debitamt = this.GetBalanceAmountForVoucherCreation(debitcmd, "", "DebitAmount");
                        if (creditamont > debitamt && creditamont > 0)
                        {
                            int bal = creditamont - debitamt;
                            if (bal > 0)
                            {
                                validatebalance = true;
                            }
                            else
                            {
                                validatebalance = false;
                            }

                        }
                        else
                        {
                            validatebalance = false;
                        }
                        if (isExists)
                        {
                            dbr.Id = 0;
                            dbr.VoucherNumber = input.VoucherNumber;
                            dbr.Status = false;
                            dbr.Message = "Voucher already exists..Please Create another Voucher!! ";

                        }
                        else if (otherExists)
                        {
                            dbr.Id = 0;
                            dbr.VoucherNumber = input.VoucherNumber;
                            dbr.Status = false;
                            dbr.Message = "Voucher already exists for this month and waiting for approval!! ";
                        }
                        else if (!validateCredit)
                        {
                            dbr.Id = 0;
                            dbr.VoucherNumber = input.VoucherNumber;
                            dbr.Status = false;
                            dbr.Message = "Cannot create voucher!!! Amount is not credited yet for this station in this month.";
                        }
                        else if (!validatebalance)
                        {
                            dbr.Id = 0;
                            dbr.VoucherNumber = input.VoucherNumber;
                            dbr.Status = false;
                            dbr.Message = "Cannot create voucher!!! There is no avaliable balance from credited amount for this station in this month.";
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

                            param = new MySqlParameter("@VoucherDate", input.V_Date);
                            param.Direction = ParameterDirection.Input;
                            param.MySqlDbType = MySqlDbType.VarChar;
                            param.Size = 30;
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
        public DataBaseResult UpdateVoucherDetails(List<Voucher> vIds,string status)
        {
            DataBaseResult dbr = new DataBaseResult();
            MySqlConnection con = new MySqlConnection(connectionString);
            int i = 0;
            con.Open();
            MySqlTransaction transaction = con.BeginTransaction();
            try
            {
                dbr.CommandType = "Update";
                if (vIds.Count > 0)
                {
                    var valid = vIds.Any(x => x.VoucherNumber == string.Empty || x.VoucherNumber == null || x.StationId == 0);
                    if (!valid)
                    {
                        List<Voucher> vouchs = new List<Voucher>();
                        vouchs = vIds.Where(x => x.VoucherStatus.ToLower() == "p").ToList();
                        //bool isExists = false;
                        if (vouchs.Count > 0)
                        {
                            foreach (var item in vouchs)
                            {
                                string approvText = "";
                                approvText = DBConnection.ApproveVoucher(item.VoucherId, status);
                                MySqlCommand command = new MySqlCommand(approvText, con, transaction);
                                //command.Connection = con;
                                //command.Transaction = transaction;
                                transaction = command.Transaction;
                                i = command.ExecuteNonQuery();
                                if (i == 0)
                                    throw new Exception("Something went wrong!!,Unable to update Voucher Status");
                                command.Dispose();
                                //i = new BasicDBOps().ExceuteCommand(connectionString, cmdText);
                            }
                            transaction.Commit();
                        }
                        else
                        {
                            i = 0;
                            dbr.Status = false;
                            dbr.Message = "Invalid Data to update with provided status, Please try again";
                        }
                    }
                    else
                    {
                        i = 0;
                        dbr.Status = false;
                        dbr.Message = "Invalid Data to update, Please try again";
                    }
                }

            }
            catch (Exception e)
            {
                try
                {
                    i = 0;
                    transaction.Rollback();
                    dbr.Status = false;
                    dbr.Message = "Cannot Complete the operation Please try again. Reason : " + e.Message;
                }
                catch (Exception ex2)
                {
                    i = 0;
                    dbr.Status = false;
                    dbr.Message = "Rollback Operation failed while updating details. Reason : " + ex2.Message + ". Please contact support team";
                    // throw ex2;
                    // This catch block will handle any errors that may have occurred 
                    // on the server that would cause the rollback to fail, such as 
                    // a closed connection.

                }
                throw e;
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
                if (i > 0)
                {
                    dbr.Status = true;
                    dbr.Message = "Updated voucher status for all Successfully!!!";
                }
            }
            return dbr;
        }
        public DataBaseResult ApproveVoucher(int voucherId, string status)
        {
            string getupdateInfo = "";
            DataBaseResult dbr = new DataBaseResult();
            MySqlCommand cmd = new MySqlCommand();
            try
            {
                dbr.CommandType = "UPDATE";
                getupdateInfo = DBConnection.ApproveVoucher(voucherId,status);

                if (string.IsNullOrEmpty(getupdateInfo) || string.IsNullOrEmpty(getupdateInfo))
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
                    int count = 0;
                    count = new BasicDBOps().ExceuteCommand(connectionString, getupdateInfo);
                    if (count > 0)
                    {

                        dbr.Status = true;
                        dbr.Message = status.ToLower() == "r"? "Voucher Rejected Successfully!!!!" : "Voucher Approved Successfully!!!!";
                    }
                    else
                    {
                        dbr.Status = false;
                        dbr.Message = "Something went wrong!!";


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
        public DataBaseResult UpdateVoucher(Voucher input)
        {
            DataBaseResult dbr = new DataBaseResult();
            try
            {
                dbr.CommandType = "UPDATE";
                string cmdtext = DBConnection.GetVoucherUpdateQuery(input);
                DataSet ds = new DataSet();
                dbr.ds = new DataSet();;
                if (!string.IsNullOrEmpty(cmdtext))
                {
                    string otherVoucher = DBConnection.CheckVoucherExists(input.VoucherNumber, false, input.StationId, input.V_Date);
                    bool otherExists = false;
                    otherExists = new BasicDBOps().CheckRecordCountExistsOrNot(connectionString, otherVoucher);
                    if (!otherExists)
                    {
                        dbr.Status = false;
                        dbr.Message = "You have to edit voucher details for current month only, You change any date within current month";
                    }
                    else
                    {
                        int count = 0;
                        count = new BasicDBOps().ExceuteCommand(connectionString, cmdtext);
                        if (count > 0)
                        {

                            dbr.Status = true;
                            dbr.Message = "Voucher Updated Successfully and went for approval!!!!";
                        }
                        else
                        {
                            dbr.Status = false;
                            dbr.Message = "Something went wrong!!";


                        }
                    }

                }
                else
                {
                    dbr.Status = false;
                    dbr.Message = "Something went wrong, No Command to Insert!!";
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

            return dbr;
        }
        public DataBaseResult GetVoucherDetailsbyVoucherNumber(int voucherId)
        {
            DataBaseResult dbr = new DataBaseResult();
            string cmdText = "";
            try
            {
                dbr.CommandType = "Select";
                cmdText = DBConnection.GetVoucherDetailsbyVoucherNumberQuery(voucherId);
                dbr.ds = new DataSet();
                DataSet ds = new DataSet();
                if (string.IsNullOrEmpty(cmdText) || string.IsNullOrEmpty(connectionString))
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
                    ds = new BasicDBOps().GetMultipleRecords(connectionString, cmdText);
                    int count = 0;
                    count = ds.Tables[0].Rows.Count;
                    if (ds.Tables.Count > 0 && count > 0)
                    {
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
                    else
                    {
                        dbr.ds = new DataSet();
                        dbr.Message = "Something went wrong!!!!";
                        dbr.Status = false;
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



            }
            return dbr;

        }
        public int GetBalanceAmountForVoucherCreation(string  debitCmd,string creditCmd,string col)
        {
            int balance = 0;
            try
            {
                if(!string.IsNullOrEmpty(debitCmd))
                {
                    balance = this.GetValuefromCommand(debitCmd, col);
                }
                if(!string.IsNullOrEmpty(creditCmd))
                {
                    balance = this.GetValuefromCommand(creditCmd, col);
                }

            }
            catch(Exception e)
            {
                balance = 0;
                throw e;
            }
            return balance;
        }
        public int GetValuefromCommand(string cmmd,string col)
        {
            int val = 0;
            try
            {
                DataSet ds = new DataSet();
                ds = new BasicDBOps().GetMultipleRecords(connectionString, cmmd);
                if (ds != null && ds.Tables.Count > 0)
                {
                    if(ds.Tables[0].Rows.Count > 0)
                    {
                        string v = ds.Tables[0].Rows[0][col].ToString();
                        val = this.HandleStringtoInt(v);
                    }
                    else
                    {
                        val = 0;
                    }
                }
                else
                {
                    val = 0;
                }
            }
            catch (Exception e)
            {
                val = 0;
                throw e;
            }
            return val;
        }
        public int HandleStringtoInt(string str)
        {
            int result = 0;
            if (!string.IsNullOrEmpty(str))
            {
                bool success = int.TryParse(str, out result);
                result = (success == true) ? result : 0;
            }
            return result;
        }
        #endregion

        public DataBaseResult GetPreviousCreditandDebitDetails(int stationId)
        {
            string query = "";
            DataBaseResult dbr = new DataBaseResult();
            MySqlCommand cmd = new MySqlCommand();
            try
            {
                int creditPrevAmnt = 0;
                int debitPrevAmnt = 0;
                dbr.CommandType = "SELECT";
                dbr.ds = new DataSet();
                query = DBConnection.GetTotalCreditamountinPreviousMonthForVoucher(stationId);
                if (string.IsNullOrEmpty(query) || string.IsNullOrEmpty(connectionString))
                {
                    dbr.Id = 0;
                    dbr.Message = "Something Wrong with getting DB Commands!!";
                    dbr.EmployeeName = "";
                    dbr.Status = false;

                }
                else
                {
                    DateTime dn = DateTime.Now.GetIndianDateTimeNow();
                    string currDate = dn.DateTimetoString();
                    //string currentCreditAmount = string.Format("select Credit as CurrentCreditAmount from financeledger Where StationId = {0} and Credit IS NOT NULL " +
                    //  " and ((MONTH(CreditDate) = MONTH('{1}') AND YEAR(CreditDate) = YEAR('{1}'))); ", stationId, currDate);
                    string cmdT = DBConnection.CheckforCreditintoStation(stationId, currDate);
                    bool isExists = false;
                    isExists = new BasicDBOps().CheckRecordCountExistsOrNot(connectionString, cmdT);
                    int curentcreditamont = 0;
                    int currentdebitamt = 0;
                    int currentBalance = 0;
                    if (isExists)
                    {
                        string creditcmd = DBConnection.GetCreditamountinCurrentMonthForVoucher(stationId, currDate);
                        curentcreditamont = this.GetBalanceAmountForVoucherCreation("", creditcmd, "CreditAmount");
                        string debitcmd = DBConnection.GetTotalDebitamountinCurrentMonthForVoucher(stationId, currDate);
                        currentdebitamt = this.GetBalanceAmountForVoucherCreation(debitcmd, "", "DebitAmount");
                    }
                    if (curentcreditamont > currentdebitamt && curentcreditamont > 0)
                    {
                        currentBalance = curentcreditamont - currentdebitamt;
                    }
                    // DataSet ds = new DataSet();
                    // int c_amount = 0;
                    //ds = new BasicDBOps().GetMultipleRecords(connectionString, currentCreditAmount);
                    //if(ds.Tables.Count > 0)
                    //{
                    //    if(ds.Tables[0].Rows.Count>0)
                    //    {
                    //        string cAmmnt = ds.Tables[0].Rows[0]["CurrentCreditAmount"].ToString();
                    //        c_amount = this.HandleStringtoInt(cAmmnt);
                    //    }
                    //}
                    creditPrevAmnt = this.GetBalanceAmountForVoucherCreation("", query, "CreditAmount");
                    query = DBConnection.GetTotalDebitamountinPreviousMonthForVoucher(stationId);
                    debitPrevAmnt = this.GetBalanceAmountForVoucherCreation(query, "", "DebitAmount");
                    dbr.dt = new DataTable();
                    dbr.dt.Clear();
                    dbr.dt.Columns.Add("CreditAmount");
                    dbr.dt.Columns.Add("DebitAmount");
                    dbr.dt.Columns.Add("CurrentCreditAmount");
                    dbr.dt.Columns.Add("CurrentDebitAmount");
                    dbr.dt.Columns.Add("CurrentBalanceAmount");
                    DataRow dr = dbr.dt.NewRow();
                    dr["CreditAmount"] = creditPrevAmnt;
                    dr["DebitAmount"] = debitPrevAmnt;
                    dr["CurrentCreditAmount"] = curentcreditamont;
                    dr["CurrentDebitAmount"] = currentdebitamt;
                    dr["CurrentBalanceAmount"] = currentBalance;
                    dbr.dt.Rows.Add(dr);
                    dbr.ds.Tables.Add(dbr.dt);
                    dbr.Status = true;
                    dbr.Message = "Details retreived";

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
                        string cmdT = DBConnection.CheckforCreditintoStation(input.StationId, input.Cred_Date);
                        bool isExists = false;
                        isExists = new BasicDBOps().CheckRecordCountExistsOrNot(connectionString, cmdT);
                        bool validatebalance = false;
                        int creditamont = 0;
                        int debitamt = 0;
                        if (isExists)
                        {
                            string creditcmd = DBConnection.GetCreditamountinCurrentMonthForVoucher(input.StationId, input.Cred_Date);
                            creditamont = this.GetBalanceAmountForVoucherCreation("", creditcmd, "CreditAmount");
                            string debitcmd = DBConnection.GetTotalDebitamountinCurrentMonthForVoucher(input.StationId, input.Cred_Date);
                            debitamt = this.GetBalanceAmountForVoucherCreation(debitcmd, "", "DebitAmount");
                        }
                        if (creditamont > debitamt && creditamont > 0)
                        {
                            int bal = creditamont - debitamt;
                            if (bal > 0)
                            {
                                validatebalance = true;
                            }
                            else
                            {
                                validatebalance = false;
                            }

                        }
                        if (isExists)
                        {
                            //dbr.Status = false;
                            //dbr.Message = "Amount already credited for this station in this month";
                            string cmTxt = string.Format("update FinanceLedger SET Credit= Credit + {0},CreditDate = '{1}' WHERE StationId = {2} AND MONTH(CreditDate) = MONTH('{1}') AND YEAR(CreditDate) = YEAR('{1}') AND Credit IS NOT NULL  AND IsActive = 1;", input.Credit, input.Cred_Date, input.StationId);
                            int ch = new BasicDBOps().ExceuteCommand(connectionString, cmTxt);
                            if (ch > 0)
                            {
                                dbr.Status = true;
                                dbr.Message = "Credit amount updated for this station successfully";
                                string c = string.Format("UPDATE FinanceLedger SET Balance = Balance + {0} " +
                                   " WHERE StationId = {1} AND MONTH(VoucherDate) = MONTH('{2}') AND " +
                                   " YEAR(VoucherDate) = YEAR('{2}') AND IsActive = 1 AND VoucherStatus = 'A' AND Debit IS NOT NULL;", input.Credit, input.StationId, input.Cred_Date);
                                ch = new BasicDBOps().ExceuteCommand(connectionString, c);
                                if(ch > 0)
                                {
                                    dbr.Status = true;
                                    dbr.Message = dbr.Message + "and also Ledger table debits updated accordingly";
                                }
                                else
                                {
                                    dbr.Status = false;
                                    dbr.Message = dbr.Message + "but  Ledger table debits are unable to update accordingly, please contact support team.";
                                }
                            }
                            else
                            {
                                dbr.Status = false;
                                dbr.Message = "Something wrong, Credit amount  is not updated for this station";
                            }
                            //    dbr.Id = 0;
                            ////    dbr.VoucherNumber = input.VoucherNumber;
                            //    dbr.Status = false;
                            //    dbr.Message = "Amount already credited to this station for this month..Please Check once and edit the existed amount further!! ";

                        }
                        else
                        {
                            cmd.CommandText = insertQuery;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Connection = conn;

                            param = new MySqlParameter("@CreditDate", input.Cred_Date);
                            param.Direction = ParameterDirection.Input;
                            param.MySqlDbType = MySqlDbType.VarChar;
                            param.Size = 30;
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
            bool isSessionExists = false;
            try
            {
                dbr.CommandType = "Insert";
                dbr.ds = new DataSet();
                isSessionExists = CheckIfSessionExists(input.User, input.EmployeeId, input.UserTypeId);
                if(!isSessionExists)
                {

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
                            DateTime dtt = DateTime.Now;
                            dtt = dtt.GetIndianDateTimeNow();
                            input.SessionStartDate = dtt.DateTimetoString();
                            input.SessionEndDate = dtt.AddMinutes(20).DateTimetoString();

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

                            MySqlParameter output7 = new MySqlParameter();
                            output7.ParameterName = "@StationId";
                            output7.MySqlDbType = MySqlDbType.Int32;
                            output7.Direction = ParameterDirection.Output;
                            cmd.Parameters.Add(output7);
                          ///  string t = string.Format("INSERT INTO UserSessions (UserName,UserTypeId,Token,EmployeeId,StartDate,EndDate,IsActive) VALUES('{0}',{1},'{2}',{3},'2020-02-02','2020=02-02',1)",input.User,input.UserTypeId,input.Token,input.EmployeeId);
                            //MySqlCommand cmd2 = new MySqlCommand(t, conn);
                            conn.Open();
                           int ind = cmd.ExecuteNonQuery();

                            dbr.ds = new DataSet();

                            string sId = output.Value.ToString();
                            string utId = output2.Value.ToString();
                            string eId = output3.Value.ToString();
                            string utkn = output4.Value.ToString();
                            string usrnm = output5.Value.ToString();
                            string isssn = output6.Value.ToString();
                            string statId = output7.Value.ToString();
                          //  cmd2.Dispose();
                            conn.Close();
                            dbr.Id = string.IsNullOrEmpty(sId) ? 0 : Convert.ToInt32(sId);

                            if (dbr.Id > 0)
                            {
                                int usrType = string.IsNullOrEmpty(utId) ? 0 : Convert.ToInt32(utId);
                                int empId = string.IsNullOrEmpty(eId) ? 0 : Convert.ToInt32(eId);
                                int stationId = string.IsNullOrEmpty(statId) ? 0 : Convert.ToInt32(statId);
                                bool isSession = string.IsNullOrEmpty(isssn) ? false : isssn == "0" ? false : true;

                                dbr.dt = new DataTable();
                                dbr.dt.Clear();
                                dbr.dt.Columns.Add("UserType");
                                dbr.dt.Columns.Add("EmployeeId");
                                dbr.dt.Columns.Add("UserToken");
                                dbr.dt.Columns.Add("UserName");
                                dbr.dt.Columns.Add("IsAlreadySession");
                                dbr.dt.Columns.Add("StationId");
                                DataRow dr = dbr.dt.NewRow();
                                dr["UserType"] = usrType;
                                dr["EmployeeId"] = empId;
                                dr["UserToken"] = utkn;
                                dr["UserName"] = usrnm;
                                dr["IsAlreadySession"] = isSession;
                                dr["StationId"] = stationId;
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
                else
                {
                    dbr.Id = 0;
                    dbr.EmployeeName = "";
                    dbr.Status = false;
                    dbr.Message = "Session already exists for this user!!";
                    dbr.IsExists = true;
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

        public DataBaseResult GetEmpDataforPDF(int employeeId)
        {
            string getempData = "";
            DataBaseResult dbr = new DataBaseResult();
            MySqlCommand cmd = new MySqlCommand();
            // MySqlDataAdapter sda;
            try
            {
                dbr.CommandType = "Select";
                getempData = DBConnection.GetEmployeedatabyEmpId(employeeId);

                if (string.IsNullOrEmpty(getempData) || string.IsNullOrEmpty(connectionString))
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

                    ds = new BasicDBOps().GetMultipleRecords(connectionString, getempData);
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
        public DataBaseResult GetAllEmpsDeliveryDetailsforPDF(int stationId, int currentMonth)
        {
            string getempData = "";
            DataBaseResult dbr = new DataBaseResult();
            MySqlCommand cmd = new MySqlCommand();
            // MySqlDataAdapter sda;
            try
            {
                dbr.CommandType = "Select";
                getempData = DBConnection.GetEmpIds(currentMonth, stationId);

                if (string.IsNullOrEmpty(getempData) || string.IsNullOrEmpty(connectionString))
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

                    ds = new BasicDBOps().GetMultipleRecords(connectionString, getempData);
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
        public DataBaseResult GetEmpDeliveryDetailsforPDF(int employeeId, int stationId, int currentMonth)
        {
            string getempData = "";
            DataBaseResult dbr = new DataBaseResult();
            MySqlCommand cmd = new MySqlCommand();
            // MySqlDataAdapter sda;
            try
            {
                dbr.CommandType = "Select";
                getempData = DBConnection.GetEmployeedeliverydetailsbyEmpId(employeeId,currentMonth,stationId);

                if (string.IsNullOrEmpty(getempData) || string.IsNullOrEmpty(connectionString))
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

                    ds = new BasicDBOps().GetMultipleRecords(connectionString, getempData);
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
        public DataBaseResult UpdateDeliveryDetails(List<DeliveryDetails> cdds)
        {
            DataBaseResult dbr = new DataBaseResult();
            MySqlConnection con = new MySqlConnection(connectionString);
            int i = 0;
            con.Open();
            MySqlTransaction transaction = con.BeginTransaction();
            try
            {
                dbr.CommandType = "Insert";            
                if (cdds.Count>0)
                {
                    var valid = cdds.Any(x => x.EmployeeId == 0 || x.StationId == 0);
                    if (!valid)
                    {
                        bool isExists = false;
                        foreach (var item in cdds)
                        {
                            string checkText = DBConnection.GetCHECKDeliveryDetailCDAforEmployeeCurrentMonth(item);
                            isExists = new BasicDBOps().CheckRecordCountExistsOrNot(connectionString, checkText);
                            if (isExists)
                            {
                                string delText = DBConnection.GetDeleteDeliveryDetailCDAforEmployeeCurrentMonth(item);
                                i = new BasicDBOps().ExceuteCommand(connectionString, delText);
                                if (i == 0)
                                    throw new Exception("Something went wrong!!,Unable to Delete Existing Delivery Details");
                            }
                            //int deliveryRate = item.DeliveryRate;
                            //int deliveryCount = item.DeliveryCount;
                            //int petrolAllowance = item.PetrolAllowance;
                            item.TotalAmount = this.GetDeliveryAmountTotal(item);
                            string cmdText = DBConnection.GetUpdateDeiverydetailInsertQuery(item);
                            MySqlCommand command = new MySqlCommand(cmdText, con, transaction);
                            //command.Connection = con;
                            //command.Transaction = transaction;
                            transaction = command.Transaction;
                            i = command.ExecuteNonQuery();
                            command.Dispose();
                            //i = new BasicDBOps().ExceuteCommand(connectionString, cmdText);
                        }
                        transaction.Commit();
                    }
                    else
                    {
                        i= 0;
                        dbr.Status = false;
                        dbr.Message = "Invalid Data to update, Please try again";
                    }
                }

            }
            catch(Exception e)
            {
                try
                {
                    i = 0;
                    transaction.Rollback();
                    dbr.Status = false;
                    dbr.Message = "Cannot Complete the operation Please try again. Reason : "+e.Message;
                }
                catch (Exception ex2)
                {
                    i = 0;
                    dbr.Status = false;
                    dbr.Message = "Rollback Operation failed while updating details. Reason : "+ex2.Message + ". Please contact support team";
                   // throw ex2;
                    // This catch block will handle any errors that may have occurred 
                    // on the server that would cause the rollback to fail, such as 
                    // a closed connection.

                }
                throw e;
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
                if (i > 0)
                {
                    dbr.Status = true;
                    dbr.Message = "Updated delivery details for all Successfully!!!";
                }
            }
            return dbr;
        }
        public int GetDeliveryAmountTotal(DeliveryDetails delv)
        {
            int result = 0;
            try
            {
                delv.TotalAmount = (delv.DeliveryCount * delv.DeliveryRate) + delv.PetrolAllowance + delv.Incentive;
                result = delv.TotalAmount;

            }
            catch 
            {
                result = 0;
            }
            return result;
        }
        public bool CheckIfSessionExists(string userName,int employeeId,int userTypeId)
        {
            string getsessionInfo = "";
            DataBaseResult dbr = new DataBaseResult();
            MySqlCommand cmd = new MySqlCommand();
            bool isExists = false;
            try
            {
                dbr.CommandType = "Select";
                getsessionInfo = DBConnection.GetSessionDetails(userName, employeeId,userTypeId);

                if (string.IsNullOrEmpty(getsessionInfo) || string.IsNullOrEmpty(connectionString))
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
                   
                    isExists = new BasicDBOps().CheckRecordCountExistsOrNot(connectionString, getsessionInfo);


                }


            }
            catch (MySqlException e)
            {
                isExists = false;
                dbr.Status = false;
                dbr.Message = "Something wrong with database : " + e.Message;
                throw e;

            }
            catch (Exception e)
            {
                isExists = false;
                dbr.Message = e.Message;
                dbr.Status = false;
                throw e;
            }
            finally
            {
                cmd.Dispose();


            }
            return isExists;

        }

        public DataBaseResult GetDeliveryRatesbyStation(int stationId)
        {
            DataBaseResult dbr = new DataBaseResult();
            string cmdText = "";
            try
            {
                dbr.CommandType = "Select";
                cmdText = DBConnection.GetDeliveryDetailCDAbyStation(stationId);
                dbr.ds = new DataSet();
                DataSet ds = new DataSet();
                if (string.IsNullOrEmpty(cmdText) || string.IsNullOrEmpty(connectionString))
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
                    ds = new BasicDBOps().GetMultipleRecords(connectionString, cmdText);
                    int count = 0;
                    count = ds.Tables[0].Rows.Count;
                    if (ds.Tables.Count > 0 && count > 0)
                    {
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
                    else
                    {
                        dbr.ds = new DataSet();
                        dbr.Message = "Something went wrong!!!!";
                        dbr.Status = false;
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



            }
            return dbr;

        }
    

        public DataBaseResult GetCDADeliveryDetails(int empId,int stationId,int currentMonth)
        {
            DataBaseResult dbr = new DataBaseResult();
            string cmdText = "";
            try
            {
                dbr.CommandType = "Select";
                cmdText = DBConnection.GetDeliveryDetailCDAbyMonth(empId,stationId,currentMonth);
                dbr.ds = new DataSet();
                DataSet ds = new DataSet();
                if (string.IsNullOrEmpty(cmdText) || string.IsNullOrEmpty(connectionString))
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
                    ds = new BasicDBOps().GetMultipleRecords(connectionString, cmdText);
                    int count = 0;
                    count = ds.Tables[0].Rows.Count;
                    if (ds.Tables.Count > 0 && count > 0)
                    {
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
              


            }
            return dbr;

        }

        public DataBaseResult CreateCommercialConsant(CommercialConstant constant)
        {
            DataBaseResult dbr = new DataBaseResult();
            try
            {
                string cmdTextCC = "";
                cmdTextCC = DBConnection.CheckConstantforStation(constant.StationId);
                bool isExists = false;
                isExists = new BasicDBOps().CheckRecordCountExistsOrNot(connectionString, cmdTextCC);
                if(isExists)
                {
                    dbr.CommandType = "Update";
                    string updateCC = "";
                    updateCC = string.Format("update CommercialConstants set DeliveryRate = {0},PetrolAllowance={1} WHERE StationId = {2};",constant.DeliveryRate,constant.PetrolAllowance,constant.StationId);
                    int changes = 0;
                    changes = new BasicDBOps().ExceuteCommand(connectionString, updateCC);
                    if(changes > 0)
                    {
                        dbr.Status = true;
                        dbr.Message = "Rates updated for this Station Successfully!!!!";
                    }
                    else
                    {
                        dbr.Status = false;
                        dbr.Message = "Something went wrong, rates unable to update  for this Station!!!!";
                    }
                }
                else
                {
                    dbr.CommandType = "Insert";

                    StringBuilder insertCmd = new StringBuilder();
                    insertCmd.Append("Insert into CommercialConstants(StationId,DeliveryRate,PetrolAllowance,Incentives,IsActive) ");
                    insertCmd.AppendLine(" VALUES(");
                    insertCmd.Append(constant.StationId + ",");
                    insertCmd.Append(constant.DeliveryRate + ",");
                    insertCmd.Append(constant.PetrolAllowance + ",");
                    insertCmd.Append(constant.Incentives + ",");
                    insertCmd.Append("1 )");

                    string cmdtext = insertCmd.ToString();
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
                    if (!string.IsNullOrEmpty(cmdtext))
                    {
                        int count = 0;
                        count = new BasicDBOps().ExceuteCommand(connectionString, cmdtext);
                        if (count > 0)
                        {

                            dbr.Status = true;
                            dbr.Message = "Rates fixed for this Station Successfully!!!!";
                        }
                        else
                        {
                            dbr.Status = false;
                            dbr.Message = "Something went wrong, rates unable to fix for this station!!";


                        }

                    }
                    else
                    {
                        dbr.Status = false;
                        dbr.Message = "Something went wrong, No Command to Insert!!";
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

            return dbr;
        }
        public DataBaseResult ResetPassword(int employeeId,string password)
        {
            string resetQuery = "";
            DataBaseResult dbr = new DataBaseResult();
            MySqlCommand cmd = new MySqlCommand();
            try
            {
                dbr.CommandType = "RESET";
                resetQuery = DBConnection.ResetPassword(password,employeeId);

                if (string.IsNullOrEmpty(resetQuery) || string.IsNullOrEmpty(resetQuery))
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
                    int count = 0;
                    count = new BasicDBOps().ExceuteCommand(connectionString, resetQuery);
                    if (count > 0)
                    {

                        dbr.Status = true;
                        dbr.Message = "Password Updated Successfully!!!!";
                    }
                    else
                    {
                        dbr.Status = false;
                        dbr.Message = "Something went wrong, Please contact Support Team!!";


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
        public DataBaseResult UpdateSession(UserType usr)
        {
            string getupdateInfo = "";
            DataBaseResult dbr = new DataBaseResult();
            MySqlCommand cmd = new MySqlCommand();
            try
            {
                dbr.CommandType = "Delete";
                getupdateInfo = DBConnection.SessionUpdate(usr);

                if (string.IsNullOrEmpty(getupdateInfo) || string.IsNullOrEmpty(getupdateInfo))
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
                    int count = 0;
                    count = new BasicDBOps().ExceuteCommand(connectionString, getupdateInfo);
                    if (count > 0)
                    {

                        dbr.Status = true;
                        dbr.Message = "Sesson Updated Successfully!!!!";
                    }
                    else
                    {
                        dbr.Status = false;
                        dbr.Message = "Session either expired or terminated, Please try login again!!";


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

        public DataBaseResult DeleteSession(string userName, int employeeId, int userTypeId)
        {
            string getDeleteInfo = "";
            DataBaseResult dbr = new DataBaseResult();
            MySqlCommand cmd = new MySqlCommand();
            try
            {
                dbr.CommandType = "Delete";
                getDeleteInfo = DBConnection.DeleteSession(userName, employeeId, userTypeId);

                if (string.IsNullOrEmpty(getDeleteInfo) || string.IsNullOrEmpty(getDeleteInfo))
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
                    int count = 0;
                    count = new BasicDBOps().ExceuteCommand(connectionString, getDeleteInfo);
                    if(count>0)
                    {

                        dbr.Status = true;
                        dbr.Message = "Signed Out Successfully!!!!";
                    }
                    else
                    {
                        dbr.Status = false;
                        dbr.Message = "Session either expired or terminated, Please try login again!!";


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

        public DataBaseResult GetConstants()
        {
            string getUserTypes = "";
            DataBaseResult dbr = new DataBaseResult();
            MySqlCommand cmd = new MySqlCommand();
           // MySqlDataAdapter sda;
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
        public DataBaseResult GetAdminDetails()
        {
            string getQuery = "";
            DataBaseResult dbr = new DataBaseResult();
            MySqlCommand cmd = new MySqlCommand();
            List<Dictionary<string, string>> result = new List<Dictionary<string, string>>();
            //MySqlDataAdapter sda;
            try
            {
                dbr.CommandType = "Select";
                result = DBConnection.GetAdminDetails();
                DataSet ds = new DataSet();
                dbr.ds = new DataSet();
                DataTable dtt = new DataTable("AdminDetails");
                DataColumn dtColumn;
                DataRow myDataRow;

                // Create id column  
                dtColumn = new DataColumn();
                dtColumn.DataType = typeof(Int32);
                dtColumn.ColumnName = "Count";
                dtColumn.Caption = "Count";
                dtColumn.ReadOnly = false;
                dtColumn.Unique = false;
                // Add column to the DataColumnCollection.  
                dtt.Columns.Add(dtColumn);

                // Create Name column.    
                dtColumn = new DataColumn();
                dtColumn.DataType = typeof(String);
                dtColumn.ColumnName = "Detail";
                dtColumn.Caption = "Detail";
                dtColumn.AutoIncrement = false;
                dtColumn.ReadOnly = false;
                dtColumn.Unique = false;
                /// Add column to the DataColumnCollection.   
                dtt.Columns.Add(dtColumn);
                foreach (var item in result)
                {
                    Dictionary<string, string> dt = new Dictionary<string, string>();
                    dt = item;
                    foreach(var i in dt.Keys)
                    {
                        getQuery = dt[i];
                        if (string.IsNullOrEmpty(getQuery) || string.IsNullOrEmpty(connectionString))
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
                  
                            //DataTable dt = new DataTable();
                            //sda = new MySqlDataAdapter(getUserTypes, conn);
                            //sda.SelectCommand.CommandType = CommandType.Text;
                            //sda.Fill(ds);
                            int count = 0;
                            count = new BasicDBOps().GetTotalCountOfQuery(connectionString, getQuery);
                            
                            if (count > 0)
                            {
                               

                                myDataRow = dtt.NewRow();
                                myDataRow["Count"] = count;
                                myDataRow["Detail"] =i;
                                dtt.Rows.Add(myDataRow);

                            }

                        }

                        }
                   

                        // }




                   

                }
                if (dtt.Rows.Count > 0)
                {
                    ds.Tables.Add(dtt);
                    dbr.ds = ds;
                    dbr.Message = "Details retreived Successfully!!!";
                    dbr.Status = true;
                }
                else
                {
                    dbr.ds = ds;
                    dbr.Message = "No Records Found for this request!!";
                    dbr.Status = true;


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
        public DataBaseResult GetPaginationRecords(int stationId, string table, string vstartDate, string vEndDate = "", int? page = 1, int? pagesize = 5, string status = "", bool isEmployee = false, int currentMonth = 0)
        {
            Dictionary<string, string> getSelectQuery = new Dictionary<string, string>();
            DataBaseResult dbr = new DataBaseResult();
            MySqlCommand cmd = new MySqlCommand();
           // MySqlDataAdapter sda;
            try
            {
                dbr.CommandType = "Select";
                getSelectQuery = DBConnection.GetRecordsforPagination(stationId, table,vstartDate,vEndDate,page,pagesize,status,isEmployee,currentMonth);

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
        public int ClearInactiveSessions(string action)
        {
            int count = 0;
            string cmdText = "";
            try
            {
                if (action == "c")
                {
                    cmdText = DBConnection.ClearInactiveSessions();
                    count = new BasicDBOps().GetTotalCountOfQuery(connectionString, cmdText);
                }
                else
                {
                    DateTime dt = DateTime.Now;
                    dt = dt.GetIndianDateTimeNow();
                    string d = dt.DateTimetoString();
                    string query = string.Format("DELETE from UserSessions where '{0}'  not between StartDate and EndDate;", d);
                    count = new BasicDBOps().ExceuteCommand(connectionString, query);
                }

            }
            catch (MySqlException e)
            {
                count = 0;
                throw e;

            }
            catch (Exception e)
            {
                count = 0;
                throw e;
            }
            finally
            {
                //cmd.Dispose();


            }
            return count;
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
        public DataBaseResult GetLoginUserInfo(int usertypeId, int employeeID)
        {
            string getUserInfo = "";
            DataBaseResult dbr = new DataBaseResult();
            MySqlCommand cmd = new MySqlCommand();
            try
            {
                dbr.CommandType = "Select";
                getUserInfo = DBConnection.GetLoginUserInfo(usertypeId, employeeID);

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
                    ds = new BasicDBOps().GetMultipleRecords(connectionString, getUserInfo);
                    int count = 0;
                    count = ds.Tables[0].Rows.Count;
                    if (ds.Tables.Count > 0 && count > 0)
                    {
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
                                result.Message = "Invalid token or Session will be terminated, please wait ten minutes,  try login again!!";
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
                                result.Message = "Invalid Session or session  will be terminated for this user, please wait ten minutes , try login again!!";
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

        public DataBaseResult GetRegisteredUser(int empId)
        {
            string getRegisteredUser = "";
            DataBaseResult dbr = new DataBaseResult();
            MySqlCommand cmd = new MySqlCommand();
            //MySqlDataAdapter sda;
            try
            {
                dbr.CommandType = "Select";
                getRegisteredUser = DBConnection.GetRegisteredUser(empId);

                if (string.IsNullOrEmpty(getRegisteredUser) || string.IsNullOrEmpty(connectionString))
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
                    ds = new BasicDBOps().GetMultipleRecords(connectionString, getRegisteredUser);
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
        public DataBaseResult GetRegisteredUsers(int stationId)
        {
            string getRegisteredUsers = "";
            DataBaseResult dbr = new DataBaseResult();
            MySqlCommand cmd = new MySqlCommand();
            //MySqlDataAdapter sda;
            try
            {
                dbr.CommandType = "Select";
                getRegisteredUsers = DBConnection.GetRegisteredUsers(stationId);

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

        public DataBaseResult ApproveUser(int registerId,string status,string empCode="",int pId=0)
        {
            string getApproveUser = "";
            DataBaseResult dbr = new DataBaseResult();
           // MySqlCommand cmd = new MySqlCommand();
            try
            {
              //  dbr.Id = registerId;
                dbr.CommandType = status=="a"?"UPDATE":"DELETE";
                getApproveUser = DBConnection.ApproveUser(registerId,status);
                dbr.Id = 0;
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
                    if(status.ToLower()=="a")
                    {
                        if(!string.IsNullOrEmpty(empCode) && pId >0)
                        {
                            string cmdtxt = string.Format("Update register SET EmpCode='{0}',PID={1} WHERE RegisterId={2};",empCode
                                ,pId,registerId);
                            int result = new BasicDBOps().ExceuteCommand(connectionString, cmdtxt);
                            if (result > 0)
                            {
                                int res = new BasicDBOps().ExceuteCommand(connectionString, getApproveUser);
                                if (res > 0)
                                {
                                    if(status == "a")
                                    {
                                        string txtCmd = string.Format("select EmployeeId from employees WHERE EmpCode='{0}' AND IsActive=1;", empCode);
                                        DataSet d = new DataSet();
                                        d = new BasicDBOps().GetMultipleRecords(connectionString, txtCmd);
                                        if(d.Tables.Count > 0)
                                        {
                                            if(d.Tables[0].Rows.Count > 0)
                                            {
                                                int r = 0;
                                                string eID = d.Tables[0].Rows[0]["EmployeeId"].ToString();
                                                bool success = int.TryParse(eID, out r);
                                                dbr.Id = (success == true) ? r : 0;
                                    
                                            }
                                        }
                                    }
                                    dbr.Message = status == "a" ? "User Approved Successfully!!!" : "User Removed Successfully!!";
                                    dbr.Status = true;

                                }
                                else
                                {

                                    dbr.Message = "Something went wrong,User not approved for this request!!";
                                    dbr.Status = false;


                                }
                            }
                            else
                            {

                                dbr.Message = "Something went wrong,User not approved for this request!!";
                                dbr.Status = false;


                            }

                        }
                        else
                        {
                            dbr.Message = "Something went wrong,invalid input for this request, try again!!";
                            dbr.Status = false;
                        }


                    }
                    else if(status.ToLower() == "r")
                    {
                        int res = new BasicDBOps().ExceuteCommand(connectionString, getApproveUser);
                        if (res > 0)
                        {

                            dbr.Message =  "User Removed Successfully!!";
                            dbr.Status = true;

                        }
                        else
                        {

                            dbr.Message = "Something went wrong,User not removed for this request!!";
                            dbr.Status = false;


                        }

                    }
                    else
                    {

                        dbr.Message = "Invalid Request, Please select proper input!!";
                        dbr.Status = false;

                    }
                    //using (MySqlConnection conn = new MySqlConnection(connectionString))
                    //{
                    //cmd.CommandText = getApproveUser;
                    //cmd.CommandType = CommandType.Text;
                    //int res = cmd.ExecuteNonQuery();


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
        public DataBaseResult CheckUserExists(string userName)
        {
            string getUserInfo = "";
            DataBaseResult dbr = new DataBaseResult();
            MySqlCommand cmd = new MySqlCommand();
            try
            {
                dbr.CommandType = "Select";
                getUserInfo = DBConnection.CheckUserNameExists(userName);

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
                    
                    dbr.IsExists = new BasicDBOps().CheckRecordCountExistsOrNot(connectionString, getUserInfo);
                    if (dbr.IsExists)
                    {

                        dbr.Message = "User Already Existed,Please try another UserName!!!";
                        dbr.Status = false;

                    }
                    else 
                    {
                        dbr.Message = "Given UserName Accepted!!";
                        dbr.Status = true;
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

        public DataBaseResult CheckEmpCodeExists(string empCode,bool isEmployee)
        {
            string getUserInfo = "";
            DataBaseResult dbr = new DataBaseResult();
            MySqlCommand cmd = new MySqlCommand();
            try
            {
                dbr.CommandType = "Select";
                getUserInfo = DBConnection.CheckEmpCodeExists(empCode,isEmployee);

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

                    dbr.IsExists = new BasicDBOps().CheckRecordCountExistsOrNot(connectionString, getUserInfo);
                    if (dbr.IsExists)
                    {

                        dbr.Message = "EmployeeCode Already Existed,Please try another one!!!";
                        dbr.Status = false;

                    }
                    else
                    {
                        dbr.Message = "Given EmployeeCode Accepted!!";
                        dbr.Status = true;
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

        public Tuple<string,string> GetStationNameByStationId(int stationId)
        {
            Tuple<string, string> station =Tuple.Create("","");
            try
            {
                DataSet ds = new DataSet();
                if(stationId>0)
                {
                    string cmd = string.Format("Select Station,StationCode from stations where StationId={0}", stationId);
                    ds = new BasicDBOps().GetMultipleRecords(connectionString, cmd);
                    if(ds.Tables.Count> 0)
                    {
                        if(ds.Tables[0].Rows.Count>0)
                        {
                            string code = ds.Tables[0].Rows[0]["StationCode"].ToString();
                            string name = ds.Tables[0].Rows[0]["Station"].ToString();
                            station = Tuple.Create(name, code);
                        }
                    }
                }

            }
            catch
            {
                station = Tuple.Create("", "");
            }
            return station;
        }
        public int TraceError(ErrorLogTrack log)
        {
            int changes = 0;
            try
            {
                string cmdText = DBConnection.GetErrorLogInsertQuery(log);
                if(!string.IsNullOrEmpty(cmdText))
                {
                    changes = new BasicDBOps().ExceuteCommand(connectionString, cmdText);
                }
            }
            catch(Exception e)
            {
                changes = 0;
                throw e;
            }
            return changes;
        }
        public void CreateBackup(string file)
        {
            try
            {
                BasicDBOps dbops = new BasicDBOps();
                dbops.TakeBackup(connectionString, file);
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                //con.
            }
        }
        public DataBaseResult RestoreDB(string file)
        {
            DataBaseResult dbr = new DataBaseResult();
            try
            {
                dbr.CommandType = "Restore";
                BasicDBOps dbops = new BasicDBOps();
                dbr.IsExists = dbops.RestoreDB(connectionString2, file);
                if(dbr.IsExists)
                {
                    dbr.Message = "DataBase recovered Succesfully with this backup file. Please try login again after 10 mins!!!!";
                    dbr.Status = true;
                }
                else
                {
                    dbr.Message = "Something went wrong!! Database not recovered properly,Please try again!!!!";
                    dbr.Status = false; 
                }
            }
            catch (Exception e)
            {
                dbr.Message = e.Message;
                dbr.Status = false;
                throw e;
            }
            finally
            {
                //con.
            }
            return dbr;
        }

    }


}
