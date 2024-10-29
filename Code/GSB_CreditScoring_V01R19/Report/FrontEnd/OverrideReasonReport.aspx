<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/CreditScoring.Master" AutoEventWireup="true" CodeBehind="OverrideReasonReport.aspx.cs" Inherits="GSB.Report.FrontEnd.OverrideReasonReport" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Header" runat="server">
    <link rel="stylesheet" media="screen" href="<%=Page.ResolveUrl("~/")%>Styles/jquery-ui-1.7.2.custom.css" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
   <script src="<%=Page.ResolveUrl("~/")%>Scripts/jquery-ui-1.8.10.offset.datepicker.min.js" type="text/javascript"></script>

    <script type="text/javascript">
        function dpc() {
            var d = new Date();
            var toDay = d.getDate() + '/' + (d.getMonth() + 1) + '/' + (d.getFullYear() + 543);
<%--            $("#<%=txtOpenDate.ClientID%>").datepicker({ changeMonth: true, changeYear: true, dateFormat: 'dd/mm/yy', isBuddhist: true, defaultDate: toDay, dayNames: ['อาทิตย์', 'จันทร์', 'อังคาร', 'พุธ', 'พฤหัสบดี', 'ศุกร์', 'เสาร์'],
                dayNamesMin: ['อา.', 'จ.', 'อ.', 'พ.', 'พฤ.', 'ศ.', 'ส.'],
                monthNames: ['มกราคม', 'กุมภาพันธ์', 'มีนาคม', 'เมษายน', 'พฤษภาคม', 'มิถุนายน', 'กรกฎาคม', 'สิงหาคม', 'กันยายน', 'ตุลาคม', 'พฤศจิกายน', 'ธันวาคม'],
                monthNamesShort: ['ม.ค.', 'ก.พ.', 'มี.ค.', 'เม.ย.', 'พ.ค.', 'มิ.ย.', 'ก.ค.', 'ส.ค.', 'ก.ย.', 'ต.ค.', 'พ.ย.', 'ธ.ค.']
            });--%>

        }
        $(document).ready(function () { dpc(); });
        prm.add_endRequest(function () { $(document).ready(function () { dpc(); }); });
    </script>
    <asp:UpdateProgress ID="UpdateProgress" runat="server">
        <ProgressTemplate>
            <div id="preloader">
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdatePanel ID="UpdatePanel" runat="server">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="Search" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="Cancel" EventName="Click" />
        </Triggers>
        <ContentTemplate>
            <asp:Table ID="Table1" runat="server" CellPadding="0" CellSpacing="0" CssClass="Search">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell ColumnSpan="2"><u>เครื่องมือค้นหา</u> : รายงานแสดงสัดส่วนเหตุผลการ Override ของลูกค้า (Override Reason Report)</asp:TableHeaderCell>
                </asp:TableHeaderRow>
                <asp:TableRow>
                    <asp:TableCell CssClass="subject">* ประเภทโมเดล</asp:TableCell>
                    <asp:TableCell CssClass="detail">
                        <asp:DropDownList ID="ddlModel" runat="server" DataTextField="MODEL_NAME" DataValueField="MODEL_CD" OnSelectedIndexChanged="LoadLoan"  AutoPostBack="true"/>
                    </asp:TableCell>
                </asp:TableRow>
