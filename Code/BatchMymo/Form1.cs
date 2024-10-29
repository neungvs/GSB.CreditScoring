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
using System.Globalization;

namespace BatchMymo
{
    public partial class Form1 : Form
    {
        SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.AppSettings["FTPIP"]);

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
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

            

            string[] allLines = File.ReadAllLines(localPath);

            if (allLines.Length > 2)
            {
                string fileName = localPathOut;

                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                }

                

                using (StreamWriter sw = File.CreateText(fileName))
                {
                    

                    sw.WriteLine("Appno|Grade|GradeDescription|ModelVersion|Model|StatusCode|Message ");

                    

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


                            if (CheckValidateField(data.Cdob, data.Tworktime_month, data.Marital, data.Occcls_id, data.Occscls_id, data.Edustatus_id) == "001")
                            {
                                sw.WriteLine(data.Appno + "|" + "" + "|" + "" + "|" + "" + "|" + "" + "|" + "001" + "|" + "Incomplete Data" + " ");
                            }
                            else if (CheckValidateField(data.Cdob, data.Tworktime_month, data.Marital, data.Occcls_id, data.Occscls_id, data.Edustatus_id) == "002")
                            {
                                sw.WriteLine(data.Appno + "|" + "" + "|" + "" + "|" + "" + "|" + "" + "|" + "002" + "|" + "Incorrect Data Type" + " ");
                            }
                            else if (CheckValidateField(data.Cdob, data.Tworktime_month, data.Marital, data.Occcls_id, data.Occscls_id, data.Edustatus_id) == "000")
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
                                sw.WriteLine(doc.GetElementsByTagName("appno")[0].InnerText + "|" + docOut.GetElementsByTagName("Grade")[0].InnerText + "|" + docOut.GetElementsByTagName("GradeDescription")[0].InnerText + "|" + docOut.GetElementsByTagName("ModelVersion")[0].InnerText + "|" + docOut.GetElementsByTagName("Model")[0].InnerText + "|" + docOut.GetElementsByTagName("StatusCode")[0].InnerText + "|" + docOut.GetElementsByTagName("Message")[0].InnerText + " ");

                                string strOutputXML = strOutput;
                                string strRespondBlaze = ConvertOutputToResponseBlaze(strOutputXML);
                                string strRespondWebservice = ConvertOutputToResponseWebservice(strOutputXML);

                                SaveResponse_MymoRetail(data.Appno, DateTime.Now, doc.InnerXml.ToString(), strRespondBlaze, strRespondWebservice, docOut.InnerXml.ToString());
                            }
                        }
                        catch(Exception ex)
                        {
                            //MessageBox.Show(ex.ToString());
                            sw.WriteLine(items[0].Trim() + "|" + "" + "|" + "" + "|" + "" + "|" + "" + "|" + "999" + "|" + "Other" + " ");
                        }

                    }

                    sw.WriteLine("Total " + (allLines.Length - 2).ToString());
                }
            }

            /// FTP
            //using (Session session = new Session())
            //{
            //    session.Open(sessionOptions);

            //    String remotePath = ConfigurationManager.AppSettings["ftpPathMymo"].ToString() + strFilenameRequest + ".txt";
            //    String localPath = ConfigurationManager.AppSettings["ftpPathLocal"].ToString() + strFilenameRequest + ".txt";
            //    String localPathOut = ConfigurationManager.AppSettings["ftpPathLocalOut"].ToString() + strFilenameResult + ".txt";
            //    String remotePathOut = ConfigurationManager.AppSettings["ftpPathMymo"].ToString() + strFilenameResult + ".txt";

            //    session.GetFiles(remotePath, localPath).Check();

            //    string[] allLines = File.ReadAllLines(localPath);

            //    if (allLines.Length > 2)
            //    {
            //        string fileName = localPathOut;

            //        if (File.Exists(fileName))
            //        {
            //            File.Delete(fileName);
            //        }


            //        using (StreamWriter sw = File.CreateText(fileName))
            //        {
            //            sw.WriteLine("Appno|Grade|GradeDescription|GradeDescription|ModelVersion|Model|StatusCode|Message ");
                     


            //            for (int index = 1; index <= (allLines.Length - 2); index++)
            //            {
            //                string[] items = allLines[index].Split(new char[] { '|' });

            //                try
            //                {

            //                    Mymodata data = new Mymodata();

            //                    data.Appno = items[0].Trim();
            //                    data.Appdate = items[1].Trim();
            //                    data.Lnacctype = items[2].Trim();
            //                    data.Lntype = items[3].Trim();
            //                    data.Lnstype = items[4].Trim();
            //                    data.Mkcode = items[5].Trim();
            //                    data.Lnppose = items[6].Trim();
            //                    data.Cifno = items[7].Trim();
            //                    data.Cdob = items[8].Trim();
            //                    data.Gender = items[9].Trim();
            //                    data.Marital = items[10].Trim();
            //                    data.Occcls_id = items[11].Trim();
            //                    data.Occscls_id = items[12].Trim();
            //                    data.Edustatus_id = items[13].Trim();
            //                    data.Income = items[14].Trim();
            //                    data.Othincome = items[15].Trim();
            //                    data.IncomefromBusiness = items[16].Trim();
            //                    data.Child = items[17].Trim();
            //                    data.Mntexpense = items[18].Trim();
            //                    data.Lbtexpense = items[19].Trim();
            //                    data.Busexpense = items[20].Trim();
            //                    data.Resstatus_id = items[21].Trim();
            //                    data.Tworktime_month = items[22].Trim();
            //                    data.Paddrtime = items[23].Trim();
            //                    data.Restype = items[24].Trim();

            //                    if (CheckValidateField(data.Cdob, data.Tworktime_month,data.Marital,data.Occcls_id,data.Occscls_id,data.Edustatus_id) == "001")
            //                    {
            //                        sw.WriteLine(data.Appno + "|" + "" + "|" + "" + "|" + "" + "|" + "" + "|" + "001" + "|" + "Incomplete Data" + " ");
            //                    }
            //                    else if (CheckValidateField(data.Cdob, data.Tworktime_month, data.Marital, data.Occcls_id, data.Occscls_id, data.Edustatus_id) == "002")
            //                    {
            //                        sw.WriteLine(data.Appno + "|" + "" + "|" + "" + "|" + "" + "|" + "" + "|" + "002" + "|" + "Incorrect Data Type" + " ");
            //                    }
            //                    else if (CheckValidateField(data.Cdob, data.Tworktime_month, data.Marital, data.Occcls_id, data.Occscls_id, data.Edustatus_id) == "000")
            //                    {
            //                        doc.GetElementsByTagName("appno")[0].InnerText = data.Appno;
            //                        doc.GetElementsByTagName("appdate")[0].InnerText = data.Appdate;
            //                        doc.GetElementsByTagName("lnacctype")[0].InnerText = data.Lnacctype;
            //                        doc.GetElementsByTagName("lntype")[0].InnerText = data.Lntype;
            //                        doc.GetElementsByTagName("lnstype")[0].InnerText = data.Lnstype;
            //                        doc.GetElementsByTagName("mkcode")[0].InnerText = data.Mkcode;
            //                        doc.GetElementsByTagName("lnppose")[0].InnerText = data.Lnppose;
            //                        doc.GetElementsByTagName("cifno")[0].InnerText = data.Cifno;
            //                        doc.GetElementsByTagName("cdob")[0].InnerText = data.Cdob;
            //                        doc.GetElementsByTagName("gender")[0].InnerText = data.Gender;
            //                        doc.GetElementsByTagName("marital")[0].InnerText = data.Marital;
            //                        doc.GetElementsByTagName("occcls_id")[0].InnerText = data.Occcls_id;
            //                        doc.GetElementsByTagName("occscls_id")[0].InnerText = data.Occscls_id;
            //                        doc.GetElementsByTagName("edustatus_id")[0].InnerText = data.Edustatus_id;
            //                        doc.GetElementsByTagName("income")[0].InnerText = data.Income;
            //                        doc.GetElementsByTagName("othincome")[0].InnerText = data.Othincome;
            //                        doc.GetElementsByTagName("child")[0].InnerText = data.Child;
            //                        doc.GetElementsByTagName("mntexpense")[0].InnerText = data.Mntexpense;
            //                        doc.GetElementsByTagName("lbtexpense")[0].InnerText = data.Lbtexpense;
            //                        doc.GetElementsByTagName("busexpense")[0].InnerText = data.Busexpense;
            //                        doc.GetElementsByTagName("resstatus_id")[0].InnerText = data.Resstatus_id;
            //                        doc.GetElementsByTagName("tworktime_month")[0].InnerText = data.Tworktime_month;
            //                        doc.GetElementsByTagName("paddrtime")[0].InnerText = data.Paddrtime;
            //                        doc.GetElementsByTagName("restype")[0].InnerText = data.Restype;
            //                        doc.GetElementsByTagName("incomefromBusiness")[0].InnerText = data.IncomefromBusiness;

            //                        string strRequestBlaze = doc.InnerXml.ToString();

            //                        RetailBlaze.RetailServiceImplService proxy = new RetailBlaze.RetailServiceImplService();

            //                        textBox1.Text = doc.InnerXml;

            //                        string strOutput = proxy.getRetailScoringDecision(doc.InnerXml);
            //                        XmlDocument docOut = new XmlDocument();
            //                        docOut.LoadXml(strOutput);

            //                        sw.WriteLine(doc.GetElementsByTagName("appno")[0].InnerText + "|" + docOut.GetElementsByTagName("Grade")[0].InnerText + "|" + docOut.GetElementsByTagName("GradeDescription")[0].InnerText + "|" + docOut.GetElementsByTagName("ModelVersion")[0].InnerText + "|" + docOut.GetElementsByTagName("Model")[0].InnerText + "|" + docOut.GetElementsByTagName("StatusCode")[0].InnerText + "|" + docOut.GetElementsByTagName("Message")[0].InnerText + " ");

            //                        string strOutputXML = strOutput;
            //                        string strRespondBlaze = ConvertOutputToResponseBlaze(strOutputXML);
            //                        string strRespondWebservice = ConvertOutputToResponseWebservice(strOutputXML);

            //                        SaveResponse_MymoRetail(data.Appno, DateTime.Now, doc.InnerXml.ToString(), strRespondBlaze, strRespondWebservice, docOut.InnerXml.ToString());
            //                    }
            //                }
            //                catch
            //                {
            //                    sw.WriteLine(items[0].Trim() + "|" + "" + "|" + "" + "|" + "" + "|" + "" + "|" + "999" + "|" + "Other" + " ");
            //                }

            //            }

            //            sw.WriteLine("Total "+ (allLines.Length-2).ToString());
            //        }
            //    }

            //    session.PutFiles(localPathOut, remotePathOut).Check();
            //}

            MessageBox.Show("Finish");
        }


        public void SaveResponse_MymoRetail(string appno, DateTime transactionDate, string serviceRequest_Update, string blazeResponse, string serviceResponse, string serviceRequest)
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

                cmd.CommandText = "INSERT INTO MymoRetailDecisionReqRes(appno,transactiondate,request,response_Blaze,response_WebService,response_LOR) Values (@appno,@transactionDate,@request,@response_Blaze,@response_WebService,@response_LOR)";



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

        public string CheckValidateField(string cdob,string Tworktime_month, string Marital, string Occcls_id, string Occscls_id, string Edustatus_id)
        {
            // "000" : Validatepass
            // "001" : Validatenull
            // "000" : ValidateFormat

            if ((cdob == "") || (Tworktime_month == "") || (Marital == "") || (Occcls_id == "") || (Occscls_id == "") || (Edustatus_id == ""))
            {
                return "001";
            }

            if (!Information.IsNumeric(Tworktime_month))
            {
                return "002";
            }

            if (!IsValidDateFormat("yyyy-MM-dd", cdob))
            {
                return "002";
            }
               
            return "000";
        }

        public bool IsValidDateFormat(string dateFormat,string cdob)
        {
            try
            {
                DateTime fromDateValue;
                var formats = new[] {"yyyy-MM-dd" };
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

        }
    }
}
