<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LoanDetails.aspx.cs" Inherits="GSB.Loan.LoanDetails" EnableEventValidation="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
     <link rel="stylesheet" media="screen" href="<%=Page.ResolveUrl("~/")%>styles/master.css" />
    <link rel="stylesheet" media="screen" href="<%=Page.ResolveUrl("~/")%>styles/LoanDetailScreen.css" />
    <link rel="stylesheet" media="print" href="<%=Page.ResolveUrl("~/")%>styles/LoanDetailPrint.css" />
</head>
<body>
    <form id="form1" runat="server">
    <div id="PrintGSB">
        <asp:Image ID="ImgLogoGsb" runat="server" ImageUrl="~/Images/logoGSB.png" ImageAlign="AbsMiddle" />
    </div>
    <div id="PrintDetail">
        <div class="ReportName">
            <asp:Label ID="title" runat="server" /></div>
        <div class="PrintDate">
            <asp:Label ID="printDateTxt" runat="server" /></div>
    </div>
    <asp:Repeater ID="Application" runat="server">
        <ItemTemplate>
            <asp:Table ID="AppTable" runat="server" BorderWidth="0" CellPadding="0" CellSpacing="0"
                CssClass="application" >
                <asp:TableRow ID="TableRow1" runat="server">
                    <asp:TableCell ColumnSpan="4" VerticalAlign="Middle" CssClass="header">
                        ข้อมูลใบคำขอสินเชื่อ
                        <div class="printicon">
                            <asp:ImageButton ID="PrintPageDetails" runat="server" ImageUrl="~/Images/Print.png" OnClick="PrintPageDetails_Click" CausesValidation="false" /></div>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow runat="server" CssClass="content">
                    <asp:TableCell CssClass="col1">เลขที่ใบคำขอ</asp:TableCell>
                    <asp:TableCell CssClass="col2"><b><u><%# DataBinder.Eval(Container.DataItem, "APP_NO")%></u></b></asp:TableCell>
                    <asp:TableCell CssClass="col3">รูปแบบโมเดล</asp:TableCell>
                    <asp:TableCell CssClass="col4"><%# DataBinder.Eval(Container.DataItem, "MODEL_NAME")%></asp:TableCell>
                </asp:TableRow>
                <asp:TableRow runat="server" CssClass="content">
                    <asp:TableCell CssClass="col1">วันที่เปิดใบคำขอ</asp:TableCell>
                    <asp:TableCell CssClass="col2"><%# DataBinder.Eval(Container.DataItem, "APP_DATE", "{0:dd/MM/yyyy}")%></asp:TableCell>
                    <asp:TableCell CssClass="col3">ประเภทสินเชื่อ</asp:TableCell>
                    <asp:TableCell CssClass="col4"><%# DataBinder.Eval(Container.DataItem, "LOAN_NAME") %></asp:TableCell>
                </asp:TableRow>
                <asp:TableRow runat="server" CssClass="content">
                    <asp:TableCell CssClass="col1">เปิดใบคำขอโดย</asp:TableCell>
                    <asp:TableCell CssClass="col2"><%# DataBinder.Eval(Container.DataItem, "CREATION_BY")%></asp:TableCell>
                    <asp:TableCell CssClass="col3">ประเภทสินเชื่อย่อย</asp:TableCell>
                    <asp:TableCell CssClass="col4"><%# DataBinder.Eval(Container.DataItem, "STYPE_NAME")%></asp:TableCell>
                </asp:TableRow>
                <asp:TableRow runat="server" CssClass="content">
                    <asp:TableCell CssClass="col1">สาขาที่เปิดใบคำขอ</asp:TableCell>
                    <asp:TableCell CssClass="col2"><%# DataBinder.Eval(Container.DataItem, "BRANCH_NAME")%></asp:TableCell>
                    <asp:TableCell CssClass="col3">Market Code</asp:TableCell>
                    <asp:TableCell CssClass="col4"><%# DataBinder.Eval(Container.DataItem, "MARKET_NAME")%></asp:TableCell>
                </asp:TableRow>
                <asp:TableRow runat="server" CssClass="content">
                    <asp:TableCell CssClass="col1">วงเงินที่ขอกู้</asp:TableCell>
                    <asp:TableCell CssClass="col2"><%# DataBinder.Eval(Container.DataItem, "LOAN_AMOUNT", "{0:N} บาท")%></asp:TableCell>
                    <asp:TableCell CssClass="col3">วัตถุประสงค์การกู้ทั่วไป</asp:TableCell>
                    <asp:TableCell CssClass="col4"><%# DataBinder.Eval(Container.DataItem, "PURPOSE_NAME")%></asp:TableCell>
                </asp:TableRow>
                <asp:TableRow runat="server" CssClass="content">
                    <asp:TableCell CssClass="col1">มูลค่าสินทรัพย์ค้ำ</asp:TableCell>
                    <asp:TableCell CssClass="col2"><%# DataBinder.Eval(Container.DataItem, "LNCOLLVAL", "{0:N} บาท")%></asp:TableCell>
                    <asp:TableCell CssClass="col3">วัตถุประสงค์ตาม BOT</asp:TableCell>
                    <asp:TableCell CssClass="col4"><%# DataBinder.Eval(Container.DataItem, "CONSUMPTION_NAME")%></asp:TableCell>
                </asp:TableRow>
                <asp:TableRow runat="server" CssClass="content">
                    <asp:TableCell CssClass="col1">ธุรกิจประเภท 1</asp:TableCell>
                    <asp:TableCell CssClass="col2"><%# DataBinder.Eval(Container.DataItem, "ISIC1_NAME")%></asp:TableCell>
                    <asp:TableCell CssClass="col3">วัตถุประสงค์การกู้ย่อย</asp:TableCell>
                    <asp:TableCell CssClass="col4"><%# DataBinder.Eval(Container.DataItem, "GSBPURPOSE_NAME")%></asp:TableCell>
                </asp:TableRow>
                <asp:TableRow runat="server" CssClass="content">
                    <asp:TableCell CssClass="col1">ธุรกิจประเภท 2</asp:TableCell>
                    <asp:TableCell CssClass="col2"><%# DataBinder.Eval(Container.DataItem, "ISIC2_NAME")%></asp:TableCell>
                    <asp:TableCell CssClass="col3">คะแนนใบคำขอ</asp:TableCell>
                    <asp:TableCell CssClass="col4"><%# DataBinder.Eval(Container.DataItem, "SCORE")%></asp:TableCell>
                </asp:TableRow>
                <asp:TableRow  runat="server" CssClass="content">
                    <asp:TableCell CssClass="col1">ธุรกิจประเภท 3</asp:TableCell>
                    <asp:TableCell CssClass="col2"><%# DataBinder.Eval(Container.DataItem, "ISIC3_NAME")%></asp:TableCell>
                    <asp:TableCell CssClass="col3">เกรดใบคำขอ</asp:TableCell>
                    <asp:TableCell CssClass="col4"><%# DataBinder.Eval(Container.DataItem, "GRADE")%></asp:TableCell>
                </asp:TableRow>                
                <asp:TableRow ID="TableRow2" runat="server" CssClass="content">
                    <asp:TableCell CssClass="col1">ระยะเวลาขอกู้สินเชื่อ</asp:TableCell>
                    <asp:TableCell CssClass="col2"><%# DataBinder.Eval(Container.DataItem, "LOAN_TERM")%></asp:TableCell>
                    <asp:TableCell CssClass="col3">สถานะใบคำขอสินเชื่อ</asp:TableCell>
                    <asp:TableCell CssClass="col4"><%# DataBinder.Eval(Container.DataItem, "LNSTATUS")%></asp:TableCell>
                </asp:TableRow>
                <asp:TableRow ID="TableRow3" runat="server" CssClass="content">
                    <asp:TableCell CssClass="col1">จำนวนเงินผ่อนชำระรายงวด</asp:TableCell>
                    <asp:TableCell CssClass="col2"><%# DataBinder.Eval(Container.DataItem, "MPAYAMT")%></asp:TableCell>
                    <asp:TableCell CssClass="col3">วงเงินอนุมัติ</asp:TableCell>
                    <asp:TableCell CssClass="col4"><%# DataBinder.Eval(Container.DataItem, "APPRAMT")%></asp:TableCell>
                </asp:TableRow>
                <asp:TableRow ID="TableRow47" runat="server" CssClass="content">
                    <asp:TableCell CssClass="col1">จำนวนผู้กู้ร่วม</asp:TableCell>
                    <asp:TableCell CssClass="col2"><%# DataBinder.Eval(Container.DataItem, "LOAN_NUM")%></asp:TableCell>
                    <asp:TableCell CssClass="col3">ระยะเวลาให้กู้</asp:TableCell>
                    <asp:TableCell CssClass="col4"><%# DataBinder.Eval(Container.DataItem, "APPRTERM")%></asp:TableCell>
                </asp:TableRow>
            </asp:Table>
            <div class="sep_table">
            </div>
            <asp:Table runat="server" BorderWidth="0" CellPadding="0" CellSpacing="0"
                CssClass="application">
                <asp:TableRow runat="server">
                    <asp:TableCell CssClass="header">ข้อมูลการพิจารณาสินเชื่อ</asp:TableCell>
                </asp:TableRow>
                <asp:TableRow runat="server" CssClass="content">
                    <asp:TableCell CssClass="subtable">
                        <asp:Table runat="server" BorderWidth="0" HorizontalAlign="Center" CellPadding="0"
                            CellSpacing="0" Width="100%">
                            <asp:TableHeaderRow ID="TableHeaderRow1" runat="server" CssClass="header">
                                <asp:TableCell CssClass="sub-header-row">วันที่อนุมัติ</asp:TableCell>
                                <asp:TableCell CssClass="sub-header-row">เหตุผลการอนุมัติ</asp:TableCell>
                                <asp:TableCell CssClass="sub-header-row">รวมมูลค่าของสินทรัพย์ค้ำ(ราคาประเมิน)</asp:TableCell>
                            </asp:TableHeaderRow>
                            <asp:TableRow runat="server">
                                <asp:TableCell CssClass="sub-col-row"><%# DataBinder.Eval(Container.DataItem, "CA_Date")%></asp:TableCell>
                                <asp:TableCell CssClass="sub-col-row"><%# DataBinder.Eval(Container.DataItem, "CA_REASON")%></asp:TableCell>
                                <asp:TableCell CssClass="sub-col-row-last"><%# DataBinder.Eval(Container.DataItem, "CBS_COL")%></asp:TableCell>
                            </asp:TableRow>
                        </asp:Table>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        </ItemTemplate>
    </asp:Repeater>
    <asp:Repeater ID="Loaner" runat="server" OnItemDataBound="Loaner_ItemDataBound">
        <ItemTemplate>
            <div class="sep_table">
            </div>
            <asp:Table ID="People" runat="server" BorderWidth="0" CellPadding="0" CellSpacing="0"
                CssClass="application">
                <asp:TableRow ID="TableRow4" runat="server">
                    <asp:TableCell ColumnSpan="2" CssClass="header"><%# DataBinder.Eval(Container.DataItem, "ROW")%></asp:TableCell>
                </asp:TableRow>
                <asp:TableRow ID="TableRow5" runat="server">
                    <asp:TableCell VerticalAlign="Top" CssClass="sub-col-row21">
                        <asp:Table ID="Table1" runat="server" BorderWidth="0" HorizontalAlign="Center" CellPadding="0"
                            CellSpacing="0" Width="100%">
                            <asp:TableHeaderRow ID="TableHeaderRow2" runat="server" CssClass="header">
                                <asp:TableCell ColumnSpan="2" CssClass="sub-header-row">ข้อมูลส่วนตัว</asp:TableCell>
                            </asp:TableHeaderRow>
                            <asp:TableRow ID="TableRow6" runat="server">
                                <asp:TableCell CssClass="sub-row-cif">ทะเบียนลูกค้า</asp:TableCell>
                                <asp:TableCell CssClass="sub-row-data">
                                    <b><u>
                                        <asp:Label ID="CIFNum" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "CBS_CIFNO")%>' /></u></b></asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow ID="TableRow7" runat="server">
                                <asp:TableCell CssClass="sub-row-cif">ชื่อ-สกุล</asp:TableCell>
                                <asp:TableCell CssClass="sub-row-data"><%# DataBinder.Eval(Container.DataItem, "CBS_CIFNAME")%></asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow ID="TableRow8" runat="server">
                                <asp:TableCell CssClass="sub-row-cif">เพศ</asp:TableCell>
                                <asp:TableCell CssClass="sub-row-data"><%# DataBinder.Eval(Container.DataItem, "CBS_GENDER_CD")%></asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow ID="TableRow9" runat="server">
                                <asp:TableCell CssClass="sub-row-cif">วัน/เดือน/ปี เกิด</asp:TableCell>
                                <asp:TableCell CssClass="sub-row-data"><%# DataBinder.Eval(Container.DataItem, "CBS_CDOB", "{0:dd/MM/yyyy}")%></asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow ID="TableRow10" runat="server">
                                <asp:TableCell CssClass="sub-row-cif">อายุของผู้กู้</asp:TableCell>
                                <asp:TableCell CssClass="sub-row-data"><%# DataBinder.Eval(Container.DataItem, "AGE")%></asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow ID="TableRow48" runat="server">
                                <asp:TableCell CssClass="sub-row-cif">เลขที่บัตรประจำตัวประชาชน</asp:TableCell>
                                <asp:TableCell CssClass="sub-row-data"><%# DataBinder.Eval(Container.DataItem, "CBS_CITIZID")%></asp:TableCell><%-- String.Format("{0:#-####-#####-##-#}", Double.Parse((string)Eval("CBS_CITIZID")))--%>
                            </asp:TableRow>
                            <asp:TableRow ID="TableRow49" runat="server">
                                <asp:TableCell CssClass="sub-row-cif">หมายเลข Passport</asp:TableCell>
                                <asp:TableCell CssClass="sub-row-data"><%# DataBinder.Eval(Container.DataItem, "PASSID")%></asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow ID="TableRow11" runat="server">
                                <asp:TableCell CssClass="sub-row-cif">ระดับการศึกษา</asp:TableCell>
                                <asp:TableCell CssClass="sub-row-data"><%# DataBinder.Eval(Container.DataItem, "CBS_EDU_CD")%></asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow ID="TableRow12" runat="server">
                                <asp:TableCell CssClass="sub-row-cif">สถานะที่พักอาศัย</asp:TableCell>
                                <asp:TableCell CssClass="sub-row-data"><%# DataBinder.Eval(Container.DataItem, "CBS_RESSTATUS_CD")%></asp:TableCell>
                            </asp:TableRow>
                           <asp:TableRow ID="TableRow13" runat="server">
                                <asp:TableCell CssClass="sub-row-cif">ระยะเวลาที่อาศัยอยู่ในที่อยู่ปัจจุบัน(เดือน)</asp:TableCell>
                                <asp:TableCell CssClass="sub-row-data"><%# DataBinder.Eval(Container.DataItem, "CBS_PADDRTIME")%> เดือน</asp:TableCell>
                            </asp:TableRow>
                           <asp:TableRow ID="TableRow14"  runat="server">
                                <asp:TableCell CssClass="sub-row-cif">ประเภทที่พักอาศัย</asp:TableCell>
                                <asp:TableCell CssClass="sub-row-data"><%# DataBinder.Eval(Container.DataItem, "CBS_RESTYPE_CD")%></asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow ID="TableRow15" runat="server">
                                <asp:TableCell CssClass="sub-row-cif">สถานภาพสมรส</asp:TableCell>
                                <asp:TableCell CssClass="sub-row-data"><%# DataBinder.Eval(Container.DataItem, "CBS_MARITAL_CD")%></asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow ID="TableRow16" runat="server">
                                <asp:TableCell CssClass="sub-row-cif">จำนวนบุตรทั้งหมด</asp:TableCell>
                                <asp:TableCell CssClass="sub-row-data"><%# DataBinder.Eval(Container.DataItem, "CBS_CHILD")%> คน</asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow ID="TableRow17" runat="server">
                                <asp:TableCell CssClass="sub-row-cif">จำนวนบุตรที่ทำงานแล้ว</asp:TableCell>
                                <asp:TableCell CssClass="sub-row-data"><%# DataBinder.Eval(Container.DataItem, "CBS_WCHILD")%> คน</asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow ID="TableRow50" runat="server">
                              <asp:TableCell CssClass="sub-row-cif">ประเภทลูกค้า</asp:TableCell>
                                <asp:TableCell CssClass="sub-row-data"><%# DataBinder.Eval(Container.DataItem, "CBS_CTYPE")%></asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow ID="TableRow51" runat="server">
                                <asp:TableCell CssClass="sub-row-cif">รหัสประเภทลูกค้า</asp:TableCell>
                                <asp:TableCell CssClass="sub-row-data"><%# DataBinder.Eval(Container.DataItem, "CBS_CCODE")%></asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow ID="TableRow52" runat="server">
                                <asp:TableCell CssClass="sub-row-cif">ความสัมพันธ์ของผู้กู้ร่วมที่มีต่อการกู้</asp:TableCell>
                                <asp:TableCell CssClass="sub-row-data"><%# DataBinder.Eval(Container.DataItem, "COBORREL")%></asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow ID="TableRow53" runat="server">
                                <asp:TableCell CssClass="sub-row-cif">ความสัมพันธ์ของผู้ค้ำประกันกับผู้กู้หลัก</asp:TableCell>
                                <asp:TableCell CssClass="sub-row-data"><%# DataBinder.Eval(Container.DataItem, "GUARAREL")%></asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow ID="TableRow18" runat="server">
                                <asp:TableCell CssClass="sub-row-cif">คะแนนผู้กู้หลัก/ผู้กู้ร่วม</asp:TableCell>
                                <asp:TableCell CssClass="sub-row-data"><%# DataBinder.Eval(Container.DataItem, "CIF_SCORE")%></asp:TableCell>
                            </asp:TableRow>
                        </asp:Table>
                        <div class="sep_table">
                        </div>
                        <asp:Table ID="Table2" runat="server" BorderWidth="0" HorizontalAlign="Center" CellPadding="0"
                            CellSpacing="0" Width="100%">
                            <asp:TableHeaderRow ID="TableHeaderRow3" runat="server" CssClass="header">
                                <asp:TableCell ColumnSpan="2" CssClass="sub-header-row">ข้อมูลด้านอาชีพ</asp:TableCell>
                            </asp:TableHeaderRow>
                            <asp:TableRow ID="TableRow19" runat="server">
                                <asp:TableCell CssClass="sub-row-cif">อาชีพหลัก</asp:TableCell>
                                <asp:TableCell CssClass="sub-row-data"><%# DataBinder.Eval(Container.DataItem, "CBS_OCCCLS_CD")%></asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow ID="TableRow20"  runat="server">
                                <asp:TableCell CssClass="sub-row-cif">อาชีพย่อย</asp:TableCell>
                                <asp:TableCell CssClass="sub-row-data"><%# DataBinder.Eval(Container.DataItem, "CBS_OCCSCLS_CD")%></asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow ID="TableRow21" runat="server">
                                <asp:TableCell CssClass="sub-row-cif">ISIC1</asp:TableCell>
                                <asp:TableCell CssClass="sub-row-data"><%# DataBinder.Eval(Container.DataItem, "ISIC1_NAME")%></asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow ID="TableRow22" runat="server">
                                <asp:TableCell CssClass="sub-row-cif">ISIC2</asp:TableCell>
                                <asp:TableCell CssClass="sub-row-data"><%# DataBinder.Eval(Container.DataItem, "ISIC2_NAME")%></asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow ID="TableRow23"  runat="server">
                                <asp:TableCell CssClass="sub-row-cif">ISIC3</asp:TableCell>
                                <asp:TableCell CssClass="sub-row-data"><%# DataBinder.Eval(Container.DataItem, "ISIC3_NAME")%></asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow ID="TableRow24" runat="server">
                                <asp:TableCell CssClass="sub-row-cif">อายุงานปัจจุบัน(เดือน)</asp:TableCell>
                                <asp:TableCell CssClass="sub-row-data"><%# DataBinder.Eval(Container.DataItem, "CBS_PWORKTIME")%> เดือน</asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow ID="TableRow25" runat="server">
                                <asp:TableCell CssClass="sub-row-cif">อายุงานรวม(เดือน)</asp:TableCell>
                                <asp:TableCell CssClass="sub-row-data"><%# DataBinder.Eval(Container.DataItem, "CBS_TWORKTIME")%> เดือน</asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow ID="TableRow26" runat="server">
                                <asp:TableCell CssClass="sub-row-cif">ระยะเวลาประกอบการ</asp:TableCell>
                                <asp:TableCell CssClass="sub-row-data"><%# DataBinder.Eval(Container.DataItem, "CBS_BUSSYEAR")%> ปี <%# DataBinder.Eval(Container.DataItem, "CBS_BUSSMONTH")%> เดือน</asp:TableCell>
                            </asp:TableRow>
                           <asp:TableRow ID="TableRow27" runat="server">
                                <asp:TableCell CssClass="sub-row-cif">กรรมสิทธิ์ในสถานที่ประกอบการ</asp:TableCell>
                                <asp:TableCell CssClass="sub-row-data"><%# DataBinder.Eval(Container.DataItem, "CBS_PROP_RIGHT")%></asp:TableCell>
                            </asp:TableRow>
                           <asp:TableRow ID="TableRow28"  runat="server">
                                <asp:TableCell CssClass="sub-row-cif">ลักษณะสถานที่ประกอบการ</asp:TableCell>
                                <asp:TableCell CssClass="sub-row-data"><%# DataBinder.Eval(Container.DataItem, "CBS_BUSSTYPE")%></asp:TableCell>
                            </asp:TableRow>
                        </asp:Table>
                    </asp:TableCell>
                    <asp:TableCell VerticalAlign="Top" CssClass="sub-col-row22">
                        <asp:Table ID="Table3" runat="server" BorderWidth="0" HorizontalAlign="Center" CellPadding="0"
                            CellSpacing="0" Width="100%">
                            <asp:TableHeaderRow ID="TableHeaderRow4" runat="server" CssClass="header">
                                <asp:TableCell ColumnSpan="2" CssClass="sub-header-row">ข้อมูลด้านรายได้</asp:TableCell>
                            </asp:TableHeaderRow>
                            <asp:TableRow ID="TableRow29" runat="server">
                                <asp:TableCell CssClass="sub-row-cif">รายได้ต่อเดือน</asp:TableCell>
                                <asp:TableCell CssClass="sub-row-data"><%# DataBinder.Eval(Container.DataItem, "CBS_INCOME", "{0:N} บาท")%></asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow ID="TableRow30" runat="server">
                                <asp:TableCell CssClass="sub-row-cif">รายได้อื่นๆ ต่อเดือน</asp:TableCell>
                                <asp:TableCell CssClass="sub-row-data"><%# DataBinder.Eval(Container.DataItem, "CBS_OTHINCOME", "{0:N} บาท")%></asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow ID="TableRow31" runat="server">
                                <asp:TableCell CssClass="sub-row-cif">รายจ่ายทั่วไปต่อเดือน</asp:TableCell>
                                <asp:TableCell CssClass="sub-row-data"><%# DataBinder.Eval(Container.DataItem, "CBS_MNTEXPEN", "{0:N} บาท")%></asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow ID="TableRow32" runat="server">
                                <asp:TableCell CssClass="sub-row-cif">รายจ่ายชำระหนี้ต่อเดือน</asp:TableCell>
                                <asp:TableCell CssClass="sub-row-data"><%# DataBinder.Eval(Container.DataItem, "CBS_LBTEXPEN", "{0:N} บาท")%></asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow ID="TableRow33" runat="server">
                                <asp:TableCell CssClass="sub-row-cif">ค่าใช้จ่ายการประกอบธุรกิจ</asp:TableCell>
                                <asp:TableCell CssClass="sub-row-data"><%# DataBinder.Eval(Container.DataItem, "CBS_BUSEXPEN", "{0:N} บาท")%></asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow ID="TableRow34" runat="server">
                                <asp:TableCell CssClass="sub-row-cif">เงินคงเหลือสุทธิ</asp:TableCell>
                                <asp:TableCell CssClass="sub-row-data"><%# DataBinder.Eval(Container.DataItem, "TOTAL_INCOME", "{0:N} บาท")%></asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow ID="TableRow35"  runat="server">
                                <asp:TableCell CssClass="sub-row-cif">จำนวนบัตรเครดิตที่ถืออยู่</asp:TableCell>
                                <asp:TableCell CssClass="sub-row-data"><%# DataBinder.Eval(Container.DataItem, "NUM_CARD", "{0:N} ใบ")%></asp:TableCell>
                            </asp:TableRow>
                           <asp:TableRow ID="TableRow45"  runat="server">
                                <asp:TableCell CssClass="sub-row-cif">ภาระหนี้สิน - รถยนต์</asp:TableCell>
                                <asp:TableCell CssClass="sub-row-data"><%# DataBinder.Eval(Container.DataItem, "CBS_AUTO_STATUS")%></asp:TableCell>
                            </asp:TableRow>
                           <asp:TableRow ID="TableRow46"  runat="server">
                                <asp:TableCell CssClass="sub-row-cif">ภาระหนี้สิน - อสังหาริมทรัพย์</asp:TableCell>
                                <asp:TableCell CssClass="sub-row-data"><%# DataBinder.Eval(Container.DataItem, "CBS_PROP_STATUS")%></asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow ID="TableRow36"  runat="server">
                                <asp:TableCell CssClass="sub-row-cif">ประเภทลูกค้า(ExtraField)</asp:TableCell>
                                <asp:TableCell CssClass="sub-row-data"><%# DataBinder.Eval(Container.DataItem, "CUST_TYPE")%></asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow ID="TableRow37"  runat="server">
                                <asp:TableCell CssClass="sub-row-cif">ภาระหนี้คงเหลือของที่พักอาศัย</asp:TableCell>
                                <asp:TableCell CssClass="sub-row-data"><%# DataBinder.Eval(Container.DataItem, "OUT_DEPT", "{0:N} บาท")%></asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow ID="TableRow38" runat="server">
                                <asp:TableCell CssClass="sub-row-cif">ประวัติ BlackList</asp:TableCell>
                                <asp:TableCell CssClass="sub-row-data"><%# DataBinder.Eval(Container.DataItem, "BKLST_HIST")%></asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow ID="TableRow39"  runat="server">
                                <asp:TableCell CssClass="sub-row-cif">สถานะฟ้องร้องคดี</asp:TableCell>
                                <asp:TableCell CssClass="sub-row-data"><%# DataBinder.Eval(Container.DataItem, "LITIG_STAT")%></asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow ID="TableRow40"  runat="server">
                                <asp:TableCell CssClass="sub-row-cif">ช่องทางการรับข่าวสาร</asp:TableCell>
                                <asp:TableCell CssClass="sub-row-data"><%# DataBinder.Eval(Container.DataItem, "MESS_CHENL")%></asp:TableCell>
                            </asp:TableRow>
                        </asp:Table>
                        <div class="sep_table">
                        </div>
                        <asp:Table ID="TableChar" runat="server" BorderWidth="0" HorizontalAlign="Center"
                            CellPadding="0" CellSpacing="0" Width="100%">
                            <asp:TableHeaderRow ID="TableHeaderRow5" runat="server" CssClass="header">
                                <asp:TableCell CssClass="sub-header-row" ColumnSpan="2">ข้อมูลด้านปัจจัย</asp:TableCell>
                            </asp:TableHeaderRow>
                            <asp:TableHeaderRow ID="TableHeaderRow6" runat="server" CssClass="header">
                                <asp:TableCell CssClass="sub-header-row">ชื่อปัจจัย</asp:TableCell>
                                <asp:TableCell CssClass="sub-header-row">คะแนน</asp:TableCell>
                            </asp:TableHeaderRow>
                            <asp:TableRow ID="TableRow41" runat="server">
                                <asp:TableCell>
                                    </td></tr>
                                    <asp:Repeater ID="AppChar" runat="server">
                                        <ItemTemplate>
                                            <tr>
                                                <td class="sub-row-char-name">
                                                    <%# DataBinder.Eval(Container.DataItem, "CHAR_NAME")%>
                                                </td>
                                                <td class="sub-row-char-data">
                                                    <%# DataBinder.Eval(Container.DataItem, "SCORE")%>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                    <tr>
                                        <td>
                                </asp:TableCell>
                            </asp:TableRow>
                        </asp:Table>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        </ItemTemplate>
    </asp:Repeater>
    <%--<asp:Repeater ID="Collateral" runat="server" >--%>
        <ItemTemplate>
            <div class="sep_table">
            </div>
            <asp:Table ID="COLL" runat="server" BorderWidth="0" CellPadding="0" CellSpacing="0"
                CssClass="application">
                <asp:TableRow ID="TableRow42" runat="server">
                    <asp:TableCell CssClass="header">ข้อมูลหลักประกัน</asp:TableCell>
                </asp:TableRow>
                <asp:TableRow ID="TableRow43" runat="server">
                    <asp:TableCell VerticalAlign="Top" CssClass="subtable">
                        <asp:Table ID="Table4" runat="server" BorderWidth="0" HorizontalAlign="Center" CellPadding="0"
                            CellSpacing="0" Width="100%">
                            <asp:TableHeaderRow ID="TableHeaderRow1" runat="server" CssClass="header">
                                <asp:TableCell CssClass="sub-header-row" Width="25%">ประเภทหลักประกัน</asp:TableCell>
                                <asp:TableCell CssClass="sub-header-row" Width="25%">ประเภทหลักประกันย่อย</asp:TableCell>
                                <asp:TableCell CssClass="sub-header-row" Width="25%">รหัสหลักประกัน</asp:TableCell>
                                <asp:TableCell CssClass="sub-header-row" Width="25%">มูลค่าหลักประกัน</asp:TableCell>
                            </asp:TableHeaderRow>
                            <asp:TableRow ID="TableRow44" runat="server">
                             <asp:TableCell>
                                    </td></tr>
                <asp:Repeater ID="Collateral" runat="server" >
                                <%--<asp:TableCell CssClass="sub-col-row" Width="25%" HorizontalAlign="Center"><%# DataBinder.Eval(Container.DataItem, "COLLTYPE")%></asp:TableCell>
                                <asp:TableCell CssClass="sub-col-row" Width="25%" HorizontalAlign="Center"><%# DataBinder.Eval(Container.DataItem, "COLLSTYPE")%></asp:TableCell>
                                <asp:TableCell CssClass="sub-col-row" Width="25%" HorizontalAlign="Center"><%# DataBinder.Eval(Container.DataItem, "COLL_ID")%></asp:TableCell>
                                <asp:TableCell CssClass="sub-col-row" Width="25%" HorizontalAlign="Center"><%# DataBinder.Eval(Container.DataItem, "COLLVAL")%></asp:TableCell>--%>
                                    <ItemTemplate>
                                            <tr >
                                                <td Width="25%" Align="Center" style="border: 1px solid #e2bbd2" >
                                                    <%# DataBinder.Eval(Container.DataItem, "COLLTYPE")%>
                                                </td>
                                                <td Width="25%" Align="Center" style="border: 1px solid #e2bbd2" >
                                                    <%# DataBinder.Eval(Container.DataItem, "COLLSTYPE")%>
                                                </td>
                                                <td Width="25%" Align="Center" style="border: 1px solid #e2bbd2" >
                                                    <%# DataBinder.Eval(Container.DataItem, "COLL_ID")%>
                                                </td>
                                                <td Width="25%" Align="Center" style="border: 1px solid #e2bbd2" >
                                                    <%# DataBinder.Eval(Container.DataItem, "COLLVAL")%>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                 </asp:Repeater>
                                    <tr>
                                        <td>
                                </asp:TableCell>
                           </asp:TableRow>
                        </asp:Table>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        </ItemTemplate>
    <%--</asp:Repeater>--%>
    </form>
</body>
</html>
