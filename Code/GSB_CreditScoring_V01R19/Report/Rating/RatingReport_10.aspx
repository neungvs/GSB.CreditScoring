<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/CreditScoring.Master" AutoEventWireup="true" CodeBehind="RatingReport_10.aspx.cs" Inherits="GSB.Report.Rating.RatingReport_10" %>
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
                    <asp:TableHeaderCell ColumnSpan="2"><u>เครื่องมือค้นหา</u> : รายงานทดสอบความแม่นยำตัวแปรเชิงปริมาณปีล่าสุดเปรียบเทียบปีฐาน </asp:TableHeaderCell>
                </asp:TableHeaderRow>
                <asp:TableRow>
                    <asp:TableCell CssClass="subject">Baseline</asp:TableCell>
                    <asp:TableCell CssClass="detail">
                        <asp:DropDownList ID="ddlModel" runat="server" DataTextField="MODEL_NAME" DataValueField="MODEL_NAME" />
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell CssClass="subject">Current &nbsp From</asp:TableCell>
                    <asp:TableCell CssClass="detail">
                        <asp:DropDownList ID="ddlLoan" runat="server" DataTextField="LOAN_NAME" DataValueField="LOAN_NAME" />
                    </asp:TableCell>
                    <asp:TableCell CssClass="subject">to</asp:TableCell>
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
                    <asp:TableCell CssClass="TableRow">
                        <asp:Table ID="HeadTxtLoan" runat="server" CellPadding="0" CellSpacing="0" HorizontalAlign="Center"  >
                              <asp:TableRow ID="TableRow1" runat="server">
                                <asp:TableCell Width="150px" BorderColor="Black" BorderWidth="1" Font-Bold="true" HorizontalAlign="Center" >ช่วงคะแนน (ปริมาณ)</asp:TableCell>
                                <asp:TableCell Width="150px" BorderColor="Black" BorderWidth="1" Font-Bold="true" HorizontalAlign="Center" >สัดส่วนคะแนนรวมเชิงปริมาณจากปีที่ใช้ในแบบจำลอง (%)</asp:TableCell>
                                <asp:TableCell Width="150px" BorderColor="Black" BorderWidth="1" Font-Bold="true" HorizontalAlign="Center" >สัดส่วนคะแนนรวมเชิงปริมาณจากปีล่าสุด (%)</asp:TableCell>
                                <asp:TableCell Width="150px" BorderColor="Black" BorderWidth="1" Font-Bold="true" HorizontalAlign="Center" >(A-B)</asp:TableCell>
                                <asp:TableCell Width="150px" BorderColor="Black" BorderWidth="1" Font-Bold="true" HorizontalAlign="Center" >A/B</asp:TableCell>
                                <asp:TableCell Width="150px" BorderColor="Black" BorderWidth="1" Font-Bold="true" HorizontalAlign="Center" >Ln(A/B)</asp:TableCell>
                                <asp:TableCell Width="150px" BorderColor="Black" BorderWidth="1" Font-Bold="true" HorizontalAlign="Center" >Index</asp:TableCell>
                            </asp:TableRow>
                          <asp:TableRow ID="TableRow3" runat="server">
                                    <asp:TableCell ColumnSpan="7" BorderWidth="0">                                        
                        <asp:GridView ID="gvTable" runat="server"  ShowHeader="False" AutoGenerateColumns="false"
                        PagerSettings-Mode="NumericFirstLast" AllowPaging="True" OnPageIndexChanging="gvTable_PageChanging" PageSize="15">
                        <Columns >					
                        <asp:BoundField DataField="RANGE" ItemStyle-Width="150px" ItemStyle-BorderColor="Black" ItemStyle-BorderWidth="1"  />
                        <asp:BoundField DataField="DEV_QUA_%" ItemStyle-Width="150px" ItemStyle-BorderColor="Black" ItemStyle-BorderWidth="1"  />
                        <asp:BoundField DataField="CUR_QUA_%" ItemStyle-Width="150px" ItemStyle-BorderColor="Black" ItemStyle-BorderWidth="1"  />
                        <asp:BoundField DataField="A-B" ItemStyle-Width="150px" ItemStyle-BorderColor="Black" ItemStyle-BorderWidth="1"  />
                        <asp:BoundField DataField="A/B" ItemStyle-Width="150px" ItemStyle-BorderColor="Black" ItemStyle-BorderWidth="1"  />
                        <asp:BoundField DataField="Ln(A/B)" ItemStyle-Width="150px" ItemStyle-BorderColor="Black" ItemStyle-BorderWidth="1"  />
                        <asp:BoundField DataField="INDEX" ItemStyle-Width="150px" ItemStyle-BorderColor="Black" ItemStyle-BorderWidth="1"  />
                        </Columns>
                        </asp:GridView> 
                        </asp:TableCell>
                        </asp:TableRow>    
                         </asp:Table>
                            </asp:TableCell>
                            </asp:TableRow>

                 <asp:TableRow>
                    <asp:TableCell CssClass="TableRow">
                        <asp:Table ID="Table2" runat="server" CellPadding="0" CellSpacing="0" HorizontalAlign="Center"  >
                             <asp:TableRow ID="TableRow4" runat="server">
                                <asp:TableCell Width="150px" BorderColor="Black" BorderWidth="1" Font-Bold="true" HorizontalAlign="Center">ช่วงคะแนน (คุณภาพ)</asp:TableCell>
                                <asp:TableCell Width="150px" BorderColor="Black" BorderWidth="1" Font-Bold="true" HorizontalAlign="Center">สัดส่วนคะแนนรวมเชิงคุณภาพจากปีที่ใช้ในแบบจำลอง (%)</asp:TableCell>
                                <asp:TableCell Width="150px" BorderColor="Black" BorderWidth="1" Font-Bold="true" HorizontalAlign="Center">สัดส่วนคะแนนรวมเชิงคุณภาพจากปีล่าสุด (%)</asp:TableCell>
                                <asp:TableCell Width="150px" BorderColor="Black" BorderWidth="1" Font-Bold="true" HorizontalAlign="Center">(A-B)</asp:TableCell>
                                <asp:TableCell Width="150px" BorderColor="Black" BorderWidth="1" Font-Bold="true" HorizontalAlign="Center">A/B</asp:TableCell>
                                <asp:TableCell Width="150px" BorderColor="Black" BorderWidth="1" Font-Bold="true" HorizontalAlign="Center">Ln(A/B)</asp:TableCell>
                                <asp:TableCell Width="150px" BorderColor="Black" BorderWidth="1" Font-Bold="true" HorizontalAlign="Center">Index</asp:TableCell>

                            </asp:TableRow>
                          <asp:TableRow ID="TableRow6" runat="server">
                                    <asp:TableCell ColumnSpan="7" BorderWidth="0">                                        
                        <asp:GridView ID="GridView1" runat="server" ShowHeader="False" AutoGenerateColumns="false"
                        PagerSettings-Mode="NumericFirstLast" AllowPaging="True" OnPageIndexChanging="gvTable_PageChanging" PageSize="15">
                        <Columns >					
                        <asp:BoundField DataField="RANGE" ItemStyle-Width="150px" ItemStyle-BorderColor="Black" ItemStyle-BorderWidth="1"  />
                        <asp:BoundField DataField="DEV_QTY_%" ItemStyle-Width="150px" ItemStyle-BorderColor="Black" ItemStyle-BorderWidth="1"  />
                        <asp:BoundField DataField="CUR_QTY_%" ItemStyle-Width="150px" ItemStyle-BorderColor="Black" ItemStyle-BorderWidth="1"  />
                        <asp:BoundField DataField="A-B" ItemStyle-Width="150px" ItemStyle-BorderColor="Black" ItemStyle-BorderWidth="1"  />
                        <asp:BoundField DataField="A/B" ItemStyle-Width="150px" ItemStyle-BorderColor="Black" ItemStyle-BorderWidth="1"  />
                        <asp:BoundField DataField="Ln(A/B)" ItemStyle-Width="150px" ItemStyle-BorderColor="Black" ItemStyle-BorderWidth="1"  />
                        <asp:BoundField DataField="INDEX" ItemStyle-Width="150px" ItemStyle-BorderColor="Black" ItemStyle-BorderWidth="1"  />
                        </Columns>
                        </asp:GridView> 
                        </asp:TableCell>
                        </asp:TableRow>    
                         </asp:Table>
                            </asp:TableCell>
                            </asp:TableRow>

                            </asp:Table>  
                       </ContentTemplate></asp:UpdatePanel></asp:Content>