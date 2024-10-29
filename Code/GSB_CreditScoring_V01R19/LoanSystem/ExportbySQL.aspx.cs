using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;
using GSB.Class;
using System.IO;

namespace GSB.LoanSystem
{
    public partial class ExportbySQL : System.Web.UI.Page
    {
        SQLToDataTable conn = new SQLToDataTable();

        #region Page_Load        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {

            }
        }

        #endregion Page_Load
        #region Private Method

        //private string ConvertFieldToQuery(string strSQL)
        //{
        //    DataTable dtExport = null;

        //    if (Session["Export"] == null)
        //    {
        //        dtExport = conn.ExcuteSQL("select ISNULL(C_QUERY,C_NAME), DESC_TH, C_NAME from ST_EXPORT where ACTIVE_FLAG = '1' order by C_ID ASC");
        //        Session.Add("Export", dtExport);
        //    }
        //    else
        //        dtExport = Session["Export"] as DataTable;

        //    DataRow[] foundRows = dtExport.Select(string.Format("C_NAME = '{0}'", strSQL));
        //    return string.Format("{0} [{1}]", foundRows[0][0], foundRows[0][1]);
        //}

        private void DataTableToExcel(string strSQL)
        {
            string Query = string.Format("{0}", strSQL);


            string filename = string.Format("GSB-Credit-Scoring_SQL_(Date_{0}-Time_{1}).csv", DateTime.Now.ToString("dd-MM-yyyy"), DateTime.Now.ToString("HH-mm-ss"));
            StringWriter tw = new StringWriter();
            //HtmlTextWriter hw = new HtmlTextWriter(tw);
            DataGrid dgGrid = new DataGrid();
            DataTable dttb = new DataTable();
            //Server.ScriptTimeout = 10000;
            //Session.Timeout = 10000;
            dttb = conn.ExcuteSQL(Query);
            //tw.Encoding.EncodingName  = UTF8Encoding.UTF8 ;
            //dgGrid.DataSource = conn.ExcuteSQL(Query); ;
            //dgGrid.DataBind();
            //StreamWriter sw;

            int iColCount = new int();
            iColCount = dttb.Columns.Count;
            for (int i = 0; i < iColCount; i++)
            {
                tw.Write(dttb.Columns[i]);
                if (i < iColCount - 1)
                {
                    tw.Write(",");
                }
            }

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
                    if (i < iColCount - 1)
                    {
                        tw.Write(",");
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
        protected void btnExcel_Click(object sender, EventArgs e)
        {
            //StringBuilder selColumn = new StringBuilder();
            //for (int i = 0; i < SelectedColumn.Items.Count; i++)
            //{
            //    selColumn.Append(ConvertFieldToQuery(SelectedColumn.Items[i].Value));
            //    if (i < (SelectedColumn.Items.Count - 1))
            //        selColumn.Append(@", ");
            //}

           // DataTableToExcel(selColumn.ToString());
            DataTableToExcel(txtSQLBX.Text.ToString());
        }
        protected void Cancel_Click(object sender, EventArgs e)
        {
            txtSQLBX.Text = "";
          

        }
 
        #endregion Protected Method
    }
}