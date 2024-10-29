<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LoanDetailMymo.aspx.cs" Inherits="GSB.Loan.LoanDetailMymo" EnableEventValidation="false"%>

<!DOCTYPE html>

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
    <asp:Repeater ID="Application" runat="server" OnItemDataBound="Loaner_ItemDataBound">
        <ItemTemplate>
            <asp:Table ID="AppTable" runat="server" BorderWidth="0" CellPadding="0" CellSpacing="0"
                CssClass="application" >
                <asp:TableRow ID="TableRow1" runat="server">
                    <asp:TableCell ColumnSpan="4" VerticalAlign="Middle" CssClass="header">
                        ข้อมูลใบคำขอสินเชื่อ
                        <div class="printicon">
                            <asp:ImageButton ID="PrintPageDetails" runat="server" ImageUrl="~/Images/Print.png" OnClick="PrintPageDetails_Click"  CausesValidation="false" /></div>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow runat="server" CssClass="content">
                    <asp:TableCell CssClass="col1">เลขที่ใบคำขอ</asp:TableCell>
                    <asp:TableCell CssClass="col2"><b><u><%# DataBinder.Eval(Container.DataItem, "APP_NO")%></u></b></asp:TableCell>
                    <asp:TableCell CssClass="col3">เงินเดือนของผู้สมัคร(ต่อเดือน)</asp:TableCell>
                    <asp:TableCell CssClass="col4"><%# DataBinder.Eval(Container.DataItem, "CBS_INCOME")%></asp:TableCell>
                </asp:TableRow>
                <asp:TableRow runat="server" CssClass="content">
                    <asp:TableCell CssClass="col1">วันที่เปิดใบคำขอ</asp:TableCell>
                    <asp:TableCell CssClass="col2"><%# DataBinder.Eval(Container.DataItem, "APP_DATE", "{0:dd/MM/yyyy}")%></asp:TableCell>
                    <asp:TableCell CssClass="col3">รายได้อื่น(ต่อเดือน)</asp:TableCell>
                    <asp:TableCell CssClass="col4"><%# DataBinder.Eval(Container.DataItem, "CBS_OTHINCOME") %></asp:TableCell>
                </asp:TableRow>
                <asp:TableRow runat="server" CssClass="content">
                    <asp:TableCell CssClass="col1">ประเภทบัญชี</asp:TableCell>
                    <asp:TableCell CssClass="col2"><%# DataBinder.Eval(Container.DataItem, "ACCTYPE")%></asp:TableCell>
                    <asp:TableCell CssClass="col3">รายได้จากการประกอบธุรกิจ(ต่อเดือน)</asp:TableCell>
                    <asp:TableCell CssClass="col4"><%# DataBinder.Eval(Container.DataItem, "CBS_NETINCOMEBUSINESS")%></asp:TableCell>
                </asp:TableRow>
                <asp:TableRow runat="server" CssClass="content">
                    <asp:TableCell CssClass="col1">รูปแบบโมเดล</asp:TableCell>
                    <asp:TableCell CssClass="col2">MyMo</asp:TableCell>
                    <asp:TableCell CssClass="col3">จำนวนบุตรทั้งหมด</asp:TableCell>
                    <asp:TableCell CssClass="col4"><%# DataBinder.Eval(Container.DataItem, "CBS_CHILD")%></asp:TableCell>
                </asp:TableRow>
                <asp:TableRow runat="server" CssClass="content">
                    <asp:TableCell CssClass="col1">ประเภทสินเชื่อ</asp:TableCell>
                    <asp:TableCell CssClass="col2"><%# DataBinder.Eval(Container.DataItem, "LOAN_NAME")%></asp:TableCell>
                    <asp:TableCell CssClass="col3">รวมรายจ่ายทั่วไป(ต่อเดือน)</asp:TableCell>
                    <asp:TableCell CssClass="col4"><%# DataBinder.Eval(Container.DataItem, "CBS_MNTEXPEN")%></asp:TableCell>
                </asp:TableRow>
                <asp:TableRow runat="server" CssClass="content">
                    <asp:TableCell CssClass="col1">ประเภทสินเชื่อย่อย</asp:TableCell>
                    <asp:TableCell CssClass="col2"><%# DataBinder.Eval(Container.DataItem, "STYPE_NAME")%></asp:TableCell>
                    <asp:TableCell CssClass="col3">รวมค่าใช้จ่ายเงินกู้(ต่อเดือน)</asp:TableCell>
                    <asp:TableCell CssClass="col4"><%# DataBinder.Eval(Container.DataItem, "CBS_LBTEXPEN")%></asp:TableCell>
                </asp:TableRow>
                <asp:TableRow runat="server" CssClass="content">
                    <asp:TableCell CssClass="col1">Marketcode</asp:TableCell>
                    <asp:TableCell CssClass="col2"><%# DataBinder.Eval(Container.DataItem, "MARKET_NAME")%></asp:TableCell>
                    <asp:TableCell CssClass="col3">รวมค่าใช้จ่ายจากการประกอบธุรกิจ(ต่อเดือน)</asp:TableCell>
                    <asp:TableCell CssClass="col4"><%# DataBinder.Eval(Container.DataItem, "CBS_BUSEXPEN")%></asp:TableCell>
                </asp:TableRow>
                <asp:TableRow runat="server" CssClass="content">
                    <asp:TableCell CssClass="col1">วัตถุประสงค์การกู้</asp:TableCell>
                    <asp:TableCell CssClass="col2"><%# DataBinder.Eval(Container.DataItem, "PURPOSE_NAME")%></asp:TableCell>
                    <asp:TableCell CssClass="col3">สถานะการอาศัย</asp:TableCell>
                    <asp:TableCell CssClass="col4"><%# DataBinder.Eval(Container.DataItem, "CBS_RESSTATUS_CD")%></asp:TableCell>
                </asp:TableRow>
                <asp:TableRow  runat="server" CssClass="content">
                    <asp:TableCell CssClass="col1">CIF Number</asp:TableCell>
                    <asp:TableCell CssClass="col2"><%# DataBinder.Eval(Container.DataItem, "CBS_CIFNO")%></asp:TableCell>
                    <asp:TableCell CssClass="col3">ระยะเวลาที่ทำงานทั้งหมด(เดือน)</asp:TableCell>
                    <asp:TableCell CssClass="col4"><%# DataBinder.Eval(Container.DataItem, "CBS_TWORKTIME")%></asp:TableCell>
                </asp:TableRow>                
                <asp:TableRow ID="TableRow2" runat="server" CssClass="content">
                    <asp:TableCell CssClass="col1">วันเดือนปีเกิด</asp:TableCell>
                    <asp:TableCell CssClass="col2"><%# DataBinder.Eval(Container.DataItem, "CBS_CDOB", "{0:dd/MM/yyyy}")%></asp:TableCell>
                    <asp:TableCell CssClass="col3">ระยะเวลาที่พักอาศัย ณ ที่อยู่ปัจจุบัน (เดือน)</asp:TableCell>
                    <asp:TableCell CssClass="col4"><%# DataBinder.Eval(Container.DataItem, "CBS_PADDRTIME")%></asp:TableCell>
                </asp:TableRow>
                <asp:TableRow ID="TableRow3" runat="server" CssClass="content">
                    <asp:TableCell CssClass="col1">เพศ</asp:TableCell>
                    <asp:TableCell CssClass="col2"><%# DataBinder.Eval(Container.DataItem, "CBS_GENDER_CD")%></asp:TableCell>
                    <asp:TableCell CssClass="col3">ประเภทที่พักอาศัย</asp:TableCell>
                    <asp:TableCell CssClass="col4"><%# DataBinder.Eval(Container.DataItem, "CBS_RESTYPE_CD")%></asp:TableCell>
                </asp:TableRow>
                <asp:TableRow ID="TableRow47" runat="server" CssClass="content">
                    <asp:TableCell CssClass="col1">สถานภาพ</asp:TableCell>
                    <asp:TableCell CssClass="col2"><%# DataBinder.Eval(Container.DataItem, "CBS_MARITAL_CD")%></asp:TableCell>
                    <asp:TableCell CssClass="col3">ผลการพิจารณา</asp:TableCell>
                    <asp:TableCell CssClass="col4"><%# DataBinder.Eval(Container.DataItem, "LNSTATUS")%></asp:TableCell>
                </asp:TableRow>
                <asp:TableRow ID="TableRow4" runat="server" CssClass="content">
                    <asp:TableCell CssClass="col1">อาชีพ</asp:TableCell>
                    <asp:TableCell CssClass="col2"><%# DataBinder.Eval(Container.DataItem, "CBS_OCCSCLS_CD")%></asp:TableCell>
                    <asp:TableCell CssClass="col3">เกรด</asp:TableCell>
                    <asp:TableCell CssClass="col4"><%# DataBinder.Eval(Container.DataItem, "GRADE")%></asp:TableCell>
                </asp:TableRow>
                <asp:TableRow ID="TableRow5" runat="server" CssClass="content">
                    <asp:TableCell CssClass="col1">ระดับการศึกษา</asp:TableCell>
                    <asp:TableCell CssClass="col2"><%# DataBinder.Eval(Container.DataItem, "CBS_EDU_CD")%></asp:TableCell>
                    <asp:TableCell CssClass="col3">คะแนน</asp:TableCell>
                    <asp:TableCell CssClass="col4"><%# DataBinder.Eval(Container.DataItem, "SCORE")%></asp:TableCell>
                </asp:TableRow>
                <asp:TableRow ID="TableRow6" runat="server" CssClass="content">
                    <asp:TableCell CssClass="col1" ColumnSpan="2">

                        <asp:Table ID="TableChar" runat="server" BorderWidth="0" HorizontalAlign="Center"
                            CellPadding="0" CellSpacing="0" Width="100%" >
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
                                                <td class="sub-row-char-name-mymo">
                                                    <%# DataBinder.Eval(Container.DataItem, "CHAR_NAME")%>
                                                </td>
                                                <td class="sub-row-char-data-mymo">
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
*
        </ItemTemplate>
    </asp:Repeater>

    </form>
</body>
</html>
