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

    public partial class CreditScoringModelGradeStatus : System.Web.UI.Page
    {
        //int TotalDev = 0;
        //decimal TotalDevPercent = 0;
        //int TotalActual = 0;
        //decimal TotalActualPercent = 0;
        //decimal TotalPopulationStabilityIndex = 0;

        SQLToDataTable conn = new SQLToDataTable();
        private static bool flg;

        string appQuery1;
        #region Page_Load

        protected void Page_Load(object sender, EventArgs e)
        {
            //ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
            //scriptManager.RegisterPostBackControl(this.btnExcel);

            if (!Page.IsPostBack)
            {
                LoadYearMonth();
                //ekk comment 09/04/2562
                LoadModel();
                //LoadLoan();
                //LoadModelVersion();

                //LoadLoan();
                flg = false;
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
            ddlSubType.Items.Insert(0, new ListItem("โปรดเลือก...", string.Empty));
        }


        private void DataTableToExcel(DataTable strSQL)
        {
            //string Query = string.Format("{0}", strSQL);

            string filename = string.Format("GSB-CS_CreditScoringModelGradeStatusReport_(Date_{0}-Time_{1}).xls", DateTime.Now.ToString("dd-MM-yyyy"), DateTime.Now.ToString("HH-mm-ss"));
            StringWriter tw = new StringWriter();

          

            DataGrid dgGrid = new DataGrid();
            DataTable dttb = new DataTable();

            dttb = strSQL;

            tw.Write(string.Format("Credit Scoring Model Grade Status Report"));
            tw.Write(tw.NewLine);
            //tw.Write(string.Format("ประเภทโมเดล :| {0} ", ddlModel.SelectedItem.Text.ToString()));
            //tw.Write(tw.NewLine);
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
            iColCount = dttb.Columns.Count;
            
            tw.Write("ระดับคะแนนเครดิต (เกรด)|Approve (จำนวน)|Approve (ร้อยละ)|Reject (จำนวน)|Reject (ร้อยละ)|อื่นๆ (จำนวน)|อื่นๆ (ร้อยละ)|รวมทั้งหมด (จำนวน)|รวมทั้งหมด (ร้อยละ)");
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

            tw.WriteLine("");
            tw.WriteLine("");
            tw.WriteLine("");

            //tw.Write("Credit Scoring Model Grade Status Index");
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
            flg = true;
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
        //    flg = true;
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
            flg = true;
        }
        protected void Search_Click(object sender, EventArgs e)
        {
            /*
            DataTable dt = new DataTable();
            dt.Columns.Add("SUBJECT");
            dt.Columns.Add("APPROVENUMBER");
            dt.Columns.Add("APPROVEPERCENT");
            dt.Columns.Add("REJECTNUMBER");
            dt.Columns.Add("REJECTPERCENT");
            dt.Columns.Add("OTHERNUMBER");
            dt.Columns.Add("OTHERPERCENT");
            dt.Columns.Add("TOTALNUMBER");
            dt.Columns.Add("TOTALPERCENT");
            

            DataRow dr = dt.NewRow();
            dr["SUBJECT"] = "ความเสี่ยงต่ำ";
            dr["APPROVENUMBER"] = "40000";
            dr["APPROVEPERCENT"] = "40.00%";
            dr["REJECTNUMBER"] = "30000";
            dr["REJECTPERCENT"] = "30.00%";
            dr["OTHERNUMBER"] = "20000";
            dr["OTHERPERCENT"] = "20.00%";
            dr["TOTALNUMBER"] = "90000";
            dr["TOTALPERCENT"] = "30.00%";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["SUBJECT"] = "ความเสี่ยงปานกลาง";
            dr["APPROVENUMBER"] = "30000";
            dr["APPROVEPERCENT"] = "30.00%";
            dr["REJECTNUMBER"] = "30000";
            dr["REJECTPERCENT"] = "30.00%";
            dr["OTHERNUMBER"] = "20000";
            dr["OTHERPERCENT"] = "20.00%";
            dr["TOTALNUMBER"] = "80000";
            dr["TOTALPERCENT"] = "25.00%";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["SUBJECT"] = "ความเสี่ยงค่อนข้างสูง";
            dr["APPROVENUMBER"] = "20000";
            dr["APPROVEPERCENT"] = "20.00%";
            dr["REJECTNUMBER"] = "20000";
            dr["REJECTPERCENT"] = "20.00%";
            dr["OTHERNUMBER"] = "20000";
            dr["OTHERPERCENT"] = "20.00%";
            dr["TOTALNUMBER"] = "60000";
            dr["TOTALPERCENT"] = "20.00%";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["SUBJECT"] = "ความเสี่ยงสูง";
            dr["APPROVENUMBER"] = "5000";
            dr["APPROVEPERCENT"] = "5.00%";
            dr["REJECTNUMBER"] = "10000";
            dr["REJECTPERCENT"] = "10.00%";
            dr["OTHERNUMBER"] = "20000";
            dr["OTHERPERCENT"] = "20.00%";
            dr["TOTALNUMBER"] = "35000";
            dr["TOTALPERCENT"] = "10.00%";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["SUBJECT"] = "เกรดอื่นๆ";
            dr["APPROVENUMBER"] = "5000";
            dr["APPROVEPERCENT"] = "5.00%";
            dr["REJECTNUMBER"] = "10000";
            dr["REJECTPERCENT"] = "10.00%";
            dr["OTHERNUMBER"] = "20000";
            dr["OTHERPERCENT"] = "20.00%";
            dr["TOTALNUMBER"] = "35000";
            dr["TOTALPERCENT"] = "10.00%";
            dt.Rows.Add(dr);


            dr = dt.NewRow();
            dr["SUBJECT"] = "จำนวนทั้งหมด";
            dr["APPROVENUMBER"] = "1000000";
            dr["APPROVEPERCENT"] = "100.00%";
            dr["REJECTNUMBER"] = "100000";
            dr["REJECTPERCENT"] = "100.00%";
            dr["OTHERNUMBER"] = "100000";
            dr["OTHERPERCENT"] = "100.00%";
            dr["TOTALNUMBER"] = "35000";
            dr["TOTALPERCENT"] = "10.00%";
            dt.Rows.Add(dr);
            
            gvTable.DataSource = dt;
            gvTable.DataBind();
            */

            if (ddlLoan.SelectedIndex <= 0 || ddlSubType.SelectedIndex <= 0 || ddlMarketFrm.SelectedIndex <= 0 || ddlMarketTo.SelectedIndex <= 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScript", "appPop('โปรดเลือกตัวเลือก','กรุณาเลือกเงื่อนไขการค้นหาให้ครบถ้วน ')", true);
                return;
            }

            if (ddlModel.SelectedIndex <= 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScript", "appPop('โปรดเลือกตัวเลือก','โปรดเลือก Model ก่อน')", true);
                return;
            }

            string modelname = "00" + ddlModel.SelectedValue.ToString();


            int monthstart = Convert.ToInt16(ddlOpenDateYear.SelectedValue) * 100 + ddlOpenDateMonth.SelectedIndex;
            int monthend = Convert.ToInt16(ddlOpenDateYearend.SelectedValue) * 100 + ddlOpenDateMonthend.SelectedIndex;

            if (monthstart > monthend)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScript", "appPop('วันที่เปิดใบคำขอสินเชื่อไม่ถูกต้อง?','เลือกวันที่เปิดใบคำขอไม่ถูกต้อง')", true);
                return;
            }


            String strConnString = ConfigurationManager.ConnectionStrings["GSBSQLServer"].ConnectionString;
            SqlConnection con = new SqlConnection(strConnString);
            con.Open();
            SqlCommand command = new SqlCommand("sp_Report_14", con);

            

            //DateTime date_open = DateTime.Parse((txtOpenDate.Text.ToString().Substring(0, 6) + (Convert.ToInt32(txtOpenDate.Text.Substring(6, 4).ToString()) - 543).ToString()));  //DateTime.ParseExact(txtOpenDate.Text, "dd/MM/yyyy", null);
            //DateTime date_close = DateTime.Parse((txtCloseDate.Text.ToString().Substring(0, 6) + (Convert.ToInt32(txtCloseDate.Text.Substring(6, 4).ToString()) - 543).ToString()));  //DateTime.ParseExact(txtCloseDate.Text, "dd/MM/yyyy", null);

            //int y_start;
            //int y_end;

            //y_start = date_open.Year;
            //y_end = date_close.Year;

            //if (y_start > 2500) y_start = y_start - 543;
            //if (y_end > 2500) y_end = y_end - 543;

            //date_open = new DateTime(y_start, date_open.Month, date_open.Day);
            //date_close = new DateTime(y_end, date_close.Month, date_close.Day);

            command.Parameters.Add(new SqlParameter("@APP_DATE_open", ddlOpenDateYear.SelectedValue.ToString() + (ddlOpenDateMonth.SelectedIndex + 1).ToString("00")));
            command.Parameters.Add(new SqlParameter("@APP_DATE_end", ddlOpenDateYearend.SelectedValue.ToString() + (ddlOpenDateMonthend.SelectedIndex + 1).ToString("00")));
            command.Parameters.Add(new SqlParameter("@LOAN_CD", ddlLoan.SelectedItem.Value));
            command.Parameters.Add(new SqlParameter("@STYPE_CD", ddlSubType.SelectedItem.Value));
            command.Parameters.Add(new SqlParameter("@MarketCodeStart", ddlMarketFrm.Text));
            command.Parameters.Add(new SqlParameter("@MarketCodeEnd", ddlMarketTo.Text));
            if (modelname == "007")
            {
                command.Parameters.Add(new SqlParameter("@Modelname", "Mymo"));
            }
            else
            {
                command.Parameters.Add(new SqlParameter("@Modelname", modelname));

            }

            command.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(command);
            DataTable dt = new DataTable();
            da.Fill(dt);
            gvTable.DataSource = dt;
            Session["dtb"] = dt;
            gvTable.DataBind();
        }

        protected void btnExcel_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)Session["dtb"];

            if (gvTable.Rows.Count > 0)
            {
                Session["rptData"] = dt; //Report Data
                                         //Report header
                Session["sHeaderRpt1"] = "ประเภทโมเดล : " + ddlModel.SelectedItem.Text;
                Session["sHeaderRpt2"] = "ประเภทสินเชื่อ / ประเภทสินเชื่อย่อย : " + ddlLoan.SelectedItem.Text + " / " + ddlSubType.SelectedItem.Text;

                if (ddlMarketFrm.SelectedIndex == 0)
                    Session["sHeaderRpt3"] = "Market Code : " + ddlMarketFrm.SelectedItem.Text;
                else
                    Session["sHeaderRpt3"] = "Market Code : " + ddlMarketFrm.SelectedItem.Text + " ถึง " + ddlMarketTo.SelectedItem.Text;

                Session["sHeaderRpt4"] = "วันที่เปิดใบคำขอสินเชื่อ : " + ddlOpenDateMonth.SelectedValue + " " + ddlOpenDateYear.SelectedItem + " ถึง " + ddlOpenDateMonthend.SelectedValue + " " + ddlOpenDateYearend.SelectedItem;



                ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "aa", "window.open('ReportLayOut/frmPreviewAMRrpt.aspx?ReportId=AMR91');", true);

            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", "appPop('','โปรดเลือกกดแสดงข้อมูลก่อน');", true);
                return;
            }

        }

        protected void Cancel_Click(object sender, EventArgs e)
        {
            //ddlModel.ClearSelection();
            ddlLoan.ClearSelection();
            ddlSubType.ClearSelection();
            ddlMarketFrm.ClearSelection();
            ddlMarketTo.ClearSelection();
            ddlSubType.Enabled = false;
            //txtOpenDate.Text = string.Empty;
            //txtCloseDate.Text = string.Empty;
        }
        protected void gvTable_PageChanging(object sender, GridViewPageEventArgs e)
        {
            gvTable.PageIndex = e.NewPageIndex;
            Search_Click(null, null);
        }
        #endregion Protected Method

        protected void gvTable_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                GridView HeaderGrid = (GridView)sender;

                e.Row.Cells.Clear();


                GridViewRow HeaderGridRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);
                GridViewRow HeaderGridRow2 = new GridViewRow(1, 0, DataControlRowType.Header, DataControlRowState.Insert);

                TableCell HeaderCell = new TableCell();
                HeaderCell.Text = "Cutoff Grade";
                HeaderCell.RowSpan = 2;
                HeaderGridRow.Cells.Add(HeaderCell);

                HeaderCell = new TableCell();
                HeaderCell.Text = "APPROVE";
                HeaderCell.ColumnSpan = 2;
                HeaderGridRow.Cells.Add(HeaderCell);

                HeaderCell = new TableCell();
                HeaderCell.Text = "REJECT";
                HeaderCell.ColumnSpan = 2;
                HeaderGridRow.Cells.Add(HeaderCell);

                HeaderCell = new TableCell();
                HeaderCell.Text = "อื่นๆ";
                HeaderCell.ColumnSpan = 2;
                HeaderGridRow.Cells.Add(HeaderCell);

                HeaderCell = new TableCell();
                HeaderCell.Text = "รวมทั้งหมด";
                HeaderCell.ColumnSpan = 2;
                HeaderGridRow.Cells.Add(HeaderCell);

                HeaderCell = new TableCell();
                HeaderCell.Text = "ราย";
                HeaderGridRow2.Cells.Add(HeaderCell);

                HeaderCell = new TableCell();
                HeaderCell.Text = "ร้อยละ";
                HeaderGridRow2.Cells.Add(HeaderCell);

                HeaderCell = new TableCell();
                HeaderCell.Text = "ราย";
                HeaderGridRow2.Cells.Add(HeaderCell);

                HeaderCell = new TableCell();
                HeaderCell.Text = "ร้อยละ";
                HeaderGridRow2.Cells.Add(HeaderCell);

                HeaderCell = new TableCell();
                HeaderCell.Text = "ราย";
                HeaderGridRow2.Cells.Add(HeaderCell);

                HeaderCell = new TableCell();
                HeaderCell.Text = "ร้อยละ";
                HeaderGridRow2.Cells.Add(HeaderCell);

                HeaderCell = new TableCell();
                HeaderCell.Text = "ราย";
                HeaderGridRow2.Cells.Add(HeaderCell);

                HeaderCell = new TableCell();
                HeaderCell.Text = "ร้อยละ";
                HeaderGridRow2.Cells.Add(HeaderCell);


                gvTable.Controls[0].Controls.AddAt(0, HeaderGridRow);
                gvTable.Controls[0].Controls.AddAt(1, HeaderGridRow2);


            }
        }


        protected void gvTable_RowDataBound(object sender, GridViewRowEventArgs e)
        {


                if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //TotalDev += Convert.ToInt32(e.Row.Cells[1].Text.Replace(",", ""));

                //TotalDevPercent += Convert.ToDecimal(e.Row.Cells[2].Text);

                //TotalActual += Convert.ToInt32(e.Row.Cells[3].Text.Replace(",", ""));

                //TotalActualPercent += Convert.ToDecimal(e.Row.Cells[4].Text);

                // Just in case we need to show the index total in the gridview, uncomment the line below.
                //TotalPopulationStabilityIndex += Convert.ToDecimal(e.Row.Cells[8].Text);
            }
