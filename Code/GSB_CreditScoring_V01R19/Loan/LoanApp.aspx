<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/CreditScoring.Master"
    AutoEventWireup="true" CodeBehind="LoanApp.aspx.cs" Inherits="GSB.LoanApp.LoanApp" %>

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

             function Numbersonly(event) {
            event = (event) ? event : window.event;
            var charCode = (event.which) ? event.which : event.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57)) {
                return false;
            }
            return true;
        }

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
            <asp:Table runat="server" CellPadding="0" CellSpacing="0" CssClass="Search">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell ColumnSpan="2">
                        <u>เครื่องมือค้นหา</u> :
                        <asp:Label ID="ReportName1" runat="server" /></asp:TableHeaderCell></asp:TableHeaderRow>
                    <asp:TableRow>
                        <asp:TableCell CssClass="subject">* ประเภทโมเดล</asp:TableCell>
                        <asp:TableCell CssClass="detail">
                            <asp:DropDownList ID="ddlModel" runat="server" DataTextField="MODEL_NAME" DataValueField="MODEL_CD" OnSelectedIndexChanged="LoadLoan" AutoPostBack="true"/>
                        </asp:TableCell>
                     </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell CssClass="subject">* ประเภทสินเชื่อ</asp:TableCell><asp:TableCell CssClass="detail">
                        <asp:DropDownList ID="ddlLoan" runat="server" DataTextField="LOAN_NAME" DataValueField="LOAN_CD"
                            OnSelectedIndexChanged="LoadSubType" AutoPostBack="true" Enabled="false" />
                    </asp:TableCell></asp:TableRow><asp:TableRow>
                    <asp:TableCell CssClass="subject">* ประเภทสินเชื่อย่อย</asp:TableCell><asp:TableCell CssClass="detail">
                        <asp:DropDownList ID="ddlSubType" runat="server" DataTextField="STYPER_NAME" DataValueField="STYPE_CD"
                            OnSelectedIndexChanged="LoadMarket" AutoPostBack="true" Enabled="false" />
                    </asp:TableCell></asp:TableRow><asp:TableRow>
                    <asp:TableCell CssClass="subject">* Market Code </asp:TableCell><asp:TableCell CssClass="detail">
                        <asp:DropDownList ID="ddlMarketFrm" runat="server" DataTextField="MARKET_NAME" DataValueField="MARKET_CD"
                            Enabled="false" />&nbsp;ถึง&nbsp;<asp:DropDownList ID="ddlMarketTo" runat="server" DataTextField="MARKET_NAME" DataValueField="MARKET_CD"
                            Enabled="false" />
                    </asp:TableCell></asp:TableRow><asp:TableRow>
                    <asp:TableCell CssClass="subject">&nbsp;&nbsp;สาขาที่เปิดใบคำขอ</asp:TableCell><asp:TableCell CssClass="detail">
                        <asp:DropDownList ID="ddlBranch" runat="server" DataTextField="BRANCH_NAME" DataValueField="BRANCH_CD" />
                    </asp:TableCell></asp:TableRow><asp:TableRow>
                    <asp:TableCell CssClass="subject">&nbsp;&nbsp;เลขที่ใบคำขอสินเชื่อ</asp:TableCell><asp:TableCell CssClass="detail">
                        <asp:TextBox ID="txtAppID" runat="server" CssClass="text" onkeypress="return Numbersonly(event)" />
                    </asp:TableCell></asp:TableRow><asp:TableRow>
                    <asp:TableCell CssClass="subject">&nbsp;&nbsp;เลขที่บัตรประชาชน</asp:TableCell><asp:TableCell CssClass="detail">
                        <asp:TextBox ID="txtNID" runat="server" CssClass="text" onkeypress="return Numbersonly(event)" MaxLength="13" />
                    </asp:TableCell></asp:TableRow><asp:TableRow>
                    <asp:TableCell CssClass="subject">* วันที่เปิดใบคำขอสินเชื่อ</asp:TableCell><asp:TableCell CssClass="detail">
                        <asp:TextBox ID="txtOpenDate" runat="server" CssClass="Cal" />&nbsp;ถึง&nbsp;<asp:TextBox
                            ID="txtCloseDate" runat="server" CssClass="Cal" />
                    </asp:TableCell></asp:TableRow><asp:TableFooterRow>
                    <asp:TableCell ColumnSpan="2" CssClass="action">
                        <asp:Button ID="Search" runat="server" Text="แสดงข้อมูล" OnClick="Search_Click" />&nbsp;
                        <asp:Button ID="Cancel" runat="server" Text="ล้างข้อมูล" OnClick="Cancel_Click" />
                    </asp:TableCell></asp:TableFooterRow></asp:Table><asp:Table ID="ResultTable" runat="server" CellPadding="0" CellSpacing="0" CssClass="Result">
                <asp:TableRow>
                    <asp:TableCell CssClass="HeaderRow">
                        <asp:TableHeaderCell>
                            <u>แสดงผลการค้นหา</u> :
                            <asp:Label ID="ReportName2" runat="server" /></asp:TableHeaderCell>
                    </asp:TableCell></asp:TableRow><asp:TableRow>
                    <asp:TableCell CssClass="TableRow">
                        <asp:Table ID="HeadTxtLoan" runat="server" CellPadding="0" CellSpacing="0" CssClass="ExpandTable">
                            <asp:TableRow ID="RowHeader" runat="server">
                                <asp:TableCell Width="90px">เลขที่ใบคำขอ</asp:TableCell>
                                <asp:TableCell Width="92px">วันที่เปิดใบคำขอ</asp:TableCell>
                                <asp:TableCell Width="103px">สาขาที่เปิดใบคำขอ</asp:TableCell>
                                <asp:TableCell Width="91px">ประเภทสินเชื่อ</asp:TableCell>
                                <asp:TableCell Width="107px">ประเภทสินเชื่อย่อย</asp:TableCell>
                                <asp:TableCell Width="111px">Market Code</asp:TableCell>
                                <asp:TableCell Width="89px">วงเงินที่ขอกู้</asp:TableCell>
                                <asp:TableCell Width="49px">คะแนน</asp:TableCell>
                                <asp:TableCell Width="37px">เกรด</asp:TableCell>
                                <asp:TableCell Width="70px">ผลพิจารณา</asp:TableCell>
                            </asp:TableRow>
                        </asp:Table>
                        <asp:GridView ID="gvTable" runat="server" CellSpacing="0" CellPadding="0" BorderWidth="0"
                            GridLines="None" OnRowDataBound="gvTable_RowBound" PagerSettings-Mode="NumericFirstLast"
                            AllowPaging="True" OnPageIndexChanging="gvTable_PageChanging" AutoGenerateColumns="False"
                            ShowHeader="False" OnRowCommand="gvTable_RowCommand" EmptyDataText="ไม่พบข้อมูลที่ค้นหา">
                            <Columns>
                                <asp:TemplateField ItemStyle-VerticalAlign="Top" ItemStyle-Width="29px">
                                    <ItemTemplate>
                                        <div class="Expand">
                                            <asp:ImageButton ID="imgExpan" runat="server" CommandName="ExpanView" CommandArgument='<%# Container.DataItemIndex %>'
                                                ImageUrl="~/Images/Plus.png" />
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:Label ID="Key_APP_NO" runat="server" Text='<%# Eval("APP_NO") %>' Visible="false" />
                                        <asp:Label ID="Key_APP_SEQ" runat="server" Text='<%# Eval("APP_SEQ") %>' Visible="false" />
                                        <asp:Repeater ID="gvApp" runat="server" OnItemDataBound="gvApp_ItemBound">
                                            <ItemTemplate>
                                                <asp:Label ID="Sub_APP_NO" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "APP_NO")%>' Visible="false" />
                                                <asp:Label ID="Sub_APP_SEQ" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "APP_SEQ")%>' Visible="false" />
                                                <asp:Table ID="LoanTable" runat="server" CellPadding="0" CellSpacing="0" BorderWidth="0" CssClass="GridViewApp" >
                                                    <asp:TableRow>
                                                        <asp:TableCell Width="88px" HorizontalAlign="Right"><%# DataBinder.Eval(Container.DataItem, "APP_NO")%></asp:TableCell>
                                                        <asp:TableCell Width="90px" HorizontalAlign="Right"><%# DataBinder.Eval(Container.DataItem, "APP_DATE", "{0:dd/MM/yyyy}") %></asp:TableCell>
                                                        <asp:TableCell Width="102px" CssClass="small"><%# DataBinder.Eval(Container.DataItem, "BRANCH_NAME")%></asp:TableCell>
                                                        <asp:TableCell Width="89px" CssClass="small"><%# DataBinder.Eval(Container.DataItem, "LOAN_NAME")%></asp:TableCell>
                                                        <asp:TableCell Width="106px" CssClass="small"><%# DataBinder.Eval(Container.DataItem, "STYPE_NAME")%></asp:TableCell>
                                                        <asp:TableCell Width="110px" CssClass="small"><%# DataBinder.Eval(Container.DataItem, "MARKET_NAME")%></asp:TableCell>
                                                        <asp:TableCell Width="87px" HorizontalAlign="Right"><%# DataBinder.Eval(Container.DataItem, "LOAN_AMOUNT", "{0:N}")%></asp:TableCell>
                                                        <asp:TableCell Width="44px" HorizontalAlign="Center"><%# DataBinder.Eval(Container.DataItem, "SCORE")%></asp:TableCell>
                                                        <asp:TableCell Width="32px" HorizontalAlign="Center"><%# DataBinder.Eval(Container.DataItem, "GRADE")%></asp:TableCell>
                                                        <asp:TableCell Width="67px" HorizontalAlign="Center"><%# DataBinder.Eval(Container.DataItem, "LNSTATUS")%></asp:TableCell>
                                                    </asp:TableRow>
                                                </asp:Table>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                        <asp:GridView ID="gvCIF" runat="server" AutoGenerateColumns="False" Visible="False"
                                            CssClass="GridViewCIF" BorderWidth="0" CellPadding="0" CellSpacing="0" GridLines="None">
                                            <Columns>
                                                <asp:BoundField HeaderText="ทะเบียนลูกค้า" DataField="CBS_CIFNO" HeaderStyle-Width="180px" />
                                                <asp:BoundField HeaderText="เลขที่บัตรประชาชน" DataField="CBS_CITIZID" HeaderStyle-Width="180px" />
                                                <asp:BoundField HeaderText="ชื่อ-นามสกุล" DataField="CBS_CIFNAME" HeaderStyle-Width="300px" />
                                                <asp:BoundField HeaderText="คะแนน" DataField="CBS_SCORE" HeaderStyle-Width="156px"
                                                    ItemStyle-HorizontalAlign="Center" />
                                            </Columns>
                                        </asp:GridView>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </asp:TableCell></asp:TableRow></asp:Table></ContentTemplate></asp:UpdatePanel></asp:Content>