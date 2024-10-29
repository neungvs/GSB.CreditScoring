using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/*To read data from excel file*/
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Configuration;
//using GSB.Class;
/*----------------------------*/


namespace GSB.UnitTest
{
    public class pkImportData
    {


        #region Protected Method (Import Master Data)

        //********* Import ST_SALECHANNEL ******************
        public void import_ST_SALECHANNEL()
        {
            //Remark
            // ST Log Table : ST_CHARBIN_Import_Tracking
            // Overall log table : S_LOG_IMPORT

            string manipTableName = "ST_SALECHANNEL";
            string logGroupID = "002";

            string tmestart;
            string tmesend;

            string queryImport = "";

            try
            {
                //Create connection string
                SqlConnection connInsert = new SqlConnection();
                connInsert.ConnectionString = ConfigurationManager.ConnectionStrings["GSBSQLServer"].ToString();
                connInsert.Open();
                SqlCommand cmdInsert = new SqlCommand();
                cmdInsert.Connection = connInsert;
                cmdInsert.CommandTimeout = 36000;
                cmdInsert.CommandType = CommandType.Text;

                //User ID
                string User_ID = "1";// ExportUtility.getCurrentUserId;

                //Declare file path
                string filePath = "";//ConfigurationManager.AppSettings["PKIMPORTPATH"].ToString();
                filePath = @"D:\Development\BOL\ST_SALECHANNEL.xls";

                //Read xls using OleDB
                string connString = "";

                connString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"", filePath);
                OleDbConnection conn = new OleDbConnection(connString);
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                string queryString = "select [FILE_DATE], [SALE_CD], [SALE_NAME],[SALE_DETAIL],[ACTIVE_FLAG] from [ST_SALECHANNEL$]";

                string FILE_DATE = "";
                string SALE_CD = "";
                string SALE_NAME = "";
                string SALE_DETAIL = "";
                string ACTIVE_FLAG = "";


                int intRowCount = 0;
                int intInsertRowCount = 0;
                int intUpdateRowCount = 0;
                int intErrorRowCount = 0;

                OleDbCommand cmd = new OleDbCommand(queryString, conn);
                OleDbDataAdapter da = new OleDbDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                SQLToDataTable conn2tb = new SQLToDataTable();
                int intExistRowCount = 0;
                string strInsertFlag = "I";


                //Check for Null or Blank on necessary field (RANGE_CD, RANGE_NAME)
                bool blDataNotCompleteInSomeColumn = false;
                intErrorRowCount = 1;
                foreach (DataRow drRow in dt.Rows)
                {
                    //Get file date
                    SALE_CD = drRow["SALE_CD"].ToString().Replace("\"", "").Trim();
                    SALE_NAME = drRow["SALE_NAME"].ToString().Replace("\"", "").Trim();
                    SALE_DETAIL = drRow["SALE_DETAIL"].ToString().Replace("\"", "").Trim();
                    ACTIVE_FLAG = drRow["ACTIVE_FLAG"].ToString().Replace("\"", "").Trim();

                    blDataNotCompleteInSomeColumn = true; //Got some necessary column is Null or Blank
                    if (!(SALE_CD == "" && SALE_NAME == "" && SALE_DETAIL == "" && ACTIVE_FLAG == ""))
                    {
                        blDataNotCompleteInSomeColumn = false;
                    }
                    //Throw exceptional error
                    if (blDataNotCompleteInSomeColumn)
                    {
                        intErrorRowCount++;
                        throw new Exception("Some field value (RANGE_CD, RANGE_NAME) is BLANK or NULL at Excel Row Number : " + intErrorRowCount.ToString() + ", Please verify the import file.");
                    }
                    //Define next row number
                    intErrorRowCount++;
                }


                //Reset variable value
                FILE_DATE = "";
                SALE_CD = "";
                SALE_NAME = "";
                SALE_DETAIL = "";
                ACTIVE_FLAG = "";

                intRowCount = 0;
                intInsertRowCount = 0;
                intUpdateRowCount = 0;
                intErrorRowCount = 0;


                tmestart = DateTime.Now.Year.ToString("0000") + DateTime.Now.ToString("-MM-dd HH:mm:ss.") + DateTime.Now.Millisecond.ToString();

                foreach (DataRow drRow in dt.Rows)
                {
                    intRowCount++;
                    try
                    {
                        //Get file date
                        FILE_DATE = drRow["FILE_DATE"].ToString().Replace("\"", "").Trim();
                        SALE_CD = drRow["SALE_CD"].ToString().Replace("\"", "").Trim();
                        SALE_NAME = drRow["SALE_NAME"].ToString().Replace("\"", "").Trim();
                        SALE_DETAIL = drRow["SALE_DETAIL"].ToString().Replace("\"", "").Trim();
                        ACTIVE_FLAG = drRow["ACTIVE_FLAG"].ToString().Replace("\"", "").Trim();

                        if (SALE_CD.Trim() != "")
                        {
                            //if (intRowCount == 1)
                            //{
                            //    //Check file date
                            //    string strDate = DateTime.Now.Year.ToString("0000") + DateTime.Now.ToString("MM01");
                            //    if (strDate.Substring(0, 6) != FILE_DATE.Substring(0, 6))
                            //    {
                            //        //Keep error log
                            //        queryImport = "insert into s_log_import(RESOURCES, START_DATE, END_DATE, TOTAL, STATUS, ERROR_MESSAGE, Group_ID) values('BATCH_MASTERs', getdate(),getdate(),0,'FAIL','Wrong file DATE : file date is " + FILE_DATE + ", Table : " + manipTableName + ", File :" + filePath + " ','" + logGroupID + "');";
                            //        cmdInsert.CommandText = queryImport;
                            //        cmdInsert.ExecuteNonQuery();

                            //        //Close all connection
                            //        conn.Close();
                            //        conn.Dispose();
                            //        connInsert.Close();
                            //        connInsert.Dispose();

                            //        //Exit method
                            //        return;
                            //    }
                            //}

                            DataTable dtST = new System.Data.DataTable();
                            dtST = conn2tb.ExcuteSQL("select count(*) from " + manipTableName + " where RANGE_CD = '" + SALE_CD + "' and RANGE_NAME = '" + SALE_NAME + "'");
                            intExistRowCount = int.Parse(dtST.Rows[0][0].ToString());
                            if (intExistRowCount == 0)
                            {
                                //Insert new record
                                strInsertFlag = "I";
                                queryImport = " insert into ST_SALECHANNEL " +
                                                " (" +
                                                " SALE_CD, SALE_NAME, SALE_DETAIL, ACTIVE_FLAG " +
                                                " )" +
                                                " values " +
                                                " ( " +
                                                " '" + SALE_CD + "'," +
                                                " '" + SALE_NAME + "'," +
                                                " '" + SALE_DETAIL + "'," +
                                                " " + ACTIVE_FLAG +
                                                " ); ";

                                cmdInsert.CommandText = queryImport;
                                cmdInsert.ExecuteNonQuery();

                                //Count inserted record
                                intInsertRowCount++;

                            }
                            else
                            {
                                //Update existing record
                                strInsertFlag = "U";
                                queryImport = " update ST_SALECHANNEL " +
                                                " set " +
                                                " SALE_CD = '" + SALE_CD + "' " +
                                                " SALE_NAME = '" + SALE_NAME + "' " +
                                                " ACTIVE_FLAG = " + ACTIVE_FLAG + " " +
                                                " where SALE_CD = '" + SALE_CD + "' and SALE_NAME = '" + SALE_NAME + "'";
                                cmdInsert.CommandText = queryImport;
                                cmdInsert.ExecuteNonQuery();

                                //Count updated record
                                intUpdateRowCount++;
                            }

                            ////Insert into ST_SCORERANGE_Import_Tracking
                            //queryImport =
                            //                " insert into ST_SCORERNGLST_Import_Tracking " +
                            //                " (" +
                            //                " RANGE_CD, " +
                            //                " RANGE_NAME, " +
                            //                " IMP_DTM, " +
                            //                " IMP_STATUS, " +
                            //                " IMP_Error_Result, " +
                            //                " USER_ID " +
                            //                " )" +
                            //                " values " +
                            //                " ( " +
                            //                " '" + SALE_CD + "', " +
                            //                " '" + SALE_NAME + "', " +
                            //                " getdate(), " +
                            //                " '" + strInsertFlag + "', " +
                            //                " 'Insert / Update Success', " +
                            //                " '" + User_ID + "' " +
                            //                " ); ";

                            //cmdInsert.CommandText = queryImport;
                            //cmdInsert.ExecuteNonQuery();

                        }//---End of key feild checking

                    }
                    catch (Exception ex)
                    {
                        //string strExceptional = ex.Message.ToString();
                        //strInsertFlag = "E";
                        ////Insert into ST_BRANCH_Import_Tracking
                        //queryImport = " insert into ST_SCORERNGLST_Import_Tracking " +
                        //                " (" +
                        //                " RANGE_CD, " +
                        //                " RANGE_NAME, " +
                        //                " IMP_DTM, " +
                        //                " IMP_STATUS, " +
                        //                " IMP_Error_Result, " +
                        //                " USER_ID " +
                        //                " )" +
                        //                " values " +
                        //                " ( " +
                        //                " '" + SALE_CD + "', " +
                        //                " '" + SALE_NAME + "', " +
                        //                " getdate(), " +
                        //                " '" + strInsertFlag + "', " +
                        //                " '" + strExceptional + "', " +
                        //                " '" + User_ID + "' " +
                        //                " ) ";

                        //cmdInsert.CommandText = queryImport;
                        //cmdInsert.ExecuteNonQuery();

                        //Count error record
                        intErrorRowCount++;

                    }

                }

                //Keep Log


                tmesend = DateTime.Now.Year.ToString("0000") + DateTime.Now.ToString("-MM-dd HH:mm:ss.") + DateTime.Now.Millisecond.ToString();
                string strStatus = "DONE";
                if (intErrorRowCount > 0) strStatus = "FAIL";
                queryImport = "insert into s_log_import(RESOURCES, START_DATE, END_DATE, TOTAL, STATUS, ERROR_MESSAGE, Group_ID) values('BATCH_MASTERs', convert(datetime,'" + tmestart.ToString() + "',121), convert(datetime,'" + tmesend.ToString() + "',121), " + (intInsertRowCount + intUpdateRowCount).ToString() + ", '" + strStatus + "', '" + strStatus + " : " + manipTableName + " : Total inserted rows = " + intInsertRowCount.ToString() + ", Total updated rows = " + intUpdateRowCount.ToString() + ", Total Error rows = " + intErrorRowCount.ToString() + "', '" + logGroupID + "');";
                cmdInsert.CommandText = queryImport;
                cmdInsert.ExecuteNonQuery();

                //Close connection
                conn.Close();
                conn.Dispose();
                connInsert.Close();
                connInsert.Dispose();

            }
            catch (Exception exc)
            {

                SqlConnection connInsert = new SqlConnection();
                connInsert.ConnectionString = ConfigurationManager.ConnectionStrings["GSBSQLServer"].ToString();
                connInsert.Open();
                SqlCommand cmdInsert = new SqlCommand();
                cmdInsert.Connection = connInsert;
                cmdInsert.CommandTimeout = 36000;
                cmdInsert.CommandType = CommandType.Text;

                //Keep Log
                tmestart = DateTime.Now.Year.ToString("0000") + DateTime.Now.ToString("-MM-dd HH:mm:ss.") + DateTime.Now.Millisecond.ToString();
                tmesend = DateTime.Now.Year.ToString("0000") + DateTime.Now.ToString("-MM-dd HH:mm:ss.") + DateTime.Now.Millisecond.ToString();
                queryImport = "insert into s_log_import(RESOURCES, START_DATE, END_DATE, TOTAL, STATUS, ERROR_MESSAGE, Group_ID) values('BATCH_MASTERs', convert(datetime,'" + tmestart.ToString() + "',121), convert(datetime,'" + tmesend.ToString() + "',121), 0, 'FAIL', 'FAIL : " + manipTableName + " : Error = " + exc.Message.ToString().Replace("'", "''") + "', '" + logGroupID + "');";
                cmdInsert.CommandText = queryImport;
                cmdInsert.ExecuteNonQuery();
                connInsert.Close();
                connInsert.Dispose();

                return;
            }

        }


        #endregion Protected Method (Import Master Data)

    }
}