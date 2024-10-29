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

namespace GSB.Report.BackEnd
{
    public partial class EarlyPerformanceScoreReport : System.Web.UI.Page
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


            ddlOpenDateYearDTM.Items.Clear();

            for (int i = 0; i < 10; i++)
            {

                ddlOpenDateYearDTM.Items.Insert(i, new ListItem((ybud - i).ToString(), (ychis - i).ToString()));
            }

            ddlOpenDateMonthDTM.DataSource = GetMonth();
            ddlOpenDateMonthDTM.DataBind();
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
            DataTable dttb = (DataTable)ViewState["DataTable1"];

            string filename = string.Format("GSB-CS_EarlyPerformanceScoreReport_(Date_{0}-Time_{1}).xls", DateTime.Now.ToString("dd-MM-yyyy"), DateTime.Now.ToString("HH-mm-ss"));
            StringWriter tw = new StringWriter();
            DataGrid dgGrid = new DataGrid();

            tw.Write(string.Format("Early Performance Score Report"));
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
            tw.Write(string.Format("As of Date :| {0} ", ""));
            tw.Write(tw.NewLine);
            tw.Write(string.Format("Export Date :| {0} : {1} ", DateTime.Now.ToString("dd-MM-yyyy"), DateTime.Now.ToString("HH-mm-ss")));
            tw.Write(tw.NewLine);
            tw.Write("ผู้พิมพ์ " + ExportUtility.getCurrentUserId);
            tw.Write(tw.NewLine);

            int iColCount = new int();
            iColCount = dttb.Columns.Count;

            tw.Write("Early Performance|Bad : Ever 1-30|||Bad : Ever 31-60|||Bad : Ever 61-90");
            tw.Write(tw.NewLine);
            tw.Write("Score Range|Past 3 Months|Past 12 Months|Year Ago 12 Months|Past 3 Months|Past 12 Months|Year Ago 12 Months|Past 3 Months|Past 12 Months|Year Ago 12 Months");
            tw.Write(tw.NewLine);

