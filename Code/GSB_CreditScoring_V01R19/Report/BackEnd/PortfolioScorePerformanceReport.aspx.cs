using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GSB.Class;
using System.Text;
using System.Globalization;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace GSB.Report.BackEnd
{
    public partial class PortfolioScorePerformanceReport : System.Web.UI.Page
    {
        SQLToDataTable conn = new SQLToDataTable();
        string appQuery1;
        #region Page_Load

        protected void Page_Load(object sender, EventArgs e)
        {
            //conn = new SQLToDataTable();
            if (!Page.IsPostBack)
            {
                LoadYearMonth();
                LoadModel();
                //LoadLoan();
                //LoadBranch();
                LoadRegian();
                //LoadModelVersion();
            }

        }

        private void LoadYearMonth()
        {
            int ychis, ybud;

            ychis = DateTime.Now.Year;

            if (ychis < 2500)
            {
                ybud = ychis + 543;
            }
            else
            {
                ybud = ychis;
                ychis = ychis - 543;
            }


            ddlOpenDateYear.Items.Clear();
            ddlOpenDateYearend.Items.Clear();

            for (int i = 0; i < 10; i++)
            {
                ddlOpenDateYear.Items.Insert(i, new ListItem((ybud - i).ToString(), (ychis - i).ToString()));
                ddlOpenDateYearend.Items.Insert(i, new ListItem((ybud - i).ToString(), (ychis - i).ToString()));
                ddlOpenDateYearDTM.Items.Insert(i, new ListItem((ybud - i).ToString(), (ychis - i).ToString()));
                ddlOpenDateYearDTMend.Items.Insert(i, new ListItem((ybud - i).ToString(), (ychis - i).ToString()));
            }

            ddlOpenDateMonth.Items.Clear();
            ddlOpenDateMonthend.Items.Clear();

            ddlOpenDateMonth.DataSource = GetMonth();
            ddlOpenDateMonth.DataBind();
            ddlOpenDateMonthend.DataSource = GetMonth();
            ddlOpenDateMonthend.DataBind();

            ddlOpenDateMonthDTM.DataSource = GetMonth();
            ddlOpenDateMonthDTM.DataBind();
            ddlOpenDateMonthDTMend.DataSource = GetMonth();
            ddlOpenDateMonthDTMend.DataBind();

        }


        private void LoadRegian()
        {
            //ddlRegion.DataSource = conn.ExcuteSQL(" SELECT DISTINCT B.REGION_CD as REGION_CD,convert(varchar,convert(integer,B.REGION_CD))+'-'+A.REGION_NAME AS REGION_NAME " +
     //" FROM [ST_GSBREGION] a, [ST_BRANCH] b, [LN_APP] c where A.REGION_CD=B.REGION_CD and c.[BRANCH_CD]=b.[BRANCH_CD]");
            //Tai 2013-12-26
            string sql = "";
            sql = " SELECT DISTINCT B.REGION_CD as REGION_CD,convert(varchar,convert(integer,B.REGION_CD))+'-'+A.REGION_NAME AS REGION_NAME " +
                  " FROM [ST_GSBREGION] a, [ST_BRANCH] b " +
                  " WHERE A.REGION_CD=B.REGION_CD  and REGION_NAME is not null";
            ddlRegion.DataSource = conn.ExcuteSQL(sql);

           ddlRegion.DataTextField = "REGION_NAME";
           ddlRegion.DataValueField = "REGION_CD";
           ddlRegion.DataBind();
           ListItem item = new ListItem("โปรดเลือก ภาค...", "null");
            ddlRegion.Items.Insert(0,item);
            ddlRegion.SelectedValue = "null";

           sql = " SELECT DISTINCT B.REGION_CD as REGION_CD,convert(varchar,convert(integer,B.REGION_CD))+'-'+A.REGION_NAME AS REGION_NAME " +
                 " FROM [ST_GSBREGION] a, [ST_BRANCH] b " +
                 " WHERE A.REGION_CD=B.REGION_CD  and REGION_NAME is not null";
           ddlRegionTo.DataSource = conn.ExcuteSQL(sql);
           //           
           ddlRegionTo.DataTextField = "REGION_NAME";
           ddlRegionTo.DataValueField = "REGION_CD";
           ddlRegionTo.DataBind();
           
           ddlRegionTo.Items.Insert(0,item);
           ddlRegionTo.SelectedValue = "null";

           //conn.CloseConnection();

        }

        //protected void loadZone()
        //{
        //    string sql = "";
        //    //sql = " select Z.[ZONE_CD] as [ZONE_CD],convert(varchar,convert(integer,Z.[ZONE_CD]))+'-'+Z.[ZONE_NAME] as [ZONE_NAME] ";
        //    //sql += " from ( SELECT distinct(b.[ZONE_CD]) as ZONE_CD,b.[ZONE] as  [ZONE_Name] FROM [LN_APP] a,[ST_BRANCH] b,[ST_GSBREGION] c  ";
        //    //sql += " where a.[BRANCH_CD]=b.[BRANCH_CD]  and b.REGION_CD=c.REGION_CD and  b.REGION_CD='" + ddlRegion.SelectedValue + "') Z order by convert(integer,Z.[ZONE_CD]) ";
        //    //Tai 2013-12-26
        //    sql = " select Z.[ZONE_CD] as [ZONE_CD],convert(varchar,convert(integer,Z.[ZONE_CD]))+'-'+Z.[ZONE_NAME] as [ZONE_NAME] ";
        //    sql += " from ( SELECT distinct(b.[ZONE_CD]) as ZONE_CD,b.[ZONE] as  [ZONE_Name] FROM [ST_BRANCH] b,[ST_GSBREGION] c  ";
        //    sql += " where b.REGION_CD=c.REGION_CD and  b.REGION_CD='" + ddlRegion.SelectedValue + "') Z order by convert(integer,Z.[ZONE_CD]) ";
        //    //            
        //    ddlSubZone.Enabled = true;
        //    ddlSubZone.DataSource = conn.ExcuteSQL(sql);
        //    ddlSubZone.DataTextField = "ZONE_NAME";
        //    ddlSubZone.DataValueField = "ZONE_CD";
        //    ddlSubZone.DataBind();
        //    ListItem item2 = new ListItem("โปรดเลือก เขต...", "null");
        //    ddlSubZone.Items.Add(item2);
        //    ddlSubZone.SelectedValue = "null";

        //}
        // เมื่อ คลิก Zone 

        protected void loadZone()
        {
            try
            {
                conn = new SQLToDataTable();
                if (ddlRegion.SelectedValue == ddlRegionTo.SelectedValue)//chan edit
                {
                    string sql = "";
                    //sql = " select Z.[ZONE_CD] as [ZONE_CD],convert(varchar,convert(integer,Z.[ZONE_CD]))+'-'+Z.[ZONE_NAME] as [ZONE_NAME] ";
                    //sql += " from ( SELECT distinct(b.[ZONE_CD]) as ZONE_CD,b.[ZONE] as  [ZONE_Name] FROM [LN_APP] a,[ST_BRANCH] b,[ST_GSBREGION] c  ";
                    //sql += " where a.[BRANCH_CD]=b.[BRANCH_CD]  and b.REGION_CD=c.REGION_CD and  b.REGION_CD='" + ddlRegion.SelectedValue + "') Z order by convert(integer,Z.[ZONE_CD]) ";
                    //Tai 2013-12-26
                    sql = " select Z.[ZONE_CD] as [ZONE_CD],convert(varchar,convert(integer,Z.[ZONE_CD]))+'-'+Z.[ZONE_NAME] as [ZONE_NAME] ";
                    sql += " from ( SELECT distinct(b.[ZONE_CD]) as ZONE_CD,b.[ZONE] as  [ZONE_Name] FROM [ST_BRANCH] b,[ST_GSBREGION] c  ";
                    sql += " where b.REGION_CD=c.REGION_CD and  b.REGION_CD='" + ddlRegion.SelectedValue + "') Z order by convert(integer,Z.[ZONE_CD]) ";
                    //
                    ddlSubZone.Enabled = true;
                    ddlSubZone.DataSource = conn.ExcuteSQL(sql);
                    ddlSubZone.DataTextField = "ZONE_NAME";
                    ddlSubZone.DataValueField = "ZONE_CD";
                    ddlSubZone.DataBind();
                    ListItem item2 = new ListItem("โปรดเลือก เขต...", "null");
                    ddlSubZone.Items.Insert(0, item2);
                    //ddlSubZone.Items.Add(item2);
                    ddlSubZone.SelectedValue = "null";

                    ddlSubZoneTo.Enabled = true;
                    ddlSubZoneTo.DataSource = conn.ExcuteSQL(sql);
                    ddlSubZoneTo.DataTextField = "ZONE_NAME";
                    ddlSubZoneTo.DataValueField = "ZONE_CD";
                    ddlSubZoneTo.DataBind();

                    ddlSubZoneTo.Items.Insert(0, item2);
                    //ddlSubZoneTo.Items.Add(item2);
                    ddlSubZoneTo.SelectedValue = "null";

                    //refreshZone();                
                }
                else
                {
                    string sql = "";
                    sql = " select Z.[ZONE_CD] as [ZONE_CD],convert(varchar,convert(integer,Z.[ZONE_CD]))+'-'+Z.[ZONE_NAME] as [ZONE_NAME] ";
                    sql += " from ( SELECT distinct(b.[ZONE_CD]) as ZONE_CD,b.[ZONE] as  [ZONE_Name] FROM [ST_BRANCH] b,[ST_GSBREGION] c  ";
                    sql += " where b.REGION_CD=c.REGION_CD and  b.REGION_CD between '" + ddlRegion.SelectedValue + "' and '" + ddlRegionTo.SelectedValue + "') Z order by convert(integer,Z.[ZONE_CD]) ";

                    ddlSubZone.DataSource = conn.ExcuteSQL(sql);
                    ddlSubZone.DataTextField = "ZONE_NAME";
                    ddlSubZone.DataValueField = "ZONE_CD";
                    ddlSubZone.DataBind();
                

                    ddlSubZoneTo.DataSource = conn.ExcuteSQL(sql);
                    ddlSubZoneTo.DataTextField = "ZONE_NAME";
                    ddlSubZoneTo.DataValueField = "ZONE_CD";
                    ddlSubZoneTo.DataBind();

                    ddlSubZone.SelectedIndex = 0;
                    ddlSubZoneTo.SelectedIndex = ddlSubZoneTo.Items.Count - 1;

                    //ddlSubZone.Enabled = false;
                    //ddlSubZoneTo.Enabled = false;

                    //refreshZone();

                    //ddlSubBranch.Enabled = false;
                    //ddlSubBranchTo.Enabled = false;
                }
                //conn.CloseConnection();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                //conn.CloseConnection();
            }
        }


        protected void refreshZone()
        {
            try
            {
                string sql = "";
                if (ddlRegion.SelectedValue == "null")
                {
                    //sql = "select Z.[BRANCH_CD] as [BRANCH_CD],convert(varchar,convert(integer,Z.[BRANCH_CD]))+'-'+Z.[BRANCH_Name] as [BRANCH_NAME] ";
                    //sql += " from ( SELECT distinct(a.[BRANCH_CD]) as BRANCH_CD,b.[BRANCH_NAME] as  [BRANCH_Name] FROM [LN_APP] a,[ST_BRANCH] b,[ST_GSBREGION] c ";
                    //sql += " where a.[BRANCH_CD]=b.[BRANCH_CD]  and b.REGION_CD=c.REGION_CD) Z order by convert(integer,Z.[BRANCH_CD])";
                    //Tai 2013-12-26
                    sql = "select Z.[BRANCH_CD] as [BRANCH_CD],convert(varchar,convert(integer,Z.[BRANCH_CD]))+'-'+Z.[BRANCH_Name] as [BRANCH_NAME] ";
                    sql += " from ( SELECT distinct(b.[BRANCH_CD]) as BRANCH_CD,b.[BRANCH_NAME] as  [BRANCH_Name] FROM [ST_BRANCH] b,[ST_GSBREGION] c ";
                    sql += " where b.REGION_CD=c.REGION_CD) Z order by convert(integer,Z.[BRANCH_CD])";
                    //
                    ddlSubBranch.Enabled = true;
                    ddlSubBranchTo.Enabled = true;

                    ddlSubBranch.DataSource = conn.ExcuteSQL(sql);
                    ddlSubBranch.DataTextField = "BRANCH_NAME";
                    ddlSubBranch.DataValueField = "BRANCH_CD";
                    ddlSubBranch.DataBind();

                    ddlSubBranchTo.DataSource = conn.ExcuteSQL(sql);
                    ddlSubBranchTo.DataTextField = "BRANCH_NAME";
                    ddlSubBranchTo.DataValueField = "BRANCH_CD";
                    ddlSubBranchTo.DataBind();
                }
                else if (ddlRegion.SelectedValue == ddlRegionTo.SelectedValue & ddlSubZone.SelectedValue != ddlSubZoneTo.SelectedValue) //chan edit
                {
                    sql = "select Z.[BRANCH_CD] as [BRANCH_CD],convert(varchar,convert(integer,Z.[BRANCH_CD]))+'-'+Z.[BRANCH_Name] as [BRANCH_NAME] ";
                    sql += " from ( SELECT distinct(b.[BRANCH_CD]) as BRANCH_CD,b.[BRANCH_NAME] as  [BRANCH_Name] FROM [ST_BRANCH] b,[ST_GSBREGION] c ";
                    sql += " where b.REGION_CD=c.REGION_CD and c.REGION_CD='" + ddlRegion.SelectedValue + "' and b.ZONE_CD between '" + ddlSubZone.SelectedValue + "' and '" + ddlSubZoneTo.SelectedValue + "'  ) Z order by convert(integer,Z.[BRANCH_CD])";

                    ddlSubBranch.Enabled = true;
                    ddlSubBranchTo.Enabled = true;

                    ddlSubBranch.DataSource = conn.ExcuteSQL(sql);
                    ddlSubBranch.DataTextField = "BRANCH_NAME";
                    ddlSubBranch.DataValueField = "BRANCH_CD";
                    ddlSubBranch.DataBind();

                    ddlSubBranchTo.DataSource = conn.ExcuteSQL(sql);
                    ddlSubBranchTo.DataTextField = "BRANCH_NAME";
                    ddlSubBranchTo.DataValueField = "BRANCH_CD";
                    ddlSubBranchTo.DataBind();

                    ListItem item = new ListItem("โปรดเลือก...", "null", true);

                    ddlSubBranch.Items.Insert(0, item);
                    //ddlSubBranch.Items.Add(item);
                    ddlSubBranch.SelectedIndex = 0;

                    ddlSubBranchTo.Items.Insert(0, item);
                    //ddlSubBranchTo.Items.Add(item);
                    ddlSubBranchTo.SelectedIndex = 0;

                }
                else if (ddlRegion.SelectedValue != ddlRegionTo.SelectedValue)//chan edit
                {
                        sql = "select Z.[BRANCH_CD] as [BRANCH_CD],convert(varchar,convert(integer,Z.[BRANCH_CD]))+'-'+Z.[BRANCH_Name] as [BRANCH_NAME] ";
                        sql += " from ( SELECT distinct(b.[BRANCH_CD]) as BRANCH_CD,b.[BRANCH_NAME] as  [BRANCH_Name] FROM [ST_BRANCH] b,[ST_GSBREGION] c ";
                        sql += " where b.REGION_CD=c.REGION_CD and c.REGION_CD between '" + ddlRegion.SelectedValue + "' and '" + ddlRegionTo.SelectedValue + "' and b.ZONE_CD between '" + ddlSubZone.SelectedValue + "' and '" + ddlSubZoneTo.SelectedValue + "'  ) Z order by convert(integer,Z.[BRANCH_CD])";

                        ddlSubBranch.Enabled = true;
                        ddlSubBranchTo.Enabled = true;

                        ddlSubBranch.DataSource = conn.ExcuteSQL(sql);
                        ddlSubBranch.DataTextField = "BRANCH_NAME";
                        ddlSubBranch.DataValueField = "BRANCH_CD";
                        ddlSubBranch.DataBind();

                        ddlSubBranchTo.DataSource = conn.ExcuteSQL(sql);
                        ddlSubBranchTo.DataTextField = "BRANCH_NAME";
                        ddlSubBranchTo.DataValueField = "BRANCH_CD";
                        ddlSubBranchTo.DataBind();

                        ddlSubBranch.SelectedIndex = 0;

                        //ddlSubBranchTo.Items.Add(item);
                        ddlSubBranchTo.SelectedIndex = ddlSubBranchTo.Items.Count - 1;

                }
                else
                {
                    //sql = "select Z.[BRANCH_CD] as [BRANCH_CD],convert(varchar,convert(integer,Z.[BRANCH_CD]))+'-'+Z.[BRANCH_Name] as [BRANCH_NAME] ";
                    //sql += " from ( SELECT distinct(a.[BRANCH_CD]) as BRANCH_CD,b.[BRANCH_NAME] as  [BRANCH_Name] FROM [LN_APP] a,[ST_BRANCH] b,[ST_GSBREGION] c ";
                    //sql += " where a.[BRANCH_CD]=b.[BRANCH_CD]  and b.REGION_CD=c.REGION_CD and c.REGION_CD='" + ddlRegion.SelectedValue + "' and b.ZONE_CD='" + ddlSubZone.SelectedValue + "'  ) Z order by convert(integer,Z.[BRANCH_CD])";
                    //Tai 2013-12-26
                    sql = "select Z.[BRANCH_CD] as [BRANCH_CD],convert(varchar,convert(integer,Z.[BRANCH_CD]))+'-'+Z.[BRANCH_Name] as [BRANCH_NAME] ";
                    sql += " from ( SELECT distinct(b.[BRANCH_CD]) as BRANCH_CD,b.[BRANCH_NAME] as  [BRANCH_Name] FROM [ST_BRANCH] b,[ST_GSBREGION] c ";
                    sql += " where b.REGION_CD=c.REGION_CD and c.REGION_CD='" + ddlRegion.SelectedValue + "' and b.ZONE_CD='" + ddlSubZone.SelectedValue + "'  ) Z order by convert(integer,Z.[BRANCH_CD])";
                    //
                    ddlSubBranch.Enabled = true;
                    ddlSubBranchTo.Enabled = true;

                    ddlSubBranch.DataSource = conn.ExcuteSQL(sql);
                    ddlSubBranch.DataTextField = "BRANCH_NAME";
                    ddlSubBranch.DataValueField = "BRANCH_CD";
                    ddlSubBranch.DataBind();

                    ddlSubBranchTo.DataSource = conn.ExcuteSQL(sql);
                    ddlSubBranchTo.DataTextField = "BRANCH_NAME";
                    ddlSubBranchTo.DataValueField = "BRANCH_CD";
                    ddlSubBranchTo.DataBind();

                    ListItem item = new ListItem("โปรดเลือก สาขา...", "null", true);

                    ddlSubBranch.Items.Insert(0, item);
                    //ddlSubBranch.Items.Add(item);
                    ddlSubBranch.SelectedValue = "null";


                    ddlSubBranchTo.Items.Insert(0, item);
                    //ddlSubBranchTo.Items.Add(item);
                    ddlSubBranchTo.SelectedValue = "null";

                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                //conn.CloseConnection();
            }
        }
        // เมื่อ คลิก Zone 

        //protected void loadZoneList(object sender, EventArgs e)
        //{
        //    string sql = "";
        //    if (ddlRegion.SelectedValue == "null")
        //    {
        //        //sql = "select Z.[BRANCH_CD] as [BRANCH_CD],convert(varchar,convert(integer,Z.[BRANCH_CD]))+'-'+Z.[BRANCH_Name] as [BRANCH_NAME] ";
        //        //sql += " from ( SELECT distinct(a.[BRANCH_CD]) as BRANCH_CD,b.[BRANCH_NAME] as  [BRANCH_Name] FROM [LN_APP] a,[ST_BRANCH] b,[ST_GSBREGION] c ";
        //        //sql += " where a.[BRANCH_CD]=b.[BRANCH_CD]  and b.REGION_CD=c.REGION_CD) Z order by convert(integer,Z.[BRANCH_CD])";
        //        //Tai 2013-12-26
        //        sql = "select Z.[BRANCH_CD] as [BRANCH_CD],convert(varchar,convert(integer,Z.[BRANCH_CD]))+'-'+Z.[BRANCH_Name] as [BRANCH_NAME] ";
        //        sql += " from ( SELECT distinct(b.[BRANCH_CD]) as BRANCH_CD,b.[BRANCH_NAME] as  [BRANCH_Name] FROM [ST_BRANCH] b,[ST_GSBREGION] c ";
        //        sql += " where b.REGION_CD=c.REGION_CD) Z order by convert(integer,Z.[BRANCH_CD])";
        //        //
        //        ddlSubBranch.Enabled = true;
        //        ddlSubBranchTo.Enabled = true;
        //    }
        //    else
        //    {
        //        //sql = "select Z.[BRANCH_CD] as [BRANCH_CD],convert(varchar,convert(integer,Z.[BRANCH_CD]))+'-'+Z.[BRANCH_Name] as [BRANCH_NAME] ";
        //        //sql += " from ( SELECT distinct(a.[BRANCH_CD]) as BRANCH_CD,b.[BRANCH_NAME] as  [BRANCH_Name] FROM [LN_APP] a,[ST_BRANCH] b,[ST_GSBREGION] c ";
        //        //sql += " where a.[BRANCH_CD]=b.[BRANCH_CD]  and b.REGION_CD=c.REGION_CD and c.REGION_CD='" + ddlRegion.SelectedValue + "' and b.ZONE_CD='"+ddlSubZone.SelectedValue+"'  ) Z order by convert(integer,Z.[BRANCH_CD])";
        //        //Tai 2013-12-26
        //        sql = "select Z.[BRANCH_CD] as [BRANCH_CD],convert(varchar,convert(integer,Z.[BRANCH_CD]))+'-'+Z.[BRANCH_Name] as [BRANCH_NAME] ";
        //        sql += " from ( SELECT distinct(b.[BRANCH_CD]) as BRANCH_CD,b.[BRANCH_NAME] as  [BRANCH_Name] FROM [ST_BRANCH] b,[ST_GSBREGION] c ";
        //        sql += " where b.REGION_CD=c.REGION_CD and c.REGION_CD='" + ddlRegion.SelectedValue + "' and b.ZONE_CD='" + ddlSubZone.SelectedValue + "'  ) Z order by convert(integer,Z.[BRANCH_CD])";
        //        //
        //        ddlSubBranch.Enabled = true;
        //        ddlSubBranchTo.Enabled = true;
        //    }
        //    ddlSubBranch.DataSource = conn.ExcuteSQL(sql);
        //    ddlSubBranch.DataTextField = "BRANCH_NAME";
        //    ddlSubBranch.DataValueField = "BRANCH_CD";
        //    ddlSubBranch.DataBind();

        //    ddlSubBranchTo.DataSource = conn.ExcuteSQL(sql);
        //    ddlSubBranchTo.DataTextField = "BRANCH_NAME";
        //    ddlSubBranchTo.DataValueField = "BRANCH_CD";
        //    ddlSubBranchTo.DataBind();
        //}


        protected void loadZoneList(object sender, EventArgs e)
        {
            if (ddlRegion.SelectedValue != "null" && ddlRegionTo.SelectedValue != "null")
            {
                string sql = "";
                sql = " select Z.[ZONE_CD] as [ZONE_CD],convert(varchar,convert(integer,Z.[ZONE_CD]))+'-'+Z.[ZONE_NAME] as [ZONE_NAME] ";
                sql += " from ( SELECT distinct(b.[ZONE_CD]) as ZONE_CD,b.[ZONE] as  [ZONE_Name] FROM [ST_BRANCH] b,[ST_GSBREGION] c  ";
                sql += " where b.REGION_CD=c.REGION_CD and  b.REGION_CD between '" + ddlRegion.SelectedValue + "' and '" + ddlRegionTo.SelectedValue + "') Z order by convert(integer,Z.[ZONE_CD]) ";

                ddlSubZone.DataSource = conn.ExcuteSQL(sql);
                ddlSubZone.DataTextField = "ZONE_NAME";
                ddlSubZone.DataValueField = "ZONE_CD";
                ddlSubZone.DataBind();


                ddlSubZoneTo.DataSource = conn.ExcuteSQL(sql);
                ddlSubZoneTo.DataTextField = "ZONE_NAME";
                ddlSubZoneTo.DataValueField = "ZONE_CD";
                ddlSubZoneTo.DataBind();
            }


            ListItem item2 = new ListItem("โปรดเลือก เขต...", "null");
            ddlSubZone.Items.Insert(0, item2);
            ddlSubZone.SelectedValue = "null";

            ddlSubZoneTo.Items.Insert(0, item2);
            ddlSubZoneTo.SelectedValue = "null";

            ListItem item = new ListItem("โปรดเลือก สาขา...", "null", true);

            ddlSubBranch.Items.Insert(0, item);
            ddlSubBranch.SelectedValue = "null";


            ddlSubBranchTo.Items.Insert(0, item);
            ddlSubBranchTo.SelectedValue = "null";

            //if (ddlSubZoneTo.SelectedValue != "null")
            //{
            //    if (ddlSubZone.SelectedIndex > ddlSubZoneTo.SelectedIndex)
            //    {
            //        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "abde", "alert('ค่าของรหัสเขตเริ่มต้น ต้องน้อยกว่า รหัสเขตปลายทาง');", true);
            //        return;
            //    }
            //    refreshZone();
            //}

            //if (ddlSubZoneTo.SelectedValue != "null")
            //{
            //    string sql = "";
            //    if (ddlRegion.SelectedValue == "null")
            //    {
            //        //sql = "select Z.[BRANCH_CD] as [BRANCH_CD],convert(varchar,convert(integer,Z.[BRANCH_CD]))+'-'+Z.[BRANCH_Name] as [BRANCH_NAME] ";
            //        //sql += " from ( SELECT distinct(a.[BRANCH_CD]) as BRANCH_CD,b.[BRANCH_NAME] as  [BRANCH_Name] FROM [LN_APP] a,[ST_BRANCH] b,[ST_GSBREGION] c ";
            //        //sql += " where a.[BRANCH_CD]=b.[BRANCH_CD]  and b.REGION_CD=c.REGION_CD) Z order by convert(integer,Z.[BRANCH_CD])";
            //        //Tai 2013-12-26
            //        sql = "select Z.[BRANCH_CD] as [BRANCH_CD],convert(varchar,convert(integer,Z.[BRANCH_CD]))+'-'+Z.[BRANCH_Name] as [BRANCH_NAME] ";
            //        sql += " from ( SELECT distinct(b.[BRANCH_CD]) as BRANCH_CD,b.[BRANCH_NAME] as  [BRANCH_Name] FROM [ST_BRANCH] b,[ST_GSBREGION] c ";
            //        sql += " where b.REGION_CD=c.REGION_CD) Z order by convert(integer,Z.[BRANCH_CD])";
            //        //
            //        ddlSubBranch.Enabled = true;
            //        ddlSubBranchTo.Enabled = true;
            //    }
            //    else if (ddlRegion.SelectedValue == ddlRegionTo.SelectedValue & ddlSubZone.SelectedValue != ddlSubZoneTo.SelectedValue) //chan edit
            //    {
            //        sql = "select Z.[BRANCH_CD] as [BRANCH_CD],convert(varchar,convert(integer,Z.[BRANCH_CD]))+'-'+Z.[BRANCH_Name] as [BRANCH_NAME] ";
            //        sql += " from ( SELECT distinct(b.[BRANCH_CD]) as BRANCH_CD,b.[BRANCH_NAME] as  [BRANCH_Name] FROM [ST_BRANCH] b,[ST_GSBREGION] c ";
            //        sql += " where b.REGION_CD=c.REGION_CD and c.REGION_CD='" + ddlRegion.SelectedValue + "' and b.ZONE_CD between '" + ddlSubZone.SelectedValue + "' and '" + ddlSubZoneTo.SelectedValue + "'  ) Z order by convert(integer,Z.[BRANCH_CD])";
            //        ddlSubBranch.Enabled = false;
            //        ddlSubBranchTo.Enabled = false;
            //    }
            //    else if (ddlRegion.SelectedValue != ddlRegionTo.SelectedValue)//chan edit
            //    {
            //        sql = "select Z.[BRANCH_CD] as [BRANCH_CD],convert(varchar,convert(integer,Z.[BRANCH_CD]))+'-'+Z.[BRANCH_Name] as [BRANCH_NAME] ";
            //        sql += " from ( SELECT distinct(b.[BRANCH_CD]) as BRANCH_CD,b.[BRANCH_NAME] as  [BRANCH_Name] FROM [ST_BRANCH] b,[ST_GSBREGION] c ";
            //        sql += " where b.REGION_CD=c.REGION_CD and c.REGION_CD between '" + ddlRegion.SelectedValue + "' and '" + ddlRegionTo.SelectedValue + "') Z order by convert(integer,Z.[BRANCH_CD])";
            //        ddlSubBranch.Enabled = false;
            //        ddlSubBranchTo.Enabled = false;
            //    }
            //    else
            //    {
            //        //sql = "select Z.[BRANCH_CD] as [BRANCH_CD],convert(varchar,convert(integer,Z.[BRANCH_CD]))+'-'+Z.[BRANCH_Name] as [BRANCH_NAME] ";
            //        //sql += " from ( SELECT distinct(a.[BRANCH_CD]) as BRANCH_CD,b.[BRANCH_NAME] as  [BRANCH_Name] FROM [LN_APP] a,[ST_BRANCH] b,[ST_GSBREGION] c ";
            //        //sql += " where a.[BRANCH_CD]=b.[BRANCH_CD]  and b.REGION_CD=c.REGION_CD and c.REGION_CD='" + ddlRegion.SelectedValue + "' and b.ZONE_CD='" + ddlSubZone.SelectedValue + "'  ) Z order by convert(integer,Z.[BRANCH_CD])";
            //        //Tai 2013-12-26
            //        sql = "select Z.[BRANCH_CD] as [BRANCH_CD],convert(varchar,convert(integer,Z.[BRANCH_CD]))+'-'+Z.[BRANCH_Name] as [BRANCH_NAME] ";
            //        sql += " from ( SELECT distinct(b.[BRANCH_CD]) as BRANCH_CD,b.[BRANCH_NAME] as  [BRANCH_Name] FROM [ST_BRANCH] b,[ST_GSBREGION] c ";
            //        sql += " where b.REGION_CD=c.REGION_CD and c.REGION_CD='" + ddlRegion.SelectedValue + "' and b.ZONE_CD='" + ddlSubZone.SelectedValue + "'  ) Z order by convert(integer,Z.[BRANCH_CD])";
            //        //
            //        ddlSubBranch.Enabled = true;
            //        ddlSubBranchTo.Enabled = true;
            //    }
            //    ddlSubBranch.DataSource = conn.ExcuteSQL(sql);
            //    ddlSubBranch.DataTextField = "BRANCH_NAME";
            //    ddlSubBranch.DataValueField = "BRANCH_CD";
            //    ddlSubBranch.DataBind();

            //    ddlSubBranchTo.DataSource = conn.ExcuteSQL(sql);
            //    ddlSubBranchTo.DataTextField = "BRANCH_NAME";
            //    ddlSubBranchTo.DataValueField = "BRANCH_CD";
            //    ddlSubBranchTo.DataBind();
            //}

        }

        // เมื่อ คลิก ปุ่ม Region
        protected void LoadBranchList(object sender, EventArgs e)
        {
            if (ddlRegion.SelectedValue != "null" && ddlRegionTo.SelectedValue != "null" && ddlSubZone.SelectedValue != "null" && ddlSubZoneTo.SelectedValue != "null")
            {
                string sql = "";
                sql = "select Z.[BRANCH_CD] as [BRANCH_CD],convert(varchar,convert(integer,Z.[BRANCH_CD]))+'-'+Z.[BRANCH_Name] as [BRANCH_NAME] ";
                sql += " from ( SELECT distinct(b.[BRANCH_CD]) as BRANCH_CD,b.[BRANCH_NAME] as  [BRANCH_Name] FROM [ST_BRANCH] b,[ST_GSBREGION] c ";
                sql += " where b.REGION_CD=c.REGION_CD and c.REGION_CD between '" + ddlRegion.SelectedValue + "' and '" + ddlRegionTo.SelectedValue + "' and b.ZONE_CD between '" + ddlSubZone.SelectedValue + "' and '" + ddlSubZoneTo.SelectedValue + "' and  b.[BRANCH_NAME] is not null )  Z order by convert(integer,Z.[BRANCH_CD])";

                ddlSubBranch.Enabled = true;
                ddlSubBranchTo.Enabled = true;

                ddlSubBranch.DataSource = conn.ExcuteSQL(sql);
                ddlSubBranch.DataTextField = "BRANCH_NAME";
                ddlSubBranch.DataValueField = "BRANCH_CD";
                ddlSubBranch.DataBind();

                ddlSubBranchTo.DataSource = conn.ExcuteSQL(sql);
                ddlSubBranchTo.DataTextField = "BRANCH_NAME";
                ddlSubBranchTo.DataValueField = "BRANCH_CD";
                ddlSubBranchTo.DataBind();

            }


            ListItem item = new ListItem("โปรดเลือก สาขา...", "null", true);

            ddlSubBranch.Items.Insert(0, item);
            ddlSubBranch.SelectedValue = "null";


            ddlSubBranchTo.Items.Insert(0, item);
            ddlSubBranchTo.SelectedValue = "null";




            //if (ddlRegionTo.SelectedValue != "null")
            //{
            //    if (ddlRegion.SelectedIndex > ddlRegionTo.SelectedIndex)
            //    {
            //        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "abde", "alert('ค่าของรหัสภาคเริ่มต้น ต้องน้อยกว่า รหัสภาคปลายทาง');", true);
            //        return;
            //    }
            //    loadZone();
            //}




        }

        #endregion Page_Load

        #region Private Method

        //private void LoadModelVersion()
        //{

        //    ListItem item2 = new ListItem(" Version1 ", "null");
        //    ddlModelVersion.Items.Add(item2);
        //    ddlModelVersion.SelectedValue = "null";
        //    //ddlModelVersion.DataBind();
        //}

        private void LoadModel()
        {
            try
            {
                string Query = string.Format("select distinct " +
                                "CONVERT(int,MODEL_CD) [MODEL_CD],  " +
                                "MODEL_CD +' - '+ MODEL_NAME [MODEL_NAME] from LN_MODEL where ACTIVE_FLAG = '1' and MODEL_CD not in ('005','006') " +
                                "union select null, 'โปรดเลือก...' " +
                                "order by MODEL_CD ASC");

                ddlModel.DataSource = conn.ExcuteSQL(Query);
                ddlModel.DataBind();
                ddlSubType.Items.Insert(0, new ListItem("โปรดเลือก...", string.Empty));
                ddlMarketFrm.Items.Insert(0, new ListItem("โปรดเลือก...", string.Empty));
                ddlMarketTo.Items.Insert(0, new ListItem("โปรดเลือก...", string.Empty));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                //conn.CloseConnection();
            }
        }


        protected void LoadLoan(object sender, EventArgs e)
        {
            string Query = "";

            if (ddlModel.SelectedItem.Value == "7")
            {
                Query = string.Format("select distinct " +
"a.LOAN_CD [LOAN_CD],  " +
"a.LOAN_CD+' - '+a.LOAN_NAME [LOAN_NAME] from ST_LOANTYPE a left join [dbo].[ST_MKTDFTMYMO] b on a.LOAN_CD = b.loantype where ACTIVE_FLAG = '1' and b.loantype is not null " +
"union select null, 'โปรดเลือก...' " +
"order by LOAN_CD ASC");
            }
            else
            {
                Query = string.Format("select distinct " +
"a.LOAN_CD [LOAN_CD],  " +
"a.LOAN_CD+' - '+a.LOAN_NAME [LOAN_NAME] from ST_LOANTYPE a left join [dbo].[ST_MKTDFTMYMO] b on a.LOAN_CD = b.loantype where ACTIVE_FLAG = '1' and b.loantype is null " +
"union select null, 'โปรดเลือก...' " +
"order by LOAN_CD ASC");
            }

            ddlLoan.DataSource = conn.ExcuteSQL(Query);
            ddlLoan.DataBind();
            ddlLoan.Enabled = true;
            ddlSubType.Items.Insert(0, new ListItem("เลือกทั้งหมด...", string.Empty));
            LoadBranch();
        }

        private void DataTableToExcel()
        {
            DataTable dttb = (DataTable)ViewState["DataTable1"];

            string filename = string.Format("GSB-CS_PortfolioScorePerformanceReport_(Date_{0}-Time_{1}).xls", DateTime.Now.ToString("dd-MM-yyyy"), DateTime.Now.ToString("HH-mm-ss"));
            StringWriter tw = new StringWriter();
            DataGrid dgGrid = new DataGrid();

            tw.Write(string.Format("Portfolio Score Performance Report"));
            tw.Write(tw.NewLine);
            tw.Write(string.Format("ประเภทโมเดล :| {0} ", ddlModel.SelectedItem.Text.ToString()));
            tw.Write(tw.NewLine);
            //tw.Write(string.Format("Model Version :| {0} ", ddlModelVersion.SelectedItem.Value));
            //tw.Write(tw.NewLine);
            tw.Write(string.Format("ประเภทสินเชื่อ :| {0} ", ddlLoan.SelectedItem.Text.ToString()));
            tw.Write(tw.NewLine);
            tw.Write(string.Format("ประเภทสินเชื่อย่อย :| {0} ", ddlSubType.SelectedItem.Text.ToString()));
            tw.Write(tw.NewLine);
            tw.Write(string.Format("Market Code :| {0} ถึง {1} ", ddlMarketFrm.SelectedItem.Text.ToString(), ddlMarketTo.SelectedItem.Text.ToString()));
            tw.Write(tw.NewLine);
            tw.Write(string.Format("ภาค :| {0} ถึง {1} ", ddlRegion.SelectedItem.Text,ddlRegionTo.SelectedItem.Text));
            tw.Write(tw.NewLine);
            tw.Write(string.Format("เขต :| {0} ถึง {1} ", ddlSubZone.SelectedItem.Text,ddlSubZoneTo.SelectedItem.Text));
            tw.Write(tw.NewLine);
            tw.Write(string.Format("สาขา :| {0} ถึง {1} ", ddlSubBranch.SelectedItem.Text,ddlSubBranchTo.SelectedItem.Text));
            tw.Write(tw.NewLine);
            tw.Write(string.Format("As of Date :| {0} ", ""));
            tw.Write(tw.NewLine);
            tw.Write(string.Format("Export Date :| {0} : {1} ", DateTime.Now.ToString("dd-MM-yyyy"), DateTime.Now.ToString("HH-mm-ss")));
            tw.Write(tw.NewLine);
            tw.Write("ผู้พิมพ์ " + ExportUtility.getCurrentUserId);
            tw.Write(tw.NewLine);

            int iColCount = new int();
            iColCount = dttb.Columns.Count;

            tw.Write("|Outstanding Balance||Non-Performing Loan");
            tw.Write(tw.NewLine);
            tw.Write("Score Range|No. of Account|Amount|No. of Account|%NPLs|Amount (NPLs)|%Amount (NPLs)|% of Cumulative Bad");
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
                    else
                    {
                        tw.Write("0.00");
                    }

                    if (i < iColCount - 1)
                    {
                        tw.Write("|");
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
        
        protected void LoadBranch()
        {
            try
            {
                conn = new SQLToDataTable();

                //string Query = "select Z.[BRANCH_CD] as [BRANCH_CD],convert(varchar,convert(integer,Z.[BRANCH_CD]))+'-'+Z.[BRANCH_Name] as [BRANCH_NAME] ";
                //Query += " from ( SELECT distinct(a.[BRANCH_CD]) as BRANCH_CD,b.[BRANCH_NAME] as  [BRANCH_Name] FROM [LN_APP] a,[ST_BRANCH] b,[ST_GSBREGION] c ";
                //Query += " where a.[BRANCH_CD]=b.[BRANCH_CD]  and b.REGION_CD=c.REGION_CD and c.REGION_CD='" + ddlRegion.SelectedValue + "'  ) Z order by convert(integer,Z.[BRANCH_CD])";
                //Tai 2013-12-26
                string Query = "select Z.[BRANCH_CD] as [BRANCH_CD],convert(varchar,convert(integer,Z.[BRANCH_CD]))+'-'+Z.[BRANCH_Name] as [BRANCH_NAME] ";
                Query += " from ( SELECT distinct(b.[BRANCH_CD]) as BRANCH_CD,b.[BRANCH_NAME] as  [BRANCH_Name] FROM [ST_BRANCH] b,[ST_GSBREGION] c ";
                Query += " where b.REGION_CD=c.REGION_CD and c.REGION_CD='" + ddlRegion.SelectedValue + "'  ) Z order by convert(integer,Z.[BRANCH_CD])";
                //

                //if (ddlModel.SelectedItem.Value == "7")
                //{
                //    Query = "select 'MyMo' as [BRANCH_CD],'MyMo' as [BRANCH_NAME]";

                //    ddlRegion.DataSource = conn.ExcuteSQL(Query);
                //    ddlRegionTo.DataSource = conn.ExcuteSQL(Query);

                //    ddlRegion.DataTextField = "BRANCH_NAME";
                //    ddlRegion.DataValueField = "BRANCH_CD";
                //    ddlRegion.DataBind();

                //    ddlRegionTo.DataTextField = "BRANCH_NAME";
                //    ddlRegionTo.DataValueField = "BRANCH_CD";
                //    ddlRegionTo.DataBind();

                //    ddlSubZone.DataSource = conn.ExcuteSQL(Query);
                //    ddlSubZoneTo.DataSource = conn.ExcuteSQL(Query);

                //    ddlSubZone.DataTextField = "BRANCH_NAME";
                //    ddlSubZone.DataValueField = "BRANCH_CD";
                //    ddlSubZone.DataBind();

                //    ddlSubZoneTo.DataTextField = "BRANCH_NAME";
                //    ddlSubZoneTo.DataValueField = "BRANCH_CD";
                //    ddlSubZoneTo.DataBind();
                    
                //}

                ddlSubBranch.DataSource = conn.ExcuteSQL(Query);
                ddlSubBranchTo.DataSource = conn.ExcuteSQL(Query);

                ddlSubBranch.DataTextField = "BRANCH_NAME";
                ddlSubBranch.DataValueField = "BRANCH_CD";
                ddlSubBranch.DataBind();
           
               // ddlSubBranch.Enabled = true;

                //ListItem item = new ListItem("", "null", true);
                //ddlSubBranch.Items.Add(item);
                //ddlSubBranch.SelectedValue = "null";

                ddlSubBranchTo.DataTextField = "BRANCH_NAME";
                ddlSubBranchTo.DataValueField = "BRANCH_CD";
                ddlSubBranchTo.DataBind();
                //ddlSubBranchTo.Enabled = true;

                //ListItem item1 = new ListItem("", "null", true);
                //ddlSubBranchTo.Items.Add(item1);
                //ddlSubBranchTo.SelectedValue = "null";
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                //conn.CloseConnection();
            }
        }

        protected void LoadSubType(object sender, EventArgs e)
        {
            string Query = "";

            if (ddlModel.SelectedItem.Value == "7")
            {
                Query = string.Format("select distinct " +
                                         "STYPE_CD [STYPE_CD], " +
                                         "STYPE_CD+' - '+STYPER_NAME [STYPER_NAME] from ST_LOANSTYPE a left join [dbo].[ST_MKTDFTMYMO] b on a.STYPE_CD = b.subtype where ACTIVE_FLAG = '1' and LOAN_CD = '{0}' and b.subtype is not null " +
                                         "union select null, 'โปรดเลือก...' " +
                                         "order by STYPE_CD ASC", ddlLoan.SelectedItem.Value);
            }
            else
            {
                Query = string.Format("select distinct " +
                         "STYPE_CD [STYPE_CD], " +
                         "STYPE_CD+' - '+STYPER_NAME [STYPER_NAME] from ST_LOANSTYPE a left join [dbo].[ST_MKTDFTMYMO] b on a.STYPE_CD = b.subtype where ACTIVE_FLAG = '1' and LOAN_CD = '{0}' and b.subtype is null " +
                         "union select null, 'โปรดเลือก...' " +
                         "order by STYPE_CD ASC", ddlLoan.SelectedItem.Value);
            }

            ddlSubType.Items.Clear();
            ddlSubType.DataSource = conn.ExcuteSQL(Query);
            ddlSubType.DataBind();
            ddlSubType.Enabled = true;
        }
        //protected void LoadVersion(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        conn = new SQLToDataTable();
        //        string sql = "";
        //        sql = string.Format("select MODEL_NAME+'_V1' as [Model_V_NAME] from LN_MODEL where ACTIVE_FLAG = '1' and");
        //        sql += "  [MODEL_CD]=" + ddlModel.SelectedValue + " ";


        //        ddlModelVersion.DataSource = conn.ExcuteSQL(sql);
        //        ddlModelVersion.DataTextField = "Model_V_NAME";
        //        ddlModelVersion.DataValueField = "Model_V_NAME";
        //        ddlModelVersion.DataBind();

        //        string Query = string.Format("select distinct " + "CONVERT(int,LOAN_CD) [LOAN_CD],  " +
        //    "LOAN_CD+' - '+LOAN_NAME [LOAN_NAME] from ST_LOANTYPE a,ST_SCORERANGE b where ACTIVE_FLAG = '1' and a.loan_cd=b.ltype " +
        //    " and [MODEL_CD]=" + ddlModel.SelectedValue + " " +
        //    "order by LOAN_CD ASC");

        //        ddlLoan.DataSource = conn.ExcuteSQL(Query);
        //        ddlLoan.DataBind();
        //        ddlLoan.Items.Insert(0, new ListItem("โปรดเลือก...", string.Empty));
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        //conn.CloseConnection();
        //    }
        //}
        protected void LoadMarket(object sender, EventArgs e)
        {
            string Query = "";

            if (ddlModel.SelectedItem.Value == "7")
            {
                Query = string.Format("select distinct " +
             "MARKET_CD [MARKET_CD], " +
             "MARKET_CD+' - '+MARKET_NAME [MARKET_NAME] from ST_LOANMKT a left join [dbo].[ST_MKTDFTMYMO] b on MARKET_CD = b.marketcode where ACTIVE_FLAG = '1' and LOAN_CD = '{0}' and STYPE_CD = '{1}' and b.marketcode is not null  " +
             "union select null, '00000 - โปรดเลือก...' " +
             "order by MARKET_CD ASC",
             ddlLoan.SelectedItem.Value, ddlSubType.SelectedItem.Value);
            }
            else
            {
                Query = string.Format("select distinct " +
"MARKET_CD [MARKET_CD], " +
"MARKET_CD+' - '+MARKET_NAME [MARKET_NAME] from ST_LOANMKT a left join [dbo].[ST_MKTDFTMYMO] b on MARKET_CD = b.marketcode where ACTIVE_FLAG = '1' and LOAN_CD = '{0}' and STYPE_CD = '{1}' and b.marketcode is null  " +
"union select null, '00000 - โปรดเลือก...' " +
"order by MARKET_CD ASC",
ddlLoan.SelectedItem.Value, ddlSubType.SelectedItem.Value);
            }

            ddlMarketFrm.Items.Clear();
            ddlMarketFrm.DataSource = conn.ExcuteSQL(Query);
            ddlMarketFrm.DataBind();
            ddlMarketFrm.Enabled = true;
            ddlMarketTo.Items.Clear();
            ddlMarketTo.DataSource = conn.ExcuteSQL(Query);
            ddlMarketTo.DataBind();
            ddlMarketTo.Enabled = true;
        }
        protected void Search_Click(object sender, EventArgs e)
        {
            if (ddlSubBranch.SelectedIndex > ddlSubBranchTo.SelectedIndex)
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "abde", "alert('ค่าของรหัสสาขาเริ่มต้น ต้องน้อยกว่า รหัสสาขาปลายทาง');", true);
                return;
            }
            //string text = "";
            //for (int i = ddlSubBranch.SelectedIndex; i <= ddlSubBranchTo.SelectedIndex; i++)
            //{

            //    if (i != ddlSubBranchTo.SelectedIndex)
            //    {
            //        text += "'" + ddlSubBranch.Items[i].Value + "',";
            //    }
            //    else
            //    {
            //        text += "'" + ddlSubBranch.Items[i].Value + "'";
            //    }
            //}

            IFormatProvider format = new CultureInfo("en-GB");
            DateTime dateOpenLoan = DateTime.MinValue;

            StringBuilder sqlQuery = new StringBuilder();
            string mktqrf = "";
            string mktqrt = "";
            string branch_CD_TO = "";
            string branch_CD = "";
            DateTime start = new DateTime();
            DateTime start12yg = new DateTime();
            DateTime end3m = new DateTime();
            DateTime end12yg = new DateTime();
            DateTime APPDATE_Start = new DateTime();
            DateTime APPDATE_End = new DateTime();

            if (ddlLoan.SelectedIndex <= 0 || ddlSubType.SelectedIndex <= 0 || ddlMarketFrm.SelectedIndex <= 0 || ddlMarketTo.SelectedIndex <= 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScript", "appPop('โปรดเลือกตัวเลือก','กรุณาเลือกเงื่อนไขการค้นหาให้ครบถ้วน ')", true);
                return;
            }

            int monthstart = Convert.ToInt16(ddlOpenDateYear.SelectedValue) * 100 + ddlOpenDateMonth.SelectedIndex;
            int monthend = Convert.ToInt16(ddlOpenDateYearend.SelectedValue) * 100 + ddlOpenDateMonthend.SelectedIndex;
            int monthstartDTM = Convert.ToInt16(ddlOpenDateYearDTM.SelectedValue) * 100 + ddlOpenDateMonthDTM.SelectedIndex;
            int monthendDTM = Convert.ToInt16(ddlOpenDateYearDTMend.SelectedValue) * 100 + ddlOpenDateMonthDTMend.SelectedIndex;

            if (monthstart > monthend || monthstartDTM > monthendDTM)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScript", "appPop('วันที่เปิดใบคำขอสินเชื่อไม่ถูกต้อง?','เลือกวันที่เปิดใบคำขอไม่ถูกต้อง')", true);
                return;
            }

            //if (!(txtOpenDate.Text == ""))
            //{
            //    start12yg = DateTime.Parse((txtOpenDate.Text.ToString().Substring(0, 6) + (Convert.ToInt32(txtOpenDate.Text.Substring(6, 4).ToString()) - 543).ToString()));
            //    start = DateTime.Parse((txtCloseDate.Text.ToString().Substring(0, 6) + (Convert.ToInt32(txtCloseDate.Text.Substring(6, 4).ToString()) - 543).ToString()));
            //    APPDATE_Start = DateTime.Parse((TextSignOpenDate.Text.ToString().Substring(0, 6) + (Convert.ToInt32(TextSignOpenDate.Text.Substring(6, 4).ToString()) - 543).ToString()));
            //    APPDATE_End = DateTime.Parse((TextSignCloseDate.Text.ToString().Substring(0, 6) + (Convert.ToInt32(TextSignCloseDate.Text.Substring(6, 4).ToString()) - 543).ToString()));
            //}
            //else
            //{
            //    start = DateTime.Now;
            //    start12yg = start.AddMonths(-12);
            //    end3m = start.AddMonths(-3);
            //    end12yg = start.AddMonths(-24);
            //}
            if (ddlLoan.SelectedItem.Value != "")
                sqlQuery.Append(String.Format(" AND (LOAN_CD = '{0}')", ddlLoan.SelectedItem.Value));

            if (ddlSubType.SelectedItem.Value != "")
                sqlQuery.Append(String.Format(" AND (STYPE_CD = '{0}')", ddlSubType.SelectedItem.Value));

            if (ddlMarketFrm.SelectedIndex <= 0 || ddlMarketTo.SelectedIndex <= 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScript", "appPop('โปรดเลือกตัวเลือก','โปรดเลือก  Market Code ก่อน')", true);
                return;
            }

            if (ddlMarketFrm.SelectedItem.Value != "")
            {
                sqlQuery.Append(String.Format(" AND (MARKET_CD >= '{0}')", ddlMarketFrm.SelectedItem.Value));
                mktqrf = String.Format(" AND (MARKET_CD >= '{0}')", ddlMarketFrm.SelectedItem.Value);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScript", "appPop('โปรดเลือกตัวเลือก','โปรดเลือก  Market Code ก่อน')", true);
                return;
            }

            if (ddlMarketTo.SelectedItem.Value != "")
            {
                sqlQuery.Append(String.Format(" AND (MARKET_CD <= '{0}')", ddlMarketTo.SelectedItem.Value));
                mktqrt = String.Format(" AND (MARKET_CD <= '{0}')", ddlMarketTo.SelectedItem.Value);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScript", "appPop('โปรดเลือกตัวเลือก','โปรดเลือกสินเชื่อย่อย ถึง อย่างน้อย 1 ค่า')", true);
                return;
            }

            if ((ddlRegion.SelectedItem.Value == "null") && (ddlRegionTo.SelectedItem.Value == "null") && (ddlSubZone.SelectedIndex == -1 || ddlSubZone.SelectedItem.Value == "null") && (ddlSubZoneTo.SelectedIndex == -1 || ddlSubZoneTo.SelectedItem.Value == "null") && (ddlSubBranch.SelectedIndex == -1 || ddlSubBranch.SelectedItem.Value == "null") && (ddlSubBranchTo.SelectedIndex == -1 || ddlSubBranchTo.SelectedItem.Value == "null"))
            {
                branch_CD_TO = "0";
                branch_CD = "0";
            }
            else
            {
                if (!(ddlSubBranchTo.SelectedIndex == -1 || ddlSubBranch.SelectedIndex == -1 || ddlSubZone.SelectedIndex == -1 || ddlSubZoneTo.SelectedIndex == -1 || ddlRegion.SelectedIndex == -1 || ddlRegionTo.SelectedIndex == -1))
                {
                    if (ddlSubBranchTo.SelectedItem.Value == "null" || ddlSubBranch.SelectedItem.Value == "null" || ddlSubZone.SelectedItem.Value == "null" || ddlSubZoneTo.SelectedItem.Value == "null" || ddlRegion.SelectedItem.Value == "null" || ddlRegionTo.SelectedItem.Value == "null")
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScript", "appPop('โปรดเลือกตัวเลือก','กรุณาระบุข้อมูล ภาค เขต สาขา ให้ครบถ้วน')", true);
                        return;
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScript", "appPop('โปรดเลือกตัวเลือก','กรุณาระบุข้อมูล ภาค เขต สาขา ให้ครบถ้วน')", true);
                    return;
                }

                branch_CD_TO = ddlSubBranchTo.SelectedValue.ToString();
                branch_CD = ddlSubBranch.SelectedValue.ToString();
            }




            //if (ddlSubBranchTo.SelectedItem.Value != "null")
            //{
            //    //sqlQuery.Append(String.Format(" AND d.BRANCH_CD in ( {0} )",text));
            //    //branch_CD_TO = String.Format(" AND d.BRANCH_CD in ( {0} )",text);
            //}   
            if (ddlModel.SelectedIndex < 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScript", "appPop('กรุณาเลือกข้อมูลให้ถูกต้อง')", true);
            }
            else
            {
                if (ddlLoan.SelectedIndex < 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScript", "appPop('กรุณาเลือกข้อมูลให้ถูกต้อง')", true);
                }
                else
                {
                    if (ddlSubType.SelectedIndex < 0)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScript", "appPop('กรุณาเลือกข้อมูลให้ถูกต้อง')", true);
                    }
                    else
                    {
                        if (ddlMarketFrm.SelectedItem.Value != "")
                        {
                            if (ddlMarketTo.SelectedItem.Value != "")
                            {
                                int st1 = Convert.ToInt32(ddlMarketFrm.SelectedItem.Value);
                                int st2 = Convert.ToInt32(ddlMarketTo.SelectedItem.Value);

                                if (st1 <= st2)
                                {
                                    try
                                    {
                                        conn = new SQLToDataTable();
                                        appQuery1 = string.Format("select distinct scrdif 	" +
                                                "	into #Heading	" +
                                                "	from	" +
                                                "	(	" +
                                                "	select case when (SCORE - scr) < 0 then convert(varchar,(scr + ((((SCORE + 1) - scr)/rint) -1 ) * rint)) + '-' + convert(varchar,((scr + ((((SCORE + 1 ) - scr)/rint)-1 ) * rint) + rint - 1))  	" +
                                                "	when (SCORE - scr) >= 0 then convert(varchar,(scr + ((SCORE - scr)/rint) * rint)) + '-' + convert(varchar,((scr + ((SCORE - scr)/rint) * rint) + rint - 1))   	" +
                                                "	end as scrdif	" +
                                                "	from (	" +
                                                "	select (select cut_off from ST_SCORERANGE y where  y.ltype= '{0}' and y.lstype = '{1}' ) as scr	" +
                                                "	,(select rint from ST_SCORERANGE y where  y.ltype= '{0}' and y.lstype = '{1}' ) as rint,b.score	" +
                                                "	from [dbo].[LN_APP] a ,LN_GRADE b	" +
                                                "	where a.APP_NO = b.APP_NO	" +
                                                "   group by b.SCORE " + //Add By ARSoft
                                                "	) ScRange	" +
                                                "	) Z	" +
                                                "	order by  scrdif	" +
        
                                                "   insert into #Heading select range_name as scrange from st_scorernglst where range_cd = (select range_cd from ST_SCORERANGE where ltype= '{0}' and lstype = '{1}') " +
                                                "   and not exists (select scrdif from #Heading where RANGE_NAME=scrdif)" +
                                                "	select case when (SCORE - scr) < 0 then convert(varchar,(scr + ((((SCORE + 1) - scr)/rint) -1 ) * rint)) + '-' + convert(varchar,((scr + ((((SCORE + 1 ) - scr)/rint)-1 ) * rint) + rint - 1))  	" +
                                                "	when (SCORE - scr) >= 0 then convert(varchar,(scr + ((SCORE - scr)/rint) * rint)) + '-' + convert(varchar,((scr + ((SCORE - scr)/rint) * rint) + rint - 1))   	" +
                                                "	end as scrdif,z.*	" +
                                                "	into #temp	" +
                                                "	from (	" +
                                                "	select c.[CBS_APP_NO],[ISACTIVE],[DW_BADMONTH],convert(decimal(15,3),[DW_OUTSTAND]) as [DW_OUTSTAND],[LAST_UPDATE_DTM]	" +
                                                "	,case when DW_BADMONTH <=3 then 'N' else 'Y' end as IsNPL 	" +
                                                "	,case when A.CBS_STATUS IN ('3','6','7')then 'Y' else 'N' end as AcceptRej	" +
                                                "	,case when C.ISACTIVE=0 then 'Y' else 'N' end as ActiveOrNot 	" +
                                                "	,b.grade,   (select cut_off from ST_SCORERANGE y where  y.ltype= '{0}' and y.lstype = '{1}' ) as scr,  	" +
                                                "	(select rint from ST_SCORERANGE y where  y.ltype= '{0}' and y.lstype = '{1}' ) as rint,b.score	" +
                                                "	from [DWH_BTFILE] c, [LN_APP] a ,LN_GRADE b ,[ST_BRANCH] d " +
                                                "   where a.APP_NO = b.APP_NO   and a.BRANCH_CD=d.BRANCH_CD  and convert(decimal(15,3),c.DW_Outstand) > 0 " +
                                                //Tai 2013-12-27
                                                "	and a.APP_SEQ = (select MAX(LN_APP2.APP_SEQ) from LN_APP AS LN_APP2 where a.APP_NO = LN_APP2.APP_NO)   	" +
                                                //
                                                "	 and LOAN_CD = '{0}' and STYPE_CD = '{1}' {2} {3} {5} " +
                                                //"	and right('000000000000' + CBS_APP_NO,12) = a.APP_NO 	" +
                                                "	and CBS_APP_NO = a.APP_NO 	" +
                                                "	and right(C.[LAST_UPDATE_DTM],7) in ('{4}')	" +
                                                //"	and replace(right(C.[LAST_UPDATE_DTM],7),'/','-') in ('{4}')	" +
                                                "	) z	" +
                                                "	where    AcceptRej='Y' and ActiveOrNot = 'Y'	" +
        
                                                "	select * 	" +
                                                "	into #NPL	" +
                                                "	from #temp	" +
                                                "	where  IsNPL='Y'	" +
        
                                                "	select * 	" +
                                                "	into #PL	" +
                                                "	from #temp	" +
                                                "	where IsNPL='N'	" +
                                                "	select CC.*,DD.cnt_NPL,DD.[Sum of DW_OUTSTAND_NPL]	" +
                                                "	into #Main_Table1	" +
                                                "	from	" +
                                                "	(	" +
                                                "	select AA.scrdif,BB.cnt,BB.[Sum of DW_OUTSTAND_PL]	" +
                                                "	from	" +
                                                "	(	" +
                                                "	select * from #Heading	" +
                                                "	) AA	" +
                                                "	left join	" +
                                                "	(	" +
                                                "	select scrdif,count(*) as cnt,sum([DW_OUTSTAND]) as [Sum of DW_OUTSTAND_PL]	" +
                                                "	from #PL	" +
                                                "	group by  scrdif	" +
                                                "		" +
                                                "	) BB	" +
                                                "	on AA.scrdif=BB.scrdif	" +
                                                "	) CC	" +
                                                "	left join	" +
                                                "	(	" +
                                                "	select scrdif,count(*) as cnt_NPL,sum([DW_OUTSTAND]) as [Sum of DW_OUTSTAND_NPL]	" +
                                                "	from #NPL	" +
                                                "	group by  scrdif	" +
                                                "	) DD	" +
                                                "	on CC.scrdif=DD.scrdif	" +
                                                "	order by CC.scrdif	" +
        
                                                "	select scrdif	" +
                                                " 	,case when [cnt] is null then 0 else [cnt] end as [cnt]		" +
                                                " 	,case when [Sum of DW_OUTSTAND_PL] is null then 0 else  [Sum of DW_OUTSTAND_PL] end as [Sum of DW_OUTSTAND_PL]		" +
                                                " 	,case when [cnt_NPL] is null then 0 else [cnt_NPL] end as [cnt_NPL]			" +
                                                " 	,case when [Sum of DW_OUTSTAND_NPL] is null then 0 else [Sum of DW_OUTSTAND_NPL] end as [Sum of DW_OUTSTAND_NPL]		" +
                                                " 	,case when [cnt] is null then 0 else [cnt] end+(case when [cnt_NPL] is null then 0 else [cnt_NPL] end) as [Total sum of cnt]	" +
                                                " 	,case when [Sum of DW_OUTSTAND_PL] is null then 0 else [Sum of DW_OUTSTAND_PL] end+case when [Sum of DW_OUTSTAND_NPL]is null then 0 else [Sum of DW_OUTSTAND_NPL] end  as [Total_of DW_OUTSTAND]	" +
                                                "	into #Main_Table2	" +
                                                "	from #Main_Table1	" +
                                                "	select scrdif	" +
                                                "	,case when [Total sum of cnt] is null then 0 else [Total sum of cnt] end as [Total sum of cnt]	" +
                                                "	,case when [Total_of DW_OUTSTAND] is null then 0 else [Total_of DW_OUTSTAND] end as [Total_of DW_OUTSTAND]	" +
                                                "	,[cnt_NPL]	" +
                                                "	,(convert(decimal(15,3),[cnt_NPL])/case when [Total sum of cnt]=0 then 1 else [Total sum of cnt] end)*100.00 as [% NPLs]	" +
                                                "	,[Sum of DW_OUTSTAND_NPL]	" +
                                                "	,([Sum of DW_OUTSTAND_NPL]/case when [Total_of DW_OUTSTAND]=0 then 1 else [Total_of DW_OUTSTAND] end)*100.00 as [% Amount (NPLs)]	" +
                                                "	into #Main_Table3	" +
                                                "	from #Main_Table2	" +
                                                "	order by scrdif desc 	" +
        
                                                //"	select *, case when [Total sum of cnt]>0 then (select sum([Total sum of cnt]) from #Main_Table3 t2 where t2.scrdif >= t1.scrdif) end as [Cumulative Total]	" +
                                                "	select *, (select sum([Total sum of cnt]) from #Main_Table3 t2 where t2.scrdif >= t1.scrdif) as [Cumulative Total]	" +
                                                "	, (select sum([cnt_NPL]) from #Main_Table3 t2 where t2.scrdif >= t1.scrdif) as [Cumulative NPLs]	" +
                                                "	into #Main_Table4	" +
                                                "	from #Main_Table3 t1	" +
                                                "	select *	" +
                                                "	,convert(decimal(15,3),[Cumulative NPLs])*100.00/case when [Cumulative Total]=0 then 1 else [Cumulative Total] end as [% of Cumulative Bad] into #Main_Table5 	" +
                                                "	from #Main_Table4	" +
                                                "	order by scrdif	" +
        
                                                " select scrdif	as scrange, 	" +
                                                " [Total sum of cnt] as totcnt	" +
                                                " ,[Total_of DW_OUTSTAND] as	TOTAMT	" +
                                                " ,[cnt_NPL] as NPLCount	" +
                                                " ,[% NPLs]  as PNPL	" +
                                                " ,[Sum of DW_OUTSTAND_NPL] as BDAMT	" +
                                                " ,[% Amount (NPLs)] as PAMT	" +
                                                " ,[% of Cumulative Bad] as cumulativeamount	" +
                                                " into #temp9	" +
                                                " from #Main_Table5	" +
        
                                                " select 'Total' as [scrange],sum([totcnt]) as [totcnt],sum([TOTAMT]) as [TOTAMT]	" +
                                                " ,sum([NPLCount]) as [NPLCount],convert(decimal(15,3),sum([NPLCount]))*100.0/case when sum([totcnt])=0 then 1 else sum([totcnt]) end as [PNPL],sum([BDAMT]) as [BDAMT]	" +
                                                " ,convert(decimal(15,3),sum([BDAMT]))*100.00/case when sum([TOTAMT])=0 then 1 else sum([TOTAMT]) end as [PAMT],convert(decimal(15,3),sum([NPLCount]))*100.0/case when sum([totcnt])=0 then 1 else sum([totcnt])  end as [cumulativeamount]	" +
                                                " into #Total from #temp9	" +
                                                " select AA.scrange,totcnt,TOTAMT,NPLCount,PNPL,BDAMT,PAMT,cumulativeamount into #Total_line from (	" +
                                                " select range_name as scrange from st_scorernglst where range_cd = 	(select range_cd from ST_SCORERANGE where ltype= '{0}' and lstype = '{1}')  	" +
                                                " ) AA  left join (	select * from #temp9 ) BB on AA.scrange=BB.scrange	" +
                                                 "	select * into #Excel_format from #Total_line union	 select * from  #Total " +
                                                "	select scrange,CASE WHEN totcnt IS NULL THEN '0' else [dbo].[comma_format](convert(varchar,convert(decimal(15,0),totcnt))) end as totcnt" +
                                                " 	,CASE WHEN TOTAMT IS NULL THEN '0.00' else [dbo].[comma_percent_format](convert(varchar,convert(decimal(15,2),TOTAMT))) end as TOTAMT	" +
                                                " 	,CASE WHEN NPLCount IS NULL THEN '0' else [dbo].[comma_format](convert(varchar,convert(decimal(15,0),NPLCount))) end  as NPLCount" +
                                                " 	,CASE WHEN PNPL IS NULL THEN '0.00' else [dbo].[comma_percent_format](convert(varchar,convert(decimal(15,2),PNPL))) end  as PNPL" +
                                                " 	,CASE WHEN BDAMT IS NULL THEN '0.00' else [dbo].[comma_percent_format](convert(varchar,convert(decimal(15,2),BDAMT))) end  as  BDAMT " +
                                                " 	 ,CASE WHEN PAMT IS NULL THEN '0.00' else [dbo].[comma_percent_format](convert(varchar,convert(decimal(15,2),PAMT))) end  as PAMT " +
                                                " 	 ,CASE WHEN cumulativeamount IS NULL THEN '0.00' else [dbo].[comma_percent_format](convert(varchar,convert(decimal(15,2),cumulativeamount))) end  as cumulativeamount	 from 	#Excel_format " +
        
                                                "	drop table #Main_Table2	drop table #temp9 drop table #Total" +
                                                "	drop table #Main_Table3	drop table #Excel_format  " +
                                                "	drop table #Heading	" +
                                                "	drop table #temp	" +
                                                "	drop table #PL	drop table #Total_line" +
                                                "	drop table #NPL	" +
                                                "	drop table #Main_Table1	" +
                                                "	drop table #Main_Table4	  drop table #Main_Table5"
                                                , ddlLoan.SelectedItem.Value, ddlSubType.SelectedItem.Value, mktqrf, mktqrt, start.ToString("MM/yyyy"), branch_CD_TO);

                                        DataTable dt = new DataTable();

                                        if (ddlModel.SelectedItem.Value == "7")
                                        {
                                            String strConnString = ConfigurationManager.ConnectionStrings["GSBSQLServer"].ConnectionString;
                                            SqlConnection con = new SqlConnection(strConnString);
                                            con.Open();
                                            SqlCommand command;
                                            command = new SqlCommand("sp_BackEnd_PortfolioScorePerformanceReport", con);

                                            command.Parameters.Add(new SqlParameter("@LASTUPDATEDTM_Start", ddlOpenDateYearDTM.SelectedValue.ToString() + (ddlOpenDateMonthDTM.SelectedIndex + 1).ToString("00")));
                                            command.Parameters.Add(new SqlParameter("@LASTUPDATEDTM_End", ddlOpenDateYearDTMend.SelectedValue.ToString() + (ddlOpenDateMonthDTMend.SelectedIndex + 1).ToString("00")));
                                            command.Parameters.Add(new SqlParameter("@ddlLoan", ddlLoan.SelectedItem.Value));
                                            command.Parameters.Add(new SqlParameter("@ddlSubType", ddlSubType.SelectedItem.Value));
                                            command.Parameters.Add(new SqlParameter("@mktqrf", ddlMarketFrm.SelectedItem.Value));
                                            command.Parameters.Add(new SqlParameter("@mktqrt", ddlMarketTo.SelectedItem.Value));
                                            command.Parameters.Add(new SqlParameter("@APPDATE_Start", ddlOpenDateYear.SelectedValue.ToString() + (ddlOpenDateMonth.SelectedIndex + 1).ToString("00")));
                                            command.Parameters.Add(new SqlParameter("@APPDATE_End", ddlOpenDateYearend.SelectedValue.ToString() + (ddlOpenDateMonthend.SelectedIndex + 1).ToString("00")));
                                            command.Parameters.Add(new SqlParameter("@branch_from", branch_CD));
                                            command.Parameters.Add(new SqlParameter("@branch_to", branch_CD_TO));
                                       
                                            command.CommandType = CommandType.StoredProcedure;
                                            command.CommandTimeout = 36000;
                                            SqlDataAdapter da = new SqlDataAdapter(command);
                                            da.Fill(dt);
                                            Session["dtb"] = dt;
                                            gvTable.DataSource = Session["dtb"];
                                            gvTable.DataBind();
                                        }
                                        else
                                        {
                                            Session["dtb"] = conn.ExcuteSQL(appQuery1);
                                            gvTable.DataSource = Session["dtb"];
                                            gvTable.DataBind();
                                            //conn.CloseConnection();
                                        }

                                    }
                                    catch (Exception ex)
                                    {
                                        throw ex;
                                    }
                                    finally
                                    {
                                        //conn.CloseConnection();
                                    }
                                }
                                else
                                {
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScript", "appPop('โปรดเลือกตัวเลือก','ค่า สินเชื่อย่อยถึง(TO) น้อยกว่า  สินเชื่อย่อยจาก(From)')", true);
                                } 
                            }
                        }
                    }
                }
            }
        }
        protected void btnExcel_Click(object sender, EventArgs e)
        {
            if (gvTable.Rows.Count > 0)
            {
                //Generate report header
                Session["sHeaderRpt1"] = "ประเภทโมเดล : " + ddlModel.SelectedItem.Text;
                Session["sHeaderRpt2"] = "ประเภทสินเชื่อ / ประเภทสินเชื่อย่อย : " + ddlLoan.SelectedItem.Text + " / " + ddlSubType.SelectedItem.Text;

                if (ddlMarketFrm.SelectedIndex == 0)
                    Session["sHeaderRpt3"] = "Market Code : " + ddlMarketFrm.SelectedItem.Text;
                else
                    Session["sHeaderRpt3"] = "Market Code : " + ddlMarketFrm.SelectedItem.Text + " ถึง " + ddlMarketTo.SelectedItem.Text;

                Session["sHeaderRpt4"] = "As of Date : " + (ddlOpenDateMonthDTM.SelectedItem) + " " + ddlOpenDateYearDTM.SelectedItem + " ถึง " + (ddlOpenDateMonthDTMend.SelectedItem) + " " + ddlOpenDateYearDTMend.SelectedItem;


                if (ddlRegion.SelectedItem.Value == "null")
                {
                    Session["sHeaderRpt5"] = "ภาค : " + "-" + " ถึง " + "-";
                    Session["sHeaderRpt6"] = "เขต : " + "-" + " ถึง " + "-";
                    Session["sHeaderRpt7"] = "สาขา : " + "-" + " ถึง " + "-";
                    Session["sHeaderRpt8"] = "วันที่เปิดใบคำขอสินเชื่อ : " + ddlOpenDateMonth.SelectedValue + " " + ddlOpenDateYear.SelectedItem + " ถึง " + ddlOpenDateMonthend.SelectedValue + " " + ddlOpenDateYearend.SelectedItem;

                }
                else
                {
                
                    Session["sHeaderRpt5"] = "ภาค : " + ddlRegion.SelectedItem.Text + " ถึง " + ddlRegionTo.SelectedItem.Text;
                    Session["sHeaderRpt6"] = "เขต : " + ddlSubZone.SelectedItem.Text + " ถึง " + ddlSubZoneTo.SelectedItem.Text;
                    Session["sHeaderRpt7"] = "สาขา : " + ddlSubBranch.SelectedItem.Text + " ถึง " + ddlSubBranchTo.SelectedItem.Text;
                    Session["sHeaderRpt8"] = "วันที่เปิดใบคำขอสินเชื่อ : " + ddlOpenDateMonth.SelectedValue + " " + ddlOpenDateYear.SelectedItem + " ถึง " + ddlOpenDateMonthend.SelectedValue + " " + ddlOpenDateYearend.SelectedItem;


                }


                //Regenerate data for report
                DataTable dt = (DataTable)Session["dtb"];
                Session["rptData"] = dt;
                ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "aa", "window.open('ReportLayOut/frmPreviewBackRpt.aspx?ReportId=95');", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScript", "appPop('โปรดเลือกตัวเลือก','กรุณากดปุ่มแสดงข้อมูลก่อน')", true);
                return;
            }
        }

        //Generate Report
        protected void PrintReport_Click(object sender, EventArgs e)
        {
            if (gvTable.Rows.Count > 0)
            {
                Label userFirstname = (Label)Master.FindControl("userFirstname");
                Session["userFirstname"] = userFirstname.Text;
                print_preview();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScript", "appPop('โปรดเลือกตัวเลือก','กรุณากดปุ่มแสดงข้อมูลก่อน')", true);
                return;
            }
        }

        private void print_preview()
        {
            //Generate report header
            Session["sHeaderRpt1"] = "ประเภทโมเดล : " + ddlModel.SelectedItem.Text;
            Session["sHeaderRpt2"] = "ประเภทสินเชื่อ / ประเภทสินเชื่อย่อย : " + ddlLoan.SelectedItem.Text + " / " + ddlSubType.SelectedItem.Text;

            if (ddlMarketFrm.SelectedIndex == 0)
                Session["sHeaderRpt3"] = "Market Code : " + ddlMarketFrm.SelectedItem.Text;
            else
                Session["sHeaderRpt3"] = "Market Code : " + ddlMarketFrm.SelectedItem.Text + " ถึง " + ddlMarketTo.SelectedItem.Text;

            Session["sHeaderRpt4"] = "As of Date : " + (ddlOpenDateMonthDTM.SelectedItem) + " " + ddlOpenDateYearDTM.SelectedItem + " ถึง " + (ddlOpenDateMonthDTMend.SelectedItem) + " " + ddlOpenDateYearDTMend.SelectedItem;



            if (ddlRegion.SelectedItem.Value == "null")
            {
                Session["sHeaderRpt5"] = "ภาค : " + "-" + " ถึง " + "-";
                Session["sHeaderRpt6"] = "เขต : " + "-" + " ถึง " + "-";
                Session["sHeaderRpt7"] = "สาขา : " + "-" + " ถึง " + "-";
                Session["sHeaderRpt8"] = "วันที่เปิดใบคำขอสินเชื่อ : " + ddlOpenDateMonth.SelectedValue + " " + ddlOpenDateYear.SelectedItem + " ถึง " + ddlOpenDateMonthend.SelectedValue + " " + ddlOpenDateYearend.SelectedItem;

            }
            else
            {

                Session["sHeaderRpt5"] = "ภาค : " + ddlRegion.SelectedItem.Text + " ถึง " + ddlRegionTo.SelectedItem.Text;
                Session["sHeaderRpt6"] = "เขต : " + ddlSubZone.SelectedItem.Text + " ถึง " + ddlSubZoneTo.SelectedItem.Text;
                Session["sHeaderRpt7"] = "สาขา : " + ddlSubBranch.SelectedItem.Text + " ถึง " + ddlSubBranchTo.SelectedItem.Text;
                Session["sHeaderRpt8"] = "วันที่เปิดใบคำขอสินเชื่อ : " + ddlOpenDateMonth.SelectedValue + " " + ddlOpenDateYear.SelectedItem + " ถึง " + ddlOpenDateMonthend.SelectedValue + " " + ddlOpenDateYearend.SelectedItem;


            }
            //Regenerate data for report
            DataTable dt = (DataTable)Session["dtb"];
            Session["rptData"] = dt;
            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "aa", "window.open('ReportLayOut/frmPreviewBackRpt.aspx?ReportId=5');", true);
        }

        protected void Cancel_Click(object sender, EventArgs e)
        {
            ddlModel.ClearSelection();
            ddlLoan.ClearSelection();
            ddlSubType.ClearSelection();
            ddlMarketFrm.ClearSelection();
            ddlMarketTo.ClearSelection();
            ddlSubType.Enabled = false;
            //txtOpenDate.Text = string.Empty;
            ddlRegion.SelectedValue = "null";
            ddlRegionTo.SelectedValue = "null";
            ddlSubZone.SelectedValue = "null";
            ddlSubZoneTo.SelectedValue = "null";
            ddlSubBranch.SelectedValue = "null";
            ddlSubBranchTo.SelectedValue = "null";

        }
        protected void gvTable_PageChanging(object sender, GridViewPageEventArgs e)
        {
            gvTable.PageIndex = e.NewPageIndex;
            Search_Click(null, null);
        }
        

        protected void gvTable_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                GridView HeaderGrid = (GridView)sender;

                e.Row.Cells.Clear();


                GridViewRow HeaderGridRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);
                GridViewRow HeaderGridRow2 = new GridViewRow(1, 0, DataControlRowType.Header, DataControlRowState.Insert);

                TableCell HeaderCell = new TableCell();
                HeaderCell.Text = "";
                HeaderGridRow.Cells.Add(HeaderCell);

                HeaderCell = new TableCell();
                HeaderCell.Text = "Active Account";
                HeaderCell.ColumnSpan = 2;
                HeaderGridRow.Cells.Add(HeaderCell);

                HeaderCell = new TableCell();
                HeaderCell.Text = "Non-Performing Loan";
                HeaderCell.ColumnSpan = 5;
                HeaderGridRow.Cells.Add(HeaderCell);

                //HeaderCell = new TableCell();
                //HeaderCell.Text = "";
                //HeaderGridRow.Cells.Add(HeaderCell);

                HeaderCell = new TableCell();
                HeaderCell.Text = "Cutoff Score";
                HeaderGridRow2.Cells.Add(HeaderCell);

                HeaderCell = new TableCell();
                HeaderCell.Text = "No. of Account";
                HeaderGridRow2.Cells.Add(HeaderCell);


                HeaderCell = new TableCell();
                HeaderCell.Text = "Outstanding Balance<br />(ล้านบาท)";
                HeaderGridRow2.Cells.Add(HeaderCell);


                HeaderCell = new TableCell();
                HeaderCell.Text = "No. of Account";
                HeaderGridRow2.Cells.Add(HeaderCell);


                HeaderCell = new TableCell();
                HeaderCell.Text = "%NPLs";
                HeaderGridRow2.Cells.Add(HeaderCell);


                HeaderCell = new TableCell();
                HeaderCell.Text = "Outstanding Balance<br />(ล้านบาท)<br />(NPLs)";
                HeaderGridRow2.Cells.Add(HeaderCell);


                HeaderCell = new TableCell();
                HeaderCell.Text = "% Outstanding Balance<br />(ล้านบาท)<br />(NPLs)";
                HeaderGridRow2.Cells.Add(HeaderCell);


                HeaderCell = new TableCell();
                HeaderCell.Text = "% of Cumulative<br />Bad";
                HeaderGridRow2.Cells.Add(HeaderCell);

                gvTable.Controls[0].Controls.AddAt(0, HeaderGridRow);
                gvTable.Controls[0].Controls.AddAt(1, HeaderGridRow2);

            }
        }



        protected void gvTable_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        #endregion Protected Method

        public ListItemCollection GetMonth()
        {
            ListItemCollection mCollection = new ListItemCollection();
            ListItem m1 = new ListItem();
            m1.Value = "มกราคม";
            mCollection.Add(m1);
            ListItem m2 = new ListItem();
            m2.Value = "กุมภาพันธ์";
            mCollection.Add(m2);
            ListItem m3 = new ListItem();
            m3.Value = "มีนาคม";
            mCollection.Add(m3);
            ListItem m4 = new ListItem();
            m4.Value = "เมษายน";
            mCollection.Add(m4);
            ListItem m5 = new ListItem();
            m5.Value = "พฤษภาคม";
            mCollection.Add(m5);
            ListItem m6 = new ListItem();
            m6.Value = "มิถุนายน";
            mCollection.Add(m6);
            ListItem m7 = new ListItem();
            m7.Value = "กรกฎาคม";
            mCollection.Add(m7);
            ListItem m8 = new ListItem();
            m8.Value = "สิงหาคม";
            mCollection.Add(m8);
            ListItem m9 = new ListItem();
            m9.Value = "กันยายน";
            mCollection.Add(m9);
            ListItem m10 = new ListItem();
            m10.Value = "ตุลาคม";
            mCollection.Add(m10);
            ListItem m11 = new ListItem();
            m11.Value = "พฤศจิกายน";
            mCollection.Add(m11);
            ListItem m12 = new ListItem();
            m12.Value = "ธันวาคม";
            mCollection.Add(m12);

            return mCollection;
        }
    }
}