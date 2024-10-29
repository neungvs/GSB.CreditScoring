using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GSB.Class;
using System.Text;
using System.Globalization;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace GSB.Report.FrontEnd
{
    public partial class OverrideReasonReport : System.Web.UI.Page
    {
        SQLToDataTable conn = new SQLToDataTable();
        string appQuery1;
        #region Page_Load

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                LoadYearMonth();
                LoadModel();
                //LoadLoan();
                //LoadModelVersion();
            }
        }

        #endregion Page_Load

        #region Private Method

        private void LoadYearMonth()
        {
            int ychis, ybud;

            ychis = DateTime.Now.Year;

            if (ychis < 2500)
            {
                ybud = ychis + 543;
            }
            else
            {
                ybud = ychis;
                ychis = ychis - 543;
            }


            ddlOpenDateYear.Items.Clear();

            for (int i = 0; i < 10; i++)
            {
                ddlOpenDateYear.Items.Insert(i, new ListItem((ybud - i).ToString(), (ychis - i).ToString()));
            }

            ddlOpenDateMonth.Items.Clear();

            ddlOpenDateMonth.DataSource = GetMonth();
            ddlOpenDateMonth.DataBind();

        }

        //private void LoadModelVersion()
        //{

        //    ListItem item2 = new ListItem(" Version1 ", "null");
        //    ddlModelVersion.Items.Add(item2);
        //    ddlModelVersion.SelectedValue = "null";
        //}

        private void LoadModel()
        {
            string Query = string.Format("select distinct " +
                            "CONVERT(int,MODEL_CD) [MODEL_CD],  " +
                            "MODEL_CD +' - '+ MODEL_NAME [MODEL_NAME] from LN_MODEL where ACTIVE_FLAG = '1' and MODEL_CD not in ('005','006') " +
                            "union select null, 'โปรดเลือก...' " +
                            "order by MODEL_CD ASC");

            ddlModel.DataSource = conn.ExcuteSQL(Query);
            ddlModel.DataBind();
            ddlSubType.Items.Insert(0, new ListItem("โปรดเลือก...", string.Empty));
            ddlMarketFrm.Items.Insert(0, new ListItem("โปรดเลือก...", string.Empty));
            ddlMarketTo.Items.Insert(0, new ListItem("โปรดเลือก...", string.Empty));
        }

        protected void LoadLoan(object sender, EventArgs e)
        {
            string Query = "";

            if (ddlModel.SelectedItem.Value == "7")
            {
                Query = string.Format("select distinct " +
"a.LOAN_CD [LOAN_CD],  " +
"a.LOAN_CD+' - '+a.LOAN_NAME [LOAN_NAME] from ST_LOANTYPE a left join [dbo].[ST_MKTDFTMYMO] b on a.LOAN_CD = b.loantype where ACTIVE_FLAG = '1' and b.loantype is not null " +
"union select null, 'โปรดเลือก...' " +
"order by LOAN_CD ASC");
            }
            else
            {
                Query = string.Format("select distinct " +
"a.LOAN_CD [LOAN_CD],  " +
"a.LOAN_CD+' - '+a.LOAN_NAME [LOAN_NAME] from ST_LOANTYPE a left join [dbo].[ST_MKTDFTMYMO] b on a.LOAN_CD = b.loantype where ACTIVE_FLAG = '1' and b.loantype is null " +
"union select null, 'โปรดเลือก...' " +
"order by LOAN_CD ASC");
            }



            ddlLoan.DataSource = conn.ExcuteSQL(Query);
            ddlLoan.DataBind();
            ddlLoan.Enabled = true;
            ddlSubType.Items.Insert(0, new ListItem("โปรดเลือก...", string.Empty));
        }

        private void DataTableToExcel()
        {
            DataTable dttb = (DataTable)ViewState["DataTable1"];
            
            string filename = string.Format("GSB-CS_OverrideReasonReport_(Date_{0}-Time_{1}).xls", DateTime.Now.ToString("dd-MM-yyyy"), DateTime.Now.ToString("HH-mm-ss"));
            StringWriter tw = new StringWriter();
            DataGrid dgGrid = new DataGrid();

            tw.Write(string.Format("Override Reason Report"));
            tw.Write(tw.NewLine);
            tw.Write(string.Format("ประเภทโมเดล :| {0} ", ddlModel.SelectedItem.Text.ToString()));
            tw.Write(tw.NewLine);
            //tw.Write(string.Format("Model Version :| {0} ", ddlModelVersion.SelectedItem.Value));
            //tw.Write(tw.NewLine);
            tw.Write(string.Format("ประเภทสินเชื่อ :| {0} ", ddlLoan.SelectedItem.Text.ToString()));
            tw.Write(tw.NewLine);
            tw.Write(string.Format("ประเภทสินเชื่อย่อย :| {0} ", ddlSubType.SelectedItem.Text.ToString()));
            tw.Write(tw.NewLine);
            tw.Write(string.Format("Market Code :| {0} ถึง {1} ", ddlMarketFrm.SelectedItem.Text.ToString(), ddlMarketTo.SelectedItem.Text.ToString()));
            tw.Write(tw.NewLine);
            tw.Write(string.Format("วันที่เปิดใบคำขอสินเชื่อ :| {0} ", ""));
            tw.Write(tw.NewLine);
            tw.Write(string.Format("Export Date :| {0} : {1} ", DateTime.Now.ToString("dd-MM-yyyy"), DateTime.Now.ToString("HH-mm-ss")));
            tw.Write(tw.NewLine);
            tw.Write("ผู้พิมพ์ " + ExportUtility.getCurrentUserId);
            tw.Write(tw.NewLine);

            int iColCount = new int();
            iColCount = dttb.Columns.Count;

            tw.Write("Category|Past 3 Months||Past 6 Months||Past 12 Months");
            tw.Write(tw.NewLine);
            tw.Write("|Number of Loans| % of Total Overrides|Number of Loans| % of Total Overrides|Number of Loans| % of Total Overrides");

            tw.Write(tw.NewLine);

            // Now write all the rows.
            foreach (DataRow dr in dttb.Rows)
            {
                switch (dr[1].ToString())
                {
                    case "*** Low-Side Overrides ***":
                        tw.Write("*** Low-Side Overrides ***");
                        tw.Write(tw.NewLine);
                        break;
                    case "*** High-Side Overrides ***":
                        tw.Write("*** High-Side Overrides ***");
                        tw.Write(tw.NewLine);
                        break;
                    default:
                        for (int i = 0; i < iColCount; i++)
                        {
                            if (!Convert.IsDBNull(dr[i]))
                            {
                                tw.Write(dr[i].ToString());
                            }
                            else
                            {
                                tw.Write("0");
                            }
                            if (i < iColCount - 1)
                            {
                                tw.Write("|");
                            }
                        }
                        tw.Write(tw.NewLine);
                        break;
                }
            }

            Response.ContentType = "text/rtf; charset=UTF-8";
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename + "");
            Response.AppendHeader("Content-Encoding", "UTF8");
            this.EnableViewState = false;
            Response.BinaryWrite(System.Text.Encoding.UTF8.GetPreamble());

            Response.Write(tw.ToString());
            Response.End();
        }

        #endregion Private Method

        #region Protected Method

        protected void LoadSubType(object sender, EventArgs e)
        {
            string Query = "";

            if (ddlModel.SelectedItem.Value == "7")
            {
                Query = string.Format("select distinct " +
                                         "STYPE_CD [STYPE_CD], " +
                                         "STYPE_CD+' - '+STYPER_NAME [STYPER_NAME] from ST_LOANSTYPE a left join [dbo].[ST_MKTDFTMYMO] b on a.STYPE_CD = b.subtype where ACTIVE_FLAG = '1' and LOAN_CD = '{0}' and b.subtype is not null " +
                                         "union select null, 'โปรดเลือก...' " +
                                         "order by STYPE_CD ASC", ddlLoan.SelectedItem.Value);
            }
            else
            {
                Query = string.Format("select distinct " +
                         "STYPE_CD [STYPE_CD], " +
                         "STYPE_CD+' - '+STYPER_NAME [STYPER_NAME] from ST_LOANSTYPE a left join [dbo].[ST_MKTDFTMYMO] b on a.STYPE_CD = b.subtype where ACTIVE_FLAG = '1' and LOAN_CD = '{0}' and b.subtype is null " +
                         "union select null, 'โปรดเลือก...' " +
                         "order by STYPE_CD ASC", ddlLoan.SelectedItem.Value);
            }




            ddlSubType.Items.Clear();
            ddlSubType.DataSource = conn.ExcuteSQL(Query);
            ddlSubType.DataBind();
            ddlSubType.Enabled = true;

            ddlMarketFrm.Enabled = false;
            ddlMarketFrm.Items.Clear();
            ddlMarketFrm.Items.Insert(0, new ListItem("โปรดเลือก...", string.Empty));
            ddlMarketTo.Enabled = false;
            ddlMarketTo.Items.Clear();
            ddlMarketTo.Items.Insert(0, new ListItem("โปรดเลือก...", string.Empty));
        }
        //protected void LoadVersion(object sender, EventArgs e)
        //{
        //    string sql = "";
        //    sql = string.Format("select MODEL_NAME+'_V1' as [Model_V_NAME] from LN_MODEL where ACTIVE_FLAG = '1' and");
        //    sql += "  [MODEL_CD]=" + ddlModel.SelectedValue + " ";


        //    ddlModelVersion.DataSource = conn.ExcuteSQL(sql);
        //    ddlModelVersion.DataTextField = "Model_V_NAME";
        //    ddlModelVersion.DataValueField = "Model_V_NAME";
        //    ddlModelVersion.DataBind();

        //    LoadLoan();
        //}
        protected void LoadMarket(object sender, EventArgs e)
        {
            string Query = "";

            if (ddlModel.SelectedItem.Value == "7")
            {
                Query = string.Format("select distinct " +
             "MARKET_CD [MARKET_CD], " +
             "MARKET_CD+' - '+MARKET_NAME [MARKET_NAME] from ST_LOANMKT a left join [dbo].[ST_MKTDFTMYMO] b on MARKET_CD = b.marketcode where ACTIVE_FLAG = '1' and LOAN_CD = '{0}' and STYPE_CD = '{1}' and b.marketcode is not null  " +
             "union select null, '00000 - โปรดเลือก...' " +
             "order by MARKET_CD ASC",
             ddlLoan.SelectedItem.Value, ddlSubType.SelectedItem.Value);
            }
            else
            {
                Query = string.Format("select distinct " +
"MARKET_CD [MARKET_CD], " +
"MARKET_CD+' - '+MARKET_NAME [MARKET_NAME] from ST_LOANMKT a left join [dbo].[ST_MKTDFTMYMO] b on MARKET_CD = b.marketcode where ACTIVE_FLAG = '1' and LOAN_CD = '{0}' and STYPE_CD = '{1}' and b.marketcode is null  " +
"union select null, '00000 - โปรดเลือก...' " +
"order by MARKET_CD ASC",
ddlLoan.SelectedItem.Value, ddlSubType.SelectedItem.Value);
            }



            ddlMarketFrm.Items.Clear();
            ddlMarketFrm.DataSource = conn.ExcuteSQL(Query);
            ddlMarketFrm.DataBind();
            ddlMarketFrm.Enabled = true;
            ddlMarketTo.Items.Clear();
            ddlMarketTo.DataSource = conn.ExcuteSQL(Query);
            ddlMarketTo.DataBind();
            ddlMarketTo.Enabled = true;
        }
        protected void Search_Click(object sender, EventArgs e)
        {
            IFormatProvider format = new CultureInfo("en-GB");
            DateTime dateOpenLoan = DateTime.MinValue;

            StringBuilder sqlQuery = new StringBuilder();
            string mktqrf = "";
            string mktqrt = "";
            DateTime start = new DateTime();
            DateTime start12yg = new DateTime();
            DateTime end3m = new DateTime();
            DateTime end12yg = new DateTime();

            if (ddlLoan.SelectedIndex <= 0 || ddlSubType.SelectedIndex <= 0 || ddlMarketFrm.SelectedIndex <= 0 || ddlMarketTo.SelectedIndex <= 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScript", "appPop('โปรดเลือกตัวเลือก','กรุณาเลือกเงื่อนไขการค้นหาให้ครบถ้วน ')", true);
                return;
            }

            int monthstart = ddlOpenDateMonth.SelectedIndex;

            if (monthstart < 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScript", "appPop('วันที่เปิดใบคำขอสินเชื่อไม่ถูกต้อง?','เลือกวันที่เปิดใบคำขอไม่ถูกต้อง')", true);
                return;
            }

            start = DateTime.Parse("01/" + (ddlOpenDateMonth.SelectedIndex + 1).ToString("00") + "/" + ddlOpenDateYear.SelectedValue.ToString());
            start12yg = start.AddMonths(-5);
            end3m = start.AddMonths(-2);
            end12yg = start.AddMonths(-11);

            //if (!(txtOpenDate.Text == ""))
            //{
            //    start = DateTime.Parse((txtOpenDate.Text.ToString().Substring(0, 6) + (Convert.ToInt32(txtOpenDate.Text.Substring(6, 4).ToString()) - 543).ToString()));
            //    start12yg = start.AddMonths(-5);
            //    end3m = start.AddMonths(-2);
            //    end12yg = start.AddMonths(-11);
            //}
            //else
            //{
            //    start = DateTime.Now;
            //    start12yg = start.AddMonths(-5);
            //    end3m = start.AddMonths(-2);
            //    end12yg = start.AddMonths(-11);
            //}
            if (ddlLoan.SelectedItem.Value != "")
                sqlQuery.Append(String.Format(" AND (LOAN_CD = '{0}')", ddlLoan.SelectedItem.Value));

            if (ddlSubType.SelectedItem.Value != "")
                sqlQuery.Append(String.Format(" AND (STYPE_CD = '{0}')", ddlSubType.SelectedItem.Value));

            if (ddlMarketFrm.SelectedIndex <= 0 || ddlMarketTo.SelectedIndex <= 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScript", "appPop('โปรดเลือกตัวเลือก','โปรดเลือก  Market Code ก่อน')", true);
                return;
            }

            if (ddlMarketFrm.SelectedItem.Value != "")
            {
                sqlQuery.Append(String.Format(" AND (MARKET_CD >= '{0}')", ddlMarketFrm.SelectedItem.Value));
                mktqrf = String.Format(" AND (MARKET_CD >= '{0}')", ddlMarketFrm.SelectedItem.Value);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScript", "appPop('โปรดเลือกตัวเลือก','โปรดเลือก  Market Code ก่อน')", true);
            }

            if (ddlMarketTo.SelectedItem.Value != "")
            {
                sqlQuery.Append(String.Format(" AND (MARKET_CD <= '{0}')", ddlMarketTo.SelectedItem.Value));
                mktqrt = String.Format(" AND (MARKET_CD <= '{0}')", ddlMarketTo.SelectedItem.Value);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScript", "appPop('โปรดเลือกตัวเลือก','โปรดเลือกสินเชื่อย่อย ถึง อย่างน้อย 1 ค่า')", true);
            }
            
            if (ddlModel.SelectedIndex < 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScript", "appPop('กรุณาเลือกข้อมูลให้ถูกต้อง')", true);
            }
            else
            {
                if (ddlLoan.SelectedIndex < 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScript", "appPop('กรุณาเลือกข้อมูลให้ถูกต้อง')", true);
                }
                else
                {
                    if (ddlSubType.SelectedIndex < 0)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScript", "appPop('กรุณาเลือกข้อมูลให้ถูกต้อง')", true);
                    }
                    else
                    {
                        if (ddlMarketFrm.SelectedItem.Value != "")
                        {
                            if (ddlMarketTo.SelectedItem.Value != "")
                            {
                                int st1 = Convert.ToInt32(ddlMarketFrm.SelectedItem.Value);
                                int st2 = Convert.ToInt32(ddlMarketTo.SelectedItem.Value);

                                if (st1 <= st2)
                                {
                                appQuery1 = string.Format("	select distinct *	" +
        "	into #main_program	" +
        "	from	" +
        "	(	" +
        "	select APP_NO,MODEL,LOAN_CD,SCORE,CBS_STATUS,STYPE_CD,MARKET_CD,DW_SIGN_DATE,APP_DATE,cut_off as scr,	" +
        "	CASE WHEN (score>=cut_off) then 'Y' else 'N' end as scapproval_flag,	" +
        "	case when CBS_STATUS in (3,6,7) then 'Y' 	" +
        "	when CBS_STATUS in (4,5) then 'N' 	" +
        "	when CBS_STATUS in (1,2) OR CBS_STATUS is null then '<>' 	" +
        "	end as Approval_Flag	" +
        "	,CBS_REASON_CD	" +
        "	, LEFT(CBS_REASON_CD,1) as CBS_REASON	" +
        "	,CONVERT(VARCHAR,YEAR(APP_DATE))+'-'+RIGHT('00'+CONVERT(VARCHAR,MONTH(APP_DATE)),2) AS MYAppDate		" +
        "	from	" +
        "	(	" +
        "	SELECT a.*,b.score,b.model,	" +
        "	(select cut_off from ST_SCORERANGE y where y.ltype= {0} and y.lstype = '{1}' ) as cut_off,CBS_REASON_CD	" +
        "	from LN_APP a,LN_GRADE b,CBS_LN_APP c 	" +
        "	where a.APP_NO = b.APP_NO	" +
        "	and a.app_no = c.CBS_APP_NO 	" +
        "	 and LOAN_CD = '{0}' and STYPE_CD = '{1}'{2}{3} 	" +
        "	) lnapp	" +
        "	left join 	" +
        "	( 	" +
        "	select * 	" +
        "	from	" +
        "	(	" +
        "	select [CBS_APP_NO], DW_SIGN_DATE,LAST_UPDATE_DTM	" +
        "	from [dbo].[DWH_BTFILE] A	" +
        "	where convert(int,(substring(right([LAST_UPDATE_DTM],7),4,4) + substring(right([LAST_UPDATE_DTM],7),1,2))) 	" +
        "	between convert(int,substring('{7}',1,4)+substring('{7}',6,2))	" +
        "	and 	" +
        "	convert(int,substring('{4}',1,4)+substring('{4}',6,2))	" +
        //"	AND LAST_UPDATE_DTM=(SELECT MAX(LAST_UPDATE_DTM) FROM DWH_BTFILE B WHERE A.CBS_APP_NO=B.CBS_APP_NO)	" +
        "   AND substring([LAST_UPDATE_DTM],7,4)+'-'+ substring([LAST_UPDATE_DTM],4,2)" +
        "   =(SELECT MAX(substring(LAST_UPDATE_DTM,7,4)+'-'+substring([LAST_UPDATE_DTM],4,2)) FROM DWH_BTFILE B WHERE A.CBS_APP_NO=B.CBS_APP_NO)" +
        "	) DW_BFILE_T	" +
        "	) gsb2T	" +
        "	on gsb2T.CBS_APP_NO = lnapp.APP_NO	" +
        "	) gsb_DWFILE_LNAPP	" +
        "	select CBS_REASON_CD,count(*) as cnt	" +
        "	into #3mAccept	" +
        "	from #main_program	" +
        "	where scapproval_flag='N' 	" +
        "	AND Approval_flag ='Y' 	" +
        "	and MyAppDate between 	substring('{6}',1,4)+'-'+ substring('{6}',6,2) and  substring('{4}',1,4)+'-'+ substring('{4}',6,2)  " +
        "	and CBS_STATUS IN (3,6,7) 	" +
        "	and CBS_REASON='A'	" +
        "	group by CBS_REASON_CD	" +
        "	select sum(cnt) as Total	" +
        "	into #total_3m_accept	" +
        "	from #3mAccept	" +
        "	select CBS_REASON_CD,count(*) as cnt	" +
        "	into #6mAccept	" +
        "	from #main_program	" +
        "	where scapproval_flag='N' 	" +
        "	AND Approval_flag ='Y' 	" +
        "	and MyAppDate between 	substring('{5}',1,4)+'-'+ substring('{5}',6,2) and  substring('{4}',1,4)+'-'+ substring('{4}',6,2)  " +
        "	and CBS_STATUS IN (3,6,7) 	" +
        "	and CBS_REASON='A'	" +
        "	group by CBS_REASON_CD	" +
        "	select sum(cnt) as Total	" +
        "	into #total_6m_accept	" +
        "	from #6mAccept	" +
        "	select CBS_REASON_CD,count(*) as cnt	" +
        "	into #12mAccept	" +
        "	from #main_program	" +
        "	where scapproval_flag='N' 	" +
        "	AND Approval_flag ='Y' 	" +
        "	and MyAppDate between 	substring('{7}',1,4)+'-'+ substring('{7}',6,2) and  substring('{4}',1,4)+'-'+ substring('{4}',6,2)  " +
        "	and CBS_STATUS IN (3,6,7) 	" +
        "	and CBS_REASON='A'	" +
        "	group by CBS_REASON_CD	" +
        "	select sum(cnt) as Total	" +
        "	into #total_12m_accept	" +
        "	from #12mAccept	" +
        "	SELECT distinct [CBS_REASON_CD] as CBS_REASON_CD	" +
        "	into #HA	" +
        "	FROM [CBS_LN_APP]	" +
        "	where LEFT(CBS_REASON_CD,1)='A'	" +
        "	order by [CBS_REASON_CD]	" +
        "	select CBS_REASON_CD,count(*) as cnt	" +
        "	into #3mReject	" +
        "	from #main_program	" +
        "	where scapproval_flag='Y' 	" +
        "	AND Approval_flag ='N' 	" +
        "	and MyAppDate between 	substring('{6}',1,4)+'-'+ substring('{6}',6,2) and  substring('{4}',1,4)+'-'+ substring('{4}',6,2)  " +
        "	and CBS_STATUS IN (4,5) 	" +
        "	and CBS_REASON='R'	" +
        "	group by CBS_REASON_CD	" +
        "	select sum(cnt) as Total	" +
        "	into #total_3m_Reject	" +
        "	from #3mReject	" +
        "	select CBS_REASON_CD,count(*) as cnt	" +
        "	into #6mReject	" +
        "	from #main_program	" +
        "	where scapproval_flag='Y' 	" +
        "	AND Approval_flag ='N' 	" +
        "	and MyAppDate between 	substring('{5}',1,4)+'-'+ substring('{5}',6,2) and  substring('{4}',1,4)+'-'+ substring('{4}',6,2)  " +
        "	and CBS_STATUS IN (4,5) 	" +
        "	and CBS_REASON='R'	" +
        "	group by CBS_REASON_CD	" +
        "	select sum(cnt) as Total	" +
        "	into #total_6m_Reject	" +
        "	from #6mReject	" +
        "	select CBS_REASON_CD,count(*) as cnt	" +
        "	into #12mReject	" +
        "	from #main_program	" +
        "	where scapproval_flag='Y' 	" +
        "	AND Approval_flag ='N' 	" +
        "	and MyAppDate between 	substring('{7}',1,4)+'-'+ substring('{7}',6,2) and  substring('{4}',1,4)+'-'+ substring('{4}',6,2)  " +
        "	and CBS_STATUS IN (4,5) 	" +
        "	and CBS_REASON='R'	" +
        "	group by CBS_REASON_CD	" +
        "	select sum(cnt) as Total	" +
        "	into #total_12m_Reject	" +
        "	from #12mReject	" +
        "	SELECT distinct [CBS_REASON_CD] as CBS_REASON_CD	" +
        "	into #HR	" +
        "	FROM [CBS_LN_APP]	" +
        "	where LEFT(CBS_REASON_CD,1)='R'	" +
        "	order by [CBS_REASON_CD]	" +
        "	select REASON_NAME	" +
        "	,dbo.comma_format(convert(varchar,num3m)) as num3m	" +
        "	,convert(varchar,convert(decimal(15,2),pct3m)) as pct3m	" +
        "	,dbo.comma_format(convert(varchar,num12m))as num12m	" +
        "	,convert(varchar,convert(decimal(15,2),pct12m)) as pct12m	" +
        "	,dbo.comma_format(convert(varchar,num12myg)) as num12myg	" +
        "	,convert(varchar,convert(decimal(15,2),pct12myg)) as pct12myg	" +
        "	into #ORR_Report	" +
        "	from	" +
        "	(	" +
        "	select 2 as seq,RHA.REASON_NAME	" +
        "	,case when (RHB.T3mAccept is null) then 0 else RHB.T3mAccept end as num3m	" +
        "	,case when (RHB.[%3mAccpt] is null) then 0 else RHB.[%3mAccpt] end as pct3m	" +
        "	,case when (RHB.T6mAccept is null) then 0 else RHB.T6mAccept end as num12m	" +
        "	,case when (RHB.[%6mAccpt] is null) then 0 else RHB.[%6mAccpt] end as pct12m	" +
        "	,case when (RHB.T12mAccept is null) then 0 else RHB.T12mAccept end as num12myg	" +
        "	,case when (RHB.[%12mAccpt] is null) then 0 else RHB.[%12mAccpt] end as pct12myg	" +
        "	from	" +
        "	(	" +
        "	select REASON_CD as CBS_REASON_CD,(REASON_CD + ' - ' + REASON_NAME ) as REASON_NAME 	" +
        "	 from LN_REASON	" +
        "	 where left(REASON_CD,1)='A'	" +
        "	) RHA	" +
        "	left join	" +
        "	(	" +
        "	select AAA.*,CC.T12mAccept,CC.[%12mAccpt]	" +
        "	from	" +
        "	(	" +
        "	select AA.*,BB.T6mAccept,BB.[%6mAccpt]	" +
        "	from	" +
        "	(	" +
        "	select HA.CBS_REASON_CD,A3m.T3mAccept,A3m.[%3mAccpt]	" +
        "	 from	" +
        "	(	" +
        "	select * from #HA	" +
        "	) HA	" +
        "	left join	" +
        "	(	" +
        "	select A.CBS_REASON_CD as CBS_REASON_CD,A.cnt as [T3mAccept],convert(decimal(15,3),A.cnt)/B.Total*100.0 as [%3mAccpt]	" +
        "	from #3mAccept A,#total_3m_accept B	" +
        "	) A3m	" +
        "	on HA.CBS_REASON_CD=A3m.CBS_REASON_CD	" +
        "	) AA	" +
        "	left join	" +
        "	(	" +
        "	select A.CBS_REASON_CD as CBS_REASON_CD,A.cnt as [T6mAccept],convert(decimal(15,3),A.cnt)/B.Total*100.0 as [%6mAccpt]	" +
        "	from #6mAccept A,#total_6m_accept B	" +
        "	) BB	" +
        "	on AA.CBS_REASON_CD=BB.CBS_REASON_CD	" +
        "	) AAA	" +
        "	left join	" +
        "	(select A.CBS_REASON_CD as CBS_REASON_CD,A.cnt as [T12mAccept],convert(decimal(15,3),A.cnt)/B.Total*100.0 as [%12mAccpt]	" +
        "	from #12mAccept A,#total_12m_accept B	" +
        "	) CC	" +
        "	on AAA.CBS_REASON_CD=CC.CBS_REASON_CD	" +
        "	) RHB	" +
        "	on RHA.CBS_REASON_CD=RHB.CBS_REASON_CD	" +
        "	union	" +
        "	select 22 as seq,RHA.REASON_NAME	" +
        "	,case when (RHB.T3mReject is null) then 0 else RHB.T3mReject end as num3m	" +
        "	,case when (RHB.[%3mReject] is null) then 0 else RHB.[%3mReject] end as pct3m	" +
        "	,case when (RHB.T6mReject is null) then 0 else RHB.T6mReject end as num12m	" +
        "	,case when (RHB.[%6mReject] is null) then 0 else RHB.[%6mReject] end as pct12m	" +
        "	,case when (RHB.T12mReject is null) then 0 else RHB.T12mReject end as num12myg	" +
        "	,case when (RHB.[%12mReject] is null) then 0 else RHB.[%12mReject] end as pct12myg	" +
        "	from	" +
        "	(	" +
        "	select REASON_CD as CBS_REASON_CD,(REASON_CD + ' - ' + REASON_NAME ) as REASON_NAME 	" +
        "	 from LN_REASON	" +
        "	 where left(REASON_CD,1)='R'	" +
        "	) RHA	" +
        "	left join	" +
        "	(	" +
        "	select AAA.*,CC.T12mReject,CC.[%12mReject]	" +
        "	from	" +
        "	(	" +
        "	select AA.*,BB.T6mReject,BB.[%6mReject]	" +
        "	from	" +
        "	(	" +
        "	select HR.CBS_REASON_CD,A3m.T3mReject,A3m.[%3mReject]	" +
        "	 from	" +
        "	(	" +
        "	select * from #HR	" +
        "	) HR	" +
        "	left join	" +
        "	(	" +
        "	select A.CBS_REASON_CD as CBS_REASON_CD,A.cnt as [T3mReject],convert(decimal(15,3),A.cnt)/B.Total*100.0 as [%3mReject]	" +
        "	from #3mReject A,#total_3m_Reject B	" +
        "	) A3m	" +
        "	on HR.CBS_REASON_CD=A3m.CBS_REASON_CD	" +
        "	) AA	" +
        "	left join	" +
        "	(	" +
        "	select A.CBS_REASON_CD as CBS_REASON_CD,A.cnt as [T6mReject],convert(decimal(15,3),A.cnt)/B.Total*100.0 as [%6mReject]	" +
        "	from #6mReject A,#total_6m_Reject B	" +
        "	) BB	" +
        "	on AA.CBS_REASON_CD=BB.CBS_REASON_CD	" +
        "	) AAA	" +
        "	left join	" +
        "	(select A.CBS_REASON_CD as CBS_REASON_CD,A.cnt as [T12mReject],convert(decimal(15,3),A.cnt)/B.Total*100.0 as [%12mReject]	" +
        "	from #12mReject A,#total_12m_Reject B	" +
        "	) CC	" +
        "	on AAA.CBS_REASON_CD=CC.CBS_REASON_CD	" +
        "	) RHB	" +
        "	on RHA.CBS_REASON_CD=RHB.CBS_REASON_CD 	" +
        "	union (select 1 as seq,'*** Low-Side Overrides ***',null,null,null,null,null,null) 	" +
        "	union (select 5 as seq,'*** High-Side Overrides ***',null,null,null,null,null,null)	" +
        "	) GsbMain	" +
        "	order by GsbMain.seq	" +
        "	SELECT [REASON_NAME]	" +
        "	,case when [num3m] is null then ' ' else [num3m] end as [num3m]	" +
        "	,case when [pct3m] is null then ' ' else [pct3m] end as [pct3m] 	" +
        "	,case when [num12m] is null then ' ' else [num12m] end as [num12m]	" +
        "	,case when [pct12m] is null then ' ' else [pct12m] end as [pct12m]	" +
        "	,case when [num12myg] is null then ' ' else [num12myg] end as [num12myg]	" +
        "	,case when [pct12myg] is null then ' ' else [pct12myg] end as [pct12myg]	" +
        "	from #ORR_Report	" +
        "	drop table #ORR_Report	" +
        "	drop table #main_program	" +
        "	drop table #HA	" +
        "	drop table #3mAccept 	" +
        "	drop table #total_3m_accept 	" +
        "	drop table #6mAccept 	" +
        "	drop table #total_6m_accept 	" +
        "	drop table #12mAccept 	" +
        "	drop table #total_12m_accept 	" +
        "	drop table #HR	" +
        "	drop table #3mReject 	" +
        "	drop table #total_3m_Reject 	" +
        "	drop table #6mReject 	" +
        "	drop table #total_6m_Reject 	" +
        "	drop table #12mReject 	" +
        "	drop table #total_12m_Reject 	"
        , ddlLoan.SelectedItem.Value, ddlSubType.SelectedItem.Value, mktqrf, mktqrt, start.ToString("yyyy-MM-dd"), start12yg.ToString("yyyy-MM-dd"), end3m.ToString("yyyy-MM-dd hh:mm:ss"), end12yg.ToString("yyyy-MM-dd hh:mm:ss"));

                                    DataTable dt = new DataTable();

                                    if (ddlModel.SelectedItem.Value == "7")
                                    {
                                        String strConnString = ConfigurationManager.ConnectionStrings["GSBSQLServer"].ConnectionString;
                                        SqlConnection con = new SqlConnection(strConnString);
                                        con.Open();
                                        SqlCommand command;
                                        command = new SqlCommand("sp_FrontEnd_OverrideReasonReport", con);

                                        command.Parameters.Add(new SqlParameter("@start", start.ToString("yyyyMM")));
                                        command.Parameters.Add(new SqlParameter("@end3m", end3m.ToString("yyyyMM")));
                                        command.Parameters.Add(new SqlParameter("@end12yg", end12yg.ToString("yyyyMM")));
                                        command.Parameters.Add(new SqlParameter("@start12yg", start12yg.ToString("yyyyMM")));
                                        command.Parameters.Add(new SqlParameter("@ddlLoan", ddlLoan.SelectedItem.Value));
                                        command.Parameters.Add(new SqlParameter("@ddlSubType", ddlSubType.SelectedItem.Value));
                                        command.Parameters.Add(new SqlParameter("@mktqrf", ddlMarketFrm.Text));
                                        command.Parameters.Add(new SqlParameter("@mktqrt", ddlMarketTo.Text));
                                        command.CommandType = CommandType.StoredProcedure;
                                        command.CommandTimeout = 36000;
                                        SqlDataAdapter da = new SqlDataAdapter(command);
                                        da.Fill(dt);
                                        Session["dtb"] = dt;
                                        gvTable.DataSource = Session["dtb"];
                                        gvTable.DataBind();
                                    }
                                    else
                                    {
                                        Session["dtb"] = conn.ExcuteSQL(appQuery1);
                                        gvTable.DataSource = Session["dtb"];
                                        gvTable.DataBind();
                                    }


                        }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScript", "appPop('โปรดเลือกตัวเลือก','ค่า สินเชื่อย่อยถึง(TO) น้อยกว่า  สินเชื่อย่อยจาก(From)')", true);
                }
                            }
                        }
                    }
                }
            }
        }
        protected void btnExcel_Click(object sender, EventArgs e)
        {
            if (gvTable.Rows.Count > 0)
            {
                DataTable dt = (DataTable)Session["dtb"];
                Session["rptData"] = dt; //Report Data
                                         //Report header
                Session["sHeaderRpt1"] = "ประเภทโมเดล : " + ddlModel.SelectedItem.Text;
                Session["sHeaderRpt2"] = "ประเภทสินเชื่อ / ประเภทสินเชื่อย่อย : " + ddlLoan.SelectedItem.Text + " / " + ddlSubType.SelectedItem.Text;

                if (ddlMarketFrm.SelectedIndex == 0)
                    Session["sHeaderRpt3"] = "Market Code : " + ddlMarketFrm.SelectedItem.Text;
                else
                    Session["sHeaderRpt3"] = "Market Code : " + ddlMarketFrm.SelectedItem.Text + " ถึง " + ddlMarketTo.SelectedItem.Text;

                Session["sHeaderRpt4"] = "วันที่เปิดใบคำขอสินเชื่อ : " + ddlOpenDateMonth.SelectedValue + " " + ddlOpenDateYear.SelectedItem;

                ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "aa", "window.open('ReportLayOut/frmPreviewFontRpt.aspx?ReportId=96');", true);

            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScript", "appPop('โปรดเลือกตัวเลือก','กรุณากดปุ่มแสดงข้อมูลก่อน')", true);
                return;
            }
        }
        protected void Cancel_Click(object sender, EventArgs e)
        {
            ddlModel.ClearSelection();
            ddlLoan.ClearSelection();
            ddlSubType.ClearSelection();
            ddlMarketFrm.ClearSelection();
            ddlMarketTo.ClearSelection();
            ddlSubType.Enabled = false;
            ddlOpenDateYear.ClearSelection();
            ddlOpenDateMonth.ClearSelection();
            //txtOpenDate.Text = string.Empty;

        }
        protected void gvTable_PageChanging(object sender, GridViewPageEventArgs e)
        {
            gvTable.PageIndex = e.NewPageIndex;
            Search_Click(null, null);
        }


        
        protected void gvTable_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                GridView HeaderGrid = (GridView)sender;

                e.Row.Cells.Clear();


                GridViewRow HeaderGridRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);
                GridViewRow HeaderGridRow2 = new GridViewRow(1, 0, DataControlRowType.Header, DataControlRowState.Insert);

                TableCell HeaderCell = new TableCell();
                HeaderCell.Text = "Category";
                HeaderGridRow.Cells.Add(HeaderCell);

                HeaderCell = new TableCell();
                HeaderCell.Text = "Past 3 Months";
                HeaderCell.ColumnSpan = 2;
                HeaderGridRow.Cells.Add(HeaderCell);

                HeaderCell = new TableCell();
                HeaderCell.Text = "Past 6 Months";
                HeaderCell.ColumnSpan = 2;
                HeaderGridRow.Cells.Add(HeaderCell);

                HeaderCell = new TableCell();
                HeaderCell.Text = "Past 12 Months";
                HeaderCell.ColumnSpan = 2;
                HeaderGridRow.Cells.Add(HeaderCell);

                HeaderCell = new TableCell();
                HeaderCell.Text = "";
                HeaderGridRow2.Cells.Add(HeaderCell);

                HeaderCell = new TableCell();
                HeaderCell.Text = "Number of Loans";
                HeaderGridRow2.Cells.Add(HeaderCell);


                HeaderCell = new TableCell();
                HeaderCell.Text = "% of Total Overrides";
                HeaderGridRow2.Cells.Add(HeaderCell);


                HeaderCell = new TableCell();
                HeaderCell.Text = "Number of Loans";
                HeaderGridRow2.Cells.Add(HeaderCell);


                HeaderCell = new TableCell();
                HeaderCell.Text = "% of Total Overrides";
                HeaderGridRow2.Cells.Add(HeaderCell);

                HeaderCell = new TableCell();
                HeaderCell.Text = "Number of Loans";
                HeaderGridRow2.Cells.Add(HeaderCell);


                HeaderCell = new TableCell();
                HeaderCell.Text = "% of Total Overrides";
                HeaderGridRow2.Cells.Add(HeaderCell);


                gvTable.Controls[0].Controls.AddAt(0, HeaderGridRow);
                gvTable.Controls[0].Controls.AddAt(1, HeaderGridRow2);


            }
        }



        protected void gvTable_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        #endregion Protected Method



        //==========================================================
        //Method to print report
        //==========================================================
        protected void PrintReport_Click(object sender, EventArgs e)
        {
            if (gvTable.Rows.Count > 0)
            {
                Label userFirstname = (Label)Master.FindControl("userFirstname");
                Session["userFirstname"] = userFirstname.Text;
                Print_Preview();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScript", "appPop('โปรดเลือกตัวเลือก','กรุณากดปุ่มแสดงข้อมูลก่อน')", true);
                return;
            }

        }
        private void Print_Preview()
        {

            DataTable dt = (DataTable)Session["dtb"];
            Session["rptData"] = dt; //Report Data
            //Report header
            Session["sHeaderRpt1"] = "ประเภทโมเดล : " + ddlModel.SelectedItem.Text;
            Session["sHeaderRpt2"] = "ประเภทสินเชื่อ / ประเภทสินเชื่อย่อย : " + ddlLoan.SelectedItem.Text + " / " + ddlSubType.SelectedItem.Text;

            if (ddlMarketFrm.SelectedIndex == 0)
                Session["sHeaderRpt3"] = "Market Code : " + ddlMarketFrm.SelectedItem.Text;
            else
                Session["sHeaderRpt3"] = "Market Code : " + ddlMarketFrm.SelectedItem.Text + " ถึง " + ddlMarketTo.SelectedItem.Text;

            Session["sHeaderRpt4"] = "วันที่เปิดใบคำขอสินเชื่อ : " + ddlOpenDateMonth.SelectedValue + " " + ddlOpenDateYear.SelectedItem;

            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "aa", "window.open('ReportLayOut/frmPreviewFontRpt.aspx?ReportId=6');", true);
        }
        //-----------------------------------------------------------

        public ListItemCollection GetMonth()
        {
            ListItemCollection mCollection = new ListItemCollection();
            ListItem m1 = new ListItem();
            m1.Value = "มกราคม";
            mCollection.Add(m1);
            ListItem m2 = new ListItem();
            m2.Value = "กุมภาพันธ์";
            mCollection.Add(m2);
            ListItem m3 = new ListItem();
            m3.Value = "มีนาคม";
            mCollection.Add(m3);
            ListItem m4 = new ListItem();
            m4.Value = "เมษายน";
            mCollection.Add(m4);
            ListItem m5 = new ListItem();
            m5.Value = "พฤษภาคม";
            mCollection.Add(m5);
            ListItem m6 = new ListItem();
            m6.Value = "มิถุนายน";
            mCollection.Add(m6);
            ListItem m7 = new ListItem();
            m7.Value = "กรกฎาคม";
            mCollection.Add(m7);
            ListItem m8 = new ListItem();
            m8.Value = "สิงหาคม";
            mCollection.Add(m8);
            ListItem m9 = new ListItem();
            m9.Value = "กันยายน";
            mCollection.Add(m9);
            ListItem m10 = new ListItem();
            m10.Value = "ตุลาคม";
            mCollection.Add(m10);
            ListItem m11 = new ListItem();
            m11.Value = "พฤศจิกายน";
            mCollection.Add(m11);
            ListItem m12 = new ListItem();
            m12.Value = "ธันวาคม";
            mCollection.Add(m12);

            return mCollection;
        }

    }
}