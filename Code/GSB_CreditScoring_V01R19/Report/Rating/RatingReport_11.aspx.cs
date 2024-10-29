using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GSB.Class;

namespace GSB.Report.Rating
{
    public partial class RatingReport_11 : System.Web.UI.Page
    {
        SQLToDataRTTable conn = new SQLToDataRTTable();

        #region Page_Load

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
  
                LoadSubType();
            }
        }

        #endregion Page_Load

        #region Private Method

          private void LoadSubType()
        {
            string Query = string.Format("select dd_List as STYPER_NAME from Report_Dropdown_List where dd_Group ='STATUS_DATE'");

            //ddlSubType.Items.Clear();
            ddlSubType.DataSource = conn.ExcuteSQL(Query);
            ddlSubType.DataBind();
            //ddlSubType.Enabled = true;
        }

        #endregion Private Method

        #region Protected Method
        protected void Search_Click(object sender, EventArgs e)
        {

                    if (ddlSubType.SelectedIndex < 0)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScript", "appPop('กรุณาเลือกข้อมูลให้ถูกต้อง')", true);
                    }
                    else
                    {
                        string appQuery1 = string.Format("EXEC [RPT_11_Distribution_Division] '{0}'", ddlSubType.SelectedItem.Value);
                        gvTable.DataSource = conn.ExcuteSQL(appQuery1);
                        gvTable.DataBind();


                    }
              
        }

        protected void Cancel_Click(object sender, EventArgs e)
        {

            ddlSubType.ClearSelection();
        }
        protected void gvTable_PageChanging(object sender, GridViewPageEventArgs e)
        {
            gvTable.PageIndex = e.NewPageIndex;
            Search_Click(null, null);
        }
        #endregion Protected Method
    }
}