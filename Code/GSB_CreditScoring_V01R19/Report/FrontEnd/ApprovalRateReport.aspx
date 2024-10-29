<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/CreditScoring.Master" AutoEventWireup="true" CodeBehind="ApprovalRateReport.aspx.cs" Inherits="GSB.Report.FrontEnd.ApprovalRateReport" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Header" runat="server">
    <link rel="stylesheet" media="screen" href="<%=Page.ResolveUrl("~/")%>Styles/jquery-ui-1.7.2.custom.css" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
   <script src="<%=Page.ResolveUrl("~/")%>Scripts/jquery-ui-1.8.10.offset.datepicker.min.js" type="text/javascript"></script>

    <script type="text/javascript">
        function dpc() {
            var d = new Date();
            var toDay = d.getDate() + '/' + (d.getMonth() + 1) + '/' + (d.getFullYear() + 543);
<%--            $("#<%=txtOpenDate.ClientID%>").datepicker({                       changeMonth: true,
                changeYear: true,
                isBuddhist: true ,
                dateFormat: 'MM/yyyy',
                monthNames: ['มกราคม', 'กุมภาพันธ์', 'มีนาคม', 'เมษายน', 'พฤษภาคม', 'มิถุนายน', 'กรกฎาคม', 'สิงหาคม', 'กันยายน', 'ตุลาคม', 'พฤศจิกายน', 'ธันวาคม'],
                monthNamesShort: ['ม.ค.', 'ก.พ.', 'มี.ค.', 'เม.ย.', 'พ.ค.', 'มิ.ย.', 'ก.ค.', 'ส.ค.', 'ก.ย.', 'ต.ค.', 'พ.ย.', 'ธ.ค.'],
                onClose: function(dateText, inst)
                 {    
                   var month = $("#ui-datepicker-div .ui-datepicker-month :selected").val();   
                   var year = $("#ui-datepicker-div .ui-datepicker-year :selected").val();
                   $(this).val($.datepicker.formatDate('MM/yyyy', new Date(year, month, 1)));   
                 }  
            });
            $("#<%= txtOpenDate.ClientID %>").click(function () {
                $(".ui-datepicker-calendar").hide();
                });
            $("#<%=txtCloseDate.ClientID%>").datepicker({ changeMonth: true, changeYear: true, dateFormat: 'mm/yy', isBuddhist: true, defaultDate: toDay, dayNames: ['อาทิตย์', 'จันทร์', 'อังคาร', 'พุธ', 'พฤหัสบดี', 'ศุกร์', 'เสาร์'],
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
             <asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />
             <asp:AsyncPostBackTrigger ControlID="btnExcel" EventName="Click" />
            
        </Triggers>
        <ContentTemplate>
            <asp:Table ID="Table1" runat="server" CellPadding="0" CellSpacing="0" CssClass="Search">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell ColumnSpan="2"><u>เครื่องมือค้นหา</u> : รายงานการติดตามผลการพิจารณาสินเชื่อ ณ ช่วงเวลาที่แตกต่างกัน (Approval Rate Report)</asp:TableHeaderCell>
                </asp:TableHeaderRow>
                <asp:TableRow>
                    <asp:TableCell CssClass="subject">* ประเภทโมเดล</asp:TableCell>
                    <asp:TableCell CssClass="detail">
                        <asp:DropDownList ID="ddlModel" runat="server" DataTextField="MODEL_NAME" DataValueField="MODEL_CD" OnSelectedIndexChanged="LoadLoan"  AutoPostBack="true"/>
                    </asp:TableCell>
                </asp:TableRow>
<%--                <asp:TableRow>
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
                        <asp:Button ID="Cancel" runat="server" Text="ล้างข้อมูล" OnClick="Cancel_Click" />&nbsp;
                        <asp:Button ID="Button1" runat="server" Text="พิมพ์รายงาน" OnClick="PrintReport_Click" />&nbsp;
                        <asp:Button ID="btnExcel" runat="server" Text="ดาวน์โหลดไฟล์" OnClick="btnExcel_Click" CssClass="sent_right" />&nbsp;
                        <asp:DropDownList ID="rpttype" runat ="server" Enabled="true">
                         <asp:ListItem Value="SCORE" Text="Cutoff Score" Selected="True" />
                         <asp:ListItem Value="GRADE" Text="Cutoff Grade" />
                        </asp:DropDownList> 
                        <asp:DropDownList ID="rptcttype" runat ="server" Enabled="true">
                         <asp:ListItem Value="Number" Text="Number" Selected="True" />
                         <asp:ListItem Value="Percent" Text="%" />
                        </asp:DropDownList> 
                    </asp:TableCell>
                </asp:TableFooterRow>
            </asp:Table>
            <asp:Table ID="ResultTable" runat="server" CellPadding="0" CellSpacing="0" CssClass="RatingRpt" Width="97%" >
                <asp:TableRow>
                    <asp:TableCell CssClass="HeaderRow"><u>รายงานการติดตามผลการพิจารณาสินเชื่อ ณ ช่วงเวลาที่แตกต่างกัน (Approval Rate Report)</u> : </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell CssClass="TableRow">
                        <asp:Table ID="HeadTxtLoan" runat="server" CellPadding="0" CellSpacing="0" CssClass="RatingRpt">

<%--                             <asp:TableRow ID="TableRow5" runat="server">
                                <asp:TableCell Width="120px" > </asp:TableCell>
                                <asp:TableCell Width="100px"  ColumnSpan="3"  >Through-the-door Population</asp:TableCell>
                                <asp:TableCell Width="100px"  ColumnSpan="3" >Reject as % TTD in range</asp:TableCell>
				                <asp:TableCell Width="100px"  ColumnSpan="3"  >Approve as % decisioned in range </asp:TableCell>
                                <asp:TableCell Width="100px"  ColumnSpan="3" >Book as % of approved in range</asp:TableCell>


                            </asp:TableRow>
                            <asp:TableRow ID="TableRow1" runat="server">
                                <asp:TableCell Width="100px" >  </asp:TableCell>
                                <asp:TableCell Width="100px" >Past</asp:TableCell>
                                <asp:TableCell Width="100px" >Past</asp:TableCell>
                                <asp:TableCell Width="100px" >Year ago</asp:TableCell>
                                <asp:TableCell Width="100px" >Past </asp:TableCell>
                                <asp:TableCell Width="100px" >Past </asp:TableCell>
                                <asp:TableCell Width="100px" >Year ago </asp:TableCell>
                                <asp:TableCell Width="100px" >Past</asp:TableCell>
                                <asp:TableCell Width="100px" >Past</asp:TableCell>
                                <asp:TableCell Width="100px" >Year ago</asp:TableCell>
                                <asp:TableCell Width="100px" >Past </asp:TableCell>
                                <asp:TableCell Width="100px" >Past </asp:TableCell>
                                <asp:TableCell Width="100px" >Year ago </asp:TableCell>
                            </asp:TableRow>--%>

                            <asp:TableRow ID="TableRow3" runat="server">
                                 <asp:TableCell ColumnSpan="13" BorderWidth="0">                                        
                        <asp:GridView ID="gvTable" runat="server" CssClass="RatingRpt" AutoGenerateColumns="false"
             PagerSettings-Mode="NumericFirstLast" AllowPaging="True" OnPageIndexChanging="gvTable_PageChanging" PageSize="85" OnRowDataBound="gvTable_RowDataBound" OnRowCreated="gvTable_RowCreated">
                        <Columns >					
                        <asp:BoundField DataField="SCRRG"  ItemStyle-Width="100px" NullDisplayText="0"  headertext="" HeaderStyle-Font-Bold="true"/>
                        <asp:BoundField DataField="TDD3M"  ItemStyle-Width="80px" DataFormatString="{0:N2}" NullDisplayText="0.00"  headertext="Past" HeaderStyle-Font-Bold="true"/>
                        <asp:BoundField DataField="TDD12M" ItemStyle-Width="80px" DataFormatString="{0:N2}" NullDisplayText="0.00"  headertext="Past" HeaderStyle-Font-Bold="true"/>
                        <asp:BoundField DataField="TDD12YG" ItemStyle-Width="80px" DataFormatString="{0:N2}" NullDisplayText="0.00"  headertext="Year ago" HeaderStyle-Font-Bold="true"/>
                        <asp:BoundField DataField="TDD3MW" ItemStyle-Width="80px" DataFormatString="{0:N2}" NullDisplayText="0.00"  headertext="Past" HeaderStyle-Font-Bold="true"/>
                        <asp:BoundField DataField="TDD12MW" ItemStyle-Width="80px" DataFormatString="{0:N2}" NullDisplayText="0.00"  headertext="Past" HeaderStyle-Font-Bold="true"/>
                        <asp:BoundField DataField="TDD12YGW" ItemStyle-Width="80px" DataFormatString="{0:N2}" NullDisplayText="0.00"  headertext="Year ago" HeaderStyle-Font-Bold="true"/>
                        <asp:BoundField DataField="TDD3MA"  ItemStyle-Width="80px" DataFormatString="{0:N2}" NullDisplayText="0.00"  headertext="Past" HeaderStyle-Font-Bold="true"/>
                        <asp:BoundField DataField="TDD12MA" ItemStyle-Width="80px" DataFormatString="{0:N2}" NullDisplayText="0.00"  headertext="Past" HeaderStyle-Font-Bold="true"/>
                        <asp:BoundField DataField="TDD12YGA" ItemStyle-Width="80px" DataFormatString="{0:N2}" NullDisplayText="0.00"  headertext="Year ago" HeaderStyle-Font-Bold="true"/>
                        <asp:BoundField DataField="TDD3MB" ItemStyle-Width="80px" DataFormatString="{0:N2}" NullDisplayText="0.00" headertext="Past" HeaderStyle-Font-Bold="true"/>
                        <asp:BoundField DataField="TDD12MB"  ItemStyle-Width="80px" DataFormatString="{0:N2}" NullDisplayText="0.00" headertext="Past" HeaderStyle-Font-Bold="true"/>
                        <asp:BoundField DataField="TDD12YGB" ItemStyle-Width="80px" DataFormatString="{0:N2}" NullDisplayText="0.00" headertext="Year ago" HeaderStyle-Font-Bold="true"/>
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