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
    public partial class VintageAnalysisReport : System.Web.UI.Page
    {
        SQLToDataTable conn = new SQLToDataTable();
        string appQuery1;
        #region Page_Load

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                LoadModel();
                //LoadModelVersion();
                LoadYear();
                ScriptManager _scriptMan = ScriptManager.GetCurrent(this);
                _scriptMan.AsyncPostBackTimeout = 36000;
            }
        }

        #endregion Page_Load

        #region Private Method

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

            string filename = string.Format("GSB-CS_VintageAnalysisReport_(Date_{0}-Time_{1}).xls", DateTime.Now.ToString("dd-MM-yyyy"), DateTime.Now.ToString("HH-mm-ss"));
            StringWriter tw = new StringWriter();
            DataGrid dgGrid = new DataGrid();

            tw.Write(string.Format("Vintage Analysis Report"));
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
            tw.Write(string.Format("Export Date :| {0} : {1} ", DateTime.Now.ToString("dd-MM-yyyy"), DateTime.Now.ToString("HH-mm-ss")));
            tw.Write(tw.NewLine);
            tw.Write("ผู้พิมพ์ " + ExportUtility.getCurrentUserId);
            tw.Write(tw.NewLine);

            int iColCount = new int();
            iColCount = dttb.Columns.Count;
            
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

        //protected void LoadYear(object sender, EventArgs e)
        //{
        //    string sql = "";
        //    sql = string.Format("SELECT convert(integer,substring(convert(varchar,CONVERT (DATE, SYSDATETIME())),1,4))-3 as YEAR_ENG,convert(integer,substring(convert(varchar,CONVERT (DATE, SYSDATETIME())),1,4))+543-3 as YEAR_THAI into #temp");
        //    sql = sql + "insert into #temp values (convert(integer,substring(convert(varchar,CONVERT (DATE, SYSDATETIME())),1,4))-2,convert(integer,substring(convert(varchar,CONVERT (DATE, SYSDATETIME())),1,4))+543-2 )";
        //    sql = sql + "insert into #temp values (convert(integer,substring(convert(varchar,CONVERT (DATE, SYSDATETIME())),1,4))-1,convert(integer,substring(convert(varchar,CONVERT (DATE, SYSDATETIME())),1,4))+543-1 )";
        //    sql = sql + "insert into #temp values (convert(integer,substring(convert(varchar,CONVERT (DATE, SYSDATETIME())),1,4)),convert(integer,substring(convert(varchar,CONVERT (DATE, SYSDATETIME())),1,4))+543 )";
        //    sql = sql + "insert into #temp values (convert(integer,substring(convert(varchar,CONVERT (DATE, SYSDATETIME())),1,4))+2,convert(integer,substring(convert(varchar,CONVERT (DATE, SYSDATETIME())),1,4))+543+2 )";
        //    sql = sql + "insert into #temp values (convert(integer,substring(convert(varchar,CONVERT (DATE, SYSDATETIME())),1,4))+1,convert(integer,substring(convert(varchar,CONVERT (DATE, SYSDATETIME())),1,4))+543+1 )";
        //    sql = sql + " SELECT YEAR_ENG,YEAR_THAI FROM #TEMP ORDER BY YEAR_ENG";

        //    txtEndAcDate.DataSource = conn.ExcuteSQL(sql);
        //    txtOpenAcDate.DataTextField = "YEAR_ENG";
        //    txtOpenAcDate.DataValueField = "YEAR_THAI";
        //    txtEndAcDate.DataBind();
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
            txtEndAcDate.Enabled = true;
        }

        protected void gvTable_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                GridView HeaderGrid = (GridView)sender;

                e.Row.Cells.Clear();
                GridViewRow HeaderGridRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);

                TableCell HeaderCell = new TableCell();
                HeaderCell.Text = "% Delinquency";
                HeaderCell.ColumnSpan = 11;
                HeaderGridRow.Cells.Add(HeaderCell);

                gvTable.Controls[0].Controls.AddAt(0, HeaderGridRow);
            }
        }

        protected void Search_Click(object sender, EventArgs e)
        {
            IFormatProvider format = new CultureInfo("en-GB");
            DateTime dateOpenLoan = DateTime.MinValue;


            if (ddlLoan.SelectedIndex <= 0 || ddlSubType.SelectedIndex <= 0 || ddlMarketFrm.SelectedIndex <= 0 || ddlMarketTo.SelectedIndex <= 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScript", "appPop('โปรดเลือกตัวเลือก','กรุณาเลือกเงื่อนไขการค้นหาให้ครบถ้วน ')", true);
                return;
            }

            StringBuilder sqlQuery = new StringBuilder();
            string start1 = "";
            string start2 = "";
            string start3 = "";
            string start4 = "";
            string start5 = "";
            string start6 = "";
            string start7 = "";
            string start8 = "";
            string start9 = "";
            string start0 = "";
            string end1 = "";
            string end2 = "";
            string end3 = "";
            string end4 = "";
            string end5 = "";
            string end6 = "";
            string end7 = "";
            string end8 = "";
            string end9 = "";
            string end0 = "";
            string prd1 = "";
            string prd2 = "";
            string prd3 = "";
            string prd4 = "";
            string prd5 = "";
            string prd6 = "";
            string prd7 = "";
            string prd8 = "";
            string prd9 = "";
            string prd0 = "";
            string mktqrf = "";
            string mktqrt = "";


            if (txtOpenAcDate.SelectedValue == "1")
            {
                start1 = "01/10/" + Convert.ToString(Convert.ToInt32(txtEndAcDate.SelectedValue.ToString()) - 3);
                start2 = "01/01/" + Convert.ToString(Convert.ToInt32(txtEndAcDate.SelectedValue.ToString()) - 2);
                start3 = "01/04/" + Convert.ToString(Convert.ToInt32(txtEndAcDate.SelectedValue.ToString()) - 2);
                start4 = "01/07/" + Convert.ToString(Convert.ToInt32(txtEndAcDate.SelectedValue.ToString()) - 2);
                start5 = "01/10/" + Convert.ToString(Convert.ToInt32(txtEndAcDate.SelectedValue.ToString()) - 2);
                start6 = "01/01/" + Convert.ToString(Convert.ToInt32(txtEndAcDate.SelectedValue.ToString()) - 1);
                start7 = "01/04/" + Convert.ToString(Convert.ToInt32(txtEndAcDate.SelectedValue.ToString()) - 1);
                start8 = "01/07/" + Convert.ToString(Convert.ToInt32(txtEndAcDate.SelectedValue.ToString()) - 1);
                start9 = "01/10/" + Convert.ToString(Convert.ToInt32(txtEndAcDate.SelectedValue.ToString()) - 1);
                start0 = "01/01/" + Convert.ToString(Convert.ToInt32(txtEndAcDate.SelectedValue.ToString()) - 0);
                end1 = "01/12/" + Convert.ToString(Convert.ToInt32(txtEndAcDate.SelectedValue.ToString()) - 3);
                end2 = "01/03/" + Convert.ToString(Convert.ToInt32(txtEndAcDate.SelectedValue.ToString()) - 2);
                end3 = "01/06/" + Convert.ToString(Convert.ToInt32(txtEndAcDate.SelectedValue.ToString()) - 2);
                end4 = "01/09/" + Convert.ToString(Convert.ToInt32(txtEndAcDate.SelectedValue.ToString()) - 2);
                end5 = "01/12/" + Convert.ToString(Convert.ToInt32(txtEndAcDate.SelectedValue.ToString()) - 2);
                end6 = "01/03/" + Convert.ToString(Convert.ToInt32(txtEndAcDate.SelectedValue.ToString()) - 1);
                end7 = "01/06/" + Convert.ToString(Convert.ToInt32(txtEndAcDate.SelectedValue.ToString()) - 1);
                end8 = "01/09/" + Convert.ToString(Convert.ToInt32(txtEndAcDate.SelectedValue.ToString()) - 1);
                end9 = "01/12/" + Convert.ToString(Convert.ToInt32(txtEndAcDate.SelectedValue.ToString()) - 1);
                end0 = "01/03/" + Convert.ToString(Convert.ToInt32(txtEndAcDate.SelectedValue.ToString()) - 0);
                prd1 = "12/" + Convert.ToString(Convert.ToInt32(txtEndAcDate.SelectedValue.ToString()) - 3);
                prd2 = "03/" + Convert.ToString(Convert.ToInt32(txtEndAcDate.SelectedValue.ToString()) - 2);
                prd3 = "06/" + Convert.ToString(Convert.ToInt32(txtEndAcDate.SelectedValue.ToString()) - 2);
                prd4 = "09/" + Convert.ToString(Convert.ToInt32(txtEndAcDate.SelectedValue.ToString()) - 2);
                prd5 = "12/" + Convert.ToString(Convert.ToInt32(txtEndAcDate.SelectedValue.ToString()) - 2);
                prd6 = "03/" + Convert.ToString(Convert.ToInt32(txtEndAcDate.SelectedValue.ToString()) - 1);
                prd7 = "06/" + Convert.ToString(Convert.ToInt32(txtEndAcDate.SelectedValue.ToString()) - 1);
                prd8 = "09/" + Convert.ToString(Convert.ToInt32(txtEndAcDate.SelectedValue.ToString()) - 1);
                prd9 = "12/" + Convert.ToString(Convert.ToInt32(txtEndAcDate.SelectedValue.ToString()) - 1);
                prd0 = "03/" + Convert.ToString(Convert.ToInt32(txtEndAcDate.SelectedValue.ToString()) - 0);
            }
            if (txtOpenAcDate.SelectedValue == "2")
            {
                start1 = "01/01/" + Convert.ToString(Convert.ToInt32(txtEndAcDate.SelectedValue.ToString()) - 2);
                start2 = "01/04/" + Convert.ToString(Convert.ToInt32(txtEndAcDate.SelectedValue.ToString()) - 2);
                start3 = "01/07/" + Convert.ToString(Convert.ToInt32(txtEndAcDate.SelectedValue.ToString()) - 2);
                start4 = "01/10/" + Convert.ToString(Convert.ToInt32(txtEndAcDate.SelectedValue.ToString()) - 2);
                start5 = "01/01/" + Convert.ToString(Convert.ToInt32(txtEndAcDate.SelectedValue.ToString()) - 1);
                start6 = "01/04/" + Convert.ToString(Convert.ToInt32(txtEndAcDate.SelectedValue.ToString()) - 1);
                start7 = "01/07/" + Convert.ToString(Convert.ToInt32(txtEndAcDate.SelectedValue.ToString()) - 1);
                start8 = "01/10/" + Convert.ToString(Convert.ToInt32(txtEndAcDate.SelectedValue.ToString()) - 1);
                start9 = "01/01/" + Convert.ToString(Convert.ToInt32(txtEndAcDate.SelectedValue.ToString()) - 0);
                start0 = "01/04/" + Convert.ToString(Convert.ToInt32(txtEndAcDate.SelectedValue.ToString()) - 0);
                end1 = "01/03/" + Convert.ToString(Convert.ToInt32(txtEndAcDate.SelectedValue.ToString()) - 2);
                end2 = "01/06/" + Convert.ToString(Convert.ToInt32(txtEndAcDate.SelectedValue.ToString()) - 2);
                end3 = "01/09/" + Convert.ToString(Convert.ToInt32(txtEndAcDate.SelectedValue.ToString()) - 2);
                end4 = "01/12/" + Convert.ToString(Convert.ToInt32(txtEndAcDate.SelectedValue.ToString()) - 2);
                end5 = "01/03/" + Convert.ToString(Convert.ToInt32(txtEndAcDate.SelectedValue.ToString()) - 1);
                end6 = "01/06/" + Convert.ToString(Convert.ToInt32(txtEndAcDate.SelectedValue.ToString()) - 1);
                end7 = "01/09/" + Convert.ToString(Convert.ToInt32(txtEndAcDate.SelectedValue.ToString()) - 1);
                end8 = "01/12/" + Convert.ToString(Convert.ToInt32(txtEndAcDate.SelectedValue.ToString()) - 1);
                end9 = "01/03/" + Convert.ToString(Convert.ToInt32(txtEndAcDate.SelectedValue.ToString()) - 0);
                end0 = "01/06/" + Convert.ToString(Convert.ToInt32(txtEndAcDate.SelectedValue.ToString()) - 0);
                prd1 = "03/" + Convert.ToString(Convert.ToInt32(txtEndAcDate.SelectedValue.ToString()) - 2);
                prd2 = "06/" + Convert.ToString(Convert.ToInt32(txtEndAcDate.SelectedValue.ToString()) - 2);
                prd3 = "09/" + Convert.ToString(Convert.ToInt32(txtEndAcDate.SelectedValue.ToString()) - 2);
                prd4 = "12/" + Convert.ToString(Convert.ToInt32(txtEndAcDate.SelectedValue.ToString()) - 2);
                prd5 = "03/" + Convert.ToString(Convert.ToInt32(txtEndAcDate.SelectedValue.ToString()) - 1);
                prd6 = "06/" + Convert.ToString(Convert.ToInt32(txtEndAcDate.SelectedValue.ToString()) - 1);
                prd7 = "09/" + Convert.ToString(Convert.ToInt32(txtEndAcDate.SelectedValue.ToString()) - 1);
                prd8 = "12/" + Convert.ToString(Convert.ToInt32(txtEndAcDate.SelectedValue.ToString()) - 1);
                prd9 = "03/" + Convert.ToString(Convert.ToInt32(txtEndAcDate.SelectedValue.ToString()) - 0);
                prd0 = "06/" + Convert.ToString(Convert.ToInt32(txtEndAcDate.SelectedValue.ToString()) - 0);
            }
            if (txtOpenAcDate.SelectedValue == "3")
            {
                start1 = "01/04/" + Convert.ToString(Convert.ToInt32(txtEndAcDate.SelectedValue.ToString()) - 2);
                start2 = "01/07/" + Convert.ToString(Convert.ToInt32(txtEndAcDate.SelectedValue.ToString()) - 2);
                start3 = "01/10/" + Convert.ToString(Convert.ToInt32(txtEndAcDate.SelectedValue.ToString()) - 2);
                start4 = "01/01/" + Convert.ToString(Convert.ToInt32(txtEndAcDate.SelectedValue.ToString()) - 1);
                start5 = "01/04/" + Convert.ToString(Convert.ToInt32(txtEndAcDate.SelectedValue.ToString()) - 1);
                start6 = "01/07/" + Convert.ToString(Convert.ToInt32(txtEndAcDate.SelectedValue.ToString()) - 1);
                start7 = "01/10/" + Convert.ToString(Convert.ToInt32(txtEndAcDate.SelectedValue.ToString()) - 1);
                start8 = "01/01/" + Convert.ToString(Convert.ToInt32(txtEndAcDate.SelectedValue.ToString()) - 0);
                start9 = "01/04/" + Convert.ToString(Convert.ToInt32(txtEndAcDate.SelectedValue.ToString()) - 0);
                start0 = "01/07/" + Convert.ToString(Convert.ToInt32(txtEndAcDate.SelectedValue.ToString()) - 0);
                end1 = "01/06/" + Convert.ToString(Convert.ToInt32(txtEndAcDate.SelectedValue.ToString()) - 2);
                end2 = "01/09/" + Convert.ToString(Convert.ToInt32(txtEndAcDate.SelectedValue.ToString()) - 2);
                end3 = "01/12/" + Convert.ToString(Convert.ToInt32(txtEndAcDate.SelectedValue.ToString()) - 2);
                end4 = "01/03/" + Convert.ToString(Convert.ToInt32(txtEndAcDate.SelectedValue.ToString()) - 1);
                end5 = "01/06/" + Convert.ToString(Convert.ToInt32(txtEndAcDate.SelectedValue.ToString()) - 1);
                end6 = "01/09/" + Convert.ToString(Convert.ToInt32(txtEndAcDate.SelectedValue.ToString()) - 1);
                end7 = "01/12/" + Convert.ToString(Convert.ToInt32(txtEndAcDate.SelectedValue.ToString()) - 1);
                end8 = "01/03/" + Convert.ToString(Convert.ToInt32(txtEndAcDate.SelectedValue.ToString()) - 0);
                end9 = "01/06/" + Convert.ToString(Convert.ToInt32(txtEndAcDate.SelectedValue.ToString()) - 0);
                end0 = "01/09/" + Convert.ToString(Convert.ToInt32(txtEndAcDate.SelectedValue.ToString()) - 0);
                prd1 = "06/" + Convert.ToString(Convert.ToInt32(txtEndAcDate.SelectedValue.ToString()) - 2);
                prd2 = "09/" + Convert.ToString(Convert.ToInt32(txtEndAcDate.SelectedValue.ToString()) - 2);
                prd3 = "12/" + Convert.ToString(Convert.ToInt32(txtEndAcDate.SelectedValue.ToString()) - 2);
                prd4 = "03/" + Convert.ToString(Convert.ToInt32(txtEndAcDate.SelectedValue.ToString()) - 1);
                prd5 = "06/" + Convert.ToString(Convert.ToInt32(txtEndAcDate.SelectedValue.ToString()) - 1);
                prd6 = "09/" + Convert.ToString(Convert.ToInt32(txtEndAcDate.SelectedValue.ToString()) - 1);
                prd7 = "12/" + Convert.ToString(Convert.ToInt32(txtEndAcDate.SelectedValue.ToString()) - 1);
                prd8 = "03/" + Convert.ToString(Convert.ToInt32(txtEndAcDate.SelectedValue.ToString()) - 0);
                prd9 = "06/" + Convert.ToString(Convert.ToInt32(txtEndAcDate.SelectedValue.ToString()) - 0);
                prd0 = "09/" + Convert.ToString(Convert.ToInt32(txtEndAcDate.SelectedValue.ToString()) - 0);
            }
            if (txtOpenAcDate.SelectedValue == "4")
            {
                start1 = "01/07/" + Convert.ToString(Convert.ToInt32(txtEndAcDate.SelectedValue.ToString()) - 2);
                start2 = "01/10/" + Convert.ToString(Convert.ToInt32(txtEndAcDate.SelectedValue.ToString()) - 2);
                start3 = "01/01/" + Convert.ToString(Convert.ToInt32(txtEndAcDate.SelectedValue.ToString()) - 1);
                start4 = "01/04/" + Convert.ToString(Convert.ToInt32(txtEndAcDate.SelectedValue.ToString()) - 1);
                start5 = "01/07/" + Convert.ToString(Convert.ToInt32(txtEndAcDate.SelectedValue.ToString()) - 1);
                start6 = "01/10/" + Convert.ToString(Convert.ToInt32(txtEndAcDate.SelectedValue.ToString()) - 1);
                start7 = "01/01/" + Convert.ToString(Convert.ToInt32(txtEndAcDate.SelectedValue.ToString()) - 0);
                start8 = "01/04/" + Convert.ToString(Convert.ToInt32(txtEndAcDate.SelectedValue.ToString()) - 0);
                start9 = "01/07/" + Convert.ToString(Convert.ToInt32(txtEndAcDate.SelectedValue.ToString()) - 0);
                start0 = "01/10/" + Convert.ToString(Convert.ToInt32(txtEndAcDate.SelectedValue.ToString()) - 0);
                end1 = "01/09/" + Convert.ToString(Convert.ToInt32(txtEndAcDate.SelectedValue.ToString()) - 2);
                end2 = "01/12/" + Convert.ToString(Convert.ToInt32(txtEndAcDate.SelectedValue.ToString()) - 2);
                end3 = "01/03/" + Convert.ToString(Convert.ToInt32(txtEndAcDate.SelectedValue.ToString()) - 1);
                end4 = "01/06/" + Convert.ToString(Convert.ToInt32(txtEndAcDate.SelectedValue.ToString()) - 1);
                end5 = "01/09/" + Convert.ToString(Convert.ToInt32(txtEndAcDate.SelectedValue.ToString()) - 1);
                end6 = "01/12/" + Convert.ToString(Convert.ToInt32(txtEndAcDate.SelectedValue.ToString()) - 1);
                end7 = "01/03/" + Convert.ToString(Convert.ToInt32(txtEndAcDate.SelectedValue.ToString()) - 0);
                end8 = "01/06/" + Convert.ToString(Convert.ToInt32(txtEndAcDate.SelectedValue.ToString()) - 0);
                end9 = "01/09/" + Convert.ToString(Convert.ToInt32(txtEndAcDate.SelectedValue.ToString()) - 0);
                end0 = "01/12/" + Convert.ToString(Convert.ToInt32(txtEndAcDate.SelectedValue.ToString()) - 0);
                prd1 = "09/" + Convert.ToString(Convert.ToInt32(txtEndAcDate.SelectedValue.ToString()) - 2);
                prd2 = "12/" + Convert.ToString(Convert.ToInt32(txtEndAcDate.SelectedValue.ToString()) - 2);
                prd3 = "03/" + Convert.ToString(Convert.ToInt32(txtEndAcDate.SelectedValue.ToString()) - 1);
                prd4 = "06/" + Convert.ToString(Convert.ToInt32(txtEndAcDate.SelectedValue.ToString()) - 1);
                prd5 = "09/" + Convert.ToString(Convert.ToInt32(txtEndAcDate.SelectedValue.ToString()) - 1);
                prd6 = "12/" + Convert.ToString(Convert.ToInt32(txtEndAcDate.SelectedValue.ToString()) - 1);
                prd7 = "03/" + Convert.ToString(Convert.ToInt32(txtEndAcDate.SelectedValue.ToString()) - 0);
                prd8 = "06/" + Convert.ToString(Convert.ToInt32(txtEndAcDate.SelectedValue.ToString()) - 0);
                prd9 = "09/" + Convert.ToString(Convert.ToInt32(txtEndAcDate.SelectedValue.ToString()) - 0);
                prd0 = "12/" + Convert.ToString(Convert.ToInt32(txtEndAcDate.SelectedValue.ToString()) - 0);
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
                                appQuery1 = string.Format("	select 1 as seq,'{14}' as endmt 	" +
        "	 into #temp	" +
        "	 union select 2 as seq,'{15}'as endmt	" +
        "	 union select 3 as seq,'{16}'as endmt	" +
        "	 union select 4 as seq,'{17}'as endmt	" +
        "	 union select 5 as seq,'{18}'as endmt	" +
        "	 union select 6 as seq,'{19}'as endmt	" +
        "	 union select 7 as seq,'{20}'as endmt	" +
        "	 union select 8 as seq,'{21}'as endmt	" +
        "	 union select 9 as seq,'{22}'as endmt	" +
        "	 union select 10 as seq,'{23}'  as endmt	" +
        "	select endmt 	" +
        "	into #Heading	" +
        "	from #temp	" +
        "	order by seq 	" +
        "	select a.endmt	" +
        "	,case when b.bm1 is null then 0 else b.bm1 end as bm1	" +
        "	 ,case when b.bm2 is null then 0 else b.bm2 end as bm2	" +
        "	 ,case when b.bm3 is null then 0 else b.bm3 end as bm3	" +
        "	 ,case when b.bm4 is null then 0 else b.bm4 end as bm4	" +
        "	 ,case when b.bm5 is null then 0 else b.bm5 end as bm5	" +
        "	 ,case when b.bm6 is null then 0 else b.bm6 end as bm6	" +
        "	 ,case when b.bm7 is null then 0 else b.bm7 end as bm7	" +
        "	 ,case when b.bm8 is null then 0 else b.bm8 end as bm8	" +
        "	 ,case when b.bm9 is null then 0 else b.bm9 end as bm9	" +
        "	 ,case when b.bm0 is null then 0 else b.bm0 end as bm0	" +
        "	into #main_program	" +
        "	from	" +
        "	(	" +
        "	select * from #Heading	" +
        "	 ) a	" +
        "	 left join 	" +
        "	 (	" +
        "	 SELECT SDATE as endmt 	" +
        "	 ,case when SUM(cntd1) = 0 then 0 else convert(decimal(15,2),(SUM(cntdbm1) * 100.00)/(SUM(cntd1))) end as bm1	" +
        "	 ,case when SUM(cntd2) = 0 then 0 else convert(decimal(15,2),(SUM(cntdbm2) * 100.00)/(SUM(cntd2))) end as bm2	" +
        "	 ,case when SUM(cntd3) = 0 then 0 else convert(decimal(15,2),(SUM(cntdbm3) * 100.00)/(SUM(cntd3))) end as bm3  	" +
        "	 ,case when SUM(cntd4) = 0 then 0 else convert(decimal(15,2),(SUM(cntdbm4) * 100.00)/(SUM(cntd4))) end as bm4	" +
        "	 ,case when SUM(cntd5) = 0 then 0 else convert(decimal(15,2),(SUM(cntdbm5) * 100.00)/(SUM(cntd5))) end as bm5	" +
        "	 ,case when SUM(cntd6) = 0 then 0 else convert(decimal(15,2),(SUM(cntdbm6) * 100.00)/(SUM(cntd6))) end as bm6	" +
        "	 ,case when SUM(cntd7) = 0 then 0 else convert(decimal(15,2),(SUM(cntdbm7) * 100.00)/(SUM(cntd7))) end as bm7	" +
        "	 ,case when SUM(cntd8) = 0 then 0 else convert(decimal(15,2),(SUM(cntdbm8) * 100.00)/(SUM(cntd8))) end as bm8 	" +
        "	 ,case when SUM(cntd9) = 0 then 0 else convert(decimal(15,2),(SUM(cntdbm9) * 100.00)/(SUM(cntd9))) end as bm9 	" +
        "	 ,case when SUM(cntd10) = 0 then 0 else convert(decimal(15,2),(SUM(cntdbm10) * 100.00)/(SUM(cntd10))) end as bm0	" +
        "	 FROM 	" +
        "	 (	" +
        "	select [CBS_APP_NO],SDATE  	" +
        "	,case when delig = '{33}' then 1 else 0 end as cntd10	" +
        "	,case when delig = '{33}' then DWBM else 0 end as cntdbm10  	" +
        "	,case when delig = '{32}' then 1 else 0 end as cntd9	" +
        "	,case when delig = '{32}' then DWBM else 0 end as cntdbm9  	" +
        "	,case when delig = '{31}' then 1 else 0 end as cntd8	" +
        "	,case when delig = '{31}' then DWBM else 0 end as cntdbm8 	" +
        "	 ,case when delig = '{30}' then 1 else 0 end as cntd7	" +
        "	 ,case when delig = '{30}' then DWBM else 0 end as cntdbm7  	" +
        "	 ,case when delig = '{29}' then 1 else 0 end as cntd6	" +
        "	 ,case when delig = '{29}' then DWBM else 0 end as cntdbm6  	" +
        "	 ,case when delig = '{28}' then 1 else 0 end as cntd5	" +
        "	 ,case when delig = '{28}' then DWBM else 0 end as cntdbm5 	" +
        "	 ,case when delig = '{27}' then 1 else 0 end as cntd4	" +
        "	 ,case when delig = '{27}' then DWBM else 0 end as cntdbm4 	" +
        "	 ,case when delig = '{26}' then 1 else 0 end as cntd3	" +
        "	 ,case when delig = '{26}' then DWBM else 0 end as cntdbm3	" +
        "	 ,case when delig = '{25}' then 1 else 0 end as cntd2	" +
        "	 ,case when delig = '{25}' then DWBM else 0 end as cntdbm2	" +
        "	 ,case when delig = '{24}' then 1 else 0 end as cntd1  	" +
        "	 ,case when delig = '{24}' then DWBM else 0 end as cntdbm1  	" +
        "	from 	" +
        "	(	" +
        "	select [CBS_APP_NO]	" +
        "	,case 	" +
        "	when convert(int,substring([DW_SIGN_DATE],7,4) + substring([DW_SIGN_DATE],4,2))	" +
        "	between convert(int,substring('{4}',7,4) + substring('{4}',4,2))	" +
        "	and convert(int,substring('{14}',7,4) + substring('{14}',4,2)) 	" +
        "	then '{14}'  	" +
        "	when convert(int,substring([DW_SIGN_DATE],7,4) + substring([DW_SIGN_DATE],4,2))	" +
        "	between convert(int,substring('{5}',7,4) + substring('{5}',4,2))	" +
        "	and convert(int,substring('{15}',7,4) + substring('{15}',4,2)) 	" +
        "	then '{15}'  	" +
        "	when convert(int,substring([DW_SIGN_DATE],7,4) + substring([DW_SIGN_DATE],4,2))	" +
        "	between convert(int,substring('{6}',7,4) + substring('{6}',4,2))	" +
        "	and convert(int,substring('{16}',7,4) + substring('{16}',4,2)) 	" +
        "	then '{16}'  	" +
        "	when convert(int,substring([DW_SIGN_DATE],7,4) + substring([DW_SIGN_DATE],4,2))  	" +
        "	between convert(int,substring('{7}',7,4) + substring('{7}',4,2))	" +
        "	and convert(int,substring('{17}',7,4) + substring('{17}',4,2))	" +
        "	then '{17}' 	" +
        "	when convert(int,substring([DW_SIGN_DATE],7,4) + substring([DW_SIGN_DATE],4,2)) 	" +
        "	between convert(int,substring('{8}',7,4) + substring('{8}',4,2))  	" +
        "	and convert(int,substring('{18}',7,4) + substring('{18}',4,2)) then '{18}' 	" +
        "	when convert(int,substring([DW_SIGN_DATE],7,4) + substring([DW_SIGN_DATE],4,2))  	" +
        "	between convert(int,substring('{9}',7,4) + substring('{9}',4,2))	" +
        "	and convert(int,substring('{19}',7,4) + substring('{19}',4,2))	" +
        "	then '{19}'	" +
        "	when convert(int,substring([DW_SIGN_DATE],7,4) + substring([DW_SIGN_DATE],4,2))  	" +
        "	between convert(int,substring('{10}',7,4) + substring('{10}',4,2))	" +
        "	and convert(int,substring('{20}',7,4) + substring('{20}',4,2))	" +
        "	then '{20}'	" +
        "	when convert(int,substring([DW_SIGN_DATE],7,4) + substring([DW_SIGN_DATE],4,2))	" +
        "	between convert(int,substring('{11}',7,4) + substring('{11}',4,2))	" +
        "	and convert(int,substring('{21}',7,4) + substring('{21}',4,2)) 	" +
        "	then '{21}'  	" +
        "	when convert(int,substring([DW_SIGN_DATE],7,4) + substring([DW_SIGN_DATE],4,2))	" +
        "	between convert(int,substring('{12}',7,4) + substring('{12}',4,2))	" +
        "	and convert(int,substring('{22}',7,4) + substring('{22}',4,2)) 	" +
        "	then '{22}'  	" +
        "	when convert(int,substring([DW_SIGN_DATE],7,4) + substring([DW_SIGN_DATE],4,2))	" +
        "	between convert(int,substring('{13}',7,4) + substring('{13}',4,2))	" +
        "	and convert(int,substring('{23}',7,4) + substring('{23}',4,2)) 	" +
        "	then '{23}'	" +
        "	end as SDATE  	" +
        "	,case when C.DW_BADMONTH >= 3 then 1 else 0 end as DWBM,right([LAST_UPDATE_DTM],7) as delig  	" +
        "	from [dbo].[DWH_BTFILE] c, [dbo].[LN_APP] a	" +
        "	where a.APP_SEQ = (select MAX(app_seq) from [dbo].[LN_APP] b where a.APP_NO = b.APP_NO )	" +
        "	and LOAN_CD = '{0}' and STYPE_CD = '{1}' {2} {3}	" +
        //"	and right('00000000000' + CBS_APP_NO,12) = a.APP_NO and 	" +
        "	and CBS_APP_NO = a.APP_NO and 	" +
        "	convert(int,substring([DW_SIGN_DATE],7,4) + substring([DW_SIGN_DATE],4,2))  	" +
        "	between convert(int,substring('{4}',7,4) + substring('{4}',4,2))	" +
        "	and convert(int,substring('{23}',7,4) + substring('{23}',4,2))	" +
        "	and right([LAST_UPDATE_DTM],7) in ('{32}','{33}','{31}','{30}','{29}','{28}','{27}','{26}','{25}','{24}')	" +
        "	and a.CBS_STATUS IN ('3','6','7')	" +
        //"	and c.ISACTIVE=0  	" +
        //Tai 2013-12-26
        "	and c.ISACTIVE=0 and convert(decimal(18,2),c.DW_OUTSTAND)>0 " +
        //
        "	) deliga 	" +
        "	) sumdel  group by SDATE 	" +
        "	) b on a.endmt = b.endmt 	" +
        " select convert(varchar(20),'1') as seq, convert(varchar(20),' Open Date') as prdmnt,[dbo].[lookupMonth](substring('{24}',1,2))+' '+convert(varchar,convert(integer,substring('{24}',4,4))+543) as bm1 " +
        " ,[dbo].[lookupMonth](substring('{25}',1,2))+' '+convert(varchar,convert(integer,substring('{25}',4,4))+543) as bm2 " +
        " ,[dbo].[lookupMonth](substring('{26}',1,2))+' '+convert(varchar,convert(integer,substring('{26}',4,4))+543) as bm3 " +
        " ,[dbo].[lookupMonth](substring('{27}',1,2))+' '+convert(varchar,convert(integer,substring('{27}',4,4))+543) as bm4 " +
        " ,[dbo].[lookupMonth](substring('{28}',1,2))+' '+convert(varchar,convert(integer,substring('{28}',4,4))+543) as bm5 " +
        " ,[dbo].[lookupMonth](substring('{29}',1,2))+' '+convert(varchar,convert(integer,substring('{29}',4,4))+543) as bm6 " +
        " ,[dbo].[lookupMonth](substring('{30}',1,2))+' '+convert(varchar,convert(integer,substring('{30}',4,4))+543) as bm7 " +
        " ,[dbo].[lookupMonth](substring('{31}',1,2))+' '+convert(varchar,convert(integer,substring('{31}',4,4))+543) as bm8 " +
        " ,[dbo].[lookupMonth](substring('{32}',1,2))+' '+convert(varchar,convert(integer,substring('{32}',4,4))+543) as bm9,[dbo].[lookupMonth](substring('{33}',1,2))+' '+convert(varchar,convert(integer,substring('{33}',4,4))+543) as bm0 	into #out  " +
        "	select seq,prdmnt,bm1,bm2,bm3,bm4,bm5,bm6,bm7,bm8,bm9,bm0  into #Main_Table " +
        "	from	(	select case when substring(a.endmt,4,2) = '03' then ' ม.ค.-มี.ค. '+convert(varchar,(convert(integer,substring(a.endmt,7,4))+543)) 	 " +
        "	when substring(a.endmt,4,2) = '06' then ' เม.ย.-มิ.ย. '+convert(varchar,(convert(integer,substring(a.endmt,7,4))+543))   " +
        "	 when substring(a.endmt,4,2) = '09' then ' ก.ค.-ก.ย. '+convert(varchar,(convert(integer,substring(a.endmt,7,4))+543))  " +
        "	 when substring(a.endmt,4,2) = '12' then ' ต.ค.-ธ.ค. '+convert(varchar,(convert(integer,substring(a.endmt,7,4))+543))  " +
        "	 else a.endmt end as prdmnt,bm1,bm2,bm3,bm4,bm5,bm6,bm7,bm8,bm9,bm0  " +
        "	 ,substring(a.endmt,7,4)+substring(a.endmt,4,2) as seq  " +
        "	 from 		 (		select endmt		,convert(varchar,bm1) as bm1		 " +
        "	 ,convert(varchar,bm2) as bm2		,convert(varchar,bm3) as bm3		 " +
        "	 ,convert(varchar,bm4) as bm4		,convert(varchar,bm5) as bm5		 " +
        "	 ,convert(varchar,bm6) as bm6		,convert(varchar,bm7) as bm7		 " +
        "	 ,convert(varchar,bm8) as bm8		,convert(varchar,bm9) as bm9		 " +
        "	 ,convert(varchar,bm0) as bm0		 " +
        "	 from #main_program		 ) a			 ) b	 order by seq  " +
        " select  prdmnt,bm1,bm2,bm3,bm4,bm5,bm6,bm7,bm8,bm9,bm0 from ( select * from #out union select * from  #Main_Table ) A order by seq " +
        " drop table #temp	drop table #Heading		drop table #main_program	drop table #out	drop table #Main_Table "
        , ddlLoan.SelectedItem.Value, ddlSubType.SelectedItem.Value, mktqrf, mktqrt, start1.ToString(), start2.ToString()
                           , start3.ToString(), start4.ToString(), start5.ToString(), start6.ToString(), start7.ToString(), start8.ToString()
                           , start9.ToString(), start0.ToString(), end1.ToString(), end2.ToString(), end3.ToString(), end4.ToString(), end5.ToString()
                           , end6.ToString(), end7.ToString(), end8.ToString(), end9.ToString(), end0.ToString(), prd1.ToString(), prd2.ToString()
                           , prd3.ToString(), prd4.ToString(), prd5.ToString(), prd6.ToString(), prd7.ToString(), prd8.ToString(), prd9.ToString(), prd0.ToString());
                                    //gvTable.DataSource = conn.ExcuteSQL(appQuery1);
                                    //gvTable.DataBind();


                                    DataTable dt = new DataTable();

                                    if (ddlModel.SelectedItem.Value == "7")
                                    {
                                        String strConnString = ConfigurationManager.ConnectionStrings["GSBSQLServer"].ConnectionString;
                                        SqlConnection con = new SqlConnection(strConnString);
                                        con.Open();
                                        SqlCommand command;
                                        command = new SqlCommand("sp_BackEnd_VintageAnalysisReport", con);

                                        command.Parameters.Add(new SqlParameter("@start1", start1.ToString()));
                                        command.Parameters.Add(new SqlParameter("@start2", start2.ToString()));
                                        command.Parameters.Add(new SqlParameter("@start3", start3.ToString()));
                                        command.Parameters.Add(new SqlParameter("@start4", start4.ToString()));
                                        command.Parameters.Add(new SqlParameter("@start5", start5.ToString()));
                                        command.Parameters.Add(new SqlParameter("@start6", start6.ToString()));
                                        command.Parameters.Add(new SqlParameter("@start7", start7.ToString()));
                                        command.Parameters.Add(new SqlParameter("@start8", start8.ToString()));
                                        command.Parameters.Add(new SqlParameter("@start9", start9.ToString()));
                                        command.Parameters.Add(new SqlParameter("@start0", start0.ToString()));
                                        command.Parameters.Add(new SqlParameter("@end1", end1.ToString()));
                                        command.Parameters.Add(new SqlParameter("@end2", end2.ToString()));
                                        command.Parameters.Add(new SqlParameter("@end3", end3.ToString()));
                                        command.Parameters.Add(new SqlParameter("@end4", end4.ToString()));
                                        command.Parameters.Add(new SqlParameter("@end5", end5.ToString()));
                                        command.Parameters.Add(new SqlParameter("@end6", end6.ToString()));
                                        command.Parameters.Add(new SqlParameter("@end7", end7.ToString()));
                                        command.Parameters.Add(new SqlParameter("@end8", end8.ToString()));
                                        command.Parameters.Add(new SqlParameter("@end9", end9.ToString()));
                                        command.Parameters.Add(new SqlParameter("@end0", end0.ToString()));
                                        command.Parameters.Add(new SqlParameter("@prd1", prd1.ToString()));
                                        command.Parameters.Add(new SqlParameter("@prd2", prd2.ToString()));
                                        command.Parameters.Add(new SqlParameter("@prd3", prd3.ToString()));
                                        command.Parameters.Add(new SqlParameter("@prd4", prd4.ToString()));
                                        command.Parameters.Add(new SqlParameter("@prd5", prd5.ToString()));
                                        command.Parameters.Add(new SqlParameter("@prd6", prd6.ToString()));
                                        command.Parameters.Add(new SqlParameter("@prd7", prd7.ToString()));
                                        command.Parameters.Add(new SqlParameter("@prd8", prd8.ToString()));
                                        command.Parameters.Add(new SqlParameter("@prd9", prd9.ToString()));
                                        command.Parameters.Add(new SqlParameter("@prd0", prd0.ToString()));
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
                int endyear;
                //Generate report header
                Session["sHeaderRpt1"] = "ประเภทโมเดล : " + ddlModel.SelectedItem.Text;
                Session["sHeaderRpt2"] = "ประเภทสินเชื่อ / ประเภทสินเชื่อย่อย : " + ddlLoan.SelectedItem.Text + " / " + ddlSubType.SelectedItem.Text;

                if (ddlMarketFrm.SelectedIndex == 0)
                    Session["sHeaderRpt3"] = "Market Code : " + ddlMarketFrm.SelectedItem.Text;
                else
                    Session["sHeaderRpt3"] = "Market Code : " + ddlMarketFrm.SelectedItem.Text + " ถึง " + ddlMarketTo.SelectedItem.Text;
                endyear = Convert.ToInt32(txtEndAcDate.Text) + 543;
                Session["sHeaderRpt4"] = "ช่วงเดือนที่เซ็นสัญญา : " + txtOpenAcDate.SelectedItem + " / ปี : " + endyear;

                //Regenerate data for report
                DataTable dt = (DataTable)Session["dtb"];
                Session["rptData"] = dt;
                ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "aa", "window.open('ReportLayOut/frmPreviewBackRpt.aspx?ReportId=93');", true);
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
            int endyear;
            //Generate report header
            Session["sHeaderRpt1"] = "ประเภทโมเดล : " + ddlModel.SelectedItem.Text ;
            Session["sHeaderRpt2"] = "ประเภทสินเชื่อ / ประเภทสินเชื่อย่อย : " + ddlLoan.SelectedItem.Text + " / " + ddlSubType.SelectedItem.Text;

            if (ddlMarketFrm.SelectedIndex == 0)
                Session["sHeaderRpt3"] = "Market Code : " + ddlMarketFrm.SelectedItem.Text;
            else
                Session["sHeaderRpt3"] = "Market Code : " + ddlMarketFrm.SelectedItem.Text + " ถึง  " + ddlMarketTo.SelectedItem.Text;
            endyear = Convert.ToInt32(txtEndAcDate.Text) + 543;
            Session["sHeaderRpt4"] = "ช่วงเดือนที่เซ็นสัญญา : " + txtOpenAcDate.SelectedItem + " / ปี : " + endyear;

            //Regenerate data for report
            DataTable dt = (DataTable)Session["dtb"];
            Session["rptData"] = dt;
            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "aa", "window.open('ReportLayOut/frmPreviewBackRpt.aspx?ReportId=3');", true);
        }

        protected void gvTable_PageChanging(object sender, GridViewPageEventArgs e)
        {
            gvTable.PageIndex = e.NewPageIndex;
            Search_Click(null, null);
        }
        #endregion Protected Method
    }
}