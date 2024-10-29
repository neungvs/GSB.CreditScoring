using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/*To read data from excel file*/
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Configuration;
using GSB.Class;
/*----------------------------*/


namespace GSB.LoanSystem
{
    public class pkImportData
    {


        #region Protected Method (Import Master Data)

        //********* Import ST_CHARACTER ******************
        public void import_ST_CHARACTER()
        {
            //Remark
            // ST Log Table : ST_CHARACTER_Import_Tracking
            // Overall log table : S_LOG_IMPORT

            string manipTableName = "ST_CHARACTER";
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
                string User_ID = ExportUtility.getCurrentUserId;

                //Declare file path
                string filePath = ConfigurationManager.AppSettings["PKIMPORTPATH"].ToString();
                filePath = filePath + "ST_CHARACTER.xls";

                //Read xls using OleDB
                string connString = "";

                connString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"", filePath);
                OleDbConnection conn = new OleDbConnection(connString);
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                string queryString = "select [FILE_DATE], [CHAR_ID], [CHAR_CD], [MODEL_CD], [MODELVER_CD], [CHAR_NAME], [MCHAR_CD], [MCHAR_NAME],[ACTIVE_FLAG] "
                                    + " from [ST_CHARACTER$] ";

                string FILE_DATE = "";
                string CHAR_ID = "";
                string CHAR_CD = "";
                string MODEL_CD = "";
                string MODELVER_CD = "";
                string CHAR_NAME = "";
                string MCHAR_CD = "";
                string MCHAR_NAME = "";
                string ACTIVE_FLAG = "";

                int intRowCount = 0;
                int intInsertRowCount = 0;
                int intUpdateRowCount = 0;
                int intErrorRowCount = 0;

                queryImport = "";

                OleDbCommand cmd = new OleDbCommand(queryString, conn);
                OleDbDataAdapter da = new OleDbDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);


                //Check blank or null value for CHAR_ID, CHAR_CD, MODEL_CD, MODELVER_CD, CHAR_NAME, ACTIVE_FLAG
                bool blDataNotCompleteInSomeColumn = false;
                intErrorRowCount = 1;
                foreach (DataRow drRow in dt.Rows)
                {
                    //Get file date                    
                    CHAR_ID = drRow["CHAR_ID"].ToString().Replace("\"", "").Trim();
                    CHAR_CD = drRow["CHAR_CD"].ToString().Replace("\"", "").Trim();
                    MODEL_CD = drRow["MODEL_CD"].ToString().Replace("\"", "").Trim();
                    MODELVER_CD = drRow["MODELVER_CD"].ToString().Replace("\"", "").Trim();
                    CHAR_NAME = drRow["CHAR_NAME"].ToString().Replace("\"", "").Trim();
                    ACTIVE_FLAG = drRow["ACTIVE_FLAG"].ToString().Replace("\"", "").Trim();

                    if (!(CHAR_ID == "" && CHAR_CD == "" && MODEL_CD == "" && MODELVER_CD == "" && CHAR_NAME == "" && ACTIVE_FLAG == ""))
                    {
                        if (CHAR_ID == "" || CHAR_CD == "" || MODEL_CD == "" || MODELVER_CD == "" || CHAR_NAME == "" || ACTIVE_FLAG == "")
                        {
                            blDataNotCompleteInSomeColumn = true; //Got some necessary column is Null or Blank
                        }
                    }
                    //Throw exceptional error
                    if (blDataNotCompleteInSomeColumn)
                    {
                        intErrorRowCount++;
                        throw new Exception("Some field value (CHAR_ID, CHAR_CD, MODEL_CD, MODELVER_CD, CHAR_NAME, ACTIVE_FLAG) is BLANK or NULL at Excel Row Number : " + intErrorRowCount.ToString() + ", Please verify the import file.");
                    }
                    //Define next row number
                    intErrorRowCount++;
                }


                //Reset variable value
                FILE_DATE = "";
                CHAR_ID = "";
                CHAR_CD = "";
                MODEL_CD = "";
                MODELVER_CD = "";
                CHAR_NAME = "";
                MCHAR_CD = "";
                MCHAR_NAME = "";
                ACTIVE_FLAG = "";

                intRowCount = 0;
                intInsertRowCount = 0;
                intUpdateRowCount = 0;
                intErrorRowCount = 0;


                SQLToDataTable conn2tb = new SQLToDataTable();
                int intExistRowCount = 0;
                string strInsertFlag = "I";

                tmestart = DateTime.Now.Year.ToString("0000") + DateTime.Now.ToString("-MM-dd HH:mm:ss.") + DateTime.Now.Millisecond.ToString();

