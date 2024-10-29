using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using CrystalDecisions.Shared;
using CrystalDecisions.Web;
using CrystalDecisions.CrystalReports.Engine;
using System.Collections.Generic;


namespace GSB.Portfolio.Web.Loan
{
    public partial class frmPreviewLoanRpt : System.Web.UI.Page
    {

        /* Get Product Information */
        public string genPrintDate()
        {

            int tempDay;
            int tempMonth;
            int tempYear;

            //Set print date time            
            tempDay = System.DateTime.Now.Day + 100;
            tempMonth = System.DateTime.Now.Month + 100;
            if (System.DateTime.Now.Year < 2400)
                tempYear = System.DateTime.Now.Year + 543;
            else
                tempYear = System.DateTime.Now.Year;


            return tempDay.ToString().Substring(1) + " " + convertMonthToText("TH", int.Parse(tempMonth.ToString().Substring(1))) + " " + tempYear.ToString();

        }

        public string genPrintTime()
        {
            return System.DateTime.Now.ToString("HH:mm");
        }

        /// <summary>
        /// To convert month in number to text
        /// </summary>
        /// <param name="strLang"></param> EN=English, TH=Thai
        /// <param name="intMonth"></param>
        /// <returns></returns>
        public string convertMonthToText(string strLang, int intMonth)
        {
            string strMonth = "";

            if (strLang == "TH")
            {
                if (intMonth == 1)
                {
                    strMonth = "มกราคม";
                }
                else if (intMonth == 2)
                {
                    strMonth = "กุมภาพันธ์";
                }
                else if (intMonth == 3)
                {
                    strMonth = "มีนาคม";
                }
                else if (intMonth == 4)
                {
                    strMonth = "เมษายน";
                }
                else if (intMonth == 5)
                {
                    strMonth = "พฤษภาคม";
                }
                else if (intMonth == 6)
                {
                    strMonth = "มิถุนายน";
                }
                else if (intMonth == 7)
                {
                    strMonth = "กรกฎาคม";
                }
                else if (intMonth == 8)
                {
                    strMonth = "สิงหาคม";
                }
                else if (intMonth == 9)
                {
                    strMonth = "กันยายน";
                }
                else if (intMonth == 10)
                {
                    strMonth = "ตุลาคม";
                }
                else if (intMonth == 11)
                {
                    strMonth = "พฤศจิกายน";
                }
                else if (intMonth == 12)
                {
                    strMonth = "ธันวาคม";
                }
            }

            return strMonth;
        }

