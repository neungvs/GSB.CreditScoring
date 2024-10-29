using System;
using GSB.Class;
using System.Data;
using System.Web.UI.WebControls;

namespace GSB.LoanSystem
{
    public partial class MasterTable : System.Web.UI.Page
    {
        SQLToDataTable conn = new SQLToDataTable();

        #region Page_Load

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ddlGrade_DataBound();
                ResultTable.Visible = false;
            }
        }

        #endregion Page_Load

        #region Private Method

        private void ddlGrade_DataBound()
        {
            string Query = string.Format("select * from ST_MASTERTABLE where ACTIVE_FLAG = '1' " +
                                         "union select  0,null,'เลือกตารางหลัก...'[DESC_TH],1 " +
                                         "order by T_ID ASC");

            ddlMasterTable.DataSource = conn.ExcuteSQL(Query);
            ddlMasterTable.DataBind();
        }

        #endregion Private Method

        #region Protected Method

        protected void gvTable_PageChanging(object sender, GridViewPageEventArgs e)
        {
            gvTable.PageIndex = e.NewPageIndex;
            Search_Click(null,null);
        }

        protected void Search_Click(object sender, EventArgs e)
        {
            if (ddlMasterTable.SelectedIndex >= 1)
            {
                ResultTable.Visible = true;
                string appQuery = string.Format("select * from {0} order by 1 asc", ddlMasterTable.SelectedItem.Value);

                gvTable.DataSource = conn.ExcuteSQL(appQuery);
                gvTable.DataBind();
            }
        }

        protected void Cancel_Click(object sender, EventArgs e)
        {
            ddlMasterTable.ClearSelection();
            ResultTable.Visible = false;
        }

        #endregion Protected Method

    }
}