using System;
using GSB.Class;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using System.Data;
using System.Text;

namespace GSB.LoanApp
{
    public partial class LoanApp : System.Web.UI.Page
    {
        SQLToDataTable conn = new SQLToDataTable();

        #region Page_Load

        protected void Page_Load(object sender, EventArgs e)
        {
            //Get user name//------------------------------------------------
            Label userFirstname = (Label)Master.FindControl("userFirstname");
            Session["userFirstname"] = userFirstname.Text;
            Session["RequestFor"] = "Customer";
            //---------------------------------------------------------------

            if (!Page.IsPostBack)
            {
                ResultTable.Visible = false;

                string PageName = string.Empty;
                if (Request.QueryString["req"] == "Analysis")
                {
                    PageName = "ตรวจสอบผลการวิเคราะห์สินเชื่อ";
                    Session["RequestFor"] = "Analysis"; //Add for crystal report
                }
                else
                {
                    PageName = "ข้อมูลลูกค้าสินเชื่อ";
                    Session["RequestFor"] = "Customer"; //Add for crystal report
                }

                ReportName1.Text = PageName;
                ReportName2.Text = PageName;

                LoadModel();
                //LoadLoan();
                //LoadBranch();
            }
        }

        #endregion Page_Load

        #region Privated Method

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
            LoadBranch();
        }



        //private void LoadLoan()
        //{
        //    string Query = string.Format("select distinct " +
        //            "LOAN_CD [LOAN_CD],  " +
        //            "LOAN_CD+' - '+LOAN_NAME [LOAN_NAME] from ST_LOANTYPE where ACTIVE_FLAG = '1' " +
        //            "union select null, 'เลือกทั้งหมด...' " +
        //            "order by LOAN_CD ASC");

        //    ddlLoan.DataSource = conn.ExcuteSQL(Query);
        //    ddlLoan.DataBind();
        //    ddlLoan.Enabled = true;
        //    ddlSubType.Items.Insert(0, new ListItem("เลือกทั้งหมด...", string.Empty));
        //    ddlMarketFrm.Items.Insert(0, new ListItem("เลือกทั้งหมด...", string.Empty));
        //    ddlMarketTo.Items.Insert(0, new ListItem("เลือกทั้งหมด...", string.Empty));
        //}

        private void LoadBranch()
        {//RIGHT ('00000'+ CAST (BRANCH_CD AS varchar), 5)
            string Query = string.Format("select distinct " +
                    "CONVERT(int,BRANCH_CD) [BRANCH_CD], " + 
                    "BRANCH_CD+' - '+BRANCH_NAME [BRANCH_NAME] from ST_BRANCH where ACTIVE_FLAG = '1' " +
                    "union select null,'เลือกทั้งหมด...' " +
                    "order by BRANCH_CD ASC");


            if (ddlModel.SelectedItem.Value == "7")
            {
                Query = "select 'MyMo' as [BRANCH_CD],'MyMo' as [BRANCH_NAME]";
            }

            ddlBranch.DataSource = conn.ExcuteSQL(Query);
            ddlBranch.DataBind();
            ddlBranch.Enabled = true;
        }

