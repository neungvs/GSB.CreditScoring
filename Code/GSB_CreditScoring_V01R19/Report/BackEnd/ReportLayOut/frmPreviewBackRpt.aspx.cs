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
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
//using GSB.Report.BackEnd.ReportLayOut;

namespace GSB.Portfolio.Web.BackEnd
{
    public partial class frmPreviewBackRpt : System.Web.UI.Page
    {
        //string _strServer = ConfigurationManager.AppSettings["Server"].ToString();
        //string _strDB = ConfigurationManager.AppSettings["DataBase"].ToString();
        //string _strUserId = ConfigurationManager.AppSettings["UserId"].ToString();
        //string _strPwd = ConfigurationManager.AppSettings["Password"].ToString();


        /* Get Product Information */
        private string genPrintDateTime()
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

        protected void load_report()
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
                    _strFileName = strPath + "\\SCORINGB0001.rpt";
                    LoadReportFB1(_strFileName);
                    break;
                case "91":
                    _strFileName = strPath + "\\SCORINGB0001.rpt";
                    LoadReportFB91(_strFileName);
                    break;
                case "2":
                    _strFileName = strPath + "\\SCORINGB0002.rpt";
                    LoadReportFB2(_strFileName);
                    break;
                case "92":
                    _strFileName = strPath + "\\SCORINGB0002.rpt";
                    LoadReportFB92(_strFileName);
                    break;
                case "3":
                    _strFileName = strPath + "\\SCORINGB0003.rpt";
                    LoadReportFB3(_strFileName);
                    break;
                case "93":
                    _strFileName = strPath + "\\SCORINGB0003.rpt";
                    LoadReportFB93(_strFileName);
                    break;
                case "4":
                    _strFileName = strPath + "\\SCORINGB0004.rpt";
                    LoadReportFB4(_strFileName);
                    break;
                case "94":
                    _strFileName = strPath + "\\SCORINGB0004.rpt";
                    LoadReportFB94(_strFileName);
                    break;
                case "5":
                    _strFileName = strPath + "\\SCORINGB0005.rpt";
                    LoadReportFB5(_strFileName);
                    break;
                case "95":
                    _strFileName = strPath + "\\SCORINGB0005.rpt";
                    LoadReportFB95(_strFileName);
                    break;
                case "6":
                    _strFileName = strPath + "\\SCORINGB0006.rpt";
                    LoadReportFB6(_strFileName);
                    break;
                case "96":
                    _strFileName = strPath + "\\SCORINGB0006.rpt";
                    LoadReportFB96(_strFileName);
                    break;
                case "7":
                    _strFileName = strPath + "\\SCORINGB0007.rpt";
                    LoadReportFB7(_strFileName);
                    break;
            }
        }

        private void LoadReport(string strFileName)
        {

            //TableLogOnInfos crtableLogoninfos = new TableLogOnInfos();
            //TableLogOnInfo crtableLogoninfo = new TableLogOnInfo();
            //ConnectionInfo crConnectionInfo = new ConnectionInfo();
            //Tables CrTables = default(Tables);

            //ReportDocument orpt = new ReportDocument();
            //orpt.Load(strFileName);

            //orpt.SetDatabaseLogon(_strUserId, _strPwd, _strServer, _strDB);

            //crConnectionInfo.ServerName = _strServer;
            //crConnectionInfo.DatabaseName = _strDB;
            //crConnectionInfo.UserID = _strUserId;
            //crConnectionInfo.Password = _strPwd;

            //CrTables = orpt.Database.Tables;
            //foreach (CrystalDecisions.CrystalReports.Engine.Table CrTable in CrTables)
            //{
            //    crtableLogoninfo = CrTable.LogOnInfo;
            //    crtableLogoninfo.ConnectionInfo = crConnectionInfo;
            //    CrTable.ApplyLogOnInfo(crtableLogoninfo);

            //    //CrTable.Location = _strDB.ToString() + ".dbo." + CrTable.Location.Substring(CrTable.Location.LastIndexOf(".") + 1);
            //}
            //orpt.RecordSelectionFormula = Session["sFS"].ToString();

            //orpt.DataDefinition.FormulaFields[0].Text = "'" + Session["sHeaderRpt1"].ToString() + "'";
            //orpt.DataDefinition.FormulaFields[1].Text = "'" + Session["sHeaderRpt2"].ToString() + "'";
            //orpt.DataDefinition.FormulaFields[2].Text = "'" + Session["sHeaderRpt3"].ToString() + "'";
            //if (Session["sHeaderRpt4"].ToString() != "")
            //{ orpt.DataDefinition.FormulaFields[3].Text = "'" + Session["sHeaderRpt4"].ToString() + "'"; }

            //rptView.ReportSource = orpt;
            //rptView.HasViewList = false;
            //rptView.HasToggleGroupTreeButton = false;

        }

        private void LoadReport(string strFileName ,String SessionName)
        {
            /*
            TableLogOnInfos crtableLogoninfos = new TableLogOnInfos();
            TableLogOnInfo crtableLogoninfo = new TableLogOnInfo();
            ConnectionInfo crConnectionInfo = new ConnectionInfo();
            Tables CrTables = default(Tables);

            ReportDocument orpt = new ReportDocument();
            orpt.Load(strFileName);

            orpt.SetDatabaseLogon(_strUserId, _strPwd, _strServer, _strDB);

            crConnectionInfo.ServerName = _strServer;
            crConnectionInfo.DatabaseName = _strDB;
            crConnectionInfo.UserID = _strUserId;
            crConnectionInfo.Password = _strPwd;

            CrTables = orpt.Database.Tables;
            foreach (CrystalDecisions.CrystalReports.Engine.Table CrTable in CrTables)
            {
                crtableLogoninfo = CrTable.LogOnInfo;
                crtableLogoninfo.ConnectionInfo = crConnectionInfo;
                CrTable.ApplyLogOnInfo(crtableLogoninfo);

                //CrTable.Location = _strDB.ToString() + ".dbo." + CrTable.Location.Substring(CrTable.Location.LastIndexOf(".") + 1);
            }
            orpt.RecordSelectionFormula = Session["sFS"].ToString();

            orpt.DataDefinition.FormulaFields[0].Text = "'" + Session["sHeaderRpt1"].ToString() + "'";
            orpt.DataDefinition.FormulaFields[1].Text = "'" + Session["sHeaderRpt2"].ToString() + "'";
            orpt.DataDefinition.FormulaFields[2].Text = "'" + Session["sHeaderRpt3"].ToString() + "'";
            if (Session["sHeaderRpt4"].ToString() != "")
            { orpt.DataDefinition.FormulaFields[3].Text = "'" + Session["sHeaderRpt4"].ToString() + "'"; }

            rptView.ReportSource = orpt;
            rptView.HasViewList = false;
            rptView.HasToggleGroupTreeButton = false;
            */
        //    try
        //    {
        //        ReportDocument orpt = new ReportDocument();
        //        orpt.Load(strFileName);

        //        DataTable DT_Table = (DataTable)Session[SessionName];
        //        orpt.SetDataSource(DT_Table);
        //        rptView.ReportSource = orpt;

        //        orpt.DataDefinition.FormulaFields["Model"].Text = "'" + Session["sHeaderRpt1"].ToString() + "'";
        //        orpt.DataDefinition.FormulaFields["Product"].Text = "'" + Session["sHeaderRpt2"].ToString() + "'";
        //        orpt.DataDefinition.FormulaFields["Market"].Text = "'" + Session["sHeaderRpt3"].ToString() + "'";
        //        orpt.DataDefinition.FormulaFields["Target"].Text = "'" + Session["sHeaderRpt4"].ToString() + "'";
        //        if (SessionName == "Delinq" | SessionName == "Performance")
        //        {
        //            orpt.DataDefinition.FormulaFields["Target2"].Text = "'" + Session["sHeaderRpt5"].ToString() + "'";
        //        }
        //        orpt.DataDefinition.FormulaFields["fmPrintDateTime"].Text = "'" + Session["printDate"].ToString() + "'";
        //        orpt.DataDefinition.FormulaFields["fmPrintBy"].Text = "'" + Session["printBy"].ToString() + "'";
        //        orpt.DataDefinition.FormulaFields["modelInfo"].Text = Session["modelInfo"].ToString();
                
        //        if (SessionName == "VINTAGE")
        //        {
        //            char[] chrCom = { ',' };
        //            string[] strHeadertTmpTb = Session["StrHeader"].ToString().Split(chrCom);
                    
        //            for (int i = 0; i <= 9; i++)
        //            {
        //                orpt.DataDefinition.FormulaFields["Header" + Convert.ToString(i + 1)].Text = "'" + strHeadertTmpTb[i].ToString() + "'";
        //            }
        //            }
        //        rptView.ReportSource = orpt;
        //        rptView.HasViewList = false;
        //        rptView.HasToggleGroupTreeButton = false;

        //    }
        //    catch (Exception ex)
        //    {
        //        //nothing to show
        //    }
        }

        private void LoadLogCoReport(string strFileName)
        {
            /*
            TableLogOnInfos crtableLogoninfos = new TableLogOnInfos();
            TableLogOnInfo crtableLogoninfo = new TableLogOnInfo();
            ConnectionInfo crConnectionInfo = new ConnectionInfo();
            Tables CrTables = default(Tables);

            ReportDocument orpt = new ReportDocument();
            orpt.Load(strFileName);

            orpt.SetDatabaseLogon(_strUserId, _strPwd, _strServer, _strDB);

            crConnectionInfo.ServerName = _strServer;
            crConnectionInfo.DatabaseName = _strDB;
            crConnectionInfo.UserID = _strUserId;
            crConnectionInfo.Password = _strPwd;

            CrTables = orpt.Database.Tables;
            foreach (CrystalDecisions.CrystalReports.Engine.Table CrTable in CrTables)
            {
                crtableLogoninfo = CrTable.LogOnInfo;
                crtableLogoninfo.ConnectionInfo = crConnectionInfo;
                CrTable.ApplyLogOnInfo(crtableLogoninfo);

                //CrTable.Location = _strDB.ToString() + ".dbo." + CrTable.Location.Substring(CrTable.Location.LastIndexOf(".") + 1);
            }
            //orpt.RecordSelectionFormula = Session["sFS"].ToString();

            rptView.ReportSource = orpt;
            rptView.HasViewList = false;
            rptView.HasToggleGroupTreeButton = false;
            */
            try
            {
                ReportDocument orpt = new ReportDocument();
                orpt.Load(strFileName);
                
                DataTable DT_Table = (DataTable)Session["Logs"];
                orpt.SetDataSource(DT_Table);
                rptView.ReportSource = orpt;
            }
            catch (Exception ex)
            {
                //nothing to show
            }
        }

        private void LoadReportFB1(string strFileName)
        {

           //Create DataSet to contain data
            DataTable dt1 = ((DataTable)(Session["rptData1"]));
            GSB.Report.BackEnd.ReportLayOut.dsRptGSBPFB0001 ds1 = new GSB.Report.BackEnd.ReportLayOut.dsRptGSBPFB0001();

            DataRow[] dr1 = dt1.Select();

            foreach (DataRow drC in dr1)
            {
                DataRow drInsert = ds1.Tables[0].NewRow();
                if (drC["SCRRG"].ToString().Trim() == "Total")
                {
                    drInsert["SCRRG"] = drC["SCRRG"];
                    drInsert["CurG"] = (drC["CurG"] == DBNull.Value ? "0" : drC["CurG"]);
                    drInsert["CurB"] = (drC["CurB"] == DBNull.Value ? "0" : drC["CurB"]);
                    drInsert["CurSep"] = (drC["CurSep"] == DBNull.Value ? "0" : drC["CurSep"]);
                    drInsert["CurBRate"] = (drC["CurBRate"] == DBNull.Value ? "0.00" : drC["CurBRate"]);
                    drInsert["DevG"] = (drC["DevG"] == DBNull.Value ? "0" : drC["DevG"]);
                    drInsert["DevB"] = (drC["DevB"] == DBNull.Value ? "0" : drC["DevB"]);
                    drInsert["DevSep"] = (drC["DevSep"] == DBNull.Value ? "0" : drC["DevSep"]);
                    drInsert["DevBRate"] = (drC["DevBRate"] == DBNull.Value ? "0.00" : drC["DevBRate"]);
                    drInsert["cntGood"] = (drC["cntGood"] == DBNull.Value ? "0.00" : drC["cntGood"]);
                    drInsert["pGood"] = (drC["pGood"] == DBNull.Value ? "0.00" : drC["pGood"]);
                    drInsert["cntBad"] = (drC["cntBad"] == DBNull.Value ? "0.00" : drC["cntBad"]);
                    drInsert["pBad"] = (drC["pBad"] == DBNull.Value ? "0.00" : drC["pBad"]);
                    drInsert["cntDevG"] = (drC["cntDevG"] == DBNull.Value ? "0.00" : drC["cntDevG"]);
                    drInsert["pDevG"] = (drC["pDevG"] == DBNull.Value ? "0.00" : drC["pDevG"]);
                    drInsert["cntDevB"] = (drC["cntDevB"] == DBNull.Value ? "0.00" : drC["cntDevB"]);
                    drInsert["pDevB"] = (drC["pDevB"] == DBNull.Value ? "0.00" : drC["pDevB"]);

                }
                else
                  if (drC["SCRRG"].ToString().Trim() == "KS")
                {
                    drInsert["SCRRG"] = drC["SCRRG"];
                    drInsert["CurG"] = " ";
                    drInsert["CurB"] = " ";
                    drInsert["CurSep"] = (drC["CurSep"] == DBNull.Value ? "0.00" : drC["CurSep"]);
                    drInsert["CurBRate"] = " ";
                    drInsert["DevG"] = " ";
                    drInsert["DevB"] = " ";
                    drInsert["DevSep"] = (drC["DevSep"] == DBNull.Value ? "0.00" : drC["DevSep"]);
                    drInsert["DevBRate"] = " ";
                    drInsert["cntGood"] = "";
                    drInsert["pGood"] = "";
                    drInsert["cntBad"] = "";
                    drInsert["pBad"] = "";
                    drInsert["cntDevG"] = "";
                    drInsert["pDevG"] = "";
                    drInsert["cntDevB"] = "";
                    drInsert["pDevB"] = "";
                }
                else
                {
                    drInsert["SCRRG"] = drC["SCRRG"];
                    drInsert["CurG"] = (drC["CurG"] == DBNull.Value ? "0.00" : drC["CurG"]);
                    drInsert["CurB"] = (drC["CurB"] == DBNull.Value ? "0.00" : drC["CurB"]);
                    drInsert["CurSep"] = (drC["CurSep"] == DBNull.Value ? "0.00" : drC["CurSep"]);
                    drInsert["CurBRate"] = (drC["CurBRate"] == DBNull.Value ? "0.00" : drC["CurBRate"]);
                    drInsert["DevG"] = (drC["DevG"] == DBNull.Value ? "0.00" : drC["DevG"]);
                    drInsert["DevB"] = (drC["DevB"] == DBNull.Value ? "0.00" : drC["DevB"]);
                    drInsert["DevSep"] = (drC["DevSep"] == DBNull.Value ? "0.00" : drC["DevSep"]);
                    drInsert["DevBRate"] = (drC["DevBRate"] == DBNull.Value ? "0.00" : drC["DevBRate"]);
                    drInsert["cntGood"] = (drC["cntGood"] == DBNull.Value ? "0.00" : drC["cntGood"]);
                    drInsert["pGood"] = (drC["pGood"] == DBNull.Value ? "0.00" : drC["pGood"]);
                    drInsert["cntBad"] = (drC["cntBad"] == DBNull.Value ? "0.00" : drC["cntBad"]);
                    drInsert["pBad"] = (drC["pBad"] == DBNull.Value ? "0.00" : drC["pBad"]);
                    drInsert["cntDevG"] = (drC["cntDevG"] == DBNull.Value ? "0.00" : drC["cntDevG"]);
                    drInsert["pDevG"] = (drC["pDevG"] == DBNull.Value ? "0.00" : drC["pDevG"]);
                    drInsert["cntDevB"] = (drC["cntDevB"] == DBNull.Value ? "0.00" : drC["cntDevB"]);
                    drInsert["pDevB"] = (drC["pDevB"] == DBNull.Value ? "0.00" : drC["pDevB"]);
                }
                ds1.Tables[0].Rows.Add(drInsert);
            }

            // Footer >> Get data from Data table 2

            //DataTable dt2 = ((DataTable)(Session["rptData2"]));
            //GSB.Report.BackEnd.ReportLayOut.dsRptGSBPFB0001 ds2 = new GSB.Report.BackEnd.ReportLayOut.dsRptGSBPFB0001();
            //DataRow[] dr2 = dt2.Select();

            //foreach (DataRow drC in dr2)
            //{
            //    DataRow drInsert = ds2.Tables[0].NewRow();

            //    if (drC["SCRRG"].ToString().Trim() == "KS")
            //    {
            //        Cur_KS = decimal.Parse(drC["KS"].ToString());
            //        Dev_KS = decimal.Parse(drC["KS_dev"].ToString());
            //        ds2.Tables[0].Rows.Add(drInsert);
            //    }

            //    else
            //        ds2.Tables[0].Rows.Add(drInsert);
            //}

            //// Footer >> Get data from Data table 3
            
            //DataTable dt3 = ((DataTable)(Session["rptData3"]));
            //GSB.Report.BackEnd.ReportLayOut.dsRptGSBPFB0001 ds3 = new GSB.Report.BackEnd.ReportLayOut.dsRptGSBPFB0001();          
            
            //DataRow[] dr3 = dt3.Select();
            
            //foreach (DataRow drC in dr3)
            //{
            //    DataRow drInsert = ds3.Tables[0].NewRow();
            //    if (drC["Grand_T"].ToString().Trim() == "Grand Total")
            //    {
            //        T_CurG = decimal.Parse(drC["T_CurG"].ToString());
            //        T_CurB = decimal.Parse(drC["T_CurB"].ToString());
            //        G_B_Total = decimal.Parse(drC["G_B_Tatal"].ToString());
            //        T_Bad_Rate_C = decimal.Parse(drC["T_Bad_Rate_C"].ToString());
            //        T_YgG = double.Parse(drC["T_YgG"].ToString());
            //        T_YgB = double.Parse(drC["T_YgB"].ToString());
            //        G_Dev_Total = double.Parse(drC["G_Dev_Tatal"].ToString());
            //        T_Bad_Rate_Dev = double.Parse(drC["T_Bad_Rate_Dev"].ToString());
            //        ds3.Tables[0].Rows.Add(drInsert);
            //    }

            //    else
            //        ds3.Tables[0].Rows.Add(drInsert);
               
            //}

            //-------------------------------------------------------------------

            ReportDocument orpt = new ReportDocument();

            orpt.Load(strFileName);
            orpt.SetDataSource(ds1);
            //Paint to Header1
            orpt.DataDefinition.FormulaFields["Model"].Text = "'" + Session["sHeaderRpt1"].ToString() + "'";
            orpt.DataDefinition.FormulaFields["Product"].Text = "'" + Session["sHeaderRpt2"].ToString() + "'";
            orpt.DataDefinition.FormulaFields["Market"].Text = "'" + Session["sHeaderRpt3"].ToString() + "'";
            orpt.DataDefinition.FormulaFields["Target"].Text = "'" + Session["sHeaderRpt4"].ToString() + "'";
            orpt.DataDefinition.FormulaFields["Target2"].Text = "'" + Session["sHeaderRpt5"].ToString() + "'";
            orpt.DataDefinition.FormulaFields["fmPrintDateTime"].Text = "'" + genPrintDateTime() + "'";
            ////Paint to Footer1
            //orpt.DataDefinition.FormulaFields["Cur_KS"].Text = "'" + Cur_KS + "'";
            //orpt.DataDefinition.FormulaFields["Dev_KS"].Text = "'" + Dev_KS + "'";
            ////Paint to Footer2
            //orpt.DataDefinition.FormulaFields["T_CurG"].Text = "'" + T_CurG + "'";
            //orpt.DataDefinition.FormulaFields["T_CurB"].Text = "'" + T_CurB + "'";
            //orpt.DataDefinition.FormulaFields["G_B_Total"].Text = "'" + G_B_Total + "'";
            //orpt.DataDefinition.FormulaFields["T_Bad_Rate_C"].Text = "'" + T_Bad_Rate_C + "'";
            //orpt.DataDefinition.FormulaFields["T_YgC"].Text = "'" + T_YgG + "'";
            //orpt.DataDefinition.FormulaFields["T_YgB"].Text = "'" + T_YgB + "'";
            //orpt.DataDefinition.FormulaFields["G_Dev_Total"].Text = "'" + G_Dev_Total + "'";
            //orpt.DataDefinition.FormulaFields["T_Bad_Rate_Dev"].Text = "'" + T_Bad_Rate_Dev + "'";

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

        private void LoadReportFB91(string strFileName)
        {

            //Create DataSet to contain data
            DataTable dt1 = ((DataTable)(Session["rptData1"]));
            GSB.Report.BackEnd.ReportLayOut.dsRptGSBPFB0001 ds1 = new GSB.Report.BackEnd.ReportLayOut.dsRptGSBPFB0001();

            DataRow[] dr1 = dt1.Select();

            foreach (DataRow drC in dr1)
            {
                DataRow drInsert = ds1.Tables[0].NewRow();
                if (drC["SCRRG"].ToString().Trim() == "Total")
                {
                    drInsert["SCRRG"] = drC["SCRRG"];
                    drInsert["CurG"] = (drC["CurG"] == DBNull.Value ? "0" : drC["CurG"]);
                    drInsert["CurB"] = (drC["CurB"] == DBNull.Value ? "0" : drC["CurB"]);
                    drInsert["CurSep"] = (drC["CurSep"] == DBNull.Value ? "0" : drC["CurSep"]);
                    drInsert["CurBRate"] = (drC["CurBRate"] == DBNull.Value ? "0.00" : drC["CurBRate"]);
                    drInsert["DevG"] = (drC["DevG"] == DBNull.Value ? "0" : drC["DevG"]);
                    drInsert["DevB"] = (drC["DevB"] == DBNull.Value ? "0" : drC["DevB"]);
                    drInsert["DevSep"] = (drC["DevSep"] == DBNull.Value ? "0" : drC["DevSep"]);
                    drInsert["DevBRate"] = (drC["DevBRate"] == DBNull.Value ? "0.00" : drC["DevBRate"]);
                    drInsert["cntGood"] = (drC["cntGood"] == DBNull.Value ? "0.00" : drC["cntGood"]);
                    drInsert["pGood"] = (drC["pGood"] == DBNull.Value ? "0.00" : drC["pGood"]);
                    drInsert["cntBad"] = (drC["cntBad"] == DBNull.Value ? "0.00" : drC["cntBad"]);
                    drInsert["pBad"] = (drC["pBad"] == DBNull.Value ? "0.00" : drC["pBad"]);
                    drInsert["cntDevG"] = (drC["cntDevG"] == DBNull.Value ? "0.00" : drC["cntDevG"]);
                    drInsert["pDevG"] = (drC["pDevG"] == DBNull.Value ? "0.00" : drC["pDevG"]);
                    drInsert["cntDevB"] = (drC["cntDevB"] == DBNull.Value ? "0.00" : drC["cntDevB"]);
                    drInsert["pDevB"] = (drC["pDevB"] == DBNull.Value ? "0.00" : drC["pDevB"]);

                }
                else
                  if (drC["SCRRG"].ToString().Trim() == "KS")
                {
                    drInsert["SCRRG"] = drC["SCRRG"];
                    drInsert["CurG"] = " ";
                    drInsert["CurB"] = " ";
                    drInsert["CurSep"] = (drC["CurSep"] == DBNull.Value ? "0.00" : drC["CurSep"]);
                    drInsert["CurBRate"] = " ";
                    drInsert["DevG"] = " ";
                    drInsert["DevB"] = " ";
                    drInsert["DevSep"] = (drC["DevSep"] == DBNull.Value ? "0.00" : drC["DevSep"]);
                    drInsert["DevBRate"] = " ";
                    drInsert["cntGood"] = "";
                    drInsert["pGood"] = "";
                    drInsert["cntBad"] = "";
                    drInsert["pBad"] = "";
                    drInsert["cntDevG"] = "";
                    drInsert["pDevG"] = "";
                    drInsert["cntDevB"] = "";
                    drInsert["pDevB"] = "";
                }
                else
                {
                    drInsert["SCRRG"] = drC["SCRRG"];
                    drInsert["CurG"] = (drC["CurG"] == DBNull.Value ? "0.00" : drC["CurG"]);
                    drInsert["CurB"] = (drC["CurB"] == DBNull.Value ? "0.00" : drC["CurB"]);
                    drInsert["CurSep"] = (drC["CurSep"] == DBNull.Value ? "0.00" : drC["CurSep"]);
                    drInsert["CurBRate"] = (drC["CurBRate"] == DBNull.Value ? "0.00" : drC["CurBRate"]);
                    drInsert["DevG"] = (drC["DevG"] == DBNull.Value ? "0.00" : drC["DevG"]);
                    drInsert["DevB"] = (drC["DevB"] == DBNull.Value ? "0.00" : drC["DevB"]);
                    drInsert["DevSep"] = (drC["DevSep"] == DBNull.Value ? "0.00" : drC["DevSep"]);
                    drInsert["DevBRate"] = (drC["DevBRate"] == DBNull.Value ? "0.00" : drC["DevBRate"]);
                    drInsert["cntGood"] = (drC["cntGood"] == DBNull.Value ? "0.00" : drC["cntGood"]);
                    drInsert["pGood"] = (drC["pGood"] == DBNull.Value ? "0.00" : drC["pGood"]);
                    drInsert["cntBad"] = (drC["cntBad"] == DBNull.Value ? "0.00" : drC["cntBad"]);
                    drInsert["pBad"] = (drC["pBad"] == DBNull.Value ? "0.00" : drC["pBad"]);
                    drInsert["cntDevG"] = (drC["cntDevG"] == DBNull.Value ? "0.00" : drC["cntDevG"]);
                    drInsert["pDevG"] = (drC["pDevG"] == DBNull.Value ? "0.00" : drC["pDevG"]);
                    drInsert["cntDevB"] = (drC["cntDevB"] == DBNull.Value ? "0.00" : drC["cntDevB"]);
                    drInsert["pDevB"] = (drC["pDevB"] == DBNull.Value ? "0.00" : drC["pDevB"]);
                }
                ds1.Tables[0].Rows.Add(drInsert);
            }

            // Footer >> Get data from Data table 2

            //DataTable dt2 = ((DataTable)(Session["rptData2"]));
            //GSB.Report.BackEnd.ReportLayOut.dsRptGSBPFB0001 ds2 = new GSB.Report.BackEnd.ReportLayOut.dsRptGSBPFB0001();
            //DataRow[] dr2 = dt2.Select();

            //foreach (DataRow drC in dr2)
            //{
            //    DataRow drInsert = ds2.Tables[0].NewRow();

            //    if (drC["SCRRG"].ToString().Trim() == "KS")
            //    {
            //        Cur_KS = decimal.Parse(drC["KS"].ToString());
            //        Dev_KS = decimal.Parse(drC["KS_dev"].ToString());
            //        ds2.Tables[0].Rows.Add(drInsert);
            //    }

            //    else
            //        ds2.Tables[0].Rows.Add(drInsert);
            //}

            //// Footer >> Get data from Data table 3

            //DataTable dt3 = ((DataTable)(Session["rptData3"]));
            //GSB.Report.BackEnd.ReportLayOut.dsRptGSBPFB0001 ds3 = new GSB.Report.BackEnd.ReportLayOut.dsRptGSBPFB0001();          

            //DataRow[] dr3 = dt3.Select();

            //foreach (DataRow drC in dr3)
            //{
            //    DataRow drInsert = ds3.Tables[0].NewRow();
            //    if (drC["Grand_T"].ToString().Trim() == "Grand Total")
            //    {
            //        T_CurG = decimal.Parse(drC["T_CurG"].ToString());
            //        T_CurB = decimal.Parse(drC["T_CurB"].ToString());
            //        G_B_Total = decimal.Parse(drC["G_B_Tatal"].ToString());
            //        T_Bad_Rate_C = decimal.Parse(drC["T_Bad_Rate_C"].ToString());
            //        T_YgG = double.Parse(drC["T_YgG"].ToString());
            //        T_YgB = double.Parse(drC["T_YgB"].ToString());
            //        G_Dev_Total = double.Parse(drC["G_Dev_Tatal"].ToString());
            //        T_Bad_Rate_Dev = double.Parse(drC["T_Bad_Rate_Dev"].ToString());
            //        ds3.Tables[0].Rows.Add(drInsert);
            //    }

            //    else
            //        ds3.Tables[0].Rows.Add(drInsert);

            //}

            //-------------------------------------------------------------------

            ReportDocument orpt = new ReportDocument();

            orpt.Load(strFileName);
            orpt.SetDataSource(ds1);
            //Paint to Header1
            orpt.DataDefinition.FormulaFields["Model"].Text = "'" + Session["sHeaderRpt1"].ToString() + "'";
            orpt.DataDefinition.FormulaFields["Product"].Text = "'" + Session["sHeaderRpt2"].ToString() + "'";
            orpt.DataDefinition.FormulaFields["Market"].Text = "'" + Session["sHeaderRpt3"].ToString() + "'";
            orpt.DataDefinition.FormulaFields["Target"].Text = "'" + Session["sHeaderRpt4"].ToString() + "'";
            orpt.DataDefinition.FormulaFields["Target2"].Text = "'" + Session["sHeaderRpt5"].ToString() + "'";
            orpt.DataDefinition.FormulaFields["fmPrintDateTime"].Text = "'" + genPrintDateTime() + "'";
            ////Paint to Footer1
            //orpt.DataDefinition.FormulaFields["Cur_KS"].Text = "'" + Cur_KS + "'";
            //orpt.DataDefinition.FormulaFields["Dev_KS"].Text = "'" + Dev_KS + "'";
            ////Paint to Footer2
            //orpt.DataDefinition.FormulaFields["T_CurG"].Text = "'" + T_CurG + "'";
            //orpt.DataDefinition.FormulaFields["T_CurB"].Text = "'" + T_CurB + "'";
            //orpt.DataDefinition.FormulaFields["G_B_Total"].Text = "'" + G_B_Total + "'";
            //orpt.DataDefinition.FormulaFields["T_Bad_Rate_C"].Text = "'" + T_Bad_Rate_C + "'";
            //orpt.DataDefinition.FormulaFields["T_YgC"].Text = "'" + T_YgG + "'";
            //orpt.DataDefinition.FormulaFields["T_YgB"].Text = "'" + T_YgB + "'";
            //orpt.DataDefinition.FormulaFields["G_Dev_Total"].Text = "'" + G_Dev_Total + "'";
            //orpt.DataDefinition.FormulaFields["T_Bad_Rate_Dev"].Text = "'" + T_Bad_Rate_Dev + "'";

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
            orpt.ExportToHttpResponse(formatType, Response, true, "GoodBadSeparationReport_" + DateTime.Now.ToString("yyyyMMdd"));
            Response.End();


        }

        private void LoadReportFB2(string strFileName)
        {
           
            //Create DataSet to contain data
            DataTable dt = ((DataTable)(Session["rptData"]));
            GSB.Report.BackEnd.ReportLayOut.dsRptGSBPFB0002 ds = new GSB.Report.BackEnd.ReportLayOut.dsRptGSBPFB0002();

            DataRow[] dr = dt.Select();

            foreach (DataRow drC in dr)
            {
                DataRow drInsert = ds.Tables[0].NewRow();

                drInsert["category"] = drC["category"];
                drInsert["dt3mBM1"] = (drC["dt3mBM1"] == DBNull.Value ? "0.00" : drC["dt3mBM1"]);
                drInsert["dt12mBM1"] = (drC["dt12mBM1"] == DBNull.Value ? "0.00" : drC["dt12mBM1"]);
                drInsert["dt12mygBM1"] = (drC["dt12mygBM1"] == DBNull.Value ? "0.00" : drC["dt12mygBM1"]);
                //drInsert["dt3mBM2"] = (drC["dt3mBM2"] == DBNull.Value ? "0.00" : drC["dt3mBM2"]);
                //drInsert["dt12mBM2"] = (drC["dt12mBM2"] == DBNull.Value ? "0.00" : drC["dt12mBM2"]);
                //drInsert["dt12mygBM2"] = (drC["dt12mygBM2"] == DBNull.Value ? "0.00" : drC["dt12mygBM2"]);
                drInsert["dt3mBM3"] = (drC["dt3mBM3"] == DBNull.Value ? "0.00" : drC["dt3mBM3"]);
                drInsert["dt12mBM3"] = (drC["dt12mBM3"] == DBNull.Value ? "0.00" : drC["dt12mBM3"]);
                drInsert["dt12mygBM3"] = (drC["dt12mygBM3"] == DBNull.Value ? "0.00" : drC["dt12mygBM3"]);
                drInsert["DPD1p_3M_Y"] = (drC["DPD1p_3M_Y"] == DBNull.Value ? "0.00" : drC["DPD1p_3M_Y"]);
                drInsert["DPD1p_12M_Y"] = (drC["DPD1p_12M_Y"] == DBNull.Value ? "0.00" : drC["DPD1p_12M_Y"]);
                drInsert["DPD1p_12YG_Y"] = (drC["DPD1p_12YG_Y"] == DBNull.Value ? "0.00" : drC["DPD1p_12YG_Y"]);
                drInsert["DPD60p_3M_Y"] = (drC["DPD60p_3M_Y"] == DBNull.Value ? "0.00" : drC["DPD60p_3M_Y"]);
                drInsert["DPD60p_12M_Y"] = (drC["DPD60p_12M_Y"] == DBNull.Value ? "0.00" : drC["DPD60p_12M_Y"]);
                drInsert["DPD60p_12YG_Y"] = (drC["DPD60p_12YG_Y"] == DBNull.Value ? "0.00" : drC["DPD60p_12YG_Y"]);
                ds.Tables[0].Rows.Add(drInsert);
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

        private void LoadReportFB92(string strFileName)
        {

            //Create DataSet to contain data
            DataTable dt = ((DataTable)(Session["rptData"]));
            GSB.Report.BackEnd.ReportLayOut.dsRptGSBPFB0002 ds = new GSB.Report.BackEnd.ReportLayOut.dsRptGSBPFB0002();

            DataRow[] dr = dt.Select();

            foreach (DataRow drC in dr)
            {
                DataRow drInsert = ds.Tables[0].NewRow();

                drInsert["category"] = drC["category"];
                drInsert["dt3mBM1"] = (drC["dt3mBM1"] == DBNull.Value ? "0.00" : drC["dt3mBM1"]);
                drInsert["dt12mBM1"] = (drC["dt12mBM1"] == DBNull.Value ? "0.00" : drC["dt12mBM1"]);
                drInsert["dt12mygBM1"] = (drC["dt12mygBM1"] == DBNull.Value ? "0.00" : drC["dt12mygBM1"]);
                //drInsert["dt3mBM2"] = (drC["dt3mBM2"] == DBNull.Value ? "0.00" : drC["dt3mBM2"]);
                //drInsert["dt12mBM2"] = (drC["dt12mBM2"] == DBNull.Value ? "0.00" : drC["dt12mBM2"]);
                //drInsert["dt12mygBM2"] = (drC["dt12mygBM2"] == DBNull.Value ? "0.00" : drC["dt12mygBM2"]);
                drInsert["dt3mBM3"] = (drC["dt3mBM3"] == DBNull.Value ? "0.00" : drC["dt3mBM3"]);
                drInsert["dt12mBM3"] = (drC["dt12mBM3"] == DBNull.Value ? "0.00" : drC["dt12mBM3"]);
                drInsert["dt12mygBM3"] = (drC["dt12mygBM3"] == DBNull.Value ? "0.00" : drC["dt12mygBM3"]);
                drInsert["DPD1p_3M_Y"] = (drC["DPD1p_3M_Y"] == DBNull.Value ? "0.00" : drC["DPD1p_3M_Y"]);
                drInsert["DPD1p_12M_Y"] = (drC["DPD1p_12M_Y"] == DBNull.Value ? "0.00" : drC["DPD1p_12M_Y"]);
                drInsert["DPD1p_12YG_Y"] = (drC["DPD1p_12YG_Y"] == DBNull.Value ? "0.00" : drC["DPD1p_12YG_Y"]);
                drInsert["DPD60p_3M_Y"] = (drC["DPD60p_3M_Y"] == DBNull.Value ? "0.00" : drC["DPD60p_3M_Y"]);
                drInsert["DPD60p_12M_Y"] = (drC["DPD60p_12M_Y"] == DBNull.Value ? "0.00" : drC["DPD60p_12M_Y"]);
                drInsert["DPD60p_12YG_Y"] = (drC["DPD60p_12YG_Y"] == DBNull.Value ? "0.00" : drC["DPD60p_12YG_Y"]);
                ds.Tables[0].Rows.Add(drInsert);
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
            orpt.ExportToHttpResponse(formatType, Response, true, "EarlyPerformanceScoreReport_" + DateTime.Now.ToString("yyyyMMdd"));
            Response.End();

        }


        private void LoadReportFB3(string strFileName)
        {
            //Create DataSet to contain data
            DataTable dt = ((DataTable)(Session["rptData"]));
            GSB.Report.BackEnd.ReportLayOut.dsRptGSBPFB0003 ds = new GSB.Report.BackEnd.ReportLayOut.dsRptGSBPFB0003();

            DataRow[] dr = dt.Select();
            
            foreach (DataRow drC in dr)
            {
                DataRow drInsert = ds.Tables[0].NewRow();
                drInsert["prdmnt"] = drC["prdmnt"];
              
                if (drC["bm1"].ToString().Trim() == "")
                {
                    drInsert["bm1"] = "0.00";
                }
                else
                {
                    drInsert["bm1"] = drC["bm1"];
                }

                if (drC["bm2"].ToString().Trim() == "")
                {
                    drInsert["bm2"] = "0.00";
                }
                else
                {
                    drInsert["bm2"] = drC["bm2"];
                }
                if (drC["bm3"].ToString().Trim() == "")
                {
                    drInsert["bm3"] = "0.00";
                }
                else
                {
                    drInsert["bm3"] = drC["bm3"];
                }
                if (drC["bm4"].ToString().Trim() == "")
                {
                    drInsert["bm4"] = "0.00";
                }
                else
                {
                    drInsert["bm4"] = drC["bm4"];
                }
                if (drC["bm5"].ToString().Trim() == "")
                {
                    drInsert["bm5"] = "0.00";
                }
                else
                {
                    drInsert["bm5"] = drC["bm5"];
                }
                if (drC["bm6"].ToString().Trim() == "")
                {
                    drInsert["bm6"] = "0.00";
                }
                else
                {
                    drInsert["bm6"] = drC["bm6"];
                }
                if (drC["bm7"].ToString().Trim() == "")
                {
                    drInsert["bm7"] = "0.00";
                }
                else
                {
                    drInsert["bm7"] = drC["bm7"];
                }
                if (drC["bm8"].ToString().Trim() == "")
                {
                    drInsert["bm8"] = "0.00";
                }
                else
                {
                    drInsert["bm8"] = drC["bm8"];
                }
                if (drC["bm9"].ToString().Trim() == "")
                {
                    drInsert["bm9"] = "0.00";
                }
                else
                {
                    drInsert["bm9"] = drC["bm9"];
                }
                if (drC["bm0"].ToString().Trim() == "")
                {
                    drInsert["bm0"] = "0.00";
                }
                else
                {
                    drInsert["bm0"] = drC["bm0"];
                }

                ds.Tables[0].Rows.Add(drInsert);
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

        private void LoadReportFB93(string strFileName)
        {
            //Create DataSet to contain data
            DataTable dt = ((DataTable)(Session["rptData"]));
            GSB.Report.BackEnd.ReportLayOut.dsRptGSBPFB0003 ds = new GSB.Report.BackEnd.ReportLayOut.dsRptGSBPFB0003();

            DataRow[] dr = dt.Select();

            foreach (DataRow drC in dr)
            {
                DataRow drInsert = ds.Tables[0].NewRow();
                drInsert["prdmnt"] = drC["prdmnt"];

                if (drC["bm1"].ToString().Trim() == "")
                {
                    drInsert["bm1"] = "0.00";
                }
                else
                {
                    drInsert["bm1"] = drC["bm1"];
                }

                if (drC["bm2"].ToString().Trim() == "")
                {
                    drInsert["bm2"] = "0.00";
                }
                else
                {
                    drInsert["bm2"] = drC["bm2"];
                }
                if (drC["bm3"].ToString().Trim() == "")
                {
                    drInsert["bm3"] = "0.00";
                }
                else
                {
                    drInsert["bm3"] = drC["bm3"];
                }
                if (drC["bm4"].ToString().Trim() == "")
                {
                    drInsert["bm4"] = "0.00";
                }
                else
                {
                    drInsert["bm4"] = drC["bm4"];
                }
                if (drC["bm5"].ToString().Trim() == "")
                {
                    drInsert["bm5"] = "0.00";
                }
                else
                {
                    drInsert["bm5"] = drC["bm5"];
                }
                if (drC["bm6"].ToString().Trim() == "")
                {
                    drInsert["bm6"] = "0.00";
                }
                else
                {
                    drInsert["bm6"] = drC["bm6"];
                }
                if (drC["bm7"].ToString().Trim() == "")
                {
                    drInsert["bm7"] = "0.00";
                }
                else
                {
                    drInsert["bm7"] = drC["bm7"];
                }
                if (drC["bm8"].ToString().Trim() == "")
                {
                    drInsert["bm8"] = "0.00";
                }
                else
                {
                    drInsert["bm8"] = drC["bm8"];
                }
                if (drC["bm9"].ToString().Trim() == "")
                {
                    drInsert["bm9"] = "0.00";
                }
                else
                {
                    drInsert["bm9"] = drC["bm9"];
                }
                if (drC["bm0"].ToString().Trim() == "")
                {
                    drInsert["bm0"] = "0.00";
                }
                else
                {
                    drInsert["bm0"] = drC["bm0"];
                }

                ds.Tables[0].Rows.Add(drInsert);
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
            orpt.ExportToHttpResponse(formatType, Response, true, "VintageAnalysisReport_" + DateTime.Now.ToString("yyyyMMdd"));
            Response.End();

        }


        private void LoadReportFB4(string strFileName)
        {
            //Create DataSet to contain data
            DataTable dt = ((DataTable)(Session["rptData"]));
            GSB.Report.BackEnd.ReportLayOut.dsRptGSBPFB0004 ds = new GSB.Report.BackEnd.ReportLayOut.dsRptGSBPFB0004();

            DataRow[] dr = dt.Select();

            foreach (DataRow drC in dr)
            {
                DataRow drInsert = ds.Tables[0].NewRow();
                drInsert["category"] = drC["category"];
                drInsert["ApproveAccounts"] = (drC["ApproveAccounts"] == DBNull.Value ? "0.00" : drC["ApproveAccounts"]);
                drInsert["apptacc"] = (drC["apptacc"] == DBNull.Value ? "0.00" : drC["apptacc"]);
                drInsert["ActiveAccounts"] = (drC["ActiveAccounts"] == DBNull.Value ? "0.00" : drC["ActiveAccounts"]);
                drInsert["atvacpt"] = (drC["atvacpt"] == DBNull.Value ? "0.00" : drC["atvacpt"]);
                drInsert["B1_Flag_Y"] = (drC["B1_Flag_Y"] == DBNull.Value ? "0.00" : drC["B1_Flag_Y"]);
                drInsert["BM1"] = (drC["BM1"] == DBNull.Value ? "0.00" : drC["BM1"]);
                drInsert["B2_Flag_Y"] = (drC["B2_Flag_Y"] == DBNull.Value ? "0.00" : drC["B2_Flag_Y"]);
                drInsert["BM2"] = (drC["BM2"] == DBNull.Value ? "0.00" : drC["BM2"]);
                drInsert["B3_Flag_Y"] = (drC["B3_Flag_Y"] == DBNull.Value ? "0.00" : drC["B3_Flag_Y"]);
                drInsert["BM3"] = (drC["BM3"] == DBNull.Value ? "0.00" : drC["BM3"]);

                ds.Tables[0].Rows.Add(drInsert);
            }
            //-------------------------------------------------------------------

            ReportDocument orpt = new ReportDocument();
            orpt.Load(strFileName);
            orpt.SetDataSource(ds);

            orpt.DataDefinition.FormulaFields["Model"].Text = "'" + Session["sHeaderRpt1"].ToString() + "'";
            orpt.DataDefinition.FormulaFields["Product"].Text = "'" + Session["sHeaderRpt2"].ToString() + "'";
            orpt.DataDefinition.FormulaFields["Market"].Text = "'" + Session["sHeaderRpt3"].ToString() + "'";
            orpt.DataDefinition.FormulaFields["Target"].Text = "'" + Session["sHeaderRpt4"].ToString() + "'";
            orpt.DataDefinition.FormulaFields["Target2"].Text = "'" + Session["sHeaderRpt5"].ToString() + "'";
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

        private void LoadReportFB94(string strFileName)
        {
            //Create DataSet to contain data
            DataTable dt = ((DataTable)(Session["rptData"]));
            GSB.Report.BackEnd.ReportLayOut.dsRptGSBPFB0004 ds = new GSB.Report.BackEnd.ReportLayOut.dsRptGSBPFB0004();

            DataRow[] dr = dt.Select();

            foreach (DataRow drC in dr)
            {
                DataRow drInsert = ds.Tables[0].NewRow();
                drInsert["category"] = drC["category"];
                drInsert["ApproveAccounts"] = (drC["ApproveAccounts"] == DBNull.Value ? "0.00" : drC["ApproveAccounts"]);
                drInsert["apptacc"] = (drC["apptacc"] == DBNull.Value ? "0.00" : drC["apptacc"]);
                drInsert["ActiveAccounts"] = (drC["ActiveAccounts"] == DBNull.Value ? "0.00" : drC["ActiveAccounts"]);
                drInsert["atvacpt"] = (drC["atvacpt"] == DBNull.Value ? "0.00" : drC["atvacpt"]);
                drInsert["B1_Flag_Y"] = (drC["B1_Flag_Y"] == DBNull.Value ? "0.00" : drC["B1_Flag_Y"]);
                drInsert["BM1"] = (drC["BM1"] == DBNull.Value ? "0.00" : drC["BM1"]);
                drInsert["B2_Flag_Y"] = (drC["B2_Flag_Y"] == DBNull.Value ? "0.00" : drC["B2_Flag_Y"]);
                drInsert["BM2"] = (drC["BM2"] == DBNull.Value ? "0.00" : drC["BM2"]);
                drInsert["B3_Flag_Y"] = (drC["B3_Flag_Y"] == DBNull.Value ? "0.00" : drC["B3_Flag_Y"]);
                drInsert["BM3"] = (drC["BM3"] == DBNull.Value ? "0.00" : drC["BM3"]);

                ds.Tables[0].Rows.Add(drInsert);
            }
            //-------------------------------------------------------------------

            ReportDocument orpt = new ReportDocument();
            orpt.Load(strFileName);
            orpt.SetDataSource(ds);

            orpt.DataDefinition.FormulaFields["Model"].Text = "'" + Session["sHeaderRpt1"].ToString() + "'";
            orpt.DataDefinition.FormulaFields["Product"].Text = "'" + Session["sHeaderRpt2"].ToString() + "'";
            orpt.DataDefinition.FormulaFields["Market"].Text = "'" + Session["sHeaderRpt3"].ToString() + "'";
            orpt.DataDefinition.FormulaFields["Target"].Text = "'" + Session["sHeaderRpt4"].ToString() + "'";
            orpt.DataDefinition.FormulaFields["Target2"].Text = "'" + Session["sHeaderRpt5"].ToString() + "'";
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
            orpt.ExportToHttpResponse(formatType, Response, true, "DelinquencyDistributionReport_" + DateTime.Now.ToString("yyyyMMdd"));
            Response.End();

        }

        private void LoadReportFB5(string strFileName)
        {
            //Create DataSet to contain data
            DataTable dt = ((DataTable)(Session["rptData"]));
            GSB.Report.BackEnd.ReportLayOut.dsRptGSBPFB0005 ds = new GSB.Report.BackEnd.ReportLayOut.dsRptGSBPFB0005();

            DataRow[] dr = dt.Select();

            foreach (DataRow drC in dr)
            {
                DataRow drInsert = ds.Tables[0].NewRow();
                drInsert["scrange"] = drC["scrange"];
                drInsert["totcnt"] = (drC["totcnt"] == DBNull.Value ? "0" : drC["totcnt"]);
                drInsert["TOTAMT"] = (drC["TOTAMT"] == DBNull.Value ? "0.000" : drC["TOTAMT"]);
                drInsert["NPLCount"] = (drC["NPLCount"] == DBNull.Value ? "0" : drC["NPLCount"]);
                drInsert["PNPL"] = (drC["PNPL"] == DBNull.Value ? "0.00" : drC["PNPL"]);
                drInsert["BDAMT"] = (drC["BDAMT"] == DBNull.Value ? "0.000" : drC["BDAMT"]);
                drInsert["PAMT"] = (drC["PAMT"] == DBNull.Value ? "0.000000" : drC["PAMT"]);
                drInsert["cumulativeamount"] = (drC["cumulativeamount"] == DBNull.Value ? "0.000000" : drC["cumulativeamount"]);
                ds.Tables[0].Rows.Add(drInsert);
            }
            //-------------------------------------------------------------------

            ReportDocument orpt = new ReportDocument();
            orpt.Load(strFileName);
            orpt.SetDataSource(ds);

            orpt.DataDefinition.FormulaFields["Model"].Text = "'" + Session["sHeaderRpt1"].ToString() + "'";
            orpt.DataDefinition.FormulaFields["Product"].Text = "'" + Session["sHeaderRpt2"].ToString() + "'";
            orpt.DataDefinition.FormulaFields["Market"].Text = "'" + Session["sHeaderRpt3"].ToString() + "'";
            orpt.DataDefinition.FormulaFields["Target"].Text = "'" + Session["sHeaderRpt4"].ToString() + "'";
            orpt.DataDefinition.FormulaFields["RegionName"].Text = "'" + Session["sHeaderRpt5"].ToString() + "'";
            orpt.DataDefinition.FormulaFields["DistrictName"].Text = "'" + Session["sHeaderRpt6"].ToString() + "'";
            orpt.DataDefinition.FormulaFields["BranchName"].Text = "'" + Session["sHeaderRpt7"].ToString() + "'";
            orpt.DataDefinition.FormulaFields["Target2"].Text = "'" + Session["sHeaderRpt8"].ToString() + "'";
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

        private void LoadReportFB95(string strFileName)
        {
            //Create DataSet to contain data
            DataTable dt = ((DataTable)(Session["rptData"]));
            GSB.Report.BackEnd.ReportLayOut.dsRptGSBPFB0005 ds = new GSB.Report.BackEnd.ReportLayOut.dsRptGSBPFB0005();

            DataRow[] dr = dt.Select();

            foreach (DataRow drC in dr)
            {
                DataRow drInsert = ds.Tables[0].NewRow();
                drInsert["scrange"] = drC["scrange"];
                drInsert["totcnt"] = (drC["totcnt"] == DBNull.Value ? "0" : drC["totcnt"]);
                drInsert["TOTAMT"] = (drC["TOTAMT"] == DBNull.Value ? "0.000" : drC["TOTAMT"]);
                drInsert["NPLCount"] = (drC["NPLCount"] == DBNull.Value ? "0" : drC["NPLCount"]);
                drInsert["PNPL"] = (drC["PNPL"] == DBNull.Value ? "0.00" : drC["PNPL"]);
                drInsert["BDAMT"] = (drC["BDAMT"] == DBNull.Value ? "0.000" : drC["BDAMT"]);
                drInsert["PAMT"] = (drC["PAMT"] == DBNull.Value ? "0.000000" : drC["PAMT"]);
                drInsert["cumulativeamount"] = (drC["cumulativeamount"] == DBNull.Value ? "0.000000" : drC["cumulativeamount"]);
                ds.Tables[0].Rows.Add(drInsert);
            }
            //-------------------------------------------------------------------

            ReportDocument orpt = new ReportDocument();
            orpt.Load(strFileName);
            orpt.SetDataSource(ds);

            orpt.DataDefinition.FormulaFields["Model"].Text = "'" + Session["sHeaderRpt1"].ToString() + "'";
            orpt.DataDefinition.FormulaFields["Product"].Text = "'" + Session["sHeaderRpt2"].ToString() + "'";
            orpt.DataDefinition.FormulaFields["Market"].Text = "'" + Session["sHeaderRpt3"].ToString() + "'";
            orpt.DataDefinition.FormulaFields["Target"].Text = "'" + Session["sHeaderRpt4"].ToString() + "'";
            orpt.DataDefinition.FormulaFields["RegionName"].Text = "'" + Session["sHeaderRpt5"].ToString() + "'";
            orpt.DataDefinition.FormulaFields["DistrictName"].Text = "'" + Session["sHeaderRpt6"].ToString() + "'";
            orpt.DataDefinition.FormulaFields["BranchName"].Text = "'" + Session["sHeaderRpt7"].ToString() + "'";
            orpt.DataDefinition.FormulaFields["Target2"].Text = "'" + Session["sHeaderRpt8"].ToString() + "'";
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
            orpt.ExportToHttpResponse(formatType, Response, true, "PortfolioScorePerformanceReport_" + DateTime.Now.ToString("yyyyMMdd"));
            Response.End();

        }


        private void LoadReportFB6(string strFileName)
        {
            //Create DataSet to contain data
            DataTable dt = ((DataTable)(Session["rptData"]));
            GSB.Report.BackEnd.ReportLayOut.dsRptGSBPFB0006 ds = new GSB.Report.BackEnd.ReportLayOut.dsRptGSBPFB0006();

            DataRow[] dr = dt.Select();

            foreach (DataRow drC in dr)
            {
                DataRow drInsert = ds.Tables[0].NewRow();
                drInsert["scrdif"] = drC["scrdif"];
                drInsert["totcnt"] = (drC["totcnt"] == DBNull.Value ? "0" : drC["totcnt"]);
                drInsert["TOTAMT"] = (drC["TOTAMT"] == DBNull.Value ? "0.00" : drC["TOTAMT"]);
                drInsert["NPLCount"] = (drC["NPLCount"] == DBNull.Value ? "0" : drC["NPLCount"]);
                drInsert["PNPL"] = (drC["PNPL"] == DBNull.Value ? "0.00" : drC["PNPL"]);
                drInsert["BDAMT"] = (drC["BDAMT"] == DBNull.Value ? "0.000" : drC["BDAMT"]);
                drInsert["PAMT"] = (drC["PAMT"] == DBNull.Value ? "0.000000" : drC["PAMT"]);
                drInsert["cumulativeamount"] = (drC["cumulativeamount"] == DBNull.Value ? "0.000000" : drC["cumulativeamount"]);
                ds.Tables[0].Rows.Add(drInsert);
            }
            //-------------------------------------------------------------------

            ReportDocument orpt = new ReportDocument();
            orpt.Load(strFileName);
            orpt.SetDataSource(ds);

            orpt.DataDefinition.FormulaFields["Model"].Text = "'" + Session["sHeaderRpt1"].ToString() + "'";
            orpt.DataDefinition.FormulaFields["Product"].Text = "'" + Session["sHeaderRpt2"].ToString() + "'";
            orpt.DataDefinition.FormulaFields["Market"].Text = "'" + Session["sHeaderRpt3"].ToString() + "'";
            orpt.DataDefinition.FormulaFields["Target"].Text = "'" + Session["sHeaderRpt4"].ToString() + "'";
            orpt.DataDefinition.FormulaFields["RegionName"].Text = "'" + Session["sHeaderRpt5"].ToString() + "'";
            orpt.DataDefinition.FormulaFields["DistrictName"].Text = "'" + Session["sHeaderRpt6"].ToString() + "'";
            orpt.DataDefinition.FormulaFields["BranchName"].Text = "'" + Session["sHeaderRpt7"].ToString() + "'";
            orpt.DataDefinition.FormulaFields["fmPrintDateTime"].Text = "'" + genPrintDateTime() + "'";
            orpt.DataDefinition.FormulaFields["fmPrintDateTime"].Text = "'" + genPrintDateTime() + "'";
            orpt.DataDefinition.FormulaFields["Target2"].Text = "'" + Session["sHeaderRpt8"].ToString() + "'";
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

        private void LoadReportFB96(string strFileName)
        {
            //Create DataSet to contain data
            DataTable dt = ((DataTable)(Session["rptData"]));
            GSB.Report.BackEnd.ReportLayOut.dsRptGSBPFB0006 ds = new GSB.Report.BackEnd.ReportLayOut.dsRptGSBPFB0006();

            DataRow[] dr = dt.Select();

            foreach (DataRow drC in dr)
            {
                DataRow drInsert = ds.Tables[0].NewRow();
                drInsert["scrdif"] = drC["scrdif"];
                drInsert["totcnt"] = (drC["totcnt"] == DBNull.Value ? "0" : drC["totcnt"]);
                drInsert["TOTAMT"] = (drC["TOTAMT"] == DBNull.Value ? "0.00" : drC["TOTAMT"]);
                drInsert["NPLCount"] = (drC["NPLCount"] == DBNull.Value ? "0" : drC["NPLCount"]);
                drInsert["PNPL"] = (drC["PNPL"] == DBNull.Value ? "0.00" : drC["PNPL"]);
                drInsert["BDAMT"] = (drC["BDAMT"] == DBNull.Value ? "0.000" : drC["BDAMT"]);
                drInsert["PAMT"] = (drC["PAMT"] == DBNull.Value ? "0.000000" : drC["PAMT"]);
                drInsert["cumulativeamount"] = (drC["cumulativeamount"] == DBNull.Value ? "0.000000" : drC["cumulativeamount"]);
                ds.Tables[0].Rows.Add(drInsert);
            }
            //-------------------------------------------------------------------

            ReportDocument orpt = new ReportDocument();
            orpt.Load(strFileName);
            orpt.SetDataSource(ds);

            orpt.DataDefinition.FormulaFields["Model"].Text = "'" + Session["sHeaderRpt1"].ToString() + "'";
            orpt.DataDefinition.FormulaFields["Product"].Text = "'" + Session["sHeaderRpt2"].ToString() + "'";
            orpt.DataDefinition.FormulaFields["Market"].Text = "'" + Session["sHeaderRpt3"].ToString() + "'";
            orpt.DataDefinition.FormulaFields["Target"].Text = "'" + Session["sHeaderRpt4"].ToString() + "'";
            orpt.DataDefinition.FormulaFields["RegionName"].Text = "'" + Session["sHeaderRpt5"].ToString() + "'";
            orpt.DataDefinition.FormulaFields["DistrictName"].Text = "'" + Session["sHeaderRpt6"].ToString() + "'";
            orpt.DataDefinition.FormulaFields["BranchName"].Text = "'" + Session["sHeaderRpt7"].ToString() + "'";
            orpt.DataDefinition.FormulaFields["fmPrintDateTime"].Text = "'" + genPrintDateTime() + "'";
            orpt.DataDefinition.FormulaFields["fmPrintDateTime"].Text = "'" + genPrintDateTime() + "'";
            orpt.DataDefinition.FormulaFields["Target2"].Text = "'" + Session["sHeaderRpt8"].ToString() + "'";
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
            orpt.ExportToHttpResponse(formatType, Response, true, "PortfolioGradePerformanceReport_" + DateTime.Now.ToString("yyyyMMdd"));
            Response.End();


        }


        private void LoadReportFB7(string strFileName)
        {
            //Create DataSet to contain data
            DataTable dt = ((DataTable)(Session["rptData"]));
            GSB.Report.BackEnd.ReportLayOut.dsRptGSBPFB0007 ds = new GSB.Report.BackEnd.ReportLayOut.dsRptGSBPFB0007();

            DataRow[] dr = dt.Select();

            foreach (DataRow drC in dr)
            {
                DataRow drInsert = ds.Tables[0].NewRow();
                drInsert["OID"] = drC["OID"];
                drInsert["DTM"] = drC["DTM"];
                drInsert["LTYPE"] = drC["LTYPE"];
                drInsert["LSTYPE"] = drC["LSTYPE"];
                drInsert["MODEL"] = drC["MODEL"];
                drInsert["LMKT"] = drC["LMKT"];
                drInsert["MES"] = drC["MES"];
                
                ds.Tables[0].Rows.Add(drInsert);
            }
            //-------------------------------------------------------------------

            ReportDocument orpt = new ReportDocument();
            orpt.Load(strFileName);
            orpt.SetDataSource(ds);

            orpt.DataDefinition.FormulaFields["Model"].Text = "'" + Session["sHeaderRpt1"].ToString() + "'";
            orpt.DataDefinition.FormulaFields["Model Version"].Text = "'" + Session["sHeaderRpt2"].ToString() + "'";
            orpt.DataDefinition.FormulaFields["AsOfDate"].Text = "'" + Session["sHeaderRpt3"].ToString() + "'";
           

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
    }


}
