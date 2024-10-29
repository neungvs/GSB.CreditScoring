using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WinSCP;
using System.Configuration;
using System.IO;
using System.Data.SqlClient;
using System.Xml;
using System.Data;
using Microsoft.VisualBasic;




namespace GSB.LoanSystem
{
    public partial class LoadAScoreMyMo : System.Web.UI.Page
    {
        SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.AppSettings["FTPIP"]);

        protected void Page_Load(object sender, EventArgs e)
        {
            //string selMNT = DateTime.Now.AddMonths(-1).Year.ToString("0000") + DateTime.Now.AddMonths(-1).ToString("00");
            //ddlMNT.Items.Insert(0, new ListItem(selMNT, selMNT));

            if (!Page.IsPostBack)
            {
                ddlMNT.Items.Insert(0, new ListItem(DateTime.Now.Year.ToString("0000") + DateTime.Now.Month.ToString("00") + DateTime.Now.Day.ToString("00"), DateTime.Now.Year.ToString("0000") + DateTime.Now.Month.ToString("00") + DateTime.Now.Day.ToString("00")));
                ddlMNT.Items.Insert(1, new ListItem(DateTime.Now.AddDays(-1).Year.ToString("0000") + DateTime.Now.AddDays(-1).Month.ToString("00") + DateTime.Now.AddDays(-1).Day.ToString("00"), DateTime.Now.AddDays(-1).Year.ToString("0000") + DateTime.Now.AddDays(-1).Month.ToString("00") + DateTime.Now.AddDays(-1).Day.ToString("00")));
                ddlMNT.Items.Insert(2, new ListItem(DateTime.Now.AddDays(-2).Year.ToString("0000") + DateTime.Now.AddDays(-2).Month.ToString("00") + DateTime.Now.AddDays(-2).Day.ToString("00"), DateTime.Now.AddDays(-2).Year.ToString("0000") + DateTime.Now.AddDays(-2).Month.ToString("00") + DateTime.Now.AddDays(-2).Day.ToString("00")));
                ddlMNT.Items.Insert(3, new ListItem(DateTime.Now.AddDays(-3).Year.ToString("0000") + DateTime.Now.AddDays(-3).Month.ToString("00") + DateTime.Now.AddDays(-3).Day.ToString("00"), DateTime.Now.AddDays(-3).Year.ToString("0000") + DateTime.Now.AddDays(-3).Month.ToString("00") + DateTime.Now.AddDays(-3).Day.ToString("00")));
                ddlMNT.Items.Insert(4, new ListItem(DateTime.Now.AddDays(-4).Year.ToString("0000") + DateTime.Now.AddDays(-4).Month.ToString("00") + DateTime.Now.AddDays(-4).Day.ToString("00"), DateTime.Now.AddDays(-4).Year.ToString("0000") + DateTime.Now.AddDays(-4).Month.ToString("00") + DateTime.Now.AddDays(-4).Day.ToString("00")));
                ddlMNT.Items.Insert(5, new ListItem(DateTime.Now.AddDays(-5).Year.ToString("0000") + DateTime.Now.AddDays(-5).Month.ToString("00") + DateTime.Now.AddDays(-5).Day.ToString("00"), DateTime.Now.AddDays(-5).Year.ToString("0000") + DateTime.Now.AddDays(-5).Month.ToString("00") + DateTime.Now.AddDays(-5).Day.ToString("00")));
                ddlMNT.Items.Insert(6, new ListItem(DateTime.Now.AddDays(-6).Year.ToString("0000") + DateTime.Now.AddDays(-6).Month.ToString("00") + DateTime.Now.AddDays(-6).Day.ToString("00"), DateTime.Now.AddDays(-6).Year.ToString("0000") + DateTime.Now.AddDays(-6).Month.ToString("00") + DateTime.Now.AddDays(-6).Day.ToString("00")));
                ddlMNT.Items.Insert(7, new ListItem(DateTime.Now.AddDays(-7).Year.ToString("0000") + DateTime.Now.AddDays(-7).Month.ToString("00") + DateTime.Now.AddDays(-7).Day.ToString("00"), DateTime.Now.AddDays(-7).Year.ToString("0000") + DateTime.Now.AddDays(-7).Month.ToString("00") + DateTime.Now.AddDays(-7).Day.ToString("00")));
                ddlMNT.Items.Insert(8, new ListItem(DateTime.Now.AddDays(-8).Year.ToString("0000") + DateTime.Now.AddDays(-8).Month.ToString("00") + DateTime.Now.AddDays(-8).Day.ToString("00"), DateTime.Now.AddDays(-8).Year.ToString("0000") + DateTime.Now.AddDays(-8).Month.ToString("00") + DateTime.Now.AddDays(-8).Day.ToString("00")));
                ddlMNT.Items.Insert(9, new ListItem(DateTime.Now.AddDays(-9).Year.ToString("0000") + DateTime.Now.AddDays(-9).Month.ToString("00") + DateTime.Now.AddDays(-9).Day.ToString("00"), DateTime.Now.AddDays(-9).Year.ToString("0000") + DateTime.Now.AddDays(-9).Month.ToString("00") + DateTime.Now.AddDays(-9).Day.ToString("00")));
                ddlMNT.Items.Insert(10, new ListItem(DateTime.Now.AddDays(-10).Year.ToString("0000") + DateTime.Now.AddDays(-10).Month.ToString("00") + DateTime.Now.AddDays(-10).Day.ToString("00"), DateTime.Now.AddDays(-10).Year.ToString("0000") + DateTime.Now.AddDays(-10).Month.ToString("00") + DateTime.Now.AddDays(-10).Day.ToString("00")));
                ddlMNT.Items.Insert(11, new ListItem(DateTime.Now.AddDays(-11).Year.ToString("0000") + DateTime.Now.AddDays(-11).Month.ToString("00") + DateTime.Now.AddDays(-11).Day.ToString("00"), DateTime.Now.AddDays(-11).Year.ToString("0000") + DateTime.Now.AddDays(-11).Month.ToString("00") + DateTime.Now.AddDays(-11).Day.ToString("00")));
                ddlMNT.Items.Insert(12, new ListItem(DateTime.Now.AddDays(-12).Year.ToString("0000") + DateTime.Now.AddDays(-12).Month.ToString("00") + DateTime.Now.AddDays(-12).Day.ToString("00"), DateTime.Now.AddDays(-12).Year.ToString("0000") + DateTime.Now.AddDays(-12).Month.ToString("00") + DateTime.Now.AddDays(-12).Day.ToString("00")));
                ddlMNT.Items.Insert(13, new ListItem(DateTime.Now.AddDays(-13).Year.ToString("0000") + DateTime.Now.AddDays(-13).Month.ToString("00") + DateTime.Now.AddDays(-13).Day.ToString("00"), DateTime.Now.AddDays(-13).Year.ToString("0000") + DateTime.Now.AddDays(-13).Month.ToString("00") + DateTime.Now.AddDays(-13).Day.ToString("00")));
                ddlMNT.Items.Insert(14, new ListItem(DateTime.Now.AddDays(-14).Year.ToString("0000") + DateTime.Now.AddDays(-14).Month.ToString("00") + DateTime.Now.AddDays(-14).Day.ToString("00"), DateTime.Now.AddDays(-14).Year.ToString("0000") + DateTime.Now.AddDays(-14).Month.ToString("00") + DateTime.Now.AddDays(-14).Day.ToString("00")));
                ddlMNT.Items.Insert(15, new ListItem(DateTime.Now.AddDays(-15).Year.ToString("0000") + DateTime.Now.AddDays(-15).Month.ToString("00") + DateTime.Now.AddDays(-15).Day.ToString("00"), DateTime.Now.AddDays(-15).Year.ToString("0000") + DateTime.Now.AddDays(-15).Month.ToString("00") + DateTime.Now.AddDays(-15).Day.ToString("00")));
                ddlMNT.Items.Insert(16, new ListItem(DateTime.Now.AddDays(-16).Year.ToString("0000") + DateTime.Now.AddDays(-16).Month.ToString("00") + DateTime.Now.AddDays(-16).Day.ToString("00"), DateTime.Now.AddDays(-16).Year.ToString("0000") + DateTime.Now.AddDays(-16).Month.ToString("00") + DateTime.Now.AddDays(-16).Day.ToString("00")));
                ddlMNT.Items.Insert(17, new ListItem(DateTime.Now.AddDays(-17).Year.ToString("0000") + DateTime.Now.AddDays(-17).Month.ToString("00") + DateTime.Now.AddDays(-17).Day.ToString("00"), DateTime.Now.AddDays(-17).Year.ToString("0000") + DateTime.Now.AddDays(-17).Month.ToString("00") + DateTime.Now.AddDays(-17).Day.ToString("00")));
                ddlMNT.Items.Insert(18, new ListItem(DateTime.Now.AddDays(-18).Year.ToString("0000") + DateTime.Now.AddDays(-18).Month.ToString("00") + DateTime.Now.AddDays(-18).Day.ToString("00"), DateTime.Now.AddDays(-18).Year.ToString("0000") + DateTime.Now.AddDays(-18).Month.ToString("00") + DateTime.Now.AddDays(-18).Day.ToString("00")));
                ddlMNT.Items.Insert(19, new ListItem(DateTime.Now.AddDays(-19).Year.ToString("0000") + DateTime.Now.AddDays(-19).Month.ToString("00") + DateTime.Now.AddDays(-19).Day.ToString("00"), DateTime.Now.AddDays(-19).Year.ToString("0000") + DateTime.Now.AddDays(-19).Month.ToString("00") + DateTime.Now.AddDays(-19).Day.ToString("00")));
                ddlMNT.Items.Insert(20, new ListItem(DateTime.Now.AddDays(-20).Year.ToString("0000") + DateTime.Now.AddDays(-20).Month.ToString("00") + DateTime.Now.AddDays(-20).Day.ToString("00"), DateTime.Now.AddDays(-20).Year.ToString("0000") + DateTime.Now.AddDays(-20).Month.ToString("00") + DateTime.Now.AddDays(-20).Day.ToString("00")));
                ddlMNT.Items.Insert(21, new ListItem(DateTime.Now.AddDays(-21).Year.ToString("0000") + DateTime.Now.AddDays(-21).Month.ToString("00") + DateTime.Now.AddDays(-21).Day.ToString("00"), DateTime.Now.AddDays(-21).Year.ToString("0000") + DateTime.Now.AddDays(-21).Month.ToString("00") + DateTime.Now.AddDays(-21).Day.ToString("00")));
                ddlMNT.Items.Insert(22, new ListItem(DateTime.Now.AddDays(-22).Year.ToString("0000") + DateTime.Now.AddDays(-22).Month.ToString("00") + DateTime.Now.AddDays(-22).Day.ToString("00"), DateTime.Now.AddDays(-22).Year.ToString("0000") + DateTime.Now.AddDays(-22).Month.ToString("00") + DateTime.Now.AddDays(-22).Day.ToString("00")));
                ddlMNT.Items.Insert(23, new ListItem(DateTime.Now.AddDays(-23).Year.ToString("0000") + DateTime.Now.AddDays(-23).Month.ToString("00") + DateTime.Now.AddDays(-23).Day.ToString("00"), DateTime.Now.AddDays(-23).Year.ToString("0000") + DateTime.Now.AddDays(-23).Month.ToString("00") + DateTime.Now.AddDays(-23).Day.ToString("00")));
                ddlMNT.Items.Insert(24, new ListItem(DateTime.Now.AddDays(-24).Year.ToString("0000") + DateTime.Now.AddDays(-24).Month.ToString("00") + DateTime.Now.AddDays(-24).Day.ToString("00"), DateTime.Now.AddDays(-24).Year.ToString("0000") + DateTime.Now.AddDays(-24).Month.ToString("00") + DateTime.Now.AddDays(-24).Day.ToString("00")));
                ddlMNT.Items.Insert(25, new ListItem(DateTime.Now.AddDays(-25).Year.ToString("0000") + DateTime.Now.AddDays(-25).Month.ToString("00") + DateTime.Now.AddDays(-25).Day.ToString("00"), DateTime.Now.AddDays(-25).Year.ToString("0000") + DateTime.Now.AddDays(-25).Month.ToString("00") + DateTime.Now.AddDays(-25).Day.ToString("00")));
                ddlMNT.Items.Insert(26, new ListItem(DateTime.Now.AddDays(-26).Year.ToString("0000") + DateTime.Now.AddDays(-26).Month.ToString("00") + DateTime.Now.AddDays(-26).Day.ToString("00"), DateTime.Now.AddDays(-26).Year.ToString("0000") + DateTime.Now.AddDays(-26).Month.ToString("00") + DateTime.Now.AddDays(-26).Day.ToString("00")));
                ddlMNT.Items.Insert(27, new ListItem(DateTime.Now.AddDays(-27).Year.ToString("0000") + DateTime.Now.AddDays(-27).Month.ToString("00") + DateTime.Now.AddDays(-27).Day.ToString("00"), DateTime.Now.AddDays(-27).Year.ToString("0000") + DateTime.Now.AddDays(-27).Month.ToString("00") + DateTime.Now.AddDays(-27).Day.ToString("00")));
                ddlMNT.Items.Insert(28, new ListItem(DateTime.Now.AddDays(-28).Year.ToString("0000") + DateTime.Now.AddDays(-28).Month.ToString("00") + DateTime.Now.AddDays(-28).Day.ToString("00"), DateTime.Now.AddDays(-28).Year.ToString("0000") + DateTime.Now.AddDays(-28).Month.ToString("00") + DateTime.Now.AddDays(-28).Day.ToString("00")));
                ddlMNT.Items.Insert(29, new ListItem(DateTime.Now.AddDays(-29).Year.ToString("0000") + DateTime.Now.AddDays(-29).Month.ToString("00") + DateTime.Now.AddDays(-29).Day.ToString("00"), DateTime.Now.AddDays(-29).Year.ToString("0000") + DateTime.Now.AddDays(-29).Month.ToString("00") + DateTime.Now.AddDays(-29).Day.ToString("00")));
                ddlMNT.Items.Insert(30, new ListItem(DateTime.Now.AddDays(-30).Year.ToString("0000") + DateTime.Now.AddDays(-30).Month.ToString("00") + DateTime.Now.AddDays(-30).Day.ToString("00"), DateTime.Now.AddDays(-30).Year.ToString("0000") + DateTime.Now.AddDays(-30).Month.ToString("00") + DateTime.Now.AddDays(-30).Day.ToString("00")));
                ddlMNT.Items.Insert(31, new ListItem(DateTime.Now.AddDays(-31).Year.ToString("0000") + DateTime.Now.AddDays(-31).Month.ToString("00") + DateTime.Now.AddDays(-31).Day.ToString("00"), DateTime.Now.AddDays(-31).Year.ToString("0000") + DateTime.Now.AddDays(-31).Month.ToString("00") + DateTime.Now.AddDays(-31).Day.ToString("00")));

                //ddlMNTMonthly.Items.Insert(0, new ListItem(DateTime.Now.AddMonths(-1).Year.ToString("0000") + DateTime.Now.AddMonths(-1).Month.ToString("00"), DateTime.Now.Year.ToString("0000") + DateTime.Now.Month.ToString("00")));
                //ddlMNTMonthly.Items.Insert(1, new ListItem(DateTime.Now.AddMonths(-2).Year.ToString("0000") + DateTime.Now.AddMonths(-2).Month.ToString("00"), DateTime.Now.AddMonths(-1).Year.ToString("0000") + DateTime.Now.AddMonths(-1).Month.ToString("00")));
                //ddlMNTMonthly.Items.Insert(2, new ListItem(DateTime.Now.AddMonths(-3).Year.ToString("0000") + DateTime.Now.AddMonths(-3).Month.ToString("00"), DateTime.Now.AddMonths(-2).Year.ToString("0000") + DateTime.Now.AddMonths(-2).Month.ToString("00")));
                //ddlMNTMonthly.Items.Insert(3, new ListItem(DateTime.Now.AddMonths(-4).Year.ToString("0000") + DateTime.Now.AddMonths(-4).Month.ToString("00"), DateTime.Now.AddMonths(-3).Year.ToString("0000") + DateTime.Now.AddMonths(-3).Month.ToString("00")));
                //ddlMNTMonthly.Items.Insert(4, new ListItem(DateTime.Now.AddMonths(-5).Year.ToString("0000") + DateTime.Now.AddMonths(-5).Month.ToString("00"), DateTime.Now.AddMonths(-4).Year.ToString("0000") + DateTime.Now.AddMonths(-4).Month.ToString("00")));
                //ddlMNTMonthly.Items.Insert(5, new ListItem(DateTime.Now.AddMonths(-6).Year.ToString("0000") + DateTime.Now.AddMonths(-6).Month.ToString("00"), DateTime.Now.AddMonths(-5).Year.ToString("0000") + DateTime.Now.AddMonths(-5).Month.ToString("00")));
                //ddlMNTMonthly.Items.Insert(6, new ListItem(DateTime.Now.AddMonths(-7).Year.ToString("0000") + DateTime.Now.AddMonths(-7).Month.ToString("00"), DateTime.Now.AddMonths(-6).Year.ToString("0000") + DateTime.Now.AddMonths(-6).Month.ToString("00")));
                //ddlMNTMonthly.Items.Insert(7, new ListItem(DateTime.Now.AddMonths(-8).Year.ToString("0000") + DateTime.Now.AddMonths(-8).Month.ToString("00"), DateTime.Now.AddMonths(-7).Year.ToString("0000") + DateTime.Now.AddMonths(-7).Month.ToString("00")));
                //ddlMNTMonthly.Items.Insert(8, new ListItem(DateTime.Now.AddMonths(-9).Year.ToString("0000") + DateTime.Now.AddMonths(-9).Month.ToString("00"), DateTime.Now.AddMonths(-8).Year.ToString("0000") + DateTime.Now.AddMonths(-8).Month.ToString("00")));
                //ddlMNTMonthly.Items.Insert(9, new ListItem(DateTime.Now.AddMonths(-10).Year.ToString("0000") + DateTime.Now.AddMonths(-10).Month.ToString("00"), DateTime.Now.AddMonths(-9).Year.ToString("0000") + DateTime.Now.AddMonths(-9).Month.ToString("00")));
                //ddlMNTMonthly.Items.Insert(10, new ListItem(DateTime.Now.AddMonths(-11).Year.ToString("0000") + DateTime.Now.AddMonths(-11).Month.ToString("00"), DateTime.Now.AddMonths(-10).Year.ToString("0000") + DateTime.Now.AddMonths(-10).Month.ToString("00")));
                //ddlMNTMonthly.Items.Insert(11, new ListItem(DateTime.Now.AddMonths(-12).Year.ToString("0000") + DateTime.Now.AddMonths(-12).Month.ToString("00"), DateTime.Now.AddMonths(-11).Year.ToString("0000") + DateTime.Now.AddMonths(-11).Month.ToString("00")));

            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            Button1.Enabled = false;
            Label2.Visible = true;
            ProcessWithFTP();
            Label2.Text = "ดำเนินการเสร็จสิ้น";
            Button1.Enabled = true;
        }

        private void ProcessWithFTP()
        {
            string xmlinStr = string.Empty;
            string xmloutStr = string.Empty;

            string tmestart = DateTime.Now.Year.ToString("0000") + DateTime.Now.ToString("-MM-dd HH:mm:ss.") + DateTime.Now.Millisecond.ToString();


            SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["FTPIP"].ToString());
            SqlCommand cmddlx;

            string d = ddlMNT.SelectedValue;


            string empty1 = string.Empty;
            StreamReader streamReader = new StreamReader(@"D:/Webapp/ScoringSME/Batch/xmlTemplateAScoreMyMo.xml");
            try
            {
                string str3 = "";
                while (true)
                {
                    string str4 = streamReader.ReadLine();
                    str3 = str4;
                    if (str4 == null)
                    {
                        break;
                    }
                    empty1 = string.Concat(empty1, str3, "\n");
                }
            }
            catch (Exception exception)
            {
            }

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(empty1);

            //DateTime dateTransaction = monthCalendar1.SelectionStart.AddDays(-1);

            //int trdd = dateTransaction.Day;
            //int trmm = dateTransaction.Month;
            //int tryyyy = dateTransaction.Year;

            //if (tryyyy > 2500) tryyyy = tryyyy - 543;


            //int dd = Convert.ToDateTime(d).Day;
            //int mm = Convert.ToDateTime(d).Month;
            //int yyyy = Convert.ToDateTime(d).Year;

            string strFilenameRequest = "AscoreRequest" + d;
            string strFilenameResult = "AscoreResult" + d;

            SessionOptions sessionOptions = new SessionOptions();
            sessionOptions.Protocol = Protocol.Sftp;
            sessionOptions.HostName = ConfigurationManager.AppSettings["ftphost"].ToString();
            sessionOptions.UserName = ConfigurationManager.AppSettings["ftpuser"].ToString();
            sessionOptions.Password = ConfigurationManager.AppSettings["ftppassword"].ToString();
            sessionOptions.SshHostKey = ConfigurationManager.AppSettings["ftphostkey"].ToString();
            sessionOptions.SshPrivateKey = ConfigurationManager.AppSettings["ftpprikey"].ToString();

            String remotePath = ConfigurationManager.AppSettings["ftpPathAScoreMymo"].ToString() + strFilenameRequest + ".txt";
            String localPath = ConfigurationManager.AppSettings["ftpPathLocalAScoreMyMo"].ToString() + strFilenameRequest + ".txt";
            String localPathOut = ConfigurationManager.AppSettings["ftpPathLocalOutAScoreMyMo"].ToString() + strFilenameResult + ".txt";
            String remotePathOut = ConfigurationManager.AppSettings["ftpPathAScoreMymo"].ToString() + strFilenameResult + ".txt";

            try
            {
                // FTP
                using (Session session = new Session())
                {
                    session.Open(sessionOptions);



                    session.GetFiles(remotePath, localPath).Check();

                    string[] allLines = File.ReadAllLines(localPath);

                    string fileName = localPathOut;

                    if (File.Exists(fileName))
                    {
                        File.Delete(fileName);
                    }

                    if (allLines.Length > 2)
                    {
                        using (StreamWriter sw = File.CreateText(fileName))
                        {
                            sw.WriteLine("Appno|Grade|GradeDescription|ModelVersion|Model|StatusCode|Message");



                            for (int index = 1; index <= (allLines.Length - 2); index++)
                            {
                                string[] items = allLines[index].Split(new char[] { '|' });

                                try
                                {

                                    ScoringApprovalData data = new ScoringApprovalData();


                                    data.Appno = items[0].Trim();
                                    data.Appdate = items[1].Trim();
                                    data.Lnacctype = items[2].Trim();
                                    data.Lntype = items[3].Trim();
                                    data.Lnstype = items[4].Trim();
                                    data.Mkcode = items[5].Trim();
                                    data.Lnppose = items[6].Trim();
                                    data.pcsumcd = items[7].Trim();
                                    data.Cifno = items[8].Trim();
                                    data.Cdob = items[9].Trim();
                                    data.Gender = items[10].Trim();
                                    data.Marital = items[11].Trim();
                                    data.Occcls_id = items[12].Trim();
                                    data.Occscls_id = items[13].Trim();
                                    data.Edustatus_id = items[14].Trim();
                                    data.Income = items[15].Trim();
                                    data.total_other_income = items[16].Trim();
                                    data.IncomefromBusiness = items[17].Trim();
                                    data.Child = items[18].Trim();
                                    data.Wchild = items[19].Trim();
                                    data.Mntexpense = items[20].Trim();
                                    data.Lbtexpense = items[21].Trim();
                                    data.Busexpense = items[22].Trim();
                                    data.Resstatus_id = items[23].Trim();
                                    data.Tworktime_month = items[24].Trim();
                                    data.pworktime_month = items[25].Trim();
                                    data.Paddrtime = items[26].Trim();
                                    data.Restype = items[27].Trim();
                                    data.payment_cd = items[28].Trim();
                                    data.loan_term = items[29].Trim();
                                    data.netincomefromsalay = items[30].Trim();
                                    data.total_other_income_of_evidence = items[31].Trim();
                                    data.proportion_shareholders = items[32].Trim();
                                    data.other_incomefrombusiness = items[33].Trim();
                                    data.salary = items[34].Trim();
                                    data.PNInc2TotRev = items[35].Trim();
                                    data.PExp2TotRev = items[36].Trim();
                                    data.bnchcd = items[37].Trim();
                                    data.propright = items[38].Trim();
                                    data.busstype = items[39].Trim();
                                    data.colltype = items[40].Trim();
                                    data.collstype = items[41].Trim();

                                    string[] errorcode = CheckValidateField(data).Split(new char[] { ':' });



                                    if (errorcode[0]  == "001")
                                    {
                                        sw.WriteLine(data.Appno + "|" + "" + "|" + "" + "|" + "" + "|" + "" + "|" + "001" + "|" + "Incomplete Data:"+ errorcode[1] + "");
                                    }
                                    else if (errorcode[0] == "002")
                                    {
                                        sw.WriteLine(data.Appno + "|" + "" + "|" + "" + "|" + "" + "|" + "" + "|" + "002" + "|" + "Incorrect Data Type:" + errorcode[1] + "");
                                    }
                                    else if (errorcode[0] == "000")
                                    {
                                        doc.GetElementsByTagName("appno")[0].InnerText = data.Appno;
                                        doc.GetElementsByTagName("appdate")[0].InnerText = data.Appdate;
                                        doc.GetElementsByTagName("lnacctype")[0].InnerText = data.Lnacctype;
                                        doc.GetElementsByTagName("lntype")[0].InnerText = data.Lntype;
                                        doc.GetElementsByTagName("lnstype")[0].InnerText = data.Lnstype;
                                        doc.GetElementsByTagName("mkcode")[0].InnerText = data.Mkcode;
                                        doc.GetElementsByTagName("lnppose")[0].InnerText = data.Lnppose;
                                        doc.GetElementsByTagName("pcsumcd")[0].InnerText = data.pcsumcd;
                                        doc.GetElementsByTagName("cifno")[0].InnerText = data.Cifno;
                                        doc.GetElementsByTagName("cdob")[0].InnerText = data.Cdob;
                                        doc.GetElementsByTagName("gender")[0].InnerText = data.Gender;
                                        doc.GetElementsByTagName("marital")[0].InnerText = data.Marital;
                                        doc.GetElementsByTagName("occcls_id")[0].InnerText = data.Occcls_id;
                                        doc.GetElementsByTagName("occscls_id")[0].InnerText = data.Occscls_id;
                                        doc.GetElementsByTagName("edustatus_id")[0].InnerText = data.Edustatus_id;
                                        doc.GetElementsByTagName("income")[0].InnerText = data.Income;
                                        doc.GetElementsByTagName("othincome")[0].InnerText = data.total_other_income;
                                        doc.GetElementsByTagName("incomefromBusiness")[0].InnerText = data.IncomefromBusiness;
                                        doc.GetElementsByTagName("child")[0].InnerText = data.Child;
                                        doc.GetElementsByTagName("wchild")[0].InnerText = data.Wchild;
                                        doc.GetElementsByTagName("mntexpense")[0].InnerText = data.Mntexpense;
                                        doc.GetElementsByTagName("lbtexpense")[0].InnerText = data.Lbtexpense;
                                        doc.GetElementsByTagName("busexpense")[0].InnerText = data.Busexpense;
                                        doc.GetElementsByTagName("resstatus_id")[0].InnerText = data.Resstatus_id;
                                        doc.GetElementsByTagName("tworktime_month")[0].InnerText = data.Tworktime_month;
                                        doc.GetElementsByTagName("pworktime_month")[0].InnerText = data.pworktime_month;
                                        doc.GetElementsByTagName("paddrtime")[0].InnerText = data.Paddrtime;
                                        doc.GetElementsByTagName("restype")[0].InnerText = data.Restype;
                                        doc.GetElementsByTagName("lnterm")[0].InnerText = data.loan_term;
                                        doc.GetElementsByTagName("netincomefromsalary")[0].InnerText = data.netincomefromsalay;
                                        doc.GetElementsByTagName("total_other_income_of_evidence")[0].InnerText = data.total_other_income_of_evidence;
                                        doc.GetElementsByTagName("proportion_shareholders")[0].InnerText = data.proportion_shareholders;
                                        doc.GetElementsByTagName("other_incomefromBusiness")[0].InnerText = data.other_incomefrombusiness;
                                        doc.GetElementsByTagName("salary")[0].InnerText = data.salary;
                                        doc.GetElementsByTagName("PNInc2TotRev")[0].InnerText = data.PNInc2TotRev;
                                        doc.GetElementsByTagName("PExp2TotRev")[0].InnerText = data.PExp2TotRev;
                                        doc.GetElementsByTagName("bnchcd")[0].InnerText = data.bnchcd;
                                        doc.GetElementsByTagName("propright")[0].InnerText = data.propright;
                                        doc.GetElementsByTagName("busstype")[0].InnerText = data.busstype;
                                        doc.GetElementsByTagName("colltype")[0].InnerText = data.colltype;
                                        doc.GetElementsByTagName("collstype")[0].InnerText = data.collstype;
                                        doc.GetElementsByTagName("paystatus")[0].InnerText = data.payment_cd;


                                        string strRequestBlaze = doc.InnerXml.ToString();

                                        RetailBlaze.RetailServiceImplService proxy = new RetailBlaze.RetailServiceImplService();

                                        string strOutput = proxy.getRetailScoringDecision(doc.InnerXml);
                                        XmlDocument docOut = new XmlDocument();
                                        docOut.LoadXml(strOutput);

                                        if (docOut.GetElementsByTagName("Message")[0].InnerText == "Failure")
                                        {
                                            sw.WriteLine(doc.GetElementsByTagName("appno")[0].InnerText + "|" + "Fail" + "|" + "" + "|" + "" + "|" + docOut.GetElementsByTagName("Model")[0].InnerText + "|" + docOut.GetElementsByTagName("code")[0].InnerText + "|" + docOut.GetElementsByTagName("description")[0].InnerText + "");
                                        }
                                        else if (docOut.GetElementsByTagName("Message")[0].InnerText == "Score not required")
                                        {
                                            sw.WriteLine(doc.GetElementsByTagName("appno")[0].InnerText + "|" + "" + "|" + "" + "|" + "" + "|" + "" + "|" + "" + "|" + "Score not Require");
                                        }
                                        else
                                        {
                                            sw.WriteLine(doc.GetElementsByTagName("appno")[0].InnerText + "|" + docOut.GetElementsByTagName("Grade")[0].InnerText + "|" + docOut.GetElementsByTagName("GradeDescription")[0].InnerText + "|" + docOut.GetElementsByTagName("ModelVersion")[0].InnerText + "|" + docOut.GetElementsByTagName("Model")[0].InnerText + "|" + docOut.GetElementsByTagName("StatusCode")[0].InnerText + "|" + docOut.GetElementsByTagName("Message")[0].InnerText + "");

                                            string strOutputXML = strOutput;
                                            string strRespondBlaze = ConvertOutputToResponseBlaze(strOutputXML);
                                            string strRespondWebservice = ConvertOutputToResponseWebservice(strOutputXML);

                                            SaveResponse_Retail(data.Appno, DateTime.Now, doc.InnerXml.ToString(), strRespondBlaze, strRespondWebservice, docOut.InnerXml.ToString());
                                        }


                                    }
                                }
                                catch (Exception ex)
                                {
                                    sw.WriteLine(doc.GetElementsByTagName("appno")[0].InnerText + "|" + "Error" + "|" + "" + "|" + "" + "|" + "" + "|" + "" + "|" + ex.Message + ": xmlin : " + xmlinStr + " : xmlout : " + xmloutStr);
                                }

                            }

                            sw.WriteLine("Total " + (allLines.Length - 2).ToString());
                        }
                    }
                    else
                    {
                        using (StreamWriter sw = File.CreateText(fileName))
                        {
                            sw.WriteLine("Appno|Grade|GradeDescription|ModelVersion|Model|StatusCode|Message");
                            sw.WriteLine("Total 0");
                        }
                    }

                    session.PutFiles(localPathOut, remotePathOut).Check();
                    string tmesend = DateTime.Now.Year.ToString("0000") + DateTime.Now.ToString("-MM-dd HH:mm:ss.") + DateTime.Now.Millisecond.ToString();
                    string sqlStatementdx = "insert into s_log_import values('BATCH_Digital_loan',convert(datetime,'" + tmestart.ToString() + "',121),convert(datetime,'" + tmesend.ToString() + "',121)," + (allLines.Length - 2).ToString() + ",'DONE','" + localPath.ToString() + "','007')";
                    con.Open();
                    cmddlx = new SqlCommand(sqlStatementdx, con);
                    cmddlx.ExecuteNonQuery();
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
                string sqlStatementderr = "insert into s_log_import values('BATCH_Digital_loan',convert(datetime,'" + tmestart.ToString() + "',121),getdate(),0,'FAIL','" + localPath.ToString() + ":" + inExc.ToString().Replace("'", "''") + "','007')";
                errcon.Open();
                SqlCommand cmddlerr = new SqlCommand(sqlStatementderr, errcon);
                cmddlerr.ExecuteNonQuery();
                errcon.Close();
            }



        }


        private string CheckValidateField(ScoringApprovalData data)
        {
            // "000" : Validatepass
            // "001" : Validatenull
            // "000" : ValidateFormat

            if (data.Appno == "")
            {
                return "001:Appno";
            }
            else if (data.Appdate == "")
            {
                return "001:Appdate";
            }
            else if (data.Lnacctype == "")
            {
                return "001:lnacctype";
            }
            else if (data.Lntype == "")
            {
                return "001:lntype";
            }
            else if (data.Lnstype == "")
            {
                return "001:lnstype";
            }
            else if (data.Mkcode == "")
            {
                return "001:mkcode";
            }
            else if (data.Lnppose == "")
            {
                return "001:Lnppose";
            }
            else if (data.pcsumcd == "")
            {
                return "001:pcsumcd";
            }
            else if (data.Cifno == "")
            {
                return "001:cifno";
            }
            else if (data.Cdob == "")
            {
                return "001:cdob";
            }
            else if (data.Gender == "")
            {
                return "001:gender";
            }
            else if (data.Marital == "")
            {
                return "001:marital";
            }
            else if (data.Occcls_id == "")
            {
                return "001:occcls_id";
            }
            else if (data.Occscls_id == "")
            {
                return "001:occscls_id";
            }
            else if (data.Edustatus_id == "")
            {
                return "001:edustatus_id";
            }
            else if (data.Edustatus_id == "")
            {
                return "001:edustatus_id";
            }
            else if (data.Income == "")
            {
                return "001:Income";
            }
            else if (data.IncomefromBusiness == "")
            {
                return "001:IncomefromBusiness";
            }
            else if (data.Child == "")
            {
                return "001:child";
            }
            else if (data.Wchild == "")
            {
                return "001:Wchild";
            }
            else if (data.Mntexpense == "")
            {
                return "001:Mntexpense";
            }
            else if (data.Lbtexpense == "")
            {
                return "001:Lbtexpense";
            }
            else if (data.Busexpense == "")
            {
                return "001:Busexpense";
            }
            else if (data.Resstatus_id == "")
            {
                return "001:Resstatus_id";
            }
            else if (data.Tworktime_month == "")
            {
                return "001:Tworktime_month";
            }
            else if (data.pworktime_month == "")
            {
                return "001:pworktime_month";
            }
            else if (data.Paddrtime == "")
            {
                return "001:Paddrtime";
            }
            else if (data.Restype == "")
            {
                return "001:Restype";
            }
            else if (data.payment_cd == "")
            {
                return "001:payment_cd";
            }
            else if (data.loan_term == "")
            {
                return "001:loan_term";
            }
            else if (data.netincomefromsalay == "")
            {
                return "001:netincomefromsalay";
            }
            else if (data.total_other_income_of_evidence == "")
            {
                return "001:total_other_income_of_evidence";
            }
            else if (data.proportion_shareholders == "")
            {
                return "001:proportion_shareholders";
            }
            else if (data.other_incomefrombusiness == "")
            {
                return "001:other_incomefrombusiness";
            }
            else if (data.salary == "")
            {
                return "001:salary";
            }
            else if (data.PNInc2TotRev == "")
            {
                return "001:PNInc2TotRev";
            }
            else if (data.PExp2TotRev == "")
            {
                return "001:PExp2TotRev";
            }
            else if (data.bnchcd == "")
            {
                return "001:bnchcd";
            }
            else if (data.propright == "")
            {
                return "001:propright";
            }
            else if (data.busstype == "")
            {
                return "001:busstype";
            }
            else if (data.colltype == "")
            {
                return "001:colltype";
            }
            else if (data.collstype == "")
            {
                return "001:collstype";
            }

            if (!IsValidDateFormat("yyyy-MM-dd", data.Cdob))
            {
                return "002:cdob";
            }
            else if (!IsValidDateFormat("yyyy-MM-dd", data.Appdate))
            {
                return "002:Appdate";
            }

            if (!Information.IsNumeric(data.Income))
            {
                return "002:income";
            }
            else if (!Information.IsNumeric(data.total_other_income))
            {
                return "002:total_other_income";
            }
            else if (!Information.IsNumeric(data.IncomefromBusiness))
            {
                return "002:IncomefromBusiness";
            }
            else if (!Information.IsNumeric(data.Child))
            {
                return "002:Child";
            }
            else if (!Information.IsNumeric(data.Wchild))
            {
                return "002:Wchild";
            }
            else if (!Information.IsNumeric(data.Mntexpense))
            {
                return "002:Mntexpense";
            }
            else if (!Information.IsNumeric(data.Lbtexpense))
            {
                return "002:Lbtexpense";
            }
            else if (!Information.IsNumeric(data.Busexpense))
            {
                return "002:Busexpense";
            }
            else if (!Information.IsNumeric(data.Tworktime_month))
            {
                return "002:Tworktime_month";
            }
            else if (!Information.IsNumeric(data.pworktime_month))
            {
                return "002:pworktime_month";
            }
            else if (!Information.IsNumeric(data.Paddrtime))
            {
                return "002:Paddrtime";
            }
            else if (!Information.IsNumeric(data.netincomefromsalay))
            {
                return "002:netincomefromsalay";
            }
            else if (!Information.IsNumeric(data.total_other_income_of_evidence))
            {
                return "002:total_other_income_of_evidence";
            }
            else if (!Information.IsNumeric(data.proportion_shareholders))
            {
                return "002:proportion_shareholders";
            }
            else if (!Information.IsNumeric(data.salary))
            {
                return "002:salary";
            }
            else if (!Information.IsNumeric(data.PNInc2TotRev))
            {
                return "002:PNInc2TotRev";
            }
            else if (!Information.IsNumeric(data.PExp2TotRev))
            {
                return "002:PExp2TotRev";
            }



            return "000:";
        }

        private void SaveResponse_Retail(string appno, DateTime transactionDate, string serviceRequest_Update, string blazeResponse, string serviceResponse, string serviceRequest)
        {
            try
            {
                SqlCommand cmd = new SqlCommand();

                if (sqlConnection.State == ConnectionState.Open)
                {
                    sqlConnection.Close();
                }
                sqlConnection.Open();

                // Query to insert data into the RetailDecisionReqRes table

                cmd.CommandText = "INSERT INTO RetailDecisionReqRes(appno,transactiondate,request,response_Blaze,response_WebService,response_LOR) Values (@appno,@transactionDate,@request,@response_Blaze,@response_WebService,@response_LOR)";

 
                cmd.Parameters.AddWithValue("@appno", appno);

                cmd.Parameters.AddWithValue("@transactiondate", DateTime.Now);

                cmd.Parameters.Add("@request", SqlDbType.Xml);
                cmd.Parameters["@request"].Value = serviceRequest_Update;

                cmd.Parameters.Add("@response_Blaze", SqlDbType.Xml);
                cmd.Parameters["@response_Blaze"].Value = blazeResponse;
                cmd.Parameters.Add("@response_WebService", SqlDbType.Xml);
                cmd.Parameters["@response_WebService"].Value = serviceResponse;
                cmd.Parameters.Add("@response_LOR", SqlDbType.Xml);
                cmd.Parameters["@response_LOR"].Value = serviceRequest;




                cmd.Connection = sqlConnection;
                cmd.ExecuteNonQuery();
            }
            catch (Exception saveResponseException)
            {
                throw saveResponseException;
            }
            finally
            {
                sqlConnection.Close();
            }
        }

        public string ConvertOutputToResponseBlaze(string output)
        {
            string serviceRes = null;
            string strserviceRes = null;
            try
            {
                XmlDocument xd = new XmlDocument();
                xd.LoadXml(output);
                XmlElement root = xd.DocumentElement;
                //creating new node "ServiceResponse"
                XmlElement serviceResponse = (XmlElement)xd.CreateNode(XmlNodeType.Element, "RetailScoringBlazeOutput", root.NamespaceURI);  //adding the node with name
                if (root.Name != "RetailScoringBlazeOutput")
                {
                    //creating new node "RetailScoringServiceRequest" node
                    XmlElement modRootEle = (XmlElement)xd.CreateNode(XmlNodeType.Element, "RetailScoringBlazeOutput", root.NamespaceURI);
                    modRootEle.InnerXml = root.OuterXml;
                    xd.ReplaceChild(modRootEle, root);
                }
                serviceRes = convertXmlToString(xd);
                strserviceRes = serviceRes.Replace("<ServiceResponse>", "");
                strserviceRes = strserviceRes.Replace("</ServiceResponse>", "");
                return strserviceRes;


            }
            catch (Exception ConvertOutputToResponseBlazeeException)
            {
                throw ConvertOutputToResponseBlazeeException;
            }

        }


        public string convertXmlToString(XmlDocument xmlDocument)
        {
            string ans = "";
            try
            {
                StringWriter stringWriter = new StringWriter();
                XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter);
                xmlDocument.WriteTo(xmlTextWriter);
                ans = stringWriter.ToString();
                return ans;
            }
            catch (Exception convertXmlToStringException)
            {
                throw convertXmlToStringException;
            }


        }

