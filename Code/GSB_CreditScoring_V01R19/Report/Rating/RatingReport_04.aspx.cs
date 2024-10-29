using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GSB.Class;

namespace GSB.Report.Rating
{
    public partial class RatingReport_04 : System.Web.UI.Page
    {
        SQLToDataRTTable conn = new SQLToDataRTTable();

        #region Page_Load

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                LoadModel();
                LoadSTDate();
            }
        }

        #endregion Page_Load

        #region Private Method

        private void LoadModel()
        {
            string Query = string.Format("select dd_List as MODEL_NAME from Report_Dropdown_List where dd_Group ='NPLS'");

            ddlModel.DataSource = conn.ExcuteSQL(Query);
            ddlModel.DataBind();
        }


        private void LoadSTDate()
        {
            string Query = string.Format("select dd_List as STAT_DATE from Report_Dropdown_List where dd_Group ='STATUS_DATE'");

            //ddlSubType.Items.Clear();
            ddlStatDate.DataSource = conn.ExcuteSQL(Query);
            ddlStatDate.DataBind();
            //ddlSubType.Enabled = true;
        }

        #endregion Private Method

        #region Protected Method
        protected void Search_Click(object sender, EventArgs e)
        {
            if (ddlModel.SelectedIndex < 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScript", "appPop('กรุณาเลือกข้อมูลให้ถูกต้อง')", true);
            }
            else
            {

                        if (ddlStatDate.SelectedIndex < 0)
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScript", "appPop('กรุณาเลือกข้อมูลให้ถูกต้อง')", true);
                        }
                        else
                        {
                            string appQuery1 = string.Format("EXEC [RPT_4_1_1_Oligor_By_Month] '{0}','{1}'", ddlModel.SelectedItem.Value, ddlStatDate.SelectedItem.Value);
                            gvTable.DataSource = conn.ExcuteSQL(appQuery1);
                            gvTable.DataBind();
                            string appQuery2 = string.Format("EXEC [RPT_4_1_2_FSClass_By_Month] '{0}','{1}'", ddlModel.SelectedItem.Value, ddlStatDate.SelectedItem.Value);
                            GridView1.DataSource = conn.ExcuteSQL(appQuery2);
                            GridView1.DataBind();
                            string appQuery3 = string.Format("EXEC [RPT_4_2_1_Oligor_By_Month_Proportion] '{0}','{1}'", ddlModel.SelectedItem.Value, ddlStatDate.SelectedItem.Value);
                            GridView4.DataSource = conn.ExcuteSQL(appQuery3);
                            GridView4.DataBind();
                            string appQuery4 = string.Format("EXEC RPT_4_2_2_FSClass_By_Month_Proportion '{0}','{1}'", ddlModel.SelectedItem.Value, ddlStatDate.SelectedItem.Value);
                            GridView2.DataSource = conn.ExcuteSQL(appQuery4);
                            GridView2.DataBind();

                        }
               }
        }

        protected void Cancel_Click(object sender, EventArgs e)
        {
            ddlModel.ClearSelection();
            ddlStatDate.ClearSelection();
        }
        protected void gvTable_PageChanging(object sender, GridViewPageEventArgs e)
        {
            gvTable.PageIndex = e.NewPageIndex;
            Search_Click(null, null);
        }
        protected void gvTable_PageChanging2(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            Search_Click(null, null);
        }
        protected void gvTable_PageChanging3(object sender, GridViewPageEventArgs e)
        {
            GridView4.PageIndex = e.NewPageIndex;
            Search_Click(null, null);
        }
        protected void gvTable_PageChanging4(object sender, GridViewPageEventArgs e)
        {
            GridView2.PageIndex = e.NewPageIndex;
            Search_Click(null, null);
        }
        #endregion Protected Method
    }
}