<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/CreditScoring.Master" AutoEventWireup="true" CodeBehind="RatingReport_12.aspx.cs" Inherits="GSB.Report.Rating.RatingReport_12" %>
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
                    <asp:TableHeaderCell ColumnSpan="2"><u>เครื่องมือค้นหา</u> : รายงานวิเคราะห์การกระจายตัวของระดับความเสี่ยงของโครงการหรือผู้กู้ </asp:TableHeaderCell>
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
                    <asp:Table ID="ResultTable" runat="server" CellPadding="0" CellSpacing="0" 
                        CssClass="Result" BorderColor="Black" BorderWidth="1px">
                <asp:TableRow>
                    <asp:TableCell CssClass="HeaderRow"><u>แสดงผลการค้นหา</u> : </asp:TableCell></asp:TableRow>
                    <asp:TableRow Width="1100px">
                    <asp:TableCell CssClass="TableRow" Width="100%">
                         <div style="overflow:scroll;table-layout:fixed" align="center">
                          <asp:Table ID="HeadTxtLoan" runat="server" CellPadding="0" CellSpacing="0" Width="100%" BorderWidth="1" BorderColor="Black">
                            <asp:TableRow ID="TableRow1" runat="server">
                                <asp:TableCell Width="17%" RowSpan="2" BorderColor="Black" BorderWidth="1">ฝ่ายงาน<br />(Optimist/Division)</asp:TableCell>
                                <asp:TableCell Width="5%" RowSpan="2" BorderColor="Black" BorderWidth="1">จำนวน</asp:TableCell>
                                <asp:TableCell Width="70%" ColumnSpan = "14" BorderColor="Black" BorderWidth="1">Obligor Rating</asp:TableCell>
                                <asp:TableCell Width="8%" RowSpan="2" BorderColor="Black" BorderWidth="1">Grand Total</asp:TableCell>
                            </asp:TableRow>                            
                            <asp:TableRow ID="TableRow2" runat="server" BorderWidth="1" BorderColor="Black">
                                <asp:TableCell Width="5%" BorderColor="Black" BorderWidth="1">1</asp:TableCell>
                                <asp:TableCell Width="5%" BorderColor="Black" BorderWidth="1">2</asp:TableCell>
                                <asp:TableCell Width="5%" BorderColor="Black" BorderWidth="1">3</asp:TableCell>
                                <asp:TableCell Width="5%" BorderColor="Black" BorderWidth="1">4</asp:TableCell>
                                <asp:TableCell Width="5%" BorderColor="Black" BorderWidth="1">5</asp:TableCell>
                                <asp:TableCell Width="5%" BorderColor="Black" BorderWidth="1">6</asp:TableCell>
                                <asp:TableCell Width="5%" BorderColor="Black" BorderWidth="1">7</asp:TableCell>
                                <asp:TableCell Width="5%" BorderColor="Black" BorderWidth="1">8</asp:TableCell>
                                <asp:TableCell Width="5%" BorderColor="Black" BorderWidth="1">9</asp:TableCell>
                                <asp:TableCell Width="5%" BorderColor="Black" BorderWidth="1">10</asp:TableCell>
                                <asp:TableCell Width="5%" BorderColor="Black" BorderWidth="1">11</asp:TableCell>
                                <asp:TableCell Width="5%" BorderColor="Black" BorderWidth="1">12</asp:TableCell>
                                <asp:TableCell Width="5%" BorderColor="Black" BorderWidth="1">13</asp:TableCell>
                                <asp:TableCell Width="5%" BorderColor="Black" BorderWidth="1" >14</asp:TableCell>
                            </asp:TableRow>
                          <asp:TableRow ID="TableRow3" runat="server" Width="1100px">
                                    <asp:TableCell ColumnSpan="17" BorderWidth="0"  Width="1100px">                                        
                        <asp:GridView ID="gvTable" runat="server" ShowHeader="false"  AutoGenerateColumns="false" EmptyDataText="No records found"
                        PagerSettings-Mode="NumericFirstLast" AllowPaging="True" OnPageIndexChanging="gvTable_PageChanging" PageSize="20" 
                        Font-Size="Small" Font-Bold="False" Font-Italic="True" Font-Names="Tahoma" BorderColor="Black" BorderWidth="1" Width="100%">
                        <Columns>					
                        <asp:BoundField DataField="DIVISION" ItemStyle-Width="17%" ItemStyle-BorderColor="Black" ItemStyle-BorderWidth="1" />
                        <asp:BoundField DataField="จำนวน" ItemStyle-Width="5%" ItemStyle-BorderColor="Black" ItemStyle-BorderWidth="1" />
                        <asp:BoundField DataField="1" ItemStyle-Width="5%" ItemStyle-BorderColor="Black" ItemStyle-BorderWidth="1" />
                        <asp:BoundField DataField="2" ItemStyle-Width="5%" ItemStyle-BorderColor="Black" ItemStyle-BorderWidth="1" />
                        <asp:BoundField DataField="3" ItemStyle-Width="5%" ItemStyle-BorderColor="Black" ItemStyle-BorderWidth="1" />
                        <asp:BoundField DataField="4" ItemStyle-Width="5%" ItemStyle-BorderColor="Black" ItemStyle-BorderWidth="1" />
                        <asp:BoundField DataField="5" ItemStyle-Width="5%" ItemStyle-BorderColor="Black" ItemStyle-BorderWidth="1" />
                        <asp:BoundField DataField="6" ItemStyle-Width="5%" ItemStyle-BorderColor="Black" ItemStyle-BorderWidth="1" />
                        <asp:BoundField DataField="7" ItemStyle-Width="5%" ItemStyle-BorderColor="Black" ItemStyle-BorderWidth="1" />
                        <asp:BoundField DataField="8" ItemStyle-Width="5%" ItemStyle-BorderColor="Black" ItemStyle-BorderWidth="1" />
                        <asp:BoundField DataField="9" ItemStyle-Width="5%" ItemStyle-BorderColor="Black" ItemStyle-BorderWidth="1" />
                        <asp:BoundField DataField="10" ItemStyle-Width="5%" ItemStyle-BorderColor="Black" ItemStyle-BorderWidth="1" />
                        <asp:BoundField DataField="11" ItemStyle-Width="5%" ItemStyle-BorderColor="Black" ItemStyle-BorderWidth="1" />
                        <asp:BoundField DataField="12" ItemStyle-Width="5%" ItemStyle-BorderColor="Black" ItemStyle-BorderWidth="1" />
                        <asp:BoundField DataField="13" ItemStyle-Width="5%" ItemStyle-BorderColor="Black" ItemStyle-BorderWidth="1" />
                        <asp:BoundField DataField="14" ItemStyle-Width="5%" ItemStyle-BorderColor="Black" ItemStyle-BorderWidth="1" />
                        <asp:BoundField DataField="GRAND_TOT" ItemStyle-Width="8%" ItemStyle-BorderColor="Black" ItemStyle-BorderWidth="1" />
                        </Columns>
                        </asp:GridView> 
                         </asp:TableCell>
                        </asp:TableRow>    
                         </asp:Table>
                        </div>
                           </asp:TableCell>
                            </asp:TableRow>

                    <asp:TableRow Width="1100px">
                    <asp:TableCell CssClass="TableRow" Width="100%">
                         <div style="overflow:scroll;Width="100%";table-layout:fixed" align="center">
                          <asp:Table ID="Table2" runat="server" CellPadding="0" CellSpacing="0" Width="100%" BorderWidth="1" BorderColor="Black">
                            <asp:TableRow ID="TableRow4" runat="server" Width="100%">
                                <asp:TableCell Width="15%" RowSpan="2" BorderColor="Black" BorderWidth="1">ฝ่ายงาน<br />(Optimist/Division)</asp:TableCell>
                                <asp:TableCell Width="5%" RowSpan="2" BorderColor="Black" BorderWidth="1">จำนวน</asp:TableCell>
                                <asp:TableCell Width="70%" ColumnSpan = "6" BorderColor="Black" BorderWidth="1">Security Coverage Rating</asp:TableCell>
                                <asp:TableCell Width="10%" RowSpan="2" BorderColor="Black" BorderWidth="1">Grand Total</asp:TableCell>
                            </asp:TableRow>                            
                            <asp:TableRow ID="TableRow5" runat="server" Width="100%" BorderWidth="1" BorderColor="Black">
                                <asp:TableCell Width="5%" BorderColor="Black" BorderWidth="1">A</asp:TableCell>
                                <asp:TableCell Width="5%" BorderColor="Black" BorderWidth="1">B</asp:TableCell>
                                <asp:TableCell Width="5%" BorderColor="Black" BorderWidth="1">C</asp:TableCell>
                                <asp:TableCell Width="5%" BorderColor="Black" BorderWidth="1">D</asp:TableCell>
                                <asp:TableCell Width="5%" BorderColor="Black" BorderWidth="1">E</asp:TableCell>
                                <asp:TableCell Width="5%" BorderColor="Black" BorderWidth="1">F</asp:TableCell>                            </asp:TableRow>
                          <asp:TableRow ID="TableRow6" runat="server" Width="100%">
                                    <asp:TableCell ColumnSpan="9" BorderWidth="1"  Width="100%">                                        
                        <asp:GridView ID="GridView1" runat="server" ShowHeader="false"  AutoGenerateColumns="false" EmptyDataText="No records found"
                        PagerSettings-Mode="NumericFirstLast" AllowPaging="True" OnPageIndexChanging="gvTable_PageChanging2" PageSize="20" 
                        Font-Size="Small" Font-Bold="False" Font-Italic="True" Font-Names="Tahoma" BorderColor="Black" BorderWidth="1" Width="100%">
                        <Columns>					
                        <asp:BoundField DataField="DIVISION" ItemStyle-Width="15%" ItemStyle-BorderColor="Black" ItemStyle-BorderWidth="1" />
                        <asp:BoundField DataField="จำนวน" ItemStyle-Width="5%" ItemStyle-BorderColor="Black" ItemStyle-BorderWidth="1" />
                        <asp:BoundField DataField="A" ItemStyle-Width="5%" ItemStyle-BorderColor="Black" ItemStyle-BorderWidth="1" />
                        <asp:BoundField DataField="B" ItemStyle-Width="5%" ItemStyle-BorderColor="Black" ItemStyle-BorderWidth="1" />
                        <asp:BoundField DataField="C" ItemStyle-Width="5%" ItemStyle-BorderColor="Black" ItemStyle-BorderWidth="1" />
                        <asp:BoundField DataField="D" ItemStyle-Width="5%" ItemStyle-BorderColor="Black" ItemStyle-BorderWidth="1" />
                        <asp:BoundField DataField="E" ItemStyle-Width="5%" ItemStyle-BorderColor="Black" ItemStyle-BorderWidth="1" />
                        <asp:BoundField DataField="F" ItemStyle-Width="5%" ItemStyle-BorderColor="Black" ItemStyle-BorderWidth="1" />
                        <asp:BoundField DataField="GRAND_TOT" ItemStyle-Width="10%" ItemStyle-BorderColor="Black" ItemStyle-BorderWidth="1" />
                        </Columns>
                        </asp:GridView> 
                         </asp:TableCell>
                        </asp:TableRow>    
                         </asp:Table>
                        </div>
                           </asp:TableCell>
                            </asp:TableRow>

                    <asp:TableRow Width="1100px">
                    <asp:TableCell CssClass="TableRow" Width="100%">
                         <div style="overflow:scroll;Width="100%";table-layout:fixed" align="center">
                          <asp:Table ID="Table3" runat="server" CellPadding="0" CellSpacing="0" Width="100%" BorderWidth="1" BorderColor="Black">
                            <asp:TableRow ID="TableRow7" runat="server" Width="100%">
                                <asp:TableCell Width="15%" RowSpan="2" BorderColor="Black" BorderWidth="1">ฝ่ายงาน<br />(Optimist/Division)</asp:TableCell>
                                <asp:TableCell Width="5%" RowSpan="2" BorderColor="Black" BorderWidth="1">จำนวน</asp:TableCell>
                                <asp:TableCell Width="70%" ColumnSpan = "6" BorderColor="Black" BorderWidth="1">FS Class</asp:TableCell>
                                <asp:TableCell Width="10%" RowSpan="2" BorderColor="Black" BorderWidth="1">Grand Total</asp:TableCell>
                            </asp:TableRow>                            
                            <asp:TableRow ID="TableRow8" runat="server" Width="100%" BorderWidth="1" BorderColor="Black">
                                <asp:TableCell Width="5%" BorderColor="Black" BorderWidth="1">1</asp:TableCell>
                                <asp:TableCell Width="5%" BorderColor="Black" BorderWidth="1">2</asp:TableCell>
                                <asp:TableCell Width="5%" BorderColor="Black" BorderWidth="1">3</asp:TableCell>
                                <asp:TableCell Width="5%" BorderColor="Black" BorderWidth="1">4</asp:TableCell>
                                <asp:TableCell Width="5%" BorderColor="Black" BorderWidth="1">5</asp:TableCell>
                                <asp:TableCell Width="5%" BorderColor="Black" BorderWidth="1">6</asp:TableCell>
                            </asp:TableRow>
                          <asp:TableRow ID="TableRow9" runat="server" Width="100%">
                                    <asp:TableCell ColumnSpan="9" BorderWidth="1"  Width="100%">                                        
                        <asp:GridView ID="GridView2" runat="server" ShowHeader="false"  AutoGenerateColumns="false" EmptyDataText="No records found"
                        PagerSettings-Mode="NumericFirstLast" AllowPaging="True" OnPageIndexChanging="gvTable_PageChanging3" PageSize="20" 
                        Font-Size="Small" Font-Bold="False" Font-Italic="True" Font-Names="Tahoma" BorderColor="Black" BorderWidth="1" Width="100%">
                        <Columns>					
                        <asp:BoundField DataField="DIVISION" ItemStyle-Width="15%" ItemStyle-BorderColor="Black" ItemStyle-BorderWidth="1" />
                        <asp:BoundField DataField="จำนวน" ItemStyle-Width="5%" ItemStyle-BorderColor="Black" ItemStyle-BorderWidth="1" />
                        <asp:BoundField DataField="1" ItemStyle-Width="5%" ItemStyle-BorderColor="Black" ItemStyle-BorderWidth="1" />
                        <asp:BoundField DataField="2" ItemStyle-Width="5%" ItemStyle-BorderColor="Black" ItemStyle-BorderWidth="1" />
                        <asp:BoundField DataField="3" ItemStyle-Width="5%" ItemStyle-BorderColor="Black" ItemStyle-BorderWidth="1" />
                        <asp:BoundField DataField="4" ItemStyle-Width="5%" ItemStyle-BorderColor="Black" ItemStyle-BorderWidth="1" />
                        <asp:BoundField DataField="5" ItemStyle-Width="5%" ItemStyle-BorderColor="Black" ItemStyle-BorderWidth="1" />
                        <asp:BoundField DataField="6" ItemStyle-Width="5%" ItemStyle-BorderColor="Black" ItemStyle-BorderWidth="1" />
                         <asp:BoundField DataField="GRAND_TOT" ItemStyle-Width="10%" ItemStyle-BorderColor="Black" ItemStyle-BorderWidth="1" />
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