                foreach (DataRow drRow in dt.Rows)
                {
                    intRowCount++;
                    try
                    {
                        //Get file date
                        FILE_DATE = drRow["FILE_DATE"].ToString().Replace("\"", "").Trim();
                        CHAR_ID = drRow["CHAR_ID"].ToString().Replace("\"", "").Trim();
                        CHAR_CD = drRow["CHAR_CD"].ToString().Replace("\"", "").Trim();
                        MODEL_CD = drRow["MODEL_CD"].ToString().Replace("\"", "").Trim();
                        MODELVER_CD = drRow["MODELVER_CD"].ToString().Replace("\"", "").Trim();
                        CHAR_NAME = drRow["CHAR_NAME"].ToString().Replace("\"", "").Trim();
                        MCHAR_CD = drRow["MCHAR_CD"].ToString().Replace("\"", "").Trim();
                        MCHAR_NAME = drRow["MCHAR_NAME"].ToString().Replace("\"", "").Trim();
                        ACTIVE_FLAG = drRow["ACTIVE_FLAG"].ToString().Replace("\"", "").Trim();

                        if (CHAR_ID.Trim() != "") //Check for Key field in table
                        {
                            //Check file date
                            if (intRowCount == 1)
                            {
                                //Check file date
                                string strDate = DateTime.Now.Year.ToString("0000") + DateTime.Now.ToString("MM01");
                                if (strDate.Substring(0, 6) != FILE_DATE.Substring(0, 6))
                                {
                                    //Keep error log
                                    queryImport = "insert into s_log_import(RESOURCES, START_DATE, END_DATE, TOTAL, STATUS, ERROR_MESSAGE, Group_ID) values('BATCH_MASTERs', getdate(),getdate(),0,'FAIL','Wrong file DATE : file date is " + FILE_DATE + ", Table : " + manipTableName + ", File :" + filePath + " ','" + logGroupID + "');";
                                    cmdInsert.CommandText = queryImport;
                                    cmdInsert.ExecuteNonQuery();

                                    //Close all connection
                                    conn.Close();
                                    conn.Dispose();
                                    connInsert.Close();
                                    connInsert.Dispose();

                                    //Exit method
                                    return;
                                }
                            }

                            DataTable dtST = new System.Data.DataTable();
                            dtST = conn2tb.ExcuteSQL("select count(*) from ST_CHARACTER where CHAR_ID = '" + CHAR_ID + "'");
                            intExistRowCount = int.Parse(dtST.Rows[0][0].ToString());
                            if (intExistRowCount == 0)
                            {
                                //Insert new brance
                                strInsertFlag = "I";
                                //queryImport = " insert into ST_CHARACTER " +
                                //                " (" +
                                //                " CHAR_ID, " +
                                //                " CHAR_CD, " +
                                //                " MODEL_CD, " +
                                //                " MODELVER_CD, " +
                                //                " CHAR_NAME, " +
                                //                " MCHAR_CD, " +
                                //                " MCHAR_NAME, " +
                                //                " ACTIVE_FLAG" +
                                //                " )" +
                                //                " values " +
                                //                " ( " +
                                //                " '" + CHAR_ID + "', " +
                                //                " '" + CHAR_CD + "', " +
                                //                " '" + MODEL_CD + "', " +
                                //                " '" + MODELVER_CD + "', " +
                                //                " '" + CHAR_NAME + "', " +
                                //                " '" + MCHAR_CD + "', " +
                                //                " '" + MCHAR_NAME + "', " +
                                //                " '" + ACTIVE_FLAG + "' " +
                                //                " ); ";

                                queryImport = " insert into ST_CHARACTER (CHAR_ID,CHAR_CD,MODEL_CD,MODELVER_CD,CHAR_NAME,MCHAR_CD,MCHAR_NAME,ACTIVE_FLAG) values ('"+CHAR_ID+"','"+CHAR_CD+"','"+MODEL_CD+"','"+MODELVER_CD+"','"+CHAR_NAME+"','"+MCHAR_CD+"','"+MCHAR_NAME+"','"+ACTIVE_FLAG+"')";                                  
                               
                                cmdInsert.CommandText = queryImport;
                                cmdInsert.ExecuteNonQuery();

                                //Count inserted record
                                intInsertRowCount++;

                            }
                            else
                            {
                                //Update existing brance
                                strInsertFlag = "U";
                                queryImport = " update ST_CHARACTER " +
                                                " set " +
                                                " CHAR_CD = '" + CHAR_CD + "', " +
                                                " MODEL_CD = '" + MODEL_CD + "', " +
                                                " MODELVER_CD = '" + MODELVER_CD + "', " +
                                                " CHAR_NAME = '" + CHAR_NAME + "', " +
                                                " MCHAR_CD = '" + MCHAR_CD + "', " +
                                                " MCHAR_NAME = '" + MCHAR_NAME + "', " +
                                                " ACTIVE_FLAG = '" + ACTIVE_FLAG + "' " +
                                                " where CHAR_ID = '" + CHAR_ID + "'; ";
                                cmdInsert.CommandText = queryImport;
                                cmdInsert.ExecuteNonQuery();

                                //Count updated record
                                intUpdateRowCount++;
                            }

                            //Insert into ST_CHARACTER_Import_Tracking
                            queryImport =
                                            " insert into ST_CHARACTER_Import_Tracking " +
                                            " (" +
                                            " CHAR_ID, " +
                                            " CHAR_CD, " +
                                            " MODEL_CD, " +
                                            " MODELVER_CD, " +
                                            " CHAR_NAME, " +
                                            " MCHAR_CD, " +
                                            " MCHAR_NAME, " +
                                            " ACTIVE_FLAG, " +
                                            " IMP_DTM, " +
                                            " IMP_STATUS, " +
                                            " IMP_Error_Result, " +
                                            " USER_ID " +
                                            " )" +
                                            " values " +
                                            " ( " +
                                            " '" + CHAR_ID + "', " +
                                            " '" + CHAR_CD + "', " +
                                            " '" + MODEL_CD + "', " +
                                            " '" + MODELVER_CD + "', " +
                                            " '" + CHAR_NAME + "', " +
                                            " '" + MCHAR_CD + "', " +
                                            " '" + MCHAR_NAME + "', " +
                                            " '" + ACTIVE_FLAG + "', " +
                                            " getdate(), " +
                                            " '" + strInsertFlag + "', " +
                                            " 'Insert / Update Success', " +
                                            " '" + User_ID + "' " +
                                            " ); ";

                            cmdInsert.CommandText = queryImport;
                            cmdInsert.ExecuteNonQuery();

                        }//End of check for space
                       
                    }
                    catch (Exception ex)
                    {
                     
                        string strExceptional = ex.Message.ToString();

                        strInsertFlag = "E";

                        //Insert into ST_CHARACTER_Import_Tracking
                        queryImport = " insert into ST_CHARACTER_Import_Tracking " +
                                        " (" +
                                        " CHAR_ID, " +
                                        " CHAR_CD, " +
                                        " MODEL_CD, " +
                                        " MODELVER_CD, " +
                                        " CHAR_NAME, " +
                                        " MCHAR_CD, " +
                                        " MCHAR_NAME, " +
                                        " ACTIVE_FLAG, " +
                                        " IMP_DTM, " +
                                        " IMP_STATUS, " +
                                        " IMP_Error_Result, " +
                                        " USER_ID " +
                                        " )" +
                                        " values " +
                                        " ( " +
                                        " '" + CHAR_ID + "', " +
                                        " '" + CHAR_CD + "', " +
                                        " '" + MODEL_CD + "', " +
                                        " '" + MODELVER_CD + "', " +
                                        " '" + CHAR_NAME + "', " +
                                        " '" + MCHAR_CD + "', " +
                                        " '" + MCHAR_NAME + "', " +
                                        " '" + ACTIVE_FLAG + "', " +
                                        " getdate(), " +
                                        " '" + strInsertFlag + "', " +
                                        " '" + strExceptional + "', " +
                                        " '" + User_ID + "' " +
                                        " ) ";

                        cmdInsert.CommandText = queryImport;
                        cmdInsert.ExecuteNonQuery();

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

        //********* Import ST_CHARBIN ******************
        public void import_ST_CHARBIN()
        {
            //Remark
            // ST Log Table : ST_CHARBIN_Import_Tracking
            // Overall log table : S_LOG_IMPORT

            string manipTableName = "ST_CHARBIN";
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
                string User_ID = ExportUtility.getCurrentUserId;

                //Declare file path
                string filePath = ConfigurationManager.AppSettings["PKIMPORTPATH"].ToString();
                filePath = filePath + "ST_CHARBIN.xls";

                //Read xls using OleDB
                string connString = "";

                connString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"", filePath);
                OleDbConnection conn = new OleDbConnection(connString);
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                string queryString = "select [FILE_DATE], [MODEL_CD], [CHAR_NAME], [BIN], [CHAR_DESC], [CHAR_SCR] from [ST_CHARBIN$]";

                string FILE_DATE = "";
                string MODEL_CD = "";
                string CHAR_NAME = "";
                string BIN = "";
                string CHAR_DESC = "";
                string CHAR_SCR = "";

                int intRowCount = 0;
                int intInsertRowCount = 0;
                int intUpdateRowCount = 0;
                int intErrorRowCount = 0;                

                OleDbCommand cmd = new OleDbCommand(queryString, conn);
                OleDbDataAdapter da = new OleDbDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);


                //Check for Null or Blank on necessary field (MODEL_CD, CHAR_NAME, BIN, CHAR_DESC, CHAR_NAME, CHAR_SCR)
                bool blDataNotCompleteInSomeColumn = false;
                intErrorRowCount = 1;
                foreach (DataRow drRow in dt.Rows)
                {
                    //Get file date                                        
                    MODEL_CD = drRow["MODEL_CD"].ToString().Replace("\"", "").Trim();
                    CHAR_NAME = drRow["CHAR_NAME"].ToString().Replace("\"", "").Trim();
                    BIN = drRow["BIN"].ToString().Replace("\"", "").Trim();
                    CHAR_DESC = drRow["CHAR_DESC"].ToString().Replace("\"", "").Trim();
                    CHAR_SCR = drRow["CHAR_SCR"].ToString().Replace("\"", "").Trim();

                    if (!(MODEL_CD == "" && CHAR_NAME == "" && BIN == "" && CHAR_DESC == "" && CHAR_NAME == "" && CHAR_SCR == ""))
                    {
                        if (MODEL_CD == "" || CHAR_NAME == "" || BIN == "" || CHAR_DESC == "" || CHAR_NAME == "" || CHAR_SCR == "")
                        {
                            blDataNotCompleteInSomeColumn = true; //Got some necessary column is Null or Blank
                        }
                    }
                    //Throw exceptional error
                    if (blDataNotCompleteInSomeColumn)
                    {
                        intErrorRowCount++;
                        throw new Exception("Some field value (MODEL_CD, CHAR_NAME, BIN, CHAR_DESC, CHAR_NAME, CHAR_SCR) is BLANK or NULL at Excel Row Number : " + intErrorRowCount.ToString() + ", Please verify the import file.");
                    }
                    //Define next row number
                    intErrorRowCount++;
                }


                //Reset variable value
                FILE_DATE = "";
                MODEL_CD = "";
                CHAR_NAME = "";
                BIN = "";
                CHAR_DESC = "";
                CHAR_SCR = "";

                intRowCount = 0;
                intInsertRowCount = 0;
                intUpdateRowCount = 0;
                intErrorRowCount = 0;                



                SQLToDataTable conn2tb = new SQLToDataTable();
                int intExistRowCount = 0;
                string strInsertFlag = "I";

                tmestart = DateTime.Now.Year.ToString("0000") + DateTime.Now.ToString("-MM-dd HH:mm:ss.") + DateTime.Now.Millisecond.ToString();

                foreach (DataRow drRow in dt.Rows)
                {
                    intRowCount++;
                    try
                    {
                        //Get file date
                        FILE_DATE = drRow["FILE_DATE"].ToString().Replace("\"", "").Trim();
                        MODEL_CD = drRow["MODEL_CD"].ToString().Replace("\"", "").Trim();
                        CHAR_NAME = drRow["CHAR_NAME"].ToString().Replace("\"", "").Trim();
                        BIN = drRow["BIN"].ToString().Replace("\"", "").Trim();
                        CHAR_DESC = drRow["CHAR_DESC"].ToString().Replace("\"", "").Trim();
                        CHAR_SCR = drRow["CHAR_SCR"].ToString().Replace("\"", "").Trim();

                        if (MODEL_CD.Trim() != "" && CHAR_NAME.Trim() != "" && BIN.Trim() != "")
                        {
                            if (intRowCount == 1)
                            {
                                //Check file date
                                string strDate = DateTime.Now.Year.ToString("0000") + DateTime.Now.ToString("MM01");
                                if (strDate.Substring(0, 6) != FILE_DATE.Substring(0, 6))
                                {
                                    //Keep error log
                                    queryImport = "insert into s_log_import(RESOURCES, START_DATE, END_DATE, TOTAL, STATUS, ERROR_MESSAGE, Group_ID) values('BATCH_MASTERs', getdate(),getdate(),0,'FAIL','Wrong file DATE : file date is " + FILE_DATE + ", Table : " + manipTableName + ", File :" + filePath + " ','" + logGroupID + "');";
                                    cmdInsert.CommandText = queryImport;
                                    cmdInsert.ExecuteNonQuery();

                                    //Close all connection
                                    conn.Close();
                                    conn.Dispose();
                                    connInsert.Close();
                                    connInsert.Dispose();

                                    //Exit method
                                    return;
                                }
                            }

                            DataTable dtST = new System.Data.DataTable();
                            dtST = conn2tb.ExcuteSQL("select count(*) from ST_CHARBIN where MODEL_CD = '" + MODEL_CD + "' and CHAR_NAME = '" + CHAR_NAME.Trim() + "' and BIN = '" + BIN.Trim() + "'");
                            intExistRowCount = int.Parse(dtST.Rows[0][0].ToString());
                            if (intExistRowCount == 0)
                            {
                                //Insert new record
                                strInsertFlag = "I";
                                queryImport = " insert into ST_CHARBIN " +
                                                " (" +
                                                " MODEL_CD, " +
                                                " CHAR_NAME, " +
                                                " BIN, " +
                                                " CHAR_DESC, " +
                                                " CHAR_SCR " +
                                                " )" +
                                                " values " +
                                                " ( " +
                                                " '" + MODEL_CD + "', " +
                                                " '" + CHAR_NAME + "', " +
                                                " '" + BIN + "', " +
                                                " '" + CHAR_DESC + "', " +
                                                " '" + CHAR_SCR + "' " +
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
                                queryImport = " update ST_CHARBIN " +
                                                " set " +
                                                " CHAR_DESC = '" + CHAR_DESC + "', " +
                                                " CHAR_SCR = '" + CHAR_SCR + "' " +
                                                " where MODEL_CD = '" + MODEL_CD + "' and CHAR_NAME = '" + CHAR_NAME.Trim() + "' and BIN = '" + BIN.Trim() + "'";
                                cmdInsert.CommandText = queryImport;
                                cmdInsert.ExecuteNonQuery();

                                //Count updated record
                                intUpdateRowCount++;
                            }

                            //Insert into ST_CHARBIN_Import_Tracking
                            queryImport =
                                            " insert into ST_CHARBIN_Import_Tracking " +
                                            " (" +
                                            " MODEL_CD, " +
                                            " CHAR_NAME, " +
                                            " BIN, " +
                                            " CHAR_DESC, " +
                                            " CHAR_SCR, " +
                                            " IMP_DTM, " +
                                            " IMP_STATUS, " +
                                            " IMP_Error_Result, " +
                                            " USER_ID " +
                                            " )" +
                                            " values " +
                                            " ( " +
                                            " '" + MODEL_CD + "', " +
                                            " '" + CHAR_NAME + "', " +
                                            " '" + BIN + "', " +
                                            " '" + CHAR_DESC + "', " +
                                            " '" + CHAR_SCR + "', " +
                                            " getdate(), " +
                                            " '" + strInsertFlag + "', " +
                                            " 'Insert / Update Success', " +
                                            " '" + User_ID + "' " +
                                            " ); ";

                            cmdInsert.CommandText = queryImport;
                            cmdInsert.ExecuteNonQuery();

                        }//---End of key feild checking
                        
                        ////-- Modify by Pee for Model_CD is null
                        //else
                        //{
                        //    strInsertFlag = "E";
                        //    //Insert into ST_BRANCH_Import_Tracking
                        //    queryImport = " insert into ST_CHARBIN_Import_Tracking " +
                        //                    " (" +
                        //                    " MODEL_CD, " +
                        //                    " CHAR_NAME, " +
                        //                    " BIN, " +
                        //                    " CHAR_DESC, " +
                        //                    " CHAR_SCR, " +
                        //                    " IMP_DTM, " +
                        //                    " IMP_STATUS, " +
                        //                    " IMP_Error_Result, " +
                        //                    " USER_ID " +
                        //                    " )" +
                        //                    " values " +
                        //                    " ( " +
                        //                    " '" + MODEL_CD + "', " +
                        //                    " '" + CHAR_NAME + "', " +
                        //                    " '" + BIN + "', " +
                        //                    " '" + CHAR_DESC + "', " +
                        //                    " '" + CHAR_SCR + "', " +
                        //                    " getdate(), " +
                        //                    " '" + strInsertFlag + "', " +
                        //                    " '" + "Model_CD is Null" + "', " +
                        //                    " '" + User_ID + "' " +
                        //                    " ) ";

                        //    cmdInsert.CommandText = queryImport;
                        //    cmdInsert.ExecuteNonQuery();

                        //    //Count error record
                        //    intErrorRowCount++;
                        //}
                    }
                    catch (Exception ex)
                    {
                        string strExceptional = ex.Message.ToString();
                        strInsertFlag = "E";
                        //Insert into ST_BRANCH_Import_Tracking
                        queryImport = " insert into ST_CHARBIN_Import_Tracking " +
                                        " (" +
                                        " MODEL_CD, " +
                                        " CHAR_NAME, " +
                                        " BIN, " +
                                        " CHAR_DESC, " +
                                        " CHAR_SCR, " +
                                        " IMP_DTM, " +
                                        " IMP_STATUS, " +
                                        " IMP_Error_Result, " +
                                        " USER_ID " +
                                        " )" +
                                        " values " +
                                        " ( " +
                                        " '" + MODEL_CD + "', " +
                                        " '" + CHAR_NAME + "', " +
                                        " '" + BIN + "', " +
                                        " '" + CHAR_DESC + "', " +
                                        " '" + CHAR_SCR + "', " +
                                        " getdate(), " +
                                        " '" + strInsertFlag + "', " +
                                        " '" + strExceptional + "', " +
                                        " '" + User_ID + "' " +
                                        " ) ";

                        cmdInsert.CommandText = queryImport;
                        cmdInsert.ExecuteNonQuery();

                        //Count error record
                        intErrorRowCount++;

                    }

                }

                //Keep Log
                ////-- Modify by Pee for count error 
                //if (intErrorRowCount == 0)

                //    intErrorRowCount = 0;

                //else
                //    //-- if last reccord not count to error
                //    intErrorRowCount = intErrorRowCount - 1;
                ////--

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

        //********* Import ST_SCORERANGE ******************
        public void import_ST_SCORERANGE()
        {
            //Remark
            // ST Log Table : ST_CHARBIN_Import_Tracking
            // Overall log table : S_LOG_IMPORT

            string manipTableName = "ST_SCORERANGE";
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
                string User_ID = ExportUtility.getCurrentUserId;

                //Declare file path
                string filePath = ConfigurationManager.AppSettings["PKIMPORTPATH"].ToString();
                filePath = filePath + "ST_SCORERANGE.xls";

                //Read xls using OleDB
                string connString = "";

                connString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"", filePath);
                OleDbConnection conn = new OleDbConnection(connString);
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                string queryString = "select [FILE_DATE], [LTYPE], [LSTYPE], [MODEL_CD], [CUT_OFF], [SCMIN], [SCMAX], [RINT], [range_CD] from [ST_SCORERANGE$]";

                string FILE_DATE = "";
                string LTYPE = "";
                string LSTYPE = "";
                string MODEL_CD = "";
                string CUT_OFF = "";
                string SCMIN = "";
                string SCMAX = "";
                string RINT = "";
                string range_CD = "";


                int intRowCount = 0;
                int intInsertRowCount = 0;
                int intUpdateRowCount = 0;
                int intErrorRowCount = 0;

                OleDbCommand cmd = new OleDbCommand(queryString, conn);
                OleDbDataAdapter da = new OleDbDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);


