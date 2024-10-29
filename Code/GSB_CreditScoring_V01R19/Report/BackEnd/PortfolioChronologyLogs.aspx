<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/CreditScoring.Master" AutoEventWireup="true" CodeBehind="PortfolioChronologyLogs.aspx.cs" Inherits="GSB.Report.BackEnd.PortfolioChronologyLogs" %>
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
            $("#<%=txtCloseDate.ClientID%>").datepicker({ changeMonth: true, changeYear: true, dateFormat: 'dd/mm/yy', isBuddhist: true, defaultDate: toDay, dayNames: ['อาทิตย์', 'จันทร์', 'อังคาร', 'พุธ', 'พฤหัสบดี', 'ศุกร์', 'เสาร์'],
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
    <asp:UpdatePanel ID="UpdatePanel" runat="server">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="Search" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="Cancel" EventName="Click" />
           <asp:AsyncPostBackTrigger ControlID="Add" EventName="Click" />
           <asp:AsyncPostBackTrigger ControlID="BtnSave" EventName="Click" />
           <asp:AsyncPostBackTrigger ControlID="BtnCanc" EventName="Click" />
        </Triggers>
        <ContentTemplate>
            <asp:Table ID="Table1" runat="server" CellPadding="0" CellSpacing="0" CssClass="Search">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell ColumnSpan="2"><u>เครื่องมือค้นหา</u> : รายงานเหตุการณ์ (Portfolio Chronology Logs)</asp:TableHeaderCell>
                </asp:TableHeaderRow>
                <asp:TableRow>
                     <asp:TableCell  ColumnSpan="2" CssClass="action">
                        <asp:Button ID="Add" runat="server" Text="เพิ่มข้อมูล" OnClick="Add_Click" />                    
                     </asp:TableCell>
                </asp:TableRow>                
                <asp:TableRow>
                    <asp:TableCell CssClass="subject">* ประเภทโมเดล</asp:TableCell>
                      <asp:TableCell CssClass="detail">
<asp:DropDownList ID="ddlModel" runat="server" DataTextField="MODEL_NAME" DataValueField="MODEL_CD"  OnSelectedIndexChanged="ShowTextBox"  AutoPostBack="true"/>
                                         </asp:TableCell>  
                  <%--  <asp:TableCell CssClass="detail">
                     <asp:DropDownList ID="ddlModel" runat ="server" Enabled="true">
                         <asp:ListItem Value="1" Text="001 - GSB_MicroFinance" Selected="True" />
                         <asp:ListItem Value="2" Text="002 - GSB_Mortgage" />
                         <asp:ListItem Value="3" Text="003 - Zaithong" />                         
                         <asp:ListItem Value="4" Text="004 - Agree" />
                        </asp:DropDownList> 
                    </asp:TableCell> --%>
                </asp:TableRow>
<%--                <asp:TableRow>
                <asp:TableCell CssClass="subject">Model Version</asp:TableCell>
                                    <asp:TableCell CssClass="detail">
                        <asp:DropDownList ID="ModelVersion" runat ="server" Enabled="true">
                         <asp:ListItem Value="0" Text="เลือกทั้งหมด" Selected="True" />                         
                         <asp:ListItem Value="1" Text="1.0"  />
                         <asp:ListItem Value="2" Text="2.0" />
                         <asp:ListItem Value="3" Text="3.0" />
                        </asp:DropDownList> 
                    </asp:TableCell>
                </asp:TableRow>--%>
                                <asp:TableRow>
                    <asp:TableCell CssClass="subject">* ระหว่างวันที่</asp:TableCell>
                    <asp:TableCell CssClass="detail">
                        <asp:TextBox ID="txtOpenDate" runat="server" CssClass="Cal" />&nbsp;ถึง&nbsp;<asp:TextBox
                            ID="txtCloseDate" runat="server" CssClass="Cal" />
                    </asp:TableCell></asp:TableRow><asp:TableFooterRow>
                    <asp:TableCell ColumnSpan="2" CssClass="action">
                        <asp:Button ID="Search" runat="server" Text="แสดงข้อมูล" OnClick="Search_Click" />&nbsp;
                        <asp:Button ID="Cancel" runat="server" Text="ล้างข้อมูล" OnClick="Cancel_Click" />
                        <asp:Button ID="Button1" runat="server" Text="พิมพ์รายงาน" OnClick="PrintReport_Click" />&nbsp;
                    </asp:TableCell></asp:TableFooterRow></asp:Table>
                    <asp:Table ID="ResultTable" runat="server" CellPadding="0" CellSpacing="0" CssClass="Result" >
                <asp:TableRow>
                    <asp:TableCell CssClass="HeaderRow"><u>รายงานเหตุการณ์ (Portfolio Chronology Logs)</u> : </asp:TableCell></asp:TableRow><asp:TableRow>
                    <asp:TableCell CssClass="TableRow">
                        <asp:Table ID="HeadTxtLoan" runat="server" CellPadding="0" CellSpacing="0" CssClass="RatingRpt">
                             <asp:TableRow ID="TableRow2" runat="server">
                                <asp:TableCell Width="332px" >Date</asp:TableCell>
                                <asp:TableCell Width="308px" >Loan Type</asp:TableCell>
                                <asp:TableCell Width="300px" >Loan SubType</asp:TableCell>                               
                                <asp:TableCell Width="300px" >Model</asp:TableCell>
                                <asp:TableCell Width="290px" >Message</asp:TableCell>
                            </asp:TableRow>
                          <asp:TableRow ID="TableRow3" runat="server">
                          <asp:TableCell ColumnSpan="5" BorderWidth="0">  
                                                
                        <asp:GridView ID="gvTable" runat="server" CssClass="RatingRpt" ShowHeader="False" ShowFooter="true" AutoGenerateColumns="false"
                        PagerSettings-Mode="NumericFirstLast" OnRowCommand="gvTable_RowCommand" AllowPaging="True" 
                         OnPageIndexChanging="gvTable_PageChanging" PageSize="15" >
                        <Columns >					
                 <%--       <asp:BoundField DataField="OID" Visible="false" />--%>
                        <asp:BoundField DataField="DTM" ItemStyle-Width="300px" />
                        <asp:BoundField DataField="LTYPE" ItemStyle-Width="300px" />
                        <asp:BoundField DataField="LSTYPE" ItemStyle-Width="300px" />
                        <asp:BoundField DataField="MODEL" ItemStyle-Width="300px" />
                      <%--  <asp:BoundField DataField="MES" ItemStyle-Width="100px"  />--%>
                        <asp:TemplateField ItemStyle-Width="300px" >
                          <ItemTemplate>
                                <div class="Expand">
                                    <asp:ImageButton ID="imgExpan"  runat="server" CommandName="ExpanView" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "OID")%>'
                                                ImageUrl="~/Images/Plus.png" />
                                    </div>
                             </ItemTemplate>
                        </asp:TemplateField>
                        </Columns>
                        </asp:GridView> 
                        </asp:TableCell>
                        </asp:TableRow>    
                         </asp:Table>
 </asp:TableCell></asp:TableRow></asp:Table>
 <asp:Table ID="Table2" runat="server" Width="700px" CellPadding="0" CellSpacing="0" CssClass="Search" Visible="false" >
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell  ColumnSpan="2"><u>รายงานบันทึกเหตุการณ์</u> : Portfolio Chronology Logs</asp:TableHeaderCell></asp:TableHeaderRow><asp:TableRow>
                    <asp:TableCell CssClass="subject">As of Date</asp:TableCell><asp:TableCell CssClass="detail">
                        <asp:TextBox ID="Asofdt" runat="server" Enabled="false"  />
                    </asp:TableCell></asp:TableRow><asp:TableRow>
                    <asp:TableCell  CssClass="subject">ประเภทโมเดล</asp:TableCell>
                   <asp:TableCell CssClass="detail">
                        <asp:DropDownList ID="ddlModel2" runat="server" DataTextField="MODEL_NAME" DataValueField="MODEL_CD"  OnSelectedIndexChanged="LoadLoan"  AutoPostBack="true"/>
                    </asp:TableCell> 
                   <%--    <asp:TableCell CssClass="detail">
                    <asp:DropDownList ID="ddlModel2" runat ="server" Enabled="true">
                         <asp:ListItem Value="1" Text="001 - GSB_MicroFinance" Selected="True" />
                         <asp:ListItem Value="2" Text="002 - GSB_Mortgage" />
                         <asp:ListItem Value="3" Text="003 - Zaithong" />                         
                         <asp:ListItem Value="4" Text="004 - Agree" />
                        </asp:DropDownList> 
                     </asp:TableCell> --%>
                    </asp:TableRow>
<%--                    <asp:TableRow>
                    <asp:TableCell CssClass="subject" ID="tbcModelVersion">Model Version</asp:TableCell>
                    <asp:TableCell CssClass="detail" ID="tbcModelV" Visible="true">
                        <asp:DropDownList ID="ModelVersion1" runat ="server" Enabled="true">
                         <asp:ListItem Value="1" Text="1.0" Selected="True" />
                         <asp:ListItem Value="2" Text="2.0" />
                         <asp:ListItem Value="3" Text="3.0" />
                        </asp:DropDownList> 
                    </asp:TableCell>
                    </asp:TableRow>--%>
                    <asp:TableRow>
                    <asp:TableCell  CssClass="subject">ประเภทสินเชื่อ</asp:TableCell><asp:TableCell CssClass="detail">
                        <asp:DropDownList ID="ddlLoan" runat="server" DataTextField="LOAN_NAME" DataValueField="LOAN_CD"
                            OnSelectedIndexChanged="LoadSubType" AutoPostBack="true"/>
                    </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                    <asp:TableCell  CssClass="subject" >ประเภทสินเชื่อย่อย</asp:TableCell>
                    <asp:TableCell CssClass="detail">
                        <asp:DropDownList ID="ddlSubType" runat="server" DataTextField="STYPER_NAME" DataValueField="STYPE_CD"
                            OnSelectedIndexChanged="LoadMarket" AutoPostBack="true" Enabled="false" />
                    </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                    <asp:TableCell  CssClass="subject">Market Code</asp:TableCell><asp:TableCell CssClass="detail">
                        <asp:DropDownList ID="ddlMarketFrm" runat="server" Enabled="false" DataTextField="MARKET_NAME"
                            DataValueField="MARKET_CD" />
                    </asp:TableCell></asp:TableRow><asp:TableRow>
                    <asp:TableCell  CssClass="subject" ID="tbcEvent">Event</asp:TableCell><asp:TableCell CssClass="detail" ID="tbcSQLBX" Visible="true">
                        <asp:TextBox ID="txtSQLBX" Width="500" Height="250" runat="server" TextMode="MultiLine" Enable="true"/>
                    </asp:TableCell></asp:TableRow><asp:TableFooterRow>
                    <asp:TableCell ColumnSpan="2" CssClass="action">
                        <asp:Button ID="BtnSave" runat="server" Text="Save" OnClick="Save_Click" />&nbsp;
                        <asp:Button ID="BtnCanc" runat="server" Text="Cancel" OnClick="SCancel_Click" />
                    </asp:TableCell></asp:TableFooterRow></asp:Table><asp:Table ID="Table3" runat="server" CellPadding="0" CellSpacing="0" CssClass="Search" Visible="false" >
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell ColumnSpan="2"><u>รายงานบันทึกเหตุการณ์</u> : Portfolio Chronology Logs</asp:TableHeaderCell></asp:TableHeaderRow><asp:TableRow>
                   <%-- <asp:TableCell CssClass="subject">Asof Date:</asp:TableCell>--%>
                    <asp:TableCell CssClass="detail">
                                        <asp:Repeater ID="gvApp" runat="server" >
                                            <ItemTemplate>
                                                <%--<asp:Label ID="Sub_APP_NO" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "APP_NO")%>' Visible="false" />
                                                <asp:Label ID="Sub_APP_SEQ" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "APP_SEQ")%>' Visible="false" />--%>
                                                <asp:Table ID="LoanTable" runat="server" CellPadding="0" CellSpacing="0" BorderWidth="0" CssClass="GridViewApp" >
                                                    <asp:TableRow>
                                                        <asp:TableCell  CssClass="subject">As of Date</asp:TableCell>
                                                        <asp:TableCell  Width="300px" HorizontalAlign="LEFT"><%# DataBinder.Eval(Container.DataItem, "DTM")%></asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell  CssClass="subject">ประเภทโมเดล</asp:TableCell>
                                                        <asp:TableCell  Width="300px" HorizontalAlign="LEFT"><%# DataBinder.Eval(Container.DataItem, "MODEL_NAME")%></asp:TableCell>
                                                    </asp:TableRow>
