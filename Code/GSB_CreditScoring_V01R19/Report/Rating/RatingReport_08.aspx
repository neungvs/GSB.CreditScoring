<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/CreditScoring.Master" AutoEventWireup="true" CodeBehind="RatingReport_08.aspx.cs" Inherits="GSB.Report.Rating.RatingReport_08" %>
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
                    <asp:TableHeaderCell ColumnSpan="2"><u>เครื่องมือค้นหา</u> : รายงานทดสอบความแม่นยำตัวแปรเชิงคุณภาพ </asp:TableHeaderCell>
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
                    <asp:TableCell CssClass="subject">ข้อมูลการใช้งาน ณ เดือน</asp:TableCell>
                    <asp:TableCell CssClass="detail">
                        <asp:DropDownList ID="ddlStatDate" runat="server" DataTextField="STAT_DATE" DataValueField="STAT_DATE" />
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
                            <asp:TableRow ID="TableRow1" runat="server">
                                <asp:TableCell Width="50px">ชื่อผู้กู้</asp:TableCell>
                                <asp:TableCell Width="50px">ID Number </asp:TableCell>
                                <asp:TableCell Width="50px">CIF Number</asp:TableCell>
                                <asp:TableCell Width="50px">Application ID</asp:TableCell>
                                <asp:TableCell Width="50px">วันที่จัดทำ CRR</asp:TableCell>
                                <asp:TableCell Width="50px">Default</asp:TableCell>
                                <asp:TableCell Width="50px">รวมคะแนนคำถามข้อมูลด้านอุตสาหกรรมก่อนถ่วงน้ำหนัก</asp:TableCell>
                                <asp:TableCell Width="50px">รวมคะแนนคำถามข้อมูลด้านอุตสาหกรรมหลังถ่วงน้ำหนัก</asp:TableCell>
                                <asp:TableCell Width="50px">รวมคะแนนคำถามข้อมูลการจัดการทั่วไปก่อนถ่วงน้ำหนัก</asp:TableCell>
                                <asp:TableCell Width="50px">รวมคะแนนคำถามข้อมูลการจัดการทั่วไปหลังถ่วงน้ำหนัก</asp:TableCell>
                                <asp:TableCell Width="50px">รวมคะแนนคำถามข้อมูลการจัดการเฉพาะธุรกิจก่อนถ่วงน้ำหนัก</asp:TableCell>
                                <asp:TableCell Width="50px">รวมคะแนนคำถามข้อมูลการจัดการเฉพาะธุรกิจหลังถ่วงน้ำหนัก</asp:TableCell>
                                <asp:TableCell Width="50px">รวมคะแนนด้านสัญญาณเตือน</asp:TableCell>
                            </asp:TableRow>
                          <asp:TableRow ID="TableRow3" runat="server">
                                    <asp:TableCell ColumnSpan="13" BorderWidth="0">                                        
                        <asp:GridView ID="gvTable" runat="server" CssClass="GridView" ShowHeader="False" AutoGenerateColumns="false"
                        PagerSettings-Mode="NumericFirstLast" AllowPaging="True" OnPageIndexChanging="gvTable_PageChanging" PageSize="15">
                        <Columns >					
                        <asp:BoundField DataField="COMPANY" ItemStyle-Width="50px" />
                        <asp:BoundField DataField="REGISTRATIONID" ItemStyle-Width="50px" />
                        <asp:BoundField DataField="CIF" ItemStyle-Width="50px" />
                        <asp:BoundField DataField="APP_ID" ItemStyle-Width="50px" />
                        <asp:BoundField DataField="CREATION_DATE" ItemStyle-Width="50px" />
                        <asp:BoundField DataField="DEFAULT" ItemStyle-Width="50px" />
                        <asp:BoundField DataField="IND_BWS" ItemStyle-Width="50px" />
                        <asp:BoundField DataField="IND_PTS" ItemStyle-Width="50px" />
                        <asp:BoundField DataField="GEN_BWS" ItemStyle-Width="50px" />
                        <asp:BoundField DataField="GEN_PTS" ItemStyle-Width="50px" />
                        <asp:BoundField DataField="SPEC_BWS" ItemStyle-Width="50px" />
                        <asp:BoundField DataField="SPEC_PTS" ItemStyle-Width="50px" />
                        <asp:BoundField DataField="WARNING" ItemStyle-Width="50px" />
                        </Columns>
                        </asp:GridView> 
                        </asp:TableCell>
                        </asp:TableRow>    
                         </asp:Table>
                            </asp:TableCell>
                            </asp:TableRow>

                            </asp:Table>  
                       </ContentTemplate></asp:UpdatePanel></asp:Content>
