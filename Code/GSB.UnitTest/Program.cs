using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Configuration;

using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;

namespace GSB.UnitTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //pkImportData import = new pkImportData();
            //import.import_ST_SALECHANNEL();

            string filename = @"D:\Development\BOL\BOL-Documnet\ST_SALECHANNEL.xls";
            ImportSaleChannel(filename);
            Console.WriteLine("Please any key.");
            Console.ReadKey();
        }

        public static bool ImportSaleChannel(string filename)
        {
            bool result = false;
            string tmestart;
            string tmesend;
            string sqlstr;
            int insertdata = 0;
            int totaldata = 0;

            string tablename = "ST_SALECHANNEL";
            string strStatus = "FAIL";
            string logGroupID = "001";

            String constr = ConfigurationManager.ConnectionStrings["GSBSQLServer"].ToString();
            SQLToDataTable dbaccess = new SQLToDataTable();
            dbaccess.Open();

            tmestart = DateTime.Now.Year.ToString("0000") + DateTime.Now.ToString("-MM-dd HH:mm:ss.") + DateTime.Now.Millisecond.ToString();
            try
            {
                System.Data.OleDb.OleDbConnection conn;
                System.Data.DataSet dtSet;
                System.Data.OleDb.OleDbDataAdapter comm;
                conn = new System.Data.OleDb.OleDbConnection("provider=Microsoft.Jet.OLEDB.4.0;Data Source='" + filename + "';Extended Properties=Excel 8.0;");
                comm = new System.Data.OleDb.OleDbDataAdapter("select * from [ST_SALECHANNEL$]", conn);
                comm.TableMappings.Add("Table", tablename);
                dtSet = new System.Data.DataSet();
                comm.Fill(dtSet);

                DataTable dt = dtSet.Tables[0];
                DataTable dtdata = new DataTable();

                dtdata = dbaccess.ExcuteSQL("select top 1 * from ST_SALECHANNEL where 1=2");
                totaldata = dt.Rows.Count;

                if (dt.Rows.Count > 0)
                {
                    DataRow row = null;
                    foreach (DataRow r in dt.Rows)
                    {
                        //Console.WriteLine("{0},{1},{2},{3}", r["SALE_CD"], r["SALE_NAME"], r[2], r[3]);
                        //dtdata.ImportRow(r);
                        if (r[0].ToString().Trim().Length > 0 &&
                            r[1].ToString().Trim().Length > 0 &&
                            //r[2].ToString().Trim().Length > 0 &&
                            r[3].ToString().Trim().Length > 0
                            )
                        {
                            row = dtdata.NewRow();
                            row["SALE_CD"] = r[0];
                            row["SALE_NAME"] = r[1];
                            row["SALE_DETAIL"] = r[2];
                            row["ACTIVE_FLAG"] = r[3];
                            
                            dtdata.Rows.Add(row); 
                        }
                    }                                        
                }

                if (dtdata.Rows.Count > 0)
                {
                    dbaccess.ExecuteNonQuery("truncate table ST_SALECHANNEL");
                    //foreach (DataRow r in dtdata.Rows)
                    //{
                    //    Console.WriteLine("{0},{1},{2},{3}", r["SALE_CD"], r["SALE_NAME"], r[2], r[3]);
                    //}

                    if (dbaccess.BulkUpload(constr, "ST_SALECHANNEL", dtdata))
                    {
                        insertdata = dtdata.Rows.Count;
                        strStatus = "DONE";
                        logGroupID = "002";
                    }                   
                }
                conn.Close();

                tmesend = DateTime.Now.Year.ToString("0000") + DateTime.Now.ToString("-MM-dd HH:mm:ss.") + DateTime.Now.Millisecond.ToString();
                sqlstr = "insert into s_log_import(RESOURCES, START_DATE, END_DATE, TOTAL, STATUS, ERROR_MESSAGE, Group_ID) values('ST_SALECHANNEL', convert(datetime,'" +
                    tmestart.ToString() + "',121), convert(datetime,'" +
                    tmesend.ToString() + "',121), " +
                    totaldata + ", '" + strStatus + "', '" +
                    strStatus + " : " + tablename + " : Total inserted rows = " +
                    insertdata + "', '" + logGroupID + "');";
                dbaccess.ExecuteNonQuery(sqlstr);
                result = true;

            }
            catch (Exception ex)
            {
                tmesend = DateTime.Now.Year.ToString("0000") + DateTime.Now.ToString("-MM-dd HH:mm:ss.") + DateTime.Now.Millisecond.ToString();
                sqlstr = "insert into s_log_import(RESOURCES, START_DATE, END_DATE, TOTAL, STATUS, ERROR_MESSAGE, Group_ID) values('ST_SALECHANNEL', convert(datetime,'" +
                    tmestart.ToString() + "',121), convert(datetime,'" +
                    tmesend.ToString() + "',121), " +
                    totaldata + ", '" + strStatus + "','" +
                    ex.Message.Replace("'","") + "', '" + logGroupID + "');";
                dbaccess.ExecuteNonQuery(sqlstr);
                //throw;
            }
            dbaccess.CloseConnection();

            return result;
        }

        public static void BulkInsert<T>(string connection, string tableName, IList<T> list)
        {
            using (var bulkCopy = new SqlBulkCopy(connection))
            {
                bulkCopy.BatchSize = list.Count;
                bulkCopy.DestinationTableName = tableName;

                var table = new DataTable();
                var props = TypeDescriptor.GetProperties(typeof(T))
                    //Dirty hack to make sure we only have system data types 
                    //i.e. filter out the relationships/collections
                                           .Cast<PropertyDescriptor>()
                                           .Where(propertyInfo => propertyInfo.PropertyType.Namespace.Equals("System"))
                                           .ToArray();

                foreach (var propertyInfo in props)
                {
                    bulkCopy.ColumnMappings.Add(propertyInfo.Name, propertyInfo.Name);
                    table.Columns.Add(propertyInfo.Name, Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType);
                }

                var values = new object[props.Length];
                foreach (var item in list)
                {
                    for (var i = 0; i < values.Length; i++)
                    {
                        values[i] = props[i].GetValue(item);
                    }

                    table.Rows.Add(values);
                }

                bulkCopy.WriteToServer(table);
            }
        }

        private static void BulkUpload(string constr, string tablename, DataTable dt)
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
        }
    
    }
}
