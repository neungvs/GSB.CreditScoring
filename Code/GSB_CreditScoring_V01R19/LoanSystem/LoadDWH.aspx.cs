using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using WinSCP;
using System.Globalization;
using System.IO;
using System.Data;
using System.Text;
using System.Net ;

namespace GSB.LoanSystem
{
    public partial class LoadDWH : System.Web.UI.Page
    {
        //string revendt_global;
        protected void Page_Load(object sender, EventArgs e)
        {
            //string selMNT = DateTime.Now.AddMonths(-1).Year.ToString("0000") + DateTime.Now.AddMonths(-1).ToString("MM");
            //ddlMNT.Items.Insert(0, new ListItem(selMNT, selMNT));
            ddlMNT.Items.Insert(0, new ListItem(DateTime.Now.AddMonths(-1).Year.ToString("0000") + DateTime.Now.AddMonths(-1).ToString("MM"), DateTime.Now.AddMonths(-1).Year.ToString("0000") + DateTime.Now.AddMonths(-1).ToString("MM")));
            ddlMNT.Items.Insert(1, new ListItem(DateTime.Now.AddMonths(-2).Year.ToString("0000") + DateTime.Now.AddMonths(-2).ToString("MM"), DateTime.Now.AddMonths(-2).Year.ToString("0000") + DateTime.Now.AddMonths(-2).ToString("MM")));
            ddlMNT.Items.Insert(2, new ListItem(DateTime.Now.AddMonths(-3).Year.ToString("0000") + DateTime.Now.AddMonths(-3).ToString("MM"), DateTime.Now.AddMonths(-3).Year.ToString("0000") + DateTime.Now.AddMonths(-3).ToString("MM")));
            ddlMNT.Items.Insert(3, new ListItem(DateTime.Now.AddMonths(-4).Year.ToString("0000") + DateTime.Now.AddMonths(-4).ToString("MM"), DateTime.Now.AddMonths(-4).Year.ToString("0000") + DateTime.Now.AddMonths(-4).ToString("MM")));
            ddlMNT.Items.Insert(4, new ListItem(DateTime.Now.AddMonths(-5).Year.ToString("0000") + DateTime.Now.AddMonths(-5).ToString("MM"), DateTime.Now.AddMonths(-5).Year.ToString("0000") + DateTime.Now.AddMonths(-5).ToString("MM")));
            ddlMNT.Items.Insert(5, new ListItem(DateTime.Now.AddMonths(-6).Year.ToString("0000") + DateTime.Now.AddMonths(-6).ToString("MM"), DateTime.Now.AddMonths(-6).Year.ToString("0000") + DateTime.Now.AddMonths(-6).ToString("MM")));
            ddlMNT.Items.Insert(6, new ListItem(DateTime.Now.AddMonths(-7).Year.ToString("0000") + DateTime.Now.AddMonths(-7).ToString("MM"), DateTime.Now.AddMonths(-7).Year.ToString("0000") + DateTime.Now.AddMonths(-7).ToString("MM")));
            ddlMNT.Items.Insert(7, new ListItem(DateTime.Now.AddMonths(-8).Year.ToString("0000") + DateTime.Now.AddMonths(-8).ToString("MM"), DateTime.Now.AddMonths(-8).Year.ToString("0000") + DateTime.Now.AddMonths(-8).ToString("MM")));
            ddlMNT.Items.Insert(8, new ListItem(DateTime.Now.AddMonths(-9).Year.ToString("0000") + DateTime.Now.AddMonths(-9).ToString("MM"), DateTime.Now.AddMonths(-9).Year.ToString("0000") + DateTime.Now.AddMonths(-9).ToString("MM")));
            ddlMNT.Items.Insert(9, new ListItem(DateTime.Now.AddMonths(-10).Year.ToString("0000") + DateTime.Now.AddMonths(-10).ToString("MM"), DateTime.Now.AddMonths(-10).Year.ToString("0000") + DateTime.Now.AddMonths(-10).ToString("MM")));
            ddlMNT.Items.Insert(10, new ListItem(DateTime.Now.AddMonths(-11).Year.ToString("0000") + DateTime.Now.AddMonths(-11).ToString("MM"), DateTime.Now.AddMonths(-11).Year.ToString("0000") + DateTime.Now.AddMonths(-11).ToString("MM")));
            ddlMNT.Items.Insert(11, new ListItem(DateTime.Now.AddMonths(-12).Year.ToString("0000") + DateTime.Now.AddMonths(-12).ToString("MM"), DateTime.Now.AddMonths(-12).Year.ToString("0000") + DateTime.Now.AddMonths(-12).ToString("MM")));
            //add 24 month
            ddlMNT.Items.Insert(12, new ListItem(DateTime.Now.AddMonths(-13).Year.ToString("0000") + DateTime.Now.AddMonths(-13).ToString("MM"), DateTime.Now.AddMonths(-13).Year.ToString("0000") + DateTime.Now.AddMonths(-13).ToString("MM")));
            ddlMNT.Items.Insert(13, new ListItem(DateTime.Now.AddMonths(-14).Year.ToString("0000") + DateTime.Now.AddMonths(-14).ToString("MM"), DateTime.Now.AddMonths(-14).Year.ToString("0000") + DateTime.Now.AddMonths(-14).ToString("MM")));
            ddlMNT.Items.Insert(14, new ListItem(DateTime.Now.AddMonths(-15).Year.ToString("0000") + DateTime.Now.AddMonths(-15).ToString("MM"), DateTime.Now.AddMonths(-15).Year.ToString("0000") + DateTime.Now.AddMonths(-15).ToString("MM")));
            ddlMNT.Items.Insert(15, new ListItem(DateTime.Now.AddMonths(-16).Year.ToString("0000") + DateTime.Now.AddMonths(-16).ToString("MM"), DateTime.Now.AddMonths(-16).Year.ToString("0000") + DateTime.Now.AddMonths(-16).ToString("MM")));
            ddlMNT.Items.Insert(16, new ListItem(DateTime.Now.AddMonths(-17).Year.ToString("0000") + DateTime.Now.AddMonths(-17).ToString("MM"), DateTime.Now.AddMonths(-17).Year.ToString("0000") + DateTime.Now.AddMonths(-17).ToString("MM")));
            ddlMNT.Items.Insert(17, new ListItem(DateTime.Now.AddMonths(-18).Year.ToString("0000") + DateTime.Now.AddMonths(-18).ToString("MM"), DateTime.Now.AddMonths(-18).Year.ToString("0000") + DateTime.Now.AddMonths(-18).ToString("MM")));
            ddlMNT.Items.Insert(18, new ListItem(DateTime.Now.AddMonths(-19).Year.ToString("0000") + DateTime.Now.AddMonths(-19).ToString("MM"), DateTime.Now.AddMonths(-19).Year.ToString("0000") + DateTime.Now.AddMonths(-19).ToString("MM")));
            ddlMNT.Items.Insert(19, new ListItem(DateTime.Now.AddMonths(-20).Year.ToString("0000") + DateTime.Now.AddMonths(-20).ToString("MM"), DateTime.Now.AddMonths(-20).Year.ToString("0000") + DateTime.Now.AddMonths(-20).ToString("MM")));
            ddlMNT.Items.Insert(20, new ListItem(DateTime.Now.AddMonths(-21).Year.ToString("0000") + DateTime.Now.AddMonths(-21).ToString("MM"), DateTime.Now.AddMonths(-21).Year.ToString("0000") + DateTime.Now.AddMonths(-21).ToString("MM")));
            ddlMNT.Items.Insert(21, new ListItem(DateTime.Now.AddMonths(-22).Year.ToString("0000") + DateTime.Now.AddMonths(-22).ToString("MM"), DateTime.Now.AddMonths(-22).Year.ToString("0000") + DateTime.Now.AddMonths(-22).ToString("MM")));
            ddlMNT.Items.Insert(22, new ListItem(DateTime.Now.AddMonths(-23).Year.ToString("0000") + DateTime.Now.AddMonths(-23).ToString("MM"), DateTime.Now.AddMonths(-23).Year.ToString("0000") + DateTime.Now.AddMonths(-23).ToString("MM")));
            ddlMNT.Items.Insert(23, new ListItem(DateTime.Now.AddMonths(-24).Year.ToString("0000") + DateTime.Now.AddMonths(-24).ToString("MM"), DateTime.Now.AddMonths(-24).Year.ToString("0000") + DateTime.Now.AddMonths(-24).ToString("MM")));
            //add 24 month
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            string confirmValue = Request.Form["confirm_value"];
            if (confirmValue == "Yes")
            {
            Button1.Enabled = false;
            //Button1_Enabled();
            Label2.Visible = true;
            //Button1.Enabled = true;
            //string revendt = DateTime.Now.AddMonths(-1).Year.ToString("0000") + DateTime.Now.AddMonths(-1).ToString("MM");
            
                string revendt = ddlMNT.SelectedValue.ToString();
                string revendt1 = ddlMNT.SelectedValue.ToString();
                string revendt2 = ddlMNT.SelectedValue.ToString();
                string revendt3 = ddlMNT.SelectedValue.ToString();
                string revendt4 = ddlMNT.SelectedValue.ToString();
                string revendt5 = ddlMNT.SelectedValue.ToString();
                string revendt6 = ddlMNT.SelectedValue.ToString();
                string revendt7 = ddlMNT.SelectedValue.ToString();
                GetSFTPfile("/FromDWH/CR_" + revendt + ".txt", "D:\\webapp\\scoring\\FTP\\IN\\DWH\\CR_" + revendt + ".txt");
                GetSFTPfile("/FromDWH/LN01DW" + revendt + ".txt", "D:\\webapp\\scoring\\FTP\\IN\\DWH\\LN01DW" + revendt + ".txt");
                GetSFTPfile("/FromDWH/LN02DW" + revendt + ".txt", "D:\\webapp\\scoring\\FTP\\IN\\DWH\\LN02DW" + revendt + ".txt");
                GetSFTPfile("/FromDWH/LN03DW" + revendt + ".txt", "D:\\webapp\\scoring\\FTP\\IN\\DWH\\LN03DW" + revendt + ".txt");
                GetSFTPfile("/FromDWH/LN04DW" + revendt + ".txt", "D:\\webapp\\scoring\\FTP\\IN\\DWH\\LN04DW" + revendt + ".txt");
                GetSFTPfile("/FromDWH/LN05DW" + revendt + ".txt", "D:\\webapp\\scoring\\FTP\\IN\\DWH\\LN05DW" + revendt + ".txt");
                GetSFTPfile("/FromDWH/LN06DW" + revendt + ".txt", "D:\\webapp\\scoring\\FTP\\IN\\DWH\\LN06DW" + revendt + ".txt");
                GetSFTPfile("/FromDWH/LN07DW" + revendt + ".txt", "D:\\webapp\\scoring\\FTP\\IN\\DWH\\LN07DW" + revendt + ".txt");
                UpdateTable(revendt);
                UpdateTable1(revendt1);
                UpdateTable2(revendt2);
                UpdateTable3(revendt3);
                UpdateTable4(revendt4);
                UpdateTable5(revendt5);
                UpdateTable6(revendt6);
                UpdateTable7(revendt7);
                ExecuteStored();
                Label2.Text = "ดำเนินการเสร็จสิ้น";
            }
            else
            {
               // this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('You clicked NO!')", true);
                Response.Redirect("../LoanSystem/LoadDWH.aspx");
            }

        }