        private void DataBound()
        {
            string ResultFilter = FilterBySearch();
            // select with distinct
            /*
            string appQuery = string.Format("select distinct APP_NO, APP_SEQ, APP_DATE " +
                                            "from LN_APP where LEN(APP_NO)>0 and " +
                                            "APP_SEQ = (select MAX(APP_SEQ) from LN_APP AS LN_APP2 where LN_APP.APP_NO = LN_APP2.APP_NO)  {0} " +
                                            "order by APP_DATE desc", ResultFilter);
             */
            //Tai 2012-03-22
            ///*
            //string appQuery = string.Format(
            //                               "SELECT distinct LN_APP.APP_NO, LN_APP.APP_SEQ, LN_APP.APP_DATE " +
            //                               "FROM LN_APP " +
            //                               " INNER JOIN LN_GRADE B ON LN_APP.APP_NO=B.APP_NO AND LN_APP.APP_SEQ=B.APP_SEQ AND ISNULL(B.SCORE,'')<>''" +
            //                               " LEFT JOIN LN_APPCIF ON LN_APP.APP_NO=LN_APPCIF.CBS_APP_NO " +
            //                               "WHERE LEN(LN_APP.APP_NO)>0 AND " +
            //                               " LN_APP.APP_SEQ = (select MAX(APP_SEQ) from LN_APP AS LN_APP2 where LN_APP.APP_NO = LN_APP2.APP_NO)  {0} " +
            //                               " AND EXISTS (SELECT * FROM LN_GRADE B WHERE LN_APP.APP_NO=B.APP_NO AND LN_APP.APP_SEQ=B.APP_SEQ ) AND ISNULL(B.SCORE,'')<>'' " +
            //                               "ORDER BY LN_APP.APP_DATE desc", ResultFilter);

            //AR Soft 2014-12-19
            string appQuery = string.Format(
                                           "SELECT distinct LN_APP.APP_NO, LN_APP.APP_SEQ, LN_APP.APP_DATE " +
                                           "FROM LN_APP, LN_GRADE, LN_APPCIF, " +
                                           " (" +
                                           "  SELECT APP_NO,MAX(APP_SEQ) AS APP_SEQ" +
                                           "  FROM LN_APP" +
                                           "  GROUP BY APP_NO" +
                                           " ) AS LN_APP_Current " +
                                           "WHERE " +
                                           " LEN(LN_APP.APP_NO) > 0" +
                                           " AND (LN_APP.APP_NO = LN_APP_Current.APP_NO  AND LN_APP.APP_SEQ=LN_APP_Current.APP_SEQ)" +
                                           " AND (LN_APP_Current.APP_NO=LN_GRADE.APP_NO AND LN_APP_Current.APP_SEQ=LN_GRADE.APP_SEQ AND ISNULL(LN_GRADE.SCORE,'')<>'')" +
                                           " AND (LN_APP_Current.APP_NO=LN_APPCIF.CBS_APP_NO AND LN_APP_Current.APP_SEQ=LN_APPCIF.APP_SEQ) {0} " +
                                           "ORDER BY LN_APP.APP_DATE desc", ResultFilter);            
           
            if (ResultFilter != "dummy")
            {
                gvTable.DataSource = conn.ExcuteSQL(appQuery);
                gvTable.DataBind();
                ResultTable.Visible = true;

                if (gvTable.Rows.Count > 0)
                {
                    RowHeader.Visible = true;
                    HeadTxtLoan.Visible = true;
                }
                else
                    HeadTxtLoan.Visible = false;
                
            }
            else
            {
                ResultTable.Visible = false;
                gvTable.DataBind();
            }
        }


