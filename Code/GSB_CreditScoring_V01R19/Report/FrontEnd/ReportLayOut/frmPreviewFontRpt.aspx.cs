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


namespace GSB.Portfolio.Web.FontEnd
{
    public partial class frmPreviewFontRpt : System.Web.UI.Page
    {

        /* Get Product Information */
        public string genPrintDateTime()
        {
            DataTable dt = new DataTable();

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

            return tempDay.ToString().Substring(1) + "/" + tempMonth.ToString().Substring(1) + "/" + tempYear.ToString() + " ( " + System.DateTime.Now.ToString("HH:mm") + " )";

        }


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

            if (Session["sFS"] != null)
            { hdFS.Value = Session["sFS"].ToString(); }
            else
            { Session["sFS"] = hdFS.Value.ToString(); }

            switch (Request.QueryString["ReportId"].ToString())
            {
                case "1":
                    //Population Stabilities Report
                    _strFileName = strPath + "\\SCORINGF0001.rpt";
                    LoadReportFF1(_strFileName);
                    break;
                case "91":
                    //Population Stabilities Report
                    _strFileName = strPath + "\\SCORINGF0001.rpt";
                    LoadReportFF91(_strFileName);
                    break;
                case "2SN":
                    //Approval Rate Report (Score and Number)
                    _strFileName = strPath + "\\SCORINGF0002SN.rpt";
                    LoadReportFF2SN(_strFileName);
                    break;
                case "92SN":
                    //Approval Rate Report (Score and Number)
                    _strFileName = strPath + "\\SCORINGF0002SN.rpt";
                    LoadReportFF92SN(_strFileName);
                    break;
                case "2SP":
                    //Approval Rate Report (Score and Percentage)
                    _strFileName = strPath + "\\SCORINGF0002SP.rpt";
                    LoadReportFF2SP(_strFileName);
                    break;
                case "92SP":
                    //Approval Rate Report (Score and Percentage)
                    _strFileName = strPath + "\\SCORINGF0002SP.rpt";
                    LoadReportFF92SP(_strFileName);
                    break;
                case "2GN":
                    //Approval Rate Report (Grade and Number)
                    _strFileName = strPath + "\\SCORINGF0002GN.rpt";
                    LoadReportFF2GN(_strFileName);
                    break;
                case "92GN":
                    //Approval Rate Report (Grade and Number)
                    _strFileName = strPath + "\\SCORINGF0002GN.rpt";
                    LoadReportFF92GN(_strFileName);
                    break;
                case "2GP":
                    //Approval Rate Report (Grade and Percentage)
                    _strFileName = strPath + "\\SCORINGF0002GP.rpt";
                    LoadReportFF2GP(_strFileName);
                    break;
                case "92GP":
                    //Approval Rate Report (Grade and Percentage)
                    _strFileName = strPath + "\\SCORINGF0002GP.rpt";
                    LoadReportFF92GP(_strFileName);
                    break;
                case "3":
                    _strFileName = strPath + "\\SCORINGF0003.rpt";
                    LoadReportFF3(_strFileName);
                    break;
                case "93":
                    _strFileName = strPath + "\\SCORINGF0003.rpt";
                    LoadReportFF93(_strFileName);
                    break;
                case "4":
                    //Final Score Report
                    _strFileName = strPath + "\\SCORINGF0004.rpt";
                    LoadReportFF4(_strFileName);
                    break;
                case "94":
                    //Final Score Report
                    _strFileName = strPath + "\\SCORINGF0004.rpt";
                    LoadReportFF94(_strFileName);
                    break;
                case "5":
                    //Overrides Rate Report
                    _strFileName = strPath + "\\SCORINGF0005.rpt";
                    LoadReportFF5(_strFileName);
                    break;
                case "95":
                    //Overrides Rate Report
                    _strFileName = strPath + "\\SCORINGF0005.rpt";
                    LoadReportFF95(_strFileName);
                    break;
                case "6":
                    //Overrides Reason Report
                    _strFileName = strPath + "\\SCORINGF0006.rpt";
                    LoadReportFF6(_strFileName);
                    break;
                case "96":
                    //Overrides Reason Report
                    _strFileName = strPath + "\\SCORINGF0006.rpt";
                    LoadReportFF96(_strFileName);
                    break;
            }

        }

        //Population Stabilities Report
        private void LoadReportFF1(string strFileName)
        {

            //Create DataSet to contain data
            DataTable dt = ((DataTable)(Session["rptData"]));
            GSB.Report.FrontEnd.ReportLayOut.dtRptGSBPFF0001 ds = new Report.FrontEnd.ReportLayOut.dtRptGSBPFF0001();

            DataRow[] dr = dt.Select();
            foreach (DataRow drC in dr)
            {
                DataRow drInsert = ds.Tables["dtRptGSBPFF0001"].NewRow();

                drInsert["SCRRG"] = drC["SCRRG"];
                drInsert["DEV"] = drC["DEV"];
                drInsert["PDEV"] = drC["PDEV"];
                drInsert["ACT"] = drC["ACT"];
                drInsert["PACT"] = drC["PACT"];
                drInsert["PCHNG"] = drC["PCHNG"];
                drInsert["PRATIO"] = drC["PRATIO"];
                drInsert["WOE"] = drC["WOE"];
                drInsert["PINDEX"] = drC["PINDEX"];
                drInsert["PAPP"] = drC["PAPP"];
                drInsert["PREJ"] = drC["PREJ"];
                drInsert["Approve"] = drC["Approve"];
                drInsert["Reject"] = drC["Reject"];
                ds.Tables["dtRptGSBPFF0001"].Rows.Add(drInsert);
            }
            //-------------------------------------------------------------------


            ReportDocument orpt = new ReportDocument();
            orpt.Load(strFileName);
            orpt.SetDataSource(ds);

            orpt.DataDefinition.FormulaFields["Model"].Text = "'" + Session["sHeaderRpt1"].ToString() + "'";
            orpt.DataDefinition.FormulaFields["Product"].Text = "'" + Session["sHeaderRpt2"].ToString() + "'";
            orpt.DataDefinition.FormulaFields["Market"].Text = "'" + Session["sHeaderRpt3"].ToString() + "'";
            orpt.DataDefinition.FormulaFields["Target"].Text = "'" + Session["sHeaderRpt4"].ToString() + "'";
            orpt.DataDefinition.FormulaFields["paraSumOfPPopStabInx"].Text = "'" + Session["lblSumIndex"].ToString() + "'";
            orpt.DataDefinition.FormulaFields["fmPrintDateTime"].Text = "'" + genPrintDateTime() + "'";

            //Define user name
            string strUserFirstName = "User not define";
            if (Session["userFirstname"] != null)
            {
                strUserFirstName = Session["userFirstname"].ToString();
            }
            orpt.DataDefinition.FormulaFields["fmPrintBy"].Text = "'" + strUserFirstName.Trim() + "'";

            rptView.ReportSource = orpt;
            rptView.HasToggleGroupTreeButton = false;

        }

