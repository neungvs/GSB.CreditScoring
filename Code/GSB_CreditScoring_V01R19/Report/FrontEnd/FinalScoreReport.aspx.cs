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
    public partial class FinalScoreReport : System.Web.UI.Page
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
            }

            ddlOpenDateMonth.Items.Clear();
            ddlOpenDateMonthend.Items.Clear();

            ddlOpenDateMonth.DataSource = GetMonth();
            ddlOpenDateMonth.DataBind();
            ddlOpenDateMonthend.DataSource = GetMonth();
            ddlOpenDateMonthend.DataBind();

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
            DataTable dttb2 = (DataTable)ViewState["DataTable2"];
            string filename = string.Format("GSB-CS_FinalScoreReport_(Date_{0}-Time_{1}).xls", DateTime.Now.ToString("dd-MM-yyyy"), DateTime.Now.ToString("HH-mm-ss"));
            StringWriter tw = new StringWriter();
            DataGrid dgGrid = new DataGrid();
            
            tw.Write(string.Format("Final Score Report"));
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
            tw.Write(string.Format("วันที่เปิดใบคำขอสินเชื่อ :| {0} ถึง {1} ", "", ""));
            tw.Write(tw.NewLine);
            tw.Write(string.Format("Export Date :| {0} : {1} ", DateTime.Now.ToString("dd-MM-yyyy"), DateTime.Now.ToString("HH-mm-ss")));
            tw.Write(tw.NewLine);
            tw.Write("ผู้พิมพ์ " + ExportUtility.getCurrentUserId);
            tw.Write(tw.NewLine);

            int iColCount = new int();
            int iColCount1 = new int();
            iColCount = dttb.Columns.Count;
            iColCount1 = dttb2.Columns.Count;
            tw.Write("Score Range|Accept|Reject|Total|Accept (%)");
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
                        tw.Write("0");
                    }
                    if (i < iColCount - 1)
                    {
                        tw.Write("|");
                    }
                }
                tw.Write(tw.NewLine);
            }

            foreach (DataRow dr in dttb2.Rows)
            {
                for (int i = 0; i < iColCount1; i++)
                {
                    if (!Convert.IsDBNull(dr[i]))
                    {
                        //tw.Write(dr[i].ToString().Replace(",",""));
                        tw.Write(dr[i].ToString());
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


        //protected void LoadVersion(object sender, EventArgs e)
        //{
        //    string sql = "";
        //    sql = string.Format("select MODEL_NAME+'_V1' as [Model_V_NAME] from LN_MODEL where ACTIVE_FLAG = '1' and");
        //    sql += "  [MODEL_CD]=" + ddlModel.SelectedValue + " ";
            

        //    ddlModelVersion.DataSource = conn.ExcuteSQL(sql);
        //    ddlModelVersion.DataTextField = "Model_V_NAME";
        //    ddlModelVersion.DataValueField = "Model_V_NAME";
        //    ddlModelVersion.DataBind();

        //    //LoadLoan();
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
            DateTime start = new DateTime();
            DateTime end = new DateTime();
            IFormatProvider format = new CultureInfo("en-GB");
            DateTime dateOpenLoan = DateTime.MinValue;
            DateTime dateCloseLoan = DateTime.MinValue;
            StringBuilder sqlQuery = new StringBuilder();
            string mktqrf = "";
            string mktqrt = "";

            if (ddlLoan.SelectedIndex <= 0 || ddlSubType.SelectedIndex <= 0 || ddlMarketFrm.SelectedIndex <= 0 || ddlMarketTo.SelectedIndex <= 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScript", "appPop('โปรดเลือกตัวเลือก','กรุณาเลือกเงื่อนไขการค้นหาให้ครบถ้วน ')", true);
                return;
            }


            if (ddlLoan.SelectedItem.Value != "")
                sqlQuery.Append(String.Format(" AND (LOAN_CD = '{0}')", ddlLoan.SelectedItem.Value));

            if (ddlSubType.SelectedItem.Value != "")
                sqlQuery.Append(String.Format(" AND (STYPE_CD = '{0}')", ddlSubType.SelectedItem.Value));
            //if (!(txtOpenDate.Text == ""))
            //{
            //    start = DateTime.Parse((txtOpenDate.Text.ToString().Substring(0, 6) + (Convert.ToInt32(txtOpenDate.Text.Substring(6, 4).ToString()) - 543).ToString()));
            //}
            //else
            //{
            //    start = DateTime.Now;
            //}
            //if (!(txtCloseDate.Text == ""))
            //{
            //    end = DateTime.Parse(txtCloseDate.Text.ToString().Substring(0, 6) + (Convert.ToInt32(txtCloseDate.Text.Substring(6, 4).ToString()) - 543).ToString() + " 23:59:59.999");
            //}
            //else
            //{
            //    end = DateTime.Now;

            //}

            if (ddlMarketFrm.SelectedIndex <= 0 || ddlMarketTo.SelectedIndex <=0)
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



            int monthstart = Convert.ToInt16(ddlOpenDateYear.SelectedValue) * 100 + ddlOpenDateMonth.SelectedIndex;
            int monthend = Convert.ToInt16(ddlOpenDateYearend.SelectedValue) * 100 + ddlOpenDateMonthend.SelectedIndex;

            //int months = (end.DayOfYear - start.DayOfYear) + 365 * (end.Year - start.Year);

            if (monthstart < 0 || monthend < 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScript", "appPop('วันที่เปิดใบคำขอสินเชื่อไม่ถูกต้อง?','เลือกวันที่เปิดใบคำขอไม่ถูกต้อง')", true);
                return;
            }
            else if (monthstart > monthend)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScript", "appPop('วันที่เปิดใบคำขอสินเชื่อไม่ถูกต้อง?','เลือกวันที่เปิดใบคำขอไม่ถูกต้อง')", true);
                return;
            }
            else
            {
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
                                    //-- Begin
                                    int st1 = Convert.ToInt32(ddlMarketFrm.SelectedItem.Value);
                                    int st2 = Convert.ToInt32(ddlMarketTo.SelectedItem.Value);

                                    if (st1 <= st2)
                                    {
                                        appQuery1 = string.Format("	select * into #Heading from (	" +
                "	select case when LEFT(CBS_REASON_CD,1)='A' AND  CBS_STATUS in (4,5) then 'Y' 	" +
                "	when LEFT(CBS_REASON_CD,1)='R' AND  CBS_STATUS in (3,6,7) then 'Y'	" +
                "	when LEFT(CBS_REASON_CD,1)='A' AND  CBS_STATUS in (3,6,7) then 'N'	" +
                "	when LEFT(CBS_REASON_CD,1)='R' AND  CBS_STATUS in (4,5) then 'N'	" +
                "	else 'N/A' end as CONTRAST_FLAG, LEFT(CBS_REASON_CD,1) as CBS_REASON	" +
                "	,CASE WHEN (score>=scr) then 'Y' else 'N' end as scapproval_flag	" +
                "	,case when CBS_STATUS in (3,6,7) then 'Y' when  CBS_STATUS in (4,5) then 'N' 	" +
                "	when  CBS_STATUS in (1,2) OR CBS_STATUS is null then '<>' end as Approval_Flag,AA.*	" +
                "	from ( select a.*,b.score as SCORE,c.CBS_REASON_CD,      	" +
                "	( select cut_off from ST_SCORERANGE y where  y.ltype= '{1}' and y.lstype = '{2}' ) as scr	" +
                "	,( select rint from ST_SCORERANGE y where  y.ltype= '{1}' and y.lstype = '{2}' ) as rint	" +
                "	from LN_APP a,LN_GRADE b,CBS_LN_APP c 	" +
                "	where a.APP_NO = b.APP_NO  and a.app_no = c.CBS_APP_NO 	" +
                "	and a.APP_SEQ = (select MAX(LN_APP2.APP_SEQ) from LN_APP AS LN_APP2 where a.APP_NO = LN_APP2.APP_NO)   	" +
                "	and LOAN_CD = '{1}' and STYPE_CD = '{2}' {3} {4}	" +
                "	and substring(convert(varchar,convert(date,a.APP_DATE,103)),1,7) between substring('{5}',1,7) and substring('{6}',1,7)	" +
                "	and CBS_STATUS IN (3,4,5,6,7) ) AA ) ZZ	" +
                "	where CONTRAST_FLAG='N'	" +
                "	select case when (SCORE - scr) < 0 then convert(varchar,(scr + ((((SCORE + 1) - scr)/rint) -1 ) * rint)) + '-' + convert(varchar,((scr + ((((SCORE + 1 ) - scr)/rint)-1 ) * rint) + rint - 1))  	" +
                "	when (SCORE - scr) >= 0 then convert(varchar,(scr + ((SCORE - scr)/rint) * rint)) + '-' + convert(varchar,((scr + ((SCORE - scr)/rint) * rint) + rint - 1))   	" +
                "	end as scrdif,[scApproval_flag],[CBS_REASON],[Approval_Flag],APP_NO	" +
                "	into #Count from #Heading	" +
                "	select scrdif, count(*) as 'Accept'	" +
                "	into #CountA from #Count	" +
                "	where [CBS_REASON] ='A' group by scrdif	" +
                "	select scrdif, count(*) as 'Reject'	" +
                "	into #CountReject from #Count	" +
                "	where [CBS_REASON] ='R' group by scrdif	" +
                "	select AA.scrange ,BB.Accept as [ACCEPT],BB.Reject as [REJ],BB.Total as [Total] into #Cumulative_Cal	" +
                "	from 	" +
                "	( select range_name as scrange from st_scorernglst where range_cd = 	" +
                "	(select range_cd from ST_SCORERANGE where ltype= '{1}' and lstype = '{2}')  	" +
                "	) AA left join	" +
                "	(select a.scrdif as scrange,case when a.Accept is null then 0 else a.Accept end as [Accept]	" +
                "	,case when b.Reject is null  then 0 else b.Reject end as [Reject]	" +
                "	,(case when a.Accept is null then 0 else a.Accept end)+(case when b.Reject is null  then 0 else b.Reject end ) as Total	" +
                "	from #CountA a left join #CountReject b	" +
                "	on a.scrdif=b.scrdif	" +
                "	) BB on AA.scrange=BB.scrange	" +
                " select scrange ,case when [ACCEPT] is null then 0 else [ACCEPT] end as [ACCEPT]	" +
                "	,case when [REJ] is null then 0 else [REJ] end as [REJ]	,case when [TOTAL] is null then 0 else [TOTAL] end as [TOTAL]	" +
                " into #Cumulative_Cal1 from #Cumulative_Cal	" +
                "	select 	*, (select sum([ACCEPT]) from #Cumulative_Cal1 t2 	where t2.scrange <= t1.scrange) as [Cumulative Total]	" +
                "	into #Cumulative_Percent from #Cumulative_Cal1 t1	" +
                "	select sum([ACCEPT]) as Total_sum into #Total_Accept FROM #Cumulative_Percent	" +
                "	select scrange as [SCRRG],[ACCEPT] as [PAPP],[REJ] as [PREJ],[TOTAL] as [ACT]	" +
                "	,case when [ACCEPT] is not null then (convert(decimal(15,3),[Cumulative Total])*100.00/(case when Total_sum=0 then 1 else Total_sum end)) else 0 end as [appvt]	" +
                "	into #Excel_Format from #Cumulative_Percent a,#Total_Accept b	" +
                " select [SCRRG],dbo.comma_format(convert(varchar,[PAPP])) as [PAPP],dbo.comma_format(convert(varchar,[PREJ])) as [PREJ] 	" +
                " ,dbo.comma_format(convert(varchar,[ACT])) as [ACT],convert(varchar,convert(decimal(15,2),[appvt])) as [appvt] from  #Excel_Format	" +
                "	drop table #Cumulative_Percent	drop table #Total_Accept " +
                "	drop table #Cumulative_Cal	drop table #Cumulative_Cal1" +
                "	drop table #Heading drop table #Count	" +
                "	drop table #CountA drop table  #CountReject	drop table  #Excel_Format "
                , ("00" + ddlModel.SelectedItem.Value), ddlLoan.SelectedItem.Value, ddlSubType.SelectedItem.Value, mktqrf, mktqrt, start.ToString("yyyy-MM-dd"), end.ToString("yyyy-MM-dd hh:mm:ss"));

                                        DataTable dtb = new DataTable();

                                        if (ddlModel.SelectedItem.Value == "7")
                                        {
                                            String strConnString = ConfigurationManager.ConnectionStrings["GSBSQLServer"].ConnectionString;
                                            SqlConnection con = new SqlConnection(strConnString);
                                            con.Open();
                                            SqlCommand command;
                                            command = new SqlCommand("sp_FrontEnd_FinalScoreReport", con);

                                            //DateTime date_open = DateTime.ParseExact(txtOpenDate.Text, "dd/MM/yyyy", null);
                                            //DateTime date_close = DateTime.ParseExact(txtCloseDate.Text, "dd/MM/yyyy", null);


                                            command.Parameters.Add(new SqlParameter("@start", ddlOpenDateYear.SelectedValue.ToString() + (ddlOpenDateMonth.SelectedIndex + 1).ToString("00")));
                                            command.Parameters.Add(new SqlParameter("@end", ddlOpenDateYearend.SelectedValue.ToString() + (ddlOpenDateMonthend.SelectedIndex + 1).ToString("00")));
                                            command.Parameters.Add(new SqlParameter("@ddlLoan", ddlLoan.SelectedItem.Value));
                                            command.Parameters.Add(new SqlParameter("@ddlSubType", ddlSubType.SelectedItem.Value));
                                            command.Parameters.Add(new SqlParameter("@mktqrf", ddlMarketFrm.Text));
                                            command.Parameters.Add(new SqlParameter("@mktqrt", ddlMarketTo.Text));
                                            command.Parameters.Add(new SqlParameter("@ddlModel", "00" + ddlModel.SelectedValue));
                                            command.CommandType = CommandType.StoredProcedure;
                                            SqlDataAdapter da = new SqlDataAdapter(command);
                                            da.Fill(dtb);
                                            Session["dtb"] = dtb;
                                            gvTable.DataSource = dtb;
                                            gvTable.DataBind();
                                        }
                                        else
                                        {

                                            Session["dtb"] = conn.ExcuteSQL(appQuery1);
                                            gvTable.DataSource = Session["dtb"];
                                            gvTable.DataBind();
                                        }



                //                        appQuery1 = string.Format("	select COUNT(1) as total, SUM(appv) as Accept	" +
                //"	,SUM(rej) as Rej	" +
                //"	into #Part1	" +
                //"	from 	" +
                //"	( 	" +
                //"	select score,scr ,CBS_STATUS 	" +
                //"	,case when (SCORE - scr) < 0 then 1 else 0 end as blcut, 	" +
                //"	case when (SCORE - scr) >= 0 then 1 else 0 end as abcut	" +
                //"	, case when CBS_STATUS IN (3,6,7) then 1 else 0 end as appv	" +
                //"	, case when CBS_STATUS IN (4,5) then 1 else 0 end as rej 	" +
                //"	,case when ((SCORE - scr) < 0) and (CBS_STATUS IN (3,6)) then 1 else 0 end as lsov, 	" +
                //"	case when ((SCORE - scr) >= 0) and (CBS_STATUS IN (4,5,7)) then 1 else 0 end as hsov 	" +
                //"	from 	" +
                //"	(	" +
                //"	select score,(select cut_off from ST_SCORERANGE y where  y.ltype= '{1}' and y.lstype = '{2}' ) as scr 	" +
                //"	,CBS_STATUS 	" +
                //"	from	" +
                //"	(	" +
                //"	select case when LEFT(CBS_REASON_CD,1)='A' AND  CBS_STATUS in (4,5) then 'Y' 	" +
                //"	when LEFT(CBS_REASON_CD,1)='R' AND  CBS_STATUS in (3,6,7) then 'Y'	" +
                //"	when LEFT(CBS_REASON_CD,1)='A' AND  CBS_STATUS in (3,6,7) then 'N'	" +
                //"	when LEFT(CBS_REASON_CD,1)='R' AND  CBS_STATUS in (4,5) then 'N'	" +
                //"	else 'N/A' end as CONTRAST_FLAG	" +
                //"	, LEFT(CBS_REASON_CD,1) as CBS_REASON	" +
                //"	,CASE WHEN (score>=scr) then 'Y' else 'N' end as scapproval_flag	" +
                //"	,case when CBS_STATUS in (3,6,7) then 'Y' 	" +
                //"	when  CBS_STATUS in (4,5) then 'N' 	" +
                //"	when  CBS_STATUS in (1,2) OR CBS_STATUS is null then '<>' end as Approval_Flag	" +
                //"	,AA.*	" +
                //"	from 	" +
                //"	(	" +
                //"	select a.*,b.score as SCORE,c.CBS_REASON_CD,      	" +
                //"	(select cut_off from ST_SCORERANGE y where  y.ltype= '{1}' and y.lstype = '{2}' ) as scr	" +
                //"	,(select rint from ST_SCORERANGE y where  y.ltype= '{1}' and y.lstype = '{2}' ) as rint	" +
                //"	from LN_APP a,LN_GRADE b,CBS_LN_APP c 	" +
                //"	where a.APP_NO = b.APP_NO  and a.app_no = c.CBS_APP_NO 	" +
                //"	and a.APP_SEQ = (select MAX(LN_APP2.APP_SEQ) from LN_APP AS LN_APP2 where a.APP_NO = LN_APP2.APP_NO)   	" +
                //"	and LOAN_CD = '{1}' and STYPE_CD = '{2}' {3} {4} " +
                //"	and substring(convert(varchar,convert(date,a.APP_DATE,103)),1,7) between substring('{5}',1,7) and substring('{6}',1,7) " +
                //"	and CBS_STATUS IN (3,4,5,6,7)  	" +
                //"	) AA	" +
                //"	) ZZ where CONTRAST_FLAG='N'	" +
                //"	) gsbdat 	" +
                //"	) gsbcnt	" +
                //"	select * 	" +
                //"	into #Heading	" +
                //"	from	" +
                //"	(	" +
                //"	select case when LEFT(CBS_REASON_CD,1)='A' AND  CBS_STATUS in (4,5) then 'Y' 	" +
                //"	when LEFT(CBS_REASON_CD,1)='R' AND  CBS_STATUS in (3,6,7) then 'Y'	" +
                //"	when LEFT(CBS_REASON_CD,1)='A' AND  CBS_STATUS in (3,6,7) then 'N'	" +
                //"	when LEFT(CBS_REASON_CD,1)='R' AND  CBS_STATUS in (4,5) then 'N'	" +
                //"	else 'N/A' end as CONTRAST_FLAG	" +
                //"	, LEFT(CBS_REASON_CD,1) as CBS_REASON	" +
                //"	,CASE WHEN (score>=scr) then 'Y' else 'N' end as scapproval_flag	" +
                //"	,case when CBS_STATUS in (3,6,7) then 'Y' 	" +
                //"	when  CBS_STATUS in (4,5) then 'N' 	" +
                //"	when  CBS_STATUS in (1,2) OR CBS_STATUS is null then '<>' end as Approval_Flag	" +
                //"	,AA.*	" +
                //"	from 	" +
                //"	(	" +
                //"	select a.*,b.score as SCORE,c.CBS_REASON_CD,      	" +
                //"	(select cut_off from ST_SCORERANGE y where  y.ltype= '{1}' and y.lstype = '{2}' ) as scr	" +
                //"	      ,(select rint from ST_SCORERANGE y where  y.ltype= '{1}' and y.lstype = '{2}' ) as rint	" +
                //"	from LN_APP a,LN_GRADE b,CBS_LN_APP c 	" +
                //"	where a.APP_NO = b.APP_NO  and a.app_no = c.CBS_APP_NO 	" +
                //"	and a.APP_SEQ = (select MAX(LN_APP2.APP_SEQ) from LN_APP AS LN_APP2 where a.APP_NO = LN_APP2.APP_NO)   	" +
                //"	and LOAN_CD = '{1}' and STYPE_CD = '{2}' {3} {4} " +
                //"	and substring(convert(varchar,convert(date,a.APP_DATE,103)),1,7) between substring('{5}',1,7) and substring('{6}',1,7) " +
                //"	and CBS_STATUS IN (3,4,5,6,7)  	" +
                //"	) AA	" +
                //"	) ZZ	" +
                //"	where CONTRAST_FLAG='N'	" +
                //"	select case when (SCORE - scr) < 0 then convert(varchar,(scr + ((((SCORE + 1) - scr)/rint) -1 ) * rint)) + '-' + convert(varchar,((scr + ((((SCORE + 1 ) - scr)/rint)-1 ) * rint) + rint - 1))  	" +
                //"	when (SCORE - scr) >= 0 then convert(varchar,(scr + ((SCORE - scr)/rint) * rint)) + '-' + convert(varchar,((scr + ((SCORE - scr)/rint) * rint) + rint - 1))   	" +
                //"	end as scrdif,[scApproval_flag],[CBS_REASON],[Approval_Flag],APP_NO	" +
                //"	into #Count	" +
                //"	from #Heading	" +
                //"	select scrdif,[scApproval_flag],[Approval_Flag],count(*) as 'A'	" +
                //"	into #LowsideOverrides	" +
                //"	from #Count	" +
                //"	where [CBS_REASON]='A'	" +
                //"	group by scrdif,[scApproval_flag],[Approval_Flag]	" +
                //"	select scrdif,[scApproval_flag],[Approval_Flag],count(*) as 'R'	" +
                //"	into #HighsideOverrides	" +
                //"	from #Count	" +
                //"	where [CBS_REASON]='R'	" +
                //"	group by scrdif,[scApproval_flag],[Approval_Flag]	" +
                //"	select a.*,b.R, a.A+(case when b.R is null then 0 else b.R end) as Total	" +
                //"	into #Sum_Total_N	" +
                //"	from #LowsideOverrides a left join #HighsideOverrides b	" +
                //"	on a.scrdif=b.scrdif	" +
                //"	where a.scApproval_flag='N'	" +
                //"	select case when sum(Total) is null then 0 else sum(Total) end as TN	" +
                //"	into #Sum_Total_N2	" +
                //"	from #Sum_Total_N	" +
                //"	select case when sum(A) is null then 0 else sum(A) end as [Low-side Overrides Count] 	" +
                //"	into #LowsideOverrides2	" +
                //"	from #LowsideOverrides	" +
                //"	where scApproval_flag='N'	" +
                //"	select convert(decimal(15,3),a.[Low-side Overrides Count])/case when b.TN=0 then 1 else b.TN end as [Percentage_low_side]	" +
                //"	into #Percentage_low_side	" +
                //"	from  #LowsideOverrides2 a, #Sum_Total_N2 b	" +
                //"	select case when sum(R) is null then 0 else sum(R) end as [High-Side Overrides Count]	" +
                //"	into #HighsideOverrides2	" +
                //"	from #HighsideOverrides	" +
                //"	where scApproval_flag='Y'	" +
                //"	select a.*,b.R, a.A+(case when b.R is null then 0 else b.R end) as TotalY	" +
                //"	into #Sum_Total_Y	" +
                //"	from #LowsideOverrides a left join #HighsideOverrides b	" +
                //"	on a.scrdif=b.scrdif	" +
                //"	where a.scApproval_flag='Y'	" +
                //"	select sum(TotalY) as TN	" +
                //"	into #Sum_Total_Y2	" +
                //"	from #Sum_Total_Y	" +
                //"	select convert(decimal(15,3),a.[High-Side Overrides Count])/b.TN as [Percentage_High_side]	" +
                //"	into #Percentage_High_Side	" +
                //"	from  #HighsideOverrides2 a, #Sum_Total_Y2 b	" +
                //" select a.*,b.R, a.A+(case when b.R is null then 0 else b.R end) as Total	" +
                //" into #Potential_Cal from #LowsideOverrides a left join #HighsideOverrides b on a.scrdif=b.scrdif where a.scApproval_flag='Y'	" +
                //" select sum(Total) as [Potential_Cal] into #Potential_Cal2 from #Potential_Cal 	" +
                //" select total, case when Accept is null then 0 else Accept end as Accept 	" +
                //" ,case when Rej is null then 0 else Rej end as Rej 	" +
                //" ,case when [Low-side Overrides Count] is null then 0 else [Low-side Overrides Count] end as blcut	" +
                //" ,case when [High-Side Overrides Count] is null then 0 else [High-Side Overrides Count] end as abcut	" +
                //" ,[Percentage_low_side]*100.00 as lsovln	 	" +
                //" ,case when ([Percentage_High_side]*100.00) is null then 0 else ([Percentage_High_side]*100.00) end as hsovln	" +
                //" , case when [Potential_Cal] is null then 0 else (convert(decimal(15,3),[Potential_Cal])*100.00/(case when total=0 then 1 else total end)) end as [Potential:]	 	" +
                //" ,case when Accept is null then 0 else convert(decimal(15,3),Accept)*100.00/(case when total=0 then 1 else total end) end as [Actual:]	" +
                //" from #Part1,#LowsideOverrides2,#HighsideOverrides2,#Percentage_low_side,#Percentage_High_Side,#Potential_Cal2	" +
                //"	drop table #Potential_Cal2 drop table #Potential_Cal drop table #Heading	" +
                //"	drop table #Percentage_low_side	" +
                //"	drop table #Count	" +
                //"	drop table #Part1	" +
                //"	drop table #LowsideOverrides	" +
                //"	drop table #LowsideOverrides2	" +
                //"	drop table #HighsideOverrides	" +
                //"	drop table #HighsideOverrides2	" +
                //"	drop table #Sum_Total_N	" +
                //"	drop table #Sum_Total_N2	" +
                //"	drop table #Sum_Total_Y	" +
                //"	drop table #Sum_Total_Y2	" +
                //"	drop table #Percentage_High_Side	", ("00" + ddlModel.SelectedItem.Value), ddlLoan.SelectedItem.Value, ddlSubType.SelectedItem.Value, mktqrf, mktqrt, start.ToString("yyyy-MM-dd"), end.ToString("yyyy-MM-dd hh:mm:ss"));

                //                        DataTable dt = new DataTable();

                //                        if (ddlModel.SelectedItem.Value == "7")
                //                        {
                //                            String strConnString = ConfigurationManager.ConnectionStrings["GSBSQLServer"].ConnectionString;
                //                            SqlConnection con = new SqlConnection(strConnString);
                //                            con.Open();
                //                            SqlCommand command;
                //                            command = new SqlCommand("sp_FrontEnd_FinalScoreReport_Total", con);

                //                            //DateTime date_open = DateTime.ParseExact(txtOpenDate.Text, "dd/MM/yyyy", null);
                //                            //DateTime date_close = DateTime.ParseExact(txtCloseDate.Text, "dd/MM/yyyy", null);


                //                            command.Parameters.Add(new SqlParameter("@start", ddlOpenDateYear.SelectedValue.ToString() + (ddlOpenDateMonth.SelectedIndex + 1).ToString("00")));
                //                            command.Parameters.Add(new SqlParameter("@end", ddlOpenDateYearend.SelectedValue.ToString() + (ddlOpenDateMonthend.SelectedIndex + 1).ToString("00")));
                //                            command.Parameters.Add(new SqlParameter("@ddlLoan", ddlLoan.SelectedItem.Value));
                //                            command.Parameters.Add(new SqlParameter("@ddlSubType", ddlSubType.SelectedItem.Value));
                //                            command.Parameters.Add(new SqlParameter("@mktqrf", ddlMarketFrm.Text));
                //                            command.Parameters.Add(new SqlParameter("@mktqrt", ddlMarketTo.Text));
                //                            command.Parameters.Add(new SqlParameter("@ddlModel", "00" + ddlModel.SelectedValue));
                //                            command.CommandType = CommandType.StoredProcedure;
                //                            SqlDataAdapter da = new SqlDataAdapter(command);
                //                            da.Fill(dt);
                //                        }
                //                        else
                //                        {
                //                            dt = conn.ExcuteSQL(appQuery1);
                //                        }

                //                        DataTable dtft = new DataTable();

                //                        dtft.Columns.Add("AAA", typeof(string));
                //                        dtft.Columns.Add("BBB", typeof(string));
                //                        dtft.Columns.Add("CCC", typeof(string));
                //                        dtft.Columns.Add("DDD", typeof(string));
                //                        dtft.Columns.Add("EEE", typeof(string));

                //                        dtft.Rows.Add("Total", int.Parse(dt.Rows[0][1].ToString()).ToString("N0"), int.Parse(dt.Rows[0][2].ToString()).ToString("N0"), int.Parse(dt.Rows[0][0].ToString()).ToString("N0"), " ");
                //                        dtft.Rows.Add(" ", " ", " ", " ", " ");
                //                        dtft.Rows.Add(" ", "Low-Side Overrides", "High-Side Overrides", " ", "Acceptance Rate");
                //                        dtft.Rows.Add(" ", "Count : " + int.Parse(dt.Rows[0][3].ToString()).ToString("N0"), "Count : " + int.Parse(dt.Rows[0][4].ToString()).ToString("N0"), " ", "Potential : " + Convert.ToDecimal(dt.Rows[0][7].ToString()).ToString("N2") + " %");
                //                        dtft.Rows.Add(" ", "Percentage : " + (Convert.ToDecimal(dt.Rows[0][5].ToString())).ToString("N2") + " %", "Percentage : " + (Convert.ToDecimal(dt.Rows[0][6].ToString())).ToString("N2") + " %", " ", "Actual : " + (Convert.ToDecimal(dt.Rows[0][8].ToString())).ToString("N2") + " %");
                //                        gvfooter.DataSource = dtft;
                //                        gvfooter.DataBind();
                //                        ViewState["DataTable2"] = dtft;
                                    }
                                    else
                                    {
                                        ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScript", "appPop('โปรดเลือกตัวเลือก','ค่า สินเชื่อย่อยถึง(TO) น้อยกว่า  สินเชื่อย่อยจาก(From)')", true);
                                    }
                                }
                                //-- End

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
                DataTable dtFT = (DataTable)ViewState["DataTable2"];
                Session["rptDataDTFT"] = dtFT; //Report Footer Information

                //Report header
                Session["sHeaderRpt1"] = "ประเภทโมเดล : " + ddlModel.SelectedItem.Text;
                Session["sHeaderRpt2"] = "ประเภทสินเชื่อ / ประเภทสินเชื่อย่อย : " + ddlLoan.SelectedItem.Text + " / " + ddlSubType.SelectedItem.Text;

                if (ddlMarketFrm.SelectedIndex == 0)
                    Session["sHeaderRpt3"] = "Market Code : " + ddlMarketFrm.SelectedItem.Text;
                else
                    Session["sHeaderRpt3"] = "Market Code : " + ddlMarketFrm.SelectedItem.Text + " ถึง " + ddlMarketTo.SelectedItem.Text;

                Session["sHeaderRpt4"] = "วันที่เปิดใบคำขอสินเชื่อ : " + ddlOpenDateMonth.SelectedValue + " " + ddlOpenDateYear.SelectedItem + " ถึง " + ddlOpenDateMonthend.SelectedValue + " " + ddlOpenDateYearend.SelectedItem;

                ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "aa", "window.open('ReportLayOut/frmPreviewFontRpt.aspx?ReportId=94');", true);
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
            ddlOpenDateYearend.ClearSelection();
            ddlOpenDateMonthend.ClearSelection();
            //txtOpenDate.Text = string.Empty;
            //txtCloseDate.Text = string.Empty;
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
            DataTable dtFT = (DataTable)ViewState["DataTable2"];
            Session["rptDataDTFT"] = dtFT; //Report Footer Information

            //Report header
            Session["sHeaderRpt1"] = "ประเภทโมเดล : " + ddlModel.SelectedItem.Text;
            Session["sHeaderRpt2"] = "ประเภทสินเชื่อ / ประเภทสินเชื่อย่อย : " + ddlLoan.SelectedItem.Text + " / " + ddlSubType.SelectedItem.Text;

            if (ddlMarketFrm.SelectedIndex == 0)
                Session["sHeaderRpt3"] = "Market Code : " + ddlMarketFrm.SelectedItem.Text;
            else
                Session["sHeaderRpt3"] = "Market Code : " + ddlMarketFrm.SelectedItem.Text + " ถึง " + ddlMarketTo.SelectedItem.Text;

            Session["sHeaderRpt4"] = "วันที่เปิดใบคำขอสินเชื่อ : " + ddlOpenDateMonth.SelectedValue + " " + ddlOpenDateYear.SelectedItem + " ถึง " + ddlOpenDateMonthend.SelectedValue + " " + ddlOpenDateYearend.SelectedItem;


            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "aa", "window.open('ReportLayOut/frmPreviewFontRpt.aspx?ReportId=4');", true);
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