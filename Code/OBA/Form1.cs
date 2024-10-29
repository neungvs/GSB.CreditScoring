using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.IO;
using System.Data.SqlClient;

namespace OBA
{
    public partial class Form1 : Form
    {
        static string strFilenameRequest = "";

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string d = monthCalendar1.SelectionStart.ToString();

            int dd = Convert.ToDateTime(d).Day;
            int mm = Convert.ToDateTime(d).Month;
            int yyyy = Convert.ToDateTime(d).Year;

            strFilenameRequest = "LOA" + dd.ToString("00") + mm.ToString("00") + yyyy.ToString("0000");

            ProcessNoFTP();
            MessageBox.Show("Finish");
        }

        private void ProcessNoFTP()
        {
            string tmestart = DateTime.Now.Year.ToString("0000") + DateTime.Now.ToString("-MM-dd HH:mm:ss.") + DateTime.Now.Millisecond.ToString();


            ////NO FTP
            ///
            String localPath = ConfigurationManager.AppSettings["ftpPathLocal"].ToString() + strFilenameRequest + ".txt";

            SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["FTPIP"].ToString());
            SqlCommand cmd;

            try
            {
                string[] allLines = File.ReadAllLines(localPath, Encoding.Default);

                if (allLines.Length > 1)
                {
                    for (int index = 0; index <= (allLines.Length - 2); index++)
                    {
                        string[] items = allLines[index].Split(new char[] { '|' });
                        int loc;

                        string strCBA_APP_NO = items[171].Trim();
                        string strDW_ACC_NO = items[19].Trim();
                        string strDW_FDISDATE = items[93].Trim();
                        string strDW_CDATE = items[84].Trim();
                        string strDW_EXPDATE = items[31].Trim();
                        string strDW_SIGN_DATE = items[41].Trim();

                        loc = items[41].Trim().IndexOf("/");

                        if (loc > 0 || items[41].Trim() == "")
                        {
                            strDW_SIGN_DATE = items[41].Trim();
                        }
                        else
                        {
                            strDW_SIGN_DATE = items[41].Trim().Substring(6, 2) + "/" + items[41].Trim().Substring(4, 2) + "/" + items[41].Trim().Substring(0, 4);
                        }

                        string strISACTIVE = items[18].Trim();
                        //string strDW_BADMONTH = "";
                        string strDW_OUTSTAND = items[36].Trim();
                        string strLAST_UPDATE_DTM;

                        loc = items[0].Trim().IndexOf("/");

                        if (loc > 0)
                        {
                            strLAST_UPDATE_DTM = items[0].Trim();
                        }
                        else
                        {
                            strLAST_UPDATE_DTM = items[0].Trim().Substring(6, 2) + "/" + items[0].Trim().Substring(4, 2) + "/" + items[0].Trim().Substring(0, 4);
                        }

                         
                        string strDW_BADAMOUNT = "";
                        string strDW_ORG_TERM = "";
                        string strDW_FREQ_TERM = items[43].Trim();
                        string strDW_ORG_TERM_MULT = "";
                        string strDW_COLLVALUE = items[106].Trim();
                        string strCSTATUS_CD = "";
                        string strDW_ISRESTRUCT = items[119].Trim();
                        string strLOAN_TYPE = items[9].Trim();
                        string strFILE_LOAD_DATE = "";
                        string strPROVCAT = items[60].Trim();
                        string strPROVCAT_NAME = items[61].Trim();
                        string strBAD_DEBT_FLG = items[62].Trim();
                        string strDELPREM_BOT = items[23].Trim();

                        if (strDELPREM_BOT == "")
                        {
                            strDELPREM_BOT = "0";
                        }


                        //if (strPROVCAT.Trim() == "0" && strBAD_DEBT_FLG.Trim() == "0")
                        //{
                        //    strDW_BADMONTH = "1";
                        //}
                        //else if (strPROVCAT.Trim() == "0" && strBAD_DEBT_FLG.Trim() == "1")
                        //{
                        //    strDW_BADMONTH = "6";
                        //}
                        //else if (strPROVCAT.Trim() == "1")
                        //{
                        //    strDW_BADMONTH = "1";
                        //}
                        //else if (strPROVCAT.Trim() == "2")
                        //{
                        //    strDW_BADMONTH = "3";
                        //}
                        //else if (strPROVCAT.Trim() == "3")
                        //{
                        //    strDW_BADMONTH = "6";
                        //}
                        //else if (strPROVCAT.Trim() == "4")
                        //{
                        //    strDW_BADMONTH = "12";
                        //}
                        //else if (strPROVCAT.Trim() == "5")
                        //{
                        //    strDW_BADMONTH = "13";
                        //}

                            decimal decFra = 0;
                        decimal decFra_int = 0;

                        if (items[71].Trim() != "")
                        {
                            decFra = Convert.ToDecimal(items[71].Trim());
                        }

                        if (items[72].Trim() != "")
                        {
                            decFra_int = Convert.ToDecimal(items[72].Trim());
                        }

                        if (items[71].Trim() != "")
                        {
                            strDW_BADAMOUNT = (decFra + decFra_int).ToString();
                        }


                        string sqlStatement =
    "INSERT INTO DWH_BTFILE(CBS_APP_NO, DW_ACC_NO, DW_FDISDATE, DW_CDATE, DW_EXPDATE," +
    "DW_SIGN_DATE,ISACTIVE, DW_BADMONTH, DW_OUTSTAND, LAST_UPDATE_DTM, DW_BADAMOUNT,DW_ORG_TERM," +
    "DW_FREQ_TERM, DW_ORG_TERM_MULT, DW_COLLVALUE, CSTATUS_CD,DW_ISRESTRUCT, LOAN_TYPE, FILE_LOAD_DATE,PROVCAT,PROVCAT_NAME,BAD_DEBT_FLG) " +
    "VALUES(@D1, @D2, @D3 , @D4,@D5, @D6, @D7 , @D8,@D9, @D10, @D11 , @D12,@D13, @D14, @D15 , @D16,@D17,@D18,@D19,@D20,@D21,@D22)";


                        cmd = new SqlCommand(sqlStatement, con);

                        cmd.Parameters.Add("@D1", SqlDbType.VarChar, 50);
                        cmd.Parameters.Add("@D2", SqlDbType.VarChar, 50);
                        cmd.Parameters.Add("@D3", SqlDbType.VarChar, 200);
                        cmd.Parameters.Add("@D4", SqlDbType.VarChar, 200);
                        cmd.Parameters.Add("@D5", SqlDbType.VarChar, 200);
                        cmd.Parameters.Add("@D6", SqlDbType.VarChar, 200);
                        cmd.Parameters.Add("@D7", SqlDbType.VarChar, 200);
                        cmd.Parameters.Add("@D8", SqlDbType.VarChar, 200);
                        cmd.Parameters.Add("@D9", SqlDbType.VarChar, 200);
                        cmd.Parameters.Add("@D10", SqlDbType.VarChar, 200);
                        cmd.Parameters.Add("@D11", SqlDbType.VarChar, 200);
                        cmd.Parameters.Add("@D12", SqlDbType.VarChar, 200);
                        cmd.Parameters.Add("@D13", SqlDbType.VarChar, 200);
                        cmd.Parameters.Add("@D14", SqlDbType.VarChar, 200);
                        cmd.Parameters.Add("@D15", SqlDbType.VarChar, 200);
                        cmd.Parameters.Add("@D16", SqlDbType.VarChar, 200);
                        cmd.Parameters.Add("@D17", SqlDbType.VarChar, 200);
                        cmd.Parameters.Add("@D18", SqlDbType.VarChar, 10);
                        cmd.Parameters.Add("@D19", SqlDbType.VarChar, 50);
                        cmd.Parameters.Add("@D20", SqlDbType.Int);
                        cmd.Parameters.Add("@D21", SqlDbType.VarChar, 50);
                        cmd.Parameters.Add("@D22", SqlDbType.Int);

                        cmd.Parameters["@D1"].Value = strCBA_APP_NO;
                        cmd.Parameters["@D2"].Value = strDW_ACC_NO;
                        cmd.Parameters["@D3"].Value = strDW_FDISDATE;
                        cmd.Parameters["@D4"].Value = strDW_CDATE;
                        cmd.Parameters["@D5"].Value = strDW_EXPDATE;
                        cmd.Parameters["@D6"].Value = strDW_SIGN_DATE;
                        cmd.Parameters["@D7"].Value = strISACTIVE;
                        cmd.Parameters["@D8"].Value = "";
                        cmd.Parameters["@D9"].Value = strDW_OUTSTAND;
                        cmd.Parameters["@D10"].Value = strLAST_UPDATE_DTM;
                        cmd.Parameters["@D11"].Value = strDW_BADAMOUNT;
                        cmd.Parameters["@D12"].Value = strDW_ORG_TERM;
                        cmd.Parameters["@D13"].Value = strDW_FREQ_TERM;
                        cmd.Parameters["@D14"].Value = strDW_ORG_TERM_MULT;
                        cmd.Parameters["@D15"].Value = strDW_COLLVALUE;
                        cmd.Parameters["@D16"].Value = strCSTATUS_CD;
                        cmd.Parameters["@D17"].Value = strDW_ISRESTRUCT;
                        cmd.Parameters["@D18"].Value = strLOAN_TYPE;
                        cmd.Parameters["@D19"].Value = strFILE_LOAD_DATE;
                        cmd.Parameters["@D21"].Value = strPROVCAT_NAME;

                        if (strPROVCAT == "")
                        {
                            cmd.Parameters["@D20"].Value = DBNull.Value;
                        }
                        else
                        {
                            cmd.Parameters["@D20"].Value = strPROVCAT;
                        }

                        if (strBAD_DEBT_FLG == "")
                        {
                            cmd.Parameters["@D22"].Value = DBNull.Value;
                        }
                        else
                        {
                            cmd.Parameters["@D22"].Value = strBAD_DEBT_FLG;
                        }

                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();


                        sqlStatement =
    "INSERT INTO OBA_Temp(CBS_APP_NO, DW_ACC_NO, DW_FDISDATE, DW_CDATE, DW_EXPDATE," +
    "DW_SIGN_DATE,ISACTIVE, DW_BADMONTH, DW_OUTSTAND, LAST_UPDATE_DTM, DW_BADAMOUNT,DW_ORG_TERM," +
    "DW_FREQ_TERM, DW_ORG_TERM_MULT, DW_COLLVALUE, CSTATUS_CD,DW_ISRESTRUCT, LOAN_TYPE, FILE_LOAD_DATE,PROVCAT,PROVCAT_NAME,BAD_DEBT_FLG,DELPREM_BOT) " +
    "VALUES(@D1, @D2, @D3 , @D4,@D5, @D6, @D7 , @D8,@D9, @D10, @D11 , @D12,@D13, @D14, @D15 , @D16,@D17,@D18,@D19,@D20,@D21,@D22,@D23)";

                        cmd = new SqlCommand(sqlStatement, con);

                        cmd.Parameters.Add("@D1", SqlDbType.VarChar, 50);
                        cmd.Parameters.Add("@D2", SqlDbType.VarChar, 50);
                        cmd.Parameters.Add("@D3", SqlDbType.VarChar, 200);
                        cmd.Parameters.Add("@D4", SqlDbType.VarChar, 200);
                        cmd.Parameters.Add("@D5", SqlDbType.VarChar, 200);
                        cmd.Parameters.Add("@D6", SqlDbType.VarChar, 200);
                        cmd.Parameters.Add("@D7", SqlDbType.VarChar, 200);
                        cmd.Parameters.Add("@D8", SqlDbType.VarChar, 200);
                        cmd.Parameters.Add("@D9", SqlDbType.VarChar, 200);
                        cmd.Parameters.Add("@D10", SqlDbType.VarChar, 200);
                        cmd.Parameters.Add("@D11", SqlDbType.VarChar, 200);
                        cmd.Parameters.Add("@D12", SqlDbType.VarChar, 200);
                        cmd.Parameters.Add("@D13", SqlDbType.VarChar, 200);
                        cmd.Parameters.Add("@D14", SqlDbType.VarChar, 200);
                        cmd.Parameters.Add("@D15", SqlDbType.VarChar, 200);
                        cmd.Parameters.Add("@D16", SqlDbType.VarChar, 200);
                        cmd.Parameters.Add("@D17", SqlDbType.VarChar, 200);
                        cmd.Parameters.Add("@D18", SqlDbType.VarChar, 10);
                        cmd.Parameters.Add("@D19", SqlDbType.VarChar, 50);
                        cmd.Parameters.Add("@D20", SqlDbType.VarChar, 50);
                        cmd.Parameters.Add("@D21", SqlDbType.VarChar, 50);
                        cmd.Parameters.Add("@D22", SqlDbType.VarChar, 50);
                        cmd.Parameters.Add("@D23", SqlDbType.Real);


                        cmd.Parameters["@D1"].Value = strCBA_APP_NO;
                        cmd.Parameters["@D2"].Value = strDW_ACC_NO;
                        cmd.Parameters["@D3"].Value = strDW_FDISDATE;
                        cmd.Parameters["@D4"].Value = strDW_CDATE;
                        cmd.Parameters["@D5"].Value = strDW_EXPDATE;
                        cmd.Parameters["@D6"].Value = strDW_SIGN_DATE;
                        cmd.Parameters["@D7"].Value = strISACTIVE;
                        cmd.Parameters["@D8"].Value = "";
                        cmd.Parameters["@D9"].Value = strDW_OUTSTAND;
                        cmd.Parameters["@D10"].Value = strLAST_UPDATE_DTM;
                        cmd.Parameters["@D11"].Value = strDW_BADAMOUNT;
                        cmd.Parameters["@D12"].Value = strDW_ORG_TERM;
                        cmd.Parameters["@D13"].Value = strDW_FREQ_TERM;
                        cmd.Parameters["@D14"].Value = strDW_ORG_TERM_MULT;
                        cmd.Parameters["@D15"].Value = strDW_COLLVALUE;
                        cmd.Parameters["@D16"].Value = strCSTATUS_CD;
                        cmd.Parameters["@D17"].Value = strDW_ISRESTRUCT;
                        cmd.Parameters["@D18"].Value = strLOAN_TYPE;
                        cmd.Parameters["@D19"].Value = strFILE_LOAD_DATE;



                        cmd.Parameters["@D20"].Value = strPROVCAT;
                        cmd.Parameters["@D21"].Value = strPROVCAT_NAME;
                        cmd.Parameters["@D22"].Value = strBAD_DEBT_FLG;
                        cmd.Parameters["@D23"].Value = Convert.ToDouble(strDELPREM_BOT);

                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }

                string tmesend = DateTime.Now.Year.ToString("0000") + DateTime.Now.ToString("-MM-dd HH:mm:ss.") + DateTime.Now.Millisecond.ToString();
                string sqlStatementdx = "insert into s_log_import values('BATCH_OBA',convert(datetime,'" + tmestart.ToString() + "',121),convert(datetime,'" + tmesend.ToString() + "',121)," + (allLines.Length - 1).ToString() + ",'DONE','" + localPath.ToString() + "','005')";
                con.Open();
                cmd = new SqlCommand(sqlStatementdx, con);
                cmd.ExecuteNonQuery();
                con.Close();



            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: {0}", ex);
                //          return 1;
                //     return "False";
                string inExc = ex.Message;
                SqlConnection errcon = new SqlConnection(ConfigurationManager.AppSettings["FTPIP"].ToString());
                string sqlStatementderr = "insert into s_log_import values('BATCH_OBA',convert(datetime,'" + tmestart.ToString() + "',121),getdate(),0,'FAIL','" + localPath.ToString() + ":" + inExc.ToString().Replace("'", "''") + "','005')";
                errcon.Open();
                SqlCommand cmddlerr = new SqlCommand(sqlStatementderr, errcon);
                cmddlerr.ExecuteNonQuery();
                errcon.Close();
            }
        }

