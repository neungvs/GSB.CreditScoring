<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/CreditScoring.Master"
    AutoEventWireup="true" CodeBehind="LoanExport.aspx.cs" Inherits="GSB.Loan.LoanExport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Header" runat="server">
   <link rel="stylesheet" media="screen" href="<%=Page.ResolveUrl("~/")%>Styles/jquery-ui-1.7.2.custom.css" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
   <script src="<%=Page.ResolveUrl("~/")%>Scripts/jquery-ui-1.8.10.offset.datepicker.min.js" type="text/javascript"></script>

    <script type="text/javascript">
        function dpc() {
            var d = new Date();
            var toDay = d.getDate() + '/' + (d.getMonth() + 1) + '/' + (d.getFullYear() + 543);
            $("#<%=txtOpenDate.ClientID%>").datepicker({ changeMonth: true, changeYear: true, dateFormat: 'dd/mm/yy', isBuddhist: true, defaultDate: toDay, dayNames: ['อาทิตย์', 'จันทร์', 'อังคาร', 'พุธ', 'พฤหัสบดี', 'ศุกร์', 'เสาร์'],
                dayNamesMin: ['อา.', 'จ.', 'อ.', 'พ.', 'พฤ.', 'ศ.', 'ส.'],
                monthNames: ['มกราคม', 'กุมภาพันธ์', 'มีนาคม', 'เมษายน', 'พฤษภาคม', 'มิถุนายน', 'กรกฎาคม', 'สิงหาคม', 'กันยายน', 'ตุลาคม', 'พฤศจิกายน', 'ธันวาคม'],
                monthNamesShort: ['ม.ค.', 'ก.พ.', 'มี.ค.', 'เม.ย.', 'พ.ค.', 'มิ.ย.', 'ก.ค.', 'ส.ค.', 'ก.ย.', 'ต.ค.', 'พ.ย.', 'ธ.ค.']
            });

            var d1 = new Date();
            var toDay1 = d1.getDate() + '/' + (d1.getMonth() + 1) + '/' + (d1.getFullYear() + 543);
            $("#<%=txtCloseDate.ClientID%>").datepicker({ changeMonth: true, changeYear: true, dateFormat: 'dd/mm/yy', isBuddhist: true, defaultDate: toDay1, dayNames: ['อาทิตย์', 'จันทร์', 'อังคาร', 'พุธ', 'พฤหัสบดี', 'ศุกร์', 'เสาร์'],
                dayNamesMin: ['อา.', 'จ.', 'อ.', 'พ.', 'พฤ.', 'ศ.', 'ส.'],
                monthNames: ['มกราคม', 'กุมภาพันธ์', 'มีนาคม', 'เมษายน', 'พฤษภาคม', 'มิถุนายน', 'กรกฎาคม', 'สิงหาคม', 'กันยายน', 'ตุลาคม', 'พฤศจิกายน', 'ธันวาคม'],
                monthNamesShort: ['ม.ค.', 'ก.พ.', 'มี.ค.', 'เม.ย.', 'พ.ค.', 'มิ.ย.', 'ก.ค.', 'ส.ค.', 'ก.ย.', 'ต.ค.', 'พ.ย.', 'ธ.ค.']
            });

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
    <asp:UpdatePanel ID="UpdatePanel" runat="server" UpdateMode="Always">
        <Triggers>
            <%--<asp:PostBackTrigger ControlID="btnExcel" />--%>
            <asp:AsyncPostBackTrigger ControlID="btnBKStep1" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnBKStep2" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnGOStep2" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnGOStep3" EventName="Click" />
            <%--<asp:AsyncPostBackTrigger ControlID="btnExcel" EventName="Click" />--%>
            <%--<asp:PostBackTrigger ControlID="btnExcel" />--%>
        </Triggers>
        <ContentTemplate>
            <%--STEP 1--%>
            <asp:Table ID="STEP1" runat="server" CellPadding="0" CellSpacing="0" CssClass="Search">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell ColumnSpan="2"><u>นำข้อมูลออก</u> : ขั้นตอนที่ 1 - กำหนดเงื่อนไข</asp:TableHeaderCell>
                </asp:TableHeaderRow>
                    <asp:TableRow>
                        <asp:TableCell CssClass="subject">* ประเภทโมเดล</asp:TableCell>
                        <asp:TableCell CssClass="detail">
                            <asp:DropDownList ID="ddlModel" runat="server" DataTextField="MODEL_NAME" DataValueField="MODEL_CD" OnSelectedIndexChanged="LoadLoan" AutoPostBack="true"/>
                        </asp:TableCell>
                     </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell CssClass="subject">* ประเภทสินเชื่อ</asp:TableCell>
                    <asp:TableCell CssClass="detail">
                        <asp:DropDownList ID="ddlLoan" runat="server" DataTextField="LOAN_NAME" DataValueField="LOAN_CD"
                            OnSelectedIndexChanged="LoadSubType" AutoPostBack="true" />
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell CssClass="subject">* ประเภทสินเชื่อย่อย</asp:TableCell>
                    <asp:TableCell CssClass="detail">
                        <asp:DropDownList ID="ddlSubType" runat="server" DataTextField="STYPE_NAME" DataValueField="STYPE_CD"
                            OnSelectedIndexChanged="LoadMarket" AutoPostBack="true" Enabled="false" />
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
                    <asp:TableCell CssClass="subject">&nbsp;&nbsp;ภาคตามธนาคาร</asp:TableCell>
                    <asp:TableCell CssClass="detail">
                        <asp:DropDownList ID="ddlRegion" runat="server" DataTextField="REGION_NAME" DataValueField="REGION_CD" />
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell CssClass="subject">&nbsp;&nbsp;เขตตามธนาคาร</asp:TableCell>
                    <asp:TableCell CssClass="detail">
                        <asp:DropDownList ID="ddlZone" runat="server" DataTextField="ZONE_NAME" DataValueField="ZONE_CD" />
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell CssClass="subject">&nbsp;&nbsp;สาขาที่เปิดใบคำขอ</asp:TableCell>
                    <asp:TableCell CssClass="detail">
                        <asp:DropDownList ID="ddlBranch" runat="server" DataTextField="BRANCH_NAME" DataValueField="BRANCH_CD" />
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell CssClass="subject">&nbsp;&nbsp;วัตถุประสงค์ตาม ธปท</asp:TableCell>
                    <asp:TableCell CssClass="detail">
                        <asp:DropDownList ID="ddlObjective" runat="server" DataTextField="PURPOSE_NAME" DataValueField="PURPOSE_CD" />
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell CssClass="subject">&nbsp;&nbsp;วัตถุประสงค์การกู้</asp:TableCell>
                    <asp:TableCell CssClass="detail">
                        <asp:DropDownList ID="ddlGSBLoan" runat="server" DataTextField="GPURPOSE_NAME" DataValueField="GSBPURPOSE_CD" />
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell CssClass="subject">&nbsp;&nbsp;วัตถุประสงค์การกู้ย่อย</asp:TableCell>
                    <asp:TableCell CssClass="detail">
                        <asp:DropDownList ID="ddlGSBSubLoan" runat="server" DataTextField="CONSUMPTION_NAME"
                            DataValueField="CONSUMPTION_CD" AutoPostBack="true" Enabled="false"/>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell CssClass="subject">* วันที่เปิดใบคำขอสินเชื่อ</asp:TableCell>
                    <asp:TableCell CssClass="detail">
                        <asp:TextBox ID="txtOpenDate" runat="server" CssClass="Cal" />&nbsp;ถึง&nbsp;<asp:TextBox
                            ID="txtCloseDate" runat="server" CssClass="Cal" />
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell CssClass="subject">&nbsp;&nbsp;เลขที่ใบคำขอ</asp:TableCell>
                    <asp:TableCell CssClass="detail">
                        <asp:TextBox ID="txtOpenApp" runat="server" CssClass="Cal" />&nbsp;ถึง&nbsp;<asp:TextBox
                            ID="txtCloseApp" runat="server" CssClass="Cal" />
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell CssClass="subject">&nbsp;&nbsp;วงเงินที่ขอกู้ (บาท)</asp:TableCell>
                    <asp:TableCell CssClass="detail">
                        <asp:TextBox ID="txtOpenLoan" runat="server" CssClass="Cal" />&nbsp;ถึง&nbsp;<asp:TextBox
                            ID="txtCloseLoan" runat="server" CssClass="Cal" />
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell CssClass="subject">&nbsp;&nbsp;วงเงินอนุมัติ (บาท)</asp:TableCell>
                    <asp:TableCell CssClass="detail">
                        <asp:TextBox ID="txtOpenMoney" runat="server" CssClass="Cal" />&nbsp;ถึง&nbsp;<asp:TextBox
                            ID="txtCloseMoney" runat="server" CssClass="Cal" />
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableFooterRow>
                    <asp:TableCell ColumnSpan="2" CssClass="action">
                      <asp:CheckBox ID="chkSEQ" runat="server" Checked="true" Text="SEQ ล่าสุดเท่านั้น" />
                      &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Button ID="btnGOStep2" runat="server" Text="ยืนยันเงื่อนไข >>" OnClick="btnGOStep2_Click" />
                    </asp:TableCell>
                </asp:TableFooterRow>
            </asp:Table>
            <%--STEP 2--%>
            <asp:Table ID="STEP2" runat="server" CellPadding="0" CellSpacing="0" CssClass="Search"
                Visible="false">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell ColumnSpan="4"><u>นำข้อมูลออก</u> : ขั้นตอนที่ 2 - เลือกชุดข้อมูล</asp:TableHeaderCell>
                </asp:TableHeaderRow>
                <asp:TableRow>
                    <asp:TableCell CssClass="export_box">
                        <asp:ListBox ID="MainColumn" runat="server" Height="350px" Width="100%" SelectionMode="Multiple"
                        DataTextField="DESC_TH" DataValueField="C_NAME" CssClass="txtExport" />
                    </asp:TableCell>
                    <asp:TableCell CssClass="export_opt">
                        <asp:ImageButton ID="btmMoveRight" runat="server" ImageUrl="~/Images/icons/move_right.png" ImageAlign="AbsMiddle" OnClick="btmMoveRight_Click" />
                        <div style="height:10px;"></div>
                        <asp:ImageButton ID="btmMoveAllRight" runat="server" ImageUrl="~/Images/icons/moveall_right.png" ImageAlign="AbsMiddle" OnClick="btmMoveAllRight_Click" />
                        <div style="height:70px;"></div>
                        <asp:ImageButton ID="btmMoveLeft" runat="server" ImageUrl="~/Images/icons/move_left.png" ImageAlign="AbsMiddle" OnClick="btmMoveLeft_Click" />
                        <div style="height:10px;"></div>
                        <asp:ImageButton ID="btmMoveAllLeft" runat="server" ImageUrl="~/Images/icons/moveall_left.png" ImageAlign="AbsMiddle" OnClick="btmMoveAllLeft_Click" />
                    </asp:TableCell>
                    <asp:TableCell CssClass="export_box">
                        <asp:ListBox ID="SelectedColumn" runat="server" Height="350px" Width="100%" SelectionMode="Multiple"
                        CssClass="txtExport" />
                    </asp:TableCell>
                    <asp:TableCell CssClass="export_opt">
                        <asp:ImageButton ID="btmMoveUp" runat="server" ImageUrl="~/Images/icons/move_up.png" ImageAlign="AbsMiddle" OnClick="btmMoveUp_Click" />
                        <div style="height:10px;"></div>
                        <asp:ImageButton ID="btmMoveDown" runat="server" ImageUrl="~/Images/icons/move_down.png" ImageAlign="AbsMiddle" OnClick="btmMoveDown_Click" />
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableFooterRow>
                    <asp:TableCell ColumnSpan="4" CssClass="action">
                        <asp:Button ID="btnBKStep1" runat="server" Text="<< กำหนดเงื่อนไข" OnClick="btnBKStep1_Click" />
                        <asp:Image ID="Image1" runat="server" height="0" Width="150"/>
                        <asp:Button ID="btnGOStep3" runat="server" Text="ยืนยันชุดข้อมูล >>" OnClick="btnGOStep3_Click" />
                    </asp:TableCell>
                </asp:TableFooterRow>
            </asp:Table>
            <%--STEP 3--%>
            <asp:Table ID="STEP3" runat="server" CellPadding="0" CellSpacing="0" CssClass="Search"
                Visible="false">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell ColumnSpan="3"><u>นำข้อมูลออก</u> : ขั้นตอนที่ 3 - ดาวโหลดข้อมูล</asp:TableHeaderCell>
                </asp:TableHeaderRow>
                <asp:TableFooterRow>
                    <asp:TableCell ColumnSpan="3" CssClass="action">
                        <asp:Button ID="btnBKStep2" runat="server" Text="<< เลือกชุดข้อมูล" OnClick="btnBKStep2_Click" CssClass="sent_left" />
                        <asp:Image ID="Image2" runat="server" height="0" Width="150"/>
                        <asp:Button ID="btnExcel" runat="server" Text="ดาวโหลดไฟล์" OnClick="btnExcel_Click" CssClass="sent_right" />
                    </asp:TableCell>
                </asp:TableFooterRow>
            </asp:Table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
