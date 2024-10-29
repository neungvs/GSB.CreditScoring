using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Diagnostics;
using WinSCP;
using System.Configuration;
using System.Data.SqlClient;
using System.Xml;
using System.Data;
using Microsoft.VisualBasic;


namespace GSB.LoanSystem
{
    public partial class OBA : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ddlMNTMonthly.Items.Insert(0, new ListItem(DateTime.Now.AddMonths(-1).Year.ToString("0000") + DateTime.Now.AddMonths(-1).Month.ToString("00"), DateTime.Now.Year.ToString("0000") + DateTime.Now.Month.ToString("00")));
                ddlMNTMonthly.Items.Insert(1, new ListItem(DateTime.Now.AddMonths(-2).Year.ToString("0000") + DateTime.Now.AddMonths(-2).Month.ToString("00"), DateTime.Now.AddMonths(-1).Year.ToString("0000") + DateTime.Now.AddMonths(-1).Month.ToString("00")));
                ddlMNTMonthly.Items.Insert(2, new ListItem(DateTime.Now.AddMonths(-3).Year.ToString("0000") + DateTime.Now.AddMonths(-3).Month.ToString("00"), DateTime.Now.AddMonths(-2).Year.ToString("0000") + DateTime.Now.AddMonths(-2).Month.ToString("00")));
                ddlMNTMonthly.Items.Insert(3, new ListItem(DateTime.Now.AddMonths(-4).Year.ToString("0000") + DateTime.Now.AddMonths(-4).Month.ToString("00"), DateTime.Now.AddMonths(-3).Year.ToString("0000") + DateTime.Now.AddMonths(-3).Month.ToString("00")));
                ddlMNTMonthly.Items.Insert(4, new ListItem(DateTime.Now.AddMonths(-5).Year.ToString("0000") + DateTime.Now.AddMonths(-5).Month.ToString("00"), DateTime.Now.AddMonths(-4).Year.ToString("0000") + DateTime.Now.AddMonths(-4).Month.ToString("00")));
                ddlMNTMonthly.Items.Insert(5, new ListItem(DateTime.Now.AddMonths(-6).Year.ToString("0000") + DateTime.Now.AddMonths(-6).Month.ToString("00"), DateTime.Now.AddMonths(-5).Year.ToString("0000") + DateTime.Now.AddMonths(-5).Month.ToString("00")));
                ddlMNTMonthly.Items.Insert(6, new ListItem(DateTime.Now.AddMonths(-7).Year.ToString("0000") + DateTime.Now.AddMonths(-7).Month.ToString("00"), DateTime.Now.AddMonths(-6).Year.ToString("0000") + DateTime.Now.AddMonths(-6).Month.ToString("00")));
                ddlMNTMonthly.Items.Insert(7, new ListItem(DateTime.Now.AddMonths(-8).Year.ToString("0000") + DateTime.Now.AddMonths(-8).Month.ToString("00"), DateTime.Now.AddMonths(-7).Year.ToString("0000") + DateTime.Now.AddMonths(-7).Month.ToString("00")));
                ddlMNTMonthly.Items.Insert(8, new ListItem(DateTime.Now.AddMonths(-9).Year.ToString("0000") + DateTime.Now.AddMonths(-9).Month.ToString("00"), DateTime.Now.AddMonths(-8).Year.ToString("0000") + DateTime.Now.AddMonths(-8).Month.ToString("00")));
                ddlMNTMonthly.Items.Insert(9, new ListItem(DateTime.Now.AddMonths(-10).Year.ToString("0000") + DateTime.Now.AddMonths(-10).Month.ToString("00"), DateTime.Now.AddMonths(-9).Year.ToString("0000") + DateTime.Now.AddMonths(-9).Month.ToString("00")));
                ddlMNTMonthly.Items.Insert(10, new ListItem(DateTime.Now.AddMonths(-11).Year.ToString("0000") + DateTime.Now.AddMonths(-11).Month.ToString("00"), DateTime.Now.AddMonths(-10).Year.ToString("0000") + DateTime.Now.AddMonths(-10).Month.ToString("00")));
                ddlMNTMonthly.Items.Insert(11, new ListItem(DateTime.Now.AddMonths(-12).Year.ToString("0000") + DateTime.Now.AddMonths(-12).Month.ToString("00"), DateTime.Now.AddMonths(-11).Year.ToString("0000") + DateTime.Now.AddMonths(-11).Month.ToString("00")));

            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            //string fileBatchManual = "D:\\webapp\\scoringSME\\Batch\\FTP_OBA_Manual.bat";
            //string strValParaBatch = "";
            //string selMonth = ddlMNTMonthly.SelectedValue;