        //Approval Reason Report (Score Range and Number)
        private void LoadReportFF2SN(string strFileName)
        {

            //Create DataSet to contain data
            DataTable dt = ((DataTable)(Session["rptData"]));
            GSB.Report.FrontEnd.ReportLayOut.dtRptGSBPFF0002 ds = new Report.FrontEnd.ReportLayOut.dtRptGSBPFF0002();

            DataRow[] dr = dt.Select();
            foreach (DataRow drC in dr)
            {
                DataRow drInsert = ds.Tables["dtRptGSBPFF0002"].NewRow();
                // -- Modify to show Header Score in every Pages by Pee 12-Dec-2013
                if (drC["SCRRG"].ToString().Trim() == "Score Range")
                {
                    ds.Tables["dtRptGSBPFF0002"].Rows.Add(drInsert);
                    ds.Tables["dtRptGSBPFF0002"].Rows.Remove(drInsert);

                }
                // -- For Show Header Score in every Pages
                else
                {

                    drInsert["SCRRG"] = drC["SCRRG"];
                    drInsert["TDD3M"] = drC["TDD3M"] == DBNull.Value ? 0 : drC["TDD3M"];
                    drInsert["TDD12M"] = drC["TDD12M"] == DBNull.Value ? 0 : drC["TDD12M"];
                    drInsert["TDD12YG"] = drC["TDD12YG"] == DBNull.Value ? 0 : drC["TDD12YG"];
                    drInsert["TDD3MW"] = drC["TDD3MW"] == DBNull.Value ? 0 : drC["TDD3MW"];
                    drInsert["TDD12MW"] = drC["TDD12MW"] == DBNull.Value ? 0 : drC["TDD12MW"];
                    drInsert["TDD12YGW"] = drC["TDD12YGW"] == DBNull.Value ? 0 : drC["TDD12YGW"];
                    drInsert["TDD3MA"] = drC["TDD3MA"] == DBNull.Value ? 0 : drC["TDD3MA"];
                    drInsert["TDD12MA"] = drC["TDD12MA"] == DBNull.Value ? 0 : drC["TDD12MA"];
                    drInsert["TDD12YGA"] = drC["TDD12YGA"] == DBNull.Value ? 0 : drC["TDD12YGA"];
                    drInsert["TDD3MB"] = drC["TDD3MB"] == DBNull.Value ? 0 : drC["TDD3MB"];
                    drInsert["TDD12MB"] = drC["TDD12MB"] == DBNull.Value ? 0 : drC["TDD12MB"];
                    drInsert["TDD12YGB"] = drC["TDD12YGB"] == DBNull.Value ? 0 : drC["TDD12YGB"];
                    ds.Tables["dtRptGSBPFF0002"].Rows.Add(drInsert);
                }
                
            }
            //-------------------------------------------------------------------


            ReportDocument orpt = new ReportDocument();
            orpt.Load(strFileName);
            orpt.SetDataSource(ds);

            orpt.DataDefinition.FormulaFields["Model"].Text = "'" + Session["sHeaderRpt1"].ToString() + "'";
            orpt.DataDefinition.FormulaFields["Product"].Text = "'" + Session["sHeaderRpt2"].ToString() + "'";
            orpt.DataDefinition.FormulaFields["Market"].Text = "'" + Session["sHeaderRpt3"].ToString() + "'";
            orpt.DataDefinition.FormulaFields["Target"].Text = "'" + Session["sHeaderRpt4"].ToString() + "'";

            orpt.DataDefinition.FormulaFields["fmPrintDateTime"].Text = "'" + genPrintDateTime() + "'";

            //Define user name
            string strUserFirstName = "User not define";
            if (Session["userFirstname"] != null)
            {
                strUserFirstName = Session["userFirstname"].ToString();
            }
            orpt.DataDefinition.FormulaFields["fmPrintBy"].Text = "'" + strUserFirstName.Trim() + "'";

            rptView.ReportSource = orpt;
            rptView.HasToggleGroupTreeButton = false;
        }

        private void LoadReportFF92SN(string strFileName)
        {

            //Create DataSet to contain data
            DataTable dt = ((DataTable)(Session["rptData"]));
            GSB.Report.FrontEnd.ReportLayOut.dtRptGSBPFF0002 ds = new Report.FrontEnd.ReportLayOut.dtRptGSBPFF0002();

            DataRow[] dr = dt.Select();
            foreach (DataRow drC in dr)
            {
                DataRow drInsert = ds.Tables["dtRptGSBPFF0002"].NewRow();
                // -- Modify to show Header Score in every Pages by Pee 12-Dec-2013
                if (drC["SCRRG"].ToString().Trim() == "Score Range")
                {
                    ds.Tables["dtRptGSBPFF0002"].Rows.Add(drInsert);
                    ds.Tables["dtRptGSBPFF0002"].Rows.Remove(drInsert);

                }
                // -- For Show Header Score in every Pages
                else
                {

                    drInsert["SCRRG"] = drC["SCRRG"];
                    drInsert["TDD3M"] = drC["TDD3M"] == DBNull.Value ? 0 : drC["TDD3M"];
                    drInsert["TDD12M"] = drC["TDD12M"] == DBNull.Value ? 0 : drC["TDD12M"];
                    drInsert["TDD12YG"] = drC["TDD12YG"] == DBNull.Value ? 0 : drC["TDD12YG"];
                    drInsert["TDD3MW"] = drC["TDD3MW"] == DBNull.Value ? 0 : drC["TDD3MW"];
                    drInsert["TDD12MW"] = drC["TDD12MW"] == DBNull.Value ? 0 : drC["TDD12MW"];
                    drInsert["TDD12YGW"] = drC["TDD12YGW"] == DBNull.Value ? 0 : drC["TDD12YGW"];
                    drInsert["TDD3MA"] = drC["TDD3MA"] == DBNull.Value ? 0 : drC["TDD3MA"];
                    drInsert["TDD12MA"] = drC["TDD12MA"] == DBNull.Value ? 0 : drC["TDD12MA"];
                    drInsert["TDD12YGA"] = drC["TDD12YGA"] == DBNull.Value ? 0 : drC["TDD12YGA"];
                    drInsert["TDD3MB"] = drC["TDD3MB"] == DBNull.Value ? 0 : drC["TDD3MB"];
                    drInsert["TDD12MB"] = drC["TDD12MB"] == DBNull.Value ? 0 : drC["TDD12MB"];
                    drInsert["TDD12YGB"] = drC["TDD12YGB"] == DBNull.Value ? 0 : drC["TDD12YGB"];
                    ds.Tables["dtRptGSBPFF0002"].Rows.Add(drInsert);
                }

            }
            //-------------------------------------------------------------------


            ReportDocument orpt = new ReportDocument();
            orpt.Load(strFileName);
            orpt.SetDataSource(ds);

            orpt.DataDefinition.FormulaFields["Model"].Text = "'" + Session["sHeaderRpt1"].ToString() + "'";
            orpt.DataDefinition.FormulaFields["Product"].Text = "'" + Session["sHeaderRpt2"].ToString() + "'";
            orpt.DataDefinition.FormulaFields["Market"].Text = "'" + Session["sHeaderRpt3"].ToString() + "'";
            orpt.DataDefinition.FormulaFields["Target"].Text = "'" + Session["sHeaderRpt4"].ToString() + "'";

            orpt.DataDefinition.FormulaFields["fmPrintDateTime"].Text = "'" + genPrintDateTime() + "'";

            //Define user name
            string strUserFirstName = "User not define";
            if (Session["userFirstname"] != null)
            {
                strUserFirstName = Session["userFirstname"].ToString();
            }
            orpt.DataDefinition.FormulaFields["fmPrintBy"].Text = "'" + strUserFirstName.Trim() + "'";

            rptView.ReportSource = orpt;

            ExportFormatType formatType = ExportFormatType.NoFormat;
            formatType = ExportFormatType.Excel;
            orpt.ExportToHttpResponse(formatType, Response, true, "ApprovalRateReport_" + DateTime.Now.ToString("yyyyMMdd"));
            Response.End();
        }

