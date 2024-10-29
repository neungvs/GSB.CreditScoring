<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/CreditScoring.Master" AutoEventWireup="true" CodeBehind="RatingReport_06.aspx.cs" Inherits="GSB.Report.Rating.RatingReport_06" %>
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
                    <asp:TableHeaderCell ColumnSpan="2"><u>เครื่องมือค้นหา</u> : รายงานทดสอบการคงค่าของ PD </asp:TableHeaderCell>
                </asp:TableHeaderRow>
                <asp:TableRow>
                    <asp:TableCell CssClass="subject">ประเภทแบบจำลอง</asp:TableCell>
                    <asp:TableCell CssClass="detail">
                        <asp:DropDownList ID="ddlModel" runat="server" DataTextField="MODEL_NAME" DataValueField="MODEL_NAME" OnSelectedIndexChanged ="LoadLoans"  AutoPostBack="true"  />
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell CssClass="subject">ประเภทแบบจำลองย่อย</asp:TableCell>
                    <asp:TableCell CssClass="detail">
                        <asp:DropDownList ID="ddlLoan" runat="server" DataTextField="LOAN_NAME" DataValueField="LOAN_NAME" />
                    </asp:TableCell>
                </asp:TableRow><asp:TableFooterRow>
                    <asp:TableCell ColumnSpan="2" CssClass="action">
                        <asp:Button ID="Search" runat="server" Text="แสดงข้อมูล" OnClick="Search_Click" />&nbsp;
<%--                        <asp:Button ID="Cancel" runat="server" Text="ล้างข้อมูล" OnClick="Cancel_Click" />--%>
                    </asp:TableCell></asp:TableFooterRow></asp:Table>
                    <asp:Table ID="ResultTable" runat="server" CellPadding="0" CellSpacing="0" CssClass="Result">
                <asp:TableRow>
                    <asp:TableCell CssClass="HeaderRow"><u>แสดงผลการค้นหา</u> : </asp:TableCell></asp:TableRow>
                    <asp:TableRow>
                    <asp:TableCell CssClass="TableRow">
                                       <div style="overflow:scroll;Width="100%";table-layout:fixed" align="center">
                        <asp:Table ID="HeadTxtLoan" runat="server" CellPadding="0" CellSpacing="0" >
                            <asp:TableRow ID="TableRow1" runat="server">
                                <asp:TableCell Width="100px" BorderColor="Black" BorderWidth="1">ระดับเกรด</asp:TableCell>
                                <asp:TableCell Width="100px" BorderColor="Black" BorderWidth="1">P1-P2</asp:TableCell>
                                <asp:TableCell Width="200px" BorderColor="Black" BorderWidth="1">1/n1 + 1/n2</asp:TableCell>
                                <asp:TableCell Width="100px" BorderColor="Black" BorderWidth="1">P*(1-P)</asp:TableCell>
                                <asp:TableCell Width="50px" BorderColor="Black" BorderWidth="1">Z</asp:TableCell>
                                <asp:TableCell Width="200px" BorderColor="Black" BorderWidth="1">Reject H0 (95%)</asp:TableCell>
                            </asp:TableRow>
                          <asp:TableRow ID="TableRow3" runat="server">
                                    <asp:TableCell ColumnSpan="12" BorderWidth="0">                                        
                        <asp:GridView ID="gvTable" runat="server"  ShowHeader="False" AutoGenerateColumns="false"
                        PagerSettings-Mode="NumericFirstLast" AllowPaging="True" OnPageIndexChanging="gvTable_PageChanging" PageSize="15">
                        <Columns >					
                        <asp:BoundField DataField="OBLIG_RATING" ItemStyle-Width="100px" ItemStyle-BorderColor="Black" ItemStyle-BorderWidth="1"  />
                        <asp:BoundField DataField="P1-P2" ItemStyle-Width="100px" ItemStyle-BorderColor="Black" ItemStyle-BorderWidth="1"  />
                        <asp:BoundField DataField="1/N1+1/N2" ItemStyle-Width="200px" ItemStyle-BorderColor="Black" ItemStyle-BorderWidth="1"  />
                        <asp:BoundField DataField="P(1-P)" ItemStyle-Width="100px" ItemStyle-BorderColor="Black" ItemStyle-BorderWidth="1"  />
                        <asp:BoundField DataField="Z" ItemStyle-Width="50px" ItemStyle-BorderColor="Black" ItemStyle-BorderWidth="1"  />
                        <asp:BoundField DataField="Reject H0 (95%)" ItemStyle-Width="200px"  ItemStyle-BorderColor="Black" ItemStyle-BorderWidth="1" />
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
