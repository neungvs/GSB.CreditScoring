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
    public partial class UserProfile : System.Web.UI.Page
    {
        SQLToDataTable conn = new SQLToDataTable();

        protected void Page_Load(object sender, EventArgs e)
        {
            //txtUserID.Attributes.Add("OnKeyPress", "ChkInt(this)");
            //  txtUserName.Attributes.Add("OnKeyPress", "DisableEnter(" + this.txtUserName.ClientID + ")");
            //  ddlGroup.Attributes.Add("OnKeyPress", "DisableEnter(" + this.ddlGroup.ClientID + ")");
            //  cbLock.Attributes.Add("OnKeyPress", "DisableEnter(" + this.cbLock.ClientID + ")");
            //       btn_SelectGroup.Attributes.Add("onclick", "javascript:return confirm('"+commonClass.Comfirm_Dialog_gp+");");

            if (!IsPostBack)
            {
                Session["UserData"] = null;
                Session["UserGroupData"] = null;
                Session.Remove("SortDirection");
                Session.Remove("SortDirection2");
                InitPage();
                ddlGrade_DataBound();
            }

            //this.btnAdd.Focus();
        }
        private void ddlGrade_DataBound()
        {
            string currentUser = string.Format("{0}", Request.Cookies["Credit_Scoring_GSB"]["UID"]);

            // SqlConnection cnn = new SqlConnection(employee.ConnectionString);
            string cnn = ConfigurationManager.ConnectionStrings["GSBSQLServer"].ConnectionString;

            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter("SELECT GROUP_ID FROM S_USER where UID = " + currentUser, cnn);
            da.Fill(dt);

            string cnn2 = ConfigurationManager.ConnectionStrings["GSBSQLServer"].ConnectionString;

            DataTable dt2 = new DataTable();
            SqlDataAdapter UID = new SqlDataAdapter("SELECT UID FROM S_USER where UID = " + currentUser, cnn);
            UID.Fill(dt2);


            if (dt.Rows[0]["GROUP_ID"].ToString() == "001")
            {

                string Query = string.Format("select USER_ID,USER_PWD,GROUP_NAME as GROUPS,USER_PNAME,USER_FNAME,USER_SNAME,USER_DESC,a.ACTIVE_FLAG from s_User a left join s_group b on a.group_id= b.group_id");

                gvUser.DataSource = conn.ExcuteSQL(Query);
                gvUser.DataBind();
            }
            else
            {

                string Query = string.Format("SELECT     USER_ID, USER_PWD, GROUPS, USER_PNAME, USER_FNAME, USER_SNAME, USER_DESC, ACTIVE_FLAG " +
                                              "FROM   (SELECT     a.UID, a.USER_ID, a.USER_PWD, b.GROUP_NAME AS GROUPS, a.USER_PNAME, a.USER_FNAME, a.USER_SNAME, a.USER_DESC, a.ACTIVE_FLAG " +
                                              "FROM     S_USER AS a LEFT OUTER JOIN S_GROUP AS b ON a.GROUP_ID = b.GROUP_ID) AS T " +
                                              "where UID= " + dt2.Rows[0]["UID"].ToString());

                gvUser.DataSource = conn.ExcuteSQL(Query);
                gvUser.DataBind();
            }
        }
        private void InitPage()
        {
            // btnAdd.Enabled = false;


            try
            {

                ListItem GroupListItem = new ListItem();
                GroupListItem.Text = "";
                GroupListItem.Value = "";
                ddlGroup.Items.Clear();
                ddlGroup.Items.Add(GroupListItem);

                string sqlStatementdgrp = "select Group_ID,Group_NAME from s_Group ";

                DataTable UserClassDS = conn.ExcuteSQL(sqlStatementdgrp);
                //DataView dataView = new DataView((DataTable)ds_FTMenuTmp.Tables[0]);
                //Session["dsMenu"] = dataView;

                if (UserClassDS.Rows.Count > 0)

                //DataSet UserClassDS = this.Services.AdminService.PositionConfig(null, 0);
                {
                    foreach (DataRow row in UserClassDS.Rows)
                    {
                        GroupListItem = new ListItem();
                        GroupListItem.Text = row.ItemArray[1].ToString().Trim();
                        GroupListItem.Value = row.ItemArray[0].ToString().Trim();

                        ddlGroup.Items.Add(GroupListItem);
                    }
                }
                txtUserpwd.Enabled = true;
                txtUserpName.Enabled = true;
                txtUserName.Enabled = true;
                txtUserSurName.Enabled = true;
                txtUserDesc.Enabled = true;


            }
            catch (Exception Ex)
            {
                //MessageBox.Show(Ex.Message);
                //LogError(Ex);
            }
        }

        //===========================================================================

        protected void gvUser_OnRowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("Delete"))
            {
                try
                {
                    ArrayList arrlist = new ArrayList();
                    arrlist.Add(e.CommandArgument.ToString().Trim());

                    string sqlStatementdgrp = "delete from s_User where user_id = '" + e.CommandArgument.ToString().Trim() + "'";

                    DataTable UserClassDS = conn.ExcuteSQL(sqlStatementdgrp);

                    btnAdd.Enabled = true;
                    btnUpdate.Enabled = false;

                    Session["UserData"] = null;

                    ddlGrade_DataBound();

                    //InitPage();

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScript", "appPop('GSB','ดำเนินการสำเร็จ')", true);

                    //MessageBox.Show(commonClass.SuccessStatus_delete);
                }
                catch (Exception Ex)
                {
                    //MessageBox.Show(Ex.Message);
                    //LogError(Ex);
                }
            }
            else if (e.CommandName.Equals("Select"))
            {
                txtUserID.Enabled = false;

                ArrayList arrlist = new ArrayList();
                arrlist.Add(e.CommandArgument.ToString().Trim());

                string sqlStatementdgrp = "select USER_ID,USER_PWD,GROUP_ID,USER_PNAME,USER_FNAME,USER_SNAME,USER_DESC,ACTIVE_FLAG from s_User where user_id = '" + e.CommandArgument.ToString().Trim() + "'";

                DataTable ds = conn.ExcuteSQL(sqlStatementdgrp);
                if (ds.Rows.Count > 0)
                {
                    try
                    {
                        txtUserID.Text = ds.Rows[0].ItemArray[0].ToString().Trim();
                        //txtUserpwd.Text = ds.Rows[0].ItemArray[1].ToString().Trim();
                        txtUserpName.Text = ds.Rows[0].ItemArray[3].ToString().Trim();
                        txtUserName.Text = ds.Rows[0].ItemArray[4].ToString().Trim();
                        txtUserSurName.Text = ds.Rows[0].ItemArray[5].ToString().Trim();
                        txtUserDesc.Text = ds.Rows[0].ItemArray[6].ToString().Trim();
                        ddlGroup.SelectedIndex = ddlGroup.Items.IndexOf(ddlGroup.Items.FindByValue(ds.Rows[0].ItemArray[2].ToString()));
                        cbLock.Checked = ((bool)ds.Rows[0].ItemArray[7]); //.ToString().Trim().Equals("True")) ? true : false;

                        btnAdd.Enabled = false;
                        btnUpdate.Enabled = true;
                        txtUserpwd.Enabled = true;
                        txtUserpName.Enabled = true;
                        txtUserName.Enabled = true;
                        txtUserSurName.Enabled = true;
                        txtUserDesc.Enabled = true;
                        lblstr.Text = "กำลังแก้ไขรายการ (Edit Mode)";
                    }
                    catch (Exception Ex)
                    {
                        lblstr.Text = Ex.Message;
                        //LogError(Ex);
                    }
                }
            }
        }
        protected void gvUser_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                try
                {
                    ImageButton lm = (ImageButton)e.Row.FindControl("ImageButton_Delete");
                    // lm.Attributes.Add("onclick", "javascript:return confirm('"+commonClass.Comfirm_Dialog+" ( UserID " + (DataBinder.Eval(e.Row.DataItem, "UserID")).ToString().Trim() + " )');");
                }
                catch (Exception ex)
                {// LogError(ex);
                }


            }
        }
        protected void gvUser_OnRowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }
        protected void gvUser_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        protected void gvUser_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.gvUser.PageIndex = e.NewPageIndex;

            this.gvUser.SelectedIndex = -1;
            this.lblstr.Text = "";

            if (Session["UserData"] != null)
            {
                this.gvUser.DataSource = (DataView)Session["UserData"];
                this.gvUser.DataBind();
            }
        }

        protected void gvUser_Sorting(object sender, GridViewSortEventArgs e)
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

            DataView dataView = (DataView)Session["UserData"];
            // dataView.Sort = e.SortExpression + " " + commonClass.ConvertSortDirection(e.SortDirection);

            ((GridView)sender).DataSource = dataView;
            ((GridView)sender).DataBind();

            Session["UserData"] = dataView;
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {

            txtUserID.Enabled = true;
            txtUserID.Text = "";
            txtUserName.Text = "";
            ddlGroup.SelectedIndex = -1;
            //  ddlPositionID.SelectedIndex = -1;        
            cbLock.Checked = false;

            gvUser.SelectedIndex = -1;
            gvUser.PageIndex = 0;

            lblstr.Text = "";

            btnAdd.Enabled = false;
            btnUpdate.Enabled = false;
            Response.Redirect("~/Default.aspx");
        }
        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            int Cpage = this.gvUser.PageIndex;
            CryptographyUtil crypt = new CryptographyUtil();

            ArrayList arTmp = new ArrayList();

            try
            {

                //arTmp.Add(txtUserID.Text.Trim());
                //arTmp.Add(txtUserName.Text.Trim());
                //arTmp.Add(ddlBranch.SelectedValue);
                //arTmp.Add(ddlPositionID.SelectedValue);
                //arTmp.Add((cbLock.Checked) ? "1" : "0");
                //arTmp.Add((cbLock.Checked) ? "L" : "U");
                //arTmp.Add((cbLock.Checked) ? "2" : "0");

                //string sqlStatementdgrp = "update s_user set USER_PWD = '" + 
                //    txtUserpwd.Text.Trim() + "', GROUP_ID = '" + 
                //    ddlGroup.SelectedValue + "',USER_PNAME = '" +
                //    txtUserpName.Text.Trim() + "',USER_FNAME = '" + 
                //    txtUserName.Text.Trim() + "',USER_SNAME = '" + 
                //    txtUserSurName.Text.Trim() + "',USER_DESC = '" +
                //    txtUserDesc.Text.Trim() + "',ACTIVE_FLAG = '" + 
                //    cbLock.Checked + "' where USER_ID = '" + txtUserID.Text.Trim() + "'";

                string sqlStatementdgrp = "update s_user set "
                    + "GROUP_ID = '" + ddlGroup.SelectedValue
                    + "',USER_PNAME = '" + txtUserpName.Text.Trim()
                    + "',USER_FNAME = '" + txtUserName.Text.Trim()
                    + "',USER_SNAME = '" + txtUserSurName.Text.Trim()
                    + "',USER_DESC = '" + txtUserDesc.Text.Trim()
                    + "',ACTIVE_FLAG = '" + cbLock.Checked + "' ";

                if (txtUserpwd.Text.Trim().Length > 0)
                {
                    sqlStatementdgrp = sqlStatementdgrp + ",USER_PWD = '" + crypt.EncryptedMD5(txtUserpwd.Text.Trim()) + "' ";
                }

                sqlStatementdgrp = sqlStatementdgrp + " where USER_ID = '" + txtUserID.Text.Trim() + "'";


                DataTable ds = conn.ExcuteSQL(sqlStatementdgrp);

                //DataSet ds = this.Services.AdminService.KTBUser_Config(commonClass.convertArryListToObjectArray(arTmp), 2);

                //if (ds == null)
                //{
                //    MessageBox.Show(Services.AdminService.GetErrorMsg("Err003"));
                //}

                //Services.AdminService.TraceUserAdmin(Session["UserID"].ToString().Trim(), "UPDATE User (" + txtUserID.Text.Trim() + ")");
                //Services.AdminService.TraceUserAdmin(Session["UserID"].ToString().Trim(), "UserID " + txtUserID.Text.Trim() + " clear status (Logoff) By " + Session["UserID"].ToString().Trim() + ".");

                btnAdd.Enabled = true;
                btnUpdate.Enabled = false;
                ddlGrade_DataBound();
                txtUserID.Text = "";
                txtUserpwd.Text = "";
                txtUserpName.Text = "";
                txtUserName.Text = "";
                txtUserSurName.Text = "";
                txtUserDesc.Text = "";
                ddlGroup.SelectedIndex = -1;
                cbLock.Checked = false;

                ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScript", "appPop('GSB','ดำเนินการสำเร็จ')", true);

            }
            catch (Exception Ex)
            {
                //MessageBox.Show(Ex.Message);
                //LogError(Ex);
            }

            Session["UserData"] = null;


            //btnCancel_Click(null, null);
            // InitPage();

            this.gvUser.PageIndex = Cpage;
            this.gvUser.DataBind();

            ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScript", "appPop('GSB','ดำเนินการสำเร็จ')", true);


            //MessageBox.Show(commonClass.SuccessStatus_update);

        }
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            ArrayList arTmp = new ArrayList();
            CryptographyUtil crypt = new CryptographyUtil();

            bool chk = true;

            if (txtUserID.Text.Length >= 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScript", "appPop('GSB','ดำเนินการสำเร็จ')", true);


                foreach (GridViewRow grow in this.gvUser.Rows)
                {
                    if (grow.Cells[1].Text.Trim().Equals(txtUserID.Text.Trim()))
                    {
                        chk = false;
                        break;
                    }
                }

                if (chk)
                {
                    try
                    {
                        //arTmp.Add(txtUserID.Text.Trim());
                        //arTmp.Add(txtUserName.Text.Trim());
                        //arTmp.Add(ddlBranch.SelectedValue);
                        //arTmp.Add(ddlPositionID.SelectedValue);
                        //arTmp.Add("0");
                        //arTmp.Add((cbLock.Checked) ? "L" : "U");
                        //arTmp.Add("0");

                        string sqlStatementdgrp = "insert into s_user (USER_ID,USER_PWD,GROUP_ID,USER_PNAME,USER_FNAME,USER_SNAME,USER_DESC,ACTIVE_FLAG) values('" +
                    txtUserID.Text.Trim() + "','" +
                    crypt.EncryptedMD5(txtUserpwd.Text.Trim()) + "','" +
                    ddlGroup.SelectedValue + "','" +
                    txtUserpName.Text.Trim() + "','" +
                    txtUserName.Text.Trim() + "','" +
                    txtUserSurName.Text.Trim() + "','" +
                    txtUserDesc.Text.Trim() + "','" +
                    cbLock.Checked + "')";


                        DataTable ds = conn.ExcuteSQL(sqlStatementdgrp);
                        btnAdd.Enabled = true;
                        btnUpdate.Enabled = false;


                        ddlGrade_DataBound();

                        ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScript", "appPop('GSB','ดำเนินการสำเร็จ')", true);

                    }
                    catch (Exception Ex)
                    {
                        //MessageBox.Show(Ex.Message);
                        //LogError(Ex);
                    }

                    //Session["UserData"] = null;


                    //InitPage();


                    //MessageBox.Show(commonClass.SuccessStatus_insert);
                }
            }
        }
    }

}