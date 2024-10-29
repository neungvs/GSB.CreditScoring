<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/CreditScoring.Master" AutoEventWireup="true" CodeBehind="RatingReport_11.aspx.cs" Inherits="GSB.Report.Rating.RatingReport_11" %>
<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Header" runat="server">
    <link rel="stylesheet" media="screen" href="<%=Page.ResolveUrl("~/")%>Scripts/datepicker/css/datepicker.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">

    <asp:UpdateProgress ID="UpdateProgress" runat="server">
        <ProgressTemplate>
            <div id="preloader">
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdatePanel ID="UpdatePanel" runat="server">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="Search" EventName="Click" />
            <%--<asp:AsyncPostBackTrigger ControlID="Cancel" EventName="Click" />--%>
        </Triggers>
        <ContentTemplate>
            <asp:Table ID="Table1" runat="server" CellPadding="0" CellSpacing="0" CssClass="Search">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell ColumnSpan="2"><u>เครื่องมือค้นหา</u> : รายงานจำแนกสัดส่วนการใช้แบบจำลองประเภทต่างๆตามหน่วยงานของธนาคาร </asp:TableHeaderCell>
                </asp:TableHeaderRow>
                <asp:TableRow>
                    <asp:TableCell CssClass="subject">ข้อมูลการใช้งาน ณ เดือน</asp:TableCell>
                    <asp:TableCell CssClass="detail">
                        <asp:DropDownList ID="ddlSubType" runat="server" DataTextField="STYPER_NAME" DataValueField="STYPER_NAME" />
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableFooterRow>
                    <asp:TableCell ColumnSpan="2" CssClass="action">
                        <asp:Button ID="Search" runat="server" Text="แสดงข้อมูล" OnClick="Search_Click" />&nbsp;
