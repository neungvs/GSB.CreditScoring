<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/CreditScoring.Master"
    AutoEventWireup="true" CodeBehind="PerformanceTesting.aspx.cs" Inherits="GSB.Report.FrontEnd.PerformanceTesting" %>

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
            });
            $("#<%=txtCloseDate.ClientID%>").datepicker({ changeMonth: true, changeYear: true, dateFormat: 'dd/mm/yy', isBuddhist: true, defaultDate: toDay, dayNames: ['อาทิตย์', 'จันทร์', 'อังคาร', 'พุธ', 'พฤหัสบดี', 'ศุกร์', 'เสาร์'],
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
            <asp:PostBackTrigger ControlID="btnExcel" />
        </Triggers>
        <ContentTemplate>
            <asp:Table runat="server" Width="950px" CellPadding="0" CellSpacing="0" CssClass="Search">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell ColumnSpan="2"><u>เครื่องมือค้นหา</u> : รายงานการทดสอบค่า Performance ของแบบจำลองฯ</asp:TableHeaderCell>
                </asp:TableHeaderRow>
<asp:TableRow>
                    <asp:TableCell CssClass="subject">* ประเภทโมเดล</asp:TableCell>
                    <asp:TableCell CssClass="detail">
<asp:DropDownList ID="ddlModel" runat="server" DataTextField="MODEL_NAME" DataValueField="MODEL_CD" OnSelectedIndexChanged="LoadLoan" AutoPostBack="true"/>
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
                        <asp:DropDownList ID="ddlOpenDateMonth" runat="server" />&nbsp;<asp:DropDownList ID="ddlOpenDateYear" runat="server" />&nbsp;ถึง&nbsp;
                        <asp:DropDownList ID="ddlOpenDateMonthend" runat="server" />&nbsp;<asp:DropDownList ID="ddlOpenDateYearend" runat="server" />
<%--                        <asp:TextBox ID="txtOpenDate" runat="server" CssClass="Cal" />&nbsp;ถึง&nbsp;<asp:TextBox
                            ID="txtCloseDate" runat="server" CssClass="Cal" />--%>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableFooterRow>
                    <asp:TableCell ColumnSpan="2" CssClass="action">
                        <asp:Button ID="Search" runat="server" Text="แสดงข้อมูล" OnClick="Search_Click" />&nbsp;
                        <asp:Button ID="Cancel" runat="server" Text="ล้างข้อมูล" OnClick="Cancel_Click" />
                        <asp:Button ID="Button1" runat="server" Text="พิมพ์รายงาน" OnClick="PrintReport_Click" />&nbsp;
                        <asp:Button ID="btnExcel" runat="server" Text="ดาวน์โหลดไฟล์" OnClick="btnExcel_Click" CssClass="sent_right" />
                    </asp:TableCell>
                </asp:TableFooterRow>
            </asp:Table>
            <asp:Table ID="ResultTable" runat="server" Width="950px" CellPadding="0" CellSpacing="0" CssClass="Result" >
                <asp:TableRow>
                    <asp:TableCell CssClass="HeaderRow"><u>รายงานการทดสอบค่า Performance ของแบบจำลองฯ</u> : </asp:TableCell></asp:TableRow>
                    <asp:TableRow>
                    <asp:TableCell CssClass="TableRow">
                        <asp:Table ID="HeadTxtLoan" runat="server" CellPadding="0" CellSpacing="0" CssClass="RatingRpt">
<%--                             <asp:TableRow ID="TableRow2" runat="server">
                                <asp:TableCell Width="140px" >Score Band</asp:TableCell>
                                <asp:TableCell Width="140px" >Default Count</asp:TableCell>
                                <asp:TableCell Width="140px" >Non-Default Count</asp:TableCell>
                                <asp:TableCell Width="140px" >Default Capture</asp:TableCell>
                                <asp:TableCell Width="140px" >Non-Default Capture</asp:TableCell>
                                <asp:TableCell Width="140px" >Cum. Cap. Default</asp:TableCell>
                                <asp:TableCell Width="140px" >Cum. Cap. Non-Default</asp:TableCell>
                                <asp:TableCell Width="140px" >KS</asp:TableCell>
                              
                            </asp:TableRow>--%>
                          <asp:TableRow ID="TableRow3" runat="server">
                                    <asp:TableCell ColumnSpan="11" BorderWidth="0">                                        
                        <asp:GridView ID="gvTable" runat="server" CssClass="RatingRpt" AutoGenerateColumns="false"
                        PagerSettings-Mode="NumericFirstLast" AllowPaging="True" OnPageIndexChanging="gvTable_PageChanging" PageSize="15" ShowFooter="True" OnRowDataBound="gvTable_RowDataBound">
                        <Columns >					
                        <asp:BoundField DataField="ScoreBand" ItemStyle-Width="140px" DataFormatString="{0:N2}"  headertext="Cutoff Score" HeaderStyle-Font-Bold="true"  />
                        <asp:BoundField DataField="DefaultCount" ItemStyle-Width="140px" DataFormatString="{0:N0}"  headertext="Default Count" HeaderStyle-Font-Bold="true"   />
                        <asp:BoundField DataField="NonDefaultCount" ItemStyle-Width="140px" DataFormatString="{0:N0}"  headertext="Non-Default Count" HeaderStyle-Font-Bold="true"  />
                        <asp:BoundField DataField="DefaultCapture" ItemStyle-Width="140px" DataFormatString="{0:N2}"  headertext="Default Capture" HeaderStyle-Font-Bold="true"   />
                        <asp:BoundField DataField="NonDefaultCapture" ItemStyle-Width="140px" DataFormatString="{0:N2}"  headertext="Non-Default Capture" HeaderStyle-Font-Bold="true" />
                        <asp:BoundField DataField="CumCapDefault" ItemStyle-Width="140px" DataFormatString="{0:N2}"  headertext="Cum Cap.Default" HeaderStyle-Font-Bold="true"  />
                        <asp:BoundField DataField="CumCapNonDefault" ItemStyle-Width="140px" DataFormatString="{0:N2}"  headertext="Cum Cap.Non-Default" HeaderStyle-Font-Bold="true"  />
                        <asp:BoundField DataField="KS" ItemStyle-Width="140px" DataFormatString="{0:N2}"  headertext="KS" HeaderStyle-Font-Bold="true"  />
                        
                        </Columns>
                        </asp:GridView> 
                        </asp:TableCell>
                        </asp:TableRow>    
                         </asp:Table>
                            </asp:TableCell>
                            </asp:TableRow>
                <asp:TableFooterRow HorizontalAlign="Center">
                <%--<asp:TableCell>Population Stability Index 
                <asp:Label runat="server" ID="lblSumIndex" DataFormatString="{0:N2}" Text="Label">
                </asp:Label></asp:TableCell>--%>
                </asp:TableFooterRow>
                         </asp:Table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
