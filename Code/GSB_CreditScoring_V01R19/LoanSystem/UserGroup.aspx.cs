using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using GSB.Class;
using System.Collections;
using System.Data.SqlClient;
using System.Configuration;
using Arsoft.Utility;

namespace GSB.LoanSystem
{
    public partial class UserGroup : System.Web.UI.Page
    {
        SQLToDataTable conn = new SQLToDataTable();

        #region Page_Load

        protected void Page_Load(object sender, EventArgs e)
        {
            btnSearchMenu.Attributes.Add("onclick", @"window.open('MenuSearch.aspx?ID=" + this.txtAddMenu.ClientID + "', 'MenuSearch','scrollbars =1,status=0,toolbar=0,height=540,width=500')");

            //txtAddMenu.Attributes.Add("OnKeyPress", "DisableEnter(this)");
            //txtDescription.Attributes.Add("OnKeyPress", "DisableEnter(this)");
            //txtGroupName.Attributes.Add("OnKeyPress", "DisableEnter(this)");
             if (!Page.IsPostBack)
            {
                Session.Remove("GroupData");
                Session.Remove("GroupID");
                Session.Remove("dsMenu");

                Session.Remove("SortDirection");
                Session.Remove("SortDirection2");

                Label_GroupName.Text = "";
                HiddenField_GroupID.Value = null;

                ddlGrade_DataBound();
                //ResultTable.Visible = false;
            }
        }

        #endregion Page_Load
        private void ddlGrade_DataBound()
        {
            string Query = string.Format("select Group_id as GroupID, Group_NAME as GROUPNAME, Group_Desc as GROUPDESCRIPTION from S_GROUP");

            gvGroups.DataSource = conn.ExcuteSQL(Query);
            gvGroups.DataBind();
        }
         protected void btnAdd_Click(object sender, EventArgs e)
    {
        if (txtGroupName.Text.Length < 1)
        {
            return;
        }

        try
        {
          
            string sqlStatementdgrp = "insert into s_group (GROUP_ID,GROUP_NAME,GROUP_DESC,ACTIVE_FLAG) values((select max(gid) as gid from s_group),'" +
               txtGroupName.Text.Trim() + "','" + txtDescription.Text.Trim() + "',1)";
           
            conn.ExcuteSQL(sqlStatementdgrp);
           
            Session.Remove("GroupData");

            ddlGrade_DataBound();

            ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScript", "appPop('GSB','ดำเนินการสำเร็จ')", true);


        }
        catch (Exception Ex)
        {
            //MessageBox.Show(Ex.Message);
            //LogError(Ex);
        }

    }
    protected void btnUpdate_Click(object sender, EventArgs e)
    {
            if (txtGroupName.Text.Length < 1)
            {
                return;
            }
            try
            {
                string sqlStatementdgrp = "update s_group set GROUP_NAME = '" + txtGroupName.Text.Trim() + "', GROUP_DESC = '" + txtDescription.Text.Trim() + "' where GROUP_ID = " + HiddenField_GroupID.Value;

                conn.ExcuteSQL(sqlStatementdgrp);

                Session.Remove("GroupData");

                ddlGrade_DataBound();

                ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScript", "appPop('GSB','ดำเนินการสำเร็จ')", true);


            }
            catch (Exception Ex)
            {
                //MessageBox.Show(Ex.Message);
                //LogError(Ex);
            }
        }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        txtGroupName.Text = "";
        txtDescription.Text = "";
        txtAddMenu.Text = "";

        btnAdd.Enabled = true;
        btnUpdate.Enabled = false;

        gvGroups.SelectedIndex = -1;
        gvGroups.PageIndex = 0;
        Response.Redirect("~/Default.aspx");
      //  this.PageInit();
       
    }
 
    protected void gvGroups_Sorting(object sender, GridViewSortEventArgs e)
    {
        ((GridView)sender).PageIndex = 0;

        if (Session["SortDirection"] != null)
        {
            e.SortDirection = (SortDirection)Session["SortDirection"];
        }

        if (e.SortDirection == SortDirection.Ascending)
        {
            e.SortDirection = SortDirection.Descending;
        }
        else
        {
            e.SortDirection = SortDirection.Ascending;
        }

        Session["SortDirection"] = e.SortDirection;

        DataView dataView = (DataView)Session["GroupData"];
        //dataView.Sort = e.SortExpression + " " + commonClass.ConvertSortDirection(e.SortDirection);

        ((GridView)sender).DataSource = dataView;
        ((GridView)sender).DataBind();

        Session["GroupData"] = dataView;
    }



