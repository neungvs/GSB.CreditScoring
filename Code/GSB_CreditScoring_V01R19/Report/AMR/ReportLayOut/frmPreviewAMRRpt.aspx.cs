using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

namespace GSB.Report.AMR.ReportLayOut
{
    public partial class frmPreviewAMRRpt : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            load_report();
        }

        private void load_report()
        {
            string strPath = MapPath(".//");
            string _strFileName = string.Empty;

            switch (Request.QueryString["ReportId"].ToString())
            {
                case "AMR1":
                    //Population Stabilities Report
                    _strFileName = strPath + "\\SCORINGF0001.rpt";
                    LoadReportFF1(_strFileName);
                    break;
                case "AMR91":
                    //Population Stabilities Report
                    _strFileName = strPath + "\\SCORINGF0001.rpt";
                    LoadReportFF91(_strFileName);
                    break;
                case "AMR2":
                    //Population Stabilities Report
                    _strFileName = strPath + "\\SCORINGF0002.rpt";
                    LoadReportFF2(_strFileName);
                    break;
                case "AMR92":
                    //Population Stabilities Report
                    _strFileName = strPath + "\\SCORINGF0002.rpt";
                    LoadReportFF92(_strFileName);
                    break;

            }
        }

        private void LoadReportFF1(string strFileName)
        {
            DataTable dt = ((DataTable)(Session["rptData"]));
            GSB.Report.AMR.ReportLayOut.dtRptAMR001 ds = new Report.AMR.ReportLayOut.dtRptAMR001();

            DataRow[] dr = dt.Select();
            foreach (DataRow drC in dr)
            {
                DataRow drInsert = ds.Tables["dtRptAMR001"].NewRow();

                drInsert["SUBJECT"] = drC["SUBJECT"];
                drInsert["APPROVENUMBER"] = drC["APPROVENUMBER"];
                drInsert["APPROVEPERCENT"] = drC["APPROVEPERCENT"];
                drInsert["REJECTNUMBER"] = drC["REJECTNUMBER"];
                drInsert["REJECTPERCENT"] = drC["REJECTPERCENT"];
                drInsert["OTHERNUMBER"] = drC["OTHERNUMBER"];
                drInsert["OTHERPERCENT"] = drC["OTHERPERCENT"];
                drInsert["TOTALNUMBER"] = drC["TOTALNUMBER"];
                drInsert["TOTALPERCENT"] = drC["TOTALPERCENT"];
                ds.Tables["dtRptAMR001"].Rows.Add(drInsert);
            }

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

        private void LoadReportFF91(string strFileName)
        {
            DataTable dt = ((DataTable)(Session["rptData"]));
            GSB.Report.AMR.ReportLayOut.dtRptAMR001 ds = new Report.AMR.ReportLayOut.dtRptAMR001();

            DataRow[] dr = dt.Select();
            foreach (DataRow drC in dr)
            {
                DataRow drInsert = ds.Tables["dtRptAMR001"].NewRow();

                drInsert["SUBJECT"] = drC["SUBJECT"];
                drInsert["APPROVENUMBER"] = drC["APPROVENUMBER"];
                drInsert["APPROVEPERCENT"] = drC["APPROVEPERCENT"];
                drInsert["REJECTNUMBER"] = drC["REJECTNUMBER"];
                drInsert["REJECTPERCENT"] = drC["REJECTPERCENT"];
                drInsert["OTHERNUMBER"] = drC["OTHERNUMBER"];
                drInsert["OTHERPERCENT"] = drC["OTHERPERCENT"];
                drInsert["TOTALNUMBER"] = drC["TOTALNUMBER"];
                drInsert["TOTALPERCENT"] = drC["TOTALPERCENT"];
                ds.Tables["dtRptAMR001"].Rows.Add(drInsert);
            }

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
            orpt.ExportToHttpResponse(formatType, Response, true, "CreditScoringModel_" + DateTime.Now.ToString("yyyyMMdd"));
            Response.End();


        }

        private void LoadReportFF2(string strFileName)
        {
            DataTable dt = ((DataTable)(Session["rptData"]));
            GSB.Report.AMR.ReportLayOut.dtRptAMR002 ds = new Report.AMR.ReportLayOut.dtRptAMR002();

            DataRow[] dr = dt.Select();
            foreach (DataRow drC in dr)
            {
                DataRow drInsert = ds.Tables["dtRptAMR002"].NewRow();

                drInsert["ScoreBand"] = drC["ScoreBand"];
                drInsert["DefaultCount"] = Convert.ToDecimal(drC["DefaultCount"]).ToString("0.00");
                drInsert["NonDefaultCount"] = Convert.ToDecimal(drC["NonDefaultCount"]).ToString("0.00");
                drInsert["DefaultCapture"] = Convert.ToDecimal(drC["DefaultCapture"]).ToString("0.00");
                drInsert["NonDefaultCapture"] = Convert.ToDecimal(drC["NonDefaultCapture"]).ToString("0.00");
                drInsert["CumCapDefault"] = Convert.ToDecimal(drC["CumCapDefault"]).ToString("0.00");
                drInsert["CumCapNonDefault"] = Convert.ToDecimal(drC["CumCapNonDefault"]).ToString("0.00");
                drInsert["KS"] = Convert.ToDecimal(drC["KS"]).ToString("0.00");
                ds.Tables["dtRptAMR002"].Rows.Add(drInsert);
            }

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

        private void LoadReportFF92(string strFileName)
        {
            DataTable dt = ((DataTable)(Session["rptData"]));
            GSB.Report.AMR.ReportLayOut.dtRptAMR002 ds = new Report.AMR.ReportLayOut.dtRptAMR002();

            DataRow[] dr = dt.Select();
            foreach (DataRow drC in dr)
            {
                DataRow drInsert = ds.Tables["dtRptAMR002"].NewRow();

                drInsert["ScoreBand"] = drC["ScoreBand"];
                drInsert["DefaultCount"] = Convert.ToDecimal(drC["DefaultCount"]).ToString("0.00");
                drInsert["NonDefaultCount"] = Convert.ToDecimal(drC["NonDefaultCount"]).ToString("0.00");
                drInsert["DefaultCapture"] = Convert.ToDecimal(drC["DefaultCapture"]).ToString("0.00");
                drInsert["NonDefaultCapture"] = Convert.ToDecimal(drC["NonDefaultCapture"]).ToString("0.00");
                drInsert["CumCapDefault"] = Convert.ToDecimal(drC["CumCapDefault"]).ToString("0.00");
                drInsert["CumCapNonDefault"] = Convert.ToDecimal(drC["CumCapNonDefault"]).ToString("0.00");
                drInsert["KS"] = Convert.ToDecimal(drC["KS"]).ToString("0.00");
                ds.Tables["dtRptAMR002"].Rows.Add(drInsert);
            }

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
            orpt.ExportToHttpResponse(formatType, Response, true, "Performance_" + DateTime.Now.ToString("yyyyMMdd"));
            Response.End();


        }

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

    }
}