            // Now write all the rows.
            foreach (DataRow dr in dttb.Rows)
            {
                for (int i = 0; i < iColCount; i++)
                {
                    //if (i != 1 && i != 2 && i != 3)
                    //{
                        if (!Convert.IsDBNull(dr[i]))
                        {
                            if (i != 0)
                            {
                                //tw.Write((Convert.ToDouble(dr[i].ToString())*100.0).ToString() + "%");
                                //tw.Write(dr[i].ToString().Replace(",", ""));
                                tw.Write(dr[i].ToString());
                            }
                            else
                            {
                                tw.Write(dr[i].ToString());
                            }
                        }
                        else
                        {
                            tw.Write("0.00");
                        }
                        if (i < iColCount - 1)
                        {
                            tw.Write("|");
                        }
                    //}
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
            DateTime start12yg1 = new DateTime();

            if (ddlLoan.SelectedIndex <= 0 || ddlSubType.SelectedIndex <= 0 || ddlMarketFrm.SelectedIndex <= 0 || ddlMarketTo.SelectedIndex <= 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScript", "appPop('โปรดเลือกตัวเลือก','กรุณาเลือกเงื่อนไขการค้นหาให้ครบถ้วน ')", true);
                return;
            }

            int monthstart = ddlOpenDateMonthDTM.SelectedIndex;

            if (monthstart < 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScript", "appPop('วันที่เปิดใบคำขอสินเชื่อไม่ถูกต้อง?','วันที่เปิดฯ เริ่มต้น ต้องน้อยกว่า หรือเท่ากับ วันที่เปิดฯ สุดท้าย')", true);
                return;
            }

            start = DateTime.Parse("01/" + (ddlOpenDateMonthDTM.SelectedIndex + 1).ToString("00") + "/" + ddlOpenDateYearDTM.SelectedValue.ToString());
            start12yg = start.AddMonths(-11);
            end3m = start.AddMonths(-2);
            end12yg = start.AddMonths(-23);
            start12yg1 = start.AddMonths(-12);

            //if (!(txtOpenDate.Text == ""))
            //{
            //    start = DateTime.Parse((txtOpenDate.Text.ToString().Substring(0, 6) + (Convert.ToInt32(txtOpenDate.Text.Substring(6, 4).ToString()) - 543).ToString()));
            //    start12yg = start.AddMonths(-11);
            //    end3m = start.AddMonths(-2);
            //    end12yg = start.AddMonths(-23);
            //    start12yg1 = start.AddMonths(-12);
            //}
            //else
            //{
            //    start = DateTime.Now;
            //    start12yg = start.AddMonths(-11);
            //    end3m = start.AddMonths(-2);
            //    end12yg = start.AddMonths(-23);
            //    start12yg1 = start.AddMonths(-12);
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
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScript", "appPop('โปรดเลือกตัวเลือก','โปรดเลือกประเภทสินเชื่อย่อยก่อน')", true);
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
                                appQuery1 = string.Format("	select   case when (SCORE - scr) < 0 then convert(varchar	" +
        "	,(scr + ((((SCORE + 1) - scr)/rint) -1 ) * rint)) + '-' + convert(varchar,((scr + ((((SCORE + 1 ) - scr)/rint)-1 ) * rint) + rint - 1))  	" +
        "	when (SCORE - scr) >= 0 then convert(varchar,(scr + ((SCORE - scr)/rint) * rint)) + '-' 	" +
        "	+ convert(varchar,((scr + ((SCORE - scr)/rint) * rint) + rint - 1)) end as scrdif 	" +
        "	,CBS_APP_NO,DW_BADMONTH_3M,DW_BADMONTH_12M,DW_BADMONTH_12YG	" +
        "	into #Heading	" +
        "	from	" +
        "	(	" +
        "	select [CBS_APP_NO],MODEL,LOAN_CD,SCORE,STYPE_CD,MARKET_CD,DW_SIGN_DATE,cut_off as scr,rint	" +
        "	,case when CBS_STATUS in (3,6,7) then 'Y' 	" +
        "	when  CBS_STATUS in (4,5) then 'N' when  CBS_STATUS in (1,2) OR CBS_STATUS is null then '<>' end as Approval_Flag	" +
        "	,case when LEFT(CBS_REASON_CD,1)='A' AND  CBS_STATUS in (4,5) then 'Y' 	" +
        "	when LEFT(CBS_REASON_CD,1)='R' AND  CBS_STATUS in (3,6,7) then 'Y' when LEFT(CBS_REASON_CD,1)='A' AND  CBS_STATUS in (3,6,7) then 'N'	" +
        "	when LEFT(CBS_REASON_CD,1)='R' AND  CBS_STATUS in (4,5) then 'N' else 'N/A' end as CONTRAST_FLAG,	" +
        "	case when DW_SIGN_DATE IS NULL THEN 'N' ELSE 'Y' END AS BOOKING_FLAG " +
        "	,LAST_UPDATE_DTM	" +
        "	,CASE WHEN convert(int,(substring(right([DW_SIGN_DATE],7),4,4) + substring(right([DW_SIGN_DATE],7),1,2)))	" +
        "	between  convert(int,(substring('{6}',4,4) + substring('{6}',1,2))) 	" +
        "	and convert(int,(substring('{4}',4,4) + substring('{4}',1,2))) THEN [DW_BADMONTH] ELSE NULL END AS [DW_BADMONTH_3M]	" +
        "	,CASE WHEN convert(int,(substring(right([DW_SIGN_DATE],7),4,4) + substring(right([DW_SIGN_DATE],7),1,2)))	" +
        "	between  convert(int,(substring('{5}',4,4) + substring('{5}',1,2))) 	" +
        "	and convert(int,(substring('{4}',4,4) + substring('{4}',1,2))) THEN [DW_BADMONTH] ELSE NULL END AS [DW_BADMONTH_12M]	" +
        "	,CASE WHEN convert(int,(substring(right([DW_SIGN_DATE],7),4,4) + substring(right([DW_SIGN_DATE],7),1,2)))	" +
        "	between  convert(int,(substring('{7}',4,4) + substring('{7}',1,2))) 	" +
        "	and convert(int,(substring('{8}',4,4) + substring('{8}',1,2))) THEN [DW_BADMONTH] ELSE NULL END AS [DW_BADMONTH_12YG]	" +
        "	from	" +
        "	(	" +
        "	SELECT a.*,b.score,b.model,(select cut_off from ST_SCORERANGE y where  y.ltype= '{0}' and y.lstype = '{1}' ) as cut_off	" +
        "	,(select rint from ST_SCORERANGE y where  y.ltype= '{0}' and y.lstype = '{1}' ) as rint	" +
        "	,CBS_REASON_CD	" +
        "	from [LN_APP] a,[LN_GRADE] b,[CBS_LN_APP] c 	" +
        "	where a.APP_NO = b.APP_NO  	" +
        "	and a.app_no = c.CBS_APP_NO 	" +
        "	and  LOAN_CD = '{0}' and STYPE_CD = '{1}' {2} {3} 	" +
        "	) lnapp	" +
        "	left join	" +
        "	(	" +
        "	select * 	" +
        "	from	" +
        "	(	" +
        "	select [CBS_APP_NO], DW_SIGN_DATE,LAST_UPDATE_DTM,[DW_BADMONTH]	" +
        "	from [DWH_BTFILE] A	" +
        //"	where A.[ISACTIVE]=0 ) DW_BFILE_T	" +
        //Tai 2013-12-26
        "	where A.[ISACTIVE]=0 and convert(decimal(18,2),DW_OUTSTAND)>0) DW_BFILE_T	" +
        //
        "	) gsb2T  on gsb2T.CBS_APP_NO = lnapp.APP_NO    	" +
        "	) Z	" +
        "	where BOOKING_FLAG='Y' and Approval_Flag='Y' and CONTRAST_FLAG='N'	" +

        "	select scrdif ,CBS_APP_NO,max(DW_BADMONTH_3M) as DW_BADMONTH_3M	" +
        "	,max(DW_BADMONTH_12M) as DW_BADMONTH_12M	" +
        "	,max(DW_BADMONTH_12YG) as DW_BADMONTH_12YG 	" +
        "	 into #Heading1	" +
        "	from #Heading	" +
        "	group by scrdif ,CBS_APP_NO	" +
        "	select distinct(scrdif) 	" +
        "	into #scrange	" +
        "	from #Heading order by scrdif 	" +
        "	select aa.scrdif	" +
        "	,SUM(BB.DPD1p_3M_N) AS DPD1p_3M_N 	" +
        "	,SUM(BB.DPD1p_3M_Y) AS DPD1p_3M_Y	" +
        "	,SUM(BB.DPD1p_12M_N) AS DPD1p_12M_N 	" +
        "	,SUM(BB.DPD1p_12M_Y) AS DPD1p_12M_Y	" +
        "	,SUM(DPD1p_12YG_Y) AS DPD1p_12YG_Y	" +
        "	,SUM(DPD1p_12YG_N) AS DPD1p_12YG_N	" +
        "	,SUM(BB.DPD30p_3M_N) AS DPD30p_3M_N 	" +
        "	,SUM(BB.DPD30p_3M_Y) AS DPD30p_3M_Y	" +
        "	,SUM(BB.DPD30p_12M_N) AS DPD30p_12M_N 	" +
        "	,SUM(BB.DPD30p_12M_Y) AS DPD30p_12M_Y	" +
        "	,SUM(DPD30p_12YG_Y) AS DPD30p_12YG_Y	" +
        "	,SUM(DPD30p_12YG_N) AS DPD30p_12YG_N	" +
        "	,SUM(BB.DPD60p_3M_N) AS DPD60p_3M_N 	" +
        "	,SUM(BB.DPD60p_3M_Y) AS DPD60p_3M_Y	" +
        "	,SUM(BB.DPD60p_12M_N) AS DPD60p_12M_N 	" +
        "	,SUM(BB.DPD60p_12M_Y) AS DPD60p_12M_Y	" +
        "	,SUM(DPD60p_12YG_Y) AS DPD60p_12YG_Y	" +
        "	,SUM(DPD60p_12YG_N) AS DPD60p_12YG_N	" +
        "	into #Main_DPD1p_3M	" +
        "	from	" +
        "	(	" +
        " select range_name as scrdif from st_scorernglst where range_cd = 	" +
        "	(select range_cd from [ST_SCORERANGE] where ltype= '{0}' and lstype = '{1}')  	" +
        "	) AA 	" +
        "	 left join 	" +
        "	(	" +
        "	select scrdif 	" +
        "	, case when DW_BADMONTH_3M<>1 THEN 1 ELSE 0 END AS 'DPD1p_3M_N'	" +
        //Tai 2014-01-02
        //"	, case when DW_BADMONTH_3m>=1 THEN 1 ELSE 0 END AS 'DPD1p_3M_Y'	" +
        "	, case when DW_BADMONTH_3m=1 THEN 1 ELSE 0 END AS 'DPD1p_3M_Y'	" +
        //
        "	, case when DW_BADMONTH_12M<>1 THEN 1 ELSE 0 END AS 'DPD1p_12M_N'	" +
        //Tai 2014-01-02
        //"	, case when DW_BADMONTH_12M>=1 THEN 1 ELSE 0 END AS 'DPD1p_12M_Y'	" +
        "	, case when DW_BADMONTH_12M=1 THEN 1 ELSE 0 END AS 'DPD1p_12M_Y'	" +
        //
        "	, case when DW_BADMONTH_12YG<>1 THEN 1 ELSE 0 END AS 'DPD1p_12YG_N'	" +
        //Tai 2014-01-02
        //"	, case when DW_BADMONTH_12YG>=1 THEN 1 ELSE 0 END AS 'DPD1p_12YG_Y'	" +
        "	, case when DW_BADMONTH_12YG=1 THEN 1 ELSE 0 END AS 'DPD1p_12YG_Y'	" +
        //
        "	, case when DW_BADMONTH_3M<>2 THEN 1 ELSE 0 END AS 'DPD30p_3M_N'	" +
        //Tai 2014-01-02        
        //"	, case when DW_BADMONTH_3m>=2 THEN 1 ELSE 0 END AS 'DPD30p_3M_Y'	" +
        "	, case when DW_BADMONTH_3m=2 THEN 1 ELSE 0 END AS 'DPD30p_3M_Y'	" +
        //
        "	, case when DW_BADMONTH_12M<>2 THEN 1 ELSE 0 END AS 'DPD30p_12M_N'	" +
        //Tai 2014-01-02        
        //"	, case when DW_BADMONTH_12M>=2 THEN 1 ELSE 0 END AS 'DPD30p_12M_Y'	" +
        "	, case when DW_BADMONTH_12M=2 THEN 1 ELSE 0 END AS 'DPD30p_12M_Y'	" +
        //
        "	, case when DW_BADMONTH_12YG<>2 THEN 1 ELSE 0 END AS 'DPD30p_12YG_N'	" +
        //Tai 2012-01-02
        //"	, case when DW_BADMONTH_12YG>=2 THEN 1 ELSE 0 END AS 'DPD30p_12YG_Y'	" +
        "	, case when DW_BADMONTH_12YG=2 THEN 1 ELSE 0 END AS 'DPD30p_12YG_Y'	" +
        //
        //faii 2014-01-03
        //"	, case when DW_BADMONTH_3M<3 THEN 1 ELSE 0 END AS 'DPD60p_3M_N'	" +
        //"	, case when DW_BADMONTH_3m>=3 THEN 1 ELSE 0 END AS 'DPD60p_3M_Y'	" +
        //"	, case when DW_BADMONTH_12M<3 THEN 1 ELSE 0 END AS 'DPD60p_12M_N'	" +
        //"	, case when DW_BADMONTH_12M>=3 THEN 1 ELSE 0 END AS 'DPD60p_12M_Y'	" +
        //"	, case when DW_BADMONTH_12YG<3 THEN 1 ELSE 0 END AS 'DPD60p_12YG_N'	" +
        //"	, case when DW_BADMONTH_12YG>=3 THEN 1 ELSE 0 END AS 'DPD60p_12YG_Y'	" +
        "	, case when DW_BADMONTH_3M<>3 THEN 1 ELSE 0 END AS 'DPD60p_3M_N'	" +
        "	, case when DW_BADMONTH_3m=3 THEN 1 ELSE 0 END AS 'DPD60p_3M_Y'	" +
        "	, case when DW_BADMONTH_12M<>3 THEN 1 ELSE 0 END AS 'DPD60p_12M_N'	" +
        "	, case when DW_BADMONTH_12M=3 THEN 1 ELSE 0 END AS 'DPD60p_12M_Y'	" +
        "	, case when DW_BADMONTH_12YG<>3 THEN 1 ELSE 0 END AS 'DPD60p_12YG_N'	" +
        "	, case when DW_BADMONTH_12YG=3 THEN 1 ELSE 0 END AS 'DPD60p_12YG_Y'	" +
        
        "	from #Heading1	" +
        "	) BB on BB.scrdif=AA.scrdif	" +
        "	Group by aa.scrdif	" +
        "	order by aa.scrdif	" +

        " select sum(DPD1p_3M_N) as DPD1p_3M_N,sum(DPD1p_3M_Y) as DPD1p_3M_Y	" +
        " ,sum(DPD1p_12M_N) as DPD1p_12M_N,sum(DPD1p_12M_Y) as DPD1p_12M_Y	" +
        " ,sum(DPD1p_12YG_N) as DPD1p_12YG_N,sum(DPD1p_12YG_Y) as DPD1p_12YG_Y	" +
        " ,sum(DPD30p_3M_N) as DPD30p_3M_N,sum(DPD30p_3M_Y) as DPD30p_3M_Y	" +
        " ,sum(DPD30p_12M_N) as DPD30p_12M_N,sum(DPD30p_12M_Y) as DPD30p_12M_Y	" +
        " ,sum(DPD30p_12YG_N) as DPD30p_12YG_N,sum(DPD30p_12YG_Y) as DPD30p_12YG_Y	" +
        " ,sum(DPD60p_3M_N) as DPD60p_3M_N,sum(DPD60p_3M_Y) as DPD60p_3M_Y	" +
        " ,sum(DPD60p_12M_N) as DPD60p_12M_N,sum(DPD60p_12M_Y) as DPD60p_12M_Y	" +
        " ,sum(DPD60p_12YG_N) as DPD60p_12YG_N,sum(DPD60p_12YG_Y) as DPD60p_12YG_Y into #Cal_Avg_bad_rate from #Main_DPD1p_3M	" +
        "	select scrdif	" +
        " , convert(decimal(15,3),DPD1p_3M_Y)*100.00/case when (DPD1p_3M_Y+DPD1p_3M_N)=0 then 1 else (DPD1p_3M_Y+DPD1p_3M_N) end as DPD1p_3M_pct		" +
        " ,convert(decimal(15,3),DPD1p_12M_Y)*100.00/case when (DPD1p_12M_Y+DPD1p_12M_N)=0 then 1 else (DPD1p_12M_Y+DPD1p_12M_N) end as DPD1p_12M_pct	" +
        " ,convert(decimal(15,3),DPD1p_12YG_Y)*100.00/case when (DPD1p_12YG_Y+DPD1p_12YG_N)=0 then 1 else (DPD1p_12YG_Y+DPD1p_12YG_N) end as DPD1p_12YG_pct	" +
        " ,convert(decimal(15,3),DPD30p_3M_Y)*100.00/case when (DPD30p_3M_Y+DPD30p_3M_N)=0 then 1 else (DPD30p_3M_Y+DPD30p_3M_N) end as DPD30p_3M_pct		" +
        " ,convert(decimal(15,3),DPD30p_12M_Y)*100.00/case when (DPD30p_12M_Y+DPD30p_12M_N)=0 then 1 else (DPD30p_12M_Y+DPD30p_12M_N) end as DPD30p_12M_pct		" +
        " ,convert(decimal(15,3),DPD30p_12YG_Y)*100.00/case when (DPD30p_12YG_Y+DPD30p_12YG_N)=0 then 1 else (DPD30p_12YG_Y+DPD30p_12YG_N) end as DPD30p_12YG_pct	" +
        " ,convert(decimal(15,3),DPD60p_3M_Y)*100.00/case when (DPD60p_3M_Y+DPD60p_3M_N)=0 then 1 else(DPD60p_3M_Y+DPD60p_3M_N) end as DPD60p_3M_pct			" +
        " ,convert(decimal(15,3),DPD60p_12M_Y)*100.00/case when (DPD60p_12M_Y+DPD60p_12M_N)=0 then 1 else (DPD60p_12M_Y+DPD60p_12M_N) end as DPD60p_12M_pct			" +
        " ,convert(decimal(15,3),DPD60p_12YG_Y)*100.00/case when (DPD60p_12YG_Y+DPD60p_12YG_N)=0 then 1 else (DPD60p_12YG_Y+DPD60p_12YG_N) end as DPD60p_12YG_pct	" +
        " INTO #FINAL_TABLE		" +
        "	from #Main_DPD1p_3M	" +

        "	SELECT scrdif AS category	" +
        "	,DPD1p_3M_pct AS dt3mBM1	" +
        "	,DPD1p_12M_pct AS dt12mBM1	" +
        "	,DPD1p_12YG_pct AS dt12mygBM1	" +
        "	,DPD30p_3M_pct AS dt3mBM2	" +
        "	,DPD30p_12M_pct AS dt12mBM2	" +
        "	,DPD30p_12YG_pct AS dt12mygBM2	" +
        "	,DPD60p_3M_pct AS dt3mBM3	" +
        "	,DPD60p_12M_pct AS dt12mBM3	" +
        "	,DPD60p_12YG_pct AS dt12mygBM3	" +
        "	into #FINAL_TABLE_AVG FROM #FINAL_TABLE	" +

        " select 'Total Loan' AS category	" +
        "	,convert(varchar,case when (sum(DPD1p_3M_N)+sum(DPD1p_3M_Y)) is null then 0 else (sum(DPD1p_3M_N)+sum(DPD1p_3M_Y)) end) as dt3mBM1	" +
        "	,convert(varchar,case when (sum(DPD1p_12M_N)+sum(DPD1p_12M_Y)) is null then 0 else (sum(DPD1p_12M_N)+sum(DPD1p_12M_Y))end) as dt12mBM1	" +
        "	,convert(varchar,case when (sum(DPD1p_12YG_N)+sum(DPD1p_12YG_Y)) is null then 0 else (sum(DPD1p_12YG_N)+sum(DPD1p_12YG_Y))end) as dt12mygBM1	" +
        "	,convert(varchar,case when (sum(DPD30p_3M_N)+sum(DPD30p_3M_Y)) is null then 0 else (sum(DPD30p_3M_N)+sum(DPD30p_3M_Y)) end) as dt3mBM2	" +
        "	,convert(varchar,case when (sum(DPD30p_12M_N)+sum(DPD30p_12M_Y)) is null then 0 else (sum(DPD30p_12M_N)+sum(DPD30p_12M_Y)) end) as dt12mBM2	" +
        "	,convert(varchar,case when (sum(DPD30p_12YG_N)+sum(DPD30p_12YG_Y)) is null then 0 else (sum(DPD30p_12YG_N)+sum(DPD30p_12YG_Y)) end) as dt12mygBM2	" +
        "	,convert(varchar,case when (sum(DPD60p_3M_N)+sum(DPD60p_3M_Y)) is null then 0 else (sum(DPD60p_3M_N)+sum(DPD60p_3M_Y)) end) as dt3mBM3	" +
        "	,convert(varchar,case when (sum(DPD60p_12M_N)+sum(DPD60p_12M_Y)) is null then 0 else (sum(DPD60p_12M_N)+sum(DPD60p_12M_Y)) end) as dt12mBM3	" +
        "	,convert(varchar,case when (sum(DPD60p_12YG_N)+sum(DPD60p_12YG_Y)) is null then 0 else (sum(DPD60p_12YG_N)+sum(DPD60p_12YG_Y)) end) as dt12mygBM3 	" +
        " into #Total_line from #Main_DPD1p_3M	" +
        "	select category	,convert(varchar,convert(decimal(15,2),dt3mBM1)) as dt3mBM1,convert(varchar,convert(decimal(15,2),dt12mBM1)) as dt12mBM1,convert(varchar,convert(decimal(15,2),dt12mygBM1)) as dt12mygBM1	" +
        "	,convert(varchar,convert(decimal(15,2),dt3mBM2)) as dt3mBM2	,convert(varchar,convert(decimal(15,2),dt12mBM2)) as dt12mBM2	" +
        "	,convert(varchar,convert(decimal(15,2),dt12mygBM2)) as dt12mygBM2	,convert(varchar,convert(decimal(15,2),dt3mBM3)) as dt3mBM3	" +
        "	,convert(varchar,convert(decimal(15,2),dt12mBM3)) as dt12mBM3	,convert(varchar,convert(decimal(15,2),dt12mygBM3)) as dt12mygBM3		" +
        "	from	(	select category,dt3mBM1,dt12mBM1,dt12mygBM1,dt3mBM2,dt12mBM2,dt12mygBM2,dt3mBM3,dt12mBM3,dt12mygBM3		" +
        "	from #FINAL_TABLE_AVG 	 union	 	" +
        " select 'Average Bad Rate' AS category" + 
        " ,case when isnull((DPD1p_3M_Y+DPD1p_3M_N),0)=0 then 0.00 else convert(decimal(15,3),DPD1p_3M_Y)/(DPD1p_3M_Y+DPD1p_3M_N)*100.00 end AS dt3mBM1	" +
        " ,case when isnull((DPD1p_12M_Y+DPD1p_12M_N),0)=0 then 0.00 else convert(decimal(15,3),DPD1p_12M_Y)/(DPD1p_12M_Y+DPD1p_12M_N)*100.00 end AS dt12mBM1" +
        " ,case when isnull((DPD1p_12YG_Y+DPD1p_12YG_N),0)=0 then 0.00 else convert(decimal(15,3),DPD1p_12YG_Y)/(DPD1p_12YG_Y+DPD1p_12YG_N)*100.00 end AS dt12mygBM1	" +
        " ,case when isnull((DPD30p_3M_Y+DPD30p_3M_N),0)=0 then 0.00 else convert(decimal(15,3),DPD30p_3M_Y)/(DPD30p_3M_Y+DPD30p_3M_N)*100.00 end AS dt3mBM2" +
        " ,case when isnull((DPD30p_12M_Y+DPD30p_12M_N),0)=0 then 0.00 else convert(decimal(15,3),DPD30p_12M_Y)/(DPD30p_12M_Y+DPD30p_12M_N)*100.00 end AS dt12mBM2 " +
        " ,case when isnull((DPD30p_12YG_Y+DPD30p_12YG_N),0)=0 then 0.00 else convert(decimal(15,3),DPD30p_12YG_Y)/(DPD30p_12YG_Y+DPD30p_12YG_N)*100.00 end AS dt12mygBM2 " +
        " ,case when isnull((DPD60p_3M_Y+DPD60p_3M_N),0)=0 then 0.00 else convert(decimal(15,3),DPD60p_3M_Y)/(DPD60p_3M_Y+DPD60p_3M_N)*100.00 end AS dt3mBM3 " +
        " ,case when isnull((DPD60p_12M_Y+DPD60p_12M_N),0)=0 then 0.00 else convert(decimal(15,3),DPD60p_12M_Y)/(DPD60p_12M_Y+DPD60p_12M_N)*100.00 end AS dt12mBM3" +
        " ,case when isnull((DPD60p_12YG_Y+DPD60p_12YG_N),0)=0 then 0.00 else convert(decimal(15,3),DPD60p_12YG_Y)/(DPD60p_12YG_Y+DPD60p_12YG_N) *100.00 end AS dt12mygBM3" +
        " from  #Cal_Avg_bad_rate	" +
        "	) A	union 	select category	,dbo.comma_format(convert(varchar,dt3mBM1)) as dt3mBM1	" +
        "	,dbo.comma_format(convert(varchar,dt12mBM1)) as dt12mBM1	,dbo.comma_format(convert(varchar,dt12mygBM1)) as dt12mygBM1	" +
        "	,dbo.comma_format(convert(varchar,dt3mBM2)) as dt3mBM2	,dbo.comma_format(convert(varchar,dt12mBM2)) as dt12mBM2	" +
        "	,dbo.comma_format(convert(varchar,dt12mygBM2)) as dt12mygBM2	,dbo.comma_format(convert(varchar,dt3mBM3)) as dt3mBM3	" +
        "	,dbo.comma_format(convert(varchar,dt12mBM3)) as dt12mBM3	,dbo.comma_format(convert(varchar,dt12mygBM3)) as dt12mygBM3	 from #Total_line	" +
        "	drop table #FINAL_TABLE	" +
        "	drop table #Heading	" +
        "	drop table #Heading1	" +
        "	drop table #scrange DROP TABLE #FINAL_TABLE_AVG  DROP TABLE #Total_line	" +
        "	DROP TABLE #Main_DPD1p_3M	"
                           , ddlLoan.SelectedItem.Value, ddlSubType.SelectedItem.Value, mktqrf, mktqrt, start.ToString("MM/yyyy"), start12yg.ToString("MM/yyyy"), end3m.ToString("MM/yyyy"), end12yg.ToString("MM/yyyy"), start12yg1.ToString("MM/yyyy"));


                                    DataTable dt = new DataTable();

                                    if (ddlModel.SelectedItem.Value == "7")
                                    {
                                        String strConnString = ConfigurationManager.ConnectionStrings["GSBSQLServer"].ConnectionString;
                                        SqlConnection con = new SqlConnection(strConnString);
                                        con.Open();
                                        SqlCommand command;
                                        command = new SqlCommand("sp_BackEnd_EarlyPerformanceScoreReport", con);

                                        command.Parameters.Add(new SqlParameter("@start", start.ToString("yyyyMM")));
                                        command.Parameters.Add(new SqlParameter("@start12yg", start12yg.ToString("yyyyMM")));
                                        command.Parameters.Add(new SqlParameter("@end3m", end3m.ToString("yyyyMM")));
                                        command.Parameters.Add(new SqlParameter("@end12yg", end12yg.ToString("yyyyMM")));
                                        command.Parameters.Add(new SqlParameter("@start12yg1", start12yg1.ToString("yyyyMM")));
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
                //Generate report header
                Session["sHeaderRpt1"] = "ประเภทโมเดล : " + ddlModel.SelectedItem.Text;
                Session["sHeaderRpt2"] = "ประเภทสินเชื่อ / ประเภทสินเชื่อย่อย : " + ddlLoan.SelectedItem.Text + " / " + ddlSubType.SelectedItem.Text;

                if (ddlMarketFrm.SelectedIndex == 0)
                    Session["sHeaderRpt3"] = "Market Code : " + ddlMarketFrm.SelectedItem.Text;
                else
                    Session["sHeaderRpt3"] = "Market Code : " + ddlMarketFrm.SelectedItem.Text + " ถึง " + ddlMarketTo.SelectedItem.Text;

                Session["sHeaderRpt4"] = "วันที่เซ็นสัญญา : " + (ddlOpenDateMonthDTM.SelectedItem) + " " + ddlOpenDateYearDTM.SelectedItem;

                //Regenerate data for report
                DataTable dt = (DataTable)Session["dtb"];
                Session["rptData"] = dt;
                ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "aa", "window.open('ReportLayOut/frmPreviewBackRpt.aspx?ReportId=92');", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScript", "appPop('โปรดเลือกตัวเลือก','กรุณากดปุ่มแสดงข้อมูลก่อน')", true);
                return;
            }
        }

        //Generate Report
        protected void PrintReport_Click(object sender, EventArgs e)
        {
            if (gvTable.Rows.Count > 0)
            {
                Label userFirstname = (Label)Master.FindControl("userFirstname");
                Session["userFirstname"] = userFirstname.Text;
                print_preview();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScript", "appPop('โปรดเลือกตัวเลือก','กรุณากดปุ่มแสดงข้อมูลก่อน')", true);
                return;
            }

        }

        private void print_preview()
        {
            //Generate report header
            Session["sHeaderRpt1"] = "ประเภทโมเดล : " + ddlModel.SelectedItem.Text;
            Session["sHeaderRpt2"] = "ประเภทสินเชื่อ / ประเภทสินเชื่อย่อย : " + ddlLoan.SelectedItem.Text + " / " + ddlSubType.SelectedItem.Text;

            if (ddlMarketFrm.SelectedIndex == 0)
                Session["sHeaderRpt3"] = "Market Code : " + ddlMarketFrm.SelectedItem.Text;
            else
                Session["sHeaderRpt3"] = "Market Code : " + ddlMarketFrm.SelectedItem.Text + " ถึง " + ddlMarketTo.SelectedItem.Text;

            Session["sHeaderRpt4"] = "วันที่เซ็นสัญญา : " + (ddlOpenDateMonthDTM.SelectedItem) + " " + ddlOpenDateYearDTM.SelectedItem;


            //Regenerate data for report
            DataTable dt = (DataTable)Session["dtb"];
            Session["rptData"] = dt;
            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "aa", "window.open('ReportLayOut/frmPreviewBackRpt.aspx?ReportId=2');", true);
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

        protected void gvTable_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                GridView HeaderGrid = (GridView)sender;

                e.Row.Cells.Clear();


                GridViewRow HeaderGridRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);
                GridViewRow HeaderGridRow2 = new GridViewRow(1, 0, DataControlRowType.Header, DataControlRowState.Insert);
                GridViewRow HeaderGridRow3 = new GridViewRow(2, 0, DataControlRowType.Header, DataControlRowState.Insert);

