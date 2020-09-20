using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using pdstest.Models;
using System.Data.Common;

namespace pdstest.DAL
{
    public class DBOperations
    {
        public static string connectionString = DBConnection.GetDBConnection(false);
  
        
        public DataBaseResult CreateEmployee(RegisterEmployee input)
        {
            string insertQuery = "";
            DataBaseResult dbr = new DataBaseResult();
            SqlCommand cmd = new SqlCommand();
            SqlParameter param;
             
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
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        cmd.CommandText = insertQuery;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = conn;

                        param = new SqlParameter("@FirstName", input.FirstName);
                        param.Direction = ParameterDirection.Input;
                        param.SqlDbType = SqlDbType.VarChar;
                        param.Size = 50;
                        cmd.Parameters.Add(param);

                        param = new SqlParameter("@LastName", input.LastName);
                        param.Direction = ParameterDirection.Input;
                        param.SqlDbType = SqlDbType.VarChar;
                        param.Size = 50;
                        cmd.Parameters.Add(param);

                        param = new SqlParameter("@DOB", input.DOB);
                        param.Direction = ParameterDirection.Input;
                        param.SqlDbType = SqlDbType.VarChar;
                        param.Size = 30;
                        cmd.Parameters.Add(param);

                        param = new SqlParameter("@Age", input.Age);
                        param.Direction = ParameterDirection.Input;
                        param.SqlDbType = SqlDbType.Int;
                        cmd.Parameters.Add(param);

                        param = new SqlParameter("@BloodGroup", input.BloodGroup);
                        param.Direction = ParameterDirection.Input;
                        param.SqlDbType = SqlDbType.VarChar;
                        param.Size = 5;
                        cmd.Parameters.Add(param);

                        param = new SqlParameter("@Gender", input.Gender);
                        param.Direction = ParameterDirection.Input;
                        param.SqlDbType = SqlDbType.VarChar;
                        param.Size = 5;
                        cmd.Parameters.Add(param);

                        param = new SqlParameter("@MaritalStatus", input.MaritalStatus);
                        param.Direction = ParameterDirection.Input;
                        param.SqlDbType = SqlDbType.Bit;
                        cmd.Parameters.Add(param);

                        param = new SqlParameter("@Phone", input.Phone);
                        param.Direction = ParameterDirection.Input;
                        param.SqlDbType = SqlDbType.VarChar;
                        param.Size = 12;
                        cmd.Parameters.Add(param);

                        param = new SqlParameter("@Address1", input.Address1);
                        param.Direction = ParameterDirection.Input;
                        param.SqlDbType = SqlDbType.NVarChar;
                        param.Size = -1;
                        cmd.Parameters.Add(param);

                        param = new SqlParameter("@Address2", input.Address2);
                        param.Direction = ParameterDirection.Input;
                        param.SqlDbType = SqlDbType.NVarChar;
                        param.Size = -1;
                        cmd.Parameters.Add(param);


                        param = new SqlParameter("@Place", input.Place);
                        param.Direction = ParameterDirection.Input;
                        param.SqlDbType = SqlDbType.VarChar;
                        param.Size = 50;
                        cmd.Parameters.Add(param);

                        param = new SqlParameter("@StateName", input.State);
                        param.Direction = ParameterDirection.Input;
                        param.SqlDbType = SqlDbType.VarChar;
                        param.Size = 100;
                        cmd.Parameters.Add(param);

                        param = new SqlParameter("@PostalCode", input.PostalCode);
                        param.Direction = ParameterDirection.Input;
                        param.SqlDbType = SqlDbType.VarChar;
                        param.Size = 10;
                        cmd.Parameters.Add(param);

                        param = new SqlParameter("@AadharNumber", input.AadharNumber);
                        param.Direction = ParameterDirection.Input;
                        param.SqlDbType = SqlDbType.VarChar;
                        param.Size = 20;
                        cmd.Parameters.Add(param);


                        param = new SqlParameter("@PAN", input.PANNumber);
                        param.Direction = ParameterDirection.Input;
                        param.SqlDbType = SqlDbType.VarChar;
                        param.Size = 20;
                        cmd.Parameters.Add(param);

                        param = new SqlParameter("@IsPermanent", input.IsPermanent);
                        param.Direction = ParameterDirection.Input;
                        param.SqlDbType = SqlDbType.Bit;
                        cmd.Parameters.Add(param);

                        param = new SqlParameter("@EmployeeType", input.EmployeeType);
                        param.Direction = ParameterDirection.Input;
                        param.SqlDbType = SqlDbType.VarChar;
                        param.Size = 50;
                        cmd.Parameters.Add(param);


                        param = new SqlParameter("@Gaurd_firstname", input.Gaurd_firstname);
                        param.Direction = ParameterDirection.Input;
                        param.SqlDbType = SqlDbType.VarChar;
                        param.Size = 50;
                        cmd.Parameters.Add(param);


                        param = new SqlParameter("@Gaurd_lastname", input.Gaurd_lastname);
                        param.Direction = ParameterDirection.Input;
                        param.SqlDbType = SqlDbType.VarChar;
                        param.Size = 50;
                        cmd.Parameters.Add(param);


                        param = new SqlParameter("@Gaurd_middlename", input.Gaurd_middlename);
                        param.Direction = ParameterDirection.Input;
                        param.SqlDbType = SqlDbType.VarChar;
                        param.Size = 50;
                        cmd.Parameters.Add(param);

                        param = new SqlParameter("@Gaurd_Phone", input.Gaurd_PhoneNumber);
                        param.Direction = ParameterDirection.Input;
                        param.SqlDbType = SqlDbType.VarChar;
                        param.Size = 12;
                        cmd.Parameters.Add(param);

                        param = new SqlParameter("@DOJ", input.DOJ);
                        param.Direction = ParameterDirection.Input;
                        param.SqlDbType = SqlDbType.VarChar;
                        param.Size = 30;
                        cmd.Parameters.Add(param);

                        param = new SqlParameter("@LoginType", input.LoginType);
                        param.Direction = ParameterDirection.Input;
                        param.SqlDbType = SqlDbType.VarChar;
                        param.Size = 50;
                        cmd.Parameters.Add(param);

                        param = new SqlParameter("@Designation", input.Designation);
                        param.Direction = ParameterDirection.Input;
                        param.SqlDbType = SqlDbType.VarChar;
                        param.Size = 50;
                        cmd.Parameters.Add(param);

                        param = new SqlParameter("@StateCode", input.StationCode);
                        param.Direction = ParameterDirection.Input;
                        param.SqlDbType = SqlDbType.VarChar;
                        param.Size = 30;
                        cmd.Parameters.Add(param);

                        param = new SqlParameter("@LocationName", input.LocationName);
                        param.Direction = ParameterDirection.Input;
                        param.SqlDbType = SqlDbType.VarChar;
                        param.Size = 50;
                        cmd.Parameters.Add(param);

                        param = new SqlParameter("@IsActive", false);
                        param.Direction = ParameterDirection.Input;
                        param.SqlDbType = SqlDbType.Bit;
                        cmd.Parameters.Add(param);

                        param = new SqlParameter("@UserTypeId", input.UserTypeId);
                        param.Direction = ParameterDirection.Input;
                        param.SqlDbType = SqlDbType.Int;
                        cmd.Parameters.Add(param);

                        SqlParameter output = new SqlParameter();
                        output.ParameterName = "@EmpId";
                        output.SqlDbType = SqlDbType.Int;
                        output.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(output);

                        SqlParameter output2 = new SqlParameter();
                        output2.ParameterName = "@EmpName";
                        output2.SqlDbType = SqlDbType.VarChar;
                        output2.Size = 50;
                        output2.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(output2);

                        conn.Open();
                        cmd.ExecuteNonQuery();

                        string empId = output.Value.ToString();

                        string empName = output2.Value.ToString();
                        conn.Close();
                        dbr.Id = string.IsNullOrEmpty(empId) ? 0 : Convert.ToInt32(empId);

                        dbr.EmployeeName = empName;
                        dbr.Status = true;
                        dbr.Message = "Employee Registered Successfully!!!";




                    }

                }



            }
            catch (SqlException e)
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


        public DataBaseResult GetUserTypes()
        {
            string getUserTypes = "";
            DataBaseResult dbr = new DataBaseResult();
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter sda;
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
                    dbr.ds = new DataSet();
                }
                else
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        DataSet ds = new DataSet();
                        dbr.ds = new DataSet();
                        DataTable dt = new DataTable();
                        sda = new SqlDataAdapter(getUserTypes, conn);
                        sda.Fill(ds);
                        int count = 0;
                        count = ds.Tables[0].Rows.Count;
                        if (ds != null || ds.Tables.Count > 0 || count > 0)
                        {
                            dbr.ds = ds;
                            dbr.Message = "Records retreived Successfully!!!";
                            dbr.Status = true;

                        }
                        else if (count == 0)
                        {
                            dbr.ds = ds;
                            dbr.Message = "No Records Found for this table!!";
                            dbr.Status = true;


                        }

                    }




                }


            }
            catch (SqlException e)
            {
                
                dbr.Status = false;
                dbr.Message = "Something wrong with database : " +e.Message;
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
    }
}