        //Approval Reason Report (Score Range and Percentage)
        private void LoadReportFF2SP(string strFileName)
        {
            //Create DataSet to contain data
            DataTable dt = ((DataTable)(Session["rptData"]));
            GSB.Report.FrontEnd.ReportLayOut.dtRptGSBPFF0002 ds = new Report.FrontEnd.ReportLayOut.dtRptGSBPFF0002();

            DataRow[] dr = dt.Select();
            foreach (DataRow drC in dr)
            {
                DataRow drInsert = ds.Tables["dtRptGSBPFF0002"].NewRow();
                // -- Modify to show Header Score in every Pages by Pee 12-Dec-2013
                if (drC["SCRRG"].ToString().Trim() == "Score Range")
                {
                    ds.Tables["dtRptGSBPFF0002"].Rows.Add(drInsert);
                    ds.Tables["dtRptGSBPFF0002"].Rows.Remove(drInsert);

                }
                // -- For Show Header Score in every Pages
                else
                {
                    drInsert["SCRRG"] = drC["SCRRG"];
                    drInsert["TDD3M"] = drC["TDD3M"] == DBNull.Value ? 0 : drC["TDD3M"];
                    drInsert["TDD12M"] = drC["TDD12M"] == DBNull.Value ? 0 : drC["TDD12M"];
                    drInsert["TDD12YG"] = drC["TDD12YG"] == DBNull.Value ? 0 : drC["TDD12YG"];
                    drInsert["TDD3MW"] = drC["TDD3MW"] == DBNull.Value ? 0 : drC["TDD3MW"];
                    drInsert["TDD12MW"] = drC["TDD12MW"] == DBNull.Value ? 0 : drC["TDD12MW"];
                    drInsert["TDD12YGW"] = drC["TDD12YGW"] == DBNull.Value ? 0 : drC["TDD12YGW"];
                    drInsert["TDD3MA"] = drC["TDD3MA"] == DBNull.Value ? 0 : drC["TDD3MA"];
                    drInsert["TDD12MA"] = drC["TDD12MA"] == DBNull.Value ? 0 : drC["TDD12MA"];
                    drInsert["TDD12YGA"] = drC["TDD12YGA"] == DBNull.Value ? 0 : drC["TDD12YGA"];
                    drInsert["TDD3MB"] = drC["TDD3MB"] == DBNull.Value ? 0 : drC["TDD3MB"];
                    drInsert["TDD12MB"] = drC["TDD12MB"] == DBNull.Value ? 0 : drC["TDD12MB"];
                    drInsert["TDD12YGB"] = drC["TDD12YGB"] == DBNull.Value ? 0 : drC["TDD12YGB"];
                    ds.Tables["dtRptGSBPFF0002"].Rows.Add(drInsert);
                }
            }
            //-------------------------------------------------------------------


            ReportDocument orpt = new ReportDocument();
            orpt.Load(strFileName);
            orpt.SetDataSource(ds);

            orpt.DataDefinition.FormulaFields["Model"].Text = "'" + Session["sHeaderRpt1"].ToString() + "'";
            orpt.DataDefinition.FormulaFields["Product"].Text = "'" + Session["sHeaderRpt2"].ToString() + "'";
            orpt.DataDefinition.FormulaFields["Market"].Text = "'" + Session["sHeaderRpt3"].ToString() + "'";
            orpt.DataDefinition.FormulaFields["Target"].Text = "'" + Session["sHeaderRpt4"].ToString() + "'";

            orpt.DataDefinition.FormulaFields["fmPrintDateTime"].Text = "'" + genPrintDateTime() + "'";

            //Define user name
            string strUserFirstName = "User not define";
            if (Session["userFirstname"] != null)
            {
                strUserFirstName = Session["userFirstname"].ToString();
            }
            orpt.DataDefinition.FormulaFields["fmPrintBy"].Text = "'" + strUserFirstName.Trim() + "'";

            rptView.ReportSource = orpt;
            rptView.HasToggleGroupTreeButton = false;
        }

        private void LoadReportFF92SP(string strFileName)
        {
            //Create DataSet to contain data
            DataTable dt = ((DataTable)(Session["rptData"]));
            GSB.Report.FrontEnd.ReportLayOut.dtRptGSBPFF0002 ds = new Report.FrontEnd.ReportLayOut.dtRptGSBPFF0002();

            DataRow[] dr = dt.Select();
            foreach (DataRow drC in dr)
            {
                DataRow drInsert = ds.Tables["dtRptGSBPFF0002"].NewRow();
                // -- Modify to show Header Score in every Pages by Pee 12-Dec-2013
                if (drC["SCRRG"].ToString().Trim() == "Score Range")
                {
                    ds.Tables["dtRptGSBPFF0002"].Rows.Add(drInsert);
                    ds.Tables["dtRptGSBPFF0002"].Rows.Remove(drInsert);

                }
                // -- For Show Header Score in every Pages
                else
                {
                    drInsert["SCRRG"] = drC["SCRRG"];
                    drInsert["TDD3M"] = drC["TDD3M"] == DBNull.Value ? 0 : drC["TDD3M"];
                    drInsert["TDD12M"] = drC["TDD12M"] == DBNull.Value ? 0 : drC["TDD12M"];
                    drInsert["TDD12YG"] = drC["TDD12YG"] == DBNull.Value ? 0 : drC["TDD12YG"];
                    drInsert["TDD3MW"] = drC["TDD3MW"] == DBNull.Value ? 0 : drC["TDD3MW"];
                    drInsert["TDD12MW"] = drC["TDD12MW"] == DBNull.Value ? 0 : drC["TDD12MW"];
                    drInsert["TDD12YGW"] = drC["TDD12YGW"] == DBNull.Value ? 0 : drC["TDD12YGW"];
                    drInsert["TDD3MA"] = drC["TDD3MA"] == DBNull.Value ? 0 : drC["TDD3MA"];
                    drInsert["TDD12MA"] = drC["TDD12MA"] == DBNull.Value ? 0 : drC["TDD12MA"];
                    drInsert["TDD12YGA"] = drC["TDD12YGA"] == DBNull.Value ? 0 : drC["TDD12YGA"];
                    drInsert["TDD3MB"] = drC["TDD3MB"] == DBNull.Value ? 0 : drC["TDD3MB"];
                    drInsert["TDD12MB"] = drC["TDD12MB"] == DBNull.Value ? 0 : drC["TDD12MB"];
                    drInsert["TDD12YGB"] = drC["TDD12YGB"] == DBNull.Value ? 0 : drC["TDD12YGB"];
                    ds.Tables["dtRptGSBPFF0002"].Rows.Add(drInsert);
                }
            }
            //-------------------------------------------------------------------


            ReportDocument orpt = new ReportDocument();
            orpt.Load(strFileName);
            orpt.SetDataSource(ds);

            orpt.DataDefinition.FormulaFields["Model"].Text = "'" + Session["sHeaderRpt1"].ToString() + "'";
            orpt.DataDefinition.FormulaFields["Product"].Text = "'" + Session["sHeaderRpt2"].ToString() + "'";
            orpt.DataDefinition.FormulaFields["Market"].Text = "'" + Session["sHeaderRpt3"].ToString() + "'";
            orpt.DataDefinition.FormulaFields["Target"].Text = "'" + Session["sHeaderRpt4"].ToString() + "'";

            orpt.DataDefinition.FormulaFields["fmPrintDateTime"].Text = "'" + genPrintDateTime() + "'";

            //Define user name
            string strUserFirstName = "User not define";
            if (Session["userFirstname"] != null)
            {
                strUserFirstName = Session["userFirstname"].ToString();
            }
            orpt.DataDefinition.FormulaFields["fmPrintBy"].Text = "'" + strUserFirstName.Trim() + "'";

            rptView.ReportSource = orpt;
            ExportFormatType formatType = ExportFormatType.NoFormat;
            formatType = ExportFormatType.Excel;
            orpt.ExportToHttpResponse(formatType, Response, true, "ApprovalRateReport_" + DateTime.Now.ToString("yyyyMMdd"));
            Response.End();
        }

        //Approval Reason Report (Grade Range and Number)
        private void LoadReportFF2GN(string strFileName)
        {
            //Create DataSet to contain data
            DataTable dt = ((DataTable)(Session["rptData"]));
            GSB.Report.FrontEnd.ReportLayOut.dtRptGSBPFF0002 ds = new Report.FrontEnd.ReportLayOut.dtRptGSBPFF0002();

            DataRow[] dr = dt.Select();
            foreach (DataRow drC in dr)
            {
                DataRow drInsert = ds.Tables["dtRptGSBPFF0002"].NewRow();
                if (drC["SCRRG"].ToString().Trim() == "Score Range")
                {
                    ds.Tables["dtRptGSBPFF0002"].Rows.Add(drInsert);
                    ds.Tables["dtRptGSBPFF0002"].Rows.Remove(drInsert);

                }
                // -- For Show Header Score in every Pages
                else
                {
                    drInsert["SCRRG"] = drC["SCRRG"];
                    drInsert["TDD3M"] = drC["TDD3M"] == DBNull.Value ? 0 : drC["TDD3M"];
                    drInsert["TDD12M"] = drC["TDD12M"] == DBNull.Value ? 0 : drC["TDD12M"];
                    drInsert["TDD12YG"] = drC["TDD12YG"] == DBNull.Value ? 0 : drC["TDD12YG"];
                    drInsert["TDD3MW"] = drC["TDD3MW"] == DBNull.Value ? 0 : drC["TDD3MW"];
                    drInsert["TDD12MW"] = drC["TDD12MW"] == DBNull.Value ? 0 : drC["TDD12MW"];
                    drInsert["TDD12YGW"] = drC["TDD12YGW"] == DBNull.Value ? 0 : drC["TDD12YGW"];
                    drInsert["TDD3MA"] = drC["TDD3MA"] == DBNull.Value ? 0 : drC["TDD3MA"];
                    drInsert["TDD12MA"] = drC["TDD12MA"] == DBNull.Value ? 0 : drC["TDD12MA"];
                    drInsert["TDD12YGA"] = drC["TDD12YGA"] == DBNull.Value ? 0 : drC["TDD12YGA"];
                    drInsert["TDD3MB"] = drC["TDD3MB"] == DBNull.Value ? 0 : drC["TDD3MB"];
                    drInsert["TDD12MB"] = drC["TDD12MB"] == DBNull.Value ? 0 : drC["TDD12MB"];
                    drInsert["TDD12YGB"] = drC["TDD12YGB"] == DBNull.Value ? 0 : drC["TDD12YGB"];
                    ds.Tables["dtRptGSBPFF0002"].Rows.Add(drInsert);
                }
               
            }
            //-------------------------------------------------------------------


            ReportDocument orpt = new ReportDocument();
            orpt.Load(strFileName);
            orpt.SetDataSource(ds);

            orpt.DataDefinition.FormulaFields["Model"].Text = "'" + Session["sHeaderRpt1"].ToString() + "'";
            orpt.DataDefinition.FormulaFields["Product"].Text = "'" + Session["sHeaderRpt2"].ToString() + "'";
            orpt.DataDefinition.FormulaFields["Market"].Text = "'" + Session["sHeaderRpt3"].ToString() + "'";
            orpt.DataDefinition.FormulaFields["Target"].Text = "'" + Session["sHeaderRpt4"].ToString() + "'";

            orpt.DataDefinition.FormulaFields["fmPrintDateTime"].Text = "'" + genPrintDateTime() + "'";

            //Define user name
            string strUserFirstName = "User not define";
            if (Session["userFirstname"] != null)
            {
                strUserFirstName = Session["userFirstname"].ToString();
            }
            orpt.DataDefinition.FormulaFields["fmPrintBy"].Text = "'" + strUserFirstName.Trim() + "'";

            rptView.ReportSource = orpt;
            rptView.HasToggleGroupTreeButton = false;
        }

