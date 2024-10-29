using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinSCP;
using System.Configuration;
using System.IO;
using System.Data.SqlClient;
using System.Xml;
using Microsoft.VisualBasic;
using System.Text.RegularExpressions;
using System.Globalization;

namespace CSMMyMo
{
    public partial class Form1 : Form
    {
        SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.AppSettings["FTPIP"]);

        public Form1()
        {
            InitializeComponent();
        }

        private void ProcessNoFTP()
        {
            int maxseq = CheckmaxSEQ();
            maxseq++;
            //int maxseq = 1;



            //maxseq++;

            string tmestart = DateTime.Now.Year.ToString("0000") + DateTime.Now.ToString("-MM-dd HH:mm:ss.") + DateTime.Now.Millisecond.ToString();

            SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["FTPIP"].ToString());
            SqlCommand cmddlx;
            string d = monthCalendar1.SelectionStart.ToString();


            string empty1 = string.Empty;
            StreamReader streamReader = new StreamReader(@"D:/Webapp/ScoringSME/Batch/xmlTemplate.xml");
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

            DateTime dateTransaction = monthCalendar1.SelectionStart.AddDays(-1);

            int trdd = dateTransaction.Day;
            int trmm = dateTransaction.Month;
            int tryyyy = dateTransaction.Year;

            if (tryyyy > 2500) tryyyy = tryyyy - 543;


            int dd = Convert.ToDateTime(d).Day;
            int mm = Convert.ToDateTime(d).Month;
            int yyyy = Convert.ToDateTime(d).Year;

            string strFilenameRequest = "AscoreRequest" + yyyy.ToString("0000") + mm.ToString("00") + dd.ToString("00");
            string strFilenameResult = "AscoreResult" + yyyy.ToString("0000") + mm.ToString("00") + dd.ToString("00");

            SessionOptions sessionOptions = new SessionOptions();
            sessionOptions.Protocol = Protocol.Sftp;
            sessionOptions.HostName = ConfigurationManager.AppSettings["ftphost"].ToString();
            sessionOptions.UserName = ConfigurationManager.AppSettings["ftpuser"].ToString();
            sessionOptions.Password = ConfigurationManager.AppSettings["ftppassword"].ToString();
            sessionOptions.SshHostKey = ConfigurationManager.AppSettings["ftphostkey"].ToString();
            sessionOptions.SshPrivateKey = ConfigurationManager.AppSettings["ftpprikey"].ToString();


            ////NO FTP
            ///
            String localPath = ConfigurationManager.AppSettings["ftpPathLocal"].ToString() + strFilenameRequest + ".txt";
            String localPathOut = ConfigurationManager.AppSettings["ftpPathLocalOut"].ToString() + strFilenameResult + ".txt";