        public string ConvertOutputToResponseWebservice(string output)
        {
            string serviceRes = null;
            try
            {
                XmlDocument xd = new XmlDocument();
                xd.LoadXml(output);
                XmlElement root = xd.DocumentElement;
                //creating new node "ServiceResponse"
                XmlElement serviceResponse = (XmlElement)xd.CreateNode(XmlNodeType.Element, "ServiceResponse", root.NamespaceURI);  //adding the node with name
                XmlNodeList charNode = xd.GetElementsByTagName("Characteristics");
                if (charNode != null && charNode.Count > 0)
                {
                    for (int i = charNode.Count - 1; i >= 0; i--)
                    {
                        //root.RemoveChild(charNode[i]);
                        charNode[i].ParentNode.RemoveChild(charNode[i]);

                    }
                }


                serviceResponse.InnerXml = root.InnerXml;
                xd.ReplaceChild(serviceResponse, root);
                serviceRes = convertXmlToString(xd);
                return serviceRes;
            }
            catch (Exception serviceResponseException)
            {
                throw serviceResponseException;
            }
            finally
            {
            }



        }

        public bool IsValidDateFormat(string dateFormat, string cdob)
        {
            try
            {
                DateTime fromDateValue;
                var formats = new[] { dateFormat };
                if (DateTime.TryParseExact(cdob, formats, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out fromDateValue))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        public static bool hasSpecialChar(string input)
        {
            string specialChar = @"\|!#$%&/()=?»«@£§€{}.-;'<>_,";
            foreach (var item in specialChar)
            {
                if (input.Contains(item)) return true;
            }

            return false;
        }

        private bool CheckMartial(string strMartial)
        {
            try
            {
                SqlCommand cmd = new SqlCommand();
                DataTable table = new DataTable();

                if (sqlConnection.State == ConnectionState.Open)
                {
                    sqlConnection.Close();
                }
                sqlConnection.Open();

                // Query to insert data into the RetailDecisionReqRes table

                cmd.CommandText = "select * from [dbo].[ST_MARITAL] where [MARITAL_CD] = '" + strMartial + "'";
                cmd.Connection = sqlConnection;

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(table);
                adapter.Dispose();
                adapter = null;

                if (table.Rows.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }



            }
            catch (Exception saveResponseException)
            {
                throw saveResponseException;
            }
            finally
            {
                sqlConnection.Close();
            }
        }

        private bool CheckEducation(string strEducation)
        {
            try
            {
                SqlCommand cmd = new SqlCommand();
                DataTable table = new DataTable();

                if (sqlConnection.State == ConnectionState.Open)
                {
                    sqlConnection.Close();
                }
                sqlConnection.Open();

                // Query to insert data into the RetailDecisionReqRes table

                cmd.CommandText = "select * from [dbo].[ST_EDUCATION] where [EDU_CD] = '" + strEducation + "'";
                cmd.Connection = sqlConnection;

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(table);
                adapter.Dispose();
                adapter = null;

                if (table.Rows.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception saveResponseException)
            {
                throw saveResponseException;
            }
            finally
            {
                sqlConnection.Close();
            }

        }


        private bool CheckOcc(string Occcls_id)
        {
            try
            {
                SqlCommand cmd = new SqlCommand();
                DataTable table = new DataTable();

                if (sqlConnection.State == ConnectionState.Open)
                {
                    sqlConnection.Close();
                }
                sqlConnection.Open();

                cmd.CommandText = "select * from [dbo].[ST_OCCCLS] where [OCCCLS_CD] = '" + Occcls_id + "'";
                cmd.Connection = sqlConnection;

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(table);
                adapter.Dispose();
                adapter = null;

                if (table.Rows.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception saveResponseException)
            {
                throw saveResponseException;
            }
            finally
            {
                sqlConnection.Close();
            }

        }

        private bool CheckOccs(string Occcls_id, string Occscls_id)
        {
            try
            {
                SqlCommand cmd = new SqlCommand();
                DataTable table = new DataTable();

                if (sqlConnection.State == ConnectionState.Open)
                {
                    sqlConnection.Close();
                }
                sqlConnection.Open();

                // Query to insert data into the RetailDecisionReqRes table

                cmd.CommandText = "select * from [dbo].[ST_OCCSCLS] where [OCCSCLS_CD] = '" + Occscls_id + "' and [OCCCLS_CD] = '" + Occcls_id + "' ";
                cmd.Connection = sqlConnection;

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(table);
                adapter.Dispose();
                adapter = null;

                if (table.Rows.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception saveResponseException)
            {
                throw saveResponseException;
            }
            finally
            {
                sqlConnection.Close();
            }

        }

        private int CheckmaxSEQ()
        {
            try
            {
                SqlCommand cmd = new SqlCommand();
                DataTable table = new DataTable();

                if (sqlConnection.State == ConnectionState.Open)
                {
                    sqlConnection.Close();
                }
                sqlConnection.Open();

                // Query to insert data into the RetailDecisionReqRes table

                cmd.CommandText = "select max(APP_SEQ) as m from mymo_ln_app";
                cmd.Connection = sqlConnection;

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(table);
                adapter.Dispose();
                adapter = null;

                if (table.Rows.Count > 0)
                {
                    if (string.IsNullOrEmpty(table.Rows[0][0].ToString()))
                    {
                        return 0;
                    }
                    else
                    {
                        return Convert.ToInt16(table.Rows[0][0].ToString());
                    }

                }
                else
                {
                    return 0;
                }

            }
            catch (Exception saveResponseException)
            {
                throw saveResponseException;
            }
            finally
            {
                sqlConnection.Close();
            }

        }

        private void ProcessWithFTPMonthly()
        {

            string tmestart = DateTime.Now.Year.ToString("0000") + DateTime.Now.ToString("-MM-dd HH:mm:ss.") + DateTime.Now.Millisecond.ToString();

            SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["FTPIP"].ToString());
            SqlCommand cmddlx;

            string d = ddlMNT.SelectedValue;

            string strFilenameRequest = "MyMoMonthly_" + d.Substring(0, 4) + d.Substring(4, 2) +"01";


            SessionOptions sessionOptions = new SessionOptions();
            sessionOptions.Protocol = Protocol.Sftp;
            sessionOptions.HostName = ConfigurationManager.AppSettings["ftphost"].ToString();
            sessionOptions.UserName = ConfigurationManager.AppSettings["ftpuser"].ToString();
            sessionOptions.Password = ConfigurationManager.AppSettings["ftppassword"].ToString();
            sessionOptions.SshHostKey = ConfigurationManager.AppSettings["ftphostkey"].ToString();
            sessionOptions.SshPrivateKey = ConfigurationManager.AppSettings["ftpprikey"].ToString();

            String remotePath = ConfigurationManager.AppSettings["ftpPathMyMoMonthly"].ToString() + strFilenameRequest + ".txt";
            String localPath = ConfigurationManager.AppSettings["ftpPathLocalMyMoMonthly"].ToString() + strFilenameRequest + ".txt";

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
                        for (int index = 1; index <= (allLines.Length - 2); index++)
                        {
                            string[] items = allLines[index].Split(new char[] { '|' });

                            MyMoMonthly temp = new MyMoMonthly();

                            temp.CBS_APP_NO = items[0].Trim();
                            temp.CBS_CIFNO = items[2].Trim();
                            temp.CBS_TITLE_CD = items[3].Trim();
                            temp.CBS_FNAME = items[4].Trim();
                            temp.CBS_MNAME = items[5].Trim();
                            temp.CBS_SNAME = items[6].Trim();
                            temp.CBS_CTYPE_CD = items[7].Trim();
                            temp.CBS_CCODE = items[8].Trim();
                            temp.CBS_CITIZID = items[9].Trim();
                            temp.CBS_CDOB = items[10].Trim();
                            temp.CBS_GENDER_CD = items[11].Trim();
                            temp.CBS_MARITAL_CD = items[12].Trim();
                            temp.CBS_OCCCLS_CD = items[13].Trim();
                            temp.CBS_OCCSCLS_CD = items[14].Trim();
                            temp.CBS_EDU_CD = items[15].Trim();
                            temp.CBS_CSTATUS_CD = items[16].Trim();
                            temp.CBS_INCOME = items[17].Trim();
                            temp.CBS_OTHINCOME = items[18].Trim();
                            temp.CBS_CHILD = items[19].Trim();
                            temp.CBS_WCHILD = items[20].Trim();
                            temp.CBS_MNTEXPEN = items[21].Trim();
                            temp.CBS_LBTEXPEN = items[22].Trim();
                            temp.CBS_BUSEXPEN = items[23].Trim();
                            temp.CBS_RESSTATUS_CD = items[24].Trim();
                            temp.CBS_PWORKTIME = items[25].Trim();
                            temp.CBS_TWORKTIME = items[26].Trim();
                            temp.CBS_BUSSMONTH = items[27].Trim();
                            temp.CBS_BUSSYEAR = items[28].Trim();
                            temp.CBS_LADDL1 = items[29].Trim();
                            temp.CBS_LADDL2 = items[30].Trim();
                            temp.CBS_LADDL3 = items[31].Trim();
                            temp.CBS_LADDL4 = items[32].Trim();
                            temp.CBS_LDISTRICT_CD = items[33].Trim();
                            temp.CBS_LAMPHUR = items[34].Trim();
                            temp.CBS_LPROVINCE = items[35].Trim();
                            temp.CBS_LPOST = items[36].Trim();
                            temp.CBS_ISIC1 = items[37].Trim();
                            temp.CBS_ISIC2 = items[38].Trim();
                            temp.CBS_ISIC3 = items[39].Trim();
                            temp.CBS_CLIENTY = items[40].Trim();
                            temp.CBS_CLIENTM = items[41].Trim();
                            temp.CBS_PADDRTIME = items[42].Trim();
                            temp.CBS_RESTYPE_CD = items[43].Trim();
                            temp.CBS_PROP_RIGHT_CD = items[44].Trim();
                            temp.CBS_BUSSTYPE_CD = items[45].Trim();
                            temp.CBS_AUTO_STATUS = items[46].Trim();
                            temp.CBS_PROP_STATUS = items[47].Trim();
                            temp.NUM_CARD = items[48].Trim();
                            temp.CUST_TYPE = items[49].Trim();
                            temp.OUT_DEPT = items[50].Trim();
                            temp.BKLST_HIST = items[51].Trim();
                            temp.LITIG_STAT = items[52].Trim();
                            temp.MESS_CHENL = items[53].Trim();
                            temp.PASSID = items[54].Trim();
                            temp.COBORREL = items[55].Trim();
                            temp.GUARAREL = items[56].Trim();
                            temp.CBS_SALARY = items[57].Trim();
                            temp.CBS_NETINCOMESALARY = items[58].Trim();
                            temp.CBS_NETINCOMEBUSINESS = items[59].Trim();
                            temp.CBS_TOTALINCOMEBUSINESS = items[60].Trim();
                            temp.CBS_TOTALOTHERINCOME = items[61].Trim();
                            temp.CBS_TOTALOTHERINCOMEEVIDENCE = items[62].Trim();
                            temp.CBS_INTEREST = items[63].Trim();
                            temp.CBS_TAX = items[64].Trim();
                            temp.CBS_PROPORTIONSHAREHOLDERS = items[65].Trim();
                            temp.CBS_TOTALOUTSTANDINGOVERDUE = items[66].Trim();
                            temp.CBS_DEBT_NCB = items[67].Trim();
                            temp.CBS_TOTALLIABILITIES_NCB = items[68].Trim();
                            temp.CBS_COOPERATIVELOAN = items[69].Trim();
                            temp.CBS_BURDEN_DEBT = items[70].Trim();
                            temp.CBS_BURDEN_GSB = items[71].Trim();
                            temp.CBS_TOTALLIABILITIES_NOTNCB = items[72].Trim();
                            temp.CBS_DTI = items[73].Trim();
                            temp.CBS_DSCR = items[74].Trim();
                            temp.CBS_PNInc2TotRev = items[75].Trim();
                            temp.CBS_LoanCr = items[76].Trim();
                            temp.CBS_PExp2TotRev = items[77].Trim();
                            temp.CBS_APPROVE_CD = items[78].Trim();
                            temp.CBS_REASON_CD = items[79].Trim();
                            temp.CBS_APPROVE_COMMENT = items[80].Trim();
                            temp.APPRAMT = items[81].Trim();
                            temp.BRANCH_CD = items[82].Trim();
                            temp.ReqDate = items[83].Trim();
                            temp.TotalAcc = items[84].Trim();
                            temp.TotalLoanAcc_Other = items[85].Trim();
                            temp.TotalLoanAcc_GSB = items[86].Trim();
                            temp.TotalCreditCard = items[87].Trim();
                            temp.TotalCreditCard_Other = items[88].Trim();
                            temp.TotalCreditCard_GSB = items[89].Trim();
                            temp.LoanCreditLimit = items[90].Trim();
                            temp.LoanCreditLimit_Other = items[91].Trim();
                            temp.LoanCreditLimit_GSB = items[92].Trim();
                            temp.CreditCardLimit_Other = items[93].Trim();
                            temp.CreditCardLimit_GSB = items[94].Trim();
                            temp.TotalOutstanding = items[95].Trim();
                            temp.TotalOutstanding_GSB = items[96].Trim();
                            temp.TotalOutstanding_Other = items[97].Trim();
                            temp.CardOutstanding_Other = items[98].Trim();
                            temp.CardOutstanding_GSB = items[99].Trim();
                            temp.TotalOverDue = items[100].Trim();
                            temp.TotalDept_Home = items[101].Trim();
                            temp.LITTIG_STAT = items[102].Trim();
                            temp.DeptPaytoNCB = items[103].Trim();
                            temp.MiniPayCard = items[104].Trim();
                            temp.MiniPayCard_GSB = items[105].Trim();
                            temp.OverlimitInt = items[106].Trim();
                            temp.NCB_TotalDept = items[107].Trim();
                            temp.NCB_TotalDept_GSB = items[108].Trim();
                            temp.Num_HomeLoan = items[109].Trim();
                            temp.DataStaus = items[110].Trim();
                            temp.ReDept_His = items[111].Trim();
                            temp.Overlimit_6mt_His = items[112].Trim();
                            temp.TotalOutstanding_NCB = items[113].Trim();
                            temp.BureauScore = items[114].Trim();
                            temp.OverlimitPercent = items[115].Trim();
                            temp.Overlimit_6mt = items[116].Trim();
                            temp.Overlimit_12mt = items[117].Trim();
                            temp.TotalDeptCard_Other = items[118].Trim();
                            temp.TotalDeptCard_GSB = items[119].Trim();
                            temp.TotalDeptLoan_Other = items[120].Trim();
                            temp.TotalDeptLoan_GSB = items[121].Trim();
                            temp.NCBCheck_1mt = items[122].Trim();
                            temp.NCBCheck_3mt = items[123].Trim();
                            temp.Num_AccLatePay = items[124].Trim();
                            temp.Num_LatePay_6mt = items[125].Trim();
                            temp.Num_LatePay_12mt = items[126].Trim();
                            temp.CreditScore = items[127].Trim();
                            temp.Score_Reason1 = items[128].Trim();
                            temp.Score_Reason2 = items[129].Trim();


                            if (CheckExistAppNo(temp.CBS_APP_NO))
                            {
                                Update_MyMoAPPCIF(temp);
                                Update_MyMoLNAPP(temp);
                            }
                            else
                            {
                                Insert_MyMoAPPCIF(temp);
                                Insert_MyMoLNAPP(temp);
                            }


                            Insert_NCB(temp);



                        }
                    }

                    string tmesend = DateTime.Now.Year.ToString("0000") + DateTime.Now.ToString("-MM-dd HH:mm:ss.") + DateTime.Now.Millisecond.ToString();
                    string sqlStatementdx = "insert into s_log_import values('BATCH_MymoMonthly',convert(datetime,'" + tmestart.ToString() + "',121),convert(datetime,'" + tmesend.ToString() + "',121)," + (allLines.Length - 2).ToString() + ",'DONE','" + localPath.ToString() + "','006')";
                    con.Open();
                    cmddlx = new SqlCommand(sqlStatementdx, con);              
                    cmddlx.ExecuteNonQuery();
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
                string sqlStatementderr = "insert into s_log_import values('BATCH_MymoMonthly',convert(datetime,'" + tmestart.ToString() + "',121),getdate(),0,'FAIL','" + localPath.ToString() + ":" + inExc.ToString().Replace("'", "''") + "','006')";
                errcon.Open();
                SqlCommand cmddlerr = new SqlCommand(sqlStatementderr, errcon);
                cmddlerr.ExecuteNonQuery();
                errcon.Close();
            }



        }

        private bool CheckExistAppNo(string appno)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["FTPIP"].ToString());
            SqlCommand cmd;

            string sqlStatement = "Select * from MYMO_LN_APP where APP_NO = '" + appno + "'";

            cmd = new SqlCommand(sqlStatement, con);
            con.Open();
            var tbl = new DataTable();
            tbl.Load(cmd.ExecuteReader());

            if (tbl.Rows.Count == 0)
            {
                con.Close();
                return false; ;
            }


            con.Close();
            return true;
        }