        private void LoadReportFF92GN(string strFileName)
        {
            //Create DataSet to contain data
            DataTable dt = ((DataTable)(Session["rptData"]));
            GSB.Report.FrontEnd.ReportLayOut.dtRptGSBPFF0002 ds = new Report.FrontEnd.ReportLayOut.dtRptGSBPFF0002();

            DataRow[] dr = dt.Select();
            foreach (DataRow drC in dr)
            {
                DataRow drInsert = ds.Tables["dtRptGSBPFF0002"].NewRow();
                if (drC["SCRRG"].ToString().Trim() == "Score Range")
                {
                    ds.Tables["dtRptGSBPFF0002"].Rows.Add(drInsert);
                    ds.Tables["dtRptGSBPFF0002"].Rows.Remove(drInsert);

                }
                // -- For Show Header Score in every Pages
                else
                {
                    drInsert["SCRRG"] = drC["SCRRG"];
                    drInsert["TDD3M"] = drC["TDD3M"] == DBNull.Value ? 0 : drC["TDD3M"];
                    drInsert["TDD12M"] = drC["TDD12M"] == DBNull.Value ? 0 : drC["TDD12M"];
                    drInsert["TDD12YG"] = drC["TDD12YG"] == DBNull.Value ? 0 : drC["TDD12YG"];
                    drInsert["TDD3MW"] = drC["TDD3MW"] == DBNull.Value ? 0 : drC["TDD3MW"];
                    drInsert["TDD12MW"] = drC["TDD12MW"] == DBNull.Value ? 0 : drC["TDD12MW"];
                    drInsert["TDD12YGW"] = drC["TDD12YGW"] == DBNull.Value ? 0 : drC["TDD12YGW"];
                    drInsert["TDD3MA"] = drC["TDD3MA"] == DBNull.Value ? 0 : drC["TDD3MA"];
                    drInsert["TDD12MA"] = drC["TDD12MA"] == DBNull.Value ? 0 : drC["TDD12MA"];
                    drInsert["TDD12YGA"] = drC["TDD12YGA"] == DBNull.Value ? 0 : drC["TDD12YGA"];
                    drInsert["TDD3MB"] = drC["TDD3MB"] == DBNull.Value ? 0 : drC["TDD3MB"];
                    drInsert["TDD12MB"] = drC["TDD12MB"] == DBNull.Value ? 0 : drC["TDD12MB"];
                    drInsert["TDD12YGB"] = drC["TDD12YGB"] == DBNull.Value ? 0 : drC["TDD12YGB"];
                    ds.Tables["dtRptGSBPFF0002"].Rows.Add(drInsert);
                }

            }
            //-------------------------------------------------------------------


            ReportDocument orpt = new ReportDocument();
            orpt.Load(strFileName);
            orpt.SetDataSource(ds);

            orpt.DataDefinition.FormulaFields["Model"].Text = "'" + Session["sHeaderRpt1"].ToString() + "'";
            orpt.DataDefinition.FormulaFields["Product"].Text = "'" + Session["sHeaderRpt2"].ToString() + "'";
            orpt.DataDefinition.FormulaFields["Market"].Text = "'" + Session["sHeaderRpt3"].ToString() + "'";
            orpt.DataDefinition.FormulaFields["Target"].Text = "'" + Session["sHeaderRpt4"].ToString() + "'";

            orpt.DataDefinition.FormulaFields["fmPrintDateTime"].Text = "'" + genPrintDateTime() + "'";

            //Define user name
            string strUserFirstName = "User not define";
            if (Session["userFirstname"] != null)
            {
                strUserFirstName = Session["userFirstname"].ToString();
            }
            orpt.DataDefinition.FormulaFields["fmPrintBy"].Text = "'" + strUserFirstName.Trim() + "'";

            rptView.ReportSource = orpt;
            ExportFormatType formatType = ExportFormatType.NoFormat;
            formatType = ExportFormatType.Excel;
            orpt.ExportToHttpResponse(formatType, Response, true, "ApprovalRateReport_" + DateTime.Now.ToString("yyyyMMdd"));
            Response.End();
        }

        //Approval Reason Report (Grade Range and Percentage)
        private void LoadReportFF2GP(string strFileName)
        {
            //Create DataSet to contain data
            DataTable dt = ((DataTable)(Session["rptData"]));
            GSB.Report.FrontEnd.ReportLayOut.dtRptGSBPFF0002 ds = new Report.FrontEnd.ReportLayOut.dtRptGSBPFF0002();

            DataRow[] dr = dt.Select();
            foreach (DataRow drC in dr)
            {
                DataRow drInsert = ds.Tables["dtRptGSBPFF0002"].NewRow();
                if (drC["SCRRG"].ToString().Trim() == "Score Range")
                {
                    ds.Tables["dtRptGSBPFF0002"].Rows.Add(drInsert);
                    ds.Tables["dtRptGSBPFF0002"].Rows.Remove(drInsert);

                }
                // -- For Show Header Score in every Pages
                else
                {
                    drInsert["SCRRG"] = drC["SCRRG"];
                    drInsert["TDD3M"] = drC["TDD3M"] == DBNull.Value ? 0 : drC["TDD3M"];
                    drInsert["TDD12M"] = drC["TDD12M"] == DBNull.Value ? 0 : drC["TDD12M"];
                    drInsert["TDD12YG"] = drC["TDD12YG"] == DBNull.Value ? 0 : drC["TDD12YG"];
                    drInsert["TDD3MW"] = drC["TDD3MW"] == DBNull.Value ? 0 : drC["TDD3MW"];
                    drInsert["TDD12MW"] = drC["TDD12MW"] == DBNull.Value ? 0 : drC["TDD12MW"];
                    drInsert["TDD12YGW"] = drC["TDD12YGW"] == DBNull.Value ? 0 : drC["TDD12YGW"];
                    drInsert["TDD3MA"] = drC["TDD3MA"] == DBNull.Value ? 0 : drC["TDD3MA"];
                    drInsert["TDD12MA"] = drC["TDD12MA"] == DBNull.Value ? 0 : drC["TDD12MA"];
                    drInsert["TDD12YGA"] = drC["TDD12YGA"] == DBNull.Value ? 0 : drC["TDD12YGA"];
                    drInsert["TDD3MB"] = drC["TDD3MB"] == DBNull.Value ? 0 : drC["TDD3MB"];
                    drInsert["TDD12MB"] = drC["TDD12MB"] == DBNull.Value ? 0 : drC["TDD12MB"];
                    drInsert["TDD12YGB"] = drC["TDD12YGB"] == DBNull.Value ? 0 : drC["TDD12YGB"];
                    ds.Tables["dtRptGSBPFF0002"].Rows.Add(drInsert);
                }
            }
            //-------------------------------------------------------------------


            ReportDocument orpt = new ReportDocument();
            orpt.Load(strFileName);
            orpt.SetDataSource(ds);

            orpt.DataDefinition.FormulaFields["Model"].Text = "'" + Session["sHeaderRpt1"].ToString() + "'";
            orpt.DataDefinition.FormulaFields["Product"].Text = "'" + Session["sHeaderRpt2"].ToString() + "'";
            orpt.DataDefinition.FormulaFields["Market"].Text = "'" + Session["sHeaderRpt3"].ToString() + "'";
            orpt.DataDefinition.FormulaFields["Target"].Text = "'" + Session["sHeaderRpt4"].ToString() + "'";

            orpt.DataDefinition.FormulaFields["fmPrintDateTime"].Text = "'" + genPrintDateTime() + "'";

            //Define user name
            string strUserFirstName = "User not define";
            if (Session["userFirstname"] != null)
            {
                strUserFirstName = Session["userFirstname"].ToString();
            }
            orpt.DataDefinition.FormulaFields["fmPrintBy"].Text = "'" + strUserFirstName.Trim() + "'";

            rptView.ReportSource = orpt;
            rptView.HasToggleGroupTreeButton = false;
        }