                TableCell HeaderCell = new TableCell();
                HeaderCell.Text = "Early Performance";
                HeaderGridRow.Cells.Add(HeaderCell);

                HeaderCell = new TableCell();
                HeaderCell.Text = "Bad Ever : <= 1 Month";
                HeaderCell.ColumnSpan = 6;
                HeaderGridRow.Cells.Add(HeaderCell);

                HeaderCell = new TableCell();
                HeaderCell.Text = "Bad Ever : >1-3 Months";
                HeaderCell.ColumnSpan = 6;
                HeaderGridRow.Cells.Add(HeaderCell);

                HeaderCell = new TableCell();
                HeaderCell.Text = "Cutoff Score";
                HeaderCell.RowSpan = 2;
                HeaderGridRow2.Cells.Add(HeaderCell);

                HeaderCell = new TableCell();
                HeaderCell.Text = "Past 3 Months";
                HeaderCell.ColumnSpan = 2;
                HeaderGridRow2.Cells.Add(HeaderCell);


                HeaderCell = new TableCell();
                HeaderCell.Text = "Past 12 Months";
                HeaderCell.ColumnSpan = 2;
                HeaderGridRow2.Cells.Add(HeaderCell);


                HeaderCell = new TableCell();
                HeaderCell.Text = "Year Ago 12 Months";
                HeaderCell.ColumnSpan = 2;
                HeaderGridRow2.Cells.Add(HeaderCell);


