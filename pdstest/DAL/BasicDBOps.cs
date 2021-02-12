using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;

namespace pdstest.DAL
{
    public class BasicDBOps
    {
        public DataSet GetMultipleRecords(string connectionString,string cmdText)
        {
            DataSet ds = new DataSet();
            MySqlCommand cmd = new MySqlCommand();
            MySqlConnection conn = new MySqlConnection(connectionString);
            try 
            {
                using (conn = new MySqlConnection(connectionString))
                {
                    cmd = new MySqlCommand(cmdText, conn);
                    MySqlDataAdapter sda = new MySqlDataAdapter(cmd);
                    sda.SelectCommand.CommandType = CommandType.Text;
                    sda.Fill(ds);

                }

            }
            catch (MySqlException e)
            {

                throw e;

            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                cmd.Dispose();
                if (conn.State == ConnectionState.Open)
                    conn.Close();


            }
            return ds;
        
        }
        public int GetTotalCountOfQuery(string connectionString, string cmdText)
        {
            int result = 0;
            MySqlCommand cmd = new MySqlCommand();
            MySqlConnection conn = new MySqlConnection(connectionString);
            try 
            {
                using (conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    using (cmd = new MySqlCommand(cmdText, conn))
                    {
                        int count = Convert.ToInt32(cmd.ExecuteScalar());
                        result= count;
                    }
                }
            }
            catch (MySqlException e)
            {
                result = 0;
                throw e;

            }
            catch (Exception e)
            {
                result = 0;
                throw e;
            }
            finally
            {
                cmd.Dispose();
                if (conn.State == ConnectionState.Open)
                    conn.Close();


            }
            return result;

        }
        public bool CheckRecordCountExistsOrNot(string connectionString, string cmdText) 
        {
            bool isExists = false;
            MySqlCommand cmd = new MySqlCommand();
            MySqlConnection conn = new MySqlConnection(connectionString);
            try
            {
                using (conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    cmd.CommandText = cmdText;
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = conn;
                    MySqlDataReader reader = cmd.ExecuteReader();
                    int res1 = 0;
                    while (reader.Read())
                        res1 = reader.GetInt32(0);
                    if (res1 > 0)
                    {
                        isExists = true;
                    }
                    else isExists = false;
                    conn.Close();
                }
            }
            catch (MySqlException e)
            {

                throw e;

            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                cmd.Dispose();
                if (conn.State == ConnectionState.Open)
                    conn.Close();


            }
            return isExists;
        
        }
        public int ExceuteCommand(string connectionString, string cmdText)
        {
            int changes = 0;
            MySqlCommand cmd = new MySqlCommand();
            MySqlConnection conn = new MySqlConnection(connectionString);
            try
            {
                using (conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    cmd.CommandText = cmdText;
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = conn;
                    changes = cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (MySqlException e)
            {

                throw e;

            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                cmd.Dispose();
                if (conn.State == ConnectionState.Open)
                    conn.Close();


            }
            return changes;

        }

        public void TakeBackup(string connectionString,string file)
        {
            try
            {
                using(MySqlConnection con = new MySqlConnection(connectionString))
                {
                    using(MySqlCommand cmd = new MySqlCommand())
                    {
                        using(MySqlBackup mb = new MySqlBackup(cmd))
                        {
                            cmd.Connection = con;
                            con.Open();
                            mb.ExportToFile(file);
                            con.Close();
                        }
                    }
                }
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

        public bool RestoreDB(string connectionString, string file)
        {
            bool isSuccess = false;
            try
            {
                using (MySqlConnection con = new MySqlConnection(connectionString))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        using (MySqlBackup mb = new MySqlBackup(cmd))
                        {
                            cmd.Connection = con;
                            con.Open();
                            mb.ImportFromFile(file);
                            con.Close();
                            isSuccess = true;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                isSuccess = false;
                throw e;
            }
            finally
            {
                //con.
            }
            return isSuccess;
        }
    }
}