        private void DataBoundMymo()
        {
            string ResultFilter = FilterBySearchMymo();
            // select with distinct
            /*
            string appQuery = string.Format("select distinct APP_NO, APP_SEQ, APP_DATE " +
                                            "from LN_APP where LEN(APP_NO)>0 and " +
                                            "APP_SEQ = (select MAX(APP_SEQ) from LN_APP AS LN_APP2 where LN_APP.APP_NO = LN_APP2.APP_NO)  {0} " +
                                            "order by APP_DATE desc", ResultFilter);
             */
            //Tai 2012-03-22
            ///*
            //string appQuery = string.Format(
            //                               "SELECT distinct LN_APP.APP_NO, LN_APP.APP_SEQ, LN_APP.APP_DATE " +
            //                               "FROM LN_APP " +
            //                               " INNER JOIN LN_GRADE B ON LN_APP.APP_NO=B.APP_NO AND LN_APP.APP_SEQ=B.APP_SEQ AND ISNULL(B.SCORE,'')<>''" +
            //                               " LEFT JOIN LN_APPCIF ON LN_APP.APP_NO=LN_APPCIF.CBS_APP_NO " +
            //                               "WHERE LEN(LN_APP.APP_NO)>0 AND " +
            //                               " LN_APP.APP_SEQ = (select MAX(APP_SEQ) from LN_APP AS LN_APP2 where LN_APP.APP_NO = LN_APP2.APP_NO)  {0} " +
            //                               " AND EXISTS (SELECT * FROM LN_GRADE B WHERE LN_APP.APP_NO=B.APP_NO AND LN_APP.APP_SEQ=B.APP_SEQ ) AND ISNULL(B.SCORE,'')<>'' " +
            //                               "ORDER BY LN_APP.APP_DATE desc", ResultFilter);

            //AR Soft 2014-12-19
            string appQuery = string.Format(
                                           "SELECT distinct MYMO_LN_APP.APP_NO, MYMO_LN_APP.APP_SEQ, MYMO_LN_APP.APP_DATE " +
                                           "FROM MYMO_LN_APP inner join  " +
                                           " (" +
                                           "  SELECT APP_NO,MAX(APP_SEQ) AS APP_SEQ" +
                                           "  FROM MYMO_LN_APP" +
                                           "  GROUP BY APP_NO" +
                                           " ) AS MYMO_LN_APP_Current on MYMO_LN_APP.APP_NO = MYMO_LN_APP_Current.APP_NO  AND MYMO_LN_APP.APP_SEQ=MYMO_LN_APP_Current.APP_SEQ" +
                                           " inner join  MYMO_LN_GRADE on MYMO_LN_APP_Current.APP_NO=MYMO_LN_GRADE.APP_NO AND MYMO_LN_APP_Current.APP_SEQ=MYMO_LN_GRADE.APP_SEQ AND ISNULL(MYMO_LN_GRADE.SCORE,'')<>'' " +
                                           " left join  MYMO_LN_APPCIF on MYMO_LN_APP_Current.APP_NO=MYMO_LN_APPCIF.CBS_APP_NO AND MYMO_LN_APP_Current.APP_SEQ=MYMO_LN_APPCIF.APP_SEQ " +
                                           "WHERE " +
                                           " LEN(MYMO_LN_APP.APP_NO) > 0" +
                                           "    {0}" +
                                           "ORDER BY MYMO_LN_APP.APP_DATE desc", ResultFilter);

            if (ResultFilter != "dummy")
            {
                gvTable.DataSource = conn.ExcuteSQL(appQuery);
                gvTable.DataBind();
                ResultTable.Visible = true;

                if (gvTable.Rows.Count > 0)
                {
                    RowHeader.Visible = true;
                    HeadTxtLoan.Visible = true;
                }
                else
                    HeadTxtLoan.Visible = false;

            }
            else
            {
                ResultTable.Visible = false;
                gvTable.DataBind();
            }
        }
        private string FilterBySearch()
        {
            string sqlQuery = string.Empty;
            IFormatProvider format = new CultureInfo("en-GB");
            DateTime dateOpenLoan = DateTime.MinValue;
            DateTime dateCloseLoan = DateTime.MinValue;

            if (ddlLoan.SelectedItem.Value != "")
                sqlQuery += String.Format(" AND (LN_APP.LOAN_CD = '{0}')", ddlLoan.SelectedItem.Value);

            if (ddlSubType.SelectedItem.Value != "")
                sqlQuery += String.Format(" AND (LN_APP.STYPE_CD = '{0}')", ddlSubType.SelectedItem.Value);

            if (ddlMarketFrm.SelectedItem.Value != "")
                sqlQuery += String.Format(" AND (LN_APP.MARKET_CD >= '{0}')", ddlMarketFrm.SelectedItem.Value);

            if (ddlMarketTo.SelectedItem.Value != "")
                sqlQuery += String.Format(" AND (LN_APP.MARKET_CD <= '{0}')", ddlMarketTo.SelectedItem.Value);

            if (ddlBranch.SelectedItem.Value != "")
                sqlQuery += String.Format(" AND (LN_APP.BRANCH_CD = '{0}')", ddlBranch.SelectedItem.Value);

            if (txtAppID.Text != "")
               //Modify by Page'Tai on 09-Apr-2014 by add "LN_APP."
                sqlQuery += String.Format(" AND (LN_APP.APP_NO = '{0}')", txtAppID.Text);

            if (txtNID.Text != "")
                //Modify by Page'Tai on 09-Apr-2014 by add "LN_APP."
                sqlQuery += String.Format(" AND (LN_APPCIF.CBS_CITIZID = '{0}')", txtNID.Text.Trim());

            if (DateTime.TryParse(txtOpenDate.Text, format, DateTimeStyles.None, out dateOpenLoan) &&
                DateTime.TryParse(txtCloseDate.Text, format, DateTimeStyles.None, out dateCloseLoan))
            {
                if (txtOpenDate.Text != null && txtCloseDate.Text != null && DateTime.Compare(dateOpenLoan, dateCloseLoan) <= 0)
                {    
                    //Tai 2014-08-29
                    //sqlQuery += String.Format(" AND (APP_DATE BETWEEN '{0}' AND '{1}')", dateOpenLoan.Year.ToString() + "-" + dateOpenLoan.Month.ToString() + "-" +
                    sqlQuery += String.Format(" AND (LN_APP.APP_DATE BETWEEN CONVERT(VARCHAR,YEAR(CONVERT(DATETIME,'{0}'))-543)+'-'+RIGHT('00'+CONVERT(VARCHAR,MONTH(CONVERT(DATETIME,'{0}'))),2)+'-'+RIGHT('00'+CONVERT(VARCHAR,DAY(CONVERT(DATETIME,'{0}'))),2) AND CONVERT(VARCHAR,YEAR(CONVERT(DATETIME,'{1}'))-543)+'-'+RIGHT('00'+CONVERT(VARCHAR,MONTH(CONVERT(DATETIME,'{1}'))),2)+'-'+RIGHT('00'+CONVERT(VARCHAR,DAY(CONVERT(DATETIME,'{1}'))),2))", dateOpenLoan.Year.ToString() + "-" + dateOpenLoan.Month.ToString() + "-" +                    
                    //Tai 2014-08-29
                        dateOpenLoan.Day.ToString(), dateCloseLoan.Year.ToString() + "-" + dateCloseLoan.Month.ToString() + "-" + dateCloseLoan.Day.ToString());
                }
                else
                {
                    sqlQuery = "dummy";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScript", "appPop('วันที่เปิดใบคำขอสินเชื่อไม่ถูกต้อง?','วันที่เปิดเริ่มต้น ต้องน้อยกว่าหรือเท่ากับ วันที่เปิดสุดท้าย')", true);
                }
            }

            return sqlQuery.ToString();
        }