                HeaderCell = new TableCell();
                HeaderCell.Text = "Past 3 Months";
                HeaderCell.ColumnSpan = 2;
                HeaderGridRow2.Cells.Add(HeaderCell);


                HeaderCell = new TableCell();
                HeaderCell.Text = "Past 12 Months";
                HeaderCell.ColumnSpan = 2;
                HeaderGridRow2.Cells.Add(HeaderCell);

                HeaderCell = new TableCell();
                HeaderCell.Text = "Year Ago 12 Months";
                HeaderCell.ColumnSpan = 2;
                HeaderGridRow2.Cells.Add(HeaderCell);

                HeaderCell = new TableCell();
                HeaderCell.Text = "Account";
                HeaderGridRow3.Cells.Add(HeaderCell);

                HeaderCell = new TableCell();
                HeaderCell.Text = "%";
                HeaderGridRow3.Cells.Add(HeaderCell);

                HeaderCell = new TableCell();
                HeaderCell.Text = "Account";
                HeaderGridRow3.Cells.Add(HeaderCell);

                HeaderCell = new TableCell();
                HeaderCell.Text = "%";
                HeaderGridRow3.Cells.Add(HeaderCell);

                HeaderCell = new TableCell();
                HeaderCell.Text = "Account";
                HeaderGridRow3.Cells.Add(HeaderCell);