                //Check for Null or Blank on necessary field (LTYPE, LSTYPE, MODEL_CD, CUT_OFF, SCMIN, SCMAX, RINT, range_CD)
                bool blDataNotCompleteInSomeColumn = false;
                intErrorRowCount = 1;
                foreach (DataRow drRow in dt.Rows)
                {
                    //Get file date
                    LTYPE = drRow["LTYPE"].ToString().Replace("\"", "").Trim();
                    LSTYPE = drRow["LSTYPE"].ToString().Replace("\"", "").Trim();
                    MODEL_CD = drRow["MODEL_CD"].ToString().Replace("\"", "").Trim();
                    CUT_OFF = drRow["CUT_OFF"].ToString().Replace("\"", "").Trim();
                    SCMIN = drRow["SCMIN"].ToString().Replace("\"", "").Trim();
                    SCMAX = drRow["SCMAX"].ToString().Replace("\"", "").Trim();
                    RINT = drRow["RINT"].ToString().Replace("\"", "").Trim();
                    range_CD = drRow["range_CD"].ToString().Replace("\"", "").Trim();

                    if (!(LTYPE == "" && LSTYPE == "" && MODEL_CD == "" && CUT_OFF == "" && SCMIN == "" && SCMAX == "" && RINT == "" && range_CD == ""))
                    {
                        if (LTYPE == "" || LSTYPE == "" || MODEL_CD == "" || CUT_OFF == "" || SCMIN == "" || SCMAX == "" || RINT == "" || range_CD == "")
                        {
                            blDataNotCompleteInSomeColumn = true; //Got some necessary column is Null or Blank
                        }
                    }
                    //Throw exceptional error
                    if (blDataNotCompleteInSomeColumn)
                    {
                        intErrorRowCount++;
                        throw new Exception("Some field value (LTYPE, LSTYPE, MODEL_CD, CUT_OFF, SCMIN, SCMAX, RINT, range_CD) is BLANK or NULL at Excel Row Number : " + intErrorRowCount.ToString() + ", Please verify the import file.");
                    }
                    //Define next row number
                    intErrorRowCount++;
                }


