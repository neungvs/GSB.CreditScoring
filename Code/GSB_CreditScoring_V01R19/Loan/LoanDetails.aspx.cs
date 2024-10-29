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
    public partial class LoanDetails : System.Web.UI.Page
    {
        SQLToDataTable sqlconn = new SQLToDataTable();

        # region PageLoad

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Cookies["Credit_Scoring_GSB"] == null)
                Response.Redirect("~/Login.aspx");

            string app_id = Request.QueryString["id"];
            string app_seq = Request.QueryString["seq"];
            string app_req = Request.QueryString["req"];

            if (app_id != null && app_seq != null && app_req != null)
            {
                BindDataApplication();
                //if (Session["Model"].ToString() != "7")
                //{
                BindDataLoaner();
                BindDataCollateral();
                //}
            }

            DateTime dt = DateTime.Now;
            string[] month = new string[] { "มกราคม", "กุมภาพันธ์", "มีนาคม", "เมษายน", "พฤษภาคม", "มิถุนายน", "กรกฎาคม", "สิงหาคม", "กันยายน", "ตุลาคม", "พฤศจิกายน", "ธันวาคม" };
            printDateTxt.Text = "วันที่ " + dt.Day + " " + month[dt.Month - 1] + " " + (dt.Year + 543) + " เวลา " + dt.ToString("H:mm") + "น.";

            Session["RequestFor"] = "Analysis";
            
            if(!this.Page.IsPostBack)Session["dtChar_CBS_CIFNO"] = "";

            if (app_req == "Analysis")
            {
                title.Text = "รายงานตรวจสอบผลการวิเคราะห์สินเชื่อ";
                Session["RequestFor"] = "Analysis";
            }
            else
            {
                title.Text = "รายงานข้อมูลลูกค้าสินเชื่อ";
                Session["RequestFor"] = "Customer";
            }
        }

        #endregion PageLoad

        #region Private Method

        private void BindDataApplication()
        {
            string Query = "";

            if (Session["Model"].ToString() == "7")
            {
                Query = string.Format("select distinct APP_NO, APP_SEQ, APP_DATE, LOAN_AMOUNT, CREATION_BY, CREATION_DT, LOAN_AMOUNT, " +
                             "(LEFT(LOAN_TERM,LEN(LOAN_TERM)-1) +' '+  CASE WHEN RIGHT(LOAN_TERM,1)='D' THEN 'วัน' ELSE CASE WHEN RIGHT(LOAN_TERM,1)='M' THEN 'เดือน' ELSE CASE WHEN RIGHT(LOAN_TERM,1)='Y' THEN 'ปี' END END END) AS LOAN_TERM, " +
                             "ISNULL(LNCOLLVAL,'0.00') AS LNCOLLVAL, " +
                             "MYMO_LN_APP.ISIC1_CD + '-' + ISNULL((select ISIC1_DETAIL from ST_ISIC1 where MYMO_LN_APP.ISIC1_CD = ISIC1_CD),'-') AS ISIC1_NAME, " +
                             "MYMO_LN_APP.ISIC2_CD + '-' + ISNULL((select ISIC2_DETAIL from ST_ISIC2 where MYMO_LN_APP.ISIC1_CD = ISIC1_CD and MYMO_LN_APP.ISIC2_CD = ISIC2_CD),'-') AS ISIC2_NAME, " +
                             "MYMO_LN_APP.ISIC3_CD + '-' + ISNULL((select ISIC3_DETAIL from ST_ISIC3 where MYMO_LN_APP.ISIC1_CD = ISIC1_CD and MYMO_LN_APP.ISIC2_CD = ISIC2_CD and MYMO_LN_APP.ISIC3_CD = ISIC3_CD),'-') AS ISIC3_NAME, " +
                             "MYMO_LN_APP.PURPOSE_CD + '-' + ISNULL((select distinct top 1 PURPOSE_NAME from ST_PURPOSE where MYMO_LN_APP.PURPOSE_CD = PURPOSE_CD),'-') AS PURPOSE_NAME, " +
                             "MYMO_LN_APP.CONSUMPTION_CD + '-' + ISNULL((select distinct top 1 CONSUMPTION_NAME from ST_CONSUMPTION where MYMO_LN_APP.CONSUMPTION_CD = CONSUMPTION_CD and MYMO_LN_APP.LOAN_CD = LOAN_CD ),'-') AS CONSUMPTION_NAME, " +
                             "MYMO_LN_APP.GSBPURPOSE_CD + '-' + ISNULL((select distinct top 1 GPURPOSE_NAME from ST_GSBPURPOSE where MYMO_LN_APP.GSBPURPOSE_CD = GSBPURPOSE_CD),'-') AS GSBPURPOSE_NAME, " +
                             "MYMO_LN_APP.LOAN_CD + '-' + ISNULL((select distinct top 1 LOAN_NAME from ST_LOANTYPE where MYMO_LN_APP.LOAN_CD = LOAN_CD),'-') as LOAN_NAME, " +
                             "MYMO_LN_APP.STYPE_CD + '-' + ISNULL((select distinct top 1 STYPER_NAME from ST_LOANSTYPE where MYMO_LN_APP.STYPE_CD = STYPE_CD),'-') as STYPE_NAME, " +
                             "MYMO_LN_APP.MARKET_CD + '-' + ISNULL((select distinct top 1 MARKET_NAME from ST_LOANMKT where MYMO_LN_APP.MARKET_CD = MARKET_CD),'-') as MARKET_NAME, " +
                             "MYMO_LN_APP.BRANCH_CD + '-' + ISNULL((select distinct top 1 BRANCH_NAME from ST_BRANCH where MYMO_LN_APP.BRANCH_CD = BRANCH_CD),'-') as BRANCH_NAME, " +
                             "ISNULL((select distinct top 1 CBS_APPROVE_CD from CBS_LN_APP where CBS_APP_NO = APP_NO),LNSTATUS) + '-' + ISNULL((select distinct top 1 APPROVE_NAME FROM ST_APPTATUS WHERE APPROVE_CD = (ISNULL((select distinct top 1 CBS_APPROVE_CD from CBS_LN_APP where CBS_APP_NO = APP_NO),LNSTATUS))),'-') AS LNSTATUS, " +
                             "ISNULL((select top 1 MIN(GRADE) from MYMO_LN_GRADE where MYMO_LN_APP.APP_NO = APP_NO and MYMO_LN_APP.APP_SEQ = APP_SEQ),'-') AS GRADE, " +
                             "ISNULL(convert(varchar,(select top 1 MAX(SCORE) from MYMO_LN_GRADE where MYMO_LN_APP.APP_NO = APP_NO and MYMO_LN_APP.APP_SEQ = APP_SEQ)),'Score Not Require') AS SCORE, " +
                             "ISNULL((select distinct top 1 MODEL from MYMO_LN_GRADE where MYMO_LN_APP.APP_NO = APP_NO and MYMO_LN_APP.APP_SEQ = APP_SEQ),'-') AS MODEL_NAME, " +
                             "ISNULL((select top 1 case when (convert(int,right([CBS_CA_DATE],4)) - 2000) > 500 then [CBS_CA_DATE] else (left([CBS_CA_DATE],6) + convert(varchar,(convert(int,right([CBS_CA_DATE],4)) + 543))) end from CBS_LN_APP where CBS_APP_NO = APP_NO order by BATCH_UPDATE_DTM desc),'-') as CA_Date, " +
                             //Tai 2014-03022 "ISNULL((select top 1 convert(varchar,convert(money,CBS_COL),1) from CBS_LN_APP where CBS_APP_NO = APP_NO order by BATCH_UPDATE_DTM desc),'-') as CBS_COL, " +
                             "dbo.comma_percent_format(ISNULL(LNCOLLVAL,'0.00')) as CBS_COL, " +
                             "case when ISNULL((select top 1 substring(CBS_REASON_CD,1,1) from CBS_LN_APP where CBS_APP_NO = APP_NO order by BATCH_UPDATE_DTM desc),'-') = 'A' then 'Approve' " +
                             "when ISNULL((select top 1 substring(CBS_REASON_CD,1,1) from CBS_LN_APP where CBS_APP_NO = APP_NO order by BATCH_UPDATE_DTM desc),'-') = 'R' then 'Reject' " +
                             "else '-' end as CA_RESULT, " +
                             "ISNULL((select top 1 CBS_REASON_CD from CBS_LN_APP where CBS_APP_NO = APP_NO order by BATCH_UPDATE_DTM desc),'') + '-' + ISNULL((select REASON_NAME from LN_REASON where LN_REASON.REASON_CD = ISNULL((select top 1 CBS_REASON_CD from CBS_LN_APP where CBS_APP_NO = APP_NO order by BATCH_UPDATE_DTM desc),'-')),'') as CA_REASON, " +
                             "convert(varchar,convert(money,MPAYAMT),1) + ' บาท' as MPAYAMT, convert(varchar,convert(money,APPRAMT),1) + ' บาท' as APPRAMT, " +
                             "(LEFT(APPRTERM,LEN(APPRTERM)-1) +' '+  CASE WHEN RIGHT(APPRTERM,1)='D' THEN 'วัน' ELSE CASE WHEN RIGHT(APPRTERM,1)='M' THEN 'เดือน' ELSE CASE WHEN RIGHT(APPRTERM,1)='Y' THEN 'ปี' END END END) as APPRTERM,(SELECT COUNT(1) FROM MYMO_LN_APPCIF WHERE  MYMO_LN_APPCIF.CBS_APP_NO = MYMO_LN_APP.APP_NO AND MYMO_LN_APPCIF.APP_SEQ = MYMO_LN_APP.APP_SEQ)-1 as LOAN_NUM  " +
                             "from MYMO_LN_APP " +
                             "where LEN(APP_NO)>0 and LEN(APP_SEQ)>0 and APP_NO = '{0}' and APP_SEQ = '{1}' " +
                             " order by MYMO_LN_APP.APP_DATE desc", Request.QueryString["id"], Request.QueryString["seq"]);
            }
            else
            {
                Query = string.Format("select distinct APP_NO, APP_SEQ, APP_DATE, LOAN_AMOUNT, CREATION_BY, CREATION_DT, LOAN_AMOUNT, " +
                             "(LEFT(LOAN_TERM,LEN(LOAN_TERM)-1) +' '+  CASE WHEN RIGHT(LOAN_TERM,1)='D' THEN 'วัน' ELSE CASE WHEN RIGHT(LOAN_TERM,1)='M' THEN 'เดือน' ELSE CASE WHEN RIGHT(LOAN_TERM,1)='Y' THEN 'ปี' END END END) AS LOAN_TERM, " +
                             "ISNULL(LNCOLLVAL,'0.00') AS LNCOLLVAL, " +
                             "LN_APP.ISIC1_CD + '-' + ISNULL((select ISIC1_DETAIL from ST_ISIC1 where LN_APP.ISIC1_CD = ISIC1_CD),'-') AS ISIC1_NAME, " +
                             "LN_APP.ISIC2_CD + '-' + ISNULL((select ISIC2_DETAIL from ST_ISIC2 where LN_APP.ISIC1_CD = ISIC1_CD and LN_APP.ISIC2_CD = ISIC2_CD),'-') AS ISIC2_NAME, " +
                             "LN_APP.ISIC3_CD + '-' + ISNULL((select ISIC3_DETAIL from ST_ISIC3 where LN_APP.ISIC1_CD = ISIC1_CD and LN_APP.ISIC2_CD = ISIC2_CD and LN_APP.ISIC3_CD = ISIC3_CD),'-') AS ISIC3_NAME, " +
                             "LN_APP.PURPOSE_CD + '-' + ISNULL((select distinct top 1 PURPOSE_NAME from ST_PURPOSE where LN_APP.PURPOSE_CD = PURPOSE_CD),'-') AS PURPOSE_NAME, " +
                             "LN_APP.CONSUMPTION_CD + '-' + ISNULL((select distinct top 1 CONSUMPTION_NAME from ST_CONSUMPTION where LN_APP.CONSUMPTION_CD = CONSUMPTION_CD and LN_APP.LOAN_CD = LOAN_CD ),'-') AS CONSUMPTION_NAME, " +
                             "LN_APP.GSBPURPOSE_CD + '-' + ISNULL((select distinct top 1 GPURPOSE_NAME from ST_GSBPURPOSE where LN_APP.GSBPURPOSE_CD = GSBPURPOSE_CD),'-') AS GSBPURPOSE_NAME, " +
                             "LN_APP.LOAN_CD + '-' + ISNULL((select distinct top 1 LOAN_NAME from ST_LOANTYPE where LN_APP.LOAN_CD = LOAN_CD),'-') as LOAN_NAME, " +
                             "LN_APP.STYPE_CD + '-' + ISNULL((select distinct top 1 STYPER_NAME from ST_LOANSTYPE where LN_APP.STYPE_CD = STYPE_CD),'-') as STYPE_NAME, " +
                             "LN_APP.MARKET_CD + '-' + ISNULL((select distinct top 1 MARKET_NAME from ST_LOANMKT where LN_APP.MARKET_CD = MARKET_CD),'-') as MARKET_NAME, " +
                             "LN_APP.BRANCH_CD + '-' + ISNULL((select distinct top 1 BRANCH_NAME from ST_BRANCH where LN_APP.BRANCH_CD = BRANCH_CD),'-') as BRANCH_NAME, " +
                             "ISNULL((select distinct top 1 CBS_APPROVE_CD from CBS_LN_APP where CBS_APP_NO = APP_NO),LNSTATUS) + '-' + ISNULL((select distinct top 1 APPROVE_NAME FROM ST_APPTATUS WHERE APPROVE_CD = (ISNULL((select distinct top 1 CBS_APPROVE_CD from CBS_LN_APP where CBS_APP_NO = APP_NO),LNSTATUS))),'-') AS LNSTATUS, " +
                             "ISNULL((select top 1 MIN(GRADE) from LN_GRADE where LN_APP.APP_NO = APP_NO and LN_APP.APP_SEQ = APP_SEQ),'-') AS GRADE, " +
                             "ISNULL(convert(varchar,(select top 1 MAX(SCORE) from LN_GRADE where LN_APP.APP_NO = APP_NO and LN_APP.APP_SEQ = APP_SEQ)),'Score Not Require') AS SCORE, " +
                             "ISNULL((select distinct top 1 MODEL from LN_GRADE where LN_APP.APP_NO = APP_NO and LN_APP.APP_SEQ = APP_SEQ),'-') AS MODEL_NAME, " +
                             "ISNULL((select top 1 case when (convert(int,right([CBS_CA_DATE],4)) - 2000) > 500 then [CBS_CA_DATE] else (left([CBS_CA_DATE],6) + convert(varchar,(convert(int,right([CBS_CA_DATE],4)) + 543))) end from CBS_LN_APP where CBS_APP_NO = APP_NO order by BATCH_UPDATE_DTM desc),'-') as CA_Date, " +
                             //Tai 2014-03022 "ISNULL((select top 1 convert(varchar,convert(money,CBS_COL),1) from CBS_LN_APP where CBS_APP_NO = APP_NO order by BATCH_UPDATE_DTM desc),'-') as CBS_COL, " +
                             "dbo.comma_percent_format(ISNULL(LNCOLLVAL,'0.00')) as CBS_COL, " +
                             "case when ISNULL((select top 1 substring(CBS_REASON_CD,1,1) from CBS_LN_APP where CBS_APP_NO = APP_NO order by BATCH_UPDATE_DTM desc),'-') = 'A' then 'Approve' " +
                             "when ISNULL((select top 1 substring(CBS_REASON_CD,1,1) from CBS_LN_APP where CBS_APP_NO = APP_NO order by BATCH_UPDATE_DTM desc),'-') = 'R' then 'Reject' " +
                             "else '-' end as CA_RESULT, " +
                             "ISNULL((select top 1 CBS_REASON_CD from CBS_LN_APP where CBS_APP_NO = APP_NO order by BATCH_UPDATE_DTM desc),'') + '-' + ISNULL((select REASON_NAME from LN_REASON where LN_REASON.REASON_CD = ISNULL((select top 1 CBS_REASON_CD from CBS_LN_APP where CBS_APP_NO = APP_NO order by BATCH_UPDATE_DTM desc),'-')),'') as CA_REASON, " +
                             "convert(varchar,convert(money,MPAYAMT),1) + ' บาท' as MPAYAMT, convert(varchar,convert(money,APPRAMT),1) + ' บาท' as APPRAMT, " +
                             "(LEFT(APPRTERM,LEN(APPRTERM)-1) +' '+  CASE WHEN RIGHT(APPRTERM,1)='D' THEN 'วัน' ELSE CASE WHEN RIGHT(APPRTERM,1)='M' THEN 'เดือน' ELSE CASE WHEN RIGHT(APPRTERM,1)='Y' THEN 'ปี' END END END) as APPRTERM,(SELECT COUNT(1) FROM LN_APPCIF WHERE  LN_APPCIF.CBS_APP_NO = LN_APP.APP_NO AND LN_APPCIF.APP_SEQ = LN_APP.APP_SEQ)-1 as LOAN_NUM  " +
                             "from LN_APP " +
                             "where LEN(APP_NO)>0 and LEN(APP_SEQ)>0 and APP_NO = '{0}' and APP_SEQ = '{1}' " +
                             " order by LN_APP.APP_DATE desc", Request.QueryString["id"], Request.QueryString["seq"]);
            }



            DataTable dt = sqlconn.ExcuteSQL(Query);

            if (dt != null)
                if (dt.Rows.Count > 0)
                {
                    Application.DataSource = dt;
                    Application.DataBind();
                    Session["dtApplication"] = dt;
                }



        }

        private void BindDataLoaner()
        {
            //string Query = string.Format("select  case when (row_number() over (order by CBS_APP_NO)) = 1 then 'ข้อมูลผู้กู้หลัก' else ('ข้อมูลผู้กู้ร่วม : รายที่ ' + convert(varchar,(row_number() over (order by CBS_APP_NO)) -1 )) end as row," +
            string Query = string.Format("select distinct case when (row_number() over (order by CBS_APP_NO)) = 1 then 'ข้อมูลผู้กู้หลัก' else ('ข้อมูลผู้กู้ร่วม : รายที่ ' + convert(varchar,(row_number() over (order by CBS_APP_NO)) -1 )) end as row," +
                                         "CBS_CIFNO, RIGHT(replicate('0',13)+ISNULL(CBS_CITIZID,'0'),13) AS CBS_CITIZID, CBS_CDOB, ISNULL(CBS_CHILD,'0') AS CBS_CHILD, ISNULL(CBS_WCHILD,'0') AS CBS_WCHILD, " +
                                         "ISNULL(PASSID,'-') as PASSID, cbs_ccode + ':' +  (select top 1 ccode_name from st_ccode where ccode_cd = cbs_ccode) as CBS_CCODE, " +
                                         "CASE WHEN CBS_CTYPE_CD = 0 then '0 - บุคคลธรรมดา' WHEN CBS_CTYPE_CD = 1 then '1 - นิติบุคคล' ELSE '-' END  as CBS_CTYPE, " +
                                         "ISNULL(CBS_INCOME,'0.00') AS CBS_INCOME, ISNULL(CBS_OTHINCOME,'0.00') AS CBS_OTHINCOME, ISNULL(CBS_MNTEXPEN,'0.00') AS CBS_MNTEXPEN, " +
                                         "ISNULL(CBS_LBTEXPEN,'0.00') AS CBS_LBTEXPEN, ISNULL(CBS_BUSEXPEN,'0.00') AS CBS_BUSEXPEN, ISNULL(CBS_PWORKTIME,'0') AS CBS_PWORKTIME, ISNULL(CBS_TWORKTIME, '0') AS CBS_TWORKTIME, " +
                                         "ISNULL(CBS_BUSSYEAR,'0') AS CBS_BUSSYEAR, ISNULL(CBS_BUSSMONTH,'0') AS CBS_BUSSMONTH,ISNULL(CBS_PADDRTIME,'0') AS CBS_PADDRTIME, " +
                                         "(ISNULL((select distinct top 1 TITLEE_NAME from ST_TITLE where LN_APPCIF.CBS_TITLE_CD = TITLE_CD and TITLE_LG = 0)+' ','') + ISNULL(CBS_FNAME+' ','') + ISNULL(CBS_MNAME+' ','') + ISNULL(CBS_SNAME,'')) AS CBS_CIFNAME, " +
                                         "LN_APPCIF.CBS_GENDER_CD + '-' + ISNULL((select distinct top 1 GENDER_NAME from ST_GENDER where LN_APPCIF.CBS_GENDER_CD = GENDER_CD),'-') AS CBS_GENDER_CD, " +
                                         "LN_APPCIF.CBS_MARITAL_CD + '-' + ISNULL((select distinct top 1 MARITAL_NAME from ST_MARITAL where LN_APPCIF.CBS_MARITAL_CD = MARITAL_CD),'-') AS CBS_MARITAL_CD, " +
                                         "LN_APPCIF.CBS_EDU_CD + '-' + ISNULL((select distinct top 1 EDU_NAME from ST_EDUCATION where LN_APPCIF.CBS_EDU_CD = EDU_CD),'-') AS CBS_EDU_CD, " +
                                         "LN_APPCIF.CBS_RESSTATUS_CD + '-' + ISNULL((select distinct top 1 RESSTATUS_NAME from ST_RESSTATUS where LN_APPCIF.CBS_RESSTATUS_CD = RESSTATUS_CD),'-') AS CBS_RESSTATUS_CD, " +
                                         "LN_APPCIF.CBS_RESTYPE_CD + '-' + ISNULL((select distinct top 1 RESTYPE_NAME from ST_RESTYPE where LN_APPCIF.CBS_RESTYPE_CD = RESTYPE_CD),'-') AS CBS_RESTYPE_CD, " +
                                         "LN_APPCIF.CBS_BUSSTYPE_CD + '-' + ISNULL((select distinct top 1 BUSSTYPET_NAME from ST_BUSSTYPE where LN_APPCIF.CBS_BUSSTYPE_CD = BUSSTYPE_CD),'-') AS CBS_BUSSTYPE, " +
                                         "LN_APPCIF.CBS_PROP_RIGHT_CD + '-' + ISNULL((select distinct top 1 PROP_RIGHT_NAME from ST_PROP_RIGHT where LN_APPCIF.CBS_PROP_RIGHT_CD = PROP_RIGHT_CD),'-') AS CBS_PROP_RIGHT, " +
                                         "LN_APPCIF.CBS_OCCCLS_CD + '-' + ISNULL((select distinct top 1 OCCCLS_DETAIL from ST_OCCCLS where LN_APPCIF.CBS_OCCCLS_CD = OCCCLS_CD),'-') AS CBS_OCCCLS_CD, " +
                                         "LN_APPCIF.CBS_OCCSCLS_CD + '-' + ISNULL((select distinct top 1 OCCSCLS_DETAIL from ST_OCCSCLS where LN_APPCIF.CBS_OCCCLS_CD = OCCSCLS_CD and LN_APPCIF.CBS_OCCSCLS_CD = OCCCLS_CD),'-') AS CBS_OCCSCLS_CD, " +
                                         "ISNULL(convert(varchar,(select top 1 CIFSCORE from LN_CHAR where LN_APPCIF.CBS_APP_NO = CBS_APP_NO and LN_APPCIF.APP_SEQ = APP_SEQ and LN_APPCIF.CBS_CIFNO = CBS_CIFNO)),'Score Not Require') AS CIF_SCORE, " +
                                         "(ISNULL(CBS_INCOME,'0.00') + ISNULL(CBS_OTHINCOME,'0.00')) - (ISNULL(CBS_MNTEXPEN,'0.00') + ISNULL(CBS_LBTEXPEN,'0.00') + ISNULL(CBS_BUSEXPEN,'0.00')) AS TOTAL_INCOME, " +
                                         "LN_APPCIF.CBS_ISIC1 + '-' + ISNULL((select top 1 ISIC1_DETAIL from ST_ISIC1 where LN_APPCIF.CBS_ISIC1 = ISIC1_CD),'-') AS ISIC1_NAME, " +
                                         "LN_APPCIF.CBS_ISIC2 + '-' + ISNULL((select top 1 ISIC2_DETAIL from ST_ISIC2 where LN_APPCIF.CBS_ISIC1 = ISIC1_CD and LN_APPCIF.CBS_ISIC2 = ISIC2_CD),'-') AS ISIC2_NAME, " +
                                         "LN_APPCIF.CBS_ISIC3 + '-' + ISNULL((select top 1 ISIC3_DETAIL from ST_ISIC3 where LN_APPCIF.CBS_ISIC1 = ISIC1_CD and LN_APPCIF.CBS_ISIC2 = ISIC2_CD and LN_APPCIF.CBS_ISIC3 = ISIC3_CD),'-') AS ISIC3_NAME, " +
                                         "case when (NUM_CARD = 0) or (NUM_CARD is null) then '-' else NUM_CARD end as NUM_CARD, " +
                                         "(CASE WHEN CUST_TYPE = '01' then '01: เคยเป็นลูกค้าสินเชื่อของธนาคารแต่ปิดบัญชีไปแล้ว' WHEN CUST_TYPE = '02' then '02: ปัจจุบันเป็นลูกค้าสินเชื่อของธนาคาร' WHEN CUST_TYPE = '03' then '03: ไม่เคยเป็นลูกค้าสินเชื่อของธนาคาร' ELSE '-' END) as CUST_TYPE, " +
                                         "ISNULL(OUT_DEPT,'-') as OUT_DEPT , (CASE WHEN BKLST_HIST = 'N' then 'N: ไม่มี' WHEN   BKLST_HIST = 'Y' then 'Y: มี' ELSE '-' END) as BKLST_HIST , " +
                                         "(CASE WHEN LITIG_STAT = '02' then '02:ไม่มี' WHEN   LITIG_STAT = '01' then '01:มี' ELSE '-' END) as LITIG_STAT, " +
                                         "(CASE WHEN MESS_CHENL = '01' THEN '01: โทรทัศน์' WHEN MESS_CHENL = '02' THEN '02: วิทยุ' WHEN MESS_CHENL = '03' THEN '03: หนังสือพิมพ์/แมกกาซีน' WHEN MESS_CHENL = '04' THEN '04: Internet/ E-mail' WHEN MESS_CHENL = '05' THEN '05: " +
                                         "บุคคลแนะนำ' WHEN MESS_CHENL = '06' THEN '06: Event' WHEN MESS_CHENL = '07' THEN '07: แผ่นปลิว/สื่อสิ่งพิมพ์/ป้ายโฆษณา' ELSE '-' END) as MESS_CHENL , " +
                                         "(CASE WHEN CBS_AUTO_STATUS = 1 then '1: ไม่มี' WHEN CBS_AUTO_STATUS = 2 then '2: มีและมีภาระหนี้' WHEN CBS_AUTO_STATUS = 3 then '3: มีและไม่มีภาระหนี้' ELSE '-' END) as CBS_AUTO_STATUS, " +
                                         "(CASE WHEN CBS_PROP_STATUS = 1 then '1: ไม่มี' WHEN CBS_PROP_STATUS = 2 then '2: มีและมีภาระหนี้' WHEN CBS_PROP_STATUS = 3 then '3: มีและไม่มีภาระหนี้' ELSE '-' END) as CBS_PROP_STATUS, " +
                       //Tai 2014-03-22  
                                         //"(DATEDIFF(YEAR,CBS_CDOB,GETDATE())-CASE WHEN DATEADD(YY,DATEDIFF(YEAR,CBS_CDOB,GETDATE()),CBS_CDOB) > GETDATE() THEN 1 ELSE 0 END) as AGE, " +
                                         "(SELECT top 1 YEAR(APP_DATE) FROM LN_APP WHERE LN_APPCIF.CBS_APP_NO=LN_APP.APP_NO)-YEAR(CBS_CDOB) AS AGE, " +                  
                                         "COBORREL,GUARAREL " +
                                         "from LN_APPCIF " +
                                         "where LEN(CBS_APP_NO)>0 AND LEN(APP_SEQ)>0 AND CBS_APP_NO = '{0}' AND APP_SEQ = '{1}' ", Request.QueryString["id"], Request.QueryString["seq"]);

            DataTable dt = sqlconn.ExcuteSQL(Query);

            if (dt != null)
                if (dt.Rows.Count > 0)
                {
                    Loaner.DataSource = dt;
                    Loaner.DataBind();
                    Session["dtLoaner"] = dt;
                }
        }
        private void BindDataCollateral()
        {
            string Query = string.Format("select LN_APPCOLL.COLLTYPE + '-' + ISNULL((select distinct top 1 COLLTYPE_NAME from ST_COLLTYPE where LN_APPCOLL.COLLTYPE = COLLTYPE_CD),'-') AS COLLTYPE, " +
                                         "LN_APPCOLL.COLLSTYPE + '-' + ISNULL((select distinct top 1 COLLSTYPE_NAME from ST_COLLSTYPE where LN_APPCOLL.COLLSTYPE = COLLSTYPE_CD),'-') AS COLLSTYPE, " +
                                         "LN_APPCOLL.COLL_ID,LN_APPCOLL.COLLVAL " +
                                         "from LN_APPCOLL " +
                                         "where LEN(APP_NO)>0 AND LEN(APP_SEQ)>0 AND APP_NO = '{0}' AND APP_SEQ = '{1}' ", Request.QueryString["id"], Request.QueryString["seq"]);

            DataTable dt = sqlconn.ExcuteSQL(Query);

            if (dt != null)
                if (dt.Rows.Count > 0)
                {
                    Collateral.DataSource = dt;
                    Collateral.DataBind();
                    Session["dtCollateral"] = dt;
                }
        }

        #endregion Private Method

        #region Protected Method

        protected void Loaner_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var People = (Table)e.Item.FindControl("People");
                var CIFNum = (Label)People.FindControl("CIFNum");
                var AppChar = (Repeater)People.FindControl("AppChar");                

                if (Request.QueryString["req"] == "Analysis")
                {
                    string Query = string.Format("select distinct LN_CHAR.CHAR_CD, LN_CHAR.SCORE, " +
                                                 "(select ST_CHAR.CHAR_NAME from ST_CHAR where LN_CHAR.CHAR_CD = ST_CHAR.CHAR_CD) [CHAR_NAME] " +
                                                 "from LN_CHAR " +
                                                 "where CBS_APP_NO = '{0}' and APP_SEQ = '{1}' and CBS_CIFNO = '{2}' ",
                                                 Request.QueryString["id"], Request.QueryString["seq"], CIFNum.Text); //dt.Rows[e.Item.ItemIndex]["CBS_CIFNO"]

                    DataTable dt = sqlconn.ExcuteSQL(Query);
                    Session["dtChar_" + CIFNum.Text.Trim()] = dt;
                    if (Session["dtChar_CBS_CIFNO"] != null)
                    {
                        if (Session["dtChar_CBS_CIFNO"].ToString().Trim().Length != 0)
                        {
                            Session["dtChar_CBS_CIFNO"] = Session["dtChar_CBS_CIFNO"].ToString().Trim() + "|" + CIFNum.Text.Trim();
                        }
                        else
                        {
                            Session["dtChar_CBS_CIFNO"] = CIFNum.Text.Trim();
                        }
                    }
                    else
                    {
                        Session["dtChar_CBS_CIFNO"] = CIFNum.Text.Trim();
                    }

                    if (dt != null)
                        if (dt.Rows.Count > 0)
                        {
                            AppChar.DataSource = dt;
                            AppChar.DataBind();
                        }
                }
                else
                {
                    var TableChar = (Table)People.FindControl("TableChar");
                    TableChar.Visible = false;
                }
            }
        }


        //protected void AppChar_OnRowCreated(object sender, GridViewRowEventArgs e)
        //{
        //    if (e.Row.RowType == DataControlRowType.Header)
        //    {
        //        GridView oGridView = (GridView)sender;
        //        GridViewRow oGridViewRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);
        //        TableHeaderCell oTableCell = new TableHeaderCell();
        //        oTableCell.Text = "ข้อมูลด้านปัจจัย";
        //        oTableCell.HorizontalAlign = HorizontalAlign.Center;
        //        oTableCell.ColumnSpan = 2;
        //        oGridViewRow.Cells.Add(oTableCell);
        //        oGridView.Controls[0].Controls.AddAt(0, oGridViewRow);
        //    }
        //}

        #endregion Protected Method

        protected void PrintPageDetails_Click(object sender, EventArgs e)
        {
            //Call report
            if (Session["RequestFor"].ToString().Trim() == "Analysis")
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "aa", "window.open('ReportLayOut/frmPreviewLoanRpt.aspx?ReportId=2');", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "aa", "window.open('ReportLayOut/frmPreviewLoanRpt.aspx?ReportId=1');", true);
            }
        }


    }
}