<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/CreditScoring.Master" AutoEventWireup="true" CodeBehind="DelinquencyDistributionReport.aspx.cs" Inherits="GSB.Report.BackEnd.DelinquencyDistributionReport" %>
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
                    <asp:TableHeaderCell ColumnSpan="2"><u>เครื่องมือค้นหา</u> : รายงานอัตราการผิดนัดชำระหนี้ของลูกค้าตามระดับคะแนน ณ ช่วงไตรมาสต่างกัน ในแต่ละช่วงคะแนน (Delinquency Distribution Report)</asp:TableHeaderCell>
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
                    <asp:TableCell CssClass="subject">* ช่วงเดือนที่เซ็นสัญญา</asp:TableCell>
                    <asp:TableCell CssClass="detail">
                        <asp:DropDownList ID="txtOpenAcDate" runat ="server" Enabled="true">
                         <asp:ListItem Value="1" Text="ม.ค. - มี.ค." Selected="True" />
                         <asp:ListItem Value="2" Text="เม.ย. - มิ.ย" />
                         <asp:ListItem Value="3" Text="ก.ค. - ก.ย." />
                         <asp:ListItem Value="4" Text="ต.ค. - ธ.ค." />
                        </asp:DropDownList> &nbsp ปี &nbsp
                        <asp:DropDownList ID="txtEndAcDate" runat ="server" Enabled="true"/>
                        </asp:DropDownList> 
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell CssClass="subject">* As of Date</asp:TableCell>
                    <asp:TableCell CssClass="detail">
                        <asp:DropDownList ID="ddlOpenDateMonthDTM" runat="server" />&nbsp;<asp:DropDownList ID="ddlOpenDateYearDTM" runat="server" />&nbsp;ถึง&nbsp;
                        <asp:DropDownList ID="ddlOpenDateMonthDTMend" runat="server" />&nbsp;<asp:DropDownList ID="ddlOpenDateYearDTMend" runat="server" />
<%--                        <asp:TextBox ID="txtOpenDate" runat="server" CssClass="Cal" />&nbsp;ถึง&nbsp;<asp:TextBox
                            ID="txtCloseDate" runat="server" CssClass="Cal" />--%>
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
                    <asp:TableCell CssClass="HeaderRow"><u>รายงานอัตราการผิดนัดชำระหนี้ของลูกค้าตามระดับคะแนน ณ ช่วงไตรมาสต่างกัน ในแต่ละช่วงคะแนน (Delinquency Distribution Report)</u> : </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell CssClass="TableRow">
                        <asp:Table ID="HeadTxtLoan" runat="server" CellPadding="0" CellSpacing="0" CssClass="RatingRpt">
<%--                             <asp:TableRow ID="TableRow2" runat="server">
                                <asp:TableCell Width="300px" >category</asp:TableCell>
                                <asp:TableCell Width="300px"  ColumnSpan="3" >Bad : Ever 1-30</asp:TableCell>
                                <asp:TableCell Width="300px"  ColumnSpan="3" >Bad : Ever 31-60</asp:TableCell>
                                <asp:TableCell Width="300px"  ColumnSpan="3" >Bad : Ever 61-90</asp:TableCell>
                            </asp:TableRow>--%>
                            <asp:TableRow ID="TableRow3" runat="server">
                                 <asp:TableCell ColumnSpan="8" BorderWidth="0">                                        
                        <asp:GridView ID="gvTable" runat="server" CssClass="RatingRpt" ShowHeader="true" AutoGenerateColumns="false"
                        PagerSettings-Mode="NumericFirstLast" AllowPaging="True" OnPageIndexChanging="gvTable_PageChanging" PageSize="20" OnRowCreated="gvTable_RowCreated">
                        <Columns >					
                        <asp:BoundField DataField="category" HeaderText="Cutoff Score"  ItemStyle-Width="300px" HtmlEncode="false"  NullDisplayText="0"/>
                        <asp:BoundField DataField="ApproveAccounts" HeaderText="Approve Accounts"  ItemStyle-Width="100px" HtmlEncode="false" DataFormatString="{0:N2}" NullDisplayText="0.00"/>
                        <asp:BoundField DataField="apptacc" HeaderText="%Approve Accounts"  ItemStyle-Width="100px" DataFormatString="{0:N2}" HtmlEncode="false" NullDisplayText="0.00"/>
                        <asp:BoundField DataField="ActiveAccounts" HeaderText="Active Accounts"  ItemStyle-Width="100px" DataFormatString="{0:N2}" HtmlEncode="false" NullDisplayText="0.00"/>
                        <asp:BoundField DataField="atvacpt" HeaderText="% Active Accounts"  ItemStyle-Width="100px" DataFormatString="{0:N2}" HtmlEncode="false" NullDisplayText="0.00"/>
                        <asp:BoundField DataField="B1_Flag_Y" HeaderText="Current <br /><= 1 Month"  ItemStyle-Width="100px" DataFormatString="{0:N2}" HtmlEncode="false" NullDisplayText="0.00"/>
                        <asp:BoundField DataField="BM1" HeaderText="%Current <br /><= 1 Month"  ItemStyle-Width="100px" DataFormatString="{0:N2}" HtmlEncode="false" NullDisplayText="0.00"/>
                        <asp:BoundField DataField="B2_Flag_Y" HeaderText="Current <br />>1-3 Months"  ItemStyle-Width="100px" DataFormatString="{0:N2}" HtmlEncode="false" NullDisplayText="0.00"/>
                        <asp:BoundField DataField="BM2" HeaderText="%Current <br />>1-3 Months"  ItemStyle-Width="100px" DataFormatString="{0:N2}" HtmlEncode="false" NullDisplayText="0.00"/>
                        <asp:BoundField DataField="B3_Flag_Y" HeaderText="Current <br />> 3 Months<br />(NPLs)"  ItemStyle-Width="100px" HtmlEncode="false" DataFormatString="{0:N2}" NullDisplayText="0.00"/>
                        <asp:BoundField DataField="BM3" HeaderText="%Current <br />> 3 Months<br />(NPLs)"  ItemStyle-Width="100px" HtmlEncode="false" DataFormatString="{0:N2}" NullDisplayText="0.00"/>
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