// ไม่แสดง ค่า ตำนวน Total ที่ Footer 16 สค 2556
            //if (e.Row.RowType == DataControlRowType.Footer)
            //{
            //    if(Session["dtb"] != null)
            //    {
            //        DataTable dt = (DataTable)Session["dtb"];

            //        if(dt != null)
            //        {
                        
            //            if(dt.Rows.Count >0)
            //            {
            //                for (int i = 0; i < dt.Rows.Count; i++)
            //                {
            //                    if (dt.Rows[i]["DEV"].ToString() != "")
            //                    {
            //                        TotalDev += Convert.ToInt32(dt.Rows[i]["DEV"].ToString());
            //                    }

            //                    if (dt.Rows[i]["PDEV"].ToString() != "")
            //                    {
            //                        //g = dt.Rows[i]["PDEV"].ToString();
            //                        TotalDevPercent += Convert.ToDecimal(dt.Rows[i]["PDEV"].ToString());
            //                    }

            //                    if (dt.Rows[i]["ACT"].ToString() != "")
            //                    {
            //                        TotalActual += Convert.ToInt32(dt.Rows[i]["ACT"].ToString());
            //                    }

            //                    if (dt.Rows[i]["PACT"].ToString() != "")
            //                    {
            //                        //p = dt.Rows[i]["PACT"].ToString();
            //                        TotalActualPercent += Convert.ToDecimal(dt.Rows[i]["PACT"].ToString());
            //                    }
            //                }
            //            }
            //        }
            //    }


            //    //=================================================
            //    //Modify for report
            //    //=================================================
            //    Session["TotalDev"] = TotalDev.ToString("#,##0");
            //    Session["TotalDevPercent"] = TotalDevPercent.ToString("0.00");
            //    Session["TotalActual"] = TotalActual.ToString("#,##0");
            //    Session["TotalActualPercent"] = TotalActualPercent.ToString("0.00");
            //    //-------------------------------------------------


            //    if (gvTable.PageIndex == gvTable.PageCount - 1)
            //    {
            //        e.Row.Cells[0].Text = "Total";

            //        e.Row.Cells[1].Text = TotalDev.ToString("#,##0");

            //        e.Row.Cells[2].Text = TotalDevPercent.ToString("0.00");

            //        e.Row.Cells[3].Text = TotalActual.ToString("#,##0");

            //        e.Row.Cells[4].Text = TotalActualPercent.ToString("0.00");

            //    }
            //    else
            //    {
            //        e.Row.Cells[0].Text = "";

            //        e.Row.Cells[1].Text = "";

            //        e.Row.Cells[2].Text = "";

            //        e.Row.Cells[3].Text = "";

            //        e.Row.Cells[4].Text = "";
            //    }

            //}
                //---- Just in case we need to show the index total in the gridview, uncomment the line below.
                //e.Row.Cells[8].Text = TotalPopulationStabilityIndex.ToString("0.000");
        }

        //==========================================================
        //Method to print report
        //==========================================================
        protected void PrintReport_Click(object sender, EventArgs e)
        {
            Label userFirstname = (Label)Master.FindControl("userFirstname");
            Session["userFirstname"] = userFirstname.Text;

            //  //เช็คว่าได้กรอก Market date หรือไม่
            //  if (ddlMarketFrm.SelectedIndex <= 0 || ddlMarketTo.SelectedIndex <= 0)
            //  {
            //      ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScript", "appPop('โปรดเลือกตัวเลือก','โปรดเลือก  Market Code ก่อน');", true);
            //      return;
            //  }

            //เช็คว่าต้องกดแสดงข้อมูลก่อน
            if (gvTable.Rows.Count > 0)
            {
                Print_Preview();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", "appPop('','โปรดเลือกกดแสดงข้อมูลก่อน');", true);
                return;
            }
            // ตัวเช็คว่าได้กดเปลี่ยนวันที่หลังจากกดแสดงข้อมูลหรือหลังจากพิมพ์รายงานหรือไม่
       
            //if ( (Session["Open"]!= null)&&(Session["Close"]!= null))
            //{
            //    string dateChangeOpen = "";
            //    string dateCompareOpen = Session["Open"].ToString();

            //    string dateChangeClose = "";
            //    string dateCompareClose = Session["Close"].ToString();

            //    bool areEqualOpen = string.Equals(dateChangeOpen, dateCompareOpen, StringComparison.Ordinal);
            //    bool areEqualClose = string.Equals(dateChangeClose, dateCompareClose, StringComparison.Ordinal);
               
                
            //    if (areEqualOpen == false)
            //    {
            //        ScriptManager.RegisterStartupScript(this, GetType(), "Alert", "appPop('วันที่เปิดใบคำขอสินเชื่อเปลี่ยนแปลง','โปรดกดแสดงข้อมูลก่อนอีกครั้ง');", true);
            //        return;
            //    }
            //    if ((areEqualOpen == true)&&(areEqualClose == false))
            //    {
            //        ScriptManager.RegisterStartupScript(this, GetType(), "Alert", "appPop('วันที่เปิดใบคำขอสินเชื่อเปลี่ยนแปลง','โปรดกดแสดงข้อมูลก่อนอีกครั้ง');", true);
            //        return;
            //    }
            //}
            // เช็คว่า ประเภทโมเดล,	Model Version	,ประเภทสินเชื่อ	,ประเภทสินเชื่อย่อ มีการกดเปลี่ยนแปลหรือไม่
            //if (flg == true)
            //{
            //    ScriptManager.RegisterStartupScript(this, GetType(), "Alert", "appPop('ข้อมูลมีการเปลี่ยนแปลง','กรุณากดแสดงข้อมูลอีกคั้ง');", true);
            //    return;
            //}

            // เช็คว่า Market Code มีการกดเปลี่ยนจากเดิมหรือไม่
            //if ((Session["mkFrom"] != null) && (Session["mkTo"] != null))
            //{
            //    string mkFromChange = ddlMarketFrm.SelectedItem.ToString();
            //    string mkToChange = ddlMarketTo.SelectedItem.ToString();

            //    string mkCompareFrom = Session["mkFrom"].ToString();
            //    string mkCompareTo = Session["mkTo"].ToString();

            //    bool areEqualFrom = string.Equals(mkFromChange, mkCompareFrom, StringComparison.Ordinal);
            //    bool areEqualTo = string.Equals(mkToChange, mkCompareTo, StringComparison.Ordinal);

            //    if (areEqualFrom == false)
            //    {
            //        ScriptManager.RegisterStartupScript(this, GetType(), "Alert", "appPop('Market Code เปลี่ยนแปลง','โปรดกดแสดงข้อมูลก่อนอีกครั้ง');", true);
            //        return;
            //    }
            //    if ((areEqualFrom == true) && (areEqualTo == false))
            //    {
            //        ScriptManager.RegisterStartupScript(this, GetType(), "Alert", "appPop('Market Code เปลี่ยนแปลง','โปรดกดแสดงข้อมูลก่อนอีกครั้ง');", true);
            //        return;
            //    }
            //}


        }
        private void Print_Preview()
        {
            DataTable dt = (DataTable)Session["dtb"];

            //if (dt == null)
            //{
            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "Alert", "appPop('','โปรดเลือกกดแสดงข้อมูลก่อน');", true);
            //    return;
            //}

            Session["rptData"] = dt; //Report Data
            //Report header
            Session["sHeaderRpt1"] = "ประเภทโมเดล : " + ddlModel.SelectedItem.Text;
            Session["sHeaderRpt2"] = "ประเภทสินเชื่อ / ประเภทสินเชื่อย่อย : " + ddlLoan.SelectedItem.Text + " / " + ddlSubType.SelectedItem.Text;

            if (ddlMarketFrm.SelectedIndex == 0)
                Session["sHeaderRpt3"] = "Market Code : " + ddlMarketFrm.SelectedItem.Text;
            else
                Session["sHeaderRpt3"] = "Market Code : " + ddlMarketFrm.SelectedItem.Text + " ถึง " + ddlMarketTo.SelectedItem.Text;

            Session["sHeaderRpt4"] = "วันที่เปิดใบคำขอสินเชื่อ : " + ddlOpenDateMonth.SelectedValue + " " + ddlOpenDateYear.SelectedItem + " ถึง " + ddlOpenDateMonthend.SelectedValue + " " + ddlOpenDateYearend.SelectedItem;



            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "aa", "window.open('ReportLayOut/frmPreviewAMRrpt.aspx?ReportId=AMR1');", true);
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