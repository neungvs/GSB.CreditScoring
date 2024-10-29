using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using GSB.Class;

namespace GSB.LoanSystem
{
    public partial class MenuSearch : System.Web.UI.Page
    {
        SQLToDataTable conn = new SQLToDataTable();

        protected void Page_Load(object sender, EventArgs e)
        {
            this.btnBack.Attributes["OnClick"] = "window.close();";

            if (!IsPostBack)
            {
                Session.Remove("dsMenuSearch");
                PageInit();
            }
        }

        private void PageInit()
        {
            try
            {
                string Query = string.Format("select [PROG_ID] as MenuID,[PROG_NAME] as MenuName,[PARENTMENUID] as Parent from [S_PROGRAM]");
                //Session["dsMenuSearch"] = conn.ExcuteSQL(Query).DataSet;  //Services.AdminService.Menu_Config(null, 0);

                //gvUser.DataSource = conn.ExcuteSQL(Query);

                this.grdView.DataSource = conn.ExcuteSQL(Query);
                this.grdView.DataBind();
            }
            catch (Exception ex)
            {
               // LogError(ex);
            }
        }

        #region "GridView Event"
        private void SortGridView(string sortExpression, string direction)
        {
        }
        public SortDirection GridViewSortDirection
        {
            get
            {
                if (ViewState["sortDirection"] == null)
                    ViewState["sortDirection"] = SortDirection.Ascending;
                return (SortDirection)ViewState["sortDirection"];
            }
            set { ViewState["sortDirection"] = value; }
        }
        protected void grdView_Sorting(object sender, GridViewSortEventArgs e)
        {
        }
        protected void grdView_OnRowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes.Add("onmouseover", "this.className='viewred'");
                if (e.Row.RowIndex == 0 || (e.Row.RowIndex % 2) == 0)
                {
                    e.Row.Attributes.Add("onmouseout", "this.className='view'");
                }
                else { e.Row.Attributes.Add("onmouseout", "this.className='view2'"); }
            }
        }
        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["OnClick"] = "window.opener.document.forms(0)." + ((Request["ID"] != null) ? Request["ID"].ToString() : "") + ".value='" + (DataBinder.Eval(e.Row.DataItem, "MenuID")).ToString().Trim() + "'; window.close();";
                e.Row.Style["cursor"] = "hand";
            }
        }
        protected void grdView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
        }
        protected void grdView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

            //if (Session["dsMenuSearch"] != null)
            //{
            //    this.grdView.DataSource = (DataSet)Session["dsMenuSearch"];
            //}
            string Query = string.Format("select [PROG_ID] as MenuID,[PROG_NAME] as MenuName,[PARENTMENUID] as Parent from [S_PROGRAM]");
            this.grdView.DataSource = conn.ExcuteSQL(Query);
            this.grdView.PageIndex = e.NewPageIndex;
            this.grdView.DataBind();
        }
        #endregion

        protected void btnMenuSearch_Click(object sender, EventArgs e)
        {
            ArrayList SearchList = new ArrayList();

            //SearchList.Add(txtMenuSearch.Text.Trim());
            //SearchList.Add(txtMenuSearch.Text.Trim());
            //SearchList.Add(txtMenuSearch.Text.Trim());
            //SearchList.Add(txtMenuSearch.Text.Trim());
            //SearchList.Add(txtMenuSearch.Text.Trim());

            try
            {
            //    if (rbByID.Checked)
            //    {
            //        Session["dsMenuSearch"] = Services.AdminService.Menu_Config(commonClass.convertArryListToObjectArray(SearchList), 4);
            //    }
            //    else if (rbByName.Checked)
            //    {
            //        Session["dsMenuSearch"] = Services.AdminService.Menu_Config(commonClass.convertArryListToObjectArray(SearchList), 6);
            //    }
            //    else if (rbByParent.Checked)
            //    {
            //        Session["dsMenuSearch"] = Services.AdminService.Menu_Config(commonClass.convertArryListToObjectArray(SearchList), 7);
            //    }
            //    else if (rbByURL.Checked)
            //    {
            //        Session["dsMenuSearch"] = Services.AdminService.Menu_Config(commonClass.convertArryListToObjectArray(SearchList), 8);
            //    }

            //    this.grdView.DataSource = (DataSet)Session["dsMenuSearch"];
            //    this.grdView.DataBind();

            //    if (this.grdView.Rows.Count < 1)
            //    {
            //        MessageBox.Show(commonClass.Record_Not_Found);
            //    }

            }
            catch (Exception Ex)
            {
               // MessageBox.Show(Ex.Message);
               // LogError(Ex);
            }
        }

    }
}