                //Reset variable value
                FILE_DATE = "";
                LTYPE = "";
                LSTYPE = "";
                MODEL_CD = "";
                CUT_OFF = "";
                SCMIN = "";
                SCMAX = "";
                RINT = "";
                range_CD = "";

                intRowCount = 0;
                intInsertRowCount = 0;
                intUpdateRowCount = 0;
                intErrorRowCount = 0;




                SQLToDataTable conn2tb = new SQLToDataTable();
                int intExistRowCount = 0;
                string strInsertFlag = "I";

                tmestart = DateTime.Now.Year.ToString("0000") + DateTime.Now.ToString("-MM-dd HH:mm:ss.") + DateTime.Now.Millisecond.ToString();

                foreach (DataRow drRow in dt.Rows)
                {
                    intRowCount++;
                    try
                    {
                        //Get file date
                        FILE_DATE = drRow["FILE_DATE"].ToString().Replace("\"", "").Trim();
                        LTYPE = drRow["LTYPE"].ToString().Replace("\"", "").Trim();
                        LSTYPE = drRow["LSTYPE"].ToString().Replace("\"", "").Trim();
                        MODEL_CD = drRow["MODEL_CD"].ToString().Replace("\"", "").Trim();
                        CUT_OFF = drRow["CUT_OFF"].ToString().Replace("\"", "").Trim();
                        SCMIN = drRow["SCMIN"].ToString().Replace("\"", "").Trim();
                        SCMAX = drRow["SCMAX"].ToString().Replace("\"", "").Trim();
                        RINT = drRow["RINT"].ToString().Replace("\"", "").Trim();
                        range_CD = drRow["range_CD"].ToString().Replace("\"", "").Trim();

                        if (LTYPE.Trim() != "" && LSTYPE.Trim() != "" && MODEL_CD.Trim() != "")
                        {
                            if (intRowCount == 1)
                            {
                                //Check file date
                                string strDate = DateTime.Now.Year.ToString("0000") + DateTime.Now.ToString("MM01");
                                if (strDate.Substring(0, 6) != FILE_DATE.Substring(0, 6))
                                {
                                    //Keep error log
                                    queryImport = "insert into s_log_import(RESOURCES, START_DATE, END_DATE, TOTAL, STATUS, ERROR_MESSAGE, Group_ID) values('BATCH_MASTERs', getdate(),getdate(),0,'FAIL','Wrong file DATE : file date is " + FILE_DATE + ", Table : " + manipTableName + ", File :" + filePath + " ','" + logGroupID + "');";
                                    cmdInsert.CommandText = queryImport;
                                    cmdInsert.ExecuteNonQuery();

                                    //Close all connection
                                    conn.Close();
                                    conn.Dispose();
                                    connInsert.Close();
                                    connInsert.Dispose();

                                    //Exit method
                                    return;
                                }
                            }

                            DataTable dtST = new System.Data.DataTable();
                            dtST = conn2tb.ExcuteSQL("select count(*) from ST_SCORERANGE where LTYPE = '" + LTYPE + "' and LSTYPE = '" + LSTYPE.Trim() + "' and MODEL_CD = '" + MODEL_CD.Trim() + "'");
                            intExistRowCount = int.Parse(dtST.Rows[0][0].ToString());
                            if (intExistRowCount == 0)
                            {
                                //Insert new record
                                strInsertFlag = "I";
                                queryImport = " insert into ST_SCORERANGE " +
                                                " (" +
                                                " MODEL_CD, " +
                                                " LTYPE, " +
                                                " LSTYPE, " +
                                                " CUT_OFF, " +
                                                " SCMIN, " +
                                                " SCMAX, " +
                                                " RINT, " +
                                                " range_CD " +
                                                " )" +
                                                " values " +
                                                " ( " +
                                                " '" + MODEL_CD + "', " +
                                                " '" + LTYPE + "', " +
                                                " '" + LSTYPE + "', " +
                                                " '" + CUT_OFF + "', " +
                                                " '" + SCMIN + "', " +
                                                " '" + SCMAX + "', " +
                                                " '" + RINT + "', " +
                                                " '" + range_CD + "' " +
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
                                queryImport = " update ST_SCORERANGE " +
                                                " set " +
                                                " CUT_OFF = '" + CUT_OFF + "', " +
                                                " SCMIN = '" + SCMIN + "', " +
                                                " SCMAX = '" + SCMAX + "', " +
                                                " RINT = '" + RINT + "', " +
                                                " range_CD = '" + range_CD + "' " +
                                                " where LTYPE = '" + LTYPE + "' and LSTYPE = '" + LSTYPE.Trim() + "' and MODEL_CD = '" + MODEL_CD.Trim() + "'";
                                cmdInsert.CommandText = queryImport;
                                cmdInsert.ExecuteNonQuery();

                                //Count updated record
                                intUpdateRowCount++;
                            }

                            //Insert into ST_SCORERANGE_Import_Tracking
                            queryImport =
                                            " insert into ST_SCORERANGE_Import_Tracking " +
                                            " (" +
                                            " MODEL_CD, " +
                                            " LTYPE, " +
                                            " LSTYPE, " +
                                            " CUT_OFF, " +
                                            " SCMIN, " +
                                            " SCMAX, " +
                                            " RINT, " +
                                            " range_CD, " +
                                            " IMP_DTM, " +
                                            " IMP_STATUS, " +
                                            " IMP_Error_Result, " +
                                            " USER_ID " +
                                            " )" +
                                            " values " +
                                            " ( " +
                                            " '" + MODEL_CD + "', " +
                                            " '" + LTYPE + "', " +
                                            " '" + LSTYPE + "', " +
                                            " '" + CUT_OFF + "', " +
                                            " '" + SCMIN + "', " +
                                            " '" + SCMAX + "', " +
                                            " '" + RINT + "', " +
                                            " '" + range_CD + "', " +
                                            " getdate(), " +
                                            " '" + strInsertFlag + "', " +
                                            " 'Insert / Update Success', " +
                                            " '" + User_ID + "' " +
                                            " ); ";

                            cmdInsert.CommandText = queryImport;
                            cmdInsert.ExecuteNonQuery();

                        }//---End of key feild checking

                   
                    }
                    catch (Exception ex)
                    {
                        string strExceptional = ex.Message.ToString();
                        strInsertFlag = "E";
                        //Insert into ST_BRANCH_Import_Tracking
                        queryImport = " insert into ST_SCORERANGE_Import_Tracking " +
                                        " (" +
                                        " MODEL_CD, " +
                                        " LTYPE, " +
                                        " LSTYPE, " +
                                        " CUT_OFF, " +
                                        " SCMIN, " +
                                        " SCMAX, " +
                                        " RINT, " +
                                        " range_CD, " +
                                        " IMP_DTM, " +
                                        " IMP_STATUS, " +
                                        " IMP_Error_Result, " +
                                        " USER_ID " +
                                        " )" +
                                        " values " +
                                        " ( " +
                                        " '" + MODEL_CD + "', " +
                                        " '" + LTYPE + "', " +
                                        " '" + LSTYPE + "', " +
                                        " '" + CUT_OFF + "', " +
                                        " '" + SCMIN + "', " +
                                        " '" + SCMAX + "', " +
                                        " '" + RINT + "', " +
                                        " '" + range_CD + "', " +
                                        " getdate(), " +
                                        " '" + strInsertFlag + "', " +
                                        " '" + strExceptional + "', " +
                                        " '" + User_ID + "' " +
                                        " ) ";

                        cmdInsert.CommandText = queryImport;
                        cmdInsert.ExecuteNonQuery();

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

        //********* Import ST_SCORERNGLST ******************
        public void import_ST_SCORERNGLST()
        {
            //Remark
            // ST Log Table : ST_CHARBIN_Import_Tracking
            // Overall log table : S_LOG_IMPORT

            string manipTableName = "ST_SCORERNGLST";
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
                string User_ID = ExportUtility.getCurrentUserId;

                //Declare file path
                string filePath = ConfigurationManager.AppSettings["PKIMPORTPATH"].ToString();
                filePath = filePath + "ST_SCORERNGLST.xls";

                //Read xls using OleDB
                string connString = "";

                connString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"", filePath);
                OleDbConnection conn = new OleDbConnection(connString);
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                string queryString = "select [FILE_DATE], [RANGE_CD], [RANGE_NAME] from [ST_SCORERNGLST$]";

                string FILE_DATE = "";
                string RANGE_CD = "";
                string RANGE_NAME = "";


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
                    RANGE_CD = drRow["RANGE_CD"].ToString().Replace("\"", "").Trim();
                    RANGE_NAME = drRow["RANGE_NAME"].ToString().Replace("\"", "").Trim();

                    if (!(RANGE_CD == "" && RANGE_NAME == ""))
                    {
                        if (RANGE_CD == "" || RANGE_NAME == "")
                        {
                            blDataNotCompleteInSomeColumn = true; //Got some necessary column is Null or Blank
                        }
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
                RANGE_CD = "";
                RANGE_NAME = "";

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
                        RANGE_CD = drRow["RANGE_CD"].ToString().Replace("\"", "").Trim();
                        RANGE_NAME = drRow["RANGE_NAME"].ToString().Replace("\"", "").Trim();

                        if (RANGE_CD.Trim() != "")
                        {
                            if (intRowCount == 1)
                            {
                                //Check file date
                                string strDate = DateTime.Now.Year.ToString("0000") + DateTime.Now.ToString("MM01");
                                if (strDate.Substring(0, 6) != FILE_DATE.Substring(0, 6))
                                {
                                    //Keep error log
                                    queryImport = "insert into s_log_import(RESOURCES, START_DATE, END_DATE, TOTAL, STATUS, ERROR_MESSAGE, Group_ID) values('BATCH_MASTERs', getdate(),getdate(),0,'FAIL','Wrong file DATE : file date is " + FILE_DATE + ", Table : " + manipTableName + ", File :" + filePath + " ','" + logGroupID + "');";
                                    cmdInsert.CommandText = queryImport;
                                    cmdInsert.ExecuteNonQuery();

                                    //Close all connection
                                    conn.Close();
                                    conn.Dispose();
                                    connInsert.Close();
                                    connInsert.Dispose();

                                    //Exit method
                                    return;
                                }
                            }

                            DataTable dtST = new System.Data.DataTable();
                            dtST = conn2tb.ExcuteSQL("select count(*) from " + manipTableName + " where RANGE_CD = '" + RANGE_CD + "' and RANGE_NAME = '" + RANGE_NAME + "'");
                            intExistRowCount = int.Parse(dtST.Rows[0][0].ToString());
                            if (intExistRowCount == 0)
                            {
                                //Insert new record
                                strInsertFlag = "I";
                                queryImport = " insert into ST_SCORERNGLST " +
                                                " (" +
                                                " RANGE_CD, " +
                                                " RANGE_NAME " +
                                                " )" +
                                                " values " +
                                                " ( " +
                                                " '" + RANGE_CD + "', " +
                                                " '" + RANGE_NAME + "' " +
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
                                queryImport = " update ST_SCORERNGLST " +
                                                " set " +
                                                " RANGE_NAME = '" + RANGE_NAME + "' " +
                                                " where RANGE_CD = '" + RANGE_CD + "' and RANGE_NAME = '" + RANGE_NAME + "'";
                                cmdInsert.CommandText = queryImport;
                                cmdInsert.ExecuteNonQuery();

                                //Count updated record
                                intUpdateRowCount++;
                            }

                            //Insert into ST_SCORERANGE_Import_Tracking
                            queryImport =
                                            " insert into ST_SCORERNGLST_Import_Tracking " +
                                            " (" +
                                            " RANGE_CD, " +
                                            " RANGE_NAME, " +
                                            " IMP_DTM, " +
                                            " IMP_STATUS, " +
                                            " IMP_Error_Result, " +
                                            " USER_ID " +
                                            " )" +
                                            " values " +
                                            " ( " +
                                            " '" + RANGE_CD + "', " +
                                            " '" + RANGE_NAME + "', " +
                                            " getdate(), " +
                                            " '" + strInsertFlag + "', " +
                                            " 'Insert / Update Success', " +
                                            " '" + User_ID + "' " +
                                            " ); ";

                            cmdInsert.CommandText = queryImport;
                            cmdInsert.ExecuteNonQuery();

                        }//---End of key feild checking
                        
                    }
                    catch (Exception ex)
                    {
                        string strExceptional = ex.Message.ToString();
                        strInsertFlag = "E";
                        //Insert into ST_BRANCH_Import_Tracking
                        queryImport = " insert into ST_SCORERNGLST_Import_Tracking " +
                                        " (" +
                                        " RANGE_CD, " +
                                        " RANGE_NAME, " +
                                        " IMP_DTM, " +
                                        " IMP_STATUS, " +
                                        " IMP_Error_Result, " +
                                        " USER_ID " +
                                        " )" +
                                        " values " +
                                        " ( " +
                                        " '" + RANGE_CD + "', " +
                                        " '" + RANGE_NAME + "', " +
                                        " getdate(), " +
                                        " '" + strInsertFlag + "', " +
                                        " '" + strExceptional + "', " +
                                        " '" + User_ID + "' " +
                                        " ) ";

                        cmdInsert.CommandText = queryImport;
                        cmdInsert.ExecuteNonQuery();

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

        //********* Import ST_SALECHANNEL ******************
        public bool import_ST_SALECHANNEL(string filename)
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
                    ex.Message.Replace("'", "") + "', '" + logGroupID + "');";
                dbaccess.ExecuteNonQuery(sqlstr);
                //throw;
            }
            dbaccess.CloseConnection();

            return result;
        }

        #endregion Protected Method (Import Master Data)

    }
}