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
    public partial class ApprovalRateReport : System.Web.UI.Page
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

        //private void LoadModelVersion()
        //{

        //    ListItem item2 = new ListItem(" Version1 ", "null");
        //    ddlModelVersion.Items.Add(item2);
        //    ddlModelVersion.SelectedValue = "null";
        //    //ddlModelVersion.DataBind();
        //}

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

            string filename = string.Format("GSB-CS_ApprovalRateReport_(Date_{0}-Time_{1}).xls", DateTime.Now.ToString("dd-MM-yyyy"), DateTime.Now.ToString("HH-mm-ss"));
            StringWriter tw = new StringWriter();
            DataGrid dgGrid = new DataGrid();

            tw.Write(string.Format("Approval Rate Report"));
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
            tw.Write(string.Format("วันที่เปิดใบคำขอสินเชื่อ :| {0} ", (ddlOpenDateMonth.SelectedIndex + 1).ToString("00")) +  "-"+ ddlOpenDateYear.SelectedItem );
            tw.Write(tw.NewLine);
            tw.Write(string.Format("Export Date :| {0} : {1} ", DateTime.Now.ToString("dd-MM-yyyy"), DateTime.Now.ToString("HH-mm-ss")));
            tw.Write(tw.NewLine);
            tw.Write("ผู้พิมพ์ " + ExportUtility.getCurrentUserId);
            tw.Write(tw.NewLine);

            int iColCount = new int();
            iColCount = dttb.Columns.Count;

            tw.WriteLine("|Through-the-door Population|||Withdraw as % TTD in range|||Approve as % decisioned in range|||Book as % of approved in range");
            tw.WriteLine("|Past |Past |Year Ago |Past |Past |Year Ago |Past |Past |Year Ago |Patst |Past |Year Ago ");

            //tw.Write(tw.NewLine);

            // Now write all the rows.
            foreach (DataRow dr in dttb.Rows)
            {
                for (int i = 0; i < iColCount; i++)
                {
                    if (!Convert.IsDBNull(dr[i]))
                    {
                        //tw.Write(dr[i].ToString().Replace("|", ""));     
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

        string Version_id;

        //protected void LoadVersion(object sender, EventArgs e)
        //{
        //    string sql = "";

        //    sql = string.Format("select MODEL_NAME+'_V1' as [Model_V_NAME] from LN_MODEL where ACTIVE_FLAG = '1' and");
        //    sql += "  [MODEL_CD]=" + ddlModel.SelectedValue + " ";


        //    ddlModelVersion.DataSource = conn.ExcuteSQL(sql);
        //    ddlModelVersion.DataTextField = "Model_V_NAME";
        //    ddlModelVersion.DataValueField = "Model_V_NAME";
        //    ddlModelVersion.DataBind();

        //    Version_id = ddlModelVersion.SelectedItem.Value;

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
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScript", "appPop('วันที่เปิดใบคำขอสินเชื่อไม่ถูกต้อง?','วันที่เปิดฯ เริ่มต้น ต้องน้อยกว่า หรือเท่ากับ วันที่เปิดฯ สุดท้าย')", true);
                return;
            }

                start = DateTime.Parse("01/"+ (ddlOpenDateMonth.SelectedIndex + 1).ToString("00")+"/"+ddlOpenDateYear.SelectedValue.ToString());
                start12yg = start.AddMonths(-12);
                start12y = start.AddMonths(-11);
                end3m = start.AddMonths(-2);
                end12yg = start.AddMonths(-23);

            if (ddlLoan.SelectedItem.Value != "")
                sqlQuery.Append(String.Format(" AND (LOAN_CD = '{0}')", ddlLoan.SelectedItem.Value));

            if (!String.IsNullOrEmpty(ddlSubType.SelectedItem.Value))
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

            if (ddlMarketTo.SelectedItem != null)
            {
                if (ddlMarketTo.SelectedItem.Value != "")
                {
                    sqlQuery.Append(String.Format(" AND (MARKET_CD <= '{0}')", ddlMarketTo.SelectedItem.Value));
                    mktqrt = String.Format(" AND (MARKET_CD <= '{0}')", ddlMarketTo.SelectedItem.Value);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScript", "appPop('โปรดเลือกตัวเลือก','โปรดเลือกสินเชื่อย่อย ถึง อย่างน้อย 1 ค่า')", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScript", "appPop('โปรดเลือกตัวเลือก','โปรดเลือกประเภทสินเชื่อและสินเชื่อย่อยก่อนดำเนินการต่อ')", true);
            }

            if (ddlModel.SelectedIndex <= 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScript", "appPop('กรุณาเลือกข้อมูลให้ถูกต้อง')", true);
            }
            else
            {
                if (ddlLoan.SelectedIndex <= 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScript", "appPop('กรุณาเลือกข้อมูลให้ถูกต้อง')", true);
                }
                else
                {
                    if (ddlSubType.SelectedIndex <= 0)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScript", "appPop('กรุณาเลือกข้อมูลให้ถูกต้อง')", true);
                    }
                    else
                    {
                        if (ddlMarketFrm.SelectedItem.Value != "")
                        {
                            if (ddlMarketTo.SelectedItem.Value != "")
                            {
                                //-- begin
                                int st1 = Convert.ToInt32(ddlMarketFrm.SelectedItem.Value);
                                int st2 = Convert.ToInt32(ddlMarketTo.SelectedItem.Value);

                                if (st1 <= st2)
                                {
                                    if (rpttype.SelectedValue == "SCORE")
                                    {
                                        if (rptcttype.SelectedValue == "Number") //Score Number
                                        {
                                            //appQuery1 = string.Format("	select 	" + Tai 2014-01-02
                                            appQuery1 = string.Format("	select 	" +
                                            "	case when (SCORE - scr) < 0 then convert(varchar	" +
                                            "	,(scr + ((((SCORE + 1) - scr)/rint) -1 ) * rint)) + '-' + convert(varchar,((scr + ((((SCORE + 1 ) - scr)/rint)-1 ) * rint) + rint - 1))  	" +
                                            "	when (SCORE - scr) >= 0 then convert(varchar,(scr + ((SCORE - scr)/rint) * rint)) + '-' 	" +
                                            "	+ convert(varchar,((scr + ((SCORE - scr)/rint) * rint) + rint - 1))   end as scrdif 	" +
                                            "	,scApproval_flag,CONTRAST_FLAG	" +
                                            "	,  case when CONVERT(VARCHAR,YEAR(APP_DATE))+'-'+RIGHT('00'+CONVERT(VARCHAR,MONTH(APP_DATE)),2) between substring('{6}',1,4)+'-'+ substring('{6}',6,2) and  substring('{4}',1,4)+'-'+ substring('{4}',6,2) then 1 else 0 end as tdd3m	" +
                                            "	,  case when CONVERT(VARCHAR,YEAR(APP_DATE))+'-'+RIGHT('00'+CONVERT(VARCHAR,MONTH(APP_DATE)),2) between substring('{5}',1,4)+'-'+ substring('{5}',6,2) and  substring('{4}',1,4)+'-'+ substring('{4}',6,2) then 1 else 0 end as tdd12m	" +
                                            "	,  case when CONVERT(VARCHAR,YEAR(APP_DATE))+'-'+RIGHT('00'+CONVERT(VARCHAR,MONTH(APP_DATE)),2) between substring('{6}',1,4)+'-'+ substring('{6}',6,2) and  substring('{4}',1,4)+'-'+ substring('{4}',6,2) and Approval_Flag ='Y' then 1 else 0 end as tdd3ma	" +
                                            "	,  case when CONVERT(VARCHAR,YEAR(APP_DATE))+'-'+RIGHT('00'+CONVERT(VARCHAR,MONTH(APP_DATE)),2) between substring('{5}',1,4)+'-'+ substring('{5}',6,2) and  substring('{4}',1,4)+'-'+ substring('{4}',6,2) and Approval_Flag ='Y' then 1 else 0 end as tdd12ma	" +
                                            "	,  case when CONVERT(VARCHAR,YEAR(APP_DATE))+'-'+RIGHT('00'+CONVERT(VARCHAR,MONTH(APP_DATE)),2) between substring('{6}',1,4)+'-'+ substring('{6}',6,2) and  substring('{4}',1,4)+'-'+ substring('{4}',6,2) and Approval_Flag ='N' then 1 else 0 end as tdd3mw	" +
                                            "	,  case when CONVERT(VARCHAR,YEAR(APP_DATE))+'-'+RIGHT('00'+CONVERT(VARCHAR,MONTH(APP_DATE)),2) between substring('{5}',1,4)+'-'+ substring('{5}',6,2) and  substring('{4}',1,4)+'-'+ substring('{4}',6,2) and Approval_Flag ='N' then 1 else 0 end as tdd12mw	" +
                                            "	into #Heading	" +
                                            "	from 	" +
                                                //Tai 2014-01-02
                                                //"	(	select APP_NO,MODEL,LOAN_CD,SCORE,CBS_STATUS,STYPE_CD,MARKET_CD,DW_SIGN_DATE,APP_DATE,cut_off as scr,rint	" +
                                            "	(	select distinct APP_NO,MODEL,LOAN_CD,SCORE,CBS_STATUS,STYPE_CD,MARKET_CD,DW_SIGN_DATE,APP_DATE,cut_off as scr,rint	" +
                                                //
                                            "	,CASE WHEN (score>=cut_off) then 'Y' else 'N' end as scapproval_flag	" +
                                            "	,case when CBS_STATUS in (3,6,7) then 'Y' when  CBS_STATUS in (4,5) then 'N' when  CBS_STATUS in (1,2) OR CBS_STATUS is null then '<>' end as Approval_Flag	" +
                                            "	,case when LEFT(CBS_REASON_CD,1)='A' AND  CBS_STATUS in (4,5) then 'Y' 	" +
                                            "	when LEFT(CBS_REASON_CD,1)='R' AND  CBS_STATUS in (3,6,7) then 'Y' when LEFT(CBS_REASON_CD,1)='A' AND  CBS_STATUS in (3,6,7) then 'N'	" +
                                            "	when LEFT(CBS_REASON_CD,1)='R' AND  CBS_STATUS in (4,5) then 'N' else 'N/A' end as CONTRAST_FLAG,	" +
                                            "	case when DW_SIGN_DATE IS NULL THEN 'N' ELSE 'Y' END AS BOOKING_FLAG	" +
                                            "	,CBS_REASON_CD, LEFT(CBS_REASON_CD,1) as CBS_REASON	" +
                                            "	, CONVERT(VARCHAR,YEAR(APP_DATE))+'-'+RIGHT('00'+CONVERT(VARCHAR,MONTH(APP_DATE)),2) AS MYAppDate	" +
                                            "	from	" +
                                            "	(	" +
                                            "	SELECT a.*,b.score,b.model	" +
                                            "	,(select cut_off from ST_SCORERANGE y where  y.ltype= '{0}' and y.lstype = '{1}' ) as cut_off	" +
                                            "	,(select rint from ST_SCORERANGE y where  y.ltype= '{0}' and y.lstype = '{1}' ) as rint	" +
                                            "	,CBS_REASON_CD	" +
                                            "	from LN_APP a,LN_GRADE b,CBS_LN_APP c 	" +
                                            "	where a.APP_NO = b.APP_NO  	" +
                                            "	and a.app_no = c.CBS_APP_NO 	" +
                                            "	and  LOAN_CD = '{0}' and STYPE_CD = '{1}' {2} {3} 	" +
                                            "	) lnapp	" +
                                            "	left join	" +
                                            "	(	" +
                                            "	select * 	" +
                                            "	from	" +
                                            "	(	" +
                                            "	select [CBS_APP_NO], DW_SIGN_DATE,LAST_UPDATE_DTM	" +
                                            "	from [DWH_BTFILE] A	" +
                                            " where SUBSTRING([LAST_UPDATE_DTM],7,4)+'-'+SUBSTRING([LAST_UPDATE_DTM],4,2)+'-'+SUBSTRING([LAST_UPDATE_DTM],1,2)=(SELECT MAX(SUBSTRING([LAST_UPDATE_DTM],7,4)+'-'+SUBSTRING([LAST_UPDATE_DTM],4,2)+'-'+SUBSTRING([LAST_UPDATE_DTM],1,2)) FROM [DWH_BTFILE] B WHERE A.CBS_APP_NO=B.CBS_APP_NO)		" +
                                            "	) DW_BFILE_T	" +
                                            "	) gsb2T	" +
                                            "	 on gsb2T.CBS_APP_NO = lnapp.APP_NO    	" +
                                            "	) gsbdat  	" +
                                            "	where  CONTRAST_FLAG='N' and Approval_Flag in ('Y','N') and BOOKING_FLAG in ('Y','N') 	" +
                                            "	select scrdif,sum(tdd3m) as tdd3m,sum(tdd12m) as tdd12m, sum(tdd3ma) as tdd3ma, sum(tdd12ma) as tdd12ma	" +
                                            "	,sum(tdd3mw) as tdd3mw, sum(tdd12mw) as tdd12mw	" +
                                            "	into #Main_Table	" +
                                            "	from #Heading	" +
                                            "	group by scrdif	" +
                                            "	order by scrdif	" +
                                            "	select case when (SCORE - scr) < 0 then convert(varchar,(scr + ((((SCORE + 1) - scr)/rint) -1 ) * rint)) + '-' + convert(varchar,((scr + ((((SCORE + 1 ) - scr)/rint)-1 ) * rint) + rint - 1))  	" +
                                            "	when (SCORE - scr) >= 0 then convert(varchar,(scr + ((SCORE - scr)/rint) * rint)) + '-' + convert(varchar,((scr + ((SCORE - scr)/rint) * rint) + rint - 1))   	" +
                                            "	end as scrdif,scApproval_flag,CONTRAST_FLAG	" +
                                            "	,  case when CONVERT(VARCHAR,YEAR(APP_DATE))+'-'+RIGHT('00'+CONVERT(VARCHAR,MONTH(APP_DATE)),2) between substring('{7}',1,4)+'-'+ substring('{7}',6,2) and  substring('{8}',1,4)+'-'+ substring('{8}',6,2) then 1 else 0 end as tdd12mAgo	" +
                                            "	,  case when (CONVERT(VARCHAR,YEAR(APP_DATE))+'-'+RIGHT('00'+CONVERT(VARCHAR,MONTH(APP_DATE)),2) between substring('{7}',1,4)+'-'+ substring('{7}',6,2) and  substring('{8}',1,4)+'-'+ substring('{8}',6,2)) and (Approval_Flag ='N') then 1 else 0 end as TDD12YGW	" +
                                            "	,  case when (CONVERT(VARCHAR,YEAR(APP_DATE))+'-'+RIGHT('00'+CONVERT(VARCHAR,MONTH(APP_DATE)),2) between substring('{7}',1,4)+'-'+ substring('{7}',6,2) and  substring('{8}',1,4)+'-'+ substring('{8}',6,2)) and (Approval_Flag ='Y') then 1 else 0 end as TDD12YGA	" +
                                            "	,  case when (CONVERT(VARCHAR,YEAR(APP_DATE))+'-'+RIGHT('00'+CONVERT(VARCHAR,MONTH(APP_DATE)),2) between substring('{7}',1,4)+'-'+ substring('{7}',6,2) and  substring('{8}',1,4)+'-'+ substring('{8}',6,2)) and (Approval_Flag ='Y' and BOOKING_FLAG='Y') then 1 else 0 end as tdd12mygb	" +
                                            "	,  case when (CONVERT(VARCHAR,YEAR(APP_DATE))+'-'+RIGHT('00'+CONVERT(VARCHAR,MONTH(APP_DATE)),2) between substring('{6}',1,4)+'-'+ substring('{6}',6,2) and  substring('{4}',1,4)+'-'+ substring('{4}',6,2)) and (Booking_Flag='Y' and Approval_Flag ='Y') then 1 else 0 end as tdd3mb	" +
                                            "	,  case when (CONVERT(VARCHAR,YEAR(APP_DATE))+'-'+RIGHT('00'+CONVERT(VARCHAR,MONTH(APP_DATE)),2) between substring('{5}',1,4)+'-'+ substring('{5}',6,2) and  substring('{4}',1,4)+'-'+ substring('{4}',6,2)) and (Booking_Flag='Y' and Approval_Flag ='Y') then 1 else 0 end as tdd12mb 	" +
                                            "	into #tdd12mAgo	" +
                                            "	from	" +
                                            "	(	" +
                                                //Tai 2014-01-02
                                                //"	select APP_NO,MODEL,LOAN_CD,SCORE,CBS_STATUS,STYPE_CD,MARKET_CD,DW_SIGN_DATE,APP_DATE,cut_off as scr,rint	" +
                                            "	select distinct APP_NO,MODEL,LOAN_CD,SCORE,CBS_STATUS,STYPE_CD,MARKET_CD,DW_SIGN_DATE,APP_DATE,cut_off as scr,rint	" +
                                                //
                                            "	,CASE WHEN (score>=cut_off) then 'Y' else 'N' end as scapproval_flag	" +
                                            "	,case when CBS_STATUS in (3,6,7) then 'Y' when  CBS_STATUS in (4,5) then 'N' when  CBS_STATUS in (1,2) OR CBS_STATUS is null then '<>' end as Approval_Flag	" +
                                            "	,case when LEFT(CBS_REASON_CD,1)='A' AND  CBS_STATUS in (4,5) then 'Y' 	" +
                                            "	when LEFT(CBS_REASON_CD,1)='R' AND  CBS_STATUS in (3,6,7) then 'Y' when LEFT(CBS_REASON_CD,1)='A' AND  CBS_STATUS in (3,6,7) then 'N'	" +
                                            "	when LEFT(CBS_REASON_CD,1)='R' AND  CBS_STATUS in (4,5) then 'N' else 'N/A' end as CONTRAST_FLAG,	" +
                                            "	case when DW_SIGN_DATE IS NULL THEN 'N' ELSE 'Y' END AS BOOKING_FLAG	" +
                                            "	,CBS_REASON_CD, LEFT(CBS_REASON_CD,1) as CBS_REASON	" +
                                            "	, CONVERT(VARCHAR,YEAR(APP_DATE))+'-'+RIGHT('00'+CONVERT(VARCHAR,MONTH(APP_DATE)),2) AS MYAppDate	" +
                                            "	from	" +
                                            "	(	" +
                                            "	SELECT a.*,b.score,b.model	" +
                                            "	,(select cut_off from ST_SCORERANGE y where  y.ltype= '{0}' and y.lstype = '{1}' ) as cut_off	" +
                                            "	,(select rint from ST_SCORERANGE y where  y.ltype= '{0}' and y.lstype = '{1}' ) as rint	" +
                                            "	      ,CBS_REASON_CD	" +
                                            "	from LN_APP a,LN_GRADE b,CBS_LN_APP c 	" +
                                            "	where a.APP_NO = b.APP_NO  	" +
                                            "	and a.app_no = c.CBS_APP_NO 	" +
                                            "	and  LOAN_CD = '{0}' and STYPE_CD = '{1}' {2} {3} 	" +
                                            "	) lnapp	" +
                                            "	left join	" +
                                            "	(	" +
                                            "	select * 	" +
                                            "	from	" +
                                            "	(	" +
                                            "	select [CBS_APP_NO], DW_SIGN_DATE,LAST_UPDATE_DTM	" +
                                            "	from [DWH_BTFILE] A	" +
                                            " where SUBSTRING([LAST_UPDATE_DTM],7,4)+'-'+SUBSTRING([LAST_UPDATE_DTM],4,2)+'-'+SUBSTRING([LAST_UPDATE_DTM],1,2)=(SELECT MAX(SUBSTRING([LAST_UPDATE_DTM],7,4)+'-'+SUBSTRING([LAST_UPDATE_DTM],4,2)+'-'+SUBSTRING([LAST_UPDATE_DTM],1,2)) FROM [DWH_BTFILE] B WHERE A.CBS_APP_NO=B.CBS_APP_NO)	" +
                                            "	 ) DW_BFILE_T	" +
                                            "	 ) gsb2T	" +
                                            "	 on gsb2T.CBS_APP_NO = lnapp.APP_NO    	" +
                                            "	) Z	" +
                                            "	where  CONTRAST_FLAG='N' and Approval_Flag in ('Y','N') and BOOKING_FLAG in ('Y','N')	" +
                                            "	select scrdif,sum(tdd12mAgo) as tdd12mAgo,sum(TDD12YGW) as TDD12YGW,sum(tdd12mygb) as tdd12mygb	" +
                                            "	,sum(TDD12YGA) as TDD12YGA	" +
                                            "	,sum(tdd3mb) as tdd3mb,sum(tdd12mb) as tdd12mb	" +
                                            "	into #tdd12mAgo2	" +
                                            "	from #tdd12mAgo	" +
                                            "	group by scrdif	" +
                                            "	order by scrdif	" +
                                            "	select row_number() over(order by a.scrdif) as seq,a.scrdif as SCRRG,tdd3m,tdd12m,tdd12mAgo as tdd12yg	" +
                                            "	,tdd3mw,tdd12mw,TDD12YGW as tdd12ygw,tdd3ma,tdd12ma,TDD12YGA as tdd12yga	" +
                                            "	,tdd3mb,tdd12mb,tdd12mygb as tdd12ygb	" +
                                            "	into #Part1	" +
                                            "	from #Main_Table a, #tdd12mAgo2 b	" +
                                            "	where a.scrdif=b.scrdif	" +
                                            "	select 131 as seq, 'No. of Loans' as SCRRG,sum(tdd3m) as tdd3m,sum(tdd12m) as tdd12m	" +
                                            "	,sum(tdd12yg) as tdd12yg,sum(tdd3mw) as tdd3mw	" +
                                            "	,sum(tdd12mw) as tdd12mw,sum(tdd12ygw) as tdd12ygw,sum(tdd3ma) as tdd3ma	" +
                                            "	,sum(tdd12ma) as tdd12ma,sum(tdd12yga) as tdd12yga	" +
                                            "	,sum(tdd3mb) as tdd3mb,sum(tdd12mb) as tdd12mb,sum(tdd12ygb) as tdd12ygb	" +
                                            "	into #No_of_Loan	" +
                                            "	from #Part1	" +
                                            "	select 	" +
                                            "	case when (SCORE - scr) < 0 then convert(varchar	" +
                                            "	,(scr + ((((SCORE + 1) - scr)/rint) -1 ) * rint)) + '-' + convert(varchar,((scr + ((((SCORE + 1 ) - scr)/rint)-1 ) * rint) + rint - 1))  	" +
                                            "	when (SCORE - scr) >= 0 then convert(varchar,(scr + ((SCORE - scr)/rint) * rint)) + '-' 	" +
                                            "	+ convert(varchar,((scr + ((SCORE - scr)/rint) * rint) + rint - 1))   end as scrdif 	" +
                                            "	,scApproval_flag,CONTRAST_FLAG	" +
                                            "	,  case when CONVERT(VARCHAR,YEAR(APP_DATE))+'-'+RIGHT('00'+CONVERT(VARCHAR,MONTH(APP_DATE)),2) between substring('{6}',1,4)+'-'+ substring('{6}',6,2) and  substring('{4}',1,4)+'-'+ substring('{4}',6,2) then score else 0 end as tdd3m_score	" +
                                            "	,  case when CONVERT(VARCHAR,YEAR(APP_DATE))+'-'+RIGHT('00'+CONVERT(VARCHAR,MONTH(APP_DATE)),2) between substring('{5}',1,4)+'-'+ substring('{5}',6,2) and  substring('{4}',1,4)+'-'+ substring('{4}',6,2) then score else 0 end as tdd12m_score	" +
                                            "	,  case when CONVERT(VARCHAR,YEAR(APP_DATE))+'-'+RIGHT('00'+CONVERT(VARCHAR,MONTH(APP_DATE)),2) between substring('{6}',1,4)+'-'+ substring('{6}',6,2) and  substring('{4}',1,4)+'-'+ substring('{4}',6,2) and Approval_Flag ='Y' then score else 0 end as tdd3ma_score	" +
                                            "	,  case when CONVERT(VARCHAR,YEAR(APP_DATE))+'-'+RIGHT('00'+CONVERT(VARCHAR,MONTH(APP_DATE)),2) between substring('{5}',1,4)+'-'+ substring('{5}',6,2) and  substring('{4}',1,4)+'-'+ substring('{4}',6,2) and Approval_Flag ='Y' then score else 0 end as tdd12ma_score	" +
                                            "	,  case when CONVERT(VARCHAR,YEAR(APP_DATE))+'-'+RIGHT('00'+CONVERT(VARCHAR,MONTH(APP_DATE)),2) between substring('{6}',1,4)+'-'+ substring('{6}',6,2) and  substring('{4}',1,4)+'-'+ substring('{4}',6,2) and Approval_Flag ='N' then score else 0 end as tdd3mw_score	" +
                                            "	,  case when CONVERT(VARCHAR,YEAR(APP_DATE))+'-'+RIGHT('00'+CONVERT(VARCHAR,MONTH(APP_DATE)),2) between substring('{5}',1,4)+'-'+ substring('{5}',6,2) and  substring('{4}',1,4)+'-'+ substring('{4}',6,2) and Approval_Flag ='N' then score else 0 end as tdd12mw_score	" +
                                            "	into #Heading_Calculate_Score	" +
                                            "	from 	" +
                                            "	(	" +
                                                //Tai 2014-01-02
                                                //"	select APP_NO,MODEL,LOAN_CD,SCORE,CBS_STATUS,STYPE_CD,MARKET_CD,DW_SIGN_DATE,APP_DATE,cut_off as scr,rint	" +
                                            "	select distinct APP_NO,MODEL,LOAN_CD,SCORE,CBS_STATUS,STYPE_CD,MARKET_CD,DW_SIGN_DATE,APP_DATE,cut_off as scr,rint	" +
                                                //
                                            "	,CASE WHEN (score>=cut_off) then 'Y' else 'N' end as scapproval_flag	" +
                                            "	,case when CBS_STATUS in (3,6,7) then 'Y' when  CBS_STATUS in (4,5) then 'N' when  CBS_STATUS in (1,2) OR CBS_STATUS is null then '<>' end as Approval_Flag	" +
                                            "	,case when LEFT(CBS_REASON_CD,1)='A' AND  CBS_STATUS in (4,5) then 'Y' 	" +
                                            "	when LEFT(CBS_REASON_CD,1)='R' AND  CBS_STATUS in (3,6,7) then 'Y' when LEFT(CBS_REASON_CD,1)='A' AND  CBS_STATUS in (3,6,7) then 'N'	" +
                                            "	when LEFT(CBS_REASON_CD,1)='R' AND  CBS_STATUS in (4,5) then 'N' else 'N/A' end as CONTRAST_FLAG,	" +
                                            "	case when DW_SIGN_DATE IS NULL THEN 'N' ELSE 'Y' END AS BOOKING_FLAG	" +
                                            "	,CBS_REASON_CD, LEFT(CBS_REASON_CD,1) as CBS_REASON	" +
                                            "	, CONVERT(VARCHAR,YEAR(APP_DATE))+'-'+RIGHT('00'+CONVERT(VARCHAR,MONTH(APP_DATE)),2) AS MYAppDate	" +
                                            "	from	" +
                                            "	(	" +
                                            "	SELECT a.*,b.score,b.model	" +
                                            "	,(select cut_off from ST_SCORERANGE y where  y.ltype= '{0}' and y.lstype = '{1}' ) as cut_off	" +
                                            "	,(select rint from ST_SCORERANGE y where  y.ltype= '{0}' and y.lstype = '{1}' ) as rint	" +
                                            "	,CBS_REASON_CD	" +
                                            "	from LN_APP a,LN_GRADE b,CBS_LN_APP c 	" +
                                            "	where a.APP_NO = b.APP_NO  	" +
                                            "	and a.app_no = c.CBS_APP_NO 	" +
                                            "	and  LOAN_CD = '{0}' and STYPE_CD = '{1}' {2} {3} 	" +
                                            "	) lnapp	" +
                                            "	left join	" +
                                            "	(	" +
                                            "	select * 	" +
                                            "	from	" +
                                            "	(	" +
                                            "	select [CBS_APP_NO], DW_SIGN_DATE,LAST_UPDATE_DTM	" +
                                            "	from [DWH_BTFILE] A	" +
                                            " where SUBSTRING([LAST_UPDATE_DTM],7,4)+'-'+SUBSTRING([LAST_UPDATE_DTM],4,2)+'-'+SUBSTRING([LAST_UPDATE_DTM],1,2)=(SELECT MAX(SUBSTRING([LAST_UPDATE_DTM],7,4)+'-'+SUBSTRING([LAST_UPDATE_DTM],4,2)+'-'+SUBSTRING([LAST_UPDATE_DTM],1,2)) FROM [DWH_BTFILE] B WHERE A.CBS_APP_NO=B.CBS_APP_NO)		" +
                                            "	) DW_BFILE_T	" +
                                            "	) gsb2T	" +
                                            "	on gsb2T.CBS_APP_NO = lnapp.APP_NO    	" +
                                            "	) gsbdat  	" +
                                            "	where  CONTRAST_FLAG='N' and Approval_Flag in ('Y','N') and BOOKING_FLAG in ('Y','N') 	" +
                                            "	select case when (SCORE - scr) < 0 then convert(varchar,(scr + ((((SCORE + 1) - scr)/rint) -1 ) * rint)) + '-' + convert(varchar,((scr + ((((SCORE + 1 ) - scr)/rint)-1 ) * rint) + rint - 1))  	" +
                                            "	when (SCORE - scr) >= 0 then convert(varchar,(scr + ((SCORE - scr)/rint) * rint)) + '-' + convert(varchar,((scr + ((SCORE - scr)/rint) * rint) + rint - 1))   	" +
                                            "	end as scrdif,scApproval_flag,CONTRAST_FLAG	" +
                                            "	,  case when CONVERT(VARCHAR,YEAR(APP_DATE))+'-'+RIGHT('00'+CONVERT(VARCHAR,MONTH(APP_DATE)),2) between substring('{7}',1,4)+'-'+ substring('{7}',6,2) and  substring('{8}',1,4)+'-'+ substring('{8}',6,2) then score else 0 end as tdd12mAgo_score	" +
                                            "	,  case when (CONVERT(VARCHAR,YEAR(APP_DATE))+'-'+RIGHT('00'+CONVERT(VARCHAR,MONTH(APP_DATE)),2) between substring('{7}',1,4)+'-'+ substring('{7}',6,2) and  substring('{8}',1,4)+'-'+ substring('{8}',6,2)) and (Approval_Flag ='N') then score else 0 end as TDD12YGW_score	" +
                                            "	,  case when (CONVERT(VARCHAR,YEAR(APP_DATE))+'-'+RIGHT('00'+CONVERT(VARCHAR,MONTH(APP_DATE)),2) between substring('{7}',1,4)+'-'+ substring('{7}',6,2) and  substring('{8}',1,4)+'-'+ substring('{8}',6,2)) and (Approval_Flag ='Y') then score else 0 end as TDD12YGA_score	" +
                                            "	,  case when (CONVERT(VARCHAR,YEAR(APP_DATE))+'-'+RIGHT('00'+CONVERT(VARCHAR,MONTH(APP_DATE)),2) between substring('{7}',1,4)+'-'+ substring('{7}',6,2) and  substring('{8}',1,4)+'-'+ substring('{8}',6,2)) and (Approval_Flag ='Y' and BOOKING_FLAG='Y') then score else 0 end as tdd12mygb_score	" +
                                            "	,  case when (CONVERT(VARCHAR,YEAR(APP_DATE))+'-'+RIGHT('00'+CONVERT(VARCHAR,MONTH(APP_DATE)),2) between substring('{6}',1,4)+'-'+ substring('{6}',6,2) and  substring('{4}',1,4)+'-'+ substring('{4}',6,2)) and (Booking_Flag='Y' and Approval_Flag ='Y') then score else 0 end as tdd3mb_score	" +
                                            "	,  case when (CONVERT(VARCHAR,YEAR(APP_DATE))+'-'+RIGHT('00'+CONVERT(VARCHAR,MONTH(APP_DATE)),2) between substring('{5}',1,4)+'-'+ substring('{5}',6,2) and  substring('{4}',1,4)+'-'+ substring('{4}',6,2)) and (Booking_Flag='Y' and Approval_Flag ='Y') then score else 0 end as tdd12mb_score 	" +
                                            "	into #Score_Cal_P4_1	" +
                                            "	from	" +
                                            "	(	" +
                                                //Tai 2014-01-02
                                                //"	select APP_NO,MODEL,LOAN_CD,SCORE,CBS_STATUS,STYPE_CD,MARKET_CD,DW_SIGN_DATE,APP_DATE,cut_off as scr,rint	" +
                                            "	select distinct APP_NO,MODEL,LOAN_CD,SCORE,CBS_STATUS,STYPE_CD,MARKET_CD,DW_SIGN_DATE,APP_DATE,cut_off as scr,rint	" +
                                                //
                                            "	,CASE WHEN (score>=cut_off) then 'Y' else 'N' end as scapproval_flag	" +
                                            "	,case when CBS_STATUS in (3,6,7) then 'Y' when  CBS_STATUS in (4,5) then 'N' when  CBS_STATUS in (1,2) OR CBS_STATUS is null then '<>' end as Approval_Flag	" +
                                            "	,case when LEFT(CBS_REASON_CD,1)='A' AND  CBS_STATUS in (4,5) then 'Y' 	" +
                                            "	when LEFT(CBS_REASON_CD,1)='R' AND  CBS_STATUS in (3,6,7) then 'Y' when LEFT(CBS_REASON_CD,1)='A' AND  CBS_STATUS in (3,6,7) then 'N'	" +
                                            "	when LEFT(CBS_REASON_CD,1)='R' AND  CBS_STATUS in (4,5) then 'N' else 'N/A' end as CONTRAST_FLAG,	" +
                                            "	case when DW_SIGN_DATE IS NULL THEN 'N' ELSE 'Y' END AS BOOKING_FLAG	" +
                                            "	,CBS_REASON_CD, LEFT(CBS_REASON_CD,1) as CBS_REASON	" +
                                            "	, CONVERT(VARCHAR,YEAR(APP_DATE))+'-'+RIGHT('00'+CONVERT(VARCHAR,MONTH(APP_DATE)),2) AS MYAppDate	" +
                                            "	from	" +
                                            "	(	" +
                                            "	SELECT a.*,b.score,b.model	" +
                                            "	,(select cut_off from ST_SCORERANGE y where  y.ltype= '{0}' and y.lstype = '{1}' ) as cut_off	" +
                                            "	,(select rint from ST_SCORERANGE y where  y.ltype= '{0}' and y.lstype = '{1}' ) as rint	" +
                                            "	      ,CBS_REASON_CD	" +
                                            "	from LN_APP a,LN_GRADE b,CBS_LN_APP c 	" +
                                            "	where a.APP_NO = b.APP_NO  	" +
                                            "	and a.app_no = c.CBS_APP_NO 	" +
                                            "	and  LOAN_CD = '{0}' and STYPE_CD = '{1}' {2} {3} 	" +
                                            "	) lnapp	" +
                                            "	left join	" +
                                            "	(	" +
                                            "	select * 	" +
                                            "	from	" +
                                            "	(	" +
                                            "	select [CBS_APP_NO], DW_SIGN_DATE,LAST_UPDATE_DTM	" +
                                            "	from [DWH_BTFILE] A	" +
                                            " where SUBSTRING([LAST_UPDATE_DTM],7,4)+'-'+SUBSTRING([LAST_UPDATE_DTM],4,2)+'-'+SUBSTRING([LAST_UPDATE_DTM],1,2)=(SELECT MAX(SUBSTRING([LAST_UPDATE_DTM],7,4)+'-'+SUBSTRING([LAST_UPDATE_DTM],4,2)+'-'+SUBSTRING([LAST_UPDATE_DTM],1,2)) FROM [DWH_BTFILE] B WHERE A.CBS_APP_NO=B.CBS_APP_NO)		" +
                                            "	 ) DW_BFILE_T	" +
                                            "	 ) gsb2T	" +
                                            "	 on gsb2T.CBS_APP_NO = lnapp.APP_NO    	" +
                                            "	) Z	" +
                                            "	where  CONTRAST_FLAG='N' and Approval_Flag in ('Y','N') and BOOKING_FLAG in ('Y','N')	" +
                                            "	select scrdif,sum(tdd12mAgo_score) as tdd12mAgo_score	" +
                                            "	,sum(TDD12YGW_score) as TDD12YGW_score,sum(tdd12mygb_score) as tdd12mygb_score	" +
                                            "	,sum(TDD12YGA_score) as TDD12YGA_score	" +
                                            "	,sum(tdd3mb_score) as tdd3mb_score,sum(tdd12mb_score) as tdd12mb_score	" +
                                            "	into #Score_Cal_P4_2	" +
                                            "	from #Score_Cal_P4_1	" +
                                            "	group by scrdif	" +
                                            "	order by scrdif	" +
                                            "	select 129 as seq, 'Total_score' as SCRRG,sum(tdd12mAgo_score) as tdd12mAgo	" +
                                            "	,sum(TDD12YGW_score) as TDD12YGW	" +
                                            "	,sum(TDD12YGA_score) as TDD12YGA	" +
                                            "	,sum(tdd3mb_score) as tdd3mb	" +
                                            "	,sum(tdd12mb_score) as tdd12mb	" +
                                            "	,sum(tdd12mygb_score) as tdd12mygb	" +
                                            "	into #Total_score_P4	" +
                                            "	from #Score_Cal_P4_2	" +
                                            "	select scrdif,sum(tdd3m_score) as tdd3m_score,sum(tdd12m_score) as tdd12m_score,sum(tdd3ma_score) as tdd3ma_score	" +
                                            "	,sum(tdd12ma_score) as tdd12ma_score, sum(tdd3mw_score) as tdd3mw_score,sum(tdd12mw_score) as tdd12mw_score	" +
                                            "	into #Heading_Calculate_Score1	" +
                                            "	from #Heading_Calculate_Score	" +
                                            "	group by scrdif	" +
                                            "	order by scrdif	" +
                                            "	select 129 as seq, 'Total_score' as SCRRG,sum(tdd3m_score) as tdd3m,sum(tdd12m_score) as tdd12m,sum(tdd3ma_score) as tdd3ma	" +
                                            "	,sum(tdd12ma_score) as tdd12ma,sum(tdd3mw_score) as tdd3mw	" +
                                            "	,sum(tdd12mw_score) as tdd12mw	" +
                                            "	into #Heading_Calculate_Score2	" +
                                            "	from #Heading_Calculate_Score1	" +
                                            "	select a.seq,a.SCRRG,tdd3m,tdd12m,tdd12mAgo as tdd12yg,tdd3ma,tdd12ma,TDD12YGA as tdd12yga,tdd3mw,tdd12mw,TDD12YGW as tdd12ygw,tdd3mb,tdd12mb	" +
                                            "	,tdd12mygb as tdd12ygb	" +
                                            "	into #Final_Total_score	" +
                                            "	from #Heading_Calculate_Score2 a, #Total_score_P4 b	" +
                                            "	where a.seq=b.seq	" +
                                            "	select 132 as seq, 'Avg. Score' as SCRRG,convert(decimal(15,3),b.tdd3m)/case when a.tdd3m=0 then 1 else a.tdd3m end as tdd3m	" +
                                            "	,convert(decimal(15,3),b.tdd12m)/case when a.tdd12m=0 then 1 else a.tdd12m end as tdd12m	" +
                                            "	,convert(decimal(15,3),b.tdd12yg)/case when a.tdd12yg=0 then 1 else a.tdd12yg end as tdd12yg	" +
                                            "	,convert(decimal(15,3),b.tdd3mw)/case when a.tdd3mw=0 then 1 else a.tdd3mw end as tdd3mw	" +
                                            "	,convert(decimal(15,3),b.tdd12mw)/case when a.tdd12mw=0 then 1 else  a.tdd12mw end as tdd12mw	" +
                                            "	,convert(decimal(15,3),b.tdd12ygw)/case when a.tdd12ygw=0 then 1 else  a.tdd12ygw end as tdd12ygw	" +
                                            "	,convert(decimal(15,3),b.tdd3ma)/case when a.tdd3ma =0 then 1 else a.tdd3ma end as tdd3ma	" +
                                            "	,convert(decimal(15,3),b.tdd12ma)/case when a.tdd12ma=0 then 1 else a.tdd12ma end as tdd12ma	" +
                                            "	,convert(decimal(15,3),b.tdd12yga)/case when a.tdd12yga=0 then 1 else a.tdd12yga end as tdd12yga	" +
                                            "	,convert(decimal(15,3),b.tdd3mb)/case when a.tdd3mb=0 then 1 else a.tdd3mb end as tdd3mb	" +
                                            "	,convert(decimal(15,3),b.tdd12mb)/case when a.tdd12mb=0  then 1 else a.tdd12mb end as tdd12mb	" +
                                            "	,convert(decimal(15,3),b.tdd12ygb)/case when a.tdd12ygb=0 then 1 else a.tdd12ygb end as tdd12ygb	" +
                                            "	into #Cal_Avg_score	" +
                                            "	from #No_of_Loan a,#Final_Total_score b	" +
                                            "	select sum(tdd3m) as tdd3m,sum(tdd12m) as tdd12m, sum(tdd3ma) as tdd3ma, sum(tdd12ma) as tdd12ma	" +
                                            "	, sum(tdd3mw) as tdd3mw, sum(tdd12mw) as tdd12mw	" +
                                            "	into #Main_cutoff1	" +
                                            "	from #Heading	" +
                                            "	where scApproval_flag='N'	" +
                                            "	select sum(tdd12mAgo) as tdd12yg,sum(TDD12YGW) as TDD12YGW,sum(tdd12mygb) as tdd12ygb	" +
                                            "	,sum(TDD12YGA) as TDD12YGA	" +
                                            "	,sum(tdd3mb) as tdd3mb,sum(tdd12mb) as tdd12mb	" +
                                            "	into #Main_cutoff2	" +
                                            "	from #tdd12mAgo	" +
                                            "	where scApproval_flag='N'	" +
                                            "	select  tdd3m,tdd12m,tdd12yg,tdd3mw,tdd12mw,tdd12ygw,tdd3ma,tdd12ma,tdd12yga,tdd3mb,tdd12mb,tdd12ygb	" +
                                            "	into #Main_cutoff3	" +
                                            "	from #Main_cutoff1 a,#Main_cutoff2 b	" +
                                            "	select 128 as seq,'% Below Cutoff' as SCRRG,convert(decimal(15,3),a.tdd3m)*100.00/case when b.tdd3m=0 then 1 else b.tdd3m end as tdd3m	" +
                                            "	,convert(decimal(15,3),a.tdd12m)*100.00/case when b.tdd12m=0 then 1 else b.tdd12m end as tdd12m	" +
                                            "	,convert(decimal(15,3),a.tdd12yg)*100.00/case when b.tdd12yg=0 then 1 else b.tdd12yg end as tdd12yg	" +
                                            "	,convert(decimal(15,3),a.tdd3mw)*100.00/case when b.tdd3mw=0 then 1 else b.tdd3mw end as tdd3mw		" +
                                            "	,convert(decimal(15,3),a.tdd12mw)*100.00/case when b.tdd12mw=0 then 1 else b.tdd12mw end as tdd12mw	" +
                                            "	,convert(decimal(15,3),a.tdd12ygw)*100.00/case when b.tdd12ygw=0 then 1 else  b.tdd12ygw end as tdd12ygw	" +
                                            "	,convert(decimal(15,3),a.tdd3ma)*100.00/case when b.tdd3ma =0 then 1 else b.tdd3ma end as tdd3ma	" +
                                            "	,convert(decimal(15,3),a.tdd12ma)*100.00/case when b.tdd12ma=0 then 1 else b.tdd12ma end as tdd12ma	" +
                                            "	,convert(decimal(15,3),a.tdd12yga)*100.00/case when b.tdd12yga=0 then 1 else b.tdd12yga end as tdd12yga	" +
                                            "	,convert(decimal(15,3),a.tdd3mb)*100.00/case when b.tdd3mb=0 then 1 else b.tdd3mb end as tdd3mb	" +
                                            "	,convert(decimal(15,3),a.tdd12mb)*100.00/case when b.tdd12mb=0  then 1 else b.tdd12mb end as tdd12mb	" +
                                            "	,convert(decimal(15,3),a.tdd12ygb)*100.00/case when b.tdd12ygb=0 then 1 else b.tdd12ygb end as tdd12ygb	" +
                                            "	into #Main_cutoff_table	" +
                                            "	from #Main_cutoff3 a, #No_of_Loan b	" +
                                            "	select sum(tdd3m) as tdd3m,sum(tdd12m) as tdd12m, sum(tdd3ma) as tdd3ma, sum(tdd12ma) as tdd12ma	" +
                                            "	, sum(tdd3mw) as tdd3mw, sum(tdd12mw) as tdd12mw	" +
                                            "	into #Main_cutoff_Passing1	" +
                                            "	from #Heading	" +
                                            "	where scApproval_flag='Y'	" +
                                            "	select sum(tdd12mAgo) as tdd12yg,sum(TDD12YGW) as TDD12YGW,sum(tdd12mygb) as tdd12ygb	" +
                                            "	,sum(TDD12YGA) as TDD12YGA	" +
                                            "	,sum(tdd3mb) as tdd3mb,sum(tdd12mb) as tdd12mb	" +
                                            "	into #Main_cutoff_Passing2	" +
                                            "	from #tdd12mAgo	" +
                                            "	where scApproval_flag='Y'	" +
                                            "	select  tdd3m,tdd12m,tdd12yg,tdd3mw,tdd12mw,tdd12ygw,tdd3ma,tdd12ma,tdd12yga,tdd3mb,tdd12mb,tdd12ygb	" +
                                            "	into #Main_cutoff_Passing3	" +
                                            "	from #Main_cutoff_Passing1 a,#Main_cutoff_Passing2 b	" +
                                            "	select 130 as seq,'% Passing Cutoff' as SCRRG,convert(decimal(15,3),a.tdd3m)*100.00/case when b.tdd3m=0 then 1 else b.tdd3m end as tdd3m	" +
                                            "	,convert(decimal(15,3),a.tdd12m)*100.00/case when b.tdd12m=0 then 1 else b.tdd12m end as tdd12m	" +
                                            "	,convert(decimal(15,3),a.tdd12yg)*100.00/case when b.tdd12yg=0 then 1 else b.tdd12yg end as tdd12yg	" +
                                            "	,convert(decimal(15,3),a.tdd3mw)*100.00/case when b.tdd3mw=0 then 1 else b.tdd3mw end as tdd3mw	" +
                                            "	,convert(decimal(15,3),a.tdd12mw)*100.00/case when b.tdd12mw=0 then 1 else b.tdd12mw end as tdd12mw	" +
                                            "	,convert(decimal(15,3),a.tdd12ygw)*100.00/case when b.tdd12ygw=0 then 1 else b.tdd12ygw end as tdd12ygw	" +
                                            "	,convert(decimal(15,3),a.tdd3ma)*100.00/case when b.tdd3ma=0 then 1 else b.tdd3ma end as tdd3ma	" +
                                            "	,convert(decimal(15,3),a.tdd12ma)*100.00/case when b.tdd12ma=0 then 1 else b.tdd12ma end as tdd12ma	" +
                                            "	,convert(decimal(15,3),a.tdd12yga)*100.00 /case when b.tdd12yga=0 then 1 else b.tdd12yga end as tdd12yga	" +
                                            "	,convert(decimal(15,3),a.tdd3mb)*100.00/case when b.tdd3mb=0 then 1 else b.tdd3mb end as tdd3mb	" +
                                            "	,convert(decimal(15,3),a.tdd12mb)*100.00/case when b.tdd12mb=0  then 1 else b.tdd12mb end as tdd12mb	" +
                                            "	,convert(decimal(15,3),a.tdd12ygb)*100.00/case when b.tdd12ygb=0 then 1 else b.tdd12ygb end as tdd12ygb	" +
                                            "	into #Main_cutoff_Passing_Table	" +
                                            "	from #Main_cutoff_Passing3 a, #No_of_Loan b	" +
                   " select AA.seq,AA.SCRRG,tdd3m,tdd12m,tdd12yg,tdd3mw,tdd12mw,tdd12ygw,tdd3ma,tdd12ma,tdd12yga,tdd3mb,tdd12mb,tdd12ygb into  #Part1_Range	" +
                   " from (select row_number() over(order by range_name) as seq,range_name as SCRRG from st_scorernglst 	" +
                   " where range_cd = (select range_cd from ST_SCORERANGE where ltype= '{0}' and lstype = '{1}')  ) AA left join (select * from #Part1 ) BB on AA.SCRRG=BB.SCRRG 	" +
                   " select AA.seq,AA.SCRRG,tdd3m,tdd12m,tdd12yg,tdd3mw,tdd12mw,tdd12ygw,tdd3ma,tdd12ma,tdd12yga,tdd3mb,tdd12mb,tdd12ygb	" +
                   " into #Solution  from (select seq,SCRRG,(select sum(tdd3m) from #Part1 t2 where t2.SCRRG <= t1.SCRRG) as tdd3m,(select sum(tdd12m)	" +
                   " from #Part1 t2  where t2.SCRRG <= t1.SCRRG) as tdd12m	,(select sum(tdd12yg) from #Part1 t2 where t2.SCRRG <= t1.SCRRG) as tdd12yg,tdd3mw as [tdd3mw]	" +
                   " ,tdd12mw as [tdd12mw],tdd12ygw as [tdd12ygw]	 ,tdd3ma as [tdd3ma],tdd12ma as [tdd12ma],tdd12yga as [tdd12yga]	" +
                   " ,tdd3mb as [tdd3mb],tdd12mb as [tdd12mb],tdd12ygb as [tdd12ygb]  from #Part1_Range t1   union   select * from #No_of_Loan  ) AA	" +
                       " select * into #PartA from (	" +
                                           " select seq,SCRRG,[dbo].[comma_format](convert(varchar,convert(integer,tdd3m))) as tdd3m,[dbo].[comma_format](convert(varchar,convert(integer,tdd12m))) as tdd12m	" +
                   " ,[dbo].[comma_format](convert(varchar,convert(integer,tdd12yg))) as tdd12yg,[dbo].[comma_format](convert(varchar,convert(integer,tdd3mw))) as tdd3mw	" +
                   " ,[dbo].[comma_format](convert(varchar,convert(integer,tdd12mw))) as tdd12mw,[dbo].[comma_format](convert(varchar,convert(integer,tdd12ygw))) as tdd12ygw	" +
                   " ,[dbo].[comma_format](convert(varchar,convert(integer,tdd3ma))) as tdd3ma,[dbo].[comma_format](convert(varchar,convert(integer,tdd12ma))) as tdd12ma " +
                   " ,[dbo].[comma_format](convert(varchar,convert(integer,tdd12yga))) as tdd12yga,[dbo].[comma_format](convert(varchar,convert(integer,tdd3mb)))	as tdd3mb" +
                   " ,[dbo].[comma_format](convert(varchar,convert(integer,tdd12mb))) as tdd12mb,[dbo].[comma_format](convert(varchar,convert(integer,tdd12ygb)))	as tdd12ygb " +
                   " from #Solution	" +
                                            "	union	" +
                   " select seq,SCRRG,[dbo].[comma_percent_format](convert(varchar,convert(decimal(15,2),tdd3m))) as tdd3m,[dbo].[comma_percent_format](convert(varchar,convert(decimal(15,2),tdd12m))) as tdd12m " +
                   " ,[dbo].[comma_percent_format](convert(varchar,convert(decimal(15,2),tdd12yg))) as tdd12yg,[dbo].[comma_percent_format](convert(varchar,convert(decimal(15,2),tdd3mw))) as tdd3mw " +
                   " ,[dbo].[comma_percent_format](convert(varchar,convert(decimal(15,2),tdd12mw))) as tdd12mw,[dbo].[comma_percent_format](convert(varchar,convert(decimal(15,2),tdd12ygw))) as tdd12ygw " +
                   " ,[dbo].[comma_percent_format](convert(varchar,convert(decimal(15,2),tdd3ma))) as tdd3ma,[dbo].[comma_percent_format](convert(varchar,convert(decimal(15,2),tdd12ma))) as tdd12ma " +
                   " ,[dbo].[comma_percent_format](convert(varchar,convert(decimal(15,2),tdd12yga))) as tdd12yga,[dbo].[comma_percent_format](convert(varchar,convert(decimal(15,2),tdd3mb))) as tdd3mb,[dbo].[comma_percent_format](convert(varchar,convert(decimal(15,2),tdd12mb))) as tdd12mb,[dbo].[comma_percent_format](convert(varchar,convert(decimal(15,2),tdd12ygb))) as tdd12ygb " +
                                            "  from #Cal_Avg_score	" +
                                                "	union	" +
                                                " select seq,SCRRG,convert(varchar,convert(decimal(15,2),tdd3m)) as tdd3m,convert(varchar,convert(decimal(15,2),tdd12m)) as tdd12m " +
                   " ,convert(varchar,convert(decimal(15,2),tdd12yg)) as tdd12yg,convert(varchar,convert(decimal(15,2),tdd3mw)) as tdd3mw " +
                   " ,convert(varchar,convert(decimal(15,2),tdd12mw)) as tdd12mw,convert(varchar,convert(decimal(15,2),tdd12ygw)) as tdd12ygw " +
                   " ,convert(varchar,convert(decimal(15,2),tdd3ma)) as tdd3ma,convert(varchar,convert(decimal(15,2),tdd12ma)) as tdd12ma " +
                   " ,convert(varchar,convert(decimal(15,2),tdd12yga)) as tdd12yga,convert(varchar,convert(decimal(15,2),tdd3mb)) as tdd3mb,convert(varchar,convert(decimal(15,2),tdd12mb)) as tdd12mb,convert(varchar,convert(decimal(15,2),tdd12ygb)) as tdd12ygb " +
                   "   from #Main_cutoff_table	" +
                                                "	union	" +
                   " select seq,SCRRG,convert(varchar,convert(decimal(15,2),tdd3m)) as tdd3m,convert(varchar,convert(decimal(15,2),tdd12m)) as tdd12m " +
                   " ,convert(varchar,convert(decimal(15,2),tdd12yg)) as tdd12yg,convert(varchar,convert(decimal(15,2),tdd3mw)) as tdd3mw " +
                   " ,convert(varchar,convert(decimal(15,2),tdd12mw)) as tdd12mw,convert(varchar,convert(decimal(15,2),tdd12ygw)) as tdd12ygw " +
                   " ,convert(varchar,convert(decimal(15,2),tdd3ma)) as tdd3ma,convert(varchar,convert(decimal(15,2),tdd12ma)) as tdd12ma " +
                   " ,convert(varchar,convert(decimal(15,2),tdd12yga)) as tdd12yga,convert(varchar,convert(decimal(15,2),tdd3mb)) as tdd3mb,convert(varchar,convert(decimal(15,2),tdd12mb)) as tdd12mb,convert(varchar,convert(decimal(15,2),tdd12ygb)) as tdd12ygb " +
                   "   from #Main_cutoff_Passing_Table ) Z  " +
                   "select * into #Format_NULL_CASE from ( select 0 as seq,'Score Range' as SCRRG,'3 Months' as tdd3m,'12 Months' as tdd12m,'12 Months' as tdd12yg,'3 Months' as tdd3mw " +
                   " ,'12 Months' as tdd12mw,'12 Months' as tdd12ygw,'3 Months' as tdd3ma,'12 Months' as tdd12ma,'12 Months' as tdd12yga,'3 Months' as tdd3mb " +
                   ",'12 Months' as tdd12mb,'12 Months' as tdd12ygb	union " +
                   " select seq,SCRRG ,case when tdd3m is null then '0' else tdd3m end as tdd3m	 " +
                   " ,case when tdd12m is null then '0' else tdd12m end as tdd12m	 " +
                   " ,case when tdd12yg is null then '0' else tdd12yg end as tdd12yg	 " +
                   " ,case when tdd3mw is null then '0' else tdd3mw end as tdd3mw	 " +
                   " ,case when tdd12mw is null then '0' else tdd12mw end as tdd12mw	 " +
                   " ,case when tdd12ygw is null then '0' else tdd12ygw end as tdd12ygw	 " +
                   " ,case when tdd3ma is null then '0' else tdd3ma end as tdd3ma	 " +
                   " ,case when tdd12ma is null then '0' else tdd12ma end as tdd12ma	 " +
                   " ,case when tdd12yga is null then '0' else tdd12yga end as tdd12yga	 " +
                   " ,case when tdd3mb is null then '0' else tdd3mb end as tdd3mb	 " +
                   " ,case when tdd12mb is null then '0' else tdd12mb end as tdd12mb	 " +
                   " ,case when tdd12ygb is null then '0' else tdd12ygb end as tdd12ygb from #PartA where (seq < 100 or seq=131)  union	 " +
                   " select seq,SCRRG ,case when tdd3m is null then '0.00' else tdd3m end as tdd3m	 " +
                   " ,case when tdd12m is null then '0.00' else tdd12m end as tdd12m	 " +
                   " ,case when tdd12yg is null then '0.00' else tdd12yg end as tdd12yg	 " +
                   " ,case when tdd3mw is null then '0.00' else tdd3mw end as tdd3mw	 " +
                   " ,case when tdd12mw is null then '0.00' else tdd12mw end as tdd12mw	 " +
                   " ,case when tdd12ygw is null then '0.00' else tdd12ygw end as tdd12ygw	 " +
                   " ,case when tdd3ma is null then '0.00' else tdd3ma end as tdd3ma	 " +
                   " ,case when tdd12ma is null then '0.00' else tdd12ma end as tdd12ma	 " +
                   " ,case when tdd12yga is null then '0.00' else tdd12yga end as tdd12yga	 " +
                   " ,case when tdd3mb is null then '0.00' else tdd3mb end as tdd3mb	 " +
                   " ,case when tdd12mb is null then '0.00' else tdd12mb end as tdd12mb	 " +
                   " ,case when tdd12ygb is null then '0.00' else tdd12ygb end as tdd12ygb	from #PartA  where seq=128 or seq=130 or seq=132 ) y order by seq	 " +
                   "  select SCRRG,tdd3m,tdd12m,tdd12yg,tdd3mw,tdd12mw,tdd12ygw,tdd3ma,tdd12ma,tdd12yga,tdd3mb,tdd12mb,tdd12ygb from #Format_NULL_CASE order by seq	 " +
                                            "	drop table #Main_cutoff_table	drop table #Solution " +
                                            "	drop table #Cal_Avg_score " +
                                            "	drop table  #Final_Total_score " +
                                            "	drop table #Heading_Calculate_Score	" +
                                            "	drop table #Heading_Calculate_Score1	" +
                                            "	drop table #Heading_Calculate_Score2	" +
                                            "	drop table #Score_Cal_P4_1	" +
                                            "	drop table #Score_Cal_P4_2	" +
                                            "	drop table #Total_score_P4	" +
                                            "	drop table #No_of_Loan	 drop table  #Part1_Range " +
                                            "	drop table #Part1	drop table #PartA " +
                                            "	drop table  #Main_Table	" +
                                            "	drop table #tdd12mAgo	" +
                                            "	drop table #tdd12mAgo2	" +
                                            "	drop table #Heading	" +
                                            "	drop table #Main_cutoff2	drop table #Format_NULL_CASE" +
                                            "	drop table #Main_cutoff1	" +
                                            "	drop table #Main_cutoff3	" +
                                            "	drop table #Main_cutoff_Passing3	" +
                                            "	drop table #Main_cutoff_Passing2	" +
                                            "	drop table #Main_cutoff_Passing1	" +
                                            "	drop table #Main_cutoff_Passing_Table	"
                   , ddlLoan.SelectedItem.Value, ddlSubType.SelectedItem.Value, mktqrf, mktqrt, start.ToString("yyyy-MM"), start12y.ToString("yyyy-MM"), end3m.ToString("yyyy-MM"), end12yg.ToString("yyyy-MM"), start12yg.ToString("yyyy-MM"));
                                        }
                                        else
                                        // Begin Score Percent 
                                        {
                                            appQuery1 = string.Format("	select 	case when (SCORE - scr) < 0 then convert(varchar	" +
                                                             "	,(scr + ((((SCORE + 1) - scr)/rint) -1 ) * rint)) + '-' + convert(varchar,((scr + ((((SCORE + 1 ) - scr)/rint)-1 ) * rint) + rint - 1))  	" +
                                                             "	when (SCORE - scr) >= 0 then convert(varchar,(scr + ((SCORE - scr)/rint) * rint)) + '-' 	" +
                                                             "	+ convert(varchar,((scr + ((SCORE - scr)/rint) * rint) + rint - 1))   end as scrdif 	" +
                                                             "	,scApproval_flag,CONTRAST_FLAG	" +
                                        "	,  case when CONVERT(VARCHAR,YEAR(APP_DATE))+'-'+RIGHT('00'+CONVERT(VARCHAR,MONTH(APP_DATE)),2) between substring('{6}',1,4)+'-'+ substring('{6}',6,2) and  substring('{4}',1,4)+'-'+ substring('{4}',6,2) then 1 else 0 end as tdd3m	" +
                                        "	,  case when CONVERT(VARCHAR,YEAR(APP_DATE))+'-'+RIGHT('00'+CONVERT(VARCHAR,MONTH(APP_DATE)),2) between substring('{5}',1,4)+'-'+ substring('{5}',6,2) and  substring('{4}',1,4)+'-'+ substring('{4}',6,2) then 1 else 0 end as tdd12m	" +
                                        "	,  case when CONVERT(VARCHAR,YEAR(APP_DATE))+'-'+RIGHT('00'+CONVERT(VARCHAR,MONTH(APP_DATE)),2) between substring('{6}',1,4)+'-'+ substring('{6}',6,2) and  substring('{4}',1,4)+'-'+ substring('{4}',6,2) and Approval_Flag ='Y' then 1 else 0 end as tdd3ma	" +
                                        "	,  case when CONVERT(VARCHAR,YEAR(APP_DATE))+'-'+RIGHT('00'+CONVERT(VARCHAR,MONTH(APP_DATE)),2) between substring('{5}',1,4)+'-'+ substring('{5}',6,2) and  substring('{4}',1,4)+'-'+ substring('{4}',6,2) and Approval_Flag ='Y' then 1 else 0 end as tdd12ma	" +
                                        "	,  case when CONVERT(VARCHAR,YEAR(APP_DATE))+'-'+RIGHT('00'+CONVERT(VARCHAR,MONTH(APP_DATE)),2) between substring('{6}',1,4)+'-'+ substring('{6}',6,2) and  substring('{4}',1,4)+'-'+ substring('{4}',6,2) and Approval_Flag ='N' then 1 else 0 end as tdd3mw	" +
                                        "	,  case when CONVERT(VARCHAR,YEAR(APP_DATE))+'-'+RIGHT('00'+CONVERT(VARCHAR,MONTH(APP_DATE)),2) between substring('{5}',1,4)+'-'+ substring('{5}',6,2) and  substring('{4}',1,4)+'-'+ substring('{4}',6,2) and Approval_Flag ='N' then 1 else 0 end as tdd12mw	" +
                                                             "	into #Heading	" +
                                                             "	from 	" +
                                                //Tai 2014-01-02
                                                //"	(	select APP_NO,MODEL,LOAN_CD,SCORE,CBS_STATUS,STYPE_CD,MARKET_CD,DW_SIGN_DATE,APP_DATE,cut_off as scr,rint	" +
                                                             "	(	select distinct APP_NO,MODEL,LOAN_CD,SCORE,CBS_STATUS,STYPE_CD,MARKET_CD,DW_SIGN_DATE,APP_DATE,cut_off as scr,rint	" +
                                                //
                                                             "	,CASE WHEN (score>=cut_off) then 'Y' else 'N' end as scapproval_flag	" +
                                                             "	,case when CBS_STATUS in (3,6,7) then 'Y' when  CBS_STATUS in (4,5) then 'N' when  CBS_STATUS in (1,2) OR CBS_STATUS is null then '<>' end as Approval_Flag	" +
                                                             "	,case when LEFT(CBS_REASON_CD,1)='A' AND  CBS_STATUS in (4,5) then 'Y' 	" +
                                                             "	when LEFT(CBS_REASON_CD,1)='R' AND  CBS_STATUS in (3,6,7) then 'Y' when LEFT(CBS_REASON_CD,1)='A' AND  CBS_STATUS in (3,6,7) then 'N'	" +
                                                             "	when LEFT(CBS_REASON_CD,1)='R' AND  CBS_STATUS in (4,5) then 'N' else 'N/A' end as CONTRAST_FLAG,	" +
                                                             "	case when DW_SIGN_DATE IS NULL THEN 'N' ELSE 'Y' END AS BOOKING_FLAG	" +
                                                             "	,CBS_REASON_CD, LEFT(CBS_REASON_CD,1) as CBS_REASON	" +
                                                             "	, CONVERT(VARCHAR,YEAR(APP_DATE))+'-'+RIGHT('00'+CONVERT(VARCHAR,MONTH(APP_DATE)),2) AS MYAppDate	" +
                                                             "	from	" +
                                                             "	(	" +
                                                             "	SELECT a.*,b.score,b.model	" +
                                                             "	,(select cut_off from ST_SCORERANGE y where  y.ltype= '{0}' and y.lstype = '{1}' ) as cut_off	" +
                                                             "	,(select rint from ST_SCORERANGE y where  y.ltype= '{0}' and y.lstype = '{1}' ) as rint	" +
                                                             "	,CBS_REASON_CD	" +
                                                             "	from LN_APP a,LN_GRADE b,CBS_LN_APP c 	" +
                                                             "	where a.APP_NO = b.APP_NO  	" +
                                                             "	and a.app_no = c.CBS_APP_NO 	" +
                                                             "	and  LOAN_CD = '{0}' and STYPE_CD = '{1}' {2} {3} 	" +
                                                             "	) lnapp	" +
                                                             "	left join	" +
                                                             "	(	" +
                                                             "	select * 	" +
                                                             "	from		" +
                                                             "	(	" +
                                                             "	select [CBS_APP_NO], DW_SIGN_DATE,LAST_UPDATE_DTM	" +
                                                             "	from [DWH_BTFILE] A	" +
                                                             " where SUBSTRING([LAST_UPDATE_DTM],7,4)+'-'+SUBSTRING([LAST_UPDATE_DTM],4,2)+'-'+SUBSTRING([LAST_UPDATE_DTM],1,2)=(SELECT MAX(SUBSTRING([LAST_UPDATE_DTM],7,4)+'-'+SUBSTRING([LAST_UPDATE_DTM],4,2)+'-'+SUBSTRING([LAST_UPDATE_DTM],1,2)) FROM [DWH_BTFILE] B WHERE A.CBS_APP_NO=B.CBS_APP_NO)		" +
                                                             "	) DW_BFILE_T	" +
                                                             "	) gsb2T	" +
                                                             "	 on gsb2T.CBS_APP_NO = lnapp.APP_NO    	" +
                                                             "	) gsbdat  	" +
                                                             "	where  CONTRAST_FLAG='N' and Approval_Flag in ('Y','N') and BOOKING_FLAG in ('Y','N') 	" +
                                                             "	select scrdif,sum(tdd3m) as tdd3m,sum(tdd12m) as tdd12m, sum(tdd3ma) as tdd3ma, sum(tdd12ma) as tdd12ma	" +
                                                             "	,sum(tdd3mw) as tdd3mw, sum(tdd12mw) as tdd12mw	" +
                                                             "	into #Main_Table	" +
                                                             "	from #Heading	" +
                                                             "	group by scrdif	" +
                                                             "	order by scrdif	" +
                                                             "	select case when (SCORE - scr) < 0 then convert(varchar,(scr + ((((SCORE + 1) - scr)/rint) -1 ) * rint)) + '-' + convert(varchar,((scr + ((((SCORE + 1 ) - scr)/rint)-1 ) * rint) + rint - 1))  	" +
                                                             "	when (SCORE - scr) >= 0 then convert(varchar,(scr + ((SCORE - scr)/rint) * rint)) + '-' + convert(varchar,((scr + ((SCORE - scr)/rint) * rint) + rint - 1))   	" +
                                                             "	end as scrdif,scApproval_flag,CONTRAST_FLAG	" +
                                        "	,  case when CONVERT(VARCHAR,YEAR(APP_DATE))+'-'+RIGHT('00'+CONVERT(VARCHAR,MONTH(APP_DATE)),2) between substring('{7}',1,4)+'-'+ substring('{7}',6,2) and  substring('{8}',1,4)+'-'+ substring('{8}',6,2) then 1 else 0 end as tdd12mAgo	" +
                                        "	,  case when (CONVERT(VARCHAR,YEAR(APP_DATE))+'-'+RIGHT('00'+CONVERT(VARCHAR,MONTH(APP_DATE)),2) between substring('{7}',1,4)+'-'+ substring('{7}',6,2) and  substring('{8}',1,4)+'-'+ substring('{8}',6,2)) and (Approval_Flag ='N') then 1 else 0 end as TDD12YGW	" +
                                        "	,  case when (CONVERT(VARCHAR,YEAR(APP_DATE))+'-'+RIGHT('00'+CONVERT(VARCHAR,MONTH(APP_DATE)),2) between substring('{7}',1,4)+'-'+ substring('{7}',6,2) and  substring('{8}',1,4)+'-'+ substring('{8}',6,2)) and (Approval_Flag ='Y') then 1 else 0 end as TDD12YGA	" +
                                        "	,  case when (CONVERT(VARCHAR,YEAR(APP_DATE))+'-'+RIGHT('00'+CONVERT(VARCHAR,MONTH(APP_DATE)),2) between substring('{7}',1,4)+'-'+ substring('{7}',6,2) and  substring('{8}',1,4)+'-'+ substring('{8}',6,2)) and (Approval_Flag ='Y' and BOOKING_FLAG='Y') then 1 else 0 end as tdd12mygb	" +
                                        "	,  case when (CONVERT(VARCHAR,YEAR(APP_DATE))+'-'+RIGHT('00'+CONVERT(VARCHAR,MONTH(APP_DATE)),2) between substring('{6}',1,4)+'-'+ substring('{6}',6,2) and  substring('{4}',1,4)+'-'+ substring('{4}',6,2)) and (Booking_Flag='Y' and Approval_Flag ='Y') then 1 else 0 end as tdd3mb	" +
                                        "	,  case when (CONVERT(VARCHAR,YEAR(APP_DATE))+'-'+RIGHT('00'+CONVERT(VARCHAR,MONTH(APP_DATE)),2) between substring('{5}',1,4)+'-'+ substring('{5}',6,2) and  substring('{4}',1,4)+'-'+ substring('{4}',6,2)) and (Booking_Flag='Y' and Approval_Flag ='Y') then 1 else 0 end as tdd12mb 	" +
                                                             "	into #tdd12mAgo	" +
                                                             "	from	" +
                                                             "	(	" +
                                                //Tai 2014-01-02
                                                //"	select APP_NO,MODEL,LOAN_CD,SCORE,CBS_STATUS,STYPE_CD,MARKET_CD,DW_SIGN_DATE,APP_DATE,cut_off as scr,rint	" +
                                                             "	select distinct APP_NO,MODEL,LOAN_CD,SCORE,CBS_STATUS,STYPE_CD,MARKET_CD,DW_SIGN_DATE,APP_DATE,cut_off as scr,rint	" +
                                                //
                                                             "	,CASE WHEN (score>=cut_off) then 'Y' else 'N' end as scapproval_flag	" +
                                                             "	,case when CBS_STATUS in (3,6,7) then 'Y' when  CBS_STATUS in (4,5) then 'N' when  CBS_STATUS in (1,2) OR CBS_STATUS is null then '<>' end as Approval_Flag	" +
                                                             "	,case when LEFT(CBS_REASON_CD,1)='A' AND  CBS_STATUS in (4,5) then 'Y' 	" +
                                                             "	when LEFT(CBS_REASON_CD,1)='R' AND  CBS_STATUS in (3,6,7) then 'Y' when LEFT(CBS_REASON_CD,1)='A' AND  CBS_STATUS in (3,6,7) then 'N'	" +
                                                             "	when LEFT(CBS_REASON_CD,1)='R' AND  CBS_STATUS in (4,5) then 'N' else 'N/A' end as CONTRAST_FLAG,	" +
                                                             "	case when DW_SIGN_DATE IS NULL THEN 'N' ELSE 'Y' END AS BOOKING_FLAG	" +
                                                             "	,CBS_REASON_CD, LEFT(CBS_REASON_CD,1) as CBS_REASON	" +
                                                             "	, CONVERT(VARCHAR,YEAR(APP_DATE))+'-'+RIGHT('00'+CONVERT(VARCHAR,MONTH(APP_DATE)),2) AS MYAppDate	" +
                                                             "	from	" +
                                                             "	(	" +
                                                             "	SELECT a.*,b.score,b.model	" +
                                                             "	,(select cut_off from ST_SCORERANGE y where  y.ltype= '{0}' and y.lstype = '{1}' ) as cut_off	" +
                                                             "	,(select rint from ST_SCORERANGE y where  y.ltype= '{0}' and y.lstype = '{1}' ) as rint	" +
                                                             "	      ,CBS_REASON_CD	" +
                                                             "	from LN_APP a,LN_GRADE b,CBS_LN_APP c 	" +
                                                             "	where a.APP_NO = b.APP_NO  	" +
                                                             "	and a.app_no = c.CBS_APP_NO 	" +
                                                             "	and  LOAN_CD = '{0}' and STYPE_CD = '{1}' {2} {3} 	" +
                                                             "	) lnapp	" +
                                                             "	left join	" +
                                                             "	(	" +
                                                             "	select * 	" +
                                                             "	from	" +
                                                             "	(	" +
                                                             "	select [CBS_APP_NO], DW_SIGN_DATE,LAST_UPDATE_DTM	" +
                                                             "	from [DWH_BTFILE] A	" +
                                         "	where SUBSTRING([LAST_UPDATE_DTM],7,4)+'-'+SUBSTRING([LAST_UPDATE_DTM],4,2)+'-'+SUBSTRING([LAST_UPDATE_DTM],1,2)=(SELECT MAX(SUBSTRING([LAST_UPDATE_DTM],7,4)+'-'+SUBSTRING([LAST_UPDATE_DTM],4,2)+'-'+SUBSTRING([LAST_UPDATE_DTM],1,2)) FROM [DWH_BTFILE] B WHERE A.CBS_APP_NO=B.CBS_APP_NO)		" +
                                                             "	 ) DW_BFILE_T	" +
                                                             "	 ) gsb2T	" +
                                                             "	 on gsb2T.CBS_APP_NO = lnapp.APP_NO    	" +
                                                             "	) Z	" +
                                                             "	where  CONTRAST_FLAG='N' and Approval_Flag in ('Y','N') and BOOKING_FLAG in ('Y','N')	" +
                                                             "	select scrdif,sum(tdd12mAgo) as tdd12mAgo,sum(TDD12YGW) as TDD12YGW,sum(tdd12mygb) as tdd12mygb	" +
                                                             "	,sum(TDD12YGA) as TDD12YGA	" +
                                                             "	,sum(tdd3mb) as tdd3mb,sum(tdd12mb) as tdd12mb	" +
                                                             "	into #tdd12mAgo2	" +
                                                             "	from #tdd12mAgo	" +
                                                             "	group by scrdif	" +
                                                             "	order by scrdif	" +
                                                             "	select row_number() over(order by a.scrdif) as seq,a.scrdif as SCRRG,tdd3m,tdd12m,tdd12mAgo as tdd12yg	" +
                                                             "	,tdd3mw,tdd12mw,TDD12YGW as tdd12ygw,tdd3ma,tdd12ma,TDD12YGA as tdd12yga	" +
                                                             "	,tdd3mb,tdd12mb,tdd12mygb as tdd12ygb	" +
                                                             "	into #Part1	" +
                                                             "	from #Main_Table a, #tdd12mAgo2 b	" +
                                                             "	where a.scrdif=b.scrdif	" +
                                                             "	select 131 as seq, 'No. of Loans' as SCRRG,sum(tdd3m) as tdd3m,sum(tdd12m) as tdd12m	" +
                                                             "	,sum(tdd12yg) as tdd12yg,sum(tdd3mw) as tdd3mw	" +
                                                             "	,sum(tdd12mw) as tdd12mw,sum(tdd12ygw) as tdd12ygw,sum(tdd3ma) as tdd3ma	" +
                                                             "	,sum(tdd12ma) as tdd12ma,sum(tdd12yga) as tdd12yga	" +
                                                             "	,sum(tdd3mb) as tdd3mb,sum(tdd12mb) as tdd12mb,sum(tdd12ygb) as tdd12ygb	" +
                                                             "	into #No_of_Loan	" +
                                                             "	from #Part1	" +
                                                             "	select 	" +
                                                             "	case when (SCORE - scr) < 0 then convert(varchar	" +
                                                             "	,(scr + ((((SCORE + 1) - scr)/rint) -1 ) * rint)) + '-' + convert(varchar,((scr + ((((SCORE + 1 ) - scr)/rint)-1 ) * rint) + rint - 1))  	" +
                                                             "	when (SCORE - scr) >= 0 then convert(varchar,(scr + ((SCORE - scr)/rint) * rint)) + '-' 	" +
                                                             "	+ convert(varchar,((scr + ((SCORE - scr)/rint) * rint) + rint - 1))   end as scrdif 	" +
                                                             "	,scApproval_flag,CONTRAST_FLAG	" +
                                        "	,  case when CONVERT(VARCHAR,YEAR(APP_DATE))+'-'+RIGHT('00'+CONVERT(VARCHAR,MONTH(APP_DATE)),2) between substring('{6}',1,4)+'-'+ substring('{6}',6,2) and  substring('{4}',1,4)+'-'+ substring('{4}',6,2) then score else 0 end as tdd3m_score	" +
                                        "	,  case when CONVERT(VARCHAR,YEAR(APP_DATE))+'-'+RIGHT('00'+CONVERT(VARCHAR,MONTH(APP_DATE)),2) between substring('{5}',1,4)+'-'+ substring('{5}',6,2) and  substring('{4}',1,4)+'-'+ substring('{4}',6,2) then score else 0 end as tdd12m_score	" +
                                        "	,  case when CONVERT(VARCHAR,YEAR(APP_DATE))+'-'+RIGHT('00'+CONVERT(VARCHAR,MONTH(APP_DATE)),2) between substring('{6}',1,4)+'-'+ substring('{6}',6,2) and  substring('{4}',1,4)+'-'+ substring('{4}',6,2) and Approval_Flag ='Y' then score else 0 end as tdd3ma_score	" +
                                        "	,  case when CONVERT(VARCHAR,YEAR(APP_DATE))+'-'+RIGHT('00'+CONVERT(VARCHAR,MONTH(APP_DATE)),2) between substring('{5}',1,4)+'-'+ substring('{5}',6,2) and  substring('{4}',1,4)+'-'+ substring('{4}',6,2) and Approval_Flag ='Y' then score else 0 end as tdd12ma_score	" +
                                        "	,  case when CONVERT(VARCHAR,YEAR(APP_DATE))+'-'+RIGHT('00'+CONVERT(VARCHAR,MONTH(APP_DATE)),2) between substring('{6}',1,4)+'-'+ substring('{6}',6,2) and  substring('{4}',1,4)+'-'+ substring('{4}',6,2) and Approval_Flag ='N' then score else 0 end as tdd3mw_score	" +
                                        "	,  case when CONVERT(VARCHAR,YEAR(APP_DATE))+'-'+RIGHT('00'+CONVERT(VARCHAR,MONTH(APP_DATE)),2) between substring('{5}',1,4)+'-'+ substring('{5}',6,2) and  substring('{4}',1,4)+'-'+ substring('{4}',6,2) and Approval_Flag ='N' then score else 0 end as tdd12mw_score	" +
                                                             "	into #Heading_Calculate_Score	" +
                                                             "	from 	" +
                                                             "	(	" +
                                                //Tai 2014-01-02
                                                //"	select APP_NO,MODEL,LOAN_CD,SCORE,CBS_STATUS,STYPE_CD,MARKET_CD,DW_SIGN_DATE,APP_DATE,cut_off as scr,rint	" +
                                                             "	select distinct APP_NO,MODEL,LOAN_CD,SCORE,CBS_STATUS,STYPE_CD,MARKET_CD,DW_SIGN_DATE,APP_DATE,cut_off as scr,rint	" +
                                                //
                                                             "	,CASE WHEN (score>=cut_off) then 'Y' else 'N' end as scapproval_flag	" +
                                                             "	,case when CBS_STATUS in (3,6,7) then 'Y' when  CBS_STATUS in (4,5) then 'N' when  CBS_STATUS in (1,2) OR CBS_STATUS is null then '<>' end as Approval_Flag	" +
                                                             "	,case when LEFT(CBS_REASON_CD,1)='A' AND  CBS_STATUS in (4,5) then 'Y' 	" +
                                                             "	when LEFT(CBS_REASON_CD,1)='R' AND  CBS_STATUS in (3,6,7) then 'Y' when LEFT(CBS_REASON_CD,1)='A' AND  CBS_STATUS in (3,6,7) then 'N'	" +
                                                             "	when LEFT(CBS_REASON_CD,1)='R' AND  CBS_STATUS in (4,5) then 'N' else 'N/A' end as CONTRAST_FLAG,	" +
                                                             "	case when DW_SIGN_DATE IS NULL THEN 'N' ELSE 'Y' END AS BOOKING_FLAG	" +
                                                             "	,CBS_REASON_CD, LEFT(CBS_REASON_CD,1) as CBS_REASON	" +
                                                             "	, CONVERT(VARCHAR,YEAR(APP_DATE))+'-'+RIGHT('00'+CONVERT(VARCHAR,MONTH(APP_DATE)),2) AS MYAppDate	" +
                                                             "	from	" +
                                                             "	(	" +
                                                             "	SELECT a.*,b.score,b.model	" +
                                                             "	,(select cut_off from ST_SCORERANGE y where  y.ltype= '{0}' and y.lstype = '{1}' ) as cut_off	" +
                                                             "	,(select rint from ST_SCORERANGE y where  y.ltype= '{0}' and y.lstype = '{1}' ) as rint	" +
                                                             "	,CBS_REASON_CD	" +
                                                             "	from LN_APP a,LN_GRADE b,CBS_LN_APP c 	" +
                                                             "	where a.APP_NO = b.APP_NO  	" +
                                                             "	and a.app_no = c.CBS_APP_NO 	" +
                                                             "	and  LOAN_CD = '{0}' and STYPE_CD = '{1}' {2} {3} 	" +
                                                             "	) lnapp	" +
                                                             "	left join	" +
                                                             "	(	" +
                                                             "	select * 	" +
                                                             "	from	" +
                                                             "	(	" +
                                                             "	select [CBS_APP_NO], DW_SIGN_DATE,LAST_UPDATE_DTM	" +
                                                             "	from [DWH_BTFILE] A	" +
                                                             " where SUBSTRING([LAST_UPDATE_DTM],7,4)+'-'+SUBSTRING([LAST_UPDATE_DTM],4,2)+'-'+SUBSTRING([LAST_UPDATE_DTM],1,2)=(SELECT MAX(SUBSTRING([LAST_UPDATE_DTM],7,4)+'-'+SUBSTRING([LAST_UPDATE_DTM],4,2)+'-'+SUBSTRING([LAST_UPDATE_DTM],1,2)) FROM [DWH_BTFILE] B WHERE A.CBS_APP_NO=B.CBS_APP_NO)		" +
                                                             "	) DW_BFILE_T	" +
                                                             "	) gsb2T	" +
                                                             "	on gsb2T.CBS_APP_NO = lnapp.APP_NO    	" +
                                                             "	) gsbdat  	" +
                                                             "	where  CONTRAST_FLAG='N' and Approval_Flag in ('Y','N') and BOOKING_FLAG in ('Y','N') 	" +
                                                             "	select case when (SCORE - scr) < 0 then convert(varchar,(scr + ((((SCORE + 1) - scr)/rint) -1 ) * rint)) + '-' + convert(varchar,((scr + ((((SCORE + 1 ) - scr)/rint)-1 ) * rint) + rint - 1))  	" +
                                                             "	when (SCORE - scr) >= 0 then convert(varchar,(scr + ((SCORE - scr)/rint) * rint)) + '-' + convert(varchar,((scr + ((SCORE - scr)/rint) * rint) + rint - 1))   	" +
                                                             "	end as scrdif,scApproval_flag,CONTRAST_FLAG	" +
                                        "	,  case when CONVERT(VARCHAR,YEAR(APP_DATE))+'-'+RIGHT('00'+CONVERT(VARCHAR,MONTH(APP_DATE)),2) between substring('{7}',1,4)+'-'+ substring('{7}',6,2) and  substring('{8}',1,4)+'-'+ substring('{8}',6,2) then score else 0 end as tdd12mAgo_score	" +
                                        "	,  case when (CONVERT(VARCHAR,YEAR(APP_DATE))+'-'+RIGHT('00'+CONVERT(VARCHAR,MONTH(APP_DATE)),2) between substring('{7}',1,4)+'-'+ substring('{7}',6,2) and  substring('{8}',1,4)+'-'+ substring('{8}',6,2)) and (Approval_Flag ='N') then score else 0 end as TDD12YGW_score	" +
                                        "	,  case when (CONVERT(VARCHAR,YEAR(APP_DATE))+'-'+RIGHT('00'+CONVERT(VARCHAR,MONTH(APP_DATE)),2) between substring('{7}',1,4)+'-'+ substring('{7}',6,2) and  substring('{8}',1,4)+'-'+ substring('{8}',6,2)) and (Approval_Flag ='Y') then score else 0 end as TDD12YGA_score	" +
                                        "	,  case when (CONVERT(VARCHAR,YEAR(APP_DATE))+'-'+RIGHT('00'+CONVERT(VARCHAR,MONTH(APP_DATE)),2) between substring('{7}',1,4)+'-'+ substring('{7}',6,2) and  substring('{8}',1,4)+'-'+ substring('{8}',6,2)) and (Approval_Flag ='Y' and BOOKING_FLAG='Y') then score else 0 end as tdd12mygb_score	" +
                                        "	,  case when (CONVERT(VARCHAR,YEAR(APP_DATE))+'-'+RIGHT('00'+CONVERT(VARCHAR,MONTH(APP_DATE)),2) between substring('{6}',1,4)+'-'+ substring('{6}',6,2) and  substring('{4}',1,4)+'-'+ substring('{4}',6,2)) and (Booking_Flag='Y' and Approval_Flag ='Y') then score else 0 end as tdd3mb_score	" +
                                        "	,  case when (CONVERT(VARCHAR,YEAR(APP_DATE))+'-'+RIGHT('00'+CONVERT(VARCHAR,MONTH(APP_DATE)),2) between substring('{5}',1,4)+'-'+ substring('{5}',6,2) and  substring('{4}',1,4)+'-'+ substring('{4}',6,2)) and (Booking_Flag='Y' and Approval_Flag ='Y') then score else 0 end as tdd12mb_score 	" +
                                                             "	into #Score_Cal_P4_1	" +
                                                             "	from	" +
                                                             "	(	" +
                                                //Tai 2014-01-02
                                                //"	select APP_NO,MODEL,LOAN_CD,SCORE,CBS_STATUS,STYPE_CD,MARKET_CD,DW_SIGN_DATE,APP_DATE,cut_off as scr,rint	" +
                                                             "	select distinct APP_NO,MODEL,LOAN_CD,SCORE,CBS_STATUS,STYPE_CD,MARKET_CD,DW_SIGN_DATE,APP_DATE,cut_off as scr,rint	" +
                                                //
                                                             "	,CASE WHEN (score>=cut_off) then 'Y' else 'N' end as scapproval_flag	" +
                                                             "	,case when CBS_STATUS in (3,6,7) then 'Y' when  CBS_STATUS in (4,5) then 'N' when  CBS_STATUS in (1,2) OR CBS_STATUS is null then '<>' end as Approval_Flag	" +
                                                             "	,case when LEFT(CBS_REASON_CD,1)='A' AND  CBS_STATUS in (4,5) then 'Y' 	" +
                                                             "	when LEFT(CBS_REASON_CD,1)='R' AND  CBS_STATUS in (3,6,7) then 'Y' when LEFT(CBS_REASON_CD,1)='A' AND  CBS_STATUS in (3,6,7) then 'N'	" +
                                                             "	when LEFT(CBS_REASON_CD,1)='R' AND  CBS_STATUS in (4,5) then 'N' else 'N/A' end as CONTRAST_FLAG,	" +
                                                             "	case when DW_SIGN_DATE IS NULL THEN 'N' ELSE 'Y' END AS BOOKING_FLAG	" +
                                                             "	,CBS_REASON_CD, LEFT(CBS_REASON_CD,1) as CBS_REASON	" +
                                                             "	, CONVERT(VARCHAR,YEAR(APP_DATE))+'-'+RIGHT('00'+CONVERT(VARCHAR,MONTH(APP_DATE)),2) AS MYAppDate	" +
                                                             "	from	" +
                                                             "	(	" +
                                                             "	SELECT a.*,b.score,b.model	" +
                                                             "	,(select cut_off from ST_SCORERANGE y where  y.ltype= '{0}' and y.lstype = '{1}' ) as cut_off	" +
                                                             "	,(select rint from ST_SCORERANGE y where  y.ltype= '{0}' and y.lstype = '{1}' ) as rint	" +
                                                             "	      ,CBS_REASON_CD	" +
                                                             "	from LN_APP a,LN_GRADE b,CBS_LN_APP c 	" +
                                                             "	where a.APP_NO = b.APP_NO  	" +
                                                             "	and a.app_no = c.CBS_APP_NO 	" +
                                                             "	and  LOAN_CD = '{0}' and STYPE_CD = '{1}' {2} {3} 	" +
                                                             "	) lnapp	" +
                                                             "	left join	" +
                                                             "	(	" +
                                                             "	select * 	" +
                                                             "	from	" +
                                                             "	(	" +
                                                             "	select [CBS_APP_NO], DW_SIGN_DATE,LAST_UPDATE_DTM	" +
                                                             "	from [DWH_BTFILE] A	" +
                                        "	where SUBSTRING([LAST_UPDATE_DTM],7,4)+'-'+SUBSTRING([LAST_UPDATE_DTM],4,2)+'-'+SUBSTRING([LAST_UPDATE_DTM],1,2)=(SELECT MAX(SUBSTRING([LAST_UPDATE_DTM],7,4)+'-'+SUBSTRING([LAST_UPDATE_DTM],4,2)+'-'+SUBSTRING([LAST_UPDATE_DTM],1,2)) FROM [DWH_BTFILE] B WHERE A.CBS_APP_NO=B.CBS_APP_NO)		" +
                                                             "	 ) DW_BFILE_T	" +
                                                             "	 ) gsb2T	" +
                                                             "	 on gsb2T.CBS_APP_NO = lnapp.APP_NO    	" +
                                                             "	) Z	" +
                                                             "	where  CONTRAST_FLAG='N' and Approval_Flag in ('Y','N') and BOOKING_FLAG in ('Y','N')	" +
                                                             "	select scrdif,sum(tdd12mAgo_score) as tdd12mAgo_score	" +
                                                             "	,sum(TDD12YGW_score) as TDD12YGW_score,sum(tdd12mygb_score) as tdd12mygb_score	" +
                                                             "	,sum(TDD12YGA_score) as TDD12YGA_score	" +
                                                             "	,sum(tdd3mb_score) as tdd3mb_score,sum(tdd12mb_score) as tdd12mb_score	" +
                                                             "	into #Score_Cal_P4_2	" +
                                                             "	from #Score_Cal_P4_1	" +
                                                             "	group by scrdif	" +
                                                             "	order by scrdif	" +
                                                             "	select 129 as seq, 'Total_score' as SCRRG,sum(tdd12mAgo_score) as tdd12mAgo	" +
                                                             "	,sum(TDD12YGW_score) as TDD12YGW	" +
                                                             "	,sum(TDD12YGA_score) as TDD12YGA	" +
                                                             "	,sum(tdd3mb_score) as tdd3mb	" +
                                                             "	,sum(tdd12mb_score) as tdd12mb	" +
                                                             "	,sum(tdd12mygb_score) as tdd12mygb	" +
                                                             "	into #Total_score_P4	" +
                                                             "	from #Score_Cal_P4_2	" +
                                                             "	select scrdif,sum(tdd3m_score) as tdd3m_score,sum(tdd12m_score) as tdd12m_score,sum(tdd3ma_score) as tdd3ma_score	" +
                                                             "	,sum(tdd12ma_score) as tdd12ma_score, sum(tdd3mw_score) as tdd3mw_score,sum(tdd12mw_score) as tdd12mw_score	" +
                                                             "	into #Heading_Calculate_Score1	" +
                                                             "	from #Heading_Calculate_Score	" +
                                                             "	group by scrdif	" +
                                                             "	order by scrdif	" +
                                                             "	select 129 as seq, 'Total_score' as SCRRG,sum(tdd3m_score) as tdd3m,sum(tdd12m_score) as tdd12m,sum(tdd3ma_score) as tdd3ma	" +
                                                             "	,sum(tdd12ma_score) as tdd12ma,sum(tdd3mw_score) as tdd3mw	" +
                                                             "	,sum(tdd12mw_score) as tdd12mw	" +
                                                             "	into #Heading_Calculate_Score2	" +
                                                             "	from #Heading_Calculate_Score1	" +
                                                             "	select a.seq,a.SCRRG,tdd3m,tdd12m,tdd12mAgo as tdd12yg,tdd3ma,tdd12ma,TDD12YGA as tdd12yga,tdd3mw,tdd12mw,TDD12YGW as tdd12ygw,tdd3mb,tdd12mb	" +
                                                             "	,tdd12mygb as tdd12ygb	" +
                                                             "	into #Final_Total_score	" +
                                                             "	from #Heading_Calculate_Score2 a, #Total_score_P4 b	" +
                                                             "	where a.seq=b.seq	" +
                                                             "	select 132 as seq, 'Avg. Score' as SCRRG,convert(decimal(15,3),b.tdd3m)/case when a.tdd3m=0 then 1 else a.tdd3m end as tdd3m	" +
                                                             "	,convert(decimal(15,3),b.tdd12m)/case when a.tdd12m=0 then 1 else a.tdd12m end as tdd12m	" +
                                                             "	,convert(decimal(15,3),b.tdd12yg)/case when a.tdd12yg=0 then 1 else a.tdd12yg end as tdd12yg	" +
                                                             "	,convert(decimal(15,3),b.tdd3mw)/case when a.tdd3mw=0 then 1 else a.tdd3mw end as tdd3mw	" +
                                                             "	,convert(decimal(15,3),b.tdd12mw)/case when a.tdd12mw=0 then 1 else  a.tdd12mw end as tdd12mw	" +
                                                             "	,convert(decimal(15,3),b.tdd12ygw)/case when a.tdd12ygw=0 then 1 else  a.tdd12ygw end as tdd12ygw	" +
                                                             "	,convert(decimal(15,3),b.tdd3ma)/case when a.tdd3ma =0 then 1 else a.tdd3ma end as tdd3ma	" +
                                                             "	,convert(decimal(15,3),b.tdd12ma)/case when a.tdd12ma=0 then 1 else a.tdd12ma end as tdd12ma	" +
                                                             "	,convert(decimal(15,3),b.tdd12yga)/case when a.tdd12yga=0 then 1 else a.tdd12yga end as tdd12yga	" +
                                                             "	,convert(decimal(15,3),b.tdd3mb)/case when a.tdd3mb=0 then 1 else a.tdd3mb end as tdd3mb	" +
                                                             "	,convert(decimal(15,3),b.tdd12mb)/case when a.tdd12mb=0  then 1 else a.tdd12mb end as tdd12mb	" +
                                                             "	,convert(decimal(15,3),b.tdd12ygb)/case when a.tdd12ygb=0 then 1 else a.tdd12ygb end as tdd12ygb	" +
                                                             "	into #Cal_Avg_score	" +
                                                             "	from #No_of_Loan a,#Final_Total_score b	" +
                                                             "	select sum(tdd3m) as tdd3m,sum(tdd12m) as tdd12m, sum(tdd3ma) as tdd3ma, sum(tdd12ma) as tdd12ma	" +
                                                             "	, sum(tdd3mw) as tdd3mw, sum(tdd12mw) as tdd12mw	" +
                                                             "	into #Main_cutoff1	" +
                                                             "	from #Heading	" +
                                                             "	where scApproval_flag='N'	" +
                                                             "	select sum(tdd12mAgo) as tdd12yg,sum(TDD12YGW) as TDD12YGW,sum(tdd12mygb) as tdd12ygb	" +
                                                             "	,sum(TDD12YGA) as TDD12YGA	" +
                                                             "	,sum(tdd3mb) as tdd3mb,sum(tdd12mb) as tdd12mb	" +
                                                             "	into #Main_cutoff2	" +
                                                             "	from #tdd12mAgo	" +
                                                             "	where scApproval_flag='N'	" +
                                                             "	select  tdd3m,tdd12m,tdd12yg,tdd3mw,tdd12mw,tdd12ygw,tdd3ma,tdd12ma,tdd12yga,tdd3mb,tdd12mb,tdd12ygb	" +
                                                             "	into #Main_cutoff3	" +
                                                             "	from #Main_cutoff1 a,#Main_cutoff2 b	" +
                                                             "	select 128 as seq,'% Below Cutoff' as SCRRG,convert(decimal(15,3),a.tdd3m)*100.00/case when b.tdd3m=0 then 1 else b.tdd3m end as tdd3m	" +
                                                             "	,convert(decimal(15,3),a.tdd12m)*100.00/case when b.tdd12m=0 then 1 else b.tdd12m end as tdd12m	" +
                                                             "	,convert(decimal(15,3),a.tdd12yg)*100.00/case when b.tdd12yg=0 then 1 else b.tdd12yg end as tdd12yg	" +
                                                             "	,convert(decimal(15,3),a.tdd3mw)*100.00/case when b.tdd3mw=0 then 1 else b.tdd3mw end as tdd3mw	" +
                                                             "	,convert(decimal(15,3),a.tdd12mw)*100.00/case when b.tdd12mw=0 then 1 else b.tdd12mw end as tdd12mw	" +
                                                             "	,convert(decimal(15,3),a.tdd12ygw)*100.00/case when b.tdd12ygw=0 then 1 else  b.tdd12ygw end as tdd12ygw	" +
                                                             "	,convert(decimal(15,3),a.tdd3ma)*100.00/case when b.tdd3ma =0 then 1 else b.tdd3ma end as tdd3ma	" +
                                                             "	,convert(decimal(15,3),a.tdd12ma)*100.00/case when b.tdd12ma=0 then 1 else b.tdd12ma end as tdd12ma	" +
                                                             "	,convert(decimal(15,3),a.tdd12yga)*100.00/case when b.tdd12yga=0 then 1 else b.tdd12yga end as tdd12yga	" +
                                                             "	,convert(decimal(15,3),a.tdd3mb)*100.00/case when b.tdd3mb=0 then 1 else b.tdd3mb end as tdd3mb	" +
                                                             "	,convert(decimal(15,3),a.tdd12mb)*100.00/case when b.tdd12mb=0  then 1 else b.tdd12mb end as tdd12mb	" +
                                                             "	,convert(decimal(15,3),a.tdd12ygb)*100.00/case when b.tdd12ygb=0 then 1 else b.tdd12ygb end as tdd12ygb	" +
                                                             "	into #Main_cutoff_table	" +
                                                             "	from #Main_cutoff3 a, #No_of_Loan b	" +
                                                             "	select sum(tdd3m) as tdd3m,sum(tdd12m) as tdd12m, sum(tdd3ma) as tdd3ma, sum(tdd12ma) as tdd12ma	" +
                                                             "	, sum(tdd3mw) as tdd3mw, sum(tdd12mw) as tdd12mw	" +
                                                             "	into #Main_cutoff_Passing1	" +
                                                             "	from #Heading	" +
                                                             "	where scApproval_flag='Y'	" +
                                                             "	select sum(tdd12mAgo) as tdd12yg,sum(TDD12YGW) as TDD12YGW,sum(tdd12mygb) as tdd12ygb	" +
                                                             "	,sum(TDD12YGA) as TDD12YGA	" +
                                                             "	,sum(tdd3mb) as tdd3mb,sum(tdd12mb) as tdd12mb	" +
                                                             "	into #Main_cutoff_Passing2	" +
                                                             "	from #tdd12mAgo	" +
                                                             "	where scApproval_flag='Y'	" +
                                                             "	select  tdd3m,tdd12m,tdd12yg,tdd3mw,tdd12mw,tdd12ygw,tdd3ma,tdd12ma,tdd12yga,tdd3mb,tdd12mb,tdd12ygb	" +
                                                             "	into #Main_cutoff_Passing3	" +
                                                             "	from #Main_cutoff_Passing1 a,#Main_cutoff_Passing2 b	" +
                                                             "	select 130 as seq,'% Passing Cutoff' as SCRRG,convert(decimal(15,3),a.tdd3m)*100.00/case when b.tdd3m=0 then 1 else b.tdd3m end as tdd3m	" +
                                                             "	,convert(decimal(15,3),a.tdd12m)*100.00/case when b.tdd12m=0 then 1 else b.tdd12m end as tdd12m	" +
                                                             "	,convert(decimal(15,3),a.tdd12yg)*100.00/case when b.tdd12yg=0 then 1 else b.tdd12yg end as tdd12yg	" +
                                                             "	,convert(decimal(15,3),a.tdd3mw)*100.00/case when b.tdd3mw=0 then 1 else b.tdd3mw end as tdd3mw	" +
                                                             "	,convert(decimal(15,3),a.tdd12mw)*100.00/case when b.tdd12mw=0 then 1 else b.tdd12mw end as tdd12mw	" +
                                                             "	,convert(decimal(15,3),a.tdd12ygw)*100.00/case when b.tdd12ygw=0 then 1 else b.tdd12ygw end as tdd12ygw	" +
                                                             "	,convert(decimal(15,3),a.tdd3ma)*100.00/case when b.tdd3ma=0 then 1 else b.tdd3ma end as tdd3ma	" +
                                                             "	,convert(decimal(15,3),a.tdd12ma)*100.00/case when b.tdd12ma=0 then 1 else b.tdd12ma end as tdd12ma	" +
                                                             "	,convert(decimal(15,3),a.tdd12yga)*100.00 /case when b.tdd12yga=0 then 1 else b.tdd12yga end as tdd12yga	" +
                                                             "	,convert(decimal(15,3),a.tdd3mb)*100.00/case when b.tdd3mb=0 then 1 else b.tdd3mb end as tdd3mb	" +
                                                             "	,convert(decimal(15,3),a.tdd12mb)*100.00/case when b.tdd12mb=0  then 1 else b.tdd12mb end as tdd12mb	" +
                                                             "	,convert(decimal(15,3),a.tdd12ygb)*100.00/case when b.tdd12ygb=0 then 1 else b.tdd12ygb end as tdd12ygb	" +
                                                             "	into #Main_cutoff_Passing_Table	" +
                                                             "	from #Main_cutoff_Passing3 a, #No_of_Loan b	" +
                                        "	select AA.seq,AA.SCRRG,tdd3m,tdd12m,tdd12yg,tdd3mw,tdd12mw,tdd12ygw,tdd3ma,tdd12ma,tdd12yga,tdd3mb,tdd12mb,tdd12ygb	into  #Part1_Range	" +
                                        " from 	 ( select row_number() over(order by range_name) as seq,range_name as SCRRG from st_scorernglst 	" +
                                        " where range_cd = (select range_cd from ST_SCORERANGE where ltype= '{0}' and lstype = '{1}') ) AA left join	  (	select * from #Part1 ) BB on AA.SCRRG=BB.SCRRG 	" +
                                        " select seq,SCRRG, (select sum(tdd3m) from #Part1 t2 	where t2.scrrg <= t1.scrrg) as [Cum_tdd3m]		" +
                                        " , (select sum(tdd12m) from #Part1 t2 	where t2.scrrg <= t1.scrrg) as [Cum_tdd12m]		" +
                                        " ,(select sum(tdd12yg) from #Part1 t2 	where t2.scrrg <= t1.scrrg) as [Cum_tdd12yg]	" +
                                        " ,tdd3mw as [Cum_tdd3mw],tdd12mw as [Cum_tdd12mw],tdd12ygw as [Cum_tdd12ygw]	" +
                                        " ,tdd3ma as [Cum_tdd3ma],tdd12ma as [Cum_tdd12ma],tdd12yga as [Cum_tdd12yga],tdd3mb as [Cum_tdd3mb],tdd12mb as [Cum_tdd12mb],tdd12ygb as [Cum_tdd12ygb]	" +
                                        " into #Calculate_Percent from #Part1_Range t1	" +
                                        " select a.seq,a.SCRRG,convert(decimal(15,3),[Cum_tdd3m])*100.00/case when b.tdd3m=0 then 1 else b.tdd3m end as tdd3m 	" +
                                        " ,convert(decimal(15,3),[Cum_tdd12m])*100.00/case when b.tdd12m=0 then 1 else tdd12m end as tdd12m 	" +
                                        " ,convert(decimal(15,3),[Cum_tdd12yg])*100.00/case when b.tdd12yg=0 then 1 else b.tdd12yg end as tdd12yg 	" +
                                        " into #Result_TTD from #Calculate_Percent a,#No_of_Loan b	" +
                                        " select a.seq,a.SCRRG	" +
                                        " ,convert(decimal(15,3),[Cum_tdd3mw])*100.00/case when tdd3m=0 then 1 else tdd3m end  as tdd3mw 		" +
                                        " ,convert(decimal(15,3),[Cum_tdd12mw])*100.00/case when tdd12m=0 then 1 else tdd12m end  as tdd12mw 	" +
                                        " ,convert(decimal(15,3),[Cum_tdd12ygw])*100.00/case when tdd12yg=0 then 1 else tdd12yg end as tdd12ygw 	" +
                                        " ,convert(decimal(15,3),[Cum_tdd3ma])*100.00/case when tdd3m=0 then 1 else tdd3m  end as tdd3ma 	" +
                                        " ,convert(decimal(15,3),[Cum_tdd12ma])*100.00/case when tdd12m=0 then 1 else tdd12m  end  as tdd12ma	" +
                                        " ,convert(decimal(15,3),[Cum_tdd12yga])*100.00/case when tdd12yg=0 then 1 else tdd12yg end as tdd12yga 	" +
                                        " ,convert(decimal(15,3),[Cum_tdd3mb])*100.00/case when tdd3ma=0 then 1 else tdd3ma end as tdd3mb 	 	" +
                                        " ,convert(decimal(15,3),[Cum_tdd12mb])*100.00/case when tdd12ma=0 then 1 else  tdd12ma end as tdd12mb 		" +
                                        " ,convert(decimal(15,3),[Cum_tdd12ygb])*100.00/case when tdd12yga=0 then 1 else tdd12yga end as tdd12ygb 	into #Result_Apv_Rej_Book	" +
                                        " from #Calculate_Percent a,#Part1_Range b where a.scrrg=b.scrrg	" +
                                        " select a.seq,a.SCRRG,a.tdd3m,a.tdd12m,a.tdd12yg,b.tdd3mw,b.tdd12mw,b.tdd12ygw,b.tdd3ma,b.tdd12ma,b.tdd12yga,b.tdd3mb,b.tdd12mb,b.tdd12ygb	" +
                                        " into #Solution  from #Result_TTD a,#Result_Apv_Rej_Book b 	" +
                                        " where a.seq=b.seq	" +
                                        "	select *	into #PartA	from	(	" +
                                        "	 select seq,SCRRG,convert(varchar,convert(decimal(15,2),tdd3m)) as tdd3m,convert(varchar,convert(decimal(15,2)	" +
                                        "	 ,tdd12m)) as tdd12m  ,convert(varchar,convert(decimal(15,2),tdd12yg)) as tdd12yg,convert(varchar,convert(decimal(15,2)	" +
                                        "	 ,tdd3mw)) as tdd3mw  ,convert(varchar,convert(decimal(15,2),tdd12mw)) as tdd12mw,convert(varchar,convert(decimal(15,2)	" +
                                        "	 ,tdd12ygw)) as tdd12ygw  ,convert(varchar,convert(decimal(15,2),tdd3ma)) as tdd3ma,convert(varchar,convert(decimal(15,2)	" +
                                        "	 ,tdd12ma)) as tdd12ma  ,convert(varchar,convert(decimal(15,2),tdd12yga)) as tdd12yga,convert(varchar,convert(decimal(15,2)	" +
                                        "	 ,tdd3mb)) as tdd3mb,convert(varchar,convert(decimal(15,2),tdd12mb)) as tdd12mb	" +
                                        " ,convert(varchar,convert(decimal(15,2),tdd12ygb)) as tdd12ygb from 	" +
                                        "	 (  select * from #Solution	" +
                                        "	 union	  select *  from #Cal_Avg_score 	" +
                                        "	 union	 select *  from #Main_cutoff_table 	" +
                                        "	 union	 select *  from #Main_cutoff_Passing_Table 	 ) A union	" +
                                        "	 select seq,SCRRG,[dbo].[comma_format](convert(varchar,convert(integer,tdd3m))) as tdd3m	" +
                                        "	 ,[dbo].[comma_format](convert(varchar,convert(integer,tdd12m))) as tdd12m	 	" +
                                        "	 ,[dbo].[comma_format](convert(varchar,convert(integer,tdd12yg))) as tdd12yg	" +
                                        "	 ,[dbo].[comma_format](convert(varchar,convert(integer,tdd3mw))) as tdd3mw		" +
                                        "  ,[dbo].[comma_format](convert(varchar,convert(integer,tdd12mw))) as tdd12mw	" +
                                        "  ,[dbo].[comma_format](convert(varchar,convert(integer,tdd12ygw))) as tdd12ygw		" +
                                        "  ,[dbo].[comma_format](convert(varchar,convert(integer,tdd3ma))) as tdd3ma	" +
                                        "  ,[dbo].[comma_format](convert(varchar,convert(integer,tdd12ma))) as tdd12ma 	 	" +
                                        "  ,[dbo].[comma_format](convert(varchar,convert(integer,tdd12yga))) as tdd12yga	" +
                                        "  ,[dbo].[comma_format](convert(varchar,convert(integer,tdd3mb))) as tdd3mb	 	" +
                                        "  ,[dbo].[comma_format](convert(varchar,convert(integer,tdd12mb))) as tdd12mb	" +
                                        "  ,[dbo].[comma_format](convert(varchar,convert(integer,tdd12ygb))) as tdd12ygb 		" +
                                        " from  #No_of_Loan	) Z 	" +
                                        "    select * into #Format_NULL_CASE from (select 0 as seq,'Score Range' as SCRRG,'3 Months' as tdd3m,'12 Months' as tdd12m,'12 Months' as tdd12yg,'3 Months' as tdd3mw	" +
                                        " ,'12 Months' as tdd12mw,'12 Months' as tdd12ygw,'3 Months' as tdd3ma,'12 Months' as tdd12ma,'12 Months' as tdd12yga,'3 Months' as tdd3mb	" +
                                        " ,'12 Months' as tdd12mb,'12 Months' as tdd12ygb	UNION select seq,SCRRG	 " +
                                        " ,case when tdd3m is null then '0' else tdd3m end as tdd3m	 " +
                                        " ,case when tdd12m is null then '0' else tdd12m end as tdd12m	 " +
                                        " ,case when tdd12yg is null then '0' else tdd12yg end as tdd12yg	 " +
                                        " ,case when tdd3mw is null then '0' else tdd3mw end as tdd3mw	 " +
                                        " ,case when tdd12mw is null then '0' else tdd12mw end as tdd12mw	 " +
                                        " ,case when tdd12ygw is null then '0' else tdd12ygw end as tdd12ygw	 " +
                                        " ,case when tdd3ma is null then '0' else tdd3ma end as tdd3ma	 " +
                                        " ,case when tdd12ma is null then '0' else tdd12ma end as tdd12ma	 " +
                                        " ,case when tdd12yga is null then '0' else tdd12yga end as tdd12yga	 " +
                                        " ,case when tdd3mb is null then '0' else tdd3mb end as tdd3mb	 " +
                                        " ,case when tdd12mb is null then '0' else tdd12mb end as tdd12mb	 " +
                                        " ,case when tdd12ygb is null then '0' else tdd12ygb end as tdd12ygb from #PartA where  seq=131  union	 " +
                                        " select seq,SCRRG ,case when tdd3m is null then '0.00' else tdd3m end as tdd3m	 " +
                                        " ,case when tdd12m is null then '0.00' else tdd12m end as tdd12m	 " +
                                        " ,case when tdd12yg is null then '0.00' else tdd12yg end as tdd12yg	 " +
                                        " ,case when tdd3mw is null then '0.00' else tdd3mw end as tdd3mw	 " +
                                        " ,case when tdd12mw is null then '0.00' else tdd12mw end as tdd12mw	 " +
                                        " ,case when tdd12ygw is null then '0.00' else tdd12ygw end as tdd12ygw	 " +
                                        " ,case when tdd3ma is null then '0.00' else tdd3ma end as tdd3ma	 " +
                                        " ,case when tdd12ma is null then '0.00' else tdd12ma end as tdd12ma	 " +
                                        " ,case when tdd12yga is null then '0.00' else tdd12yga end as tdd12yga	 " +
                                        " ,case when tdd3mb is null then '0.00' else tdd3mb end as tdd3mb	 " +
                                        " ,case when tdd12mb is null then '0.00' else tdd12mb end as tdd12mb	 " +
                                        " ,case when tdd12ygb is null then '0.00' else tdd12ygb end as tdd12ygb	from #PartA  where (seq < 100 or seq=128 or seq=130 or seq=132)  ) y order by seq	 " +
                                        "  select SCRRG,tdd3m,tdd12m,tdd12yg,tdd3mw,tdd12mw,tdd12ygw,tdd3ma,tdd12ma,tdd12yga,tdd3mb,tdd12mb,tdd12ygb from #Format_NULL_CASE order by seq	 " +
                                         "	drop table #Main_cutoff_table	drop table #Result_TTD	drop table #Solution" +
                                                             "	drop table #Cal_Avg_score	drop table #Result_Apv_Rej_Book " +
                                                             "	drop table  #Final_Total_score	" +
                                                             "	drop table #Heading_Calculate_Score	" +
                                                             "	drop table #Heading_Calculate_Score1	" +
                                                             "	drop table #Heading_Calculate_Score2	" +
                                                             "	drop table #Score_Cal_P4_1	" +
                                                             "	drop table #Score_Cal_P4_2	" +
                                                             "	drop table #Total_score_P4	" +
                                                             "	drop table #No_of_Loan	drop table #Format_NULL_CASE " +
                                                             "	drop table #Part1	drop table  #Part1_Range  drop table #Calculate_Percent" +
                                                             "	drop table  #Main_Table	" +
                                                             "	drop table #tdd12mAgo	" +
                                                             "	drop table #tdd12mAgo2	" +
                                                             "	drop table #Heading	drop table #PartA " +
                                                             "	drop table #Main_cutoff2	" +
                                                             "	drop table #Main_cutoff1	" +
                                                             "	drop table #Main_cutoff3	" +
                                                             "	drop table #Main_cutoff_Passing3	" +
                                                             "	drop table #Main_cutoff_Passing2	" +
                                                             "	drop table #Main_cutoff_Passing1	" +
                                                "	drop table #Main_cutoff_Passing_Table	"
                                                , ddlLoan.SelectedItem.Value, ddlSubType.SelectedItem.Value, mktqrf, mktqrt, start.ToString("yyyy-MM"), start12y.ToString("yyyy-MM"), end3m.ToString("yyyy-MM"), end12yg.ToString("yyyy-MM"), start12yg.ToString("yyyy-MM"));
                                        }
                                    } //Score Part end Start Grade Calculation Part
                                    else
                                    {
                                        if (rptcttype.SelectedValue == "Number") //Grade Number
                                        {
                                            appQuery1 = string.Format("	select 	" +
                                            "	case when (SCORE - scr) < 0 then convert(varchar	" +
                                            "	,(scr + ((((SCORE + 1) - scr)/rint) -1 ) * rint)) + '-' + convert(varchar,((scr + ((((SCORE + 1 ) - scr)/rint)-1 ) * rint) + rint - 1))  	" +
                                            "	when (SCORE - scr) >= 0 then convert(varchar,(scr + ((SCORE - scr)/rint) * rint)) + '-' 	" +
                                            "	+ convert(varchar,((scr + ((SCORE - scr)/rint) * rint) + rint - 1))   end as scrdif 	" +
                                            "	,scApproval_flag,CONTRAST_FLAG	" +
                    "	,  case when CONVERT(VARCHAR,YEAR(APP_DATE))+'-'+RIGHT('00'+CONVERT(VARCHAR,MONTH(APP_DATE)),2) between substring('{6}',1,4)+'-'+ substring('{6}',6,2) and  substring('{4}',1,4)+'-'+ substring('{4}',6,2) then 1 else 0 end as tdd3m	" +
                    "	,  case when CONVERT(VARCHAR,YEAR(APP_DATE))+'-'+RIGHT('00'+CONVERT(VARCHAR,MONTH(APP_DATE)),2) between substring('{5}',1,4)+'-'+ substring('{5}',6,2) and  substring('{4}',1,4)+'-'+ substring('{4}',6,2) then 1 else 0 end as tdd12m	" +
                    "	,  case when CONVERT(VARCHAR,YEAR(APP_DATE))+'-'+RIGHT('00'+CONVERT(VARCHAR,MONTH(APP_DATE)),2) between substring('{6}',1,4)+'-'+ substring('{6}',6,2) and  substring('{4}',1,4)+'-'+ substring('{4}',6,2) and Approval_Flag ='Y' then 1 else 0 end as tdd3ma	" +
                    "	,  case when CONVERT(VARCHAR,YEAR(APP_DATE))+'-'+RIGHT('00'+CONVERT(VARCHAR,MONTH(APP_DATE)),2) between substring('{5}',1,4)+'-'+ substring('{5}',6,2) and  substring('{4}',1,4)+'-'+ substring('{4}',6,2) and Approval_Flag ='Y' then 1 else 0 end as tdd12ma	" +
                    "	,  case when CONVERT(VARCHAR,YEAR(APP_DATE))+'-'+RIGHT('00'+CONVERT(VARCHAR,MONTH(APP_DATE)),2) between substring('{6}',1,4)+'-'+ substring('{6}',6,2) and  substring('{4}',1,4)+'-'+ substring('{4}',6,2) and Approval_Flag ='N' then 1 else 0 end as tdd3mw	" +
                    "	,  case when CONVERT(VARCHAR,YEAR(APP_DATE))+'-'+RIGHT('00'+CONVERT(VARCHAR,MONTH(APP_DATE)),2) between substring('{5}',1,4)+'-'+ substring('{5}',6,2) and  substring('{4}',1,4)+'-'+ substring('{4}',6,2) and Approval_Flag ='N' then 1 else 0 end as tdd12mw	" +
                                            "	into #Heading	" +
                                            "	from 	" +
                                                //Tai 2014-01-02
                                                //"	(	select APP_NO,MODEL,LOAN_CD,SCORE,CBS_STATUS,STYPE_CD,MARKET_CD,DW_SIGN_DATE,APP_DATE,cut_off as scr,rint	" +
                                            "	(	select distinct APP_NO,MODEL,LOAN_CD,SCORE,CBS_STATUS,STYPE_CD,MARKET_CD,DW_SIGN_DATE,APP_DATE,cut_off as scr,rint	" +
                                                //
                                            "	,CASE WHEN (score>=cut_off) then 'Y' else 'N' end as scapproval_flag	" +
                                            "	,case when CBS_STATUS in (3,6,7) then 'Y' when  CBS_STATUS in (4,5) then 'N' when  CBS_STATUS in (1,2) OR CBS_STATUS is null then '<>' end as Approval_Flag	" +
                                            "	,case when LEFT(CBS_REASON_CD,1)='A' AND  CBS_STATUS in (4,5) then 'Y' 	" +
                                            "	when LEFT(CBS_REASON_CD,1)='R' AND  CBS_STATUS in (3,6,7) then 'Y' when LEFT(CBS_REASON_CD,1)='A' AND  CBS_STATUS in (3,6,7) then 'N'	" +
                                            "	when LEFT(CBS_REASON_CD,1)='R' AND  CBS_STATUS in (4,5) then 'N' else 'N/A' end as CONTRAST_FLAG,	" +
                                            "	case when DW_SIGN_DATE IS NULL THEN 'N' ELSE 'Y' END AS BOOKING_FLAG	" +
                                            "	,CBS_REASON_CD, LEFT(CBS_REASON_CD,1) as CBS_REASON	" +
                                            "	, CONVERT(VARCHAR,YEAR(APP_DATE))+'-'+RIGHT('00'+CONVERT(VARCHAR,MONTH(APP_DATE)),2) AS MYAppDate	" +
                                            "	from	" +
                                            "	(	" +
                                            "	SELECT a.*,b.score,b.model	" +
                                            "	,(select cut_off from ST_SCORERANGE y where  y.ltype= '{0}' and y.lstype = '{1}' ) as cut_off	" +
                                            "	,(select rint from ST_SCORERANGE y where  y.ltype= '{0}' and y.lstype = '{1}' ) as rint	" +
                                            "	,CBS_REASON_CD	" +
                                            "	from LN_APP a,LN_GRADE b,CBS_LN_APP c 	" +
                                            "	where a.APP_NO = b.APP_NO  	" +
                                            "	and a.app_no = c.CBS_APP_NO 	" +
                                            "	and  LOAN_CD = '{0}' and STYPE_CD = '{1}' {2} {3} 	" +
                                            "	) lnapp	" +
                                            "	left join	" +
                                            "	(	" +
                                            "	select * 	" +
                                            "	from	" +
                                            "	(	" +
                                            "	select [CBS_APP_NO], DW_SIGN_DATE,LAST_UPDATE_DTM	" +
                                            "	from [DWH_BTFILE] A	" +
                                            " where SUBSTRING([LAST_UPDATE_DTM],7,4)+'-'+SUBSTRING([LAST_UPDATE_DTM],4,2)+'-'+SUBSTRING([LAST_UPDATE_DTM],1,2)=(SELECT MAX(SUBSTRING([LAST_UPDATE_DTM],7,4)+'-'+SUBSTRING([LAST_UPDATE_DTM],4,2)+'-'+SUBSTRING([LAST_UPDATE_DTM],1,2)) FROM [DWH_BTFILE] B WHERE A.CBS_APP_NO=B.CBS_APP_NO)		" +
                                            "	) DW_BFILE_T	" +
                                            "	) gsb2T	" +
                                            "	 on gsb2T.CBS_APP_NO = lnapp.APP_NO    	" +
                                            "	) gsbdat  	" +
                                            "	where  CONTRAST_FLAG='N' and Approval_Flag in ('Y','N') and BOOKING_FLAG in ('Y','N') 	" +
                                            "	select scrdif,sum(tdd3m) as tdd3m,sum(tdd12m) as tdd12m, sum(tdd3ma) as tdd3ma, sum(tdd12ma) as tdd12ma	" +
                                            "	,sum(tdd3mw) as tdd3mw, sum(tdd12mw) as tdd12mw	" +
                                            "	into #Main_Table	" +
                                            "	from #Heading	" +
                                            "	group by scrdif	" +
                                            "	order by scrdif	" +
                                            "	select case when (SCORE - scr) < 0 then convert(varchar,(scr + ((((SCORE + 1) - scr)/rint) -1 ) * rint)) + '-' + convert(varchar,((scr + ((((SCORE + 1 ) - scr)/rint)-1 ) * rint) + rint - 1))  	" +
                                            "	when (SCORE - scr) >= 0 then convert(varchar,(scr + ((SCORE - scr)/rint) * rint)) + '-' + convert(varchar,((scr + ((SCORE - scr)/rint) * rint) + rint - 1))   	" +
                                            "	end as scrdif,scApproval_flag,CONTRAST_FLAG	" +
                   "	,  case when CONVERT(VARCHAR,YEAR(APP_DATE))+'-'+RIGHT('00'+CONVERT(VARCHAR,MONTH(APP_DATE)),2) between substring('{7}',1,4)+'-'+ substring('{7}',6,2) and  substring('{8}',1,4)+'-'+ substring('{8}',6,2) then 1 else 0 end as tdd12mAgo	" +
                   "	,  case when (CONVERT(VARCHAR,YEAR(APP_DATE))+'-'+RIGHT('00'+CONVERT(VARCHAR,MONTH(APP_DATE)),2) between substring('{7}',1,4)+'-'+ substring('{7}',6,2) and  substring('{8}',1,4)+'-'+ substring('{8}',6,2)) and (Approval_Flag ='N') then 1 else 0 end as TDD12YGW	" +
                   "	,  case when (CONVERT(VARCHAR,YEAR(APP_DATE))+'-'+RIGHT('00'+CONVERT(VARCHAR,MONTH(APP_DATE)),2) between substring('{7}',1,4)+'-'+ substring('{7}',6,2) and  substring('{8}',1,4)+'-'+ substring('{8}',6,2)) and (Approval_Flag ='Y') then 1 else 0 end as TDD12YGA	" +
                   "	,  case when (CONVERT(VARCHAR,YEAR(APP_DATE))+'-'+RIGHT('00'+CONVERT(VARCHAR,MONTH(APP_DATE)),2) between substring('{7}',1,4)+'-'+ substring('{7}',6,2) and  substring('{8}',1,4)+'-'+ substring('{8}',6,2)) and (Approval_Flag ='Y' and BOOKING_FLAG='Y') then 1 else 0 end as tdd12mygb	" +
                   "	,  case when (CONVERT(VARCHAR,YEAR(APP_DATE))+'-'+RIGHT('00'+CONVERT(VARCHAR,MONTH(APP_DATE)),2) between substring('{6}',1,4)+'-'+ substring('{6}',6,2) and  substring('{4}',1,4)+'-'+ substring('{4}',6,2)) and (Booking_Flag='Y' and Approval_Flag ='Y') then 1 else 0 end as tdd3mb	" +
                   "	,  case when (CONVERT(VARCHAR,YEAR(APP_DATE))+'-'+RIGHT('00'+CONVERT(VARCHAR,MONTH(APP_DATE)),2) between substring('{5}',1,4)+'-'+ substring('{5}',6,2) and  substring('{4}',1,4)+'-'+ substring('{4}',6,2)) and (Booking_Flag='Y' and Approval_Flag ='Y') then 1 else 0 end as tdd12mb 	" +
                                            "	into #tdd12mAgo	" +
                                            "	from	" +
                                            "	(	" +
                                                //Tai 2014-01-02
                                                //"	select APP_NO,MODEL,LOAN_CD,SCORE,CBS_STATUS,STYPE_CD,MARKET_CD,DW_SIGN_DATE,APP_DATE,cut_off as scr,rint	" +
                                            "	select distinct APP_NO,MODEL,LOAN_CD,SCORE,CBS_STATUS,STYPE_CD,MARKET_CD,DW_SIGN_DATE,APP_DATE,cut_off as scr,rint	" +
                                                //
                                            "	,CASE WHEN (score>=cut_off) then 'Y' else 'N' end as scapproval_flag	" +
                                            "	,case when CBS_STATUS in (3,6,7) then 'Y' when  CBS_STATUS in (4,5) then 'N' when  CBS_STATUS in (1,2) OR CBS_STATUS is null then '<>' end as Approval_Flag	" +
                                            "	,case when LEFT(CBS_REASON_CD,1)='A' AND  CBS_STATUS in (4,5) then 'Y' 	" +
                                            "	when LEFT(CBS_REASON_CD,1)='R' AND  CBS_STATUS in (3,6,7) then 'Y' when LEFT(CBS_REASON_CD,1)='A' AND  CBS_STATUS in (3,6,7) then 'N'	" +
                                            "	when LEFT(CBS_REASON_CD,1)='R' AND  CBS_STATUS in (4,5) then 'N' else 'N/A' end as CONTRAST_FLAG,	" +
                                            "	case when DW_SIGN_DATE IS NULL THEN 'N' ELSE 'Y' END AS BOOKING_FLAG	" +
                                            "	,CBS_REASON_CD, LEFT(CBS_REASON_CD,1) as CBS_REASON	" +
                                            "	, CONVERT(VARCHAR,YEAR(APP_DATE))+'-'+RIGHT('00'+CONVERT(VARCHAR,MONTH(APP_DATE)),2) AS MYAppDate	" +
                                            "	from	" +
                                            "	(	" +
                                            "	SELECT a.*,b.score,b.model	" +
                                            "	,(select cut_off from ST_SCORERANGE y where  y.ltype= '{0}' and y.lstype = '{1}' ) as cut_off	" +
                                            "	,(select rint from ST_SCORERANGE y where  y.ltype= '{0}' and y.lstype = '{1}' ) as rint	" +
                                            "	      ,CBS_REASON_CD	" +
                                            "	from LN_APP a,LN_GRADE b,CBS_LN_APP c 	" +
                                            "	where a.APP_NO = b.APP_NO  	" +
                                            "	and a.app_no = c.CBS_APP_NO 	" +
                                            "	and  LOAN_CD = '{0}' and STYPE_CD = '{1}' {2} {3} 	" +
                                            "	) lnapp	" +
                                            "	left join	" +
                                            "	(	" +
                                            "	select * 	" +
                                            "	from	" +
                                            "	(	" +
                                            "	select [CBS_APP_NO], DW_SIGN_DATE,LAST_UPDATE_DTM	" +
                                            "	from [DWH_BTFILE] A	" +
                   " where SUBSTRING([LAST_UPDATE_DTM],7,4)+'-'+SUBSTRING([LAST_UPDATE_DTM],4,2)+'-'+SUBSTRING([LAST_UPDATE_DTM],1,2)=(SELECT MAX(SUBSTRING([LAST_UPDATE_DTM],7,4)+'-'+SUBSTRING([LAST_UPDATE_DTM],4,2)+'-'+SUBSTRING([LAST_UPDATE_DTM],1,2)) FROM [DWH_BTFILE] B WHERE A.CBS_APP_NO=B.CBS_APP_NO)	" +
                                            "	 ) DW_BFILE_T	" +
                                            "	 ) gsb2T	" +
                                            "	 on gsb2T.CBS_APP_NO = lnapp.APP_NO    	" +
                                            "	) Z	" +
                                            "	where  CONTRAST_FLAG='N' and Approval_Flag in ('Y','N') and BOOKING_FLAG in ('Y','N')	" +
                                            "	select scrdif,sum(tdd12mAgo) as tdd12mAgo,sum(TDD12YGW) as TDD12YGW,sum(tdd12mygb) as tdd12mygb	" +
                                            "	,sum(TDD12YGA) as TDD12YGA	" +
                                            "	,sum(tdd3mb) as tdd3mb,sum(tdd12mb) as tdd12mb	" +
                                            "	into #tdd12mAgo2	" +
                                            "	from #tdd12mAgo	" +
                                            "	group by scrdif	" +
                                            "	order by scrdif	" +
                                            "	select row_number() over(order by a.scrdif) as seq,a.scrdif as SCRRG,tdd3m,tdd12m,tdd12mAgo as tdd12yg	" +
                                            "	,tdd3mw,tdd12mw,TDD12YGW as tdd12ygw,tdd3ma,tdd12ma,TDD12YGA as tdd12yga	" +
                                            "	,tdd3mb,tdd12mb,tdd12mygb as tdd12ygb	" +
                                            "	into #Part1	" +
                                            "	from #Main_Table a, #tdd12mAgo2 b	" +
                                            "	where a.scrdif=b.scrdif	" +
                                            "	select 131 as seq, 'No. of Loans' as SCRRG,sum(tdd3m) as tdd3m,sum(tdd12m) as tdd12m	" +
                                            "	,sum(tdd12yg) as tdd12yg,sum(tdd3mw) as tdd3mw	" +
                                            "	,sum(tdd12mw) as tdd12mw,sum(tdd12ygw) as tdd12ygw,sum(tdd3ma) as tdd3ma	" +
                                            "	,sum(tdd12ma) as tdd12ma,sum(tdd12yga) as tdd12yga	" +
                                            "	,sum(tdd3mb) as tdd3mb,sum(tdd12mb) as tdd12mb,sum(tdd12ygb) as tdd12ygb	" +
                                            "	into #No_of_Loan	" +
                                            "	from #Part1	" +
                                            "	select 	" +
                                            "	case when (SCORE - scr) < 0 then convert(varchar	" +
                                            "	,(scr + ((((SCORE + 1) - scr)/rint) -1 ) * rint)) + '-' + convert(varchar,((scr + ((((SCORE + 1 ) - scr)/rint)-1 ) * rint) + rint - 1))  	" +
                                            "	when (SCORE - scr) >= 0 then convert(varchar,(scr + ((SCORE - scr)/rint) * rint)) + '-' 	" +
                                            "	+ convert(varchar,((scr + ((SCORE - scr)/rint) * rint) + rint - 1))   end as scrdif 	" +
                                            "	,scApproval_flag,CONTRAST_FLAG	" +
                   "	,  case when CONVERT(VARCHAR,YEAR(APP_DATE))+'-'+RIGHT('00'+CONVERT(VARCHAR,MONTH(APP_DATE)),2) between substring('{6}',1,4)+'-'+ substring('{6}',6,2) and  substring('{4}',1,4)+'-'+ substring('{4}',6,2) then score else 0 end as tdd3m_score	" +
                   "	,  case when CONVERT(VARCHAR,YEAR(APP_DATE))+'-'+RIGHT('00'+CONVERT(VARCHAR,MONTH(APP_DATE)),2) between substring('{5}',1,4)+'-'+ substring('{5}',6,2) and  substring('{4}',1,4)+'-'+ substring('{4}',6,2) then score else 0 end as tdd12m_score	" +
                   "	,  case when CONVERT(VARCHAR,YEAR(APP_DATE))+'-'+RIGHT('00'+CONVERT(VARCHAR,MONTH(APP_DATE)),2) between substring('{6}',1,4)+'-'+ substring('{6}',6,2) and  substring('{4}',1,4)+'-'+ substring('{4}',6,2) and Approval_Flag ='Y' then score else 0 end as tdd3ma_score	" +
                   "	,  case when CONVERT(VARCHAR,YEAR(APP_DATE))+'-'+RIGHT('00'+CONVERT(VARCHAR,MONTH(APP_DATE)),2) between substring('{5}',1,4)+'-'+ substring('{5}',6,2) and  substring('{4}',1,4)+'-'+ substring('{4}',6,2) and Approval_Flag ='Y' then score else 0 end as tdd12ma_score	" +
                   "	,  case when CONVERT(VARCHAR,YEAR(APP_DATE))+'-'+RIGHT('00'+CONVERT(VARCHAR,MONTH(APP_DATE)),2) between substring('{6}',1,4)+'-'+ substring('{6}',6,2) and  substring('{4}',1,4)+'-'+ substring('{4}',6,2) and Approval_Flag ='N' then score else 0 end as tdd3mw_score	" +
                   "	,  case when CONVERT(VARCHAR,YEAR(APP_DATE))+'-'+RIGHT('00'+CONVERT(VARCHAR,MONTH(APP_DATE)),2) between substring('{5}',1,4)+'-'+ substring('{5}',6,2) and  substring('{4}',1,4)+'-'+ substring('{4}',6,2) and Approval_Flag ='N' then score else 0 end as tdd12mw_score	" +
                                            "	into #Heading_Calculate_Score	" +
                                            "	from 	" +
                                            "	(	" +
                                                //Tai 2014-01-02
                                                //"	select APP_NO,MODEL,LOAN_CD,SCORE,CBS_STATUS,STYPE_CD,MARKET_CD,DW_SIGN_DATE,APP_DATE,cut_off as scr,rint	" +
                                            "	select distinct APP_NO,MODEL,LOAN_CD,SCORE,CBS_STATUS,STYPE_CD,MARKET_CD,DW_SIGN_DATE,APP_DATE,cut_off as scr,rint	" +
                                                //
                                            "	,CASE WHEN (score>=cut_off) then 'Y' else 'N' end as scapproval_flag	" +
                                            "	,case when CBS_STATUS in (3,6,7) then 'Y' when  CBS_STATUS in (4,5) then 'N' when  CBS_STATUS in (1,2) OR CBS_STATUS is null then '<>' end as Approval_Flag	" +
                                            "	,case when LEFT(CBS_REASON_CD,1)='A' AND  CBS_STATUS in (4,5) then 'Y' 	" +
                                            "	when LEFT(CBS_REASON_CD,1)='R' AND  CBS_STATUS in (3,6,7) then 'Y' when LEFT(CBS_REASON_CD,1)='A' AND  CBS_STATUS in (3,6,7) then 'N'	" +
                                            "	when LEFT(CBS_REASON_CD,1)='R' AND  CBS_STATUS in (4,5) then 'N' else 'N/A' end as CONTRAST_FLAG,	" +
                                            "	case when DW_SIGN_DATE IS NULL THEN 'N' ELSE 'Y' END AS BOOKING_FLAG	" +
                                            "	,CBS_REASON_CD, LEFT(CBS_REASON_CD,1) as CBS_REASON	" +
                                            "	, CONVERT(VARCHAR,YEAR(APP_DATE))+'-'+RIGHT('00'+CONVERT(VARCHAR,MONTH(APP_DATE)),2) AS MYAppDate	" +
                                            "	from	" +
                                            "	(	" +
                                            "	SELECT a.*,b.score,b.model	" +
                                            "	,(select cut_off from ST_SCORERANGE y where  y.ltype= '{0}' and y.lstype = '{1}' ) as cut_off	" +
                                            "	,(select rint from ST_SCORERANGE y where  y.ltype= '{0}' and y.lstype = '{1}' ) as rint	" +
                                            "	,CBS_REASON_CD	" +
                                            "	from LN_APP a,LN_GRADE b,CBS_LN_APP c 	" +
                                            "	where a.APP_NO = b.APP_NO  	" +
                                            "	and a.app_no = c.CBS_APP_NO 	" +
                                            "	and  LOAN_CD = '{0}' and STYPE_CD = '{1}' {2} {3} 	" +
                                            "	) lnapp	" +
                                            "	left join	" +
                                            "	(	" +
                                            "	select * 	" +
                                            "	from	" +
                                            "	(	" +
                                            "	select [CBS_APP_NO], DW_SIGN_DATE,LAST_UPDATE_DTM	" +
                                            "	from [DWH_BTFILE] A	" +
                                            " where SUBSTRING([LAST_UPDATE_DTM],7,4)+'-'+SUBSTRING([LAST_UPDATE_DTM],4,2)+'-'+SUBSTRING([LAST_UPDATE_DTM],1,2)=(SELECT MAX(SUBSTRING([LAST_UPDATE_DTM],7,4)+'-'+SUBSTRING([LAST_UPDATE_DTM],4,2)+'-'+SUBSTRING([LAST_UPDATE_DTM],1,2)) FROM [DWH_BTFILE] B WHERE A.CBS_APP_NO=B.CBS_APP_NO)		" +
                                            "	) DW_BFILE_T	" +
                                            "	) gsb2T	" +
                                            "	on gsb2T.CBS_APP_NO = lnapp.APP_NO    	" +
                                            "	) gsbdat  	" +
                                            "	where  CONTRAST_FLAG='N' and Approval_Flag in ('Y','N') and BOOKING_FLAG in ('Y','N') 	" +
                                            "	select case when (SCORE - scr) < 0 then convert(varchar,(scr + ((((SCORE + 1) - scr)/rint) -1 ) * rint)) + '-' + convert(varchar,((scr + ((((SCORE + 1 ) - scr)/rint)-1 ) * rint) + rint - 1))  	" +
                                            "	when (SCORE - scr) >= 0 then convert(varchar,(scr + ((SCORE - scr)/rint) * rint)) + '-' + convert(varchar,((scr + ((SCORE - scr)/rint) * rint) + rint - 1))   	" +
                                            "	end as scrdif,scApproval_flag,CONTRAST_FLAG	" +
                   "	,  case when CONVERT(VARCHAR,YEAR(APP_DATE))+'-'+RIGHT('00'+CONVERT(VARCHAR,MONTH(APP_DATE)),2) between substring('{7}',1,4)+'-'+ substring('{7}',6,2) and  substring('{8}',1,4)+'-'+ substring('{8}',6,2) then score else 0 end as tdd12mAgo_score	" +
                   "	,  case when (CONVERT(VARCHAR,YEAR(APP_DATE))+'-'+RIGHT('00'+CONVERT(VARCHAR,MONTH(APP_DATE)),2) between substring('{7}',1,4)+'-'+ substring('{7}',6,2) and  substring('{8}',1,4)+'-'+ substring('{8}',6,2)) and (Approval_Flag ='N') then score else 0 end as TDD12YGW_score	" +
                   "	,  case when (CONVERT(VARCHAR,YEAR(APP_DATE))+'-'+RIGHT('00'+CONVERT(VARCHAR,MONTH(APP_DATE)),2) between substring('{7}',1,4)+'-'+ substring('{7}',6,2) and  substring('{8}',1,4)+'-'+ substring('{8}',6,2)) and (Approval_Flag ='Y') then score else 0 end as TDD12YGA_score	" +
                   "	,  case when (CONVERT(VARCHAR,YEAR(APP_DATE))+'-'+RIGHT('00'+CONVERT(VARCHAR,MONTH(APP_DATE)),2) between substring('{7}',1,4)+'-'+ substring('{7}',6,2) and  substring('{8}',1,4)+'-'+ substring('{8}',6,2)) and (Approval_Flag ='Y' and BOOKING_FLAG='Y') then score else 0 end as tdd12mygb_score	" +
                   "	,  case when (CONVERT(VARCHAR,YEAR(APP_DATE))+'-'+RIGHT('00'+CONVERT(VARCHAR,MONTH(APP_DATE)),2) between substring('{6}',1,4)+'-'+ substring('{6}',6,2) and  substring('{4}',1,4)+'-'+ substring('{4}',6,2)) and (Booking_Flag='Y' and Approval_Flag ='Y') then score else 0 end as tdd3mb_score	" +
                   "	,  case when (CONVERT(VARCHAR,YEAR(APP_DATE))+'-'+RIGHT('00'+CONVERT(VARCHAR,MONTH(APP_DATE)),2) between substring('{5}',1,4)+'-'+ substring('{5}',6,2) and  substring('{4}',1,4)+'-'+ substring('{4}',6,2)) and (Booking_Flag='Y' and Approval_Flag ='Y') then score else 0 end as tdd12mb_score 	" +
                                            "	into #Score_Cal_P4_1	" +
                                            "	from	" +
                                            "	(	" +
                                                //Tai 2014-01-02
                                                //"	select APP_NO,MODEL,LOAN_CD,SCORE,CBS_STATUS,STYPE_CD,MARKET_CD,DW_SIGN_DATE,APP_DATE,cut_off as scr,rint	" +
                                            "	select distinct APP_NO,MODEL,LOAN_CD,SCORE,CBS_STATUS,STYPE_CD,MARKET_CD,DW_SIGN_DATE,APP_DATE,cut_off as scr,rint	" +
                                                //
                                            "	,CASE WHEN (score>=cut_off) then 'Y' else 'N' end as scapproval_flag	" +
                                            "	,case when CBS_STATUS in (3,6,7) then 'Y' when  CBS_STATUS in (4,5) then 'N' when  CBS_STATUS in (1,2) OR CBS_STATUS is null then '<>' end as Approval_Flag	" +
                                            "	,case when LEFT(CBS_REASON_CD,1)='A' AND  CBS_STATUS in (4,5) then 'Y' 	" +
                                            "	when LEFT(CBS_REASON_CD,1)='R' AND  CBS_STATUS in (3,6,7) then 'Y' when LEFT(CBS_REASON_CD,1)='A' AND  CBS_STATUS in (3,6,7) then 'N'	" +
                                            "	when LEFT(CBS_REASON_CD,1)='R' AND  CBS_STATUS in (4,5) then 'N' else 'N/A' end as CONTRAST_FLAG,	" +
                                            "	case when DW_SIGN_DATE IS NULL THEN 'N' ELSE 'Y' END AS BOOKING_FLAG	" +
                                            "	,CBS_REASON_CD, LEFT(CBS_REASON_CD,1) as CBS_REASON	" +
                                            "	, CONVERT(VARCHAR,YEAR(APP_DATE))+'-'+RIGHT('00'+CONVERT(VARCHAR,MONTH(APP_DATE)),2) AS MYAppDate	" +
                                            "	from	" +
                                            "	(	" +
                                            "	SELECT a.*,b.score,b.model	" +
                                            "	,(select cut_off from ST_SCORERANGE y where  y.ltype= '{0}' and y.lstype = '{1}' ) as cut_off	" +
                                            "	,(select rint from ST_SCORERANGE y where  y.ltype= '{0}' and y.lstype = '{1}' ) as rint	" +
                                            "	      ,CBS_REASON_CD	" +
                                            "	from LN_APP a,LN_GRADE b,CBS_LN_APP c 	" +
                                            "	where a.APP_NO = b.APP_NO  	" +
                                            "	and a.app_no = c.CBS_APP_NO 	" +
                                            "	and  LOAN_CD = '{0}' and STYPE_CD = '{1}' {2} {3} 	" +
                                            "	) lnapp	" +
                                            "	left join	" +
                                            "	(	" +
                                            "	select * 	" +
                                            "	from	" +
                                            "	(	" +
                                            "	select [CBS_APP_NO], DW_SIGN_DATE,LAST_UPDATE_DTM	" +
                                            "	from [DWH_BTFILE] A	" +
                   " where SUBSTRING([LAST_UPDATE_DTM],7,4)+'-'+SUBSTRING([LAST_UPDATE_DTM],4,2)+'-'+SUBSTRING([LAST_UPDATE_DTM],1,2)=(SELECT MAX(SUBSTRING([LAST_UPDATE_DTM],7,4)+'-'+SUBSTRING([LAST_UPDATE_DTM],4,2)+'-'+SUBSTRING([LAST_UPDATE_DTM],1,2)) FROM [DWH_BTFILE] B WHERE A.CBS_APP_NO=B.CBS_APP_NO)		" +
                                            "	 ) DW_BFILE_T	" +
                                            "	 ) gsb2T	" +
                                            "	 on gsb2T.CBS_APP_NO = lnapp.APP_NO    	" +
                                            "	) Z	" +
                                            "	where  CONTRAST_FLAG='N' and Approval_Flag in ('Y','N') and BOOKING_FLAG in ('Y','N')	" +
                                            "	select scrdif,sum(tdd12mAgo_score) as tdd12mAgo_score	" +
                                            "	,sum(TDD12YGW_score) as TDD12YGW_score,sum(tdd12mygb_score) as tdd12mygb_score	" +
                                            "	,sum(TDD12YGA_score) as TDD12YGA_score	" +
                                            "	,sum(tdd3mb_score) as tdd3mb_score,sum(tdd12mb_score) as tdd12mb_score	" +
                                            "	into #Score_Cal_P4_2	" +
                                            "	from #Score_Cal_P4_1	" +
                                            "	group by scrdif	" +
                                            "	order by scrdif	" +
                                            "	select 129 as seq, 'Total_score' as SCRRG,sum(tdd12mAgo_score) as tdd12mAgo	" +
                                            "	,sum(TDD12YGW_score) as TDD12YGW	" +
                                            "	,sum(TDD12YGA_score) as TDD12YGA	" +
                                            "	,sum(tdd3mb_score) as tdd3mb	" +
                                            "	,sum(tdd12mb_score) as tdd12mb	" +
                                            "	,sum(tdd12mygb_score) as tdd12mygb	" +
                                            "	into #Total_score_P4	" +
                                            "	from #Score_Cal_P4_2	" +
                                            "	select scrdif,sum(tdd3m_score) as tdd3m_score,sum(tdd12m_score) as tdd12m_score,sum(tdd3ma_score) as tdd3ma_score	" +
                                            "	,sum(tdd12ma_score) as tdd12ma_score, sum(tdd3mw_score) as tdd3mw_score,sum(tdd12mw_score) as tdd12mw_score	" +
                                            "	into #Heading_Calculate_Score1	" +
                                            "	from #Heading_Calculate_Score	" +
                                            "	group by scrdif	" +
                                            "	order by scrdif	" +
                                            "	select 129 as seq, 'Total_score' as SCRRG,sum(tdd3m_score) as tdd3m,sum(tdd12m_score) as tdd12m,sum(tdd3ma_score) as tdd3ma	" +
                                            "	,sum(tdd12ma_score) as tdd12ma,sum(tdd3mw_score) as tdd3mw	" +
                                            "	,sum(tdd12mw_score) as tdd12mw	" +
                                            "	into #Heading_Calculate_Score2	" +
                                            "	from #Heading_Calculate_Score1	" +
                                            "	select a.seq,a.SCRRG,tdd3m,tdd12m,tdd12mAgo as tdd12yg,tdd3ma,tdd12ma,TDD12YGA as tdd12yga,tdd3mw,tdd12mw,TDD12YGW as tdd12ygw,tdd3mb,tdd12mb	" +
                                            "	,tdd12mygb as tdd12ygb	" +
                                            "	into #Final_Total_score	" +
                                            "	from #Heading_Calculate_Score2 a, #Total_score_P4 b	" +
                                            "	where a.seq=b.seq	" +
                                            "	select 132 as seq, 'Avg. Score' as SCRRG,convert(decimal(15,3),b.tdd3m)/case when a.tdd3m=0 then 1 else a.tdd3m end as tdd3m	" +
                                            "	,convert(decimal(15,3),b.tdd12m)/case when a.tdd12m=0 then 1 else a.tdd12m end as tdd12m	" +
                                            "	,convert(decimal(15,3),b.tdd12yg)/case when a.tdd12yg=0 then 1 else a.tdd12yg end as tdd12yg	" +
                                            "	,convert(decimal(15,3),b.tdd3mw)/case when a.tdd3mw=0 then 1 else a.tdd3mw end as tdd3mw	" +
                                            "	,convert(decimal(15,3),b.tdd12mw)/case when a.tdd12mw=0 then 1 else  a.tdd12mw end as tdd12mw	" +
                                            "	,convert(decimal(15,3),b.tdd12ygw)/case when a.tdd12ygw=0 then 1 else  a.tdd12ygw end as tdd12ygw	" +
                                            "	,convert(decimal(15,3),b.tdd3ma)/case when a.tdd3ma =0 then 1 else a.tdd3ma end as tdd3ma	" +
                                            "	,convert(decimal(15,3),b.tdd12ma)/case when a.tdd12ma=0 then 1 else a.tdd12ma end as tdd12ma	" +
                                            "	,convert(decimal(15,3),b.tdd12yga)/case when a.tdd12yga=0 then 1 else a.tdd12yga end as tdd12yga	" +
                                            "	,convert(decimal(15,3),b.tdd3mb)/case when a.tdd3mb=0 then 1 else a.tdd3mb end as tdd3mb	" +
                                            "	,convert(decimal(15,3),b.tdd12mb)/case when a.tdd12mb=0  then 1 else a.tdd12mb end as tdd12mb	" +
                                            "	,convert(decimal(15,3),b.tdd12ygb)/case when a.tdd12ygb=0 then 1 else a.tdd12ygb end as tdd12ygb	" +
                                            "	into #Cal_Avg_score	" +
                                            "	from #No_of_Loan a,#Final_Total_score b	" +
                                            "	select sum(tdd3m) as tdd3m,sum(tdd12m) as tdd12m, sum(tdd3ma) as tdd3ma, sum(tdd12ma) as tdd12ma	" +
                                            "	, sum(tdd3mw) as tdd3mw, sum(tdd12mw) as tdd12mw	" +
                                            "	into #Main_cutoff1	" +
                                            "	from #Heading	" +
                                            "	where scApproval_flag='N'	" +
                                            "	select sum(tdd12mAgo) as tdd12yg,sum(TDD12YGW) as TDD12YGW,sum(tdd12mygb) as tdd12ygb	" +
                                            "	,sum(TDD12YGA) as TDD12YGA	" +
                                            "	,sum(tdd3mb) as tdd3mb,sum(tdd12mb) as tdd12mb	" +
                                            "	into #Main_cutoff2	" +
                                            "	from #tdd12mAgo	" +
                                            "	where scApproval_flag='N'	" +
                                            "	select  tdd3m,tdd12m,tdd12yg,tdd3mw,tdd12mw,tdd12ygw,tdd3ma,tdd12ma,tdd12yga,tdd3mb,tdd12mb,tdd12ygb	" +
                                            "	into #Main_cutoff3	" +
                                            "	from #Main_cutoff1 a,#Main_cutoff2 b	" +
                                            "	select 128 as seq,'% Below Cutoff' as SCRRG,convert(decimal(15,3),a.tdd3m)*100.00/case when b.tdd3m=0 then 1 else b.tdd3m end as tdd3m	" +
                                            "	,convert(decimal(15,3),a.tdd12m)*100.00/case when b.tdd12m=0 then 1 else b.tdd12m end as tdd12m	" +
                                            "	,convert(decimal(15,3),a.tdd12yg)*100.00/case when b.tdd12yg=0 then 1 else b.tdd12yg end as tdd12yg	" +
                                            "	,convert(decimal(15,3),a.tdd3mw)*100.00/case when b.tdd3mw=0 then 1 else b.tdd3mw end as tdd3mw		" +
                                            "	,convert(decimal(15,3),a.tdd12mw)*100.00/case when b.tdd12mw=0 then 1 else b.tdd12mw end as tdd12mw	" +
                                            "	,convert(decimal(15,3),a.tdd12ygw)*100.00/case when b.tdd12ygw=0 then 1 else  b.tdd12ygw end as tdd12ygw	" +
                                            "	,convert(decimal(15,3),a.tdd3ma)*100.00/case when b.tdd3ma =0 then 1 else b.tdd3ma end as tdd3ma	" +
                                            "	,convert(decimal(15,3),a.tdd12ma)*100.00/case when b.tdd12ma=0 then 1 else b.tdd12ma end as tdd12ma	" +
                                            "	,convert(decimal(15,3),a.tdd12yga)*100.00/case when b.tdd12yga=0 then 1 else b.tdd12yga end as tdd12yga	" +
                                            "	,convert(decimal(15,3),a.tdd3mb)*100.00/case when b.tdd3mb=0 then 1 else b.tdd3mb end as tdd3mb	" +
                                            "	,convert(decimal(15,3),a.tdd12mb)*100.00/case when b.tdd12mb=0  then 1 else b.tdd12mb end as tdd12mb	" +
                                            "	,convert(decimal(15,3),a.tdd12ygb)*100.00/case when b.tdd12ygb=0 then 1 else b.tdd12ygb end as tdd12ygb	" +
                                            "	into #Main_cutoff_table	" +
                                            "	from #Main_cutoff3 a, #No_of_Loan b	" +
                                            "	select sum(tdd3m) as tdd3m,sum(tdd12m) as tdd12m, sum(tdd3ma) as tdd3ma, sum(tdd12ma) as tdd12ma	" +
                                            "	, sum(tdd3mw) as tdd3mw, sum(tdd12mw) as tdd12mw	" +
                                            "	into #Main_cutoff_Passing1	" +
                                            "	from #Heading	" +
                                            "	where scApproval_flag='Y'	" +
                                            "	select sum(tdd12mAgo) as tdd12yg,sum(TDD12YGW) as TDD12YGW,sum(tdd12mygb) as tdd12ygb	" +
                                            "	,sum(TDD12YGA) as TDD12YGA	" +
                                            "	,sum(tdd3mb) as tdd3mb,sum(tdd12mb) as tdd12mb	" +
                                            "	into #Main_cutoff_Passing2	" +
                                            "	from #tdd12mAgo	" +
                                            "	where scApproval_flag='Y'	" +
                                            "	select  tdd3m,tdd12m,tdd12yg,tdd3mw,tdd12mw,tdd12ygw,tdd3ma,tdd12ma,tdd12yga,tdd3mb,tdd12mb,tdd12ygb	" +
                                            "	into #Main_cutoff_Passing3	" +
                                            "	from #Main_cutoff_Passing1 a,#Main_cutoff_Passing2 b	" +
                                            "	select 130 as seq,'% Passing Cutoff' as SCRRG,convert(decimal(15,3),a.tdd3m)*100.00/case when b.tdd3m=0 then 1 else b.tdd3m end as tdd3m	" +
                                            "	,convert(decimal(15,3),a.tdd12m)*100.00/case when b.tdd12m=0 then 1 else b.tdd12m end as tdd12m	" +
                                            "	,convert(decimal(15,3),a.tdd12yg)*100.00/case when b.tdd12yg=0 then 1 else b.tdd12yg end as tdd12yg	" +
                                            "	,convert(decimal(15,3),a.tdd3mw)*100.00/case when b.tdd3mw=0 then 1 else b.tdd3mw end as tdd3mw	" +
                                            "	,convert(decimal(15,3),a.tdd12mw)*100.00/case when b.tdd12mw=0 then 1 else b.tdd12mw end as tdd12mw	" +
                                            "	,convert(decimal(15,3),a.tdd12ygw)*100.00/case when b.tdd12ygw=0 then 1 else b.tdd12ygw end as tdd12ygw	" +
                                            "	,convert(decimal(15,3),a.tdd3ma)*100.00/case when b.tdd3ma=0 then 1 else b.tdd3ma end as tdd3ma	" +
                                            "	,convert(decimal(15,3),a.tdd12ma)*100.00/case when b.tdd12ma=0 then 1 else b.tdd12ma end as tdd12ma	" +
                                            "	,convert(decimal(15,3),a.tdd12yga)*100.00 /case when b.tdd12yga=0 then 1 else b.tdd12yga end as tdd12yga	" +
                                            "	,convert(decimal(15,3),a.tdd3mb)*100.00/case when b.tdd3mb=0 then 1 else b.tdd3mb end as tdd3mb	" +
                                            "	,convert(decimal(15,3),a.tdd12mb)*100.00/case when b.tdd12mb=0  then 1 else b.tdd12mb end as tdd12mb	" +
                                            "	,convert(decimal(15,3),a.tdd12ygb)*100.00/case when b.tdd12ygb=0 then 1 else b.tdd12ygb end as tdd12ygb	" +
                                            "	into #Main_cutoff_Passing_Table	" +
                                            "	from #Main_cutoff_Passing3 a, #No_of_Loan b	" +
                   " select AA.seq,AA.SCRRG,tdd3m,tdd12m,tdd12yg,tdd3mw,tdd12mw,tdd12ygw,tdd3ma,tdd12ma,tdd12yga,tdd3mb,tdd12mb,tdd12ygb into  #Part1_Range	" +
                   " from (select row_number() over(order by range_name) as seq,range_name as SCRRG from st_scorernglst 	" +
                   " where range_cd = (select range_cd from ST_SCORERANGE where ltype= '{0}' and lstype = '{1}')  ) AA left join (select * from #Part1 ) BB on AA.SCRRG=BB.SCRRG 	" +
                   " select AA.seq,AA.SCRRG,tdd3m,tdd12m,tdd12yg,tdd3mw,tdd12mw,tdd12ygw,tdd3ma,tdd12ma,tdd12yga,tdd3mb,tdd12mb,tdd12ygb	" +
                   " into #Solution  from (select seq,SCRRG,(select sum(tdd3m) from #Part1 t2 where t2.SCRRG <= t1.SCRRG) as tdd3m,(select sum(tdd12m)	" +
                   " from #Part1 t2  where t2.SCRRG <= t1.SCRRG) as tdd12m	,(select sum(tdd12yg) from #Part1 t2 where t2.SCRRG <= t1.SCRRG) as tdd12yg,tdd3mw as [tdd3mw]	" +
                   " ,tdd12mw as [tdd12mw],tdd12ygw as [tdd12ygw]	 ,tdd3ma as [tdd3ma],tdd12ma as [tdd12ma],tdd12yga as [tdd12yga]	" +
                   " ,tdd3mb as [tdd3mb],tdd12mb as [tdd12mb],tdd12ygb as [tdd12ygb]  from #Part1_Range t1   union   select * from #No_of_Loan  ) AA	" +
                       " select * into #PartA from (	" +
                                           " select seq,SCRRG,[dbo].[comma_format](convert(varchar,convert(integer,tdd3m))) as tdd3m,[dbo].[comma_format](convert(varchar,convert(integer,tdd12m))) as tdd12m	" +
                   " ,[dbo].[comma_format](convert(varchar,convert(integer,tdd12yg))) as tdd12yg,[dbo].[comma_format](convert(varchar,convert(integer,tdd3mw))) as tdd3mw	" +
                   " ,[dbo].[comma_format](convert(varchar,convert(integer,tdd12mw))) as tdd12mw,[dbo].[comma_format](convert(varchar,convert(integer,tdd12ygw))) as tdd12ygw	" +
                   " ,[dbo].[comma_format](convert(varchar,convert(integer,tdd3ma))) as tdd3ma,[dbo].[comma_format](convert(varchar,convert(integer,tdd12ma))) as tdd12ma " +
                   " ,[dbo].[comma_format](convert(varchar,convert(integer,tdd12yga))) as tdd12yga,[dbo].[comma_format](convert(varchar,convert(integer,tdd3mb)))	as tdd3mb" +
                   " ,[dbo].[comma_format](convert(varchar,convert(integer,tdd12mb))) as tdd12mb,[dbo].[comma_format](convert(varchar,convert(integer,tdd12ygb)))	as tdd12ygb " +
                   " from #Solution	" +
                                            "	union	" +
                   " select seq,SCRRG,[dbo].[comma_percent_format](convert(varchar,convert(decimal(15,2),tdd3m))) as tdd3m,[dbo].[comma_percent_format](convert(varchar,convert(decimal(15,2),tdd12m))) as tdd12m " +
                   " ,[dbo].[comma_percent_format](convert(varchar,convert(decimal(15,2),tdd12yg))) as tdd12yg,[dbo].[comma_percent_format](convert(varchar,convert(decimal(15,2),tdd3mw))) as tdd3mw " +
                   " ,[dbo].[comma_percent_format](convert(varchar,convert(decimal(15,2),tdd12mw))) as tdd12mw,[dbo].[comma_percent_format](convert(varchar,convert(decimal(15,2),tdd12ygw))) as tdd12ygw " +
                   " ,[dbo].[comma_percent_format](convert(varchar,convert(decimal(15,2),tdd3ma))) as tdd3ma,[dbo].[comma_percent_format](convert(varchar,convert(decimal(15,2),tdd12ma))) as tdd12ma " +
                   " ,[dbo].[comma_percent_format](convert(varchar,convert(decimal(15,2),tdd12yga))) as tdd12yga,[dbo].[comma_percent_format](convert(varchar,convert(decimal(15,2),tdd3mb))) as tdd3mb,[dbo].[comma_percent_format](convert(varchar,convert(decimal(15,2),tdd12mb))) as tdd12mb,[dbo].[comma_percent_format](convert(varchar,convert(decimal(15,2),tdd12ygb))) as tdd12ygb " +
                                            "  from #Cal_Avg_score	" +
                                                "	union	" +
                                                " select seq,SCRRG,convert(varchar,convert(decimal(15,2),tdd3m)) as tdd3m,convert(varchar,convert(decimal(15,2),tdd12m)) as tdd12m " +
                   " ,convert(varchar,convert(decimal(15,2),tdd12yg)) as tdd12yg,convert(varchar,convert(decimal(15,2),tdd3mw)) as tdd3mw " +
                   " ,convert(varchar,convert(decimal(15,2),tdd12mw)) as tdd12mw,convert(varchar,convert(decimal(15,2),tdd12ygw)) as tdd12ygw " +
                   " ,convert(varchar,convert(decimal(15,2),tdd3ma)) as tdd3ma,convert(varchar,convert(decimal(15,2),tdd12ma)) as tdd12ma " +
                   " ,convert(varchar,convert(decimal(15,2),tdd12yga)) as tdd12yga,convert(varchar,convert(decimal(15,2),tdd3mb)) as tdd3mb,convert(varchar,convert(decimal(15,2),tdd12mb)) as tdd12mb,convert(varchar,convert(decimal(15,2),tdd12ygb)) as tdd12ygb " +
                   "   from #Main_cutoff_table	" +
                                                "	union	" +
                   " select seq,SCRRG,convert(varchar,convert(decimal(15,2),tdd3m)) as tdd3m,convert(varchar,convert(decimal(15,2),tdd12m)) as tdd12m " +
                   " ,convert(varchar,convert(decimal(15,2),tdd12yg)) as tdd12yg,convert(varchar,convert(decimal(15,2),tdd3mw)) as tdd3mw " +
                   " ,convert(varchar,convert(decimal(15,2),tdd12mw)) as tdd12mw,convert(varchar,convert(decimal(15,2),tdd12ygw)) as tdd12ygw " +
                   " ,convert(varchar,convert(decimal(15,2),tdd3ma)) as tdd3ma,convert(varchar,convert(decimal(15,2),tdd12ma)) as tdd12ma " +
                   " ,convert(varchar,convert(decimal(15,2),tdd12yga)) as tdd12yga,convert(varchar,convert(decimal(15,2),tdd3mb)) as tdd3mb,convert(varchar,convert(decimal(15,2),tdd12mb)) as tdd12mb,convert(varchar,convert(decimal(15,2),tdd12ygb)) as tdd12ygb " +
                   "   from #Main_cutoff_Passing_Table ) Z  " +
                   "select * into #Format_NULL_CASE from ( select 0 as seq,'Score Range' as SCRRG,'3 Months' as tdd3m,'12 Months' as tdd12m,'12 Months' as tdd12yg,'3 Months' as tdd3mw " +
                   " ,'12 Months' as tdd12mw,'12 Months' as tdd12ygw,'3 Months' as tdd3ma,'12 Months' as tdd12ma,'12 Months' as tdd12yga,'3 Months' as tdd3mb " +
                   ",'12 Months' as tdd12mb,'12 Months' as tdd12ygb	union " +
                   " select seq,SCRRG ,case when tdd3m is null then '0' else tdd3m end as tdd3m	 " +
                   " ,case when tdd12m is null then '0' else tdd12m end as tdd12m	 " +
                   " ,case when tdd12yg is null then '0' else tdd12yg end as tdd12yg	 " +
                   " ,case when tdd3mw is null then '0' else tdd3mw end as tdd3mw	 " +
                   " ,case when tdd12mw is null then '0' else tdd12mw end as tdd12mw	 " +
                   " ,case when tdd12ygw is null then '0' else tdd12ygw end as tdd12ygw	 " +
                   " ,case when tdd3ma is null then '0' else tdd3ma end as tdd3ma	 " +
                   " ,case when tdd12ma is null then '0' else tdd12ma end as tdd12ma	 " +
                   " ,case when tdd12yga is null then '0' else tdd12yga end as tdd12yga	 " +
                   " ,case when tdd3mb is null then '0' else tdd3mb end as tdd3mb	 " +
                   " ,case when tdd12mb is null then '0' else tdd12mb end as tdd12mb	 " +
                   " ,case when tdd12ygb is null then '0' else tdd12ygb end as tdd12ygb from #PartA where (seq < 100 or seq=131)  union	 " +
                   " select seq,SCRRG ,case when tdd3m is null then '0.00' else tdd3m end as tdd3m	 " +
                   " ,case when tdd12m is null then '0.00' else tdd12m end as tdd12m	 " +
                   " ,case when tdd12yg is null then '0.00' else tdd12yg end as tdd12yg	 " +
                   " ,case when tdd3mw is null then '0.00' else tdd3mw end as tdd3mw	 " +
                   " ,case when tdd12mw is null then '0.00' else tdd12mw end as tdd12mw	 " +
                   " ,case when tdd12ygw is null then '0.00' else tdd12ygw end as tdd12ygw	 " +
                   " ,case when tdd3ma is null then '0.00' else tdd3ma end as tdd3ma	 " +
                   " ,case when tdd12ma is null then '0.00' else tdd12ma end as tdd12ma	 " +
                   " ,case when tdd12yga is null then '0.00' else tdd12yga end as tdd12yga	 " +
                   " ,case when tdd3mb is null then '0.00' else tdd3mb end as tdd3mb	 " +
                   " ,case when tdd12mb is null then '0.00' else tdd12mb end as tdd12mb	 " +
                   " ,case when tdd12ygb is null then '0.00' else tdd12ygb end as tdd12ygb	from #PartA  where seq=128 or seq=130 or seq=132 ) y order by seq	 " +
                   " select identity(int,1,1) as Seq, SCRRG,tdd3m,tdd12m,tdd12yg,tdd3mw,tdd12mw,tdd12ygw,tdd3ma,tdd12ma,tdd12yga,tdd3mb,tdd12mb,tdd12ygb " +
                    " into #z_Approval_Rate_Score " +
                    " from #Format_NULL_CASE order by seq " +
                    " select GRADE as scrrg" +
                    " , case when seq not in (1,6,7) then dbo.comma_format(tdd3m) else tdd3m end as tdd3m " +
                    " , case when seq not in (1,6,7) then dbo.comma_format(tdd12m) else tdd12m end as tdd12m " +
                    " , case when seq not in (1,6,7) then dbo.comma_format(tdd12yg) else tdd12yg end as tdd12yg " +
                    " , case when seq not in (1,6,7) then dbo.comma_format(tdd3mw) else tdd3mw end as tdd3mw " +
                    " , case when seq not in (1,6,7) then dbo.comma_format(tdd12mw) else tdd12mw end as tdd12mw " +
                    " , case when seq not in (1,6,7) then dbo.comma_format(tdd12ygw) else tdd12ygw end as tdd12ygw " +
                    " , case when seq not in (1,6,7) then dbo.comma_format(tdd3ma) else tdd3ma end as tdd3ma " +
                    " , case when seq not in (1,6,7) then dbo.comma_format(tdd12ma) else tdd12ma end as tdd12ma " +
                    " , case when seq not in (1,6,7) then dbo.comma_format(tdd12yga) else tdd12yga end as tdd12yga " +
                    " , case when seq not in (1,6,7) then dbo.comma_format(tdd3mb) else tdd3mb end as tdd3mb " +
                    " , case when seq not in (1,6,7) then dbo.comma_format(tdd12mb) else tdd12mb end as tdd12mb " +
                    " , case when seq not in (1,6,7) then dbo.comma_format(tdd12ygb) else tdd12ygb end as tdd12ygb " +
                    " into #Result " +
                    " from " +
                    " ( " +
                    " select '5' as seq, a.GRADE , convert(varchar,sum(a.tdd3m)) as tdd3m, convert(varchar,sum(a.tdd12m)) as tdd12m, convert(varchar,sum(a.tdd12yg)) as tdd12yg " +
                    " , convert(varchar,sum(a.tdd3mw)) as tdd3mw, convert(varchar,sum(a.tdd12mw)) as tdd12mw, convert(varchar,sum(a.tdd12ygw)) as tdd12ygw " +
                    " , convert(varchar,sum(a.tdd3ma)) as tdd3ma " +
                    " , convert(varchar,sum(a.tdd12ma)) as tdd12ma, convert(varchar,sum(a.tdd12yga)) as tdd12yga " +
                    " , convert(varchar,sum(a.tdd3mb)) as tdd3mb, convert(varchar,sum(a.tdd12mb)) as tdd12mb, convert(varchar,sum(a.tdd12ygb)) as tdd12ygb " +
                    " from " +
                    " ( " +
                    " select a.seq, a.scrrg  " +
                    " , convert(int,a.tdd3m) as tdd3m, convert(int,a.tdd12m) as tdd12m, convert(int,a.tdd12yg) as tdd12yg " +
                    " , convert(int,a.tdd3mw) as tdd3mw, convert(int,a.tdd12mw) as tdd12mw, convert(int,a.tdd12ygw) as tdd12ygw, convert(int,a.tdd3ma) as tdd3ma " +
                    " , convert(int,a.tdd12ma) as tdd12ma, convert(int,a.tdd12yga) as tdd12yga, convert(int,a.tdd3mb) as tdd3mb, convert(int,a.tdd12mb) as tdd12mb " +
                    " , convert(int,a.tdd12ygb) as tdd12ygb " +
                    " , a.MinScr, a.MaxScr " +
                    " , b.GRADE, b.MIN_SCORE, b.MAX_SCORE " +
                    " from " +
                    " ( " +
                    " select a.seq, a.scrrg " +
                    " , convert(varchar,convert(int,replace(a.tdd3m,',',''))-convert(int,replace(b.tdd3m,',',''))) as tdd3m " +
                    " , convert(varchar,convert(int,replace(a.tdd12m,',',''))-convert(int,replace(b.tdd12m,',',''))) as tdd12m " +
                    " , convert(varchar,convert(int,replace(a.tdd12yg,',',''))-convert(int,replace(b.tdd12yg,',',''))) as tdd12yg " +
                    " , convert(varchar,convert(int,replace(a.tdd3mw,',',''))) as tdd3mw " +
                    " , convert(varchar,convert(int, replace(a.tdd12mw,',',''))) as tdd12mw " +
                    " , convert(varchar,convert(int,replace(a.tdd12ygw,',',''))) as tdd12ygw " +
                    " , convert(varchar,convert(int,replace(a.tdd3ma,',',''))) as tdd3ma " +
                    " , convert(varchar,convert(int,replace(a.tdd12ma,',',''))) as tdd12ma " +
                    " , convert(varchar,convert(int,replace(a.tdd12yga,',',''))) as tdd12yga " +
                    " , convert(varchar,convert(int,replace(a.tdd3mb,',',''))) as tdd3mb " +
                    " , convert(varchar,convert(int,replace(a.tdd12mb,',',''))) as tdd12mb " +
                    " , convert(varchar,convert(int,replace(a.tdd12ygb,',',''))) as tdd12ygb " +
                    " , case when isnull(SUBSTRING(a.scrrg,1,3),'')='' then '999' else SUBSTRING(a.scrrg,1,3) end as MinScr " +
                    " , case when isnull(SUBSTRING(a.scrrg,5,3),'')='' then '999' else SUBSTRING(a.scrrg,5,3) end as MaxScr " +
                    " from #z_Approval_Rate_Score a " +
                    " inner join #z_Approval_Rate_Score b on a.seq=b.seq+1  " +
                    " and isnumeric(SUBSTRING(a.scrrg,1,1))=1 " +
                    " and isnumeric(SUBSTRING(b.scrrg,1,1))=1 " +
                    " ) a " +
                    " left join " +
                    " ( " +
                    " select * from ST_CUTOFF a " +
                    " WHERE CUTOFF_CD in " +
                    " (select CUTOFF_CD from LN_CUTOFF " +
                    " WHERE LOAN_CD = '{0}' and STYPE_CD = '{1}') " +
                    " ) b " +
                    " on a.MinScr>=convert(varchar,b.MIN_SCORE) " +
                    " and a.MaxScr<=case when b.max_score='1000' then '999' else convert(varchar,b.MAX_SCORE) end " +
                    " ) a " +
                    " group by a.GRADE " +
                    " union " +
                    " select case when scrrg='Score Range' then '1' when scrrg='Avg. Score' then '7' else '6' end as seq " +
                    " ,scrrg, tdd3m, tdd12m, tdd12yg, tdd3mw, tdd12mw, tdd12ygw, tdd3ma, tdd12ma, tdd12yga, tdd3mb, tdd12mb, tdd12ygb " +
                    " from #z_Approval_Rate_Score a " +
                    " where ISNUMERIC(substring(scrrg,1,1))=0  " +
                    " ) a " +
                    " order by seq,GRADE " +
                    " UPDATE #Result " +
                    " SET tdd3m=a.tdd3m_Cum " +
                    " , tdd12m=a.tdd12m_Cum " +
                    " , tdd12yg=a.tdd12yg_Cum " +
                    " from " +
                    " ( " +
                    " select scrrg, tdd3m, tdd12m, tdd12yg  " +
                    " , case when scrrg ='A' then (select dbo.comma_format(convert(varchar,SUM(convert(int,replace(tdd3m,',',''))))) from #Result where scrrg in ('A','B','C','D')) " +
                    " when scrrg ='B' then (select dbo.comma_format(convert(varchar,SUM(convert(int,replace(tdd3m,',',''))))) from #Result where scrrg in ('B','C','D')) " +
                    " when scrrg ='C' then (select dbo.comma_format(convert(varchar,SUM(convert(int,replace(tdd3m,',',''))))) from #Result where scrrg in ('C','D')) " +
                    " else tdd3m " +
                    "   end as tdd3m_Cum  " +
                    " , case when scrrg ='A' then (select dbo.comma_format(convert(varchar,SUM(convert(int,replace(tdd12m,',',''))))) from #Result where scrrg in ('A','B','C','D')) " +
                    " when scrrg ='B' then (select dbo.comma_format(convert(varchar,SUM(convert(int,replace(tdd12m,',',''))))) from #Result where scrrg in ('B','C','D')) " +
                    " when scrrg ='C' then (select dbo.comma_format(convert(varchar,SUM(convert(int,replace(tdd12m,',',''))))) from #Result where scrrg in ('C','D')) " +
                    " else tdd12m " +
                    "   end as tdd12m_Cum  " +
                    " , case when scrrg ='A' then (select dbo.comma_format(convert(varchar,SUM(convert(int,replace(tdd12yg,',',''))))) from #Result where scrrg in ('A','B','C','D')) " +
                    " when scrrg ='B' then (select dbo.comma_format(convert(varchar,SUM(convert(int,replace(tdd12yg,',',''))))) from #Result where scrrg in ('B','C','D')) " +
                    " when scrrg ='C' then (select dbo.comma_format(convert(varchar,SUM(convert(int,replace(tdd12yg,',',''))))) from #Result where scrrg in ('C','D')) " +
                    " else tdd12yg " +
                    "   end as tdd12yg_Cum  " +
                    " from #Result " +
                    " where scrrg in ('A','B','C','D') " +
                    " ) A " +
                    " WHERE #Result.scrrg=A.scrrg " +
                    " SELECT * FROM #Result " +
                    " drop table #z_Approval_Rate_Score " +
                    " drop table #Result " +
                                            "	drop table #Main_cutoff_table	drop table #Solution " +
                                            "	drop table #Cal_Avg_score " +
                                            "	drop table  #Final_Total_score " +
                                            "	drop table #Heading_Calculate_Score	" +
                                            "	drop table #Heading_Calculate_Score1	" +
                                            "	drop table #Heading_Calculate_Score2	" +
                                            "	drop table #Score_Cal_P4_1	" +
                                            "	drop table #Score_Cal_P4_2	" +
                                            "	drop table #Total_score_P4	" +
                                            "	drop table #No_of_Loan	 drop table  #Part1_Range " +
                                            "	drop table #Part1	drop table #PartA " +
                                            "	drop table  #Main_Table	" +
                                            "	drop table #tdd12mAgo	" +
                                            "	drop table #tdd12mAgo2	" +
                                            "	drop table #Heading	" +
                                            "	drop table #Main_cutoff2	drop table #Format_NULL_CASE" +
                                            "	drop table #Main_cutoff1	" +
                                            "	drop table #Main_cutoff3	" +
                                            "	drop table #Main_cutoff_Passing3	" +
                                            "	drop table #Main_cutoff_Passing2	" +
                                            "	drop table #Main_cutoff_Passing1	" +
                                            "	drop table #Main_cutoff_Passing_Table	"
                                            , ddlLoan.SelectedItem.Value, ddlSubType.SelectedItem.Value, mktqrf, mktqrt, start.ToString("yyyy-MM"), start12y.ToString("yyyy-MM"), end3m.ToString("yyyy-MM"), end12yg.ToString("yyyy-MM"), start12yg.ToString("yyyy-MM"));
                                        }
                                        else //Grade (%)
                                        {
                                            appQuery1 = string.Format("	select 	" +
                                            "	case when (SCORE - scr) < 0 then convert(varchar	" +
                                            "	,(scr + ((((SCORE + 1) - scr)/rint) -1 ) * rint)) + '-' + convert(varchar,((scr + ((((SCORE + 1 ) - scr)/rint)-1 ) * rint) + rint - 1))  	" +
                                            "	when (SCORE - scr) >= 0 then convert(varchar,(scr + ((SCORE - scr)/rint) * rint)) + '-' 	" +
                                            "	+ convert(varchar,((scr + ((SCORE - scr)/rint) * rint) + rint - 1))   end as scrdif 	" +
                                            "	,scApproval_flag,CONTRAST_FLAG	" +
                    "	,  case when CONVERT(VARCHAR,YEAR(APP_DATE))+'-'+RIGHT('00'+CONVERT(VARCHAR,MONTH(APP_DATE)),2) between substring('{6}',1,4)+'-'+ substring('{6}',6,2) and  substring('{4}',1,4)+'-'+ substring('{4}',6,2) then 1 else 0 end as tdd3m	" +
                    "	,  case when CONVERT(VARCHAR,YEAR(APP_DATE))+'-'+RIGHT('00'+CONVERT(VARCHAR,MONTH(APP_DATE)),2) between substring('{5}',1,4)+'-'+ substring('{5}',6,2) and  substring('{4}',1,4)+'-'+ substring('{4}',6,2) then 1 else 0 end as tdd12m	" +
                    "	,  case when CONVERT(VARCHAR,YEAR(APP_DATE))+'-'+RIGHT('00'+CONVERT(VARCHAR,MONTH(APP_DATE)),2) between substring('{6}',1,4)+'-'+ substring('{6}',6,2) and  substring('{4}',1,4)+'-'+ substring('{4}',6,2) and Approval_Flag ='Y' then 1 else 0 end as tdd3ma	" +
                    "	,  case when CONVERT(VARCHAR,YEAR(APP_DATE))+'-'+RIGHT('00'+CONVERT(VARCHAR,MONTH(APP_DATE)),2) between substring('{5}',1,4)+'-'+ substring('{5}',6,2) and  substring('{4}',1,4)+'-'+ substring('{4}',6,2) and Approval_Flag ='Y' then 1 else 0 end as tdd12ma	" +
                    "	,  case when CONVERT(VARCHAR,YEAR(APP_DATE))+'-'+RIGHT('00'+CONVERT(VARCHAR,MONTH(APP_DATE)),2) between substring('{6}',1,4)+'-'+ substring('{6}',6,2) and  substring('{4}',1,4)+'-'+ substring('{4}',6,2) and Approval_Flag ='N' then 1 else 0 end as tdd3mw	" +
                    "	,  case when CONVERT(VARCHAR,YEAR(APP_DATE))+'-'+RIGHT('00'+CONVERT(VARCHAR,MONTH(APP_DATE)),2) between substring('{5}',1,4)+'-'+ substring('{5}',6,2) and  substring('{4}',1,4)+'-'+ substring('{4}',6,2) and Approval_Flag ='N' then 1 else 0 end as tdd12mw	" +
                                            "	into #Heading	" +
                                            "	from 	" +
                                                //Tai 2013-01-02
                                                //"	(	select APP_NO,MODEL,LOAN_CD,SCORE,CBS_STATUS,STYPE_CD,MARKET_CD,DW_SIGN_DATE,APP_DATE,cut_off as scr,rint	" +
                                            "	(	select distinct APP_NO,MODEL,LOAN_CD,SCORE,CBS_STATUS,STYPE_CD,MARKET_CD,DW_SIGN_DATE,APP_DATE,cut_off as scr,rint	" +
                                                //
                                            "	,CASE WHEN (score>=cut_off) then 'Y' else 'N' end as scapproval_flag	" +
                                            "	,case when CBS_STATUS in (3,6,7) then 'Y' when  CBS_STATUS in (4,5) then 'N' when  CBS_STATUS in (1,2) OR CBS_STATUS is null then '<>' end as Approval_Flag	" +
                                            "	,case when LEFT(CBS_REASON_CD,1)='A' AND  CBS_STATUS in (4,5) then 'Y' 	" +
                                            "	when LEFT(CBS_REASON_CD,1)='R' AND  CBS_STATUS in (3,6,7) then 'Y' when LEFT(CBS_REASON_CD,1)='A' AND  CBS_STATUS in (3,6,7) then 'N'	" +
                                            "	when LEFT(CBS_REASON_CD,1)='R' AND  CBS_STATUS in (4,5) then 'N' else 'N/A' end as CONTRAST_FLAG,	" +
                                            "	case when DW_SIGN_DATE IS NULL THEN 'N' ELSE 'Y' END AS BOOKING_FLAG	" +
                                            "	,CBS_REASON_CD, LEFT(CBS_REASON_CD,1) as CBS_REASON	" +
                                            "	, CONVERT(VARCHAR,YEAR(APP_DATE))+'-'+RIGHT('00'+CONVERT(VARCHAR,MONTH(APP_DATE)),2) AS MYAppDate	" +
                                            "	from	" +
                                            "	(	" +
                                            "	SELECT a.*,b.score,b.model	" +
                                            "	,(select cut_off from ST_SCORERANGE y where  y.ltype= '{0}' and y.lstype = '{1}' ) as cut_off	" +
                                            "	,(select rint from ST_SCORERANGE y where  y.ltype= '{0}' and y.lstype = '{1}' ) as rint	" +
                                            "	,CBS_REASON_CD	" +
                                            "	from LN_APP a,LN_GRADE b,CBS_LN_APP c 	" +
                                            "	where a.APP_NO = b.APP_NO  	" +
                                            "	and a.app_no = c.CBS_APP_NO 	" +
                                            "	and  LOAN_CD = '{0}' and STYPE_CD = '{1}' {2} {3} 	" +
                                            "	) lnapp	" +
                                            "	left join	" +
                                            "	(	" +
                                            "	select * 	" +
                                            "	from	" +
                                            "	(	" +
                                            "	select [CBS_APP_NO], DW_SIGN_DATE,LAST_UPDATE_DTM	" +
                                            "	from [DWH_BTFILE] A	" +
                                            " where SUBSTRING([LAST_UPDATE_DTM],7,4)+'-'+SUBSTRING([LAST_UPDATE_DTM],4,2)+'-'+SUBSTRING([LAST_UPDATE_DTM],1,2)=(SELECT MAX(SUBSTRING([LAST_UPDATE_DTM],7,4)+'-'+SUBSTRING([LAST_UPDATE_DTM],4,2)+'-'+SUBSTRING([LAST_UPDATE_DTM],1,2)) FROM [DWH_BTFILE] B WHERE A.CBS_APP_NO=B.CBS_APP_NO)		" +
                                            "	) DW_BFILE_T	" +
                                            "	) gsb2T	" +
                                            "	 on gsb2T.CBS_APP_NO = lnapp.APP_NO    	" +
                                            "	) gsbdat  	" +
                                            "	where  CONTRAST_FLAG='N' and Approval_Flag in ('Y','N') and BOOKING_FLAG in ('Y','N') 	" +
                                            "	select scrdif,sum(tdd3m) as tdd3m,sum(tdd12m) as tdd12m, sum(tdd3ma) as tdd3ma, sum(tdd12ma) as tdd12ma	" +
                                            "	,sum(tdd3mw) as tdd3mw, sum(tdd12mw) as tdd12mw	" +
                                            "	into #Main_Table	" +
                                            "	from #Heading	" +
                                            "	group by scrdif	" +
                                            "	order by scrdif	" +
                                            "	select case when (SCORE - scr) < 0 then convert(varchar,(scr + ((((SCORE + 1) - scr)/rint) -1 ) * rint)) + '-' + convert(varchar,((scr + ((((SCORE + 1 ) - scr)/rint)-1 ) * rint) + rint - 1))  	" +
                                            "	when (SCORE - scr) >= 0 then convert(varchar,(scr + ((SCORE - scr)/rint) * rint)) + '-' + convert(varchar,((scr + ((SCORE - scr)/rint) * rint) + rint - 1))   	" +
                                            "	end as scrdif,scApproval_flag,CONTRAST_FLAG	" +
                   "	,  case when CONVERT(VARCHAR,YEAR(APP_DATE))+'-'+RIGHT('00'+CONVERT(VARCHAR,MONTH(APP_DATE)),2) between substring('{7}',1,4)+'-'+ substring('{7}',6,2) and  substring('{8}',1,4)+'-'+ substring('{8}',6,2) then 1 else 0 end as tdd12mAgo	" +
                   "	,  case when (CONVERT(VARCHAR,YEAR(APP_DATE))+'-'+RIGHT('00'+CONVERT(VARCHAR,MONTH(APP_DATE)),2) between substring('{7}',1,4)+'-'+ substring('{7}',6,2) and  substring('{8}',1,4)+'-'+ substring('{8}',6,2)) and (Approval_Flag ='N') then 1 else 0 end as TDD12YGW	" +
                   "	,  case when (CONVERT(VARCHAR,YEAR(APP_DATE))+'-'+RIGHT('00'+CONVERT(VARCHAR,MONTH(APP_DATE)),2) between substring('{7}',1,4)+'-'+ substring('{7}',6,2) and  substring('{8}',1,4)+'-'+ substring('{8}',6,2)) and (Approval_Flag ='Y') then 1 else 0 end as TDD12YGA	" +
                   "	,  case when (CONVERT(VARCHAR,YEAR(APP_DATE))+'-'+RIGHT('00'+CONVERT(VARCHAR,MONTH(APP_DATE)),2) between substring('{7}',1,4)+'-'+ substring('{7}',6,2) and  substring('{8}',1,4)+'-'+ substring('{8}',6,2)) and (Approval_Flag ='Y' and BOOKING_FLAG='Y') then 1 else 0 end as tdd12mygb	" +
                   "	,  case when (CONVERT(VARCHAR,YEAR(APP_DATE))+'-'+RIGHT('00'+CONVERT(VARCHAR,MONTH(APP_DATE)),2) between substring('{6}',1,4)+'-'+ substring('{6}',6,2) and  substring('{4}',1,4)+'-'+ substring('{4}',6,2)) and (Booking_Flag='Y' and Approval_Flag ='Y') then 1 else 0 end as tdd3mb	" +
                   "	,  case when (CONVERT(VARCHAR,YEAR(APP_DATE))+'-'+RIGHT('00'+CONVERT(VARCHAR,MONTH(APP_DATE)),2) between substring('{5}',1,4)+'-'+ substring('{5}',6,2) and  substring('{4}',1,4)+'-'+ substring('{4}',6,2)) and (Booking_Flag='Y' and Approval_Flag ='Y') then 1 else 0 end as tdd12mb 	" +
                                            "	into #tdd12mAgo	" +
                                            "	from	" +
                                            "	(	" +
                                                //Tai 2014-01-02
                                                //"	select APP_NO,MODEL,LOAN_CD,SCORE,CBS_STATUS,STYPE_CD,MARKET_CD,DW_SIGN_DATE,APP_DATE,cut_off as scr,rint	" +
                                            "	select distinct APP_NO,MODEL,LOAN_CD,SCORE,CBS_STATUS,STYPE_CD,MARKET_CD,DW_SIGN_DATE,APP_DATE,cut_off as scr,rint	" +
                                                //
                                            "	,CASE WHEN (score>=cut_off) then 'Y' else 'N' end as scapproval_flag	" +
                                            "	,case when CBS_STATUS in (3,6,7) then 'Y' when  CBS_STATUS in (4,5) then 'N' when  CBS_STATUS in (1,2) OR CBS_STATUS is null then '<>' end as Approval_Flag	" +
                                            "	,case when LEFT(CBS_REASON_CD,1)='A' AND  CBS_STATUS in (4,5) then 'Y' 	" +
                                            "	when LEFT(CBS_REASON_CD,1)='R' AND  CBS_STATUS in (3,6,7) then 'Y' when LEFT(CBS_REASON_CD,1)='A' AND  CBS_STATUS in (3,6,7) then 'N'	" +
                                            "	when LEFT(CBS_REASON_CD,1)='R' AND  CBS_STATUS in (4,5) then 'N' else 'N/A' end as CONTRAST_FLAG,	" +
                                            "	case when DW_SIGN_DATE IS NULL THEN 'N' ELSE 'Y' END AS BOOKING_FLAG	" +
                                            "	,CBS_REASON_CD, LEFT(CBS_REASON_CD,1) as CBS_REASON	" +
                                            "	, CONVERT(VARCHAR,YEAR(APP_DATE))+'-'+RIGHT('00'+CONVERT(VARCHAR,MONTH(APP_DATE)),2) AS MYAppDate	" +
                                            "	from	" +
                                            "	(	" +
                                            "	SELECT a.*,b.score,b.model	" +
                                            "	,(select cut_off from ST_SCORERANGE y where  y.ltype= '{0}' and y.lstype = '{1}' ) as cut_off	" +
                                            "	,(select rint from ST_SCORERANGE y where  y.ltype= '{0}' and y.lstype = '{1}' ) as rint	" +
                                            "	      ,CBS_REASON_CD	" +
                                            "	from LN_APP a,LN_GRADE b,CBS_LN_APP c 	" +
                                            "	where a.APP_NO = b.APP_NO  	" +
                                            "	and a.app_no = c.CBS_APP_NO 	" +
                                            "	and  LOAN_CD = '{0}' and STYPE_CD = '{1}' {2} {3} 	" +
                                            "	) lnapp	" +
                                            "	left join	" +
                                            "	(	" +
                                            "	select * 	" +
                                            "	from	" +
                                            "	(	" +
                                            "	select [CBS_APP_NO], DW_SIGN_DATE,LAST_UPDATE_DTM	" +
                                            "	from [DWH_BTFILE] A	" +
                   " where SUBSTRING([LAST_UPDATE_DTM],7,4)+'-'+SUBSTRING([LAST_UPDATE_DTM],4,2)+'-'+SUBSTRING([LAST_UPDATE_DTM],1,2)=(SELECT MAX(SUBSTRING([LAST_UPDATE_DTM],7,4)+'-'+SUBSTRING([LAST_UPDATE_DTM],4,2)+'-'+SUBSTRING([LAST_UPDATE_DTM],1,2)) FROM [DWH_BTFILE] B WHERE A.CBS_APP_NO=B.CBS_APP_NO)	" +
                                            "	 ) DW_BFILE_T	" +
                                            "	 ) gsb2T	" +
                                            "	 on gsb2T.CBS_APP_NO = lnapp.APP_NO    	" +
                                            "	) Z	" +
                                            "	where  CONTRAST_FLAG='N' and Approval_Flag in ('Y','N') and BOOKING_FLAG in ('Y','N')	" +
                                            "	select scrdif,sum(tdd12mAgo) as tdd12mAgo,sum(TDD12YGW) as TDD12YGW,sum(tdd12mygb) as tdd12mygb	" +
                                            "	,sum(TDD12YGA) as TDD12YGA	" +
                                            "	,sum(tdd3mb) as tdd3mb,sum(tdd12mb) as tdd12mb	" +
                                            "	into #tdd12mAgo2	" +
                                            "	from #tdd12mAgo	" +
                                            "	group by scrdif	" +
                                            "	order by scrdif	" +
                                            "	select row_number() over(order by a.scrdif) as seq,a.scrdif as SCRRG,tdd3m,tdd12m,tdd12mAgo as tdd12yg	" +
                                            "	,tdd3mw,tdd12mw,TDD12YGW as tdd12ygw,tdd3ma,tdd12ma,TDD12YGA as tdd12yga	" +
                                            "	,tdd3mb,tdd12mb,tdd12mygb as tdd12ygb	" +
                                            "	into #Part1	" +
                                            "	from #Main_Table a, #tdd12mAgo2 b	" +
                                            "	where a.scrdif=b.scrdif	" +
                                            "	select 131 as seq, 'No. of Loans' as SCRRG,sum(tdd3m) as tdd3m,sum(tdd12m) as tdd12m	" +
                                            "	,sum(tdd12yg) as tdd12yg,sum(tdd3mw) as tdd3mw	" +
                                            "	,sum(tdd12mw) as tdd12mw,sum(tdd12ygw) as tdd12ygw,sum(tdd3ma) as tdd3ma	" +
                                            "	,sum(tdd12ma) as tdd12ma,sum(tdd12yga) as tdd12yga	" +
                                            "	,sum(tdd3mb) as tdd3mb,sum(tdd12mb) as tdd12mb,sum(tdd12ygb) as tdd12ygb	" +
                                            "	into #No_of_Loan	" +
                                            "	from #Part1	" +
                                            "	select 	" +
                                            "	case when (SCORE - scr) < 0 then convert(varchar	" +
                                            "	,(scr + ((((SCORE + 1) - scr)/rint) -1 ) * rint)) + '-' + convert(varchar,((scr + ((((SCORE + 1 ) - scr)/rint)-1 ) * rint) + rint - 1))  	" +
                                            "	when (SCORE - scr) >= 0 then convert(varchar,(scr + ((SCORE - scr)/rint) * rint)) + '-' 	" +
                                            "	+ convert(varchar,((scr + ((SCORE - scr)/rint) * rint) + rint - 1))   end as scrdif 	" +
                                            "	,scApproval_flag,CONTRAST_FLAG	" +
                   "	,  case when CONVERT(VARCHAR,YEAR(APP_DATE))+'-'+RIGHT('00'+CONVERT(VARCHAR,MONTH(APP_DATE)),2) between substring('{6}',1,4)+'-'+ substring('{6}',6,2) and  substring('{4}',1,4)+'-'+ substring('{4}',6,2) then score else 0 end as tdd3m_score	" +
                   "	,  case when CONVERT(VARCHAR,YEAR(APP_DATE))+'-'+RIGHT('00'+CONVERT(VARCHAR,MONTH(APP_DATE)),2) between substring('{5}',1,4)+'-'+ substring('{5}',6,2) and  substring('{4}',1,4)+'-'+ substring('{4}',6,2) then score else 0 end as tdd12m_score	" +
                   "	,  case when CONVERT(VARCHAR,YEAR(APP_DATE))+'-'+RIGHT('00'+CONVERT(VARCHAR,MONTH(APP_DATE)),2) between substring('{6}',1,4)+'-'+ substring('{6}',6,2) and  substring('{4}',1,4)+'-'+ substring('{4}',6,2) and Approval_Flag ='Y' then score else 0 end as tdd3ma_score	" +
                   "	,  case when CONVERT(VARCHAR,YEAR(APP_DATE))+'-'+RIGHT('00'+CONVERT(VARCHAR,MONTH(APP_DATE)),2) between substring('{5}',1,4)+'-'+ substring('{5}',6,2) and  substring('{4}',1,4)+'-'+ substring('{4}',6,2) and Approval_Flag ='Y' then score else 0 end as tdd12ma_score	" +
                   "	,  case when CONVERT(VARCHAR,YEAR(APP_DATE))+'-'+RIGHT('00'+CONVERT(VARCHAR,MONTH(APP_DATE)),2) between substring('{6}',1,4)+'-'+ substring('{6}',6,2) and  substring('{4}',1,4)+'-'+ substring('{4}',6,2) and Approval_Flag ='N' then score else 0 end as tdd3mw_score	" +
                   "	,  case when CONVERT(VARCHAR,YEAR(APP_DATE))+'-'+RIGHT('00'+CONVERT(VARCHAR,MONTH(APP_DATE)),2) between substring('{5}',1,4)+'-'+ substring('{5}',6,2) and  substring('{4}',1,4)+'-'+ substring('{4}',6,2) and Approval_Flag ='N' then score else 0 end as tdd12mw_score	" +
                                            "	into #Heading_Calculate_Score	" +
                                            "	from 	" +
                                            "	(	" +
                                                //Tai 2014-01-02
                                                //"	select APP_NO,MODEL,LOAN_CD,SCORE,CBS_STATUS,STYPE_CD,MARKET_CD,DW_SIGN_DATE,APP_DATE,cut_off as scr,rint	" +
                                            "	select distinct APP_NO,MODEL,LOAN_CD,SCORE,CBS_STATUS,STYPE_CD,MARKET_CD,DW_SIGN_DATE,APP_DATE,cut_off as scr,rint	" +
                                                //
                                            "	,CASE WHEN (score>=cut_off) then 'Y' else 'N' end as scapproval_flag	" +
                                            "	,case when CBS_STATUS in (3,6,7) then 'Y' when  CBS_STATUS in (4,5) then 'N' when  CBS_STATUS in (1,2) OR CBS_STATUS is null then '<>' end as Approval_Flag	" +
                                            "	,case when LEFT(CBS_REASON_CD,1)='A' AND  CBS_STATUS in (4,5) then 'Y' 	" +
                                            "	when LEFT(CBS_REASON_CD,1)='R' AND  CBS_STATUS in (3,6,7) then 'Y' when LEFT(CBS_REASON_CD,1)='A' AND  CBS_STATUS in (3,6,7) then 'N'	" +
                                            "	when LEFT(CBS_REASON_CD,1)='R' AND  CBS_STATUS in (4,5) then 'N' else 'N/A' end as CONTRAST_FLAG,	" +
                                            "	case when DW_SIGN_DATE IS NULL THEN 'N' ELSE 'Y' END AS BOOKING_FLAG	" +
                                            "	,CBS_REASON_CD, LEFT(CBS_REASON_CD,1) as CBS_REASON	" +
                                            "	, CONVERT(VARCHAR,YEAR(APP_DATE))+'-'+RIGHT('00'+CONVERT(VARCHAR,MONTH(APP_DATE)),2) AS MYAppDate	" +
                                            "	from	" +
                                            "	(	" +
                                            "	SELECT a.*,b.score,b.model	" +
                                            "	,(select cut_off from ST_SCORERANGE y where  y.ltype= '{0}' and y.lstype = '{1}' ) as cut_off	" +
                                            "	,(select rint from ST_SCORERANGE y where  y.ltype= '{0}' and y.lstype = '{1}' ) as rint	" +
                                            "	,CBS_REASON_CD	" +
                                            "	from LN_APP a,LN_GRADE b,CBS_LN_APP c 	" +
                                            "	where a.APP_NO = b.APP_NO  	" +
                                            "	and a.app_no = c.CBS_APP_NO 	" +
                                            "	and  LOAN_CD = '{0}' and STYPE_CD = '{1}' {2} {3} 	" +
                                            "	) lnapp	" +
                                            "	left join	" +
                                            "	(	" +
                                            "	select * 	" +
                                            "	from	" +
                                            "	(	" +
                                            "	select [CBS_APP_NO], DW_SIGN_DATE,LAST_UPDATE_DTM	" +
                                            "	from [DWH_BTFILE] A	" +
                                            " where SUBSTRING([LAST_UPDATE_DTM],7,4)+'-'+SUBSTRING([LAST_UPDATE_DTM],4,2)+'-'+SUBSTRING([LAST_UPDATE_DTM],1,2)=(SELECT MAX(SUBSTRING([LAST_UPDATE_DTM],7,4)+'-'+SUBSTRING([LAST_UPDATE_DTM],4,2)+'-'+SUBSTRING([LAST_UPDATE_DTM],1,2)) FROM [DWH_BTFILE] B WHERE A.CBS_APP_NO=B.CBS_APP_NO)		" +
                                            "	) DW_BFILE_T	" +
                                            "	) gsb2T	" +
                                            "	on gsb2T.CBS_APP_NO = lnapp.APP_NO    	" +
                                            "	) gsbdat  	" +
                                            "	where  CONTRAST_FLAG='N' and Approval_Flag in ('Y','N') and BOOKING_FLAG in ('Y','N') 	" +
                                            "	select case when (SCORE - scr) < 0 then convert(varchar,(scr + ((((SCORE + 1) - scr)/rint) -1 ) * rint)) + '-' + convert(varchar,((scr + ((((SCORE + 1 ) - scr)/rint)-1 ) * rint) + rint - 1))  	" +
                                            "	when (SCORE - scr) >= 0 then convert(varchar,(scr + ((SCORE - scr)/rint) * rint)) + '-' + convert(varchar,((scr + ((SCORE - scr)/rint) * rint) + rint - 1))   	" +
                                            "	end as scrdif,scApproval_flag,CONTRAST_FLAG	" +
                   "	,  case when CONVERT(VARCHAR,YEAR(APP_DATE))+'-'+RIGHT('00'+CONVERT(VARCHAR,MONTH(APP_DATE)),2) between substring('{7}',1,4)+'-'+ substring('{7}',6,2) and  substring('{8}',1,4)+'-'+ substring('{8}',6,2) then score else 0 end as tdd12mAgo_score	" +
                   "	,  case when (CONVERT(VARCHAR,YEAR(APP_DATE))+'-'+RIGHT('00'+CONVERT(VARCHAR,MONTH(APP_DATE)),2) between substring('{7}',1,4)+'-'+ substring('{7}',6,2) and  substring('{8}',1,4)+'-'+ substring('{8}',6,2)) and (Approval_Flag ='N') then score else 0 end as TDD12YGW_score	" +
                   "	,  case when (CONVERT(VARCHAR,YEAR(APP_DATE))+'-'+RIGHT('00'+CONVERT(VARCHAR,MONTH(APP_DATE)),2) between substring('{7}',1,4)+'-'+ substring('{7}',6,2) and  substring('{8}',1,4)+'-'+ substring('{8}',6,2)) and (Approval_Flag ='Y') then score else 0 end as TDD12YGA_score	" +
                   "	,  case when (CONVERT(VARCHAR,YEAR(APP_DATE))+'-'+RIGHT('00'+CONVERT(VARCHAR,MONTH(APP_DATE)),2) between substring('{7}',1,4)+'-'+ substring('{7}',6,2) and  substring('{8}',1,4)+'-'+ substring('{8}',6,2)) and (Approval_Flag ='Y' and BOOKING_FLAG='Y') then score else 0 end as tdd12mygb_score	" +
                   "	,  case when (CONVERT(VARCHAR,YEAR(APP_DATE))+'-'+RIGHT('00'+CONVERT(VARCHAR,MONTH(APP_DATE)),2) between substring('{6}',1,4)+'-'+ substring('{6}',6,2) and  substring('{4}',1,4)+'-'+ substring('{4}',6,2)) and (Booking_Flag='Y' and Approval_Flag ='Y') then score else 0 end as tdd3mb_score	" +
                   "	,  case when (CONVERT(VARCHAR,YEAR(APP_DATE))+'-'+RIGHT('00'+CONVERT(VARCHAR,MONTH(APP_DATE)),2) between substring('{5}',1,4)+'-'+ substring('{5}',6,2) and  substring('{4}',1,4)+'-'+ substring('{4}',6,2)) and (Booking_Flag='Y' and Approval_Flag ='Y') then score else 0 end as tdd12mb_score 	" +
                                            "	into #Score_Cal_P4_1	" +
                                            "	from	" +
                                            "	(	" +
                                                //Tai 2014-02-01
                                                //"	select APP_NO,MODEL,LOAN_CD,SCORE,CBS_STATUS,STYPE_CD,MARKET_CD,DW_SIGN_DATE,APP_DATE,cut_off as scr,rint	" +
                                            "	select distinct APP_NO,MODEL,LOAN_CD,SCORE,CBS_STATUS,STYPE_CD,MARKET_CD,DW_SIGN_DATE,APP_DATE,cut_off as scr,rint	" +
                                                //
                                            "	,CASE WHEN (score>=cut_off) then 'Y' else 'N' end as scapproval_flag	" +
                                            "	,case when CBS_STATUS in (3,6,7) then 'Y' when  CBS_STATUS in (4,5) then 'N' when  CBS_STATUS in (1,2) OR CBS_STATUS is null then '<>' end as Approval_Flag	" +
                                            "	,case when LEFT(CBS_REASON_CD,1)='A' AND  CBS_STATUS in (4,5) then 'Y' 	" +
                                            "	when LEFT(CBS_REASON_CD,1)='R' AND  CBS_STATUS in (3,6,7) then 'Y' when LEFT(CBS_REASON_CD,1)='A' AND  CBS_STATUS in (3,6,7) then 'N'	" +
                                            "	when LEFT(CBS_REASON_CD,1)='R' AND  CBS_STATUS in (4,5) then 'N' else 'N/A' end as CONTRAST_FLAG,	" +
                                            "	case when DW_SIGN_DATE IS NULL THEN 'N' ELSE 'Y' END AS BOOKING_FLAG	" +
                                            "	,CBS_REASON_CD, LEFT(CBS_REASON_CD,1) as CBS_REASON	" +
                                            "	, CONVERT(VARCHAR,YEAR(APP_DATE))+'-'+RIGHT('00'+CONVERT(VARCHAR,MONTH(APP_DATE)),2) AS MYAppDate	" +
                                            "	from	" +
                                            "	(	" +
                                            "	SELECT a.*,b.score,b.model	" +
                                            "	,(select cut_off from ST_SCORERANGE y where  y.ltype= '{0}' and y.lstype = '{1}' ) as cut_off	" +
                                            "	,(select rint from ST_SCORERANGE y where  y.ltype= '{0}' and y.lstype = '{1}' ) as rint	" +
                                            "	      ,CBS_REASON_CD	" +
                                            "	from LN_APP a,LN_GRADE b,CBS_LN_APP c 	" +
                                            "	where a.APP_NO = b.APP_NO  	" +
                                            "	and a.app_no = c.CBS_APP_NO 	" +
                                            "	and  LOAN_CD = '{0}' and STYPE_CD = '{1}' {2} {3} 	" +
                                            "	) lnapp	" +
                                            "	left join	" +
                                            "	(	" +
                                            "	select * 	" +
                                            "	from	" +
                                            "	(	" +
                                            "	select [CBS_APP_NO], DW_SIGN_DATE,LAST_UPDATE_DTM	" +
                                            "	from [DWH_BTFILE] A	" +
                   " where SUBSTRING([LAST_UPDATE_DTM],7,4)+'-'+SUBSTRING([LAST_UPDATE_DTM],4,2)+'-'+SUBSTRING([LAST_UPDATE_DTM],1,2)=(SELECT MAX(SUBSTRING([LAST_UPDATE_DTM],7,4)+'-'+SUBSTRING([LAST_UPDATE_DTM],4,2)+'-'+SUBSTRING([LAST_UPDATE_DTM],1,2)) FROM [DWH_BTFILE] B WHERE A.CBS_APP_NO=B.CBS_APP_NO)		" +
                                            "	 ) DW_BFILE_T	" +
                                            "	 ) gsb2T	" +
                                            "	 on gsb2T.CBS_APP_NO = lnapp.APP_NO    	" +
                                            "	) Z	" +
                                            "	where  CONTRAST_FLAG='N' and Approval_Flag in ('Y','N') and BOOKING_FLAG in ('Y','N')	" +
                                            "	select scrdif,sum(tdd12mAgo_score) as tdd12mAgo_score	" +
                                            "	,sum(TDD12YGW_score) as TDD12YGW_score,sum(tdd12mygb_score) as tdd12mygb_score	" +
                                            "	,sum(TDD12YGA_score) as TDD12YGA_score	" +
                                            "	,sum(tdd3mb_score) as tdd3mb_score,sum(tdd12mb_score) as tdd12mb_score	" +
                                            "	into #Score_Cal_P4_2	" +
                                            "	from #Score_Cal_P4_1	" +
                                            "	group by scrdif	" +
                                            "	order by scrdif	" +
                                            "	select 129 as seq, 'Total_score' as SCRRG,sum(tdd12mAgo_score) as tdd12mAgo	" +
                                            "	,sum(TDD12YGW_score) as TDD12YGW	" +
                                            "	,sum(TDD12YGA_score) as TDD12YGA	" +
                                            "	,sum(tdd3mb_score) as tdd3mb	" +
                                            "	,sum(tdd12mb_score) as tdd12mb	" +
                                            "	,sum(tdd12mygb_score) as tdd12mygb	" +
                                            "	into #Total_score_P4	" +
                                            "	from #Score_Cal_P4_2	" +
                                            "	select scrdif,sum(tdd3m_score) as tdd3m_score,sum(tdd12m_score) as tdd12m_score,sum(tdd3ma_score) as tdd3ma_score	" +
                                            "	,sum(tdd12ma_score) as tdd12ma_score, sum(tdd3mw_score) as tdd3mw_score,sum(tdd12mw_score) as tdd12mw_score	" +
                                            "	into #Heading_Calculate_Score1	" +
                                            "	from #Heading_Calculate_Score	" +
                                            "	group by scrdif	" +
                                            "	order by scrdif	" +
                                            "	select 129 as seq, 'Total_score' as SCRRG,sum(tdd3m_score) as tdd3m,sum(tdd12m_score) as tdd12m,sum(tdd3ma_score) as tdd3ma	" +
                                            "	,sum(tdd12ma_score) as tdd12ma,sum(tdd3mw_score) as tdd3mw	" +
                                            "	,sum(tdd12mw_score) as tdd12mw	" +
                                            "	into #Heading_Calculate_Score2	" +
                                            "	from #Heading_Calculate_Score1	" +
                                            "	select a.seq,a.SCRRG,tdd3m,tdd12m,tdd12mAgo as tdd12yg,tdd3ma,tdd12ma,TDD12YGA as tdd12yga,tdd3mw,tdd12mw,TDD12YGW as tdd12ygw,tdd3mb,tdd12mb	" +
                                            "	,tdd12mygb as tdd12ygb	" +
                                            "	into #Final_Total_score	" +
                                            "	from #Heading_Calculate_Score2 a, #Total_score_P4 b	" +
                                            "	where a.seq=b.seq	" +
                                            "	select 132 as seq, 'Avg. Score' as SCRRG,convert(decimal(15,3),b.tdd3m)/case when a.tdd3m=0 then 1 else a.tdd3m end as tdd3m	" +
                                            "	,convert(decimal(15,3),b.tdd12m)/case when a.tdd12m=0 then 1 else a.tdd12m end as tdd12m	" +
                                            "	,convert(decimal(15,3),b.tdd12yg)/case when a.tdd12yg=0 then 1 else a.tdd12yg end as tdd12yg	" +
                                            "	,convert(decimal(15,3),b.tdd3mw)/case when a.tdd3mw=0 then 1 else a.tdd3mw end as tdd3mw	" +
                                            "	,convert(decimal(15,3),b.tdd12mw)/case when a.tdd12mw=0 then 1 else  a.tdd12mw end as tdd12mw	" +
                                            "	,convert(decimal(15,3),b.tdd12ygw)/case when a.tdd12ygw=0 then 1 else  a.tdd12ygw end as tdd12ygw	" +
                                            "	,convert(decimal(15,3),b.tdd3ma)/case when a.tdd3ma =0 then 1 else a.tdd3ma end as tdd3ma	" +
                                            "	,convert(decimal(15,3),b.tdd12ma)/case when a.tdd12ma=0 then 1 else a.tdd12ma end as tdd12ma	" +
                                            "	,convert(decimal(15,3),b.tdd12yga)/case when a.tdd12yga=0 then 1 else a.tdd12yga end as tdd12yga	" +
                                            "	,convert(decimal(15,3),b.tdd3mb)/case when a.tdd3mb=0 then 1 else a.tdd3mb end as tdd3mb	" +
                                            "	,convert(decimal(15,3),b.tdd12mb)/case when a.tdd12mb=0  then 1 else a.tdd12mb end as tdd12mb	" +
                                            "	,convert(decimal(15,3),b.tdd12ygb)/case when a.tdd12ygb=0 then 1 else a.tdd12ygb end as tdd12ygb	" +
                                            "	into #Cal_Avg_score	" +
                                            "	from #No_of_Loan a,#Final_Total_score b	" +
                                            "	select sum(tdd3m) as tdd3m,sum(tdd12m) as tdd12m, sum(tdd3ma) as tdd3ma, sum(tdd12ma) as tdd12ma	" +
                                            "	, sum(tdd3mw) as tdd3mw, sum(tdd12mw) as tdd12mw	" +
                                            "	into #Main_cutoff1	" +
                                            "	from #Heading	" +
                                            "	where scApproval_flag='N'	" +
                                            "	select sum(tdd12mAgo) as tdd12yg,sum(TDD12YGW) as TDD12YGW,sum(tdd12mygb) as tdd12ygb	" +
                                            "	,sum(TDD12YGA) as TDD12YGA	" +
                                            "	,sum(tdd3mb) as tdd3mb,sum(tdd12mb) as tdd12mb	" +
                                            "	into #Main_cutoff2	" +
                                            "	from #tdd12mAgo	" +
                                            "	where scApproval_flag='N'	" +
                                            "	select  tdd3m,tdd12m,tdd12yg,tdd3mw,tdd12mw,tdd12ygw,tdd3ma,tdd12ma,tdd12yga,tdd3mb,tdd12mb,tdd12ygb	" +
                                            "	into #Main_cutoff3	" +
                                            "	from #Main_cutoff1 a,#Main_cutoff2 b	" +
                                            "	select 128 as seq,'% Below Cutoff' as SCRRG,convert(decimal(15,3),a.tdd3m)*100.00/case when b.tdd3m=0 then 1 else b.tdd3m end as tdd3m	" +
                                            "	,convert(decimal(15,3),a.tdd12m)*100.00/case when b.tdd12m=0 then 1 else b.tdd12m end as tdd12m	" +
                                            "	,convert(decimal(15,3),a.tdd12yg)*100.00/case when b.tdd12yg=0 then 1 else b.tdd12yg end as tdd12yg	" +
                                            "	,convert(decimal(15,3),a.tdd3mw)*100.00/case when b.tdd3mw=0 then 1 else b.tdd3mw end as tdd3mw		" +
                                            "	,convert(decimal(15,3),a.tdd12mw)*100.00/case when b.tdd12mw=0 then 1 else b.tdd12mw end as tdd12mw	" +
                                            "	,convert(decimal(15,3),a.tdd12ygw)*100.00/case when b.tdd12ygw=0 then 1 else  b.tdd12ygw end as tdd12ygw	" +
                                            "	,convert(decimal(15,3),a.tdd3ma)*100.00/case when b.tdd3ma =0 then 1 else b.tdd3ma end as tdd3ma	" +
                                            "	,convert(decimal(15,3),a.tdd12ma)*100.00/case when b.tdd12ma=0 then 1 else b.tdd12ma end as tdd12ma	" +
                                            "	,convert(decimal(15,3),a.tdd12yga)*100.00/case when b.tdd12yga=0 then 1 else b.tdd12yga end as tdd12yga	" +
                                            "	,convert(decimal(15,3),a.tdd3mb)*100.00/case when b.tdd3mb=0 then 1 else b.tdd3mb end as tdd3mb	" +
                                            "	,convert(decimal(15,3),a.tdd12mb)*100.00/case when b.tdd12mb=0  then 1 else b.tdd12mb end as tdd12mb	" +
                                            "	,convert(decimal(15,3),a.tdd12ygb)*100.00/case when b.tdd12ygb=0 then 1 else b.tdd12ygb end as tdd12ygb	" +
                                            "	into #Main_cutoff_table	" +
                                            "	from #Main_cutoff3 a, #No_of_Loan b	" +
                                            "	select sum(tdd3m) as tdd3m,sum(tdd12m) as tdd12m, sum(tdd3ma) as tdd3ma, sum(tdd12ma) as tdd12ma	" +
                                            "	, sum(tdd3mw) as tdd3mw, sum(tdd12mw) as tdd12mw	" +
                                            "	into #Main_cutoff_Passing1	" +
                                            "	from #Heading	" +
                                            "	where scApproval_flag='Y'	" +
                                            "	select sum(tdd12mAgo) as tdd12yg,sum(TDD12YGW) as TDD12YGW,sum(tdd12mygb) as tdd12ygb	" +
                                            "	,sum(TDD12YGA) as TDD12YGA	" +
                                            "	,sum(tdd3mb) as tdd3mb,sum(tdd12mb) as tdd12mb	" +
                                            "	into #Main_cutoff_Passing2	" +
                                            "	from #tdd12mAgo	" +
                                            "	where scApproval_flag='Y'	" +
                                            "	select  tdd3m,tdd12m,tdd12yg,tdd3mw,tdd12mw,tdd12ygw,tdd3ma,tdd12ma,tdd12yga,tdd3mb,tdd12mb,tdd12ygb	" +
                                            "	into #Main_cutoff_Passing3	" +
                                            "	from #Main_cutoff_Passing1 a,#Main_cutoff_Passing2 b	" +
                                            "	select 130 as seq,'% Passing Cutoff' as SCRRG,convert(decimal(15,3),a.tdd3m)*100.00/case when b.tdd3m=0 then 1 else b.tdd3m end as tdd3m	" +
                                            "	,convert(decimal(15,3),a.tdd12m)*100.00/case when b.tdd12m=0 then 1 else b.tdd12m end as tdd12m	" +
                                            "	,convert(decimal(15,3),a.tdd12yg)*100.00/case when b.tdd12yg=0 then 1 else b.tdd12yg end as tdd12yg	" +
                                            "	,convert(decimal(15,3),a.tdd3mw)*100.00/case when b.tdd3mw=0 then 1 else b.tdd3mw end as tdd3mw	" +
                                            "	,convert(decimal(15,3),a.tdd12mw)*100.00/case when b.tdd12mw=0 then 1 else b.tdd12mw end as tdd12mw	" +
                                            "	,convert(decimal(15,3),a.tdd12ygw)*100.00/case when b.tdd12ygw=0 then 1 else b.tdd12ygw end as tdd12ygw	" +
                                            "	,convert(decimal(15,3),a.tdd3ma)*100.00/case when b.tdd3ma=0 then 1 else b.tdd3ma end as tdd3ma	" +
                                            "	,convert(decimal(15,3),a.tdd12ma)*100.00/case when b.tdd12ma=0 then 1 else b.tdd12ma end as tdd12ma	" +
                                            "	,convert(decimal(15,3),a.tdd12yga)*100.00 /case when b.tdd12yga=0 then 1 else b.tdd12yga end as tdd12yga	" +
                                            "	,convert(decimal(15,3),a.tdd3mb)*100.00/case when b.tdd3mb=0 then 1 else b.tdd3mb end as tdd3mb	" +
                                            "	,convert(decimal(15,3),a.tdd12mb)*100.00/case when b.tdd12mb=0  then 1 else b.tdd12mb end as tdd12mb	" +
                                            "	,convert(decimal(15,3),a.tdd12ygb)*100.00/case when b.tdd12ygb=0 then 1 else b.tdd12ygb end as tdd12ygb	" +
                                            "	into #Main_cutoff_Passing_Table	" +
                                            "	from #Main_cutoff_Passing3 a, #No_of_Loan b	" +
                   " select AA.seq,AA.SCRRG,tdd3m,tdd12m,tdd12yg,tdd3mw,tdd12mw,tdd12ygw,tdd3ma,tdd12ma,tdd12yga,tdd3mb,tdd12mb,tdd12ygb into  #Part1_Range	" +
                   " from (select row_number() over(order by range_name) as seq,range_name as SCRRG from st_scorernglst 	" +
                   " where range_cd = (select range_cd from ST_SCORERANGE where ltype= '{0}' and lstype = '{1}')  ) AA left join (select * from #Part1 ) BB on AA.SCRRG=BB.SCRRG 	" +
                   " select AA.seq,AA.SCRRG,tdd3m,tdd12m,tdd12yg,tdd3mw,tdd12mw,tdd12ygw,tdd3ma,tdd12ma,tdd12yga,tdd3mb,tdd12mb,tdd12ygb	" +
                   " into #Solution  from (select seq,SCRRG,(select sum(tdd3m) from #Part1 t2 where t2.SCRRG <= t1.SCRRG) as tdd3m,(select sum(tdd12m)	" +
                   " from #Part1 t2  where t2.SCRRG <= t1.SCRRG) as tdd12m	,(select sum(tdd12yg) from #Part1 t2 where t2.SCRRG <= t1.SCRRG) as tdd12yg,tdd3mw as [tdd3mw]	" +
                   " ,tdd12mw as [tdd12mw],tdd12ygw as [tdd12ygw]	 ,tdd3ma as [tdd3ma],tdd12ma as [tdd12ma],tdd12yga as [tdd12yga]	" +
                   " ,tdd3mb as [tdd3mb],tdd12mb as [tdd12mb],tdd12ygb as [tdd12ygb]  from #Part1_Range t1   union   select * from #No_of_Loan  ) AA	" +
                       " select * into #PartA from (	" +
                                           " select seq,SCRRG,[dbo].[comma_format](convert(varchar,convert(integer,tdd3m))) as tdd3m,[dbo].[comma_format](convert(varchar,convert(integer,tdd12m))) as tdd12m	" +
                   " ,[dbo].[comma_format](convert(varchar,convert(integer,tdd12yg))) as tdd12yg,[dbo].[comma_format](convert(varchar,convert(integer,tdd3mw))) as tdd3mw	" +
                   " ,[dbo].[comma_format](convert(varchar,convert(integer,tdd12mw))) as tdd12mw,[dbo].[comma_format](convert(varchar,convert(integer,tdd12ygw))) as tdd12ygw	" +
                   " ,[dbo].[comma_format](convert(varchar,convert(integer,tdd3ma))) as tdd3ma,[dbo].[comma_format](convert(varchar,convert(integer,tdd12ma))) as tdd12ma " +
                   " ,[dbo].[comma_format](convert(varchar,convert(integer,tdd12yga))) as tdd12yga,[dbo].[comma_format](convert(varchar,convert(integer,tdd3mb)))	as tdd3mb" +
                   " ,[dbo].[comma_format](convert(varchar,convert(integer,tdd12mb))) as tdd12mb,[dbo].[comma_format](convert(varchar,convert(integer,tdd12ygb)))	as tdd12ygb " +
                   " from #Solution	" +
                                            "	union	" +
                   " select seq,SCRRG,convert(varchar,[dbo].[comma_percent_format](convert(decimal(15,2),tdd3m))) as tdd3m,[dbo].[comma_percent_format](convert(varchar,convert(decimal(15,2),tdd12m))) as tdd12m " +
                   " ,[dbo].[comma_percent_format](convert(varchar,convert(decimal(15,2),tdd12yg))) as tdd12yg,[dbo].[comma_percent_format](convert(varchar,convert(decimal(15,2),tdd3mw))) as tdd3mw " +
                   " ,[dbo].[comma_percent_format](convert(varchar,convert(decimal(15,2),tdd12mw))) as tdd12mw,[dbo].[comma_percent_format](convert(varchar,convert(decimal(15,2),tdd12ygw))) as tdd12ygw " +
                   " ,[dbo].[comma_percent_format](convert(varchar,convert(decimal(15,2),tdd3ma))) as tdd3ma,[dbo].[comma_percent_format](convert(varchar,convert(decimal(15,2),tdd12ma))) as tdd12ma " +
                   " ,[dbo].[comma_percent_format](convert(varchar,convert(decimal(15,2),tdd12yga))) as tdd12yga,[dbo].[comma_percent_format](convert(varchar,convert(decimal(15,2),tdd3mb))) as tdd3mb,[dbo].[comma_percent_format](convert(varchar,convert(decimal(15,2),tdd12mb))) as tdd12mb,[dbo].[comma_percent_format](convert(varchar,convert(decimal(15,2),tdd12ygb))) as tdd12ygb " +
                                            "  from #Cal_Avg_score	" +
                                                "	union	" +
                                                " select seq,SCRRG,convert(varchar,convert(decimal(15,2),tdd3m)) as tdd3m,convert(varchar,convert(decimal(15,2),tdd12m)) as tdd12m " +
                   " ,convert(varchar,convert(decimal(15,2),tdd12yg)) as tdd12yg,convert(varchar,convert(decimal(15,2),tdd3mw)) as tdd3mw " +
                   " ,convert(varchar,convert(decimal(15,2),tdd12mw)) as tdd12mw,convert(varchar,convert(decimal(15,2),tdd12ygw)) as tdd12ygw " +
                   " ,convert(varchar,convert(decimal(15,2),tdd3ma)) as tdd3ma,convert(varchar,convert(decimal(15,2),tdd12ma)) as tdd12ma " +
                   " ,convert(varchar,convert(decimal(15,2),tdd12yga)) as tdd12yga,convert(varchar,convert(decimal(15,2),tdd3mb)) as tdd3mb,convert(varchar,convert(decimal(15,2),tdd12mb)) as tdd12mb,convert(varchar,convert(decimal(15,2),tdd12ygb)) as tdd12ygb " +
                   "   from #Main_cutoff_table	" +
                                                "	union	" +
                   " select seq,SCRRG,convert(varchar,convert(decimal(15,2),tdd3m)) as tdd3m,convert(varchar,convert(decimal(15,2),tdd12m)) as tdd12m " +
                   " ,convert(varchar,convert(decimal(15,2),tdd12yg)) as tdd12yg,convert(varchar,convert(decimal(15,2),tdd3mw)) as tdd3mw " +
                   " ,convert(varchar,convert(decimal(15,2),tdd12mw)) as tdd12mw,convert(varchar,convert(decimal(15,2),tdd12ygw)) as tdd12ygw " +
                   " ,convert(varchar,convert(decimal(15,2),tdd3ma)) as tdd3ma,convert(varchar,convert(decimal(15,2),tdd12ma)) as tdd12ma " +
                   " ,convert(varchar,convert(decimal(15,2),tdd12yga)) as tdd12yga,convert(varchar,convert(decimal(15,2),tdd3mb)) as tdd3mb,convert(varchar,convert(decimal(15,2),tdd12mb)) as tdd12mb,convert(varchar,convert(decimal(15,2),tdd12ygb)) as tdd12ygb " +
                   "   from #Main_cutoff_Passing_Table ) Z  " +
                   "select * into #Format_NULL_CASE from ( select 0 as seq,'Score Range' as SCRRG,'3 Months' as tdd3m,'12 Months' as tdd12m,'12 Months' as tdd12yg,'3 Months' as tdd3mw " +
                   " ,'12 Months' as tdd12mw,'12 Months' as tdd12ygw,'3 Months' as tdd3ma,'12 Months' as tdd12ma,'12 Months' as tdd12yga,'3 Months' as tdd3mb " +
                   ",'12 Months' as tdd12mb,'12 Months' as tdd12ygb	union " +
                   " select seq,SCRRG ,case when tdd3m is null then '0' else tdd3m end as tdd3m	 " +
                   " ,case when tdd12m is null then '0' else tdd12m end as tdd12m	 " +
                   " ,case when tdd12yg is null then '0' else tdd12yg end as tdd12yg	 " +
                   " ,case when tdd3mw is null then '0' else tdd3mw end as tdd3mw	 " +
                   " ,case when tdd12mw is null then '0' else tdd12mw end as tdd12mw	 " +
                   " ,case when tdd12ygw is null then '0' else tdd12ygw end as tdd12ygw	 " +
                   " ,case when tdd3ma is null then '0' else tdd3ma end as tdd3ma	 " +
                   " ,case when tdd12ma is null then '0' else tdd12ma end as tdd12ma	 " +
                   " ,case when tdd12yga is null then '0' else tdd12yga end as tdd12yga	 " +
                   " ,case when tdd3mb is null then '0' else tdd3mb end as tdd3mb	 " +
                   " ,case when tdd12mb is null then '0' else tdd12mb end as tdd12mb	 " +
                   " ,case when tdd12ygb is null then '0' else tdd12ygb end as tdd12ygb from #PartA where (seq < 100 or seq=131)  union	 " +
                   " select seq,SCRRG ,case when tdd3m is null then '0.00' else tdd3m end as tdd3m	 " +
                   " ,case when tdd12m is null then '0.00' else tdd12m end as tdd12m	 " +
                   " ,case when tdd12yg is null then '0.00' else tdd12yg end as tdd12yg	 " +
                   " ,case when tdd3mw is null then '0.00' else tdd3mw end as tdd3mw	 " +
                   " ,case when tdd12mw is null then '0.00' else tdd12mw end as tdd12mw	 " +
                   " ,case when tdd12ygw is null then '0.00' else tdd12ygw end as tdd12ygw	 " +
                   " ,case when tdd3ma is null then '0.00' else tdd3ma end as tdd3ma	 " +
                   " ,case when tdd12ma is null then '0.00' else tdd12ma end as tdd12ma	 " +
                   " ,case when tdd12yga is null then '0.00' else tdd12yga end as tdd12yga	 " +
                   " ,case when tdd3mb is null then '0.00' else tdd3mb end as tdd3mb	 " +
                   " ,case when tdd12mb is null then '0.00' else tdd12mb end as tdd12mb	 " +
                   " ,case when tdd12ygb is null then '0.00' else tdd12ygb end as tdd12ygb	from #PartA  where seq=128 or seq=130 or seq=132 ) y order by seq	 " +
                   " select identity(int,1,1) as Seq, SCRRG,tdd3m,tdd12m,tdd12yg,tdd3mw,tdd12mw,tdd12ygw,tdd3ma,tdd12ma,tdd12yga,tdd3mb,tdd12mb,tdd12ygb " +
                    " into #z_Approval_Rate_Score " +
                    " from #Format_NULL_CASE order by seq " +
                    " select GRADE as scrrg" +
                    " , case when seq not in (1,6,7) then dbo.comma_format(tdd3m) else tdd3m end as tdd3m " +
                    " , case when seq not in (1,6,7) then dbo.comma_format(tdd12m) else tdd12m end as tdd12m " +
                    " , case when seq not in (1,6,7) then dbo.comma_format(tdd12yg) else tdd12yg end as tdd12yg " +
                    " , case when seq not in (1,6,7) then dbo.comma_format(tdd3mw) else tdd3mw end as tdd3mw " +
                    " , case when seq not in (1,6,7) then dbo.comma_format(tdd12mw) else tdd12mw end as tdd12mw " +
                    " , case when seq not in (1,6,7) then dbo.comma_format(tdd12ygw) else tdd12ygw end as tdd12ygw " +
                    " , case when seq not in (1,6,7) then dbo.comma_format(tdd3ma) else tdd3ma end as tdd3ma " +
                    " , case when seq not in (1,6,7) then dbo.comma_format(tdd12ma) else tdd12ma end as tdd12ma " +
                    " , case when seq not in (1,6,7) then dbo.comma_format(tdd12yga) else tdd12yga end as tdd12yga " +
                    " , case when seq not in (1,6,7) then dbo.comma_format(tdd3mb) else tdd3mb end as tdd3mb " +
                    " , case when seq not in (1,6,7) then dbo.comma_format(tdd12mb) else tdd12mb end as tdd12mb " +
                    " , case when seq not in (1,6,7) then dbo.comma_format(tdd12ygb) else tdd12ygb end as tdd12ygb " +
                    " into #Result " +
                    " from " +
                    " ( " +
                    " select '5' as seq, a.GRADE , convert(varchar,sum(a.tdd3m)) as tdd3m, convert(varchar,sum(a.tdd12m)) as tdd12m, convert(varchar,sum(a.tdd12yg)) as tdd12yg " +
                    " , convert(varchar,sum(a.tdd3mw)) as tdd3mw, convert(varchar,sum(a.tdd12mw)) as tdd12mw, convert(varchar,sum(a.tdd12ygw)) as tdd12ygw " +
                    " , convert(varchar,sum(a.tdd3ma)) as tdd3ma " +
                    " , convert(varchar,sum(a.tdd12ma)) as tdd12ma, convert(varchar,sum(a.tdd12yga)) as tdd12yga " +
                    " , convert(varchar,sum(a.tdd3mb)) as tdd3mb, convert(varchar,sum(a.tdd12mb)) as tdd12mb, convert(varchar,sum(a.tdd12ygb)) as tdd12ygb " +
                    " from " +
                    " ( " +
                    " select a.seq, a.scrrg  " +
                    " , convert(int,a.tdd3m) as tdd3m, convert(int,a.tdd12m) as tdd12m, convert(int,a.tdd12yg) as tdd12yg " +
                    " , convert(int,a.tdd3mw) as tdd3mw, convert(int,a.tdd12mw) as tdd12mw, convert(int,a.tdd12ygw) as tdd12ygw, convert(int,a.tdd3ma) as tdd3ma " +
                    " , convert(int,a.tdd12ma) as tdd12ma, convert(int,a.tdd12yga) as tdd12yga, convert(int,a.tdd3mb) as tdd3mb, convert(int,a.tdd12mb) as tdd12mb " +
                    " , convert(int,a.tdd12ygb) as tdd12ygb " +
                    " , a.MinScr, a.MaxScr " +
                    " , b.GRADE, b.MIN_SCORE, b.MAX_SCORE " +
                    " from " +
                    " ( " +
                    " select a.seq, a.scrrg " +
                    " , convert(varchar,convert(int,replace(a.tdd3m,',',''))-convert(int,replace(b.tdd3m,',',''))) as tdd3m " +
                    " , convert(varchar,convert(int,replace(a.tdd12m,',',''))-convert(int,replace(b.tdd12m,',',''))) as tdd12m " +
                    " , convert(varchar,convert(int,replace(a.tdd12yg,',',''))-convert(int,replace(b.tdd12yg,',',''))) as tdd12yg " +
                    " , convert(varchar,convert(int,replace(a.tdd3mw,',',''))) as tdd3mw " +
                    " , convert(varchar,convert(int, replace(a.tdd12mw,',',''))) as tdd12mw " +
                    " , convert(varchar,convert(int,replace(a.tdd12ygw,',',''))) as tdd12ygw " +
                    " , convert(varchar,convert(int,replace(a.tdd3ma,',',''))) as tdd3ma " +
                    " , convert(varchar,convert(int,replace(a.tdd12ma,',',''))) as tdd12ma " +
                    " , convert(varchar,convert(int,replace(a.tdd12yga,',',''))) as tdd12yga " +
                    " , convert(varchar,convert(int,replace(a.tdd3mb,',',''))) as tdd3mb " +
                    " , convert(varchar,convert(int,replace(a.tdd12mb,',',''))) as tdd12mb " +
                    " , convert(varchar,convert(int,replace(a.tdd12ygb,',',''))) as tdd12ygb " +
                    " , case when isnull(SUBSTRING(a.scrrg,1,3),'')='' then '999' else SUBSTRING(a.scrrg,1,3) end as MinScr " +
                    " , case when isnull(SUBSTRING(a.scrrg,5,3),'')='' then '999' else SUBSTRING(a.scrrg,5,3) end as MaxScr " +
                    " from #z_Approval_Rate_Score a " +
                    " inner join #z_Approval_Rate_Score b on a.seq=b.seq+1  " +
                    " and isnumeric(SUBSTRING(a.scrrg,1,1))=1 " +
                    " and isnumeric(SUBSTRING(b.scrrg,1,1))=1 " +
                    " ) a " +
                    " left join " +
                    " ( " +
                    " select * from ST_CUTOFF a " +
                    " WHERE CUTOFF_CD in " +
                    " (select CUTOFF_CD from LN_CUTOFF " +
                    " WHERE LOAN_CD = '{0}' and STYPE_CD = '{1}') " +
                    " ) b " +
                    " on a.MinScr>=convert(varchar,b.MIN_SCORE) " +
                    " and a.MaxScr<=case when b.max_score='1000' then '999' else convert(varchar,b.MAX_SCORE) end " +
                    " ) a " +
                    " group by a.GRADE " +
                    " union " +
                    " select case when scrrg='Score Range' then '1' when scrrg='Avg. Score' then '7' else '6' end as seq " +
                    " ,scrrg, tdd3m, tdd12m, tdd12yg, tdd3mw, tdd12mw, tdd12ygw, tdd3ma, tdd12ma, tdd12yga, tdd3mb, tdd12mb, tdd12ygb " +
                    " from #z_Approval_Rate_Score a " +
                    " where ISNUMERIC(substring(scrrg,1,1))=0  " +
                    " ) a " +
                    " order by seq,GRADE " +
                    "Update #Result         " +
                    "set tdd3m=convert(int,replace(tdd3m,',',''))         " +
                    ", tdd12m=convert(int,replace(tdd12m,',',''))         " +
                    ", tdd12yg=convert(int,replace(tdd12yg,',',''))         " +
                    ", tdd3mw=convert(int,replace(tdd3mw,',',''))         " +
                    ", tdd12mw=convert(int,replace(tdd12mw,',',''))         " +
                    ", tdd12ygw=convert(int,replace(tdd12ygw,',',''))         " +
                    ", tdd3ma=convert(int,replace(tdd3ma,',',''))         " +
                    ", tdd12ma=convert(int,replace(tdd12ma,',',''))         " +
                    ", tdd12yga=convert(int,replace(tdd12yga,',',''))         " +
                    ", tdd3mb=convert(int,replace(tdd3mb,',',''))         " +
                    ", tdd12mb=convert(int,replace(tdd12mb,',',''))         " +
                    ", tdd12ygb=convert(int,replace(tdd12ygb,',',''))         " +
                    "where scrrg in ('A','B','C','D')         " +
                    "Update #Result         " +
                    "set          " +
                    " tdd3mb=case when tdd3ma=0 then 0 else convert(decimal(18,2),convert(decimal(18,2),tdd3mb)/convert(decimal(18,2),tdd3ma)*100.0) end         " +
                    ", tdd12mb=case when tdd12ma=0 then 0 else convert(decimal(18,2),convert(decimal(18,2),tdd12mb)/convert(decimal(18,2),tdd12ma)*100.0) end         " +
                    ", tdd12ygb=case when tdd12yga=0 then 0 else convert(decimal(18,2),convert(decimal(18,2),tdd12ygb)/convert(decimal(18,2),tdd12yga)*100.0) end         " +
                    ", tdd3ma=case when tdd3m=0 then 0 else convert(decimal(18,2),convert(decimal(18,2),tdd3ma)/convert(decimal(18,2),tdd3m)*100.0) end         " +
                    ", tdd12ma=case when tdd12m=0 then 0 else convert(decimal(18,2),convert(decimal(18,2),tdd12ma)/convert(decimal(18,2),tdd12m)*100.0) end         " +
                    ", tdd12yga=case when tdd12yg=0 then 0 else convert(decimal(18,2),convert(decimal(18,2),tdd12yga)/convert(decimal(18,2),tdd12yg)*100.0) end         " +
                    ", tdd3mw=case when tdd3m=0 then 0 else convert(decimal(18,2),convert(decimal(18,2),tdd3mw)/convert(decimal(18,2),tdd3m)*100.0) end         " +
                    ", tdd12mw=case when tdd12m=0 then 0 else convert(decimal(18,2),convert(decimal(18,2),tdd12mw)/convert(decimal(18,2),tdd12m)*100.0) end         " +
                    ", tdd12ygw=case when tdd12yg=0 then 0 else convert(decimal(18,2),convert(decimal(18,2),tdd12ygw)/convert(decimal(18,2),tdd12yg)*100.0) end         " +
                    "where scrrg in ('A','B','C','D')         " +
                    " UPDATE #Result " +
                    " SET tdd3m=a.tdd3m_Cum " +
                    " , tdd12m=a.tdd12m_Cum " +
                    " , tdd12yg=a.tdd12yg_Cum " +
                    " from " +
                    " ( " +
                    " select scrrg, tdd3m, tdd12m, tdd12yg  " +
                    " , case when scrrg ='A' then (select dbo.comma_format(convert(varchar,SUM(convert(int,replace(tdd3m,',',''))))) from #Result where scrrg in ('A','B','C','D')) " +
                    " when scrrg ='B' then (select dbo.comma_format(convert(varchar,SUM(convert(int,replace(tdd3m,',',''))))) from #Result where scrrg in ('B','C','D')) " +
                    " when scrrg ='C' then (select dbo.comma_format(convert(varchar,SUM(convert(int,replace(tdd3m,',',''))))) from #Result where scrrg in ('C','D')) " +
                    " else tdd3m " +
                    "   end as tdd3m_Cum  " +
                    " , case when scrrg ='A' then (select dbo.comma_format(convert(varchar,SUM(convert(int,replace(tdd12m,',',''))))) from #Result where scrrg in ('A','B','C','D')) " +
                    " when scrrg ='B' then (select dbo.comma_format(convert(varchar,SUM(convert(int,replace(tdd12m,',',''))))) from #Result where scrrg in ('B','C','D')) " +
                    " when scrrg ='C' then (select dbo.comma_format(convert(varchar,SUM(convert(int,replace(tdd12m,',',''))))) from #Result where scrrg in ('C','D')) " +
                    " else tdd12m " +
                    "   end as tdd12m_Cum  " +
                    " , case when scrrg ='A' then (select dbo.comma_format(convert(varchar,SUM(convert(int,replace(tdd12yg,',',''))))) from #Result where scrrg in ('A','B','C','D')) " +
                    " when scrrg ='B' then (select dbo.comma_format(convert(varchar,SUM(convert(int,replace(tdd12yg,',',''))))) from #Result where scrrg in ('B','C','D')) " +
                    " when scrrg ='C' then (select dbo.comma_format(convert(varchar,SUM(convert(int,replace(tdd12yg,',',''))))) from #Result where scrrg in ('C','D')) " +
                    " else tdd12yg " +
                    "   end as tdd12yg_Cum  " +
                    " from #Result " +
                    " where scrrg in ('A','B','C','D') " +
                    " ) A " +
                    " WHERE #Result.scrrg=A.scrrg " +
                    "Update #Result         " +
                    "set #Result.tdd3m=case when b.tdd3m='0' then '0.00' else convert(decimal(18,2),(convert(decimal(18,2),replace(#Result.tdd3m,',',''))/convert(decimal(18,2),replace(b.tdd3m,',',''))*100.0)) end         " +
                    ",#Result.tdd12m=case when b.tdd12m='0' then '0.00' else convert(decimal(18,2),(convert(decimal(18,2),replace(#Result.tdd12m,',',''))/convert(decimal(18,2),replace(b.tdd12m,',',''))*100.0)) end         " +
                    ",tdd12yg=case when b.tdd12yg='0' then '0.00' else convert(decimal(18,2),(convert(decimal(18,2),replace(#Result.tdd12yg,',',''))/convert(decimal(18,2),replace(b.tdd12yg,',',''))*100.0)) end         " +
                    "from (select scrrg,tdd3m, tdd12m, tdd12yg from #Result where scrrg='No. of Loans') b         " +
                    "where #Result.scrrg in ('A','B','C','D')         " +
                    "and b.scrrg='No. of Loans'         " +
                    "SELECT scrrg" +
                    ",case when scrrg='Score Range' then tdd3m else dbo.comma_percent_format(convert(varchar,tdd3m)) end as tdd3m" +
                    ",case when scrrg='Score Range' then tdd12m else dbo.comma_percent_format(convert(varchar,tdd12m)) end as tdd12m" +
                    ",case when scrrg='Score Range' then tdd12yg else dbo.comma_percent_format(convert(varchar,tdd12yg)) end as tdd12yg" +
                    ",case when scrrg='Score Range' then tdd3mw else dbo.comma_percent_format(convert(varchar,tdd3mw)) end as tdd3mw" +
                    ",case when scrrg='Score Range' then tdd12mw else dbo.comma_percent_format(convert(varchar,tdd12mw)) end as tdd12mw" +
                    ",case when scrrg='Score Range' then tdd12ygw else dbo.comma_percent_format(convert(varchar,tdd12ygw)) end as tdd12ygw" +
                    ",case when scrrg='Score Range' then tdd3ma else dbo.comma_percent_format(convert(varchar,tdd3ma)) end as tdd3ma" +
                    ",case when scrrg='Score Range' then tdd12ma else dbo.comma_percent_format(convert(varchar,tdd12ma)) end as tdd12ma" +
                    ",case when scrrg='Score Range' then tdd12yga else dbo.comma_percent_format(convert(varchar,tdd12yga)) end as tdd12yga" +
                    ",case when scrrg='Score Range' then tdd3mb else dbo.comma_percent_format(convert(varchar,tdd3mb)) end as tdd3mb" +
                    ",case when scrrg='Score Range' then tdd12mb else dbo.comma_percent_format(convert(varchar,tdd12mb)) end as tdd12mb" +
                    ",case when scrrg='Score Range' then tdd12ygb else dbo.comma_percent_format(convert(varchar,tdd12ygb)) end as tdd12ygb " +
                    "FROM #Result" +
                    " drop table #z_Approval_Rate_Score " +
                    " drop table #Result " +
                                            "	drop table #Main_cutoff_table	drop table #Solution " +
                                            "	drop table #Cal_Avg_score " +
                                            "	drop table  #Final_Total_score " +
                                            "	drop table #Heading_Calculate_Score	" +
                                            "	drop table #Heading_Calculate_Score1	" +
                                            "	drop table #Heading_Calculate_Score2	" +
                                            "	drop table #Score_Cal_P4_1	" +
                                            "	drop table #Score_Cal_P4_2	" +
                                            "	drop table #Total_score_P4	" +
                                            "	drop table #No_of_Loan	 drop table  #Part1_Range " +
                                            "	drop table #Part1	drop table #PartA " +
                                            "	drop table  #Main_Table	" +
                                            "	drop table #tdd12mAgo	" +
                                            "	drop table #tdd12mAgo2	" +
                                            "	drop table #Heading	" +
                                            "	drop table #Main_cutoff2	drop table #Format_NULL_CASE" +
                                            "	drop table #Main_cutoff1	" +
                                            "	drop table #Main_cutoff3	" +
                                            "	drop table #Main_cutoff_Passing3	" +
                                            "	drop table #Main_cutoff_Passing2	" +
                                            "	drop table #Main_cutoff_Passing1	" +
                                            "	drop table #Main_cutoff_Passing_Table	"
            , ddlLoan.SelectedItem.Value, ddlSubType.SelectedItem.Value, mktqrf, mktqrt, start.ToString("yyyy-MM"), start12y.ToString("yyyy-MM"), end3m.ToString("yyyy-MM"), end12yg.ToString("yyyy-MM"), start12yg.ToString("yyyy-MM"));
                                        }
                                    }
                                    //-- End ถ้า Maket_CD To เป็น null 
                                }
                                else
                                {
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScript", "appPop('โปรดเลือกตัวเลือก','ค่า สินเชื่อย่อยถึง(TO) น้อยกว่า  สินเชื่อย่อยจาก(From)')", true);
                                }
                            }

                        }
                        //-- End Grade

                        DataTable dtb = new DataTable();

                        if (ddlModel.SelectedItem.Value == "7")
                        {
                            String strConnString = ConfigurationManager.ConnectionStrings["GSBSQLServer"].ConnectionString;
                            SqlConnection con = new SqlConnection(strConnString);
                            con.Open();
                            SqlCommand command = new SqlCommand("sp_FrontEnd_ApprovalRateReport", con);

                            command.Parameters.Add(new SqlParameter("@ddlLoan", ddlLoan.SelectedItem.Value));
                            command.Parameters.Add(new SqlParameter("@ddlSubType", ddlSubType.SelectedItem.Value));
                            command.Parameters.Add(new SqlParameter("@mktqrf", ddlMarketFrm.Text));
                            command.Parameters.Add(new SqlParameter("@mktqrt", ddlMarketTo.Text));
                            command.Parameters.Add(new SqlParameter("@start", start.ToString("yyyyMM")));
                            command.Parameters.Add(new SqlParameter("@start12y", start12y.ToString("yyyyMM")));
                            command.Parameters.Add(new SqlParameter("@end3m", end3m.ToString("yyyyMM")));
                            command.Parameters.Add(new SqlParameter("@end12yg", end12yg.ToString("yyyyMM")));
                            command.Parameters.Add(new SqlParameter("@start12yg", start12yg.ToString("yyyyMM")));
                            command.Parameters.Add(new SqlParameter("@rpttype", rpttype.SelectedValue));
                            command.Parameters.Add(new SqlParameter("@rptcttype", rptcttype.SelectedValue));

                            command.CommandType = CommandType.StoredProcedure;
                            command.CommandTimeout = 36000;
                            SqlDataAdapter da = new SqlDataAdapter(command);
                            da.Fill(dtb);
                            Session["dtb"] = dtb;
                            //ViewState["DataTable1"] = dtb;
                            gvTable.DataSource = dtb;
                            gvTable.DataBind();
                        }
                        else
                        {
                            Session["dtb"] = dtb;
                            //ViewState["DataTable1"] = conn.ExcuteSQL(appQuery1);
                            gvTable.DataSource = dtb;
                            gvTable.DataBind();
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
                    Session["sHeaderRpt3"] = "Market Code : " + ddlMarketFrm.SelectedItem.Text + " ถึง  " + ddlMarketTo.SelectedItem.Text;

                Session["sHeaderRpt4"] = "วันที่เปิดใบคำขอสินเชื่อ : " + (ddlOpenDateMonth.SelectedItem) + " " + ddlOpenDateYear.SelectedItem;

                if (rpttype.SelectedValue.ToString().Trim() == "SCORE" && rptcttype.SelectedValue.ToString().Trim() == "Number")
                {
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "aa", "window.open('ReportLayOut/frmPreviewFontRpt.aspx?ReportId=92SN');", true);
                }
                else if (rpttype.SelectedValue.ToString().Trim() == "SCORE" && rptcttype.SelectedValue.ToString().Trim() == "Percent")
                {
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "aa", "window.open('ReportLayOut/frmPreviewFontRpt.aspx?ReportId=92SP');", true);
                }
                else if (rpttype.SelectedValue.ToString().Trim() == "GRADE" && rptcttype.SelectedValue.ToString().Trim() == "Number")
                {
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "aa", "window.open('ReportLayOut/frmPreviewFontRpt.aspx?ReportId=92GN');", true);
                }
                else if (rpttype.SelectedValue.ToString().Trim() == "GRADE" && rptcttype.SelectedValue.ToString().Trim() == "Percent")
                {
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "aa", "window.open('ReportLayOut/frmPreviewFontRpt.aspx?ReportId=92GP');", true);
                }
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
            //txtOpenDate.Text = string.Empty;

        }
        protected void gvTable_PageChanging(object sender, GridViewPageEventArgs e)
        {
            gvTable.PageIndex = e.NewPageIndex;
            Search_Click(null, null);
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

            Session["sHeaderRpt4"] = "วันที่เปิดใบคำขอสินเชื่อ : " + (ddlOpenDateMonth.SelectedItem)+ " " + ddlOpenDateYear.SelectedItem;

            if (rpttype.SelectedValue.ToString().Trim() == "SCORE" && rptcttype.SelectedValue.ToString().Trim() == "Number")
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "aa", "window.open('ReportLayOut/frmPreviewFontRpt.aspx?ReportId=2SN');", true);
            }
            else if (rpttype.SelectedValue.ToString().Trim() == "SCORE" && rptcttype.SelectedValue.ToString().Trim() == "Percent")
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "aa", "window.open('ReportLayOut/frmPreviewFontRpt.aspx?ReportId=2SP');", true);
            }
            else if (rpttype.SelectedValue.ToString().Trim() == "GRADE" && rptcttype.SelectedValue.ToString().Trim() == "Number")
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "aa", "window.open('ReportLayOut/frmPreviewFontRpt.aspx?ReportId=2GN');", true);
            }
            else if (rpttype.SelectedValue.ToString().Trim() == "GRADE" && rptcttype.SelectedValue.ToString().Trim() == "Percent")
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "aa", "window.open('ReportLayOut/frmPreviewFontRpt.aspx?ReportId=2GP');", true);
            }

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
                HeaderCell.Text = "";
                HeaderGridRow.Cells.Add(HeaderCell);

                HeaderCell = new TableCell();
                HeaderCell.Text = "Through-the-door Population";
                HeaderCell.ColumnSpan = 3;
                HeaderGridRow.Cells.Add(HeaderCell);

                HeaderCell = new TableCell();
                HeaderCell.Text = "Reject as % TTD in range";
                HeaderCell.ColumnSpan = 3;
                HeaderGridRow.Cells.Add(HeaderCell);

                HeaderCell = new TableCell();
                HeaderCell.Text = "Approve as % decisioned in range";
                HeaderCell.ColumnSpan = 3;
                HeaderGridRow.Cells.Add(HeaderCell);

                HeaderCell = new TableCell();
                HeaderCell.Text = "Book as % of approved in range";
                HeaderCell.ColumnSpan = 3;
                HeaderGridRow.Cells.Add(HeaderCell);

                HeaderCell = new TableCell();
                HeaderCell.Text = "";
                HeaderGridRow2.Cells.Add(HeaderCell);

                HeaderCell = new TableCell();
                HeaderCell.Text = "Past";
                HeaderGridRow2.Cells.Add(HeaderCell);


                HeaderCell = new TableCell();
                HeaderCell.Text = "Past";
                HeaderGridRow2.Cells.Add(HeaderCell);


                HeaderCell = new TableCell();
                HeaderCell.Text = "Year ago";
                HeaderGridRow2.Cells.Add(HeaderCell);


                HeaderCell = new TableCell();
                HeaderCell.Text = "Past";
                HeaderGridRow2.Cells.Add(HeaderCell);


                HeaderCell = new TableCell();
                HeaderCell.Text = "Past";
                HeaderGridRow2.Cells.Add(HeaderCell);


                HeaderCell = new TableCell();
                HeaderCell.Text = "Year ago";
                HeaderGridRow2.Cells.Add(HeaderCell);

                HeaderCell = new TableCell();
                HeaderCell.Text = "Past";
                HeaderGridRow2.Cells.Add(HeaderCell);

                HeaderCell = new TableCell();
                HeaderCell.Text = "Past";
                HeaderGridRow2.Cells.Add(HeaderCell);


                HeaderCell = new TableCell();
                HeaderCell.Text = "Year ago";
                HeaderGridRow2.Cells.Add(HeaderCell);


                HeaderCell = new TableCell();
                HeaderCell.Text = "Past";
                HeaderGridRow2.Cells.Add(HeaderCell);


                HeaderCell = new TableCell();
                HeaderCell.Text = "Past";
                HeaderGridRow2.Cells.Add(HeaderCell);


                HeaderCell = new TableCell();
                HeaderCell.Text = "Year ago";
                HeaderGridRow2.Cells.Add(HeaderCell);

                gvTable.Controls[0].Controls.AddAt(0, HeaderGridRow);
                gvTable.Controls[0].Controls.AddAt(1, HeaderGridRow2);


            }
        }


        protected void gvTable_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }


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

        //-----------------------------------------------------------

    }
}