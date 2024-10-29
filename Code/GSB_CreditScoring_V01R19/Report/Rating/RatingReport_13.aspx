<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/CreditScoring.Master" AutoEventWireup="true" CodeBehind="RatingReport_13.aspx.cs" Inherits="GSB.Report.Rating.RatingReport_13" %>
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
                    <asp:TableHeaderCell ColumnSpan="2"><u>เครื่องมือค้นหา</u> : รายงานการกระจายตัวของตัวแปรเชิงคุณภาพ/ปริมาณเปรียบเทียบกับการผิดชำระหนี้ </asp:TableHeaderCell>
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
                </asp:TableRow>                
                <asp:TableRow>
                    <asp:TableCell CssClass="subject">ประเภทคำถาม</asp:TableCell>
                    <asp:TableCell CssClass="detail">
                        <asp:DropDownList ID="ddlqtion" runat="server" DataTextField="QT_NAME" DataValueField="QT_NAME" />
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell CssClass="subject">ข้อมูลการใช้งาน ณ เดือน</asp:TableCell>
                    <asp:TableCell CssClass="detail">
                        <asp:DropDownList ID="ddlStatDate" runat="server" DataTextField="STAT_DATE" DataValueField="STAT_DATE" />
                    </asp:TableCell>
                    </asp:TableRow><asp:TableFooterRow>
                    <asp:TableCell ColumnSpan="2" CssClass="action">
                        <asp:Button ID="Search" runat="server" Text="แสดงข้อมูล" OnClick="Search_Click" />&nbsp;
<%--                        <asp:Button ID="Cancel" runat="server" Text="ล้างข้อมูล" OnClick="Cancel_Click" />--%>
                    </asp:TableCell></asp:TableFooterRow></asp:Table>
                    <asp:Table ID="ResultTable" runat="server" CellPadding="0" CellSpacing="0" 
                        CssClass="Result" BorderColor="Black" BorderWidth="1px">
                <asp:TableRow>
                    <asp:TableCell CssClass="HeaderRow"><u>แสดงผลการค้นหา</u> : </asp:TableCell></asp:TableRow>
                    <asp:TableRow Width="900px">
                    <asp:TableCell CssClass="TableRow" Width="100%">
                         <div style="overflow:scroll;table-layout:fixed" align="center">
                          <asp:Table ID="HeadTxtLoan" runat="server" CellPadding="0" CellSpacing="0" Width="100%" BorderWidth="1" BorderColor="Black">
                            <asp:TableRow ID="TableRow2" runat="server" Width="100%" BorderWidth="1" BorderColor="Black">
                                <asp:TableCell Width="12%" BorderColor="Black" BorderWidth="1">คำถามข้อที่</asp:TableCell>
                                <asp:TableCell Width="10%" BorderColor="Black" BorderWidth="1">คำตอบข้อที่</asp:TableCell>
                                <asp:TableCell Width="13%" BorderColor="Black" BorderWidth="1">ผลการเลือกคำตอบ (ราย)</asp:TableCell>
                                <asp:TableCell Width="13%" BorderColor="Black" BorderWidth="1">ผลการเลือกคำตอบ (%)</asp:TableCell>
                                <asp:TableCell Width="13%" BorderColor="Black" BorderWidth="1">NPL (ราย)</asp:TableCell>
                                <asp:TableCell Width="13%" BorderColor="Black" BorderWidth="1">Default (ราย)</asp:TableCell>
                                <asp:TableCell Width="13%" BorderColor="Black" BorderWidth="1">NPL (%)</asp:TableCell>
                                <asp:TableCell Width="13%" BorderColor="Black" BorderWidth="1">Default (%)</asp:TableCell>
                            </asp:TableRow>
                          <asp:TableRow ID="TableRow3" runat="server" Width="100%">
                                    <asp:TableCell ColumnSpan="8" BorderWidth="1"  Width="100%">                                        
                        <asp:GridView ID="gvTable" runat="server" ShowHeader="false"  AutoGenerateColumns="false" EmptyDataText="No records found"
                        PagerSettings-Mode="NumericFirstLast" AllowPaging="True" OnPageIndexChanging="gvTable_PageChanging" PageSize="20" 
                        Font-Size="Small" Font-Bold="False" Font-Italic="True" Font-Names="Tahoma" BorderColor="Black" BorderWidth="1" Width="100%">
                        <Columns>			
                        <asp:BoundField DataField="QUESTION" ItemStyle-Width="12%" ItemStyle-BorderColor="Black" ItemStyle-BorderWidth="1" />
                        <asp:BoundField DataField="ANSWER" ItemStyle-Width="10%" ItemStyle-BorderColor="Black" ItemStyle-BorderWidth="1" />
                        <asp:BoundField DataField="AMT" ItemStyle-Width="13%" ItemStyle-BorderColor="Black" ItemStyle-BorderWidth="1" />
                        <asp:BoundField DataField="%" ItemStyle-Width="13%" ItemStyle-BorderColor="Black" ItemStyle-BorderWidth="1" />
                        <asp:BoundField DataField="NPLs" ItemStyle-Width="13%" ItemStyle-BorderColor="Black" ItemStyle-BorderWidth="1" />
                        <asp:BoundField DataField="DEFAULT" ItemStyle-Width="13%" ItemStyle-BorderColor="Black" ItemStyle-BorderWidth="1" />
                        <asp:BoundField DataField="NPLs(%)" ItemStyle-Width="13%" ItemStyle-BorderColor="Black" ItemStyle-BorderWidth="1" />
                        <asp:BoundField DataField="DEFAULT(%)" ItemStyle-Width="13%" ItemStyle-BorderColor="Black" ItemStyle-BorderWidth="1" />
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