        private void Update_MyMoAPPCIF(MyMoMonthly MyMoMonthly)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["FTPIP"].ToString());
            SqlCommand cmd;

            string sqlStatement =
            @"update MYMO_LN_APPCIF set      [CBS_CIFNO] = @D3
      ,[CBS_TITLE_CD] = @D4
      ,[CBS_FNAME] = @D5
      ,[CBS_MNAME] = @D6
      ,[CBS_SNAME] = @D7
      ,[CBS_CTYPE_CD] = @D8
      ,[CBS_CCODE] = @D9
      ,[CBS_CITIZID] = @D10
      ,[CBS_GENDER_CD] = @D12
      ,[CBS_CSTATUS_CD] = @D17
      ,[CBS_INCOME] = @D18
      ,[CBS_OTHINCOME] = @D19
      ,[CBS_CHILD] = @D20
      ,[CBS_WCHILD] = @D21
      ,[CBS_MNTEXPEN] = @D22
      ,[CBS_LBTEXPEN] = @D23
      ,[CBS_BUSEXPEN] = @D24
      ,[CBS_RESSTATUS_CD] = @D25
      ,[CBS_PWORKTIME] = @D26
      ,[CBS_BUSSMONTH] = @D28
      ,[CBS_BUSSYEAR] = @D29
      ,[CBS_LADDL1] = @D30
      ,[CBS_LADDL2] = @D31
      ,[CBS_LADDL3] = @D32
      ,[CBS_LADDL4] = @D33
      ,[CBS_LDISTRICT_CD] = @D34
      ,[CBS_LAMPHUR] = @D35
      ,[CBS_LPROVINCE] = @D36
      ,[CBS_LPOST] = @D37
      ,[CBS_ISIC1] = @D38
      ,[CBS_ISIC2] = @D39
      ,[CBS_ISIC3] = @D40
      ,[CBS_CLIENTY] = @D41
      ,[CBS_CLIENTM] = @D42
      ,[CBS_PADDRTIME] = @D43
      ,[CBS_RESTYPE_CD] = @D44
      ,[CBS_PROP_RIGHT_CD] = @D45
      ,[CBS_BUSSTYPE_CD] = @D46
      ,[CBS_AUTO_STATUS] = @D47
      ,[CBS_PROP_STATUS] = @D48
      ,[NUM_CARD] = @D49
      ,[CUST_TYPE] = @D50
      ,[OUT_DEPT] = @D51
      ,[BKLST_HIST] = @D52
      ,[LITIG_STAT] = @D53
      ,[MESS_CHENL] = @D54
      ,[PASSID] = @D55
      ,[COBORREL] = @D56
      ,[GUARAREL] = @D57
      ,[CBS_SALARY] = @D58
      ,[CBS_NETINCOMESALARY] = @D59
      ,[CBS_NETINCOMEBUSINESS] = @D60
      ,[CBS_TOTALINCOMEBUSINESS] = @D61
      ,[CBS_TOTALOTHERINCOME] = @D62
      ,[CBS_TOTALOTHERINCOMEEVIDENCE] = @D63
      ,[CBS_INTEREST] = @D64
      ,[CBS_TAX] = @D65
      ,[CBS_PROPORTIONSHAREHOLDERS] = @D66
      ,[CBS_TOTALOUTSTANDINGOVERDUE] = @D67
      ,[CBS_DEBT_NCB] = @D68
      ,[CBS_TOTALLIABILITIES_NCB] = @D69
      ,[CBS_COOPERATIVELOAN] = @D70
      ,[CBS_BURDEN_DEBT] = @D71
      ,[CBS_BURDEN_GSB] = @D72
      ,[CBS_TOTALLIABILITIES_NOTNCB] = @D73
      ,[CBS_DTI] = @D74
      ,[CBS_DSCR] = @D75
      ,[CBS_PNInc2TotRev] = @D76
      ,[CBS_LoanCr] = @D77
      ,[CBS_PExp2TotRev] = @D78
	  where CBS_APP_NO = @D1 and APP_SEQ = (select max(APP_SEQ) from MYMO_LN_APPCIF where CBS_APP_NO = @D1)";

            cmd = new SqlCommand(sqlStatement, con);

            cmd.Parameters.Add("@D1", SqlDbType.VarChar, 22);
            cmd.Parameters.Add("@D3", SqlDbType.VarChar, 12);
            cmd.Parameters.Add("@D4", SqlDbType.VarChar, 5);
            cmd.Parameters.Add("@D5", SqlDbType.VarChar, 40);
            cmd.Parameters.Add("@D6", SqlDbType.VarChar, 17);
            cmd.Parameters.Add("@D7", SqlDbType.VarChar, 20);
            cmd.Parameters.Add("@D8", SqlDbType.VarChar, 1);
            cmd.Parameters.Add("@D9", SqlDbType.VarChar, 4);
            cmd.Parameters.Add("@D10", SqlDbType.VarChar, 13);
            cmd.Parameters.Add("@D12", SqlDbType.VarChar, 1);
            cmd.Parameters.Add("@D17", SqlDbType.VarChar, 5);
            cmd.Parameters.Add("@D18", SqlDbType.Decimal);
            cmd.Parameters.Add("@D19", SqlDbType.Decimal);
            cmd.Parameters.Add("@D20", SqlDbType.Int);
            cmd.Parameters.Add("@D21", SqlDbType.Int);
            cmd.Parameters.Add("@D22", SqlDbType.Decimal);
            cmd.Parameters.Add("@D23", SqlDbType.Decimal);
            cmd.Parameters.Add("@D24", SqlDbType.Decimal);
            cmd.Parameters.Add("@D25", SqlDbType.VarChar, 5);
            cmd.Parameters.Add("@D26", SqlDbType.Int);
            cmd.Parameters.Add("@D28", SqlDbType.Int);
            cmd.Parameters.Add("@D29", SqlDbType.Int);
            cmd.Parameters.Add("@D30", SqlDbType.VarChar, 100);
            cmd.Parameters.Add("@D31", SqlDbType.VarChar, 100);
            cmd.Parameters.Add("@D32", SqlDbType.VarChar, 100);
            cmd.Parameters.Add("@D33", SqlDbType.VarChar, 100);
            cmd.Parameters.Add("@D34", SqlDbType.VarChar, 6);
            cmd.Parameters.Add("@D35", SqlDbType.VarChar, 4);
            cmd.Parameters.Add("@D36", SqlDbType.VarChar, 4);
            cmd.Parameters.Add("@D37", SqlDbType.VarChar, 5);
            cmd.Parameters.Add("@D38", SqlDbType.VarChar, 3);
            cmd.Parameters.Add("@D39", SqlDbType.VarChar, 1);
            cmd.Parameters.Add("@D40", SqlDbType.VarChar, 3);
            cmd.Parameters.Add("@D41", SqlDbType.Int);
            cmd.Parameters.Add("@D42", SqlDbType.Int);
            cmd.Parameters.Add("@D43", SqlDbType.Int);
            cmd.Parameters.Add("@D44", SqlDbType.VarChar, 5);
            cmd.Parameters.Add("@D45", SqlDbType.VarChar, 5);
            cmd.Parameters.Add("@D46", SqlDbType.VarChar, 5);
            cmd.Parameters.Add("@D47", SqlDbType.Bit);
            cmd.Parameters.Add("@D48", SqlDbType.Bit);
            cmd.Parameters.Add("@D49", SqlDbType.VarChar, 50);
            cmd.Parameters.Add("@D50", SqlDbType.VarChar, 50);
            cmd.Parameters.Add("@D51", SqlDbType.VarChar, 50);
            cmd.Parameters.Add("@D52", SqlDbType.VarChar, 50);
            cmd.Parameters.Add("@D53", SqlDbType.VarChar, 15);
            cmd.Parameters.Add("@D54", SqlDbType.VarChar, 50);
            cmd.Parameters.Add("@D55", SqlDbType.VarChar, 50);
            cmd.Parameters.Add("@D56", SqlDbType.VarChar, 50);
            cmd.Parameters.Add("@D57", SqlDbType.VarChar, 50);
            cmd.Parameters.Add("@D58", SqlDbType.Decimal);
            cmd.Parameters.Add("@D59", SqlDbType.Decimal);
            cmd.Parameters.Add("@D60", SqlDbType.Decimal);
            cmd.Parameters.Add("@D61", SqlDbType.Decimal);
            cmd.Parameters.Add("@D62", SqlDbType.Decimal);
            cmd.Parameters.Add("@D63", SqlDbType.Decimal);
            cmd.Parameters.Add("@D64", SqlDbType.Decimal);
            cmd.Parameters.Add("@D65", SqlDbType.Decimal);
            cmd.Parameters.Add("@D66", SqlDbType.Decimal);
            cmd.Parameters.Add("@D67", SqlDbType.Decimal);
            cmd.Parameters.Add("@D68", SqlDbType.Decimal);
            cmd.Parameters.Add("@D69", SqlDbType.Decimal);
            cmd.Parameters.Add("@D70", SqlDbType.Decimal);
            cmd.Parameters.Add("@D71", SqlDbType.Decimal);
            cmd.Parameters.Add("@D72", SqlDbType.Decimal);
            cmd.Parameters.Add("@D73", SqlDbType.Decimal);
            cmd.Parameters.Add("@D74", SqlDbType.Decimal);
            cmd.Parameters.Add("@D75", SqlDbType.Decimal);
            cmd.Parameters.Add("@D76", SqlDbType.Decimal);
            cmd.Parameters.Add("@D77", SqlDbType.Decimal);
            cmd.Parameters.Add("@D78", SqlDbType.Decimal);


            cmd.Parameters["@D1"].Value = MyMoMonthly.CBS_APP_NO;
            cmd.Parameters["@D3"].Value = MyMoMonthly.CBS_CIFNO;
            cmd.Parameters["@D4"].Value = MyMoMonthly.CBS_TITLE_CD;
            cmd.Parameters["@D5"].Value = MyMoMonthly.CBS_FNAME;
            cmd.Parameters["@D6"].Value = MyMoMonthly.CBS_MNAME;
            cmd.Parameters["@D7"].Value = MyMoMonthly.CBS_SNAME;
            cmd.Parameters["@D8"].Value = MyMoMonthly.CBS_CTYPE_CD;
            cmd.Parameters["@D9"].Value = MyMoMonthly.CBS_CCODE;
            cmd.Parameters["@D10"].Value = MyMoMonthly.CBS_CITIZID;
            cmd.Parameters["@D12"].Value = MyMoMonthly.CBS_GENDER_CD;
            cmd.Parameters["@D17"].Value = MyMoMonthly.CBS_CSTATUS_CD;

            if (MyMoMonthly.CBS_INCOME.Trim() == "")
            {
                cmd.Parameters["@D18"].Value = DBNull.Value;
            }
            else
            {
                cmd.Parameters["@D18"].Value = Convert.ToDecimal(MyMoMonthly.CBS_INCOME);
            }

            if (MyMoMonthly.CBS_OTHINCOME.Trim() == "")
            {
                cmd.Parameters["@D19"].Value = DBNull.Value;
            }
            else
            {
                cmd.Parameters["@D19"].Value = Convert.ToDecimal(MyMoMonthly.CBS_OTHINCOME);
            }

            if (MyMoMonthly.CBS_CHILD.Trim() == "")
            {
                cmd.Parameters["@D20"].Value = DBNull.Value;
            }
            else
            {
                cmd.Parameters["@D20"].Value = Convert.ToInt32(MyMoMonthly.CBS_CHILD);
            }

            if (MyMoMonthly.CBS_WCHILD.Trim() == "")
            {
                cmd.Parameters["@D21"].Value = DBNull.Value;
            }
            else
            {
                cmd.Parameters["@D21"].Value = Convert.ToInt32(MyMoMonthly.CBS_WCHILD);
            }

            if (MyMoMonthly.CBS_MNTEXPEN.Trim() == "")
            {
                cmd.Parameters["@D22"].Value = DBNull.Value;
            }
            else
            {
                cmd.Parameters["@D22"].Value = Convert.ToDecimal(MyMoMonthly.CBS_MNTEXPEN);
            }

            if (MyMoMonthly.CBS_LBTEXPEN.Trim() == "")
            {
                cmd.Parameters["@D23"].Value = DBNull.Value;
            }
            else
            {
                cmd.Parameters["@D23"].Value = Convert.ToDecimal(MyMoMonthly.CBS_LBTEXPEN);
            }

            if (MyMoMonthly.CBS_BUSEXPEN.Trim() == "")
            {
                cmd.Parameters["@D24"].Value = DBNull.Value;
            }
            else
            {
                cmd.Parameters["@D24"].Value = Convert.ToDecimal(MyMoMonthly.CBS_BUSEXPEN);
            }

            cmd.Parameters["@D25"].Value = MyMoMonthly.CBS_RESSTATUS_CD;


            if (MyMoMonthly.CBS_PWORKTIME.Trim() == "")
            {
                cmd.Parameters["@D26"].Value = DBNull.Value;
            }
            else
            {
                cmd.Parameters["@D26"].Value = Convert.ToInt32(MyMoMonthly.CBS_PWORKTIME);
            }



            if (MyMoMonthly.CBS_BUSSMONTH.Trim() == "")
            {
                cmd.Parameters["@D28"].Value = DBNull.Value;
            }
            else
            {
                cmd.Parameters["@D28"].Value = Convert.ToInt32(MyMoMonthly.CBS_BUSSMONTH);
            }

            if (MyMoMonthly.CBS_BUSSYEAR.Trim() == "")
            {
                cmd.Parameters["@D29"].Value = DBNull.Value;
            }
            else
            {
                cmd.Parameters["@D29"].Value = Convert.ToInt32(MyMoMonthly.CBS_BUSSYEAR);
            }




            cmd.Parameters["@D30"].Value = MyMoMonthly.CBS_LADDL1;
            cmd.Parameters["@D31"].Value = MyMoMonthly.CBS_LADDL2;
            cmd.Parameters["@D32"].Value = MyMoMonthly.CBS_LADDL3;
            cmd.Parameters["@D33"].Value = MyMoMonthly.CBS_LADDL4;
            cmd.Parameters["@D34"].Value = MyMoMonthly.CBS_LDISTRICT_CD;
            cmd.Parameters["@D35"].Value = MyMoMonthly.CBS_LAMPHUR;
            cmd.Parameters["@D36"].Value = MyMoMonthly.CBS_LPROVINCE;
            cmd.Parameters["@D37"].Value = MyMoMonthly.CBS_LPOST;
            cmd.Parameters["@D38"].Value = MyMoMonthly.CBS_ISIC1;
            cmd.Parameters["@D39"].Value = MyMoMonthly.CBS_ISIC2;
            cmd.Parameters["@D40"].Value = MyMoMonthly.CBS_ISIC3;

            if (MyMoMonthly.CBS_CLIENTY.Trim() == "")
            {
                cmd.Parameters["@D41"].Value = DBNull.Value;
            }
            else
            {
                cmd.Parameters["@D41"].Value = Convert.ToInt32(MyMoMonthly.CBS_CLIENTY);
            }

            if (MyMoMonthly.CBS_CLIENTM.Trim() == "")
            {
                cmd.Parameters["@D42"].Value = DBNull.Value;
            }
            else
            {
                cmd.Parameters["@D42"].Value = Convert.ToInt32(MyMoMonthly.CBS_CLIENTM);
            }

            if (MyMoMonthly.CBS_PADDRTIME.Trim() == "")
            {
                cmd.Parameters["@D43"].Value = DBNull.Value;
            }
            else
            {
                cmd.Parameters["@D43"].Value = Convert.ToInt32(MyMoMonthly.CBS_PADDRTIME);
            }


            cmd.Parameters["@D44"].Value = MyMoMonthly.CBS_RESTYPE_CD;
            cmd.Parameters["@D45"].Value = MyMoMonthly.CBS_PROP_RIGHT_CD;
            cmd.Parameters["@D46"].Value = MyMoMonthly.CBS_BUSSTYPE_CD;

            if (MyMoMonthly.CBS_AUTO_STATUS.Trim() == "")
            {
                cmd.Parameters["@D47"].Value = DBNull.Value;
            }
            else
            {
                cmd.Parameters["@D47"].Value = Convert.ToInt32(MyMoMonthly.CBS_AUTO_STATUS);
            }

            if (MyMoMonthly.CBS_PROP_STATUS.Trim() == "")
            {
                cmd.Parameters["@D48"].Value = DBNull.Value;
            }
            else
            {
                cmd.Parameters["@D48"].Value = Convert.ToInt32(MyMoMonthly.CBS_PROP_STATUS);
            }



            cmd.Parameters["@D49"].Value = MyMoMonthly.NUM_CARD;
            cmd.Parameters["@D50"].Value = MyMoMonthly.CUST_TYPE;
            cmd.Parameters["@D51"].Value = MyMoMonthly.OUT_DEPT;
            cmd.Parameters["@D52"].Value = MyMoMonthly.BKLST_HIST;
            cmd.Parameters["@D53"].Value = MyMoMonthly.LITIG_STAT;
            cmd.Parameters["@D54"].Value = MyMoMonthly.MESS_CHENL;
            cmd.Parameters["@D55"].Value = MyMoMonthly.PASSID;
            cmd.Parameters["@D56"].Value = MyMoMonthly.COBORREL;
            cmd.Parameters["@D57"].Value = MyMoMonthly.GUARAREL;

            if (MyMoMonthly.CBS_SALARY.Trim() == "")
            {
                cmd.Parameters["@D58"].Value = DBNull.Value;
            }
            else
            {
                cmd.Parameters["@D58"].Value = Convert.ToDecimal(MyMoMonthly.CBS_SALARY);
            }

            if (MyMoMonthly.CBS_NETINCOMESALARY.Trim() == "")
            {
                cmd.Parameters["@D59"].Value = DBNull.Value;
            }
            else
            {
                cmd.Parameters["@D59"].Value = Convert.ToDecimal(MyMoMonthly.CBS_NETINCOMESALARY);
            }

            if (MyMoMonthly.CBS_NETINCOMEBUSINESS.Trim() == "")
            {
                cmd.Parameters["@D60"].Value = DBNull.Value;
            }
            else
            {
                cmd.Parameters["@D60"].Value = Convert.ToDecimal(MyMoMonthly.CBS_NETINCOMEBUSINESS);
            }

            if (MyMoMonthly.CBS_TOTALINCOMEBUSINESS.Trim() == "")
            {
                cmd.Parameters["@D61"].Value = DBNull.Value;
            }
            else
            {
                cmd.Parameters["@D61"].Value = Convert.ToDecimal(MyMoMonthly.CBS_TOTALINCOMEBUSINESS);
            }

            if (MyMoMonthly.CBS_TOTALOTHERINCOME.Trim() == "")
            {
                cmd.Parameters["@D62"].Value = DBNull.Value;
            }
            else
            {
                cmd.Parameters["@D62"].Value = Convert.ToDecimal(MyMoMonthly.CBS_TOTALOTHERINCOME);
            }

            if (MyMoMonthly.CBS_TOTALOTHERINCOMEEVIDENCE.Trim() == "")
            {
                cmd.Parameters["@D63"].Value = DBNull.Value;
            }
            else
            {
                cmd.Parameters["@D63"].Value = Convert.ToDecimal(MyMoMonthly.CBS_TOTALOTHERINCOMEEVIDENCE);
            }

            if (MyMoMonthly.CBS_INTEREST.Trim() == "")
            {
                cmd.Parameters["@D64"].Value = DBNull.Value;
            }
            else
            {
                cmd.Parameters["@D64"].Value = Convert.ToDecimal(MyMoMonthly.CBS_INTEREST);
            }

            if (MyMoMonthly.CBS_TAX.Trim() == "")
            {
                cmd.Parameters["@D65"].Value = DBNull.Value;
            }
            else
            {
                cmd.Parameters["@D65"].Value = Convert.ToDecimal(MyMoMonthly.CBS_TAX);
            }

            if (MyMoMonthly.CBS_PROPORTIONSHAREHOLDERS.Trim() == "")
            {
                cmd.Parameters["@D66"].Value = DBNull.Value;
            }
            else
            {
                cmd.Parameters["@D66"].Value = Convert.ToDecimal(MyMoMonthly.CBS_PROPORTIONSHAREHOLDERS);
            }

            if (MyMoMonthly.CBS_TOTALOUTSTANDINGOVERDUE.Trim() == "")
            {
                cmd.Parameters["@D67"].Value = DBNull.Value;
            }
            else
            {
                cmd.Parameters["@D67"].Value = Convert.ToDecimal(MyMoMonthly.CBS_TOTALOUTSTANDINGOVERDUE);
            }

            if (MyMoMonthly.CBS_DEBT_NCB.Trim() == "")
            {
                cmd.Parameters["@D68"].Value = DBNull.Value;
            }
            else
            {
                cmd.Parameters["@D68"].Value = Convert.ToDecimal(MyMoMonthly.CBS_DEBT_NCB);
            }

            if (MyMoMonthly.CBS_TOTALLIABILITIES_NCB.Trim() == "")
            {
                cmd.Parameters["@D69"].Value = DBNull.Value;
            }
            else
            {
                cmd.Parameters["@D69"].Value = Convert.ToDecimal(MyMoMonthly.CBS_TOTALLIABILITIES_NCB);
            }

            if (MyMoMonthly.CBS_COOPERATIVELOAN.Trim() == "")
            {
                cmd.Parameters["@D70"].Value = DBNull.Value;
            }
            else
            {
                cmd.Parameters["@D70"].Value = Convert.ToDecimal(MyMoMonthly.CBS_COOPERATIVELOAN);
            }

            if (MyMoMonthly.CBS_BURDEN_DEBT.Trim() == "")
            {
                cmd.Parameters["@D71"].Value = DBNull.Value;
            }
            else
            {
                cmd.Parameters["@D71"].Value = Convert.ToDecimal(MyMoMonthly.CBS_BURDEN_DEBT);
            }

            if (MyMoMonthly.CBS_BURDEN_GSB.Trim() == "")
            {
                cmd.Parameters["@D72"].Value = DBNull.Value;
            }
            else
            {
                cmd.Parameters["@D72"].Value = Convert.ToDecimal(MyMoMonthly.CBS_BURDEN_GSB);
            }

            if (MyMoMonthly.CBS_TOTALLIABILITIES_NOTNCB.Trim() == "")
            {
                cmd.Parameters["@D73"].Value = DBNull.Value;
            }
            else
            {
                cmd.Parameters["@D73"].Value = Convert.ToDecimal(MyMoMonthly.CBS_TOTALLIABILITIES_NOTNCB);
            }

            if (MyMoMonthly.CBS_DTI.Trim() == "")
            {
                cmd.Parameters["@D74"].Value = DBNull.Value;
            }
            else
            {
                cmd.Parameters["@D74"].Value = Convert.ToDecimal(MyMoMonthly.CBS_DTI);
            }

            if (MyMoMonthly.CBS_DSCR.Trim() == "")
            {
                cmd.Parameters["@D75"].Value = DBNull.Value;
            }
            else
            {
                cmd.Parameters["@D75"].Value = Convert.ToDecimal(MyMoMonthly.CBS_DSCR);
            }

            if (MyMoMonthly.CBS_PNInc2TotRev.Trim() == "")
            {
                cmd.Parameters["@D76"].Value = DBNull.Value;
            }
            else
            {
                cmd.Parameters["@D76"].Value = Convert.ToDecimal(MyMoMonthly.CBS_PNInc2TotRev);
            }

            if (MyMoMonthly.CBS_LoanCr.Trim() == "")
            {
                cmd.Parameters["@D77"].Value = DBNull.Value;
            }
            else
            {
                cmd.Parameters["@D77"].Value = Convert.ToDecimal(MyMoMonthly.CBS_LoanCr);
            }

            if (MyMoMonthly.CBS_PExp2TotRev.Trim() == "")
            {
                cmd.Parameters["@D78"].Value = DBNull.Value;
            }
            else
            {
                cmd.Parameters["@D78"].Value = Convert.ToDecimal(MyMoMonthly.CBS_PExp2TotRev);
            }




            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
        }

        private void Insert_MyMoAPPCIF(MyMoMonthly MyMoMonthly)
        {

            SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["FTPIP"].ToString());
            SqlCommand cmd;

            string sqlStatement =