<%--                 <asp:TableRow>
                    <asp:TableCell CssClass="subject">Model Version</asp:TableCell>
                    <asp:TableCell CssClass="detail">
                 <asp:DropDownList ID="ddlModelVersion" runat="server" />
                    </asp:TableCell>
                </asp:TableRow>--%>
                <asp:TableRow>
                    <asp:TableCell CssClass="subject">* ประเภทสินเชื่อ</asp:TableCell>
                    <asp:TableCell CssClass="detail">
                        <asp:DropDownList ID="ddlLoan" runat="server" DataTextField="LOAN_NAME" DataValueField="LOAN_CD" OnSelectedIndexChanged="LoadSubType" AutoPostBack="true" />
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell CssClass="subject">* ประเภทสินเชื่อย่อย</asp:TableCell>
                    <asp:TableCell CssClass="detail">
                        <asp:DropDownList ID="ddlSubType" runat="server" DataTextField="STYPER_NAME" DataValueField="STYPE_CD" AutoPostBack="true" OnSelectedIndexChanged="LoadMarket"  Enabled="false" />
                    </asp:TableCell>
                </asp:TableRow>
                 <asp:TableRow>
                    <asp:TableCell CssClass="subject">* Market Code</asp:TableCell>
                    <asp:TableCell CssClass="detail">
                        <asp:DropDownList ID="ddlMarketFrm" runat="server" Enabled="false" DataTextField="MARKET_NAME"
                            DataValueField="MARKET_CD" />&nbsp;ถึง&nbsp;<asp:DropDownList ID="ddlMarketTo" runat="server" Enabled="false" DataTextField="MARKET_NAME"
                            DataValueField="MARKET_CD" />
                    </asp:TableCell>
                </asp:TableRow> 
                <asp:TableRow>
                    <asp:TableCell CssClass="subject">* วันที่เปิดใบคำขอสินเชื่อ</asp:TableCell>
                    <asp:TableCell CssClass="detail">
                        <asp:DropDownList ID="ddlOpenDateMonth" runat="server" />&nbsp;<asp:DropDownList ID="ddlOpenDateYear" runat="server" />
                        <%--<asp:TextBox ID="txtOpenDate" runat="server" CssClass="Cal" />--%>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableFooterRow>
                    <asp:TableCell ColumnSpan="2" CssClass="action">
                        <asp:Button ID="Search" runat="server" Text="แสดงข้อมูล" OnClick="Search_Click" />&nbsp;
                        <asp:Button ID="Cancel" runat="server" Text="ล้างข้อมูล" OnClick="Cancel_Click" />
                        <asp:Button ID="Button1" runat="server" Text="พิมพ์รายงาน" OnClick="PrintReport_Click" />&nbsp;
                         <asp:Button ID="btnExcel" runat="server" Text="ดาวน์โหลดไฟล์" OnClick="btnExcel_Click" CssClass="sent_right" />                   </asp:TableCell>
                </asp:TableFooterRow>
            </asp:Table>
            <asp:Table ID="ResultTable" runat="server" CellPadding="0" CellSpacing="0" CssClass="Result" >
                <asp:TableRow>
                    <asp:TableCell CssClass="HeaderRow"><u>รายงานแสดงสัดส่วนเหตุผลการ Override ของลูกค้า (Override Reason Report)</u> : </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell CssClass="TableRow">
                        <asp:Table ID="HeadTxtLoan" runat="server" CellPadding="0" CellSpacing="0" CssClass="RatingRpt">
<%--                             <asp:TableRow ID="TableRow2" runat="server">
                                <asp:TableCell Width="300px" >Category</asp:TableCell>
                                <asp:TableCell Width="100px"  ColumnSpan="2"  >Past 3 Months</asp:TableCell>
                                <asp:TableCell Width="100px"  ColumnSpan="2" >Past 6 Months</asp:TableCell>
                                <asp:TableCell Width="100px"  ColumnSpan="2" >Past 12 Months</asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow ID="TableRow1" runat="server">
                                <asp:TableCell Width="300px" ></asp:TableCell>
                                <asp:TableCell Width="100px" >Number of Loans</asp:TableCell>
                                <asp:TableCell Width="100px" >% of Total Overrides</asp:TableCell>
                                <asp:TableCell Width="100px" >Number of Loans</asp:TableCell>
                                <asp:TableCell Width="100px" >% of Total Overrides</asp:TableCell>
                                <asp:TableCell Width="100px" >Number of Loans</asp:TableCell>
                                <asp:TableCell Width="100px" >% of Total Overrides</asp:TableCell>
                            </asp:TableRow>--%>
                            <asp:TableRow ID="TableRow3" runat="server">
                                 <asp:TableCell ColumnSpan="7" BorderWidth="0">                                        
                        <asp:GridView ID="gvTable" runat="server" CssClass="RatingRpt" AutoGenerateColumns="false"
                        PagerSettings-Mode="NumericFirstLast" AllowPaging="True" OnPageIndexChanging="gvTable_PageChanging" PageSize="15"  OnRowDataBound="gvTable_RowDataBound" OnRowCreated="gvTable_RowCreated">
                        <Columns >					
                        <asp:BoundField DataField="REASON_NAME" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="300px" NullDisplayText="0" headertext=""  HeaderStyle-Font-Bold="true" />
                        <asp:BoundField DataField="num3m" ItemStyle-Width="100px" DataFormatString="{0:N0}" NullDisplayText="0" headertext=""  HeaderStyle-Font-Bold="true" />
                        <asp:BoundField DataField="pct3m" ItemStyle-Width="100px" DataFormatString="{0:N2}" NullDisplayText="0" headertext=""  HeaderStyle-Font-Bold="true" />
                        <asp:BoundField DataField="num12m" ItemStyle-Width="100px" DataFormatString="{0:N0}" NullDisplayText="0" headertext=""  HeaderStyle-Font-Bold="true" />
                        <asp:BoundField DataField="pct12m" ItemStyle-Width="100px" DataFormatString="{0:N2}" NullDisplayText="0" headertext=""  HeaderStyle-Font-Bold="true" />
                        <asp:BoundField DataField="num12myg" ItemStyle-Width="100px" DataFormatString="{0:N0}" NullDisplayText="0" headertext=""  HeaderStyle-Font-Bold="true" />
                        <asp:BoundField DataField="pct12myg" ItemStyle-Width="100px" DataFormatString="{0:N2}" NullDisplayText="0" headertext=""  HeaderStyle-Font-Bold="true" />
                         </Columns>
                        </asp:GridView> 
                        </asp:TableCell>
                        </asp:TableRow>    
                         </asp:Table>
                    </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