        /// <summary>
        /// Page initialization
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Init(object sender, EventArgs e)
        {
            load_report();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        private void load_report()
        {
            string strPath = MapPath(".//");
            string _strFileName = string.Empty;
            if(Request.QueryString["ReportId"].ToString()=="1")
            {
                //Population Customer Report
                _strFileName = strPath + "\\LoanDetailReport.rpt";
                LoadReportFF1(_strFileName);
            }
            else if(Request.QueryString["ReportId"].ToString() == "2")
            {
                //Population Analysis Report
                _strFileName = strPath + "\\LoanDetailAnalysisReport.rpt";
                LoadReportFF1(_strFileName);
            }
            else if (Request.QueryString["ReportId"].ToString() == "3")
            {
                //Population Analysis Report
                _strFileName = strPath + "\\LoanDetailMymoReport.rpt";
                LoadReportFF2(_strFileName);
            }
            else if (Request.QueryString["ReportId"].ToString() == "4")
            {
                //Population Analysis Report
                _strFileName = strPath + "\\LoanDetailAnalysisMymoReport.rpt";
                LoadReportFF2(_strFileName);
            }
        }

        //Population Stabilities Report
        private void LoadReportFF1(string strFileName)
        {

            string strAPP_NO = "";

            //Create DataSet to contain data for Application
            DataTable dt = ((DataTable)(Session["dtApplication"]));
            GSB.Loan.ReportLayOut.LoanDetailDataSet ds = new GSB.Loan.ReportLayOut.LoanDetailDataSet();                      

            DataRow[] dr = dt.Select();
            foreach (DataRow drC in dr)
            {
                DataRow drInsert = ds.Tables["dtApplicationInfomation"].NewRow();

                drInsert["APP_NO"] = drC["APP_NO"];
                strAPP_NO = drC["APP_NO"].ToString() ;
                drInsert["MODEL_NAME"] = drC["MODEL_NAME"];

                string[] strAPP_DATE = DateTime.Parse(drC["APP_DATE"].ToString()).ToString("dd/MM/yyyy").Split('/');
                string strApplicationDAte = "";
                if (strAPP_DATE.Length == 3)
                {
                    int intApplicationYear = int.Parse(strAPP_DATE[2]);
                    if (intApplicationYear < 2500) intApplicationYear += 543;
                    strApplicationDAte = int.Parse(strAPP_DATE[0]).ToString() + " " + convertMonthToText("TH", int.Parse(strAPP_DATE[1])) + " " + intApplicationYear.ToString();
                }
                drInsert["APP_DATE"] = strApplicationDAte;

                drInsert["LOAN_NAME"] = drC["LOAN_NAME"];
                drInsert["CREATION_BY"] = drC["CREATION_BY"];
                drInsert["STYPE_NAME"] = drC["STYPE_NAME"];
                drInsert["BRANCH_NAME"] = drC["BRANCH_NAME"];
                drInsert["MARKET_NAME"] = drC["MARKET_NAME"];
                drInsert["LOAN_AMOUNT"] = drC["LOAN_AMOUNT"];
                drInsert["PURPOSE_NAME"] = drC["PURPOSE_NAME"];
                drInsert["LNCOLLVAL"] = drC["LNCOLLVAL"];
                drInsert["CONSUMPTION_NAME"] = drC["CONSUMPTION_NAME"];

                drInsert["ISIC1_NAME"] = drC["ISIC1_NAME"];
                drInsert["GSBPURPOSE_NAME"] = drC["GSBPURPOSE_NAME"];
                drInsert["ISIC2_NAME"] = drC["ISIC2_NAME"];
                drInsert["SCORE"] = drC["SCORE"];
                drInsert["ISIC3_NAME"] = drC["ISIC3_NAME"];
                drInsert["GRADE"] = drC["GRADE"];
                drInsert["LOAN_TERM"] = drC["LOAN_TERM"];
                drInsert["LNSTATUS"] = drC["LNSTATUS"];

                drInsert["MPAYAMT"] = drC["MPAYAMT"];
                drInsert["APPRAMT"] = drC["APPRAMT"];
                drInsert["LOAN_NUM"] = drC["LOAN_NUM"];
                drInsert["APPRTERM"] = drC["APPRTERM"];

                drInsert["CA_Date"] = drC["CA_Date"];
                drInsert["CA_REASON"] = drC["CA_REASON"];
                drInsert["CBS_COL"] = drC["CBS_COL"];

                ds.Tables["dtApplicationInfomation"].Rows.Add(drInsert);
            }
            //-------------------------------------------------------------------


            //Create DataSet to contain data for Loaner
            DataTable dtLoaner = ((DataTable)(Session["dtLoaner"]));

            DataRow[] drLoaner = dtLoaner.Select();
            int intRowID = 1;
            foreach (DataRow drC in drLoaner)
            {
                DataRow drInsert = ds.Tables["dtLoanerInformation"].NewRow();

                drInsert["APP_NO"] = strAPP_NO;
                drInsert["ROW"] = drC["ROW"];
                drInsert["LOANERTYPE"] = drC["ROW"].ToString().Replace("ข้อมูล","");
                drInsert["ROWID"] = intRowID;
                intRowID++;
                drInsert["CBS_CIFNO"] = drC["CBS_CIFNO"];
                drInsert["CBS_CIFNAME"] = drC["CBS_CIFNAME"];
                drInsert["CBS_GENDER_CD"] = drC["CBS_GENDER_CD"];
                
                string[] DOB = DateTime.Parse(drC["CBS_CDOB"].ToString()).ToString("dd/MM/yyyy").Split('/');
                string DateOfBirth = "";
                if (DOB.Length == 3)
                {
                    int intDOBYear = int.Parse(DOB[2]);
                    if (intDOBYear < 2500) intDOBYear += 543;
                    DateOfBirth = int.Parse(DOB[0]).ToString() + " " + convertMonthToText("TH", int.Parse(DOB[1])) + " " + intDOBYear.ToString();
                }
                drInsert["CBS_CDOB"] = DateOfBirth;

                drInsert["AGE"] = drC["AGE"];
                drInsert["CBS_CITIZID"] = drC["CBS_CITIZID"];
                drInsert["PASSID"] = drC["PASSID"];
                drInsert["CBS_EDU_CD"] = drC["CBS_EDU_CD"];
                drInsert["CBS_RESSTATUS_CD"] = drC["CBS_RESSTATUS_CD"];
                drInsert["CBS_PADDRTIME"] = drC["CBS_PADDRTIME"];
                drInsert["CBS_RESTYPE_CD"] = drC["CBS_RESTYPE_CD"];
                drInsert["CBS_MARITAL_CD"] = drC["CBS_MARITAL_CD"];
                drInsert["CBS_CHILD"] = drC["CBS_CHILD"];
                drInsert["CBS_WCHILD"] = drC["CBS_WCHILD"];
                drInsert["CBS_CTYPE"] = drC["CBS_CTYPE"];
                drInsert["CBS_CCODE"] = drC["CBS_CCODE"];
                drInsert["COBORREL"] = drC["COBORREL"] == DBNull.Value ? "-" : drC["COBORREL"];

                drInsert["GUARAREL"] = drC["GUARAREL"]== DBNull.Value ? "-" : drC["GUARAREL"];
                drInsert["CIF_SCORE"] = drC["CIF_SCORE"];
                drInsert["CBS_OCCCLS_CD"] = drC["CBS_OCCCLS_CD"];
                drInsert["CBS_OCCSCLS_CD"] = drC["CBS_OCCSCLS_CD"];
                drInsert["ISIC1_NAME"] = drC["ISIC1_NAME"];
                drInsert["ISIC2_NAME"] = drC["ISIC2_NAME"];
                drInsert["ISIC3_NAME"] = drC["ISIC3_NAME"];
                drInsert["CBS_PWORKTIME"] = drC["CBS_PWORKTIME"];
                drInsert["CBS_TWORKTIME"] = drC["CBS_TWORKTIME"];
                drInsert["CBS_BUSSYEAR"] = drC["CBS_BUSSYEAR"];
                drInsert["CBS_BUSSMONTH"] = drC["CBS_BUSSMONTH"];
                drInsert["CBS_PROP_RIGHT"] = drC["CBS_PROP_RIGHT"];
                drInsert["CBS_BUSSTYPE"] = drC["CBS_BUSSTYPE"];
                drInsert["CBS_INCOME"] = drC["CBS_INCOME"];
                drInsert["CBS_OTHINCOME"] = drC["CBS_OTHINCOME"];
                drInsert["CBS_MNTEXPEN"] = drC["CBS_MNTEXPEN"];
                drInsert["CBS_LBTEXPEN"] = drC["CBS_LBTEXPEN"];
                drInsert["CBS_BUSEXPEN"] = drC["CBS_BUSEXPEN"];
                drInsert["TOTAL_INCOME"] = drC["TOTAL_INCOME"];
                drInsert["NUM_CARD"] = drC["NUM_CARD"];
                drInsert["CBS_AUTO_STATUS"] = drC["CBS_AUTO_STATUS"];
                drInsert["CBS_PROP_STATUS"] = drC["CBS_PROP_STATUS"];
                drInsert["CUST_TYPE"] = drC["CUST_TYPE"];
                drInsert["OUT_DEPT"] = drC["OUT_DEPT"] == DBNull.Value ? "0.00" : drC["OUT_DEPT"];
                drInsert["BKLST_HIST"] = drC["BKLST_HIST"];
                drInsert["LITIG_STAT"] = drC["LITIG_STAT"];
                drInsert["MESS_CHENL"] = drC["MESS_CHENL"];

                ds.Tables["dtLoanerInformation"].Rows.Add(drInsert);
            }
            //-------------------------------------------------------------------

            //Create DataSet to contain data for Loaner
            DataTable dtCollateral = ((DataTable)(Session["dtCollateral"]));

            DataRow[] drCollateral = dtCollateral.Select();
            foreach (DataRow drC in drCollateral)
            {
                DataRow drInsert = ds.Tables["dtCollateral"].NewRow();

                drInsert["APP_NO"] = strAPP_NO;
                drInsert["COLLTYPE"] = drC["COLLTYPE"];
                drInsert["COLLSTYPE"] = drC["COLLSTYPE"];
                drInsert["COLL_ID"] = drC["COLL_ID"];
                drInsert["COLLVAL"] = drC["COLLVAL"] == DBNull.Value ? "0.00" : drC["COLLVAL"]; ;

                ds.Tables["dtCollateral"].Rows.Add(drInsert);
            }
            //-------------------------------------------------------------------

            //Create DataSet to contain data for charactor

            if (Session["dtChar_CBS_CIFNO"] != null)
            {

                string[] strCBS_CIFNO = Session["dtChar_CBS_CIFNO"].ToString().Split('|');
                int intNumberOfCIF = strCBS_CIFNO.Length;

                for (int i = 0; i < intNumberOfCIF; i++)
                {
                    //DataTable dtChar = ((DataTable)(Session["dtChar"]));
                    if (Session["dtChar_" + strCBS_CIFNO[i]] != null)
                    {
                        DataRow[] drChar = (((DataTable)(Session["dtChar_" + strCBS_CIFNO[i]]))).Select();
                        foreach (DataRow drC in drChar)
                        {
                            DataRow drInsert = ds.Tables["dtChar"].NewRow();

                            drInsert["APP_NO"] = strAPP_NO;
                            drInsert["CHAR_NAME"] = drC["CHAR_NAME"];
                            drInsert["SCORE"] = drC["SCORE"];
                            drInsert["CBS_CIFNO"] = strCBS_CIFNO[i].Trim();

                            ds.Tables["dtChar"].Rows.Add(drInsert);
                        }
                    }                                       
                }
            }
            //-------------------------------------------------------------------


            ReportDocument orpt = new ReportDocument();
            orpt.Load(strFileName);
            orpt.SetDataSource(ds);

            orpt.DataDefinition.FormulaFields["frmPrintDate"].Text = "'" + genPrintDate() + "'";
            orpt.DataDefinition.FormulaFields["frmPrintTime"].Text = "'" + genPrintTime() + "'";

            //Define user name
            string strUserFirstName = "User not define";
            if (Session["userFirstname"] != null)
            {
                strUserFirstName = Session["userFirstname"].ToString();
            }
            orpt.DataDefinition.FormulaFields["frmPrintBy"].Text = "'" + strUserFirstName.Trim() + "'";

            rptView.ReportSource = orpt;
            rptView.HasToggleGroupTreeButton = false;

        }

        private void LoadReportFF2(string strFileName)
        {

            string strAPP_NO = "";

            //Create DataSet to contain data for Application
            DataTable dt = ((DataTable)(Session["dtApplication"]));
            GSB.Loan.ReportLayOut.LoanDetailDataSet ds = new GSB.Loan.ReportLayOut.LoanDetailDataSet();

            DataRow[] dr = dt.Select();
            foreach (DataRow drC in dr)
            {
                DataRow drInsert = ds.Tables["dtApplicationInfomation"].NewRow();

                drInsert["APP_NO"] = drC["APP_NO"];
                strAPP_NO = drC["APP_NO"].ToString();
                drInsert["MODEL_NAME"] = drC["MODEL_NAME"];

                string[] strAPP_DATE = DateTime.Parse(drC["APP_DATE"].ToString()).ToString("dd/MM/yyyy").Split('/');
                string strApplicationDAte = "";
                if (strAPP_DATE.Length == 3)
                {
                    int intApplicationYear = int.Parse(strAPP_DATE[2]);
                    if (intApplicationYear < 2500) intApplicationYear += 543;
                    strApplicationDAte = int.Parse(strAPP_DATE[0]).ToString() + " " + convertMonthToText("TH", int.Parse(strAPP_DATE[1])) + " " + intApplicationYear.ToString();
                }
                drInsert["APP_DATE"] = strApplicationDAte;

                drInsert["LOAN_NAME"] = drC["LOAN_NAME"];
                drInsert["CREATION_BY"] = drC["CREATION_BY"];
                drInsert["STYPE_NAME"] = drC["STYPE_NAME"];
                drInsert["BRANCH_NAME"] = drC["BRANCH_NAME"];
                drInsert["MARKET_NAME"] = drC["MARKET_NAME"];
                drInsert["LOAN_AMOUNT"] = drC["LOAN_AMOUNT"];
                drInsert["PURPOSE_NAME"] = drC["PURPOSE_NAME"];
                drInsert["LNCOLLVAL"] = drC["LNCOLLVAL"];
                drInsert["CONSUMPTION_NAME"] = drC["CONSUMPTION_NAME"];

                drInsert["ISIC1_NAME"] = drC["ISIC1_NAME"];
                drInsert["GSBPURPOSE_NAME"] = drC["GSBPURPOSE_NAME"];
                drInsert["ISIC2_NAME"] = drC["ISIC2_NAME"];
                drInsert["SCORE"] = drC["SCORE"];
                drInsert["ISIC3_NAME"] = drC["ISIC3_NAME"];
                drInsert["GRADE"] = drC["GRADE"];
                drInsert["LOAN_TERM"] = drC["LOAN_TERM"];
                drInsert["LNSTATUS"] = drC["LNSTATUS"];

                drInsert["MPAYAMT"] = drC["MPAYAMT"];
                drInsert["APPRAMT"] = drC["APPRAMT"];
                drInsert["LOAN_NUM"] = drC["LOAN_NUM"];
                drInsert["APPRTERM"] = drC["APPRTERM"];

                drInsert["CA_Date"] = "";//drC["CA_Date"];
                drInsert["CA_REASON"] = drC["CA_REASON"];
                drInsert["CBS_COL"] = drC["CBS_COL"];

                ds.Tables["dtApplicationInfomation"].Rows.Add(drInsert);
            }
            //-------------------------------------------------------------------


            //Create DataSet to contain data for Loaner
            DataTable dtLoaner = ((DataTable)(Session["dtLoaner"]));

            DataRow[] drLoaner = dtLoaner.Select();
            int intRowID = 1;
            foreach (DataRow drC in drLoaner)
            {
                DataRow drInsert = ds.Tables["dtLoanerInformation"].NewRow();

                drInsert["APP_NO"] = strAPP_NO;
                drInsert["ROW"] = drC["ROW"];
                drInsert["LOANERTYPE"] = drC["ROW"].ToString().Replace("ข้อมูล", "");
                drInsert["ROWID"] = intRowID;
                intRowID++;
                drInsert["CBS_CIFNO"] = drC["CBS_CIFNO"];
                drInsert["CBS_CIFNAME"] = drC["CBS_CIFNAME"];
                drInsert["CBS_GENDER_CD"] = drC["CBS_GENDER_CD"];

                string[] DOB = DateTime.Parse(drC["CBS_CDOB"].ToString()).ToString("dd/MM/yyyy").Split('/');
                string DateOfBirth = "";
                if (DOB.Length == 3)
                {
                    int intDOBYear = int.Parse(DOB[2]);
                    if (intDOBYear < 2500) intDOBYear += 543;
                    DateOfBirth = int.Parse(DOB[0]).ToString() + " " + convertMonthToText("TH", int.Parse(DOB[1])) + " " + intDOBYear.ToString();
                }
                drInsert["CBS_CDOB"] = DateOfBirth;

                drInsert["AGE"] = drC["AGE"];
                drInsert["CBS_CITIZID"] = drC["CBS_CITIZID"];
                drInsert["PASSID"] = drC["PASSID"];
                drInsert["CBS_EDU_CD"] = drC["CBS_EDU_CD"];
                drInsert["CBS_RESSTATUS_CD"] = drC["CBS_RESSTATUS_CD"];
                drInsert["CBS_PADDRTIME"] = drC["CBS_PADDRTIME"];
                drInsert["CBS_RESTYPE_CD"] = drC["CBS_RESTYPE_CD"];
                drInsert["CBS_MARITAL_CD"] = drC["CBS_MARITAL_CD"];
                drInsert["CBS_CHILD"] = drC["CBS_CHILD"];
                drInsert["CBS_WCHILD"] = drC["CBS_WCHILD"];
                drInsert["CBS_CTYPE"] = drC["CBS_CTYPE"];
                drInsert["CBS_CCODE"] = drC["CBS_CCODE"];
                drInsert["COBORREL"] = drC["COBORREL"] == DBNull.Value ? "-" : drC["COBORREL"];

                drInsert["GUARAREL"] = drC["GUARAREL"] == DBNull.Value ? "-" : drC["GUARAREL"];
                drInsert["CIF_SCORE"] = drC["CIF_SCORE"];
                drInsert["CBS_OCCCLS_CD"] = drC["CBS_OCCCLS_CD"];
                drInsert["CBS_OCCSCLS_CD"] = drC["CBS_OCCSCLS_CD"];
                drInsert["ISIC1_NAME"] = drC["ISIC1_NAME"];
                drInsert["ISIC2_NAME"] = drC["ISIC2_NAME"];
                drInsert["ISIC3_NAME"] = drC["ISIC3_NAME"];
                drInsert["CBS_PWORKTIME"] = drC["CBS_PWORKTIME"];
                drInsert["CBS_TWORKTIME"] = drC["CBS_TWORKTIME"];
                drInsert["CBS_BUSSYEAR"] = drC["CBS_BUSSYEAR"];
                drInsert["CBS_BUSSMONTH"] = drC["CBS_BUSSMONTH"];
                drInsert["CBS_PROP_RIGHT"] = drC["CBS_PROP_RIGHT"];
                drInsert["CBS_BUSSTYPE"] = drC["CBS_BUSSTYPE"];
                drInsert["CBS_INCOME"] = drC["CBS_INCOME"];
                drInsert["CBS_OTHINCOME"] = drC["CBS_OTHINCOME"];
                drInsert["CBS_MNTEXPEN"] = drC["CBS_MNTEXPEN"];
                drInsert["CBS_LBTEXPEN"] = drC["CBS_LBTEXPEN"];
                drInsert["CBS_BUSEXPEN"] = drC["CBS_BUSEXPEN"];
                drInsert["TOTAL_INCOME"] = drC["TOTAL_INCOME"];
                drInsert["NUM_CARD"] = drC["NUM_CARD"];
                drInsert["CBS_AUTO_STATUS"] = drC["CBS_AUTO_STATUS"];
                drInsert["CBS_PROP_STATUS"] = drC["CBS_PROP_STATUS"];
                drInsert["CUST_TYPE"] = drC["CUST_TYPE"];
                drInsert["OUT_DEPT"] = drC["OUT_DEPT"] == DBNull.Value ? "0.00" : drC["OUT_DEPT"];
                drInsert["BKLST_HIST"] = drC["BKLST_HIST"];
                drInsert["LITIG_STAT"] = drC["LITIG_STAT"];
                drInsert["MESS_CHENL"] = drC["MESS_CHENL"];

                ds.Tables["dtLoanerInformation"].Rows.Add(drInsert);
            }
            //-------------------------------------------------------------------


            //Create DataSet to contain data for charactor

            if (Session["dtChar_CBS_CIFNO"] != null)
            {

                string[] strCBS_CIFNO = Session["dtChar_CBS_CIFNO"].ToString().Split('|');
                int intNumberOfCIF = strCBS_CIFNO.Length;

                DataTable drChar = (((DataTable)(Session["dtChar_CBS_CIFNO"])));

                foreach (DataRow drC in drChar.Rows)
                {
                    DataRow drInsert = ds.Tables["dtChar"].NewRow();

                    drInsert["APP_NO"] = strAPP_NO;
                    drInsert["CHAR_NAME"] = drC["CHAR_NAME"];
                    drInsert["SCORE"] = drC["SCORE"];
                    drInsert["CBS_CIFNO"] = "";

                    ds.Tables["dtChar"].Rows.Add(drInsert);
                }


                //for (int i = 0; i < intNumberOfCIF; i++)
                //{
                //    //DataTable dtChar = ((DataTable)(Session["dtChar"]));
                //    if (Session["dtChar_" + strCBS_CIFNO[i]] != null)
                //    {
                //        DataRow[] drChar = (((DataTable)(Session["dtChar_" + strCBS_CIFNO[i]]))).Select();
                //        foreach (DataRow drC in drChar)
                //        {
                //            DataRow drInsert = ds.Tables["dtChar"].NewRow();

                //            drInsert["APP_NO"] = strAPP_NO;
                //            drInsert["CHAR_NAME"] = drC["CHAR_NAME"];
                //            drInsert["SCORE"] = drC["SCORE"];
                //            drInsert["CBS_CIFNO"] = strCBS_CIFNO[i].Trim();

                //            ds.Tables["dtChar"].Rows.Add(drInsert);
                //        }
                //    }
                //}
            }
            //-------------------------------------------------------------------


            ReportDocument orpt = new ReportDocument();
            orpt.Load(strFileName);
            orpt.SetDataSource(ds);

            orpt.DataDefinition.FormulaFields["frmPrintDate"].Text = "'" + genPrintDate() + "'";
            orpt.DataDefinition.FormulaFields["frmPrintTime"].Text = "'" + genPrintTime() + "'";

            //Define user name
            string strUserFirstName = "User not define";
            if (Session["userFirstname"] != null)
            {
                strUserFirstName = Session["userFirstname"].ToString();
            }
            orpt.DataDefinition.FormulaFields["frmPrintBy"].Text = "'" + strUserFirstName.Trim() + "'";

            rptView.ReportSource = orpt;
            rptView.HasToggleGroupTreeButton = false;

        }





    }
}