@"INSERT INTO MYMO_LN_APPCIF
        VALUES(@D1, @D2, @D3 , @D4,@D5, @D6, @D7 , @D8,@D9, @D10, @D11 , @D12,@D13, @D14, @D15 , @D16,@D17,@D18,@D19, 
        @D20, @D21 , @D22,@D23, @D24, @D25 , @D26,@D27, @D28, @D29 , @D30,@D31, @D32, @D33 , @D34,@D35,@D36,@D37, @D38, @D39 , @D40,
        @D41, @D42, @D43 , @D44,@D45, @D46, @D47 , @D48,@D49, @D50, @D51 , @D52,@D53,@D54,@D55, @D56, @D57 , @D58,@D59, @D60, 
        @D61 , @D62,@D63, @D64, @D65 , @D66,@D67, @D68, @D69 , @D70,@D71,@D72,@D73,@D74 , @D75,@D76, @D77, @D78)";

            cmd = new SqlCommand(sqlStatement, con);

            cmd.Parameters.Add("@D1", SqlDbType.VarChar, 22);
            cmd.Parameters.Add("@D2", SqlDbType.Int);
            cmd.Parameters.Add("@D3", SqlDbType.VarChar, 12);
            cmd.Parameters.Add("@D4", SqlDbType.VarChar, 5);
            cmd.Parameters.Add("@D5", SqlDbType.VarChar, 40);
            cmd.Parameters.Add("@D6", SqlDbType.VarChar, 17);
            cmd.Parameters.Add("@D7", SqlDbType.VarChar, 20);
            cmd.Parameters.Add("@D8", SqlDbType.VarChar, 1);
            cmd.Parameters.Add("@D9", SqlDbType.VarChar, 4);
            cmd.Parameters.Add("@D10", SqlDbType.VarChar, 13);
            cmd.Parameters.Add("@D11", SqlDbType.DateTime);
            cmd.Parameters.Add("@D12", SqlDbType.VarChar, 1);
            cmd.Parameters.Add("@D13", SqlDbType.VarChar, 2);
            cmd.Parameters.Add("@D14", SqlDbType.VarChar, 4);
            cmd.Parameters.Add("@D15", SqlDbType.VarChar, 2);
            cmd.Parameters.Add("@D16", SqlDbType.VarChar, 4);
            cmd.Parameters.Add("@D17", SqlDbType.VarChar, 5);
            cmd.Parameters.Add("@D18", SqlDbType.Decimal);
            cmd.Parameters.Add("@D19", SqlDbType.Decimal);
            cmd.Parameters.Add("@D20", SqlDbType.Int);
            cmd.Parameters.Add("@D21", SqlDbType.Int);
            cmd.Parameters.Add("@D22", SqlDbType.Decimal);
            cmd.Parameters.Add("@D23", SqlDbType.Decimal);
            cmd.Parameters.Add("@D24", SqlDbType.Decimal);
            cmd.Parameters.Add("@D25", SqlDbType.VarChar, 5);
            cmd.Parameters.Add("@D26", SqlDbType.Int);
            cmd.Parameters.Add("@D27", SqlDbType.Int);
            cmd.Parameters.Add("@D28", SqlDbType.Int);
            cmd.Parameters.Add("@D29", SqlDbType.Int);
            cmd.Parameters.Add("@D30", SqlDbType.VarChar, 100);
            cmd.Parameters.Add("@D31", SqlDbType.VarChar, 100);
            cmd.Parameters.Add("@D32", SqlDbType.VarChar, 100);
            cmd.Parameters.Add("@D33", SqlDbType.VarChar, 100);
            cmd.Parameters.Add("@D34", SqlDbType.VarChar, 6);
            cmd.Parameters.Add("@D35", SqlDbType.VarChar, 4);
            cmd.Parameters.Add("@D36", SqlDbType.VarChar, 4);
            cmd.Parameters.Add("@D37", SqlDbType.VarChar, 5);
            cmd.Parameters.Add("@D38", SqlDbType.VarChar, 3);
            cmd.Parameters.Add("@D39", SqlDbType.VarChar, 1);
            cmd.Parameters.Add("@D40", SqlDbType.VarChar, 3);
            cmd.Parameters.Add("@D41", SqlDbType.Int);
            cmd.Parameters.Add("@D42", SqlDbType.Int);
            cmd.Parameters.Add("@D43", SqlDbType.Int);
            cmd.Parameters.Add("@D44", SqlDbType.VarChar, 5);
            cmd.Parameters.Add("@D45", SqlDbType.VarChar, 5);
            cmd.Parameters.Add("@D46", SqlDbType.VarChar, 5);
            cmd.Parameters.Add("@D47", SqlDbType.Bit);
            cmd.Parameters.Add("@D48", SqlDbType.Bit);
            cmd.Parameters.Add("@D49", SqlDbType.VarChar, 50);
            cmd.Parameters.Add("@D50", SqlDbType.VarChar, 50);
            cmd.Parameters.Add("@D51", SqlDbType.VarChar, 50);
            cmd.Parameters.Add("@D52", SqlDbType.VarChar, 50);
            cmd.Parameters.Add("@D53", SqlDbType.VarChar, 15);
            cmd.Parameters.Add("@D54", SqlDbType.VarChar, 50);
            cmd.Parameters.Add("@D55", SqlDbType.VarChar, 50);
            cmd.Parameters.Add("@D56", SqlDbType.VarChar, 50);
            cmd.Parameters.Add("@D57", SqlDbType.VarChar, 50);
            cmd.Parameters.Add("@D58", SqlDbType.Decimal);
            cmd.Parameters.Add("@D59", SqlDbType.Decimal);
            cmd.Parameters.Add("@D60", SqlDbType.Decimal);
            cmd.Parameters.Add("@D61", SqlDbType.Decimal);
            cmd.Parameters.Add("@D62", SqlDbType.Decimal);
            cmd.Parameters.Add("@D63", SqlDbType.Decimal);
            cmd.Parameters.Add("@D64", SqlDbType.Decimal);
            cmd.Parameters.Add("@D65", SqlDbType.Decimal);
            cmd.Parameters.Add("@D66", SqlDbType.Decimal);
            cmd.Parameters.Add("@D67", SqlDbType.Decimal);
            cmd.Parameters.Add("@D68", SqlDbType.Decimal);
            cmd.Parameters.Add("@D69", SqlDbType.Decimal);
            cmd.Parameters.Add("@D70", SqlDbType.Decimal);
            cmd.Parameters.Add("@D71", SqlDbType.Decimal);
            cmd.Parameters.Add("@D72", SqlDbType.Decimal);
            cmd.Parameters.Add("@D73", SqlDbType.Decimal);
            cmd.Parameters.Add("@D74", SqlDbType.Decimal);
            cmd.Parameters.Add("@D75", SqlDbType.Decimal);
            cmd.Parameters.Add("@D76", SqlDbType.Decimal);
            cmd.Parameters.Add("@D77", SqlDbType.Decimal);
            cmd.Parameters.Add("@D78", SqlDbType.Decimal);


            cmd.Parameters["@D1"].Value = MyMoMonthly.CBS_APP_NO;
            cmd.Parameters["@D2"].Value = 1;
            cmd.Parameters["@D3"].Value = MyMoMonthly.CBS_CIFNO;
            cmd.Parameters["@D4"].Value = MyMoMonthly.CBS_TITLE_CD;
            cmd.Parameters["@D5"].Value = MyMoMonthly.CBS_FNAME;
            cmd.Parameters["@D6"].Value = MyMoMonthly.CBS_MNAME;
            cmd.Parameters["@D7"].Value = MyMoMonthly.CBS_SNAME;
            cmd.Parameters["@D8"].Value = MyMoMonthly.CBS_CTYPE_CD;
            cmd.Parameters["@D9"].Value = MyMoMonthly.CBS_CCODE;
            cmd.Parameters["@D10"].Value = MyMoMonthly.CBS_CITIZID;
            if (MyMoMonthly.CBS_CDOB.Trim() == "")
            {
                cmd.Parameters["@D11"].Value = DBNull.Value;
            }
            else
            {
                cmd.Parameters["@D11"].Value = Convert.ToDateTime(MyMoMonthly.CBS_CDOB, System.Globalization.CultureInfo.InvariantCulture);
            }

            cmd.Parameters["@D12"].Value = MyMoMonthly.CBS_GENDER_CD;
            cmd.Parameters["@D13"].Value = MyMoMonthly.CBS_MARITAL_CD;
            cmd.Parameters["@D14"].Value = MyMoMonthly.CBS_OCCCLS_CD;
            cmd.Parameters["@D15"].Value = MyMoMonthly.CBS_OCCSCLS_CD;
            cmd.Parameters["@D16"].Value = MyMoMonthly.CBS_EDU_CD;
            cmd.Parameters["@D17"].Value = MyMoMonthly.CBS_CSTATUS_CD;

            if (MyMoMonthly.CBS_INCOME.Trim() == "")
            {
                cmd.Parameters["@D18"].Value = DBNull.Value;
            }
            else
            {
                cmd.Parameters["@D18"].Value = Convert.ToDecimal(MyMoMonthly.CBS_INCOME);
            }

            if (MyMoMonthly.CBS_OTHINCOME.Trim() == "")
            {
                cmd.Parameters["@D19"].Value = DBNull.Value;
            }
            else
            {
                cmd.Parameters["@D19"].Value = Convert.ToDecimal(MyMoMonthly.CBS_OTHINCOME);
            }

            if (MyMoMonthly.CBS_CHILD.Trim() == "")
            {
                cmd.Parameters["@D20"].Value = DBNull.Value;
            }
            else
            {
                cmd.Parameters["@D20"].Value = Convert.ToInt32(MyMoMonthly.CBS_CHILD);
            }

            if (MyMoMonthly.CBS_WCHILD.Trim() == "")
            {
                cmd.Parameters["@D21"].Value = DBNull.Value;
            }
            else
            {
                cmd.Parameters["@D21"].Value = Convert.ToInt32(MyMoMonthly.CBS_WCHILD);
            }

            if (MyMoMonthly.CBS_MNTEXPEN.Trim() == "")
            {
                cmd.Parameters["@D22"].Value = DBNull.Value;
            }
            else
            {
                cmd.Parameters["@D22"].Value = Convert.ToDecimal(MyMoMonthly.CBS_MNTEXPEN);
            }

            if (MyMoMonthly.CBS_LBTEXPEN.Trim() == "")
            {
                cmd.Parameters["@D23"].Value = DBNull.Value;
            }
            else
            {
                cmd.Parameters["@D23"].Value = Convert.ToDecimal(MyMoMonthly.CBS_LBTEXPEN);
            }

            if (MyMoMonthly.CBS_BUSEXPEN.Trim() == "")
            {
                cmd.Parameters["@D24"].Value = DBNull.Value;
            }
            else
            {
                cmd.Parameters["@D24"].Value = Convert.ToDecimal(MyMoMonthly.CBS_BUSEXPEN);
            }

            cmd.Parameters["@D25"].Value = MyMoMonthly.CBS_RESSTATUS_CD;


            if (MyMoMonthly.CBS_PWORKTIME.Trim() == "")
            {
                cmd.Parameters["@D26"].Value = DBNull.Value;
            }
            else
            {
                cmd.Parameters["@D26"].Value = Convert.ToInt32(MyMoMonthly.CBS_PWORKTIME);
            }

            if (MyMoMonthly.CBS_TWORKTIME.Trim() == "")
            {
                cmd.Parameters["@D27"].Value = DBNull.Value;
            }
            else
            {
                cmd.Parameters["@D27"].Value = Convert.ToInt32(MyMoMonthly.CBS_TWORKTIME);
            }

            if (MyMoMonthly.CBS_BUSSMONTH.Trim() == "")
            {
                cmd.Parameters["@D28"].Value = DBNull.Value;
            }
            else
            {
                cmd.Parameters["@D28"].Value = Convert.ToInt32(MyMoMonthly.CBS_BUSSMONTH);
            }

            if (MyMoMonthly.CBS_BUSSYEAR.Trim() == "")
            {
                cmd.Parameters["@D29"].Value = DBNull.Value;
            }
            else
            {
                cmd.Parameters["@D29"].Value = Convert.ToInt32(MyMoMonthly.CBS_BUSSYEAR);
            }




            cmd.Parameters["@D30"].Value = MyMoMonthly.CBS_LADDL1;
            cmd.Parameters["@D31"].Value = MyMoMonthly.CBS_LADDL2;
            cmd.Parameters["@D32"].Value = MyMoMonthly.CBS_LADDL3;
            cmd.Parameters["@D33"].Value = MyMoMonthly.CBS_LADDL4;
            cmd.Parameters["@D34"].Value = MyMoMonthly.CBS_LDISTRICT_CD;
            cmd.Parameters["@D35"].Value = MyMoMonthly.CBS_LAMPHUR;
            cmd.Parameters["@D36"].Value = MyMoMonthly.CBS_LPROVINCE;
            cmd.Parameters["@D37"].Value = MyMoMonthly.CBS_LPOST;
            cmd.Parameters["@D38"].Value = MyMoMonthly.CBS_ISIC1;
            cmd.Parameters["@D39"].Value = MyMoMonthly.CBS_ISIC2;
            cmd.Parameters["@D40"].Value = MyMoMonthly.CBS_ISIC3;

            if (MyMoMonthly.CBS_CLIENTY.Trim() == "")
            {
                cmd.Parameters["@D41"].Value = DBNull.Value;
            }
            else
            {
                cmd.Parameters["@D41"].Value = Convert.ToInt32(MyMoMonthly.CBS_CLIENTY);
            }

            if (MyMoMonthly.CBS_CLIENTM.Trim() == "")
            {
                cmd.Parameters["@D42"].Value = DBNull.Value;
            }
            else
            {
                cmd.Parameters["@D42"].Value = Convert.ToInt32(MyMoMonthly.CBS_CLIENTM);
            }

            if (MyMoMonthly.CBS_PADDRTIME.Trim() == "")
            {
                cmd.Parameters["@D43"].Value = DBNull.Value;
            }
            else
            {
                cmd.Parameters["@D43"].Value = Convert.ToInt32(MyMoMonthly.CBS_PADDRTIME);
            }


            cmd.Parameters["@D44"].Value = MyMoMonthly.CBS_RESTYPE_CD;
            cmd.Parameters["@D45"].Value = MyMoMonthly.CBS_PROP_RIGHT_CD;
            cmd.Parameters["@D46"].Value = MyMoMonthly.CBS_BUSSTYPE_CD;

            if (MyMoMonthly.CBS_AUTO_STATUS.Trim() == "")
            {
                cmd.Parameters["@D47"].Value = DBNull.Value;
            }
            else
            {
                cmd.Parameters["@D47"].Value = Convert.ToInt32(MyMoMonthly.CBS_AUTO_STATUS);
            }

            if (MyMoMonthly.CBS_PROP_STATUS.Trim() == "")
            {
                cmd.Parameters["@D48"].Value = DBNull.Value;
            }
            else
            {
                cmd.Parameters["@D48"].Value = Convert.ToInt32(MyMoMonthly.CBS_PROP_STATUS);
            }



            cmd.Parameters["@D49"].Value = MyMoMonthly.NUM_CARD;
            cmd.Parameters["@D50"].Value = MyMoMonthly.CUST_TYPE;
            cmd.Parameters["@D51"].Value = MyMoMonthly.OUT_DEPT;
            cmd.Parameters["@D52"].Value = MyMoMonthly.BKLST_HIST;
            cmd.Parameters["@D53"].Value = MyMoMonthly.LITIG_STAT;
            cmd.Parameters["@D54"].Value = MyMoMonthly.MESS_CHENL;
            cmd.Parameters["@D55"].Value = MyMoMonthly.PASSID;
            cmd.Parameters["@D56"].Value = MyMoMonthly.COBORREL;
            cmd.Parameters["@D57"].Value = MyMoMonthly.GUARAREL;

            if (MyMoMonthly.CBS_SALARY.Trim() == "")
            {
                cmd.Parameters["@D58"].Value = DBNull.Value;
            }
            else
            {
                cmd.Parameters["@D58"].Value = Convert.ToDecimal(MyMoMonthly.CBS_SALARY);
            }

            if (MyMoMonthly.CBS_NETINCOMESALARY.Trim() == "")
            {
                cmd.Parameters["@D59"].Value = DBNull.Value;
            }
            else
            {
                cmd.Parameters["@D59"].Value = Convert.ToDecimal(MyMoMonthly.CBS_NETINCOMESALARY);
            }

            if (MyMoMonthly.CBS_NETINCOMEBUSINESS.Trim() == "")
            {
                cmd.Parameters["@D60"].Value = DBNull.Value;
            }
            else
            {
                cmd.Parameters["@D60"].Value = Convert.ToDecimal(MyMoMonthly.CBS_NETINCOMEBUSINESS);
            }

            if (MyMoMonthly.CBS_TOTALINCOMEBUSINESS.Trim() == "")
            {
                cmd.Parameters["@D61"].Value = DBNull.Value;
            }
            else
            {
                cmd.Parameters["@D61"].Value = Convert.ToDecimal(MyMoMonthly.CBS_TOTALINCOMEBUSINESS);
            }

            if (MyMoMonthly.CBS_TOTALOTHERINCOME.Trim() == "")
            {
                cmd.Parameters["@D62"].Value = DBNull.Value;
            }
            else
            {
                cmd.Parameters["@D62"].Value = Convert.ToDecimal(MyMoMonthly.CBS_TOTALOTHERINCOME);
            }

            if (MyMoMonthly.CBS_TOTALOTHERINCOMEEVIDENCE.Trim() == "")
            {
                cmd.Parameters["@D63"].Value = DBNull.Value;
            }
            else
            {
                cmd.Parameters["@D63"].Value = Convert.ToDecimal(MyMoMonthly.CBS_TOTALOTHERINCOMEEVIDENCE);
            }

            if (MyMoMonthly.CBS_INTEREST.Trim() == "")
            {
                cmd.Parameters["@D64"].Value = DBNull.Value;
            }
            else
            {
                cmd.Parameters["@D64"].Value = Convert.ToDecimal(MyMoMonthly.CBS_INTEREST);
            }

            if (MyMoMonthly.CBS_TAX.Trim() == "")
            {
                cmd.Parameters["@D65"].Value = DBNull.Value;
            }
            else
            {
                cmd.Parameters["@D65"].Value = Convert.ToDecimal(MyMoMonthly.CBS_TAX);
            }

            if (MyMoMonthly.CBS_PROPORTIONSHAREHOLDERS.Trim() == "")
            {
                cmd.Parameters["@D66"].Value = DBNull.Value;
            }
            else
            {
                cmd.Parameters["@D66"].Value = Convert.ToDecimal(MyMoMonthly.CBS_PROPORTIONSHAREHOLDERS);
            }

            if (MyMoMonthly.CBS_TOTALOUTSTANDINGOVERDUE.Trim() == "")
            {
                cmd.Parameters["@D67"].Value = DBNull.Value;
            }
            else
            {
                cmd.Parameters["@D67"].Value = Convert.ToDecimal(MyMoMonthly.CBS_TOTALOUTSTANDINGOVERDUE);
            }

            if (MyMoMonthly.CBS_DEBT_NCB.Trim() == "")
            {
                cmd.Parameters["@D68"].Value = DBNull.Value;
            }
            else
            {
                cmd.Parameters["@D68"].Value = Convert.ToDecimal(MyMoMonthly.CBS_DEBT_NCB);
            }

            if (MyMoMonthly.CBS_TOTALLIABILITIES_NCB.Trim() == "")
            {
                cmd.Parameters["@D69"].Value = DBNull.Value;
            }
            else
            {
                cmd.Parameters["@D69"].Value = Convert.ToDecimal(MyMoMonthly.CBS_TOTALLIABILITIES_NCB);
            }

            if (MyMoMonthly.CBS_COOPERATIVELOAN.Trim() == "")
            {
                cmd.Parameters["@D70"].Value = DBNull.Value;
            }
            else
            {
                cmd.Parameters["@D70"].Value = Convert.ToDecimal(MyMoMonthly.CBS_COOPERATIVELOAN);
            }

            if (MyMoMonthly.CBS_BURDEN_DEBT.Trim() == "")
            {
                cmd.Parameters["@D71"].Value = DBNull.Value;
            }
            else
            {
                cmd.Parameters["@D71"].Value = Convert.ToDecimal(MyMoMonthly.CBS_BURDEN_DEBT);
            }

            if (MyMoMonthly.CBS_BURDEN_GSB.Trim() == "")
            {
                cmd.Parameters["@D72"].Value = DBNull.Value;
            }
            else
            {
                cmd.Parameters["@D72"].Value = Convert.ToDecimal(MyMoMonthly.CBS_BURDEN_GSB);
            }

            if (MyMoMonthly.CBS_TOTALLIABILITIES_NOTNCB.Trim() == "")
            {
                cmd.Parameters["@D73"].Value = DBNull.Value;
            }
            else
            {
                cmd.Parameters["@D73"].Value = Convert.ToDecimal(MyMoMonthly.CBS_TOTALLIABILITIES_NOTNCB);
            }

            if (MyMoMonthly.CBS_DTI.Trim() == "")
            {
                cmd.Parameters["@D74"].Value = DBNull.Value;
            }
            else
            {
                cmd.Parameters["@D74"].Value = Convert.ToDecimal(MyMoMonthly.CBS_DTI);
            }

            if (MyMoMonthly.CBS_DSCR.Trim() == "")
            {
                cmd.Parameters["@D75"].Value = DBNull.Value;
            }
            else
            {
                cmd.Parameters["@D75"].Value = Convert.ToDecimal(MyMoMonthly.CBS_DSCR);
            }

            if (MyMoMonthly.CBS_PNInc2TotRev.Trim() == "")
            {
                cmd.Parameters["@D76"].Value = DBNull.Value;
            }
            else
            {
                cmd.Parameters["@D76"].Value = Convert.ToDecimal(MyMoMonthly.CBS_PNInc2TotRev);
            }

            if (MyMoMonthly.CBS_LoanCr.Trim() == "")
            {
                cmd.Parameters["@D77"].Value = DBNull.Value;
            }
            else
            {
                cmd.Parameters["@D77"].Value = Convert.ToDecimal(MyMoMonthly.CBS_LoanCr);
            }

            if (MyMoMonthly.CBS_PExp2TotRev.Trim() == "")
            {
                cmd.Parameters["@D78"].Value = DBNull.Value;
            }
            else
            {
                cmd.Parameters["@D78"].Value = Convert.ToDecimal(MyMoMonthly.CBS_PExp2TotRev);
            }




            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
        }

        private void Insert_MyMoLNAPP(MyMoMonthly MyMoMonthly)
        {

            SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["FTPIP"].ToString());
            SqlCommand cmd;

            string sqlStatement =