<%--                        <asp:Button ID="Cancel" runat="server" Text="ล้างข้อมูล" OnClick="Cancel_Click" />--%>
                    </asp:TableCell></asp:TableFooterRow></asp:Table>
                    <asp:Table ID="ResultTable" runat="server" CellPadding="0" CellSpacing="0" CssClass="Result">
                <asp:TableRow>
                    <asp:TableCell CssClass="HeaderRow"><u>แสดงผลการค้นหา</u> : </asp:TableCell></asp:TableRow>
                    <asp:TableRow>
                    <asp:TableCell CssClass="TableRow" Width="950px" >
                    <div style="overflow:scroll;table-layout:fixed" align="center">
                        <asp:Table ID="HeadTxtLoan" runat="server" CellPadding="0" CellSpacing="0" Width="950px" >
                              <asp:TableRow ID="TableRow1" runat="server">
                                <asp:TableCell Width="75px" RowSpan="2" BorderColor="Black" BorderWidth="1" Font-Bold="true" HorizontalAlign="Center" >ประเภทแบบจำลอง</asp:TableCell>
                                <asp:TableCell Width="75px" RowSpan="2" BorderColor="Black" BorderWidth="1" Font-Bold="true" HorizontalAlign="Center" >ประเภทแบบจำลองย่อย</asp:TableCell>
                                <asp:TableCell Width="100px" ColumnSpan="2" BorderColor="Black" BorderWidth="1" Font-Bold="true" HorizontalAlign="Center" >สินเชื่อธุรกิจ SMEs 1</asp:TableCell>
                                <asp:TableCell Width="100px" ColumnSpan="2" BorderColor="Black" BorderWidth="1" Font-Bold="true" HorizontalAlign="Center" >สินเชื่อธุรกิจ SMEs 2</asp:TableCell>
                                <asp:TableCell Width="100px" ColumnSpan="2" BorderColor="Black" BorderWidth="1" Font-Bold="true" HorizontalAlign="Center" >สินเชื่อธุรกิจ SMEs 3</asp:TableCell>
                                <asp:TableCell Width="100px" ColumnSpan="2" BorderColor="Black" BorderWidth="1" Font-Bold="true" HorizontalAlign="Center" >สินเชื่อธุรกิจ SMEs 4</asp:TableCell>
                                <asp:TableCell Width="100px" ColumnSpan="2" BorderColor="Black" BorderWidth="1" Font-Bold="true" HorizontalAlign="Center" >สินเชื่อธุรกิจขนาดใหญ่</asp:TableCell>
                                <asp:TableCell Width="100px" ColumnSpan="2" BorderColor="Black" BorderWidth="1" Font-Bold="true" HorizontalAlign="Center" >สินเชื่อภาครัฐและสถาบัน 1</asp:TableCell>
                                <asp:TableCell Width="100px" ColumnSpan="2" BorderColor="Black" BorderWidth="1" Font-Bold="true" HorizontalAlign="Center" >สินเชื่อภาครัฐและสถาบัน 2</asp:TableCell>
                                <asp:TableCell Width="100px" ColumnSpan="2" BorderColor="Black" BorderWidth="1" Font-Bold="true" HorizontalAlign="Center" >สินเชื่อลิซซิ่งและแฟคตอริ่ง</asp:TableCell>
                            </asp:TableRow>                              
                            <asp:TableRow ID="TableRow2" runat="server" Width="950px">
                                <asp:TableCell Width="50px" BorderColor="Black" BorderWidth="1" Font-Bold="true" HorizontalAlign="Center" >ราย</asp:TableCell>
                                <asp:TableCell Width="50px" BorderColor="Black" BorderWidth="1" Font-Bold="true" HorizontalAlign="Center" >%</asp:TableCell>
                                <asp:TableCell Width="50px" BorderColor="Black" BorderWidth="1" Font-Bold="true" HorizontalAlign="Center" >ราย</asp:TableCell>
                                <asp:TableCell Width="50px" BorderColor="Black" BorderWidth="1" Font-Bold="true" HorizontalAlign="Center" >%</asp:TableCell>
                                <asp:TableCell Width="50px" BorderColor="Black" BorderWidth="1" Font-Bold="true" HorizontalAlign="Center" >ราย</asp:TableCell>
                                <asp:TableCell Width="50px" BorderColor="Black" BorderWidth="1" Font-Bold="true" HorizontalAlign="Center" >%</asp:TableCell>
                                <asp:TableCell Width="50px" BorderColor="Black" BorderWidth="1" Font-Bold="true" HorizontalAlign="Center" >ราย</asp:TableCell>
                                <asp:TableCell Width="50px" BorderColor="Black" BorderWidth="1" Font-Bold="true" HorizontalAlign="Center" >%</asp:TableCell>
                                <asp:TableCell Width="50px" BorderColor="Black" BorderWidth="1" Font-Bold="true" HorizontalAlign="Center" >ราย</asp:TableCell>
                                <asp:TableCell Width="50px" BorderColor="Black" BorderWidth="1" Font-Bold="true" HorizontalAlign="Center" >%</asp:TableCell>
                                <asp:TableCell Width="50px" BorderColor="Black" BorderWidth="1" Font-Bold="true" HorizontalAlign="Center" >ราย</asp:TableCell>
                                <asp:TableCell Width="50px" BorderColor="Black" BorderWidth="1" Font-Bold="true" HorizontalAlign="Center" >%</asp:TableCell>
                                <asp:TableCell Width="50px" BorderColor="Black" BorderWidth="1" Font-Bold="true" HorizontalAlign="Center" >ราย</asp:TableCell>
                                <asp:TableCell Width="50px" BorderColor="Black" BorderWidth="1" Font-Bold="true" HorizontalAlign="Center" >%</asp:TableCell>
                                <asp:TableCell Width="50px" BorderColor="Black" BorderWidth="1" Font-Bold="true" HorizontalAlign="Center" >ราย</asp:TableCell>
                                <asp:TableCell Width="50px" BorderColor="Black" BorderWidth="1" Font-Bold="true" HorizontalAlign="Center" >%</asp:TableCell>
                            </asp:TableRow>
                          <asp:TableRow ID="TableRow3" runat="server" Width="950px">
                                    <asp:TableCell ColumnSpan="18" BorderWidth="1">                                        
                        <asp:GridView ID="gvTable" runat="server" ShowHeader="False" AutoGenerateColumns="false"
                        PagerSettings-Mode="NumericFirstLast" AllowPaging="True" OnPageIndexChanging="gvTable_PageChanging" PageSize="20" Width="950px">
                        <Columns >	
                        <asp:BoundField DataField="SCORE_CARD_TYPE" ItemStyle-Width="75px" ItemStyle-BorderColor="Black" ItemStyle-BorderWidth="1"  />
                        <asp:BoundField DataField="SCORE_CARD_NAME" ItemStyle-Width="75px" ItemStyle-BorderColor="Black" ItemStyle-BorderWidth="1"  />
                        <asp:BoundField DataField="สินเชื่อธุรกิจ SMEs 1" ItemStyle-Width="50px" ItemStyle-BorderColor="Black" ItemStyle-BorderWidth="1"  />
                        <asp:BoundField DataField="%1" ItemStyle-Width="50px" ItemStyle-BorderColor="Black" ItemStyle-BorderWidth="1"  />
                        <asp:BoundField DataField="สินเชื่อธุรกิจ SMEs 2" ItemStyle-Width="50px" ItemStyle-BorderColor="Black" ItemStyle-BorderWidth="1"  />
                        <asp:BoundField DataField="%2" ItemStyle-Width="50px" ItemStyle-BorderColor="Black" ItemStyle-BorderWidth="1"  />
                        <asp:BoundField DataField="สินเชื่อธุรกิจ SMEs 3" ItemStyle-Width="50px" ItemStyle-BorderColor="Black" ItemStyle-BorderWidth="1"  />
                        <asp:BoundField DataField="%3" ItemStyle-Width="50px" ItemStyle-BorderColor="Black" ItemStyle-BorderWidth="1"  />
                        <asp:BoundField DataField="สินเชื่อธุรกิจ SMEs 4" ItemStyle-Width="50px" ItemStyle-BorderColor="Black" ItemStyle-BorderWidth="1"  />
                        <asp:BoundField DataField="%4" ItemStyle-Width="50px" ItemStyle-BorderColor="Black" ItemStyle-BorderWidth="1"  />
                        <asp:BoundField DataField="สินเชื่อธุรกิจขนาดใหญ่" ItemStyle-Width="50px" ItemStyle-BorderColor="Black" ItemStyle-BorderWidth="1"  />
                        <asp:BoundField DataField="%5" ItemStyle-Width="50px" ItemStyle-BorderColor="Black" ItemStyle-BorderWidth="1"  />
                        <asp:BoundField DataField="สินเชื่อภาครัฐและสถาบัน 1" ItemStyle-Width="50px" ItemStyle-BorderColor="Black" ItemStyle-BorderWidth="1"  />
                        <asp:BoundField DataField="%6" ItemStyle-Width="50px" ItemStyle-BorderColor="Black" ItemStyle-BorderWidth="1"  />
                        <asp:BoundField DataField="สินเชื่อภาครัฐและสถาบัน 2" ItemStyle-Width="50px" ItemStyle-BorderColor="Black" ItemStyle-BorderWidth="1"  />
                        <asp:BoundField DataField="%7" ItemStyle-Width="50px" ItemStyle-BorderColor="Black" ItemStyle-BorderWidth="1"  />
                        <asp:BoundField DataField="สินเชื่อลิซซิ่งและแฟคตอริ่ง" ItemStyle-Width="50px" ItemStyle-BorderColor="Black" ItemStyle-BorderWidth="1"  />
                        <asp:BoundField DataField="%8" ItemStyle-Width="50px" ItemStyle-BorderColor="Black" ItemStyle-BorderWidth="1"  />
                        </Columns>
                        </asp:GridView> 
                        </asp:TableCell>
                        </asp:TableRow>    
                         </asp:Table>
                         </div>
                            </asp:TableCell>
                            </asp:TableRow>


                            </asp:Table>  
                       </ContentTemplate></asp:UpdatePanel></asp:Content>