        static void GetSFTPfile(String Remotefile, String Localfile)
        {
            string tmestart = DateTime.Now.Year.ToString("0000") + DateTime.Now.ToString("-MM-dd HH:mm:ss.") + DateTime.Now.Millisecond.ToString();
            try
            {

                // Setup session options
                //(ConfigurationManager.AppSettings["FTPIP"].ToString()
                string protocaluse = (ConfigurationManager.AppSettings["Protcl"].ToString());
                SessionOptions sessionOptions = new SessionOptions();
                if (protocaluse == "SFTP")
                {
                    sessionOptions.Protocol = Protocol.Sftp;
                    sessionOptions.HostName = ConfigurationManager.AppSettings["dwhftphost"].ToString();
                    sessionOptions.UserName = ConfigurationManager.AppSettings["dwhftpuser"].ToString();
                    sessionOptions.Password = ConfigurationManager.AppSettings["dwhftppassword"].ToString();
                    //HostName = "10.4.31.80",
                    //UserName = "locrsuat",
                    //Password = "locrsuat",
                    ////                SshHostKey = "ssh-rsa 2048 f4:99:dc:54:13:8d:a1:1d:99:0b:d8:92:77:50:4b:1b"
                    sessionOptions.SshHostKey = ConfigurationManager.AppSettings["dwhftphostkey"].ToString();
                    sessionOptions.SshPrivateKey = ConfigurationManager.AppSettings["dwhftpprikey"].ToString();
                }
                else
                {
                    sessionOptions.Protocol = Protocol.Ftp;
                    sessionOptions.HostName = ConfigurationManager.AppSettings["dwhftphost"].ToString();
                    sessionOptions.UserName = ConfigurationManager.AppSettings["dwhftpuser"].ToString();
                    sessionOptions.Password = ConfigurationManager.AppSettings["dwhftppassword"].ToString();
                    FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://" + ConfigurationManager.AppSettings["dwhftphost"].ToString() + Remotefile);
                    //FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://10.20.4.250/FromDWH/LN01DW201212.txt");
                    request.Method = WebRequestMethods.Ftp.DownloadFile;
                    request.UseBinary = true;
                    // request.Credentials = new NetworkCredential("crscore", "8w!5n)Ag%");
                    request.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["dwhftpuser"].ToString(), ConfigurationManager.AppSettings["dwhftppassword"].ToString());
                    FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                    Stream responseStream = response.GetResponseStream();
                    StreamReader reader = new StreamReader(responseStream, Encoding.Default);
                    //Console.WriteLine(reader.ReadToEnd());
                    StreamWriter oWriter = new StreamWriter((Localfile), false, Encoding.Default);
                    oWriter.WriteLine(reader.ReadToEnd());
                    //Console.WriteLine("Directory List Complete, status {0}", response.StatusDescription);
                    oWriter.Close();
                    reader.Close();
                    response.Close();

                    SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["FTPIP"].ToString());
                    string tmesend = DateTime.Now.Year.ToString("0000") + DateTime.Now.ToString("-MM-dd HH:mm:ss.") + DateTime.Now.Millisecond.ToString();
                    string sqlStatementdx = "insert into s_log_import values('BATCH_DWH',convert(datetime,'" + tmestart.ToString() + "',121),convert(datetime,'" + tmesend.ToString() + "',121),0,'DONE','" + Localfile.ToString() + "','003')";
                    con.Open();
                    SqlCommand cmddlx = new SqlCommand(sqlStatementdx, con);
                    cmddlx.ExecuteNonQuery();
                    con.Close();
                }