@"INSERT INTO MYMO_LN_APP([APP_NO],[APP_DATE],[ACCTYPE],[LOAN_CD],[STYPE_CD],[MARKET_CD],[CBS_STATUS],[APPRAMT],[REASON_CD],[CBS_APPROVE_COMMENT],[APP_SEQ],BRANCH_CD,CBS_APPROVE_CD)
        VALUES(@D1, @D2, @D3 , @D4,@D5, @D6, @D7 , @D8,@D9, @D10,@D11,@D12,@D13)";

            cmd = new SqlCommand(sqlStatement, con);

            cmd.Parameters.Add("@D1", SqlDbType.VarChar, 22);
            cmd.Parameters.Add("@D2", SqlDbType.DateTime);
            cmd.Parameters.Add("@D3", SqlDbType.VarChar, 4);
            cmd.Parameters.Add("@D4", SqlDbType.VarChar, 4);
            cmd.Parameters.Add("@D5", SqlDbType.VarChar, 5);
            cmd.Parameters.Add("@D6", SqlDbType.VarChar, 5);
            cmd.Parameters.Add("@D7", SqlDbType.VarChar, 2);
            cmd.Parameters.Add("@D8", SqlDbType.Decimal);
            cmd.Parameters.Add("@D9", SqlDbType.VarChar, 200);
            cmd.Parameters.Add("@D10", SqlDbType.VarChar, 200);
            cmd.Parameters.Add("@D11", SqlDbType.Int);
            cmd.Parameters.Add("@D12", SqlDbType.VarChar, 10);
            cmd.Parameters.Add("@D13", SqlDbType.Int);

            cmd.Parameters["@D1"].Value = MyMoMonthly.CBS_APP_NO;

            cmd.Parameters["@D2"].Value = Convert.ToDateTime(ddlMNT.SelectedValue.Substring(0, 4) + "-" + ddlMNT.SelectedValue.Substring(4, 2) + "-" + ddlMNT.SelectedValue.Substring(6, 2), System.Globalization.CultureInfo.InvariantCulture);
            cmd.Parameters["@D3"].Value = "LN";
            cmd.Parameters["@D4"].Value = "7900";
            cmd.Parameters["@D5"].Value = "30014";
            cmd.Parameters["@D6"].Value = "1354";
            cmd.Parameters["@D7"].Value = MyMoMonthly.CBS_APPROVE_CD;

            if (MyMoMonthly.APPRAMT.Trim() == "")
            {
                cmd.Parameters["@D8"].Value = DBNull.Value;
            }
            else
            {
                cmd.Parameters["@D8"].Value = MyMoMonthly.APPRAMT;
            }

            cmd.Parameters["@D9"].Value = MyMoMonthly.CBS_REASON_CD;
            cmd.Parameters["@D10"].Value = MyMoMonthly.CBS_APPROVE_COMMENT;
            cmd.Parameters["@D11"].Value = 1;
            cmd.Parameters["@D12"].Value = MyMoMonthly.BRANCH_CD;

            if (MyMoMonthly.CBS_APPROVE_CD.Trim() == "Y")
            {
                cmd.Parameters["@D13"].Value = 3;
            }
            else if (MyMoMonthly.CBS_APPROVE_CD.Trim() == "N")
            {
                cmd.Parameters["@D13"].Value = 4;
            }
            else
            {
                cmd.Parameters["@D13"].Value = 0;
            }

            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();

        }

        private void Update_MyMoLNAPP(MyMoMonthly MyMoMonthly)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["FTPIP"].ToString());
            SqlCommand cmd;

            string sqlStatement =
