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

namespace GSB.Report.FrontEnd
{
    public partial class PopulationStabilityReport : System.Web.UI.Page
    {
        //int TotalDev = 0;
        //decimal TotalDevPercent = 0;
        //int TotalActual = 0;
        //decimal TotalActualPercent = 0;
        //decimal TotalPopulationStabilityIndex = 0;


        SQLToDataTable conn = new SQLToDataTable();
        string appQuery1;
        #region Page_Load

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                LoadYearMonth();
                LoadModel();
                //LoadModelVersion();
            }
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

        private void LoadYearMonth()
        {
            int ychis,ybud;

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
                ddlOpenDateYear.Items.Insert(i, new ListItem((ybud-i).ToString(), (ychis-i).ToString()));
                ddlOpenDateYearend.Items.Insert(i, new ListItem((ybud-i).ToString(), (ychis-i).ToString()));
            }

            ddlOpenDateMonth.Items.Clear();
            ddlOpenDateMonthend.Items.Clear();

            ddlOpenDateMonth.DataSource = GetMonth();
            ddlOpenDateMonth.DataBind();
            ddlOpenDateMonthend.DataSource = GetMonth();
            ddlOpenDateMonthend.DataBind();

        }


        private void LoadModel()
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
            ddlSubType.Items.Insert(0, new ListItem("โปรดเลือก...", string.Empty));

        }

        //private void LoadLoan()
        //{
        //    string Query = string.Format("select distinct " +
        //            "CONVERT(int,LOAN_CD) [LOAN_CD],  " +
        //            "LOAN_CD+' - '+LOAN_NAME [LOAN_NAME] from ST_LOANTYPE where ACTIVE_FLAG = '1' " +
        //            "union select null, 'โปรดเลือก...' " +
        //            "order by LOAN_CD ASC");

        //    ddlLoan.DataSource = conn.ExcuteSQL(Query);
        //    ddlLoan.DataBind();
        //    ddlSubType.Items.Insert(0, new ListItem("โปรดเลือก...", string.Empty));
        //}


        private void DataTableToExcel(string strSQL)
        {



            string Query = string.Format("{0}", strSQL);


            string filename = string.Format("GSB-CS_PopulationStabilityReport_(Date_{0}-Time_{1}).xls", DateTime.Now.ToString("dd-MM-yyyy"), DateTime.Now.ToString("HH-mm-ss"));
            StringWriter tw = new StringWriter();

            DataGrid dgGrid = new DataGrid();
            DataTable dttb = new DataTable();

            dttb = conn.ExcuteSQL(Query);

            tw.Write(string.Format("Population Stability Report"));
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
            //tw.Write(string.Format("วันที่เปิดใบคำขอสินเชื่อ :| {0} ถึง {1} ", DateTime.Parse(txtOpenDate.Text.ToString()).ToString("dd-MM-yyyy"), DateTime.Parse(txtCloseDate.Text.ToString()).ToString("dd-MM-yyyy")));
            //tw.Write(tw.NewLine);
            tw.Write(string.Format("Export Date :| {0} : {1} ", DateTime.Now.ToString("dd-MM-yyyy"), DateTime.Now.ToString("HH-mm-ss")));
            tw.Write(tw.NewLine);
            tw.Write("ผู้พิมพ์ " + ExportUtility.getCurrentUserId);
            tw.Write(tw.NewLine);

            int iColCount = new int();
            iColCount = dttb.Columns.Count;
            
            tw.Write("Score Range|DEV|%DEV|Actual|%Actual|%Change|Ratio|WOE|Index|%Approve|%Reject");
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
                        tw.Write("  ");
                    }
                    if (i < iColCount - 1)
                    {
                        tw.Write("|");
                    }
                }
                tw.Write(tw.NewLine);
            }

            tw.WriteLine("");
            tw.WriteLine("");
            tw.WriteLine("");

            //tw.Write("Population Stability Index");
            //tw.Write(lblSumIndex.Text);
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

            ddlMarketFrm.Enabled = false;
            ddlMarketFrm.Items.Clear();
            ddlMarketFrm.Items.Insert(0, new ListItem("โปรดเลือก...", string.Empty));
            ddlMarketTo.Enabled = false;
            ddlMarketTo.Items.Clear();
            ddlMarketTo.Items.Insert(0, new ListItem("โปรดเลือก...", string.Empty));
        }
        protected void LoadVersion(object sender, EventArgs e)
        {
            string sql = "";
            sql = string.Format("select MODEL_NAME+'_V1' as [Model_V_NAME] from LN_MODEL where ACTIVE_FLAG = '1' and");
            sql += "  [MODEL_CD]=" + ddlModel.SelectedValue + " ";


            //ddlModelVersion.DataSource = conn.ExcuteSQL(sql);
            //ddlModelVersion.DataTextField = "Model_V_NAME";
            //ddlModelVersion.DataValueField = "Model_V_NAME";
            //ddlModelVersion.DataBind();

            //oadLoan();

            //    string Query = string.Format("select distinct " + "CONVERT(int,LOAN_CD) [LOAN_CD],  " +
            //"LOAN_CD+' - '+LOAN_NAME [LOAN_NAME] from ST_LOANTYPE a,ST_SCORERANGE b where ACTIVE_FLAG = '1' and a.loan_cd=b.ltype " +
            //" and [MODEL_CD]=" + ddlModel.SelectedValue + " " +
            //"order by LOAN_CD ASC");

            //    ddlLoan.DataSource = conn.ExcuteSQL(Query);
            //    ddlLoan.DataBind();
            //    ddlLoan.Items.Insert(0, new ListItem("โปรดเลือก...", string.Empty));
        }
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
            //DateTime start = new DateTime();
            //DateTime end = new DateTime();
            IFormatProvider format = new CultureInfo("en-GB");
            DateTime dateOpenLoan = DateTime.MinValue;
            DateTime dateCloseLoan = DateTime.MinValue;
            StringBuilder sqlQuery = new StringBuilder();
            string mktqrf = "";
            string mktqrt = "";

            if (ddlLoan.SelectedIndex <= 0 || ddlSubType.SelectedIndex <= 0 || ddlMarketFrm.SelectedIndex <= 0 || ddlMarketTo.SelectedIndex <= 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScript", "appPop('โปรดเลือกตัวเลือก','กรุณาเลือกเงื่อนไขการค้นหาให้ครบถ้วน ')", true);
                return;
            }

            if (ddlLoan.SelectedItem.Value != "")
                sqlQuery.Append(String.Format(" AND (LOAN_CD = '{0}')", ddlLoan.SelectedItem.Value));

            if (ddlSubType.SelectedItem.Value != "")
                sqlQuery.Append(String.Format(" AND (STYPE_CD = '{0}')", ddlSubType.SelectedItem.Value));
            //if (!(txtOpenDate.Text == ""))
            //{
            //    start = DateTime.Parse((txtOpenDate.Text.ToString().Substring(0, 6) + (Convert.ToInt32(txtOpenDate.Text.Substring(6, 4).ToString()) - 543).ToString()));
            //}
            //else
            //{
            //    start = DateTime.Now;
            //}
            //if (!(txtCloseDate.Text == ""))
            //{
            //    end = DateTime.Parse(txtCloseDate.Text.ToString().Substring(0, 6) + (Convert.ToInt32(txtCloseDate.Text.Substring(6, 4).ToString()) - 543).ToString() + " 23:59:59.999");
            //}
            //else
            //{
            //    end = DateTime.Now;

            //}

            //if (ddlMarketFrm.SelectedIndex <= 0)
            //{
            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScript", "appPop('โปรดเลือกตัวเลือก','โปรดเลือก  Market Code ก่อน')", true);
            //    return;                
            //}
            //if (ddlMarketTo.SelectedIndex <= 0)
            //{
            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScript", "appPop('โปรดเลือกตัวเลือก','โปรดเลือกสินเชื่อย่อย ถึง อย่างน้อย 1 ค่า')", true);
            //    return;
            //}

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

            int monthstart =  Convert.ToInt16(ddlOpenDateYear.SelectedValue)*100+ ddlOpenDateMonth.SelectedIndex;
            int monthend = Convert.ToInt16(ddlOpenDateYearend.SelectedValue) * 100 + ddlOpenDateMonthend.SelectedIndex;

            if (monthstart < 0  || monthend < 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScript", "appPop('วันที่เปิดใบคำขอสินเชื่อไม่ถูกต้อง?','เลือกวันที่เปิดใบคำขอไม่ถูกต้อง')", true);
                return;
            }
            else if (monthstart > monthend)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScript", "appPop('วันที่เปิดใบคำขอสินเชื่อไม่ถูกต้อง?','เลือกวันที่เปิดใบคำขอไม่ถูกต้อง')", true);
                return;
            }
            else
            {
                if (ddlModel.SelectedIndex <= 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScript", "appPop('กรุณาเลือกข้อมูลให้ถูกต้อง')", true);
                }
                else
                {
                    if (ddlLoan.SelectedIndex <= 0)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScript", "appPop('กรุณาเลือกข้อมูลให้ถูกต้อง')", true);
                    }
                    else
                    {
                        if (ddlSubType.SelectedIndex <= 0)
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

                                        appQuery1 = string.Format("	 select scrange as SCRRG,case when b.cntdev IS null then 0 else b.cntdev end as DEV  	" +
                "	 ,case when b.tot is null then 0 else convert(decimal(15,3)	" +
                "	 ,(((case when b.cntdev IS null then 0 else b.cntdev end) / convert(decimal(15,4),b.tot) ) * 1000000.00)) end as PDEV     	" +
                "	 into #temp	" +
                "	 from 	" +
                "	 ( 	" +
                "	 select range_name as scrange 	" +
                "	 from st_scorernglst 	" +
                "	 where range_cd =   (select range_cd from ST_SCORERANGE where ltype= '{1}' and lstype = '{2}')	" +
                "	 ) defrng  	" +
                "	  left join  	" +
                "	 (	" +
                "	 select case when Devcnt.tot IS null then 0 else Devcnt.tot end as tot ,Devcnt.scrrange	" +
                "	 , case when COUNT(Devcnt.tot) is null then 0 else COUNT(Devcnt.tot) end as cntdev   	" +
                "	 from (select case when (SCORE - scr) < 0 then convert(varchar,(scr + ((((SCORE + 1) - scr)/rint) -1 ) * rint)) + '-' + convert(varchar,((scr + ((((SCORE + 1 ) - scr)/rint)-1 ) * rint) + rint - 1))  when (SCORE - scr) >= 0 then convert(varchar,(scr + ((SCORE - scr)/rint) * rint)) + '-' + convert(varchar,((scr + ((SCORE - scr)/rint) * rint) + rint - 1))   end as scrrange ,SCORE,(select COUNT(SCORE) 	" +
                "	 from [LN_DEV] where MODEL_CD = '{0}') as tot 	" +
                "	 from 	" +
                "	 ( 	" +
                "	 SELECT [SCORE] 	" +
                "	 ,  (select cut_off from ST_SCORERANGE y where  y.ltype= '{1}' and y.lstype = '{2}' ) as scr	" +
                "	 ,   (select rint from ST_SCORERANGE y where  y.ltype= '{1}' and y.lstype = '{2}' ) as rint   	" +
                "	 , (select COUNT(score) as scdev from [LN_DEV] where MODEL_CD = '{0}') as devcnt   	" +
                "	 FROM [LN_DEV] where MODEL_CD = '{0}' ) devdat 	" +
                "	 ) Devcnt   group by Devcnt.tot,Devcnt.scrrange 	" +
                "	 ) b  on defrng.scrange = b.scrrange   	" +
                "	 left join 	" +
                "	 (	" +
                "	 select case when gsbcnt.lcnt is null then 0 else gsbcnt.lcnt end as lcnt,gsbcnt.scrdif	" +
                "	 ,  case when COUNT(gsbcnt.lcnt) IS not null then COUNT(gsbcnt.lcnt) else 0 end as cntln, SUM(appv) as appln,SUM(rej) as rejln   	" +
                "	 from 	" +
                "	  (	" +
                "	  select score,scr,lcnt,CBS_STATUS 	" +
                "	  ,  case when (SCORE - scr) < 0 then convert(varchar,(scr + ((((SCORE + 1) - scr)/rint) -1 ) * rint)) + '-' 	" +
                "	  + convert(varchar,((scr + ((((SCORE + 1 ) - scr)/rint)-1 ) * rint) + rint - 1))  when (SCORE - scr) >= 0 	" +
                "	  then convert(varchar,(scr + ((SCORE - scr)/rint) * rint)) + '-' + convert(varchar,((scr + ((SCORE - scr)/rint) * rint) + rint - 1))   	" +
                "	  end as scrdif ,  case when CBS_STATUS IN (3,6,7) then 1 else 0 end as appv	" +
                "	  ,   case when CBS_STATUS IN (4,5) then 1 else 0 end as rej   	" +
                "	  from 	" +
                "	  (	" +
                "	  select score	" +
                "	  ,  (select cut_off from ST_SCORERANGE y where  y.ltype= '{1}' and y.lstype = '{2}' ) as scr	" +
                "	  ,   (select rint from ST_SCORERANGE y where  y.ltype= '{1}' and y.lstype = '{2}' ) as rint   	" +
                "	  ,lcnt,CBS_STATUS 	" +
                "	  from LN_APP a,LN_GRADE b 	" +
                "	  ,(	" +
                "	  select count(1) as lcnt   	" +
                "	  from LN_APP a 	" +
                "	  where a.APP_SEQ = (select MAX(APP_SEQ) from LN_APP AS LN_APP2 where a.APP_NO = LN_APP2.APP_NO)   	" +
                "	  and LOAN_CD = '{1}' and STYPE_CD = '{2}'    {3} {4}  	" +
                //"	   and a.APP_DATE between {5} and {6} and CBS_STATUS IN (3,4,5,6,7) 	" +
                "	   and convert(varchar,year(a.APP_DATE))+'-'+right('00'+convert(varchar,month(a.APP_DATE)),2) between " +
                "      convert(varchar,year('{5}'))+'-'+right('00'+convert(varchar,month('{5}')),2) and " +
                "      convert(varchar,year('{6}'))+'-'+right('00'+convert(varchar,month('{6}')),2) " + 
                "      and CBS_STATUS IN (3,4,5,6,7) 	" +
                "	   ) wq   	" +
                "	   where a.APP_NO = b.APP_NO  and a.APP_SEQ = b.APP_SEQ    	" +
                "	   and a.APP_SEQ = (select MAX(APP_SEQ) from LN_APP AS LN_APP2 where a.APP_NO = LN_APP2.APP_NO)   	" +
                "	   and LOAN_CD = '{1}' and STYPE_CD = '{2}'     {3} {4}  	" +
                //"	    and a.APP_DATE between {5} and {6} and CBS_STATUS IN (3,4,5,6,7)	" +
                "	   and convert(varchar,year(a.APP_DATE))+'-'+right('00'+convert(varchar,month(a.APP_DATE)),2) between " +
                "      convert(varchar,year('{5}'))+'-'+right('00'+convert(varchar,month('{5}')),2) and " +
                "      convert(varchar,year('{6}'))+'-'+right('00'+convert(varchar,month('{6}')),2) " +
                "      and CBS_STATUS IN (3,4,5,6,7) 	" +
                "	    ) 	" +
                "	 gsbdat 	" +
                "	 ) gsbcnt   	" +
                "	 group by gsbcnt.lcnt,gsbcnt.scrdif 	" +
                "	 )c   on defrng.scrange = c.scrdif   	" +
                "	 order by defrng.scrange  	" +
                "	  select *,case when (SCORE - scr) < 0 then convert(varchar,(scr + ((((SCORE + 1) - scr)/rint) -1 ) * rint)) 	" +
                "	 + '-' + convert(varchar,((scr + ((((SCORE + 1 ) - scr)/rint)-1 ) * rint) + rint - 1))  	" +
                "	 when (SCORE - scr) >= 0 then convert(varchar,(scr + ((SCORE - scr)/rint) * rint)) 	" +
                "	 + '-' + convert(varchar,((scr + ((SCORE - scr)/rint) * rint) + rint - 1))   end as scrrange 	" +
                "	 into #cal_approve	" +
                "	from	" +
                "	(	" +
                //Tai 2014-01-02
                //"	select APP_NO,MODEL,LOAN_CD,SCORE,CBS_STATUS,STYPE_CD,MARKET_CD,APP_DATE	" +
                "	select distinct APP_NO,MODEL,LOAN_CD,SCORE,CBS_STATUS,STYPE_CD,MARKET_CD,APP_DATE	" +
                //
                "	 ,  (select cut_off from ST_SCORERANGE y where  y.ltype= '{1}' and y.lstype = '{2}' ) as scr	" +
                "	 ,   (select rint from ST_SCORERANGE y where  y.ltype= '{1}' and y.lstype = '{2}' ) as rint 	" +
                "	,CASE WHEN (score>=cut_off) then 'Y' else 'N' end as scapproval_flag	" +
                "	,case when CBS_STATUS in (3,6,7) then 'Y'  when  CBS_STATUS in (4,5) then 'N' 	" +
                "	when  CBS_STATUS in (1,2) OR CBS_STATUS is null then '<>' end as Approval_Flag	" +
                "	,case when LEFT(CBS_REASON_CD,1)='A' AND  CBS_STATUS in (4,5) then 'Y' 	" +
                "	when LEFT(CBS_REASON_CD,1)='R' AND  CBS_STATUS in (3,6,7) then 'Y'	" +
                "	when LEFT(CBS_REASON_CD,1)='A' AND  CBS_STATUS in (3,6,7) then 'N'	" +
                "	when LEFT(CBS_REASON_CD,1)='R' AND  CBS_STATUS in (4,5) then 'N'	" +
                "	else 'N/A' end as CONTRAST_FLAG	" +
                "	, CONVERT(VARCHAR,YEAR(APP_DATE))+'-'+RIGHT('00'+CONVERT(VARCHAR,MONTH(APP_DATE)),2) AS MYAppDate	" +
                "	from	" +
                "	(	" +
                "	SELECT a.*,b.score,b.model,	" +
                "	(select cut_off from ST_SCORERANGE y where  y.ltype= '{1}' and y.lstype = '{2}' ) as cut_off	" +
                "	,CBS_REASON_CD  from LN_APP a,LN_GRADE b,CBS_LN_APP c 	" +
                "	where a.APP_NO = b.APP_NO  and a.app_no = c.CBS_APP_NO 	" +
                "	and  LOAN_CD = '{1}' and STYPE_CD = '{2}'   {3} {4} 	" +
                //Tai 2014-01-02
                "   and a.APP_SEQ = (select MAX(APP_SEQ) from LN_APP AS LN_APP2 where a.APP_NO = LN_APP2.APP_NO) " +
                //
                "	) AA	" +
                "	) BB 	" +
                "	where MyAppDate between 	" +
                "	substring('{5}',1,4)+'-'+ substring('{5}',6,2) 	" +
                "	and  substring('{6}',1,4)+'-'+ substring('{6}',6,2) 	" +
                "	and Contrast_flag='N' 	" +
                "	select scrrange,count(*) as Actual	" +
                "	 into #temp2	" +
                "	 from #cal_approve	" +
                "	 group by scrrange	" +
                "	select sum(Actual) as Total	" +
                "	into #temp3	" +
                "	from #temp2	" +
                "	select A.*,(convert(decimal(15,6),A.Actual)/B.Total)*100.00 as [%Actual]	" +
                "	into #temp4	" +
                "	from #temp2 A, #temp3 B 	" +
                "	select  AA.scrrg as [scrrg],AA.DEV as [DEV],AA.PDEV/10000.00 as [PDEV]	" +
                "	,case when (BB.Actual is null) then 0 else BB.Actual end as Actual	" +
                "	,case when (BB.[%Actual] is null) then 0 else BB.[%Actual] end as [%Actual]	" +
                "	into #temp5	" +
                "	from (	" +
                "	select * from #temp ) AA	" +
                "	left join	" +
                "	(	" +
                "	select * from #temp4	" +
                "	) BB	" +
                "	on AA.scrrg=BB.scrrange	" +
                "	select *,[%Actual]-[PDEV] as [%Change]	" +
                "	into #temp6	" +
                "	from #temp5 	" +
                "	select *,case when [PDEV]=0 then 1 else convert(decimal(15,4),[%Actual])/convert(decimal(15,4),[PDEV]) end as [Ratio]	" +
                "	into #temp7	" +
                "	from #temp6	" +
                "	select *,log(case when [Ratio]=0 then 1 else [Ratio] end) as [WOE]	" +
                "	into #temp8	" +
                "	from #temp7	" +
                "	select *,[%Change]/100*[WOE] as [Index]	" +
                "	into #temp9	" +
                "	from #temp8	" +
                "	select scrrange,count(*) as Actual	" +
                "	into #Approve	" +
                "	 from #cal_approve	" +
                "	 where CBS_STATUS IN (3,6,7) 	" +
                "	group by scrrange	" +
                "	select scrrange,count(*) as Actual	" +
                "	into #Reject	" +
                "	 from #cal_approve	" +
                "	 where CBS_STATUS IN (4,5) 	" +
                "	 group by scrrange	" +
                "	select A.scrrange as SCRRG, A.Actual as Approve	" +
                "	,case when B.Actual is null then 0 else B.Actual end  as Reject 	" +
                "	into #Approve_Reject_Temp	" +
                "	from 	" +
                "	#Approve A left join #Reject B on A.scrrange=B.scrrange	" +
                "	select A.*	" +
                "	,case when B.Approve is null then 0 else B.Approve end as Approve	" +
                "	,case when B.Reject is null then 0 else B.Reject end as Reject	" +
                "	into #temp10	" +
                "	from #temp9 A left join #Approve_Reject_Temp B on A.SCRRG=B.SCRRG	" +
                "	select *	" +
                "	,(CONVERT(DECIMAL(15,3),[Approve])/(case when [Actual]=0 then 1 else [Actual] end) )*100.00 as [%Approve] " +
                "	,(CONVERT(DECIMAL(15,3),[Reject])/(case when [Actual]=0 then 1 else [Actual] end) )*100.00 as [%Reject] " +
                "	into #final_temp	" +
                "	from #temp10	" +
                "	select[SCRRG],[DEV],[PDEV],[Actual] as [ACT],[%Actual] as [PACT],[%Change] as [PCHNG],[Ratio] as[PRATIO],[WOE],[Index] as [PINDEX],[%Approve] as [PAPP],[%Reject] as [PREJ] 	" +
                "	into #Part1  from #final_temp	" +
                "	 select 'Total' as [SCRRG],sum([DEV]) as [DEV],sum([PDEV]) as [PDEV],sum([ACT]) as [ACT]	" +
                "	 ,sum([PACT]) as [PACT],null as [PCHNG],null as [PRATIO],null as [WOE],sum([PINDEX]) as [PINDEX], null as [PAPP],null as [PREJ] 	" +
                "	into #Part2	 from #Part1 	" +
                "   select * from #Part1 union select * from #Part2 	" +
                "	drop table #temp	drop table #Part1 drop table #Part2 " +
                "	drop table #temp2	" +
                "	drop table #temp3	" +
                "	drop table #temp4	" +
                "	drop table #temp5	" +
                "	drop table #temp6	" +
                "	drop table #temp7	" +
                "	drop table #temp8	" +
                "	drop table #temp9	" +
                "	drop table #temp10	" +
                "	drop table #Approve	" +
                "	drop table #Reject	" +
                "   drop table #final_temp	" +
                "	drop table #cal_approve	" +
                "	drop table #Approve_Reject_Temp	"
                , ("00" + ddlModel.SelectedItem.Value), ddlLoan.SelectedItem.Value, ddlSubType.SelectedItem.Value, mktqrf, mktqrt, ddlOpenDateYear.SelectedValue.ToString()+"-"+ (ddlOpenDateMonth.SelectedIndex + 1).ToString("00")+"-01", ddlOpenDateYearend.SelectedValue.ToString() + "-" + (ddlOpenDateMonthend.SelectedIndex + 1).ToString("00") + "-01");

                                        DataTable dtb = new DataTable();

                                        if (ddlModel.SelectedItem.Value == "7")
                                        {
                                            //appQuery1 = "exec [sp_FrontEnd_PopulationStabilityReport] '007','7900','30001','9999','9999','2021-01-01','2022-02-01'";
                                            //dtb = conn.ExcuteSQL(appQuery1);
                                            //Session["dtb"] = dtb;
                                            //gvTable.DataSource = dtb;
                                            //gvTable.DataBind();

                                            String strConnString = ConfigurationManager.ConnectionStrings["GSBSQLServer"].ConnectionString;
                                            SqlConnection con = new SqlConnection(strConnString);
                                            con.Open();
                                            SqlCommand command = new SqlCommand("sp_FrontEnd_PopulationStabilityReport", con);

                                            //DateTime date_open = DateTime.ParseExact("", "dd/MM/yyyy", null);
                                            //DateTime date_close = DateTime.ParseExact("", "dd/MM/yyyy", null);

                                            //string a = ddlOpenDateYear.SelectedValue.ToString() + (ddlOpenDateMonth.SelectedIndex + 1).ToString("00");
                                            //string b = ddlOpenDateYearend.SelectedValue.ToString() + (ddlOpenDateMonthend.SelectedIndex + 1).ToString("00");


                                            command.Parameters.Add(new SqlParameter("@start", ddlOpenDateYear.SelectedValue.ToString()+(ddlOpenDateMonth.SelectedIndex+1).ToString("00")));
                                            command.Parameters.Add(new SqlParameter("@end", ddlOpenDateYearend.SelectedValue.ToString() + (ddlOpenDateMonthend.SelectedIndex + 1).ToString("00")));
                                            command.Parameters.Add(new SqlParameter("@ddlLoan", ddlLoan.SelectedItem.Value));
                                            command.Parameters.Add(new SqlParameter("@ddlSubType", ddlSubType.SelectedItem.Value));
                                            command.Parameters.Add(new SqlParameter("@mktqrf", ddlMarketFrm.Text));
                                            command.Parameters.Add(new SqlParameter("@mktqrt", ddlMarketTo.Text));
                                            command.Parameters.Add(new SqlParameter("@ddlModel", "00"+ddlModel.SelectedValue));

                                            command.CommandType = CommandType.StoredProcedure;
                                            command.CommandTimeout = 120;
                                            SqlDataAdapter da = new SqlDataAdapter(command);

                                            try
                                            {
                                                da.Fill(dtb);
                                            }
                                            catch
                                            {
                                                ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScript", "appPop('GSB','เกินระยะเวลาที่กำหนด กรุณาติดต่อเจ้าหน้าที่ผู้ดูแลระบบ <br /> (หน่วยระบบงานสินเชื่อเฉพาะด้าน ติดต่อ 880166 หรือ 880452)')", true);
                                                return;
                                            }

                                            if (dtb.Rows[0][0].ToString() == "data not found")
                                            {
                                                ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScript", "appPop('โปรดเลือกตัวเลือก','ไม่พบข้อมูล กรุณาเลือกเงื่อนไขใหม่')", true);
                                                return;
                                            }
                                            Session["dtb"] = dtb;
                                            gvTable.DataSource = dtb;
                                            gvTable.DataBind();
                                        }
                                        else
                                        {
                                            
                                            dtb = conn.ExcuteSQL(appQuery1);
                                            Session["dtb"] = dtb;
                                            gvTable.DataSource = dtb;
                                            gvTable.DataBind();
                                        }


                                        //object sumObject = dtb.Compute("Sum(PINDEX)/2", "");
                                        //lblSumIndex.Text = "0.00";//Convert.ToDecimal(sumObject.ToString()).ToString("0.00");
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
        }

        protected void btnExcel_Click(object sender, EventArgs e)
        {
            if (gvTable.Rows.Count > 0)
            {
                Label userFirstname = (Label)Master.FindControl("userFirstname");
                Session["userFirstname"] = userFirstname.Text;
                DataTable dt = (DataTable)Session["dtb"];
                Session["rptData"] = dt; //Report Data
                Session["lblSumIndex"] = "";// lblSumIndex.Text; //Summarry of Index
                                                           //Report header
                Session["sHeaderRpt1"] = "ประเภทโมเดล : " + ddlModel.SelectedItem.Text;
                Session["sHeaderRpt2"] = "ประเภทสินเชื่อ / ประเภทสินเชื่อย่อย : " + ddlLoan.SelectedItem.Text + " / " + ddlSubType.SelectedItem.Text;

                if (ddlMarketFrm.SelectedIndex == 0)
                    Session["sHeaderRpt3"] = "Market Code : " + ddlMarketFrm.SelectedItem.Text;
                else
                    Session["sHeaderRpt3"] = "Market Code : " + ddlMarketFrm.SelectedItem.Text + " ถึง " + ddlMarketTo.SelectedItem.Text;

                    Session["sHeaderRpt4"] = "วันที่เปิดใบคำขอสินเชื่อ : " + ddlOpenDateMonth.SelectedValue + " " + ddlOpenDateYear.SelectedItem + " ถึง " + ddlOpenDateMonthend.SelectedValue + " " + ddlOpenDateYearend.SelectedItem;


                    ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "aa", "window.open('ReportLayOut/frmPreviewFontRpt.aspx?ReportId=91');", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScript", "appPop('โปรดเลือกตัวเลือก','กรุณากดปุ่มแสดงข้อมูลก่อน')", true);
                return;
            }

        }

//        protected void btnExcel_Click(object sender, EventArgs e)
//        {
//            DateTime start = new DateTime();
//            DateTime end = new DateTime();
//            //Tai
//            StringBuilder sqlQuery = new StringBuilder();
//            string mktqrf = "";
//            string mktqrt = "";
//            //Tai
//            if (!(txtOpenDate.Text == ""))
//            {
//                //start = DateTime.Parse(txtOpenDate.Text.ToString());
//                start = DateTime.Parse((txtOpenDate.Text.ToString().Substring(0, 6) + (Convert.ToInt32(txtOpenDate.Text.Substring(6, 4).ToString()) - 543).ToString()));
//            }
//            else
//            {
//                start = DateTime.Now;
//            }
//            if (!(txtCloseDate.Text == ""))
//            {
//                //end = DateTime.Parse(txtCloseDate.Text.ToString());
//                end = DateTime.Parse(txtCloseDate.Text.ToString().Substring(0, 6) + (Convert.ToInt32(txtCloseDate.Text.Substring(6, 4).ToString()) - 543).ToString() + " 23:59:59.999");
//            }
//            else
//            {
//                end = DateTime.Now;

        //            }

        //            if (ddlMarketFrm.SelectedItem.Value != "")
        //            {
        //                mktqrf = String.Format(" AND (MARKET_CD >= '{0}')", ddlMarketFrm.SelectedItem.Value);
        //            }


        //            if (ddlMarketTo.SelectedItem.Value != "")
        //            {
        //                sqlQuery.Append(String.Format(" AND (MARKET_CD <= '{0}')", ddlMarketTo.SelectedItem.Value));
        //                mktqrt = String.Format(" AND (MARKET_CD <= '{0}')", ddlMarketTo.SelectedItem.Value);
        //            }

        //            /*
        //            if (ddlMarketTo.SelectedItem.Value != "")
        //            {
        //                mktqrt = String.Format(" AND (MARKET_CD <= '{0}')", ddlMarketFrm.SelectedItem.Value);
        //            }
        //            */
        //                        appQuery1 = string.Format("	 select scrange as SCRRG,case when b.cntdev IS null then 0 else b.cntdev end as DEV  	"+
        //"	 ,case when b.tot is null then 0 else convert(decimal(15,3)	"+
        //"	 ,(((case when b.cntdev IS null then 0 else b.cntdev end) / convert(decimal(15,4),b.tot) ) * 1000000.00)) end as PDEV     	" +
        //"	 into #temp	"+
        //"	 from 	"+
        //"	 ( 	"+
        //"	 select range_name as scrange 	"+
        //"	 from st_scorernglst 	"+
        //"	 where range_cd =   (select range_cd from ST_SCORERANGE where ltype= '{1}' and lstype = '{2}')	"+
        //"	 ) defrng  	"+
        //"	  left join  	"+
        //"	 (	"+
        //"	 select case when Devcnt.tot IS null then 0 else Devcnt.tot end as tot ,Devcnt.scrrange	"+
        //"	 , case when COUNT(Devcnt.tot) is null then 0 else COUNT(Devcnt.tot) end as cntdev   	"+
        //"	 from (select case when (SCORE - scr) < 0 then convert(varchar,(scr + ((((SCORE + 1) - scr)/rint) -1 ) * rint)) + '-' + convert(varchar,((scr + ((((SCORE + 1 ) - scr)/rint)-1 ) * rint) + rint - 1))  when (SCORE - scr) >= 0 then convert(varchar,(scr + ((SCORE - scr)/rint) * rint)) + '-' + convert(varchar,((scr + ((SCORE - scr)/rint) * rint) + rint - 1))   end as scrrange ,SCORE,(select COUNT(SCORE) 	"+
        //"	 from [LN_DEV] where MODEL_CD = '{0}') as tot 	"+
        //"	 from 	"+
        //"	 ( 	"+
        //"	 SELECT [SCORE] 	"+
        //"	 ,  (select cut_off from ST_SCORERANGE y where  y.ltype= '{1}' and y.lstype = '{2}' ) as scr	"+
        //"	 ,   (select rint from ST_SCORERANGE y where  y.ltype= '{1}' and y.lstype = '{2}' ) as rint   	"+
        //"	 , (select COUNT(score) as scdev from [LN_DEV] where MODEL_CD = '{0}') as devcnt   	"+
        //"	 FROM [LN_DEV] where MODEL_CD = '{0}' ) devdat 	"+
        //"	 ) Devcnt   group by Devcnt.tot,Devcnt.scrrange 	"+
        //"	 ) b  on defrng.scrange = b.scrrange   	"+
        //"	 left join 	"+
        //"	 (	"+
        //"	 select case when gsbcnt.lcnt is null then 0 else gsbcnt.lcnt end as lcnt,gsbcnt.scrdif	"+
        //"	 ,  case when COUNT(gsbcnt.lcnt) IS not null then COUNT(gsbcnt.lcnt) else 0 end as cntln, SUM(appv) as appln,SUM(rej) as rejln   	"+
        //"	 from 	"+
        //"	  (	"+
        //"	  select score,scr,lcnt,CBS_STATUS 	"+
        //"	  ,  case when (SCORE - scr) < 0 then convert(varchar,(scr + ((((SCORE + 1) - scr)/rint) -1 ) * rint)) + '-' 	"+
        //"	  + convert(varchar,((scr + ((((SCORE + 1 ) - scr)/rint)-1 ) * rint) + rint - 1))  when (SCORE - scr) >= 0 	"+
        //"	  then convert(varchar,(scr + ((SCORE - scr)/rint) * rint)) + '-' + convert(varchar,((scr + ((SCORE - scr)/rint) * rint) + rint - 1))   	"+
        //"	  end as scrdif ,  case when CBS_STATUS IN (3,6,7) then 1 else 0 end as appv	"+
        //"	  ,   case when CBS_STATUS IN (4,5) then 1 else 0 end as rej   	"+
        //"	  from 	"+
        //"	  (	"+
        //"	  select score	"+
        //"	  ,  (select cut_off from ST_SCORERANGE y where  y.ltype= '{1}' and y.lstype = '{2}' ) as scr	"+
        //"	  ,   (select rint from ST_SCORERANGE y where  y.ltype= '{1}' and y.lstype = '{2}' ) as rint   	"+
        //"	  ,lcnt,CBS_STATUS 	"+
        //"	  from LN_APP a,LN_GRADE b 	"+
        //"	  ,(	"+
        //"	  select count(1) as lcnt   	"+
        //"	  from LN_APP a 	"+
        //"	  where a.APP_SEQ = (select MAX(APP_SEQ) from LN_APP AS LN_APP2 where a.APP_NO = LN_APP2.APP_NO)   	"+
        //"	  and LOAN_CD = '{1}' and STYPE_CD = '{2}'    {3} {4}  	"+
        ////"	   and a.APP_DATE between {5} and {6} and CBS_STATUS IN (3,4,5,6,7) 	"+
        //"	   and convert(varchar,year(a.APP_DATE))+'-'+right('00'+convert(varchar,month(a.APP_DATE)),2) between " +
        //"      convert(varchar,year('{5}'))+'-'+right('00'+convert(varchar,month('{5}')),2) and " +
        //"      convert(varchar,year('{6}'))+'-'+right('00'+convert(varchar,month('{6}')),2) " +
        //"      and CBS_STATUS IN (3,4,5,6,7) 	" +
        //"	   ) wq   	"+
        //"	   where a.APP_NO = b.APP_NO  and a.APP_SEQ = b.APP_SEQ    	"+
        //"	   and a.APP_SEQ = (select MAX(APP_SEQ) from LN_APP AS LN_APP2 where a.APP_NO = LN_APP2.APP_NO)   	"+
        //"	   and LOAN_CD = '{1}' and STYPE_CD = '{2}'     {3} {4}  	"+
        ////"	    and a.APP_DATE between {5} and {6} and CBS_STATUS IN (3,4,5,6,7)	"+
        //"	   and convert(varchar,year(a.APP_DATE))+'-'+right('00'+convert(varchar,month(a.APP_DATE)),2) between " +
        //"      convert(varchar,year('{5}'))+'-'+right('00'+convert(varchar,month('{5}')),2) and " +
        //"      convert(varchar,year('{6}'))+'-'+right('00'+convert(varchar,month('{6}')),2) " +
        //"      and CBS_STATUS IN (3,4,5,6,7) 	" +
        //"	    ) 	"+
        //"	 gsbdat 	"+
        //"	 ) gsbcnt   	"+
        //"	 group by gsbcnt.lcnt,gsbcnt.scrdif 	"+
        //"	 ) c   on defrng.scrange = c.scrdif   	"+
        //"	 order by defrng.scrange  	"+
        //"	  select *,case when (SCORE - scr) < 0 then convert(varchar,(scr + ((((SCORE + 1) - scr)/rint) -1 ) * rint)) 	"+
        //"	 + '-' + convert(varchar,((scr + ((((SCORE + 1 ) - scr)/rint)-1 ) * rint) + rint - 1))  	"+
        //"	 when (SCORE - scr) >= 0 then convert(varchar,(scr + ((SCORE - scr)/rint) * rint)) 	"+
        //"	 + '-' + convert(varchar,((scr + ((SCORE - scr)/rint) * rint) + rint - 1))   end as scrrange 	"+
        //"	 into #cal_approve	"+
        //"	from	"+
        //"	(	"+
        ////Tai 2014-01-02
        ////"	select APP_NO,MODEL,LOAN_CD,SCORE,CBS_STATUS,STYPE_CD,MARKET_CD,APP_DATE	"+
        //"	select distinct APP_NO,MODEL,LOAN_CD,SCORE,CBS_STATUS,STYPE_CD,MARKET_CD,APP_DATE	" +
        ////
        //"	 ,  (select cut_off from ST_SCORERANGE y where  y.ltype= '{1}' and y.lstype = '{2}' ) as scr	"+
        //"	 ,   (select rint from ST_SCORERANGE y where  y.ltype= '{1}' and y.lstype = '{2}' ) as rint 	"+
        //"	,CASE WHEN (score>=cut_off) then 'Y' else 'N' end as scapproval_flag	"+
        //"	,case when CBS_STATUS in (3,6,7) then 'Y'  when  CBS_STATUS in (4,5) then 'N' 	"+
        //"	when  CBS_STATUS in (1,2) OR CBS_STATUS is null then '<>' end as Approval_Flag	"+
        //"	,case when LEFT(CBS_REASON_CD,1)='A' AND  CBS_STATUS in (4,5) then 'Y' 	"+
        //"	when LEFT(CBS_REASON_CD,1)='R' AND  CBS_STATUS in (3,6,7) then 'Y'	"+
        //"	when LEFT(CBS_REASON_CD,1)='A' AND  CBS_STATUS in (3,6,7) then 'N'	"+
        //"	when LEFT(CBS_REASON_CD,1)='R' AND  CBS_STATUS in (4,5) then 'N'	"+
        //"	else 'N/A' end as CONTRAST_FLAG	"+
        //"	, CONVERT(VARCHAR,YEAR(APP_DATE))+'-'+RIGHT('00'+CONVERT(VARCHAR,MONTH(APP_DATE)),2) AS MYAppDate	"+
        //"	from	"+
        //"	(	"+
        //"	SELECT a.*,b.score,b.model,	"+
        //"	(select cut_off from ST_SCORERANGE y where  y.ltype= '{1}' and y.lstype = '{2}' ) as cut_off	"+
        //"	,CBS_REASON_CD  from LN_APP a,LN_GRADE b,CBS_LN_APP c 	"+
        //"	where a.APP_NO = b.APP_NO  and a.app_no = c.CBS_APP_NO 	"+
        //"	and  LOAN_CD = '{1}' and STYPE_CD = '{2}'   {3} {4} 	"+
        ////Tai 2014-01-02
        //"   and a.APP_SEQ = (select MAX(APP_SEQ) from LN_APP AS LN_APP2 where a.APP_NO = LN_APP2.APP_NO) " +
        ////
        //"	) AA	"+
        //"	) BB 	"+
        //"	where MyAppDate between 	"+
        //"	substring('{5}',1,4)+'-'+ substring('{5}',6,2) 	"+
        //"	and  substring('{6}',1,4)+'-'+ substring('{6}',6,2) 	"+
        //"	and Contrast_flag='N' 	"+
        //"	select scrrange,count(*) as Actual	"+
        //"	 into #temp2	"+
        //"	 from #cal_approve	"+
        //"	 group by scrrange	"+
        //"	select sum(Actual) as Total	"+
        //"	into #temp3	"+
        //"	from #temp2	"+
        //"	select A.*,(convert(decimal(15,6),A.Actual)/B.Total)*100.00 as [%Actual]	"+
        //"	into #temp4	"+
        //"	from #temp2 A, #temp3 B 	"+
        //"	select  AA.scrrg as [scrrg],AA.DEV as [DEV],AA.PDEV/10000.00 as [PDEV]	" +
        //"	,case when (BB.Actual is null) then 0 else BB.Actual end as Actual	"+
        //"	,case when (BB.[%Actual] is null) then 0 else BB.[%Actual] end as [%Actual]	"+
        //"	into #temp5	"+
        //"	from (	"+
        //"	select * from #temp ) AA	"+
        //"	left join	"+
        //"	(	"+
        //"	select * from #temp4	"+
        //"	) BB	"+
        //"	on AA.scrrg=BB.scrrange	"+
        //"	select *,[%Actual]-[PDEV] as [%Change]	"+
        //"	into #temp6	"+
        //"	from #temp5 	"+
        //"	select *,case when [PDEV]=0 then 1 else convert(decimal(15,4),[%Actual])/convert(decimal(15,4),[PDEV]) end as [Ratio]	" +
        //"	into #temp7	"+
        //"	from #temp6	"+
        //"	select *,log(case when [Ratio]=0 then 1 else [Ratio] end) as [WOE]	"+
        //"	into #temp8	"+
        //"	from #temp7	"+
        //"	select *,[%Change]/100*[WOE] as [Index]	"+
        //"	into #temp9	"+
        //"	from #temp8	"+
        //"	select scrrange,count(*) as Actual	"+
        //"	into #Approve	"+
        //"	 from #cal_approve	"+
        //"	 where CBS_STATUS IN (3,6,7) 	"+
        //"	group by scrrange	"+
        //"	select scrrange,count(*) as Actual	"+
        //"	into #Reject	"+
        //"	 from #cal_approve	"+
        //"	 where CBS_STATUS IN (4,5) 	"+
        //"	 group by scrrange	"+
        //"	select A.scrrange as SCRRG, A.Actual as Approve	"+
        //"	,case when B.Actual is null then 0 else B.Actual end  as Reject 	"+
        //"	into #Approve_Reject_Temp	"+
        //"	from 	"+
        //"	#Approve A left join #Reject B on A.scrrange=B.scrrange	"+
        //"	select A.*	"+
        //"	,case when B.Approve is null then 0 else B.Approve end as Approve	"+
        //"	,case when B.Reject is null then 0 else B.Reject end as Reject	"+
        //"	into #temp10	"+
        //"	from #temp9 A left join #Approve_Reject_Temp B on A.SCRRG=B.SCRRG	"+
        //"	select *	"+
        //"	,(CONVERT(DECIMAL(15,3),[Approve])/(case when [Actual]=0 then 1 else [Actual] end) )*100.00 as [%Approve] " +
        //"	,(CONVERT(DECIMAL(15,3),[Reject])/(case when [Actual]=0 then 1 else [Actual] end) )*100.00 as [%Reject] " +
        //"	into #final_temp	"+
        //"	from #temp10	"+
        //"	select[SCRRG],[DEV],[PDEV],[Actual] as [ACT],[%Actual] as [PACT],[%Change] as [PCHNG],[Ratio] as[PRATIO],[WOE],[Index] as [PINDEX],[%Approve] as [PAPP],[%Reject] as [PREJ] 	"+
        //"	into #Part1  from #final_temp	" +
        //"	 select 'Total' as [SCRRG],sum([DEV]) as [DEV],sum([PDEV]) as [PDEV],sum([ACT]) as [ACT]	"+
        //"	 ,sum([PACT]) as [PACT],null as [PCHNG],null as [PRATIO],null as [WOE],sum([PINDEX]) as [PINDEX], null as [PAPP],null as [PREJ] 	" +
        //"	into #Part2	 from #Part1 	"+
        //"   select convert(varchar,SCRRG) as SCRRG	"+
        //"	 ,dbo.comma_format(convert(varchar,DEV)) as DEV "+
        //"	 ,convert(varchar,convert(decimal(15,2),PDEV)) as PDEV "+
        //"	 ,dbo.comma_format(convert(varchar,ACT)) as ACT "+
        //"	 ,convert(varchar,convert(decimal(15,2),PACT)) as PACT	"+
        //"	 ,convert(varchar,convert(decimal(15,2),PCHNG)) as PCHNG	"+
        //"	 ,convert(varchar,convert(decimal(15,2),PRATIO)) as PRATIO	"+
        //"	 ,convert(varchar,convert(decimal(15,2),WOE)) as WOE	"+
        //"	 ,convert(varchar,convert(decimal(15,2),PINDEX)) as PINDEX	"+
        //"	 ,convert(varchar,convert(decimal(15,2),PAPP)) as PAPP	"+
        //"	 ,convert(varchar,convert(decimal(15,2),PREJ)) as PREJ	"+
        //"	  from #Part1 	  union 	  select convert(varchar,SCRRG) as SCRRG 	"+
        //"	 ,dbo.comma_format(convert(varchar,DEV)) as DEV	 ,convert(varchar,convert(decimal(15,2),PDEV)) as PDEV	" +
        //"	 ,dbo.comma_format(convert(varchar,ACT)) as ACT	 ,convert(varchar,convert(decimal(15,2),PACT)) as PACT	" +
        //"	 ,convert(varchar,convert(decimal(15,2),PCHNG)) as PCHNG	 ,convert(varchar,convert(decimal(15,2),PRATIO)) as PRATIO	"+
        //"	 ,convert(varchar,convert(decimal(15,2),WOE)) as WOE	 ,convert(varchar,convert(decimal(15,2),PINDEX)) as PINDEX	"+
        //"	 ,convert(varchar,convert(decimal(15,2),PAPP)) as PAPP	 ,convert(varchar,convert(decimal(15,2),PREJ)) as PREJ	  from #Part2 	"+
        //"	drop table #temp	drop table #Part1 drop table #Part2 " +
        //"	drop table #temp2	"+
        //"	drop table #temp3	"+
        //"	drop table #temp4	"+
        //"	drop table #temp5	"+
        //"	drop table #temp6	"+
        //"	drop table #temp7	"+
        //"	drop table #temp8	"+
        //"	drop table #temp9	"+
        //"	drop table #temp10	"+
        //"	drop table #Approve	"+
        //"	drop table #Reject	"+
        //"   drop table #final_temp	"+
        //"	drop table #cal_approve	"+
        //"	drop table #Approve_Reject_Temp	", ("00" + ddlModel.SelectedItem.Value), ddlLoan.SelectedItem.Value, ddlSubType.SelectedItem.Value, mktqrf, mktqrt, start.ToString("yyyy-MM-dd"), end.ToString("yyyy-MM-dd"));
        //            DataTableToExcel(appQuery1.ToString());
        //        }

        protected void Cancel_Click(object sender, EventArgs e)
        {
            ddlModel.ClearSelection();
            ddlLoan.ClearSelection();
            ddlSubType.ClearSelection();
            ddlMarketFrm.ClearSelection();
            ddlMarketTo.ClearSelection();
            ddlSubType.Enabled = false;
            //txtOpenDate.Text = string.Empty;
            //txtCloseDate.Text = string.Empty;
        }
        protected void gvTable_PageChanging(object sender, GridViewPageEventArgs e)
        {
            gvTable.PageIndex = e.NewPageIndex;
            Search_Click(null, null);
        }
        #endregion Protected Method

        protected void gvTable_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //TotalDev += Convert.ToInt32(e.Row.Cells[1].Text.Replace(",", ""));

                //TotalDevPercent += Convert.ToDecimal(e.Row.Cells[2].Text);

                //TotalActual += Convert.ToInt32(e.Row.Cells[3].Text.Replace(",", ""));

                //TotalActualPercent += Convert.ToDecimal(e.Row.Cells[4].Text);

                // Just in case we need to show the index total in the gridview, uncomment the line below.
                //TotalPopulationStabilityIndex += Convert.ToDecimal(e.Row.Cells[8].Text);
            }