        private void Form1_Load_1(object sender, EventArgs e)
        {
            if (ConfigurationManager.AppSettings["RunWithTaskschedule"] == "1")
            {

                string d = monthCalendar1.SelectionStart.ToString();

                int dd = Convert.ToDateTime(d).Day;
                int mm = Convert.ToDateTime(d).Month;
                int yyyy = Convert.ToDateTime(d).Year;

                //string fileauto = "D:\\webapp\\scoringSME\\Batch\\BatchAuto_Temp.txt";
                //string filemanual = "D:\\webapp\\scoringSME\\Batch\\BatchManual_Temp.txt";
                //string fileBatchManual = "D:\\webapp\\scoringSME\\Batch\\FTP_OBA_Manual.bat";

                //if (File.Exists(fileauto))
                //{
                    strFilenameRequest = "LOA" + dd.ToString("00") + mm.ToString("00") + yyyy.ToString("0000");
                //}
                //else if (File.Exists(filemanual))
                //{
                //    string[] BatchManualLines = File.ReadAllLines(fileBatchManual);
                //    string strValParaBatch = BatchManualLines[0].Substring(BatchManualLines[0].Length - 8, 8);
                //    strFilenameRequest = "LOA" + strValParaBatch;

                //}

                ProcessNoFTP();
                this.Close();
            }

        }
    }
}