                HeaderCell = new TableCell();
                HeaderCell.Text = "%";
                HeaderGridRow3.Cells.Add(HeaderCell);

                HeaderCell = new TableCell();
                HeaderCell.Text = "Account";
                HeaderGridRow3.Cells.Add(HeaderCell);

                HeaderCell = new TableCell();
                HeaderCell.Text = "%";
                HeaderGridRow3.Cells.Add(HeaderCell);

                HeaderCell = new TableCell();
                HeaderCell.Text = "Account";
                HeaderGridRow3.Cells.Add(HeaderCell);

                HeaderCell = new TableCell();
                HeaderCell.Text = "%";
                HeaderGridRow3.Cells.Add(HeaderCell);

                HeaderCell = new TableCell();
                HeaderCell.Text = "Account";
                HeaderGridRow3.Cells.Add(HeaderCell);

                HeaderCell = new TableCell();
                HeaderCell.Text = "%";
                HeaderGridRow3.Cells.Add(HeaderCell);

                gvTable.Controls[0].Controls.AddAt(0, HeaderGridRow);
                gvTable.Controls[0].Controls.AddAt(1, HeaderGridRow2);
                gvTable.Controls[0].Controls.AddAt(2, HeaderGridRow3);


            }
        }

        protected void gvTable_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        #endregion Protected Method

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