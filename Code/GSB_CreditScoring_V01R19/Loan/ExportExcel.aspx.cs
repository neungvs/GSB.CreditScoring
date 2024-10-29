using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GSB.Loan
{
    public partial class ExportExcel : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.ContentType = "text/rtf; charset=UTF-8";


            Response.AppendHeader("Content-Disposition", "attachment; filename=" + Session["filename"] + "");
            Response.AppendHeader("Content-Encoding", "UTF8");
            this.EnableViewState = false;
            //Response.Write(tw.ToString());
            Response.BinaryWrite(System.Text.Encoding.UTF8.GetPreamble());

            string a = Session["str"].ToString();

            Response.Write(Session["str"].ToString());
            Response.End();
        }
    }
}