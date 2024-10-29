using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace GSB.LoanSystem
{
    public partial class ExportLog : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Label1.Text = Request.QueryString["GROUP"];
            
        }

        GSB.Class.SQLToDataTable comm = new GSB.Class.SQLToDataTable();
        protected void btnShow_Click(object sender, EventArgs e)
        {
             string Query = "SELECT (ROW_NUMBER() OVER(order by END_DATE DESC)) AS NO, " +
                               "RESOURCES, START_DATE, END_DATE, TOTAL, STATUS, case when ERROR_MESSAGE is NULL then '' else ERROR_MESSAGE end as ERROR_MESSAGE " +
                               "FROM S_LOG_IMPORT " +
                               "WHERE GROUP_ID = '" + Request.QueryString["GROUP"].Substring(0,3) + "' " +
                               "ORDER BY END_DATE DESC";

            //Get Data
            //string a = ddlBatchGroup.SelectedItem.Text;
            //string b = ddlBatchGroup.SelectedValue.ToString();

            DataTable dtLOG_LN_CHAR = comm.ExcuteSQL(Query);
            string strFileName = Request.QueryString["GROUP"] + System.DateTime.Now.ToString("_yyyyMMdd") + ".csv";
            Response.Clear();
            Response.ContentType = "text/csv";
            Response.AddHeader("content-disposition", "attachment;filename=" + strFileName);
            Response.ContentEncoding = System.Text.Encoding.GetEncoding("ISO-8859-11");
            Response.Charset = "ISO-8859-11";
            Response.Write("NO,RESOURCES,START_DATE,END_DATE,TOTAL,STATUS,ERROR_MESSAGE");
            Response.Write(System.Environment.NewLine);
            foreach (DataRow dr in dtLOG_LN_CHAR.Rows)
            {
                Response.Write(dr["NO"].ToString().Trim() + "," + dr["RESOURCES"].ToString().Trim()
                                + "," + dr["START_DATE"].ToString().Trim() + "," + dr["END_DATE"].ToString().Trim()
                                + "," + dr["TOTAL"].ToString().Trim() + "," + dr["STATUS"].ToString().Trim()
                                + "," + dr["ERROR_MESSAGE"].ToString().Trim()
                                );
                Response.Write(System.Environment.NewLine);
            }
            Response.Flush();
            Response.End();
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            Response.Redirect("BatchAndLog.aspx");
        }
    }
}