        private string FilterBySearchMymo()
        {
            string sqlQuery = string.Empty;
            IFormatProvider format = new CultureInfo("en-GB");
            DateTime dateOpenLoan = DateTime.MinValue;
            DateTime dateCloseLoan = DateTime.MinValue;

            if (ddlLoan.SelectedItem.Value != "")
                sqlQuery += String.Format(" AND (MYMO_LN_APP.LOAN_CD = '{0}')", ddlLoan.SelectedItem.Value);

            if (ddlSubType.SelectedItem.Value != "")
                sqlQuery += String.Format(" AND (MYMO_LN_APP.STYPE_CD = '{0}')", ddlSubType.SelectedItem.Value);

            if (ddlMarketFrm.SelectedItem.Value != "")
                sqlQuery += String.Format(" AND (MYMO_LN_APP.MARKET_CD >= '{0}')", ddlMarketFrm.SelectedItem.Value);

            if (ddlMarketTo.SelectedItem.Value != "")
                sqlQuery += String.Format(" AND (MYMO_LN_APP.MARKET_CD <= '{0}')", ddlMarketTo.SelectedItem.Value);

            if (txtAppID.Text != "")
                //Modify by Page'Tai on 09-Apr-2014 by add "LN_APP."
                sqlQuery += String.Format(" AND (MYMO_LN_APP.APP_NO = '{0}')", txtAppID.Text);

            if (txtNID.Text != "")
                //Modify by Page'Tai on 09-Apr-2014 by add "LN_APP."
                sqlQuery += String.Format(" AND (MYMO_LN_APPCIF.CBS_CITIZID = '{0}')", txtNID.Text.Trim());

            if (DateTime.TryParse(txtOpenDate.Text, format, DateTimeStyles.None, out dateOpenLoan) &&
                DateTime.TryParse(txtCloseDate.Text, format, DateTimeStyles.None, out dateCloseLoan))
            {
                if (txtOpenDate.Text != null && txtCloseDate.Text != null && DateTime.Compare(dateOpenLoan, dateCloseLoan) <= 0)
                {
                    //Tai 2014-08-29
                    //sqlQuery += String.Format(" AND (APP_DATE BETWEEN '{0}' AND '{1}')", dateOpenLoan.Year.ToString() + "-" + dateOpenLoan.Month.ToString() + "-" +
                    sqlQuery += String.Format(" AND (MYMO_LN_APP.APP_DATE BETWEEN CONVERT(VARCHAR,YEAR(CONVERT(DATETIME,'{0}'))-543)+'-'+RIGHT('00'+CONVERT(VARCHAR,MONTH(CONVERT(DATETIME,'{0}'))),2)+'-'+RIGHT('00'+CONVERT(VARCHAR,DAY(CONVERT(DATETIME,'{0}'))),2) AND CONVERT(VARCHAR,YEAR(CONVERT(DATETIME,'{1}'))-543)+'-'+RIGHT('00'+CONVERT(VARCHAR,MONTH(CONVERT(DATETIME,'{1}'))),2)+'-'+RIGHT('00'+CONVERT(VARCHAR,DAY(CONVERT(DATETIME,'{1}'))),2))", dateOpenLoan.Year.ToString() + "-" + dateOpenLoan.Month.ToString() + "-" +
                        //Tai 2014-08-29
                        dateOpenLoan.Day.ToString(), dateCloseLoan.Year.ToString() + "-" + dateCloseLoan.Month.ToString() + "-" + dateCloseLoan.Day.ToString());
                }
                else
                {
                    sqlQuery = "dummy";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScript", "appPop('วันที่เปิดใบคำขอสินเชื่อไม่ถูกต้อง?','วันที่เปิดเริ่มต้น ต้องน้อยกว่าหรือเท่ากับ วันที่เปิดสุดท้าย')", true);
                }
            }

            return sqlQuery.ToString();
        }