                if (protocaluse == "SFTP")
                {

                    using (Session session = new Session())
                    {
                        // Connect
                        session.Open(sessionOptions);

                        string stamp = DateTime.Now.ToString("yyyyMMdd", CultureInfo.InvariantCulture);
                        //string fileName = "COLLINFO25550630.csv";
                        //string remotePath = "/LOPs/UAT/CMS/CollateralScoring/FromCMS/" + fileName;
                        //string localPath = "d:\\BOL\\" + fileName;
                        string remotePath = Remotefile;
                        string localPath = Localfile;

                        // Manual "remote to local" synchronization.

                        // You can achieve the same using:
                        // session.SynchronizeDirectories(
                        //     SynchronizationMode.Local, localPath, remotePath, false, false, SynchronizationCriteria.Time, 
                        //     new TransferOptions { IncludeMask = fileName }).Check();
                        if (session.FileExists(remotePath))
                        {
                            bool download;
                            if (!File.Exists(localPath))
                            {
                                Console.WriteLine("File {0} exists, local backup {1} does not", remotePath, localPath);
                                download = true;
                            }
                            else
                            {
                                DateTime remoteWriteTime = session.GetFileInfo(remotePath).LastWriteTime;
                                DateTime localWriteTime = File.GetLastWriteTime(localPath);

                                if (remoteWriteTime > localWriteTime)
                                {
                                    Console.WriteLine(
                                        "File {0} as well as local backup {1} exist, " +
                                        "but remote file is newer ({2}) than local backup ({3})",
                                        remotePath, localPath, remoteWriteTime, localWriteTime);
                                    download = true;
                                }
                                else
                                {
                                    Console.WriteLine(
                                        "File {0} as well as local backup {1} exist, " +
                                        "but remote file is not newer ({2}) than local backup ({3})",
                                        remotePath, localPath, remoteWriteTime, localWriteTime);
                                    download = false;
                                }
                            }

                            if (download)
                            {
                                // Download the file and throw on any error
                                session.GetFiles(remotePath, localPath).Check();

                                Console.WriteLine("Download to backup done.");
                                //    return "Done";
                            }
                        }
                        else
                        {
                            Console.WriteLine("File {0} does not exist yet", remotePath);
                            //  return "False";
                        }
                    }
                }
                //   return "Done";
                //            return 0;
                SqlConnection cons = new SqlConnection(ConfigurationManager.AppSettings["FTPIP"].ToString());
                string tmesend2 = DateTime.Now.Year.ToString("0000") + DateTime.Now.ToString("-MM-dd HH:mm:ss.") + DateTime.Now.Millisecond.ToString();
                string sqlStatementdx2 = "insert into s_log_import values('BATCH_DWH',convert(datetime,'" + tmestart.ToString() + "',121),convert(datetime,'" + tmesend2.ToString() + "',121),0,'DONE','" + Localfile.ToString() + "','003')";
                cons.Open();
                SqlCommand cmddlx2 = new SqlCommand(sqlStatementdx2, cons);
                cmddlx2.ExecuteNonQuery();
                cons.Close();

            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e);
                //          return 1;
                //     return "False";
                string inExc = e.Message;
                SqlConnection errcon = new SqlConnection(ConfigurationManager.AppSettings["FTPIP"].ToString());
                errcon.Open();
                string sqlStatementderr = "insert into s_log_import values('BATCH_DWH',convert(datetime,'" + tmestart.ToString() + "',121),getdate(),0,'FAIL','" + inExc.ToString().Replace("'", "''") + "','003')";
                SqlCommand cmddlerr = new SqlCommand(sqlStatementderr, errcon);
                cmddlerr.ExecuteNonQuery();
                errcon.Close();
            }


        }


        static void UpdateTable(string revendt1)
        {
            //Console.WriteLine(reader.ReadToEnd());
            //"Data Source=(Local);Initial Catalog=GSBBlaze;UID=sa;PWD=abcd@1234"
            //StreamWriter oWriter = new StreamWriter(("D:\\FTP\\IN\\MASTER\\TEXT.TXT"), false, Encoding.Default);
            string tmestart = DateTime.Now.Year.ToString("0000") + DateTime.Now.ToString("-MM-dd HH:mm:ss.") + DateTime.Now.Millisecond.ToString();

            using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["FTPIP"].ToString()))
                //{
                //    //**************DWH_LOAN_TYPE_01******************
                try
                {

                    string[] importfiles = Directory.GetFiles(@"D:\webapp\scoring\FTP\IN\DWH", "CR_" + revendt1 + ".txt");
                    string sqlStatement =
                        "INSERT INTO dbo.TMP_DWH_BTFILE(CBS_APP_NO, DW_ACC_NO, DW_FDISDATE, DW_CDATE, DW_EXPDATE," +
                        "DW_SIGN_DATE,ISACTIVE, DW_BADMONTH, DW_OUTSTAND, LAST_UPDATE_DTM, DW_BADAMOUNT,DW_ORG_TERM," +
                        "DW_FREQ_TERM, DW_ORG_TERM_MULT, DW_COLLVALUE, CSTATUS_CD,DW_ISRESTRUCT, LOAN_TYPE, FILE_LOAD_DATE) " +
                        "VALUES(@D1, @D2, @D3 , @D4,@D5, @D6, @D7 , @D8,@D9, @D10, @D11 , @D12,@D13, @D14, @D15 , @D16,@D17,@D18,@D19)";
                    using (SqlCommand cmd = new SqlCommand(sqlStatement, con))
                    {

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
                        //cmd.Parameters.Add("@D4", SqlDbType.Bit);
                        string SQLuplnApp = "";
                        foreach (string importfile in importfiles)
                        {
                            con.Open();
                            string[] allLines = File.ReadAllLines(importfile);
                            if (allLines.Length > 1)
                            {
                                string sqlStatementdl = "truncate table dbo.TMP_DWH_BTFILE";
                                SqlCommand cmddl = new SqlCommand(sqlStatementdl, con);
                                cmddl.CommandTimeout = 0;
                                cmddl.ExecuteNonQuery();
                            }
                            int ttl = 0;
                            string fldatestmp = "";
                            for (int index = 0; index < allLines.Length; index++)
                            {
                                if (index == 0)
                                {
                                    fldatestmp = allLines[0].ToString();
                                }
                                else
                                {
                                    if (allLines[index].Length > 0)
                                    {
                                        string[] items = allLines[index].Split(new char[] { '|' });
                                        cmd.Parameters["@D1"].Value = items[2];
                                        cmd.Parameters["@D2"].Value = items[3];
                                        cmd.Parameters["@D3"].Value = "";
                                        cmd.Parameters["@D4"].Value = "";
                                        cmd.Parameters["@D5"].Value = "";
                                        cmd.Parameters["@D6"].Value = items[4];
                                        cmd.Parameters["@D7"].Value = "0";
                                        cmd.Parameters["@D8"].Value = items[8];
                                        cmd.Parameters["@D9"].Value = items[9];
                                        cmd.Parameters["@D10"].Value = items[10];
                                        cmd.Parameters["@D11"].Value = "";
                                        cmd.Parameters["@D12"].Value = "";
                                        cmd.Parameters["@D13"].Value = "";
                                        cmd.Parameters["@D14"].Value = "";
                                        cmd.Parameters["@D15"].Value = "";
                                        cmd.Parameters["@D16"].Value = items[13];
                                        cmd.Parameters["@D17"].Value = "";
                                        cmd.Parameters["@D18"].Value = items[12];
                                        cmd.Parameters["@D19"].Value = fldatestmp.TrimEnd();
                                        //cmd.Parameters["@D4"].Value = 1;

                                        cmd.ExecuteNonQuery();
                                        //cmd.CommandTimeout = 0;
                                        //SQLuplnApp = "Update dbo.LN_APP set DWH_BADMNT ='" + items[8] + "' where APP_NO = RIGHT('000000000000' + '" + items[2] + "',12)";
                                        //SqlCommand cmddlu = new SqlCommand(SQLuplnApp, con);
                                        //cmddlu.ExecuteNonQuery();
                                    }
                                    ttl++;
                                }
                            }
                            string sqlStatementdx = "insert into s_log_import values('DWH_LOAN01',convert(datetime,'" + tmestart.ToString() + "',121),getdate()," + ttl.ToString() + ",'DONE',NULL,'003')";
                            SqlCommand cmddlx = new SqlCommand(sqlStatementdx, con);
                            cmddlx.ExecuteNonQuery();
                            con.Close();
                        }
                    }
                }
                catch (Exception ex)
                {
                    string inExc = ex.Message;
                    SqlConnection errcon = new SqlConnection(ConfigurationManager.AppSettings["FTPIP"].ToString());
                    errcon.Open();
                    string sqlStatementderr = "insert into s_log_import values('DWH_LOAN01',convert(datetime,'" + tmestart.ToString() + "',121),getdate(),0,'FAIL','" + inExc.ToString().Replace("'", "''") + "','003')";
                    SqlCommand cmddlerr = new SqlCommand(sqlStatementderr, errcon);
                    cmddlerr.ExecuteNonQuery();
                    errcon.Close();
                }


        }

        static void UpdateTable1(string revendt1) 
        {
            //Console.WriteLine(reader.ReadToEnd());
            //"Data Source=(Local);Initial Catalog=GSBBlaze;UID=sa;PWD=abcd@1234"
            //StreamWriter oWriter = new StreamWriter(("D:\\FTP\\IN\\MASTER\\TEXT.TXT"), false, Encoding.Default);
            string tmestart = DateTime.Now.Year.ToString("0000") + DateTime.Now.ToString("-MM-dd HH:mm:ss.") + DateTime.Now.Millisecond.ToString();

            using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["FTPIP"].ToString()))
                //{
                //    //**************DWH_LOAN_TYPE_01******************
                try
                {

                    string[] importfiles = Directory.GetFiles(@"D:\webapp\scoring\FTP\IN\DWH", "LN01DW" + revendt1 + ".txt");
                    string sqlStatement =
                        "INSERT INTO dbo.TMP_DWH_BTFILE(CBS_APP_NO, DW_ACC_NO, DW_FDISDATE, DW_CDATE, DW_EXPDATE," +
                        "DW_SIGN_DATE,ISACTIVE, DW_BADMONTH, DW_OUTSTAND, LAST_UPDATE_DTM, DW_BADAMOUNT,DW_ORG_TERM," +
                        "DW_FREQ_TERM, DW_ORG_TERM_MULT, DW_COLLVALUE, CSTATUS_CD,DW_ISRESTRUCT, LOAN_TYPE, FILE_LOAD_DATE) " +
                        "VALUES(@D1, @D2, @D3 , @D4,@D5, @D6, @D7 , @D8,@D9, @D10, @D11 , @D12,@D13, @D14, @D15 , @D16,@D17,@D18,@D19)";
                    using (SqlCommand cmd = new SqlCommand(sqlStatement, con) )
                    {
                       
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
                        //cmd.Parameters.Add("@D4", SqlDbType.Bit);
                        string SQLuplnApp = "";
                        foreach (string importfile in importfiles)
                        {
                            con.Open();
                            string[] allLines = File.ReadAllLines(importfile, Encoding.Default);
                            if (allLines.Length > 1)
                            {
                                string sqlStatementdl = "truncate table dbo.TMP_DWH_BTFILE";
                                SqlCommand cmddl = new SqlCommand(sqlStatementdl, con);
                                cmddl.CommandTimeout = 0;
                                cmddl.ExecuteNonQuery();
                            }
                            int ttl = 0;
                            string fldatestmp = "";
                            for (int index = 0; index < allLines.Length; index++)
                            {
                                if (index == 0)
                                {
                                    fldatestmp = allLines[0].ToString();
                                }
                                else
                                {
                                    if (allLines[index].Length > 0)
                                    {
                                        string[] items = allLines[index].Split(new char[] { ',' });
                                        cmd.Parameters["@D1"].Value = items[0];
                                        cmd.Parameters["@D2"].Value = items[1];
                                        cmd.Parameters["@D3"].Value = items[2];
                                        cmd.Parameters["@D4"].Value = items[3];
                                        cmd.Parameters["@D5"].Value = items[4];
                                        cmd.Parameters["@D6"].Value = items[5];
                                        cmd.Parameters["@D7"].Value = items[6];
                                        cmd.Parameters["@D8"].Value = items[7];
                                        cmd.Parameters["@D9"].Value = items[8];
                                        cmd.Parameters["@D10"].Value = items[9];
                                        cmd.Parameters["@D11"].Value = items[10];
                                        cmd.Parameters["@D12"].Value = items[11];
                                        cmd.Parameters["@D13"].Value = items[12];
                                        cmd.Parameters["@D14"].Value = items[13];
                                        cmd.Parameters["@D15"].Value = items[14];
                                        cmd.Parameters["@D16"].Value = items[15];
                                        cmd.Parameters["@D17"].Value = items[16];
                                        cmd.Parameters["@D18"].Value = "01";
                                        cmd.Parameters["@D19"].Value = fldatestmp.TrimEnd();
                                        //cmd.Parameters["@D4"].Value = 1;
                                        
                                        cmd.ExecuteNonQuery();
                                        cmd.CommandTimeout = 0;
                                        SQLuplnApp = "Update dbo.LN_APP set DWH_BADMNT ='" + items[7] + "' where APP_NO = RIGHT('000000000000' + '" + items[0] + "',12)";
                                        SqlCommand cmddlu = new SqlCommand(SQLuplnApp, con);
                                        cmddlu.ExecuteNonQuery();
                                    }
                                    ttl++;
                                }
                            }
                            string sqlStatementdx = "insert into s_log_import values('DWH_LOAN01',convert(datetime,'" + tmestart.ToString() + "',121),getdate()," + ttl.ToString() + ",'DONE',NULL,'003')";
                            SqlCommand cmddlx = new SqlCommand(sqlStatementdx, con);
                            cmddlx.ExecuteNonQuery();
                            con.Close();
                        }
                    }
                }
                catch (Exception ex)
                {
                    string inExc = ex.Message;
                    SqlConnection errcon = new SqlConnection(ConfigurationManager.AppSettings["FTPIP"].ToString());
                    errcon.Open();
                    string sqlStatementderr = "insert into s_log_import values('DWH_LOAN01',convert(datetime,'" + tmestart.ToString() + "',121),getdate(),0,'FAIL','" + inExc.ToString().Replace("'", "''") + "','003')";
                    SqlCommand cmddlerr = new SqlCommand(sqlStatementderr, errcon);
                    cmddlerr.ExecuteNonQuery();
                    errcon.Close();
                }


        }
        static void UpdateTable2(string revendt2)
        {
            //Console.WriteLine(reader.ReadToEnd());
            //"Data Source=(Local);Initial Catalog=GSBBlaze;UID=sa;PWD=abcd@1234"
            //StreamWriter oWriter = new StreamWriter(("D:\\FTP\\IN\\MASTER\\TEXT.TXT"), false, Encoding.Default);
            string tmestart = DateTime.Now.Year.ToString("0000") + DateTime.Now.ToString("-MM-dd HH:mm:ss.") + DateTime.Now.Millisecond.ToString();

            using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["FTPIP"].ToString()))
                //{
                //    //**************DWH_LOAN_TYPE_02******************
                try
                {

                    //Console.WriteLine(reader.ReadToEnd());
                    //sql = "INSERT INTO " + TempTableName + " " +
                    //          "(CBS_APP_NO, DW_ACC_NO, DW_FDISDATE, DW_CDATE, DW_EXPDATE, DW_SIGN_DATE, " +
                    //          "ISACTIVE, DW_BADMONTH, DW_OUTSTAND, LAST_UPDATE_DTM, DW_BADAMOUNT, " +
                    //          "DW_ORG_TERM, DW_FREQ_TERM, DW_ORG_TERM_MULT, DW_COLLVALUE, CSTATUS_CD, " +
                    //          "DW_ISRESTRUCT, LOAN_TYPE) " +
                    //          "VALUES('" + strArray[0] + "', '" + strArray[1] + "', " +
                    //          "'" + strArray[2] + "', '" + strArray[3] + "', " +
                    //          "'" + strArray[4] + "', '" + strArray[5] + "', " +
                    //          "'" + strArray[6] + "', '" + strArray[7] + "', " +
                    //          "'" + strArray[8] + "', '" + strArray[9] + "', " +
                    //          "'" + strArray[10] + "', '" + strArray[11] + "', " +
                    //          "'" + strArray[12] + "', '" + strArray[13] + "', " +
                    //          "'" + strArray[14] + "', '" + strArray[15] + "', " +
                    //          "'" + strArray[16].Trim() + "', '" + i.ToString("00") + "')";
                    //string revendt = DateTime.Now.AddMonths(-1).Year.ToString("0000") + DateTime.Now.AddMonths(-1).ToString("MM");

                    string[] importfiles = Directory.GetFiles(@"D:\webapp\scoring\FTP\IN\DWH", "LN02DW" + revendt2 + ".txt"); string sqlStatement =
                        "INSERT INTO dbo.TMP_DWH_BTFILE(CBS_APP_NO, DW_ACC_NO, DW_FDISDATE, DW_CDATE, DW_EXPDATE," +
                        "DW_SIGN_DATE,ISACTIVE, DW_BADMONTH, DW_OUTSTAND, LAST_UPDATE_DTM, DW_BADAMOUNT,DW_ORG_TERM," +
                        "DW_FREQ_TERM, DW_ORG_TERM_MULT, DW_COLLVALUE, CSTATUS_CD,DW_ISRESTRUCT, LOAN_TYPE, FILE_LOAD_DATE) " +
                        "VALUES(@D1, @D2, @D3 , @D4,@D5, @D6, @D7 , @D8,@D9, @D10, @D11 , @D12,@D13, @D14, @D15 , @D16,@D17,@D18,@D19)";
                    using (SqlCommand cmd = new SqlCommand(sqlStatement, con))
                    {
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
                        //cmd.Parameters.Add("@D4", SqlDbType.Bit);
                        string SQLuplnApp = "";
                        foreach (string importfile in importfiles)
                        {
                            con.Open();
                            string[] allLines = File.ReadAllLines(importfile, Encoding.Default);
                            if (allLines.Length > 1)
                            {
                                //string sqlStatementdl = "delete from dbo.ST_PURPOSE";
                                //SqlCommand cmddl = new SqlCommand(sqlStatementdl, con);
                                //cmddl.ExecuteNonQuery();
                            }
                            int ttl = 0;
                            string fldatestmp = "";
                            for (int index = 0; index < allLines.Length; index++)
                            {
                                if (index == 0)
                                {
                                    fldatestmp = allLines[0].ToString();
                                }
                                else
                                {
                                    if (allLines[index].Length > 0)
                                    {
                                        string[] items = allLines[index].Split(new char[] { ',' });
                                        cmd.Parameters["@D1"].Value = items[0];
                                        cmd.Parameters["@D2"].Value = items[1];
                                        cmd.Parameters["@D3"].Value = items[2];
                                        cmd.Parameters["@D4"].Value = items[3];
                                        cmd.Parameters["@D5"].Value = items[4];
                                        cmd.Parameters["@D6"].Value = items[5];
                                        cmd.Parameters["@D7"].Value = items[6];
                                        cmd.Parameters["@D8"].Value = items[7];
                                        cmd.Parameters["@D9"].Value = items[8];
                                        cmd.Parameters["@D10"].Value = items[9];
                                        cmd.Parameters["@D11"].Value = items[10];
                                        cmd.Parameters["@D12"].Value = items[11];
                                        cmd.Parameters["@D13"].Value = items[12];
                                        cmd.Parameters["@D14"].Value = items[13];
                                        cmd.Parameters["@D15"].Value = items[14];
                                        cmd.Parameters["@D16"].Value = items[15];
                                        cmd.Parameters["@D17"].Value = items[16];
                                        cmd.Parameters["@D18"].Value = "02";
                                        cmd.Parameters["@D19"].Value = fldatestmp.TrimEnd();
                                        //cmd.Parameters["@D4"].Value = 1;

                                        cmd.ExecuteNonQuery();
                                        SQLuplnApp = "Update dbo.LN_APP set DWH_BADMNT ='" + items[7] + "' where APP_NO = RIGHT('000000000000' + '" + items[0] + "',12)";
                                        SqlCommand cmddlu = new SqlCommand(SQLuplnApp, con);
                                        cmddlu.ExecuteNonQuery();
                                    }
                                    ttl++;
                                }
                            }
                            string sqlStatementdx = "insert into s_log_import values('DWH_LOAN02',convert(datetime,'" + tmestart.ToString() + "',121),getdate()," + ttl.ToString() + ",'DONE',NULL,'003')";
                            SqlCommand cmddlx = new SqlCommand(sqlStatementdx, con);
                            cmddlx.ExecuteNonQuery();
                            con.Close();
                        }
                    }
                }
                catch (Exception ex)
                {
                    string inExc = ex.Message;
                    SqlConnection errcon = new SqlConnection(ConfigurationManager.AppSettings["FTPIP"].ToString());
                    errcon.Open();
                    string sqlStatementderr = "insert into s_log_import values('DWH_LOAN02',convert(datetime,'" + tmestart.ToString() + "',121),getdate(),0,'FAIL','" + inExc.ToString().Replace("'", "''") + "','003')";
                    SqlCommand cmddlerr = new SqlCommand(sqlStatementderr, errcon);
                    cmddlerr.ExecuteNonQuery();
                    errcon.Close();
                }


        }
        static void UpdateTable3(string revendt3)
        {
            //Console.WriteLine(reader.ReadToEnd());
            //"Data Source=(Local);Initial Catalog=GSBBlaze;UID=sa;PWD=abcd@1234"
            //StreamWriter oWriter = new StreamWriter(("D:\\FTP\\IN\\MASTER\\TEXT.TXT"), false, Encoding.Default);
            string tmestart = DateTime.Now.Year.ToString("0000") + DateTime.Now.ToString("-MM-dd HH:mm:ss.") + DateTime.Now.Millisecond.ToString();

            using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["FTPIP"].ToString()))
                //{
                //    //**************DWH_LOAN_TYPE_03******************
                try
                {

                    //Console.WriteLine(reader.ReadToEnd());
                    //sql = "INSERT INTO " + TempTableName + " " +
                    //          "(CBS_APP_NO, DW_ACC_NO, DW_FDISDATE, DW_CDATE, DW_EXPDATE, DW_SIGN_DATE, " +
                    //          "ISACTIVE, DW_BADMONTH, DW_OUTSTAND, LAST_UPDATE_DTM, DW_BADAMOUNT, " +
                    //          "DW_ORG_TERM, DW_FREQ_TERM, DW_ORG_TERM_MULT, DW_COLLVALUE, CSTATUS_CD, " +
                    //          "DW_ISRESTRUCT, LOAN_TYPE) " +
                    //          "VALUES('" + strArray[0] + "', '" + strArray[1] + "', " +
                    //          "'" + strArray[2] + "', '" + strArray[3] + "', " +
                    //          "'" + strArray[4] + "', '" + strArray[5] + "', " +
                    //          "'" + strArray[6] + "', '" + strArray[7] + "', " +
                    //          "'" + strArray[8] + "', '" + strArray[9] + "', " +
                    //          "'" + strArray[10] + "', '" + strArray[11] + "', " +
                    //          "'" + strArray[12] + "', '" + strArray[13] + "', " +
                    //          "'" + strArray[14] + "', '" + strArray[15] + "', " +
                    //          "'" + strArray[16].Trim() + "', '" + i.ToString("00") + "')";
                    //string revendt = DateTime.Now.AddMonths(-1).Year.ToString("0000") + DateTime.Now.AddMonths(-1).ToString("MM");

                    string[] importfiles = Directory.GetFiles(@"D:\webapp\scoring\FTP\IN\DWH", "LN03DW" + revendt3 + ".txt"); string sqlStatement =
                        "INSERT INTO dbo.TMP_DWH_BTFILE(CBS_APP_NO, DW_ACC_NO, DW_FDISDATE, DW_CDATE, DW_EXPDATE," +
                        "DW_SIGN_DATE,ISACTIVE, DW_BADMONTH, DW_OUTSTAND, LAST_UPDATE_DTM, DW_BADAMOUNT,DW_ORG_TERM," +
                        "DW_FREQ_TERM, DW_ORG_TERM_MULT, DW_COLLVALUE, CSTATUS_CD,DW_ISRESTRUCT, LOAN_TYPE, FILE_LOAD_DATE) " +
                        "VALUES(@D1, @D2, @D3 , @D4,@D5, @D6, @D7 , @D8,@D9, @D10, @D11 , @D12,@D13, @D14, @D15 , @D16,@D17,@D18,@D19)";
                    using (SqlCommand cmd = new SqlCommand(sqlStatement, con))
                    {
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
                        //cmd.Parameters.Add("@D4", SqlDbType.Bit);
                        string SQLuplnApp = "";
                        foreach (string importfile in importfiles)
                        {
                            con.Open();
                            string[] allLines = File.ReadAllLines(importfile, Encoding.Default);
                            if (allLines.Length > 1)
                            {
                                //string sqlStatementdl = "delete from dbo.ST_PURPOSE";
                                //SqlCommand cmddl = new SqlCommand(sqlStatementdl, con);
                                //cmddl.ExecuteNonQuery();
                            }
                            int ttl = 0;
                            string fldatestmp = "";
                            for (int index = 0; index < allLines.Length; index++)
                            {
                                if (index == 0)
                                {
                                    fldatestmp = allLines[0].ToString();
                                }
                                else
                                {
                                    if (allLines[index].Length > 0)
                                    {
                                        string[] items = allLines[index].Split(new char[] { ',' });
                                        cmd.Parameters["@D1"].Value = items[0];
                                        cmd.Parameters["@D2"].Value = items[1];
                                        cmd.Parameters["@D3"].Value = items[2];
                                        cmd.Parameters["@D4"].Value = items[3];
                                        cmd.Parameters["@D5"].Value = items[4];
                                        cmd.Parameters["@D6"].Value = items[5];
                                        cmd.Parameters["@D7"].Value = items[6];
                                        cmd.Parameters["@D8"].Value = items[7];
                                        cmd.Parameters["@D9"].Value = items[8];
                                        cmd.Parameters["@D10"].Value = items[9];
                                        cmd.Parameters["@D11"].Value = items[10];
                                        cmd.Parameters["@D12"].Value = items[11];
                                        cmd.Parameters["@D13"].Value = items[12];
                                        cmd.Parameters["@D14"].Value = items[13];
                                        cmd.Parameters["@D15"].Value = items[14];
                                        cmd.Parameters["@D16"].Value = items[15];
                                        cmd.Parameters["@D17"].Value = items[16];
                                        cmd.Parameters["@D18"].Value = "03";
                                        cmd.Parameters["@D19"].Value = fldatestmp.TrimEnd();
                                        //cmd.Parameters["@D4"].Value = 1;

                                        cmd.ExecuteNonQuery();
                                        SQLuplnApp = "Update dbo.LN_APP set DWH_BADMNT ='" + items[7] + "' where APP_NO = RIGHT('000000000000' + '" + items[0] + "',12)";
                                        SqlCommand cmddlu = new SqlCommand(SQLuplnApp, con);
                                        cmddlu.ExecuteNonQuery();
                                    }
                                    ttl++;
                                }
                            }
                            string sqlStatementdx = "insert into s_log_import values('DWH_LOAN03',convert(datetime,'" + tmestart.ToString() + "',121),getdate()," + ttl.ToString() + ",'DONE',NULL,'003')";
                            SqlCommand cmddlx = new SqlCommand(sqlStatementdx, con);
                            cmddlx.ExecuteNonQuery();
                            con.Close();
                        }
                    }
                }
                catch (Exception ex)
                {
                    string inExc = ex.Message;
                    SqlConnection errcon = new SqlConnection(ConfigurationManager.AppSettings["FTPIP"].ToString());
                    errcon.Open();
                    string sqlStatementderr = "insert into s_log_import values('DWH_LOAN03',convert(datetime,'" + tmestart.ToString() + "',121),getdate(),0,'FAIL','" + inExc.ToString().Replace("'", "''") + "','003')";
                    SqlCommand cmddlerr = new SqlCommand(sqlStatementderr, errcon);
                    cmddlerr.ExecuteNonQuery();
                    errcon.Close();
                }


        }
        static void UpdateTable4(string revendt4)
        {
            //Console.WriteLine(reader.ReadToEnd());
            //"Data Source=(Local);Initial Catalog=GSBBlaze;UID=sa;PWD=abcd@1234"
            //StreamWriter oWriter = new StreamWriter(("D:\\FTP\\IN\\MASTER\\TEXT.TXT"), false, Encoding.Default);
            string tmestart = DateTime.Now.Year.ToString("0000") + DateTime.Now.ToString("-MM-dd HH:mm:ss.") + DateTime.Now.Millisecond.ToString();

            using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["FTPIP"].ToString()))
                //{
                //    //**************DWH_LOAN_TYPE_04******************
                try
                {

                    //Console.WriteLine(reader.ReadToEnd());
                    //sql = "INSERT INTO " + TempTableName + " " +
                    //          "(CBS_APP_NO, DW_ACC_NO, DW_FDISDATE, DW_CDATE, DW_EXPDATE, DW_SIGN_DATE, " +
                    //          "ISACTIVE, DW_BADMONTH, DW_OUTSTAND, LAST_UPDATE_DTM, DW_BADAMOUNT, " +
                    //          "DW_ORG_TERM, DW_FREQ_TERM, DW_ORG_TERM_MULT, DW_COLLVALUE, CSTATUS_CD, " +
                    //          "DW_ISRESTRUCT, LOAN_TYPE) " +
                    //          "VALUES('" + strArray[0] + "', '" + strArray[1] + "', " +
                    //          "'" + strArray[2] + "', '" + strArray[3] + "', " +
                    //          "'" + strArray[4] + "', '" + strArray[5] + "', " +
                    //          "'" + strArray[6] + "', '" + strArray[7] + "', " +
                    //          "'" + strArray[8] + "', '" + strArray[9] + "', " +
                    //          "'" + strArray[10] + "', '" + strArray[11] + "', " +
                    //          "'" + strArray[12] + "', '" + strArray[13] + "', " +
                    //          "'" + strArray[14] + "', '" + strArray[15] + "', " +
                    //          "'" + strArray[16].Trim() + "', '" + i.ToString("00") + "')";
                    //string revendt = DateTime.Now.AddMonths(-1).Year.ToString("0000") + DateTime.Now.AddMonths(-1).ToString("MM");

                    string[] importfiles = Directory.GetFiles(@"D:\webapp\scoring\FTP\IN\DWH", "LN04DW" + revendt4 + ".txt"); string sqlStatement =
                        "INSERT INTO dbo.TMP_DWH_BTFILE(CBS_APP_NO, DW_ACC_NO, DW_FDISDATE, DW_CDATE, DW_EXPDATE," +
                        "DW_SIGN_DATE,ISACTIVE, DW_BADMONTH, DW_OUTSTAND, LAST_UPDATE_DTM, DW_BADAMOUNT,DW_ORG_TERM," +
                        "DW_FREQ_TERM, DW_ORG_TERM_MULT, DW_COLLVALUE, CSTATUS_CD,DW_ISRESTRUCT, LOAN_TYPE, FILE_LOAD_DATE) " +
                        "VALUES(@D1, @D2, @D3 , @D4,@D5, @D6, @D7 , @D8,@D9, @D10, @D11 , @D12,@D13, @D14, @D15 , @D16,@D17,@D18,@D19)";
                    using (SqlCommand cmd = new SqlCommand(sqlStatement, con))
                    {
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
                        //cmd.Parameters.Add("@D4", SqlDbType.Bit);
                        string SQLuplnApp = "";
                        foreach (string importfile in importfiles)
                        {
                            con.Open();
                            string[] allLines = File.ReadAllLines(importfile, Encoding.Default);
                            if (allLines.Length > 1)
                            {
                                //string sqlStatementdl = "delete from dbo.ST_PURPOSE";
                                //SqlCommand cmddl = new SqlCommand(sqlStatementdl, con);
                                //cmddl.ExecuteNonQuery();
                            }
                            int ttl = 0;
                            string fldatestmp = "";
                            for (int index = 0; index < allLines.Length; index++)
                            {
                                if (index == 0)
                                {
                                    fldatestmp = allLines[0].ToString();
                                }
                                else
                                {
                                    if (allLines[index].Length > 0)
                                    {
                                        string[] items = allLines[index].Split(new char[] { ',' });
                                        cmd.Parameters["@D1"].Value = items[0];
                                        cmd.Parameters["@D2"].Value = items[1];
                                        cmd.Parameters["@D3"].Value = items[2];
                                        cmd.Parameters["@D4"].Value = items[3];
                                        cmd.Parameters["@D5"].Value = items[4];
                                        cmd.Parameters["@D6"].Value = items[5];
                                        cmd.Parameters["@D7"].Value = items[6];
                                        cmd.Parameters["@D8"].Value = items[7];
                                        cmd.Parameters["@D9"].Value = items[8];
                                        cmd.Parameters["@D10"].Value = items[9];
                                        cmd.Parameters["@D11"].Value = items[10];
                                        cmd.Parameters["@D12"].Value = items[11];
                                        cmd.Parameters["@D13"].Value = items[12];
                                        cmd.Parameters["@D14"].Value = items[13];
                                        cmd.Parameters["@D15"].Value = items[14];
                                        cmd.Parameters["@D16"].Value = items[15];
                                        cmd.Parameters["@D17"].Value = items[16];
                                        cmd.Parameters["@D18"].Value = "04";
                                        cmd.Parameters["@D19"].Value = fldatestmp.TrimEnd();
                                        //cmd.Parameters["@D4"].Value = 1;

                                        cmd.ExecuteNonQuery();
                                        SQLuplnApp = "Update dbo.LN_APP set DWH_BADMNT ='" + items[7] + "' where APP_NO = RIGHT('000000000000' + '" + items[0] + "',12)";
                                        SqlCommand cmddlu = new SqlCommand(SQLuplnApp, con);
                                        cmddlu.ExecuteNonQuery();
                                    }
                                    ttl++;
                                }
                            }
                            string sqlStatementdx = "insert into s_log_import values('DWH_LOAN04',convert(datetime,'" + tmestart.ToString() + "',121),getdate()," + ttl.ToString() + ",'DONE',NULL,'003')";
                            SqlCommand cmddlx = new SqlCommand(sqlStatementdx, con);
                            cmddlx.ExecuteNonQuery();
                            con.Close();
                        }
                    }
                }
                catch (Exception ex)
                {
                    string inExc = ex.Message;
                    SqlConnection errcon = new SqlConnection(ConfigurationManager.AppSettings["FTPIP"].ToString());
                    errcon.Open();
                    string sqlStatementderr = "insert into s_log_import values('DWH_LOAN04',convert(datetime,'" + tmestart.ToString() + "',121),getdate(),0,'FAIL','" + inExc.ToString().Replace("'", "''") + "','003')";
                    SqlCommand cmddlerr = new SqlCommand(sqlStatementderr, errcon);
                    cmddlerr.ExecuteNonQuery();
                    errcon.Close();
                }


        }
        static void UpdateTable5(string revendt5)
        {
            //Console.WriteLine(reader.ReadToEnd());
            //"Data Source=(Local);Initial Catalog=GSBBlaze;UID=sa;PWD=abcd@1234"
            //StreamWriter oWriter = new StreamWriter(("D:\\FTP\\IN\\MASTER\\TEXT.TXT"), false, Encoding.Default);
            string tmestart = DateTime.Now.AddDays(-30).Year.ToString("0000") + DateTime.Now.ToString("-MM-dd HH:mm:ss.") + DateTime.Now.Millisecond.ToString();

            using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["FTPIP"].ToString()))
                //{
                //    //**************DWH_LOAN_TYPE_05******************
                try
                {

                    //Console.WriteLine(reader.ReadToEnd());
                    //sql = "INSERT INTO " + TempTableName + " " +
                    //          "(CBS_APP_NO, DW_ACC_NO, DW_FDISDATE, DW_CDATE, DW_EXPDATE, DW_SIGN_DATE, " +
                    //          "ISACTIVE, DW_BADMONTH, DW_OUTSTAND, LAST_UPDATE_DTM, DW_BADAMOUNT, " +
                    //          "DW_ORG_TERM, DW_FREQ_TERM, DW_ORG_TERM_MULT, DW_COLLVALUE, CSTATUS_CD, " +
                    //          "DW_ISRESTRUCT, LOAN_TYPE) " +
                    //          "VALUES('" + strArray[0] + "', '" + strArray[1] + "', " +
                    //          "'" + strArray[2] + "', '" + strArray[3] + "', " +
                    //          "'" + strArray[4] + "', '" + strArray[5] + "', " +
                    //          "'" + strArray[6] + "', '" + strArray[7] + "', " +
                    //          "'" + strArray[8] + "', '" + strArray[9] + "', " +
                    //          "'" + strArray[10] + "', '" + strArray[11] + "', " +
                    //          "'" + strArray[12] + "', '" + strArray[13] + "', " +
                    //          "'" + strArray[14] + "', '" + strArray[15] + "', " +
                    //          "'" + strArray[16].Trim() + "', '" + i.ToString("00") + "')";
                    //string revendt = DateTime.Now.AddMonths(-1).Year.ToString("0000") + DateTime.Now.AddMonths(-1).ToString("MM");

                    string[] importfiles = Directory.GetFiles(@"D:\webapp\scoring\FTP\IN\DWH", "LN05DW" + revendt5 + ".txt");
                    string sqlStatement =
                        "INSERT INTO dbo.TMP_DWH_BTFILE(CBS_APP_NO, DW_ACC_NO, DW_FDISDATE, DW_CDATE, DW_EXPDATE," +
                        "DW_SIGN_DATE,ISACTIVE, DW_BADMONTH, DW_OUTSTAND, LAST_UPDATE_DTM, DW_BADAMOUNT,DW_ORG_TERM," +
                        "DW_FREQ_TERM, DW_ORG_TERM_MULT, DW_COLLVALUE, CSTATUS_CD,DW_ISRESTRUCT, LOAN_TYPE, FILE_LOAD_DATE) " +
                        "VALUES(@D1, @D2, @D3 , @D4,@D5, @D6, @D7 , @D8,@D9, @D10, @D11 , @D12,@D13, @D14, @D15 , @D16,@D17,@D18,@D19)";
                    using (SqlCommand cmd = new SqlCommand(sqlStatement, con))
                    {
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
                        //cmd.Parameters.Add("@D4", SqlDbType.Bit);
                        string SQLuplnApp = "";
                        foreach (string importfile in importfiles)
                        {
                            con.Open();
                            string[] allLines = File.ReadAllLines(importfile, Encoding.Default);
                            if (allLines.Length > 1)
                            {
                                //string sqlStatementdl = "delete from dbo.ST_PURPOSE";
                                //SqlCommand cmddl = new SqlCommand(sqlStatementdl, con);
                                //cmddl.ExecuteNonQuery();
                            }
                            int ttl = 0;
                            string fldatestmp = "";
                            for (int index = 0; index < allLines.Length; index++)
                            {
                                if (index == 0)
                                {
                                    fldatestmp = allLines[0].ToString();
                                }
                                else
                                {
                                    if (allLines[index].Length > 0)
                                    {
                                        string[] items = allLines[index].Split(new char[] { ',' });
                                        cmd.Parameters["@D1"].Value = items[0];
                                        cmd.Parameters["@D2"].Value = items[1];
                                        cmd.Parameters["@D3"].Value = items[2];
                                        cmd.Parameters["@D4"].Value = items[3];
                                        cmd.Parameters["@D5"].Value = items[4];
                                        cmd.Parameters["@D6"].Value = items[5];
                                        cmd.Parameters["@D7"].Value = items[6];
                                        cmd.Parameters["@D8"].Value = items[7];
                                        cmd.Parameters["@D9"].Value = items[8];
                                        cmd.Parameters["@D10"].Value = items[9];
                                        cmd.Parameters["@D11"].Value = items[10];
                                        cmd.Parameters["@D12"].Value = items[11];
                                        cmd.Parameters["@D13"].Value = items[12];
                                        cmd.Parameters["@D14"].Value = items[13];
                                        cmd.Parameters["@D15"].Value = items[14];
                                        cmd.Parameters["@D16"].Value = items[15];
                                        cmd.Parameters["@D17"].Value = items[16];
                                        cmd.Parameters["@D18"].Value = "05";
                                        cmd.Parameters["@D19"].Value = fldatestmp.TrimEnd();
                                        //cmd.Parameters["@D4"].Value = 1;

                                        cmd.ExecuteNonQuery();
                                        SQLuplnApp = "Update dbo.LN_APP set DWH_BADMNT ='" + items[7] + "' where APP_NO = RIGHT('000000000000' + '" + items[0] + "',12)";
                                        SqlCommand cmddlu = new SqlCommand(SQLuplnApp, con);
                                        cmddlu.ExecuteNonQuery();
                                    }
                                    ttl++;
                                }
                            }
                            string sqlStatementdx = "insert into s_log_import values('DWH_LOAN05',convert(datetime,'" + tmestart.ToString() + "',121),getdate()," + ttl.ToString() + ",'DONE',NULL,'003')";
                            SqlCommand cmddlx = new SqlCommand(sqlStatementdx, con);
                            cmddlx.ExecuteNonQuery();
                            con.Close();
                        }
                    }
                }
                catch (Exception ex)
                {
                    string inExc = ex.Message;
                    SqlConnection errcon = new SqlConnection(ConfigurationManager.AppSettings["FTPIP"].ToString());
                    errcon.Open();
                    string sqlStatementderr = "insert into s_log_import values('DWH_LOAN05',convert(datetime,'" + tmestart.ToString() + "',121),getdate(),0,'FAIL','" + inExc.ToString().Replace("'", "''") + "','003')";
                    SqlCommand cmddlerr = new SqlCommand(sqlStatementderr, errcon);
                    cmddlerr.ExecuteNonQuery();
                    errcon.Close();
                }


        }
        static void UpdateTable6(string revendt6)
        {
            //Console.WriteLine(reader.ReadToEnd());
            //"Data Source=(Local);Initial Catalog=GSBBlaze;UID=sa;PWD=abcd@1234"
            //StreamWriter oWriter = new StreamWriter(("D:\\FTP\\IN\\MASTER\\TEXT.TXT"), false, Encoding.Default);
            string tmestart = DateTime.Now.Year.ToString("0000") + DateTime.Now.ToString("-MM-dd HH:mm:ss.") + DateTime.Now.Millisecond.ToString();

            using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["FTPIP"].ToString()))
                //{
                //    //**************DWH_LOAN_TYPE_06******************
                try
                {

                    //Console.WriteLine(reader.ReadToEnd());
                    //sql = "INSERT INTO " + TempTableName + " " +
                    //          "(CBS_APP_NO, DW_ACC_NO, DW_FDISDATE, DW_CDATE, DW_EXPDATE, DW_SIGN_DATE, " +
                    //          "ISACTIVE, DW_BADMONTH, DW_OUTSTAND, LAST_UPDATE_DTM, DW_BADAMOUNT, " +
                    //          "DW_ORG_TERM, DW_FREQ_TERM, DW_ORG_TERM_MULT, DW_COLLVALUE, CSTATUS_CD, " +
                    //          "DW_ISRESTRUCT, LOAN_TYPE) " +
                    //          "VALUES('" + strArray[0] + "', '" + strArray[1] + "', " +
                    //          "'" + strArray[2] + "', '" + strArray[3] + "', " +
                    //          "'" + strArray[4] + "', '" + strArray[5] + "', " +
                    //          "'" + strArray[6] + "', '" + strArray[7] + "', " +
                    //          "'" + strArray[8] + "', '" + strArray[9] + "', " +
                    //          "'" + strArray[10] + "', '" + strArray[11] + "', " +
                    //          "'" + strArray[12] + "', '" + strArray[13] + "', " +
                    //          "'" + strArray[14] + "', '" + strArray[15] + "', " +
                    //          "'" + strArray[16].Trim() + "', '" + i.ToString("00") + "')";
                    //string revendt = DateTime.Now.AddMonths(-1).Year.ToString("0000") + DateTime.Now.AddMonths(-1).ToString("MM");

                    string[] importfiles = Directory.GetFiles(@"D:\webapp\scoring\FTP\IN\DWH", "LN06DW" + revendt6 + ".txt");
                    string sqlStatement =
                        "INSERT INTO dbo.TMP_DWH_BTFILE(CBS_APP_NO, DW_ACC_NO, DW_FDISDATE, DW_CDATE, DW_EXPDATE," +
                        "DW_SIGN_DATE,ISACTIVE, DW_BADMONTH, DW_OUTSTAND, LAST_UPDATE_DTM, DW_BADAMOUNT,DW_ORG_TERM," +
                        "DW_FREQ_TERM, DW_ORG_TERM_MULT, DW_COLLVALUE, CSTATUS_CD,DW_ISRESTRUCT, LOAN_TYPE, FILE_LOAD_DATE) " +
                        "VALUES(@D1, @D2, @D3 , @D4,@D5, @D6, @D7 , @D8,@D9, @D10, @D11 , @D12,@D13, @D14, @D15 , @D16,@D17,@D18,@D19)";
                    using (SqlCommand cmd = new SqlCommand(sqlStatement, con))
                    {
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
                        //cmd.Parameters.Add("@D4", SqlDbType.Bit);
                        string SQLuplnApp = "";
                        foreach (string importfile in importfiles)
                        {
                            con.Open();
                            string[] allLines = File.ReadAllLines(importfile, Encoding.Default);
                            if (allLines.Length > 1)
                            {
                                //string sqlStatementdl = "delete from dbo.ST_PURPOSE";
                                //SqlCommand cmddl = new SqlCommand(sqlStatementdl, con);
                                //cmddl.ExecuteNonQuery();
                            }
                            int ttl = 0;
                            string fldatestmp = "";
                            for (int index = 0; index < allLines.Length; index++)
                            {
                                if (index == 0)
                                {
                                    fldatestmp = allLines[0].ToString();
                                }
                                else
                                {
                                    if (allLines[index].Length > 0)
                                    {
                                        string[] items = allLines[index].Split(new char[] { ',' });
                                        cmd.Parameters["@D1"].Value = items[0];
                                        cmd.Parameters["@D2"].Value = items[1];
                                        cmd.Parameters["@D3"].Value = items[2];
                                        cmd.Parameters["@D4"].Value = items[3];
                                        cmd.Parameters["@D5"].Value = items[4];
                                        cmd.Parameters["@D6"].Value = items[5];
                                        cmd.Parameters["@D7"].Value = items[6];
                                        cmd.Parameters["@D8"].Value = items[7];
                                        cmd.Parameters["@D9"].Value = items[8];
                                        cmd.Parameters["@D10"].Value = items[9];
                                        cmd.Parameters["@D11"].Value = items[10];
                                        cmd.Parameters["@D12"].Value = items[11];
                                        cmd.Parameters["@D13"].Value = items[12];
                                        cmd.Parameters["@D14"].Value = items[13];
                                        cmd.Parameters["@D15"].Value = items[14];
                                        cmd.Parameters["@D16"].Value = items[15];
                                        cmd.Parameters["@D17"].Value = items[16];
                                        cmd.Parameters["@D18"].Value = "06";
                                        cmd.Parameters["@D19"].Value = fldatestmp.TrimEnd();
                                        //cmd.Parameters["@D4"].Value = 1;

                                        cmd.ExecuteNonQuery();
                                        SQLuplnApp = "Update dbo.LN_APP set DWH_BADMNT ='" + items[7] + "' where APP_NO = RIGHT('000000000000' + '" + items[0] + "',12)";
                                        SqlCommand cmddlu = new SqlCommand(SQLuplnApp, con);
                                        cmddlu.ExecuteNonQuery();
                                    }
                                    ttl++;
                                }
                            }
                            string sqlStatementdx = "insert into s_log_import values('DWH_LOAN06',convert(datetime,'" + tmestart.ToString() + "',121),getdate()," + ttl.ToString() + ",'DONE',NULL,'003')";
                            SqlCommand cmddlx = new SqlCommand(sqlStatementdx, con);
                            cmddlx.ExecuteNonQuery();
                            con.Close();
                        }
                    }
                }
                catch (Exception ex)
                {
                    string inExc = ex.Message;
                    SqlConnection errcon = new SqlConnection(ConfigurationManager.AppSettings["FTPIP"].ToString());
                    errcon.Open();
                    string sqlStatementderr = "insert into s_log_import values('DWH_LOAN06',convert(datetime,'" + tmestart.ToString() + "',121),getdate(),0,'FAIL','" + inExc.ToString().Replace("'", "''") + "','003')";
                    SqlCommand cmddlerr = new SqlCommand(sqlStatementderr, errcon);
                    cmddlerr.ExecuteNonQuery();
                    errcon.Close();
                }


        }
        static void UpdateTable7(string revendt7)
        {
            //Console.WriteLine(reader.ReadToEnd());
            //"Data Source=(Local);Initial Catalog=GSBBlaze;UID=sa;PWD=abcd@1234"
            //StreamWriter oWriter = new StreamWriter(("D:\\FTP\\IN\\MASTER\\TEXT.TXT"), false, Encoding.Default);
            string tmestart = DateTime.Now.Year.ToString("0000") + DateTime.Now.ToString("-MM-dd HH:mm:ss.") + DateTime.Now.Millisecond.ToString();

            using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["FTPIP"].ToString()))
                //{
                //    //**************DWH_LOAN_TYPE_07******************
                try
                {

                    //Console.WriteLine(reader.ReadToEnd());
                    //sql = "INSERT INTO " + TempTableName + " " +
                    //          "(CBS_APP_NO, DW_ACC_NO, DW_FDISDATE, DW_CDATE, DW_EXPDATE, DW_SIGN_DATE, " +
                    //          "ISACTIVE, DW_BADMONTH, DW_OUTSTAND, LAST_UPDATE_DTM, DW_BADAMOUNT, " +
                    //          "DW_ORG_TERM, DW_FREQ_TERM, DW_ORG_TERM_MULT, DW_COLLVALUE, CSTATUS_CD, " +
                    //          "DW_ISRESTRUCT, LOAN_TYPE) " +
                    //          "VALUES('" + strArray[0] + "', '" + strArray[1] + "', " +
                    //          "'" + strArray[2] + "', '" + strArray[3] + "', " +
                    //          "'" + strArray[4] + "', '" + strArray[5] + "', " +
                    //          "'" + strArray[6] + "', '" + strArray[7] + "', " +
                    //          "'" + strArray[8] + "', '" + strArray[9] + "', " +
                    //          "'" + strArray[10] + "', '" + strArray[11] + "', " +
                    //          "'" + strArray[12] + "', '" + strArray[13] + "', " +
                    //          "'" + strArray[14] + "', '" + strArray[15] + "', " +
                    //          "'" + strArray[16].Trim() + "', '" + i.ToString("00") + "')";
                    //string revendt = DateTime.Now.AddMonths(-1).Year.ToString("0000") + DateTime.Now.AddMonths(-1).ToString("MM");

                    string[] importfiles = Directory.GetFiles(@"D:\webapp\scoring\FTP\IN\DWH", "LN07DW" + revendt7 + ".txt");
                    string sqlStatement =
                        "INSERT INTO dbo.TMP_DWH_BTFILE(CBS_APP_NO, DW_ACC_NO, DW_FDISDATE, DW_CDATE, DW_EXPDATE," +
                        "DW_SIGN_DATE,ISACTIVE, DW_BADMONTH, DW_OUTSTAND, LAST_UPDATE_DTM, DW_BADAMOUNT,DW_ORG_TERM," +
                        "DW_FREQ_TERM, DW_ORG_TERM_MULT, DW_COLLVALUE, CSTATUS_CD,DW_ISRESTRUCT, LOAN_TYPE, FILE_LOAD_DATE) " +
                        "VALUES(@D1, @D2, @D3 , @D4,@D5, @D6, @D7 , @D8,@D9, @D10, @D11 , @D12,@D13, @D14, @D15 , @D16,@D17,@D18,@D19)";
                    using (SqlCommand cmd = new SqlCommand(sqlStatement, con))
                    {
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
                        //cmd.Parameters.Add("@D4", SqlDbType.Bit);
                        string SQLuplnApp = "";
                        foreach (string importfile in importfiles)
                        {
                            con.Open();
                            string[] allLines = File.ReadAllLines(importfile, Encoding.Default);
                            if (allLines.Length > 1)
                            {
                                //string sqlStatementdl = "delete from dbo.ST_PURPOSE";
                                //SqlCommand cmddl = new SqlCommand(sqlStatementdl, con);
                                //cmddl.ExecuteNonQuery();
                            }
                            int ttl = 0;
                            string fldatestmp = "";
                            for (int index = 0; index < allLines.Length; index++)
                            {
                                if (index == 0)
                                {
                                    fldatestmp = allLines[0].ToString();
                                }
                                else
                                {
                                    if (allLines[index].Length > 0)
                                    {
                                        string[] items = allLines[index].Split(new char[] { ',' });
                                        cmd.Parameters["@D1"].Value = items[0];
                                        cmd.Parameters["@D2"].Value = items[1];
                                        cmd.Parameters["@D3"].Value = items[2];
                                        cmd.Parameters["@D4"].Value = items[3];
                                        cmd.Parameters["@D5"].Value = items[4];
                                        cmd.Parameters["@D6"].Value = items[5];
                                        cmd.Parameters["@D7"].Value = items[6];
                                        cmd.Parameters["@D8"].Value = items[7];
                                        cmd.Parameters["@D9"].Value = items[8];
                                        cmd.Parameters["@D10"].Value = items[9];
                                        cmd.Parameters["@D11"].Value = items[10];
                                        cmd.Parameters["@D12"].Value = items[11];
                                        cmd.Parameters["@D13"].Value = items[12];
                                        cmd.Parameters["@D14"].Value = items[13];
                                        cmd.Parameters["@D15"].Value = items[14];
                                        cmd.Parameters["@D16"].Value = items[15];
                                        cmd.Parameters["@D17"].Value = items[16];
                                        cmd.Parameters["@D18"].Value = "07";
                                        cmd.Parameters["@D19"].Value = fldatestmp.TrimEnd();
                                        //cmd.Parameters["@D4"].Value = 1;

                                        cmd.ExecuteNonQuery();
                                        SQLuplnApp = "Update dbo.LN_APP set DWH_BADMNT ='" + items[7] + "' where APP_NO = RIGHT('000000000000' + '" + items[0] + "',12)";
                                        SqlCommand cmddlu = new SqlCommand(SQLuplnApp, con);
                                        cmddlu.ExecuteNonQuery();
                                    }
                                    ttl++;
                                }
                            }
                            string sqlStatementdx = "insert into s_log_import values('DWH_LOAN07',convert(datetime,'" + tmestart.ToString() + "',121),getdate()," + ttl.ToString() + ",'DONE',NULL,'003')";
                            SqlCommand cmddlx = new SqlCommand(sqlStatementdx, con);
                            cmddlx.ExecuteNonQuery();
                            con.Close();
                        }
                    }
                }
                catch (Exception ex)
                {
                    string inExc = ex.Message;
                    SqlConnection errcon = new SqlConnection(ConfigurationManager.AppSettings["FTPIP"].ToString());
                    errcon.Open();
                    string sqlStatementderr = "insert into s_log_import values('DWH_LOAN07',convert(datetime,'" + tmestart.ToString() + "',121),getdate(),0,'FAIL','" + inExc.ToString().Replace("'", "''") + "','003')";
                    SqlCommand cmddlerr = new SqlCommand(sqlStatementderr, errcon);
                    cmddlerr.ExecuteNonQuery();
                    errcon.Close();
                }


        }
        //include Cleansing Step (Execute StoreProcedured)
        static void ExecuteStored()
        {

            using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["FTPIP"].ToString()))
            {
                con.Open();
                string sqlStatementdx = "exec SP_Scoring_Cleansing_Monthly_DWHBTFILE";
                SqlCommand cmddlx = new SqlCommand(sqlStatementdx, con);
                cmddlx.CommandTimeout = 0;
                cmddlx.ExecuteNonQuery();
                con.Close();

            }


        }
          
    }
}