            //string[] BatchManualLines = File.ReadAllLines(fileBatchManual);

            //if (selMonth.Substring(4,2) == "12")
            //{
            //    strValParaBatch = "01" + "01" + (Convert.ToInt16(selMonth.Substring(0, 4)) + 1).ToString();

            //}
            //else
            //{
            //    strValParaBatch = "01" + (Convert.ToInt16(selMonth.Substring(4, 2)) + 1).ToString("00") + selMonth.Substring(0, 4);
            //}

            //BatchManualLines[0] = "set mydate="+ strValParaBatch;

            //File.WriteAllLines(fileBatchManual, BatchManualLines);

            //Process proc = new Process();
            //proc.StartInfo.FileName = @"OBA.exe";
            //proc.StartInfo.WorkingDirectory = @"D:\webapp\scoringSME\Batch";
            //proc.StartInfo.Arguments = "if any";
            //proc.StartInfo.UseShellExecute = true;
            //proc.StartInfo.CreateNoWindow = true;
            //proc.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
            //proc.Start();

            Button1.Enabled = false;
            Label2.Visible = true;
            ProcessWithFTP();
            Label2.Text = "ดำเนินการเสร็จสิ้น";
            Button1.Enabled = true;

        }

        private void ProcessWithFTP()
        {

            string tmestart = DateTime.Now.Year.ToString("0000") + DateTime.Now.ToString("-MM-dd HH:mm:ss.") + DateTime.Now.Millisecond.ToString();

            SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["FTPIP"].ToString());
            SqlCommand cmd;

            string m = ddlMNTMonthly.SelectedValue;


            //DateTime dateTransaction = monthCalendar1.SelectionStart.AddDays(-1);

            //int trdd = dateTransaction.Day;
            //int trmm = dateTransaction.Month;
            //int tryyyy = dateTransaction.Year;

            //if (tryyyy > 2500) tryyyy = tryyyy - 543;


            //int dd = Convert.ToDateTime(d).Day;
            //int mm = Convert.ToDateTime(d).Month;
            //int yyyy = Convert.ToDateTime(d).Year;

            string strFilenameRequest = "LOA01" + m.Substring(4,2)+m.Substring(0,4);

            SessionOptions sessionOptions = new SessionOptions();
            sessionOptions.Protocol = Protocol.Sftp;
            sessionOptions.HostName = ConfigurationManager.AppSettings["ftphost"].ToString();
            sessionOptions.UserName = ConfigurationManager.AppSettings["ftpuser"].ToString();
            sessionOptions.Password = ConfigurationManager.AppSettings["ftppassword"].ToString();
            sessionOptions.SshHostKey = ConfigurationManager.AppSettings["ftphostkey"].ToString();
            sessionOptions.SshPrivateKey = ConfigurationManager.AppSettings["ftpprikey"].ToString();

            String remotePath = ConfigurationManager.AppSettings["ftpPathOBA"].ToString() + strFilenameRequest + ".txt";
            String localPath = ConfigurationManager.AppSettings["ftpPathLocalOBA"].ToString() + strFilenameRequest + ".txt";


            try
            {
                // FTP
                using (Session session = new Session())
                {
                    session.Open(sessionOptions);



                    session.GetFiles(remotePath, localPath).Check();

                    string[] allLines = File.ReadAllLines(localPath);

                    if (allLines.Length > 1)
                    {
                        for (int index = 0; index <= (allLines.Length - 2); index++)
                        {
                            string[] items = allLines[index].Split(new char[] { '|' });

                            string strCBA_APP_NO = items[171].Trim();
                            string strDW_ACC_NO = items[19].Trim();
                            string strDW_FDISDATE = items[93].Trim();
                            string strDW_CDATE = items[84].Trim();
                            string strDW_EXPDATE = items[31].Trim();
                            string strDW_SIGN_DATE = items[41].Trim();
                            string strISACTIVE = items[18].Trim();
                            string strDW_BADMONTH = "";
                            string strDW_OUTSTAND = items[36].Trim();
                            string strLAST_UPDATE_DTM = items[0].Trim();
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


                            if (strPROVCAT.Trim() == "0" && strBAD_DEBT_FLG.Trim() == "0")
                            {
                                strDW_BADMONTH = "1";
                            }
                            else if (strPROVCAT.Trim() == "0" && strBAD_DEBT_FLG.Trim() == "1")
                            {
                                strDW_BADMONTH = "6";
                            }
                            else if (strPROVCAT.Trim() == "1")
                            {
                                strDW_BADMONTH = "1";
                            }
                            else if (strPROVCAT.Trim() == "2")
                            {
                                strDW_BADMONTH = "3";
                            }
                            else if (strPROVCAT.Trim() == "3")
                            {
                                strDW_BADMONTH = "6";
                            }
                            else if (strPROVCAT.Trim() == "4")
                            {
                                strDW_BADMONTH = "12";
                            }
                            else if (strPROVCAT.Trim() == "5")
                            {
                                strDW_BADMONTH = "13";
                            }

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
        "DW_FREQ_TERM, DW_ORG_TERM_MULT, DW_COLLVALUE, CSTATUS_CD,DW_ISRESTRUCT, LOAN_TYPE, FILE_LOAD_DATE) " +
        "VALUES(@D1, @D2, @D3 , @D4,@D5, @D6, @D7 , @D8,@D9, @D10, @D11 , @D12,@D13, @D14, @D15 , @D16,@D17,@D18,@D19)";


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

                            cmd.Parameters["@D1"].Value = strCBA_APP_NO;
                            cmd.Parameters["@D2"].Value = strDW_ACC_NO;
                            cmd.Parameters["@D3"].Value = strDW_FDISDATE;
                            cmd.Parameters["@D4"].Value = strDW_CDATE;
                            cmd.Parameters["@D5"].Value = strDW_EXPDATE;
                            cmd.Parameters["@D6"].Value = strDW_SIGN_DATE;
                            cmd.Parameters["@D7"].Value = strISACTIVE;
                            cmd.Parameters["@D8"].Value = strDW_BADMONTH;
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

                            con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();


                            sqlStatement =
        "INSERT INTO OBA_Temp(CBS_APP_NO, DW_ACC_NO, DW_FDISDATE, DW_CDATE, DW_EXPDATE," +
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
                            cmd.Parameters.Add("@D20", SqlDbType.VarChar, 50);
                            cmd.Parameters.Add("@D21", SqlDbType.VarChar, 50);
                            cmd.Parameters.Add("@D22", SqlDbType.VarChar, 50);

                            cmd.Parameters["@D1"].Value = strCBA_APP_NO;
                            cmd.Parameters["@D2"].Value = strDW_ACC_NO;
                            cmd.Parameters["@D3"].Value = strDW_FDISDATE;
                            cmd.Parameters["@D4"].Value = strDW_CDATE;
                            cmd.Parameters["@D5"].Value = strDW_EXPDATE;
                            cmd.Parameters["@D6"].Value = strDW_SIGN_DATE;
                            cmd.Parameters["@D7"].Value = strISACTIVE;
                            cmd.Parameters["@D8"].Value = strDW_BADMONTH;
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

    }
}