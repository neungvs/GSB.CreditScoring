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
    public partial class CharacteristicAnalysisReport : System.Web.UI.Page
    {
        SQLToDataTable conn = new SQLToDataTable();
        string appQuery1;
        #region Page_Load

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                LoadYearMonth();
                LoadModel();
                //LoadLoan();
                //LoadModelVersion();
            }
        }

        #endregion Page_Load

        #region Private Method

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
            }

            ddlOpenDateMonth.Items.Clear();
            ddlOpenDateMonthend.Items.Clear();

            ddlOpenDateMonth.DataSource = GetMonth();
            ddlOpenDateMonth.DataBind();
            ddlOpenDateMonthend.DataSource = GetMonth();
            ddlOpenDateMonthend.DataBind();

        }

        //private void LoadModelVersion()
        //{

        //    ListItem item2 = new ListItem(" Version1 ", "null");
        //    ddlModelVersion.Items.Add(item2);
        //    ddlModelVersion.SelectedValue = "null";
        //    //ddlModelVersion.DataBind();
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


        private void DataTableToExcel()
        {
            DataTable dttb = (DataTable)ViewState["DataTable1"];

            string filename = string.Format("GSB-CS_CharacteristicAnalysisReport_(Date_{0}-Time_{1}).xls", DateTime.Now.ToString("dd-MM-yyyy"), DateTime.Now.ToString("HH-mm-ss"));
            StringWriter tw = new StringWriter();
            DataGrid dgGrid = new DataGrid();
                        
            tw.Write(string.Format("Characteristic Analysis Report"));
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
            //tw.Write(string.Format("วันที่เปิดใบคำขอสินเชื่อ :| {0} ถึง {1} ", DateTime.Parse(txtOpenDate.Text.ToString()).Date.ToString("dd-MM-yyyy"), DateTime.Parse(txtCloseDate.Text.ToString()).ToString("dd-MM-yyyy")));
            //tw.Write(tw.NewLine);
            tw.Write(string.Format("ประเภทคุณลักษณะ :| {0} ", ddlchar.SelectedItem.Text));
            tw.Write(tw.NewLine);
            tw.Write(string.Format("Export Date :| {0} : {1} ", DateTime.Now.ToString("dd-MM-yyyy"), DateTime.Now.ToString("HH-mm-ss")));
            tw.Write(tw.NewLine);
            tw.Write("ผู้พิมพ์ " + ExportUtility.getCurrentUserId);
            tw.Write(tw.NewLine);

            int iColCount = new int();
            iColCount = dttb.Columns.Count;
            
            tw.Write("Attribute|Dev.|% Dev.|Actual |% Actual |Change|Point|Point Diff.");
            tw.Write(tw.NewLine);

            // Now write all the rows.
            foreach (DataRow dr in dttb.Rows)
            {
                for (int i = 0; i < iColCount; i++)
                {
                    if (!Convert.IsDBNull(dr[i]) && i == 0)
                    {
                        tw.Write(dr[i].ToString());
                    }
                    else if (!Convert.IsDBNull(dr[i]) && i != 0)
                    {
                        tw.Write(dr[i].ToString());
                    }
                    else 
                    {
                        tw.Write("0");
                    }
                    //if (!Convert.IsDBNull(dr[i]) && i != 0)
                    //{
                    //    tw.Write(dr[i].ToString());
                    //}
                    //else
                    //{
                    //    tw.Write(dr[i].ToString());
                    //    //tw.Write("0");
                    //}
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

        protected void LoadChar(object sender, EventArgs e)
        {
            string Query = string.Format(@"select distinct case when a.char_name = 'cdob' then b.CHAR_NAME+' (Age)' 
                            when a.char_name = 'career' then b.CHAR_NAME + ' (Occupation)'
                            when a.char_name = 'edustatus_id' then b.CHAR_NAME + ' (Education)'
                            when a.char_name = 'marital' then b.CHAR_NAME + ' (Marital)'
                            when a.char_name = 'tworktime_month' then b.CHAR_NAME + ' (Working Time)' 
                            else b.CHAR_NAME + ' (' + a.char_name + ')' end as char_name, a.char_name as char_cd from ST_CHARBIN a inner join ST_CHAR b on b.CHAR_CD = a.char_name where MODEL_CD = '00{0}' " +
                                         " order by a.char_name ASC", ddlModel.SelectedItem.Value);

            ddlchar.Items.Clear();
            ddlchar.DataSource = conn.ExcuteSQL(Query);
            ddlchar.DataBind();
            ddlchar.Enabled = true;
        }

        //protected void LoadVersion(object sender, EventArgs e)
        //{
        //    string sql = "";
        //    sql = string.Format("select MODEL_NAME+'_V1' as [Model_V_NAME] from LN_MODEL where ACTIVE_FLAG = '1' and");
        //    sql += "  [MODEL_CD]=" + ddlModel.SelectedValue + " ";


        //    ddlModelVersion.DataSource = conn.ExcuteSQL(sql);
        //    ddlModelVersion.DataTextField = "Model_V_NAME";
        //    ddlModelVersion.DataValueField = "Model_V_NAME";
        //    ddlModelVersion.DataBind();

        //    LoadLoan();
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
            DateTime start = new DateTime();
            DateTime end = new DateTime();
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

            if (ddlMarketFrm.SelectedIndex <=0 || ddlMarketTo.SelectedIndex <=0 )
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
            }

            if (ddlMarketTo.SelectedItem.Value != "")
            {
                sqlQuery.Append(String.Format(" AND (MARKET_CD <= '{0}')", ddlMarketTo.SelectedItem.Value));
                mktqrt = String.Format(" AND (MARKET_CD <= '{0}')", ddlMarketTo.SelectedItem.Value);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScript", "appPop('โปรดเลือกตัวเลือก','โปรดเลือกสินเชื่อย่อย ถึง อย่างน้อย 1 ค่า')", true);
            }

            int monthstart = Convert.ToInt16(ddlOpenDateYear.SelectedValue) * 100 + ddlOpenDateMonth.SelectedIndex;
            int monthend = Convert.ToInt16(ddlOpenDateYearend.SelectedValue) * 100 + ddlOpenDateMonthend.SelectedIndex;

            //int months = (end.DayOfYear - start.DayOfYear) + 365 * (end.Year - start.Year);

            if (monthstart < 0 || monthend < 0)
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
                                        appQuery1 = string.Format(" select [ST_CHARBIN].*,[ST_CHARACTER].CHAR_ID  into #temp  FROM [ST_CHARBIN]  left join [ST_CHARACTER] on  [ST_CHARBIN].model_cd=[ST_CHARACTER].MODEL_CD	" +
                "  and [ST_CHARBIN].char_name=[ST_CHARACTER].CHAR_NAME  where [ST_CHARBIN].model_cd='{0}' and [ST_CHARBIN].char_name='{7}' 	" +
                " select cb.char_name,cb.bin,cb.char_desc as Attrib,pp1.cnt as dev into  #CountOther from #Temp cb left join (	" +
                " select a.CHAR_NAME,c.[ATTR_CD] as [ATTR_CD],c.CHAR_CD as [CHAR_CD], COUNT(1) as cnt,sum(COUNT(1)) over(partition by char_name) as sum,a.model_cd from (select distinct APP_NO, CIFNO,CHAR_CD,ATTR_CD,SCORE,POINT  from LN_DEVCIFATTR) c,ST_CHARACTER a " +//, [LN_DEV] b	" +
                " where c.CHAR_CD = a.CHAR_ID  " +
                //" and right('0000000000'+c.[APP_NO],12)=right('0000000000'+b.[APP_NO],12)  " +
                " group by a.CHAR_NAME,c.[ATTR_CD],a.model_cd,c.CHAR_CD  	" +
                " ) pp1 on cb.char_name = pp1.char_name and cb.bin = pp1.[ATTR_CD] and cb.model_cd=pp1.model_cd AND pp1.CHAR_CD=cb.CHAR_ID " +
                "select sum(cnt) as cnt into #Temp1 from ( select a.CHAR_NAME,c.[ATTR_CD] as [ATTR_CD], COUNT(1) as cnt,sum(COUNT(1)) over(partition by char_name) as sum ,a.model_cd	" +
                " from LN_DEVCIFATTR c,ST_CHARACTER a " +//, [LN_DEV] b	" +
                " where c.CHAR_CD = a.CHAR_ID  " +
                //" and right('0000000000'+c.[APP_NO],12)=right('0000000000'+b.[APP_NO],12) " +
                " group by a.CHAR_NAME,c.[ATTR_CD]	" +
                " ,a.model_cd having a.char_name  = '{7}' and a.model_cd='{0}'  and  [ATTR_CD] not in (  select bin  from #Temp) ) Z	" +
                " update #CountOther set dev=(select cnt from #Temp1) where bin='All Other'	" +
                "  select CHAR_NAME,bin as ATTR_CD,dev as cnt,convert(decimal(15,4),(convert(float,dev)/SUM(dev) over()) * 100.00) as tot	" +
                "  into #Dev_Part   from #CountOther   order by bin	" +

                "	select a.*,c.CBS_REASON_CD	" +
                "   into #ln " +
                "	from LN_APP_CHAR a,LN_GRADE b,CBS_LN_APP c 	" + 
                "	where a.APP_NO = b.APP_NO  and a.app_no = c.CBS_APP_NO 	" +
                "	and a.APP_SEQ = (select MAX(LN_APP2.APP_SEQ) from LN_APP_CHAR AS LN_APP2 where a.APP_NO = LN_APP2.APP_NO)   	" +
                "	and LOAN_CD = '{1}' and STYPE_CD = '{2}' {3} {4} " +
                " and convert(varchar,year(a.APP_DATE))+'-'+right('00'+convert(varchar,month(a.APP_DATE)),2) between '{5}' and '{6}'  	" +
                "	and CBS_STATUS IN (3,4,5,6,7)	" +

                "	select * into #aa	" +
                "	from [LN_CHAR] lc   	" +
                "	where 	" +
                "	lc.APP_SEQ = (select MAX(APP_SEQ) from [LN_CHAR] AS LNCH where lc.CBS_APP_NO = LNCH.CBS_APP_NO ) and  	" +
                "   lc.CIFSCORE = (select MAX(CIFSCORE) from [LN_CHAR] AS b WHERE lc.CBS_APP_NO = B.CBS_APP_NO) and " +
                "	CHAR_CD='{7}'	" +

                "	select distinct * into #Temp3 from	" +
                "	( select case when LEFT(CBS_REASON_CD,1)='A' AND  CBS_STATUS in (4,5) then 'Y' 	" +
                "	when LEFT(CBS_REASON_CD,1)='R' AND  CBS_STATUS in (3,6,7) then 'Y'	" +
                "	when LEFT(CBS_REASON_CD,1)='A' AND  CBS_STATUS in (3,6,7) then 'N'	" +
                "	when LEFT(CBS_REASON_CD,1)='R' AND  CBS_STATUS in (4,5) then 'N'	" +
                //"	else 'N/A' end as CONTRAST_FLAG,aa.* from  (	" +
                "	else 'N/A' end as CONTRAST_FLAG,aa.* from  #ln as ln " +
 /*
                "	select a.*,c.CBS_REASON_CD	" +
                //"	from LN_APP a,LN_GRADE b,CBS_LN_APP c 	" +
                "	from LN_APP_CHAR a,LN_GRADE b,CBS_LN_APP c 	" + //2014-05-09
                "	where a.APP_NO = b.APP_NO  and a.app_no = c.CBS_APP_NO 	" +
                //"	and a.APP_SEQ = (select MAX(LN_APP2.APP_SEQ) from LN_APP AS LN_APP2 where a.APP_NO = LN_APP2.APP_NO)   	" +
                "	and a.APP_SEQ = (select MAX(LN_APP2.APP_SEQ) from LN_APP_CHAR AS LN_APP2 where a.APP_NO = LN_APP2.APP_NO)   	" + //2014-05-09
                "	and LOAN_CD = '{1}' and STYPE_CD = '{2}' {3} {4} " +
                " and convert(varchar,year(a.APP_DATE))+'-'+right('00'+convert(varchar,month(a.APP_DATE)),2) between '{5}' and '{6}'  	" +
                "	and CBS_STATUS IN (3,4,5,6,7)	" +
                "	) ln	" +
*/
                "	left join #aa as aa	" +
/*              "	left join  	" +
                "	(select * 	" +
                "	from [LN_CHAR] lc   	" +
                "	where 	" +
                "	lc.APP_SEQ = (select MAX(APP_SEQ) from [LN_CHAR] AS LNCH where lc.CBS_APP_NO = LNCH.CBS_APP_NO ) and  	" +
                "   lc.CIFSCORE = (select MAX(CIFSCORE) from [LN_CHAR] AS b WHERE lc.CBS_APP_NO = B.CBS_APP_NO) and " +
                "	CHAR_CD='{7}'	" +
                "	) aa  
*/
                " on ln.APP_NO = aa.CBS_APP_NO   	" +
                "	) ZZ	" +
                "	where CONTRAST_FLAG='N'	" +
                "	select CBS_APP_NO as APP_NO,*	" +
                "	into #Temp2 from #Temp3	" +
                " select cb.char_name,cb.bin,cb.char_desc as Attrib ,pp1.cnt as dev, " +
                " convert(decimal(15,4),(convert(float,pp1.cnt)/SUM(pp1.cnt) over()) * 100.00) as pdev, " +
                " pp2.cnt as actv,convert(decimal(15,4),(convert(float,pp2.cnt)/SUM(pp2.cnt) over()) * 100.00)  as pactv, " +
                " cb.char_scr as pnt " +
                " into #Part1 from #Temp cb left join " +
                "	(  select * from #Dev_Part    ) pp1 on cb.char_name = pp1.char_name and cb.bin = pp1.attr_cd " +
                " left join (select CHAR_CD,ATTR_CD,count(1) as cnt,sum(COUNT(1)) over(partition by char_cd) AS tot from " +
                //" ( select APP_NO,ln.APP_SEQ,a.CHAR_CD,a.ATTR_CD from " +
                //" #Temp2 ln left join " +
                //" (select CBS_APP_NO,APP_SEQ,CHAR_CD,ATTR_CD from [LN_CHAR] lc  " +
                //" where lc.APP_SEQ = (select MAX(APP_SEQ) from [LN_CHAR] AS LNCH where lc.CBS_APP_NO = LNCH.CBS_APP_NO ) ) a " +
                //" on ln.APP_NO = a.CBS_APP_NO and ln.APP_SEQ = a.APP_SEQ ) dd group by CHAR_CD,ATTR_CD  ) pp2 " +
                //" ( select distinct APP_NO,APP_SEQ,CHAR_CD,ATTR_CD from #Temp2 ) dd group by CHAR_CD,ATTR_CD  ) pp2 " + //2013-12-26
                " ( select APP_NO,APP_SEQ,CHAR_CD,ATTR_CD from #Temp2 ) dd group by CHAR_CD,ATTR_CD  ) pp2 " + //2014-05-09
                " on cb.char_name = pp2.CHAR_CD and cb.bin = pp2.attr_cd " +
                //Tai 2014-08-22 " order by cb.bin " +
                //Tai 2014-08-22 "select  row_number() over(order by bin) as seq,Attrib,dev,pdev,actv,pactv " +
                " select  1 as seq,Attrib,dev,pdev,actv,pactv " +
                //
                " ,((case when pactv is null then 0 else pactv end)-(case when pdev is null then 0 else pdev end))/100  as chng " +
                " ,pnt,(((case when pactv is null then 0 else pactv end)-(case when pdev is null then 0 else pdev end))/100 )*pnt  as pntdf  into #Part2 from #Part1 " +
                " select seq,convert(varchar(130),Attrib) as Attrib,convert(varchar,dev) as dev,convert(varchar,pdev) as pdev,convert(varchar,actv) as actv" +
                " ,convert(varchar,pactv) as pactv,convert(varchar,convert(decimal(15,2),chng)) as chng,convert(varchar,pnt) as pnt,convert(varchar,pntdf) as pntdf into #Final from #Part2 " +
                // Tai 2014-08-22"  union select 100 as seq,'Total' as Attrib,sum(convert(decimal(15,3),dev)) as dev" +
                "  union all select 100 as seq,'Total' as Attrib,sum(convert(decimal(15,3),dev)) as dev" +
                //
                " ,round(sum(convert(decimal(15,4),pdev)),1) as pdev,sum(convert(decimal(15,4),actv)) as actv" +
                " ,round(sum(convert(decimal(15,4),pactv)),1) as pactv,' ' as chng,'Score Difference = ' as pnt" +
                " ,sum(convert(decimal(15,6),pntdf))  as pntdf from #Part2   " +
                " select Attrib ,CASE WHEN dev IS NULL THEN '0' else [dbo].[comma_format](convert(varchar,convert(decimal(15,0),dev))) end as dev " +
                " ,CASE WHEN pdev IS NULL THEN '0.00' else convert(varchar,convert(decimal(15,2),pdev)) end  as  pdev " +
                " ,CASE WHEN actv IS NULL THEN '0' else [dbo].[comma_format](convert(varchar,convert(decimal(15,0),actv))) end  as actv  " +
                " ,CASE WHEN pactv IS NULL THEN '0.00' else convert(varchar,convert(decimal(15,2),pactv)) end  as pactv " +
                " ,CASE WHEN chng IS NULL THEN '0.00' else convert(varchar,chng) end  as chng ,CASE WHEN pnt IS NULL THEN '0.00' else convert(varchar,pnt) end  as pnt " +
                " ,CASE WHEN pntdf IS NULL THEN '0.00' else convert(varchar,convert(decimal(15,2),pntdf)) end  as pntdf from #Final order by seq " +
                " drop table #Temp drop table #Part1 drop table #Part2 " +
                " drop table #Temp2  drop table #Final" +
                " drop table #Temp3   drop table #Dev_Part    drop table #CountOther      drop table #Temp1 "
                , ("00" + ddlModel.SelectedItem.Value), ddlLoan.SelectedItem.Value, ddlSubType.SelectedItem.Value, mktqrf, mktqrt, start.ToString("yyyy-MM"), end.ToString("yyyy-MM"), ddlchar.SelectedItem.Value);

                                        DataTable dtb = new DataTable();

                                        if (ddlModel.SelectedItem.Value == "7")
                                        {
                                            String strConnString = ConfigurationManager.ConnectionStrings["GSBSQLServer"].ConnectionString;
                                            SqlConnection con = new SqlConnection(strConnString);
                                            con.Open();
                                            SqlCommand command;

                                            if ((ddlchar.SelectedItem.Value == "cdob") || (ddlchar.SelectedItem.Value == "tworktime_month"))
                                            {
                                                command = new SqlCommand("sp_FrontEnd_CharacteristicAnalysisReport_AgeTwork", con);

                                                //DateTime date_open = DateTime.ParseExact(txtOpenDate.Text, "dd/MM/yyyy", null);
                                                //DateTime date_close = DateTime.ParseExact(txtCloseDate.Text, "dd/MM/yyyy", null);


                                                command.Parameters.Add(new SqlParameter("@start", ddlOpenDateYear.SelectedValue.ToString() + (ddlOpenDateMonth.SelectedIndex + 1).ToString("00")));
                                                command.Parameters.Add(new SqlParameter("@end", ddlOpenDateYearend.SelectedValue.ToString() + (ddlOpenDateMonthend.SelectedIndex + 1).ToString("00")));
                                                command.Parameters.Add(new SqlParameter("@ddlLoan", ddlLoan.SelectedItem.Value));
                                                command.Parameters.Add(new SqlParameter("@ddlSubType", ddlSubType.SelectedItem.Value));
                                                command.Parameters.Add(new SqlParameter("@mktqrf", ddlMarketFrm.Text));
                                                command.Parameters.Add(new SqlParameter("@mktqrt", ddlMarketTo.Text));
                                                command.Parameters.Add(new SqlParameter("@ddlModel", "00"+ddlModel.SelectedValue));
                                                command.Parameters.Add(new SqlParameter("@ddlchar", ddlchar.SelectedValue));
                                                command.CommandType = CommandType.StoredProcedure;
                                                command.CommandTimeout = 36000;
                                                SqlDataAdapter da = new SqlDataAdapter(command);

                                                da.Fill(dtb);
                                            }
                                            else if (ddlchar.SelectedItem.Value == "career")
                                            {
                                                command = new SqlCommand("sp_FrontEnd_CharacteristicAnalysisReport_career", con);

                                                //DateTime date_open = DateTime.ParseExact(txtOpenDate.Text, "dd/MM/yyyy", null);
                                                //DateTime date_close = DateTime.ParseExact(txtCloseDate.Text, "dd/MM/yyyy", null);


                                                command.Parameters.Add(new SqlParameter("@start", ddlOpenDateYear.SelectedValue.ToString() + (ddlOpenDateMonth.SelectedIndex + 1).ToString("00")));
                                                command.Parameters.Add(new SqlParameter("@end", ddlOpenDateYearend.SelectedValue.ToString() + (ddlOpenDateMonthend.SelectedIndex + 1).ToString("00")));
                                                command.Parameters.Add(new SqlParameter("@ddlLoan", ddlLoan.SelectedItem.Value));
                                                command.Parameters.Add(new SqlParameter("@ddlSubType", ddlSubType.SelectedItem.Value));
                                                command.Parameters.Add(new SqlParameter("@mktqrf", ddlMarketFrm.Text));
                                                command.Parameters.Add(new SqlParameter("@mktqrt", ddlMarketTo.Text));
                                                command.Parameters.Add(new SqlParameter("@ddlModel", "00" + ddlModel.SelectedValue));
                                                command.CommandType = CommandType.StoredProcedure;
                                                command.CommandTimeout = 36000;
                                                SqlDataAdapter da = new SqlDataAdapter(command);
                                                da.Fill(dtb);
                                            }
                                            else if (ddlchar.SelectedItem.Value == "edustatus_id")
                                            {
                                                command = new SqlCommand("sp_FrontEnd_CharacteristicAnalysisReport_Education", con);

                                                //DateTime date_open = DateTime.ParseExact(txtOpenDate.Text, "dd/MM/yyyy", null);
                                                //DateTime date_close = DateTime.ParseExact(txtCloseDate.Text, "dd/MM/yyyy", null);


                                                command.Parameters.Add(new SqlParameter("@start", ddlOpenDateYear.SelectedValue.ToString() + (ddlOpenDateMonth.SelectedIndex + 1).ToString("00")));
                                                command.Parameters.Add(new SqlParameter("@end", ddlOpenDateYearend.SelectedValue.ToString() + (ddlOpenDateMonthend.SelectedIndex + 1).ToString("00")));
                                                command.Parameters.Add(new SqlParameter("@ddlLoan", ddlLoan.SelectedItem.Value));
                                                command.Parameters.Add(new SqlParameter("@ddlSubType", ddlSubType.SelectedItem.Value));
                                                command.Parameters.Add(new SqlParameter("@mktqrf", ddlMarketFrm.Text));
                                                command.Parameters.Add(new SqlParameter("@mktqrt", ddlMarketTo.Text));
                                                command.Parameters.Add(new SqlParameter("@ddlModel", "00" + ddlModel.SelectedValue));
                                                command.CommandType = CommandType.StoredProcedure;
                                                command.CommandTimeout = 36000;
                                                SqlDataAdapter da = new SqlDataAdapter(command);
                                                da.Fill(dtb);
                                            }
                                            else if (ddlchar.SelectedItem.Value == "marital")
                                            {
                                                command = new SqlCommand("sp_FrontEnd_CharacteristicAnalysisReport_Marital", con);

                                                //DateTime date_open = DateTime.ParseExact(txtOpenDate.Text, "dd/MM/yyyy", null);
                                                //DateTime date_close = DateTime.ParseExact(txtCloseDate.Text, "dd/MM/yyyy", null);


                                                command.Parameters.Add(new SqlParameter("@start", ddlOpenDateYear.SelectedValue.ToString() + (ddlOpenDateMonth.SelectedIndex + 1).ToString("00")));
                                                command.Parameters.Add(new SqlParameter("@end", ddlOpenDateYearend.SelectedValue.ToString() + (ddlOpenDateMonthend.SelectedIndex + 1).ToString("00")));
                                                command.Parameters.Add(new SqlParameter("@ddlLoan", ddlLoan.SelectedItem.Value));
                                                command.Parameters.Add(new SqlParameter("@ddlSubType", ddlSubType.SelectedItem.Value));
                                                command.Parameters.Add(new SqlParameter("@mktqrf", ddlMarketFrm.Text));
                                                command.Parameters.Add(new SqlParameter("@mktqrt", ddlMarketTo.Text));
                                                command.Parameters.Add(new SqlParameter("@ddlModel", "00" + ddlModel.SelectedValue));
                                                command.CommandType = CommandType.StoredProcedure;
                                                command.CommandTimeout = 36000;
                                                SqlDataAdapter da = new SqlDataAdapter(command);
                                                da.Fill(dtb);
                                            }







                                            Session["dtb"] = dtb;
                                            gvTable.DataSource = dtb;
                                            gvTable.DataBind();
                                        }
                                        else
                                        {
                                            Session["dtb"] = conn.ExcuteSQL(appQuery1);
                                            gvTable.DataSource = Session["dtb"];
                                            gvTable.DataBind();
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
        }

        protected void btnExcel_Click(object sender, EventArgs e)
        {
            if (gvTable.Rows.Count > 0)
            {
                DataTable dt = (DataTable)Session["dtb"];
                Session["rptData"] = dt; //Report Data

                //Report header
                Session["sHeaderRpt1"] = "ประเภทโมเดล : " + ddlModel.SelectedItem.Text;
                Session["sHeaderRpt2"] = "ประเภทสินเชื่อ / ประเภทสินเชื่อย่อย : " + ddlLoan.SelectedItem.Text + " / " + ddlSubType.SelectedItem.Text;

                if (ddlMarketFrm.SelectedIndex == 0)
                    Session["sHeaderRpt3"] = "Market Code : " + ddlMarketFrm.SelectedItem.Text;
                else
                    Session["sHeaderRpt3"] = "Market Code : " + ddlMarketFrm.SelectedItem.Text + " ถึง " + ddlMarketTo.SelectedItem.Text;

                Session["sHeaderRpt4"] = "วันที่เปิดใบคำขอสินเชื่อ : " + ddlOpenDateMonth.SelectedValue + " " + ddlOpenDateYear.SelectedItem + " ถึง " + ddlOpenDateMonthend.SelectedValue + " " + ddlOpenDateYearend.SelectedItem;

                Session["sHeaderRpt5"] = "ประเภทคุณลักษณะ : " + ddlchar.SelectedItem.Text;

                ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "aa", "window.open('ReportLayOut/frmPreviewFontRpt.aspx?ReportId=93');", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScript", "appPop('โปรดเลือกตัวเลือก','กรุณากดปุ่มแสดงข้อมูลก่อน')", true);
                return;
            }
        }


        protected void Cancel_Click(object sender, EventArgs e)
        {
            ddlModel.ClearSelection();
            ddlLoan.ClearSelection();
            ddlSubType.ClearSelection();
            ddlMarketFrm.ClearSelection();
            ddlMarketTo.ClearSelection();
            ddlSubType.Enabled = false;
            ddlOpenDateYear.ClearSelection();
            ddlOpenDateMonth.ClearSelection();
            ddlOpenDateYearend.ClearSelection();
            ddlOpenDateMonthend.ClearSelection();
            //txtOpenDate.Text = string.Empty;
            //txtCloseDate.Text = string.Empty;
        }
        protected void gvTable_PageChanging(object sender, GridViewPageEventArgs e)
        {
            gvTable.PageIndex = e.NewPageIndex;
            Search_Click(null, null);
        }

        protected void gvTable_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (ddlchar.SelectedItem.Value == "career" || ddlchar.SelectedItem.Value == "edustatus_id")
                {
                    e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Left;
                }
                else 
                {
                    e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
                }
            }
        }

        #endregion Protected Method


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

            //Report header
            Session["sHeaderRpt1"] = "ประเภทโมเดล : " + ddlModel.SelectedItem.Text;
            Session["sHeaderRpt2"] = "ประเภทสินเชื่อ / ประเภทสินเชื่อย่อย : " + ddlLoan.SelectedItem.Text + " / " + ddlSubType.SelectedItem.Text;

            if (ddlMarketFrm.SelectedIndex == 0)
                Session["sHeaderRpt3"] = "Market Code : " + ddlMarketFrm.SelectedItem.Text;
            else
                Session["sHeaderRpt3"] = "Market Code : " + ddlMarketFrm.SelectedItem.Text + " ถึง " + ddlMarketTo.SelectedItem.Text;

            Session["sHeaderRpt4"] = "วันที่เปิดใบคำขอสินเชื่อ : " + ddlOpenDateMonth.SelectedValue + " " + ddlOpenDateYear.SelectedItem + " ถึง " + ddlOpenDateMonthend.SelectedValue + " " + ddlOpenDateYearend.SelectedItem;

            Session["sHeaderRpt5"] = "ประเภทคุณลักษณะ : " + ddlchar.SelectedItem.Text;

            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "aa", "window.open('ReportLayOut/frmPreviewFontRpt.aspx?ReportId=3');", true);
        }
        //-----------------------------------------------------------

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