        private void LoadReportFF92GP(string strFileName)
        {
            //Create DataSet to contain data
            DataTable dt = ((DataTable)(Session["rptData"]));
            GSB.Report.FrontEnd.ReportLayOut.dtRptGSBPFF0002 ds = new Report.FrontEnd.ReportLayOut.dtRptGSBPFF0002();

            DataRow[] dr = dt.Select();
            foreach (DataRow drC in dr)
            {
                DataRow drInsert = ds.Tables["dtRptGSBPFF0002"].NewRow();
                if (drC["SCRRG"].ToString().Trim() == "Score Range")
                {
                    ds.Tables["dtRptGSBPFF0002"].Rows.Add(drInsert);
                    ds.Tables["dtRptGSBPFF0002"].Rows.Remove(drInsert);

                }
                // -- For Show Header Score in every Pages
                else
                {
                    drInsert["SCRRG"] = drC["SCRRG"];
                    drInsert["TDD3M"] = drC["TDD3M"] == DBNull.Value ? 0 : drC["TDD3M"];
                    drInsert["TDD12M"] = drC["TDD12M"] == DBNull.Value ? 0 : drC["TDD12M"];
                    drInsert["TDD12YG"] = drC["TDD12YG"] == DBNull.Value ? 0 : drC["TDD12YG"];
                    drInsert["TDD3MW"] = drC["TDD3MW"] == DBNull.Value ? 0 : drC["TDD3MW"];
                    drInsert["TDD12MW"] = drC["TDD12MW"] == DBNull.Value ? 0 : drC["TDD12MW"];
                    drInsert["TDD12YGW"] = drC["TDD12YGW"] == DBNull.Value ? 0 : drC["TDD12YGW"];
                    drInsert["TDD3MA"] = drC["TDD3MA"] == DBNull.Value ? 0 : drC["TDD3MA"];
                    drInsert["TDD12MA"] = drC["TDD12MA"] == DBNull.Value ? 0 : drC["TDD12MA"];
                    drInsert["TDD12YGA"] = drC["TDD12YGA"] == DBNull.Value ? 0 : drC["TDD12YGA"];
                    drInsert["TDD3MB"] = drC["TDD3MB"] == DBNull.Value ? 0 : drC["TDD3MB"];
                    drInsert["TDD12MB"] = drC["TDD12MB"] == DBNull.Value ? 0 : drC["TDD12MB"];
                    drInsert["TDD12YGB"] = drC["TDD12YGB"] == DBNull.Value ? 0 : drC["TDD12YGB"];
                    ds.Tables["dtRptGSBPFF0002"].Rows.Add(drInsert);
                }
            }
            //-------------------------------------------------------------------


            ReportDocument orpt = new ReportDocument();
            orpt.Load(strFileName);
            orpt.SetDataSource(ds);

            orpt.DataDefinition.FormulaFields["Model"].Text = "'" + Session["sHeaderRpt1"].ToString() + "'";
            orpt.DataDefinition.FormulaFields["Product"].Text = "'" + Session["sHeaderRpt2"].ToString() + "'";
            orpt.DataDefinition.FormulaFields["Market"].Text = "'" + Session["sHeaderRpt3"].ToString() + "'";
            orpt.DataDefinition.FormulaFields["Target"].Text = "'" + Session["sHeaderRpt4"].ToString() + "'";

            orpt.DataDefinition.FormulaFields["fmPrintDateTime"].Text = "'" + genPrintDateTime() + "'";

            //Define user name
            string strUserFirstName = "User not define";
            if (Session["userFirstname"] != null)
            {
                strUserFirstName = Session["userFirstname"].ToString();
            }
            orpt.DataDefinition.FormulaFields["fmPrintBy"].Text = "'" + strUserFirstName.Trim() + "'";

            rptView.ReportSource = orpt;
            ExportFormatType formatType = ExportFormatType.NoFormat;
            formatType = ExportFormatType.Excel;
            orpt.ExportToHttpResponse(formatType, Response, true, "ApprovalRateReport_" + DateTime.Now.ToString("yyyyMMdd"));
            Response.End();
        }

        //Characteristic Analysis Report
        private void LoadReportFF3(string strFileName)
        {
            //Create DataSet to contain data
            DataTable dt = ((DataTable)(Session["rptData"]));
            GSB.Report.FrontEnd.ReportLayOut.dtRptGSBPFF0003 ds = new Report.FrontEnd.ReportLayOut.dtRptGSBPFF0003();

            DataRow[] dr = dt.Select();
            foreach (DataRow drC in dr)
            {
                DataRow drInsert = ds.Tables["dtRptGSBPFF0003"].NewRow();

                drInsert["Attrib"] = (drC["Attrib"] == DBNull.Value) ? "" : drC["Attrib"];
                drInsert["dev"] = (drC["dev"] == DBNull.Value) ? 0 : drC["dev"];
                drInsert["pdev"] = (drC["pdev"] == DBNull.Value) ? 0 : drC["pdev"];
                drInsert["actv"] = (drC["actv"] == DBNull.Value) ? 0 : drC["actv"];
                drInsert["pactv"] = (drC["pactv"] == DBNull.Value) ? 0.00 : drC["pactv"];
                drInsert["chng"] = (drC["chng"] == DBNull.Value) ? 0 : drC["chng"];
                drInsert["pnt"] = (drC["pnt"] == DBNull.Value) ? 0.00 : drC["pnt"];
                drInsert["pntdf"] = (drC["pntdf"] == DBNull.Value) ? 0.00 : drC["pntdf"];


                ds.Tables["dtRptGSBPFF0003"].Rows.Add(drInsert);
            }
            //-------------------------------------------------------------------


            ReportDocument orpt = new ReportDocument();
            orpt.Load(strFileName);
            orpt.SetDataSource(ds);

            orpt.DataDefinition.FormulaFields["Model"].Text = "'" + Session["sHeaderRpt1"].ToString() + "'";
            orpt.DataDefinition.FormulaFields["Product"].Text = "'" + Session["sHeaderRpt2"].ToString() + "'";
            orpt.DataDefinition.FormulaFields["Market"].Text = "'" + Session["sHeaderRpt3"].ToString() + "'";
            orpt.DataDefinition.FormulaFields["Target"].Text = "'" + Session["sHeaderRpt4"].ToString() + "'";
            orpt.DataDefinition.FormulaFields["Characteristic"].Text = "'" + Session["sHeaderRpt5"].ToString() + "'";

            orpt.DataDefinition.FormulaFields["fmPrintDateTime"].Text = "'" + genPrintDateTime() + "'";

            //Define user name
            string strUserFirstName = "User not define";
            if (Session["userFirstname"] != null)
            {
                strUserFirstName = Session["userFirstname"].ToString();
            }
            orpt.DataDefinition.FormulaFields["fmPrintBy"].Text = "'" + strUserFirstName.Trim() + "'";


            rptView.ReportSource = orpt;
            rptView.HasToggleGroupTreeButton = false;
        }

