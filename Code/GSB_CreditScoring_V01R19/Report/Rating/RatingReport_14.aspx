<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/CreditScoring.Master" AutoEventWireup="true" CodeBehind="RatingReport_14.aspx.cs" Inherits="GSB.Report.Rating.RatingReport_14" %>
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
                    <asp:TableHeaderCell ColumnSpan="2"><u>เครื่องมือค้นหา</u> : แสดงการเปลี่ยนผ่านเครดิตเรตติ้ง </asp:TableHeaderCell>
                </asp:TableHeaderRow>
                <asp:TableRow>
                    <asp:TableCell CssClass="subject">Definition</asp:TableCell>
                    <asp:TableCell CssClass="detail">
                        <asp:DropDownList ID="ddlModel" runat="server" DataTextField="MODEL_NAME" DataValueField="MODEL_NAME" />
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell CssClass="subject">ข้อมูลการใช้งาน ณ สิ้นเดือน</asp:TableCell>
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
                        <asp:Table ID="HeadTxtLoan" runat="server" CellPadding="0" CellSpacing="0">
                             <asp:TableRow ID="TableRow2" runat="server">
                                <asp:TableCell Width="100px" BorderColor="Black" BorderWidth="1" Font-Bold="true" HorizontalAlign="Center">OBLIG_RATING</asp:TableCell>
                                <asp:TableCell Width="35px" BorderColor="Black" BorderWidth="1" Font-Bold="true" HorizontalAlign="Center">1</asp:TableCell>
                                <asp:TableCell Width="35px" BorderColor="Black" BorderWidth="1" Font-Bold="true" HorizontalAlign="Center">2</asp:TableCell>
                                <asp:TableCell Width="35px" BorderColor="Black" BorderWidth="1" Font-Bold="true" HorizontalAlign="Center">3</asp:TableCell>
                                <asp:TableCell Width="35px" BorderColor="Black" BorderWidth="1" Font-Bold="true" HorizontalAlign="Center">4</asp:TableCell>
                                <asp:TableCell Width="35px" BorderColor="Black" BorderWidth="1" Font-Bold="true" HorizontalAlign="Center">5</asp:TableCell>
                                <asp:TableCell Width="35px" BorderColor="Black" BorderWidth="1" Font-Bold="true" HorizontalAlign="Center">6</asp:TableCell>
                                <asp:TableCell Width="35px" BorderColor="Black" BorderWidth="1" Font-Bold="true" HorizontalAlign="Center">7</asp:TableCell>
                                <asp:TableCell Width="35px" BorderColor="Black" BorderWidth="1" Font-Bold="true" HorizontalAlign="Center">8</asp:TableCell>
                                <asp:TableCell Width="35px" BorderColor="Black" BorderWidth="1" Font-Bold="true" HorizontalAlign="Center">9</asp:TableCell>
                                <asp:TableCell Width="35px" BorderColor="Black" BorderWidth="1" Font-Bold="true" HorizontalAlign="Center">10</asp:TableCell>
                                <asp:TableCell Width="35px" BorderColor="Black" BorderWidth="1" Font-Bold="true" HorizontalAlign="Center">11</asp:TableCell>
                                <asp:TableCell Width="35px" BorderColor="Black" BorderWidth="1" Font-Bold="true" HorizontalAlign="Center">12</asp:TableCell>
                                <asp:TableCell Width="35px" BorderColor="Black" BorderWidth="1" Font-Bold="true" HorizontalAlign="Center">13</asp:TableCell>
                                <asp:TableCell Width="35px" BorderColor="Black" BorderWidth="1" Font-Bold="true" HorizontalAlign="Center">14</asp:TableCell>
                                <asp:TableCell Width="55px" BorderColor="Black" BorderWidth="1" Font-Bold="true" HorizontalAlign="Center">รวม</asp:TableCell>
                            </asp:TableRow>
                          <asp:TableRow ID="TableRow3" runat="server">
                                    <asp:TableCell ColumnSpan="16" BorderWidth="0">                                        
                        <asp:GridView ID="gvTable" runat="server" ShowHeader="False" AutoGenerateColumns="false"
                        PagerSettings-Mode="NumericFirstLast" AllowPaging="True" OnPageIndexChanging="gvTable_PageChanging" PageSize="15">
                        <Columns >					
                        <asp:BoundField DataField="OBLIG_RATING" ItemStyle-Width="100px" ItemStyle-BorderColor="Black" ItemStyle-BorderWidth="1"  />
                        <asp:BoundField DataField="1" ItemStyle-Width="35px" ItemStyle-BorderColor="Black" ItemStyle-BorderWidth="1"  />
                        <asp:BoundField DataField="2" ItemStyle-Width="35px" ItemStyle-BorderColor="Black" ItemStyle-BorderWidth="1"  />
                        <asp:BoundField DataField="3" ItemStyle-Width="35px" ItemStyle-BorderColor="Black" ItemStyle-BorderWidth="1"  />
                        <asp:BoundField DataField="4" ItemStyle-Width="35px" ItemStyle-BorderColor="Black" ItemStyle-BorderWidth="1"  />
                        <asp:BoundField DataField="5" ItemStyle-Width="35px" ItemStyle-BorderColor="Black" ItemStyle-BorderWidth="1"  />
                        <asp:BoundField DataField="6" ItemStyle-Width="35px" ItemStyle-BorderColor="Black" ItemStyle-BorderWidth="1"  />
                        <asp:BoundField DataField="7" ItemStyle-Width="35px" ItemStyle-BorderColor="Black" ItemStyle-BorderWidth="1"  />
                        <asp:BoundField DataField="8" ItemStyle-Width="35px" ItemStyle-BorderColor="Black" ItemStyle-BorderWidth="1"  />
                        <asp:BoundField DataField="9" ItemStyle-Width="35px" ItemStyle-BorderColor="Black" ItemStyle-BorderWidth="1"  />
                        <asp:BoundField DataField="10" ItemStyle-Width="35px" ItemStyle-BorderColor="Black" ItemStyle-BorderWidth="1"  />
                        <asp:BoundField DataField="11" ItemStyle-Width="35px" ItemStyle-BorderColor="Black" ItemStyle-BorderWidth="1"  />
                        <asp:BoundField DataField="12" ItemStyle-Width="35px" ItemStyle-BorderColor="Black" ItemStyle-BorderWidth="1"  />
                        <asp:BoundField DataField="13" ItemStyle-Width="35px" ItemStyle-BorderColor="Black" ItemStyle-BorderWidth="1"  />
                        <asp:BoundField DataField="14" ItemStyle-Width="35px" ItemStyle-BorderColor="Black" ItemStyle-BorderWidth="1"  />
                        <asp:BoundField DataField="TOTAL" ItemStyle-Width="55px" ItemStyle-BorderColor="Black" ItemStyle-BorderWidth="1"  />
                        </Columns>
                        </asp:GridView> 
                        </asp:TableCell>
                        </asp:TableRow>    
                         </asp:Table>
                            </asp:TableCell>
                            </asp:TableRow>

                    <asp:TableRow>
                    <asp:TableCell CssClass="TableRow">
                        <asp:Table ID="Table2" runat="server" CellPadding="0" CellSpacing="0" >
                             <asp:TableRow ID="TableRow4" runat="server">
                                <asp:TableCell Width="100px" BorderColor="Black" BorderWidth="1" Font-Bold="true" HorizontalAlign="Center">FS_CLASS</asp:TableCell>
                                <asp:TableCell Width="35px" BorderColor="Black" BorderWidth="1" Font-Bold="true" HorizontalAlign="Center">1</asp:TableCell>
                                <asp:TableCell Width="35px" BorderColor="Black" BorderWidth="1" Font-Bold="true" HorizontalAlign="Center">2</asp:TableCell>
                                <asp:TableCell Width="35px" BorderColor="Black" BorderWidth="1" Font-Bold="true" HorizontalAlign="Center">3</asp:TableCell>
                                <asp:TableCell Width="35px" BorderColor="Black" BorderWidth="1" Font-Bold="true" HorizontalAlign="Center">4</asp:TableCell>
                                <asp:TableCell Width="35px" BorderColor="Black" BorderWidth="1" Font-Bold="true" HorizontalAlign="Center">5</asp:TableCell>
                                <asp:TableCell Width="35px" BorderColor="Black" BorderWidth="1" Font-Bold="true" HorizontalAlign="Center">6</asp:TableCell>
                                <asp:TableCell Width="55px" BorderColor="Black" BorderWidth="1" Font-Bold="true" HorizontalAlign="Center">รวม</asp:TableCell>
                            </asp:TableRow>
                          <asp:TableRow ID="TableRow6" runat="server">
                                    <asp:TableCell ColumnSpan="9" BorderWidth="0">                                        
                        <asp:GridView ID="GridView1" runat="server"  ShowHeader="False" AutoGenerateColumns="false"
                        PagerSettings-Mode="NumericFirstLast" AllowPaging="True" OnPageIndexChanging="gvTable_PageChanging2" PageSize="15">
                        <Columns >					
                        <asp:BoundField DataField="FS_CLASS" ItemStyle-Width="100px" ItemStyle-BorderColor="Black" ItemStyle-BorderWidth="1"  />
                        <asp:BoundField DataField="1" ItemStyle-Width="35px" ItemStyle-BorderColor="Black" ItemStyle-BorderWidth="1"  />
                        <asp:BoundField DataField="2" ItemStyle-Width="35px" ItemStyle-BorderColor="Black" ItemStyle-BorderWidth="1"  />
                        <asp:BoundField DataField="3" ItemStyle-Width="35px" ItemStyle-BorderColor="Black" ItemStyle-BorderWidth="1"  />
                        <asp:BoundField DataField="4" ItemStyle-Width="35px" ItemStyle-BorderColor="Black" ItemStyle-BorderWidth="1"  />
                        <asp:BoundField DataField="5" ItemStyle-Width="35px" ItemStyle-BorderColor="Black" ItemStyle-BorderWidth="1"  />
                        <asp:BoundField DataField="6" ItemStyle-Width="35px" ItemStyle-BorderColor="Black" ItemStyle-BorderWidth="1"  />
                        <asp:BoundField DataField="TOTAL" ItemStyle-Width="55px" ItemStyle-BorderColor="Black" ItemStyle-BorderWidth="1"  />
                        </Columns>
                        </asp:GridView> 
                        </asp:TableCell>
                        </asp:TableRow>    
                         </asp:Table>
                            </asp:TableCell>
                            </asp:TableRow>

                    <asp:TableRow>
                    <asp:TableCell CssClass="TableRow">
                        <asp:Table ID="Table5" runat="server" CellPadding="0" CellSpacing="0" >
                             <asp:TableRow ID="TableRow13" runat="server">
                                <asp:TableCell Width="100px" BorderColor="Black" BorderWidth="1" Font-Bold="true" HorizontalAlign="Center">OBLIG_RATING</asp:TableCell>
                                <asp:TableCell Width="35px" BorderColor="Black" BorderWidth="1" Font-Bold="true" HorizontalAlign="Center">1</asp:TableCell>
                                <asp:TableCell Width="35px" BorderColor="Black" BorderWidth="1" Font-Bold="true" HorizontalAlign="Center">2</asp:TableCell>
                                <asp:TableCell Width="35px" BorderColor="Black" BorderWidth="1" Font-Bold="true" HorizontalAlign="Center">3</asp:TableCell>
                                <asp:TableCell Width="35px" BorderColor="Black" BorderWidth="1" Font-Bold="true" HorizontalAlign="Center">4</asp:TableCell>
                                <asp:TableCell Width="35px" BorderColor="Black" BorderWidth="1" Font-Bold="true" HorizontalAlign="Center">5</asp:TableCell>
                                <asp:TableCell Width="35px" BorderColor="Black" BorderWidth="1" Font-Bold="true" HorizontalAlign="Center">6</asp:TableCell>
                                <asp:TableCell Width="35px" BorderColor="Black" BorderWidth="1" Font-Bold="true" HorizontalAlign="Center">7</asp:TableCell>
                                <asp:TableCell Width="35px" BorderColor="Black" BorderWidth="1" Font-Bold="true" HorizontalAlign="Center">8</asp:TableCell>
                                <asp:TableCell Width="35px" BorderColor="Black" BorderWidth="1" Font-Bold="true" HorizontalAlign="Center">9</asp:TableCell>
                                <asp:TableCell Width="35px" BorderColor="Black" BorderWidth="1" Font-Bold="true" HorizontalAlign="Center">10</asp:TableCell>
                                <asp:TableCell Width="35px" BorderColor="Black" BorderWidth="1" Font-Bold="true" HorizontalAlign="Center">11</asp:TableCell>
                                <asp:TableCell Width="35px" BorderColor="Black" BorderWidth="1" Font-Bold="true" HorizontalAlign="Center">12</asp:TableCell>
                                <asp:TableCell Width="35px" BorderColor="Black" BorderWidth="1" Font-Bold="true" HorizontalAlign="Center">13</asp:TableCell>
                                <asp:TableCell Width="35px" BorderColor="Black" BorderWidth="1" Font-Bold="true" HorizontalAlign="Center">14</asp:TableCell>
                                <asp:TableCell Width="55px" BorderColor="Black" BorderWidth="1" Font-Bold="true" HorizontalAlign="Center">รวม</asp:TableCell>
                            </asp:TableRow>
                          <asp:TableRow ID="TableRow15" runat="server">
                                    <asp:TableCell ColumnSpan="16" BorderWidth="0">                                        
                        <asp:GridView ID="GridView4" runat="server" ShowHeader="False" AutoGenerateColumns="false"
                        PagerSettings-Mode="NumericFirstLast" AllowPaging="True" OnPageIndexChanging="gvTable_PageChanging3" PageSize="15">
                        <Columns >					
                        <asp:BoundField DataField="OBLIG_RATING" ItemStyle-Width="100px" ItemStyle-BorderColor="Black" ItemStyle-BorderWidth="1"  />
                        <asp:BoundField DataField="1" ItemStyle-Width="35px" ItemStyle-BorderColor="Black" ItemStyle-BorderWidth="1"  />
                        <asp:BoundField DataField="2" ItemStyle-Width="35px" ItemStyle-BorderColor="Black" ItemStyle-BorderWidth="1"  />
                        <asp:BoundField DataField="3" ItemStyle-Width="35px" ItemStyle-BorderColor="Black" ItemStyle-BorderWidth="1"  />
                        <asp:BoundField DataField="4" ItemStyle-Width="35px" ItemStyle-BorderColor="Black" ItemStyle-BorderWidth="1"  />
                        <asp:BoundField DataField="5" ItemStyle-Width="35px" ItemStyle-BorderColor="Black" ItemStyle-BorderWidth="1"  />
                        <asp:BoundField DataField="6" ItemStyle-Width="35px" ItemStyle-BorderColor="Black" ItemStyle-BorderWidth="1"  />
                        <asp:BoundField DataField="7" ItemStyle-Width="35px" ItemStyle-BorderColor="Black" ItemStyle-BorderWidth="1"  />
                        <asp:BoundField DataField="8" ItemStyle-Width="35px" ItemStyle-BorderColor="Black" ItemStyle-BorderWidth="1"  />
                        <asp:BoundField DataField="9" ItemStyle-Width="35px" ItemStyle-BorderColor="Black" ItemStyle-BorderWidth="1"  />
                        <asp:BoundField DataField="10" ItemStyle-Width="35px" ItemStyle-BorderColor="Black" ItemStyle-BorderWidth="1"  />
                        <asp:BoundField DataField="11" ItemStyle-Width="35px" ItemStyle-BorderColor="Black" ItemStyle-BorderWidth="1"  />
                        <asp:BoundField DataField="12" ItemStyle-Width="35px" ItemStyle-BorderColor="Black" ItemStyle-BorderWidth="1"  />
                        <asp:BoundField DataField="13" ItemStyle-Width="35px" ItemStyle-BorderColor="Black" ItemStyle-BorderWidth="1"  />
                        <asp:BoundField DataField="14" ItemStyle-Width="35px" ItemStyle-BorderColor="Black" ItemStyle-BorderWidth="1"  />
                        <asp:BoundField DataField="TOTAL" ItemStyle-Width="55px" ItemStyle-BorderColor="Black" ItemStyle-BorderWidth="1"  />
                        </Columns>
                        </asp:GridView> 
                        </asp:TableCell>
                        </asp:TableRow>    
                         </asp:Table>
                            </asp:TableCell>
                            </asp:TableRow>

                    <asp:TableRow>
                    <asp:TableCell CssClass="TableRow">
                        <asp:Table ID="Table3" runat="server" CellPadding="0" CellSpacing="0">
                             <asp:TableRow ID="TableRow7" runat="server">
                                <asp:TableCell Width="100px" BorderColor="Black" BorderWidth="1" Font-Bold="true" HorizontalAlign="Center">FS_CLASS</asp:TableCell>
                                <asp:TableCell Width="35px" BorderColor="Black" BorderWidth="1" Font-Bold="true" HorizontalAlign="Center">1</asp:TableCell>
                                <asp:TableCell Width="35px" BorderColor="Black" BorderWidth="1" Font-Bold="true" HorizontalAlign="Center">2</asp:TableCell>
                                <asp:TableCell Width="35px" BorderColor="Black" BorderWidth="1" Font-Bold="true" HorizontalAlign="Center">3</asp:TableCell>
                                <asp:TableCell Width="35px" BorderColor="Black" BorderWidth="1" Font-Bold="true" HorizontalAlign="Center">4</asp:TableCell>
                                <asp:TableCell Width="35px" BorderColor="Black" BorderWidth="1" Font-Bold="true" HorizontalAlign="Center">5</asp:TableCell>
                                <asp:TableCell Width="35px" BorderColor="Black" BorderWidth="1" Font-Bold="true" HorizontalAlign="Center">6</asp:TableCell>
                                <asp:TableCell Width="55px" BorderColor="Black" BorderWidth="1" Font-Bold="true" HorizontalAlign="Center">รวม</asp:TableCell>
                            </asp:TableRow>
                          <asp:TableRow ID="TableRow9" runat="server">
                                    <asp:TableCell ColumnSpan="9" BorderWidth="0">                                        
                        <asp:GridView ID="GridView2" runat="server"  ShowHeader="False" AutoGenerateColumns="false"
                        PagerSettings-Mode="NumericFirstLast" AllowPaging="True" OnPageIndexChanging="gvTable_PageChanging4" PageSize="15">
                        <Columns >					
                        <asp:BoundField DataField="FS_CLASS" ItemStyle-Width="100px" ItemStyle-BorderColor="Black" ItemStyle-BorderWidth="1"  />
                        <asp:BoundField DataField="1" ItemStyle-Width="35px" ItemStyle-BorderColor="Black" ItemStyle-BorderWidth="1"  />
                        <asp:BoundField DataField="2" ItemStyle-Width="35px" ItemStyle-BorderColor="Black" ItemStyle-BorderWidth="1"  />
                        <asp:BoundField DataField="3" ItemStyle-Width="35px" ItemStyle-BorderColor="Black" ItemStyle-BorderWidth="1"  />
                        <asp:BoundField DataField="4" ItemStyle-Width="35px" ItemStyle-BorderColor="Black" ItemStyle-BorderWidth="1"  />
                        <asp:BoundField DataField="5" ItemStyle-Width="35px" ItemStyle-BorderColor="Black" ItemStyle-BorderWidth="1"  />
                        <asp:BoundField DataField="6" ItemStyle-Width="35px" ItemStyle-BorderColor="Black" ItemStyle-BorderWidth="1"  />
                        <asp:BoundField DataField="TOTAL" ItemStyle-Width="55px" ItemStyle-BorderColor="Black" ItemStyle-BorderWidth="1"  />
                        </Columns>
                        </asp:GridView> 
                        </asp:TableCell>
                        </asp:TableRow>    
                         </asp:Table>
                            </asp:TableCell>
                            </asp:TableRow>

                            </asp:Table>  
                       </ContentTemplate></asp:UpdatePanel></asp:Content>