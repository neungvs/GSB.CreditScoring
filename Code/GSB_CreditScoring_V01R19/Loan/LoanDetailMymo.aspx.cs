using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GSB.Class;
using System.Data;

namespace GSB.Loan
{
    public partial class LoanDetailMymo : System.Web.UI.Page
    {
        SQLToDataTable sqlconn = new SQLToDataTable();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Cookies["Credit_Scoring_GSB"] == null)
                Response.Redirect("~/Login.aspx");

            string app_id = Request.QueryString["id"];
            string app_seq = Request.QueryString["seq"];
            string app_req = Request.QueryString["req"];

            BindDataApplication();
        }
        private void BindDataApplication()
        {
            string Query = "";

                Query = string.Format("select distinct a.APP_NO, a.APP_SEQ, a.APP_DATE, Isnull(a.LOAN_AMOUNT,'0') as LOAN_AMOUNT, isnull(a.CREATION_DT,''), " +
                             " cast(isnull(b.CBS_INCOME,'0') as varchar(12))+' บาท' as CBS_INCOME," +
                             " cast(isnull(b.CBS_OTHINCOME,'0') as varchar(12))+' บาท' as CBS_OTHINCOME,a.ACCTYPE," +
                             "cast(isnull(b.CBS_NETINCOMEBUSINESS,'0') as varchar(12))+' บาท' as CBS_NETINCOMEBUSINESS ," +
                             " cast(isnull(b.CBS_CHILD,'0') as varchar(12))+' คน' as CBS_CHILD," +
                             "a.LOAN_CD," +
                             " cast(isnull(b.CBS_MNTEXPEN,'0') as varchar(12))+' บาท' as CBS_MNTEXPEN," +
                             "a.STYPE_CD," +
                             " cast(isnull(b.CBS_LBTEXPEN,'0') as varchar(12))+' บาท' as CBS_LBTEXPEN," +
                             "a.MARKET_CD," +
                             " cast(isnull(b.CBS_BUSEXPEN,'0') as varchar(12))+' บาท' as CBS_BUSEXPEN ," +
                             "a.PURPOSE_CD," +
                             "b.CBS_RESSTATUS_CD + '-' + ISNULL((select distinct top 1 RESSTATUS_NAME from ST_RESSTATUS where b.CBS_RESSTATUS_CD = RESSTATUS_CD),'-') AS CBS_RESSTATUS_CD," +
                             "b.CBS_CIFNO," +
                             " cast(isnull(b.CBS_TWORKTIME,'0') as varchar(12))+' เดือน' as CBS_TWORKTIME," +
                             " b.CBS_CDOB," +
                             " cast(isnull(b.CBS_PADDRTIME,'0') as varchar(12))+' เดือน' as CBS_PADDRTIME," +
                             "b.CBS_GENDER_CD + '-' + ISNULL((select distinct top 1 GENDER_NAME from ST_GENDER where b.CBS_GENDER_CD = GENDER_CD),'-') AS CBS_GENDER_CD," +
                             "b.CBS_RESTYPE_CD + '-' + ISNULL((select distinct top 1 RESTYPE_NAME from ST_RESTYPE where b.CBS_RESTYPE_CD = RESTYPE_CD),'-') AS CBS_RESTYPE_CD," +
                             "b.CBS_MARITAL_CD + '-' + ISNULL((select distinct top 1 MARITAL_NAME from ST_MARITAL where b.CBS_MARITAL_CD = MARITAL_CD),'-') AS CBS_MARITAL_CD," +
                             "b.CBS_OCCCLS_CD + '-' + ISNULL((select distinct top 1 OCCCLS_DETAIL from ST_OCCCLS where b.CBS_OCCCLS_CD = OCCCLS_CD),'-') AS CBS_OCCCLS_CD," +
                             "b.CBS_OCCCLS_CD +b.CBS_OCCSCLS_CD + '-' + ISNULL((select distinct top 1 OCCSCLS_DETAIL from ST_OCCSCLS where b.CBS_OCCCLS_CD = OCCCLS_CD and b.CBS_OCCSCLS_CD = OCCSCLS_CD),'-') AS CBS_OCCSCLS_CD, " +
                             "c.GRADE," +
                             "b.CBS_EDU_CD + '-' + ISNULL((select distinct top 1 EDU_NAME from ST_EDUCATION where b.CBS_EDU_CD = EDU_CD),'-') AS CBS_EDU_CD," +
                             "c.score,'MyMo' as CREATION_BY, " +
                             "ISNULL(LNCOLLVAL,'0.00') AS LNCOLLVAL, " +
                             "isnull((LEFT(LOAN_TERM,LEN(LOAN_TERM)-1) +' '+  CASE WHEN RIGHT(LOAN_TERM,1)='D' THEN 'วัน' ELSE CASE WHEN RIGHT(LOAN_TERM,1)='M' THEN 'เดือน' ELSE CASE WHEN RIGHT(LOAN_TERM,1)='Y' THEN 'ปี' END END END),'') AS LOAN_TERM, " +
                             "isnull(a.ISIC1_CD,'') + '-' + ISNULL((select ISIC1_DETAIL from ST_ISIC1 where a.ISIC1_CD = ISIC1_CD),'-') AS ISIC1_NAME, " +
                             "isnull(a.ISIC2_CD,'') + '-' + ISNULL((select ISIC2_DETAIL from ST_ISIC2 where a.ISIC1_CD = ISIC1_CD and a.ISIC2_CD = ISIC2_CD),'-') AS ISIC2_NAME, " +
                             "isnull(a.ISIC3_CD,'') + '-' + ISNULL((select ISIC3_DETAIL from ST_ISIC3 where a.ISIC1_CD = ISIC1_CD and a.ISIC2_CD = ISIC2_CD and a.ISIC3_CD = ISIC3_CD),'-') AS ISIC3_NAME, " +
                             "a.PURPOSE_CD + '-' + ISNULL((select distinct top 1 PURPOSE_NAME from ST_PURPOSE where a.PURPOSE_CD = PURPOSE_CD),'-') AS PURPOSE_NAME, " +
                             "isnull(a.CONSUMPTION_CD,'') + '-' + ISNULL((select distinct top 1 CONSUMPTION_NAME from ST_CONSUMPTION where a.CONSUMPTION_CD = CONSUMPTION_CD and a.LOAN_CD = LOAN_CD ),'-') AS CONSUMPTION_NAME, " +
                             "isnull(a.GSBPURPOSE_CD,'') + '-' + ISNULL((select distinct top 1 GPURPOSE_NAME from ST_GSBPURPOSE where a.GSBPURPOSE_CD = GSBPURPOSE_CD),'-') AS GSBPURPOSE_NAME, " +
                             "a.LOAN_CD + '-' + ISNULL((select distinct top 1 LOAN_NAME from ST_LOANTYPE where a.LOAN_CD = LOAN_CD),'-') as LOAN_NAME, " +
                             "a.STYPE_CD + '-' + ISNULL((select distinct top 1 STYPER_NAME from ST_LOANSTYPE where a.STYPE_CD = STYPE_CD),'-') as STYPE_NAME, " +
                             "a.MARKET_CD + '-' + ISNULL((select distinct top 1 MARKET_NAME from ST_LOANMKT where a.MARKET_CD = MARKET_CD),'-') as MARKET_NAME, " +
                             "'MyMo' as BRANCH_NAME, " +
                             "case when a.CBS_APPROVE_CD = 3 then '3-Approved' when a.CBS_APPROVE_CD = 4 then '4-Rejected' when a.CBS_APPROVE_CD = 0 then cast(a.CBS_APPROVE_CD as varchar(1))+'-Other' else '-' end AS LNSTATUS , " +
                             "ISNULL((select top 1 MIN(GRADE) from MYMO_LN_GRADE where a.APP_NO = APP_NO and a.APP_SEQ = APP_SEQ),'-') AS GRADE, " +
                             "ISNULL(convert(varchar,(select top 1 MAX(SCORE) from MYMO_LN_GRADE where a.APP_NO = APP_NO and a.APP_SEQ = APP_SEQ)),'Score Not Require') AS SCORE, " +
                             "ISNULL((select distinct top 1 MODEL from MYMO_LN_GRADE where a.APP_NO = APP_NO and a.APP_SEQ = APP_SEQ),'-') AS MODEL_NAME, " +
                             "null as CA_Date, " +
                             //Tai 2014-03022 "ISNULL((select top 1 convert(varchar,convert(money,CBS_COL),1) from CBS_LN_APP where CBS_APP_NO = APP_NO order by BATCH_UPDATE_DTM desc),'-') as CBS_COL, " +
                             "dbo.comma_percent_format(ISNULL(LNCOLLVAL,'0.00')) as CBS_COL, " +
                             "case when a.CBS_APPROVE_CD = 3 then '3-Approved' when a.CBS_APPROVE_CD = 4 then '4-Rejected' when a.CBS_APPROVE_CD = 0 then cast(a.CBS_APPROVE_CD as varchar(1))+'-Other'  " +
                            // "when ISNULL((select top 1 substring(CBS_REASON_CD,1,1) from CBS_LN_APP where CBS_APP_NO = a.APP_NO order by BATCH_UPDATE_DTM desc),'-') = 'R' then 'Rejected' " +
                             "else '-' end as CA_RESULT, " +
                             "ISNULL(a.REASON_CD,'') + '-' + ISNULL((select REASON_NAME from LN_REASON where LN_REASON.REASON_CD = a.REASON_CD),'') as CA_REASON, " +
                             "ISNULL(convert(varchar,convert(money,MPAYAMT),1),0) + ' บาท' as MPAYAMT, ISNULL(convert(varchar,convert(money,APPRAMT),1),0) + ' บาท' as APPRAMT, " +
                             "ISNULL((LEFT(APPRTERM,LEN(APPRTERM)-1) +' '+  CASE WHEN RIGHT(APPRTERM,1)='D' THEN 'วัน' ELSE CASE WHEN RIGHT(APPRTERM,1)='M' THEN 'เดือน' ELSE CASE WHEN RIGHT(APPRTERM,1)='Y' THEN 'ปี' END END END),'-') as APPRTERM,(SELECT COUNT(1) FROM MYMO_LN_APPCIF WHERE  MYMO_LN_APPCIF.CBS_APP_NO = a.APP_NO AND MYMO_LN_APPCIF.APP_SEQ = a.APP_SEQ)-1 as LOAN_NUM  " +
                             " from MYMO_LN_APP a inner join MYMO_LN_APPCIF  b on  a.APP_NO = b.CBS_APP_NO and a.APP_SEQ = b.APP_SEQ  " +
                             " inner join MYMO_LN_GRADE c  on  a.APP_NO = c.APP_NO and a.APP_SEQ = c.APP_SEQ  " +
                             " where LEN(a.APP_NO)>0 and LEN(a.APP_SEQ)>0 and a.APP_NO = '{0}' and a.APP_SEQ = '{1}' " +
                             " order by a.APP_DATE desc", Request.QueryString["id"], Request.QueryString["seq"]);
  


            DataTable dt = sqlconn.ExcuteSQL(Query);

            if (dt != null)
                if (dt.Rows.Count > 0)
                {
                    Application.DataSource = dt;
                    Application.DataBind();
                    Session["dtApplication"] = dt;
                }

            Query = string.Format("select distinct case when (row_number() over (order by CBS_APP_NO)) = 1 then 'ข้อมูลผู้กู้' else ('ข้อมูลผู้กู้ร่วม : รายที่ ' + convert(varchar,(row_number() over (order by CBS_APP_NO)) -1 )) end as row," +
                                         "CBS_CIFNO, RIGHT(replicate('0',13)+ISNULL(CBS_CITIZID,'0'),13) AS CBS_CITIZID, CBS_CDOB, ISNULL(CBS_CHILD,'0') AS CBS_CHILD, ISNULL(CBS_WCHILD,'0') AS CBS_WCHILD, " +
                                         "ISNULL(PASSID,'-') as PASSID, " +
                                         "isnull(cbs_ccode + ':' +  (select top 1 ccode_name from st_ccode where ccode_cd = cbs_ccode),'-') as CBS_CCODE, " +
                                         "CASE WHEN CBS_CTYPE_CD = 0 then '0 - บุคคลธรรมดา' WHEN CBS_CTYPE_CD = 1 then '1 - นิติบุคคล' ELSE '-' END  as CBS_CTYPE, " +
                                         "ISNULL(CBS_INCOME,'0.00') AS CBS_INCOME, ISNULL(CBS_OTHINCOME,'0.00') AS CBS_OTHINCOME, ISNULL(CBS_MNTEXPEN,'0.00') AS CBS_MNTEXPEN, " +
                                         "ISNULL(CBS_LBTEXPEN,'0.00') AS CBS_LBTEXPEN, ISNULL(CBS_BUSEXPEN,'0.00') AS CBS_BUSEXPEN, ISNULL(CBS_PWORKTIME,'0') AS CBS_PWORKTIME, ISNULL(CBS_TWORKTIME, '0') AS CBS_TWORKTIME, " +
                                         "ISNULL(CBS_BUSSYEAR,'0') AS CBS_BUSSYEAR, ISNULL(CBS_BUSSMONTH,'0') AS CBS_BUSSMONTH,ISNULL(CBS_PADDRTIME,'0') AS CBS_PADDRTIME, " +
                                         "(ISNULL((select distinct top 1 TITLEE_NAME from ST_TITLE where MYMO_LN_APPCIF.CBS_TITLE_CD = TITLE_CD and TITLE_LG = 0)+' ','') + ISNULL(CBS_FNAME+' ','') + ISNULL(CBS_MNAME+' ','') + ISNULL(CBS_SNAME,'')) AS CBS_CIFNAME, " +
                                         "MYMO_LN_APPCIF.CBS_GENDER_CD + '-' + ISNULL((select distinct top 1 GENDER_NAME from ST_GENDER where MYMO_LN_APPCIF.CBS_GENDER_CD = GENDER_CD),'-') AS CBS_GENDER_CD, " +
                                         "MYMO_LN_APPCIF.CBS_MARITAL_CD + '-' + ISNULL((select distinct top 1 MARITAL_NAME from ST_MARITAL where MYMO_LN_APPCIF.CBS_MARITAL_CD = MARITAL_CD),'-') AS CBS_MARITAL_CD, " +
                                         "MYMO_LN_APPCIF.CBS_EDU_CD + '-' + ISNULL((select distinct top 1 EDU_NAME from ST_EDUCATION where MYMO_LN_APPCIF.CBS_EDU_CD = EDU_CD),'-') AS CBS_EDU_CD, " +
                                         "MYMO_LN_APPCIF.CBS_RESSTATUS_CD + '-' + ISNULL((select distinct top 1 RESSTATUS_NAME from ST_RESSTATUS where MYMO_LN_APPCIF.CBS_RESSTATUS_CD = RESSTATUS_CD),'-') AS CBS_RESSTATUS_CD, " +
                                         "MYMO_LN_APPCIF.CBS_RESTYPE_CD + '-' + ISNULL((select distinct top 1 RESTYPE_NAME from ST_RESTYPE where MYMO_LN_APPCIF.CBS_RESTYPE_CD = RESTYPE_CD),'-') AS CBS_RESTYPE_CD, " +
                                         "isnull(MYMO_LN_APPCIF.CBS_BUSSTYPE_CD + '-' + ISNULL((select distinct top 1 BUSSTYPET_NAME from ST_BUSSTYPE where MYMO_LN_APPCIF.CBS_BUSSTYPE_CD = BUSSTYPE_CD),'-'),'-') AS CBS_BUSSTYPE, " +
                                         "isnull(MYMO_LN_APPCIF.CBS_PROP_RIGHT_CD + '-' + ISNULL((select distinct top 1 PROP_RIGHT_NAME from ST_PROP_RIGHT where MYMO_LN_APPCIF.CBS_PROP_RIGHT_CD = PROP_RIGHT_CD),'-'),'-') AS CBS_PROP_RIGHT, " +
                                         "MYMO_LN_APPCIF.CBS_OCCCLS_CD + '-' + ISNULL((select distinct top 1 OCCCLS_DETAIL from ST_OCCCLS where MYMO_LN_APPCIF.CBS_OCCCLS_CD = OCCCLS_CD),'-') AS CBS_OCCCLS_CD, " +
                                         "MYMO_LN_APPCIF.CBS_OCCCLS_CD +MYMO_LN_APPCIF.CBS_OCCSCLS_CD + '-' + ISNULL((select distinct top 1 OCCSCLS_DETAIL from ST_OCCSCLS where MYMO_LN_APPCIF.CBS_OCCCLS_CD = OCCCLS_CD and MYMO_LN_APPCIF.CBS_OCCSCLS_CD = OCCSCLS_CD),'-') AS CBS_OCCSCLS_CD, " +
                                         "ISNULL(convert(varchar,(select top 1 CIFSCORE from MYMO_LN_CHAR where MYMO_LN_APPCIF.CBS_APP_NO = CBS_APP_NO and MYMO_LN_APPCIF.APP_SEQ = APP_SEQ and MYMO_LN_APPCIF.CBS_CIFNO = CBS_CIFNO)),'Score Not Require') AS CIF_SCORE, " +
                                         "(ISNULL(CBS_INCOME,'0.00') + ISNULL(CBS_OTHINCOME,'0.00')) - (ISNULL(CBS_MNTEXPEN,'0.00') + ISNULL(CBS_LBTEXPEN,'0.00') + ISNULL(CBS_BUSEXPEN,'0.00')) AS TOTAL_INCOME, " +
                                         "isnull(MYMO_LN_APPCIF.CBS_ISIC1,'') + '-' + ISNULL((select top 1 ISIC1_DETAIL from ST_ISIC1 where MYMO_LN_APPCIF.CBS_ISIC1 = ISIC1_CD),'-') AS ISIC1_NAME, " +
                                         "isnull(MYMO_LN_APPCIF.CBS_ISIC2,'') + '-' + ISNULL((select top 1 ISIC2_DETAIL from ST_ISIC2 where MYMO_LN_APPCIF.CBS_ISIC1 = ISIC1_CD and MYMO_LN_APPCIF.CBS_ISIC2 = ISIC2_CD),'-') AS ISIC2_NAME, " +
                                         "isnull(MYMO_LN_APPCIF.CBS_ISIC3,'') + '-' + ISNULL((select top 1 ISIC3_DETAIL from ST_ISIC3 where MYMO_LN_APPCIF.CBS_ISIC1 = ISIC1_CD and MYMO_LN_APPCIF.CBS_ISIC2 = ISIC2_CD and MYMO_LN_APPCIF.CBS_ISIC3 = ISIC3_CD),'-') AS ISIC3_NAME, " +
                                         "case when (NUM_CARD = 0) or (NUM_CARD is null) then '-' else NUM_CARD end as NUM_CARD, " +
                                         "(CASE WHEN CUST_TYPE = '01' then '01: เคยเป็นลูกค้าสินเชื่อของธนาคารแต่ปิดบัญชีไปแล้ว' WHEN CUST_TYPE = '02' then '02: ปัจจุบันเป็นลูกค้าสินเชื่อของธนาคาร' WHEN CUST_TYPE = '03' then '03: ไม่เคยเป็นลูกค้าสินเชื่อของธนาคาร' ELSE '-' END) as CUST_TYPE, " +
                                         "ISNULL(case when OUT_DEPT = '' then '0' else OUT_DEPT end,'0') as OUT_DEPT , (CASE WHEN BKLST_HIST = 'N' then 'N: ไม่มี' WHEN   BKLST_HIST = 'Y' then 'Y: มี' ELSE '-' END) as BKLST_HIST , " +
                                         "(CASE WHEN LITIG_STAT = '02' then '02:ไม่มี' WHEN   LITIG_STAT = '01' then '01:มี' ELSE '-' END) as LITIG_STAT, " +
                                         "(CASE WHEN MESS_CHENL = '01' THEN '01: โทรทัศน์' WHEN MESS_CHENL = '02' THEN '02: วิทยุ' WHEN MESS_CHENL = '03' THEN '03: หนังสือพิมพ์/แมกกาซีน' WHEN MESS_CHENL = '04' THEN '04: Internet/ E-mail' WHEN MESS_CHENL = '05' THEN '05: " +
                                         "บุคคลแนะนำ' WHEN MESS_CHENL = '06' THEN '06: Event' WHEN MESS_CHENL = '07' THEN '07: แผ่นปลิว/สื่อสิ่งพิมพ์/ป้ายโฆษณา' ELSE '-' END) as MESS_CHENL , " +
                                         "(CASE WHEN CBS_AUTO_STATUS = 1 then '1: ไม่มี' WHEN CBS_AUTO_STATUS = 2 then '2: มีและมีภาระหนี้' WHEN CBS_AUTO_STATUS = 3 then '3: มีและไม่มีภาระหนี้' ELSE '-' END) as CBS_AUTO_STATUS, " +
                                         "(CASE WHEN CBS_PROP_STATUS = 1 then '1: ไม่มี' WHEN CBS_PROP_STATUS = 2 then '2: มีและมีภาระหนี้' WHEN CBS_PROP_STATUS = 3 then '3: มีและไม่มีภาระหนี้' ELSE '-' END) as CBS_PROP_STATUS, " +
                                         //Tai 2014-03-22  
                                         //"(DATEDIFF(YEAR,CBS_CDOB,GETDATE())-CASE WHEN DATEADD(YY,DATEDIFF(YEAR,CBS_CDOB,GETDATE()),CBS_CDOB) > GETDATE() THEN 1 ELSE 0 END) as AGE, " +
                                         "(SELECT top 1 YEAR(APP_DATE) FROM MYMO_LN_APP WHERE MYMO_LN_APPCIF.CBS_APP_NO=MYMO_LN_APP.APP_NO)-YEAR(CBS_CDOB) AS AGE, " +
                                         "isnull(COBORREL,'-') as COBORREL,isnull(GUARAREL,'-') as GUARAREL " +
                                         "from MYMO_LN_APPCIF " +
                                         "where LEN(CBS_APP_NO)>0 AND LEN(APP_SEQ)>0 AND CBS_APP_NO = '{0}' AND APP_SEQ = '{1}' ", Request.QueryString["id"], Request.QueryString["seq"]);

            dt = sqlconn.ExcuteSQL(Query);

            if (dt != null)
                if (dt.Rows.Count > 0)
                {
                    Session["dtLoaner"] = dt;
                }

            if (Request.QueryString["req"] == "Analysis")
            {
                Query = string.Format("select distinct MYMO_LN_CHAR.CHAR_CD, MYMO_LN_CHAR.SCORE, " +
                                             "(select ST_CHAR.CHAR_NAME from ST_CHAR where MYMO_LN_CHAR.CHAR_CD = ST_CHAR.CHAR_CD) [CHAR_NAME] " +
                                             "from MYMO_LN_CHAR " +
                                             "where CBS_APP_NO = '{0}' and APP_SEQ = '{1}' ",
                                             Request.QueryString["id"], Request.QueryString["seq"]);

                dt = sqlconn.ExcuteSQL(Query);

                if (dt != null)
                    if (dt.Rows.Count > 0)
                    {
                        Session["dtChar_CBS_CIFNO"] = dt;
                    }
            }

        }

        protected void Loaner_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var AppChar = (Repeater)e.Item.FindControl("AppChar");


                if (Request.QueryString["req"] == "Analysis")
                {
                    string Query = string.Format("select distinct MYMO_LN_CHAR.CHAR_CD, MYMO_LN_CHAR.SCORE, " +
                                                 "(select ST_CHAR.CHAR_NAME from ST_CHAR where MYMO_LN_CHAR.CHAR_CD = ST_CHAR.CHAR_CD) [CHAR_NAME] " +
                                                 "from MYMO_LN_CHAR " +
                                                 "where CBS_APP_NO = '{0}' and APP_SEQ = '{1}' ",
                                                 Request.QueryString["id"], Request.QueryString["seq"]); //dt.Rows[e.Item.ItemIndex]["CBS_CIFNO"]

                    DataTable dt = sqlconn.ExcuteSQL(Query);

                    //Session["dtChar_CBS_CIFNO"] = dt;
                    //if (Session["dtChar_CBS_CIFNO"] != null)
                    //{
                    //    if (Session["dtChar_CBS_CIFNO"].ToString().Trim().Length != 0)
                    //    {
                    //        Session["dtChar_CBS_CIFNO"] = Session["dtChar_CBS_CIFNO"].ToString().Trim();
                    //    }
                    //    else
                    //    {
                    //        Session["dtChar_CBS_CIFNO"] = "";
                    //    }
                    //}
                    //else
                    //{
                    //    Session["dtChar_CBS_CIFNO"] = "";
                    //}

                    if (dt != null)
                        if (dt.Rows.Count > 0)
                        {
                            AppChar.DataSource = dt;
                            AppChar.DataBind();
                        }
                }
                else
                {
                    var TableChar = (Table)e.Item.FindControl("TableChar");
                    TableChar.Visible = false;
                }
            }
        }

        protected void PrintPageDetails_Click(object sender, EventArgs e)
        {
            //Call report
            string a = Request.QueryString["req"].ToString();
            if (Request.QueryString["req"].ToString() == "Analysis")
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "aa", "window.open('ReportLayOut/frmPreviewLoanRpt.aspx?ReportId=4');", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "aa", "window.open('ReportLayOut/frmPreviewLoanRpt.aspx?ReportId=3');", true);
            }
        }
    }
}