        private void LoadReportFF93(string strFileName)
        {
            //Create DataSet to contain data
            DataTable dt = ((DataTable)(Session["rptData"]));
            GSB.Report.FrontEnd.ReportLayOut.dtRptGSBPFF0003 ds = new Report.FrontEnd.ReportLayOut.dtRptGSBPFF0003();

            DataRow[] dr = dt.Select();
            foreach (DataRow drC in dr)
            {
                DataRow drInsert = ds.Tables["dtRptGSBPFF0003"].NewRow();

                drInsert["Attrib"] = (drC["Attrib"] == DBNull.Value) ? "" : drC["Attrib"];
                drInsert["dev"] = (drC["dev"] == DBNull.Value) ? 0 : drC["dev"];
                drInsert["pdev"] = (drC["pdev"] == DBNull.Value) ? 0 : drC["pdev"];
                drInsert["actv"] = (drC["actv"] == DBNull.Value) ? 0 : drC["actv"];
                drInsert["pactv"] = (drC["pactv"] == DBNull.Value) ? 0.00 : drC["pactv"];
                drInsert["chng"] = (drC["chng"] == DBNull.Value) ? 0 : drC["chng"];
                drInsert["pnt"] = (drC["pnt"] == DBNull.Value) ? 0.00 : drC["pnt"];
                drInsert["pntdf"] = (drC["pntdf"] == DBNull.Value) ? 0.00 : drC["pntdf"];


                ds.Tables["dtRptGSBPFF0003"].Rows.Add(drInsert);
            }
            //-------------------------------------------------------------------


            ReportDocument orpt = new ReportDocument();
            orpt.Load(strFileName);
            orpt.SetDataSource(ds);

            orpt.DataDefinition.FormulaFields["Model"].Text = "'" + Session["sHeaderRpt1"].ToString() + "'";
            orpt.DataDefinition.FormulaFields["Product"].Text = "'" + Session["sHeaderRpt2"].ToString() + "'";
            orpt.DataDefinition.FormulaFields["Market"].Text = "'" + Session["sHeaderRpt3"].ToString() + "'";
            orpt.DataDefinition.FormulaFields["Target"].Text = "'" + Session["sHeaderRpt4"].ToString() + "'";
            orpt.DataDefinition.FormulaFields["Characteristic"].Text = "'" + Session["sHeaderRpt5"].ToString() + "'";

            orpt.DataDefinition.FormulaFields["fmPrintDateTime"].Text = "'" + genPrintDateTime() + "'";

            //Define user name
            string strUserFirstName = "User not define";
            if (Session["userFirstname"] != null)
            {
                strUserFirstName = Session["userFirstname"].ToString();
            }
            orpt.DataDefinition.FormulaFields["fmPrintBy"].Text = "'" + strUserFirstName.Trim() + "'";


            rptView.ReportSource = orpt;
            ExportFormatType formatType = ExportFormatType.NoFormat;
            formatType = ExportFormatType.Excel;
            orpt.ExportToHttpResponse(formatType, Response, true, "CharacteristicAnalysisReport_" + DateTime.Now.ToString("yyyyMMdd"));
            Response.End();
        }

        //Final Score Report
        private void LoadReportFF4(string strFileName)
        {
            //Create DataSet to contain data
            DataTable dt = ((DataTable)(Session["rptData"]));
            //DataTable dtFT = ((DataTable)(Session["rptDataDTFT"]));
            GSB.Report.FrontEnd.ReportLayOut.dtRptGSBPFF0004 ds = new Report.FrontEnd.ReportLayOut.dtRptGSBPFF0004();

            DataRow[] dr = dt.Select("SCRRG <> '' and SCRRG <> 'Total'");
            foreach (DataRow drC in dr)
            {
                DataRow drInsert = ds.Tables["dtRptGSBPFF0004"].NewRow();

                drInsert["SCRRG"] = (drC["SCRRG"]==DBNull.Value) ? 0 : drC["SCRRG"];
                drInsert["PAPP"] = (drC["PAPP"] == DBNull.Value) ? 0 : drC["PAPP"];
                drInsert["PREJ"] = (drC["PREJ"] == DBNull.Value) ? 0 : drC["PREJ"];
                drInsert["ACT"] = (drC["ACT"] == DBNull.Value) ? 0 : drC["ACT"];
                drInsert["appvt"] = (drC["appvt"] == DBNull.Value) ? 0.00 : drC["appvt"];
                ds.Tables["dtRptGSBPFF0004"].Rows.Add(drInsert);
            }
            //-------------------------------------------------------------------

             
            ReportDocument orpt = new ReportDocument();
            orpt.Load(strFileName);
            orpt.SetDataSource(ds);

            orpt.DataDefinition.FormulaFields["Model"].Text = "'" + Session["sHeaderRpt1"].ToString() + "'";
            orpt.DataDefinition.FormulaFields["Product"].Text = "'" + Session["sHeaderRpt2"].ToString() + "'";
            orpt.DataDefinition.FormulaFields["Market"].Text = "'" + Session["sHeaderRpt3"].ToString() + "'";
            orpt.DataDefinition.FormulaFields["Target"].Text = "'" + Session["sHeaderRpt4"].ToString() + "'";

            orpt.DataDefinition.FormulaFields["fmPrintDateTime"].Text = "'" + genPrintDateTime() + "'";

            //Define user name
            string strUserFirstName = "User not define";
            if (Session["userFirstname"] != null)
            {
                strUserFirstName = Session["userFirstname"].ToString();
            }
            orpt.DataDefinition.FormulaFields["fmPrintBy"].Text = "'" + strUserFirstName.Trim() + "'";

            //Report footer
            DataRow[] drtotal = dt.Select("SCRRG = 'Total'");

            orpt.DataDefinition.FormulaFields["TotalAccept"].Text = "'" + drtotal[0]["PAPP"] + "'";
            orpt.DataDefinition.FormulaFields["TotalReject"].Text = "'" + drtotal[0]["PREJ"].ToString() + "'";
            orpt.DataDefinition.FormulaFields["GrandTotal"].Text = "'" + drtotal[0]["ACT"].ToString() + "'";

            DataRow[] drFooter = dt.Select("SCRRG = ''");

            orpt.DataDefinition.FormulaFields["lsoCount"].Text = "'" + drFooter[1]["PAPP"].ToString() + "'";
            orpt.DataDefinition.FormulaFields["lsoPercentage"].Text = "'" + drFooter[2]["PAPP"].ToString() + "'";
            orpt.DataDefinition.FormulaFields["hsoCount"].Text = "'" + drFooter[1]["PREJ"].ToString() + "'";
            orpt.DataDefinition.FormulaFields["hsoPercentage"].Text = "'" + drFooter[2]["PREJ"].ToString() + "'";
            orpt.DataDefinition.FormulaFields["accRatePotential"].Text = "'" + drFooter[1]["appvt"].ToString() + "'";
            orpt.DataDefinition.FormulaFields["accRateActual"].Text = "'" + drFooter[2]["appvt"].ToString() + "'";


            //if (dtFT.Rows[0]["BBB"] != DBNull.Value)
            //{
            //    orpt.DataDefinition.FormulaFields["TotalAccept"].Text = "'" + dtFT.Rows[0]["BBB"].ToString() + "'";
            //}
            //else
            //{
            //    orpt.DataDefinition.FormulaFields["TotalAccept"].Text = "'0'";
            //}

            //if (dtFT.Rows[0]["CCC"] != DBNull.Value)
            //{
            //    orpt.DataDefinition.FormulaFields["TotalReject"].Text = "'" + dtFT.Rows[0]["CCC"].ToString() + "'";
            //}
            //else
            //{
            //    orpt.DataDefinition.FormulaFields["TotalReject"].Text = "'0'";
            //}


            //if (dtFT.Rows[0]["DDD"] != DBNull.Value)
            //{
            //    orpt.DataDefinition.FormulaFields["GrandTotal"].Text = "'" + dtFT.Rows[0]["DDD"].ToString() + "'";
            //}
            //else
            //{
            //    orpt.DataDefinition.FormulaFields["GrandTotal"].Text = "'0'";
            //}

            ////Report Summary
            //orpt.DataDefinition.FormulaFields["lsoCount"].Text = "'" + dtFT.Rows[3]["BBB"].ToString() + "'";
            //orpt.DataDefinition.FormulaFields["lsoPercentage"].Text = "'" + dtFT.Rows[4]["BBB"].ToString() + "'";
            //orpt.DataDefinition.FormulaFields["hsoCount"].Text = "'" + dtFT.Rows[3]["CCC"].ToString() + "'";
            //orpt.DataDefinition.FormulaFields["hsoPercentage"].Text = "'" + dtFT.Rows[4]["CCC"].ToString() + "'";
            //orpt.DataDefinition.FormulaFields["accRatePotential"].Text = "'" + dtFT.Rows[3]["EEE"].ToString() + "'";
            //orpt.DataDefinition.FormulaFields["accRateActual"].Text = "'" + dtFT.Rows[4]["EEE"].ToString() + "'";

            rptView.ReportSource = orpt;
            rptView.HasToggleGroupTreeButton = false;
        }