// ไม่แสดง ค่า ตำนวน Total ที่ Footer 16 สค 2556
            //if (e.Row.RowType == DataControlRowType.Footer)
            //{
            //    if(Session["dtb"] != null)
            //    {
            //        DataTable dt = (DataTable)Session["dtb"];

            //        if(dt != null)
            //        {
                        
            //            if(dt.Rows.Count >0)
            //            {
            //                for (int i = 0; i < dt.Rows.Count; i++)
            //                {
            //                    if (dt.Rows[i]["DEV"].ToString() != "")
            //                    {
            //                        TotalDev += Convert.ToInt32(dt.Rows[i]["DEV"].ToString());
            //                    }

            //                    if (dt.Rows[i]["PDEV"].ToString() != "")
            //                    {
            //                        //g = dt.Rows[i]["PDEV"].ToString();
            //                        TotalDevPercent += Convert.ToDecimal(dt.Rows[i]["PDEV"].ToString());
            //                    }

            //                    if (dt.Rows[i]["ACT"].ToString() != "")
            //                    {
            //                        TotalActual += Convert.ToInt32(dt.Rows[i]["ACT"].ToString());
            //                    }

            //                    if (dt.Rows[i]["PACT"].ToString() != "")
            //                    {
            //                        //p = dt.Rows[i]["PACT"].ToString();
            //                        TotalActualPercent += Convert.ToDecimal(dt.Rows[i]["PACT"].ToString());
            //                    }
            //                }
            //            }
            //        }
            //    }


            //    //=================================================
            //    //Modify for report
            //    //=================================================
            //    Session["TotalDev"] = TotalDev.ToString("#,##0");
            //    Session["TotalDevPercent"] = TotalDevPercent.ToString("0.00");
            //    Session["TotalActual"] = TotalActual.ToString("#,##0");
            //    Session["TotalActualPercent"] = TotalActualPercent.ToString("0.00");
            //    //-------------------------------------------------


            //    if (gvTable.PageIndex == gvTable.PageCount - 1)
            //    {
            //        e.Row.Cells[0].Text = "Total";

            //        e.Row.Cells[1].Text = TotalDev.ToString("#,##0");

            //        e.Row.Cells[2].Text = TotalDevPercent.ToString("0.00");

            //        e.Row.Cells[3].Text = TotalActual.ToString("#,##0");

            //        e.Row.Cells[4].Text = TotalActualPercent.ToString("0.00");

            //    }
            //    else
            //    {
            //        e.Row.Cells[0].Text = "";

            //        e.Row.Cells[1].Text = "";

            //        e.Row.Cells[2].Text = "";

            //        e.Row.Cells[3].Text = "";

            //        e.Row.Cells[4].Text = "";
            //    }

            //}
                //---- Just in case we need to show the index total in the gridview, uncomment the line below.
                //e.Row.Cells[8].Text = TotalPopulationStabilityIndex.ToString("0.000");
        }

        //==========================================================
        //Method to print report
        //==========================================================
        protected void PrintReport_Click(object sender, EventArgs e)
        {
            if (gvTable.Rows.Count > 0)
            {
                Label userFirstname = (Label)Master.FindControl("userFirstname");
                Session["userFirstname"] = userFirstname.Text;
                Print_Preview();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScript", "appPop('โปรดเลือกตัวเลือก','กรุณากดปุ่มแสดงข้อมูลก่อน')", true);
                return;
            }


        }
        private void Print_Preview()
        {

            DataTable dt = (DataTable)Session["dtb"];
            Session["rptData"] = dt; //Report Data
            Session["lblSumIndex"] = "";//lblSumIndex.Text; //Summarry of Index
            //Report header
            Session["sHeaderRpt1"] = "ประเภทโมเดล : " + ddlModel.SelectedItem.Text;
            Session["sHeaderRpt2"] = "ประเภทสินเชื่อ / ประเภทสินเชื่อย่อย : " + ddlLoan.SelectedItem.Text + " / " + ddlSubType.SelectedItem.Text;

            if (ddlMarketFrm.SelectedIndex == 0)
                Session["sHeaderRpt3"] = "Market Code : " + ddlMarketFrm.SelectedItem.Text;
            else
                Session["sHeaderRpt3"] = "Market Code : " + ddlMarketFrm.SelectedItem.Text + " ถึง " + ddlMarketTo.SelectedItem.Text;

            Session["sHeaderRpt4"] = "วันที่เปิดใบคำขอสินเชื่อ : " + ddlOpenDateMonth.SelectedValue+" "+ddlOpenDateYear.SelectedItem + " ถึง " + ddlOpenDateMonthend.SelectedValue + " " + ddlOpenDateYearend.SelectedItem;

            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "aa", "window.open('ReportLayOut/frmPreviewFontRpt.aspx?ReportId=1');", true);
        }
        //-----------------------------------------------------------

        public ListItemCollection  GetMonth()
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