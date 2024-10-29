using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace Arsoft.Utility.MSSQL
{
    public class MsSqlAccess
    {
        #region "Attibutes"
        SqlConnection conn;
        SqlCommand comm;
        SqlTransaction tran;
        SqlDataReader reader;
        bool setprepare = false;
        SqlBulkCopy bulkCopy;
        #endregion

        #region "Methods"

        public MsSqlAccess()
        {
            try
            {
                conn = new SqlConnection(MsSqlConfiguration.ConnectionString);
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
                    conn.Open();
                }
            }
            else
            {
                conn = new SqlConnection(MsSqlConfiguration.ConnectionString);
                conn.Open();
            }

            if (null == comm)
            {
                comm = new SqlCommand() { Connection = conn, CommandTimeout = 0, CommandType = CommandType.Text };
            }

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

        public void Dispose()
        {
            try
            {
                this.RollbackTransaction();
                this.CloseReader();
                this.CloseConnection();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
            finally
            {
                reader = null;
                tran = null;
                comm = null;
                conn = null;
            }
            GC.SuppressFinalize(this);
        }

        #region "ExecuteNonQuery"

        public int ExecuteNonQuery(string sql)
        {
            return this.ExecuteNonQuery(sql, null);
        }

        public int ExecuteNonQuery(string sql, MsSqlParameter[] param)
        {
            int result = 0;
            try
            {
                OpenConnection();
                //comm = new SqlCommand(sql, conn, tran);
                comm.CommandText = sql;
                comm.Transaction = tran;
                ExtractParameter(param);
                result = comm.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
            return result;
        }

        #endregion

        #region "ExecuteScalar"

        public object ExecuteScalar(string sql)
        {
            return this.ExecuteScalar(sql, null);
        }

        public object ExecuteScalar(string sql, MsSqlParameter[] param)
        {
            object result = null;
            try
            {
                OpenConnection();
                //comm = new SqlCommand(sql, conn, tran);
                comm.CommandText = sql;
                comm.Transaction = tran;
                ExtractParameter(param);
                result = comm.ExecuteScalar();
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
            return result;
        }

        private SqlParameter[] testParameter(MsSqlParameter[] p)
        {
            SqlParameter[] param = new SqlParameter[p.Length];
            int i = 0;
            for (i = 0; i <= param.Length - 1; i++)
            {
                param[i] = new SqlParameter(p[i].Name, p[i].Values);
            }
            return param;
        }

        #endregion

        #region "ExecuteReader"

        public void ExecuteReader(string sql)
        {
            this.ExecuteReader(sql, null);
        }

        public void ExecuteReader(string sql, MsSqlParameter[] param)
        {
            try
            {
                OpenConnection();
                //comm = new SqlCommand(sql, conn, tran);
                comm.CommandText = sql;
                comm.Transaction = tran;
                this.ExtractParameter(param);
                reader = comm.ExecuteReader();
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        public int CommandExecuteNonQuery(string sql, CommandType commandsType, MsSqlParameter[] param)
        {
            int result = 0;
            try
            {
                OpenConnection();
                //comm = new SqlCommand(sql, conn, tran);
                comm.CommandText = sql;
                comm.Transaction = tran;
                comm.CommandType = commandsType;
                this.ExtractParameter(param);
                result = comm.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
            return result;
        }

        public void CommandExecuteReader(string sql, CommandType commandsType, MsSqlParameter[] param = null)
        {
            try
            {
                OpenConnection();
                //comm = new SqlCommand(sql, conn, tran);
                comm.CommandText = sql;
                comm.Transaction = tran;
                comm.CommandType = commandsType;
                this.ExtractParameter(param);
                reader = comm.ExecuteReader();
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        public bool Read()
        {
            bool result = false;
            if ((reader != null) & !reader.IsClosed)
            {
                result = reader.Read();
            }
            return result;
        }

        public object GetItem(int index)
        {
            if (DBNull.Value.Equals(reader[index]))
            {
                return null;
            }
            else
            {
                return reader[index];
            }
        }

        public object GetItem(string filename)
        {
            if (DBNull.Value.Equals(reader[filename]))
            {
                return null;
            }
            else
            {
                return reader[filename];
            }
        }

        public object GetItemParameters(string filename)
        {
            //@filename
            if (DBNull.Value.Equals(comm.Parameters[filename].Value))
            {
                return null;
            }
            else
            {
                return comm.Parameters[filename].Value;
            }
        }

        public void CloseReader()
        {
            if ((reader != null))
            {
                if (!reader.IsClosed)
                {
                    reader.Close();
                }
            }
            reader = null;
            if ((comm != null))
                comm.Dispose();
            comm = null;
        }

        public int FieldCount()
        {
            int fcount = 0;
            if ((reader != null))
            {
                if (!reader.IsClosed)
                {
                    fcount = reader.FieldCount;
                }
            }
            return fcount;
        }

        #endregion

        #region "ExecuteAdapter"

        public DataTable ExecuteAdapter(string sql)
        {
            return this.ExecuteAdapter(sql, null);
        }

        public DataTable ExecuteAdapter(string sql, MsSqlParameter[] param)
        {
            DataTable table = new DataTable();
            try
            {
                OpenConnection();
                //comm = new SqlCommand(sql, conn, tran);
                comm.CommandText = sql;
                comm.Transaction = tran;
                this.ExtractParameter(param);

                SqlDataAdapter adapter = new SqlDataAdapter(comm);
                adapter.Fill(table);
                adapter.Dispose();
                adapter = null;
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
            return table;
        }

        public DataSet ExecuteAdapter(string sql, int startRecord, int maxRecords)
        {
            return this.ExecuteAdapter(sql, null, startRecord, maxRecords);
        }

        public DataSet ExecuteAdapter(string sql, MsSqlParameter[] param, int startRecord, int maxRecords)
        {
            DataSet table = new DataSet();
            try
            {
                OpenConnection();
                //comm = new SqlCommand(sql, conn, tran);
                comm.CommandText = sql;
                comm.Transaction = tran;
                this.ExtractParameter(param);

                SqlDataAdapter adapter = new SqlDataAdapter(comm);
                adapter.Fill(table, startRecord, maxRecords, "resulttable");
                adapter.Dispose();
                adapter = null;
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
            return table;
        }

        #endregion

        #region "Transaction"

        public void BeginTransaction()
        {
            if (tran == null)
            {
                tran = conn.BeginTransaction();
            }
        }

        public void RollbackTransaction()
        {
            if ((tran != null))
            {
                tran.Rollback();
                tran = null;
            }
        }

        public void CommitTransaction()
        {
            if ((tran != null))
            {
                tran.Commit();
                tran = null;
            }
        }

        #endregion

        #region "Prepare"

        public void BeginPrepare(string sql)
        {
            try
            {
                OpenConnection();
                //comm = new SqlCommand(sql, conn, tran);
                comm.CommandText = sql;
                comm.Transaction = tran;
                comm.Prepare();
                setprepare = false;
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        public int ExecutePrepare(MsSqlParameter[] param)
        {
            int result = 0;
            try
            {
                ExtractParameterPrepare(param);
                setprepare = true;
                result = comm.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
            return result;
        }

        private void ExtractParameterPrepare(MsSqlParameter[] param)
        {
            if ((param != null))
            {
                SqlParameter parameter;
                if (!setprepare)
                {
                    foreach (var p in param)
                    {
                        if ((p != null))
                        {
                            parameter = new SqlParameter(p.Name, p.DbType);
                            parameter.Size = p.Size;
                            parameter.Precision = (byte)p.Precision;
                            parameter.Scale = (byte)p.Scale;

                            //.IsNullable = True
                            //.Direction = ParameterDirection.Output
                            //.Value = p.Values
                            //Select Case p.DbType
                            //    Case SqlDbType.Decimal
                            //        .Precision = 18
                            //        .Scale = 4
                            //End Select
                            comm.Parameters.Add(parameter);
                        }
                    }
                }

                int i = 0;
                foreach (var _param in comm.Parameters)
                {
                    ((SqlParameter)_param).Value = param[i].Values;
                    i = i + 1;
                }
            }
        }

        private void SqlCommandPrepareEx(string connectionString)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("", connection);

                // Create and prepare an SQL statement.
                command.CommandText = "INSERT INTO Region (RegionID, RegionDescription) " + "VALUES (@id, @desc)";
                SqlParameter idParam = new SqlParameter("@id", SqlDbType.Int, 0);
                SqlParameter descParam = new SqlParameter("@desc", SqlDbType.Decimal, 0);
                //New SqlParameter("@desc", SqlDbType.Text, 100)
                idParam.Value = 20;
                descParam.Value = "First Region";
                command.Parameters.Add(idParam);
                command.Parameters.Add(descParam);

                // Call Prepare after setting the Commandtext and Parameters.
                command.Prepare();
                command.ExecuteNonQuery();

                // Change parameter values and call ExecuteNonQuery.
                command.Parameters[0].Value = 21;
                command.Parameters[1].Value = "Second Region";
                command.ExecuteNonQuery();
            }
        }

        public void ClearPrepare()
        {
            comm.Parameters.Clear();
            setprepare = false;
        }

        #endregion

        #region "SqlBulkCopy"

        public void BeginBulkCopy()
        {
            bulkCopy = new SqlBulkCopy(conn);
            bulkCopy.BulkCopyTimeout = int.MaxValue;
        }

        public DataTable GetTableStructure(string tableName)
        {
            //DataTable tb = new DataTable( );
            //using (SqlConnection conn = new SqlConnection(destConnString))
            //{
            //    SqlDataAdapter dt = new SqlDataAdapter(" select top 1 * from [GSB_CREDITBUREAU].[dbo].[ld_guarantor_tmp] where 1!=2 ", conn);
            //    dt.Fill(tb);
            //}
            //return tb;

            DataTable table = new DataTable();
            string sql = "SELECT TOP 1 * FROM " + tableName + " WHERE 1!=2 ";
            try
            {
                OpenConnection();
                //comm = new SqlCommand(sql, conn, tran);
                comm.CommandText = sql;
                //comm.Transaction = tran;
                //this.ExtractParameter(param);

                SqlDataAdapter adapter = new SqlDataAdapter(comm);
                adapter.Fill(table);
                adapter.Dispose();
                adapter = null;
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
            return table;
        }

        public bool BulkCopyData(DataTable sourceData, string destinationTableName)
        {
            //using (SqlBulkCopy bulkCopy = new SqlBulkCopy(destConnString) { DestinationTableName = "ld_guarantor_tmp", BulkCopyTimeout = int.MaxValue })
            //{
            //    bulkCopy.WriteToServer(sourceData);
            //} 

            bool result = false;
            try
            {
                bulkCopy.DestinationTableName = destinationTableName;
                bulkCopy.WriteToServer(sourceData);
                result = true;
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }

            return result;
        }

        public void EndBulkCopy()
        {
            bulkCopy.Close();
            bulkCopy = null;
        }

        #endregion

        #region "Internal Method"

        private void ExtractParameter(MsSqlParameter[] param)
        {
            if ((param != null))
            {
                comm.Parameters.Clear();
                MsSqlParameter p = null;
                foreach (MsSqlParameter p_loopVariable in param)
                {
                    p = p_loopVariable;
                    if ((p != null))
                    {
                        comm.Parameters.Add(new SqlParameter(p.Name, p.Values));
                        if (p.Direction) //return output
                        {
                            //comm.Parameters.AddWithValue(p.Name, p.Values);
                            comm.Parameters[p.Name].Direction = ParameterDirection.Output;
                        }
                        //else
                        //{
                        //    comm.Parameters.Add(new SqlParameter(p.Name, p.Values));
                        //}
                    }
                }
                p = null;
            }
        }

        #endregion

        #endregion
    }
}