        private void LoadReportFF94(string strFileName)
        {
            //Create DataSet to contain data
            DataTable dt = ((DataTable)(Session["rptData"]));
            //DataTable dtFT = ((DataTable)(Session["rptDataDTFT"]));
            GSB.Report.FrontEnd.ReportLayOut.dtRptGSBPFF0004 ds = new Report.FrontEnd.ReportLayOut.dtRptGSBPFF0004();

            DataRow[] dr = dt.Select("SCRRG <> '' and SCRRG <> 'Total'");
            foreach (DataRow drC in dr)
            {
                DataRow drInsert = ds.Tables["dtRptGSBPFF0004"].NewRow();

                drInsert["SCRRG"] = (drC["SCRRG"] == DBNull.Value) ? 0 : drC["SCRRG"];
                drInsert["PAPP"] = (drC["PAPP"] == DBNull.Value) ? 0 : drC["PAPP"];
                drInsert["PREJ"] = (drC["PREJ"] == DBNull.Value) ? 0 : drC["PREJ"];
                drInsert["ACT"] = (drC["ACT"] == DBNull.Value) ? 0 : drC["ACT"];
                drInsert["appvt"] = (drC["appvt"] == DBNull.Value) ? 0.00 : drC["appvt"];
                ds.Tables["dtRptGSBPFF0004"].Rows.Add(drInsert);
            }
            //-------------------------------------------------------------------


            ReportDocument orpt = new ReportDocument();
            orpt.Load(strFileName);
            orpt.SetDataSource(ds);

            orpt.DataDefinition.FormulaFields["Model"].Text = "'" + Session["sHeaderRpt1"].ToString() + "'";
            orpt.DataDefinition.FormulaFields["Product"].Text = "'" + Session["sHeaderRpt2"].ToString() + "'";
            orpt.DataDefinition.FormulaFields["Market"].Text = "'" + Session["sHeaderRpt3"].ToString() + "'";
            orpt.DataDefinition.FormulaFields["Target"].Text = "'" + Session["sHeaderRpt4"].ToString() + "'";

            orpt.DataDefinition.FormulaFields["fmPrintDateTime"].Text = "'" + genPrintDateTime() + "'";

            //Define user name
            string strUserFirstName = "User not define";
            if (Session["userFirstname"] != null)
            {
                strUserFirstName = Session["userFirstname"].ToString();
            }
            orpt.DataDefinition.FormulaFields["fmPrintBy"].Text = "'" + strUserFirstName.Trim() + "'";

            //Report footer
            DataRow[] drtotal = dt.Select("SCRRG = 'Total'");

            orpt.DataDefinition.FormulaFields["TotalAccept"].Text = "'" + drtotal[0]["PAPP"] + "'";
            orpt.DataDefinition.FormulaFields["TotalReject"].Text = "'" + drtotal[0]["PREJ"].ToString() + "'";
            orpt.DataDefinition.FormulaFields["GrandTotal"].Text = "'" + drtotal[0]["ACT"].ToString() + "'";

            DataRow[] drFooter = dt.Select("SCRRG = ''");

            orpt.DataDefinition.FormulaFields["lsoCount"].Text = "'" + drFooter[1]["PAPP"].ToString() + "'";
            orpt.DataDefinition.FormulaFields["lsoPercentage"].Text = "'" + drFooter[2]["PAPP"].ToString() + "'";
            orpt.DataDefinition.FormulaFields["hsoCount"].Text = "'" + drFooter[1]["PREJ"].ToString() + "'";
            orpt.DataDefinition.FormulaFields["hsoPercentage"].Text = "'" + drFooter[2]["PREJ"].ToString() + "'";
            orpt.DataDefinition.FormulaFields["accRatePotential"].Text = "'" + drFooter[1]["appvt"].ToString() + "'";
            orpt.DataDefinition.FormulaFields["accRateActual"].Text = "'" + drFooter[2]["appvt"].ToString() + "'";

            rptView.ReportSource = orpt;
            ExportFormatType formatType = ExportFormatType.NoFormat;
            formatType = ExportFormatType.Excel;
            orpt.ExportToHttpResponse(formatType, Response, true, "FinalScoreReport_" + DateTime.Now.ToString("yyyyMMdd"));
            Response.End();
        }

        //Override Rate Report
        private void LoadReportFF5(string strFileName)
        {
            //Create DataSet to contain data
            DataTable dt = ((DataTable)(Session["rptData"]));
            GSB.Report.FrontEnd.ReportLayOut.dtRptGSBPFF0005 ds = new Report.FrontEnd.ReportLayOut.dtRptGSBPFF0005();

            DataRow[] dr = dt.Select();
            foreach (DataRow drC in dr)
            {
                DataRow drInsert = ds.Tables["dtRptGSBPFF0005"].NewRow();

                drInsert["category"] = drC["category"];
                drInsert["ovcnt3m"] = drC["ovcnt3m"];
                drInsert["ovcnt12m"] = drC["ovcnt12m"];
                drInsert["ovcnt12myg"] = drC["ovcnt12myg"];
                drInsert["category_Booked"] = drC["category_Booked"];
                drInsert["ovbcnt3m"] = drC["ovbcnt3m"];
                drInsert["ovbcnt12m"] = drC["ovbcnt12m"];
                drInsert["ovbcnt12myg"] = drC["ovbcnt12myg"];
                ds.Tables["dtRptGSBPFF0005"].Rows.Add(drInsert);
            }
            //-------------------------------------------------------------------


            ReportDocument orpt = new ReportDocument();
            orpt.Load(strFileName);
            orpt.SetDataSource(ds);

            orpt.DataDefinition.FormulaFields["Model"].Text = "'" + Session["sHeaderRpt1"].ToString() + "'";
            orpt.DataDefinition.FormulaFields["Product"].Text = "'" + Session["sHeaderRpt2"].ToString() + "'";
            orpt.DataDefinition.FormulaFields["Market"].Text = "'" + Session["sHeaderRpt3"].ToString() + "'";
            orpt.DataDefinition.FormulaFields["Target"].Text = "'" + Session["sHeaderRpt4"].ToString() + "'";

            orpt.DataDefinition.FormulaFields["fmPrintDateTime"].Text = "'" + genPrintDateTime() + "'";

            //Define user name
            string strUserFirstName = "User not define";
            if (Session["userFirstname"] != null)
            {
                strUserFirstName = Session["userFirstname"].ToString();
            }
            orpt.DataDefinition.FormulaFields["fmPrintBy"].Text = "'" + strUserFirstName.Trim() + "'";

            rptView.ReportSource = orpt;
            rptView.HasToggleGroupTreeButton = false;
        }

        private void LoadReportFF95(string strFileName)
        {
            //Create DataSet to contain data
            DataTable dt = ((DataTable)(Session["rptData"]));
            GSB.Report.FrontEnd.ReportLayOut.dtRptGSBPFF0005 ds = new Report.FrontEnd.ReportLayOut.dtRptGSBPFF0005();

            DataRow[] dr = dt.Select();
            foreach (DataRow drC in dr)
            {
                DataRow drInsert = ds.Tables["dtRptGSBPFF0005"].NewRow();

                drInsert["category"] = drC["category"];
                drInsert["ovcnt3m"] = drC["ovcnt3m"];
                drInsert["ovcnt12m"] = drC["ovcnt12m"];
                drInsert["ovcnt12myg"] = drC["ovcnt12myg"];
                drInsert["category_Booked"] = drC["category_Booked"];
                drInsert["ovbcnt3m"] = drC["ovbcnt3m"];
                drInsert["ovbcnt12m"] = drC["ovbcnt12m"];
                drInsert["ovbcnt12myg"] = drC["ovbcnt12myg"];
                ds.Tables["dtRptGSBPFF0005"].Rows.Add(drInsert);
            }
            //-------------------------------------------------------------------


            ReportDocument orpt = new ReportDocument();
            orpt.Load(strFileName);
            orpt.SetDataSource(ds);

            orpt.DataDefinition.FormulaFields["Model"].Text = "'" + Session["sHeaderRpt1"].ToString() + "'";
            orpt.DataDefinition.FormulaFields["Product"].Text = "'" + Session["sHeaderRpt2"].ToString() + "'";
            orpt.DataDefinition.FormulaFields["Market"].Text = "'" + Session["sHeaderRpt3"].ToString() + "'";
            orpt.DataDefinition.FormulaFields["Target"].Text = "'" + Session["sHeaderRpt4"].ToString() + "'";

            orpt.DataDefinition.FormulaFields["fmPrintDateTime"].Text = "'" + genPrintDateTime() + "'";

            //Define user name
            string strUserFirstName = "User not define";
            if (Session["userFirstname"] != null)
            {
                strUserFirstName = Session["userFirstname"].ToString();
            }
            orpt.DataDefinition.FormulaFields["fmPrintBy"].Text = "'" + strUserFirstName.Trim() + "'";

            rptView.ReportSource = orpt;
            ExportFormatType formatType = ExportFormatType.NoFormat;
            formatType = ExportFormatType.Excel;
            orpt.ExportToHttpResponse(formatType, Response, true, "OverrideRateReport_" + DateTime.Now.ToString("yyyyMMdd"));
            Response.End();
        }

