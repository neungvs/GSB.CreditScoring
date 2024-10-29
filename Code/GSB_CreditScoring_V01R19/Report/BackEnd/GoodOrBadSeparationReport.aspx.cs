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
    public partial class GoodOrBadSeparationReport : System.Web.UI.Page
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
            ddlOpenDateYearend.Items.Clear();

            for (int i = 0; i < 10; i++)
            {
                ddlOpenDateYear.Items.Insert(i, new ListItem((ybud - i).ToString(), (ychis - i).ToString()));
                ddlOpenDateYearend.Items.Insert(i, new ListItem((ybud - i).ToString(), (ychis - i).ToString()));
                ddlOpenDateYearDTM.Items.Insert(i, new ListItem((ybud - i).ToString(), (ychis - i).ToString()));
                ddlOpenDateYearDTMend.Items.Insert(i, new ListItem((ybud - i).ToString(), (ychis - i).ToString()));
            }

            ddlOpenDateMonth.Items.Clear();
            ddlOpenDateMonthend.Items.Clear();

            ddlOpenDateMonth.DataSource = GetMonth();
            ddlOpenDateMonth.DataBind();
            ddlOpenDateMonthend.DataSource = GetMonth();
            ddlOpenDateMonthend.DataBind();

            ddlOpenDateMonthDTM.DataSource = GetMonth();
            ddlOpenDateMonthDTM.DataBind();
            ddlOpenDateMonthDTMend.DataSource = GetMonth();
            ddlOpenDateMonthDTMend.DataBind();

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
            //DataTable dttb2 = (DataTable)ViewState["DataTable2"];
            //DataTable dttb3 = (DataTable)ViewState["DataTable3"];


            string filename = string.Format("GSB-CS_GoodOrBadSeparationReport_(Date_{0}-Time_{1}).xls", DateTime.Now.ToString("dd-MM-yyyy"), DateTime.Now.ToString("HH-mm-ss"));
            StringWriter tw = new StringWriter();
            DataGrid dgGrid = new DataGrid();

            tw.Write(string.Format("Good Or Bad Separation Report"));
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
            tw.Write(string.Format("As Of Date :| {0} ", ""));
            tw.Write(tw.NewLine);
            tw.Write(string.Format("Export Date :| {0} : {1} ", DateTime.Now.ToString("dd-MM-yyyy"), DateTime.Now.ToString("HH-mm-ss")));
            tw.Write(tw.NewLine);
            tw.Write("ผู้พิมพ์ " + ExportUtility.getCurrentUserId);
            tw.Write(tw.NewLine);

            int iColCount = new int();
            iColCount = dttb.Columns.Count;
            int iColCount2 = new int();
            iColCount2 = dttb.Columns.Count;
            int iColCount3 = new int();
            iColCount3 = dttb.Columns.Count;

            tw.Write("Score Range| Current Validation Sample (%)||||Development Sample (%)|||"); 
            tw.Write(tw.NewLine);            
            tw.Write("|%Cum_G|%Cum_B|Sep_BG|%BadRate(Current)|%Cum_G(Dev)|%Cum_B(Dev)|Sep_BG(Dev)|%BadRate");
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

            //foreach (DataRow dr in dttb2.Rows)
            //{
            //    for (int i = 0; i < iColCount2; i++)
            //    {
            //        if (!Convert.IsDBNull(dr[i]))
            //        {
            //            tw.Write(dr[i].ToString());
            //        }
            //        else
            //        {
            //            tw.Write("0");
            //        }
            //        if (i < iColCount - 1)
            //        {
            //            tw.Write(",");
            //        }
            //    }
            //    tw.Write(tw.NewLine);


            //}

            //foreach (DataRow dr in dttb3.Rows)
            //{
            //    for (int i = 0; i < iColCount3; i++)
            //    {
            //        if (!Convert.IsDBNull(dr[i]))
            //        {
            //            tw.Write(dr[i].ToString());
            //        }
            //        else
            //        {
            //            tw.Write("0");
            //        }
            //        if (i < iColCount - 1)
            //        {
            //            tw.Write(",");
            //        }
            //    }
            //    tw.Write(tw.NewLine);
            //}

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
            DateTime APPDATE_Start = new DateTime();
            DateTime APPDATE_End = new DateTime();


            if (ddlLoan.SelectedIndex <= 0 || ddlSubType.SelectedIndex <= 0 || ddlMarketFrm.SelectedIndex <= 0 || ddlMarketTo.SelectedIndex <= 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScript", "appPop('โปรดเลือกตัวเลือก','กรุณาเลือกเงื่อนไขการค้นหาให้ครบถ้วน ')", true);
                return;
            }

            int monthstart = Convert.ToInt16(ddlOpenDateYear.SelectedValue) * 100 + ddlOpenDateMonth.SelectedIndex;
            int monthend = Convert.ToInt16(ddlOpenDateYearend.SelectedValue) * 100 + ddlOpenDateMonthend.SelectedIndex;
            int monthstartDTM = Convert.ToInt16(ddlOpenDateYearDTM.SelectedValue) * 100 + ddlOpenDateMonthDTM.SelectedIndex;
            int monthendDTM = Convert.ToInt16(ddlOpenDateYearDTMend.SelectedValue) * 100 + ddlOpenDateMonthDTMend.SelectedIndex;

            if (monthstart > monthend || monthstartDTM > monthendDTM)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScript", "appPop('วันที่เปิดใบคำขอสินเชื่อไม่ถูกต้อง?','เลือกวันที่เปิดใบคำขอไม่ถูกต้อง')", true);
                return;
            }


            //if (!(txtOpenDate.Text == ""))
            //{
            //    start12yg = DateTime.Parse((txtOpenDate.Text.ToString().Substring(0, 6) + (Convert.ToInt32(txtOpenDate.Text.Substring(6, 4).ToString()) - 543).ToString()));
            //    start = DateTime.Parse((txtCloseDate.Text.ToString().Substring(0, 6) + (Convert.ToInt32(txtCloseDate.Text.Substring(6, 4).ToString()) - 543).ToString()));
            //    APPDATE_Start = DateTime.Parse((TextSignOpenDate.Text.ToString().Substring(0, 6) + (Convert.ToInt32(TextSignOpenDate.Text.Substring(6, 4).ToString()) - 543).ToString()));
            //    APPDATE_End = DateTime.Parse((TextSignCloseDate.Text.ToString().Substring(0, 6) + (Convert.ToInt32(TextSignCloseDate.Text.Substring(6, 4).ToString()) - 543).ToString()));


            //    //start12yg = start.AddMonths(-12);
            //    //end3m = DateTime.Parse((txtCloseDate.Text.ToString().Substring(0, 6) + (Convert.ToInt32(txtCloseDate.Text.Substring(6, 4).ToString()) - 543).ToString()));
            //    //                end3m = start.AddMonths(-3);
            //    //end12yg = start.AddMonths(-24);
            //}
            //else
            //{
            //    start = DateTime.Now;
            //    start12yg = start.AddMonths(-12);
            //    end3m = start.AddMonths(-3);
            //    end12yg = start.AddMonths(-24);
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
                                appQuery1 = string.Format(" select [CBS_APP_NO],convert(varchar,Max_Date, 103) as [Max_LAST_UPDATE_DTM]	" +
        "	into #Max_LAST_UPDATE_DTM	" +
        "	from	" +
        "	(	" +
        "	SELECT [CBS_APP_NO],max(convert(datetime,substring([LAST_UPDATE_DTM],7,4)+'-'+substring([LAST_UPDATE_DTM],4,2)+'-'+substring([LAST_UPDATE_DTM],1,2))) as Max_Date	" +
        "	  FROM DWH_BTFILE A	" +
        "	  where 	" +
        "	convert(int,(substring(right([LAST_UPDATE_DTM],7),4,4) + substring(right([LAST_UPDATE_DTM],7),1,2)))	" +
        "	between   convert(int,(substring('{5}',1,4) + substring('{5}',6,2))) 	" +
        "	and convert(int,(substring('{4}',1,4) + substring('{4}',6,2)))	" +
        "	AND EXISTS " +
        "(SELECT APP_NO FROM LN_APP B WHERE  LOAN_CD = '{0}' and STYPE_CD = '{1}'  {2} {3}  AND A.CBS_APP_NO=B.APP_NO	)" +
        "	group by [CBS_APP_NO]	" +
        "	) a	" +
        "	select *	" +
        "	into #Good_Bad_Temp	" +
        "	from	" +
        "	(	" +
        "	select range_name as scrange from st_scorernglst where range_cd =  	" +
        "	                        (select range_cd from ST_SCORERANGE where ltype= '{0}' and lstype = '{1}') 	" +
        "	) defrng 	" +
        "	left join	" +
        "	(                	" +
        "	select scrdif, sum(Normal_Flag) as PL,sum(Bad_Flag) as NPL 	" +
        "	from	" +
        "	(	" +
        "	select DISTINCT gsb2T.CBS_APP_NO, LOAN_CD,STYPE_CD,MARKET_CD,score,CBS_STATUS,ISACTIVE,DW_BADMONTH	" +
        "	,case when DW_BADMONTH >= 3 then 1 else 0 end as Bad_Flag,	" +
        "	case when (SCORE - scr) < 0 then convert(varchar,(scr + ((((SCORE + 1) - scr)/rint) -1 ) * rint)) + '-' + convert(varchar,((scr + ((((SCORE + 1 ) - scr)/rint)-1 ) * rint) + rint - 1))  	" +
        "	when (SCORE - scr) >= 0 then convert(varchar,(scr + ((SCORE - scr)/rint) * rint)) + '-' + convert(varchar,((scr + ((SCORE - scr)/rint) * rint) + rint - 1))   	" +
        "	end as scrdif,grade	" +
        "	,case when DW_BADMONTH < 3 then 1 else 0 end as Normal_Flag	" +
        "	from	" +
        "	(	" +
        "	SELECT a.*,b.score,(select cut_off from ST_SCORERANGE y where  y.ltype= '{0}' and y.lstype = '{1}' ) as scr,   	" +
        "	                        (select rint from ST_SCORERANGE y where  y.ltype= '{0}' and y.lstype = '{1}' ) as rint,b.[GRADE] grade	" +
        "	from LN_APP a,LN_GRADE b,CBS_LN_APP c 	" +
        "	where a.APP_NO = b.APP_NO  and a.app_no = c.CBS_APP_NO 	" +
        "	and  LOAN_CD = '{0}' and STYPE_CD = '{1}' {2} {3}  	" +
        "	and CBS_STATUS IN (3,6,7) 	" +
        "		" +
        "	) lnapp	" +
        "	inner join 	" +
        "	( 	" +
        "	select * from 	" +
        "	(	" +
        "	select A.[CBS_APP_NO], DW_SIGN_DATE,LAST_UPDATE_DTM,ISACTIVE,DW_BADMONTH	" +
        "		" +
        "	from dbo.[DWH_BTFILE] A, #Max_LAST_UPDATE_DTM B	" +
        "	where 	" +
        "	convert(int,(substring(right([LAST_UPDATE_DTM],7),4,4) + substring(right([LAST_UPDATE_DTM],7),1,2)))	" +
        "	between   convert(int,(substring('{5}',1,4) + substring('{5}',6,2))) 	" +
        "	and convert(int,(substring('{4}',1,4) + substring('{4}',6,2))) 	" +
        "	and ISACTIVE=0	" +
        //Tai 2013-12-26
        "	and convert(decimal(18,2),DW_OUTSTAND)>0 " +
        //
        "	and A.[CBS_APP_NO]=B.[CBS_APP_NO] and A.LAST_UPDATE_DTM=B.[Max_LAST_UPDATE_DTM]	" +
        "		" +
        "	) DW_BFILE_T	" +
        "	) gsb2T on gsb2T.CBS_APP_NO = lnapp.APP_NO   	" +
        "		" +
        "	) gsbdat group by scrdif	" +
        "	)c  	" +
        "	on defrng.scrange = c.scrdif  	" +
        " select CC.scrange,DD.PL,DD.NPL into #temp1  	" +
        " from ( select range_name as scrange from st_scorernglst where range_cd =(select range_cd from ST_SCORERANGE where ltype= '{0}' and lstype = '{1}')  	" +
        " )CC left join ( select * from #Good_Bad_Temp A ) DD  on CC.scrange=DD.scrange  	" +
        " SELECT A.scrange , SUM(case when A.[NPL] is null then 0 else A.[NPL] end) as [Y]		" +
        " , SUM(case when A.[PL] is null then 0 else A.[PL] end) as [N]			" +
        " ,(SELECT SUM([PL]) FROM [dbo].[#Good_Bad_Temp] b where a.scrange >= b.scrange) as Cum_Good	" +
        " ,(SELECT SUM([NPL]) FROM [dbo].[#Good_Bad_Temp] b where a.scrange >= b.scrange) as Cum_Bad	" +
        " into #temp	 FROM #temp1 A	 GROUP BY scrange  ORDER BY scrange  	" +
        " select case when max(Cum_Good)=0 then null else max(Cum_Good)end as max_Cum_Good	" +
        " ,case when max(Cum_Bad)=0 then null else max(Cum_Bad) end as max_Cum_Bad into #temp2	 from #temp	" +
        "  select scrange,[N],[Y],cum_Good,Cum_Bad	" +
        "  ,case when (convert(decimal(15,0),cum_Good)/b.max_Cum_Good*100.0)is null then 0 else ((convert(decimal(15,0)	" +
        "  ,cum_Good)/b.max_Cum_Good)*100.0)end as [%CumGood]		" +
        "   ,(convert(decimal(15,0),cum_Bad)/b.max_Cum_Bad)*100.00 as [%CumBad]	 	" +
        "  into #Find_Diff_ABSDiff	   from #temp a,#temp2 b	" +
        " select scrange,[N],[Y]	" +
        " ,cum_Good,Cum_Bad,[%CumGood],[%CumBad],[%CumGood]-[%CumBad] as Diff,abs([%CumGood]-[%CumBad]) as ABSDiff	" +
        " ,(convert(decimal(15,3),[Y])/case when (convert(decimal(15,3),[Y])+convert(decimal(15,3),[N]))=0 then 1 else (convert(decimal(15,3),[Y])+convert(decimal(15,3),[N])) end )*100 as [BadRate (%)]	" +
        " ,(convert(decimal(15,3),Cum_Bad)/case when (Cum_Bad+cum_Good) =0 then null else (Cum_Bad+cum_Good) end )*100 as [Cum BadRate (%)]	" +
        " into #Find_KS_Grand_Total	" +
        " from #Find_Diff_ABSDiff	" +
        "	select defrng.scrange as scrange,[%CumGood] as [%Cum_G],[%CumBad] as [%Cum_B],isnull([%CumGood],0)-isnull([%CumBad],0) as [Sep_BG]	" +
        "	,(	" +
        "	convert(decimal(15,3),[Y])/	" +
        "	case when (convert(decimal(15,3),[Y])+convert(decimal(15,3),[N]))=0 then 1 else (convert(decimal(15,3),[Y])+convert(decimal(15,3),[N])) end	" +
        "	)*100 as [%BadRate(Current)]	" +
        "	into #Current_Part	" +
        "	from 	" +
        "	         ( select range_name as scrange from st_scorernglst where range_cd =  	" +
        "	         (select range_cd from ST_SCORERANGE where ltype= '{0}' and lstype = '{1}') 	" +
        "	         ) defrng  	" +
        "	         left join   	" +
        "	 #Find_Diff_ABSDiff  b  on defrng.scrange = b.scrange  	" +
        "	         order by defrng.scrange       	" +
        "select case when (SCORE - scr) < 0 then convert(varchar,(scr + ((((SCORE + 1) - scr)/rint) -1 ) * rint)) + '-' + convert(varchar,((scr + ((((SCORE + 1 ) - scr)/rint)-1 ) * rint) + rint - 1))	" +
        " when (SCORE - scr) >= 0 then convert(varchar,(scr + ((SCORE - scr)/rint) * rint)) + '-' + convert(varchar,((scr + ((SCORE - scr)/rint) * rint) + rint - 1)) 	" +
        " end as scrrange, count(*) as 'Y' into #Part1_d1 from ( select a.APP_NO,a.MODEL_CD,MODELVER_CD ,SCORE,LOAN_STATUS,LOAN_ISBAD,LTYPE,LSTYPE,SCMIN,SCMAX	" +
        " ,b.range_CD,(select cut_off from [ST_SCORERANGE] where [LTYPE]='{0}' and [LSTYPE] = '{1}' ) as scr,(SELECT rint  FROM [ST_SCORERANGE]  where [LTYPE]='{0}' and [LSTYPE]='{1}' ) as rint	" +
        ",count(1) as amt " +
        " from Ln_Dev a, St_scorerange b,St_scorernglst c where a.model_cd=b.model_cd and b.RANGE_CD=c.RANGE_CD and a.MODEL_CD = '{6}'and LOAN_ISBAD = '1'" +
        //--Tai 2014-08-21
        " AND [LTYPE]='{0}' and [LSTYPE]='{1}' " +
        //--Tai 2014-08-21
        " group by a.APP_NO,a.MODEL_CD,MODELVER_CD ,SCORE,LOAN_STATUS,LOAN_ISBAD,LTYPE,LSTYPE,SCMIN,SCMAX,b.range_CD) A	" +
        " group by case when (SCORE - scr) < 0 then convert(varchar,(scr + ((((SCORE + 1) - scr)/rint) -1 ) * rint)) + '-' + convert(varchar,((scr + ((((SCORE + 1 ) - scr)/rint)-1 ) * rint) + rint - 1)) 	" +
        " when (SCORE - scr) >= 0 then convert(varchar,(scr + ((SCORE - scr)/rint) * rint)) + '-' + convert(varchar,((scr + ((SCORE - scr)/rint) * rint) + rint - 1)) end	" +
        " order by case when (SCORE - scr) < 0 then convert(varchar,(scr + ((((SCORE + 1) - scr)/rint) -1 ) * rint)) + '-' + convert(varchar,((scr + ((((SCORE + 1 ) - scr)/rint)-1 ) * rint) + rint - 1)) 	" +
        " when (SCORE - scr) >= 0 then convert(varchar,(scr + ((SCORE - scr)/rint) * rint)) + '-' + convert(varchar,((scr + ((SCORE - scr)/rint) * rint) + rint - 1)) end	" +
        " select case when (SCORE - scr) < 0 then convert(varchar,(scr + ((((SCORE + 1) - scr)/rint) -1 ) * rint)) + '-' + convert(varchar,((scr + ((((SCORE + 1 ) - scr)/rint)-1 ) * rint) + rint - 1)) 	" +
        " when (SCORE - scr) >= 0 then convert(varchar,(scr + ((SCORE - scr)/rint) * rint)) + '-' + convert(varchar,((scr + ((SCORE - scr)/rint) * rint) + rint - 1))  	" +
        "  end as scrrange, count(*) as 'N' into #Part2_d1 from ( select a.APP_NO,a.MODEL_CD,MODELVER_CD,SCORE,LOAN_STATUS,LOAN_ISBAD,LTYPE,LSTYPE,SCMIN,SCMAX	" +
        " ,b.range_CD,(select cut_off from [ST_SCORERANGE] where [LTYPE]='{0}' and [LSTYPE] = '{1}' ) as scr,(SELECT rint  FROM [ST_SCORERANGE]  where [LTYPE]='{0}' and [LSTYPE]='{1}' ) as rint	" +
        " ,count(1) as amt " +
        " from Ln_Dev a, St_scorerange b,St_scorernglst c where a.model_cd=b.model_cd and b.RANGE_CD=c.RANGE_CD and a.MODEL_CD = '{6}' and LOAN_ISBAD = '0'" +
        //--Tai 2014-08-21
        " AND [LTYPE]='{0}' and [LSTYPE]='{1}' " +
        //--Tai 2014-08-21
        " group by a.APP_NO,a.MODEL_CD,MODELVER_CD ,SCORE,LOAN_STATUS,LOAN_ISBAD,LTYPE,LSTYPE,SCMIN,SCMAX,b.range_CD) A	" +
        " group by case when (SCORE - scr) < 0 then convert(varchar,(scr + ((((SCORE + 1) - scr)/rint) -1 ) * rint)) + '-' + convert(varchar,((scr + ((((SCORE + 1 ) - scr)/rint)-1 ) * rint) + rint - 1)) 	" +
        " when (SCORE - scr) >= 0 then convert(varchar,(scr + ((SCORE - scr)/rint) * rint)) + '-' + convert(varchar,((scr + ((SCORE - scr)/rint) * rint) + rint - 1)) end	" +
        " order by case when (SCORE - scr) < 0 then convert(varchar,(scr + ((((SCORE + 1) - scr)/rint) -1 ) * rint)) + '-' + convert(varchar,((scr + ((((SCORE + 1 ) - scr)/rint)-1 ) * rint) + rint - 1)) 	" +
        " when (SCORE - scr) >= 0 then convert(varchar,(scr + ((SCORE - scr)/rint) * rint)) + '-' + convert(varchar,((scr + ((SCORE - scr)/rint) * rint) + rint - 1)) end	" +
        " select defrng.scrrange,AA.Y,BB.N into #Good_Bad_Temp1 from ( select range_name as scrrange from st_scorernglst where range_cd = 	" +
        " (select range_cd from ST_SCORERANGE where ltype= '{0}' and lstype = '{1}')) defrng left join (  select * from #Part1_d1 ) AA on AA.scrrange=defrng.scrrange	" +
        " left join ( select * from #Part2_d1) BB on BB.scrrange=defrng.scrrange	" +
        " SELECT A.scrrange,[Y],[N],(case when [Y] is null then 0 else [Y] end)+(case when [N] is null then 0 else [N] end) as [Grand Total]	" +
        " , (SELECT SUM([N]) FROM [dbo].[#Good_Bad_Temp1] b where a.scrrange >= b.scrrange) as Cum_Good	" +
        " , (SELECT SUM([Y]) FROM [dbo].[#Good_Bad_Temp1] b where a.scrrange >= b.scrrange) as Cum_Bad into #temp_1 FROM #Good_Bad_Temp1 A	" +
        " select case when max(Cum_Good)=0 then null else max(Cum_Good)end as max_Cum_Good,case when max(Cum_Bad)=0 then null else max(Cum_Bad) end as max_Cum_Bad	" +
        " into #temp2_1 from #temp_1	" +
        " select scrrange,[N],[Y],cum_Good,Cum_Bad,case when (convert(decimal(15,0),cum_Good)/b.max_Cum_Good*100.0)is null then 0 else (convert(decimal(15,0),cum_Good)/b.max_Cum_Good*100.0)end as [%CumGood]	" +
        
        // Tai 2014-08-22" ,convert(decimal(15,0),cum_Bad)/b.max_Cum_Bad*100.00 as [%CumBad],((convert(decimal(15,0),cum_Good)/b.max_Cum_Good)-(convert(decimal(15,0),cum_Bad)/b.max_Cum_Bad))*100.00 as Diff	" +
        " ,convert(decimal(15,0),cum_Bad)/b.max_Cum_Bad*100.00 as [%CumBad],(isnull((convert(decimal(15,0),cum_Good)/b.max_Cum_Good),0)-isnull((convert(decimal(15,0),cum_Bad)/b.max_Cum_Bad),0))*100.00 as Diff	" +
        
        " ,ABS(((convert(decimal(15,0),cum_Good)/b.max_Cum_Good)-(convert(decimal(15,0),cum_Bad)/b.max_Cum_Bad))*100.00 ) as ABS_Diff,(convert(decimal(15,3),[Y])/	" +
        " ([Y]+case when [N] is null then 0 else [N] end) )*100 as [BadRate (%)],(convert(decimal(15,3),Cum_Bad)/(Cum_Bad+cum_Good))*100  as [Cum BadRate (%)]	" +
        " into #temp3_1 from #temp_1 a,#temp2_1 b	" +
        //" select scrrange as scrange,[%CumGood] as [DevG],[%CumBad] as DevB,Diff as DevSep,[BadRate (%)] as DevBRate into #Develop_Part from  #temp3_1	" +
        // Tai 2014-08-22
        " select scrrange as scrange, c.cg as [DevG], c.cb as DevB, c.cg-c.cb as DevSep, c.[%BadRate] as DevBRate " +
        " into #Develop_Part from  #temp3_1 " +
        " ,(SELECT A.MODEL_CD, A.LTYPE, A.LSTYPE, B.* FROM ST_SCORERANGE A INNER JOIN ST_SCORERNGLST_VALUE B ON A.range_CD=B.RANGE_CD " +
        " and a.LTYPE='{0}' and a.LSTYPE='{1}') c where scrrange=c.range_name " +
        // Tai 2014-08-22
        " select max(cum_Good) as [%Cum_G],max(Cum_Bad) as [%Cum_B] " +
        " ,(max(cum_Good)+max(Cum_Bad))as [Sep_BG],(max(convert(decimal(15,2),Cum_Bad)))/(max(cum_Good)+max(Cum_Bad))*100.00 as [%BadRate(Current)] " +
        " ,max(B.DevG) as DevG_T,max(B.DevB) as DevB_T,max(B.DevSep) as DevSep_T,max(B.DevBRate) as DevBRate_T " +
        " into #KS_Grand_Total_Temp_Table  " +
        " from #Find_KS_Grand_Total A, #Develop_Part B  where A.scrange =B.scrange " +
        " select 1 as seq,A.scrange as SCRRG,case when A.[%Cum_G] is null then 0 else A.[%Cum_G] end as CurG,case when A.[%Cum_B] is null then 0 else A.[%Cum_B] end as CurB,A.Sep_BG as CurSep,A.[%BadRate(Current)] as CurBRate,B.DevG,B.DevB,B.DevSep,B.DevBRate " +
        " into #Find_KS_Total_Dev_Current from " +
        " #Current_Part A, #Develop_Part B where A.scrange =B.scrange  " +
        " select 29 as seq,' KS ' as SCRRG,' -' as CurG_T,'   ' as CurB_T,Max(abs(case when CurSep is null then 0 else CurSep end)) as KS_Current,'    ' as CurBRate_T,'    ' as DevG_T,' ' as DevB_T,Max(abs(case when DevSep is null then 0 else DevSep end)) as KS_Dev " +
        " ,' ' as DevBRate_T into #KS_Calculation from #Find_KS_Total_Dev_Current " +
        " select seq,SCRRG,case when CurG_T>0 then 0 end as CurG_T " +
        " ,case when CurB_T>0 then 0 end as CurB_T,KS_Current,case when CurB_T>0 then 0 end as CurBRate_T " +
        " ,case when CurB_T>0 then 0 end as DevG_T,case when CurB_T>0 then 0 end as DevB_T,KS_Dev " +
        " ,case when CurB_T>0 then 0 end as DevBRate_T into #KS_Calculation_Blank from #KS_Calculation " +
        " select 30 as seq,' Total ' as SCRRG   ,[%Cum_G] as CurG   ,[%Cum_B] as CurB,[Sep_BG] as CurSep   ,[%BadRate(Current)] as CurBRate    " +
        " ,(select max(cum_Good) as DevG  from #temp3_1) as DevG   ,(select max(Cum_Bad) as DevB  from #temp3_1) as DevB   ,(select max(cum_Good) from #temp3_1)+(select max(Cum_Bad) from #temp3_1) as DevSep,(select convert(decimal(15,3),sum([Y]))*100.00/sum([Grand Total]) as DevBRate from #temp_1) as DevBRate into #Part2  from #KS_Grand_Total_Temp_Table  " +
        " select * into #Part1 " +
        " from (  select 1 as seq,A.scrange as SCRRG ,case when A.[%Cum_G] is null then 0 else A.[%Cum_G] end as CurG " +
        " ,case when A.[%Cum_B] is null then 0 else A.[%Cum_B] end as CurB,case when A.Sep_BG is null then 0 else A.Sep_BG end as CurSep  " +
        " ,case when A.[%BadRate(Current)] is null then 0 else A.[%BadRate(Current)] end as CurBRate " +
        " ,case when B.DevG is null then 0 else B.DevG end as DevG,case when B.DevB is null then 0 else B.DevB end as DevB " +
        " ,case when B.DevSep is null then 0 else B.DevSep end as DevSep,case when B.DevBRate is null then 0 else B.DevBRate end as DevBRate  " +
        " from  #Current_Part A, #Develop_Part B  where A.scrange =B.scrange   " +
        " union select seq,SCRRG  ,CurG_T,CurB_T,KS_Current,CurBRate_T,DevG_T,DevB_T,KS_Dev,DevBRate_T  from #KS_Calculation_Blank  ) Main  order by seq  " +
        " select convert(varchar,seq) as seq,convert(varchar,SCRRG) as SCRRG,convert(varchar,convert(decimal(15,2),CurG)) as CurG,convert(varchar,convert(decimal(15,2),CurB)) as CurB " +
        " ,convert(varchar,convert(decimal(15,2),CurSep)) as CurSep,convert(varchar,convert(decimal(15,2),CurBRate)) as CurBRate  " +
        " ,convert(varchar,convert(decimal(15,2),DevG)) as DevG    " +
        " ,convert(varchar,convert(decimal(15,2),DevB)) as DevB  " +
        " ,convert(varchar,convert(decimal(15,2),DevSep)) as DevSep,convert(varchar,convert(decimal(15,2),DevBRate)) as DevBRate " +
        " into #Excel_format from  #Part1 union  select convert(varchar,seq) as seq " +
        "  ,convert(varchar,SCRRG) as SCRRG,case when CurG is null then '0' else dbo.comma_format(convert(varchar,convert(decimal(15,0),CurG))) end as CurG " +
 " ,case when CurB is null then '0' else dbo.comma_format(convert(varchar,convert(decimal(15,0),CurB))) end as CurB    " +
 " ,case when CurSep is null then '0' else dbo.comma_format(convert(varchar,convert(decimal(15,0),CurSep))) end as CurSep " +
 " ,case when CurBRate is null then '0.00' else convert(varchar,convert(decimal(15,2),CurBRate)) end as CurBRate " +
 " ,case when DevG is null then '0.00' else convert(varchar,dbo.comma_format(convert(decimal(15,0),DevG))) end as DevG    " +
 " ,case when DevB is null then '0.00' else dbo.comma_format(convert(varchar,convert(decimal(15,0),DevB))) end as DevB " +
 " ,case when DevSep is null then '0.00' else dbo.comma_format(convert(varchar,convert(decimal(15,0),DevSep))) end as DevSep " +
 " ,case when DevBRate is null then '0.00' else convert(varchar,convert(decimal(15,2),DevBRate)) end as DevBRate  from  #Part2 " +
        " select SCRRG,CurG,CurB,CurSep,CurBRate,DevG,DevB,DevSep,DevBRate from #Excel_format  order by seq " +
        " drop table #Part1 drop table #Part2 drop table  #Excel_format " +
        " drop table #Find_KS_Total_Dev_Current " +
        " drop table #Current_Part " +
        " drop table #Develop_Part " +
        " drop table #temp  drop table #temp1 " +
        " drop table #temp2 " +
        " drop table  #Good_Bad_Temp1 " +
        " drop table #Max_LAST_UPDATE_DTM " +
        " drop table #Find_KS_Grand_Total " +
        " drop table #Find_Diff_ABSDiff " +
        " drop table #KS_Grand_Total_Temp_Table " +
        " drop table #KS_Calculation_Blank " +
        " drop table #KS_Calculation  " +
        " drop table #Part1_d1 drop table #Part2_d1 " +
        " drop table #Good_Bad_Temp drop table #temp_1 " +
        " drop table #temp2_1 drop table #temp3_1 "
        , ddlLoan.SelectedItem.Value, ddlSubType.SelectedItem.Value, mktqrf, mktqrt, start.ToString("yyyy-MM-dd"), start12yg.ToString("yyyy-MM-dd"), "00" + ddlModel.SelectedItem.Value);

                                    DataTable dt = new DataTable();

                                    if (ddlModel.SelectedItem.Value == "7")
                                    {
                                        String strConnString = ConfigurationManager.ConnectionStrings["GSBSQLServer"].ConnectionString;
                                        SqlConnection con = new SqlConnection(strConnString);
                                        con.Open();
                                        SqlCommand command;
                                        command = new SqlCommand("sp_BackEnd_GoodOrBadSeparationReport", con);

                                        command.Parameters.Add(new SqlParameter("@start", ddlOpenDateYearDTM.SelectedValue.ToString() + (ddlOpenDateMonthDTM.SelectedIndex + 1).ToString("00")));
                                        command.Parameters.Add(new SqlParameter("@start12yg", ddlOpenDateYearDTMend.SelectedValue.ToString() + (ddlOpenDateMonthDTMend.SelectedIndex + 1).ToString("00")));
                                        command.Parameters.Add(new SqlParameter("@ddlLoan", ddlLoan.SelectedItem.Value));
                                        command.Parameters.Add(new SqlParameter("@ddlSubType", ddlSubType.SelectedItem.Value));
                                        command.Parameters.Add(new SqlParameter("@mktqrf", ddlMarketFrm.Text));
                                        command.Parameters.Add(new SqlParameter("@mktqrt", ddlMarketTo.Text));
                                        command.Parameters.Add(new SqlParameter("@ddlModel", "00" + ddlModel.SelectedItem.Value));
                                        command.Parameters.Add(new SqlParameter("@APPDATE_Start", ddlOpenDateYear.SelectedValue.ToString() + (ddlOpenDateMonth.SelectedIndex + 1).ToString("00")));
                                        command.Parameters.Add(new SqlParameter("@APPDATE_End", ddlOpenDateYearend.SelectedValue.ToString() + (ddlOpenDateMonthend.SelectedIndex + 1).ToString("00")));
                                        command.CommandType = CommandType.StoredProcedure;
                                        command.CommandTimeout = 36000;
                                        SqlDataAdapter da = new SqlDataAdapter(command);
                                        da.Fill(dt);
                                        Session["dtb"] = dt;
                                        gvTable.DataSource = dt;
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
                Session["sHeaderRpt1"] = "ประเภทโมเดล : " + ddlModel.SelectedItem.Text;
                Session["sHeaderRpt2"] = "ประเภทสินเชื่อ / ประเภทสินเชื่อย่อย : " + ddlLoan.SelectedItem.Text + " / " + ddlSubType.SelectedItem.Text;

                if (ddlMarketFrm.SelectedIndex == 0)
                    Session["sHeaderRpt3"] = "Market Code : " + ddlMarketFrm.SelectedItem.Text;
                else
                    Session["sHeaderRpt3"] = "Market Code : " + ddlMarketFrm.SelectedItem.Text + " ถึง " + ddlMarketTo.SelectedItem.Text;

                Session["sHeaderRpt4"] = "วันที่เปิดใบคำขอสินเชื่อ : " + ddlOpenDateMonth.SelectedValue + " " + ddlOpenDateYear.SelectedItem + " ถึง " + ddlOpenDateMonthend.SelectedValue + " " + ddlOpenDateYearend.SelectedItem;
                Session["sHeaderRpt5"] = "As of Date : " + (ddlOpenDateMonthDTM.SelectedItem) + " " + ddlOpenDateYearDTM.SelectedItem + " ถึง " + (ddlOpenDateMonthDTMend.SelectedItem) + " " + ddlOpenDateYearDTMend.SelectedItem;



                //Regenerate data for report
                DataTable dt1 = (DataTable)Session["dtb"];
                Session["rptData1"] = dt1;
                //DataTable dt2 = (DataTable)ViewState["DataTable2"];
                //Session["rptData2"] = dt2;
                //DataTable dt3 = (DataTable)ViewState["DataTable3"];
                //Session["rptData3"] = dt3;



                ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "aa", "window.open('ReportLayOut/frmPreviewBackRpt.aspx?ReportId=91');", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScript", "appPop('โปรดเลือกตัวเลือก','กรุณากดปุ่มแสดงข้อมูลก่อน')", true);
                return;
            }
        }

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

            Session["sHeaderRpt1"] = "ประเภทโมเดล : " + ddlModel.SelectedItem.Text ;
            Session["sHeaderRpt2"] = "ประเภทสินเชื่อ / ประเภทสินเชื่อย่อย : " + ddlLoan.SelectedItem.Text + " / " + ddlSubType.SelectedItem.Text;

            if (ddlMarketFrm.SelectedIndex == 0)
                Session["sHeaderRpt3"] = "Market Code : " + ddlMarketFrm.SelectedItem.Text;
            else
                Session["sHeaderRpt3"] = "Market Code : " + ddlMarketFrm.SelectedItem.Text + " ถึง " + ddlMarketTo.SelectedItem.Text;

            Session["sHeaderRpt4"] = "วันที่เปิดใบคำขอสินเชื่อ : " + ddlOpenDateMonth.SelectedValue + " " + ddlOpenDateYear.SelectedItem + " ถึง " + ddlOpenDateMonthend.SelectedValue + " " + ddlOpenDateYearend.SelectedItem;
            Session["sHeaderRpt5"] = "As of Date : " + (ddlOpenDateMonthDTM.SelectedItem) + " " + ddlOpenDateYearDTM.SelectedItem + " ถึง " + (ddlOpenDateMonthDTMend.SelectedItem) + " " + ddlOpenDateYearDTMend.SelectedItem;




            //Regenerate data for report
            DataTable dt1 = (DataTable)Session["dtb"];
            Session["rptData1"] = dt1;
            //DataTable dt2 = (DataTable)ViewState["DataTable2"];
            //Session["rptData2"] = dt2;
            //DataTable dt3 = (DataTable)ViewState["DataTable3"];
            //Session["rptData3"] = dt3;



            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "aa", "window.open('ReportLayOut/frmPreviewBackRpt.aspx?ReportId=1');", true);
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

                TableCell HeaderCell = new TableCell();
                HeaderCell.Text = "Cutoff Score";
                HeaderCell.RowSpan = 2;
                HeaderGridRow.Cells.Add(HeaderCell);

                HeaderCell = new TableCell();
                HeaderCell.Text = "Current Validation Sample(%)";
                HeaderCell.ColumnSpan = 8;
                HeaderGridRow.Cells.Add(HeaderCell);

                HeaderCell = new TableCell();
                HeaderCell.Text = "Development Sample(%)";
                HeaderCell.ColumnSpan = 8;
                HeaderGridRow.Cells.Add(HeaderCell);

                HeaderCell = new TableCell();
                HeaderCell.Text = "Good";
                HeaderGridRow2.Cells.Add(HeaderCell);

                HeaderCell = new TableCell();
                HeaderCell.Text = "%Good";
                HeaderGridRow2.Cells.Add(HeaderCell);


                HeaderCell = new TableCell();
                HeaderCell.Text = "%Cum_G";
                HeaderGridRow2.Cells.Add(HeaderCell);


                HeaderCell = new TableCell();
                HeaderCell.Text = "Bad";
                HeaderGridRow2.Cells.Add(HeaderCell);


                HeaderCell = new TableCell();
                HeaderCell.Text = "%Bad";
                HeaderGridRow2.Cells.Add(HeaderCell);


                HeaderCell = new TableCell();
                HeaderCell.Text = "%Cum_B";
                HeaderGridRow2.Cells.Add(HeaderCell);

                HeaderCell = new TableCell();
                HeaderCell.Text = "Sep_BG";
                HeaderGridRow2.Cells.Add(HeaderCell);

                HeaderCell = new TableCell();
                HeaderCell.Text = "%BadRate";
                HeaderGridRow2.Cells.Add(HeaderCell);

                HeaderCell = new TableCell();
                HeaderCell.Text = "Good";
                HeaderGridRow2.Cells.Add(HeaderCell);

                HeaderCell = new TableCell();
                HeaderCell.Text = "%Good";
                HeaderGridRow2.Cells.Add(HeaderCell);


                HeaderCell = new TableCell();
                HeaderCell.Text = "%Cum_G";
                HeaderGridRow2.Cells.Add(HeaderCell);


                HeaderCell = new TableCell();
                HeaderCell.Text = "Bad";
                HeaderGridRow2.Cells.Add(HeaderCell);


                HeaderCell = new TableCell();
                HeaderCell.Text = "%Bad";
                HeaderGridRow2.Cells.Add(HeaderCell);


                HeaderCell = new TableCell();
                HeaderCell.Text = "%Cum_B";
                HeaderGridRow2.Cells.Add(HeaderCell);

                HeaderCell = new TableCell();
                HeaderCell.Text = "Sep_BG";
                HeaderGridRow2.Cells.Add(HeaderCell);

                HeaderCell = new TableCell();
                HeaderCell.Text = "%BadRate";
                HeaderGridRow2.Cells.Add(HeaderCell);

                gvTable.Controls[0].Controls.AddAt(0, HeaderGridRow);
                gvTable.Controls[0].Controls.AddAt(1, HeaderGridRow2);

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