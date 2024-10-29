<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/CreditScoring.Master" AutoEventWireup="true" CodeBehind="RatingReport_05.aspx.cs" Inherits="GSB.Report.Rating.RatingReport_05" %>
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
                    <asp:TableHeaderCell ColumnSpan="2"><u>เครื่องมือค้นหา</u> : รายงานแสดงการเรียงลำดับค่า PD </asp:TableHeaderCell>
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
                        <asp:Table ID="HeadTxtLoan" runat="server" CellPadding="0" CellSpacing="0" CssClass="ExpandTable">
                             <asp:TableRow ID="TableRow2" runat="server">
                                <asp:TableCell Width="200px" ColumnSpan="4">กลุ่มตัวอย่างตั้งแต่ปีแรกจนถึงก่อนปีล่าสุด</asp:TableCell>
                                <asp:TableCell Width="200px" ColumnSpan="4">กลุ่มตัวอย่างปีล่าสุด</asp:TableCell>
                                <asp:TableCell Width="200px" ColumnSpan="4">กลุ่มตัวอย่างรวม</asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow ID="TableRow1" runat="server">
                                <asp:TableCell Width="50px">ระดับเกรด</asp:TableCell>
                                <asp:TableCell Width="50px">จำนวน</asp:TableCell>
                                <asp:TableCell Width="50px">จำนวนผิดนัด</asp:TableCell>
                                <asp:TableCell Width="50px">ค่า PD</asp:TableCell>
                                <asp:TableCell Width="50px">ระดับเกรด</asp:TableCell>
                                <asp:TableCell Width="50px">จำนวน</asp:TableCell>
                                <asp:TableCell Width="50px">จำนวนผิดนัด</asp:TableCell>
                                <asp:TableCell Width="50px">ค่า PD</asp:TableCell>
                                <asp:TableCell Width="50px">ระดับเกรด</asp:TableCell>
                                <asp:TableCell Width="50px">จำนวน</asp:TableCell>
                                <asp:TableCell Width="50px">จำนวนผิดนัด</asp:TableCell>
                                <asp:TableCell Width="50px">ค่า PD</asp:TableCell>
                            </asp:TableRow>
                          <asp:TableRow ID="TableRow3" runat="server">
                                    <asp:TableCell ColumnSpan="12" BorderWidth="0">                                        
                        <asp:GridView ID="gvTable" runat="server" CssClass="GridView" ShowHeader="False" AutoGenerateColumns="false"
                        PagerSettings-Mode="NumericFirstLast" AllowPaging="True" OnPageIndexChanging="gvTable_PageChanging" PageSize="15">
                        <Columns >					
                        <asp:BoundField DataField="OBLIG_RATING_1" ItemStyle-Width="50px" />
                        <asp:BoundField DataField="N1" ItemStyle-Width="50px" />
                        <asp:BoundField DataField="DF1" ItemStyle-Width="50px" />
                        <asp:BoundField DataField="P1" ItemStyle-Width="50px" />
                        <asp:BoundField DataField="OBLIG_RATING_2" ItemStyle-Width="50px" />
                        <asp:BoundField DataField="N2" ItemStyle-Width="50px" />
                        <asp:BoundField DataField="DF2" ItemStyle-Width="50px" />
                        <asp:BoundField DataField="P2" ItemStyle-Width="50px" />
                        <asp:BoundField DataField="OBLIG_RATING" ItemStyle-Width="50px" />
                        <asp:BoundField DataField="N" ItemStyle-Width="50px" />
                        <asp:BoundField DataField="DF" ItemStyle-Width="50px" />
                        <asp:BoundField DataField="P" ItemStyle-Width="50px" />
                        </Columns>
                        </asp:GridView> 
                        </asp:TableCell>
                        </asp:TableRow>    
                         </asp:Table>
                            </asp:TableCell>
                            </asp:TableRow>

                            </asp:Table>  
                       </ContentTemplate></asp:UpdatePanel></asp:Content>