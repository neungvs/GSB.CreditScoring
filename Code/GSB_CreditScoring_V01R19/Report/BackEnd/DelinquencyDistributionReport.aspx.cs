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
    public partial class DelinquencyDistributionReport : System.Web.UI.Page
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
                LoadYear();
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



            for (int i = 0; i < 10; i++)
            {
                ddlOpenDateYearDTM.Items.Insert(i, new ListItem((ybud - i).ToString(), (ychis - i).ToString()));
                ddlOpenDateYearDTMend.Items.Insert(i, new ListItem((ybud - i).ToString(), (ychis - i).ToString()));
            }

            ddlOpenDateMonthDTM.DataSource = GetMonth();
            ddlOpenDateMonthDTM.DataBind();
            ddlOpenDateMonthDTMend.DataSource = GetMonth();
            ddlOpenDateMonthDTMend.DataBind();

        }

        protected void LoadYear()
        {
            string sql = "";
sql = string.Format("SELECT convert(integer,substring(convert(varchar,CONVERT (DATE, SYSDATETIME())),1,4))-4 as YEAR_ENG,convert(integer,substring(convert(varchar,CONVERT (DATE, SYSDATETIME())),1,4))+543-4 as YEAR_THAI union ");
sql = sql + "select convert(integer,substring(convert(varchar,CONVERT (DATE, SYSDATETIME())),1,4))-3,convert(integer,substring(convert(varchar,CONVERT (DATE, SYSDATETIME())),1,4))+543-3 union ";
sql = sql + "select convert(integer,substring(convert(varchar,CONVERT (DATE, SYSDATETIME())),1,4))-2,convert(integer,substring(convert(varchar,CONVERT (DATE, SYSDATETIME())),1,4))+543-2 union ";
sql = sql + "select convert(integer,substring(convert(varchar,CONVERT (DATE, SYSDATETIME())),1,4))-1,convert(integer,substring(convert(varchar,CONVERT (DATE, SYSDATETIME())),1,4))+543-1 union ";
sql = sql + "select convert(integer,substring(convert(varchar,CONVERT (DATE, SYSDATETIME())),1,4)),convert(integer,substring(convert(varchar,CONVERT (DATE, SYSDATETIME())),1,4))+543 union ";
sql = sql + "select convert(integer,substring(convert(varchar,CONVERT (DATE, SYSDATETIME())),1,4))+1,convert(integer,substring(convert(varchar,CONVERT (DATE, SYSDATETIME())),1,4))+543+1 union ";
sql = sql + "select convert(integer,substring(convert(varchar,CONVERT (DATE, SYSDATETIME())),1,4))+2,convert(integer,substring(convert(varchar,CONVERT (DATE, SYSDATETIME())),1,4))+543+2 union ";
sql = sql + "select convert(integer,substring(convert(varchar,CONVERT (DATE, SYSDATETIME())),1,4))+3,convert(integer,substring(convert(varchar,CONVERT (DATE, SYSDATETIME())),1,4))+543+3 ";
            txtEndAcDate.DataSource = conn.ExcuteSQL(sql);
            txtEndAcDate.DataTextField = "YEAR_THAI";
            txtEndAcDate.DataValueField = "YEAR_ENG";
            txtEndAcDate.DataBind();
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

            string filename = string.Format("GSB-CS_DelinquencyDistributionReport_(Date_{0}-Time_{1}).xls", DateTime.Now.ToString("dd-MM-yyyy"), DateTime.Now.ToString("HH-mm-ss"));
            StringWriter tw = new StringWriter();
            DataGrid dgGrid = new DataGrid();

            tw.Write(string.Format("Delinquency Distribution Report"));
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
            tw.Write(string.Format("Open Account Date :| {0} ปี พ.ศ. {1} ", txtOpenAcDate.SelectedItem.Text.ToString(), (Convert.ToInt32(txtEndAcDate.SelectedItem.Text)).ToString()));
            tw.Write(tw.NewLine);
            tw.Write(string.Format("As of Date :| {0} ", ""));
            tw.Write(tw.NewLine);
            tw.Write(string.Format("Export Date :| {0} : {1} ", DateTime.Now.ToString("dd-MM-yyyy"), DateTime.Now.ToString("HH-mm-ss")));
            tw.Write(tw.NewLine);
            tw.Write("ผู้พิมพ์ " + ExportUtility.getCurrentUserId);
            tw.Write(tw.NewLine);

            int iColCount = new int();
            iColCount = dttb.Columns.Count;

            tw.Write("Score Range|% Accepted Accounts|% Active Accounts| % Current 1-30 Days|% Current 31-60 Days| % Current 61-90 Days");
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
                        tw.Write("0.00");
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

        //    string Query = string.Format("select distinct " + "CONVERT(int,LOAN_CD) [LOAN_CD],  " +
        //"LOAN_CD+' - '+LOAN_NAME [LOAN_NAME] from ST_LOANTYPE a,ST_SCORERANGE b where ACTIVE_FLAG = '1' and a.loan_cd=b.ltype " +
        //" and [MODEL_CD]=" + ddlModel.SelectedValue + " " +
        //"order by LOAN_CD ASC");

        //    ddlLoan.DataSource = conn.ExcuteSQL(Query);
        //    ddlLoan.DataBind();
        //    ddlLoan.Items.Insert(0, new ListItem("โปรดเลือก...", string.Empty));
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

        protected void ChgOpenAcDt(object sender, EventArgs e)
        {
            DateTime startAc = new DateTime();
            DateTime EndAc = new DateTime();
            startAc = DateTime.Parse(txtOpenAcDate.Text.ToString());
            EndAc = startAc.AddMonths(+3);
            string Query = string.Format("select '{0}' [DT_NAME] ", EndAc.ToString("yyyy-MM-dd")); 
            txtEndAcDate.Items.Clear();
            txtEndAcDate.DataSource = conn.ExcuteSQL(Query);
            txtEndAcDate.DataBind();
            txtEndAcDate.Enabled = true  ;            
        }

        protected void gvTable_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                //GridView HeaderGrid = (GridView)sender;

                //e.Row.Cells.Clear();
                //GridViewRow HeaderGridRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);

                //TableCell HeaderCell = new TableCell();
                //HeaderCell.Text = "";
                //HeaderGridRow.Cells.Add(HeaderCell);

                //HeaderCell = new TableCell();
                //HeaderCell.Text = "% Delinquency";
                //HeaderCell.ColumnSpan = 10;
                //HeaderGridRow.Cells.Add(HeaderCell);

                //gvTable.Controls[0].Controls.AddAt(0, HeaderGridRow);
            }
        }

        protected void Search_Click(object sender, EventArgs e)
        {
            IFormatProvider format = new CultureInfo("en-GB");
            DateTime dateOpenLoan = DateTime.MinValue;

            StringBuilder sqlQuery = new StringBuilder();
            string mktqrf = "";
            string mktqrt = "";
            DateTime start = new DateTime();
            DateTime close = new DateTime();
            String end3m = "";
            String end12yg  = "";

            if (ddlLoan.SelectedIndex <= 0 || ddlSubType.SelectedIndex <= 0 || ddlMarketFrm.SelectedIndex <= 0 || ddlMarketTo.SelectedIndex <= 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScript", "appPop('โปรดเลือกตัวเลือก','กรุณาเลือกเงื่อนไขการค้นหาให้ครบถ้วน ')", true);
                return;
            }

            int monthstartDTM = Convert.ToInt16(ddlOpenDateYearDTM.SelectedValue) * 100 + ddlOpenDateMonthDTM.SelectedIndex;
            int monthendDTM = Convert.ToInt16(ddlOpenDateYearDTMend.SelectedValue) * 100 + ddlOpenDateMonthDTMend.SelectedIndex;

            if (monthstartDTM > monthendDTM)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScript", "appPop('วันที่เปิดใบคำขอสินเชื่อไม่ถูกต้อง?','เลือกวันที่เปิดใบคำขอไม่ถูกต้อง')", true);
                return;
            }

            //if (!(txtOpenDate.Text == ""))
            //{
            //    start = DateTime.Parse((txtOpenDate.Text.ToString().Substring(0, 6) + (Convert.ToInt32(txtOpenDate.Text.Substring(6, 4).ToString()) - 543).ToString()));
            //    close = DateTime.Parse((txtCloseDate.Text.ToString().Substring(0, 6) + (Convert.ToInt32(txtCloseDate.Text.Substring(6, 4).ToString()) - 543).ToString()));

            //}
            //else
            //{
            //    start = DateTime.Now;
            //}

            if (txtOpenAcDate.SelectedValue == "1")
            {
                end3m = txtEndAcDate.SelectedValue.ToString()+"01";
                end12yg = txtEndAcDate.SelectedValue.ToString()+"03";
                //end3m = "01/01/" + txtEndAcDate.SelectedValue.ToString();
                //end12yg = "31/03/" + txtEndAcDate.SelectedValue.ToString();
            }
            if (txtOpenAcDate.SelectedValue == "2")
            {
                end3m = txtEndAcDate.SelectedValue.ToString() + "04";
                end12yg = txtEndAcDate.SelectedValue.ToString() + "06";
                //end3m = "01/04/" + txtEndAcDate.SelectedValue.ToString();
                //end12yg = "30/06/" + txtEndAcDate.SelectedValue.ToString();
            }
            if (txtOpenAcDate.SelectedValue == "3")
            {
                end3m = txtEndAcDate.SelectedValue.ToString() + "07";
                end12yg = txtEndAcDate.SelectedValue.ToString() + "09";
                //end3m = "01/07/" + txtEndAcDate.SelectedValue.ToString();
                //end12yg = "30/09/" + txtEndAcDate.SelectedValue.ToString();
            }
            if (txtOpenAcDate.SelectedValue == "4")
            {
                end3m = txtEndAcDate.SelectedValue.ToString() + "10";
                end12yg = txtEndAcDate.SelectedValue.ToString() + "12";
                //end3m = "01/10/" + txtEndAcDate.SelectedValue.ToString();
                //end12yg = "31/12/" + txtEndAcDate.SelectedValue.ToString();
            }


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
                                appQuery1 = string.Format("select [CBS_APP_NO],convert(varchar,Max_Date, 103) as [Max_LAST_UPDATE_DTM]	" +
        "	into #temp	" +
        "	from	" +
        "	(	" +
        "	SELECT [CBS_APP_NO],max(convert(datetime,substring([LAST_UPDATE_DTM],7,4)+'-'+substring([LAST_UPDATE_DTM],4,2)+'-'+substring([LAST_UPDATE_DTM],1,2))) as Max_Date	" +
        "	  FROM DWH_BTFILE A	" +
        "	  where 	" +
        "	convert(int,(substring(right([LAST_UPDATE_DTM],7),4,4) + substring(right([LAST_UPDATE_DTM],7),1,2)))	" +
        "	= convert(int,(substring('{4}',4,4) + substring('{4}',1,2)))	" +
        "	group by [CBS_APP_NO]	" +
        "	) a	" +
        "		" +
        "		" +
        "	select APP_NO,APP_DATE,LOAN_CD,STYPE_CD,CBS_STATUS,DW_SIGN_DATE,LAST_UPDATE_DTM,ISACTIVE,DW_BADMONTH,B1_Flag,B2_Flag,B3_Flag	" +
        "	,case when (SCORE - cut_off) < 0 then convert(varchar,(cut_off + ((((SCORE + 1) - cut_off)/rint) -1 ) * rint)) 	" +
        "	+ '-' + convert(varchar,((cut_off + ((((SCORE + 1 ) - cut_off)/rint)-1 ) * rint) + rint - 1)) 	" +
        "	 when (SCORE - cut_off) >= 0 then convert(varchar,(cut_off + ((SCORE - cut_off)/rint) * rint)) + '-' 	" +
        "	 + convert(varchar,((cut_off + ((SCORE - cut_off)/rint) * rint) + rint - 1))  	" +
        "	 end as scrdif 	" +
        //Tai
        "   , DW_OUTSTAND " +
        //
        "	 into #Main_Program	" +
        "	from	" +
        "	(	" +
        "	SELECT a.*,b.score,(select cut_off from ST_SCORERANGE y where  y.ltype= '{0}' and y.lstype = '{1}' ) as cut_off,	" +
        "	(select rint from ST_SCORERANGE y where  y.ltype= '{0}' and y.lstype = '{1}' ) as rint 	" +
        "	from LN_APP a,LN_GRADE b,CBS_LN_APP c 	" +
        "	where a.APP_NO = b.APP_NO  and a.app_no = c.CBS_APP_NO 	" +
        "   and LOAN_CD = '{0}' and STYPE_CD = '{1}'  " +
        " {2} " +
        " {3} " +
        "	) lnapp	" +
        "	inner join 	" +
        "	( 	" +
        "	select *	" +
        "	from	" +
        "	(	" +
        "	select A.[CBS_APP_NO], A.DW_SIGN_DATE,A.LAST_UPDATE_DTM,A.ISACTIVE,DW_BADMONTH	" +
        "	,case when DW_BADMONTH=1 then 'Y' else 'N' end as B1_Flag	" +
        "	,case when DW_BADMONTH=2 then 'Y' else 'N' end as B2_Flag	" +
        "	,case when DW_BADMONTH=3 then 'Y' else 'N' end as B3_Flag	" +
        //Tai 2013-12-26
        "   , DW_OUTSTAND " +
        //
        "	from DWH_BTFILE A, #temp B	" +
        //Tai 2014-01-02
        //"	where convert(int,(substring(right([LAST_UPDATE_DTM],7),4,4) + substring(right([LAST_UPDATE_DTM],7),1,2)))	" +
        //"	= convert(int,(substring('{4}',4,4) + substring('{4}',1,2)))	" +
        //"	and convert(int,(substring(right([DW_SIGN_DATE],7),4,4) + substring(right([DW_SIGN_DATE],7),1,2)))	" +
        "	where convert(int,(substring(right([DW_SIGN_DATE],7),4,4) + substring(right([DW_SIGN_DATE],7),1,2)))	" +
        "	between convert(int,(substring('{5}',7,4) + substring('{5}',4,2))) 	" +
        "	and convert(int,(substring('{6}',7,4) + substring('{6}',4,2)))	" +
        "	and A.[CBS_APP_NO]=B.[CBS_APP_NO] and A.[LAST_UPDATE_DTM]=B.[Max_LAST_UPDATE_DTM]	" +
        "		" +
        "	) DW_BFILE_T	" +
        "	) gsb2T	" +
        "	on gsb2T.CBS_APP_NO = lnapp.APP_NO	" +
        "		" +
        "	select 1 as seq,scrdif,TotalAccount,AcceptAccount,ActiveAccount,B1_Flag_N,B1_Flag_Y,B2_Flag_N,B2_Flag_Y,B3_Flag_N,B3_Flag_Y	" +
        "	into #MainTable	" +
        "	from	" +
        "	(	" +
        "	select Q.*,case when R.B3_Flag_Y is null then 0 else R.B3_Flag_Y end as B3_Flag_Y	" +
        "	from	" +
        "	(	" +
        "	select O.*,case when P.B3_Flag_N is null then 0 else P.B3_Flag_N end as B3_Flag_N 	" +
        "	from	" +
        "	(	" +
        "	select M.*,case when N.B2_Flag_Y is null then 0 else N.B2_Flag_Y end as B2_Flag_Y	" +
        "	from	" +
        "	(	" +
        "	select K.*,case when L.B2_Flag_N is null then 0 else L.B2_Flag_N end as B2_Flag_N	" +
        "	from	" +
        "	(	" +
        "	select I.*,case when J.B1_Flag_Y is null then 0 else J.B1_Flag_Y end as B1_Flag_Y	" +
        "	from	" +
        "	(	" +
        "	select G.*	" +
        "	,case when H.B1_Flag_N is null then 0 else H.B1_Flag_N end as B1_Flag_N	" +
        "	from	" +
        "	(	" +
        "	select E.*	" +
        "	,case when F.ActiveAccount is null then 0 else F.ActiveAccount end as ActiveAccount	" +
        "	from	" +
        "	(	" +
        "	select C.*,D.AcceptAccount 	" +
        "	from	" +
        "	( 	" +
        "	 select A.*,B.TotalAccount 	" +
        "	 from	" +
        "	 (	" +
        "	select distinct case when (SCORE - cut_off) < 0 then convert(varchar,(cut_off + ((((SCORE + 1) - cut_off)/rint) -1 ) * rint)) 	" +
        "	+ '-' + convert(varchar,((cut_off + ((((SCORE + 1 ) - cut_off)/rint)-1 ) * rint) + rint - 1)) 	" +
        "	 when (SCORE - cut_off) >= 0 then convert(varchar,(cut_off + ((SCORE - cut_off)/rint) * rint)) + '-' 	" +
        "	 + convert(varchar,((cut_off + ((SCORE - cut_off)/rint) * rint) + rint - 1))  	" +
        "	 end as scrdif	" +
        "	 from	" +
        "	(	" +
        "	SELECT a.*,b.score,(select cut_off from ST_SCORERANGE y where  y.ltype= '{0}' and y.lstype = '{1}' ) as cut_off,	" +
        "	(select rint from ST_SCORERANGE y where  y.ltype= '{0}' and y.lstype = '{1}' ) as rint 	" +
        "	from LN_APP a,LN_GRADE b,CBS_LN_APP c 	" +
        "	where a.APP_NO = b.APP_NO  and a.app_no = c.CBS_APP_NO 	" +
        "   and LOAN_CD = '{0}' and STYPE_CD = '{1}'  " +
        " {2} " +
        " {3} " +
        "	) main_scrange	" +
        "	 ) A	" +
        "	 left join	" +
        "	 (	" +
        "	select scrdif,count(*) as TotalAccount 	" +
        "	from #Main_Program	" +
        "	group by scrdif	" +
        "	) B	" +
        "	on A.scrdif=B.scrdif	" +
        "	) C	" +
        "	left join	" +
        "	(	" +
        "	select scrdif,count(*) as AcceptAccount  	" +
        "	from #Main_Program	" +
        "	where CBS_STATUS in (3,6,7) 	" +
        "	group by scrdif	" +
        "	) D	" +
        "	on C.scrdif=D.scrdif	" +
        "	) E	" +
        "	left join	" +
        "	(	" +
        "	select scrdif,count(*) as ActiveAccount 	" +
        "	from #Main_Program	" +
        "	where CBS_STATUS in (3,6,7) and ISACTIVE=0	" +
        //Tai 2014-01-02
        "	and convert(int,(substring(right([LAST_UPDATE_DTM],7),4,4) + substring(right([LAST_UPDATE_DTM],7),1,2)))	" +
        "	= convert(int,(substring('{4}',4,4) + substring('{4}',1,2)))	" +
        //
        //Tai 2013-12-26
        "   AND convert(decimal(18,2),DW_OUTSTAND)>0 " +
        //
        "	group by scrdif	" +
        "	) F	" +
        "	on E.scrdif=F.scrdif	" +
        "	) G	" +
        "	left join	" +
        "	(	" +
        "	select scrdif,count(*) as B1_Flag_N	" +
        "	from #Main_Program	" +
        "	where CBS_STATUS in (3,6,7) and ISACTIVE=0 	" +
        //Tai 2014-01-02
        "	and convert(int,(substring(right([LAST_UPDATE_DTM],7),4,4) + substring(right([LAST_UPDATE_DTM],7),1,2)))	" +
        "	= convert(int,(substring('{4}',4,4) + substring('{4}',1,2)))	" +
        //
        "	and B1_Flag='N'	" +
        //Tai 2013-12-26
        "   and convert(decimal(18,2),DW_OUTSTAND)>0 " +
        //
        "	group by scrdif	" +
        "	) H 	" +
        "	on G.scrdif=H.scrdif	" +
        "	) I	" +
        "	left join	" +
        "	(	" +
        "	select scrdif,count(*) as B1_Flag_Y 	" +
        "	from #Main_Program	" +
        "	where CBS_STATUS in (3,6,7) and ISACTIVE=0 	" +
        //Tai 2014-01-02
        "	and convert(int,(substring(right([LAST_UPDATE_DTM],7),4,4) + substring(right([LAST_UPDATE_DTM],7),1,2)))	" +
        "	= convert(int,(substring('{4}',4,4) + substring('{4}',1,2)))	" +
        //
        "	and B1_Flag='Y'	" +
        //Tai 2013-12-26
        "   AND convert(decimal(18,2),DW_OUTSTAND)>0 " +
        //
        "	group by scrdif	" +
        "	) J	" +
        "	on I.scrdif=J.scrdif	" +
        "	) K	" +
        "	left join	" +
        "	(	" +
        "	select scrdif,count(*) as B2_Flag_N 	" +
        "	from #Main_Program	" +
        "	where CBS_STATUS in (3,6,7) and ISACTIVE=0 	" +
        //Tai 2014-01-02
        "	and convert(int,(substring(right([LAST_UPDATE_DTM],7),4,4) + substring(right([LAST_UPDATE_DTM],7),1,2)))	" +
        "	= convert(int,(substring('{4}',4,4) + substring('{4}',1,2)))	" +
        //
        "	and B2_Flag='N'	" +
        //Tai 2013-12-26
        "   AND convert(decimal(18,2),DW_OUTSTAND)>0 " +
        //
        "	group by scrdif	" +
        "	) L	" +
        "	on K.scrdif=L.scrdif	" +
        "	) M	" +
        "	left join	" +
        "	(	" +
        "	select scrdif,count(*) as B2_Flag_Y 	" +
        "	from #Main_Program	" +
        "	where CBS_STATUS in (3,6,7) and ISACTIVE=0 	" +
        //Tai 2014-01-02
        "	and convert(int,(substring(right([LAST_UPDATE_DTM],7),4,4) + substring(right([LAST_UPDATE_DTM],7),1,2)))	" +
        "	= convert(int,(substring('{4}',4,4) + substring('{4}',1,2)))	" +
        //
        "	and B2_Flag='Y'	" +
        //Tai 2013-12-26
        "   AND convert(decimal(18,2),DW_OUTSTAND)>0 " +
        //
        "	group by scrdif	" +
        "	) N	" +
        "	on M.scrdif=N.scrdif	" +
        "	) O	" +
        "	left join	" +
        "	(	" +
        "	select scrdif,count(*) as B3_Flag_N 	" +
        "	from #Main_Program	" +
        "	where CBS_STATUS in (3,6,7) and ISACTIVE=0 	" +
        //Tai 2014-01-02
        "	and convert(int,(substring(right([LAST_UPDATE_DTM],7),4,4) + substring(right([LAST_UPDATE_DTM],7),1,2)))	" +
        "	= convert(int,(substring('{4}',4,4) + substring('{4}',1,2)))	" +
        //
        "	and B3_Flag='N'	" +
        //Tai 2013-12-26
        "   AND convert(decimal(18,2),DW_OUTSTAND)>0 " +
        //
        "	group by scrdif	" +
        "	) P	" +
        "	on O.scrdif=P.scrdif	" +
        "	) Q	" +
        "	left join 	" +
        "	(	" +
        "	select scrdif,count(*) as B3_Flag_Y 	" +
        "	from #Main_Program	" +
        "	where CBS_STATUS in (3,6,7) and ISACTIVE=0 	" +
        //Tai 2014-01-02
        "	and convert(int,(substring(right([LAST_UPDATE_DTM],7),4,4) + substring(right([LAST_UPDATE_DTM],7),1,2)))	" +
        "	= convert(int,(substring('{4}',4,4) + substring('{4}',1,2)))	" +
        //
        "	and B3_Flag='Y'	" +
        //Tai 2013-12-26
        "   AND convert(decimal(18,2),DW_OUTSTAND)>0 " +
        //
        "	group by scrdif	" +
        "	) R	" +
        "	on Q.scrdif=R.scrdif	" +
        "	) S	" +
        "	order by S.scrdif	" +
        " select seq,A.scrdif as scrdif,TotalAccount,AcceptAccount,ActiveAccount,B1_Flag_N,B1_Flag_Y,B2_Flag_N,B2_Flag_Y,B3_Flag_N,B3_Flag_Y into #MainTable_Temp	" +
        " from ( select range_name as scrdif from st_scorernglst where range_cd = (select range_cd from [ST_SCORERANGE] where ltype= '{0}' and lstype = '{1}')	" +
        " ) A left join ( select * from #MainTable) B on A.scrdif=B.scrdif " +
        "	select 'Total' as scrdif,sum(TotalAccount) as T_TotalAccount,sum(AcceptAccount) as T_AcceptAccount,sum(ActiveAccount) as T_ActiveAccount	" +
        "	,sum(B1_Flag_N) as T_B1_Flag_N,sum(B1_Flag_Y) as T_B1_Flag_Y,sum(B2_Flag_N) as T_B2_Flag_N,sum(B2_Flag_Y) as T_B2_Flag_Y	" +
        "	,sum(B3_Flag_N) as T_B3_Flag_N,sum(B3_Flag_Y) as T_B3_Flag_Y	" +
        "	into #MainTableTotal	" +
        "	from #MainTable_Temp " +
        //"	select A.scrdif as category,(convert(decimal(15,3),A.TotalAccount)/B.T_TotalAccount)*100.0 as acptacc	" +
        //"	,(convert(decimal(15,3),A.ActiveAccount)/A.TotalAccount)*100.0 as atvacpt	" +
        //Tai
	    "   select A.scrdif as category,(convert(decimal(15,3),A.AcceptAccount)/B.T_AcceptAccount)*100.0 as acptacc "+
        "   ,(convert(decimal(15,3),A.ActiveAccount)/A.AcceptAccount)*100.0 as atvacpt " +
        //
        "	,(convert(decimal(15,3),A.B1_Flag_Y)/case when (A.B1_Flag_N+A.B1_Flag_Y)=0 then 1 else (A.B1_Flag_N+A.B1_Flag_Y) end)*100.0 as BM1	" +
        "	,(convert(decimal(15,3),A.B2_Flag_Y)/case when (A.B2_Flag_N+A.B2_Flag_Y)=0 then 1 else (A.B2_Flag_N+A.B2_Flag_Y) end)*100.0 as BM2	" +
        "	,(convert(decimal(15,3),A.B3_Flag_Y)/case when (A.B3_Flag_N+A.B3_Flag_Y)=0 then 1 else (A.B3_Flag_N+A.B3_Flag_Y) end)*100.0 as BM3	" +
        "	into #MainTable1 from #MainTable_Temp A,#MainTableTotal B	" +
        "	select 'Total' as category,null as acptacc	" +
        //" ,convert(decimal(15,3),T_ActiveAccount)*100.00/(case when T_TotalAccount is null then 1 else T_TotalAccount end) as atvacpt	" +
        //Tai
        "   ,convert(decimal(15,3),T_ActiveAccount)*100.00/(case when T_AcceptAccount is null then 1 else T_AcceptAccount end) as atvacpt " +
        //
        " ,(convert(decimal(15,3),T_B1_Flag_Y)*100.00/(case when (T_B1_Flag_Y+T_B1_Flag_N)=0 then 1 else (T_B1_Flag_Y+T_B1_Flag_N) end))as BM1	" +
        " ,(convert(decimal(15,3),T_B2_Flag_Y)*100.00/case when (T_B2_Flag_Y+T_B2_Flag_N)=0 then 1 else (T_B2_Flag_Y+T_B2_Flag_N) end)as BM2	" +
        " ,(convert(decimal(15,3),T_B3_Flag_Y)*100.00/case when (T_B3_Flag_Y+T_B3_Flag_N)=0 then 1 else (T_B3_Flag_Y+T_B3_Flag_N) end)as BM3	" +
        " into #Tatal_line from #MainTableTotal	" +
        " update #Tatal_line set acptacc=(select round(sum(acptacc),1) as acptacc from #MainTable1) where acptacc is null " +
        " select * into #Excel_Format from #MainTable1 union select * from #Tatal_line " +
        " select category,convert(varchar,convert(decimal(15,2),CASE WHEN acptacc IS NULL THEN '0.00' ELSE acptacc END)) as acptacc " +
        " ,convert(varchar,convert(decimal(15,2),CASE WHEN atvacpt IS NULL THEN '0.00' ELSE atvacpt END)) as atvacpt " +
        " ,convert(varchar,convert(decimal(15,2),CASE WHEN BM1 IS NULL THEN '0.00' ELSE BM1 END)) as BM1 " +
        " ,convert(varchar,convert(decimal(15,2),CASE WHEN BM2 IS NULL THEN '0.00' ELSE BM2 END)) as BM2  " +
        " ,convert(varchar,convert(decimal(15,2),CASE WHEN BM3 IS NULL THEN '0.00' ELSE BM3 END)) as BM3   from  #Excel_Format " +
        "	drop table #temp drop table #Excel_Format drop table #Tatal_line   " +
        "	drop table #Main_Program drop table  #MainTable1	" +
        "	drop table #MainTable	drop table #MainTable_Temp	 " +
        "	drop table #MainTableTotal	"
        , ddlLoan.SelectedItem.Value, ddlSubType.SelectedItem.Value, mktqrf, mktqrt, start.ToString("MM/yyyy"), end3m.ToString(), end12yg.ToString());


                                    DataTable dt = new DataTable();

                                    if (ddlModel.SelectedItem.Value == "7")
                                    {
                                        String strConnString = ConfigurationManager.ConnectionStrings["GSBSQLServer"].ConnectionString;
                                        SqlConnection con = new SqlConnection(strConnString);
                                        con.Open();
                                        SqlCommand command;
                                        command = new SqlCommand("sp_BackEnd_DelinquencyDistributionReport", con);

                                        command.Parameters.Add(new SqlParameter("@LASTUPDATEDTM_Start", ddlOpenDateYearDTM.SelectedValue.ToString() + (ddlOpenDateMonthDTM.SelectedIndex + 1).ToString("00")));
                                        command.Parameters.Add(new SqlParameter("@LASTUPDATEDTM_End", ddlOpenDateYearDTMend.SelectedValue.ToString() + (ddlOpenDateMonthDTMend.SelectedIndex + 1).ToString("00")));
                                        command.Parameters.Add(new SqlParameter("@DWSIGNDATE_Start", end3m.ToString()));
                                        command.Parameters.Add(new SqlParameter("@DWSIGNDATE_End", end12yg.ToString()));
                                        command.Parameters.Add(new SqlParameter("@ddlLoan", ddlLoan.SelectedItem.Value));
                                        command.Parameters.Add(new SqlParameter("@ddlSubType", ddlSubType.SelectedItem.Value));
                                        command.Parameters.Add(new SqlParameter("@mktqrf", ddlMarketFrm.Text));
                                        command.Parameters.Add(new SqlParameter("@mktqrt", ddlMarketTo.Text));
                                        command.CommandType = CommandType.StoredProcedure;
                                        command.CommandTimeout = 36000;
                                        SqlDataAdapter da = new SqlDataAdapter(command);
                                        da.Fill(dt);

                                        if (dt.Rows[0][0].ToString() == "data not found")
                                        {
                                            ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScript", "appPop('โปรดเลือกตัวเลือก','ไม่พบข้อมูล กรุณาเลือกเงื่อนไขใหม่')", true);
                                            return;
                                        }

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
                int endyear;
                //Generate report header
                Session["sHeaderRpt1"] = "ประเภทโมเดล : " + ddlModel.SelectedItem.Text;
                Session["sHeaderRpt2"] = "ประเภทสินเชื่อ / ประเภทสินเชื่อย่อย : " + ddlLoan.SelectedItem.Text + " / " + ddlSubType.SelectedItem.Text;

                if (ddlMarketFrm.SelectedIndex == 0)
                    Session["sHeaderRpt3"] = "Market Code : " + ddlMarketFrm.SelectedItem.Text;
                else
                    Session["sHeaderRpt3"] = "Market Code : " + ddlMarketFrm.SelectedItem.Text + " ถึง " + ddlMarketTo.SelectedItem.Text;

                endyear = Convert.ToInt32(txtEndAcDate.Text) + 543;
                Session["sHeaderRpt4"] = "ช่วงเดือนที่เปิดบัญชีสินเชื่อ : " + txtOpenAcDate.SelectedItem.Text + " / ปี : " + endyear;
                Session["sHeaderRpt5"] = "As of Date : " + (ddlOpenDateMonthDTM.SelectedItem) + " " + ddlOpenDateYearDTM.SelectedItem + " ถึง " + (ddlOpenDateMonthDTMend.SelectedItem) + " " + ddlOpenDateYearDTMend.SelectedItem;



                //Regenerate data for report
                DataTable dt = (DataTable)Session["dtb"];
                Session["rptData"] = dt;
                ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "aa", "window.open('ReportLayOut/frmPreviewBackRpt.aspx?ReportId=94');", true);
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
            int endyear;
            //Generate report header
            Session["sHeaderRpt1"] = "ประเภทโมเดล : " + ddlModel.SelectedItem.Text;
            Session["sHeaderRpt2"] = "ประเภทสินเชื่อ / ประเภทสินเชื่อย่อย : " + ddlLoan.SelectedItem.Text + " / " + ddlSubType.SelectedItem.Text;

            if (ddlMarketFrm.SelectedIndex == 0)
                Session["sHeaderRpt3"] = "Market Code : " + ddlMarketFrm.SelectedItem.Text;
            else
                Session["sHeaderRpt3"] = "Market Code : " + ddlMarketFrm.SelectedItem.Text + " ถึง " + ddlMarketTo.SelectedItem.Text;
            
            endyear = Convert.ToInt32(txtEndAcDate.Text) + 543;
            Session["sHeaderRpt4"] = "ช่วงเดือนที่เซ็นสัญญา : " + txtOpenAcDate.SelectedItem.Text + " / ปี : " +endyear;
            Session["sHeaderRpt5"] = "As of Date : " + (ddlOpenDateMonthDTM.SelectedItem) + " " + ddlOpenDateYearDTM.SelectedItem + " ถึง " + (ddlOpenDateMonthDTMend.SelectedItem) + " " + ddlOpenDateYearDTMend.SelectedItem;


            //Regenerate data for report
            DataTable dt = (DataTable)Session["dtb"];
            Session["rptData"] = dt;
            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "aa", "window.open('ReportLayOut/frmPreviewBackRpt.aspx?ReportId=4');", true);
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