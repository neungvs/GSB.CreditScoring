using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace GSB.LoanSystem
{
    public partial class BatchAndLog : System.Web.UI.Page
    {
        GSB.Class.SQLToDataTable comm = new GSB.Class.SQLToDataTable();

        protected void Page_Load(object sender, EventArgs e)

        {
            rf_pgload_Sum();
            GridViewSum.Visible = true;

            if (!IsPostBack)
            {
                SetDefaultView();
            }

            if (Session["BatchLog_SQLQuery"] != null || Request.QueryString["page"] != null)
            {
                GridView1.Visible = true;
                btnShow_Click(null, null);
            }
            else
            {
                GridView1.Visible = false;
                ShowNoResultFound(GridView1);
            }
        }

        protected void btnShow_Click(object sender, EventArgs e)
        {
            if (ddlBatchGroup.SelectedItem.Value != null)
            {
                string Query = "SELECT (ROW_NUMBER() OVER(order by END_DATE DESC)) AS NO, " +
                               "RESOURCES, START_DATE, END_DATE, TOTAL, STATUS, case when ERROR_MESSAGE is NULL then '' else ERROR_MESSAGE end as ERROR_MESSAGE " +
                               "FROM S_LOG_IMPORT " +
                               "WHERE GROUP_ID = '" + ddlBatchGroup.SelectedItem.Value + "' " +
                               "ORDER BY log_id DESC";

                DataTable dt = comm.ExcuteSQL(Query);

                if (dt != null)
                {
                    GridView1.Visible = true;
                    Session["BatchLog_SQLQuery"] = dt;
                    GridView1.DataSource = dt;
                    GridView1.DataBind();
                }
                else
                {
                    GridView1.Visible = false;
                    ShowNoResultFound(GridView1);
                }

            }
        }
        protected void rf_pgload_Sum()
        {
            string Query = "SELECT (ROW_NUMBER() OVER(order by GROUP_ID, daterun desc )) AS NO, " + 
                            "case when Group_ID = '001' then 'Batch LOPs'  " + 
                            "	when Group_ID = '002' then 'Batch Master'  " + 
                            "	when Group_ID = '003' then 'Batch DWH'  " + 
                            "	when Group_ID = '004' then 'Batch Mymo' " +
                            "  when Group_ID = '005' then 'Batch OBA'" +
                            " when Group_ID = '006' then 'Batch MyMoMonthly' " +
                            " when Group_ID = '007' then 'Batch Digital loan' " +
                            " END as RESOURCES,  " + 
                            "	daterun as START_DATE,  " + 
                            "	case when st>0 then 'FAIL' else 'DONE' end AS STATUS " + 
                            "from (select  " + 
                            "bb.GROUP_ID ,convert(varchar,maxdt,111) as daterun , sum(case when STATUS = 'DONE' then 0 else 1 end) as st " + 
                            "from (select distinct " + 
                            "GROUP_ID ,RESOURCES, (select top 1 START_DATE from S_LOG_IMPORT a  " + 
                            "where a.GROUP_ID = S_LOG_IMPORT.GROUP_ID and a.RESOURCES = S_LOG_IMPORT.RESOURCES  order by START_DATE desc) as maxdt " + 
                            "FROM S_LOG_IMPORT ) aa, S_LOG_IMPORT bb " + 
                            "where aa.GROUP_ID = bb.GROUP_ID  and aa.RESOURCES = bb.RESOURCES  " +
                            "and bb.START_DATE = aa.maxdt " + 
                            "group by bb.GROUP_ID ,convert(varchar,maxdt,111) ) cc ";

                DataTable dt = comm.ExcuteSQL(Query);

                if (dt != null)
                {
                    GridViewSum.Visible = true;
                   // Session["BatchLog_SQLQuery"] = dt;
                    GridViewSum.DataSource = dt;
                    GridViewSum.DataBind();
                }
                else
                {
                    GridViewSum.Visible = false;
                    DataTable source = new DataTable();
                    source.Columns.Add("NO");
                    source.Columns.Add("RESOURCES");
                    source.Columns.Add("START_DATE");
                     source.Columns.Add("STATUS");
                    source.Rows.Add(source.NewRow()); // create a new blank row to the DataTable
                    // Bind the DataTable which contain a blank row to the GridView

                    GridViewSum.DataSource = source;
                    GridViewSum.DataBind();
                    // Get the total number of columns in the GridView to know what the Column Span should be
                    int columnsCount = GridViewSum.Columns.Count;
                    GridViewSum.Rows[0].Cells.Clear();// clear all the cells in the row
                    GridViewSum.Rows[0].Cells.Add(new TableCell()); //add a new blank cell
                    GridViewSum.Rows[0].Cells[0].ColumnSpan = columnsCount; //set the column span to the new added cell

                    //You can set the styles here
                    GridViewSum.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                    GridViewSum.Rows[0].Cells[0].ForeColor = System.Drawing.Color.Red;
                    GridViewSum.Rows[0].Cells[0].Font.Bold = true;
                    //set No Results found to the new added cell
                    GridViewSum.Rows[0].Cells[0].Text = "ไม่พบข้อมูล!";
                }

            }
        //protected void btnShow_Click2(object sender, EventArgs e)
        //{
        //    if (ddlBatchGroup2.SelectedItem.Value != null)
        //    {
        //        //string Query = "SELECT (ROW_NUMBER() OVER(order by END_DATE DESC)) AS NO, " +
        //        //               "RESOURCES, START_DATE, END_DATE, TOTAL, STATUS, case when ERROR_MESSAGE is NULL then '' else ERROR_MESSAGE end as ERROR_MESSAGE " +
        //        //               "FROM S_LOG_IMPORT " +
        //        //               "WHERE GROUP_ID = '" + ddlBatchGroup2.SelectedItem.Value + "' " +
        //        //               "ORDER BY END_DATE DESC";
        //        //SqlCommand comm = 
        //        //DataTable dt = comm.ExcuteSQL(Query);
        //        MultiView1.ActiveViewIndex = 0;
        //        ddlBatchGroup2.Enabled = false;
        //        btn2Show.Enabled = false;
        //        btn2Show.Visible = false;
        //        GridView1.Visible = false;
        //        DataTable source = new DataTable();
        //        source.Columns.Add("NO");
        //        source.Columns.Add("RESOURCES");
        //        source.Columns.Add("START_DATE");
        //        source.Columns.Add("END_DATE");
        //        source.Columns.Add("TOTAL");
        //        source.Columns.Add("ERROR_MESSAGE");
        //        source.Columns.Add("STATUS");
        //        source.Rows.Add(source.NewRow()); // create a new blank row to the DataTable
        //        // Bind the DataTable which contain a blank row to the GridView
        //        GridView1.DataSource = source;
        //        GridView1.DataBind();
        //        // Get the total number of columns in the GridView to know what the Column Span should be
        //        int columnsCount = GridView1.Columns.Count;
        //        GridView1.Rows[0].Cells.Clear();// clear all the cells in the row
        //        GridView1.Rows[0].Cells.Add(new TableCell()); //add a new blank cell
        //        GridView1.Rows[0].Cells[0].ColumnSpan = columnsCount; //set the column span to the new added cell

        //        //You can set the styles here
        //        GridView1.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
        //        GridView1.Rows[0].Cells[0].ForeColor = System.Drawing.Color.Red;
        //        GridView1.Rows[0].Cells[0].Font.Bold = true;
        //        //set No Results found to the new added cell
        //        GridView1.Rows[0].Cells[0].Text = "กำลังดำเนินงาน";
        //        GridView1.DataBind();

        //        //using (var conn = new SqlConnection(connectionString)) 
        //        //using (var command = new SqlCommand("ProcedureName", conn) {                             
        //        //    CommandType = CommandType.StoredProcedure }) {    
        //        //    conn.Open();    
        //        //    command.ExecuteNonQuery();    
        //        //    conn.Close(); 
        //        //} 
                
        //                SqlConnection errcon = new SqlConnection(ConfigurationManager.AppSettings["FTPIP"].ToString());
        //                errcon.Open();
        //                string sqlStatementderr = "";
        //        switch (ddlBatchGroup2.SelectedItem.Value)
        //        {
        //            case "001":
        //               sqlStatementderr = "sprc_BTCCBS";
        //               break;
        //            case "002":
        //               sqlStatementderr = "sprc_BTCMaster";
        //               break;
        //            case "003":
        //               sqlStatementderr = "sprc_BTCDWH";
        //               break;
        //            case "004":
        //               sqlStatementderr = "";
        //               break;
        //        }
        //                SqlCommand cmddlerr = new SqlCommand(sqlStatementderr, errcon);
        //                cmddlerr.ExecuteNonQuery();
        //                errcon.Close();

        //                ddlBatchGroup2.Enabled = true;
        //                btn2Show.Visible = true;
                
        //        //if (dt != null)
        //        //{
        //        //    GridView1.Visible = true;
        //        //    Session["BatchLog_SQLQuery"] = dt;
        //        //    GridView1.DataSource = dt;
        //        //    GridView1.DataBind();
        //        //}
        //        //else
        //        //{
        //        //    GridView1.Visible = false;
        //        //    ShowNoResultFound(GridView1);
        //        //}

        //    }
        //}

        private void ShowNoResultFound(GridView gv)
        {
            DataTable source = new DataTable();
            source.Columns.Add("NO");
            source.Columns.Add("RESOURCES");
            source.Columns.Add("START_DATE");
            source.Columns.Add("END_DATE");
            source.Columns.Add("TOTAL");
            source.Columns.Add("ERROR_MESSAGE");
            source.Columns.Add("STATUS");
            source.Rows.Add(source.NewRow()); // create a new blank row to the DataTable
            // Bind the DataTable which contain a blank row to the GridView
            GridView1.DataSource = source;
            GridView1.DataBind();
            // Get the total number of columns in the GridView to know what the Column Span should be
            int columnsCount = gv.Columns.Count;
            GridView1.Rows[0].Cells.Clear();// clear all the cells in the row
            GridView1.Rows[0].Cells.Add(new TableCell()); //add a new blank cell
            GridView1.Rows[0].Cells[0].ColumnSpan = columnsCount; //set the column span to the new added cell

            //You can set the styles here
            GridView1.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
            GridView1.Rows[0].Cells[0].ForeColor = System.Drawing.Color.Red;
            GridView1.Rows[0].Cells[0].Font.Bold = true;
            //set No Results found to the new added cell
            GridView1.Rows[0].Cells[0].Text = "ไม่พบข้อมูล!";
        }

        private void SetDefaultView()
        {
            MultiView1.ActiveViewIndex = 0;
        }

        protected void lnkTab1_Click(object sender, EventArgs e)
        {
            MultiView1.ActiveViewIndex = 0;
        }
        protected void lnkTab2_Click(object sender, EventArgs e)
        {
            MultiView1.ActiveViewIndex = 1;
        }
        protected void GridViewSum_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.Cells[3].Text == "FAIL")
                {
                    e.Row.Cells[3].BackColor = System.Drawing.Color.Red;
                }
                //else if (e.Row.Cells[3].Text == "WARN")
                //{
                //    e.Row.Cells[3].ForeColor = System.Drawing.Color.Black;
                //}
                else
                {
                    e.Row.Cells[3].BackColor = System.Drawing.Color.Green;
                }
            }
        }
        protected void RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.Cells[5].Text == "FAIL")
                {
                    e.Row.Cells[0].BackColor = System.Drawing.Color.Pink;
                    e.Row.Cells[1].BackColor = System.Drawing.Color.Pink;
                    e.Row.Cells[2].BackColor = System.Drawing.Color.Pink;
                    e.Row.Cells[3].BackColor = System.Drawing.Color.Pink;
                    e.Row.Cells[4].BackColor = System.Drawing.Color.Pink;
                    e.Row.Cells[5].BackColor = System.Drawing.Color.Pink;
                    e.Row.Cells[6].BackColor = System.Drawing.Color.Pink;
                }

            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            Response.Redirect("ExportLog.aspx?GROUP=" + ddlBatchGroup.SelectedItem.Text.ToString());
        }
    }
}