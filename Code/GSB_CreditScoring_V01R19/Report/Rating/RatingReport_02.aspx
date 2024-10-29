<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/CreditScoring.Master" AutoEventWireup="true" CodeBehind="RatingReport_02.aspx.cs" Inherits="GSB.Report.Rating.RatingReport_02" %>
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
                    <asp:TableHeaderCell ColumnSpan="2"><u>เครื่องมือค้นหา</u> : รายงานแสดงเครดิตเรตติ้งโดยผู้มีอำนาจอนุมัติ </asp:TableHeaderCell>
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
                    <asp:TableCell CssClass="subject">ชุดของผู้มีอำนาจอนุมัติ</asp:TableCell>
                    <asp:TableCell CssClass="detail">
                        <asp:DropDownList ID="ddlSubType" runat="server" DataTextField="STYPER_NAME" DataValueField="STYPER_NAME" />
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
                             <asp:TableRow ID="TableRow2" runat="server">
                                <asp:TableCell Width="50px" RowSpan="2">ชื่อกิจการ</asp:TableCell>
                                <asp:TableCell Width="45px" RowSpan="2">ฝ่ายงานที่นำเสนอ</asp:TableCell>
                                <asp:TableCell Width="85px" RowSpan="2">ประเภทแบบจำลอง</asp:TableCell>
                                <asp:TableCell Width="210px" ColumnSpan="3">รายที่ปฏิเสธ (LOPs)</asp:TableCell>
                                <asp:TableCell Width="210px" ColumnSpan="3">รายที่อนุมัติ (LOPs)</asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow ID="TableRow1" runat="server">
                                <asp:TableCell Width="80px">อันดับเครดิตโดยฝ่ายงานวิเคราะห์สินเชื่อ</asp:TableCell>
                                <asp:TableCell Width="80px">อันดับเครดิตโดยฝ่ายงานควบคุมความเสี่ยงสินเชื่อ</asp:TableCell>
                                <asp:TableCell Width="50px">เหตุผลในการปฏิเสธ</asp:TableCell>                                
                                <asp:TableCell Width="80px">อันดับเครดิตโดยฝ่ายงานวิเคราะห์สินเชื่อ</asp:TableCell>
                                <asp:TableCell Width="80px">อันดับเครดิตโดยฝ่ายงานควบคุมความเสี่ยงสินเชื่อ</asp:TableCell>
                                <asp:TableCell Width="50px">หมายเหตุ</asp:TableCell>
                            </asp:TableRow>
                          <asp:TableRow ID="TableRow3" runat="server">
                                    <asp:TableCell ColumnSpan="9" BorderWidth="0">                                        
                        <asp:GridView ID="gvTable" runat="server" CssClass="GridView" ShowHeader="False" AutoGenerateColumns="false"
                        PagerSettings-Mode="NumericFirstLast" AllowPaging="True" OnPageIndexChanging="gvTable_PageChanging" PageSize="15">
                        <Columns >					
                        <asp:BoundField DataField="COMPANY" ItemStyle-Width="48px" />
                        <asp:BoundField DataField="DIVISION" ItemStyle-Width="45px" />
                        <asp:BoundField DataField="SCORE_CARD_TYPE" ItemStyle-Width="85px" />
                        <asp:BoundField DataField="ASSM_RATING_Reject" ItemStyle-Width="80px" />
                        <asp:BoundField DataField="ASSM_RATING_CONTROL_CREDIT_Reject" ItemStyle-Width="80px" />
                        <asp:BoundField DataField="REMARK_Reject" ItemStyle-Width="50px" />                        
                        <asp:BoundField DataField="ASSM_RATING_Approve" ItemStyle-Width="80px" />
                        <asp:BoundField DataField="ASSM_RATING_CONTROL_CREDIT_Approve" ItemStyle-Width="80px" />
                        <asp:BoundField DataField="REMARK_Approve" ItemStyle-Width="50px" />
                        </Columns>
                        </asp:GridView> 
                        </asp:TableCell>
                        </asp:TableRow>    
                         </asp:Table>
                            </asp:TableCell>
                            </asp:TableRow>
                            </asp:Table>  
                       </ContentTemplate></asp:UpdatePanel></asp:Content>
