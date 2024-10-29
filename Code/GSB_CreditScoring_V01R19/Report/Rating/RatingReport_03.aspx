<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/CreditScoring.Master" AutoEventWireup="true" CodeBehind="RatingReport_03.aspx.cs" Inherits="GSB.Report.Rating.RatingReport_03" %>
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
                    <asp:TableHeaderCell ColumnSpan="2"><u>เครื่องมือค้นหา</u> : รายงานแสดงการกระจายตัวของการผิดนัดชำระหนี้ </asp:TableHeaderCell>
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
                             <asp:TableRow ID="TableRow2" runat="server">
                                <asp:TableCell Width="60px" >อันดับเครดิตผู้กู้จากแบบจำลอง<br />(Obligor Rating)</asp:TableCell>
                                <asp:TableCell Width="50px" >จำนวนรายที่จัดชั้นปกติ</asp:TableCell>
                                <asp:TableCell Width="50px" >จำนวนรายที่จัดชั้นกล่าวถึงเป็นพิเศษ</asp:TableCell>
                                <asp:TableCell Width="50px" >จำนวนรายที่จัดชั้นต่ำกว่ามาตรฐาน</asp:TableCell>
                                <asp:TableCell Width="50px" >จำนวนรายที่จัดชั้นสงสัย</asp:TableCell>
                                <asp:TableCell Width="50px" >จำนวนรายที่จัดชั้นสงสัยจะสูญ</asp:TableCell>
                                <asp:TableCell Width="50px" >จำนวนรายที่จัดชั้นเป็ฯหนี้สูญ</asp:TableCell>
                                <asp:TableCell Width="40px" >รวม</asp:TableCell>
                                <asp:TableCell Width="40px" >รวม (%)</asp:TableCell>
                                <asp:TableCell Width="40px" >NPLs (%)</asp:TableCell>
                                <asp:TableCell Width="40px" >Default (%)</asp:TableCell>
                            </asp:TableRow>
                          <asp:TableRow ID="TableRow3" runat="server">
                                    <asp:TableCell ColumnSpan="11" BorderWidth="0">                                        
                        <asp:GridView ID="gvTable" runat="server" CssClass="GridView" ShowHeader="False" AutoGenerateColumns="false"
                        PagerSettings-Mode="NumericFirstLast" AllowPaging="True" OnPageIndexChanging="gvTable_PageChanging" PageSize="15">
                        <Columns >					
                        <asp:BoundField DataField="OBLIG_RATING" ItemStyle-Width="60px" />
                        <asp:BoundField DataField="P" ItemStyle-Width="50px" />
                        <asp:BoundField DataField="SM" ItemStyle-Width="50px" />
                        <asp:BoundField DataField="SS" ItemStyle-Width="50px" />
                        <asp:BoundField DataField="D" ItemStyle-Width="50px" />
                        <asp:BoundField DataField="DL" ItemStyle-Width="50px" />
                        <asp:BoundField DataField="L" ItemStyle-Width="50px" />
                        <asp:BoundField DataField="Total" ItemStyle-Width="40px" />
                        <asp:BoundField DataField="Total(%)" ItemStyle-Width="40px" />
                        <asp:BoundField DataField="NPLs(%)" ItemStyle-Width="40px" />
                        <asp:BoundField DataField="Default(%)" ItemStyle-Width="40px" />
                        </Columns>
                        </asp:GridView> 
                        </asp:TableCell>
                        </asp:TableRow>    
                         </asp:Table>
                            </asp:TableCell>
                            </asp:TableRow>

                    <asp:TableRow>
                    <asp:TableCell CssClass="TableRow">
                        <asp:Table ID="Table2" runat="server" CellPadding="0" CellSpacing="0" CssClass="ExpandTable">
                             <asp:TableRow ID="TableRow1" runat="server">
                                <asp:TableCell Width="60px" >อันดับเครดิตรวม<br />(Obligor Rating)</asp:TableCell>
                                <asp:TableCell Width="50px" >จำนวนรายที่จัดชั้นปกติ</asp:TableCell>
                                <asp:TableCell Width="50px" >จำนวนรายที่จัดชั้นกล่าวถึงเป็นพิเศษ</asp:TableCell>
                                <asp:TableCell Width="50px" >จำนวนรายที่จัดชั้นต่ำกว่ามาตรฐาน</asp:TableCell>
                                <asp:TableCell Width="50px" >จำนวนรายที่จัดชั้นสงสัย</asp:TableCell>
                                <asp:TableCell Width="50px" >จำนวนรายที่จัดชั้นสงสัยจะสูญ</asp:TableCell>
                                <asp:TableCell Width="50px" >จำนวนรายที่จัดชั้นเป็ฯหนี้สูญ</asp:TableCell>
                                <asp:TableCell Width="40px" >รวม</asp:TableCell>
                                <asp:TableCell Width="40px" >รวม (%)</asp:TableCell>
                                <asp:TableCell Width="40px" >NPLs (%)</asp:TableCell>
                                <asp:TableCell Width="40px" >Default (%)</asp:TableCell>
                            </asp:TableRow>
                          <asp:TableRow ID="TableRow6" runat="server">
                                    <asp:TableCell ColumnSpan="11" BorderWidth="0">                                        
                        <asp:GridView ID="GridView1" runat="server" CssClass="GridView" ShowHeader="False" AutoGenerateColumns="false"
                        PagerSettings-Mode="NumericFirstLast" AllowPaging="True" OnPageIndexChanging="gvTable_PageChanging2" PageSize="15">
                        <Columns >					
                        <asp:BoundField DataField="OA_RATING" ItemStyle-Width="60px" />
                        <asp:BoundField DataField="P" ItemStyle-Width="50px" />
                        <asp:BoundField DataField="SM" ItemStyle-Width="50px" />
                        <asp:BoundField DataField="SS" ItemStyle-Width="50px" />
                        <asp:BoundField DataField="D" ItemStyle-Width="50px" />
                        <asp:BoundField DataField="DL" ItemStyle-Width="50px" />
                        <asp:BoundField DataField="L" ItemStyle-Width="50px" />
                        <asp:BoundField DataField="Total" ItemStyle-Width="40px" />
                        <asp:BoundField DataField="Total(%)" ItemStyle-Width="40px" />
                        <asp:BoundField DataField="NPLs(%)" ItemStyle-Width="40px" />
                        <asp:BoundField DataField="Default(%)" ItemStyle-Width="40px" />
                        </Columns>
                        </asp:GridView> 
                        </asp:TableCell>
                        </asp:TableRow>    
                         </asp:Table>
                            </asp:TableCell>
                            </asp:TableRow>

                    <asp:TableRow>
                    <asp:TableCell CssClass="TableRow">
                        <asp:Table ID="Table5" runat="server" CellPadding="0" CellSpacing="0" CssClass="ExpandTable">
                             <asp:TableRow ID="TableRow4" runat="server">
                                <asp:TableCell Width="60px" >อันดับความเสี่ยงด้านหลักประกัน<br />(Security Coverage Rating</asp:TableCell>
                                <asp:TableCell Width="50px" >จำนวนรายที่จัดชั้นปกติ</asp:TableCell>
                                <asp:TableCell Width="50px" >จำนวนรายที่จัดชั้นกล่าวถึงเป็นพิเศษ</asp:TableCell>
                                <asp:TableCell Width="50px" >จำนวนรายที่จัดชั้นต่ำกว่ามาตรฐาน</asp:TableCell>
                                <asp:TableCell Width="50px" >จำนวนรายที่จัดชั้นสงสัย</asp:TableCell>
                                <asp:TableCell Width="50px" >จำนวนรายที่จัดชั้นสงสัยจะสูญ</asp:TableCell>
                                <asp:TableCell Width="50px" >จำนวนรายที่จัดชั้นเป็ฯหนี้สูญ</asp:TableCell>
                                <asp:TableCell Width="40px" >รวม</asp:TableCell>
                                <asp:TableCell Width="40px" >รวม (%)</asp:TableCell>
                                <asp:TableCell Width="40px" >NPLs (%)</asp:TableCell>
                                <asp:TableCell Width="40px" >Default (%)</asp:TableCell>
                            </asp:TableRow>
                          <asp:TableRow ID="TableRow15" runat="server">
                                    <asp:TableCell ColumnSpan="11" BorderWidth="0">                                        
                        <asp:GridView ID="GridView4" runat="server" CssClass="GridView" ShowHeader="False" AutoGenerateColumns="false"
                        PagerSettings-Mode="NumericFirstLast" AllowPaging="True" OnPageIndexChanging="gvTable_PageChanging3" PageSize="15">
                        <Columns >					
                        <asp:BoundField DataField="CO_RATING" ItemStyle-Width="60px" />
                        <asp:BoundField DataField="P" ItemStyle-Width="50px" />
                        <asp:BoundField DataField="SM" ItemStyle-Width="50px" />
                        <asp:BoundField DataField="SS" ItemStyle-Width="50px" />
                        <asp:BoundField DataField="D" ItemStyle-Width="50px" />
                        <asp:BoundField DataField="DL" ItemStyle-Width="50px" />
                        <asp:BoundField DataField="L" ItemStyle-Width="50px" />
                        <asp:BoundField DataField="Total" ItemStyle-Width="40px" />
                        <asp:BoundField DataField="Total(%)" ItemStyle-Width="40px" />
                        <asp:BoundField DataField="NPLs(%)" ItemStyle-Width="40px" />
                        <asp:BoundField DataField="Default(%)" ItemStyle-Width="40px" />
                        </Columns>
                        </asp:GridView> 
                        </asp:TableCell>
                        </asp:TableRow>    
                         </asp:Table>
                            </asp:TableCell>
                            </asp:TableRow>

                    <asp:TableRow>
                    <asp:TableCell CssClass="TableRow">
                        <asp:Table ID="Table3" runat="server" CellPadding="0" CellSpacing="0" CssClass="ExpandTable">
                             <asp:TableRow ID="TableRow5" runat="server">
                                <asp:TableCell Width="60px" >อันดับเครดิตผู้กู้จาก<br />FS Class</asp:TableCell>
                                <asp:TableCell Width="50px" >จำนวนรายที่จัดชั้นปกติ</asp:TableCell>
                                <asp:TableCell Width="50px" >จำนวนรายที่จัดชั้นกล่าวถึงเป็นพิเศษ</asp:TableCell>
                                <asp:TableCell Width="50px" >จำนวนรายที่จัดชั้นต่ำกว่ามาตรฐาน</asp:TableCell>
                                <asp:TableCell Width="50px" >จำนวนรายที่จัดชั้นสงสัย</asp:TableCell>
                                <asp:TableCell Width="50px" >จำนวนรายที่จัดชั้นสงสัยจะสูญ</asp:TableCell>
                                <asp:TableCell Width="50px" >จำนวนรายที่จัดชั้นเป็ฯหนี้สูญ</asp:TableCell>
                                <asp:TableCell Width="40px" >รวม</asp:TableCell>
                                <asp:TableCell Width="40px" >รวม (%)</asp:TableCell>
                                <asp:TableCell Width="40px" >NPLs (%)</asp:TableCell>
                                <asp:TableCell Width="40px" >Default (%)</asp:TableCell>
                            </asp:TableRow>
                          <asp:TableRow ID="TableRow9" runat="server">
                                    <asp:TableCell ColumnSpan="11" BorderWidth="0">                                        
                        <asp:GridView ID="GridView2" runat="server" CssClass="GridView" ShowHeader="False" AutoGenerateColumns="false"
                        PagerSettings-Mode="NumericFirstLast" AllowPaging="True" OnPageIndexChanging="gvTable_PageChanging4" PageSize="15">
                        <Columns >					
                        <asp:BoundField DataField="FS_CLASS" ItemStyle-Width="60px" />
                        <asp:BoundField DataField="P" ItemStyle-Width="50px" />
                        <asp:BoundField DataField="SM" ItemStyle-Width="50px" />
                        <asp:BoundField DataField="SS" ItemStyle-Width="50px" />
                        <asp:BoundField DataField="D" ItemStyle-Width="50px" />
                        <asp:BoundField DataField="DL" ItemStyle-Width="50px" />
                        <asp:BoundField DataField="L" ItemStyle-Width="50px" />
                        <asp:BoundField DataField="Total" ItemStyle-Width="40px" />
                        <asp:BoundField DataField="Total(%)" ItemStyle-Width="40px" />
                        <asp:BoundField DataField="NPLs(%)" ItemStyle-Width="40px" />
                        <asp:BoundField DataField="Default(%)" ItemStyle-Width="40px" />
                        </Columns>
                        </asp:GridView> 
                        </asp:TableCell>
                        </asp:TableRow>    
                         </asp:Table>
                            </asp:TableCell>
                            </asp:TableRow>
                            </asp:Table>  
                       </ContentTemplate></asp:UpdatePanel></asp:Content>