            try
            {
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

                                Mymodata data = new Mymodata();

                                data.Appno = items[0].Trim();
                                data.Appdate = items[1].Trim();
                                data.Lnacctype = items[2].Trim();
                                data.Lntype = items[3].Trim();
                                data.Lnstype = items[4].Trim();
                                data.Mkcode = items[5].Trim();
                                data.Lnppose = items[6].Trim();
                                data.Cifno = items[7].Trim();
                                data.Cdob = items[8].Trim();
                                data.Gender = items[9].Trim();
                                data.Marital = items[10].Trim();
                                data.Occcls_id = items[11].Trim();
                                data.Occscls_id = items[12].Trim();
                                data.Edustatus_id = items[13].Trim();
                                data.Income = items[14].Trim();
                                data.Othincome = items[15].Trim();
                                data.IncomefromBusiness = items[16].Trim();
                                data.Child = items[17].Trim();
                                data.Mntexpense = items[18].Trim();
                                data.Lbtexpense = items[19].Trim();
                                data.Busexpense = items[20].Trim();
                                data.Resstatus_id = items[21].Trim();
                                data.Tworktime_month = items[22].Trim();
                                data.Paddrtime = items[23].Trim();
                                data.Restype = items[24].Trim();

                                if (IsValidDateFormat("yyyy/MM/dd", data.Cdob))
                                {
                                    data.Cdob = data.Cdob.Replace("/", "-");
                                }

                                string[] errorcode = CheckValidateField(data.Cdob, data.Tworktime_month, data.Marital, data.Occcls_id, data.Occscls_id, data.Edustatus_id).Split(new char[] { ':' });

                                if (errorcode[0] == "001")
                                {
                                    sw.WriteLine(data.Appno + "|" + "" + "|" + "" + "|" + "" + "|" + "" + "|" + "001" + "|" + "Incomplete Data:" + errorcode[1] + "");
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
                                    doc.GetElementsByTagName("cifno")[0].InnerText = data.Cifno;
                                    doc.GetElementsByTagName("cdob")[0].InnerText = data.Cdob;
                                    doc.GetElementsByTagName("gender")[0].InnerText = data.Gender;
                                    doc.GetElementsByTagName("marital")[0].InnerText = data.Marital;
                                    doc.GetElementsByTagName("occcls_id")[0].InnerText = data.Occcls_id;
                                    doc.GetElementsByTagName("occscls_id")[0].InnerText = data.Occscls_id;
                                    doc.GetElementsByTagName("edustatus_id")[0].InnerText = data.Edustatus_id;
                                    doc.GetElementsByTagName("income")[0].InnerText = data.Income;
                                    doc.GetElementsByTagName("othincome")[0].InnerText = data.Othincome;
                                    doc.GetElementsByTagName("child")[0].InnerText = data.Child;
                                    doc.GetElementsByTagName("mntexpense")[0].InnerText = data.Mntexpense;
                                    doc.GetElementsByTagName("lbtexpense")[0].InnerText = data.Lbtexpense;
                                    doc.GetElementsByTagName("busexpense")[0].InnerText = data.Busexpense;
                                    doc.GetElementsByTagName("resstatus_id")[0].InnerText = data.Resstatus_id;
                                    doc.GetElementsByTagName("tworktime_month")[0].InnerText = data.Tworktime_month;
                                    doc.GetElementsByTagName("paddrtime")[0].InnerText = data.Paddrtime;
                                    doc.GetElementsByTagName("restype")[0].InnerText = data.Restype;
                                    doc.GetElementsByTagName("incomefromBusiness")[0].InnerText = data.IncomefromBusiness;

                                    string strRequestBlaze = doc.InnerXml.ToString();

                                    RetailBlaze.RetailServiceImplService proxy = new RetailBlaze.RetailServiceImplService();

                                    textBox1.Text = doc.InnerXml;

                                    //MessageBox.Show(doc.InnerXml);


                                    string strOutput = proxy.getRetailScoringDecision(doc.InnerXml);
                                    XmlDocument docOut = new XmlDocument();
                                    docOut.LoadXml(strOutput);

                                    if (docOut.GetElementsByTagName("StatusCode")[0].InnerText == "200")
                                    {
                                        sw.WriteLine(items[0].Trim() + "|" + "" + "|" + "" + "|" + "" + "|" + "" + "|" + "200" + "|" + "Score not required");
                                    }
                                    else
                                    {
                                        sw.WriteLine(doc.GetElementsByTagName("appno")[0].InnerText + "|" + docOut.GetElementsByTagName("Grade")[0].InnerText + "|" + docOut.GetElementsByTagName("GradeDescription")[0].InnerText + "|" + docOut.GetElementsByTagName("ModelVersion")[0].InnerText + "|" + docOut.GetElementsByTagName("Model")[0].InnerText + "|" + docOut.GetElementsByTagName("StatusCode")[0].InnerText + "|" + docOut.GetElementsByTagName("Message")[0].InnerText + "");

                                        string strOutputXML = strOutput;
                                        string strRespondBlaze = ConvertOutputToResponseBlaze(strOutputXML);
                                        string strRespondWebservice = ConvertOutputToResponseWebservice(strOutputXML);

                                        SaveResponse_MymoRetail(data.Appno, DateTime.Now, doc.InnerXml.ToString(), strRespondBlaze, strRespondWebservice, docOut.InnerXml.ToString(), maxseq);
                                    }

                                }
                            }
                            catch (Exception ex)
                            {
                                //MessageBox.Show(ex.ToString());
                                sw.WriteLine(items[0].Trim() + "|" + "" + "|" + "" + "|" + "" + "|" + "" + "|" + "999" + "|" + "Other+" + ":"+ex.ToString());
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

                string tmesend = DateTime.Now.Year.ToString("0000") + DateTime.Now.ToString("-MM-dd HH:mm:ss.") + DateTime.Now.Millisecond.ToString();
                string sqlStatementdx = "insert into s_log_import values('BATCH_CSMMymo',convert(datetime,'" + tmestart.ToString() + "',121),convert(datetime,'" + tmesend.ToString() + "',121),"+ (allLines.Length - 2).ToString() + ",'DONE','" + localPath.ToString() + "','004')";
                con.Open();
                cmddlx = new SqlCommand(sqlStatementdx, con);
                cmddlx.ExecuteNonQuery();
                con.Close();

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: {0}", ex);
                //          return 1;
                //     return "False";
                string inExc = ex.Message;
                SqlConnection errcon = new SqlConnection(ConfigurationManager.AppSettings["FTPIP"].ToString());
                string sqlStatementderr = "insert into s_log_import values('BATCH_CSMMymo',convert(datetime,'" + tmestart.ToString() + "',121),getdate(),0,'FAIL','" + localPath.ToString() + ":" + inExc.ToString().Replace("'", "''") + "','004')";
                errcon.Open();
                SqlCommand cmddlerr = new SqlCommand(sqlStatementderr, errcon);
                cmddlerr.ExecuteNonQuery();
                errcon.Close();
            }

        }

        private void ProcessWithFTP()
        {
            int maxseq = CheckmaxSEQ();

            maxseq++;

            string tmestart = DateTime.Now.Year.ToString("0000") + DateTime.Now.ToString("-MM-dd HH:mm:ss.") + DateTime.Now.Millisecond.ToString();

            SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["FTPIP"].ToString());
            SqlCommand cmddlx;
            string d = monthCalendar1.SelectionStart.ToString();


            string empty1 = string.Empty;
            StreamReader streamReader = new StreamReader(@"D:/Webapp/ScoringSME/Batch/xmlTemplate.xml");
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

            DateTime dateTransaction = monthCalendar1.SelectionStart.AddDays(-1);

            int trdd = dateTransaction.Day;
            int trmm = dateTransaction.Month;
            int tryyyy = dateTransaction.Year;

            if (tryyyy > 2500) tryyyy = tryyyy - 543;


            int dd = Convert.ToDateTime(d).Day;
            int mm = Convert.ToDateTime(d).Month;
            int yyyy = Convert.ToDateTime(d).Year;

            string strFilenameRequest = "AscoreRequest" + yyyy.ToString("0000") + mm.ToString("00") + dd.ToString("00");
            string strFilenameResult = "AscoreResult" + yyyy.ToString("0000") + mm.ToString("00") + dd.ToString("00");

            SessionOptions sessionOptions = new SessionOptions();
            sessionOptions.Protocol = Protocol.Sftp;
            sessionOptions.HostName = ConfigurationManager.AppSettings["ftphost"].ToString();
            sessionOptions.UserName = ConfigurationManager.AppSettings["ftpuser"].ToString();
            sessionOptions.Password = ConfigurationManager.AppSettings["ftppassword"].ToString();
            sessionOptions.SshHostKey = ConfigurationManager.AppSettings["ftphostkey"].ToString();
            sessionOptions.SshPrivateKey = ConfigurationManager.AppSettings["ftpprikey"].ToString();

            String remotePath = ConfigurationManager.AppSettings["ftpPathMymo"].ToString() + strFilenameRequest + ".txt";
            String localPath = ConfigurationManager.AppSettings["ftpPathLocal"].ToString() + strFilenameRequest + ".txt";
            String localPathOut = ConfigurationManager.AppSettings["ftpPathLocalOut"].ToString() + strFilenameResult + ".txt";
            String remotePathOut = ConfigurationManager.AppSettings["ftpPathMymo"].ToString() + strFilenameResult + ".txt";

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

                                        Mymodata data = new Mymodata();

                                        data.Appno = items[0].Trim();
                                        data.Appdate = items[1].Trim();
                                        data.Lnacctype = items[2].Trim();
                                        data.Lntype = items[3].Trim();
                                        data.Lnstype = items[4].Trim();
                                        data.Mkcode = items[5].Trim();
                                        data.Lnppose = items[6].Trim();
                                        data.Cifno = items[7].Trim();
                                        data.Cdob = items[8].Trim();
                                        data.Gender = items[9].Trim();
                                        data.Marital = items[10].Trim();
                                        data.Occcls_id = items[11].Trim();
                                        data.Occscls_id = items[12].Trim();
                                        data.Edustatus_id = items[13].Trim();
                                        data.Income = items[14].Trim();
                                        data.Othincome = items[15].Trim();
                                        data.IncomefromBusiness = items[16].Trim();
                                        data.Child = items[17].Trim();
                                        data.Mntexpense = items[18].Trim();
                                        data.Lbtexpense = items[19].Trim();
                                        data.Busexpense = items[20].Trim();
                                        data.Resstatus_id = items[21].Trim();
                                        data.Tworktime_month = items[22].Trim();
                                        data.Paddrtime = items[23].Trim();
                                        data.Restype = items[24].Trim();

                                        string[] errorcode = CheckValidateField(data.Cdob, data.Tworktime_month, data.Marital, data.Occcls_id, data.Occscls_id, data.Edustatus_id).Split(new char[] { ':' });

                                        if (errorcode[0] == "001")
                                        {
                                            sw.WriteLine(data.Appno + "|" + "" + "|" + "" + "|" + "" + "|" + "" + "|" + "001" + "|" + "Incomplete Data:" + errorcode[1] + "");
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
                                            doc.GetElementsByTagName("cifno")[0].InnerText = data.Cifno;
                                            doc.GetElementsByTagName("cdob")[0].InnerText = data.Cdob;
                                            doc.GetElementsByTagName("gender")[0].InnerText = data.Gender;
                                            doc.GetElementsByTagName("marital")[0].InnerText = data.Marital;
                                            doc.GetElementsByTagName("occcls_id")[0].InnerText = data.Occcls_id;
                                            doc.GetElementsByTagName("occscls_id")[0].InnerText = data.Occscls_id;
                                            doc.GetElementsByTagName("edustatus_id")[0].InnerText = data.Edustatus_id;
                                            doc.GetElementsByTagName("income")[0].InnerText = data.Income;
                                            doc.GetElementsByTagName("othincome")[0].InnerText = data.Othincome;
                                            doc.GetElementsByTagName("child")[0].InnerText = data.Child;
                                            doc.GetElementsByTagName("mntexpense")[0].InnerText = data.Mntexpense;
                                            doc.GetElementsByTagName("lbtexpense")[0].InnerText = data.Lbtexpense;
                                            doc.GetElementsByTagName("busexpense")[0].InnerText = data.Busexpense;
                                            doc.GetElementsByTagName("resstatus_id")[0].InnerText = data.Resstatus_id;
                                            doc.GetElementsByTagName("tworktime_month")[0].InnerText = data.Tworktime_month;
                                            doc.GetElementsByTagName("paddrtime")[0].InnerText = data.Paddrtime;
                                            doc.GetElementsByTagName("restype")[0].InnerText = data.Restype;
                                            doc.GetElementsByTagName("incomefromBusiness")[0].InnerText = data.IncomefromBusiness;

                                            string strRequestBlaze = doc.InnerXml.ToString();

                                            RetailBlaze.RetailServiceImplService proxy = new RetailBlaze.RetailServiceImplService();

                                            textBox1.Text = doc.InnerXml;

                                            string strOutput = proxy.getRetailScoringDecision(doc.InnerXml);
                                            XmlDocument docOut = new XmlDocument();
                                            docOut.LoadXml(strOutput);

                                            sw.WriteLine(doc.GetElementsByTagName("appno")[0].InnerText + "|" + docOut.GetElementsByTagName("Grade")[0].InnerText + "|" + docOut.GetElementsByTagName("GradeDescription")[0].InnerText + "|" + docOut.GetElementsByTagName("ModelVersion")[0].InnerText + "|" + docOut.GetElementsByTagName("Model")[0].InnerText + "|" + docOut.GetElementsByTagName("StatusCode")[0].InnerText + "|" + docOut.GetElementsByTagName("Message")[0].InnerText + "");

                                            string strOutputXML = strOutput;
                                            string strRespondBlaze = ConvertOutputToResponseBlaze(strOutputXML);
                                            string strRespondWebservice = ConvertOutputToResponseWebservice(strOutputXML);

                                            SaveResponse_MymoRetail(data.Appno, DateTime.Now, doc.InnerXml.ToString(), strRespondBlaze, strRespondWebservice, docOut.InnerXml.ToString(), maxseq);


                                        }
                                    }
                                    catch
                                    {
                                        sw.WriteLine(items[0].Trim() + "|" + "" + "|" + "" + "|" + "" + "|" + "" + "|" + "999" + "|" + "Other" + "");
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
                    string sqlStatementdx = "insert into s_log_import values('BATCH_CSMMymo',convert(datetime,'" + tmestart.ToString() + "',121),convert(datetime,'" + tmesend.ToString() + "',121)," + (allLines.Length - 2).ToString() + ",'DONE','" + localPath.ToString() + "','004')";
                    con.Open();
                    cmddlx = new SqlCommand(sqlStatementdx, con);
                    cmddlx.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error: {0}", ex);
                //          return 1;
                //     return "False";
                string inExc = ex.Message;
                SqlConnection errcon = new SqlConnection(ConfigurationManager.AppSettings["FTPIP"].ToString());
                string sqlStatementderr = "insert into s_log_import values('BATCH_CSMMymo',convert(datetime,'" + tmestart.ToString() + "',121),getdate(),0,'FAIL','" + localPath.ToString() + ":" + inExc.ToString().Replace("'", "''") + "','004')";
                errcon.Open();
                SqlCommand cmddlerr = new SqlCommand(sqlStatementderr, errcon);
                cmddlerr.ExecuteNonQuery();
                errcon.Close();
            }



        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (ConfigurationManager.AppSettings["RunWithFTP"] == "1")
            {
                ProcessWithFTP();
            }
            else
            {
                ProcessNoFTP();
            }

            MessageBox.Show("Finish");
        }


        public void SaveResponse_MymoRetail(string appno, DateTime transactionDate, string serviceRequest_Update, string blazeResponse, string serviceResponse, string serviceRequest,int maxseq)
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

                cmd.CommandText = "INSERT INTO MymoRetailDecisionReqRes(appno,transactiondate,request,response_Blaze,response_WebService,response_MYMO,SEQ) Values (@appno,@transactionDate,@request,@response_Blaze,@response_WebService,@response_LOR,@maxSEQ)";



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
                cmd.Parameters.AddWithValue("@maxSEQ", maxseq);


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

        public string CheckValidateField(string cdob, string Tworktime_month, string Marital, string Occcls_id, string Occscls_id, string Edustatus_id)
        {
            // "000" : Validatepass
            // "001" : Validatenull
            // "000" : ValidateFormat

            if (cdob == "")
            {
                return "001:cdob";
            }
            else if (Tworktime_month == "")
            {
                return "001:Tworktime_month";
            }
            else if (Marital == "")
            {
                return "001:Marital";
            }
            else if (Occcls_id == "")
            {
                return "001:Occcls_id";
            }
            else if (Occscls_id == "")
            {
                return "001:Occscls_id";
            }
            else if (Edustatus_id == "")
            {
                return "001:Edustatus_id";
            }


            if (!IsValidDateFormat("yyyy-MM-dd", cdob))
            {
                return "002:cdob";
            }

            if (hasSpecialChar(Tworktime_month))
            {
                return "002:Tworktime_month";
            }
            if (!Information.IsNumeric(Tworktime_month))
            {
                return "002:Tworktime_month";
            }

            if (hasSpecialChar(Marital))
            {
                return "002:Marital";
            }

            if (!CheckMartial(Marital))
            {
                return "002:Marital";
            }

            if (hasSpecialChar(Occcls_id))
            {
                return "002:Occcls_id";
            }

            if (!Information.IsNumeric(Occcls_id))
            {
                return "002:Occcls_id";
            }

            if (hasSpecialChar(Occscls_id))
            {
                return "002:Occscls_id";
            }

            if (!Information.IsNumeric(Occscls_id))
            {
                return "002:Occscls_id";
            }

            if (hasSpecialChar(Edustatus_id))
            {
                return "002:Edustatus_id";
            }

            if (!CheckEducation(Edustatus_id))
            {
                return "002:Edustatus_id";
            }



            return "000:";
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

        private void Form1_Load(object sender, EventArgs e)
        {


            if (ConfigurationManager.AppSettings["RunWithTaskschedule"] == "1")
            {

                if (ConfigurationManager.AppSettings["RunWithFTP"] == "1")
                {
                    ProcessWithFTP();
                }
                else
                {
                    ProcessNoFTP();
                }

                this.Close();
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

                cmd.CommandText = "select * from [dbo].[ST_MARITAL] where [MARITAL_CD] = '"+ strMartial + "'";
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

        private bool CheckOccs(string Occcls_id,string Occscls_id)
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

                cmd.CommandText = "select * from [dbo].[ST_OCCSCLS] where [OCCSCLS_CD] = '" + Occscls_id + "' and [OCCCLS_CD] = '"+ Occcls_id + "' ";
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
    }
}
