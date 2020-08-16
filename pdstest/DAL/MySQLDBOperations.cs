using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using pdstest.Models;
using MySql.Data.MySqlClient;
using System.Data;

namespace pdstest.DAL
{
    public class MySQLDBOperations
    {

        public static string connectionString = DBConnection.GetDBConnection(false);


        public DataBaseResult CreateEmployee(RegisterEmployee input)
        {
            string insertQuery = "";
            DataBaseResult dbr = new DataBaseResult();
            MySqlCommand cmd = new MySqlCommand();
            MySqlParameter param;

            try
            {
                dbr.CommandType = "Insert";
                insertQuery = DBConnection.GetInsertQuery();

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

                        param = new MySqlParameter("@UserTypeId", input.UserTypeId);
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


        public DataBaseResult GetUserTypes()
        {
            string getUserTypes = "";
            DataBaseResult dbr = new DataBaseResult();
            MySqlCommand cmd = new MySqlCommand();
            MySqlDataAdapter sda;
            try
            {
                dbr.CommandType = "Select";
                getUserTypes = DBConnection.GetUserTypes();

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
                    using (MySqlConnection conn = new MySqlConnection(connectionString))
                    {
                        DataSet ds = new DataSet();
                        dbr.ds = new DataSet();
                        DataTable dt = new DataTable();
                        sda = new MySqlDataAdapter(getUserTypes, conn);
                        sda.SelectCommand.CommandType = CommandType.Text;
                        sda.Fill(ds);
                        int count = 0;
                        count = ds.Tables[0].Rows.Count;
                        if (ds != null || ds.Tables.Count > 0 || count > 0)
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
                            dbr.Message = "No Records Found for this table!!";
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

    }
}