    protected void gvGroups_OnRowDeleting(object sender, GridViewDeleteEventArgs e)
    {

    }
    protected void gvGroups_SelectedIndexChanged(object sender, EventArgs e)
    {
    }
    protected void gvGroups_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        ((GridView)sender).PageIndex = e.NewPageIndex;
        ((GridView)sender).DataSource = (DataView)Session["GroupData"];
        ((GridView)sender).DataBind();
    }
    protected void gvGroups_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton lm = (ImageButton)e.Row.FindControl("ImageButton_Delete");
                //lm.Attributes.Add("onclick", "javascript:return confirm('" + commonClass.Comfirm_Dialog + "( " + (DataBinder.Eval(e.Row.DataItem, "GroupName")).ToString().Trim() + " )');");
            }
        }
        catch (Exception ex)
        {
            //LogError(ex);
            //MessageBox.Show(Ex.Message);
        }
    }
    protected void gvGroups_OnRowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.Equals("Delete"))
        {

            try
            {
                ArrayList arrlist = new ArrayList();
                arrlist.Add(e.CommandArgument.ToString().Trim());

                string sqlStatementdgrp = "delete from s_group where Group_id ='" + e.CommandArgument.ToString().Trim() + "'";
           
                conn.ExcuteSQL(sqlStatementdgrp);
           
                Session.Remove("GroupData");

                ddlGrade_DataBound();

                ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScript", "appPop('GSB','ดำเนินการสำเร็จ')", true);

            }
            catch (Exception Ex)
            {
                //MessageBox.Show(Ex.Message);
                //LogError(Ex);
            }

        }
        else if (e.CommandName.Equals("Select"))
        {
            //ArrayList arrlist = new ArrayList();
            //arrlist.Add(e.CommandArgument.ToString().Trim());

            //DataSet ds = Services.AdminService.KTBGroup_Config(commonClass.convertArryListToObjectArray(arrlist), 4);
            string sqlStatementdgrp = "select GROUP_ID,GROUP_NAME,GROUP_DESC from s_group where Group_id ='" + e.CommandArgument.ToString().Trim() + "'";

            DataTable ds = conn.ExcuteSQL(sqlStatementdgrp);
            if (ds.Rows.Count > 0)
            {
                try
                {
                    HiddenField_GroupID.Value = ds.Rows[0].ItemArray[0].ToString().Trim();
                    txtGroupName.Text = ds.Rows[0].ItemArray[1].ToString().Trim();
                    txtDescription.Text = ds.Rows[0].ItemArray[2].ToString().Trim();

                    ////Save Log for Admin
                    //ArrayList TmpOldValue = new ArrayList();
                    //TmpOldValue.Add(ds.Rows[0].ItemArray[1].ToString().Trim());
                    //TmpOldValue.Add(ds.Rows[0].ItemArray[2].ToString().Trim());
                    //ViewState["OldValue"] = TmpOldValue;

                    btnAdd.Enabled = false;
                    btnUpdate.Enabled = true;

                }
                catch (Exception Ex)
                {
                    //    MessageBox.Show( Ex.Message );
                    //    LogError(Ex);
                }
            }
        }
        else if (e.CommandName.Equals("Function"))
        {
            Session["GroupID"] = e.CommandArgument.ToString().Trim();

            Label_GroupName.Text = "";
            HiddenField_GroupID.Value = null;

            try
            {
                ArrayList arrlist = new ArrayList();
                arrlist.Add(e.CommandArgument.ToString().Trim());
                string sqlStatementdgrp = "select GROUP_ID,GROUP_NAME,GROUP_DESC from s_group where Group_id ='" + e.CommandArgument.ToString().Trim() + "'";

                DataTable ds = conn.ExcuteSQL(sqlStatementdgrp);
               // DataSet ds = Services.AdminService.KTBGroup_Config(commonClass.convertArryListToObjectArray(arrlist), 4);

                if (ds.Rows.Count > 0)
                {
                    HiddenField_GroupID.Value = ds.Rows[0].ItemArray[0].ToString().Trim();
                    Label_GroupName.Text = ds.Rows[0].ItemArray[1].ToString().Trim();
                }

                //DataSet ds_FTMenuTmp = this.Services.AdminService.GroupMenu_Config(commonClass.convertArryListToObjectArray(arrlist), 6);
                string sqlStatementdgrp2 = "SELECT GROUP_ID,a.PROG_ID as menuid,PROG_NAME as menuname,PARENTMENUID as parent " +
                    "FROM [S_GPROG] a, [S_PROGRAM] b where a.[PROG_ID] = b.[PROG_ID] and a.Group_id ='" + e.CommandArgument.ToString().Trim() + "'";

                DataTable ds_FTMenuTmp = conn.ExcuteSQL(sqlStatementdgrp2);

                DataView dataView = new DataView((DataTable)ds_FTMenuTmp);
                Session["dsMenu"] = dataView;


                gvAvaMenu.DataSource = dataView;
                gvAvaMenu.DataBind();
                gvAvaMenu.SelectedIndex = -1;


            }
            catch (Exception Ex)
            {
                //MessageBox.Show(Ex.Message);
                //LogError(Ex);
            }
            PanelMain.Visible = false;
            PanelFunction.Visible = true;
        }
    }

    // Function Section ====================================================================================

    protected void btnAddMenu_Click(object sender, EventArgs e)
    {
        bool chk = true;

        if (txtAddMenu.Text.Length >= 0)
        {

            foreach (GridViewRow grow in this.gvAvaMenu.Rows)
            {
                if (grow.Cells[1].Text.Trim().Equals(txtAddMenu.Text.Trim()))
                {
                    chk = false;
                    break;
                }
            }

            if (chk && HiddenField_GroupID.Value != null)
            {
                try
                {
                    ArrayList list = new ArrayList();
                    list.Add(HiddenField_GroupID.Value);
                    list.Add(txtAddMenu.Text.Trim());

                    //DataSet ds_FTMenu = Services.AdminService.GroupMenu_Config(commonClass.convertArryListToObjectArray(list), 1);

                    //DataSet ds_FTMenuTmp = this.Services.AdminService.GroupMenu_Config(commonClass.convertArryListToObjectArray(list), 6);
                    string sqlStatementdgrp = "select PROG_ID,PROG_NAME from s_program where prog_id ='" + txtAddMenu.Text.Trim() + "'";

                    DataTable ds_FTMenu = conn.ExcuteSQL(sqlStatementdgrp);
                    //DataView dataView = new DataView((DataTable)ds_FTMenuTmp.Tables[0]);
                    //Session["dsMenu"] = dataView;

                    if (ds_FTMenu.Rows.Count > 0)
                    {
                        string sqlStatementdadd = "insert into s_gprog values('" + HiddenField_GroupID.Value + "','" + txtAddMenu.Text.Trim() + "',1)";

                        conn.ExcuteSQL(sqlStatementdadd); 
                     //   return;
                    }

                    //Save Log For Admin
                    //ArrayList arlist = new ArrayList();
                    //arlist.Add(txtAddMenu.Text.Trim());
                    //LogForAdmin(BaseAdmin.Add, arlist);
                    string sqlStatementdgrp2 = "SELECT GROUP_ID,a.PROG_ID as menuid,PROG_NAME as menuname,PARENTMENUID as parent " +
    "FROM [S_GPROG] a, [S_PROGRAM] b where a.[PROG_ID] = b.[PROG_ID] and a.Group_id ='" + HiddenField_GroupID.Value + "'";

                    DataTable ds_FTMenuTmp = conn.ExcuteSQL(sqlStatementdgrp2);

                    DataView dataView = new DataView((DataTable)ds_FTMenuTmp);
                    Session["dsMenu"] = dataView;
                    gvAvaMenu.DataSource = dataView;
                    gvAvaMenu.DataBind();
                    gvAvaMenu.SelectedIndex = -1;


                    txtAddMenu.Text = "";

                    //MessageBox.Show(commonClass.SuccessStatus_insert);
                }
                catch (Exception Ex)
                {
                    //MessageBox.Show(Ex.Message);
                    //LogError(Ex);
                }
            }
            else
            {
                //ArrayList list = new ArrayList();
                //list.Add("Err001");

                //DataSet ds = Services.AdminService.ErrorCode(commonClass.convertArryListToObjectArray(list), 4);

                //if (ds != null && ds.Tables.Count > 0)
                //{
                //    MessageBox.Show(ds.Tables[0].Rows[0].ItemArray[0].ToString().Trim());
                //}
                //else
                //{
                //    MessageBox.Show("Cannot Add This Menu.");
                //}
                
            }

        }
    }

    protected void btnMenuClear_Click(object sender, EventArgs e)
    {
        Session.Remove("GroupID");
        Session.Remove("dsMenu");

        txtGroupName.Text = "";
        txtDescription.Text = "";
        txtAddMenu.Text = "";

        PanelMain.Visible = true;
        PanelFunction.Visible = false;
    }    

    protected void gvAvaMenu_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        //if (e.Row.RowType == DataControlRowType.DataRow)
        //{
        //    try
        //    {
        //        ImageButton lm = (ImageButton)e.Row.FindControl("ImageButton_MenuRemove");
        //        if (lm != null)
        //        {
        //            lm.Attributes.Add("onclick", "javascript:return confirm('"+ commonClass.Comfirm_Dialog_remove +" ( " + (DataBinder.Eval(e.Row.DataItem, "MenuName")).ToString().Trim() + " )');");
        //        }
        //    }
        //    catch (Exception ex) { LogError(ex); }
        //}
    }
    protected void gvAvaMenu_OnRowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.Equals("MenuRemove"))
            {

                ArrayList arListTmp = new ArrayList();
                arListTmp.Add(Session["GroupID"]);
                arListTmp.Add(e.CommandArgument.ToString().Trim());

                string sqlStatementdgrp = "delete from s_gprog where group_id = '" + HiddenField_GroupID.Value + "' and prog_id = '" + e.CommandArgument.ToString().Trim() + "'";

                DataTable ds_FTMenu = conn.ExcuteSQL(sqlStatementdgrp);

                string sqlStatementdgrp2 = "SELECT GROUP_ID,a.PROG_ID as menuid,PROG_NAME as menuname,PARENTMENUID as parent " +
"FROM [S_GPROG] a, [S_PROGRAM] b where a.[PROG_ID] = b.[PROG_ID] and a.Group_id ='" + HiddenField_GroupID.Value + "'";

                DataTable ds_FTMenuTmp = conn.ExcuteSQL(sqlStatementdgrp2);

                DataView dataView = new DataView((DataTable)ds_FTMenuTmp);

                Session["dsMenu"] = dataView;

                gvAvaMenu.DataSource = dataView;
                gvAvaMenu.DataBind();
                gvAvaMenu.SelectedIndex = -1;

                //MessageBox.Show(commonClass.SuccessStatus_remove);
            }

        }
        catch (Exception Ex)
        {
            //MessageBox.Show(Ex.Message);
            //LogError(Ex);
        }

    }
    protected void gvAvaMenu_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        this.gvAvaMenu.PageIndex = e.NewPageIndex;

        if (Session["dsMenu"] != null)
        {
            this.gvAvaMenu.DataSource = (DataView)Session["dsMenu"];
        }

        this.gvAvaMenu.DataBind();
    }

    protected void gvAvaMenu_Sorting(object sender, GridViewSortEventArgs e)
    {
        //((GridView)sender).PageIndex = 0;

        //if (Session["SortDirection2"] != null)
        //{
        //    e.SortDirection = (SortDirection)Session["SortDirection2"];
        //}

        //if (e.SortDirection == SortDirection.Ascending)
        //{
        //    e.SortDirection = SortDirection.Descending;
        //}
        //else
        //{
        //    e.SortDirection = SortDirection.Ascending;
        //}

        //Session["SortDirection2"] = e.SortDirection;

        //DataView dataView = (DataView)Session["dsMenu"];
        //dataView.Sort = e.SortExpression + " " + commonClass.ConvertSortDirection(e.SortDirection);

        //((GridView)sender).DataSource = dataView;
        //((GridView)sender).DataBind();

        //Session["dsMenu"] = dataView;
    }

    protected void btnSearchMenu_Click(object sender, EventArgs e)
    {

    }


    //protected void ddlRecsPage_IndexChanged(object sender, EventArgs e)
    //{
    //    this.PageInit();
    //}



    // Menu Section ====================================================================================

 
    }
}