        //Override Reason Report
        private void LoadReportFF6(string strFileName)
        {
            //Create DataSet to contain data
            DataTable dt = ((DataTable)(Session["rptData"]));
            GSB.Report.FrontEnd.ReportLayOut.dtRptGSBPFF0006 ds = new Report.FrontEnd.ReportLayOut.dtRptGSBPFF0006();

            DataRow[] dr = dt.Select();
            int intRowID = 1;
            foreach (DataRow drC in dr)
            {
                DataRow drInsert = ds.Tables["dtRptGSBPFF0006"].NewRow();

                drInsert["REASON_NAME"] = drC["REASON_NAME"];
                drInsert["num3m"] = drC["num3m"];
                drInsert["pct3m"] = drC["pct3m"];
                drInsert["num12m"] = drC["num12m"];
                drInsert["pct12m"] = drC["pct12m"];
                drInsert["num12myg"] = drC["num12myg"];
                drInsert["pct12myg"] = drC["pct12myg"];
                drInsert["rowid"] = intRowID;

                ds.Tables["dtRptGSBPFF0006"].Rows.Add(drInsert);
                intRowID++;
            }
            //-------------------------------------------------------------------


            ReportDocument orpt = new ReportDocument();
            orpt.Load(strFileName);
            orpt.SetDataSource(ds);

            orpt.DataDefinition.FormulaFields["Model"].Text = "'" + Session["sHeaderRpt1"].ToString() + "'";
            orpt.DataDefinition.FormulaFields["Product"].Text = "'" + Session["sHeaderRpt2"].ToString() + "'";
            orpt.DataDefinition.FormulaFields["Market"].Text = "'" + Session["sHeaderRpt3"].ToString() + "'";
            orpt.DataDefinition.FormulaFields["Target"].Text = "'" + Session["sHeaderRpt4"].ToString() + "'";

            orpt.DataDefinition.FormulaFields["fmPrintDateTime"].Text = "'" + genPrintDateTime() + "'";

            //Define user name
            string strUserFirstName = "User not define";
            if (Session["userFirstname"] != null)
            {
                strUserFirstName = Session["userFirstname"].ToString();
            }
            orpt.DataDefinition.FormulaFields["fmPrintBy"].Text = "'" + strUserFirstName.Trim() + "'";

            rptView.ReportSource = orpt;
            rptView.HasToggleGroupTreeButton = false;
        }

        private void LoadReportFF96(string strFileName)
        {
            //Create DataSet to contain data
            DataTable dt = ((DataTable)(Session["rptData"]));
            GSB.Report.FrontEnd.ReportLayOut.dtRptGSBPFF0006 ds = new Report.FrontEnd.ReportLayOut.dtRptGSBPFF0006();

            DataRow[] dr = dt.Select();
            int intRowID = 1;
            foreach (DataRow drC in dr)
            {
                DataRow drInsert = ds.Tables["dtRptGSBPFF0006"].NewRow();

                drInsert["REASON_NAME"] = drC["REASON_NAME"];
                drInsert["num3m"] = drC["num3m"];
                drInsert["pct3m"] = drC["pct3m"];
                drInsert["num12m"] = drC["num12m"];
                drInsert["pct12m"] = drC["pct12m"];
                drInsert["num12myg"] = drC["num12myg"];
                drInsert["pct12myg"] = drC["pct12myg"];
                drInsert["rowid"] = intRowID;

                ds.Tables["dtRptGSBPFF0006"].Rows.Add(drInsert);
                intRowID++;
            }
            //-------------------------------------------------------------------


            ReportDocument orpt = new ReportDocument();
            orpt.Load(strFileName);
            orpt.SetDataSource(ds);

            orpt.DataDefinition.FormulaFields["Model"].Text = "'" + Session["sHeaderRpt1"].ToString() + "'";
            orpt.DataDefinition.FormulaFields["Product"].Text = "'" + Session["sHeaderRpt2"].ToString() + "'";
            orpt.DataDefinition.FormulaFields["Market"].Text = "'" + Session["sHeaderRpt3"].ToString() + "'";
            orpt.DataDefinition.FormulaFields["Target"].Text = "'" + Session["sHeaderRpt4"].ToString() + "'";

            orpt.DataDefinition.FormulaFields["fmPrintDateTime"].Text = "'" + genPrintDateTime() + "'";

            //Define user name
            string strUserFirstName = "User not define";
            if (Session["userFirstname"] != null)
            {
                strUserFirstName = Session["userFirstname"].ToString();
            }
            orpt.DataDefinition.FormulaFields["fmPrintBy"].Text = "'" + strUserFirstName.Trim() + "'";

            rptView.ReportSource = orpt;
            ExportFormatType formatType = ExportFormatType.NoFormat;
            formatType = ExportFormatType.Excel;
            orpt.ExportToHttpResponse(formatType, Response, true, "OverrideReasonReport_" + DateTime.Now.ToString("yyyyMMdd"));
            Response.End();
        }

        private void LoadReportFF91(string strFileName)
        {

            //Create DataSet to contain data
            DataTable dt = ((DataTable)(Session["rptData"]));
            GSB.Report.FrontEnd.ReportLayOut.dtRptGSBPFF0001 ds = new Report.FrontEnd.ReportLayOut.dtRptGSBPFF0001();

            DataRow[] dr = dt.Select();
            foreach (DataRow drC in dr)
            {
                DataRow drInsert = ds.Tables["dtRptGSBPFF0001"].NewRow();

                drInsert["SCRRG"] = drC["SCRRG"];
                drInsert["DEV"] = drC["DEV"];
                drInsert["PDEV"] = drC["PDEV"];
                drInsert["ACT"] = drC["ACT"];
                drInsert["PACT"] = drC["PACT"];
                drInsert["PCHNG"] = drC["PCHNG"];
                drInsert["PRATIO"] = drC["PRATIO"];
                drInsert["WOE"] = drC["WOE"];
                drInsert["PINDEX"] = drC["PINDEX"];
                drInsert["PAPP"] = drC["PAPP"];
                drInsert["PREJ"] = drC["PREJ"];
                drInsert["Approve"] = drC["Approve"];
                drInsert["Reject"] = drC["Reject"];
                ds.Tables["dtRptGSBPFF0001"].Rows.Add(drInsert);
            }
            //-------------------------------------------------------------------


            ReportDocument orpt = new ReportDocument();
            orpt.Load(strFileName);
            orpt.SetDataSource(ds);

            orpt.DataDefinition.FormulaFields["Model"].Text = "'" + Session["sHeaderRpt1"].ToString() + "'";
            orpt.DataDefinition.FormulaFields["Product"].Text = "'" + Session["sHeaderRpt2"].ToString() + "'";
            orpt.DataDefinition.FormulaFields["Market"].Text = "'" + Session["sHeaderRpt3"].ToString() + "'";
            orpt.DataDefinition.FormulaFields["Target"].Text = "'" + Session["sHeaderRpt4"].ToString() + "'";
            orpt.DataDefinition.FormulaFields["paraSumOfPPopStabInx"].Text = "'" + Session["lblSumIndex"].ToString() + "'";
            orpt.DataDefinition.FormulaFields["fmPrintDateTime"].Text = "'" + genPrintDateTime() + "'";

            //Define user name
            string strUserFirstName = "User not define";
            if (Session["userFirstname"] != null)
            {
                strUserFirstName = Session["userFirstname"].ToString();
            }
            orpt.DataDefinition.FormulaFields["fmPrintBy"].Text = "'" + strUserFirstName.Trim() + "'";


            rptView.ReportSource = orpt;

            ExportFormatType formatType = ExportFormatType.NoFormat;
            formatType = ExportFormatType.Excel;
            orpt.ExportToHttpResponse(formatType, Response, true, "PopulationStabilityReport_" + DateTime.Now.ToString("yyyyMMdd"));
            Response.End();



        }



    }
}
