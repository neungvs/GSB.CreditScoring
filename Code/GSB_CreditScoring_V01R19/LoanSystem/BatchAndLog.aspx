<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/CreditScoring.Master" AutoEventWireup="true" CodeBehind="BatchAndLog.aspx.cs" Inherits="GSB.LoanSystem.BatchAndLog" %>
<%@ Register Assembly="MattBerseth.WebControls.AJAX" Namespace="MattBerseth.WebControls.AJAX.GridViewControl"
    TagPrefix="mb" %>
    <asp:Content ID="Content1" ContentPlaceHolderID="Header" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
    <asp:UpdatePanel ID="uplddlBatchGroup" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:GridView ID="GridViewSum" runat="server" OnRowDataBound="GridViewSum_RowDataBound"  AutoGenerateColumns="False" CssClass="yui-datatable-theme">
                <Columns>
                    <asp:TemplateField HeaderText="ลำดับ" HeaderStyle-Width="35">
                        <ItemTemplate>
                            <center>
                                <b>
                                    <%# Eval("NO") %></b></center>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="RESOURCES" HeaderText="ประเภท Batch" HeaderStyle-Width="210" >
                    <ItemStyle HorizontalAlign = "Center"></ItemStyle>
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="วันที่ RUN ล่าสุด" HeaderStyle-Width="130">
                        <ItemTemplate>
                            <center>
                                <%# (Eval("START_DATE") == DBNull.Value ? "" : Convert.ToDateTime(Eval("START_DATE")).ToString("dd/MM/yyyy"))%></center>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="STATUS" HeaderText="สถานะ" HeaderStyle-Width="110" >
                  <ItemStyle HorizontalAlign = "Center"></ItemStyle>
                    </asp:BoundField>
                </Columns>
            </asp:GridView>
            <table border="0" cellpadding="2" cellspacing="3" width="100%">
                <tr>
                    <td>
                        <asp:LinkButton ID="lnkTab1" runat="server" OnClick="lnkTab1_Click">[ Batch-Log ]</asp:LinkButton>
                    </td>
                    <td>
                        <asp:LinkButton ID="lnkTab2" runat="server" OnClick="lnkTab2_Click">[ Batch-Activate ]</asp:LinkButton>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <asp:MultiView ID="MultiView1" runat="server">
                            <table width="100%" cellpadding="2" cellspacing="5">
                                <tr>
                                    <td>
                                        <asp:View ID="View1" runat="server">
                                            เลือกข้อมูล
                                            <asp:DropDownList ID="ddlBatchGroup" runat="server" AutoPostBack="True" OnSelectedIndexChanged="btnShow_Click">
                                             <%--   <asp:ListItem>000 - เลือกประเภท...</asp:ListItem>--%>
                                                <asp:ListItem Value="001">001 - BatchLog LOPs</asp:ListItem>
                                                <asp:ListItem Value="002">002 - BatchLog Master</asp:ListItem>
                                                <asp:ListItem Value="003">003 - BatchLog Data Warehouse</asp:ListItem>
                                                <asp:ListItem Value="004">004 - BatchLog MyMo</asp:ListItem>
                                                <asp:ListItem Value="005">005 - BatchLog OBA</asp:ListItem>
                                                <asp:ListItem Value="006">006 - BatchLog MyMoMonthly</asp:ListItem>
                                                <asp:ListItem Value="007">007 - BatchLog Digital loan</asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:Button ID="btnShow" runat="server" Text="แสดง" OnClick="btnShow_Click" />
                                            <asp:Button ID="Button1" runat="server" onclick="Button1_Click" 
                                                Text="Export Log" />
                                        </asp:View>
                                    </td>
                                    <td>
                                        <asp:View ID="View2" runat="server">
                                            เลือกข้อมูล<br />
                                      <%--      <asp:DropDownList ID="ddlBatchGroup2" runat="server" AutoPostBack="True" OnSelectedIndexChanged="btnShow_Click2">
                                                <asp:ListItem Value="001">001 - Import CBS Data</asp:ListItem>
                                                <asp:ListItem Value="002">002 - Import Master</asp:ListItem>
                                                <asp:ListItem Value="003">003 - Import Data Warehouse</asp:ListItem>
                                                <asp:ListItem Value="004">004 - Report Generate</asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:Button ID="btn2Show" runat="server" Text="ดำเนินการ" OnClick="btnShow_Click2" />--%>
                                            <a href="LoadMaster.aspx" target="_blank"  >Import Master Data</a><br />
                                            <a href="LoadCBS.aspx" target="_blank"  >Import LOPs Data</a><br />
                                            <a href="LoadDWH.aspx" target="_blank"  >Import DWH Data</a><br />
                                            <a href="LoadMymo.aspx" target="_blank"  >Import MyMo Data</a><br />
                                            <a href="OBA.aspx" target="_blank"  >Import OBA Data</a><br />
                                            <a href="LoadAScoreMyMo.aspx" target="_blank"  >Import Digital loan Data</a><br />
                                            </asp:View>
                                    </td>
                                </tr>
                            </table>
                        </asp:MultiView>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    <br>
    <asp:UpdatePanel ID="uplGridView1" runat="server" UpdateMode="Conditional">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="ddlBatchGroup" EventName="SelectedIndexChanged" />
            <asp:AsyncPostBackTrigger ControlID="btnShow" EventName="Click" />
        </Triggers>
        <ContentTemplate>
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
                CssClass="yui-datatable-theme" onrowdatabound="RowDataBound">
                <Columns>
                    <asp:TemplateField HeaderText="ลำดับ" HeaderStyle-Width="35">
                        <ItemTemplate>
                            <center>
                                <b>
                                    <%# Eval("NO") %></b></center>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="RESOURCES" HeaderText="แหล่งข้อมูล" HeaderStyle-Width="210" >
                    <ItemStyle HorizontalAlign = "Center"></ItemStyle>
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="วันที่เริ่ม" HeaderStyle-Width="130">
                        <ItemTemplate>
                            <center>
                                <%# (Eval("START_DATE") == DBNull.Value ? "" : Convert.ToDateTime(Eval("START_DATE")).ToString("dd/MM/yyyy HH:mm:ss"))%></center>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="วันที่เสร็จ" HeaderStyle-Width="130">
                        <ItemTemplate>
                            <center>
                                <%# (Eval("END_DATE") == DBNull.Value ? "" : Convert.ToDateTime(Eval("END_DATE")).ToString("dd/MM/yyyy HH:mm:ss")) %></center>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="จำนวนทั้งหมด" HeaderStyle-Width="75">
                        <ItemTemplate>
                        <center>
                            <%# (Eval("TOTAL") == DBNull.Value ? "" : Convert.ToDouble(Eval("TOTAL")).ToString("#,##0")) %></center>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="STATUS" HeaderText="สถานะ" HeaderStyle-Width="110" >
                  <ItemStyle HorizontalAlign = "Center"></ItemStyle>
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="ผิดพลาด">
                        <ItemTemplate>
                        <center>
                            <%# Eval("ERROR_MESSAGE").ToString() == "" ? "ไม่พบข้อผิดพลาด" : Eval("ERROR_MESSAGE").ToString()  %></center>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <mb:GridViewControlExtender ID="GridViewControlExtender1" runat="server" TargetControlID="GridView1"
                RowHoverCssClass="row-over" RowSelectCssClass="row-select" />
        </ContentTemplate>
    </asp:UpdatePanel>
    </asp:Content>
