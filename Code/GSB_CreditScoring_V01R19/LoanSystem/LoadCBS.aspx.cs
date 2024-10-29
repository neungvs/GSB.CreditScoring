using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
using System.Text;
using WinSCP;
using System.Globalization;

namespace GSB.LoanSystem
{
    public partial class LoadCBS : System.Web.UI.Page
    {

        static string fldl, flln, flcol, flcif;
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


                flln = "up";
                flcol = "up";
                flcif = "up";

                //client.Connect("10.4.31.80", 22, new NetworkCredential("locrsist", "locrsist"), ESSLSupportMode.ClearText, null, null, 0, 0, 0, 30, false);
                //client.GetFile("/LNCBS.csv", "D:\\BOL\\FTP\\IN\\CBS\\LNCBS.csv");
                //client.GetFile("/COLCBS.csv", "D:\\BOL\\FTP\\IN\\CBS\\COLCBS.csv");
                //client.GetFile("/CIFCBS.csv", "D:\\BOL\\FTP\\IN\\CBS\\CIFCBS.csv");


                GetSFTPfile("/FromCBS/LNCBS.csv", "D:\\webapp\\scoring\\FTP\\IN\\LOPS\\LNCBS.csv");
                flln = fldl;
                //GetSFTPfile("COLCBS.csv", "D:\\webapp\\scoring\\FTP\\IN\\LOPS\\COLCBS.csv");
                //flcol = fldl;
                //GetSFTPfile("/FromCBS/CIFCBS.csv", "D:\\webapp\\scoring\\FTP\\IN\\LOPS\\CIFCBS.csv");
                //flcif = fldl;

                string revendt = DateTime.Now.Year.ToString("0000") + DateTime.Now.ToString("MMdd");
                if (flln == "up")
                {
                    GetSFTPfile("/FromCBS/LNCBS.csv", "D:\\webapp\\scoring\\FTP\\IN\\LOPS\\BCK\\LNCBS" + revendt + ".csv");
                }
                if (flcol == "up")
                {
                    GetSFTPfile("FromCBS\\COLCBS.csv", "D:\\webapp\\scoring\\FTP\\IN\\LOPS\\BCK\\COLCBS" + revendt + ".csv");
                }
                if (flcif == "up")
                {
                    GetSFTPfile("FromCBS\\CIFCBS.csv", "D:\\webapp\\scoring\\FTP\\IN\\LOPS\\BCK\\CIFCBS" + revendt + ".csv");
                }
                 
                if (flln == "up")
                {
                    UpdateTable1();
                    //UpdateTable4();
                }

                if (flcol == "up")
                {
                    UpdateTable2();
                }
                if (flcif == "up")
                {
                    UpdateTable3();
                } 
                 
                //UpdateTable1();
                //UpdateTable2();
                //UpdateTable3();
                ExecuteStored();

