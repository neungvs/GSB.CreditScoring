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
    public partial class UpdateDataLog : System.Web.UI.Page
    {
        GSB.Class.SQLToDataTable comm = new GSB.Class.SQLToDataTable();

        protected void Page_Load(object sender, EventArgs e)
        {
            lblErrorMessage.Visible = false;
            rf_pgload_Sum();
            GridViewSum.Visible = true;
            GridViewLog.Visible = true;
        }

        protected void btnShow_Click(object sender, EventArgs e)
        {
        }

        protected void rf_pgload_Sum()
        {
            string Query = " SELECT (ROW_NUMBER() OVER(order by t1.TABLE_NAME, t1.CLEANSING_DATE desc)) AS NO, " +
                            " t1.TABLE_NAME,  " +
                            " t1.CLEANSING_DATE " +                            
                            " from (select TABLE_NAME, max(CLEANSING_DATE) as CLEANSING_DATE from LOG_TRN_HISTORY_CLEANSING group by TABLE_NAME) t1 ";

                DataTable dt = comm.ExcuteSQL(Query);

                if (dt != null)
                {
                    GridViewSum.Visible = true;
                    GridViewSum.DataSource = dt;
                    GridViewSum.DataBind();
                }
                else
                {
                    GridViewSum.Visible = false;
                    DataTable source = new DataTable();
                    source.Columns.Add("NO");
                    source.Columns.Add("TABLE_NAME");
                    source.Columns.Add("CLEANSING_DATE");                   
                    source.Rows.Add(source.NewRow());
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

        private void ShowNoResultFound(GridView gv)
        {
        }
        private void SetDefaultView()
        {
            
        }
        protected void lnkTab1_Click(object sender, EventArgs e)
        {
            
        }
        protected void lnkTab2_Click(object sender, EventArgs e)
        {
            
        }
        protected void GridViewSum_RowDataBound(object sender, GridViewRowEventArgs e)
        {
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {

            //Check for search condition
            lblErrorMessage.Visible = false;
            if (txtOpenDate.Text.Trim().Length == 0 || txtCloseDate.Text.Trim().Length == 0)
            {
                lblErrorMessage.Text = "เลื่อกวันที่ที่ต้องการก่อนค้นหา";
                lblErrorMessage.Visible = true;
                return;
            }

            string strSearch = "";
            string strDateSearchCondition = "";
            string strTableName = "";

            //Settle date
            string[] strOpendate = txtOpenDate.Text.Split('/');
            string[] strClosedate = txtCloseDate.Text.Split('/');
            if (int.Parse(strOpendate[2]) > 2500) strOpendate[2] = (int.Parse(strOpendate[2]) - 543).ToString();
            if (int.Parse(strClosedate[2]) > 2500) strClosedate[2] = (int.Parse(strClosedate[2]) - 543).ToString();

            strDateSearchCondition = " convert(char(10),CLEANSING_DATE,121) between '" + strOpendate[2] + "-" + strOpendate[1] + "-" + strOpendate[0] + "' and '" + strClosedate[2] + "-" + strClosedate[1] + "-" + strClosedate[0] + "' ";

            //Select table
            strSearch = " select table_name, CLEAN_REMARK, CLEANSING_DATE,END_Date, COUNT from LOG_TRN_HISTORY_CLEANSING ";
            if (strDateSearchCondition.Trim().Length > 0)
                strSearch += " where " + strDateSearchCondition;

            if ((chkCBS_LN_APP.Checked))
            {
                strTableName += "'CBS_LN_APP',";
            }

            if (chkLN_APP.Checked)
            {
                strTableName += "'LN_APP',";
            }

            if (chkLN_GRADE.Checked)
            {
                strTableName += "'LN_GRADE',";
            }

            if (chkLN_CHAR.Checked)
            {
                strTableName += "'LN_CHAR',";
            }

            if (chkDWH_BTFILE.Checked)
            {
                strTableName += "'DWH_BTFILE',";
            }
            if (strTableName.Trim().Length > 0)
            {
                strTableName = strTableName.Substring(0, strTableName.Trim().Length - 1);
                strSearch += " and table_name in (" + strTableName + ")";
            }
            
            //Create statement to show data
            strSearch = " SELECT (ROW_NUMBER() OVER(order by t1.TABLE_NAME, t1.CLEANSING_DATE,t1.END_DATE desc)) AS ROWNUM, "
                        + " t1.table_name, t1.CLEAN_REMARK, t1.CLEANSING_DATE,t1.END_DATE, t1.COUNT from "
                        + " ( " + strSearch + " )t1 ";

            DataTable dt = comm.ExcuteSQL(strSearch);
           

            if (dt != null) 
            {
                
                Session["SummaryDataPK"] = dt;
               
                    GridViewLog.Visible = true;
                    GridViewLog.DataSource = dt;
                    GridViewLog.DataBind();
                    int rowsCount = GridViewLog.Rows.Count;
                    if (rowsCount == 0)
                    {
                        int columnsCount = GridViewLog.Columns.Count;
                        DataTable source = new DataTable();
                        source.Columns.Add("ROWNUM");
                        source.Columns.Add("table_name");
                        source.Columns.Add("CLEAN_REMARK");
                        source.Columns.Add("CLEANSING_DATE");
                        source.Columns.Add("END_DATE");
                        source.Columns.Add("COUNT");
                        source.Rows.Add(source.NewRow());
                        GridViewLog.DataSource = source;
                        GridViewLog.DataBind();
                        GridViewLog.Rows[0].Cells.Clear();// clear all the cells in the row
                        GridViewLog.Rows[0].Cells.Add(new TableCell()); //add a new blank cell
                        GridViewLog.Rows[0].Cells[0].ColumnSpan = columnsCount; //set the column span to the new added cell
                        GridViewLog.Height = 50;
                        GridViewLog.Width = 720;
                        //You can set the styles here
                        GridViewLog.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                        GridViewLog.Rows[0].Cells[0].ForeColor = System.Drawing.Color.Red;
                        GridViewLog.Rows[0].Cells[0].Font.Bold = true;
                        
        
                        GridViewLog.Rows[0].Cells[0].Text = "ไม่พบข้อมูล!";
                    }

            }
            else 
            {
                GridViewLog.Visible = false;
                DataTable source = new DataTable();
                source.Columns.Add("SearchResult");
                source.Rows.Add(source.NewRow());

                GridViewLog.DataSource = source;
                GridViewLog.DataBind();

                // Get the total number of columns in the GridView to know what the Column Span should be
                int columnsCount = GridViewLog.Columns.Count;
                GridViewLog.Rows[0].Cells.Clear();// clear all the cells in the row
                GridViewLog.Rows[0].Cells.Add(new TableCell()); //add a new blank cell
                GridViewLog.Rows[0].Cells[0].ColumnSpan = columnsCount; //set the column span to the new added cell

                //You can set the styles here
                GridViewLog.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                GridViewLog.Rows[0].Cells[0].ForeColor = System.Drawing.Color.Red;
                GridViewLog.Rows[0].Cells[0].Font.Bold = true;
                //set No Results found to the new added cell
                GridViewLog.Rows[0].Cells[0].Text = "ไม่พบข้อมูล!";
            }


        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            //Search before export
            //--- Summary
            btnSearch_Click(null, null);
            if (Session["SummaryDataPK"] != null)
            {
                DataTable dt = (DataTable)Session["SummaryDataPK"];

                string strFileName = "Summary_" + System.DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".csv";
                Response.Clear();
                Response.ContentType = "text/csv";
                Response.AddHeader("content-disposition", "attachment;filename=" + strFileName);
                Response.ContentEncoding = System.Text.Encoding.GetEncoding("ISO-8859-11");
                Response.Charset = "ISO-8859-11";
                Response.Write("ลำดับ,ชื่อตาราง,ขั้นตอน,วันที่เริ่มดำเนินการ,วันที่ดำเนินการสิ้นสุด,จำนวน");
                Response.Write(System.Environment.NewLine);

                foreach(DataRow dr in dt.Rows)
                {
                    Response.Write(dr["ROWNUM"].ToString().Trim() + "," + dr["TABLE_NAME"].ToString().Trim() + "," + dr["CLEAN_REMARK"].ToString().Trim() + "," + dr["CLEANSING_DATE"].ToString().Trim() + "," + dr["END_DATE"].ToString().Trim() + "," + dr["COUNT"].ToString().Trim());
                    Response.Write(System.Environment.NewLine);
                }

                Response.Flush();
                Response.End();
            }
        }


        protected void btnExport_LOG_CBS_LN_APP_Click(object sender, EventArgs e)
        {
            //Check date
            lblErrorMessage.Visible = false;
            if (txtOpenDate.Text.Trim().Length == 0 || txtCloseDate.Text.Trim().Length == 0)
            {
                lblErrorMessage.Text = "เลื่อกวันที่ที่ต้องการก่อนค้นหา";
                lblErrorMessage.Visible = true;
                return;
            }

            //LOG_CBS_LN_APP
            string strSearch = "";
            string strDateSearchCondition = "";
            //Settle search by date
            string[] strOpendate = txtOpenDate.Text.Split('/');
            string[] strClosedate = txtCloseDate.Text.Split('/');
            if (int.Parse(strOpendate[2]) > 2500) strOpendate[2] = (int.Parse(strOpendate[2]) - 543).ToString();
            if (int.Parse(strClosedate[2]) > 2500) strClosedate[2] = (int.Parse(strClosedate[2]) - 543).ToString();

            strDateSearchCondition = " convert(char(10),CLEANSING_DATE,121) between '" + strOpendate[2] + "-" + strOpendate[1] + "-" + strOpendate[0] + "' and '" + strClosedate[2] + "-" + strClosedate[1] + "-" + strClosedate[0] + "' ";

            strSearch = " select (ROW_NUMBER() OVER(order by [CBS_APP_NO])) AS ROWNUM"
                        + ",[CBS_APP_NO] "
                        + ",[CBS_ISIC1_CD] "
                        + ",[CBS_ISIC2_CD] "
                        + ",[CBS_ISIC3_CD] "
                        + ",[CBS_MPAYAMT] "
                        + ",[CBS_CA_NO] "
                        + ",[CBS_COMMITTEE] "
                        + ",[CBS_APPROVE_CD] "
                        + ",[CBS_REASON_CD] "
                        + ",[CBS_APPROVE_COMMENT] "
                        + ",[CBS_MATURITYDATE] "
                        + ",[CBS_COL] "
                        + ",[CBS_PAYSTATUS] "
                        + ",[BATCH_UPDATE_DTM] "
                        + ",[CBS_CA_DATE] "
                        + ",[CLEAN_REMARK] "
                        + ",[COUNT] "
                        + ",[CLEANSING_DATE] "
                        + " from LOG_CBS_LN_APP ";
            if (strDateSearchCondition.Trim().Length > 0)
                strSearch += " where " + strDateSearchCondition;

            //Get Data
            DataTable dtCBS_LN_APP = comm.ExcuteSQL(strSearch);
            string strFileName = "LOG_CBS_LN_APP_" + System.DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".csv";
            Response.Clear();
            Response.ContentType = "text/csv";
            Response.AddHeader("content-disposition", "attachment;filename=" + strFileName);
            Response.ContentEncoding = System.Text.Encoding.GetEncoding("ISO-8859-11");
            Response.Charset = "ISO-8859-11";
            Response.Write("ROWNUM,CBS_APP_NO,CBS_ISIC1_CD,CBS_ISIC2_CD,CBS_ISIC3_CD,CBS_MPAYAMT,CBS_CA_NO,CBS_COMMITTEE,CBS_APPROVE_CD,CBS_REASON_CD,CBS_APPROVE_COMMENT,CBS_MATURITYDATE,CBS_COL,CBS_PAYSTATUS,BATCH_UPDATE_DTM,CBS_CA_DATE,CLEAN_REMARK,COUNT,CLEANSING_DATE");
            Response.Write(System.Environment.NewLine);
            foreach (DataRow dr in dtCBS_LN_APP.Rows)
            {
                Response.Write(dr["ROWNUM"].ToString().Trim() + "," + dr["CBS_APP_NO"].ToString().Trim()
                                + "," + dr["CBS_ISIC1_CD"].ToString().Trim() + "," + dr["CBS_ISIC2_CD"].ToString().Trim()
                                + "," + dr["CBS_ISIC3_CD"].ToString().Trim() + "," + dr["CBS_MPAYAMT"].ToString().Trim()
                                + "," + dr["CBS_CA_NO"].ToString().Trim() + "," + dr["CBS_COMMITTEE"].ToString().Trim()
                                + "," + dr["CBS_APPROVE_CD"].ToString().Trim() + "," + dr["CBS_REASON_CD"].ToString().Trim()
                                + "," + dr["CBS_APPROVE_COMMENT"].ToString().Trim() + "," + dr["CBS_MATURITYDATE"].ToString().Trim()
                                + "," + dr["CBS_COL"].ToString().Trim() + "," + dr["CBS_PAYSTATUS"].ToString().Trim()
                                + "," + dr["BATCH_UPDATE_DTM"].ToString().Trim() + "," + dr["CBS_CA_DATE"].ToString().Trim()
                                + "," + dr["CLEAN_REMARK"].ToString().Trim() + "," + dr["COUNT"].ToString().Trim()
                                + "," + dr["CLEANSING_DATE"].ToString().Trim()
                                );
                Response.Write(System.Environment.NewLine);
            }
            Response.Flush();
            Response.End();

        }

        protected void btnExport_LOG_LN_APP_Click(object sender, EventArgs e)
        {
            //Check date
            lblErrorMessage.Visible = false;
            if (txtOpenDate.Text.Trim().Length == 0 || txtCloseDate.Text.Trim().Length == 0)
            {
                lblErrorMessage.Text = "เลื่อกวันที่ที่ต้องการก่อนค้นหา";
                lblErrorMessage.Visible = true;
                return;
            }

            //LN_APP
            string strSearch = "";
            string strDateSearchCondition = "";
            //Settle search by date
            string[] strOpendate = txtOpenDate.Text.Split('/');
            string[] strClosedate = txtCloseDate.Text.Split('/');
            if (int.Parse(strOpendate[2]) > 2500) strOpendate[2] = (int.Parse(strOpendate[2]) - 543).ToString();
            if (int.Parse(strClosedate[2]) > 2500) strClosedate[2] = (int.Parse(strClosedate[2]) - 543).ToString();

            strDateSearchCondition = " convert(char(10),CLEANSING_DATE,121) between '" + strOpendate[2] + "-" + strOpendate[1] + "-" + strOpendate[0] + "' and '" + strClosedate[2] + "-" + strClosedate[1] + "-" + strClosedate[0] + "' ";

            //LN_APP
            strSearch = " select (ROW_NUMBER() OVER(order by [APP_NO])) AS ROWNUM"
                            + " ,[APP_NO] "
                            + " ,[APP_DATE] "
                            + " ,[ACCTYPE] "
                            + " ,[LOAN_CD] "
                            + " ,[STYPE_CD] "
                            + " ,[MARKET_CD] "
                            + " ,[LOAN_TERM] "
                            + " ,[LOAN_AMOUNT] "
                            + " ,[BRANCH_CD] "
                            + " ,[PURPOSE_CD] "
                            + " ,[CONSUMPTION_CD] "
                            + " ,[GSBPURPOSE_CD] "
                            + " ,[CREATION_BY] "
                            + " ,[CREATION_DT] "
                            + " ,[CREATION_TM] "
                            + " ,[LNSTATUS] "
                            + " ,[LNDATESTATUS] "
                            + " ,[PAYMENT_CD] "
                            + " ,[LNCOLLVAL] "
                            + " ,[MPAYAMT] "
                            + " ,[APPRAMT] "
                            + " ,[APPRTERM] "
                            + " ,[ISIC1_CD] "
                            + " ,[ISIC2_CD] "
                            + " ,[ISIC3_CD] "
                            + " ,[APP_SEQ] "
                            + " ,[CBS_STATUS] "
                            + " ,[CA_DATE] "
                            + " ,[DWH_BADMNT] "
                            + " ,[CLEAN_REMARK] "
                            + " ,[COUNT] "
                            + " ,[CLEANSING_DATE] "
                        + " from LOG_LN_APP ";
            if (strDateSearchCondition.Trim().Length > 0)
                strSearch += " where " + strDateSearchCondition;

            //Get Data
            DataTable dtLN_APP = comm.ExcuteSQL(strSearch);
            string strFileName = "LOG_LN_APP_" + System.DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".csv";
            Response.Clear();
            Response.ContentType = "text/csv";
            Response.AddHeader("content-disposition", "attachment;filename=" + strFileName);
            Response.ContentEncoding = System.Text.Encoding.GetEncoding("ISO-8859-11");
            Response.Charset = "ISO-8859-11";
            Response.Write("ROWNUM,APP_NO,APP_DATE,ACCTYPE,LOAN_CD,STYPE_CD,MARKET_CD,LOAN_TERM,LOAN_AMOUNT,BRANCH_CD,PURPOSE_CD,CONSUMPTION_CD,GSBPURPOSE_CD,CREATION_BY,CREATION_DT,CREATION_TM,LNSTATUS,LNDATESTATUS,PAYMENT_CD,LNCOLLVAL,MPAYAMT,APPRAMT,APPRTERM,ISIC1_CD,ISIC2_CD,ISIC3_CD,APP_SEQ,CBS_STATUS,CA_DATE,DWH_BADMNT,CLEAN_REMARK,COUNT,CLEANSING_DATE");
            Response.Write(System.Environment.NewLine);
            foreach (DataRow dr in dtLN_APP.Rows)
            {
                Response.Write(dr["ROWNUM"].ToString().Trim() + "," + dr["APP_NO"].ToString().Trim()
                                + "," + dr["APP_DATE"].ToString().Trim() + "," + dr["ACCTYPE"].ToString().Trim()
                                + "," + dr["LOAN_CD"].ToString().Trim() + "," + dr["STYPE_CD"].ToString().Trim()
                                + "," + dr["MARKET_CD"].ToString().Trim() + "," + dr["LOAN_TERM"].ToString().Trim()
                                + "," + dr["LOAN_AMOUNT"].ToString().Trim() + "," + dr["BRANCH_CD"].ToString().Trim()
                                + "," + dr["PURPOSE_CD"].ToString().Trim() + "," + dr["CONSUMPTION_CD"].ToString().Trim()
                                + "," + dr["GSBPURPOSE_CD"].ToString().Trim() + "," + dr["CREATION_BY"].ToString().Trim()
                                + "," + dr["CREATION_DT"].ToString().Trim() + "," + dr["CREATION_TM"].ToString().Trim()
                                + "," + dr["LNSTATUS"].ToString().Trim() + "," + dr["LNDATESTATUS"].ToString().Trim()
                                + "," + dr["PAYMENT_CD"].ToString().Trim() + "," + dr["LNCOLLVAL"].ToString().Trim()
                                + "," + dr["MPAYAMT"].ToString().Trim() + "," + dr["APPRAMT"].ToString().Trim()
                                + "," + dr["APPRTERM"].ToString().Trim() + "," + dr["ISIC1_CD"].ToString().Trim()
                                + "," + dr["ISIC2_CD"].ToString().Trim() + "," + dr["ISIC3_CD"].ToString().Trim()
                                + "," + dr["APP_SEQ"].ToString().Trim()
                                + "," + dr["CBS_STATUS"].ToString().Trim() + "," + dr["CA_DATE"].ToString().Trim() + "," + dr["DWH_BADMNT"].ToString().Trim()
                                + "," + dr["CLEAN_REMARK"].ToString().Trim() + "," + dr["COUNT"].ToString().Trim()
                                + "," + dr["CLEANSING_DATE"].ToString().Trim()
                                );
                Response.Write(System.Environment.NewLine);
            }
            Response.Flush();
            Response.End();

        }

        protected void btnExport_LOG_LN_GRADE_Click(object sender, EventArgs e)
        {
            //Check date
            lblErrorMessage.Visible = false;
            if (txtOpenDate.Text.Trim().Length == 0 || txtCloseDate.Text.Trim().Length == 0)
            {
                lblErrorMessage.Text = "เลื่อกวันที่ที่ต้องการก่อนค้นหา";
                lblErrorMessage.Visible = true;
                return;
            }

            //LN_GRADE
            string strSearch = "";
            string strDateSearchCondition = "";
            //Settle search by date
            string[] strOpendate = txtOpenDate.Text.Split('/');
            string[] strClosedate = txtCloseDate.Text.Split('/');
            if (int.Parse(strOpendate[2]) > 2500) strOpendate[2] = (int.Parse(strOpendate[2]) - 543).ToString();
            if (int.Parse(strClosedate[2]) > 2500) strClosedate[2] = (int.Parse(strClosedate[2]) - 543).ToString();

            strDateSearchCondition = " convert(char(10),CLEANSING_DATE,121) between '" + strOpendate[2] + "-" + strOpendate[1] + "-" + strOpendate[0] + "' and '" + strClosedate[2] + "-" + strClosedate[1] + "-" + strClosedate[0] + "' ";

            //LN_GRADE
            strSearch = " select (ROW_NUMBER() OVER(order by [APP_NO])) AS ROWNUM"
                            + " ,APP_NO "
                            + " ,APP_DATE "
                            + " ,APP_SEQ "
                            + " ,SCORE "
                            + " ,GRADE "
                            + " ,GRADE_ACTION "
                            + " ,MODEL "
                            + " ,MODEL_VERSION "
                            + " ,CLEAN_REMARK "
                            + " ,COUNT "
                            + " ,CLEANSING_DATE "
                        + " from LOG_LN_GRADE ";
            if (strDateSearchCondition.Trim().Length > 0)
                strSearch += " where " + strDateSearchCondition;

            //Get Data
            DataTable dtLN_APP = comm.ExcuteSQL(strSearch);
            string strFileName = "LOG_LN_GRADE_" + System.DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".csv";
            Response.Clear();
            Response.ContentType = "text/csv";
            Response.AddHeader("content-disposition", "attachment;filename=" + strFileName);
            Response.ContentEncoding = System.Text.Encoding.GetEncoding("ISO-8859-11");
            Response.Charset = "ISO-8859-11";
            Response.Write("ROWNUM,APP_NO,APP_DATE,APP_SEQ,SCORE,GRADE,GRADE_ACTION,MODEL,MODEL_VERSION,CLEAN_REMARK,COUNT,CLEANSING_DATE");
            Response.Write(System.Environment.NewLine);
            foreach (DataRow dr in dtLN_APP.Rows)
            {
                Response.Write(dr["ROWNUM"].ToString().Trim() + "," + dr["APP_NO"].ToString().Trim()
                                + "," + dr["APP_DATE"].ToString().Trim() + "," + dr["APP_SEQ"].ToString().Trim()
                                + "," + dr["SCORE"].ToString().Trim() + "," + dr["GRADE"].ToString().Trim()
                                + "," + dr["GRADE_ACTION"].ToString().Trim() + "," + dr["MODEL"].ToString().Trim()
                                + "," + dr["MODEL_VERSION"].ToString().Trim()
                                + "," + dr["CLEAN_REMARK"].ToString().Trim() + "," + dr["COUNT"].ToString().Trim()
                                + "," + dr["CLEANSING_DATE"].ToString().Trim()
                                );
                Response.Write(System.Environment.NewLine);
            }
            Response.Flush();
            Response.End();
        }

        protected void btnExport_LOG_LN_CHAR_Click(object sender, EventArgs e)
        {
            //Check date
            lblErrorMessage.Visible = false;
            if (txtOpenDate.Text.Trim().Length == 0 || txtCloseDate.Text.Trim().Length == 0)
            {
                lblErrorMessage.Text = "เลื่อกวันที่ที่ต้องการก่อนค้นหา";
                lblErrorMessage.Visible = true;
                return;
            }

            //LOG_LN_CHAR
            string strSearch = "";
            string strDateSearchCondition = "";
            //Settle search by date
            string[] strOpendate = txtOpenDate.Text.Split('/');
            string[] strClosedate = txtCloseDate.Text.Split('/');
            if (int.Parse(strOpendate[2]) > 2500) strOpendate[2] = (int.Parse(strOpendate[2]) - 543).ToString();
            if (int.Parse(strClosedate[2]) > 2500) strClosedate[2] = (int.Parse(strClosedate[2]) - 543).ToString();

            strDateSearchCondition = " convert(char(10),CLEANSING_DATE,121) between '" + strOpendate[2] + "-" + strOpendate[1] + "-" + strOpendate[0] + "' and '" + strClosedate[2] + "-" + strClosedate[1] + "-" + strClosedate[0] + "' ";

            //LOG_LN_CHAR
            strSearch = " select (ROW_NUMBER() OVER(order by [CBS_APP_NO])) AS ROWNUM"
                        + " ,CBS_APP_NO "
                        + " ,APP_SEQ "
                        + " ,CBS_CIFNO "
                        + " ,CHAR_CD "
                        + " ,ATTR_CD "
                        + " ,SCORE "
                        + " ,CIFSCORE "
                        + " ,CLEAN_REMARK "
                        + " ,COUNT "
                        + " ,CLEANSING_DATE "
                        + " from LOG_LN_CHAR ";
            if (strDateSearchCondition.Trim().Length > 0)
                strSearch += " where " + strDateSearchCondition;

            //Get Data
            DataTable dtLOG_LN_CHAR = comm.ExcuteSQL(strSearch);
            string strFileName = "LOG_LN_CHAR" + System.DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".csv";
            Response.Clear();
            Response.ContentType = "text/csv";
            Response.AddHeader("content-disposition", "attachment;filename=" + strFileName);
            Response.ContentEncoding = System.Text.Encoding.GetEncoding("ISO-8859-11");
            Response.Charset = "ISO-8859-11";
            Response.Write("ROWNUM,CBS_APP_NO,APP_SEQ,CBS_CIFNO,CHAR_CD,ATTR_CD,SCORE,CIFSCORE,CLEAN_REMARK,COUNT,CLEANSING_DATE");
            Response.Write(System.Environment.NewLine);
            foreach (DataRow dr in dtLOG_LN_CHAR.Rows)
            {
                Response.Write(dr["ROWNUM"].ToString().Trim() + "," + dr["CBS_APP_NO"].ToString().Trim()
                                + "," + dr["APP_SEQ"].ToString().Trim() + "," + dr["CBS_CIFNO"].ToString().Trim()
                                + "," + dr["CHAR_CD"].ToString().Trim() + "," + dr["ATTR_CD"].ToString().Trim()
                                + "," + dr["SCORE"].ToString().Trim() + "," + dr["CIFSCORE"].ToString().Trim()
                                + "," + dr["CLEAN_REMARK"].ToString().Trim() + "," + dr["COUNT"].ToString().Trim()
                                + "," + dr["CLEANSING_DATE"].ToString().Trim()
                                );
                Response.Write(System.Environment.NewLine);
            }
            Response.Flush();
            Response.End();


        }

        protected void btnExport_LOG_DWH_BTFILE_Click(object sender, EventArgs e)
        {
            //Check date
            lblErrorMessage.Visible = false;
            if (txtOpenDate.Text.Trim().Length == 0 || txtCloseDate.Text.Trim().Length == 0)
            {
                lblErrorMessage.Text = "เลื่อกวันที่ที่ต้องการก่อนค้นหา";
                lblErrorMessage.Visible = true;
                return;
            }

            //LOG_DWH_BTFILE
            string strSearch = "";
            string strDateSearchCondition = "";
            //Settle search by date
            string[] strOpendate = txtOpenDate.Text.Split('/');
            string[] strClosedate = txtCloseDate.Text.Split('/');
            if (int.Parse(strOpendate[2]) > 2500) strOpendate[2] = (int.Parse(strOpendate[2]) - 543).ToString();
            if (int.Parse(strClosedate[2]) > 2500) strClosedate[2] = (int.Parse(strClosedate[2]) - 543).ToString();

            strDateSearchCondition = " convert(char(10),CLEANSING_DATE,121) between '" + strOpendate[2] + "-" + strOpendate[1] + "-" + strOpendate[0] + "' and '" + strClosedate[2] + "-" + strClosedate[1] + "-" + strClosedate[0] + "' ";

            //LOG_DWH_BTFILE
            strSearch = " select (ROW_NUMBER() OVER(order by [CBS_APP_NO])) AS ROWNUM"
                        + " ,[CBS_APP_NO] "
                        + " ,[DW_ACC_NO] "
                        + " ,[DW_FDISDATE] "
                        + " ,[DW_CDATE] "
                        + " ,[DW_EXPDATE] "
                        + " ,[DW_SIGN_DATE] "
                        + " ,[ISACTIVE] "
                        + " ,[DW_BADMONTH] "
                        + " ,[DW_OUTSTAND] "
                        + " ,[LAST_UPDATE_DTM] "
                        + " ,[DW_BADAMOUNT] "
                        + " ,[DW_ORG_TERM] "
                        + " ,[DW_FREQ_TERM] "
                        + " ,[DW_ORG_TERM_MULT] "
                        + " ,[DW_COLLVALUE] "
                        + " ,[CSTATUS_CD] "
                        + " ,[DW_ISRESTRUCT] "
                        + " ,[LOAN_TYPE] "
                        + " ,[FILE_LOAD_DATE] "
                        + " ,[CLEAN_REMARK] "
                        + " ,[COUNT] "
                        + " ,[CLEANSING_DATE] "
                        + " from LOG_DWH_BTFILE ";
            if (strDateSearchCondition.Trim().Length > 0)
                strSearch += " where " + strDateSearchCondition;

            //Get Data
            DataTable dtLOG_DWH_BTFILE = comm.ExcuteSQL(strSearch);
            string strFileName = "LOG_DWH_BTFILE" + System.DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".csv";
            Response.Clear();
            Response.ContentType = "text/csv";
            Response.AddHeader("content-disposition", "attachment;filename=" + strFileName);
            Response.ContentEncoding = System.Text.Encoding.GetEncoding("ISO-8859-11");
            Response.Charset = "ISO-8859-11";
            Response.Write("ROWNUM,CBS_APP_NO,DW_ACC_NO,DW_FDISDATE,DW_CDATE,DW_EXPDATE,DW_SIGN_DATE,ISACTIVE,DW_BADMONTH,DW_OUTSTAND,LAST_UPDATE_DTM,DW_BADAMOUNT,DW_ORG_TERM,DW_FREQ_TERM,DW_ORG_TERM_MULT,DW_COLLVALUE,CSTATUS_CD,DW_ISRESTRUCT,LOAN_TYPE,FILE_LOAD_DATE,CLEAN_REMARK,COUNT,CLEANSING_DATE");

            Response.Write(System.Environment.NewLine);
            foreach (DataRow dr in dtLOG_DWH_BTFILE.Rows)
            {
                Response.Write(dr["ROWNUM"].ToString().Trim() + "," + dr["CBS_APP_NO"].ToString().Trim()
                                + "," + dr["DW_ACC_NO"].ToString().Trim() + "," + dr["DW_FDISDATE"].ToString().Trim()
                                + "," + dr["DW_CDATE"].ToString().Trim() + "," + dr["DW_EXPDATE"].ToString().Trim()
                                + "," + dr["DW_SIGN_DATE"].ToString().Trim() + "," + dr["ISACTIVE"].ToString().Trim()
                                + "," + dr["DW_BADMONTH"].ToString().Trim() + "," + dr["DW_OUTSTAND"].ToString().Trim()
                                + "," + dr["LAST_UPDATE_DTM"].ToString().Trim() + "," + dr["DW_BADAMOUNT"].ToString().Trim()
                                + "," + dr["DW_ORG_TERM"].ToString().Trim() + "," + dr["DW_FREQ_TERM"].ToString().Trim()
                                + "," + dr["DW_ORG_TERM_MULT"].ToString().Trim() + "," + dr["DW_COLLVALUE"].ToString().Trim()
                                + "," + dr["CSTATUS_CD"].ToString().Trim() + "," + dr["DW_ISRESTRUCT"].ToString().Trim()
                                + "," + dr["LOAN_TYPE"].ToString().Trim() + "," + dr["FILE_LOAD_DATE"].ToString().Trim()
                                + "," + dr["CLEAN_REMARK"].ToString().Trim() + "," + dr["COUNT"].ToString().Trim()
                                + "," + dr["CLEANSING_DATE"].ToString().Trim()
                                );
                Response.Write(System.Environment.NewLine);
            }
            Response.Flush();
            Response.End();

        }
        protected void btnBack_Click(Object sender, EventArgs e)
        {
            Response.Redirect("UpdateDataLog.aspx");
        }
    }
}