        #endregion Privated Method

        #region Protected Method

        protected void LoadSubType(object sender, EventArgs e)
        {
            string Query = "";

            if (ddlModel.SelectedItem.Value == "7")
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
                         "STYPE_CD [STYPE_CD], "  +
                         "STYPE_CD+' - '+STYPER_NAME [STYPER_NAME] from ST_LOANSTYPE a left join [dbo].[ST_MKTDFTMYMO] b on a.STYPE_CD = b.subtype where ACTIVE_FLAG = '1' and LOAN_CD = '{0}' and b.subtype is null " +
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

        protected void gvTable_RowBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var APP_NO = (Label)(e.Row.FindControl("Key_APP_NO"));
                var APP_SEQ = (Label)(e.Row.FindControl("Key_APP_SEQ"));

                // select with distinct
                /*
                 string appQuery = string.Format("select distinct APP_NO, APP_SEQ, APP_DATE, LOAN_AMOUNT, " +
                                                 "(select distinct LOAN_NAME from ST_LOANTYPE where LN_APP.LOAN_CD = LOAN_CD) as LOAN_NAME, " +
                                                 "(select distinct STYPER_NAME from ST_LOANSTYPE where LN_APP.STYPE_CD = STYPE_CD) as STYPE_NAME, " +
                                                 "(select distinct MARKET_NAME from ST_LOANMKT where LN_APP.MARKET_CD = MARKET_CD) as MARKET_NAME, " +
                                                 "(select distinct BRANCH_NAME from ST_BRANCH where LN_APP.BRANCH_CD = BRANCH_CD) as BRANCH_NAME, " +
                                                 "ISNULL((select distinct top 1 CBS_APPROVE_CD from CBS_LN_APP where CBS_APP_NO = APP_NO),LNSTATUS) + '-' + ISNULL((select distinct top 1 APPROVE_NAME FROM ST_APPTATUS WHERE APPROVE_CD = (ISNULL((select distinct top 1 CBS_APPROVE_CD from CBS_LN_APP where CBS_APP_NO = APP_NO),LNSTATUS))),'-') AS LNSTATUS, " +
                                                 "ISNULL((select distinct MIN(GRADE) from LN_GRADE where LN_APP.APP_NO = APP_NO and LN_APP.APP_SEQ = APP_SEQ),'-') AS GRADE, " +
                                                 "ISNULL(convert(varchar,(select distinct MAX(SCORE) from LN_GRADE where LN_APP.APP_NO = APP_NO and LN_APP.APP_SEQ = APP_SEQ)),'Score Not Req') AS SCORE " +
                                                 "from LN_APP where LEN(APP_NO)>0 and LEN(APP_SEQ)>0 and APP_NO = '{0}' and APP_SEQ = '{1}' " +
                                                 "order by APP_DATE desc", APP_NO.Text, APP_SEQ.Text);
                 */
                //Tai 2014-03-22
                ///*
                string appQuery = string.Empty;

                if (ddlModel.SelectedValue == "7")
                {
                    appQuery = string.Format("select distinct a.APP_NO, a.APP_SEQ, a.APP_DATE, LOAN_AMOUNT, " +
                            "(select distinct LOAN_NAME from ST_LOANTYPE where a.LOAN_CD = LOAN_CD) as LOAN_NAME, " +
                            "(select distinct STYPER_NAME from ST_LOANSTYPE where a.STYPE_CD = STYPE_CD) as STYPE_NAME, " +
                            "(select distinct MARKET_NAME from ST_LOANMKT where a.MARKET_CD = MARKET_CD " +
                            //Tai 2014-03-22
                            "and a.LOAN_CD = LOAN_CD) " +
                            //Tai 2014-03-22
                            "as MARKET_NAME, " +
                            "'MyMo' as BRANCH_NAME, " +
                            "case when a.CBS_APPROVE_CD = 3 then '3-Approved' when a.CBS_APPROVE_CD = 4 then '4-Rejected' when a.CBS_APPROVE_CD = 0 then cast(a.CBS_APPROVE_CD as varchar(1))+'-Other' else '-' end AS LNSTATUS, " +
                            "ISNULL((select distinct MIN(GRADE) from MYMO_LN_GRADE where a.APP_NO = APP_NO and a.APP_SEQ = APP_SEQ),'-') AS GRADE, " +
                            "ISNULL(convert(varchar,(select distinct MAX(SCORE) from MYMO_LN_GRADE where a.APP_NO = APP_NO and a.APP_SEQ = APP_SEQ)),'Score Not Req') AS SCORE " +
                            "from MYMO_LN_APP a " +
                            "inner join MYMO_LN_GRADE B ON A.APP_NO=B.APP_NO AND A.APP_SEQ=B.APP_SEQ AND ISNULL(B.SCORE,'')<>''" +
                            "where LEN(a.APP_NO)>0 and LEN(a.APP_SEQ)>0 and a.APP_NO = '{0}' and a.APP_SEQ = '{1}' " +
                            "order by a.APP_DATE desc", APP_NO.Text, APP_SEQ.Text);
                }
                else
                {
                    appQuery = string.Format("select distinct a.APP_NO, a.APP_SEQ, a.APP_DATE, LOAN_AMOUNT, " +
                                                "(select distinct LOAN_NAME from ST_LOANTYPE where a.LOAN_CD = LOAN_CD) as LOAN_NAME, " +
                                                "(select distinct STYPER_NAME from ST_LOANSTYPE where a.STYPE_CD = STYPE_CD) as STYPE_NAME, " +
                                                "(select distinct MARKET_NAME from ST_LOANMKT where a.MARKET_CD = MARKET_CD " +
                    //Tai 2014-03-22
                                                "and a.LOAN_CD = LOAN_CD) " +
                    //Tai 2014-03-22
                                                "as MARKET_NAME, " +
                                                "(select distinct BRANCH_NAME from ST_BRANCH where a.BRANCH_CD = BRANCH_CD) as BRANCH_NAME, " +
                                                "ISNULL((select distinct top 1 CBS_APPROVE_CD from CBS_LN_APP where CBS_APP_NO = a.APP_NO),LNSTATUS) + '-' + ISNULL((select distinct top 1 APPROVE_NAME FROM ST_APPTATUS WHERE APPROVE_CD = (ISNULL((select distinct top 1 CBS_APPROVE_CD from CBS_LN_APP where CBS_APP_NO = a.APP_NO),LNSTATUS))),'-') AS LNSTATUS, " +
                                                "ISNULL((select distinct MIN(GRADE) from LN_GRADE where a.APP_NO = APP_NO and a.APP_SEQ = APP_SEQ),'-') AS GRADE, " +
                                                "ISNULL(convert(varchar,(select distinct MAX(SCORE) from LN_GRADE where a.APP_NO = APP_NO and a.APP_SEQ = APP_SEQ)),'Score Not Req') AS SCORE " +
                                                "from LN_APP a " +
                                                "inner join LN_GRADE B ON A.APP_NO=B.APP_NO AND A.APP_SEQ=B.APP_SEQ AND ISNULL(B.SCORE,'')<>''" +
                                                "where LEN(a.APP_NO)>0 and LEN(a.APP_SEQ)>0 and a.APP_NO = '{0}' and a.APP_SEQ = '{1}' " +
                                                "order by a.APP_DATE desc", APP_NO.Text, APP_SEQ.Text);

                }

                //*/
                var gvApp = (Repeater)e.Row.FindControl("gvApp");
                gvApp.DataSource = conn.ExcuteSQL(appQuery);
                gvApp.DataBind();
            }
        }

        protected void gvApp_ItemBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var APP_REQ = string.Empty;
                var APP_NO = (Label)(e.Item.FindControl("Sub_APP_NO"));
                var APP_SEQ = (Label)(e.Item.FindControl("Sub_APP_SEQ"));
                if (Request.QueryString["req"] == "Analysis")
                    APP_REQ = "Analysis";
                else
                    APP_REQ = "Customer";

                var LoanTable = (Table)(e.Item.FindControl("LoanTable"));

                if (ddlModel.SelectedValue == "7")
                {
                    LoanTable.Rows[0].Attributes.Add("onclick", "appOpenMymo('" + APP_NO.Text + "','" + APP_SEQ.Text + "','" + APP_REQ + "')");

                }
                else
                {
                    LoanTable.Rows[0].Attributes.Add("onclick", "appOpen('" + APP_NO.Text + "','" + APP_SEQ.Text + "','" + APP_REQ + "')");

                }
            }
        }

        protected void gvTable_PageChanging(object sender, GridViewPageEventArgs e)
        {
            gvTable.PageIndex = e.NewPageIndex;
            if (ddlModel.SelectedItem.Value == "7")
            {
                DataBoundMymo();
            }
            else
            {
                DataBound();
            }
        }

        protected void Search_Click(object sender, EventArgs e)
        {
            Session["Model"] = ddlModel.SelectedValue.ToString();

            DateTime start = new DateTime();
            DateTime end = new DateTime();
            IFormatProvider format = new CultureInfo("en-GB");
            DateTime dateOpenLoan = DateTime.MinValue;
            DateTime dateCloseLoan = DateTime.MinValue;
            StringBuilder sqlQuery = new StringBuilder();
            string mktqrf = "";
            string mktqrt = "";

            if (string.IsNullOrEmpty(txtNID.Text) && string.IsNullOrEmpty(txtAppID.Text))
            {
                if (ddlModel.SelectedItem.Value == "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScript", "appPop('โปรดเลือกตัวเลือก','กรุณาเลือกเงื่อนไขการค้นหาให้ครบถ้วน')", true);
                    return;
                }

                if (ddlLoan.SelectedItem.Value == "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScript", "appPop('โปรดเลือกตัวเลือก','กรุณาเลือกเงื่อนไขการค้นหาให้ครบถ้วน')", true);
                    return;
                }

                if (ddlSubType.SelectedItem.Value == "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScript", "appPop('โปรดเลือกตัวเลือก','กรุณาเลือกเงื่อนไขการค้นหาให้ครบถ้วน')", true);
                    return;
                }

                if (txtOpenDate.Text == "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScript", "appPop('โปรดเลือกตัวเลือก','กรุณาเลือกเงื่อนไขการค้นหาให้ครบถ้วน')", true);
                    return;
                }

                if (txtCloseDate.Text == "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScript", "appPop('โปรดเลือกตัวเลือก','กรุณาเลือกเงื่อนไขการค้นหาให้ครบถ้วน')", true);
                    return;
                }

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

                if (ddlMarketFrm.SelectedIndex <= 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScript", "appPop('โปรดเลือกตัวเลือก','กรุณาเลือกเงื่อนไขการค้นหาให้ครบถ้วน')", true);
                    return;
                }
                if (ddlMarketTo.SelectedIndex <= 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScript", "appPop('โปรดเลือกตัวเลือก','กรุณาเลือกเงื่อนไขการค้นหาให้ครบถ้วน')", true);
                    return;
                }

                if ((txtNID.Text.Length != 13) && !string.IsNullOrEmpty(txtNID.Text))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScript", "appPop('โปรดเลือกตัวเลือก','กรุณาระบุเลขบัตรประจำตัวประชาชนให้ถูกต้อง')", true);
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
            }
            else
            {
                if (ddlModel.SelectedItem.Value == "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScript", "appPop('โปรดเลือกตัวเลือก','กรุณาระบุประเภทโมเดล')", true);
                    return;
                }


                if ((txtNID.Text.Length != 13) && !string.IsNullOrEmpty(txtNID.Text))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScript", "appPop('โปรดเลือกตัวเลือก','กรุณาระบุเลขบัตรประจำตัวประชาชนให้ถูกต้อง')", true);
                    return;
                }
            }



            int months = (end.DayOfYear - start.DayOfYear) + 365 * (end.Year - start.Year);


            if (ddlModel.SelectedItem.Value == "7")
            {
                DataBoundMymo();
            }
            else
            {
                DataBound();
            }
            
        }

        protected void Cancel_Click(object sender, EventArgs e)
        {
            ResultTable.Visible = false;
            ddlLoan.ClearSelection();
            ddlBranch.ClearSelection();
            ddlSubType.ClearSelection();
            ddlMarketFrm.ClearSelection();
            ddlMarketTo.ClearSelection();
            txtAppID.Text = string.Empty;
            txtOpenDate.Text = string.Empty;
            txtCloseDate.Text = string.Empty;
            ddlSubType.Enabled = false;
            ddlMarketFrm.Enabled = false;
            ddlMarketTo.Enabled = false;
        }

        protected void gvTable_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ExpanView")
            {
                int pagging = gvTable.PageSize;
                int index = int.Parse(e.CommandArgument.ToString()) % pagging;
                GridViewRow row = gvTable.Rows[index];

                var Image = (ImageButton)row.Cells[0].FindControl("imgExpan");
                var gvCIF = (GridView)row.Cells[1].FindControl("gvCIF");

                if (Image.ImageUrl == "~/Images/Plus.png")
                {
                    var APP_NO = (Label)row.Cells[1].FindControl("Key_APP_NO");
                    var APP_SEQ = (Label)row.Cells[1].FindControl("Key_APP_SEQ");

                    string appQuery = string.Empty;
                    
                    if (ddlModel.SelectedValue == "7")
                    {
                        appQuery = string.Format("select CBS_CIFNO,CBS_CITIZID, " +
                                                    "(ISNULL((select distinct TITLEE_NAME from ST_TITLE where MYMO_LN_APPCIF.CBS_TITLE_CD = TITLE_CD and TITLE_LG = 0)+' ','') + ISNULL(CBS_FNAME+' ','') + ISNULL(CBS_MNAME+' ','') + ISNULL(CBS_SNAME,'')) AS CBS_CIFNAME, " +
                                                    "(select top 1 CIFSCORE from MYMO_LN_CHAR where MYMO_LN_APPCIF.CBS_APP_NO = CBS_APP_NO and MYMO_LN_APPCIF.APP_SEQ = APP_SEQ and MYMO_LN_APPCIF.CBS_CIFNO = CBS_CIFNO) AS CBS_SCORE " +
                                                    "from MYMO_LN_APPCIF where CBS_APP_NO = '{0}' and APP_SEQ = '{1}' ", APP_NO.Text, APP_SEQ.Text);

                    }
                    else
                    {
                        appQuery = string.Format("select CBS_CIFNO,CBS_CITIZID, " +
                                                    "(ISNULL((select distinct TITLEE_NAME from ST_TITLE where LN_APPCIF.CBS_TITLE_CD = TITLE_CD and TITLE_LG = 0)+' ','') + ISNULL(CBS_FNAME+' ','') + ISNULL(CBS_MNAME+' ','') + ISNULL(CBS_SNAME,'')) AS CBS_CIFNAME, " +
                                                    "(select top 1 CIFSCORE from LN_CHAR where LN_APPCIF.CBS_APP_NO = CBS_APP_NO and LN_APPCIF.APP_SEQ = APP_SEQ and LN_APPCIF.CBS_CIFNO = CBS_CIFNO) AS CBS_SCORE " +
                                                    "from LN_APPCIF where CBS_APP_NO = '{0}' and APP_SEQ = '{1}' ", APP_NO.Text, APP_SEQ.Text);

                    }

                    DataTable dtTemp = conn.ExcuteSQL(appQuery);
                    if (dtTemp != null)
                    {
                        if (dtTemp.Rows.Count > 0)
                        {
                            gvCIF.DataSource = dtTemp;
                            gvCIF.DataBind();

                            gvCIF.Visible = true;
                            Image.ImageUrl = "~/Images/Minus.png";
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScript", "appPop('แสดงข้อมูลรายชื่อผู้กู้ร่วม?','ไม่พบข้อมูลผ้กู้ร่วมของ เลขที่ใบคำขอนี้')", true);
                        }
                    }
                }
                else
                {
                    gvCIF.Visible = false;
                    Image.ImageUrl = "~/Images/Plus.png";

                    gvCIF.DataSource = null;
                    gvCIF.DataBind();
                }
            }
            }


        #endregion Protected Method

    }
}