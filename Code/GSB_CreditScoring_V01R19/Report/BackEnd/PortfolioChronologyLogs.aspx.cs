using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GSB.Class;
using System.Data;
namespace GSB.Report.BackEnd
{
    public partial class PortfolioChronologyLogs : System.Web.UI.Page
    {
        SQLToDataTable conn = new SQLToDataTable();

        #region Page_Load

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                LoadModel();
                LoadModel2();
                //LoadLoan();
            }
        }

        #endregion Page_Load

        #region Private Method

        private void LoadModel()
        {
            string Query = string.Format("select distinct " +
                            "CONVERT(int,MODEL_CD) [MODEL_CD],  " +
                            "MODEL_CD +' - '+ MODEL_NAME [MODEL_NAME] from LN_MODEL where ACTIVE_FLAG = '1' and MODEL_CD not in ('005','006') " +
                            "union select null, 'โปรดเลือก...' " +
                            "order by MODEL_CD ASC");

            ddlModel.DataSource = conn.ExcuteSQL(Query);
            ddlModel.DataBind();
        }

        private void LoadModel2()
        {
            string Query = string.Format("select distinct " +
                            "CONVERT(int,MODEL_CD) [MODEL_CD],  " +
                            "MODEL_CD +' - '+ MODEL_NAME [MODEL_NAME] from LN_MODEL where ACTIVE_FLAG = '1' and MODEL_CD not in ('005','006') " +
                            "union select null, 'โปรดเลือก...' " +
                            "order by MODEL_CD ASC");

            //ddlModel2.Items.Clear(); 
            ddlModel2.DataSource = conn.ExcuteSQL(Query);
            ddlModel2.DataBind();
        }

        protected void LoadLoan(object sender, EventArgs e)
        {
            string Query = "";

            if (ddlModel2.SelectedItem.Value == "7")
            {
                Query = string.Format("select distinct " +
"a.LOAN_CD [LOAN_CD],  " +
"a.LOAN_CD+' - '+a.LOAN_NAME [LOAN_NAME] from ST_LOANTYPE a left join [dbo].[ST_MKTDFTMYMO] b on a.LOAN_CD = b.loantype where ACTIVE_FLAG = '1' and b.loantype is not null " +
"union select null, 'เลือกทั้งหมด...' " +
"order by LOAN_CD ASC");
            }
            else
            {
                Query = string.Format("select distinct " +
"a.LOAN_CD [LOAN_CD],  " +
"a.LOAN_CD+' - '+a.LOAN_NAME [LOAN_NAME] from ST_LOANTYPE a left join [dbo].[ST_MKTDFTMYMO] b on a.LOAN_CD = b.loantype where ACTIVE_FLAG = '1' and b.loantype is null " +
"union select null, 'เลือกทั้งหมด...' " +
"order by LOAN_CD ASC");
            }

            ddlLoan.DataSource = conn.ExcuteSQL(Query);
            ddlLoan.DataBind();
            ddlLoan.Enabled = true;
            ddlSubType.Items.Insert(0, new ListItem("เลือกทั้งหมด...", string.Empty));
            ddlMarketFrm.Items.Insert(0, new ListItem("เลือกทั้งหมด...", "0"));
        }

        #endregion Private Method

        #region Protected Method

        protected void LoadSubType(object sender, EventArgs e)
        {
            string Query = "";

            if (ddlModel2.SelectedItem.Value == "7")
            {
                Query = string.Format("select distinct " +
                                         "STYPE_CD [STYPE_CD], " +
                                         "STYPE_CD+' - '+STYPER_NAME [STYPER_NAME] from ST_LOANSTYPE a left join [dbo].[ST_MKTDFTMYMO] b on a.STYPE_CD = b.subtype where ACTIVE_FLAG = '1' and LOAN_CD = '{0}' and b.subtype is not null " +
                                         "union select null, 'เลือกทั้งหมด...' " +
                                         "order by STYPE_CD ASC", ddlLoan.SelectedItem.Value);
            }
            else
            {
                Query = string.Format("select distinct " +
                         "STYPE_CD [STYPE_CD], " +
                         "STYPE_CD+' - '+STYPER_NAME [STYPER_NAME] from ST_LOANSTYPE a left join [dbo].[ST_MKTDFTMYMO] b on a.STYPE_CD = b.subtype where ACTIVE_FLAG = '1' and LOAN_CD = '{0}' and b.subtype is null " +
                         "union select null, 'เลือกทั้งหมด...' " +
                         "order by STYPE_CD ASC", ddlLoan.SelectedItem.Value);
            }

            ddlSubType.Items.Clear();
            ddlSubType.DataSource = conn.ExcuteSQL(Query);
            ddlSubType.DataBind();
            ddlSubType.Enabled = true;
            ddlMarketFrm.Items.Insert(0, new ListItem("เลือกทั้งหมด...", "0"));
        }

        //protected void LoadVersion(object sender, EventArgs e)
        //{
        //    string sql = "";
        //    sql = string.Format("select MODEL_NAME as [Model_V_NAME] from LN_MODEL where ACTIVE_FLAG = '1' and");
        //    //sql += "  [MODEL_CD]=" + ddlModel2.SelectedValue + " ";


        //    ddlModel2.DataSource = conn.ExcuteSQL(sql);
        //    ddlModel2.DataTextField = "Model_V_NAME";
        //    ddlModel2.DataValueField = "Model_V_NAME";
        //    ddlModel2.DataBind();

        //    string Query = string.Format("select distinct " + "CONVERT(int,LOAN_CD) [LOAN_CD],  " +
        //"LOAN_CD+' - '+LOAN_NAME [LOAN_NAME] from ST_LOANTYPE a,ST_SCORERANGE b where ACTIVE_FLAG = '1' and a.loan_cd=b.ltype " +
        //" and [MODEL_CD]=" + ddlModel2.SelectedValue + " " +
        //"order by LOAN_CD ASC");

        //    ddlLoan.DataSource = conn.ExcuteSQL(Query);
        //    ddlLoan.DataBind();
        //    ddlLoan.Items.Insert(0, new ListItem("โปรดเลือก...", string.Empty));
        //}
        protected void LoadMarket(object sender, EventArgs e)
        {
            string Query = "";

            if (ddlModel2.SelectedItem.Value == "7")
            {
                Query = string.Format("select distinct " +
             "MARKET_CD [MARKET_CD], " +
             "MARKET_CD+' - '+MARKET_NAME [MARKET_NAME] from ST_LOANMKT a left join [dbo].[ST_MKTDFTMYMO] b on MARKET_CD = b.marketcode where ACTIVE_FLAG = '1' and LOAN_CD = '{0}' and STYPE_CD = '{1}' and b.marketcode is not null  " +
             "union select null, '00000 - เลือกทั้งหมด...' " +
             "order by MARKET_CD ASC",
             ddlLoan.SelectedItem.Value, ddlSubType.SelectedItem.Value);
            }
            else
            {
                Query = string.Format("select distinct " +
"MARKET_CD [MARKET_CD], " +
"MARKET_CD+' - '+MARKET_NAME [MARKET_NAME] from ST_LOANMKT a left join [dbo].[ST_MKTDFTMYMO] b on MARKET_CD = b.marketcode where ACTIVE_FLAG = '1' and LOAN_CD = '{0}' and STYPE_CD = '{1}' and b.marketcode is null  " +
"union select null, '00000 - เลือกทั้งหมด...' " +
"order by MARKET_CD ASC",
ddlLoan.SelectedItem.Value, ddlSubType.SelectedItem.Value);
            }

            ddlMarketFrm.Items.Clear();
            ddlMarketFrm.DataSource = conn.ExcuteSQL(Query);
            ddlMarketFrm.DataBind();
            ddlMarketFrm.Enabled = true;
        }

        protected void Search_Click(object sender, EventArgs e)
        { 
            DateTime start = new DateTime();
            DateTime end = new DateTime();
            string appQuery1 = "";

            if (this.txtOpenDate.Text == "" || this.txtCloseDate.Text == "") // no specified date means all data
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScript", "appPop('โปรดเลือกตัวเลือก','กรุณาเลือกเงื่อนไขการค้นหาให้ครบถ้วน ')", true);
                return;

                //appQuery1 = string.Format("SELECT OID,[LOAN_TYPE] as LTYPE ,[LOAN_STYPE] as LSTYPE,MODEL_CD as MODEL, [LOAN_MKT] as LMKT,[MESSAGE] as MES,[CREATION_DTM] as DTM FROM [LOG_CHRONOLOGY] where MODEL_CD in ('{0}') ORDER BY DTM, LTYPE, LSTYPE, MODEL, LMKT", ("00" + ddlModel.SelectedItem.Value));
            }
            else // specifying date continue to check for more conditions
            {
                if (!(txtOpenDate.Text == ""))
                {
                    start = DateTime.Parse((txtOpenDate.Text.ToString().Substring(0, 6) + (Convert.ToInt32(txtOpenDate.Text.Substring(6, 4).ToString()) - 543).ToString()));
                }
                else
                {
                    start = DateTime.Now;
                }
                if (!(txtCloseDate.Text == ""))
                {
                    end = DateTime.Parse(txtCloseDate.Text.ToString().Substring(0, 6) + (Convert.ToInt32(txtCloseDate.Text.Substring(6, 4).ToString()) - 543).ToString() + " 23:59:59.999");
                }
                else
                {
                    end = DateTime.Now;

                }

               // int months = end.CompareTo(start);//(end.Month - start.Month) + 12 * (end.Year - start.Year);
                int months = (end.DayOfYear - start.DayOfYear) + 365 * (end.Year - start.Year);
                if (months < 0)
ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScript", "appPop('วันที่เปิดใบคำขอสินเชื่อไม่ถูกต้อง?','วันที่เปิดฯ เริ่มต้น ต้องน้อยกว่า หรือเท่ากับ วันที่เปิดฯ สุดท้าย')", true);

                if (this.ddlModel.SelectedItem.Value.Equals("0"))
                {
                        appQuery1 = string.Format("SELECT OID,case when [LOAN_TYPE]=0 then 'ไม่ได้เลือก' else [LOAN_TYPE] end as LTYPE,case when [LOAN_STYPE]=0 then 'ทั้งหมด' else [LOAN_STYPE] end as LSTYPE,case when MODEL_CD=0 then 'ไม่ได้เลือก' else MODEL_CD end as MODEL,case when  [LOAN_MKT]=0 then 'ทั้งหมด' else [LOAN_MKT] end as LMKT,[MESSAGE] as MES,[CREATION_DTM] as DTM " +
    " FROM [LOG_CHRONOLOGY] where convert(integer,substring(convert(varchar,convert(date,CREATION_DTM,103)),1,4)+substring(convert(varchar,convert(date,CREATION_DTM,103)),6,2)+substring(convert(varchar,convert(date,CREATION_DTM,103)),9,2))  " +
            " between convert(integer,(substring('{1}',1,4)+substring('{1}',6,2)+substring('{1}',9,2))) " +
            " and convert(integer,(substring('{2}',1,4)+substring('{2}',6,2)+substring('{2}',9,2))) order by convert(integer,CREATION_DTM) ", (ddlModel.SelectedItem.Value), start.ToString("yyyy-MM-dd"), end.ToString("yyyy-MM-dd"));
                }
                else
                {

                        appQuery1 = string.Format("SELECT OID,case when [LOAN_TYPE]=0 then 'ไม่ได้เลือก' else [LOAN_TYPE] end as LTYPE ,case when [LOAN_STYPE]=0 then 'ทั้งหมด' else [LOAN_STYPE] end as LSTYPE,case when MODEL_CD=0 then 'ไม่ได้เลือก' else MODEL_CD end as MODEL,case when  [LOAN_MKT]=0 then 'ทั้งหมด' else [LOAN_MKT] end as LMKT,[MESSAGE] as MES,[CREATION_DTM] as DTM " +
" FROM [LOG_CHRONOLOGY] where MODEL_CD = {0} and convert(integer,substring(convert(varchar,convert(date,CREATION_DTM,103)),1,4)+substring(convert(varchar,convert(date,CREATION_DTM,103)),6,2)+substring(convert(varchar,convert(date,CREATION_DTM,103)),9,2)) " +
" between convert(integer,(substring('{1}',1,4)+substring('{1}',6,2)+substring('{1}',9,2))) " +
" and convert(integer,(substring('{2}',1,4)+substring('{2}',6,2)+substring('{2}',9,2))) order by convert(integer,CREATION_DTM)  ", (ddlModel.SelectedItem.Value), start.ToString("yyyy-MM-dd"), end.ToString("yyyy-MM-dd"));

                }
            }
            //For Report Generate
            
            gvTable.Visible=true;

            DataTable dt = new DataTable();
            
            ViewState["DataTable1"] = conn.ExcuteSQL(appQuery1);
            gvTable.DataSource = ViewState["DataTable1"];

            gvTable.DataBind();
            if (gvTable.PageCount.Equals(0))
            {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScript", "appPop('กรุณาเพิ่มข้อมูล','ไม่มีข้อมูล(NO DATA)ในช่วงเวลาที่เลือก')", true);
            }             
            else
            {
            this.Table1.Visible = true;
            this.ResultTable.Visible = true;
            this.Table3.Visible = false; // prevent back button to be shown
            //this.tbcSQLBX.Text=null;
            }
        }


        protected void gvTable_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Table3.Visible = true;
            Table1.Visible = false;
            ResultTable.Visible = false; 
            if (e.CommandName == "ExpanView")
            {
                string appQuery = string.Format("SELECT (select top 1 a.MODEL_CD +' - '+ a.MODEL_NAME from LN_MODEL a where x.MODEL_CD = a.MODEL_CD) as [MODEL_NAME] " +
" ,MODELVER_CD,(select top 1 LOAN_CD+' - '+LOAN_NAME from ST_LOANTYPE b where b.LOAN_CD =  [LOAN_TYPE]) as [LOAN_NAME] " +
" ,(select top 1 RIGHT ('00000'+ CAST (STYPE_CD AS varchar), 5)+' - '+STYPER_NAME from ST_LOANSTYPE c where c.STYPE_CD = [LOAN_STYPE] and c.LOAN_CD  =  [LOAN_TYPE] ) as [STYPER_NAME] " +
" ,(select top 1 RIGHT ('00000'+ CAST (MARKET_CD AS varchar), 5)+' - '+MARKET_NAME from ST_LOANMKT d where d.MARKET_CD = [LOAN_MKT] and d.LOAN_CD = [LOAN_TYPE] and d.STYPE_CD = [LOAN_STYPE] ) as [MARKET_NAME]  " +
" ,[MESSAGE],[CREATION_DTM] as DTM into #temp FROM [LOG_CHRONOLOGY] x where OID =  {0} " +
" select case when MODEL_NAME is null then 'ไม่ได้เลือก' else MODEL_NAME end as MODEL_NAME,MODELVER_CD,case when LOAN_NAME is null then 'ไม่ได้เลือก' else LOAN_NAME end as LOAN_NAME,case when STYPER_NAME is null then 'ทั้งหมด' else STYPER_NAME end as STYPER_NAME,case when [MARKET_NAME]  is null then 'ทั้งหมด' else [MARKET_NAME]  end as [MARKET_NAME] ,MESSAGE,DTM from #temp  drop table #temp", e.CommandArgument.ToString());

                    DataTable dtTemp = conn.ExcuteSQL(appQuery);
                    for (int i = 0; i < dtTemp.Rows.Count; i++)
                    {
                        for (int j = 0; j < dtTemp.Columns.Count; j++)
                        {
                            if (dtTemp.Rows[i][j].ToString() == "")
                            {
                                dtTemp.Rows[i][j] = "0";
                            }
                            
                        }
                    }
                    if (dtTemp != null)
                    {
                        if (dtTemp.Rows.Count > 0)
                        {
                            gvApp.DataSource = dtTemp;
                            gvApp.DataBind();
                            gvApp.Visible = true;

                        }
                    }
            }

            
        }

        protected void Cancel_Click(object sender, EventArgs e)
        {
            ddlModel.ClearSelection();
            ddlLoan.ClearSelection();
            ddlSubType.Enabled = false;
            txtOpenDate.Text = "";
            txtCloseDate.Text = "";
            gvTable.Visible = false;

        }

        //Generate Report
        protected void PrintReport_Click(object sender, EventArgs e)
        {

            Label userFirstname = (Label)Master.FindControl("userFirstname");
            Session["userFirstname"] = userFirstname.Text;
            print_preview();
        }

        private void print_preview()
        {
            //Generate report header
            Session["sHeaderRpt1"] = "ประเภทโมเดล : " + ddlModel.SelectedItem.Text;
            Session["sHeaderRpt2"] = "Model Version : ";
            Session["sHeaderRpt3"] = "ระหว่างวันที่ : " + txtOpenDate.Text + " ถึง " + txtCloseDate.Text;

            //Regenerate data for report
            DataTable dt = (DataTable)ViewState["DataTable1"];
            Session["rptData"] = dt;
            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "aa", "window.open('ReportLayOut/frmPreviewBackRpt.aspx?ReportId=7');", true);

        }
        protected void Add_Click(object sender, EventArgs e)
        {
            Table2.Visible = true;
            Table1.Visible = false;
            ResultTable.Visible = false;
            Asofdt.Text = DateTime.Now.ToString();
            // chonawat
            ddlModel2.ClearSelection();
            ddlLoan.ClearSelection();
            ddlSubType.ClearSelection();
            ddlSubType.SelectedValue = "0";
            //ModelVersion1.SelectedValue = "1";
            ddlMarketFrm.ClearSelection();

            // Kayun added
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScript", "appPop('กรุณาเลือก','กรุณาเลือกโมเดลก่อนทำงานต่อ')", true);
            //this.tbcSQLBX.Text = "";
        }

        protected void ShowTextBox(object sender, EventArgs e) // Kayun added
        {
            //this.tbcModelVersion.Visible = true;
            //this.tbcModelV.Visible = true;
            this.tbcSQLBX.Visible = true;
        }

        protected void Back_Click(object sender, EventArgs e)
        {
            Table3.Visible = false;
            Table1.Visible = true;
            ResultTable.Visible = true;
            //ddlModel2.ClearSelection();
            ////ddlLoan.ClearSelection();
            ////ddlSubType.ClearSelection();
            ////ddlSubType.SelectedValue = "0"; ;
            ////ddlMarketFrm.ClearSelection();
            //txtOpenDate.Text = "";
            //txtCloseDate.Text = "";
        }
        
        protected void Save_Click(object sender, EventArgs e)
        {
            try
            {
                string appQuery1 = string.Format("INSERT INTO LOG_CHRONOLOGY([MODEL_CD],[MODELVER_CD],[LOAN_TYPE],[LOAN_STYPE],[LOAN_MKT],[MESSAGE],[CREATION_DTM],[CREATION_BY],[LAST_UPDATE_DTM],[LAST_UPDATE_BY]) values('{0}','{5}','{1}','{2}','{3}','{4}',GETDATE(),'Admin',NULL,NULL)"
                    , ("00" + ddlModel2.SelectedItem.Value), ddlLoan.SelectedItem.Value, ddlSubType.SelectedItem.Value, ddlMarketFrm.SelectedItem.Value, txtSQLBX.Text.ToString(),"");
                gvTable.DataSource = conn.ExcuteSQL(appQuery1);
                gvTable.DataBind();

                //ddlModel2.Items.Clear();
                //LoadModel2();
                Table2.Visible = false;
                Table1.Visible = true;
                ResultTable.Visible = true;
                ddlModel.ClearSelection();
                ddlModel.SelectedValue = "0";

                //ModelVersion.SelectedValue = "0";
                txtOpenDate.Text = "";
                txtCloseDate.Text = "";

                ddlModel.Visible = false;
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScript", "appPop('กรุณาเลือก','กรุณาเลือกช่วงเวลาก่อนทำงานต่อ')", true);
                ddlModel.Visible = true;
                

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScript", "appPop('มีปัญหาในการเซฟ','" + ex.Message + "')", true);
            }
            this.tbcSQLBX.Text = "";
        }

        protected void SCancel_Click(object sender, EventArgs e)
        {
            Table2.Visible = false;
            Table1.Visible = true;
            ResultTable.Visible = true;
        }
        protected void gvTable_PageChanging(object sender, GridViewPageEventArgs e)
        {
            gvTable.PageIndex = e.NewPageIndex;
            Search_Click(null, null);
        }
        #endregion Protected Method
    }
}