<%--                                                   <asp:TableRow>
                                                        <asp:TableCell  CssClass="subject">Model Version</asp:TableCell>
                                                        <asp:TableCell  Width="300px" HorizontalAlign="LEFT"><%# DataBinder.Eval(Container.DataItem, "MODELVER_CD")%></asp:TableCell>
                                                    </asp:TableRow>--%>
                                                        <asp:TableRow>
                                                        <asp:TableCell  CssClass="subject">ประเภทสินเชื่อ</asp:TableCell>
                                                        <asp:TableCell  Width="300px" HorizontalAlign="LEFT"><%# DataBinder.Eval(Container.DataItem, "LOAN_NAME")%></asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell  CssClass="subject">ประเภทสินเชื่อย่อย</asp:TableCell>
                                                        <asp:TableCell Width="300px" HorizontalAlign="LEFT"><%# DataBinder.Eval(Container.DataItem, "STYPER_NAME")%></asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell  CssClass="subject">Market Code</asp:TableCell>
                                                        <asp:TableCell Width="300px" HorizontalAlign="LEFT"><%# DataBinder.Eval(Container.DataItem, "MARKET_NAME")%></asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell  CssClass="subject">Event</asp:TableCell>
                                                        <asp:TableCell Width="300px" HorizontalAlign="LEFT"><%# DataBinder.Eval(Container.DataItem, "MESSAGE")%></asp:TableCell>
                                                    </asp:TableRow>
                                                </asp:Table>
                                            </ItemTemplate>
                                        </asp:Repeater>
    <%--                    <asp:TextBox ID="TextBox1" runat="server" Enabled="false"  />
                    </asp:TableCell>
                </asp:TableRow>                
                <asp:TableRow>
                    <asp:TableCell CssClass="subject">ประเภทโมเดล</asp:TableCell>
                    <asp:TableCell CssClass="detail">
                       <asp:TextBox ID="TextBox3" runat="server" Enabled="false"  />
                    </asp:TableCell>
                </asp:TableRow>
                 <asp:TableRow>
                    <asp:TableCell CssClass="subject">ประเภทสินเชื่อ</asp:TableCell>
                    <asp:TableCell CssClass="detail">
                      <asp:TextBox ID="TextBox4" runat="server" Enabled="false"  />
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell CssClass="subject">ประเภทสินเชื่อย่อย</asp:TableCell>
                    <asp:TableCell CssClass="detail">
                       <asp:TextBox ID="TextBox5" runat="server" Enabled="false"  />
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell CssClass="subject">Market Code</asp:TableCell>
                    <asp:TableCell CssClass="detail">
                       <asp:TextBox ID="TextBox6" runat="server" Enabled="false"  />
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell CssClass="subject">Event:</asp:TableCell>
                    <asp:TableCell CssClass="detail">
                        <asp:TextBox ID="TextBox2" Width="800" Height="250" runat="server" TextMode="MultiLine" AutoPostBack="True" />--%>
                    </asp:TableCell></asp:TableRow><asp:TableFooterRow>
                    <asp:TableCell ColumnSpan="2" CssClass="action">
                        <asp:Button ID="BtnDetBck" runat="server" Text="Back" OnClick="Back_Click" />
                    </asp:TableCell></asp:TableFooterRow></asp:Table></ContentTemplate></asp:UpdatePanel></asp:Content>