                Label2.Text = "ดำเนินการเสร็จสิ้น";
            }
            else
            {
                // this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('You clicked NO!')", true);
                Response.Redirect("../LoanSystem/LoadCBS.aspx");
            }
        }

        static void GetSFTPfile(String Remotefile, String Localfile)
        {
            string tmestart = DateTime.Now.Year.ToString("0000") + DateTime.Now.ToString("-MM-dd HH:mm:ss.") + DateTime.Now.Millisecond.ToString();
            try
            {
                // Setup session options
                //(ConfigurationManager.AppSettings["FTPIP"].ToString()
                string protocaluse = (ConfigurationManager.AppSettings["Protclcbs"].ToString()); 
                 SessionOptions sessionOptions = new SessionOptions();
                if (protocaluse == "SFTP")
                {
                    sessionOptions.Protocol = Protocol.Sftp;
                    sessionOptions.HostName = ConfigurationManager.AppSettings["cbsftphost"].ToString();
                    sessionOptions.UserName = ConfigurationManager.AppSettings["cbsftpuser"].ToString();
                    sessionOptions.Password = ConfigurationManager.AppSettings["cbsftppassword"].ToString();
                    //HostName = "10.4.31.80",
                    //UserName = "locrsuat",
                    //Password = "locrsuat",
                    ////                SshHostKey = "ssh-rsa 2048 f4:99:dc:54:13:8d:a1:1d:99:0b:d8:92:77:50:4b:1b"
                    sessionOptions.SshHostKey = ConfigurationManager.AppSettings["cbsftphostkey"].ToString();
                    sessionOptions.SshPrivateKey = ConfigurationManager.AppSettings["cbsftpprikey"].ToString();
                }
                else
                {
                    sessionOptions.Protocol = Protocol.Ftp;
                    sessionOptions.HostName = ConfigurationManager.AppSettings["cbsftphost"].ToString();
                    sessionOptions.UserName = ConfigurationManager.AppSettings["cbsftpuser"].ToString();
                    sessionOptions.Password = ConfigurationManager.AppSettings["cbsftppassword"].ToString();
                }


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
                            fldl = "up";
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
                                fldl = "up";
                            }
                            else
                            {
                                Console.WriteLine(
                                    "File {0} as well as local backup {1} exist, " +
                                    "but remote file is not newer ({2}) than local backup ({3})",
                                    remotePath, localPath, remoteWriteTime, localWriteTime);
                                download = false;
                                fldl = "non";
                            }
                        }

                        if (download)
                        {
                            // Download the file and throw on any error
                            session.GetFiles(remotePath, localPath).Check();

                            Console.WriteLine("Download to Server done.");
                            //    return "Done";
                            fldl = "up";
                        }
                    }
                    else
                    {
                        Console.WriteLine("File {0} does not exist yet", remotePath);
                        //  return "False";
                        fldl = "non";
                    }
                }
                //   return "Done";
                //            return 0;
                SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["FTPIP"].ToString());
                string tmesend = DateTime.Now.Year.ToString("0000") + DateTime.Now.ToString("-MM-dd HH:mm:ss.") + DateTime.Now.Millisecond.ToString();
                string sqlStatementdx = "insert into s_log_import values('BATCH_LOPs',convert(datetime,'" + tmestart.ToString() + "',121),convert(datetime,'" + tmesend.ToString() + "',121),0,'DONE','" + Localfile.ToString() + "','001')";
                con.Open();
                SqlCommand cmddlx = new SqlCommand(sqlStatementdx, con);
                cmddlx.ExecuteNonQuery();
                con.Close();

            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e);
                //          return 1;
                //     return "False";
                string inExc = e.Message;
                SqlConnection errcon = new SqlConnection(ConfigurationManager.AppSettings["FTPIP"].ToString());
                string sqlStatementderr = "insert into s_log_import values('BATCH_LOPs',convert(datetime,'" + tmestart.ToString() + "',121),getdate(),0,'FAIL','" + Localfile.ToString() + ":" + inExc.ToString().Replace("'", "''") + "','001')";
                errcon.Open();
                SqlCommand cmddlerr = new SqlCommand(sqlStatementderr, errcon);
                cmddlerr.ExecuteNonQuery();
                errcon.Close();
                fldl = "non";
            }


        }

        static void UpdateTable1() //****** LNCBS 
        {
            //Console.WriteLine(reader.ReadToEnd());
            //"Data Source=(Local);Initial Catalog=GSBBlaze;UID=sa;PWD=abcd@1234"
            //StreamWriter oWriter = new StreamWriter(("D:\\FTP\\IN\\MASTER\\TEXT.TXT"), false, Encoding.Default);
            string tmestart = DateTime.Now.Year.ToString("0000") + DateTime.Now.ToString("-MM-dd HH:mm:ss.") + DateTime.Now.Millisecond.ToString();

            using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["FTPIP"].ToString()))
                //{
                //    //**************LNCBS******************
                try
                {

                    //[CBS_APP_NO] [varchar](22) NOT NULL,
                    //[CBS_ISIC1_CD] [varchar](3) NULL,
                    //[CBS_ISIC2_CD] [varchar](10) NULL,
                    //[CBS_ISIC3_CD] [varchar](3) NULL,
                    //[CBS_CA_NO] [varchar](200) NULL,
                    //[CBS_COMMITTEE] [varchar](5) NULL,
                    //[CBS_APPROVE_CD] [varchar](2) NULL,
                    //[CBS_REASON_CD] [varchar](5) NULL,
                    //[CBS_APPROVE_COMMENT] [varchar](100) NULL,
                    //[CBS_PAYSTATUS] [varchar](5) NULL,
                    //[CBS_MATURITYDATE] [datetime] NULL,
                    //[CBS_COL] [varchar](5) NULL,
                    //[CBS_MPAYAMT] [decimal](18, 2) NULL,

                    string[] importfiles = Directory.GetFiles(@"D:\webapp\scoring\FTP\IN\LOPs", "LNCBS.csv");
                    string sqlStatement =
                        "INSERT INTO dbo.CBS_LN_APP(CBS_APP_NO,CBS_ISIC1_CD,CBS_ISIC2_CD,CBS_ISIC3_CD,CBS_CA_NO,CBS_COMMITTEE,CBS_APPROVE_CD,CBS_REASON_CD,CBS_APPROVE_COMMENT,CBS_PAYSTATUS,CBS_MATURITYDATE,CBS_COL,CBS_MPAYAMT,BATCH_UPDATE_DTM,CBS_CA_DATE) " +
                        "VALUES(@D1, @D2, @D3 , @D4,@D5, @D6, @D7 , @D8,@D9, @D10, @D11 , @D12,@D13,@D14,@D15)";
                    using (SqlCommand cmd = new SqlCommand(sqlStatement, con))
                    {
                        cmd.Parameters.Add("@D1", SqlDbType.VarChar, 22);
                        cmd.Parameters.Add("@D2", SqlDbType.VarChar, 3);
                        cmd.Parameters.Add("@D3", SqlDbType.VarChar, 10);
                        cmd.Parameters.Add("@D4", SqlDbType.VarChar, 3);
                        cmd.Parameters.Add("@D5", SqlDbType.VarChar, 200);
                        cmd.Parameters.Add("@D6", SqlDbType.VarChar, 5);
                        cmd.Parameters.Add("@D7", SqlDbType.VarChar, 2);
                        cmd.Parameters.Add("@D8", SqlDbType.VarChar, 5);
                        cmd.Parameters.Add("@D9", SqlDbType.VarChar, 100);
                        cmd.Parameters.Add("@D10", SqlDbType.VarChar, 5);
                        cmd.Parameters.Add("@D11", SqlDbType.VarChar, 50);
                        cmd.Parameters.Add("@D12", SqlDbType.VarChar, 5);
                        cmd.Parameters.Add("@D13", SqlDbType.VarChar, 5);
                        cmd.Parameters.Add("@D14", SqlDbType.VarChar, 50);
                        cmd.Parameters.Add("@D15", SqlDbType.VarChar, 50);
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
                            for (int index = 0; index < (allLines.Length - 1); index++)
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
                                        cmd.Parameters["@D5"].Value = "";
                                        cmd.Parameters["@D6"].Value = "";
                                        cmd.Parameters["@D7"].Value = items[4];
                                        cmd.Parameters["@D8"].Value = items[5];
                                        cmd.Parameters["@D9"].Value = "";
                                        cmd.Parameters["@D10"].Value = items[6];
                                        cmd.Parameters["@D11"].Value = items[7];
                                        cmd.Parameters["@D12"].Value = items[8];
                                        if (string.IsNullOrEmpty(items[9]))
                                        { cmd.Parameters["@D13"].Value = 0; }
                                        else
                                        { cmd.Parameters["@D13"].Value = items[9]; }

                                        //                                        cmd.Parameters["@D13"].Value = if(string.IsNullOrEmpty(items[9]) 0;10);
                                        cmd.Parameters["@D14"].Value = fldatestmp.TrimEnd();
                                        cmd.Parameters["@D15"].Value = items[10];
                                        //cmd.Parameters["@D4"].Value = 1;

                                        cmd.ExecuteNonQuery();
                                        SQLuplnApp = "Update dbo.LN_APP set CBS_STATUS ='" + items[4] + "' where APP_NO = '" + items[0] + "'";
                                        SqlCommand cmddlu = new SqlCommand(SQLuplnApp, con);
                                        cmddlu.ExecuteNonQuery();
                                    }
                                    ttl++;
                                }
                            }
                            string tmesend = DateTime.Now.Year.ToString("0000") + DateTime.Now.ToString("-MM-dd HH:mm:ss.") + DateTime.Now.Millisecond.ToString();
                            string sqlStatementdx = "insert into s_log_import values('LNCBS',convert(datetime,'" + tmestart.ToString() + "',121),convert(datetime,'" + tmesend.ToString() + "',121)," + ttl.ToString() + ",'DONE',NULL,'001')";
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
                    string sqlStatementderr = "insert into s_log_import values('LNCBS',convert(datetime,'" + tmestart.ToString() + "',121),getdate(),0,'FAIL','" + inExc.ToString().Replace("'", "''") + "','001')";
                    SqlCommand cmddlerr = new SqlCommand(sqlStatementderr, errcon);
                    cmddlerr.ExecuteNonQuery();
                    errcon.Close();
                }


        }
       
        static void UpdateTable2() //****** COLCBS 
        {
            //Console.WriteLine(reader.ReadToEnd());
            //"Data Source=(Local);Initial Catalog=GSBBlaze;UID=sa;PWD=abcd@1234"
            //StreamWriter oWriter = new StreamWriter(("D:\\FTP\\IN\\MASTER\\TEXT.TXT"), false, Encoding.Default);
            string tmestart = DateTime.Now.Year.ToString("0000") + DateTime.Now.ToString("-MM-dd HH:mm:ss.") + DateTime.Now.Millisecond.ToString();

            using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["FTPIP"].ToString()))
                //{
                //    //**************COLCBS******************
                try
                {

                    //[CBS_APP_NO] [varchar](22) NOT NULL,
                    //[CBS_COLLTYPE] [varchar](4) NOT NULL,
                    //[CBS_COLLSTYPE] [varchar](4) NOT NULL,
                    //[ID] [varchar](20) NOT NULL,
                    //[VALUE] [varchar](10) NOT NULL,


                    string[] importfiles = Directory.GetFiles(@"D:\webapp\scoring\FTP\IN\LOPs", "COLCBS.csv");
                    string sqlStatement =
                        "INSERT INTO dbo.CBS_LN_APPCOL(CBS_APP_NO,CBS_COLLTYPE,CBS_COLLSTYPE,ID,VALUE,BATCH_UPDATE_DTM) " +
                        "VALUES(@D1, @D2, @D3 , @D4,@D5, @D6)";
                    using (SqlCommand cmd = new SqlCommand(sqlStatement, con))
                    {
                        cmd.Parameters.Add("@D1", SqlDbType.VarChar, 22);
                        cmd.Parameters.Add("@D2", SqlDbType.VarChar, 4);
                        cmd.Parameters.Add("@D3", SqlDbType.VarChar, 4);
                        cmd.Parameters.Add("@D4", SqlDbType.VarChar, 20);
                        cmd.Parameters.Add("@D5", SqlDbType.VarChar, 10);
                        cmd.Parameters.Add("@D6", SqlDbType.VarChar, 50);
                        //cmd.Parameters.Add("@D4", SqlDbType.Bit);

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
                            for (int index = 0; index < (allLines.Length - 1); index++)
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
                                        cmd.Parameters["@D6"].Value = fldatestmp.TrimEnd();
                                        //cmd.Parameters["@D4"].Value = 1;

                                        cmd.ExecuteNonQuery();
                                    }
                                    ttl++;
                                }
                            }
                            string tmesend = DateTime.Now.Year.ToString("0000") + DateTime.Now.ToString("-MM-dd HH:mm:ss.") + DateTime.Now.Millisecond.ToString();
                            string sqlStatementdx = "insert into s_log_import values('COLCBS',convert(datetime,'" + tmestart.ToString() + "',121),convert(datetime,'" + tmesend.ToString() + "',121)," + ttl.ToString() + ",'DONE',NULL,'001')";
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
                    string sqlStatementderr = "insert into s_log_import values('COLCBS',convert(datetime,'" + tmestart.ToString() + "',121),getdate(),0,'FAIL','" + inExc.ToString().Replace("'", "''") + "','001')";
                    SqlCommand cmddlerr = new SqlCommand(sqlStatementderr, errcon);
                    cmddlerr.ExecuteNonQuery();
                    errcon.Close();
                }


        }
        
        //incluse Cleansing Step for LOPs        
        static void ExecuteStored()
        {

            using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["FTPIP"].ToString()))
            {
                con.Open();
                string sqlStatementdx = "exec SP_Scoring_Cleansing_Monthly_Batch";
                SqlCommand cmddlx = new SqlCommand(sqlStatementdx, con);
                cmddlx.CommandTimeout = 0;
                cmddlx.ExecuteNonQuery();
                
                con.Close();
            }
               
        }
       
        static void UpdateTable3() //****** CIFCBS 
        {
            //Console.WriteLine(reader.ReadToEnd());
            //"Data Source=(Local);Initial Catalog=GSBBlaze;UID=sa;PWD=abcd@1234"
            //StreamWriter oWriter = new StreamWriter(("D:\\FTP\\IN\\MASTER\\TEXT.TXT"), false, Encoding.Default);

            string tmestart = DateTime.Now.Year.ToString("0000") + DateTime.Now.ToString("-MM-dd HH:mm:ss.") + DateTime.Now.Millisecond.ToString();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["FTPIP"].ToString()))
                //{
                //    //**************CIFCBS******************
                try
                {

                    //[CBS_APP_NO] [varchar](22) NOT NULL,
                    //[CBS_CIFNO] [varchar](12) NOT NULL,
                    //[CBS_CITIZID] [varchar](13) NULL,
                    //[CBS_CCODE_CD] [varchar](5) NULL,
                    //[CBS_CLIENTM] [int] NULL,
                    //[CBS_MNTEXPEN] [decimal](18, 2) NULL,
                    //[CBS_MNTEXPEN1] [decimal](18, 2) NULL,
                    //[CBS_MNTEXPEN2] [decimal](18, 2) NULL,
                    //[CBS_MNTEXPEN3] [decimal](18, 2) NULL,
                    //[CBS_LBTEXPEN] [decimal](18, 2) NULL,
                    //[CBS_LBTEXPEN1] [decimal](18, 2) NULL,
                    //[CBS_LBTEXPEN2] [decimal](18, 2) NULL,
                    //[CBS_LBTEXPEN3] [decimal](18, 2) NULL,
                    //[CBS_BUSEXPEN] [decimal](18, 2) NULL,
                    //[CBS_BUSEXPEN1] [decimal](18, 2) NULL,
                    //[CBS_BUSEXPEN2] [decimal](18, 2) NULL,
                    //[CBS_BUSEXPEN3] [decimal](18, 2) NULL,
                    //[CBS_BUSEXPEN4] [decimal](18, 2) NULL,
                    //[CBS_BUSEXPEN5] [decimal](18, 2) NULL,
                    //[CBS_BUSEXPEN6] [decimal](18, 2) NULL,
                    //[CBS_RELATION] [varchar](2) NULL,
                    //[CBS_LADDL1] [varchar](100) NULL,
                    //[CBS_LADDL2] [varchar](100) NULL,
                    //[CBS_LADDL3] [varchar](100) NULL,
                    //[CBS_LADDL4] [varchar](100) NULL,
                    //[CBS_LDISTRICT_CD] [varchar](6) NULL,
                    //[CBS_LAMPHUR] [varchar](4) NULL,
                    //[CBS_LPROVINCE] [varchar](4) NULL,
                    //[CBS_LPOST] [varchar](5) NULL,
                    //[CBS_WADDL1] [varchar](100) NULL,
                    //[CBS_WADDL2] [varchar](100) NULL,
                    //[CBS_WADDL3] [varchar](100) NULL,
                    //[CBS_WADDL4] [varchar](100) NULL,
                    //[CBS_WDISTRICT_CD] [varchar](6) NULL,
                    //[CBS_WAMPHUR] [varchar](4) NULL,
                    //[CBS_WPROVINCE] [varchar](4) NULL,
                    //[CBS_WPOST] [varchar](5) NULL,
                    //[CBS_CADDL1] [varchar](100) NULL,
                    //[CBS_CADDL2] [varchar](100) NULL,
                    //[CBS_CADDL3] [varchar](100) NULL,
                    //[CBS_CADDL4] [varchar](100) NULL,
                    //[CBS_CLDISTRICT_CD] [varchar](6) NULL,
                    //[CBS_CAMPHUR] [varchar](4) NULL,
                    //[CBS_CPROVINCE] [varchar](4) NULL,
                    //[CBS_CPOST] [varchar](5) NULL,
                    //[CBS_HOME_PHONE] [varchar](50) NULL,
                    //[CBS_WORK_PHONE] [varchar](50) NULL,
                    //[CBS_MOBILE] [varchar](50) NULL,
                    //[CBS_EMAIL] [varchar](50) NULL,
                    //[CBS_ISIC1] [varchar](3) NULL,
                    //[CBS_ISIC2] [varchar](1) NULL,
                    //[CBS_ISIC3] [varchar](3) NULL,
                    //[CBS_SAVING_ACCT_REF] [varchar](15) NULL,
                    //[CBS_COLREQ_CD] [varchar](6) NULL,
                    //[CBS_LINSURANCE] [bit] NULL,


                    string[] importfiles = Directory.GetFiles(@"D:\webapp\scoring\FTP\IN\LOPs", "CIFCBS.csv");
                    string sqlStatement =
                        "INSERT INTO dbo.CBS_LN_APPCIF(CBS_APP_NO,CBS_CIFNO,CBS_CITIZID,CBS_CCODE_CD,CBS_CLIENTM,CBS_MNTEXPEN," +
                        "CBS_MNTEXPEN1,CBS_MNTEXPEN2,CBS_MNTEXPEN3,CBS_LBTEXPEN,CBS_LBTEXPEN1,CBS_LBTEXPEN2,CBS_LBTEXPEN3,CBS_BUSEXPEN," +
                        "CBS_BUSEXPEN1,CBS_BUSEXPEN2,CBS_BUSEXPEN3,CBS_BUSEXPEN4,CBS_BUSEXPEN5,CBS_BUSEXPEN6,CBS_RELATION,CBS_LADDL1," +
                        "CBS_LADDL2,CBS_LADDL3,CBS_LADDL4,CBS_LDISTRICT_CD,CBS_LAMPHUR,CBS_LPROVINCE,CBS_LPOST,CBS_WADDL1,CBS_WADDL2," +
                        "CBS_WADDL3,CBS_WADDL4,CBS_WDISTRICT_CD,CBS_WAMPHUR,CBS_WPROVINCE,CBS_WPOST,CBS_CADDL1,CBS_CADDL2,CBS_CADDL3," +
                        "CBS_CADDL4,CBS_CLDISTRICT_CD,CBS_CAMPHUR,CBS_CPROVINCE,CBS_CPOST,CBS_HOME_PHONE,CBS_WORK_PHONE,CBS_MOBILE," +
                        "CBS_EMAIL,CBS_ISIC1,CBS_ISIC2,CBS_ISIC3,CBS_SAVING_ACCT_REF,CBS_COLREQ_CD,CBS_LINSURANCE,BATCH_UPDATE_DTM) " +
                        "VALUES(@D1,@D2,@D3,@D4,@D5,@D6,@D7,@D8,@D9,@D10,@D11,@D12,@D13,@D14,@D15,@D16,@D17,@D18,@D19,@D20,@D21,@D22,@D23," +
                        "@D24,@D25,@D26,@D27,@D28,@D29,@D30,@D31,@D32,@D33,@D34,@D35,@D36,@D37,@D38,@D39,@D40,@D41,@D42,@D43,@D44,@D45,@D46," +
                        "@D47,@D48,@D49,@D50,@D51,@D52,@D53,@D54,@D55,@D56)";
                    using (SqlCommand cmd = new SqlCommand(sqlStatement, con))
                    {
                        cmd.Parameters.Add("@D1", SqlDbType.NVarChar, 22);
                        cmd.Parameters.Add("@D2", SqlDbType.NVarChar, 12);
                        cmd.Parameters.Add("@D3", SqlDbType.NVarChar, 13);
                        cmd.Parameters.Add("@D4", SqlDbType.NVarChar, 5);
                        cmd.Parameters.Add("@D5", SqlDbType.NVarChar, 5);
                        cmd.Parameters.Add("@D6", SqlDbType.Decimal);
                        cmd.Parameters.Add("@D7", SqlDbType.Decimal);
                        cmd.Parameters.Add("@D8", SqlDbType.Decimal);
                        cmd.Parameters.Add("@D9", SqlDbType.Decimal);
                        cmd.Parameters.Add("@D10", SqlDbType.Decimal);
                        cmd.Parameters.Add("@D11", SqlDbType.Decimal);
                        cmd.Parameters.Add("@D12", SqlDbType.Decimal);
                        cmd.Parameters.Add("@D13", SqlDbType.Decimal);
                        cmd.Parameters.Add("@D14", SqlDbType.Decimal);
                        cmd.Parameters.Add("@D15", SqlDbType.Decimal);
                        cmd.Parameters.Add("@D16", SqlDbType.Decimal);
                        cmd.Parameters.Add("@D17", SqlDbType.Decimal);
                        cmd.Parameters.Add("@D18", SqlDbType.Decimal);
                        cmd.Parameters.Add("@D19", SqlDbType.Decimal);
                        cmd.Parameters.Add("@D20", SqlDbType.Decimal);
                        cmd.Parameters.Add("@D21", SqlDbType.NVarChar, 2);
                        cmd.Parameters.Add("@D22", SqlDbType.NVarChar, 100);
                        cmd.Parameters.Add("@D23", SqlDbType.NVarChar, 100);
                        cmd.Parameters.Add("@D24", SqlDbType.NVarChar, 100);
                        cmd.Parameters.Add("@D25", SqlDbType.NVarChar, 100);
                        cmd.Parameters.Add("@D26", SqlDbType.NVarChar, 6);
                        cmd.Parameters.Add("@D27", SqlDbType.NVarChar, 4);
                        cmd.Parameters.Add("@D28", SqlDbType.NVarChar, 4);
                        cmd.Parameters.Add("@D29", SqlDbType.NVarChar, 5);
                        cmd.Parameters.Add("@D30", SqlDbType.NVarChar, 100);
                        cmd.Parameters.Add("@D31", SqlDbType.NVarChar, 100);
                        cmd.Parameters.Add("@D32", SqlDbType.NVarChar, 100);
                        cmd.Parameters.Add("@D33", SqlDbType.NVarChar, 100);
                        cmd.Parameters.Add("@D34", SqlDbType.NVarChar, 6);
                        cmd.Parameters.Add("@D35", SqlDbType.NVarChar, 4);
                        cmd.Parameters.Add("@D36", SqlDbType.NVarChar, 4);
                        cmd.Parameters.Add("@D37", SqlDbType.NVarChar, 5);
                        cmd.Parameters.Add("@D38", SqlDbType.NVarChar, 100);
                        cmd.Parameters.Add("@D39", SqlDbType.NVarChar, 100);
                        cmd.Parameters.Add("@D40", SqlDbType.NVarChar, 100);
                        cmd.Parameters.Add("@D41", SqlDbType.NVarChar, 100);
                        cmd.Parameters.Add("@D42", SqlDbType.NVarChar, 6);
                        cmd.Parameters.Add("@D43", SqlDbType.NVarChar, 4);
                        cmd.Parameters.Add("@D44", SqlDbType.NVarChar, 4);
                        cmd.Parameters.Add("@D45", SqlDbType.NVarChar, 5);
                        cmd.Parameters.Add("@D46", SqlDbType.NVarChar, 50);
                        cmd.Parameters.Add("@D47", SqlDbType.NVarChar, 50);
                        cmd.Parameters.Add("@D48", SqlDbType.NVarChar, 50);
                        cmd.Parameters.Add("@D49", SqlDbType.NVarChar, 50);
                        cmd.Parameters.Add("@D50", SqlDbType.NVarChar, 3);
                        cmd.Parameters.Add("@D51", SqlDbType.NVarChar, 1);
                        cmd.Parameters.Add("@D52", SqlDbType.NVarChar, 3);
                        cmd.Parameters.Add("@D53", SqlDbType.NVarChar, 15);
                        cmd.Parameters.Add("@D54", SqlDbType.NVarChar, 6);
                        cmd.Parameters.Add("@D55", SqlDbType.NVarChar, 4);
                        cmd.Parameters.Add("@D56", SqlDbType.NVarChar, 50);

                        //cmd.Parameters.Add("@D4", SqlDbType.Bit);

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
                            for (int index = 0; index < (allLines.Length - 1); index++)
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
                                        if (string.IsNullOrEmpty(items[5]))
                                        { cmd.Parameters["@D6"].Value = 0; }
                                        else
                                        { cmd.Parameters["@D6"].Value = items[5]; }
                                        if (string.IsNullOrEmpty(items[6]))
                                        { cmd.Parameters["@D7"].Value = 0; }
                                        else
                                        { cmd.Parameters["@D7"].Value = items[6]; }
                                        if (string.IsNullOrEmpty(items[7]))
                                        { cmd.Parameters["@D8"].Value = 0; }
                                        else
                                        { cmd.Parameters["@D8"].Value = items[7]; }
                                        if (string.IsNullOrEmpty(items[8]))
                                        { cmd.Parameters["@D9"].Value = 0; }
                                        else
                                        { cmd.Parameters["@D9"].Value = items[8]; }
                                        if (string.IsNullOrEmpty(items[9]))
                                        { cmd.Parameters["@D10"].Value = 0; }
                                        else
                                        { cmd.Parameters["@D10"].Value = items[9]; }
                                        if (string.IsNullOrEmpty(items[10]))
                                        { cmd.Parameters["@D11"].Value = 0; }
                                        else
                                        { cmd.Parameters["@D11"].Value = items[10]; }
                                        if (string.IsNullOrEmpty(items[11]))
                                        { cmd.Parameters["@D12"].Value = 0; }
                                        else
                                        { cmd.Parameters["@D12"].Value = items[11]; }
                                        if (string.IsNullOrEmpty(items[12]))
                                        { cmd.Parameters["@D13"].Value = 0; }
                                        else
                                        { cmd.Parameters["@D13"].Value = items[12]; }
                                        if (string.IsNullOrEmpty(items[13]))
                                        { cmd.Parameters["@D14"].Value = 0; }
                                        else
                                        { cmd.Parameters["@D14"].Value = items[13]; }
                                        if (string.IsNullOrEmpty(items[14]))
                                        { cmd.Parameters["@D15"].Value = 0; }
                                        else
                                        { cmd.Parameters["@D15"].Value = items[14]; }
                                        if (string.IsNullOrEmpty(items[15]))
                                        { cmd.Parameters["@D16"].Value = 0; }
                                        else
                                        { cmd.Parameters["@D16"].Value = items[15]; }
                                        if (string.IsNullOrEmpty(items[16]))
                                        { cmd.Parameters["@D17"].Value = 0; }
                                        else
                                        { cmd.Parameters["@D17"].Value = items[16]; }
                                        if (string.IsNullOrEmpty(items[17]))
                                        { cmd.Parameters["@D18"].Value = 0; }
                                        else
                                        { cmd.Parameters["@D18"].Value = items[17]; }
                                        if (string.IsNullOrEmpty(items[18]))
                                        { cmd.Parameters["@D19"].Value = 0; }
                                        else
                                        { cmd.Parameters["@D19"].Value = items[18]; }
                                        if (string.IsNullOrEmpty(items[19]))
                                        { cmd.Parameters["@D20"].Value = 0; }
                                        else
                                        { cmd.Parameters["@D20"].Value = items[19]; }
                                        //if (string.IsNullOrEmpty(items[5]))
                                        //{ cmd.Parameters["@D6"].Value = 0; }
                                        //else
                                        //{ cmd.Parameters["@D6"].Value = items[5]; } 
                                        cmd.Parameters["@D21"].Value = items[20];
                                        cmd.Parameters["@D22"].Value = items[21];
                                        cmd.Parameters["@D23"].Value = items[22];
                                        cmd.Parameters["@D24"].Value = items[23];
                                        cmd.Parameters["@D25"].Value = items[24];
                                        cmd.Parameters["@D26"].Value = items[25];
                                        cmd.Parameters["@D27"].Value = items[26];
                                        cmd.Parameters["@D28"].Value = items[27];
                                        cmd.Parameters["@D29"].Value = items[28];
                                        cmd.Parameters["@D30"].Value = items[29];
                                        cmd.Parameters["@D31"].Value = items[30];
                                        cmd.Parameters["@D32"].Value = items[31];
                                        cmd.Parameters["@D33"].Value = items[32];
                                        cmd.Parameters["@D34"].Value = items[33];
                                        cmd.Parameters["@D35"].Value = items[34];
                                        cmd.Parameters["@D36"].Value = items[35];
                                        cmd.Parameters["@D37"].Value = items[36];
                                        cmd.Parameters["@D38"].Value = items[37];
                                        cmd.Parameters["@D39"].Value = items[38];
                                        cmd.Parameters["@D40"].Value = items[39];
                                        cmd.Parameters["@D41"].Value = items[40];
                                        cmd.Parameters["@D42"].Value = items[41];
                                        cmd.Parameters["@D43"].Value = items[42];
                                        cmd.Parameters["@D44"].Value = items[43];
                                        cmd.Parameters["@D45"].Value = items[44];
                                        cmd.Parameters["@D46"].Value = items[45];
                                        cmd.Parameters["@D47"].Value = items[46];
                                        cmd.Parameters["@D48"].Value = items[47];
                                        cmd.Parameters["@D49"].Value = items[48];
                                        cmd.Parameters["@D50"].Value = items[49];
                                        cmd.Parameters["@D51"].Value = items[50];
                                        cmd.Parameters["@D52"].Value = items[51];
                                        cmd.Parameters["@D53"].Value = items[52];
                                        cmd.Parameters["@D54"].Value = items[53];
                                        cmd.Parameters["@D55"].Value = items[54];
                                        cmd.Parameters["@D56"].Value = fldatestmp.TrimEnd();
                                        //cmd.Parameters["@D4"].Value = 1;

                                        cmd.ExecuteNonQuery();
                                    }
                                    ttl++;
                                }
                            }
                            string tmesend = DateTime.Now.Year.ToString("0000") + DateTime.Now.ToString("-MM-dd HH:mm:ss.") + DateTime.Now.Millisecond.ToString();
                            string sqlStatementdx = "insert into s_log_import values('CIFCBS',convert(datetime,'" + tmestart.ToString() + "',121),convert(datetime,'" + tmesend.ToString() + "',121)," + ttl.ToString() + ",'DONE',NULL,'001')";
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
                    string sqlStatementderr = "insert into s_log_import values('CIFCBS',convert(datetime,'" + tmestart.ToString() + "',121),getdate(),0,'FAIL','" + inExc.ToString().Replace("'", "''") + "','001')";
                    SqlCommand cmddlerr = new SqlCommand(sqlStatementderr, errcon);
                    cmddlerr.ExecuteNonQuery();
                    errcon.Close();
                }


        }

        static void UpdateTable4() //****** CBS_LN_COL
        {
            //Console.WriteLine(reader.ReadToEnd());
            //"Data Source=(Local);Initial Catalog=GSBBlaze;UID=sa;PWD=abcd@1234"
            //StreamWriter oWriter = new StreamWriter(("D:\\FTP\\IN\\MASTER\\TEXT.TXT"), false, Encoding.Default);
            string tmestart = DateTime.Now.Year.ToString("0000") + DateTime.Now.ToString("-MM-dd HH:mm:ss.") + DateTime.Now.Millisecond.ToString();

            using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["FTPIP"].ToString()))
                //{
                //    //**************COLCBS******************
                try
                {

                    string[] importfiles = Directory.GetFiles(@"D:\webapp\scoring\FTP\IN\LOPs", "CBS_LN_COL.TXT");
                    string sqlStatement =
                        "INSERT INTO CBS_LN_COL(CBS_APP_NO,CBS_COLLTYPE,CBS_COLLSTYPE,ID,VALUE,BATCH_UPDATE_DTM) " +
                        "VALUES(@D1, @D2, @D3 , @D4,@D5, @D6)";

                    string sqlstr = "";

                    using (SqlCommand cmd = new SqlCommand(sqlStatement, con))
                    {
                        cmd.Parameters.Add("@D1", SqlDbType.VarChar, 22);
                        cmd.Parameters.Add("@D2", SqlDbType.VarChar, 4);
                        cmd.Parameters.Add("@D3", SqlDbType.VarChar, 4);
                        cmd.Parameters.Add("@D4", SqlDbType.VarChar, 20);
                        cmd.Parameters.Add("@D5", SqlDbType.VarChar, 10);
                        cmd.Parameters.Add("@D6", SqlDbType.VarChar, 50);
                        //cmd.Parameters.Add("@D4", SqlDbType.Bit);
                        con.Open();

                        //SqlCommand dbaccess;
                        //dbaccess = new SqlCommand(sqlstr, con);
                        //dbaccess.ExecuteNonQuery();

                        foreach (string importfile in importfiles)
                        {                          
                            string[] allLines = File.ReadAllLines(importfile, Encoding.Default);
                            if (allLines.Length > 1)
                            {
                                //string sqlStatementdl = "delete from dbo.ST_PURPOSE";
                                //SqlCommand cmddl = new SqlCommand(sqlStatementdl, con);
                                //cmddl.ExecuteNonQuery();
                            }
                            int ttl = 0;
                            string fldatestmp = "";
                            for (int index = 0; index < (allLines.Length - 1); index++)
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
                                        cmd.Parameters["@D1"].Value = items[0];
                                        cmd.Parameters["@D2"].Value = items[1];
                                        cmd.Parameters["@D3"].Value = items[2];
                                        cmd.Parameters["@D4"].Value = items[3];
                                        cmd.Parameters["@D5"].Value = items[4];
                                        cmd.Parameters["@D6"].Value = fldatestmp.TrimEnd();
                                        //cmd.Parameters["@D4"].Value = 1;
                                        cmd.ExecuteNonQuery();
                                    }
                                    ttl++;
                                }
                            }

                            string tmesend = DateTime.Now.Year.ToString("0000") + DateTime.Now.ToString("-MM-dd HH:mm:ss.") + DateTime.Now.Millisecond.ToString();
                            
                            sqlstr = "insert into s_log_import values('LNCOLCBS',convert(datetime,'" + tmestart.ToString() + "',121),convert(datetime,'" + tmesend.ToString() + "',121)," + ttl.ToString() + ",'DONE',NULL,'001')";
                            SqlCommand dbaccess = new SqlCommand(sqlstr, con);
                            dbaccess.ExecuteNonQuery();
                            dbaccess.Dispose();
                            con.Close();
                        }
                    }
                }
                catch (Exception ex)
                {
                    string inExc = ex.Message;
                    SqlConnection errcon = new SqlConnection(ConfigurationManager.AppSettings["FTPIP"].ToString());
                    errcon.Open();
                    string sqlStatementderr = "insert into s_log_import values('LNCOLCBS',convert(datetime,'" + tmestart.ToString() + "',121),getdate(),0,'FAIL','" + inExc.ToString().Replace("'", "''") + "','001')";
                    SqlCommand cmddlerr = new SqlCommand(sqlStatementderr, errcon);
                    cmddlerr.ExecuteNonQuery();
                    errcon.Close();
                }


        }
        
    }
}