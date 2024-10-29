<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/CreditScoring.Master" AutoEventWireup="true" CodeBehind="VintageAnalysisReport.aspx.cs" Inherits="GSB.Report.BackEnd.VintageAnalysisReport" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Header" runat="server">
    <link rel="stylesheet" media="screen" href="<%=Page.ResolveUrl("~/")%>Scripts/datepicker/css/datepicker.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
    <script src="<%=Page.ResolveUrl("~/")%>Scripts/datepicker/js/datepicker.js" type="text/javascript"></script>
     <script type="text/javascript">
         function callDatePicker() {

         }

         $(document).ready(function () { callDatePicker(); });
         prm.add_endRequest(function () { $(document).ready(function () { callDatePicker(); }); });
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
                    <asp:TableHeaderCell ColumnSpan="2"><u>เครื่องมือค้นหา</u> : รายงานเปรียบเทียบการผิดนัดชำระหนี้ของลูกหนี้ที่ได้เปิดบัญชีมาเป็นระยะเวลาเท่ากัน (Vintage Analysis Report)</asp:TableHeaderCell>
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
                        <asp:DropDownList ID="ddlMarketFrm" runat="server" Enabled="false" DataTextField="MARKET_NAME" DataValueField="MARKET_CD" />
                        &nbsp;ถึง&nbsp;
                        <asp:DropDownList ID="ddlMarketTo" runat="server" Enabled="false" DataTextField="MARKET_NAME" DataValueField="MARKET_CD" />
                    </asp:TableCell>
                </asp:TableRow> 
                <asp:TableRow>
                    <asp:TableCell CssClass="subject">* ช่วงเดือนที่เซ็นสัญญา</asp:TableCell>
                    <asp:TableCell CssClass="detail">
                        <asp:DropDownList ID="txtOpenAcDate" runat ="server" Enabled="true" >
                         <asp:ListItem Value="1" Text="ม.ค. - มี.ค." Selected="True" />
                         <asp:ListItem Value="2" Text="เม.ย. - มิ.ย." />
                         <asp:ListItem Value="3" Text="ก.ค. - ก.ย." />
                         <asp:ListItem Value="4" Text="ต.ค. - ธ.ค." />
                        </asp:DropDownList>
                         &nbsp ปี &nbsp
                        <asp:DropDownList ID="txtEndAcDate" runat ="server" Enabled="true" />
                        
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
                    <asp:TableCell CssClass="HeaderRow"><u>รายงานเปรียบเทียบการผิดนัดชำระหนี้ของลูกหนี้ที่ได้เปิดบัญชีมาเป็นระยะเวลาเท่ากัน (Vintage Analysis Report)</u> : </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell CssClass="TableRow">
                        <asp:Table ID="HeadTxtLoan" runat="server" CellPadding="0" CellSpacing="0" CssClass="RatingRpt">
                            <asp:TableRow ID="TableRow3" runat="server">
                                 <asp:TableCell ColumnSpan="8" BorderWidth="0">                                        
                        <asp:GridView ID="gvTable" runat="server" CssClass="RatingRpt" ShowHeader="true" AutoGenerateColumns="false"
                        PagerSettings-Mode="NumericFirstLast" AllowPaging="True" OnPageIndexChanging="gvTable_PageChanging" OnRowCreated="gvTable_RowCreated" PageSize="20">
                        <Columns >					
                        <asp:BoundField DataField="prdmnt"  ItemStyle-Width="110px"   NullDisplayText="0"/>
                        <asp:BoundField DataField="bm1"  ItemStyle-Width="90px" DataFormatString="{0:N2}"  NullDisplayText="0"/>
                        <asp:BoundField DataField="bm2"  ItemStyle-Width="90px"  DataFormatString="{0:N2}" NullDisplayText="0"/>
                        <asp:BoundField DataField="bm3"  ItemStyle-Width="90px" DataFormatString="{0:N2}"  NullDisplayText="0"/>
                        <asp:BoundField DataField="bm4"  ItemStyle-Width="90px" DataFormatString="{0:N2}"  NullDisplayText="0"/>
                        <asp:BoundField DataField="bm5"  ItemStyle-Width="90px" DataFormatString="{0:N2}"  NullDisplayText="0"/>
                        <asp:BoundField DataField="bm6"  ItemStyle-Width="90px" DataFormatString="{0:N2}"  NullDisplayText="0"/>
                        <asp:BoundField DataField="bm7"  ItemStyle-Width="90px" DataFormatString="{0:N2}"  NullDisplayText="0"/>
                        <asp:BoundField DataField="bm8"  ItemStyle-Width="90px" DataFormatString="{0:N2}"  NullDisplayText="0"/>
                        <asp:BoundField DataField="bm9"  ItemStyle-Width="90px" DataFormatString="{0:N2}"  NullDisplayText="0"/>
                        <asp:BoundField DataField="bm0"  ItemStyle-Width="90px" DataFormatString="{0:N2}"  NullDisplayText="0"/>
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