@"update MYMO_LN_APP set  
[CBS_STATUS] = @D7,
[APPRAMT] = @D8,
[REASON_CD] = @D9,
[CBS_APPROVE_COMMENT] = @D10,
BRANCH_CD = @D12,
CBS_APPROVE_CD = @D13
where APP_NO = @D1 and APP_SEQ = (select max(APP_SEQ) from MYMO_LN_APPCIF where CBS_APP_NO = @D1)";

            cmd = new SqlCommand(sqlStatement, con);

            cmd.Parameters.Add("@D1", SqlDbType.VarChar, 22);
            cmd.Parameters.Add("@D7", SqlDbType.VarChar, 22);
            cmd.Parameters.Add("@D8", SqlDbType.Decimal);
            cmd.Parameters.Add("@D9", SqlDbType.VarChar, 200);
            cmd.Parameters.Add("@D10", SqlDbType.VarChar, 200);
            cmd.Parameters.Add("@D12", SqlDbType.VarChar, 10);
            cmd.Parameters.Add("@D13", SqlDbType.Int);

            cmd.Parameters["@D1"].Value = MyMoMonthly.CBS_APP_NO;
            cmd.Parameters["@D7"].Value = MyMoMonthly.CBS_APPROVE_CD;

            if (MyMoMonthly.APPRAMT.Trim() == "")
            {
                cmd.Parameters["@D8"].Value = DBNull.Value;
            }
            else
            {
                cmd.Parameters["@D8"].Value = MyMoMonthly.APPRAMT;
            }

            cmd.Parameters["@D9"].Value = MyMoMonthly.CBS_REASON_CD;
            cmd.Parameters["@D10"].Value = MyMoMonthly.CBS_APPROVE_COMMENT;
            cmd.Parameters["@D12"].Value = MyMoMonthly.BRANCH_CD;

            if (MyMoMonthly.CBS_APPROVE_CD.Trim() == "Y")
            {
                cmd.Parameters["@D13"].Value = 3;
            }
            else if (MyMoMonthly.CBS_APPROVE_CD.Trim() == "N")
            {
                cmd.Parameters["@D13"].Value = 4;
            }
            else
            {
                cmd.Parameters["@D13"].Value = 0;
            }

            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
        }


        private void Insert_NCB(MyMoMonthly MyMoMonthly)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["FTPIP"].ToString());
            SqlCommand cmd;

            string sqlStatement =
