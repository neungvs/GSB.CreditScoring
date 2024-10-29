<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/CreditScoring.Master" AutoEventWireup="true" CodeBehind="RatingReport_01.aspx.cs" Inherits="GSB.Report.Rating.RatingReport_01" %>
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
                    <asp:TableHeaderCell ColumnSpan="2"><u>เครื่องมือค้นหา</u> : รายงานแสดงเครดิตเรตติ้งจำแนกตามหน่วยงาน </asp:TableHeaderCell>
                </asp:TableHeaderRow>
                <asp:TableRow>
                    <asp:TableCell CssClass="subject">ประเภทแบบจำลอง</asp:TableCell>
                    <asp:TableCell CssClass="detail">
                        <asp:DropDownList ID="ddlModel" runat="server" DataTextField="MODEL_NAME" DataValueField="MODEL_NAME" OnSelectedIndexChanged ="LoadLoans"  AutoPostBack="true" />
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell CssClass="subject">ประเภทแบบจำลองย่อย</asp:TableCell>
                    <asp:TableCell CssClass="detail">
                        <asp:DropDownList ID="ddlLoan" runat="server" DataTextField="LOAN_NAME" DataValueField="LOAN_NAME" />
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell CssClass="subject">หน่วยงานผู้จัดทำ</asp:TableCell>
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
                        <asp:Table ID="HeadTxtLoan" runat="server" CellPadding="0" CellSpacing="0" CssClass="RatingRpt">
                             <asp:TableRow ID="TableRow2" runat="server">
                                <asp:TableCell Width="100px" RowSpan="2">ชื่อกิจการ</asp:TableCell>
                                <asp:TableCell Width="100px" RowSpan="2">เจ้าหน้าที่ผู้จัดทำ</asp:TableCell>
                                <asp:TableCell Width="200px" RowSpan="2">ประเภทแบบจำลอง</asp:TableCell>
                                <asp:TableCell Width="450px" ColumnSpan="3">รายที่อนุมัติจากอันดับเครดิตที่ไม่ผ่านเกณฑ์ของธนาคาร<br />(10D-10F,11C-11F,12B-12F,13A-13F,14A-14F)</asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow ID="TableRow1" runat="server">
                                <asp:TableCell Width="150px">อันดับเครดิตโดยฝ่ายงานวิเคราะห์สินเชื่อ</asp:TableCell>
                                <asp:TableCell Width="150px">อันดับเครดิตโดยฝ่ายงานควบคุมความเสี่ยงสินเชื่อ</asp:TableCell>
                                <asp:TableCell Width="150px">หมายเหตุ</asp:TableCell>
                            </asp:TableRow>
                          <asp:TableRow ID="TableRow3" runat="server">
                                    <asp:TableCell ColumnSpan="6" BorderWidth="0">                                        
                        <asp:GridView ID="gvTable" runat="server" CssClass="RatingRpt" ShowHeader="False" AutoGenerateColumns="false"
                        PagerSettings-Mode="NumericFirstLast" AllowPaging="True" OnPageIndexChanging="gvTable_PageChanging" PageSize="15">
                        <Columns >					
                        <asp:BoundField DataField="COMPANY" ItemStyle-Width="100px" />
                        <asp:BoundField DataField="ANALYST_NAME" ItemStyle-Width="100px" />
                        <asp:BoundField DataField="SCORE_CARD_TYPE" ItemStyle-Width="200px" />
                        <asp:BoundField DataField="ASSM_RATING" ItemStyle-Width="150px" />
                        <asp:BoundField DataField="ASSM_RATING_CONTROL_CREDIT" ItemStyle-Width="150px" />
                        <asp:BoundField DataField="REMARK" ItemStyle-Width="150px" />
                        </Columns>
                        </asp:GridView> 
                        </asp:TableCell>
                        </asp:TableRow>    
                         </asp:Table>
                            </asp:TableCell>
                            </asp:TableRow>

                    <asp:TableRow>
                    <asp:TableCell CssClass="TableRow">
                        <asp:Table ID="Table2" runat="server" CellPadding="0" CellSpacing="0" CssClass="RatingRpt">
                             <asp:TableRow ID="TableRow4" runat="server">
                                <asp:TableCell Width="100px" RowSpan="2">ชื่อกิจการ</asp:TableCell>
                                <asp:TableCell Width="100px" RowSpan="2">เจ้าหน้าที่ผู้จัดทำ</asp:TableCell>
                                <asp:TableCell Width="200px" RowSpan="2">ประเภทแบบจำลอง</asp:TableCell>
                                <asp:TableCell Width="450px" ColumnSpan="3">รายที่ปฏิเสธเนื่องจากอันดับเครดิตที่ไม่ผ่านเกณฑ์ของธนาคาร<br />(10D-10F,11C-11F,12B-12F,13A-13F,14A-14F)</asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow ID="TableRow5" runat="server">
                                <asp:TableCell Width="150px">อันดับเครดิตโดยฝ่ายงานวิเคราะห์สินเชื่อ</asp:TableCell>
                                <asp:TableCell Width="150px">อันดับเครดิตโดยฝ่ายงานควบคุมความเสี่ยงสินเชื่อ</asp:TableCell>
                                <asp:TableCell Width="150px">หมายเหตุ</asp:TableCell>
                            </asp:TableRow>
                          <asp:TableRow ID="TableRow6" runat="server">
                                    <asp:TableCell ColumnSpan="6" BorderWidth="0">                                        
                        <asp:GridView ID="GridView1" runat="server" CssClass="RatingRpt" ShowHeader="False" AutoGenerateColumns="false"
                        PagerSettings-Mode="NumericFirstLast" AllowPaging="True" OnPageIndexChanging="gvTable_PageChanging2" PageSize="15">
                        <Columns >					
                        <asp:BoundField DataField="COMPANY" ItemStyle-Width="100px" />
                        <asp:BoundField DataField="ANALYST_NAME" ItemStyle-Width="100px" />
                        <asp:BoundField DataField="SCORE_CARD_TYPE" ItemStyle-Width="200px" />
                        <asp:BoundField DataField="ASSM_RATING" ItemStyle-Width="150px" />
                        <asp:BoundField DataField="ASSM_RATING_CONTROL_CREDIT" ItemStyle-Width="150px" />
                        <asp:BoundField DataField="REMARK" ItemStyle-Width="150px" />
                        </Columns>
                        </asp:GridView> 
                        </asp:TableCell>
                        </asp:TableRow>    
                         </asp:Table>
                            </asp:TableCell>
                            </asp:TableRow>

                    <asp:TableRow>
                    <asp:TableCell CssClass="TableRow">
                        <asp:Table ID="Table5" runat="server" CellPadding="0" CellSpacing="0" CssClass="RatingRpt">
                             <asp:TableRow ID="TableRow13" runat="server">
                                <asp:TableCell Width="100px" RowSpan="2">ชื่อกิจการ</asp:TableCell>
                                <asp:TableCell Width="100px" RowSpan="2">เจ้าหน้าที่ผู้จัดทำ</asp:TableCell>
                                <asp:TableCell Width="200px" RowSpan="2">ประเภทแบบจำลอง</asp:TableCell>
                                <asp:TableCell Width="450px" ColumnSpan="3">รายที่ปฏิเสธและอันดับเครดิตผ่านเกณฑ์ของธนาคาร</asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow ID="TableRow14" runat="server">
                                <asp:TableCell Width="150px">อันดับเครดิตโดยฝ่ายงานวิเคราะห์สินเชื่อ</asp:TableCell>
                                <asp:TableCell Width="150px">อันดับเครดิตโดยฝ่ายงานควบคุมความเสี่ยงสินเชื่อ</asp:TableCell>
                                <asp:TableCell Width="150px">หมายเหตุ</asp:TableCell>
                            </asp:TableRow>
                          <asp:TableRow ID="TableRow15" runat="server">
                                    <asp:TableCell ColumnSpan="6" BorderWidth="0">                                        
                        <asp:GridView ID="GridView4" runat="server" CssClass="RatingRpt" ShowHeader="False" AutoGenerateColumns="false"
                        PagerSettings-Mode="NumericFirstLast" AllowPaging="True" OnPageIndexChanging="gvTable_PageChanging3" PageSize="15">
                        <Columns >					
                        <asp:BoundField DataField="COMPANY" ItemStyle-Width="100px" />
                        <asp:BoundField DataField="ANALYST_NAME" ItemStyle-Width="100px" />
                        <asp:BoundField DataField="SCORE_CARD_TYPE" ItemStyle-Width="200px" />
                        <asp:BoundField DataField="ASSM_RATING" ItemStyle-Width="150px" />
                        <asp:BoundField DataField="ASSM_RATING_CONTROL_CREDIT" ItemStyle-Width="150px" />
                        <asp:BoundField DataField="REMARK" ItemStyle-Width="150px" />
                        </Columns>
                        </asp:GridView> 
                        </asp:TableCell>
                        </asp:TableRow>    
                         </asp:Table>
                            </asp:TableCell>
                            </asp:TableRow>

                    <asp:TableRow>
                    <asp:TableCell CssClass="TableRow">
                        <asp:Table ID="Table3" runat="server" CellPadding="0" CellSpacing="0" CssClass="RatingRpt">
                             <asp:TableRow ID="TableRow7" runat="server">
                                <asp:TableCell Width="100px" RowSpan="2">ชื่อกิจการ</asp:TableCell>
                                <asp:TableCell Width="100px" RowSpan="2">เจ้าหน้าที่ผู้จัดทำ</asp:TableCell>
                                <asp:TableCell Width="200px" RowSpan="2">ประเภทแบบจำลอง</asp:TableCell>
                                <asp:TableCell Width="450px" ColumnSpan="3">รายที่อนุมัติและอันดับเครดิตผ่านเกณฑ์ของธนาคาร</asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow ID="TableRow8" runat="server">
                                <asp:TableCell Width="150px">อันดับเครดิตโดยฝ่ายงานวิเคราะห์สินเชื่อ</asp:TableCell>
                                <asp:TableCell Width="150px">อันดับเครดิตโดยฝ่ายงานควบคุมความเสี่ยงสินเชื่อ</asp:TableCell>
                                <asp:TableCell Width="150px">หมายเหตุ</asp:TableCell>
                            </asp:TableRow>
                          <asp:TableRow ID="TableRow9" runat="server">
                                    <asp:TableCell ColumnSpan="6" BorderWidth="0">                                        
                        <asp:GridView ID="GridView2" runat="server" CssClass="RatingRpt" ShowHeader="False" AutoGenerateColumns="false"
                        PagerSettings-Mode="NumericFirstLast" AllowPaging="True" OnPageIndexChanging="gvTable_PageChanging4" PageSize="15">
                        <Columns >					
                        <asp:BoundField DataField="COMPANY" ItemStyle-Width="100px" />
                        <asp:BoundField DataField="ANALYST_NAME" ItemStyle-Width="100px" />
                        <asp:BoundField DataField="SCORE_CARD_TYPE" ItemStyle-Width="200px" />
                        <asp:BoundField DataField="ASSM_RATING" ItemStyle-Width="150px" />
                        <asp:BoundField DataField="ASSM_RATING_CONTROL_CREDIT" ItemStyle-Width="150px" />
                        <asp:BoundField DataField="REMARK" ItemStyle-Width="150px" />
                        </Columns>
                        </asp:GridView> 
                        </asp:TableCell>
                        </asp:TableRow>    
                         </asp:Table>
                            </asp:TableCell>
                            </asp:TableRow>
                    <asp:TableRow>
                    <asp:TableCell CssClass="TableRow">
                        <asp:Table ID="Table4" runat="server" CellPadding="0" CellSpacing="0" CssClass="RatingRpt">
                             <asp:TableRow ID="TableRow10" runat="server">
                                <asp:TableCell Width="100px" RowSpan="2">ชื่อกิจการ</asp:TableCell>
                                <asp:TableCell Width="100px" RowSpan="2">เจ้าหน้าที่ผู้จัดทำ</asp:TableCell>
                                <asp:TableCell Width="200px" RowSpan="2">ประเภทแบบจำลอง</asp:TableCell>
                                <asp:TableCell Width="450px" ColumnSpan="3">รายที่อนุมัติจากอันดับเครดิตของเจ้าหน้าที่ควบคุมความเสี่ยงสินเชื่อแตกต่างจากเจ้าหน้าที่วิเคราะห์สินเชื่อ</asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow ID="TableRow11" runat="server">
                                <asp:TableCell Width="150px">อันดับเครดิตโดยฝ่ายงานวิเคราะห์สินเชื่อ</asp:TableCell>
                                <asp:TableCell Width="150px">อันดับเครดิตโดยฝ่ายงานควบคุมความเสี่ยงสินเชื่อ</asp:TableCell>
                                <asp:TableCell Width="150px">หมายเหตุ</asp:TableCell>
                            </asp:TableRow>
                          <asp:TableRow ID="TableRow12" runat="server">
                                    <asp:TableCell ColumnSpan="6" BorderWidth="0">                                        
                        <asp:GridView ID="GridView3" runat="server" CssClass="RatingRpt" ShowHeader="False" AutoGenerateColumns="false"
                        PagerSettings-Mode="NumericFirstLast" AllowPaging="True" OnPageIndexChanging="gvTable_PageChanging5" PageSize="15">
                        <Columns >					
                        <asp:BoundField DataField="COMPANY" ItemStyle-Width="100px" />
                        <asp:BoundField DataField="ANALYST_NAME" ItemStyle-Width="100px" />
                        <asp:BoundField DataField="SCORE_CARD_TYPE" ItemStyle-Width="200px" />
                        <asp:BoundField DataField="ASSM_RATING" ItemStyle-Width="150px" />
                        <asp:BoundField DataField="ASSM_RATING_CONTROL_CREDIT" ItemStyle-Width="150px" />
                        <asp:BoundField DataField="REMARK" ItemStyle-Width="150px" />
                        </Columns>
                        </asp:GridView> 
                        </asp:TableCell>
                        </asp:TableRow>    
                         </asp:Table>
                            </asp:TableCell>
                            </asp:TableRow>
                            </asp:Table>  
                       </ContentTemplate></asp:UpdatePanel></asp:Content>