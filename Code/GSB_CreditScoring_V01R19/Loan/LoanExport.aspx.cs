using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GSB.Class;
using System.Collections;
using System.Data;
using System.IO;
using System.Text;
using System.Globalization;
using System.Data.SqlClient;
using System.Configuration;

namespace GSB.Loan
{
    public partial class LoanExport : System.Web.UI.Page
    {
        SQLToDataTable conn = new SQLToDataTable();

        #region Page_Load

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                LoadModel();
                LoadRegion();
                LoadZone();
                LoadBranch();
                LoadObjective();
                LoadGSBLoan();
                LoadMainColumn();
            }
        }

        #endregion Page_Load

        #region Private Method

        //private void LoadLoan()
        //{
        //    string Query = string.Format("select distinct " +
        //            "CONVERT(int,LOAN_CD) [LOAN_CD],  " +
        //            "LOAN_CD+' - '+LOAN_NAME [LOAN_NAME] from ST_LOANTYPE where ACTIVE_FLAG = '1' " +
        //            "union select null, 'เลือกทั้งหมด...' " +
        //            "order by LOAN_CD ASC");

        //    ddlLoan.DataSource = conn.ExcuteSQL(Query);
        //    ddlLoan.DataBind();
        //    ddlSubType.Items.Insert(0, new ListItem("เลือกทั้งหมด...", string.Empty));
        //    ddlMarketFrm.Items.Insert(0, new ListItem("เลือกทั้งหมด...", string.Empty));
        //    ddlMarketTo.Items.Insert(0, new ListItem("เลือกทั้งหมด...", string.Empty));
        //    ddlGSBSubLoan.Items.Insert(0, new ListItem("เลือกทั้งหมด...", string.Empty));
        //}

        private void LoadModel()
        {
            string Query = string.Format("select distinct " +
                            "CONVERT(int,MODEL_CD) [MODEL_CD],  " +
                            "MODEL_CD +' - '+ MODEL_NAME [MODEL_NAME] from LN_MODEL where ACTIVE_FLAG = '1' and MODEL_CD not in ('005','006') " +
                            "union select null, 'โปรดเลือก...' " +
                            "order by MODEL_CD ASC");

            ddlModel.DataSource = conn.ExcuteSQL(Query);
            ddlModel.DataBind();
            ddlSubType.Items.Insert(0, new ListItem("เลือกทั้งหมด...", string.Empty));
            ddlMarketFrm.Items.Insert(0, new ListItem("เลือกทั้งหมด...", string.Empty));
            ddlMarketTo.Items.Insert(0, new ListItem("เลือกทั้งหมด...", string.Empty));
        }

        protected void LoadLoan(object sender, EventArgs e)
        {
            string Query = "";

            if (ddlModel.SelectedItem.Value == "7")
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
            ddlMarketFrm.Items.Insert(0, new ListItem("เลือกทั้งหมด...", string.Empty));
            ddlMarketTo.Items.Insert(0, new ListItem("เลือกทั้งหมด...", string.Empty));
            ddlGSBSubLoan.Items.Insert(0, new ListItem("เลือกทั้งหมด...", string.Empty));
        }

        private void LoadRegion()
        {
            string Query = string.Format("select distinct " +
                    "CONVERT(int,REGION_CD) [REGION_CD], " +
                    "REGION_CD+' - '+REGION_NAME [REGION_NAME] from ST_GSBREGION where ACTIVE_FLAG = '1' " +
                    "union select null,'เลือกทั้งหมด...' " +
                    "order by REGION_CD ASC");

            ddlRegion.DataSource = conn.ExcuteSQL(Query);
            ddlRegion.DataBind();
        }

        private void LoadZone()
        {
            string Query = string.Format("select distinct " +
                    "CONVERT(int,ZONE_CD) [ZONE_CD], " +
                    "ZONE_CD+' - '+ZONE_NAME [ZONE_NAME] from ST_GSBZONE where ACTIVE_FLAG = '1' " +
                    "union select null,'เลือกทั้งหมด...' " +
                    "order by ZONE_CD ASC");

            ddlZone.DataSource = conn.ExcuteSQL(Query);
            ddlZone.DataBind();
        }

        private void LoadBranch()
        {
            string Query = string.Format("select distinct " +
                    "CONVERT(int,BRANCH_CD) [BRANCH_CD], " +
                    "BRANCH_CD+' - '+BRANCH_NAME [BRANCH_NAME] from ST_BRANCH where ACTIVE_FLAG = '1' " +
                    "union select null,'เลือกทั้งหมด...' " +
                    "order by BRANCH_CD ASC");

            ddlBranch.DataSource = conn.ExcuteSQL(Query);
            ddlBranch.DataBind();
        }

        private void LoadObjective()
        {
            string Query = string.Format("select distinct " +
                    "CONVERT(int,PURPOSE_CD) [PURPOSE_CD], " +
                    "PURPOSE_CD+' - '+PURPOSE_NAME [PURPOSE_NAME] from ST_PURPOSE where ACTIVE_FLAG = '1' " +
                    "union select null,'เลือกทั้งหมด...' " +
                    "order by PURPOSE_CD ASC");

            ddlObjective.DataSource = conn.ExcuteSQL(Query);
            ddlObjective.DataBind();
        }

        private void LoadGSBLoan()
        {
            string Query = string.Format("select distinct " +
                    "CONVERT(int,GSBPURPOSE_CD) [GSBPURPOSE_CD], " +
                    "GSBPURPOSE_CD+' - '+GPURPOSE_NAME [GPURPOSE_NAME] from ST_GSBPURPOSE where ACTIVE_FLAG = '1' " +
                    "union select null,'เลือกทั้งหมด...' " +
                    "order by GSBPURPOSE_CD ASC");

            ddlGSBLoan.DataSource = conn.ExcuteSQL(Query);
            ddlGSBLoan.DataBind();
 
        }

        private void LoadMainColumn()
        {
            string Query = string.Format("select * from ST_EXPORT where ACTIVE_FLAG = '1' order by C_ID ASC");

            if (ddlModel.SelectedValue == "7")
            {
                Query = string.Format("select * from ST_EXPORT_MYMO where ACTIVE_FLAG = '1' order by C_ID ASC");
            }
            else
            {
                Query = string.Format("select * from ST_EXPORT where ACTIVE_FLAG = '1' order by C_ID ASC");
            }

                MainColumn.DataSource = conn.ExcuteSQL(Query);
            MainColumn.DataBind();
        }

        private string CriteriaOfMember()
        {
            IFormatProvider format = new CultureInfo("en-GB");
            DateTime dateOpenLoan = DateTime.MinValue;
            DateTime dateCloseLoan = DateTime.MinValue;
            StringBuilder sqlQuery = new StringBuilder();

            if (ddlLoan.SelectedItem.Value != "")
                sqlQuery.Append(String.Format(" AND (LOAN_CD = '{0}')", ddlLoan.SelectedItem.Value));

            if (ddlSubType.SelectedItem.Value != "")
                sqlQuery.Append(String.Format(" AND (STYPE_CD = '{0}')", ddlSubType.SelectedItem.Value));

            if (ddlMarketFrm.SelectedItem.Value != "")
                sqlQuery.Append(String.Format(" AND (MARKET_CD >= '{0}')", ddlMarketFrm.SelectedItem.Value));

            if (ddlMarketTo.SelectedItem.Value != "")
                sqlQuery.Append(String.Format(" AND (MARKET_CD <= '{0}')", ddlMarketTo.SelectedItem.Value));

            //if (ddlRegion.SelectedItem.Value != "") //err
            //    sqlQuery.Append(String.Format(" AND (MARKET_CD = '{0}')", ddlRegion.SelectedItem.Value));

            //if (ddlZone.SelectedItem.Value != "") //err
            //    sqlQuery.Append(String.Format(" AND (MARKET_CD = '{0}')", ddlZone.SelectedItem.Value));

            if (ddlBranch.SelectedItem.Value != "")
                sqlQuery.Append(String.Format(" AND (BRANCH_CD = '{0}')", ddlBranch.SelectedItem.Value));

            if (ddlObjective.SelectedItem.Value != "")
                sqlQuery.Append(String.Format(" AND (PURPOSE_CD = '{0}')", ddlObjective.SelectedItem.Value));

            if (ddlGSBLoan.SelectedItem.Value != "")
                sqlQuery.Append(String.Format(" AND (GSBPURPOSE_CD = '{0}')", ddlGSBLoan.SelectedItem.Value));

            if (ddlGSBSubLoan.SelectedItem.Value != "")
                sqlQuery.Append(String.Format(" AND (CONSUMPTION_CD = '{0}')", ddlGSBSubLoan.SelectedItem.Value));

            if (DateTime.TryParse(txtOpenDate.Text, format, DateTimeStyles.None, out dateOpenLoan) &&
                DateTime.TryParse(txtCloseDate.Text, format, DateTimeStyles.None, out dateCloseLoan))
            {
                if (txtOpenDate.Text != null && txtCloseDate.Text != null && DateTime.Compare(dateOpenLoan, dateCloseLoan) <= 0)
                {
                    sqlQuery.Append(String.Format(" AND (APP_DATE BETWEEN '{0}' AND '{1}')", dateOpenLoan.Year.ToString() + "-" + dateOpenLoan.Month.ToString() + "-" +
                                dateOpenLoan.Day.ToString(), dateCloseLoan.Year.ToString() + "-" + dateCloseLoan.Month.ToString() + "-" + dateCloseLoan.Day.ToString()));
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScript", "appPop('วันที่เปิดใบคำขอสินเชื่อไม่ถูกต้อง?','วันที่เปิดเริ่มต้น น้อยกว่าหรือเท่ากับ วันที่เปิดสุดท้าย')", true);
                }
            }

            if (txtOpenApp.Text != "" && txtCloseApp.Text != "")
                sqlQuery.Append(String.Format(" AND (APP_NO between '{0}' and '{1}')", txtOpenApp.Text, txtCloseApp.Text));

            if (txtOpenLoan.Text != "" && txtCloseLoan.Text != "")
                sqlQuery.Append(String.Format(" AND (LOAN_AMOUNT between '{0}' and '{1}')", txtOpenLoan.Text, txtCloseLoan.Text));

            if (txtOpenMoney.Text != "" && txtOpenMoney.Text != "")
                sqlQuery.Append(String.Format(" AND (APPRAMT between '{0}' and '{1}')", txtOpenMoney.Text, txtOpenMoney.Text));

            if (chkSEQ.Checked )
                sqlQuery.Append(String.Format(" AND LN_APP.APP_SEQ = (select MAX(APP_SEQ) from LN_APP AS LN_APP2 where LN_APP.APP_NO = LN_APP2.APP_NO)"));

            return sqlQuery.ToString();
        }

        private string CriteriaOfMemberMymo()
        {
            IFormatProvider format = new CultureInfo("en-GB");
            DateTime dateOpenLoan = DateTime.MinValue;
            DateTime dateCloseLoan = DateTime.MinValue;
            StringBuilder sqlQuery = new StringBuilder();

            if (ddlLoan.SelectedItem.Value != "")
                sqlQuery.Append(String.Format(" AND (LOAN_CD = '{0}')", ddlLoan.SelectedItem.Value));

            if (ddlSubType.SelectedItem.Value != "")
                sqlQuery.Append(String.Format(" AND (STYPE_CD = '{0}')", ddlSubType.SelectedItem.Value));

            if (ddlMarketFrm.SelectedItem.Value != "")
                sqlQuery.Append(String.Format(" AND (MARKET_CD >= '{0}')", ddlMarketFrm.SelectedItem.Value));

            if (ddlMarketTo.SelectedItem.Value != "")
                sqlQuery.Append(String.Format(" AND (MARKET_CD <= '{0}')", ddlMarketTo.SelectedItem.Value));

            //if (ddlRegion.SelectedItem.Value != "") //err
            //    sqlQuery.Append(String.Format(" AND (MARKET_CD = '{0}')", ddlRegion.SelectedItem.Value));

            //if (ddlZone.SelectedItem.Value != "") //err
            //    sqlQuery.Append(String.Format(" AND (MARKET_CD = '{0}')", ddlZone.SelectedItem.Value));

            if (ddlBranch.SelectedItem.Value != "")
                sqlQuery.Append(String.Format(" AND (BRANCH_CD = '{0}')", ddlBranch.SelectedItem.Value));

            if (ddlObjective.SelectedItem.Value != "")
                sqlQuery.Append(String.Format(" AND (PURPOSE_CD = '{0}')", ddlObjective.SelectedItem.Value));

            if (ddlGSBLoan.SelectedItem.Value != "")
                sqlQuery.Append(String.Format(" AND (GSBPURPOSE_CD = '{0}')", ddlGSBLoan.SelectedItem.Value));

            if (ddlGSBSubLoan.SelectedItem.Value != "")
                sqlQuery.Append(String.Format(" AND (CONSUMPTION_CD = '{0}')", ddlGSBSubLoan.SelectedItem.Value));

            if (DateTime.TryParse(txtOpenDate.Text, format, DateTimeStyles.None, out dateOpenLoan) &&
                DateTime.TryParse(txtCloseDate.Text, format, DateTimeStyles.None, out dateCloseLoan))
            {
                if (txtOpenDate.Text != null && txtCloseDate.Text != null && DateTime.Compare(dateOpenLoan, dateCloseLoan) <= 0)
                {
                    sqlQuery.Append(String.Format(" AND (APP_DATE BETWEEN '{0}' AND '{1}')", (dateOpenLoan.Year-543).ToString() + "-" + dateOpenLoan.Month.ToString() + "-" +
                                dateOpenLoan.Day.ToString(), (dateCloseLoan.Year-543).ToString() + "-" + dateCloseLoan.Month.ToString() + "-" + dateCloseLoan.Day.ToString()));
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScript", "appPop('วันที่เปิดใบคำขอสินเชื่อไม่ถูกต้อง?','วันที่เปิดเริ่มต้น น้อยกว่าหรือเท่ากับ วันที่เปิดสุดท้าย')", true);
                }
            }

            if (txtOpenApp.Text != "" && txtCloseApp.Text != "")
                sqlQuery.Append(String.Format(" AND (APP_NO between '{0}' and '{1}')", txtOpenApp.Text, txtCloseApp.Text));

            if (txtOpenLoan.Text != "" && txtCloseLoan.Text != "")
                sqlQuery.Append(String.Format(" AND (LOAN_AMOUNT between '{0}' and '{1}')", txtOpenLoan.Text, txtCloseLoan.Text));

            if (txtOpenMoney.Text != "" && txtOpenMoney.Text != "")
                sqlQuery.Append(String.Format(" AND (APPRAMT between '{0}' and '{1}')", txtOpenMoney.Text, txtOpenMoney.Text));

            if (chkSEQ.Checked)
                sqlQuery.Append(String.Format(" AND MYMO_LN_APP.APP_SEQ = (select MAX(APP_SEQ) from MYMO_LN_APP AS LN_APP2 where MYMO_LN_APP.APP_NO = LN_APP2.APP_NO)"));

            return sqlQuery.ToString();
        }

        private string ConvertFieldToQuery(string strSQL)
        {
            DataTable dtExport = null;

            if (Session["Export"] == null)
            {
                if (ddlModel.SelectedValue == "7")
                {
                    dtExport = conn.ExcuteSQL("select ISNULL(C_QUERY,C_NAME), DESC_TH, C_NAME from ST_EXPORT_MYMO where ACTIVE_FLAG = '1' order by C_ID ASC");
                }
                else
                {
                    dtExport = conn.ExcuteSQL("select ISNULL(C_QUERY,C_NAME), DESC_TH, C_NAME from ST_EXPORT where ACTIVE_FLAG = '1' order by C_ID ASC");

                }
                Session.Add("Export", dtExport);
            }
            else
                dtExport = Session["Export"] as DataTable;

            DataRow[] foundRows = dtExport.Select(string.Format("C_NAME = '{0}'", strSQL));
            return string.Format("{0} [{1}]", foundRows[0][0], foundRows[0][1]);
        }

        private void DataTableToExcel(string strSQL)
        {
            string Query = "";

            if (ddlModel.SelectedValue == "7")
            {
                Query = string.Format("select distinct APP_NO [เลขที่ใบคำขอ], " +
 "MYMO_LN_APP.APP_SEQ [ลำดับการบันทึก], " +
 "substring(CONVERT(VARCHAR,APP_DATE,103),1,6) + convert(varchar,(convert(decimal,substring(CONVERT(VARCHAR,APP_DATE,103),7,4)) + 543)) [วันที่เปิดใบคำขอ], " +
 "{0} from MYMO_LN_APP  , MYMO_LN_APPCIF " +
 "where MYMO_LN_APPCIF.CBS_APP_NO = MYMO_LN_APP.APP_NO AND MYMO_LN_APPCIF.APP_SEQ = MYMO_LN_APP.APP_SEQ " +
 "{1} order by [วันที่เปิดใบคำขอ] desc", strSQL, CriteriaOfMemberMymo());

            }
            else
            {
                Query = string.Format("select distinct APP_NO [เลขที่ใบคำขอ], " +
                 "LN_APP.APP_SEQ [ลำดับการบันทึก], " +
                 "substring(CONVERT(VARCHAR,APP_DATE,103),1,6) + convert(varchar,(convert(decimal,substring(CONVERT(VARCHAR,APP_DATE,103),7,4)) + 543)) [วันที่เปิดใบคำขอ], " +
                 "{0} from LN_APP  , LN_APPCIF , LN_CHAR " +
                 "where LN_APPCIF.CBS_APP_NO = LN_APP.APP_NO AND LN_APPCIF.APP_SEQ = LN_APP.APP_SEQ " +
                 "AND LN_APP.APP_NO = LN_CHAR.CBS_APP_NO AND LN_APP.APP_SEQ = LN_CHAR.APP_SEQ AND LN_APPCIF.CBS_CIFNO = LN_CHAR.CBS_CIFNO {1} order by [วันที่เปิดใบคำขอ] desc", strSQL, CriteriaOfMember());

            }


            //SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["GSBSQLServer"].ToString());
            //string tmesend = DateTime.Now.Year.ToString("0000") + DateTime.Now.ToString("-MM-dd HH:mm:ss.") + DateTime.Now.Millisecond.ToString();
            //string sqlStatementdx = "insert into s_log_import values('BATCH_MASTERs',convert(datetime,'" + tmestart.ToString() + "',121),convert(datetime,'" + tmesend.ToString() + "',121),0,'DONE','DONE:" + Localfile.ToString() + "','002')";
            //con.Open();
            //SqlCommand cmddlx = new SqlCommand(sqlStatementdx, con);
            //cmddlx.ExecuteNonQuery();
            //con.Close();

            string filename = string.Format("GSB-Credit-Scoring_(Date_{0}-Time_{1}).csv", DateTime.Now.ToString("dd-MM-yyyy"), DateTime.Now.ToString("HH-mm-ss"));

            Session["filename"] = filename;

            StringWriter tw = new StringWriter();
            //HtmlTextWriter hw = new HtmlTextWriter(tw);
            DataGrid dgGrid = new DataGrid();
            DataTable dttb = new DataTable();
            //Server.ScriptTimeout = 10000;
            //Session.Timeout = 10000;
            dttb = conn.ExcuteSQL(Query);

            if (dttb == null)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScript", "appPop('GSB','เกินระยะเวลาที่กำหนด กรุณาติดต่อเจ้าหน้าที่ผู้ดูแลระบบ <br /> (หน่วยระบบงานสินเชื่อเฉพาะด้าน ติดต่อ 880166 หรือ 880452)')", true);
                return;
            }

            //tw.Encoding.EncodingName = UTF8Encoding.UTF8;
            //dgGrid.DataSource = conn.ExcuteSQL(Query); ;
            //dgGrid.DataBind();
            //StreamWriter sw;

            int iColCount = new int() ;
            iColCount =  dttb.Columns.Count;
            for (int i = 0; i < iColCount; i++)
            {
                tw.Write(dttb.Columns[i]);
                if (i < iColCount - 1)
                {
                    tw.Write(",");
                }
            }

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
                    if (i < iColCount - 1)
                    {
                        tw.Write(",");
                    }
                }
                tw.Write(tw.NewLine);
            }

            Session["str"] = tw.ToString();

            //sw.Close();

            //            string strExport = "";            
            //            List<string> lstFields = new List<string>();
            //            foreach (DataGridColumn dgcol in dgGrid.Columns)                    
            //            {
            //                lstFields.Add(dgcol.HeaderText.ToString());                
            //                //BuildStringOfRow(strBuilder, lstFields, strFormat);            
            //            }
            //            //Loop through all the columns in DataGridView to Set the             
            //            //Column Heading            
            //            //foreach (DataGridItem dc in dgGrid.Items )            
            //            //{                
            //            //    strExport += dc. + "   ";             
            //            //}            
            //            //strExport = strExport.Substring(0, strExport.Length - 3) + Environment.NewLine.ToString();            
            //            //Loop through all the row and append the value with 3 spaces            
            //            foreach (Object data in dgGrid.Items )            
            //            {                
            //                foreach (DataGridColumn dc in dgGrid.Columns )                
            //                {                  
            //                    //if ( dc is 
            //                    //dc.ToString();
            //                    //if (dc.Value != null)                    
            //                    //{                        
            //                        strExport += dc.ToString() + ",";                    
            //                    //}                
            //                }                
            //                strExport += Environment.NewLine.ToString();            
            //            }            
            //            strExport = strExport.Substring(0, strExport.Length - 3) + Environment.NewLine.ToString();
            //            //StringBuilder strbld = new StringBuilder();

            //            ////System.Collections.IList source = (dgGrid.co as System.Collections.IList);
            //            ////if (source == null)
            //            ////    return "";

            //            //foreach (Object data in dgGrid.DataMember )
            //            //{
            //            //    foreach (DataGridColumn col in dgGrid.Columns )
            //            //    {
            //            //        if (col is datag DataGridBoundColumn)
            //            //        {
            //            //            binding = (col as DataGridBoundColumn).Binding;
            //            //            colPath = binding.Path.Path;
            //            //            propInfo = data.GetType().GetProperty(colPath);
            //            //            if (propInfo != null)
            //            //            {
            //            //                strBuilder.Append(propInfo.GetValue(data, null).ToString());
            //            //                strBuilder.Append(",");
            //            //            }
            //            //        }

            //            //    }
            //            //    strBuilder.Append("\r\n");
            //            //}


            //            //return strBuilder.ToString();


            //           // dgGrid.RenderControl(hw);
            //            //tw.Write = strExport.ToString();
            //           Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            //            Response.ContentType = "application/vnd.ms-excel";
            // Response.ContentType = "application/text";


            //UpdatePanel.Triggers.RemoveAt(4);

            //UpdatePanel.ChildrenAsTriggers = false;


            //ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
            //scriptManager.
            //scriptManager.RegisterAsyncPostBackControl(this.btnExcel);

            //UpdatePanel.UpdateMode = UpdatePanelUpdateMode.Conditional;

            //Button btnExcel = UpdatePanel.FindControl("btnExcel") as Button;
            //ScriptManager.GetCurrent(this).RegisterPostBackControl(btnExcel);


            //PostBackTrigger pTrigger = new System.Web.UI.PostBackTrigger() { ControlID = btnExcel.ID };

            //UpdatePanel.Triggers.Add(pTrigger);

            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "aa", "window.open('ExportExcel.aspx');", true);

            //Response.Redirect("~/Default.aspx");

            //            Response.ContentType = "text/rtf; charset=UTF-8";
            ////            Response.Charset = "UTF-8";
            ////            Response.ContentEncoding = System.Text.Encoding.UTF8;

            //            Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename + "");
            //            Response.AppendHeader("Content-Encoding", "UTF8");
            //            this.EnableViewState = false;
            //            //Response.Write(tw.ToString());
            //            Response.BinaryWrite(System.Text.Encoding.UTF8.GetPreamble());

            //            Response.Write(tw.ToString());
            //            //Response.End();

            //Response.Flush();
            //Response.SuppressContent = true;
            //ApplicationInstance.CompleteRequest();

            //UpdatePanel.Triggers.RemoveAt(4);

            //scriptManager = ScriptManager.GetCurrent(this.Page);
            //scriptManager.RegisterAsyncPostBackControl(this.btnExcel);

            //AsyncPostBackTrigger trigger = new AsyncPostBackTrigger();

            //trigger.ControlID = btnExcel.ID;

            //trigger.EventName = "Click";

            //UpdatePanel.Triggers.Add(trigger);

        }

        #endregion Private Method

        #region Protected Method

        protected void LoadSubType(object sender, EventArgs e)
        {
            string Query = "";

            if (ddlModel.SelectedItem.Value == "7")
            {
                Query = string.Format("select distinct " +
                                         "STYPE_CD [STYPE_CD], " +
                                         "STYPE_CD+' - '+STYPER_NAME [STYPE_NAME] from ST_LOANSTYPE a left join [dbo].[ST_MKTDFTMYMO] b on a.STYPE_CD = b.subtype where ACTIVE_FLAG = '1' and LOAN_CD = '{0}' and b.subtype is not null " +
                                         "union select null, 'เลือกทั้งหมด...' " +
                                         "order by STYPE_CD ASC", ddlLoan.SelectedItem.Value);
            }
            else
            {
                Query = string.Format("select distinct " +
                         "STYPE_CD [STYPE_CD], " +
                         "STYPE_CD+' - '+STYPER_NAME [STYPE_NAME] from ST_LOANSTYPE a left join [dbo].[ST_MKTDFTMYMO] b on a.STYPE_CD = b.subtype where ACTIVE_FLAG = '1' and LOAN_CD = '{0}' and b.subtype is null " +
                         "union select null, 'เลือกทั้งหมด...' " +
                         "order by STYPE_CD ASC", ddlLoan.SelectedItem.Value);
            }




            ddlSubType.Items.Clear();
            ddlSubType.DataSource = conn.ExcuteSQL(Query);
            ddlSubType.DataBind();
            ddlSubType.Enabled = true;

            ddlMarketFrm.Enabled = false;
            ddlMarketFrm.Items.Clear();
            ddlMarketFrm.Items.Insert(0, new ListItem("เลือกทั้งหมด...", string.Empty));
            ddlMarketTo.Enabled = false;
            ddlMarketTo.Items.Clear();
            ddlMarketTo.Items.Insert(0, new ListItem("เลือกทั้งหมด...", string.Empty));

            string Query2 = string.Format("select distinct " +
                                         "CONVERT(int,CONSUMPTION_CD) [CONSUMPTION_CD], " +
                                         "CONSUMPTION_CD + ' - '+CONSUMPTION_NAME [CONSUMPTION_NAME] from ST_CONSUMPTION where ACTIVE_FLAG = '1' and LOAN_CD = '{0}' " +
                                         "union select null, 'เลือกทั้งหมด...' " +
                                         "order by CONSUMPTION_CD ASC", ddlLoan.SelectedItem.Value);

            ddlGSBSubLoan.Items.Clear();
            ddlGSBSubLoan.DataSource = conn.ExcuteSQL(Query2);
            ddlGSBSubLoan.DataBind();
            ddlGSBSubLoan.Enabled = true;
        }

        protected void LoadMarket(object sender, EventArgs e)
        {
            string Query = "";

            if (ddlModel.SelectedItem.Value == "7")
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
            ddlMarketTo.Items.Clear();
            ddlMarketTo.DataSource = conn.ExcuteSQL(Query);
            ddlMarketTo.DataBind();
            ddlMarketTo.Enabled = true;


        }



        protected void btnGOStep2_Click(object sender, EventArgs e)
        {
            var verDate = true;
            IFormatProvider format = new CultureInfo("en-GB");
            DateTime dateOpenLoan = DateTime.MinValue;
            DateTime dateCloseLoan = DateTime.MinValue;

            if (ddlModel.SelectedItem.Value == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScript", "appPop('โปรดเลือกตัวเลือก','กรุณาเลือกเงื่อนไขการค้าหาให้ครบถ้วน')", true);
                return;
            }

            if (ddlLoan.SelectedItem.Value == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScript", "appPop('โปรดเลือกตัวเลือก','กรุณาเลือกเงื่อนไขการค้าหาให้ครบถ้วน')", true);
                return;
            }

            if (ddlSubType.SelectedItem.Value == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScript", "appPop('โปรดเลือกตัวเลือก','กรุณาเลือกเงื่อนไขการค้าหาให้ครบถ้วน')", true);
                return;
            }

            if (txtOpenDate.Text == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScript", "appPop('โปรดเลือกตัวเลือก','กรุณาเลือกเงื่อนไขการค้าหาให้ครบถ้วน')", true);
                return;
            }

            if (txtCloseDate.Text == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScript", "appPop('โปรดเลือกตัวเลือก','กรุณาเลือกเงื่อนไขการค้าหาให้ครบถ้วน')", true);
                return;
            }

            if (ddlMarketFrm.SelectedIndex <= 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScript", "appPop('โปรดเลือกตัวเลือก','กรุณาเลือกเงื่อนไขการค้าหาให้ครบถ้วน')", true);
                return;
            }
            if (ddlMarketTo.SelectedIndex <= 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScript", "appPop('โปรดเลือกตัวเลือก','กรุณาเลือกเงื่อนไขการค้าหาให้ครบถ้วน')", true);
                return;
            }


            if (DateTime.TryParse(txtOpenDate.Text, format, DateTimeStyles.None, out dateOpenLoan) &&
                DateTime.TryParse(txtCloseDate.Text, format, DateTimeStyles.None, out dateCloseLoan))
            {
                if (txtOpenDate.Text != null && txtCloseDate.Text != null && DateTime.Compare(dateOpenLoan, dateCloseLoan) > 0)
                {
                    verDate = false;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScript", "appPop('วันที่เปิดใบคำขอสินเชื่อไม่ถูกต้อง?','วันที่เปิดเริ่มต้น น้อยกว่าหรือเท่ากับ วันที่เปิดสุดท้าย')", true);
                }
                else
                {
                    verDate = true;
                }
            }

            if (verDate)
            {
                STEP1.Visible = false;
                STEP2.Visible = true;

                LoadMainColumn();
            }

        }

        protected void btnGOStep3_Click(object sender, EventArgs e)
        {
            STEP2.Visible = false;
            STEP3.Visible = true;
        }

        protected void btnBKStep1_Click(object sender, EventArgs e)
        {
            STEP1.Visible = true;
            STEP2.Visible = false;
        }

        protected void btnBKStep2_Click(object sender, EventArgs e)
        {
            STEP2.Visible = true;
            STEP3.Visible = false;
        }

        protected void btmMoveUp_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < SelectedColumn.Items.Count; i++)
                if (SelectedColumn.Items[i].Selected)
                    if (i > 0 && !SelectedColumn.Items[i - 1].Selected)
                    {
                        ListItem bottom = SelectedColumn.Items[i];
                        SelectedColumn.Items.Remove(bottom);
                        SelectedColumn.Items.Insert(i - 1, bottom);
                        SelectedColumn.Items[i - 1].Selected = true;
                    }
        }

        protected void btmMoveDown_Click(object sender, EventArgs e)
        {
            int startindex = SelectedColumn.Items.Count - 1;
            for (int i = startindex; i > -1; i--)
                if (SelectedColumn.Items[i].Selected)
                    if (i < startindex && !SelectedColumn.Items[i + 1].Selected)
                    {
                        ListItem bottom = SelectedColumn.Items[i];
                        SelectedColumn.Items.Remove(bottom);
                        SelectedColumn.Items.Insert(i + 1, bottom);
                        SelectedColumn.Items[i + 1].Selected = true;
                    }
        }

        protected void btmMoveRight_Click(object sender, EventArgs e)
        {
            ArrayList arrList = new ArrayList();
            if (MainColumn.SelectedIndex >= 0)
                for (int i = 0; i < MainColumn.Items.Count; i++)
                    if (MainColumn.Items[i].Selected)
                        if (!arrList.Contains(MainColumn.Items[i]))
                            arrList.Add(MainColumn.Items[i]);

            for (int i = 0; i < arrList.Count; i++)
            {
                if (!SelectedColumn.Items.Contains(((ListItem)arrList[i])))
                    SelectedColumn.Items.Add(((ListItem)arrList[i]));
                MainColumn.Items.Remove(((ListItem)arrList[i]));
            }
            SelectedColumn.SelectedIndex = -1;
        }

        protected void btmMoveLeft_Click(object sender, EventArgs e)
        {
            ArrayList arrList = new ArrayList();
            if (SelectedColumn.SelectedIndex >= 0)
                for (int i = 0; i < SelectedColumn.Items.Count; i++)
                    if (SelectedColumn.Items[i].Selected)
                        if (!arrList.Contains(SelectedColumn.Items[i]))
                            arrList.Add(SelectedColumn.Items[i]);

            for (int i = 0; i < arrList.Count; i++)
            {
                if (!MainColumn.Items.Contains(((ListItem)arrList[i])))
                    MainColumn.Items.Add(((ListItem)arrList[i]));
                SelectedColumn.Items.Remove(((ListItem)arrList[i]));
            }
            SelectedColumn.SelectedIndex = -1;
        }

        protected void btmMoveAllRight_Click(object sender, EventArgs e)
        {
            for (int i = MainColumn.Items.Count; i > 0; i--)
                {
                    SelectedColumn.Items.Insert(SelectedColumn.Items.Count, new ListItem(MainColumn.Items[0].Text, MainColumn.Items[0].Value));
                    MainColumn.Items.RemoveAt(0);
                }
            MainColumn.SelectedIndex = -1;
            SelectedColumn.SelectedIndex = -1;
        }

        protected void btmMoveAllLeft_Click(object sender, EventArgs e)
        {
            for (int i = SelectedColumn.Items.Count; i > 0; i--)
            {
                MainColumn.Items.Insert(MainColumn.Items.Count, new ListItem(SelectedColumn.Items[0].Text, SelectedColumn.Items[0].Value));
                SelectedColumn.Items.RemoveAt(0);
            }
            MainColumn.SelectedIndex = -1;
            SelectedColumn.SelectedIndex = -1;
        }

        protected void btnExcel_Click(object sender, EventArgs e)
        {
            StringBuilder selColumn = new StringBuilder();
            for (int i = 0; i < SelectedColumn.Items.Count; i++)
            {
                selColumn.Append(ConvertFieldToQuery(SelectedColumn.Items[i].Value));
                if (i < (SelectedColumn.Items.Count - 1))
                    selColumn.Append(@", ");
            }

            DataTableToExcel(selColumn.ToString());
        }

        #endregion Protected Method
    }
}