@"INSERT INTO NCB
        VALUES(@D1, @D2, @D3 , @D4,@D5, @D6, @D7 , @D8,@D9, @D10,@D11,@D12,@D13, @D14, @D15 , @D16,@D17, @D18, @D19 , @D20,@D21, @D22,@D23,@D24
        ,@D25, @D26, @D27 , @D28,@D29, @D30, @D31 , @D32,@D33, @D34,@D35,@D36,@D37, @D38, @D39 , @D40,@D41, @D42, @D43 , @D44,@D45, @D46,@D47)";

            cmd = new SqlCommand(sqlStatement, con);

            cmd.Parameters.Add("@D1", SqlDbType.Date);
            cmd.Parameters.Add("@D2", SqlDbType.VarChar, 50);
            cmd.Parameters.Add("@D3", SqlDbType.VarChar, 50);
            cmd.Parameters.Add("@D4", SqlDbType.VarChar, 50);
            cmd.Parameters.Add("@D5", SqlDbType.VarChar, 50);
            cmd.Parameters.Add("@D6", SqlDbType.VarChar, 50);
            cmd.Parameters.Add("@D7", SqlDbType.VarChar, 50);
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
            cmd.Parameters.Add("@D18", SqlDbType.VarChar, 200);
            cmd.Parameters.Add("@D19", SqlDbType.VarChar, 200);
            cmd.Parameters.Add("@D20", SqlDbType.VarChar, 200);
            cmd.Parameters.Add("@D21", SqlDbType.VarChar, 200);
            cmd.Parameters.Add("@D22", SqlDbType.VarChar, 200);
            cmd.Parameters.Add("@D23", SqlDbType.VarChar, 200);
            cmd.Parameters.Add("@D24", SqlDbType.VarChar, 200);
            cmd.Parameters.Add("@D25", SqlDbType.VarChar, 200);
            cmd.Parameters.Add("@D26", SqlDbType.VarChar, 200);
            cmd.Parameters.Add("@D27", SqlDbType.VarChar, 50);
            cmd.Parameters.Add("@D28", SqlDbType.VarChar, 50);
            cmd.Parameters.Add("@D29", SqlDbType.VarChar, 50);
            cmd.Parameters.Add("@D30", SqlDbType.VarChar, 50);
            cmd.Parameters.Add("@D31", SqlDbType.VarChar, 200);
            cmd.Parameters.Add("@D32", SqlDbType.VarChar, 50);
            cmd.Parameters.Add("@D33", SqlDbType.VarChar, 50);
            cmd.Parameters.Add("@D34", SqlDbType.VarChar, 200);
            cmd.Parameters.Add("@D35", SqlDbType.VarChar, 200);
            cmd.Parameters.Add("@D36", SqlDbType.VarChar, 200);
            cmd.Parameters.Add("@D37", SqlDbType.VarChar, 200);
            cmd.Parameters.Add("@D38", SqlDbType.VarChar, 200);
            cmd.Parameters.Add("@D39", SqlDbType.VarChar, 200);
            cmd.Parameters.Add("@D40", SqlDbType.VarChar, 50);
            cmd.Parameters.Add("@D41", SqlDbType.VarChar, 50);
            cmd.Parameters.Add("@D42", SqlDbType.VarChar, 50);
            cmd.Parameters.Add("@D43", SqlDbType.VarChar, 50);
            cmd.Parameters.Add("@D44", SqlDbType.VarChar, 50);
            cmd.Parameters.Add("@D45", SqlDbType.VarChar, 50);
            cmd.Parameters.Add("@D46", SqlDbType.VarChar, 200);
            cmd.Parameters.Add("@D47", SqlDbType.VarChar, 200);

            cmd.Parameters["@D1"].Value = Convert.ToDateTime(MyMoMonthly.ReqDate, System.Globalization.CultureInfo.InvariantCulture);
            cmd.Parameters["@D2"].Value = MyMoMonthly.TotalAcc;
            cmd.Parameters["@D3"].Value = MyMoMonthly.TotalLoanAcc_Other;
            cmd.Parameters["@D4"].Value = MyMoMonthly.TotalLoanAcc_GSB;
            cmd.Parameters["@D5"].Value = MyMoMonthly.TotalCreditCard;
            cmd.Parameters["@D6"].Value = MyMoMonthly.TotalCreditCard_Other;
            cmd.Parameters["@D7"].Value = MyMoMonthly.TotalCreditCard_GSB;
            cmd.Parameters["@D8"].Value = MyMoMonthly.LoanCreditLimit;
            cmd.Parameters["@D9"].Value = MyMoMonthly.LoanCreditLimit_Other;
            cmd.Parameters["@D10"].Value = MyMoMonthly.LoanCreditLimit_GSB;
            cmd.Parameters["@D11"].Value = MyMoMonthly.CreditCardLimit_Other;
            cmd.Parameters["@D12"].Value = MyMoMonthly.CreditCardLimit_GSB;
            cmd.Parameters["@D13"].Value = MyMoMonthly.TotalOutstanding;
            cmd.Parameters["@D14"].Value = MyMoMonthly.TotalOutstanding_GSB;
            cmd.Parameters["@D15"].Value = MyMoMonthly.TotalOutstanding_Other;
            cmd.Parameters["@D16"].Value = MyMoMonthly.CardOutstanding_Other;
            cmd.Parameters["@D17"].Value = MyMoMonthly.CardOutstanding_GSB;
            cmd.Parameters["@D18"].Value = MyMoMonthly.TotalOverDue;
            cmd.Parameters["@D19"].Value = MyMoMonthly.TotalDept_Home;
            cmd.Parameters["@D20"].Value = MyMoMonthly.LITTIG_STAT;
            cmd.Parameters["@D21"].Value = MyMoMonthly.DeptPaytoNCB;
            cmd.Parameters["@D22"].Value = MyMoMonthly.MiniPayCard;
            cmd.Parameters["@D23"].Value = MyMoMonthly.MiniPayCard_GSB;
            cmd.Parameters["@D24"].Value = MyMoMonthly.OverlimitInt;
            cmd.Parameters["@D25"].Value = MyMoMonthly.NCB_TotalDept;
            cmd.Parameters["@D26"].Value = MyMoMonthly.NCB_TotalDept_GSB;
            cmd.Parameters["@D27"].Value = MyMoMonthly.Num_HomeLoan;
            cmd.Parameters["@D28"].Value = MyMoMonthly.DataStaus;
            cmd.Parameters["@D29"].Value = MyMoMonthly.ReDept_His;
            cmd.Parameters["@D30"].Value = MyMoMonthly.Overlimit_6mt_His;
            cmd.Parameters["@D31"].Value = MyMoMonthly.TotalOutstanding_NCB;
            cmd.Parameters["@D32"].Value = MyMoMonthly.BureauScore;
            cmd.Parameters["@D33"].Value = MyMoMonthly.OverlimitPercent;
            cmd.Parameters["@D34"].Value = MyMoMonthly.Overlimit_6mt;
            cmd.Parameters["@D35"].Value = MyMoMonthly.Overlimit_12mt;
            cmd.Parameters["@D36"].Value = MyMoMonthly.TotalDeptCard_Other;
            cmd.Parameters["@D37"].Value = MyMoMonthly.TotalDeptCard_GSB;
            cmd.Parameters["@D38"].Value = MyMoMonthly.TotalDeptLoan_Other;
            cmd.Parameters["@D39"].Value = MyMoMonthly.TotalDeptLoan_GSB;
            cmd.Parameters["@D40"].Value = MyMoMonthly.NCBCheck_1mt;
            cmd.Parameters["@D41"].Value = MyMoMonthly.NCBCheck_3mt;
            cmd.Parameters["@D42"].Value = MyMoMonthly.Num_AccLatePay;
            cmd.Parameters["@D43"].Value = MyMoMonthly.Num_LatePay_6mt;
            cmd.Parameters["@D44"].Value = MyMoMonthly.Num_LatePay_12mt;
            cmd.Parameters["@D45"].Value = MyMoMonthly.CreditScore;
            cmd.Parameters["@D46"].Value = MyMoMonthly.Score_Reason1;
            cmd.Parameters["@D47"].Value = MyMoMonthly.Score_Reason2;

            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();


        }
    }


}