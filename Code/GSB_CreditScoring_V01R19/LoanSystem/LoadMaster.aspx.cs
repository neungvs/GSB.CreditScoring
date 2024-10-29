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
using System.Net;
using System.Collections;

namespace GSB.LoanSystem
{
    class DateFile
    {
        public DateTime date;
    }
    public partial class LoadMaster : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void Button1_Click(object sender, EventArgs e)
        {

            string confirmValue = Request.Form["confirm_value"];
            if (confirmValue == "Yes")
            {
                //this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('You clicked YES!')", true);
                //Button1_Enabled();

                Button1.Enabled = false;
                Button2.Enabled = false;
                Button3.Enabled = false;

                Session.Timeout = 20000;
                Server.ScriptTimeout = 36000;
                Label2.Visible = true;

                string DateNow = DateTime.Now.Year.ToString("0000") + DateTime.Now.ToString("MMdd");
                string DateFile = GetFileDate();


                if (DateNow.Substring(0, 6) == DateFile.Substring(0, 6))
                {
                    //string revendt = DateTime.Now.Year.ToString("0000") + DateTime.Now.ToString("MM") + "01";

                    string revendt = DateFile;
                    
                    GetSFTPfile("/FromCBS/ZUTBLARRPUR_" + revendt + ".TXT", "D:\\webapp\\scoring\\FTP\\IN\\MASTER\\ZUTBLARRPUR.txt");
                    GetSFTPfile("/FromCBS/UTBLEDUC_" + revendt + ".TXT", "D:\\webapp\\scoring\\FTP\\IN\\MASTER\\UTBLEDUC.txt");
                    GetSFTPfile("/FromCBS/UTBLMS_" + revendt + ".TXT", "D:\\webapp\\scoring\\FTP\\IN\\MASTER\\UTBLMS.txt");
                    GetSFTPfile("/FromCBS/UTBLSEX_" + revendt + ".TXT", "D:\\webapp\\scoring\\FTP\\IN\\MASTER\\UTBLSEX.txt");
                    GetSFTPfile("/FromCBS/ZUTBLAPPSTAT_" + revendt + ".TXT", "D:\\webapp\\scoring\\FTP\\IN\\MASTER\\ZUTBLAPPSTAT.txt");
                    GetSFTPfile("/FromCBS/ZUTBLCOLLREQ_" + revendt + ".TXT", "D:\\webapp\\scoring\\FTP\\IN\\MASTER\\ZUTBLCOLLREQ.txt");
                    GetSFTPfile("/FromCBS/ZUTBLCOLSUBT_" + revendt + ".TXT", "D:\\webapp\\scoring\\FTP\\IN\\MASTER\\ZUTBLCOLSUBT.txt");
                    GetSFTPfile("/FromCBS/ZUTBLCOLTYP_" + revendt + ".TXT", "D:\\webapp\\scoring\\FTP\\IN\\MASTER\\ZUTBLCOLTYP.txt");
                    GetSFTPfile("/FromCBS/ZUTBLGSBPUR_" + revendt + ".TXT", "D:\\webapp\\scoring\\FTP\\IN\\MASTER\\ZUTBLGSBPUR.txt");
                    GetSFTPfile("/FromCBS/ZUTBLISICSD_" + revendt + ".TXT", "D:\\webapp\\scoring\\FTP\\IN\\MASTER\\ZUTBLISICSD.txt");
                    GetSFTPfile("/FromCBS/ZUTBLISICGC_" + revendt + ".TXT", "D:\\webapp\\scoring\\FTP\\IN\\MASTER\\ZUTBLISICGC.txt");
                    GetSFTPfile("/FromCBS/ZUTBLISICTS_" + revendt + ".TXT", "D:\\webapp\\scoring\\FTP\\IN\\MASTER\\ZUTBLISICTS.txt");
                    GetSFTPfile("/FromCBS/ZUTBLKTBCUST_" + revendt + ".TXT", "D:\\webapp\\scoring\\FTP\\IN\\MASTER\\ZUTBLKTBCUST.txt");
                    GetSFTPfile("/FromCBS/ZUTBLLNAPAPRS_" + revendt + ".TXT", "D:\\webapp\\scoring\\FTP\\IN\\MASTER\\ZUTBLLNAPAPRS.txt");
                    GetSFTPfile("/FromCBS/ZUTBLLNAPASHLD_" + revendt + ".TXT", "D:\\webapp\\scoring\\FTP\\IN\\MASTER\\ZUTBLLNAPASHLD.txt");
                    GetSFTPfile("/FromCBS/ZUTBLLNAPCMTL_" + revendt + ".TXT", "D:\\webapp\\scoring\\FTP\\IN\\MASTER\\ZUTBLLNAPCMTL.txt");
                    GetSFTPfile("/FromCBS/ZUTBLLNAPRSTAT_" + revendt + ".TXT", "D:\\webapp\\scoring\\FTP\\IN\\MASTER\\ZUTBLLNAPRSTAT.txt");
                    GetSFTPfile("/FromCBS/ZUTBLLNAPESTYP_" + revendt + ".TXT", "D:\\webapp\\scoring\\FTP\\IN\\MASTER\\ZUTBLLNAPESTYP.txt");
                    GetSFTPfile("/FromCBS/ZUTBLLNAPRTYP_" + revendt + ".TXT", "D:\\webapp\\scoring\\FTP\\IN\\MASTER\\ZUTBLLNAPRTYP.txt");
                    GetSFTPfile("/FromCBS/ZUTBLLNAPESOWN_" + revendt + ".TXT", "D:\\webapp\\scoring\\FTP\\IN\\MASTER\\ZUTBLLNAPESOWN.txt");
                    GetSFTPfile("/FromCBS/ZUTBLOC_" + revendt + ".TXT", "D:\\webapp\\scoring\\FTP\\IN\\MASTER\\ZUTBLOC.txt");
                    GetSFTPfile("/FromCBS/ZUTBLPERCONS_" + revendt + ".TXT", "D:\\webapp\\scoring\\FTP\\IN\\MASTER\\ZUTBLPERCONS.txt");
                    GetSFTPfile("/FromCBS/ZUTBLRPAYMETH_" + revendt + ".TXT", "D:\\webapp\\scoring\\FTP\\IN\\MASTER\\ZUTBLRPAYMETH.txt");
                    GetSFTPfile("/FromCBS/ZUTBLSOC_" + revendt + ".TXT", "D:\\webapp\\scoring\\FTP\\IN\\MASTER\\ZUTBLSOC.txt");
                    GetSFTPfile("/FromCBS/ZUTBLTITLE_" + revendt + ".TXT", "D:\\webapp\\scoring\\FTP\\IN\\MASTER\\ZUTBLTITLE.txt");
                    //ann GetSFTPfile("/FromCBS/UTBLBRCD_" + revendt + ".TXT", "D:\\webapp\\scoring\\FTP\\IN\\MASTER\\UTBLBRCD.txt");
                    //ann GetSFTPfile("/FromCBS/ZUTBLREGREP_" + revendt + ".TXT", "D:\\webapp\\scoring\\FTP\\IN\\MASTER\\ZUTBLREGREP.txt");
                    //ann GetSFTPfile("/FromCBS/ZUTBLMKTCD_" + revendt + ".TXT", "D:\\webapp\\scoring\\FTP\\IN\\MASTER\\ZUTBLMKTCD.txt");
                    GetSFTPfile("/FromCBS/ZUTBLDIST_" + revendt + ".TXT", "D:\\webapp\\scoring\\FTP\\IN\\MASTER\\ZUTBLDIST.txt");
                    //ann GetSFTPfile("/FromCBS/PRODCTL_" + revendt + ".TXT", "D:\\webapp\\scoring\\FTP\\IN\\MASTER\\PRODCTL.txt");
                    GetSFTPfile("/FromCBS/STBLCNTRY1_" + revendt + ".TXT", "D:\\webapp\\scoring\\FTP\\IN\\MASTER\\STBLCNTRY1.txt");
                    //ann GetSFTPfile("/FromCBS/ZUTBLMKTDFT_" + revendt + ".TXT", "D:\\webapp\\scoring\\FTP\\IN\\MASTER\\ZUTBLMKTDFT.txt");
                    GetSFTPfile("/FromCBS/ZUTBLSDISTCD_" + revendt + ".TXT", "D:\\webapp\\scoring\\FTP\\IN\\MASTER\\ZUTBLSDISTCD.txt");
                    GetSFTPfile("/FromCBS/STBLGRP_" + revendt + ".TXT", "D:\\webapp\\scoring\\FTP\\IN\\MASTER\\STBLGRP.txt");//ann
                    //GetSFTPfile("/FromCBS/UTBLSUBT_" + revendt + ".TXT", "D:\\webapp\\scoring\\FTP\\IN\\MASTER\\UTBLSUBT.txt");
                    
                    
                    UpdateTable1();//ST_PurPose
                    UpdateTable2();//ST_EDUCATION
                    UpdateTable3();//ST_MARITAL
                    UpdateTable4();//ST_GENDER
                    UpdateTable5();//ST_APPTATUS
                    UpdateTable6();//ST_Colreq
                    UpdateTable7();//ST_COLLSTYPE
                    UpdateTable8();//ST_COLLTYPE
                    UpdateTable9();//ST_GSBPURPOSE
                    UpdateTable10();//ST_ISIC1
                    UpdateTable11();//ST_ISIC2
                    UpdateTable12();//ST_ISIC3
                    UpdateTable13();//ST_CCODE
                    UpdateTable14();//LN_Reason
                    UpdateTable15();//ST_ASSETHOLDSTA
                    UpdateTable16();//LN_Committee
                    UpdateTable17();//ST_Resstatus
                    UpdateTable18();//ST_BUSSTYPE
                    UpdateTable19();//ST_RESType
                    UpdateTable20();//ST_PROP_Right
                    UpdateTable21();//ST_OCCCLS
                    UpdateTable22();//ST_Consumption
                    UpdateTable23();//ST_PAYMENTMETH
                    UpdateTable24();//ST_OCCSCLS
                    UpdateTable25();//ST_TITLE
                    //UpdateTable26();//ST_Branch,ST_GSBZONE,ST_GSBRegion
                    UpdateTable27();//ST_AMPHUR
                    //UpdateTable28();//ST_LOANMKT
                    //UpdateTable29();//ST_LOANTYPE
                    UpdateTable30();//ST_PROVINCE 
                    //UpdateTable32();//UTBLEDUC 
                    //UpdateTable31();//ST_MKTDFT*สร้างขึ้นมาเอง เพราะยังไม่รู้ เทเบิล*
                    //UpdateTable37();//TMP_LOANSTYPE
                    //UpdateTable38();//ST_DISTRICT
                    UpdateTable38();//ST_LOANGRP
                    //UpdateTable39();//[ST_RELATION]
                    UpdateTable40();//ST_DISTRICT //ann
                    
                    Label2.Text = "ดำเนินการเสร็จสิ้น";
                }
                else
                {
                    using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["FTPIP"].ToString()))
                    {
                        SqlConnection errcon = new SqlConnection(ConfigurationManager.AppSettings["FTPIP"].ToString());

                        string sqlStatementderr1 = "insert into s_log_import values('ST_PURPOSE',getdate(),getdate(),0,'FAIL','File Not Found','002')";
                        string sqlStatementderr2 = "insert into s_log_import values('ST_EDUCATION',getdate(),getdate(),0,'FAIL','File Not Found','002')";
                        string sqlStatementderr3 = "insert into s_log_import values('ST_MARITAL',getdate(),getdate(),0,'FAIL','File Not Found','002')";
                        string sqlStatementderr4 = "insert into s_log_import values('ST_GENDER',getdate(),getdate(),0,'FAIL','File Not Found','002')";
                        string sqlStatementderr5 = "insert into s_log_import values('ST_APPTATUS',getdate(),getdate(),0,'FAIL','File Not Found','002')";
                        string sqlStatementderr6 = "insert into s_log_import values('ST_Colreq',getdate(),getdate(),0,'FAIL','File Not Found','002')";
                        string sqlStatementderr7 = "insert into s_log_import values('ST_COLLSTYPE',getdate(),getdate(),0,'FAIL','File Not Found','002')";
                        string sqlStatementderr8 = "insert into s_log_import values('ST_COLLTYPE',getdate(),getdate(),0,'FAIL','File Not Found','002')";
                        string sqlStatementderr9 = "insert into s_log_import values('ST_GSBPURPOSE',getdate(),getdate(),0,'FAIL','File Not Found','002')";
                        string sqlStatementderr10 = "insert into s_log_import values('ST_ISIC1',getdate(),getdate(),0,'FAIL','File Not Found','002')";
                        string sqlStatementderr11 = "insert into s_log_import values('ST_ISIC2',getdate(),getdate(),0,'FAIL','File Not Found','002')";
                        string sqlStatementderr12 = "insert into s_log_import values('ST_ISIC3',getdate(),getdate(),0,'FAIL','File Not Found','002')";
                        string sqlStatementderr13 = "insert into s_log_import values('ST_CCODE',getdate(),getdate(),0,'FAIL','File Not Found','002')";
                        string sqlStatementderr14 = "insert into s_log_import values('LN_Reason',getdate(),getdate(),0,'FAIL','File Not Found','002')";
                        string sqlStatementderr15 = "insert into s_log_import values('ST_ASSETHOLDSTA',getdate(),getdate(),0,'FAIL','File Not Found','002')";
                        string sqlStatementderr16 = "insert into s_log_import values('LN_Committee',getdate(),getdate(),0,'FAIL','File Not Found','002')";
                        string sqlStatementderr17 = "insert into s_log_import values('ST_Resstatus',getdate(),getdate(),0,'FAIL','File Not Found','002')";
                        string sqlStatementderr18 = "insert into s_log_import values('ST_BUSSTYPE',getdate(),getdate(),0,'FAIL','File Not Found','002')";
                        string sqlStatementderr19 = "insert into s_log_import values('ST_RESType',getdate(),getdate(),0,'FAIL','File Not Found','002')";
                        string sqlStatementderr20 = "insert into s_log_import values('ST_PROP_Right',getdate(),getdate(),0,'FAIL','File Not Found','002')";
                        string sqlStatementderr21 = "insert into s_log_import values('ST_OCCCLS',getdate(),getdate(),0,'FAIL','File Not Found','002')";
                        string sqlStatementderr22 = "insert into s_log_import values('ST_Consumption',getdate(),getdate(),0,'FAIL','File Not Found','002')";
                        string sqlStatementderr23 = "insert into s_log_import values('ST_PAYMENTMETH',getdate(),getdate(),0,'FAIL','File Not Found','002')";
                        string sqlStatementderr24 = "insert into s_log_import values('ST_OCCSCLS',getdate(),getdate(),0,'FAIL','File Not Found','002')";
                        string sqlStatementderr25 = "insert into s_log_import values('ST_TITLE',getdate(),getdate(),0,'FAIL','File Not Found','002')";
                        string sqlStatementderr27 = "insert into s_log_import values('ST_AMPHUR',getdate(),getdate(),0,'FAIL','File Not Found','002')";
                        string sqlStatementderr30 = "insert into s_log_import values('ST_PROVINCE',getdate(),getdate(),0,'FAIL','File Not Found','002')";
                        //string sqlStatementderr32 = "insert into s_log_import values('ST_EDUCATION',getdate(),getdate(),0,'FAIL','File Not Found','002')";
                        string sqlStatementderr40 = "insert into s_log_import values('ST_DISTRICT',getdate(),getdate(),0,'FAIL','File Not Found','002')";


                        errcon.Open();
                        SqlCommand cmddlerr1 = new SqlCommand(sqlStatementderr1, errcon);
                        cmddlerr1.ExecuteNonQuery();
                        SqlCommand cmddlerr2 = new SqlCommand(sqlStatementderr2, errcon);
                        cmddlerr2.ExecuteNonQuery();
                        SqlCommand cmddlerr3 = new SqlCommand(sqlStatementderr3, errcon);
                        cmddlerr3.ExecuteNonQuery();
                        SqlCommand cmddlerr4 = new SqlCommand(sqlStatementderr4, errcon);
                        cmddlerr4.ExecuteNonQuery();
                        SqlCommand cmddlerr5 = new SqlCommand(sqlStatementderr5, errcon);
                        cmddlerr5.ExecuteNonQuery();
                        SqlCommand cmddlerr6 = new SqlCommand(sqlStatementderr6, errcon);
                        cmddlerr6.ExecuteNonQuery();
                        SqlCommand cmddlerr7 = new SqlCommand(sqlStatementderr7, errcon);
                        cmddlerr7.ExecuteNonQuery();
                        SqlCommand cmddlerr8 = new SqlCommand(sqlStatementderr8, errcon);
                        cmddlerr8.ExecuteNonQuery();
                        SqlCommand cmddlerr9 = new SqlCommand(sqlStatementderr9, errcon);
                        cmddlerr9.ExecuteNonQuery();
                        SqlCommand cmddlerr10 = new SqlCommand(sqlStatementderr10, errcon);
                        cmddlerr10.ExecuteNonQuery();
                        SqlCommand cmddlerr11 = new SqlCommand(sqlStatementderr11, errcon);
                        cmddlerr11.ExecuteNonQuery();
                        SqlCommand cmddlerr12 = new SqlCommand(sqlStatementderr12, errcon);
                        cmddlerr12.ExecuteNonQuery();
                        SqlCommand cmddlerr13 = new SqlCommand(sqlStatementderr13, errcon);
                        cmddlerr13.ExecuteNonQuery();
                        SqlCommand cmddlerr14 = new SqlCommand(sqlStatementderr14, errcon);
                        cmddlerr14.ExecuteNonQuery();
                        SqlCommand cmddlerr15 = new SqlCommand(sqlStatementderr15, errcon);
                        cmddlerr15.ExecuteNonQuery();
                        SqlCommand cmddlerr16 = new SqlCommand(sqlStatementderr16, errcon);
                        cmddlerr16.ExecuteNonQuery();
                        SqlCommand cmddlerr17 = new SqlCommand(sqlStatementderr17, errcon);
                        cmddlerr17.ExecuteNonQuery();
                        SqlCommand cmddlerr18 = new SqlCommand(sqlStatementderr18, errcon);
                        cmddlerr18.ExecuteNonQuery();
                        SqlCommand cmddlerr19 = new SqlCommand(sqlStatementderr19, errcon);
                        cmddlerr19.ExecuteNonQuery();
                        SqlCommand cmddlerr20 = new SqlCommand(sqlStatementderr20, errcon);
                        cmddlerr20.ExecuteNonQuery();
                        SqlCommand cmddlerr21 = new SqlCommand(sqlStatementderr21, errcon);
                        cmddlerr21.ExecuteNonQuery();
                        SqlCommand cmddlerr22 = new SqlCommand(sqlStatementderr22, errcon);
                        cmddlerr22.ExecuteNonQuery();
                        SqlCommand cmddlerr23 = new SqlCommand(sqlStatementderr23, errcon);
                        cmddlerr23.ExecuteNonQuery();
                        SqlCommand cmddlerr24 = new SqlCommand(sqlStatementderr24, errcon);
                        cmddlerr24.ExecuteNonQuery();
                        SqlCommand cmddlerr25 = new SqlCommand(sqlStatementderr25, errcon);
                        cmddlerr25.ExecuteNonQuery();
                        SqlCommand cmddlerr27 = new SqlCommand(sqlStatementderr27, errcon);
                        cmddlerr27.ExecuteNonQuery();
                        SqlCommand cmddlerr30 = new SqlCommand(sqlStatementderr30, errcon);
                        cmddlerr30.ExecuteNonQuery();
                        //SqlCommand cmddlerr32 = new SqlCommand(sqlStatementderr32, errcon);
                        //cmddlerr32.ExecuteNonQuery();
                        SqlCommand cmddlerr40 = new SqlCommand(sqlStatementderr40, errcon);
                        cmddlerr40.ExecuteNonQuery();
                        errcon.Close();

                        Label2.Text = "ดำเนินการเสร็จสิ้น";

                    }
                }
            }
            else
            {
                // this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('You clicked NO!')", true);
                Response.Redirect("../LoanSystem/LoadMaster.aspx");
            }
        }
        public static string ReverseString(string s)
        {
            char[] arr = s.ToCharArray();
            Array.Reverse(arr);
            return new string(arr);
        }
        public string GetFileDate()
        {

            string str = "";
            string protocaluse = (ConfigurationManager.AppSettings["Protcl"].ToString());
            SessionOptions sessionOptions = new SessionOptions();
            if (protocaluse == "SFTP")
            {
                sessionOptions.Protocol = Protocol.Sftp;
                sessionOptions.HostName = ConfigurationManager.AppSettings["ftphost"].ToString();
                sessionOptions.UserName = ConfigurationManager.AppSettings["ftpuser"].ToString();
                sessionOptions.Password = ConfigurationManager.AppSettings["ftppassword"].ToString();
                //HostName = "10.4.31.80",
                //UserName = "locrsuat",
                //Password = "locrsuat",
                ////                SshHostKey = "ssh-rsa 2048 f4:99:dc:54:13:8d:a1:1d:99:0b:d8:92:77:50:4b:1b"
                sessionOptions.SshHostKey = ConfigurationManager.AppSettings["ftphostkey"].ToString();
                sessionOptions.SshPrivateKey = ConfigurationManager.AppSettings["ftpprikey"].ToString();
            }
            else
            {

                FtpWebRequest reqFTP;
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://" + ConfigurationManager.AppSettings["dwhftphost"].ToString() + "/FromCBS/"));
                reqFTP.UseBinary = true;
                reqFTP.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["dwhftpuser"].ToString(), ConfigurationManager.AppSettings["dwhftppassword"].ToString());
                reqFTP.Method = WebRequestMethods.Ftp.ListDirectory;

                WebResponse response = reqFTP.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());
                string line = reader.ReadLine();
                ArrayList list = new ArrayList();
                var list2 = new List<DateTime>();
                var list3 = new List<DateTime>();
                ArrayList myDate = new ArrayList();

                while (line != null)
                {
                    line = reader.ReadLine();
                    list.Add(line);
                    foreach (string da in list)
                    {
                        try
                        {
                            if (da != null)
                            {
                                if (da.StartsWith("ZUTBLARRPUR_") && ((da.EndsWith(".TXT") || (da.EndsWith(".txt")))))
                                {
                                    string file = da;
                                    string[] pathArr = file.Split('\\');
                                    string[] fileArr = pathArr.Last().Split('.');
                                    string fileLastName = fileArr.Last().ToString();

                                    string Filename = file.Replace(fileLastName, "");

                                    string dd = ReverseString(ReverseString(Filename).Substring(0, 9));
                                    string d1 = dd.Substring(0, 4) + "-" + dd.Substring(4, 2) + "-" + dd.Substring(6, 2);
                                    DateTime date1 = Convert.ToDateTime(string.Format(d1, "YYYY/MM/dd"));
                                    list2.Add(date1);
                                }
                            }
                        }
                        catch { }
                    }
                }
                foreach (DateTime d in list2)
                {
                    myDate.Add(new DateFile { date = d });
                }
                IEnumerable<DateFile> myFileEnum = myDate.OfType<DateFile>();
                // Create a query expression. 
                var filedate = myFileEnum.OrderByDescending(t => t.date)
                .Select(a => a);
                foreach (var date in filedate)
                {
                    list3.Add(date.date);
                }
                str = string.Format("{0:yyyy/MM/dd}", list3[0]).ToString().Replace("/", "");

            }
            return str;
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
                    sessionOptions.HostName = ConfigurationManager.AppSettings["ftphost"].ToString();
                    sessionOptions.UserName = ConfigurationManager.AppSettings["ftpuser"].ToString();
                    sessionOptions.Password = ConfigurationManager.AppSettings["ftppassword"].ToString();
                    //HostName = "10.4.31.80",
                    //UserName = "locrsuat",
                    //Password = "locrsuat",
                    ////                SshHostKey = "ssh-rsa 2048 f4:99:dc:54:13:8d:a1:1d:99:0b:d8:92:77:50:4b:1b"
                    sessionOptions.SshHostKey = ConfigurationManager.AppSettings["ftphostkey"].ToString();
                    sessionOptions.SshPrivateKey = ConfigurationManager.AppSettings["ftpprikey"].ToString();
                }
                else
                {
                    //sessionOptions.Protocol = Protocol.Ftp;
                    //sessionOptions.HostName = ConfigurationManager.AppSettings["ftphost"].ToString();
                    //sessionOptions.UserName = ConfigurationManager.AppSettings["ftpuser"].ToString();
                    //sessionOptions.Password = ConfigurationManager.AppSettings["ftppassword"].ToString();

                    FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://" + ConfigurationManager.AppSettings["ftphost"].ToString() + Remotefile);
                    //FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://10.20.4.250/FromCBS/ZUTBLARRPUR_20130201.txt");
                    request.Method = WebRequestMethods.Ftp.DownloadFile;
                    request.UseBinary = true;
                    // request.Credentials = new NetworkCredential("crscore", "8w!5n)Ag%");
                    request.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["ftpuser"].ToString(), ConfigurationManager.AppSettings["ftppassword"].ToString());
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
                    string sqlStatementdx = "insert into s_log_import values('BATCH_MASTERs',convert(datetime,'" + tmestart.ToString() + "',121),convert(datetime,'" + tmesend.ToString() + "',121),0,'DONE','DONE:" + Localfile.ToString() + "','002')";
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
                        //                    session.Timeout = " ;
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
                                SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["FTPIP"].ToString());
                                string tmesend = DateTime.Now.Year.ToString("0000") + DateTime.Now.ToString("-MM-dd HH:mm:ss.") + DateTime.Now.Millisecond.ToString();
                                string sqlStatementdx = "insert into s_log_import values('BATCH_MASTERs',convert(datetime,'" + tmestart.ToString() + "',121),convert(datetime,'" + tmesend.ToString() + "',121),0,'DONE','DONE:" + Localfile.ToString() + "','002')";
                                con.Open();
                                SqlCommand cmddlx = new SqlCommand(sqlStatementdx, con);
                                cmddlx.ExecuteNonQuery();
                                con.Close();
                            }
                        }
                        else
                        {
                            Console.WriteLine("File {0} does not exist yet", remotePath);
                            SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["FTPIP"].ToString());
                            string tmesend = DateTime.Now.Year.ToString("0000") + DateTime.Now.ToString("-MM-dd HH:mm:ss.") + DateTime.Now.Millisecond.ToString();
                            string sqlStatementdx = "insert into s_log_import values('BATCH_MASTERs',convert(datetime,'" + tmestart.ToString() + "',121),convert(datetime,'" + tmesend.ToString() + "',121),0,'DONE','NO File:" + Localfile.ToString() + "','002')";
                            con.Open();
                            SqlCommand cmddlx = new SqlCommand(sqlStatementdx, con);
                            cmddlx.ExecuteNonQuery();
                            con.Close();                       //  return "False";
                        }
                    }
                }
                //   return "Done";
                //            return 0;

            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e);
                //          return 1;
                //     return "False";
                string inExc = e.Message;
                SqlConnection errcon = new SqlConnection(ConfigurationManager.AppSettings["FTPIP"].ToString());
                errcon.Open();
                string sqlStatementderr = "insert into s_log_import values('BATCH_MASTERs',convert(datetime,'" + tmestart.ToString() + "',121),getdate(),0,'FAIL','" + inExc.ToString().Replace("'", "''") + "','002')";
                SqlCommand cmddlerr = new SqlCommand(sqlStatementderr, errcon);
                cmddlerr.ExecuteNonQuery();
                errcon.Close();
            }
        }

        static void UpdateTable1() //**************ST_PURPOSE******************
        {
            //Console.WriteLine(reader.ReadToEnd());
            //"Data Source=(Local);Initial Catalog=GSBBlaze;UID=sa;PWD=abcd@1234"
            //StreamWriter oWriter = new StreamWriter(("D:\\webapp\\scoring\\FTP\\IN\\MASTER\\TEXT.TXT"), false, Encoding.Default);

            using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["FTPIP"].ToString()))
                //{
                //    //**************ST_PURPOSE******************
                try
                {

                    string[] importfiles = Directory.GetFiles(@"D:\webapp\scoring\FTP\IN\MASTER", "ZUTBLARRPUR.txt");
                    string sqlStatement =
                        "INSERT INTO dbo.ST_PURPOSE(PURPOSE_CD, GRP, PURPOSE_NAME,ACTIVE_FLAG) VALUES(@D1, @D2, @D3 , @D4)";
                    using (SqlCommand cmd = new SqlCommand(sqlStatement, con))
                    {
                        cmd.Parameters.Add("@D1", SqlDbType.VarChar, 6);
                        cmd.Parameters.Add("@D2", SqlDbType.VarChar, 3);
                        cmd.Parameters.Add("@D3", SqlDbType.VarChar, 200);
                        cmd.Parameters.Add("@D4", SqlDbType.Bit);

                        foreach (string importfile in importfiles)
                        {
                            con.Open();
                            string[] allLines = File.ReadAllLines(importfile, Encoding.Default);
                            if (allLines.Length > 1)
                            {
                                string sqlStatementdl = "delete from dbo.ST_PURPOSE";
                                SqlCommand cmddl = new SqlCommand(sqlStatementdl, con);
                                cmddl.ExecuteNonQuery();
                            }
                            int ttl = 0;
                            for (int index = 0; index < allLines.Length; index++)
                            {
                                if (allLines[index].Length > 0)
                                {
                                    string[] items = allLines[index].Split(new char[] { ',' });
                                    cmd.Parameters["@D1"].Value = items[1];
                                    cmd.Parameters["@D2"].Value = items[0];
                                    cmd.Parameters["@D3"].Value = items[2];
                                    cmd.Parameters["@D4"].Value = 1;

                                    cmd.ExecuteNonQuery();
                                }
                                ttl++;
                            }
                            string sqlStatementdx = "insert into s_log_import values('ST_PURPOSE',getdate(),getdate()," + ttl.ToString() + ",'DONE',NULL,'002')";
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
                    string sqlStatementderr = "insert into s_log_import values('ST_PURPOSE',getdate(),getdate(),0,'FAIL','" + inExc.ToString().Replace("'", "''") + "','002')";
                    SqlCommand cmddlerr = new SqlCommand(sqlStatementderr, con);
                    cmddlerr.ExecuteNonQuery();
                    errcon.Close();
                }
        }
        static void UpdateTable2() //**************ST_EDUCATION*****************
        {
            //Console.WriteLine(reader.ReadToEnd());

            //StreamWriter oWriter = new StreamWriter(("D:\\webapp\\scoring\\FTP\\IN\\MASTER\\TEXT.TXT"), false, Encoding.Default);
            using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["FTPIP"].ToString()))
                //{
                //    //**************ST_EDUCATION******************
                try
                {

                    string[] importfiles = Directory.GetFiles(@"D:\webapp\scoring\FTP\IN\MASTER", "UTBLEDUC.txt");
                    string sqlStatement =
                        "INSERT INTO dbo.ST_EDUCATION(EDU_CD,EDU_NAME,EDU_DETAIL,ACTIVE_FLAG) VALUES(@D1, @D2, @D3 , @D4)";
                    using (SqlCommand cmd = new SqlCommand(sqlStatement, con))
                    {
                        cmd.Parameters.Add("@D1", SqlDbType.VarChar, 4);
                        cmd.Parameters.Add("@D2", SqlDbType.VarChar, 100);
                        cmd.Parameters.Add("@D3", SqlDbType.VarChar, 200);
                        cmd.Parameters.Add("@D4", SqlDbType.Bit);

                        foreach (string importfile in importfiles)
                        {
                            con.Open();
                            string[] allLines = File.ReadAllLines(importfile, Encoding.Default);
                            if (allLines.Length > 1)
                            {
                                string sqlStatementdl = "delete from dbo.ST_EDUCATION";
                                SqlCommand cmddl = new SqlCommand(sqlStatementdl, con);
                                cmddl.ExecuteNonQuery();
                            }
                            int ttl = 0;
                            for (int index = 0; index < allLines.Length; index++)
                            {
                                if (allLines[index].Length > 0)
                                {
                                    string[] items = allLines[index].Split(new char[] { ',' });
                                    cmd.Parameters["@D1"].Value = items[0];
                                    cmd.Parameters["@D2"].Value = items[1];
                                    cmd.Parameters["@D3"].Value = items[1];
                                    cmd.Parameters["@D4"].Value = 1;

                                    cmd.ExecuteNonQuery();
                                }
                                ttl++;
                            }
                            string sqlStatementdx = "insert into s_log_import values('ST_EDUCATION',getdate(),getdate()," + ttl.ToString() + ",'DONE',NULL,'002')";
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
                    string sqlStatementderr = "insert into s_log_import values('ST_EDUCATION',getdate(),getdate(),0,'FAIL','" + inExc.ToString().Replace("'", "''") + "','002')";
                    SqlCommand cmddlerr = new SqlCommand(sqlStatementderr, con);
                    cmddlerr.ExecuteNonQuery();
                    errcon.Close();
                }


        }
        static void UpdateTable3() //**************ST_MARITAL*******
        {
            //Console.WriteLine(reader.ReadToEnd());

            //StreamWriter oWriter = new StreamWriter(("D:\\webapp\\scoring\\FTP\\IN\\MASTER\\TEXT.TXT"), false, Encoding.Default);
            using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["FTPIP"].ToString()))
                //{
                //    //**************ST_MARITAL******************
                try
                {

                    string[] importfiles = Directory.GetFiles(@"D:\webapp\scoring\FTP\IN\MASTER", "UTBLMS.txt");
                    string sqlStatement =
                        "INSERT INTO dbo.ST_MARITAL(MARITAL_CD,MARITAL_NAME, MARITAL_DETAIL,ACTIVE_FLAG) VALUES(@D1, @D2, @D3 , @D4)";
                    using (SqlCommand cmd = new SqlCommand(sqlStatement, con))
                    {
                        cmd.Parameters.Add("@D1", SqlDbType.VarChar, 2);
                        cmd.Parameters.Add("@D2", SqlDbType.VarChar, 100);
                        cmd.Parameters.Add("@D3", SqlDbType.VarChar, 200);
                        cmd.Parameters.Add("@D4", SqlDbType.Bit);

                        foreach (string importfile in importfiles)
                        {
                            con.Open();
                            string[] allLines = File.ReadAllLines(importfile, Encoding.Default);
                            if (allLines.Length > 1)
                            {
                                string sqlStatementdl = "delete from dbo.ST_MARITAL";
                                SqlCommand cmddl = new SqlCommand(sqlStatementdl, con);
                                cmddl.ExecuteNonQuery();
                            }
                            int ttl = 0;
                            for (int index = 0; index < allLines.Length; index++)
                            {
                                if (allLines[index].Length > 0)
                                {
                                    string[] items = allLines[index].Split(new char[] { ',' });
                                    cmd.Parameters["@D1"].Value = items[0];
                                    cmd.Parameters["@D2"].Value = items[1];
                                    cmd.Parameters["@D3"].Value = items[1];
                                    cmd.Parameters["@D4"].Value = 1;

                                    cmd.ExecuteNonQuery();
                                }
                                ttl++;
                            }
                            string sqlStatementdx = "insert into s_log_import values('ST_MARITAL',getdate(),getdate()," + ttl.ToString() + ",'DONE',NULL,'002')";
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
                    string sqlStatementderr = "insert into s_log_import values('ST_MARITAL',getdate(),getdate(),0,'FAIL','" + inExc.ToString().Replace("'", "''") + "','002')";
                    SqlCommand cmddlerr = new SqlCommand(sqlStatementderr, con);
                    cmddlerr.ExecuteNonQuery();
                    errcon.Close();
                }


        }
        static void UpdateTable4() //**************ST_GENDER*******
        {
            //Console.WriteLine(reader.ReadToEnd());

            //StreamWriter oWriter = new StreamWriter(("D:\\webapp\\scoring\\FTP\\IN\\MASTER\\TEXT.TXT"), false, Encoding.Default);
            using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["FTPIP"].ToString()))
                //{
                //    //**************ST_GENDER******************
                try
                {

                    string[] importfiles = Directory.GetFiles(@"D:\webapp\scoring\FTP\IN\MASTER", "UTBLSEX.txt");
                    string sqlStatement =
                        "INSERT INTO dbo.ST_GENDER(GENDER_CD,GENDER_NAME,GENDER_DETAIL,ACTIVE_FLAG) VALUES(@D1, @D2, @D3 , @D4)";
                    using (SqlCommand cmd = new SqlCommand(sqlStatement, con))
                    {
                        cmd.Parameters.Add("@D1", SqlDbType.VarChar, 4);
                        cmd.Parameters.Add("@D2", SqlDbType.VarChar, 100);
                        cmd.Parameters.Add("@D3", SqlDbType.VarChar, 200);
                        cmd.Parameters.Add("@D4", SqlDbType.Bit);

                        foreach (string importfile in importfiles)
                        {
                            con.Open();
                            string[] allLines = File.ReadAllLines(importfile, Encoding.Default);
                            if (allLines.Length > 1)
                            {
                                string sqlStatementdl = "delete from dbo.ST_GENDER";
                                SqlCommand cmddl = new SqlCommand(sqlStatementdl, con);
                                cmddl.ExecuteNonQuery();
                            }
                            int ttl = 0;
                            for (int index = 0; index < allLines.Length; index++)
                            {
                                if (allLines[index].Length > 0)
                                {
                                    string[] items = allLines[index].Split(new char[] { ',' });
                                    cmd.Parameters["@D1"].Value = items[0];
                                    cmd.Parameters["@D2"].Value = items[1];
                                    cmd.Parameters["@D3"].Value = items[1];
                                    cmd.Parameters["@D4"].Value = 1;

                                    cmd.ExecuteNonQuery();
                                }
                                ttl++;
                            }
                            string sqlStatementdx = "insert into s_log_import values('ST_GENDER',getdate(),getdate()," + ttl.ToString() + ",'DONE',NULL,'002')";
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
                    string sqlStatementderr = "insert into s_log_import values('ST_GENDER',getdate(),getdate(),0,'FAIL','" + inExc.ToString().Replace("'", "''") + "','002')";
                    SqlCommand cmddlerr = new SqlCommand(sqlStatementderr, con);
                    cmddlerr.ExecuteNonQuery();
                    errcon.Close();
                }


        }
        static void UpdateTable5() //**************ST_APPTATUS********
        {
            //Console.WriteLine(reader.ReadToEnd());

            //StreamWriter oWriter = new StreamWriter(("D:\\webapp\\scoring\\FTP\\IN\\MASTER\\TEXT.TXT"), false, Encoding.Default);
            using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["FTPIP"].ToString()))
                //{
                //    //**************ST_APPTATUS******************
                try
                {

                    string[] importfiles = Directory.GetFiles(@"D:\webapp\scoring\FTP\IN\MASTER", "ZUTBLAPPSTAT.txt");
                    string sqlStatement =
                        "INSERT INTO dbo.ST_APPTATUS(APPROVE_CD, APPROVE_NAME,ACTIVE_FLAG) VALUES(@D1, @D2, @D3)";
                    using (SqlCommand cmd = new SqlCommand(sqlStatement, con))
                    {
                        cmd.Parameters.Add("@D1", SqlDbType.VarChar, 6);
                        cmd.Parameters.Add("@D2", SqlDbType.VarChar, 255);
                        cmd.Parameters.Add("@D3", SqlDbType.Bit);

                        foreach (string importfile in importfiles)
                        {
                            con.Open();
                            string[] allLines = File.ReadAllLines(importfile, Encoding.Default);
                            if (allLines.Length > 1)
                            {
                                string sqlStatementdl = "delete from dbo.ST_APPTATUS";
                                SqlCommand cmddl = new SqlCommand(sqlStatementdl, con);
                                cmddl.ExecuteNonQuery();
                            }
                            int ttl = 0;
                            for (int index = 0; index < allLines.Length; index++)
                            {
                                if (allLines[index].Length > 0)
                                {
                                    string[] items = allLines[index].Split(new char[] { ',' });
                                    cmd.Parameters["@D1"].Value = items[0];
                                    cmd.Parameters["@D2"].Value = items[1];
                                    cmd.Parameters["@D3"].Value = 1;

                                    cmd.ExecuteNonQuery();
                                }
                                ttl++;
                            }
                            string sqlStatementdx = "insert into s_log_import values('ST_APPTATUS',getdate(),getdate()," + ttl.ToString() + ",'DONE',NULL,'002')";
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
                    string sqlStatementderr = "insert into s_log_import values('ST_APPTATUS',getdate(),getdate(),0,'FAIL','" + inExc.ToString().Replace("'", "''") + "','002')";
                    SqlCommand cmddlerr = new SqlCommand(sqlStatementderr, con);
                    cmddlerr.ExecuteNonQuery();
                    errcon.Close();
                }


        }
        static void UpdateTable6() //**************ST_COLREQ******
        {
            //Console.WriteLine(reader.ReadToEnd());

            //StreamWriter oWriter = new StreamWriter(("D:\\webapp\\scoring\\FTP\\IN\\MASTER\\TEXT.TXT"), false, Encoding.Default);
            using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["FTPIP"].ToString()))
                //{
                //    //**************ST_COLREQ******************
                try
                {

                    string[] importfiles = Directory.GetFiles(@"D:\webapp\scoring\FTP\IN\MASTER", "ZUTBLCOLLREQ.txt");
                    string sqlStatement =
                        "INSERT INTO dbo.ST_COLREQ(COLREQ_CD,COLREQ_NAME,COLREQ_DETAIL,ACTIVE_FLAG) VALUES(@D1, @D2, @D3 , @D4)";
                    using (SqlCommand cmd = new SqlCommand(sqlStatement, con))
                    {
                        cmd.Parameters.Add("@D1", SqlDbType.VarChar, 6);
                        cmd.Parameters.Add("@D2", SqlDbType.VarChar, 100);
                        cmd.Parameters.Add("@D3", SqlDbType.VarChar, 200);
                        cmd.Parameters.Add("@D4", SqlDbType.Bit);

                        foreach (string importfile in importfiles)
                        {
                            con.Open();
                            string[] allLines = File.ReadAllLines(importfile, Encoding.Default);
                            if (allLines.Length > 1)
                            {
                                string sqlStatementdl = "delete from dbo.ST_COLREQ";
                                SqlCommand cmddl = new SqlCommand(sqlStatementdl, con);
                                cmddl.ExecuteNonQuery();
                            }
                            int ttl = 0;
                            for (int index = 0; index < allLines.Length; index++)
                            {
                                if (allLines[index].Length > 0)
                                {
                                    string[] items = allLines[index].Split(new char[] { ',' });
                                    cmd.Parameters["@D1"].Value = items[0];
                                    cmd.Parameters["@D2"].Value = items[1];
                                    cmd.Parameters["@D3"].Value = items[1];
                                    cmd.Parameters["@D4"].Value = 1;

                                    cmd.ExecuteNonQuery();
                                }
                                ttl++;
                            }
                            string sqlStatementdx = "insert into s_log_import values('ST_COLREQ',getdate(),getdate()," + ttl.ToString() + ",'DONE',NULL,'002')";
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
                    string sqlStatementderr = "insert into s_log_import values('ST_COLREQ',getdate(),getdate(),0,'FAIL','" + inExc.ToString().Replace("'", "''") + "','002')";
                    SqlCommand cmddlerr = new SqlCommand(sqlStatementderr, con);
                    cmddlerr.ExecuteNonQuery();
                    errcon.Close();
                }


        }
        static void UpdateTable7() //**************ST_COLLSTYPE******
        {
            //Console.WriteLine(reader.ReadToEnd());

            //StreamWriter oWriter = new StreamWriter(("D:\\webapp\\scoring\\FTP\\IN\\MASTER\\TEXT.TXT"), false, Encoding.Default);
            using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["FTPIP"].ToString()))
                //{
                //    //**************ST_COLLSTYPE******************
                try
                {

                    string[] importfiles = Directory.GetFiles(@"D:\webapp\scoring\FTP\IN\MASTER", "ZUTBLCOLSUBT.txt");
                    string sqlStatement =
                        "INSERT INTO dbo.ST_COLLSTYPE(COLLSTYPE_CD, COLLTYPE_CD,COLL_ID, COLLSTYPE_NAME,COLLSTYPE_DETAIL,ACTIVE_FLAG) VALUES(@D1, @D2, @D3 , @D4, @D5, @D6)";
                    using (SqlCommand cmd = new SqlCommand(sqlStatement, con))
                    {
                        cmd.Parameters.Add("@D1", SqlDbType.VarChar, 1);
                        cmd.Parameters.Add("@D2", SqlDbType.VarChar, 2);
                        cmd.Parameters.Add("@D3", SqlDbType.VarChar, 100);
                        cmd.Parameters.Add("@D4", SqlDbType.VarChar, 100);
                        cmd.Parameters.Add("@D5", SqlDbType.VarChar, 200);
                        cmd.Parameters.Add("@D6", SqlDbType.Bit);

                        foreach (string importfile in importfiles)
                        {
                            con.Open();
                            string[] allLines = File.ReadAllLines(importfile, Encoding.Default);
                            if (allLines.Length > 1)
                            {
                                string sqlStatementdl = "delete from dbo.ST_COLLSTYPE";
                                SqlCommand cmddl = new SqlCommand(sqlStatementdl, con);
                                cmddl.ExecuteNonQuery();
                            }
                            int ttl = 0;
                            for (int index = 0; index < allLines.Length; index++)
                            {
                                if (allLines[index].Length > 0)
                                {
                                    string[] items = allLines[index].Split(new char[] { ',' });
                                    cmd.Parameters["@D1"].Value = items[1];
                                    cmd.Parameters["@D2"].Value = items[0];
                                    cmd.Parameters["@D3"].Value = items[1] + items[0];
                                    cmd.Parameters["@D4"].Value = items[2];
                                    cmd.Parameters["@D5"].Value = items[2];
                                    cmd.Parameters["@D6"].Value = 1;

                                    cmd.ExecuteNonQuery();
                                }
                                ttl++;
                            }
                            string sqlStatementdx = "insert into s_log_import values('ST_COLLSTYPE',getdate(),getdate()," + ttl.ToString() + ",'DONE',NULL,'002')";
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
                    string sqlStatementderr = "insert into s_log_import values('ST_COLLSTYPE',getdate(),getdate(),0,'FAIL','" + inExc.ToString().Replace("'", "''") + "','002')";
                    SqlCommand cmddlerr = new SqlCommand(sqlStatementderr, con);
                    cmddlerr.ExecuteNonQuery();
                    errcon.Close();
                }


        }
        static void UpdateTable8() //**************ST_COLLTYPE*******
        {
            //Console.WriteLine(reader.ReadToEnd());

            //StreamWriter oWriter = new StreamWriter(("D:\\webapp\\scoring\\FTP\\IN\\MASTER\\TEXT.TXT"), false, Encoding.Default);
            using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["FTPIP"].ToString()))
                //{
                //    //**************ST_COLLTYPE******************
                try
                {

                    string[] importfiles = Directory.GetFiles(@"D:\webapp\scoring\FTP\IN\MASTER", "ZUTBLCOLTYP.txt");
                    string sqlStatement =
                        "INSERT INTO dbo.ST_COLLTYPE(COLLTYPE_CD,COLLTYPE_NAME,COLLTYPE_DETAIL,ACTIVE_FLAG) VALUES(@D1, @D2, @D3 , @D4)";
                    using (SqlCommand cmd = new SqlCommand(sqlStatement, con))
                    {
                        cmd.Parameters.Add("@D1", SqlDbType.VarChar, 2);
                        cmd.Parameters.Add("@D2", SqlDbType.VarChar, 100);
                        cmd.Parameters.Add("@D3", SqlDbType.VarChar, 200);
                        cmd.Parameters.Add("@D4", SqlDbType.Bit);

                        foreach (string importfile in importfiles)
                        {
                            con.Open();
                            string[] allLines = File.ReadAllLines(importfile, Encoding.Default);
                            if (allLines.Length > 1)
                            {
                                string sqlStatementdl = "delete from dbo.ST_COLLTYPE";
                                SqlCommand cmddl = new SqlCommand(sqlStatementdl, con);
                                cmddl.ExecuteNonQuery();
                            }
                            int ttl = 0;
                            for (int index = 0; index < allLines.Length; index++)
                            {
                                if (allLines[index].Length > 0)
                                {
                                    string[] items = allLines[index].Split(new char[] { ',' });
                                    cmd.Parameters["@D1"].Value = items[0];
                                    cmd.Parameters["@D2"].Value = items[1];
                                    cmd.Parameters["@D3"].Value = items[1];
                                    cmd.Parameters["@D4"].Value = 1;

                                    cmd.ExecuteNonQuery();
                                }
                                ttl++;
                            }
                            string sqlStatementdx = "insert into s_log_import values('ST_COLLTYPE',getdate(),getdate()," + ttl.ToString() + ",'DONE',NULL,'002')";
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
                    string sqlStatementderr = "insert into s_log_import values('ST_COLLTYPE',getdate(),getdate(),0,'FAIL','" + inExc.ToString().Replace("'", "''") + "','002')";
                    SqlCommand cmddlerr = new SqlCommand(sqlStatementderr, con);
                    cmddlerr.ExecuteNonQuery();
                    errcon.Close();
                }


        }
        static void UpdateTable9() //**************ST_GSBPURPOSE*****
        {
            //Console.WriteLine(reader.ReadToEnd());

            //StreamWriter oWriter = new StreamWriter(("D:\\webapp\\scoring\\FTP\\IN\\MASTER\\TEXT.TXT"), false, Encoding.Default);
            using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["FTPIP"].ToString()))
                //{
                //    //**************ST_GSBPURPOSE******************
                try
                {

                    string[] importfiles = Directory.GetFiles(@"D:\webapp\scoring\FTP\IN\MASTER", "ZUTBLGSBPUR.txt");
                    string sqlStatement =
                        "INSERT INTO dbo.ST_GSBPURPOSE(GSBPURPOSE_CD, GPURPOSE_NAME,GPURPOSE_DETAIL,ACTIVE_FLAG) VALUES(@D1, @D2, @D3 , @D4)";
                    using (SqlCommand cmd = new SqlCommand(sqlStatement, con))
                    {
                        cmd.Parameters.Add("@D1", SqlDbType.VarChar, 12);
                        cmd.Parameters.Add("@D2", SqlDbType.VarChar, 100);
                        cmd.Parameters.Add("@D3", SqlDbType.VarChar, 200);
                        cmd.Parameters.Add("@D4", SqlDbType.Bit);

                        foreach (string importfile in importfiles)
                        {
                            con.Open();
                            string[] allLines = File.ReadAllLines(importfile, Encoding.Default);
                            if (allLines.Length > 1)
                            {
                                string sqlStatementdl = "delete from dbo.ST_GSBPURPOSE";
                                SqlCommand cmddl = new SqlCommand(sqlStatementdl, con);
                                cmddl.ExecuteNonQuery();
                            }
                            int ttl = 0;
                            for (int index = 0; index < allLines.Length; index++)
                            {
                                if (allLines[index].Length > 0)
                                {
                                    string[] items = allLines[index].Split(new char[] { ',' });
                                    cmd.Parameters["@D1"].Value = items[0];
                                    cmd.Parameters["@D2"].Value = items[1];
                                    cmd.Parameters["@D3"].Value = items[1];
                                    cmd.Parameters["@D4"].Value = 1;

                                    cmd.ExecuteNonQuery();
                                }
                                ttl++;
                            }
                            string sqlStatementdx = "insert into s_log_import values('ST_GSBPURPOSE',getdate(),getdate()," + ttl.ToString() + ",'DONE',NULL,'002')";
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
                    string sqlStatementderr = "insert into s_log_import values('ST_GSBPURPOSE',getdate(),getdate(),0,'FAIL','" + inExc.ToString().Replace("'", "''") + "','002')";
                    SqlCommand cmddlerr = new SqlCommand(sqlStatementderr, con);
                    cmddlerr.ExecuteNonQuery();
                    errcon.Close();
                }


        }
        static void UpdateTable10() //**************ST_ISIC1***
        {
            //Console.WriteLine(reader.ReadToEnd());

            //StreamWriter oWriter = new StreamWriter(("D:\\webapp\\scoring\\FTP\\IN\\MASTER\\TEXT.TXT"), false, Encoding.Default);
            using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["FTPIP"].ToString()))
                //{
                //    //**************ST_ISIC1******************
                try
                {

                    string[] importfiles = Directory.GetFiles(@"D:\webapp\scoring\FTP\IN\MASTER", "ZUTBLISICSD.txt");
                    string sqlStatement =
                        "INSERT INTO dbo.ST_ISIC1(ISIC1_CD,ISIC1_NAME,ISIC1_DETAIL,ACTIVE_FLAG) VALUES(@D1, @D2, @D3 , @D4)";
                    using (SqlCommand cmd = new SqlCommand(sqlStatement, con))
                    {
                        cmd.Parameters.Add("@D1", SqlDbType.VarChar, 3);
                        cmd.Parameters.Add("@D2", SqlDbType.VarChar, 200);
                        cmd.Parameters.Add("@D3", SqlDbType.VarChar, 200);
                        cmd.Parameters.Add("@D4", SqlDbType.Bit);

                        foreach (string importfile in importfiles)
                        {
                            con.Open();
                            string[] allLines = File.ReadAllLines(importfile, Encoding.Default);
                            if (allLines.Length > 1)
                            {
                                string sqlStatementdl = "delete from dbo.ST_ISIC1";
                                SqlCommand cmddl = new SqlCommand(sqlStatementdl, con);
                                cmddl.ExecuteNonQuery();
                            }
                            int ttl = 0;
                            for (int index = 0; index < allLines.Length; index++)
                            {
                                if (allLines[index].Length > 0)
                                {
                                    string[] items = allLines[index].Split(new char[] { ',' });
                                    cmd.Parameters["@D1"].Value = items[0];
                                    cmd.Parameters["@D2"].Value = items[1];
                                    cmd.Parameters["@D3"].Value = items[2];
                                    cmd.Parameters["@D4"].Value = 1;

                                    cmd.ExecuteNonQuery();
                                }
                                ttl++;
                            }
                            string sqlStatementdx = "insert into s_log_import values('ST_ISIC1',getdate(),getdate()," + ttl.ToString() + ",'DONE',NULL,'002')";
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
                    string sqlStatementderr = "insert into s_log_import values('ST_ISIC1',getdate(),getdate(),0,'FAIL','" + inExc.ToString().Replace("'", "''") + "','002')";
                    SqlCommand cmddlerr = new SqlCommand(sqlStatementderr, con);
                    cmddlerr.ExecuteNonQuery();
                    errcon.Close();
                }
        }
        static void UpdateTable11() //**************ST_ISIC2*******
        {
            //Console.WriteLine(reader.ReadToEnd());

            //StreamWriter oWriter = new StreamWriter(("D:\\webapp\\scoring\\FTP\\IN\\MASTER\\TEXT.TXT"), false, Encoding.Default);
            using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["FTPIP"].ToString()))
                //{
                //    //**************ST_ISIC2******************
                try
                {

                    string[] importfiles = Directory.GetFiles(@"D:\webapp\scoring\FTP\IN\MASTER", "ZUTBLISICGC.txt");
                    string sqlStatement =
                        "INSERT INTO dbo.ST_ISIC2(ISIC1_CD,ISIC2_CD,ISIC2_NAME,ISIC2_DETAIL,ACTIVE_FLAG) VALUES(@D1, @D2, @D3 , @D4 , @D5)";
                    using (SqlCommand cmd = new SqlCommand(sqlStatement, con))
                    {
                        cmd.Parameters.Add("@D1", SqlDbType.VarChar, 3);
                        cmd.Parameters.Add("@D2", SqlDbType.VarChar, 10);
                        cmd.Parameters.Add("@D3", SqlDbType.VarChar, 200);
                        cmd.Parameters.Add("@D4", SqlDbType.VarChar, 200);
                        cmd.Parameters.Add("@D5", SqlDbType.Bit);

                        foreach (string importfile in importfiles)
                        {
                            con.Open();
                            string[] allLines = File.ReadAllLines(importfile, Encoding.Default);
                            if (allLines.Length > 1)
                            {
                                string sqlStatementdl = "delete from dbo.ST_ISIC2";
                                SqlCommand cmddl = new SqlCommand(sqlStatementdl, con);
                                cmddl.ExecuteNonQuery();
                            }
                            int ttl = 0;
                            for (int index = 0; index < allLines.Length; index++)
                            {
                                if (allLines[index].Length > 0)
                                {
                                    string[] items = allLines[index].Split(new char[] { ',' });
                                    cmd.Parameters["@D1"].Value = items[0];
                                    cmd.Parameters["@D2"].Value = items[1];
                                    cmd.Parameters["@D3"].Value = items[2];
                                    cmd.Parameters["@D4"].Value = items[3];
                                    cmd.Parameters["@D5"].Value = 1;

                                    cmd.ExecuteNonQuery();
                                }
                                ttl++;
                            }
                            string sqlStatementdx = "insert into s_log_import values('ST_ISIC2',getdate(),getdate()," + ttl.ToString() + ",'DONE',NULL,'002')";
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
                    string sqlStatementderr = "insert into s_log_import values('ST_ISIC2',getdate(),getdate(),0,'FAIL','" + inExc.ToString().Replace("'", "''") + "','002')";
                    SqlCommand cmddlerr = new SqlCommand(sqlStatementderr, con);
                    cmddlerr.ExecuteNonQuery();
                    errcon.Close();
                }


        }
        static void UpdateTable12() //**************ST_ISIC3*******
        {
            //Console.WriteLine(reader.ReadToEnd());

            //StreamWriter oWriter = new StreamWriter(("D:\\webapp\\scoring\\FTP\\IN\\MASTER\\TEXT.TXT"), false, Encoding.Default);
            using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["FTPIP"].ToString()))
                //{
                //    //**************ST_ISIC3******************
                try
                {

                    string[] importfiles = Directory.GetFiles(@"D:\webapp\scoring\FTP\IN\MASTER", "ZUTBLISICTS.txt");
                    string sqlStatement =
                        "INSERT INTO dbo.ST_ISIC3(ISIC1_CD,ISIC2_CD,ISIC3_CD,ISIC3_NAME,ISIC3_DETAIL,ACTIVE_FLAG)" +
                        "VALUES(@D1, @D2, @D3 , @D4 , @D5,@D6)";
                    using (SqlCommand cmd = new SqlCommand(sqlStatement, con))
                    {
                        cmd.Parameters.Add("@D1", SqlDbType.VarChar, 3);
                        cmd.Parameters.Add("@D2", SqlDbType.VarChar, 10);
                        cmd.Parameters.Add("@D3", SqlDbType.VarChar, 3);
                        cmd.Parameters.Add("@D4", SqlDbType.VarChar, 200);
                        cmd.Parameters.Add("@D5", SqlDbType.VarChar, 200);
                        cmd.Parameters.Add("@D6", SqlDbType.Bit);

                        foreach (string importfile in importfiles)
                        {
                            con.Open();
                            string[] allLines = File.ReadAllLines(importfile, Encoding.Default);
                            if (allLines.Length > 1)
                            {
                                string sqlStatementdl = "delete from dbo.ST_ISIC3";
                                SqlCommand cmddl = new SqlCommand(sqlStatementdl, con);
                                cmddl.ExecuteNonQuery();
                            }
                            int ttl = 0;
                            for (int index = 0; index < allLines.Length; index++)
                            {
                                if (allLines[index].Length > 0)
                                {
                                    string[] items = allLines[index].Split(new char[] { ',' });
                                    cmd.Parameters["@D1"].Value = items[0];
                                    cmd.Parameters["@D2"].Value = items[1];
                                    cmd.Parameters["@D3"].Value = items[2];
                                    cmd.Parameters["@D4"].Value = items[3];
                                    cmd.Parameters["@D5"].Value = items[4];
                                    cmd.Parameters["@D6"].Value = 1;

                                    cmd.ExecuteNonQuery();
                                }
                                ttl++;
                            }
                            string sqlStatementdx = "insert into s_log_import values('ST_ISIC3',getdate(),getdate()," + ttl.ToString() + ",'DONE',NULL,'002')";
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
                    string sqlStatementderr = "insert into s_log_import values('ST_ISIC3',getdate(),getdate(),0,'FAIL','" + inExc.ToString().Replace("'", "''") + "','002')";
                    SqlCommand cmddlerr = new SqlCommand(sqlStatementderr, con);
                    cmddlerr.ExecuteNonQuery();
                    errcon.Close();
                }


        }
        static void UpdateTable13() //**************ST_CCODE*****
        {
            //Console.WriteLine(reader.ReadToEnd());

            //StreamWriter oWriter = new StreamWriter(("D:\\webapp\\scoring\\FTP\\IN\\MASTER\\TEXT.TXT"), false, Encoding.Default);
            using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["FTPIP"].ToString()))
                //{
                //    //**************ST_CCODE******************
                try
                {

                    string[] importfiles = Directory.GetFiles(@"D:\webapp\scoring\FTP\IN\MASTER", "ZUTBLKTBCUST.txt");
                    string sqlStatement =
                        "INSERT INTO dbo.ST_CCODE(CCODE_CD,CCODE_NAME,CCODE_DETAIL,ACTIVE_FLAG) VALUES(@D1, @D2, @D3 , @D4)";
                    using (SqlCommand cmd = new SqlCommand(sqlStatement, con))
                    {
                        cmd.Parameters.Add("@D1", SqlDbType.VarChar, 5);
                        cmd.Parameters.Add("@D2", SqlDbType.VarChar, 200);
                        cmd.Parameters.Add("@D3", SqlDbType.VarChar, 200);
                        cmd.Parameters.Add("@D4", SqlDbType.Bit);

                        foreach (string importfile in importfiles)
                        {
                            con.Open();
                            string[] allLines = File.ReadAllLines(importfile, Encoding.Default);
                            if (allLines.Length > 1)
                            {
                                string sqlStatementdl = "delete from dbo.ST_CCODE";
                                SqlCommand cmddl = new SqlCommand(sqlStatementdl, con);
                                cmddl.ExecuteNonQuery();
                            }
                            int ttl = 0;
                            for (int index = 0; index < allLines.Length; index++)
                            {
                                if (allLines[index].Length > 0)
                                {
                                    string[] items = allLines[index].Split(new char[] { ',' });
                                    cmd.Parameters["@D1"].Value = items[0];
                                    cmd.Parameters["@D2"].Value = items[1];
                                    cmd.Parameters["@D3"].Value = items[2];
                                    cmd.Parameters["@D4"].Value = 1;

                                    cmd.ExecuteNonQuery();
                                }
                                ttl++;
                            }
                            string sqlStatementdx = "insert into s_log_import values('ST_CCODE',getdate(),getdate()," + ttl.ToString() + ",'DONE',NULL,'002')";
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
                    string sqlStatementderr = "insert into s_log_import values('ST_CCODE',getdate(),getdate(),0,'FAIL','" + inExc.ToString().Replace("'", "''") + "','002')";
                    SqlCommand cmddlerr = new SqlCommand(sqlStatementderr, con);
                    cmddlerr.ExecuteNonQuery();
                    errcon.Close();
                }


        }
        static void UpdateTable14() //**************LN_REASON*****
        {
            //Console.WriteLine(reader.ReadToEnd());

            //StreamWriter oWriter = new StreamWriter(("D:\\webapp\\scoring\\FTP\\IN\\MASTER\\TEXT.TXT"), false, Encoding.Default);
            using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["FTPIP"].ToString()))
                //{
                //    //**************LN_REASON******************
                try
                {

                    string[] importfiles = Directory.GetFiles(@"D:\webapp\scoring\FTP\IN\MASTER", "ZUTBLLNAPAPRS.txt");
                    string sqlStatement =
                        "INSERT INTO dbo.LN_REASON(REASON_CD,APPROVE_CD,REASON_NAME,ACTIVE_FLAG) VALUES(@D1, @D2, @D3 , @D4)";
                    using (SqlCommand cmd = new SqlCommand(sqlStatement, con))
                    {
                        cmd.Parameters.Add("@D1", SqlDbType.VarChar, 5);
                        cmd.Parameters.Add("@D2", SqlDbType.VarChar, 1);
                        cmd.Parameters.Add("@D3", SqlDbType.VarChar, 255);
                        cmd.Parameters.Add("@D4", SqlDbType.Bit);

                        foreach (string importfile in importfiles)
                        {
                            con.Open();
                            string[] allLines = File.ReadAllLines(importfile, Encoding.Default);
                            if (allLines.Length > 1)
                            {
                                string sqlStatementdl = "delete from dbo.LN_REASON";
                                SqlCommand cmddl = new SqlCommand(sqlStatementdl, con);
                                cmddl.ExecuteNonQuery();
                            }
                            int ttl = 0;
                            for (int index = 0; index < allLines.Length; index++)
                            {
                                if (allLines[index].Length > 0)
                                {
                                    string[] items = allLines[index].Split(new char[] { ',' });
                                    cmd.Parameters["@D1"].Value = items[0];
                                    cmd.Parameters["@D2"].Value = items[1];
                                    cmd.Parameters["@D3"].Value = items[2];
                                    cmd.Parameters["@D4"].Value = 1;

                                    cmd.ExecuteNonQuery();
                                }
                                ttl++;
                            }
                            string sqlStatementdx = "insert into s_log_import values('LN_REASON',getdate(),getdate()," + ttl.ToString() + ",'DONE',NULL,'002')";
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
                    string sqlStatementderr = "insert into s_log_import values('LN_REASON',getdate(),getdate(),0,'FAIL','" + inExc.ToString().Replace("'", "''") + "','002')";
                    SqlCommand cmddlerr = new SqlCommand(sqlStatementderr, con);
                    cmddlerr.ExecuteNonQuery();
                    errcon.Close();
                }


        }
        static void UpdateTable15() //**************ST_ASSETHOLDSTA****
        {
            //Console.WriteLine(reader.ReadToEnd());

            //StreamWriter oWriter = new StreamWriter(("D:\\webapp\\scoring\\FTP\\IN\\MASTER\\TEXT.TXT"), false, Encoding.Default);
            using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["FTPIP"].ToString()))
                //{
                //    //**************ST_ASSETHOLDSTA******************
                try
                {

                    string[] importfiles = Directory.GetFiles(@"D:\webapp\scoring\FTP\IN\MASTER", "ZUTBLLNAPASHLD.txt");
                    string sqlStatement =
                         "INSERT INTO dbo.ST_ASSETHOLDSTA(ASSETHOLD_CD,ASSETHOLD_NAME ,ASSETHOLD_DETAIL,ACTIVE_FLAG) VALUES(@D1, @D2, @D3 , @D4)";
                    using (SqlCommand cmd = new SqlCommand(sqlStatement, con))
                    {
                        cmd.Parameters.Add("@D1", SqlDbType.VarChar, 5);
                        cmd.Parameters.Add("@D2", SqlDbType.VarChar, 100);
                        cmd.Parameters.Add("@D3", SqlDbType.VarChar, 200);
                        cmd.Parameters.Add("@D4", SqlDbType.Bit);

                        foreach (string importfile in importfiles)
                        {
                            con.Open();
                            string[] allLines = File.ReadAllLines(importfile, Encoding.Default);
                            if (allLines.Length > 1)
                            {
                                string sqlStatementdl = "delete from dbo.ST_ASSETHOLDSTA";
                                SqlCommand cmddl = new SqlCommand(sqlStatementdl, con);
                                cmddl.ExecuteNonQuery();
                            }
                            int ttl = 0;
                            for (int index = 0; index < allLines.Length; index++)
                            {
                                if (allLines[index].Length > 0)
                                {
                                    string[] items = allLines[index].Split(new char[] { ',' });
                                    cmd.Parameters["@D1"].Value = items[0];
                                    cmd.Parameters["@D2"].Value = items[1];
                                    cmd.Parameters["@D3"].Value = items[1];
                                    cmd.Parameters["@D4"].Value = 1;

                                    cmd.ExecuteNonQuery();
                                }
                                ttl++;
                            }
                            string sqlStatementdx = "insert into s_log_import values('ST_ASSETHOLDSTA',getdate(),getdate()," + ttl.ToString() + ",'DONE',NULL,'002')";
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
                    string sqlStatementderr = "insert into s_log_import values('ST_ASSETHOLDSTA',getdate(),getdate(),0,'FAIL','" + inExc.ToString().Replace("'", "''") + "','002')";
                    SqlCommand cmddlerr = new SqlCommand(sqlStatementderr, con);
                    cmddlerr.ExecuteNonQuery();
                    errcon.Close();
                }


        }
        static void UpdateTable16() //**************LN_COMMITTEE******
        {
            //Console.WriteLine(reader.ReadToEnd());

            //StreamWriter oWriter = new StreamWriter(("D:\\webapp\\scoring\\FTP\\IN\\MASTER\\TEXT.TXT"), false, Encoding.Default);
            using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["FTPIP"].ToString()))
                //{
                //    //**************LN_COMMITTEE******************
                try
                {

                    string[] importfiles = Directory.GetFiles(@"D:\webapp\scoring\FTP\IN\MASTER", "ZUTBLLNAPCMTL.txt");
                    string sqlStatement =
                        "INSERT INTO dbo.LN_COMMITTEE(COMMITTEE_CD,COMMITTEE_NAME,COMMITTEE_DETAIL,ACTIVE_FLAG) VALUES(@D1, @D2, @D3 , @D4)";
                    using (SqlCommand cmd = new SqlCommand(sqlStatement, con))
                    {
                        cmd.Parameters.Add("@D1", SqlDbType.VarChar, 5);
                        cmd.Parameters.Add("@D2", SqlDbType.VarChar, 100);
                        cmd.Parameters.Add("@D3", SqlDbType.VarChar, 500);
                        cmd.Parameters.Add("@D4", SqlDbType.Bit);

                        foreach (string importfile in importfiles)
                        {
                            con.Open();
                            string[] allLines = File.ReadAllLines(importfile, Encoding.Default);
                            if (allLines.Length > 1)
                            {
                                string sqlStatementdl = "delete from dbo.LN_COMMITTEE";
                                SqlCommand cmddl = new SqlCommand(sqlStatementdl, con);
                                cmddl.ExecuteNonQuery();
                            }
                            int ttl = 0;
                            for (int index = 0; index < allLines.Length; index++)
                            {
                                if (allLines[index].Length > 0)
                                {
                                    string[] items = allLines[index].Split(new char[] { ',' });
                                    cmd.Parameters["@D1"].Value = items[0];
                                    cmd.Parameters["@D2"].Value = items[1];
                                    cmd.Parameters["@D3"].Value = items[1];
                                    cmd.Parameters["@D4"].Value = 1;

                                    cmd.ExecuteNonQuery();
                                }
                                ttl++;
                            }
                            string sqlStatementdx = "insert into s_log_import values('LN_COMMITTEE',getdate(),getdate()," + ttl.ToString() + ",'DONE',NULL,'002')";
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
                    string sqlStatementderr = "insert into s_log_import values('LN_COMMITTEE',getdate(),getdate(),0,'FAIL','" + inExc.ToString().Replace("'", "''") + "','002')";
                    SqlCommand cmddlerr = new SqlCommand(sqlStatementderr, con);
                    cmddlerr.ExecuteNonQuery();
                    errcon.Close();

                }


        }
        static void UpdateTable17() //**************ST_RESSTATUS******
        {
            //Console.WriteLine(reader.ReadToEnd());

            //StreamWriter oWriter = new StreamWriter(("D:\\webapp\\scoring\\FTP\\IN\\MASTER\\TEXT.TXT"), false, Encoding.Default);
            using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["FTPIP"].ToString()))
                //{
                //    //**************ST_RESSTATUS******************
                try
                {

                    string[] importfiles = Directory.GetFiles(@"D:\webapp\scoring\FTP\IN\MASTER", "ZUTBLLNAPRSTAT.txt");
                    string sqlStatement =
                        "INSERT INTO dbo.ST_RESSTATUS(RESSTATUS_CD, RESSTATUS_NAME,RESSTATUS_DETAIL,ACTIVE_FLAG) VALUES(@D1, @D2, @D3 , @D4)";
                    using (SqlCommand cmd = new SqlCommand(sqlStatement, con))
                    {
                        cmd.Parameters.Add("@D1", SqlDbType.VarChar, 5);
                        cmd.Parameters.Add("@D2", SqlDbType.VarChar, 100);
                        cmd.Parameters.Add("@D3", SqlDbType.VarChar, 200);
                        cmd.Parameters.Add("@D4", SqlDbType.Bit);

                        foreach (string importfile in importfiles)
                        {
                            con.Open();
                            string[] allLines = File.ReadAllLines(importfile, Encoding.Default);
                            if (allLines.Length > 1)
                            {
                                string sqlStatementdl = "delete from dbo.ST_RESSTATUS";
                                SqlCommand cmddl = new SqlCommand(sqlStatementdl, con);
                                cmddl.ExecuteNonQuery();
                            }
                            int ttl = 0;
                            for (int index = 0; index < allLines.Length; index++)
                            {
                                if (allLines[index].Length > 0)
                                {
                                    string[] items = allLines[index].Split(new char[] { ',' });
                                    cmd.Parameters["@D1"].Value = items[0];
                                    cmd.Parameters["@D2"].Value = items[1];
                                    cmd.Parameters["@D3"].Value = items[1];
                                    cmd.Parameters["@D4"].Value = 1;

                                    cmd.ExecuteNonQuery();
                                }
                                ttl++;
                            }
                            string sqlStatementdx = "insert into s_log_import values('ST_RESSTATUS',getdate(),getdate()," + ttl.ToString() + ",'DONE',NULL,'002')";
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
                    string sqlStatementderr = "insert into s_log_import values('ST_RESSTATUS',getdate(),getdate(),0,'FAIL','" + inExc.ToString().Replace("'", "''") + "','002')";
                    SqlCommand cmddlerr = new SqlCommand(sqlStatementderr, con);
                    cmddlerr.ExecuteNonQuery();
                    errcon.Close();
                }


        }
        static void UpdateTable18()  //**************ST_BUSSTYPE******
        {
            //Console.WriteLine(reader.ReadToEnd());

            //StreamWriter oWriter = new StreamWriter(("D:\\webapp\\scoring\\FTP\\IN\\MASTER\\TEXT.TXT"), false, Encoding.Default);
            using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["FTPIP"].ToString()))
                //{
                //    //**************ST_BUSSTYPE******************
                try
                {

                    string[] importfiles = Directory.GetFiles(@"D:\webapp\scoring\FTP\IN\MASTER", "ZUTBLLNAPESTYP.txt");
                    string sqlStatement =
                        "INSERT INTO dbo.ST_BUSSTYPE(BUSSTYPE_CD,BUSSTYPET_NAME,BUSSTYPE_DETAIL,ACTIVE_FLAG) VALUES(@D1, @D2, @D3 , @D4)";
                    using (SqlCommand cmd = new SqlCommand(sqlStatement, con))
                    {
                        cmd.Parameters.Add("@D1", SqlDbType.VarChar, 5);
                        cmd.Parameters.Add("@D2", SqlDbType.VarChar, 100);
                        cmd.Parameters.Add("@D3", SqlDbType.VarChar, 200);
                        cmd.Parameters.Add("@D4", SqlDbType.Bit);

                        foreach (string importfile in importfiles)
                        {
                            con.Open();
                            string[] allLines = File.ReadAllLines(importfile, Encoding.Default);
                            if (allLines.Length > 1)
                            {
                                string sqlStatementdl = "delete from dbo.ST_BUSSTYPE";
                                SqlCommand cmddl = new SqlCommand(sqlStatementdl, con);
                                cmddl.ExecuteNonQuery();
                            }
                            int ttl = 0;
                            for (int index = 0; index < allLines.Length; index++)
                            {
                                if (allLines[index].Length > 0)
                                {
                                    string[] items = allLines[index].Split(new char[] { ',' });
                                    cmd.Parameters["@D1"].Value = items[0];
                                    cmd.Parameters["@D2"].Value = items[1];
                                    cmd.Parameters["@D3"].Value = items[1];
                                    cmd.Parameters["@D4"].Value = 1;

                                    cmd.ExecuteNonQuery();
                                }
                                ttl++;
                            }
                            string sqlStatementdx = "insert into s_log_import values('ST_BUSSTYPE',getdate(),getdate()," + ttl.ToString() + ",'DONE',NULL,'002')";
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
                    string sqlStatementderr = "insert into s_log_import values('ST_BUSSTYPE',getdate(),getdate(),0,'FAIL','" + inExc.ToString().Replace("'", "''") + "','002')";
                    SqlCommand cmddlerr = new SqlCommand(sqlStatementderr, con);
                    cmddlerr.ExecuteNonQuery();
                    errcon.Close();
                }


        }
        static void UpdateTable19()  //**************ST_RESTYPE*****
        {
            //Console.WriteLine(reader.ReadToEnd());

            //StreamWriter oWriter = new StreamWriter(("D:\\webapp\\scoring\\FTP\\IN\\MASTER\\TEXT.TXT"), false, Encoding.Default);
            using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["FTPIP"].ToString()))
                //{
                //    //**************ST_RESTYPE******************
                try
                {

                    string[] importfiles = Directory.GetFiles(@"D:\webapp\scoring\FTP\IN\MASTER", "ZUTBLLNAPRTYP.txt");
                    string sqlStatement =
                        "INSERT INTO dbo.ST_RESTYPE(RESTYPE_CD, RESTYPE_NAME,RESTYPE_DETAIL,ACTIVE_FLAG) VALUES(@D1, @D2, @D3 , @D4)";
                    using (SqlCommand cmd = new SqlCommand(sqlStatement, con))
                    {
                        cmd.Parameters.Add("@D1", SqlDbType.VarChar, 5);
                        cmd.Parameters.Add("@D2", SqlDbType.VarChar, 100);
                        cmd.Parameters.Add("@D3", SqlDbType.VarChar, 200);
                        cmd.Parameters.Add("@D4", SqlDbType.Bit);

                        foreach (string importfile in importfiles)
                        {
                            con.Open();
                            string[] allLines = File.ReadAllLines(importfile, Encoding.Default);
                            if (allLines.Length > 1)
                            {
                                string sqlStatementdl = "delete from dbo.ST_RESTYPE";
                                SqlCommand cmddl = new SqlCommand(sqlStatementdl, con);
                                cmddl.ExecuteNonQuery();
                            }
                            int ttl = 0;
                            for (int index = 0; index < allLines.Length; index++)
                            {
                                if (allLines[index].Length > 0)
                                {
                                    string[] items = allLines[index].Split(new char[] { ',' });
                                    cmd.Parameters["@D1"].Value = items[0];
                                    cmd.Parameters["@D2"].Value = items[1];
                                    cmd.Parameters["@D3"].Value = items[1];
                                    cmd.Parameters["@D4"].Value = 1;

                                    cmd.ExecuteNonQuery();
                                }
                                ttl++;
                            }
                            string sqlStatementdx = "insert into s_log_import values('ST_RESTYPE',getdate(),getdate()," + ttl.ToString() + ",'DONE',NULL,'002')";
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
                    string sqlStatementderr = "insert into s_log_import values('ST_RESTYPE',getdate(),getdate(),0,'FAIL','" + inExc.ToString().Replace("'", "''") + "','002')";
                    SqlCommand cmddlerr = new SqlCommand(sqlStatementderr, con);
                    cmddlerr.ExecuteNonQuery();
                    errcon.Close();
                }


        }
        static void UpdateTable20() //**************ST_PROP_RIGHT*****
        {
            //Console.WriteLine(reader.ReadToEnd());

            //StreamWriter oWriter = new StreamWriter(("D:\\webapp\\scoring\\FTP\\IN\\MASTER\\TEXT.TXT"), false, Encoding.Default);
            using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["FTPIP"].ToString()))
                //{
                //    //**************ST_PROP_RIGHT******************
                try
                {

                    string[] importfiles = Directory.GetFiles(@"D:\webapp\scoring\FTP\IN\MASTER", "ZUTBLLNAPESOWN.txt");
                    string sqlStatement =
                        "INSERT INTO dbo.ST_PROP_RIGHT(PROP_RIGHT_CD,PROP_RIGHT_NAME,PROP_RIGHT_DETAIL,ACTIVE_FLAG) VALUES(@D1, @D2, @D3 , @D4)";
                    using (SqlCommand cmd = new SqlCommand(sqlStatement, con))
                    {
                        cmd.Parameters.Add("@D1", SqlDbType.VarChar, 5);
                        cmd.Parameters.Add("@D2", SqlDbType.VarChar, 100);
                        cmd.Parameters.Add("@D3", SqlDbType.VarChar, 200);
                        cmd.Parameters.Add("@D4", SqlDbType.Bit);

                        foreach (string importfile in importfiles)
                        {
                            con.Open();
                            string[] allLines = File.ReadAllLines(importfile, Encoding.Default);
                            if (allLines.Length > 1)
                            {
                                string sqlStatementdl = "delete from dbo.ST_PROP_RIGHT";
                                SqlCommand cmddl = new SqlCommand(sqlStatementdl, con);
                                cmddl.ExecuteNonQuery();
                            }
                            int ttl = 0;
                            for (int index = 0; index < allLines.Length; index++)
                            {
                                if (allLines[index].Length > 0)
                                {
                                    string[] items = allLines[index].Split(new char[] { ',' });
                                    cmd.Parameters["@D1"].Value = items[0];
                                    cmd.Parameters["@D2"].Value = items[1];
                                    cmd.Parameters["@D3"].Value = items[1];
                                    cmd.Parameters["@D4"].Value = 1;

                                    cmd.ExecuteNonQuery();
                                }
                                ttl++;
                            }
                            string sqlStatementdx = "insert into s_log_import values('ST_PROP_RIGHT',getdate(),getdate()," + ttl.ToString() + ",'DONE',NULL,'002')";
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
                    string sqlStatementderr = "insert into s_log_import values('ST_PROP_RIGHT',getdate(),getdate(),0,'FAIL','" + inExc.ToString().Replace("'", "''") + "','002')";
                    SqlCommand cmddlerr = new SqlCommand(sqlStatementderr, con);
                    cmddlerr.ExecuteNonQuery();
                    errcon.Close();
                }


        }
        static void UpdateTable21() //**************ST_OCCCLS*****
        {
            //Console.WriteLine(reader.ReadToEnd());

            //StreamWriter oWriter = new StreamWriter(("D:\\webapp\\scoring\\FTP\\IN\\MASTER\\TEXT.TXT"), false, Encoding.Default);
            using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["FTPIP"].ToString()))
                //{
                //    //**************ST_OCCCLS******************
                try
                {

                    string[] importfiles = Directory.GetFiles(@"D:\webapp\scoring\FTP\IN\MASTER", "ZUTBLOC.txt");
                    string sqlStatement =
                        "INSERT INTO dbo.ST_OCCCLS(OCCCLS_CD, OCCCLS_NAME,OCCCLS_DETAIL,ACTIVE_FLAG) VALUES(@D1, @D2, @D3 , @D4)";
                    using (SqlCommand cmd = new SqlCommand(sqlStatement, con))
                    {
                        cmd.Parameters.Add("@D1", SqlDbType.VarChar, 4);
                        cmd.Parameters.Add("@D2", SqlDbType.VarChar, 100);
                        cmd.Parameters.Add("@D3", SqlDbType.VarChar, 200);
                        cmd.Parameters.Add("@D4", SqlDbType.Bit);

                        foreach (string importfile in importfiles)
                        {
                            con.Open();
                            string[] allLines = File.ReadAllLines(importfile, Encoding.Default);
                            if (allLines.Length > 1)
                            {
                                string sqlStatementdl = "delete from dbo.ST_OCCCLS";
                                SqlCommand cmddl = new SqlCommand(sqlStatementdl, con);
                                cmddl.ExecuteNonQuery();
                            }
                            int ttl = 0;
                            for (int index = 0; index < allLines.Length; index++)
                            {
                                if (allLines[index].Length > 0)
                                {
                                    string[] items = allLines[index].Split(new char[] { ',' });
                                    cmd.Parameters["@D1"].Value = items[0];
                                    cmd.Parameters["@D2"].Value = items[1];
                                    cmd.Parameters["@D3"].Value = items[2];
                                    cmd.Parameters["@D4"].Value = 1;

                                    cmd.ExecuteNonQuery();
                                }
                                ttl++;
                            }
                            string sqlStatementdx = "insert into s_log_import values('ST_OCCCLS',getdate(),getdate()," + ttl.ToString() + ",'DONE',NULL,'002')";
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
                    string sqlStatementderr = "insert into s_log_import values('ST_OCCCLS',getdate(),getdate(),0,'FAIL','" + inExc.ToString().Replace("'", "''") + "','002')";
                    SqlCommand cmddlerr = new SqlCommand(sqlStatementderr, con);
                    cmddlerr.ExecuteNonQuery();
                    errcon.Close();
                }


        }
        static void UpdateTable22() //**************ST_CONSUMPTION*****
        {
            //Console.WriteLine(reader.ReadToEnd());

            //StreamWriter oWriter = new StreamWriter(("D:\\webapp\\scoring\\FTP\\IN\\MASTER\\TEXT.TXT"), false, Encoding.Default);
            using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["FTPIP"].ToString()))
                //{
                //    //**************ST_CONSUMPTION******************
                try
                {

                    string[] importfiles = Directory.GetFiles(@"D:\webapp\scoring\FTP\IN\MASTER", "ZUTBLPERCONS.txt");
                    string sqlStatement =
                        "INSERT INTO dbo.ST_CONSUMPTION(CONSUMPTION_CD,LOAN_CD,CONSUMPTION_NAME,CONSUMPTION_DETAIL,ACTIVE_FLAG) VALUES(@D1, @D2, @D3 , @D4, @D5)";
                    using (SqlCommand cmd = new SqlCommand(sqlStatement, con))
                    {
                        cmd.Parameters.Add("@D1", SqlDbType.VarChar, 6);
                        cmd.Parameters.Add("@D2", SqlDbType.VarChar, 4);
                        cmd.Parameters.Add("@D3", SqlDbType.VarChar, 100);
                        cmd.Parameters.Add("@D4", SqlDbType.VarChar, 200);
                        cmd.Parameters.Add("@D5", SqlDbType.Bit);

                        foreach (string importfile in importfiles)
                        {
                            con.Open();
                            string[] allLines = File.ReadAllLines(importfile, Encoding.Default);
                            if (allLines.Length > 1)
                            {
                                string sqlStatementdl = "delete from dbo.ST_CONSUMPTION";
                                SqlCommand cmddl = new SqlCommand(sqlStatementdl, con);
                                cmddl.ExecuteNonQuery();
                            }
                            int ttl = 0;
                            for (int index = 0; index < allLines.Length; index++)
                            {
                                if (allLines[index].Length > 0)
                                {
                                    string[] items = allLines[index].Split(new char[] { ',' });
                                    cmd.Parameters["@D1"].Value = items[1];
                                    cmd.Parameters["@D2"].Value = items[0];
                                    cmd.Parameters["@D3"].Value = items[2];
                                    cmd.Parameters["@D4"].Value = items[2];
                                    cmd.Parameters["@D5"].Value = 1;

                                    cmd.ExecuteNonQuery();
                                }
                                ttl++;
                            }
                            string sqlStatementdx = "insert into s_log_import values('ST_CONSUMPTION',getdate(),getdate()," + ttl.ToString() + ",'DONE',NULL,'002')";
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
                    string sqlStatementderr = "insert into s_log_import values('ST_CONSUMPTION',getdate(),getdate(),0,'FAIL','" + inExc.ToString().Replace("'", "''") + "','002')";
                    SqlCommand cmddlerr = new SqlCommand(sqlStatementderr, con);
                    cmddlerr.ExecuteNonQuery();
                    errcon.Close();
                }


        }
        static void UpdateTable23() //**************ST_PAYMENTMETH******
        {
            //Console.WriteLine(reader.ReadToEnd());

            //StreamWriter oWriter = new StreamWriter(("D:\\webapp\\scoring\\FTP\\IN\\MASTER\\TEXT.TXT"), false, Encoding.Default);
            using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["FTPIP"].ToString()))
                //{
                //    //**************ST_PAYMENTMETH******************
                try
                {

                    string[] importfiles = Directory.GetFiles(@"D:\webapp\scoring\FTP\IN\MASTER", "ZUTBLRPAYMETH.txt");
                    string sqlStatement =
                        "INSERT INTO dbo.ST_PAYMENTMETH(PAYMENT_CD,PAYMENT_NAME,PAYMENT_DETAIL,ACTIVE_FLAG) VALUES(@D1, @D2, @D3 , @D4)";
                    using (SqlCommand cmd = new SqlCommand(sqlStatement, con))
                    {
                        cmd.Parameters.Add("@D1", SqlDbType.VarChar, 1);
                        cmd.Parameters.Add("@D2", SqlDbType.VarChar, 200);
                        cmd.Parameters.Add("@D3", SqlDbType.VarChar, 200);
                        cmd.Parameters.Add("@D4", SqlDbType.Bit);

                        foreach (string importfile in importfiles)
                        {
                            con.Open();
                            string[] allLines = File.ReadAllLines(importfile, Encoding.Default);
                            if (allLines.Length > 1)
                            {
                                string sqlStatementdl = "delete from dbo.ST_PAYMENTMETH";
                                SqlCommand cmddl = new SqlCommand(sqlStatementdl, con);
                                cmddl.ExecuteNonQuery();
                            }
                            int ttl = 0;
                            for (int index = 0; index < allLines.Length; index++)
                            {
                                if (allLines[index].Length > 0)
                                {
                                    string[] items = allLines[index].Split(new char[] { ',' });
                                    cmd.Parameters["@D1"].Value = items[0];
                                    cmd.Parameters["@D2"].Value = items[1];
                                    cmd.Parameters["@D3"].Value = items[1];
                                    cmd.Parameters["@D4"].Value = 1;

                                    cmd.ExecuteNonQuery();
                                }
                                ttl++;
                            }
                            string sqlStatementdx = "insert into s_log_import values('ST_PAYMENTMETH',getdate(),getdate()," + ttl.ToString() + ",'DONE',NULL,'002')";
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
                    string sqlStatementderr = "insert into s_log_import values('ST_PAYMENTMETH',getdate(),getdate(),0,'FAIL','" + inExc.ToString().Replace("'", "''") + "','002')";
                    SqlCommand cmddlerr = new SqlCommand(sqlStatementderr, con);
                    cmddlerr.ExecuteNonQuery();
                    errcon.Close();
                }


        }
        static void UpdateTable24() //**************ST_OCCSCLS*******
        {
            //Console.WriteLine(reader.ReadToEnd());

            //StreamWriter oWriter = new StreamWriter(("D:\\webapp\\scoring\\FTP\\IN\\MASTER\\TEXT.TXT"), false, Encoding.Default);
            using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["FTPIP"].ToString()))
                //{
                //    //**************ST_OCCSCLS******************
                try
                {

                    string[] importfiles = Directory.GetFiles(@"D:\webapp\scoring\FTP\IN\MASTER", "ZUTBLSOC.txt");
                    string sqlStatement =
                        "INSERT INTO dbo.ST_OCCSCLS(OCCCLS_CD,OCCSCLS_CD,OCCSCLS_NAME,OCCSCLS_DETAIL,ACTIVE_FLAG) VALUES(@D1, @D2, @D3 , @D4 , @D5)";
                    using (SqlCommand cmd = new SqlCommand(sqlStatement, con))
                    {
                        cmd.Parameters.Add("@D1", SqlDbType.VarChar, 2);
                        cmd.Parameters.Add("@D2", SqlDbType.VarChar, 4);
                        cmd.Parameters.Add("@D3", SqlDbType.VarChar, 100);
                        cmd.Parameters.Add("@D4", SqlDbType.VarChar, 200);
                        cmd.Parameters.Add("@D5", SqlDbType.Bit);

                        foreach (string importfile in importfiles)
                        {
                            con.Open();
                            string[] allLines = File.ReadAllLines(importfile, Encoding.Default);
                            if (allLines.Length > 1)
                            {
                                string sqlStatementdl = "delete from dbo.ST_OCCSCLS";
                                SqlCommand cmddl = new SqlCommand(sqlStatementdl, con);
                                cmddl.ExecuteNonQuery();
                            }
                            int ttl = 0;
                            for (int index = 0; index < allLines.Length; index++)
                            {
                                if (allLines[index].Length > 0)
                                {
                                    string[] items = allLines[index].Split(new char[] { ',' });
                                    cmd.Parameters["@D1"].Value = items[0];
                                    cmd.Parameters["@D2"].Value = items[1];
                                    cmd.Parameters["@D3"].Value = items[2];
                                    cmd.Parameters["@D4"].Value = items[3];
                                    cmd.Parameters["@D5"].Value = 1;

                                    cmd.ExecuteNonQuery();
                                }
                                ttl++;
                            }
                            string sqlStatementdx = "insert into s_log_import values('ST_OCCSCLS',getdate(),getdate()," + ttl.ToString() + ",'DONE',NULL,'002')";
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
                    string sqlStatementderr = "insert into s_log_import values('ST_OCCSCLS',getdate(),getdate(),0,'FAIL','" + inExc.ToString().Replace("'", "''") + "','002')";
                    SqlCommand cmddlerr = new SqlCommand(sqlStatementderr, con);
                    cmddlerr.ExecuteNonQuery();
                    errcon.Close();
                }


        }
        static void UpdateTable25() //**************ST_TITLE*******
        {
            //Console.WriteLine(reader.ReadToEnd());

            //StreamWriter oWriter = new StreamWriter(("D:\\webapp\\scoring\\FTP\\IN\\MASTER\\TEXT.TXT"), false, Encoding.Default);
            using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["FTPIP"].ToString()))
                //{
                //    //**************ST_TITLE******************
                try
                {

                    string[] importfiles = Directory.GetFiles(@"D:\webapp\scoring\FTP\IN\MASTER", "ZUTBLTITLE.txt");
                    string sqlStatement =
                        "INSERT INTO dbo.ST_TITLE(TITLE_CD, TITLE_LG, TITLEE_NAME,TITLE_DETAIL,ACTIVE_FLAG) VALUES(@D1, @D2, @D3 , @D4, @D5)";
                    using (SqlCommand cmd = new SqlCommand(sqlStatement, con))
                    {
                        cmd.Parameters.Add("@D1", SqlDbType.VarChar, 5);
                        cmd.Parameters.Add("@D2", SqlDbType.VarChar, 1);
                        cmd.Parameters.Add("@D3", SqlDbType.VarChar, 100);
                        cmd.Parameters.Add("@D4", SqlDbType.VarChar, 200);
                        cmd.Parameters.Add("@D5", SqlDbType.Bit);

                        foreach (string importfile in importfiles)
                        {
                            con.Open();
                            string[] allLines = File.ReadAllLines(importfile, Encoding.Default);
                            if (allLines.Length > 1)
                            {
                                string sqlStatementdl = "delete from dbo.ST_TITLE";
                                SqlCommand cmddl = new SqlCommand(sqlStatementdl, con);
                                cmddl.ExecuteNonQuery();
                            }
                            int ttl = 0;
                            for (int index = 0; index < allLines.Length; index++)
                            {
                                if (allLines[index].Length > 0)
                                {
                                    string[] items = allLines[index].Split(new char[] { ',' });
                                    cmd.Parameters["@D1"].Value = items[0];
                                    cmd.Parameters["@D2"].Value = items[1];
                                    cmd.Parameters["@D3"].Value = items[2];
                                    cmd.Parameters["@D4"].Value = items[3];
                                    cmd.Parameters["@D5"].Value = 1;

                                    cmd.ExecuteNonQuery();
                                }
                                ttl++;
                            }
                            string sqlStatementdx = "insert into s_log_import values('ST_TITLE',getdate(),getdate()," + ttl.ToString() + ",'DONE',NULL,'002')";
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
                    string sqlStatementderr = "insert into s_log_import values('ST_TITLE',getdate(),getdate(),0,'FAIL','" + inExc.ToString().Replace("'", "''") + "','002')";
                    SqlCommand cmddlerr = new SqlCommand(sqlStatementderr, con);
                    cmddlerr.ExecuteNonQuery();
                    errcon.Close();
                }


        }
        static void UpdateTable26() //**************ST_BRANCH ST_GSBZONE ST_GSBREGION******
        {
            //Console.WriteLine(reader.ReadToEnd());

            //StreamWriter oWriter = new StreamWriter(("D:\\webapp\\scoring\\FTP\\IN\\MASTER\\TEXT.TXT"), false, Encoding.Default);
            using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["FTPIP"].ToString()))
                //{
                //    //**************ST_BRANCH ST_GSBZONE ST_GSBREGION******************
                try
                {

                    string[] importfiles = Directory.GetFiles(@"D:\webapp\scoring\FTP\IN\MASTER", "UTBLBRCD.txt");
                    string sqlStatement =
                        "INSERT INTO dbo.TM_BRANCH(BRANCH_CD,BRANCH_NAME) VALUES(@D1, @D2)";
                    using (SqlCommand cmd = new SqlCommand(sqlStatement, con))
                    {
                        cmd.Parameters.Add("@D1", SqlDbType.VarChar, 6);
                        cmd.Parameters.Add("@D2", SqlDbType.VarChar, 100);
                        foreach (string importfile in importfiles)
                        {
                            con.Open();
                            string[] allLines = File.ReadAllLines(importfile, Encoding.Default);
                            if (allLines.Length > 1)
                            {
                                string sqlStatementdl = "delete from dbo.TM_BRANCH";
                                SqlCommand cmddl = new SqlCommand(sqlStatementdl, con);
                                cmddl.ExecuteNonQuery();
                            }
                            int ttl = 0;
                            for (int index = 0; index < allLines.Length; index++)
                            {
                                if (allLines[index].Length > 0)
                                {
                                    string[] items = allLines[index].Split(new char[] { ',' });
                                    cmd.Parameters["@D1"].Value = items[0];
                                    cmd.Parameters["@D2"].Value = items[1];
                                    cmd.ExecuteNonQuery();
                                }
                                ttl++;
                            }

                            string sqlStatementdx = "insert into s_log_import values('TM_BRANCH',getdate(),getdate()," + ttl.ToString() + ",'DONE',NULL,'002')";
                            SqlCommand cmddlx = new SqlCommand(sqlStatementdx, con);
                            cmddlx.ExecuteNonQuery();
                        }
                    }
                    string[] importfiles2 = Directory.GetFiles(@"D:\webapp\scoring\FTP\IN\MASTER", "ZUTBLREGREP.txt");
                    string sqlStatement2 =
                        "INSERT INTO dbo.TM_REGREP(BRANCH_CD,REGION_CD,ZONE_CD) VALUES(@D1, @D2, @D3)";
                    using (SqlCommand cmd2 = new SqlCommand(sqlStatement2, con))
                    {
                        cmd2.Parameters.Add("@D1", SqlDbType.VarChar, 6);
                        cmd2.Parameters.Add("@D2", SqlDbType.VarChar, 6);
                        cmd2.Parameters.Add("@D3", SqlDbType.VarChar, 6);
                        foreach (string importfile in importfiles2)
                        {
                            //con.Open();
                            string[] allLines2 = File.ReadAllLines(importfile, Encoding.Default);
                            if (allLines2.Length > 1)
                            {
                                string sqlStatementdl2 = "delete from dbo.TM_REGREP";
                                SqlCommand cmddl2 = new SqlCommand(sqlStatementdl2, con);
                                cmddl2.ExecuteNonQuery();
                            }
                            int ttl2 = 0;
                            for (int index = 0; index < allLines2.Length; index++)
                            {
                                if (allLines2[index].Length > 0)
                                {
                                    string[] items = allLines2[index].Split(new char[] { ',' });
                                    cmd2.Parameters["@D1"].Value = items[2];
                                    cmd2.Parameters["@D2"].Value = items[0];
                                    cmd2.Parameters["@D3"].Value = items[1];
                                    cmd2.ExecuteNonQuery();
                                }
                                ttl2++;
                            }
                            string sqlStatementdx2 = "insert into s_log_import values('TM_REGREP',getdate(),getdate()," + ttl2.ToString() + ",'DONE',NULL,'002')";
                            SqlCommand cmddlx2 = new SqlCommand(sqlStatementdx2, con);
                            cmddlx2.ExecuteNonQuery();


                        }

                    }
                    string sqlStatementdxg = "delete from st_branch";
                    SqlCommand cmddlxg = new SqlCommand(sqlStatementdxg, con);
                    cmddlxg.ExecuteNonQuery();

                    string sqlStatementdxA = "insert into st_branch SELECT [BRANCH_CD],[REGION_CD],[ZONE_CD],(select [BRANCH_NAME] from [TM_BRANCH] a where b.[BRANCH_CD]=a.[BRANCH_CD]) as [BRANCH_NAME] ,(select [BRANCH_NAME] from [TM_BRANCH] a where b.[BRANCH_CD]=a.[BRANCH_CD]) as [BRANCH_DETAIL],'1' as [ACTIVE_FLAG]      ,(select [BRANCH_NAME] from [TM_BRANCH] a where b.[REGION_CD]=a.[BRANCH_CD]) as [REGION],(select [BRANCH_NAME] from [TM_BRANCH] a where b.[ZONE_CD]=a.[BRANCH_CD]) as [ZONE] FROM [TM_regrep] b ";
                    SqlCommand cmddlxA = new SqlCommand(sqlStatementdxA, con);
                    cmddlxA.ExecuteNonQuery();

                    string sqlStatementdxB = "insert into s_log_import values('ST_BRANCH',getdate(),getdate(),(select count(1) from st_branch),'DONE',NULL,'002')";
                    SqlCommand cmddlxB = new SqlCommand(sqlStatementdxB, con);
                    cmddlxB.ExecuteNonQuery();

                    string sqlStatementdxH = "delete from st_gsbzone";
                    SqlCommand cmddlxH = new SqlCommand(sqlStatementdxH, con);
                    cmddlxH.ExecuteNonQuery();

                    string sqlStatementdxC = "insert into st_gsbzone SELECT distinct [ZONE_CD],(select [BRANCH_NAME] from [TM_BRANCH] a where b.[ZONE_CD]=a.[BRANCH_CD]) as [BRANCH_NAME] ,(select [BRANCH_NAME] from [TM_BRANCH] a where b.[ZONE_CD]=a.[BRANCH_CD]) as [BRANCH_DETAIL],'1' as [ACTIVE_FLAG] FROM [GSBBlaze].[dbo].[TM_regrep] b";
                    SqlCommand cmddlxC = new SqlCommand(sqlStatementdxC, con);
                    cmddlxC.ExecuteNonQuery();

                    string sqlStatementdxD = "insert into s_log_import values('ST_GSBZONE',getdate(),getdate(),(select count(1) from st_gsbzone),'DONE',NULL,'002')";
                    SqlCommand cmddlxD = new SqlCommand(sqlStatementdxD, con);
                    cmddlxD.ExecuteNonQuery();

                    string sqlStatementdxI = "delete from st_gsbregion";
                    SqlCommand cmddlxI = new SqlCommand(sqlStatementdxI, con);
                    cmddlxI.ExecuteNonQuery();

                    string sqlStatementdxE = "insert into st_gsbregion SELECT distinct [REGION_CD],(select [BRANCH_NAME] from [TM_BRANCH] a where b.[REGION_CD]=a.[BRANCH_CD]) as [BRANCH_NAME] ,(select [BRANCH_NAME] from [TM_BRANCH] a where b.[REGION_CD]=a.[BRANCH_CD]) as [BRANCH_DETAIL],'1' as [ACTIVE_FLAG] FROM [GSBBlaze].[dbo].[TM_regrep] b";
                    SqlCommand cmddlxE = new SqlCommand(sqlStatementdxE, con);
                    cmddlxE.ExecuteNonQuery();

                    string sqlStatementdxF = "insert into s_log_import values('ST_GSBREGION',getdate(),getdate(),(select count(1) from st_gsbregion),'DONE',NULL,'002')";
                    SqlCommand cmddlxF = new SqlCommand(sqlStatementdxF, con);
                    cmddlxF.ExecuteNonQuery();

                    con.Close();

                }
                catch (Exception ex)
                {
                    string inExc = ex.Message;
                    SqlConnection errcon = new SqlConnection(ConfigurationManager.AppSettings["FTPIP"].ToString());
                    errcon.Open();
                    string sqlStatementderr = "insert into s_log_import values('ST_BRANCH,REGION,ZONE',getdate(),getdate(),0,'FAIL','" + inExc.ToString().Replace("'", "''") + "','002')";
                    SqlCommand cmddlerr = new SqlCommand(sqlStatementderr, errcon);
                    cmddlerr.ExecuteNonQuery();
                    errcon.Close();
                }


        }
        static void UpdateTable27() //**
        {
            //Console.WriteLine(reader.ReadToEnd());

            //StreamWriter oWriter = new StreamWriter(("D:\\webapp\\scoring\\FTP\\IN\\MASTER\\TEXT.TXT"), false, Encoding.Default);
            using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["FTPIP"].ToString()))
                //{
                //    //**************TMP_AMPHUR******************
                try
                {

                    string[] importfiles = Directory.GetFiles(@"D:\webapp\scoring\FTP\IN\MASTER", "ZUTBLDIST.txt");
                    string sqlStatement =
                        "INSERT INTO TMP_AMPHUR2( [AMPHUR_CODE], [AMPHUR_NAME]) VALUES( @D6, @D3)";
                    using (SqlCommand cmd = new SqlCommand(sqlStatement, con))
                    {
                        //cmd.Parameters.Add("@D1", SqlDbType.Int);
                        //cmd.Parameters.Add("@D2", SqlDbType.VarChar, 4);
                        cmd.Parameters.Add("@D3", SqlDbType.VarChar, 150);
                        //cmd.Parameters.Add("@D4", SqlDbType.Int);
                        //cmd.Parameters.Add("@D5", SqlDbType.Int);
                        cmd.Parameters.Add("@D6", SqlDbType.VarChar, 4);

                        foreach (string importfile in importfiles)
                        {
                            con.Open();
                            string[] allLines = File.ReadAllLines(importfile, Encoding.Default);
                            if (allLines.Length > 1)
                            {
                                string sqlStatementdl = "delete from dbo.TMP_AMPHUR";
                                SqlCommand cmddl = new SqlCommand(sqlStatementdl, con);
                                cmddl.ExecuteNonQuery();
                                string sqlStatementd3 = "delete from dbo.TMP_AMPHUR2";
                                SqlCommand cmdd3 = new SqlCommand(sqlStatementd3, con);
                                cmdd3.ExecuteNonQuery();

                                string sqlStatementd2 = "delete from dbo.ST_AMPHUR";
                                SqlCommand cmdd2 = new SqlCommand(sqlStatementd2, con);
                                cmdd2.ExecuteNonQuery();
                            }
                            int ttl = 0;
                            for (int index = 0; index < allLines.Length; index++)
                            {
                                if (allLines[index].Length > 0)
                                {
                                    string[] items = allLines[index].Split(new char[] { ',' });
                                    //cmd.Parameters["@D1"].Value = ttl+1;
                                    //cmd.Parameters["@D2"].Value = items[0];
                                    cmd.Parameters["@D3"].Value = items[3];
                                    //cmd.Parameters["@D4"].Value = items[3];
                                    //cmd.Parameters["@D5"].Value = items[4];
                                    cmd.Parameters["@D6"].Value = items[1] + items[2];
                                    cmd.ExecuteNonQuery();
                                }
                                ttl++;
                            }
                            string sqlStatementdx = "insert into s_log_import values('ST_AMPHUR',getdate(),getdate()," + ttl.ToString() + ",'DONE',NULL,'002')";
                            SqlCommand cmddlx = new SqlCommand(sqlStatementdx, con);
                            cmddlx.ExecuteNonQuery();

                            string sqlStatementdx2 = "delete from TMP_AMPHUR2 where [AMPHUR_CODE] = '9999'";
                            SqlCommand cmddlx2 = new SqlCommand(sqlStatementdx2, con);
                            cmddlx2.ExecuteNonQuery();

                            string sqlStatementdx3 = "insert into TMP_AMPHUR([AMPHUR_CODE],[AMPHUR_NAME])select [AMPHUR_CODE],[AMPHUR_NAME] from TMP_AMPHUR2";
                            SqlCommand cmddlx3 = new SqlCommand(sqlStatementdx3, con);
                            cmddlx3.ExecuteNonQuery();

                            string sqlStatementdx4 = "insert into ST_AMPHUR([AMPHUR_ID],[AMPHUR_CODE],[AMPHUR_NAME]) select [AMPHUR_ID],[AMPHUR_CODE],[AMPHUR_NAME] from TMP_AMPHUR";
                            SqlCommand cmddlx4 = new SqlCommand(sqlStatementdx4, con);
                            cmddlx4.ExecuteNonQuery();

                            string sqlStatementdx5 = "update [ST_AMPHUR]set [ST_AMPHUR].PROVINCE_ID = ST_PROVINCE.PROVINCE_ID,[ST_AMPHUR].GEO_ID=ST_PROVINCE.GEO_ID from [ST_AMPHUR] inner join ST_PROVINCE on LEFT([ST_AMPHUR].AMPHUR_CODE,2)= ST_PROVINCE.PROVINCE_CODE";
                            SqlCommand cmddlx5 = new SqlCommand(sqlStatementdx5, con);
                            cmddlx5.ExecuteNonQuery();

                            con.Close();
                        }
                    }
                }
                catch (Exception ex)
                {
                    string inExc = ex.Message;
                    SqlConnection errcon = new SqlConnection(ConfigurationManager.AppSettings["FTPIP"].ToString());
                    errcon.Open();
                    string sqlStatementderr = "insert into s_log_import values('ST_AMPHUR',getdate(),getdate(),0,'FAIL','" + inExc.ToString().Replace("'", "''") + "','002')";
                    SqlCommand cmddlerr = new SqlCommand(sqlStatementderr, con);
                    cmddlerr.ExecuteNonQuery();
                    errcon.Close();
                }


        }
        //static void UpdateTable27() //**
        //{
        //    //Console.WriteLine(reader.ReadToEnd());

        //    //StreamWriter oWriter = new StreamWriter(("D:\\webapp\\scoring\\FTP\\IN\\MASTER\\TEXT.TXT"), false, Encoding.Default);
        //    using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["FTPIP"].ToString()))
        //        //{
        //        //    //**************ST_PURPOSE******************
        //        try
        //        {

        //            string[] importfiles = Directory.GetFiles(@"D:\webapp\scoring\FTP\IN\MASTER", "ZUTBLARRPUR.txt");
        //            string sqlStatement =
        //                "INSERT INTO dbo.ST_PURPOSE(PURPOSE_CD, GRP, PURPOSE_NAME,ACTIVE_FLAG) VALUES(@D1, @D2, @D3 , @D4)";
        //            using (SqlCommand cmd = new SqlCommand(sqlStatement, con))
        //            {
        //                cmd.Parameters.Add("@D1", SqlDbType.VarChar, 6);
        //                cmd.Parameters.Add("@D2", SqlDbType.VarChar, 3);
        //                cmd.Parameters.Add("@D3", SqlDbType.VarChar, 200);
        //                cmd.Parameters.Add("@D4", SqlDbType.Bit);

        //                foreach (string importfile in importfiles)
        //                {
        //                    con.Open();
        //                    string[] allLines = File.ReadAllLines(importfile, Encoding.Default);
        //                    if (allLines.Length > 1)
        //                    {
        //                        string sqlStatementdl = "delete from dbo.ST_PURPOSE";
        //                        SqlCommand cmddl = new SqlCommand(sqlStatementdl, con);
        //                        cmddl.ExecuteNonQuery();
        //                    }
        //                    int ttl = 0;
        //                    for (int index = 0; index < allLines.Length; index++)
        //                    {
        //                        if (allLines[index].Length > 0)
        //                        {
        //                            string[] items = allLines[index].Split(new char[] { ',' });
        //                            cmd.Parameters["@D1"].Value = items[1];
        //                            cmd.Parameters["@D2"].Value = items[0];
        //                            cmd.Parameters["@D3"].Value = items[2];
        //                            cmd.Parameters["@D4"].Value = 1;

        //                            cmd.ExecuteNonQuery();
        //                        }
        //                        ttl++;
        //                    }
        //                    string sqlStatementdx = "insert into s_log_import values('ST_PURPOSE',getdate(),getdate()," + ttl.ToString() + ",'DONE',NULL,'002')";
        //                    SqlCommand cmddlx = new SqlCommand(sqlStatementdx, con);
        //                    cmddlx.ExecuteNonQuery();
        //                    con.Close();
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            string inExc = ex.Message;
        //            SqlConnection errcon = new SqlConnection(ConfigurationManager.AppSettings["FTPIP"].ToString());
        //            errcon.Open();
        //            string sqlStatementderr = "insert into s_log_import values('ST_TITLE',getdate(),getdate(),0,'FAIL','" + inExc.ToString().Replace("'", "''") + "','002')";
        //            SqlCommand cmddlerr = new SqlCommand(sqlStatementderr, con);
        //            cmddlerr.ExecuteNonQuery();
        //            errcon.Close();
        //        }


        //}
        static void UpdateTable28() //**************TMP_LOANMKT*******
        {
            //Console.WriteLine(reader.ReadToEnd());

            //StreamWriter oWriter = new StreamWriter(("D:\\webapp\\scoring\\FTP\\IN\\MASTER\\TEXT.TXT"), false, Encoding.Default);
            using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["FTPIP"].ToString()))
                //{
                //    //**************TMP_LOANMKT******************
                try
                {

                    string[] importfiles = Directory.GetFiles(@"D:\webapp\scoring\FTP\IN\MASTER", "ZUTBLMKTCD.txt");
                    string sqlStatement =
                        "INSERT INTO dbo.TMP_LOANMKT(LOAN_CD,STYPE_CD,MARKET_CD,MARKET_NAME,MARKET_DETAIL,ACTIVE_FLAG) VALUES(@D1, @D2, @D3 , @D4, @D5 , @D6)";
                    using (SqlCommand cmd = new SqlCommand(sqlStatement, con))
                    {
                        cmd.Parameters.Add("@D1", SqlDbType.VarChar, 4);
                        cmd.Parameters.Add("@D2", SqlDbType.VarChar, 5);
                        cmd.Parameters.Add("@D3", SqlDbType.VarChar, 5);
                        cmd.Parameters.Add("@D4", SqlDbType.VarChar, 100);
                        cmd.Parameters.Add("@D5", SqlDbType.VarChar, 200);
                        cmd.Parameters.Add("@D6", SqlDbType.Bit);

                        foreach (string importfile in importfiles)
                        {
                            con.Open();
                            string[] allLines = File.ReadAllLines(importfile, Encoding.Default);
                            if (allLines.Length > 1)
                            {
                                string sqlStatementdl = "delete from dbo.TMP_LOANMKT";
                                SqlCommand cmddl = new SqlCommand(sqlStatementdl, con);
                                cmddl.ExecuteNonQuery();
                            }
                            int ttl = 0;
                            for (int index = 0; index < allLines.Length; index++)
                            {
                                if (allLines[index].Length > 0)
                                {
                                    string[] items = allLines[index].Split(new char[] { ',' });
                                    cmd.Parameters["@D1"].Value = items[0];
                                    cmd.Parameters["@D2"].Value = items[1];
                                    cmd.Parameters["@D3"].Value = items[2];
                                    cmd.Parameters["@D4"].Value = items[3];
                                    cmd.Parameters["@D5"].Value = items[3];
                                    cmd.Parameters["@D6"].Value = 1;

                                    cmd.ExecuteNonQuery();
                                }
                                ttl++;
                            }
                            string sqlStatementdx = "insert into s_log_import values('TMP_LOANMKT',getdate(),getdate()," + ttl.ToString() + ",'DONE',NULL,'002')";
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
                    string sqlStatementderr = "insert into s_log_import values('TMP_LOANMKT',getdate(),getdate(),0,'FAIL','" + inExc.ToString().Replace("'", "''") + "','002')";
                    SqlCommand cmddlerr = new SqlCommand(sqlStatementderr, con);
                    errcon.Open();
                    cmddlerr.ExecuteNonQuery();
                    errcon.Close();
                }


        }
        static void UpdateTable29()
        {
            //Console.WriteLine(reader.ReadToEnd());

            //StreamWriter oWriter = new StreamWriter(("D:\\webapp\\scoring\\FTP\\IN\\MASTER\\TEXT.TXT"), false, Encoding.Default);
            using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["FTPIP"].ToString()))
                //{
                //    //**************ST_PURPOSE******************
                try
                {

                    string[] importfiles = Directory.GetFiles(@"D:\webapp\scoring\FTP\IN\MASTER", "PRODCTL.txt");
                    string sqlStatement =
                        "INSERT INTO dbo.ST_LOANTYPE([LOAN_CD], [LOAN_NAME], [LOAN_DETAIL],ACTIVE_FLAG,[GRP]) VALUES(@D1, @D2, @D3 , @D4,@D5)";
                    using (SqlCommand cmd = new SqlCommand(sqlStatement, con))
                    {
                        cmd.Parameters.Add("@D1", SqlDbType.VarChar, 4);
                        cmd.Parameters.Add("@D2", SqlDbType.VarChar, 100);
                        cmd.Parameters.Add("@D3", SqlDbType.VarChar, 200);
                        cmd.Parameters.Add("@D4", SqlDbType.Bit);
                        cmd.Parameters.Add("@D5", SqlDbType.VarChar, 5);

                        foreach (string importfile in importfiles)
                        {
                            con.Open();
                            string[] allLines = File.ReadAllLines(importfile, Encoding.Default);
                            if (allLines.Length > 1)
                            {
                                string sqlStatementdl = "delete from dbo.ST_LOANTYPE";
                                SqlCommand cmddl = new SqlCommand(sqlStatementdl, con);
                                cmddl.ExecuteNonQuery();
                            }
                            int ttl = 0;
                            for (int index = 0; index < allLines.Length; index++)
                            {
                                if (allLines[index].Length > 0)
                                {
                                    string[] items = allLines[index].Split(new char[] { ',' });
                                    cmd.Parameters["@D1"].Value = items[2];
                                    cmd.Parameters["@D2"].Value = items[3];
                                    cmd.Parameters["@D3"].Value = items[3];
                                    cmd.Parameters["@D4"].Value = 1;
                                    cmd.Parameters["@D5"].Value = items[1];

                                    cmd.ExecuteNonQuery();
                                }
                                ttl++;
                            }
                            string sqlStatementdx = "insert into s_log_import values('ST_LOANTYPE',getdate(),getdate()," + ttl.ToString() + ",'DONE',NULL,'002')";
                            SqlCommand cmddlx = new SqlCommand(sqlStatementdx, con);
                            cmddlx.ExecuteNonQuery();
                            //con.Close();

                            string sqlStatementdxF = "Update [ST_LOANTYPE] set ACTIVE_FLAG = 0 where GRP not in ('LN','COM')";
                            SqlCommand cmddlxF = new SqlCommand(sqlStatementdxF, con);
                            cmddlxF.ExecuteNonQuery();
                            con.Close();
                        }
                    }
                }
                catch (Exception ex)
                {
                    string inExc = "Message";
                    SqlConnection errcon = new SqlConnection(ConfigurationManager.AppSettings["FTPIP"].ToString());
                    errcon.Open();
                    string sqlStatementderr = "insert into s_log_import values('ST_LOANTYPE',getdate(),getdate(),0,'FAIL','" + inExc.ToString().Replace("'", "''") + "','002')";
                    SqlCommand cmddlerr = new SqlCommand(sqlStatementderr, con);
                    cmddlerr.ExecuteNonQuery();
                    errcon.Close();

                    //string inExc = ex.Message;
                    //SqlConnection errcon = new SqlConnection(ConfigurationManager.AppSettings["FTPIP"].ToString());
                    //string sqlStatementderr = "insert into s_log_import values('ST_LOANTYPE',getdate(),getdate(),0,'FAIL','" + inExc.ToString().Replace("'", "''") + "','002')";
                    ////Console.WriteLine(sqlStatementderr);
                    //SqlCommand cmddlerr = new SqlCommand(sqlStatementderr, con);
                    //errcon.Open();
                    //cmddlerr.ExecuteNonQuery();
                    //errcon.Close();

                    //string inExc = ex.Message;
                    //SqlConnection errcon = new SqlConnection(ConfigurationManager.AppSettings["FTPIP"].ToString());
                    ////errcon.Open();
                    //string sqlStatementderr = "insert into s_log_import values('ST_LOANTYPE',getdate(),getdate(),0,'FAIL','" + inExc.ToString().Replace("'", "''") + "','002')";
                    //SqlCommand cmddlerr = new SqlCommand(sqlStatementderr, con);
                    //errcon.Open();
                    //cmddlerr.ExecuteNonQuery();
                    //errcon.Close();
                }


        }
        //static void UpdateTable29()
        //{
        //    //Console.WriteLine(reader.ReadToEnd());

        //    //StreamWriter oWriter = new StreamWriter(("D:\\webapp\\scoring\\FTP\\IN\\MASTER\\TEXT.TXT"), false, Encoding.Default);
        //    using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["FTPIP"].ToString()))
        //        //{
        //        //    //**************ST_PURPOSE******************
        //        try
        //        {

        //            string[] importfiles = Directory.GetFiles(@"D:\webapp\scoring\FTP\IN\MASTER", "ZUTBLARRPUR.txt");
        //            string sqlStatement =
        //                "INSERT INTO dbo.ST_PURPOSE(PURPOSE_CD, GRP, PURPOSE_NAME,ACTIVE_FLAG) VALUES(@D1, @D2, @D3 , @D4)";
        //            using (SqlCommand cmd = new SqlCommand(sqlStatement, con))
        //            {
        //                cmd.Parameters.Add("@D1", SqlDbType.VarChar, 6);
        //                cmd.Parameters.Add("@D2", SqlDbType.VarChar, 3);
        //                cmd.Parameters.Add("@D3", SqlDbType.VarChar, 200);
        //                cmd.Parameters.Add("@D4", SqlDbType.Bit);

        //                foreach (string importfile in importfiles)
        //                {
        //                    con.Open();
        //                    string[] allLines = File.ReadAllLines(importfile, Encoding.Default);
        //                    if (allLines.Length > 1)
        //                    {
        //                        string sqlStatementdl = "delete from dbo.ST_PURPOSE";
        //                        SqlCommand cmddl = new SqlCommand(sqlStatementdl, con);
        //                        cmddl.ExecuteNonQuery();
        //                    }
        //                    int ttl = 0;
        //                    for (int index = 0; index < allLines.Length; index++)
        //                    {
        //                        if (allLines[index].Length > 0)
        //                        {
        //                            string[] items = allLines[index].Split(new char[] { ',' });
        //                            cmd.Parameters["@D1"].Value = items[1];
        //                            cmd.Parameters["@D2"].Value = items[0];
        //                            cmd.Parameters["@D3"].Value = items[2];
        //                            cmd.Parameters["@D4"].Value = 1;

        //                            cmd.ExecuteNonQuery();
        //                        }
        //                        ttl++;
        //                    }
        //                    string sqlStatementdx = "insert into s_log_import values('ST_PURPOSE',getdate(),getdate()," + ttl.ToString() + ",'DONE',NULL,'002')";
        //                    SqlCommand cmddlx = new SqlCommand(sqlStatementdx, con);
        //                    cmddlx.ExecuteNonQuery();
        //                    con.Close();
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            string inExc = ex.Message;
        //            SqlConnection errcon = new SqlConnection(ConfigurationManager.AppSettings["FTPIP"].ToString());
        //            errcon.Open();
        //            string sqlStatementderr = "insert into s_log_import values('ST_TITLE',getdate(),getdate(),0,'FAIL','" + inExc.ToString().Replace("'", "''") + "','002')";
        //            SqlCommand cmddlerr = new SqlCommand(sqlStatementderr, con);
        //            cmddlerr.ExecuteNonQuery();
        //            errcon.Close();
        //        }


        //}
        static void UpdateTable30()
        {
            //Console.WriteLine(reader.ReadToEnd());

            //StreamWriter oWriter = new StreamWriter(("D:\\webapp\\scoring\\FTP\\IN\\MASTER\\TEXT.TXT"), false, Encoding.Default);
            using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["FTPIP"].ToString()))
                //{
                //    //**************ST_PROVINCE******************
                try
                {

                    string[] importfiles = Directory.GetFiles(@"D:\webapp\scoring\FTP\IN\MASTER", "STBLCNTRY1.txt");
                    string sqlStatement =
                        "INSERT INTO dbo.ST_PROVINCE([PROVINCE_ID],[PROVINCE_CODE],[PROVINCE_NAME],[GEO_ID]) VALUES(@D1, @D2, @D3 , @D4)";
                    using (SqlCommand cmd = new SqlCommand(sqlStatement, con))
                    {
                        cmd.Parameters.Add("@D1", SqlDbType.Int);
                        cmd.Parameters.Add("@D2", SqlDbType.VarChar, 50);
                        cmd.Parameters.Add("@D3", SqlDbType.VarChar, 150);
                        cmd.Parameters.Add("@D4", SqlDbType.Int);

                        foreach (string importfile in importfiles)
                        {
                            con.Open();
                            string[] allLines = File.ReadAllLines(importfile, Encoding.Default);
                            if (allLines.Length > 1)
                            {
                                string sqlStatementdl = "delete from dbo.ST_PROVINCE";
                                SqlCommand cmddl = new SqlCommand(sqlStatementdl, con);
                                cmddl.ExecuteNonQuery();
                            }
                            int ttl = 0;
                            for (int index = 0; index < allLines.Length; index++)
                            {
                                if (allLines[index].Length > 0)
                                {
                                    string[] items = allLines[index].Split(new char[] { ',' });
                                    cmd.Parameters["@D1"].Value = ttl + 1;
                                    cmd.Parameters["@D2"].Value = items[1];
                                    cmd.Parameters["@D3"].Value = items[2];
                                    cmd.Parameters["@D4"].Value = 1;

                                    cmd.ExecuteNonQuery();
                                }
                                ttl++;
                            }
                            string sqlStatementdx = "insert into s_log_import values('ST_PROVINCE',getdate(),getdate()," + ttl.ToString() + ",'DONE',NULL,'002')";
                            SqlCommand cmddlx = new SqlCommand(sqlStatementdx, con);
                            cmddlx.ExecuteNonQuery();
                            string sqlStatementdx2 = "delete FROM [ST_PROVINCE]where [PROVINCE_NAME]='อื่นๆ'";
                            SqlCommand cmddlx2 = new SqlCommand(sqlStatementdx2, con);
                            cmddlx2.ExecuteNonQuery();
                            con.Close();
                        }
                    }
                }
                catch (Exception ex)
                {
                    string inExc = ex.Message;
                    SqlConnection errcon = new SqlConnection(ConfigurationManager.AppSettings["FTPIP"].ToString());
                    errcon.Open();
                    string sqlStatementderr = "insert into s_log_import values('ST_PROVINCE',getdate(),getdate(),0,'FAIL','" + inExc.ToString().Replace("'", "''") + "','002')";
                    SqlCommand cmddlerr = new SqlCommand(sqlStatementderr, con);
                    cmddlerr.ExecuteNonQuery();


                    errcon.Close();



                }


        }
        //static void UpdateTable30()
        //{
        //    //Console.WriteLine(reader.ReadToEnd());

        //    //StreamWriter oWriter = new StreamWriter(("D:\\webapp\\scoring\\FTP\\IN\\MASTER\\TEXT.TXT"), false, Encoding.Default);
        //    using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["FTPIP"].ToString()))
        //        //{
        //        //    //**************ST_PURPOSE******************
        //        try
        //        {

        //            string[] importfiles = Directory.GetFiles(@"D:\webapp\scoring\FTP\IN\MASTER", "UTBLEDUC.txt");
        //            string sqlStatement =
        //                "INSERT INTO dbo.ST_EDUCATION(EDU_CD,EDU_NAME,EDU_DETAIL,ACTIVE_FLAG) VALUES(@D1, @D2, @D3 , @D4)";
        //            using (SqlCommand cmd = new SqlCommand(sqlStatement, con))
        //            {
        //                cmd.Parameters.Add("@D1", SqlDbType.VarChar, 4);
        //                cmd.Parameters.Add("@D2", SqlDbType.VarChar, 100);
        //                cmd.Parameters.Add("@D3", SqlDbType.VarChar, 200);
        //                cmd.Parameters.Add("@D4", SqlDbType.Bit);

        //                foreach (string importfile in importfiles)
        //                {
        //                    con.Open();
        //                    string[] allLines = File.ReadAllLines(importfile, Encoding.Default);
        //                    if (allLines.Length > 1)
        //                    {
        //                        string sqlStatementdl = "delete from dbo.ST_EDUCATION";
        //                        SqlCommand cmddl = new SqlCommand(sqlStatementdl, con);
        //                        cmddl.ExecuteNonQuery();
        //                    }
        //                    int ttl = 0;
        //                    for (int index = 0; index < allLines.Length; index++)
        //                    {
        //                        if (allLines[index].Length > 0)
        //                        {
        //                            string[] items = allLines[index].Split(new char[] { ',' });
        //                            cmd.Parameters["@D1"].Value = items[0];
        //                            cmd.Parameters["@D2"].Value = items[1];
        //                            cmd.Parameters["@D3"].Value = items[1];
        //                            cmd.Parameters["@D4"].Value = 1;

        //                            cmd.ExecuteNonQuery();
        //                        }
        //                        ttl++;
        //                    }
        //                    string sqlStatementdx = "insert into s_log_import values('ST_EDUCATION',getdate(),getdate()," + ttl.ToString() + ",'DONE',NULL,'002')";
        //                    SqlCommand cmddlx = new SqlCommand(sqlStatementdx, con);
        //                    cmddlx.ExecuteNonQuery();
        //                    con.Close();
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            string inExc = ex.Message;
        //            SqlConnection errcon = new SqlConnection(ConfigurationManager.AppSettings["FTPIP"].ToString());
        //            errcon.Open();
        //            string sqlStatementderr = "insert into s_log_import values('ST_TITLE',getdate(),getdate(),0,'FAIL','" + inExc.ToString().Replace("'", "''") + "','002')";
        //            SqlCommand cmddlerr = new SqlCommand(sqlStatementderr, con);
        //            cmddlerr.ExecuteNonQuery();
        //            errcon.Close();
        //        }


        //}
        static void UpdateTable31()
        {
            //Console.WriteLine(reader.ReadToEnd());
            //Table ST_MKTDFT//
            //StreamWriter oWriter = new StreamWriter(("D:\\webapp\\scoring\\FTP\\IN\\MASTER\\TEXT.TXT"), false, Encoding.Default);
            using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["FTPIP"].ToString()))
                //{
                //    //**************ST_PURPOSE******************
                try
                {

                    string[] importfiles = Directory.GetFiles(@"D:\webapp\scoring\FTP\IN\MASTER", "ZUTBLMKTDFT.txt");
                    string sqlStatement =
                        "INSERT INTO dbo.ST_MKTDFT(LoanType, SubType, MarketCode) VALUES(@D1, @D2, @D3)";
                    using (SqlCommand cmd = new SqlCommand(sqlStatement, con))
                    {
                        cmd.Parameters.Add("@D1", SqlDbType.VarChar, 50);
                        cmd.Parameters.Add("@D2", SqlDbType.VarChar, 50);
                        cmd.Parameters.Add("@D3", SqlDbType.VarChar, 50);
                        //cmd.Parameters.Add("@D4", SqlDbType.Bit);

                        foreach (string importfile in importfiles)
                        {
                            con.Open();
                            string[] allLines = File.ReadAllLines(importfile, Encoding.Default);
                            if (allLines.Length > 1)
                            {
                                string sqlStatementdl = "delete from dbo.ST_MKTDFT";
                                SqlCommand cmddl = new SqlCommand(sqlStatementdl, con);
                                cmddl.ExecuteNonQuery();
                            }
                            int ttl = 0;
                            for (int index = 0; index < allLines.Length; index++)
                            {
                                if (allLines[index].Length > 0)
                                {
                                    string[] items = allLines[index].Split(new char[] { ',' });
                                    cmd.Parameters["@D1"].Value = items[0];
                                    cmd.Parameters["@D2"].Value = items[1];
                                    cmd.Parameters["@D3"].Value = items[2];
                                    //cmd.Parameters["@D4"].Value = 1;

                                    cmd.ExecuteNonQuery();
                                }
                                ttl++;
                            }
                            string sqlStatementdx = "insert into s_log_import values('ST_MKTDFT',getdate(),getdate()," + ttl.ToString() + ",'DONE',NULL,'002')";
                            SqlCommand cmddlx = new SqlCommand(sqlStatementdx, con);
                            cmddlx.ExecuteNonQuery();
                            //string sqlStatementdx1 = "delete from TMP_MKTDFT";
                            //SqlCommand cmddlx1 = new SqlCommand(sqlStatementdx1, con);
                            //cmddlx1.ExecuteNonQuery();
                            //string sqlStatementdx2 = "insert into TMP_MKTDFT SELECT M.*,GRP FROM  ST_MKTDFT M INNER JOIN ST_LOANTYPE L ON M.LOANTYPE = L.LOAN_CD" ;
                            //SqlCommand cmddlx2 = new SqlCommand(sqlStatementdx2, con);
                            //cmddlx2.ExecuteNonQuery();
                            //string sqlStatementdx3 = "UPDATE ST_LOANMKT SET ST_LOANMKT.LOAN_CD = TMP_MKTDFT.LoanType from ST_LOANMKT inner join TMP_MKTDFT on TMP_MKTDFT.MarketCode = ST_LOANMKT.MARKET_CD and TMP_MKTDFT.SubType =ST_LOANMKT.STYPE_CD and TMP_MKTDFT.GRP =ST_LOANMKT.LOAN_CD";
                            //SqlCommand cmddlx3 = new SqlCommand(sqlStatementdx3, con);
                            //cmddlx3.ExecuteNonQuery();
                            //string sqlStatementdx4 = "delete from [ST_LOANMKT] where  isnumeric(LOAN_CD)<>1";
                            //SqlCommand cmddlx4 = new SqlCommand(sqlStatementdx4, con);
                            //cmddlx4.ExecuteNonQuery();


                            con.Close();
                        }
                    }
                }
                catch (Exception ex)
                {
                    string inExc = ex.Message;
                    SqlConnection errcon = new SqlConnection(ConfigurationManager.AppSettings["FTPIP"].ToString());
                    errcon.Open();
                    string sqlStatementderr = "insert into s_log_import values('ST_MKTDFT',getdate(),getdate(),0,'FAIL','" + inExc.ToString().Replace("'", "''") + "','002')";
                    SqlCommand cmddlerr = new SqlCommand(sqlStatementderr, con);
                    cmddlerr.ExecuteNonQuery();
                    errcon.Close();
                }


        }
        //static void UpdateTable31()
        //{
        //    //Console.WriteLine(reader.ReadToEnd());

        //    //StreamWriter oWriter = new StreamWriter(("D:\\webapp\\scoring\\FTP\\IN\\MASTER\\TEXT.TXT"), false, Encoding.Default);
        //    using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["FTPIP"].ToString()))
        //        //{
        //        //    //**************ST_PURPOSE******************
        //        try
        //        {

        //            string[] importfiles = Directory.GetFiles(@"D:\webapp\scoring\FTP\IN\MASTER", "ZUTBLARRPUR.txt");
        //            string sqlStatement =
        //                "INSERT INTO dbo.ST_PURPOSE(PURPOSE_CD, GRP, PURPOSE_NAME,ACTIVE_FLAG) VALUES(@D1, @D2, @D3 , @D4)";
        //            using (SqlCommand cmd = new SqlCommand(sqlStatement, con))
        //            {
        //                cmd.Parameters.Add("@D1", SqlDbType.VarChar, 6);
        //                cmd.Parameters.Add("@D2", SqlDbType.VarChar, 3);
        //                cmd.Parameters.Add("@D3", SqlDbType.VarChar, 200);
        //                cmd.Parameters.Add("@D4", SqlDbType.Bit);

        //                foreach (string importfile in importfiles)
        //                {
        //                    con.Open();
        //                    string[] allLines = File.ReadAllLines(importfile, Encoding.Default);
        //                    if (allLines.Length > 1)
        //                    {
        //                        string sqlStatementdl = "delete from dbo.ST_PURPOSE";
        //                        SqlCommand cmddl = new SqlCommand(sqlStatementdl, con);
        //                        cmddl.ExecuteNonQuery();
        //                    }
        //                    int ttl = 0;
        //                    for (int index = 0; index < allLines.Length; index++)
        //                    {
        //                        if (allLines[index].Length > 0)
        //                        {
        //                            string[] items = allLines[index].Split(new char[] { ',' });
        //                            cmd.Parameters["@D1"].Value = items[1];
        //                            cmd.Parameters["@D2"].Value = items[0];
        //                            cmd.Parameters["@D3"].Value = items[2];
        //                            cmd.Parameters["@D4"].Value = 1;

        //                            cmd.ExecuteNonQuery();
        //                        }
        //                        ttl++;
        //                    }
        //                    string sqlStatementdx = "insert into s_log_import values('ST_PURPOSE',getdate(),getdate()," + ttl.ToString() + ",'DONE',NULL,'002')";
        //                    SqlCommand cmddlx = new SqlCommand(sqlStatementdx, con);
        //                    cmddlx.ExecuteNonQuery();
        //                    con.Close();
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            string inExc = ex.Message;
        //            SqlConnection errcon = new SqlConnection(ConfigurationManager.AppSettings["FTPIP"].ToString());
        //            errcon.Open();
        //            string sqlStatementderr = "insert into s_log_import values('ST_TITLE',getdate(),getdate(),0,'FAIL','" + inExc.ToString().Replace("'", "''") + "','002')";
        //            SqlCommand cmddlerr = new SqlCommand(sqlStatementderr, con);
        //            cmddlerr.ExecuteNonQuery();
        //            errcon.Close();
        //        }


        //}
        /*static void UpdateTable32()
        {
            //Console.WriteLine(reader.ReadToEnd());

            //StreamWriter oWriter = new StreamWriter(("D:\\webapp\\scoring\\FTP\\IN\\MASTER\\TEXT.TXT"), false, Encoding.Default);
            using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["FTPIP"].ToString()))
                //{
                //    //**************ST_PURPOSE******************
                try
                {

                    string[] importfiles = Directory.GetFiles(@"D:\webapp\scoring\FTP\IN\MASTER", "UTBLEDUC.txt");
                    string sqlStatement =
                        "INSERT INTO dbo.ST_EDUCATION(EDU_CD,EDU_NAME,EDU_DETAIL,ACTIVE_FLAG) VALUES(@D1, @D2, @D3 , @D4)";
                    using (SqlCommand cmd = new SqlCommand(sqlStatement, con))
                    {
                        cmd.Parameters.Add("@D1", SqlDbType.VarChar, 4);
                        cmd.Parameters.Add("@D2", SqlDbType.VarChar, 100);
                        cmd.Parameters.Add("@D3", SqlDbType.VarChar, 200);
                        cmd.Parameters.Add("@D4", SqlDbType.Bit);

                        foreach (string importfile in importfiles)
                        {
                            con.Open();
                            string[] allLines = File.ReadAllLines(importfile, Encoding.Default);
                            if (allLines.Length > 1)
                            {
                                string sqlStatementdl = "delete from dbo.ST_EDUCATION";
                                SqlCommand cmddl = new SqlCommand(sqlStatementdl, con);
                                cmddl.ExecuteNonQuery();
                            }
                            int ttl = 0;
                            for (int index = 0; index < allLines.Length; index++)
                            {
                                if (allLines[index].Length > 0)
                                {
                                    string[] items = allLines[index].Split(new char[] { ',' });
                                    cmd.Parameters["@D1"].Value = items[0];
                                    cmd.Parameters["@D2"].Value = items[1];
                                    cmd.Parameters["@D3"].Value = items[1];
                                    cmd.Parameters["@D4"].Value = 1;

                                    cmd.ExecuteNonQuery();
                                }
                                ttl++;
                            }
                            string sqlStatementdx = "insert into s_log_import values('ST_EDUCATION',getdate(),getdate()," + ttl.ToString() + ",'DONE',NULL,'002')";
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
                    string sqlStatementderr = "insert into s_log_import values('ST_TITLE',getdate(),getdate(),0,'FAIL','" + inExc.ToString().Replace("'", "''") + "','002')";
                    SqlCommand cmddlerr = new SqlCommand(sqlStatementderr, con);
                    cmddlerr.ExecuteNonQuery();
                    errcon.Close();
                }


        }*/
        static void UpdateTable33()
        {
            //Console.WriteLine(reader.ReadToEnd());

            //StreamWriter oWriter = new StreamWriter(("D:\\webapp\\scoring\\FTP\\IN\\MASTER\\TEXT.TXT"), false, Encoding.Default);
            using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["FTPIP"].ToString()))
                //{
                //    //**************ST_PURPOSE******************
                try
                {

                    string[] importfiles = Directory.GetFiles(@"D:\webapp\scoring\FTP\IN\MASTER", "ZUTBLARRPUR.txt");
                    string sqlStatement =
                        "INSERT INTO dbo.ST_PURPOSE(PURPOSE_CD, GRP, PURPOSE_NAME,ACTIVE_FLAG) VALUES(@D1, @D2, @D3 , @D4)";
                    using (SqlCommand cmd = new SqlCommand(sqlStatement, con))
                    {
                        cmd.Parameters.Add("@D1", SqlDbType.VarChar, 6);
                        cmd.Parameters.Add("@D2", SqlDbType.VarChar, 3);
                        cmd.Parameters.Add("@D3", SqlDbType.VarChar, 200);
                        cmd.Parameters.Add("@D4", SqlDbType.Bit);

                        foreach (string importfile in importfiles)
                        {
                            con.Open();
                            string[] allLines = File.ReadAllLines(importfile, Encoding.Default);
                            if (allLines.Length > 1)
                            {
                                string sqlStatementdl = "delete from dbo.ST_PURPOSE";
                                SqlCommand cmddl = new SqlCommand(sqlStatementdl, con);
                                cmddl.ExecuteNonQuery();
                            }
                            int ttl = 0;
                            for (int index = 0; index < allLines.Length; index++)
                            {
                                if (allLines[index].Length > 0)
                                {
                                    string[] items = allLines[index].Split(new char[] { ',' });
                                    cmd.Parameters["@D1"].Value = items[1];
                                    cmd.Parameters["@D2"].Value = items[0];
                                    cmd.Parameters["@D3"].Value = items[2];
                                    cmd.Parameters["@D4"].Value = 1;

                                    cmd.ExecuteNonQuery();
                                }
                                ttl++;
                            }
                            string sqlStatementdx = "insert into s_log_import values('ST_PURPOSE',getdate(),getdate()," + ttl.ToString() + ",'DONE',NULL,'002')";
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
                    string sqlStatementderr = "insert into s_log_import values('ST_TITLE',getdate(),getdate(),0,'FAIL','" + inExc.ToString().Replace("'", "''") + "','002')";
                    SqlCommand cmddlerr = new SqlCommand(sqlStatementderr, con);
                    cmddlerr.ExecuteNonQuery();
                    errcon.Close();
                }


        }
        static void UpdateTable34()
        {
            //Console.WriteLine(reader.ReadToEnd());

            //StreamWriter oWriter = new StreamWriter(("D:\\webapp\\scoring\\FTP\\IN\\MASTER\\TEXT.TXT"), false, Encoding.Default);
            using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["FTPIP"].ToString()))
                //{
                //    //**************ST_PURPOSE******************
                try
                {

                    string[] importfiles = Directory.GetFiles(@"D:\webapp\scoring\FTP\IN\MASTER", "UTBLEDUC.txt");
                    string sqlStatement =
                        "INSERT INTO dbo.ST_EDUCATION(EDU_CD,EDU_NAME,EDU_DETAIL,ACTIVE_FLAG) VALUES(@D1, @D2, @D3 , @D4)";
                    using (SqlCommand cmd = new SqlCommand(sqlStatement, con))
                    {
                        cmd.Parameters.Add("@D1", SqlDbType.VarChar, 4);
                        cmd.Parameters.Add("@D2", SqlDbType.VarChar, 100);
                        cmd.Parameters.Add("@D3", SqlDbType.VarChar, 200);
                        cmd.Parameters.Add("@D4", SqlDbType.Bit);

                        foreach (string importfile in importfiles)
                        {
                            con.Open();
                            string[] allLines = File.ReadAllLines(importfile, Encoding.Default);
                            if (allLines.Length > 1)
                            {
                                string sqlStatementdl = "delete from dbo.ST_EDUCATION";
                                SqlCommand cmddl = new SqlCommand(sqlStatementdl, con);
                                cmddl.ExecuteNonQuery();
                            }
                            int ttl = 0;
                            for (int index = 0; index < allLines.Length; index++)
                            {
                                if (allLines[index].Length > 0)
                                {
                                    string[] items = allLines[index].Split(new char[] { ',' });
                                    cmd.Parameters["@D1"].Value = items[0];
                                    cmd.Parameters["@D2"].Value = items[1];
                                    cmd.Parameters["@D3"].Value = items[1];
                                    cmd.Parameters["@D4"].Value = 1;

                                    cmd.ExecuteNonQuery();
                                }
                                ttl++;
                            }
                            string sqlStatementdx = "insert into s_log_import values('ST_EDUCATION',getdate(),getdate()," + ttl.ToString() + ",'DONE',NULL,'002')";
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
                    string sqlStatementderr = "insert into s_log_import values('ST_TITLE',getdate(),getdate(),0,'FAIL','" + inExc.ToString().Replace("'", "''") + "','002')";
                    SqlCommand cmddlerr = new SqlCommand(sqlStatementderr, con);
                    cmddlerr.ExecuteNonQuery();
                    errcon.Close();
                }


        }
        static void UpdateTable35()
        {
            //Console.WriteLine(reader.ReadToEnd());

            //StreamWriter oWriter = new StreamWriter(("D:\\webapp\\scoring\\FTP\\IN\\MASTER\\TEXT.TXT"), false, Encoding.Default);
            using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["FTPIP"].ToString()))
                //{
                //    //**************ST_PURPOSE******************
                try
                {

                    string[] importfiles = Directory.GetFiles(@"D:\webapp\scoring\FTP\IN\MASTER", "ZUTBLARRPUR.txt");
                    string sqlStatement =
                        "INSERT INTO dbo.ST_PURPOSE(PURPOSE_CD, GRP, PURPOSE_NAME,ACTIVE_FLAG) VALUES(@D1, @D2, @D3 , @D4)";
                    using (SqlCommand cmd = new SqlCommand(sqlStatement, con))
                    {
                        cmd.Parameters.Add("@D1", SqlDbType.VarChar, 6);
                        cmd.Parameters.Add("@D2", SqlDbType.VarChar, 3);
                        cmd.Parameters.Add("@D3", SqlDbType.VarChar, 200);
                        cmd.Parameters.Add("@D4", SqlDbType.Bit);

                        foreach (string importfile in importfiles)
                        {
                            con.Open();
                            string[] allLines = File.ReadAllLines(importfile, Encoding.Default);
                            if (allLines.Length > 1)
                            {
                                string sqlStatementdl = "delete from dbo.ST_PURPOSE";
                                SqlCommand cmddl = new SqlCommand(sqlStatementdl, con);
                                cmddl.ExecuteNonQuery();
                            }
                            int ttl = 0;
                            for (int index = 0; index < allLines.Length; index++)
                            {
                                if (allLines[index].Length > 0)
                                {
                                    string[] items = allLines[index].Split(new char[] { ',' });
                                    cmd.Parameters["@D1"].Value = items[1];
                                    cmd.Parameters["@D2"].Value = items[0];
                                    cmd.Parameters["@D3"].Value = items[2];
                                    cmd.Parameters["@D4"].Value = 1;

                                    cmd.ExecuteNonQuery();
                                }
                                ttl++;
                            }
                            string sqlStatementdx = "insert into s_log_import values('ST_PURPOSE',getdate(),getdate()," + ttl.ToString() + ",'DONE',NULL,'002')";
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
                    string sqlStatementderr = "insert into s_log_import values('ST_TITLE',getdate(),getdate(),0,'FAIL','" + inExc.ToString().Replace("'", "''") + "','002')";
                    SqlCommand cmddlerr = new SqlCommand(sqlStatementderr, con);
                    cmddlerr.ExecuteNonQuery();
                    errcon.Close();
                }


        }
        /*static void UpdateTable36()
        {
            //Console.WriteLine(reader.ReadToEnd());

            //StreamWriter oWriter = new StreamWriter(("D:\\webapp\\scoring\\FTP\\IN\\MASTER\\TEXT.TXT"), false, Encoding.Default);
            using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["FTPIP"].ToString()))
                //{
                //    //**************ST_PURPOSE******************
                try
                {

                    string[] importfiles = Directory.GetFiles(@"D:\webapp\scoring\FTP\IN\MASTER", "UTBLEDUC.txt");
                    string sqlStatement =
                        "INSERT INTO dbo.ST_EDUCATION(EDU_CD,EDU_NAME,EDU_DETAIL,ACTIVE_FLAG) VALUES(@D1, @D2, @D3 , @D4)";
                    using (SqlCommand cmd = new SqlCommand(sqlStatement, con))
                    {
                        cmd.Parameters.Add("@D1", SqlDbType.VarChar, 4);
                        cmd.Parameters.Add("@D2", SqlDbType.VarChar, 100);
                        cmd.Parameters.Add("@D3", SqlDbType.VarChar, 200);
                        cmd.Parameters.Add("@D4", SqlDbType.Bit);

                        foreach (string importfile in importfiles)
                        {
                            con.Open();
                            string[] allLines = File.ReadAllLines(importfile, Encoding.Default);
                            if (allLines.Length > 1)
                            {
                                string sqlStatementdl = "delete from dbo.ST_EDUCATION";
                                SqlCommand cmddl = new SqlCommand(sqlStatementdl, con);
                                cmddl.ExecuteNonQuery();
                            }
                            int ttl = 0;
                            for (int index = 0; index < allLines.Length; index++)
                            {
                                if (allLines[index].Length > 0)
                                {
                                    string[] items = allLines[index].Split(new char[] { ',' });
                                    cmd.Parameters["@D1"].Value = items[0];
                                    cmd.Parameters["@D2"].Value = items[1];
                                    cmd.Parameters["@D3"].Value = items[1];
                                    cmd.Parameters["@D4"].Value = 1;

                                    cmd.ExecuteNonQuery();
                                }
                                ttl++;
                            }
                            string sqlStatementdx = "insert into s_log_import values('ST_EDUCATION',getdate(),getdate()," + ttl.ToString() + ",'DONE',NULL,'002')";
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
                    string sqlStatementderr = "insert into s_log_import values('ST_TITLE',getdate(),getdate(),0,'FAIL','" + inExc.ToString().Replace("'", "''") + "','002')";
                    SqlCommand cmddlerr = new SqlCommand(sqlStatementderr, con);
                    cmddlerr.ExecuteNonQuery();
                    errcon.Close();
                }


        }*/
        static void UpdateTable37() //**************LOANSTYPE*******
        {
            //Console.WriteLine(reader.ReadToEnd());

            //StreamWriter oWriter = new StreamWriter(("D:\\webapp\\scoring\\FTP\\IN\\MASTER\\TEXT.TXT"), false, Encoding.Default);
            using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["FTPIP"].ToString()))
                //{
                //    //**************TMP_LOANSTYPE******************
                try
                {

                    string[] importfiles = Directory.GetFiles(@"D:\webapp\scoring\FTP\IN\MASTER", "UTBLSUBT.txt");
                    string sqlStatement =
                        "INSERT INTO dbo.TMP_LOANSTYPE(GRP,SubType,SubTypeName) VALUES(@D1,@D2,@D3)";
                    using (SqlCommand cmd = new SqlCommand(sqlStatement, con))
                    {
                        cmd.Parameters.Add("@D1", SqlDbType.VarChar, 4);
                        cmd.Parameters.Add("@D2", SqlDbType.VarChar, 5);
                        cmd.Parameters.Add("@D3", SqlDbType.VarChar, 200);


                        foreach (string importfile in importfiles)
                        {
                            con.Open();
                            string[] allLines = File.ReadAllLines(importfile, Encoding.Default);
                            if (allLines.Length > 1)
                            {
                                string sqlStatementdl = "delete from dbo.TMP_LOANSTYPE";
                                SqlCommand cmddl = new SqlCommand(sqlStatementdl, con);
                                cmddl.ExecuteNonQuery();
                            }
                            int ttl = 0;
                            for (int index = 0; index < allLines.Length; index++)
                            {
                                if (allLines[index].Length > 0)
                                {
                                    string[] items = allLines[index].Split(new char[] { ',' });
                                    cmd.Parameters["@D1"].Value = items[0];
                                    cmd.Parameters["@D2"].Value = items[1];
                                    cmd.Parameters["@D3"].Value = items[2];

                                    cmd.ExecuteNonQuery();
                                }
                                ttl++;
                            }
                            string sqlStatementdx = "insert into s_log_import values('TMP_LOANSTYPE',getdate(),getdate()," + ttl.ToString() + ",'DONE',NULL,'002')";
                            SqlCommand cmddlx = new SqlCommand(sqlStatementdx, con);
                            cmddlx.ExecuteNonQuery();

                            string sqlStatementdx4 = "delete from ST_LOANSTYPE";
                            SqlCommand cmddlx4 = new SqlCommand(sqlStatementdx4, con);
                            cmddlx4.ExecuteNonQuery();

                            string sqlStatementdx2 = "insert into ST_LOANSTYPE select distinct s.SubType,L.LOAN_CD,SubTypeName,SubTypeName,1 as ACTIVE_FLAG from ST_MKTDFT M left join ST_LOANTYPE L on M.LoanType = L.LOAN_CD left join TMP_LOANMKT K on (M.MarketCode = K.[MARKET_CD] and M.SubType = K.[STYPE_CD] and L.GRP = K.Loan_cd)left join TMP_LOANSTYPE S on  (M.SubType = s.SubType and L.GRP = s.GRP)";
                            SqlCommand cmddlx2 = new SqlCommand(sqlStatementdx2, con);
                            cmddlx2.ExecuteNonQuery();

                            string sqlStatementdx5 = "delete from ST_LOANMKT";
                            SqlCommand cmddlx5 = new SqlCommand(sqlStatementdx5, con);
                            cmddlx5.ExecuteNonQuery();

                            string sqlStatementdx3 = "insert into ST_LOANMKT select MarketCode,s.SubType,L.LOAN_CD,MARKET_NAME,MARKET_DETAIL,1 as ACTIVE_FLAG from ST_MKTDFT M left join ST_LOANTYPE L on M.LoanType = L.LOAN_CD left join TMP_LOANMKT K on (M.MarketCode = K.[MARKET_CD] and M.SubType = K.[STYPE_CD] and L.GRP = K.Loan_cd)left join TMP_LOANSTYPE S on  (M.SubType = s.SubType and L.GRP = s.GRP)";
                            SqlCommand cmddlx3 = new SqlCommand(sqlStatementdx3, con);
                            cmddlx3.ExecuteNonQuery();

                            con.Close();
                        }
                    }
                }
                catch (Exception ex)
                {
                    string inExc = ex.Message;
                    SqlConnection errcon = new SqlConnection(ConfigurationManager.AppSettings["FTPIP"].ToString());
                    string sqlStatementderr = "insert into s_log_import values('TMP_LOANSTYPE',getdate(),getdate(),0,'FAIL','" + inExc.ToString().Replace("'", "''") + "','002')";
                    SqlCommand cmddlerr = new SqlCommand(sqlStatementderr, con);
                    errcon.Open();
                    cmddlerr.ExecuteNonQuery();
                    errcon.Close();
                }


        }
        static void UpdateTable38() //**************STBLGRP*******
        {
            //Console.WriteLine(reader.ReadToEnd());

            //StreamWriter oWriter = new StreamWriter(("D:\\webapp\\scoring\\FTP\\IN\\MASTER\\TEXT.TXT"), false, Encoding.Default);
            using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["FTPIP"].ToString()))
                //{
                //    //**************ST_LOANGRP******************
                try
                {

                    string[] importfiles = Directory.GetFiles(@"D:\webapp\scoring\FTP\IN\MASTER", "STBLGRP.txt");
                    string sqlStatement =
                        "INSERT INTO dbo.ST_LOANGRP(CLS,GRP,DES,FMDESC) VALUES(@D1,@D2, @D3,@D4)";
                    using (SqlCommand cmd = new SqlCommand(sqlStatement, con))
                    {
                        cmd.Parameters.Add("@D1", SqlDbType.VarChar, 5);
                        cmd.Parameters.Add("@D2", SqlDbType.VarChar, 10);
                        cmd.Parameters.Add("@D3", SqlDbType.VarChar, 200);
                        cmd.Parameters.Add("@D4", SqlDbType.VarChar, 200);


                        foreach (string importfile in importfiles)
                        {
                            con.Open();
                            string[] allLines = File.ReadAllLines(importfile, Encoding.Default);
                            if (allLines.Length > 1)
                            {
                                string sqlStatementdl = "delete from dbo.ST_LOANGRP";
                                SqlCommand cmddl = new SqlCommand(sqlStatementdl, con);
                                cmddl.ExecuteNonQuery();
                            }
                            int ttl = 0;
                            for (int index = 0; index < allLines.Length; index++)
                            {
                                if (allLines[index].Length > 0)
                                {
                                    string[] items = allLines[index].Split(new char[] { ',' });
                                    cmd.Parameters["@D1"].Value = items[0];
                                    cmd.Parameters["@D2"].Value = items[1];
                                    cmd.Parameters["@D3"].Value = items[2];
                                    cmd.Parameters["@D4"].Value = items[2];

                                    cmd.ExecuteNonQuery();
                                }
                                ttl++;
                            }
                            string sqlStatementdx = "insert into s_log_import values('ST_LOANGRP',getdate(),getdate()," + ttl.ToString() + ",'DONE',NULL,'002')";
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
                    string sqlStatementderr = "insert into s_log_import values('ST_LOANGRP',getdate(),getdate(),0,'FAIL','" + inExc.ToString().Replace("'", "''") + "','002')";
                    SqlCommand cmddlerr = new SqlCommand(sqlStatementderr, con);
                    errcon.Open();
                    cmddlerr.ExecuteNonQuery();
                    errcon.Close();
                }


        }
        static void UpdateTable39() //**************ZUTBLLNAPREL*******
        {
            //Console.WriteLine(reader.ReadToEnd());

            //StreamWriter oWriter = new StreamWriter(("D:\\webapp\\scoring\\FTP\\IN\\MASTER\\TEXT.TXT"), false, Encoding.Default);
            using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["FTPIP"].ToString()))
                //{
                //    //**************ST_RELATION******************
                try
                {

                    string[] importfiles = Directory.GetFiles(@"D:\webapp\scoring\FTP\IN\MASTER", "ZUTBLLNAPREL.txt");
                    string sqlStatement =
                        "INSERT INTO dbo.ST_RELATION(RELATION_CD,RELATION_NAME,RELATION_DETAIL,ACTIVE_FLAG) VALUES(@D1, @D2, @D2, @D3)";
                    using (SqlCommand cmd = new SqlCommand(sqlStatement, con))
                    {
                        cmd.Parameters.Add("@D1", SqlDbType.VarChar, 2);
                        cmd.Parameters.Add("@D2", SqlDbType.VarChar, 100);
                        cmd.Parameters.Add("@D3", SqlDbType.Bit);

                        foreach (string importfile in importfiles)
                        {
                            con.Open();
                            string[] allLines = File.ReadAllLines(importfile, Encoding.Default);
                            if (allLines.Length > 1)
                            {
                                string sqlStatementdl = "delete from dbo.ST_RELATION";
                                SqlCommand cmddl = new SqlCommand(sqlStatementdl, con);
                                cmddl.ExecuteNonQuery();
                            }
                            int ttl = 0;
                            for (int index = 0; index < allLines.Length; index++)
                            {
                                if (allLines[index].Length > 0)
                                {
                                    string[] items = allLines[index].Split(new char[] { ',' });
                                    cmd.Parameters["@D1"].Value = items[0];
                                    cmd.Parameters["@D2"].Value = items[1];
                                    cmd.Parameters["@D3"].Value = 1;

                                    cmd.ExecuteNonQuery();
                                }
                                ttl++;
                            }
                            string sqlStatementdx = "insert into s_log_import values('ST_RELATION',getdate(),getdate()," + ttl.ToString() + ",'DONE',NULL,'002')";
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
                    string sqlStatementderr = "insert into s_log_import values('ST_RELATION',getdate(),getdate(),0,'FAIL','" + inExc.ToString().Replace("'", "''") + "','002')";
                    SqlCommand cmddlerr = new SqlCommand(sqlStatementderr, con);
                    errcon.Open();
                    cmddlerr.ExecuteNonQuery();
                    errcon.Close();
                }


        }
        static void UpdateTable40() //**
        {
            //Console.WriteLine(reader.ReadToEnd());

            //StreamWriter oWriter = new StreamWriter(("D:\\webapp\\scoring\\FTP\\IN\\MASTER\\TEXT.TXT"), false, Encoding.Default);
            using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["FTPIP"].ToString()))
                //{
                //    //**************ST_DISTRICT******************
                try
                {

                    string[] importfiles = Directory.GetFiles(@"D:\webapp\scoring\FTP\IN\MASTER", "ZUTBLSDISTCD.txt");
                    string sqlStatement =
                        "INSERT INTO dbo.TMP_DISTRICT2([DISTRICT_CD], [DISTRICT_NAME]) VALUES(@D1, @D2)";
                    using (SqlCommand cmd = new SqlCommand(sqlStatement, con))
                    {
                        cmd.Parameters.Add("@D1", SqlDbType.Int);
                        cmd.Parameters.Add("@D2", SqlDbType.VarChar, 150);

                        foreach (string importfile in importfiles)
                        {
                            con.Open();
                            string[] allLines = File.ReadAllLines(importfile, Encoding.Default);
                            if (allLines.Length > 1)
                            {
                                string sqlStatementdl = "delete from dbo.TMP_DISTRICT2";
                                SqlCommand cmddl = new SqlCommand(sqlStatementdl, con);
                                cmddl.ExecuteNonQuery();
                                string sqlStatementdl2 = "delete from dbo.TMP_DISTRICT";
                                SqlCommand cmddl2 = new SqlCommand(sqlStatementdl2, con);
                                cmddl2.ExecuteNonQuery();
                                string sqlStatementdl3 = "delete from dbo.ST_DISTRICT";
                                SqlCommand cmddl3 = new SqlCommand(sqlStatementdl3, con);
                                cmddl3.ExecuteNonQuery();
                            }
                            int ttl = 0;
                            for (int index = 0; index < allLines.Length; index++)
                            {
                                if (allLines[index].Length > 0)
                                {
                                    string[] items = allLines[index].Split(new char[] { ',' });
                                    cmd.Parameters["@D1"].Value = items[1] + items[2] + items[3];
                                    cmd.Parameters["@D2"].Value = items[4];

                                    cmd.ExecuteNonQuery();
                                }
                                ttl++;
                            }
                            string sqlStatementdx = "insert into s_log_import values('DISTRICT',getdate(),getdate()," + ttl.ToString() + ",'DONE',NULL,'002')";
                            SqlCommand cmddlx = new SqlCommand(sqlStatementdx, con);
                            cmddlx.ExecuteNonQuery();

                            string sqlStatementdx2 = "delete from TMP_DISTRICT2 where DISTRICT_CD='99999999' or DISTRICT_CD='*'";
                            SqlCommand cmddlx2 = new SqlCommand(sqlStatementdx2, con);
                            cmddlx2.ExecuteNonQuery();

                            string sqlStatementdx3 = "insert into TMP_DISTRICT(DISTRICT_CD,DISTRICT_NAME) select DISTRICT_CD,DISTRICT_NAME from TMP_DISTRICT2";
                            SqlCommand cmddlx3 = new SqlCommand(sqlStatementdx3, con);
                            cmddlx3.ExecuteNonQuery();

                            string sqlStatementdx4 = "insert into ST_DISTRICT(DISTRICT_ID,DISTRICT_CD,DISTRICT_NAME) select DISTRICT_ID,DISTRICT_CD,DISTRICT_NAME from TMP_DISTRICT";
                            SqlCommand cmddlx4 = new SqlCommand(sqlStatementdx4, con);
                            cmddlx4.ExecuteNonQuery();

                            string sqlStatementdx5 = " update ST_DISTRICT set ST_DISTRICT.PROVINCE_ID = [ST_AMPHUR].PROVINCE_ID,ST_DISTRICT.GEO_ID=[ST_AMPHUR].GEO_ID,ST_DISTRICT.AMPHUR_ID=[ST_AMPHUR].AMPHUR_ID  from ST_DISTRICT inner join [ST_AMPHUR] on LEFT(ST_DISTRICT.[DISTRICT_CD],4)= [ST_AMPHUR].AMPHUR_CODE";
                            SqlCommand cmddlx5 = new SqlCommand(sqlStatementdx5, con);
                            cmddlx5.ExecuteNonQuery();
                            con.Close();
                        }
                    }
                }
                catch (Exception ex)
                {
                    string inExc = ex.Message;
                    SqlConnection errcon = new SqlConnection(ConfigurationManager.AppSettings["FTPIP"].ToString());
                    errcon.Open();
                    string sqlStatementderr = "insert into s_log_import values('ST_DISTRICT',getdate(),getdate(),0,'FAIL','" + inExc.ToString().Replace("'", "''") + "','002')";
                    SqlCommand cmddlerr = new SqlCommand(sqlStatementderr, con);
                    cmddlerr.ExecuteNonQuery();
                    errcon.Close();
                }

        }
        protected void Button2_Click(object sender, EventArgs e)
        {
            string confirmValue = Request.Form["confirm_value"];
            if (confirmValue == "Yes")
            {
                Button1.Enabled = false;
                Button2.Enabled = false;
                Button3.Enabled = false;

                //Button1_Enabled();
                Session.Timeout = 20000;
                Server.ScriptTimeout = 36000;
                Label2.Visible = true;
                //string revendt = DateTime.Now.Year.ToString("0000") + DateTime.Now.AddDays(-1).ToString("MMdd");
                //string revendt = DateTime.Now.Year.ToString("0000") + DateTime.Now.ToString("MMdd");

                string DateNow = DateTime.Now.Year.ToString("0000") + DateTime.Now.ToString("MMdd");
                string DateFile = GetFileDate();
                if (DateNow.Substring(0, 6) == DateFile.Substring(0, 6))
                {
                    string revendt = DateFile;
                    GetSFTPfile("/FromCBS/UTBLBRCD_" + revendt + ".TXT", "D:\\webapp\\scoring\\FTP\\IN\\MASTER\\UTBLBRCD.txt");
                    GetSFTPfile("/FromCBS/ZUTBLMKTCD_" + revendt + ".TXT", "D:\\webapp\\scoring\\FTP\\IN\\MASTER\\ZUTBLMKTCD.txt");
                    GetSFTPfile("/FromCBS/PRODCTL_" + revendt + ".TXT", "D:\\webapp\\scoring\\FTP\\IN\\MASTER\\PRODCTL.txt");
                    GetSFTPfile("/FromCBS/ZUTBLMKTDFT_" + revendt + ".TXT", "D:\\webapp\\scoring\\FTP\\IN\\MASTER\\ZUTBLMKTDFT.txt");
                    GetSFTPfile("/FromCBS/UTBLSUBT_" + revendt + ".TXT", "D:\\webapp\\scoring\\FTP\\IN\\MASTER\\UTBLSUBT.txt");
                    GetSFTPfile("/FromCBS/ZUTBLREGREP_" + revendt + ".TXT", "D:\\webapp\\scoring\\FTP\\IN\\MASTER\\ZUTBLREGREP.txt");//ann
                    //ann GetSFTPfile("/FromCBS/ZUTBLDIST_" + revendt + ".TXT", "D:\\webapp\\scoring\\FTP\\IN\\MASTER\\ZUTBLDIST.txt");
                    //ann GetSFTPfile("/FromCBS/ZUTBLSDISTCD_" + revendt + ".TXT", "D:\\webapp\\scoring\\FTP\\IN\\MASTER\\ZUTBLSDISTCD.txt");
                    UpdateTable26();//UTBLBRCD,ZUTBLREGREP
                    UpdateTable28();//ZUTBLMKTCD
                    UpdateTable29();//PRODCTL
                    UpdateTable31();//ZUTBLMKTDFT
                    UpdateTable37();//UTBLSUBT
                    //UpdateTable27();//ZUTBLDIST
                    //UpdateTable40();//ZUTBLSDISTCD
                    Label2.Text = "ดำเนินการเสร็จสิ้น";
                }
                else
                {
                    using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["FTPIP"].ToString()))
                    {
                        SqlConnection errcon = new SqlConnection(ConfigurationManager.AppSettings["FTPIP"].ToString());

                        string sqlStatementderr26 = "insert into s_log_import values('TM_BRANCH',getdate(),getdate(),0,'FAIL','File Not Found','002')";
                        string sqlStatementderr28 = "insert into s_log_import values('TMP_LOANMKT',getdate(),getdate(),0,'FAIL','File Not Found','002')";
                        string sqlStatementderr29 = "insert into s_log_import values('ST_LOANTYPE',getdate(),getdate(),0,'FAIL','File Not Found','002')";
                        string sqlStatementderr31 = "insert into s_log_import values('ST_MKTDFT',getdate(),getdate(),0,'FAIL','File Not Found','002')";
                        string sqlStatementderr37 = "insert into s_log_import values('TMP_LOANSTYPE',getdate(),getdate(),0,'FAIL','File Not Found','002')";
                        
                        errcon.Open();
                        SqlCommand cmddlerr1 = new SqlCommand(sqlStatementderr26, errcon);
                        cmddlerr1.ExecuteNonQuery();
                        SqlCommand cmddlerr2 = new SqlCommand(sqlStatementderr28, errcon);
                        cmddlerr2.ExecuteNonQuery();
                        SqlCommand cmddlerr3 = new SqlCommand(sqlStatementderr29, errcon);
                        cmddlerr3.ExecuteNonQuery();
                        SqlCommand cmddlerr4 = new SqlCommand(sqlStatementderr31, errcon);
                        cmddlerr4.ExecuteNonQuery();
                        SqlCommand cmddlerr5 = new SqlCommand(sqlStatementderr37, errcon);
                        cmddlerr5.ExecuteNonQuery();
                        errcon.Close();
                    }
                }
            }
            else
            {
                // this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('You clicked NO!')", true);
                Response.Redirect("../LoanSystem/LoadMaster.aspx");
            }
        }

        protected void Button3_Click(object sender, EventArgs e)
        {

            string confirmValue = Request.Form["confirm_value"];
            if (confirmValue == "Yes")
            {

                Button1.Enabled = false;
                Button2.Enabled = false;
                Button3.Enabled = false;
                //Button1.setAttribute('disabled', true); 
                //Button1.Attributes.AddAttributes('disabled',true);
                Button1.Enabled = false;
                Button2.Enabled = false;
                Button3.Enabled = false;
                //Button1_Enabled();
                Session.Timeout = 20000;
                Server.ScriptTimeout = 36000;
                Label2.Visible = true;
                string revendt = DateTime.Now.Year.ToString("0000") + DateTime.Now.AddDays(-1).ToString("MMdd");

                GetSFTPfiles("/FromCBS/ST_CHARACTER" + ".xls", "D:\\webapp\\scoring\\FTP\\IN\\Static Table\\ST_CHARACTER" + ".xls");
                GetSFTPfiles("/FromCBS/ST_CHARBIN" + ".xls", "D:\\webapp\\scoring\\FTP\\IN\\Static Table\\ST_CHARBIN" + ".xls");
                GetSFTPfiles("/FromCBS/ST_SCORERANGE" + ".xls", "D:\\webapp\\scoring\\FTP\\IN\\Static Table\\ST_SCORERANGE" + ".xls");
                GetSFTPfiles("/FromCBS/ST_SCORERNGLST" + ".xls", "D:\\webapp\\scoring\\FTP\\IN\\Static Table\\ST_SCORERNGLST" + ".xls");

                //Add by ARSoft 2015/01/16
                GetSFTPfiles("/FromCBS/ST_SALECHANNEL" + ".xls", "D:\\webapp\\scoring\\FTP\\IN\\Static Table\\ST_SALECHANNEL" + ".xls");

                //===========================
                //GetSFTPfile("/CreditScoring/FromCBS/PRODCTL_" + revendt + ".TXT", "D:\\webapp\\scoring\\FTP\\IN\\MASTER\\PRODCTL.txt");
                //GetSFTPfile("/CreditScoring/FromCBS/UTBLBRCD_" + revendt + ".TXT", "D:\\webapp\\scoring\\FTP\\IN\\MASTER\\UTBLBRCD.txt");
                //GetSFTPfile("/CreditScoring/FromCBS/UTBLSUBT_" + revendt + ".TXT", "D:\\webapp\\scoring\\FTP\\IN\\MASTER\\UTBLSUBT.txt");
                //GetSFTPfile("/CreditScoring/FromCBS/ZUTBLMKTCD_" + revendt + ".TXT", "D:\\webapp\\scoring\\FTP\\IN\\MASTER\\ZUTBLMKTCD.txt");
                ////GetSFTPfile("/mt/ZUTBLMKTDFT.txt", "D:\\webapp\\scoring\\FTP\\IN\\MASTER\\ZUTBLMKTDFT.txt");
                //GetSFTPfile("/CreditScoring/FromCBS/ZUTBLREGREP_" + revendt + ".TXT", "D:\\webapp\\scoring\\FTP\\IN\\MASTER\\ZUTBLREGREP.txt");
                //GetSFTPfile("/FromDWH/ST_CHARACTER" + ".xls", "D:\\webapp\\scoring\\FTP\\IN\\Static Table\\ST_CHARACTER" + ".xls");
                //GetSFTPfile("/FromDWH/ST_CHARBIN" + ".xls", "D:\\webapp\\scoring\\FTP\\IN\\Static Table\\ST_CHARBIN" + ".xls");
                //GetSFTPfile("/FromDWH/ST_SCORERANGE" + ".xls", "D:\\webapp\\scoring\\FTP\\IN\\Static Table\\ST_SCORERANGE" + ".xls");
                //GetSFTPfile("/FromDWH/ST_SCORERNGLST" + ".xls", "D:\\webapp\\scoring\\FTP\\IN\\Static Table\\ST_SCORERNGLST" + ".xls");
                //UpdateTable26();
                ////UpdateTable27();
                //UpdateTable28();


                //------------PK : Import data ----------------//
                pkImportData pkImpData = new pkImportData();
                try
                {
                    //Import ST_CHARACTER
                    pkImpData.import_ST_CHARACTER();
                }
                catch { }
                try
                {
                    //Import ST_CHARBIN
                    pkImpData.import_ST_CHARBIN();
                }
                catch { }
                try
                {
                    //Import ST_SCORERANGE
                    pkImpData.import_ST_SCORERANGE();
                }
                catch { }
                try
                {
                    //Import ST_SCORERNGLST
                    pkImpData.import_ST_SCORERNGLST();
                }
                catch { }

                try
                {
                    //Import ST_SCORERNGLST
                    string fname = @"D:\\webapp\\scoring\\FTP\\IN\\Static Table\\ST_SALECHANNEL.xls";
                    //string fname = @"D:\Development\BOL\BOL-Documnet\ST_SALECHANNEL.xls";
                    pkImpData.import_ST_SALECHANNEL(fname);
                }
                catch { }

                //------------ PK : End of import data ---------//


                //Button1.Enabled = true;
                Label2.Text = "ดำเนินการ Import Static Table เสร็จสิ้น";
            }
            else
            {
                // this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('You clicked NO!')", true);
                Response.Redirect("../LoanSystem/LoadMaster.aspx");
            }
        }

        static void GetSFTPfiles(String Remotefile, String Localfile)
        {
            string tmestart = DateTime.Now.Year.ToString("0000") + DateTime.Now.ToString("-MM-dd HH:mm:ss.") + DateTime.Now.Millisecond.ToString();
            try
            {
                // Setup session options
                //(ConfigurationManager.AppSettings["FTPIP"].ToString()
                string protocaluse = (ConfigurationManager.AppSettings["FTPIP"].ToString());
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
                    string sqlStatementdx = "insert into s_log_import values('CR_BATCH_DWH',convert(datetime,'" + tmestart.ToString() + "',121),convert(datetime,'" + tmesend.ToString() + "',121),0,'DONE','" + Localfile.ToString() + "','003')";
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
                                // Console.WriteLine("File {0} exists, local backup {1} does not", remotePath, localPath);
                                download = true;
                            }
                            else
                            {
                                DateTime remoteWriteTime = session.GetFileInfo(remotePath).LastWriteTime;
                                DateTime localWriteTime = File.GetLastWriteTime(localPath);

                                if (remoteWriteTime > localWriteTime)
                                {
                                    //Console.WriteLine(
                                    //    "File {0} as well as local backup {1} exist, " +
                                    //    "but remote file is newer ({2}) than local backup ({3})",
                                    //    remotePath, localPath, remoteWriteTime, localWriteTime);
                                    download = true;
                                }
                                else
                                {
                                    //Console.WriteLine(
                                    //    "File {0} as well as local backup {1} exist, " +
                                    //    "but remote file is not newer ({2}) than local backup ({3})",
                                    //    remotePath, localPath, remoteWriteTime, localWriteTime);
                                    download = false;
                                }
                            }

                            if (download)
                            {
                                // Download the file and throw on any error
                                session.GetFiles(remotePath, localPath).Check();

                                //Console.WriteLine("Download to backup done.");
                                //    return "Done";
                            }
                        }
                        else
                        {
                            //Console.WriteLine("File {0} does not exist yet", remotePath);
                            //  return "False";
                        }
                    }
                }
                //   return "Done";
                //            return 0;
                SqlConnection cons = new SqlConnection(ConfigurationManager.AppSettings["FTPIP"].ToString());
                string tmesend2 = DateTime.Now.Year.ToString("0000") + DateTime.Now.ToString("-MM-dd HH:mm:ss.") + DateTime.Now.Millisecond.ToString();
                string sqlStatementdx2 = "insert into s_log_import values('Static_BATCH',convert(datetime,'" + tmestart.ToString() + "',121),convert(datetime,'" + tmesend2.ToString() + "',121),0,'DONE','" + Localfile.ToString() + "','003')";
                cons.Open();
                SqlCommand cmddlx2 = new SqlCommand(sqlStatementdx2, cons);
                cmddlx2.ExecuteNonQuery();
                cons.Close();

            }
            catch (Exception e)
            {
                //Console.WriteLine("Error: {0}", e);
                //          return 1;
                //     return "False";
                string inExc = e.Message;
                SqlConnection errcon = new SqlConnection(ConfigurationManager.AppSettings["FTPIP"].ToString());
                errcon.Open();
                string sqlStatementderr = "insert into s_log_import values('Static_BATCH',convert(datetime,'" + tmestart.ToString() + "',121),getdate(),0,'FAIL','" + inExc.ToString().Replace("'", "''") + "','003')";
                SqlCommand cmddlerr = new SqlCommand(sqlStatementderr, errcon);
                cmddlerr.ExecuteNonQuery();
                errcon.Close();
            }
        }

    }
}