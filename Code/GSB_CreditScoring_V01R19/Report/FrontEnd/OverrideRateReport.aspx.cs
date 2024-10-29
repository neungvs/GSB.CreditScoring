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
    public partial class OverrideRateReport : System.Web.UI.Page
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
        //    //ddlModelVersion.DataBind();
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
            DataTable dttb = (DataTable)Session["dtb"];

            string filename = string.Format("GSB-CS_OverrideRateReport_(Date_{0}-Time_{1}).xls", DateTime.Now.ToString("dd-MM-yyyy"), DateTime.Now.ToString("HH-mm-ss"));
            StringWriter tw = new StringWriter();
            DataGrid dgGrid = new DataGrid();

            tw.Write(string.Format("Override Rate Report"));
            tw.Write(tw.NewLine);
            tw.Write(string.Format("ประเภทโมเดล :| {0} ", ddlModel.SelectedItem.Text.ToString()));
            tw.Write(tw.NewLine);
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

            tw.Write("Category|As % of Application In Range|||As % of Total Booked Loan");
            tw.Write(tw.NewLine);
            tw.Write("|Past 3 Months|Past 12 Months|Year Ago 12 Months|Past 3 Months|Past 12 Months|Year Ago 12 Months");
            tw.Write(tw.NewLine);

            // Now write all the rows.
            foreach (DataRow dr in dttb.Rows)
            {
                for (int i = 0; i < iColCount; i++)
                {
                    if (!Convert.IsDBNull(dr[i]))
                    {
                        tw.Write(dr[i].ToString());
                    }
                    else
                    {
                        tw.Write("  ");
                    }
                    if (i < iColCount - 1)
                    {
                        tw.Write("|");
                    }
                }
                tw.Write(tw.NewLine);
            }

            Response.ContentType = "text/rtf; charset=UTF-8";
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename + "");
            Response.AppendHeader("Content-Encoding", "UTF8");
            //this.EnableViewState = false;
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
            DateTime start12y = new DateTime();

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

            start = DateTime.Parse("01/"+ (ddlOpenDateMonth.SelectedIndex + 1).ToString("00") + "/" + ddlOpenDateYear.SelectedValue.ToString());
            start12yg = start.AddMonths(-12);
            start12y = start.AddMonths(-11);
            end3m = start.AddMonths(-2);
            end12yg = start.AddMonths(-23);


            //if (!(txtOpenDate.Text == ""))
            //{
            //    start = DateTime.Parse((txtOpenDate.Text.ToString().Substring(0, 6) + (Convert.ToInt32(txtOpenDate.Text.Substring(6, 4).ToString()) - 543).ToString()));
            //    start12yg = start.AddMonths(-12);
            //    start12y = start.AddMonths(-11);
            //    end3m = start.AddMonths(-2);
            //    end12yg = start.AddMonths(-23);
            //}
            //else
            //{
            //    start = DateTime.Now;
            //    start12yg = start.AddMonths(-12);
            //    start12y = start.AddMonths(-11);
            //    end3m = start.AddMonths(-2);
            //    end12yg = start.AddMonths(-23);
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

            if (ddlMarketFrm.SelectedItem != null)
            {
                if (ddlMarketFrm.SelectedItem.Value != "")
                {
                    sqlQuery.Append(String.Format(" AND (MARKET_CD >= '{0}')", ddlMarketFrm.SelectedItem.Value));
                    mktqrf = String.Format(" AND (MARKET_CD >= '{0}')", ddlMarketFrm.SelectedItem.Value);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScript", "appPop('โปรดเลือกตัวเลือก','โปรดเลือก  Market Code ก่อน')", true);
                }
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
                                appQuery1 = string.Format("	DECLARE @F_YEAR VARCHAR(7), @T_YEAR VARCHAR(7), @F_3M VARCHAR(7), @T_3M VARCHAR(7)	" +
        "	DECLARE @F_12M VARCHAR(7) , @T_12M VARCHAR(7)	" +
        "	DECLARE @F_12M_AGO VARCHAR(7), @T_12M_AGO VARCHAR(7)	" +
        "	SET @F_YEAR=substring('{7}',1,4)+'-'+ substring('{7}',6,2)	" +
        "	SET @T_YEAR=substring('{4}',1,4)+'-'+ substring('{4}',6,2)	" +
        "	SET @F_3M=substring('{6}',1,4)+'-'+ substring('{6}',6,2)	" +
        "	SET @T_3M=@T_YEAR	" +
        "	SET @F_12M=substring('{5}',1,4)+'-'+ substring('{5}',6,2)	" +
        "	SET @T_12M=@T_YEAR	" +
        "	SET @F_12M_AGO=@F_YEAR	" +
        "	SET @T_12M_AGO=substring('{8}',1,4)+'-'+ substring('{8}',6,2)	" +
        "	select [CBS_APP_NO], DW_SIGN_DATE,LAST_UPDATE_DTM	" +
        "	into #MaxLUDTM	" +
        "	from [dbo].[DWH_BTFILE] A	" +
        "	WHERE SUBSTRING([LAST_UPDATE_DTM],7,4)+'-'+SUBSTRING([LAST_UPDATE_DTM],4,2)+'-'+SUBSTRING([LAST_UPDATE_DTM],1,2)=	" +
        "	(	" +
        "	SELECT MAX(SUBSTRING(F.[LAST_UPDATE_DTM],7,4)+'-'+SUBSTRING(F.[LAST_UPDATE_DTM],4,2)+'-'+SUBSTRING(F.[LAST_UPDATE_DTM],1,2))	" +
        "	FROM DWH_BTFILE f 	" +
        "	WHERE a.CBS_APP_NO=f.CBS_APP_NO	" +
        "	AND substring(F.[LAST_UPDATE_DTM],7,4)+'-'+ substring(F.[LAST_UPDATE_DTM],4,2) between @F_YEAR AND @T_YEAR 	" +
        "	)	" +
        "	CREATE NONCLUSTERED INDEX [DWH_IDX1] ON #MaxLUDTM	" +
        "	(	" +
        "	[CBS_APP_NO] ASC	" +
        "	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]	" +
        "	select distinct APP_NO,MODEL,LOAN_CD,SCORE,CBS_STATUS,STYPE_CD,MARKET_CD,DW_SIGN_DATE,APP_DATE,cut_off as scr	" +
        "	, CASE WHEN (score>=cut_off) then 'Y' else 'N' end as scapproval_flag	" +
        "	, case when CBS_STATUS in (3,6,7) then 'Y'	" +
        "	when  CBS_STATUS in (4,5) then 'N'	" +
        "	when  CBS_STATUS in (1,2) OR CBS_STATUS is null then '<>' end as Approval_Flag	" +
        "	, case when LEFT(CBS_REASON_CD,1)='A' AND  CBS_STATUS in (4,5) then 'Y'	" +
        "	when LEFT(CBS_REASON_CD,1)='R' AND  CBS_STATUS in (3,6,7) then 'Y'	" +
        "	when LEFT(CBS_REASON_CD,1)='A' AND  CBS_STATUS in (3,6,7) then 'N'	" +
        "	when LEFT(CBS_REASON_CD,1)='R' AND  CBS_STATUS in (4,5) then 'N' else 'N/A' end as CONTRAST_FLAG	" +
        "	, case when DW_SIGN_DATE IS NULL THEN 'N' ELSE 'Y' END AS BOOKING_FLAG,CBS_REASON_CD	" +
        "	, LEFT(CBS_REASON_CD,1) as CBS_REASON	" +
        "	, CONVERT(VARCHAR,YEAR(APP_DATE))+'-'+RIGHT('00'+CONVERT(VARCHAR,MONTH(APP_DATE)),2) AS MYAppDate	" +
        "	, SUBSTRING(DW_SIGN_DATE,7,4)+'-'+SUBSTRING(DW_SIGN_DATE,4,2) as MYDWSignDate	" +
        "	into #Low_Side_Approval	" +
        "	from	" +
        "	(	" +
        "	SELECT a.*,b.score,b.model	" +
        "	,(select cut_off from ST_SCORERANGE y where  y.ltype= '{0}' and y.lstype = '{1}' ) as cut_off	" +
        "	,CBS_REASON_CD	" +
        "	,D.DW_SIGN_DATE	" +
        "	from LN_APP a	" +
        "	inner join LN_GRADE b on a.APP_NO = b.APP_NO	" +
        "	inner join CBS_LN_APP c on a.app_no = c.CBS_APP_NO	" +
        "	left join #MaxLUDTM d on a.APP_NO=d.CBS_APP_NO	" +
        "	WHERE  LOAN_CD = '{0}' and STYPE_CD = '{1}' " +
        " {2} {3} " +
        "	and CBS_STATUS IN (3,4,5,6,7)	" +
        "	) lnapp	" +
        "	select 'Low-Side Approval' as Heading, SUM(LowSide_Approvals) as [Part3Month1]	" +
        "	, ((SUM(LowSide_Approvals)*1.0)/case when ((SUM(TTD_LowSide)*1.0))>0 then (SUM(TTD_LowSide)*1.0) else null end)*100.0 as [% Approved1]	" +
        "	, SUM(LowSide_Approvals12) as [Part12Month1]	" +
        "	, ((SUM(LowSide_Approvals12)*1.0)/case when ((SUM(TTD_LowSide12)*1.0))>0 then (SUM(TTD_LowSide12)*1.0) else null end)*100.0 as [% Approved12m1]	" +
        "	, SUM(LowSide_Approvals12ago) as [Part12MonthAgo1]	" +
        "	, ((SUM(LowSide_Approvals12ago)*1.0)/case when (SUM(TTD_LowSide12ago)*1.0) >0 then (SUM(TTD_LowSide12ago)*1.0) else null end)*100.0 as [% Approved12mAgo1]	" +
        "	, SUM(LowSide_Approvals) as [Part3Month]	" +
        "	, ((SUM(LowSide_Approvals)*1.0)/case when (SUM(TTD_Booked_Loan)*1.0) > 0 then (SUM(TTD_Booked_Loan)*1.0) else null end)*100.0 as [% Approved]	" +
        "	, SUM(LowSide_Approvals12) as [Part12Month]	" +
        "	, ((SUM(LowSide_Approvals12)*1.0)/case when (SUM(TTD_Booked_Loan12)*1.0) > 0 then (SUM(TTD_Booked_Loan12)*1.0) else null end)*100.0 as [% Approved12]	" +
        "	, SUM(LowSide_Approvals12Ago) as [Part12MonthAgo]	" +
        "	, ((SUM(LowSide_Approvals12ago)*1.0)/case when (SUM(TTD_Booked_Loan12ago)*1.0) >0 then (SUM(TTD_Booked_Loan12ago)*1.0) else null end)*100.0 as [% Approved12mAgo]	" +
        "	into #temp1	" +
        "	from	" +
        "	(	" +
        "	SELECT MODEL, LOAN_CD, STYPE_CD, MARKET_CD	" +
        "	, SUM(TTD_LowSide) as TTD_LowSide	" +
        "	, SUM(LowSide_Approvals) as LowSide_Approvals	" +
        "	, SUM(TTD_LowSide12) as TTD_LowSide12	" +
        "	, SUM(LowSide_Approvals12) as LowSide_Approvals12	" +
        "	, SUM(TTD_LowSide12ago) as TTD_LowSide12ago	" +
        "	, SUM(LowSide_Approvals12ago) as LowSide_Approvals12ago	" +
        "	, SUM(TTD_Booked_Loan) as TTD_Booked_Loan	" +
        "	, SUM(LowSide_Approvals_Booked) as LowSide_Approvals_Booked	" +
        "	, SUM(TTD_Booked_Loan12) as TTD_Booked_Loan12	" +
        "	, SUM(LowSide_Approvals12_Booked) as LowSide_Approvals12_Booked	" +
        "	, SUM(TTD_Booked_Loan12ago) as TTD_Booked_Loan12ago	" +
        "	, SUM(LowSide_Approvals12ago_Booked) as LowSide_Approvals12ago_Booked	" +
        "	FROM	" +
        "	(	" +
        "	select *	" +
        "	,case when CBS_REASON IN ('A','R') AND scapproval_flag='N' AND Approval_flag in ('Y','N') 	" +
        "	and MyAppDate between @F_3M and @T_3M	" +
        "	and CONTRAST_FLAG='N' then 1 else 0 end as TTD_LowSide	" +
        "	,case when CBS_REASON IN ('A') AND scapproval_flag='N' AND Approval_flag in ('Y') 	" +
        "	and MyAppDate between @F_3M and @T_3M	" +
        "	and CONTRAST_FLAG='N' then 1 else 0 end as LowSide_Approvals	" +
        "	,case when CBS_REASON IN ('A','R') AND scapproval_flag='N' AND Approval_flag in ('Y','N') 	" +
        "	and MyAppDate between @F_12M AND @T_12M 	" +
        "	and CONTRAST_FLAG='N' then 1 else 0 end as TTD_LowSide12	" +
        "	,case when CBS_REASON IN ('A') AND scapproval_flag='N' AND Approval_flag in ('Y') 	" +
        "	and MyAppDate between @F_12M AND @T_12M and CONTRAST_FLAG='N' then 1 else 0 end as LowSide_Approvals12	" +
        "	,case when CBS_REASON IN ('A','R') AND scapproval_flag='N' AND Approval_flag in ('Y','N') 	" +
        "	and MyAppDate between @F_12M_AGO and @T_12M_AGO and CONTRAST_FLAG='N' then 1 else 0 end as TTD_LowSide12ago	" +
        "	,case when CBS_REASON IN ('A') AND scapproval_flag='N' AND Approval_flag in ('Y') 	" +
        "	and MyAppDate between @F_12M_AGO and @T_12M_AGO and CONTRAST_FLAG='N' then 1 else 0 end as LowSide_Approvals12ago	" +
        "		" +
        //"	,case when BOOKING_FLAG='Y' and MYDWSignDate between @F_3M and @T_3M then 1 else 0 end as TTD_Booked_Loan	" +
        "	,case when BOOKING_FLAG='Y' and MyAppDate between @F_3M and @T_3M then 1 else 0 end as TTD_Booked_Loan	" +
        "	,case when CBS_REASON IN ('A') AND scapproval_flag='N' AND Approval_flag in ('Y') 	" +
        "	and MyAppDate between @F_3M and @T_3M	" +
        "	and CONTRAST_FLAG='N' then 1 else 0 end as LowSide_Approvals_Booked	" +
        //"	,case when BOOKING_FLAG='Y' and MYDWSignDate between @F_12M AND @T_12M then 1 else 0 end as TTD_Booked_Loan12	" +
        "	,case when BOOKING_FLAG='Y' and MyAppDate between @F_12M AND @T_12M then 1 else 0 end as TTD_Booked_Loan12	" +
        "	,case when CBS_REASON IN ('A') AND scapproval_flag='N' AND Approval_flag in ('Y') 	" +
        "	and MyAppDate between @F_12M AND @T_12M	" +
        "	and CONTRAST_FLAG='N' then 1 else 0 end as LowSide_Approvals12_Booked	" +
        //"	,case when BOOKING_FLAG='Y' and MYDWSignDate between @F_12M_AGO and @T_12M_AGO then 1 else 0 end as TTD_Booked_Loan12ago	" +
        "	,case when BOOKING_FLAG='Y' and MyAppDate between @F_12M_AGO and @T_12M_AGO then 1 else 0 end as TTD_Booked_Loan12ago	" +
        "	,case when CBS_REASON IN ('A') AND scapproval_flag='N' AND Approval_flag in ('Y')	" +
        "	and MyAppDate between @F_12M_AGO and @T_12M_AGO	" +
        "	and CONTRAST_FLAG='N' then 1 else 0 end as LowSide_Approvals12ago_Booked	" +
        "	from #Low_Side_Approval as gsb_DWFILE_LNAPP	" +
        "	) gsb_sub_main1	" +
        "	group by MODEL, LOAN_CD, STYPE_CD, MARKET_CD	" +
        "	) gsb_main	" +
        "	select 'Low-Side Booked' as Heading	" +
        "	, SUM(Low_Side_Booked) as [Part3Month1]	" +
        "	,((SUM(Low_Side_Booked)*1.0)/case when ((SUM(TTD_LowSide)*1.0))>0 then (SUM(TTD_LowSide)*1.0) else NULL end)*100.0 as [% Approved1]	" +
        "	, SUM(Low_Side_Booked12) as [Part12Month1]	" +
        "	, ((SUM(Low_Side_Booked12)*1.0)/case when ((SUM(TTD_LowSide12)*1.0))>0 then (SUM(TTD_LowSide12)*1.0) else NULL end)*100.0 as [% Approved12m1]	" +
        "	, SUM(Low_Side_Booked12ago) as [Part12MonthAgo1]	" +
        "	, ((SUM(Low_Side_Booked12ago)*1.0)/case when (SUM(TTD_LowSide12ago)*1.0)> 0 then (SUM(TTD_LowSide12ago)*1.0) else NULL end)*100.0 as [% Approved12mAgo1]	" +
        "	, SUM(Low_Side_Booked) as [Part3Month]	" +
        "	, ((SUM(Low_Side_Booked)*1.0)/case when (SUM(TTD_Booked_Loan)*1.0) > 0 then (SUM(TTD_Booked_Loan)*1.0) else null end)*100.0 as [% Approved]	" +
        "	, SUM(Low_Side_Booked12) as [Part12Month]	" +
        "	, ((SUM(Low_Side_Booked12)*1.0)/case when (SUM(TTD_Booked_Loan12)*1.0) > 0 then (SUM(TTD_Booked_Loan12)*1.0) else null end)*100.0 as [% Approved12]	" +
        "	, SUM(Low_Side_Booked12mAgo_Loan) as [Part12MonthAgo]	" +
        "	, ((SUM(Low_Side_Booked12mAgo_Loan)*1.0)/case when (SUM(TTD_Booked_Loan12mAgo)*1.0) > 0 then (SUM(TTD_Booked_Loan12mAgo)*1.0) else null end)*100.0 as [% Approved12mAgo]	" +
        "	into #temp2	" +
        "	from	" +
        "	(	" +
        "	SELECT MODEL, LOAN_CD, STYPE_CD, MARKET_CD	" +
        "	, SUM(TTD_LowSide) as TTD_LowSide, SUM(Low_Side_Booked) as Low_Side_Booked	" +
        "	, SUM(TTD_LowSide12) as TTD_LowSide12, SUM(Low_Side_Booked12) as Low_Side_Booked12	" +
        "	, SUM(TTD_LowSide12ago) as TTD_LowSide12ago, SUM(Low_Side_Booked12ago) as Low_Side_Booked12ago	" +
        "	, SUM(TTD_Booked_Loan) as TTD_Booked_Loan	" +
        "	, SUM(Low_Side_Booked_Loan) as Low_Side_Booked_Loan	" +
        "	, SUM(TTD_Booked_Loan12) as TTD_Booked_Loan12	" +
        "	, SUM(Low_Side_Booked12_Loan) as Low_Side_Booked12_Loan	" +
        "	, SUM(TTD_Booked_Loan12mAgo) as TTD_Booked_Loan12mAgo	" +
        "	, SUM(Low_Side_Booked12mAgo_Loan) as Low_Side_Booked12mAgo_Loan	" +
        "	FROM	" +
        "	(	" +
        "	select *	" +
        "	, case when CBS_REASON IN ('A','R') AND scapproval_flag='N' AND Approval_flag in ('Y','N') 	" +
        "	and MyAppDate between @F_3M and @T_3M	" +
        "	and CONTRAST_FLAG='N' then 1 else 0 end as TTD_LowSide	" +
        "	, case when CBS_REASON IN ('A') AND scapproval_flag='N' AND Approval_flag in ('Y') and BOOKING_FLAG='Y'	" +
        "	and MyAppDate between @F_3M and @T_3M	" +
        "	and CONTRAST_FLAG='N' then 1 else 0 end as Low_Side_Booked	" +
        "	, case when CBS_REASON IN ('A','R') AND scapproval_flag='N' AND Approval_flag in ('Y','N')	" +
        "	and MyAppDate between @F_12M AND @T_12M and CONTRAST_FLAG='N' then 1 else 0 end as TTD_LowSide12	" +
        "	, case when CBS_REASON IN ('A') AND scapproval_flag='N' AND Approval_flag in ('Y') and BOOKING_FLAG='Y'	" +
        "	and MyAppDate between @F_12M AND @T_12M and CONTRAST_FLAG='N' then 1 else 0 end as Low_Side_Booked12	" +
        "	, case when CBS_REASON IN ('A','R') AND scapproval_flag='N' AND Approval_flag in ('Y','N')	" +
        "	and MyAppDate between @F_12M_AGO and @T_12M_AGO and CONTRAST_FLAG='N' then 1 else 0 end as TTD_LowSide12ago	" +
        "	, case when CBS_REASON IN ('A') AND scapproval_flag='N' AND Approval_flag in ('Y') and BOOKING_FLAG='Y'	" +
        "	and MyAppDate between @F_12M_AGO and @T_12M_AGO and CONTRAST_FLAG='N' then 1 else 0 end as Low_Side_Booked12ago	" +
        //"	, case when MYDWSignDate between @F_3M and @T_3M and BOOKING_FLAG='Y' then 1 else 0 	" +
        "	, case when MyAppDate between @F_3M and @T_3M and BOOKING_FLAG='Y' then 1 else 0 	" +
        "	end as TTD_Booked_Loan	" +
        "	, case when CBS_REASON IN ('A') AND scapproval_flag='N'AND Approval_flag ='Y' and BOOKING_FLAG='Y' 	" +
        "	and MyAppDate between @F_3M and @T_3M	" +
        "	and CONTRAST_FLAG='N' then 1 else 0 	" +
        "	end as Low_Side_Booked_Loan	" +
        //"	, case when MYDWSignDate between @F_12M AND @T_12M and BOOKING_FLAG='Y' then 1 else 0 	" +
        "	, case when MyAppDate between @F_12M AND @T_12M and BOOKING_FLAG='Y' then 1 else 0 	" +
        "	end as TTD_Booked_Loan12	" +
        "	, case when CBS_REASON ='A' AND scapproval_flag='N' AND Approval_flag ='Y' and BOOKING_FLAG='Y' 	" +
        "	and MyAppDate between @F_12M AND @T_12M and CONTRAST_FLAG='N' then 1 else 0 	" +
        "	end as Low_Side_Booked12_Loan	" +
        //"	, case when MYDWSignDate between @F_12M_AGO and @T_12M_AGO and BOOKING_FLAG='Y' then 1 else 0	" +
        "	, case when MyAppDate between @F_12M_AGO and @T_12M_AGO and BOOKING_FLAG='Y' then 1 else 0	" +
        "	  end as TTD_Booked_Loan12mAgo	" +
        "	, case when CBS_REASON IN ('A') AND scapproval_flag='N' AND Approval_flag in ('Y') and BOOKING_FLAG='Y'	" +
        "	and MyAppDate between @F_12M_AGO and @T_12M_AGO and CONTRAST_FLAG='N' then 1 else 0 	" +
        "	  end as Low_Side_Booked12mAgo_Loan	" +
        "	from #Low_Side_Approval	" +
        "	) gsb_sub_main1	" +
        "	group by MODEL, LOAN_CD, STYPE_CD, MARKET_CD	" +
        "	) gsb_main	" +
        "	select 'High-Side Declined' as Heading	" +
        "	, SUM(HighSide_Declines) as [Part3Month1]	" +
        "	, ((SUM(HighSide_Declines)*1.0)/case when (SUM(TTD_HighSide)*1.0) > 0 then (SUM(TTD_HighSide)*1.0) else null end)*100.0 as [% Decline1]	" +
        "	, SUM(HighSide_Declines12) as [Part12Month1]	" +
        "	, ((SUM(HighSide_Declines12)*1.0)/case when (SUM(TTD_HighSide12)*1.0) > 0 then (SUM(TTD_HighSide12)*1.0) else null end)*100.0 as [% Decline12m1]	" +
        "	, SUM(HighSide_Declines12ago) as [Part12MonthAgo1]	" +
        "	, ((SUM(HighSide_Declines12ago)*1.0)/case when (SUM(TTD_HighSide12ago)*1.0) >0 then (SUM(TTD_HighSide12ago)*1.0) else null end)*100.0 as [% Decline12mAgo1]	" +
        "	, SUM(HighSide_Declines_Booked) as [Part3Month]	" +
        "	, ((SUM(HighSide_Declines_Booked)*1.0)/case when (SUM(TTD_Booked_Loan)*1.0) > 0 then (SUM(TTD_Booked_Loan)*1.0) else null end)*100.0 as [% Decline]	" +
        "	, SUM(HighSide_Declines12_Booked) as [Part12Month]	" +
        "	, ((SUM(HighSide_Declines12_Booked)*1.0)/case when (SUM(TTD_Booked_Loan12)*1.0) > 0 then (SUM(TTD_Booked_Loan12)*1.0) else null end)*100.0 as [% Decline12m]	" +
        "	, SUM(HighSide_Declines12ago_Booked) as [Part12MonthAgo]	" +
        "	, ((SUM(HighSide_Declines12ago_Booked)*1.0)/case when (SUM(TTD_Booked_Loan12ago)*1.0) > 0 then (SUM(TTD_Booked_Loan12ago)*1.0) else null end)*100.0 as [% Decline12mAgo]	" +
        "	into #temp3	" +
        "	from	" +
        "	(	" +
        "	SELECT MODEL, LOAN_CD, STYPE_CD, MARKET_CD	" +
        "	, SUM(TTD_HighSide) as TTD_HighSide, SUM(HighSide_Declines) as HighSide_Declines	" +
        "	, SUM(TTD_HighSide12) as TTD_HighSide12, SUM(HighSide_Declines12) as HighSide_Declines12	" +
        "	, SUM(TTD_HighSide12ago) as TTD_HighSide12ago, SUM(HighSide_Declines12ago) as HighSide_Declines12ago	" +
        "	, SUM(TTD_Booked_Loan) as TTD_Booked_Loan	" +
        "	, SUM(HighSide_Declines_Booked) as HighSide_Declines_Booked	" +
        "	, SUM(TTD_Booked_Loan12) as TTD_Booked_Loan12	" +
        "	, SUM(HighSide_Declines12_Booked) as HighSide_Declines12_Booked	" +
        "	, SUM(TTD_Booked_Loan12ago) as TTD_Booked_Loan12ago	" +
        "	, SUM(HighSide_Declines12ago_Booked) as HighSide_Declines12ago_Booked	" +
        "	FROM	" +
        "	(	" +
        "	select *	" +
        "	, case when CBS_REASON IN ('A','R') AND scapproval_flag='Y' AND Approval_flag in ('Y','N') 	" +
        "	and MyAppDate between @F_3M and @T_3M and CONTRAST_FLAG='N' then 1 else 0	" +
        "	  end as TTD_HighSide	" +
        "	, case when CBS_REASON IN ('R') AND scapproval_flag='Y' AND Approval_flag in ('N') 	" +
        "	and MyAppDate between @F_3M and @T_3M and CONTRAST_FLAG='N' then 1 else 0 end as HighSide_Declines	" +
        "	, case when CBS_REASON IN ('A','R') AND scapproval_flag='Y' AND Approval_flag in ('Y','N') 	" +
        "	and MyAppDate between @F_12M AND @T_12M and CONTRAST_FLAG='N' then 1 else 0 end as TTD_HighSide12	" +
        "	, case when CBS_REASON IN ('R') AND scapproval_flag='Y' AND Approval_flag in ('N') 	" +
        "	and MyAppDate between @F_12M AND @T_12M and CONTRAST_FLAG='N' then 1 else 0 end as HighSide_Declines12	" +
        "	, case when CBS_REASON IN ('A','R') AND scapproval_flag='Y' AND Approval_flag in ('Y','N')	" +
        "	and MyAppDate between @F_12M_AGO and @T_12M_AGO and CONTRAST_FLAG='N' then 1 else 0 end as TTD_HighSide12ago	" +
        "	, case when CBS_REASON IN ('R') AND scapproval_flag='Y' AND Approval_flag in ('N') 	" +
        "	and MyAppDate between @F_12M_AGO and @T_12M_AGO and CONTRAST_FLAG='N' then 1 else 0  end as HighSide_Declines12ago	" +
        "	, case when CBS_REASON ='R' AND scapproval_flag='Y' AND Approval_flag = 'N' 	" +
        "	and MyAppDate between @F_3M and @T_3M and CONTRAST_FLAG='N' then 1 else 0 end as HighSide_Declines_Booked	" +
        //"	, case when BOOKING_FLAG='Y' and MYDWSignDate between @F_3M and @T_3M then 1 else 0 end as TTD_Booked_Loan	" +
        "	, case when BOOKING_FLAG='Y' and MyAppDate between @F_3M and @T_3M then 1 else 0 end as TTD_Booked_Loan	" +
        "	, case when CBS_REASON ='R' AND scapproval_flag='Y' AND Approval_flag = 'N' 	" +
        "	and MyAppDate between @F_12M AND @T_12M and CONTRAST_FLAG='N' then 1 else 0 end as HighSide_Declines12_Booked	" +
        "	, case when BOOKING_FLAG='Y'	" +
        //"	and MYDWSignDate between @F_12M AND @T_12M then 1 else 0 end as TTD_Booked_Loan12	" +
        "	and MyAppDate between @F_12M AND @T_12M then 1 else 0 end as TTD_Booked_Loan12	" +
        "	, case when CBS_REASON ='R' AND scapproval_flag='Y' AND Approval_flag = 'N'	" +
        "	and MyAppDate between @F_12M_AGO and @T_12M_AGO and CONTRAST_FLAG='N' then 1 else 0 end as HighSide_Declines12ago_Booked	" +
        "	, case when BOOKING_FLAG='Y'	" +
        //"	and MYDWSignDate between @F_12M_AGO and @T_12M_AGO then 1 else 0 end as TTD_Booked_Loan12ago	" +
        "	and MyAppDate between @F_12M_AGO and @T_12M_AGO then 1 else 0 end as TTD_Booked_Loan12ago	" +
        "	from #Low_Side_Approval	" +
        "	) gsb_sub_main1	" +
        "	group by MODEL, LOAN_CD, STYPE_CD, MARKET_CD	" +
        "	) gsb_main	" +
        "	select Heading as category,ovcnt3m,ovcnt12m,ovcnt12myg,ovbcnt3m,ovbcnt12m,ovbcnt12myg	" +
        "	from	" +
        "	(	" +
        "	select 1 as seq,a.Heading	" +
        "	, dbo.comma_format(convert(varchar,convert(decimal(15,0),Isnull(a.[Part3Month1],0)))) as ovcnt3m	" +
        "	, dbo.comma_format(convert(varchar,convert(decimal(15,0),Isnull(a.[Part12Month1],0)))) as ovcnt12m	" +
        "	, dbo.comma_format(convert(varchar,convert(decimal(15,0),Isnull(a.[Part12MonthAgo1],0)))) as ovcnt12myg	" +
        "	, dbo.comma_format(convert(varchar,convert(decimal(15,0),Isnull(a.[Part3Month],0)))) as ovbcnt3m	" +
        "	, dbo.comma_format(convert(varchar,convert(decimal(15,0),Isnull(a.[Part12Month],0)))) as ovbcnt12m	" +
        "	, dbo.comma_format(convert(varchar,convert(decimal(15,0),Isnull(a.[Part12MonthAgo],0)))) as ovbcnt12myg	" +
        "	from #temp1 a	" +
        "	union	" +
        "	select 2 as seq,'% Approved' as Heading	" +
        "	, [dbo].[comma_percent_format](convert(varchar,convert(decimal(15,2),a.[% Approved1])))	" +
        "	, [dbo].[comma_percent_format](convert(varchar,convert(decimal(15,2),a.[% Approved12m1])))	" +
        "	, [dbo].[comma_percent_format](convert(varchar,convert(decimal(15,2),a.[% Approved12mAgo1])))	" +
        "	, [dbo].[comma_percent_format](convert(varchar,convert(decimal(15,2),a.[% Approved])))	" +
        "	, [dbo].[comma_percent_format](convert(varchar,convert(decimal(15,2),a.[% Approved12])))	" +
        "	, [dbo].[comma_percent_format](convert(varchar,convert(decimal(15,2),a.[% Approved12mAgo])))	" +
        "	from #temp1 a	" +
        "	union	" +
        "	select 3 as seq,Heading	" +
        "	, dbo.comma_format(convert(varchar,convert(decimal(15,0),Isnull(a.[Part3Month1],0))))	" +
        "	, dbo.comma_format(convert(varchar,convert(decimal(15,0),Isnull(a.[Part12Month1],0))))	" +
        "	, dbo.comma_format(convert(varchar,convert(decimal(15,0),Isnull(a.[Part12MonthAgo1],0))))	" +
        "	, dbo.comma_format(convert(varchar,convert(decimal(15,0),Isnull(a.[Part3Month],0))))	" +
        "	, dbo.comma_format(convert(varchar,convert(decimal(15,0),Isnull(a.[Part12Month],0))))	" +
        "	, dbo.comma_format(convert(varchar,convert(decimal(15,0),Isnull(a.[Part12MonthAgo],0))))	" +
        "	from #temp2 a	" +
        "	union	" +
        "	select 4 as seq,'% Booked' as Heading	" +
        "	, [dbo].[comma_percent_format](convert(varchar,convert(decimal(15,2),a.[% Approved1])))	" +
        "	, [dbo].[comma_percent_format](convert(varchar,convert(decimal(15,2),a.[% Approved12m1])))	" +
        "	, [dbo].[comma_percent_format](convert(varchar,convert(decimal(15,2),a.[% Approved12mAgo1])))	" +
        "	, [dbo].[comma_percent_format](convert(varchar,convert(decimal(15,2),a.[% Approved])))	" +
        "	, [dbo].[comma_percent_format](convert(varchar,convert(decimal(15,2),a.[% Approved12])))	" +
        "	, [dbo].[comma_percent_format](convert(varchar,convert(decimal(15,2),a.[% Approved12mAgo])))	" +
        "	from #temp2 a	" +
        "	union	" +
        "	select 5 as seq,a.Heading	" +
        "	, dbo.comma_format(convert(varchar,convert(decimal(15,0),Isnull(a.[Part3Month1],0))))	" +
        "	, dbo.comma_format(convert(varchar,convert(decimal(15,0),Isnull(a.[Part12Month1],0))))	" +
        "	, dbo.comma_format(convert(varchar,convert(decimal(15,0),Isnull(a.[Part12MonthAgo1],0))))	" +
        "	, dbo.comma_format(convert(varchar,convert(decimal(15,0),Isnull(a.[Part3Month],0))))	" +
        "	, dbo.comma_format(convert(varchar,convert(decimal(15,0),Isnull(a.[Part12Month],0))))	" +
        "	, dbo.comma_format(convert(varchar,convert(decimal(15,0),Isnull(a.[Part12MonthAgo],0))))	" +
        "	from #temp3 a	" +
        "	union	" +
        "	select 6 as seq,'% Declined' as Heading	" +
        "	, [dbo].[comma_percent_format](convert(varchar,convert(decimal(15,2),a.[% Decline1])))	" +
        "	, [dbo].[comma_percent_format](convert(varchar,convert(decimal(15,2),a.[% Decline12m1])))	" +
        "	, [dbo].[comma_percent_format](convert(varchar,convert(decimal(15,2),a.[% Decline12mAgo1])))	" +
        "	, [dbo].[comma_percent_format](convert(varchar,convert(decimal(15,2),a.[% Decline])))	" +
        "	, [dbo].[comma_percent_format](convert(varchar,convert(decimal(15,2),a.[% Decline12m])))	" +
        "	, [dbo].[comma_percent_format](convert(varchar,convert(decimal(15,2),a.[% Decline12mAgo])))	" +
        "	from #temp3 a	" +
        "	) xx	" +
        "	order by seq	" +
        "		" +
        "	DROP TABLE #temp1	" +
        "	DROP TABLE #temp2	" +
        "	DROP TABLE #temp3	" +
        "	drop table #Low_Side_Approval	" +
        "	drop table #MaxLUDTM 		"
        , ddlLoan.SelectedItem.Value, ddlSubType.SelectedItem.Value, mktqrf, mktqrt, start.ToString("yyyy-MM-dd"), start12y.ToString("yyyy-MM-dd"), end3m.ToString("yyyy-MM-dd hh:mm:ss"), end12yg.ToString("yyyy-MM-dd hh:mm:ss"), start12yg.ToString("yyyy-MM-dd"));


                                    DataTable dt = new DataTable();

                                    if (ddlModel.SelectedItem.Value == "7")
                                    {
                                        String strConnString = ConfigurationManager.ConnectionStrings["GSBSQLServer"].ConnectionString;
                                        SqlConnection con = new SqlConnection(strConnString);
                                        con.Open();
                                        SqlCommand command;
                                        command = new SqlCommand("sp_FrontEnd_OverrideRateReport", con);

                                        command.Parameters.Add(new SqlParameter("@start", start.ToString("yyyyMM")));
                                        command.Parameters.Add(new SqlParameter("@start12y", start12y.ToString("yyyyMM")));
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

                ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "aa", "window.open('ReportLayOut/frmPreviewFontRpt.aspx?ReportId=95');", true);
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
                HeaderCell.Text = "As % of Application In Range";
                HeaderCell.ColumnSpan = 3;
                HeaderGridRow.Cells.Add(HeaderCell);

                HeaderCell = new TableCell();
                HeaderCell.Text = "Category";
                HeaderGridRow.Cells.Add(HeaderCell);

                HeaderCell = new TableCell();
                HeaderCell.Text = "As % of Total Booked Loan";
                HeaderCell.ColumnSpan = 3;
                HeaderGridRow.Cells.Add(HeaderCell);

                HeaderCell = new TableCell();
                HeaderCell.Text = "";
                HeaderGridRow2.Cells.Add(HeaderCell);

                HeaderCell = new TableCell();
                HeaderCell.Text = "Past 3 Months";
                HeaderGridRow2.Cells.Add(HeaderCell);

                HeaderCell = new TableCell();
                HeaderCell.Text = "Past 12 Months";
                HeaderGridRow2.Cells.Add(HeaderCell);

                HeaderCell = new TableCell();
                HeaderCell.Text = "Year Ago 12 Months";
                HeaderGridRow2.Cells.Add(HeaderCell);

                HeaderCell = new TableCell();
                HeaderCell.Text = "";
                HeaderGridRow2.Cells.Add(HeaderCell);

                HeaderCell = new TableCell();
                HeaderCell.Text = "Past 3 Months";
                HeaderGridRow2.Cells.Add(HeaderCell);

                HeaderCell = new TableCell();
                HeaderCell.Text = "Past 12 Months";
                HeaderGridRow2.Cells.Add(HeaderCell);

                HeaderCell = new TableCell();
                HeaderCell.Text = "Year Ago 12 Months";
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

            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "aa", "window.open('ReportLayOut/frmPreviewFontRpt.aspx?ReportId=5');", true);
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