using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace GSB.Class
{

    public class SQLToDataTable
    {
        public static SqlConnection conn;
        SqlCommand comm;
        string constr = ConfigurationManager.ConnectionStrings["GSBSQLServer"].ToString();

        public SQLToDataTable()
        {
            try
            {
                conn = new SqlConnection(constr);
                conn.Open();
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        public bool Open()
        {
            bool result = false;
            if (conn.State == ConnectionState.Open)
            {
                result = true;
            }
            return result;
        }

        public void OpenConnection()
        {
            if ((conn != null))
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn = new SqlConnection(constr);
                    conn.Open();
                }
            }
            else
            {
                conn = new SqlConnection(constr);
                conn.Open();
            }

            if (null == comm)
            {
                comm = new SqlCommand() { Connection = conn, CommandTimeout = 0, CommandType = CommandType.Text };
            }

        }

        //public bool Open()
        //{
        //    bool result = false;
        //    try
        //    {
        //        //if (conn.State != ConnectionState.Open)
        //        //{
        //        //    conn.ConnectionString = ConfigurationManager.ConnectionStrings["GSBSQLServer"].ToString();
        //        //    //string abcd = ConfigurationManager.AppSettings["FTPIP"].ToString();
        //        //    conn.Open();

        //        //    return true;
        //        //}
        //        //else
        //        //{
        //        //    return true;
        //        //}

        //        string constr = ConfigurationManager.ConnectionStrings["GSBSQLServer"].ToString();
        //        if ((conn != null))
        //        {
        //            if (conn.State == ConnectionState.Closed)
        //            {
        //                conn.Open();
        //            }
        //        }
        //        else
        //        {
        //            conn = new SqlConnection(constr);
        //            conn.Open();
        //        }

        //        if (null == comm)
        //        {
        //            comm = new SqlCommand() { Connection = conn, CommandTimeout = 0, CommandType = CommandType.Text };
        //        }

        //        result = true;
        //    }
        //    catch
        //    {
        //        if (conn != null)
        //            ((IDisposable)conn).Dispose();
        //    }

        //    return result;
        //}

        public DataTable ExcuteSQL(string sql)
        {
            DataTable table = new DataTable();
            try
            {
                OpenConnection();
                //comm = new SqlCommand(sql, conn, tran);
                comm.CommandText = sql;
                comm.CommandTimeout = 120;

                SqlDataAdapter adapter = new SqlDataAdapter(comm);
                adapter.Fill(table);

                adapter.Dispose();
                adapter = null;
            }
            catch (SqlException ex)
            {                               
                //throw new Exception(ex.Message, ex.InnerException);
            }

            return table;
        }

        public bool BulkUpload(string constr, string tablename, DataTable dt)
        {
            bool result = false;
            try
            {
                dt.TableName = tablename;
                //string constr = "your connection string";
                using (SqlConnection connection = new SqlConnection(constr))
                {
                    connection.Open();
                    //CreatingTranscationsothatitcanrollbackifgotanyerrorwhileuploading
                    SqlTransaction trans = connection.BeginTransaction();
                    //Start bulkCopy
                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection,
                        SqlBulkCopyOptions.TableLock |
                        SqlBulkCopyOptions.FireTriggers,
                        trans))
                    {
                        //Setting timeout to 0 means no time out for this command will not timeout until upload complete.
                        //Change as per you
                        bulkCopy.BulkCopyTimeout = 0;
                        bulkCopy.DestinationTableName = dt.TableName;
                        //write the data in the "dataTable"
                        bulkCopy.WriteToServer(dt);
                        bulkCopy.Close();
                        trans.Commit();
                    }
                }
                result = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        public int ExecuteNonQuery(string sql)
        {
            int result = 0;
            try
            {
                OpenConnection();
                comm = new SqlCommand(sql, conn);
                comm.CommandText = sql;
                result = comm.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
            return result;
        }

        public void CloseConnection()
        {
            if ((conn != null))
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
        }
    }

    public class SQLToDataRTTable
    {
        public static SqlConnection conn = new SqlConnection();

        public bool Open()
        {
            try
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.ConnectionString = ConfigurationManager.ConnectionStrings["GSBRTSQLServer"].ToString();
                    //string abcd = ConfigurationManager.AppSettings["FTPIP"].ToString();
                    conn.Open();
                    return true;
                }
                else
                {
                    return true;
                }
            }
            catch
            {
                if (conn != null)
                    ((IDisposable)conn).Dispose();
            }

            return false;
        }

        public DataTable ExcuteSQL(string query)
        {
            DataTable dt = new DataTable();
            if (Open())
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                //cmd.CommandTimeout = 20;
                cmd.CommandType = CommandType.Text;
                if (query.Length > 6)
                    if (query.ToLower().Substring(0, 6).ToString() != "select") ;
                //cmd.ExecuteNonQuery();
                SqlDataAdapter adap = new SqlDataAdapter(cmd);

                try
                {
                    adap.Fill(dt);
                    return dt;
                }
                catch
                {
                    conn.Dispose();
                }
                finally
                {
                    conn.Dispose();
                }
            }
            return null;
        }

        //public void Excute_ComboBox(string query, DropDownList dp, string name, string value)
        //{
        //    DataTable dt = ExcuteSQL(query);

        //    dp.DataMember = name;
        //    dp.DataValueField = value;
        //    dp.DataSource = dt;

        //    if (dp.Items.Count > 0)
        //        dp.SelectedIndex = 0;
        //}
    }

}