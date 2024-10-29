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
using System.Data.SqlClient;
using System.IO;



namespace MyMoMonthly
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

            strFilenameRequest = "MyMoMonthly_" + yyyy.ToString("0000") + mm.ToString("00") + dd.ToString("00")  ;

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
                        temp.CBS_REASON_CD = items[81].Trim();
                        temp.CBS_APPROVE_COMMENT = items[82].Trim();
                        temp.APPRAMT = items[79].Trim();
                        temp.BRANCH_CD = items[80].Trim();
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


                    string tmesend = DateTime.Now.Year.ToString("0000") + DateTime.Now.ToString("-MM-dd HH:mm:ss.") + DateTime.Now.Millisecond.ToString();
                    string sqlStatementdx = "insert into s_log_import values('BATCH_MyMoMonthly',convert(datetime,'" + tmestart.ToString() + "',121),convert(datetime,'" + tmesend.ToString() + "',121)," + (allLines.Length - 2).ToString() + ",'DONE','" + localPath.ToString() + "','006')";
                    con.Open();
                    cmd = new SqlCommand(sqlStatementdx, con);
                    cmd.ExecuteNonQuery();
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
                string sqlStatementderr = "insert into s_log_import values('BATCH_MyMoMonthly',convert(datetime,'" + tmestart.ToString() + "',121),getdate(),0,'FAIL','" + localPath.ToString() + ":" + inExc.ToString().Replace("'", "''") + "','006')";
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

            string sqlStatement = "Select * from MYMO_LN_APP where APP_NO = '"+ appno + "'";

            cmd = new SqlCommand(sqlStatement,con);
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


            if (ConfigurationManager.AppSettings["RunWithTaskschedule"] == "1")
            {
                cmd.Parameters["@D2"].Value = DateTime.Now;

            }
            else
            {
                cmd.Parameters["@D2"].Value = monthCalendar1.SelectionStart;

            }


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


            if (MyMoMonthly.ReqDate == "")
            {
                cmd.Parameters["@D1"].Value = DBNull.Value;
            }
            else
            {
                cmd.Parameters["@D1"].Value = Convert.ToDateTime(MyMoMonthly.ReqDate);
            }

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

        private void Form1_Load(object sender, EventArgs e)
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
                    strFilenameRequest = "MyMoMonthly_" + yyyy.ToString("0000") + mm.ToString("00") + dd.ToString("00");
                //}


                ProcessNoFTP();
                this.Close();
            }
        }
    }
}
