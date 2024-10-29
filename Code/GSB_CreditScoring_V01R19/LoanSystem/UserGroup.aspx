<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/CreditScoring.Master" 
AutoEventWireup="true" CodeBehind="UserGroup.aspx.cs" Inherits="GSB.LoanSystem.UserGroup" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Header" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
<asp:Panel ID="PanelMain" runat="server" Visible="true" Width="100%">
    <table border="0" cellpadding="5" cellspacing="3" style="width: 95%">
        <tr>
            <td class="searchbox" colspan="3" style="width: 942px; text-align: center" valign="top">
                <table>
                    <tr>
                        <td colspan="1">
                            GroupName :
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtGroupName" runat="server" CssClass="txt" Width="150px" MaxLength="100"></asp:TextBox></td>
                        <td>
                            Description :
                        </td>
                        <td>
                            &nbsp;<asp:TextBox ID="txtDescription" runat="server" CssClass="txt" Width="350px" MaxLength="255"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td colspan="10">
                            <asp:Button ID="btnAdd" runat="server" CssClass="btnsearch" OnClick="btnAdd_Click" Text="ADD" Width="50px" />
                            <asp:Button ID="btnUpdate" runat="server" CssClass="btnsearch" Enabled="False" OnClick="btnUpdate_Click" Text="Update" />
                            <asp:Button ID="btnCancel" runat="server" CssClass="btnsearch" OnClick="btnCancel_Click" Text="Cancel" />&nbsp;
                        </td>
                    </tr>
                </table>
                             </td></tr></table>
                <asp:GridView ID="gvGroups" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                    BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CaptionAlign="Top" AllowSorting="true"  OnSorting="gvGroups_Sorting"
                    CellPadding="3" CssClass="gridtxlist" EnableTheming="True" OnPageIndexChanging="gvGroups_OnPageIndexChanging"
                    OnRowCommand="gvGroups_OnRowCommand" OnRowDataBound="gvGroups_OnRowDataBound" OnRowDeleting="gvGroups_OnRowDeleting"
                    OnSelectedIndexChanged="gvGroups_SelectedIndexChanged" PagerSettings-FirstPageText="First"
                    PagerSettings-LastPageText="Last" PagerSettings-Mode="NumericFirstLast" PagerSettings-NextPageText=">"
                    PagerSettings-PageButtonCount="10" PagerSettings-PreviousPageText="<" PagerStyle-CssClass="viewpager"
                    PagerStyle-HorizontalAlign="Center" PageSize="19" ShowFooter="True" Width="95%">
                    <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" 
                        NextPageText="&gt;" PreviousPageText="&lt;" />
                    <Columns>                                            
                        <asp:TemplateField HeaderText="Edit">
                            <ItemStyle Width="50px" />
                            <ItemTemplate>
                                <asp:ImageButton ID="ImageButton_Select" runat="server" CommandArgument='<%# Eval("GroupID") %>'
                                    CommandName="Select" ImageUrl="~/Images/pen.gif" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Delete">
                            <ItemStyle Width="50px" />
                            <ItemTemplate>
                                <asp:ImageButton ID="ImageButton_Delete" runat="server" CommandArgument='<%# Eval("GroupID") %>'
                                    CommandName="Delete" ImageUrl="~/Images/cancel.gif" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="GroupName" HeaderText="GroupName" SortExpression="GroupName"  >
                            <ItemStyle Width="200px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="GroupDescription" HeaderText="Description" SortExpression="GroupDescription" ItemStyle-HorizontalAlign="Left"/>
                        <asp:TemplateField HeaderText="Function">
                            <ItemStyle Width="100px" />
                            <ItemTemplate>
                                <asp:ImageButton ID="ImageButton_Function" runat="server" CommandArgument='<%# Eval("GroupID") %>'
                                    CommandName="Function" ImageUrl="~/Images/Symbol Add.png" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <RowStyle CssClass="view" />
                    <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" />
                    <PagerStyle CssClass="viewpager" HorizontalAlign="Center" />
                    <HeaderStyle CssClass="viewheader" Font-Bold="True" />
                    <AlternatingRowStyle CssClass="view2" />
                </asp:GridView>
        </asp:Panel>       
        <%-----------------------------------------------------------------------------------------------------------------------------------------------------------%>
    <asp:Panel ID="PanelFunction" runat="server" Visible="false" Width="100%"><br />
        <strong>Group Name : </strong>&nbsp;<asp:Label ID="Label_GroupName" runat="server"
            CssClass="lbl" Font-Bold="True" ForeColor="#0000CC"></asp:Label><br />
        <br />
        <strong>Menu Assignment<br />
            <br />
        </strong>MenuID:
        <asp:TextBox ID="txtAddMenu" runat="server" CssClass="txt" Width="150px"></asp:TextBox>&nbsp;
        <asp:Button ID="btnSearchMenu" runat="server" CssClass="btnsearch" 
            Text="Browse..." onclick="btnSearchMenu_Click" />
        <asp:Button ID="btnAddMenu" runat="server" CssClass="btnsearch" OnClick="btnAddMenu_Click" Text="AddMenu" />
        <asp:Button ID="btnMenuClear" runat="server" CssClass="btnsearch" OnClick="btnMenuClear_Click" Text="Back" />
        <asp:HiddenField runat="server" ID="HiddenField_GroupID" /><br />
        <br />
        <asp:GridView ID="gvAvaMenu" runat="server" AllowPaging="True" AutoGenerateColumns="False"
            BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px" AllowSorting="True" OnSorting="gvAvaMenu_Sorting" 
            CellPadding="3" CssClass="gridtxlist" GridLines="Vertical" OnPageIndexChanging="gvAvaMenu_OnPageIndexChanging"
            OnRowCommand="gvAvaMenu_OnRowCommand" OnRowDataBound="gvAvaMenu_OnRowDataBound" PagerSettings-FirstPageText="First"
            PagerSettings-LastPageText="Last" PagerSettings-Mode="NumericFirstLast" PagerSettings-NextPageText=">"
            PagerSettings-PageButtonCount="10" PagerSettings-PreviousPageText="<" PagerStyle-CssClass="viewpager"
            PagerStyle-HorizontalAlign="Center" PageSize="18" Width="95%">
            <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast"
                NextPageText="&gt;" PreviousPageText="&lt;" />
            <Columns>
                <asp:TemplateField HeaderText="Remove">
                    <ItemStyle Width="50px" />
                    <ItemTemplate>
                        <asp:ImageButton ID="ImageButton_MenuRemove" runat="server" CommandArgument='<%# Eval("MenuID") %>'
                            CommandName="MenuRemove" ImageUrl="~/Images/cancel.gif" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="MenuID" HeaderText="MenuID" SortExpression="MenuID">
                    <ItemStyle Width="50px" />
                </asp:BoundField>
                <asp:BoundField DataField="MenuName" HeaderText="MenuName" SortExpression="MenuName">
                    <ItemStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField DataField="Parent" HeaderText="Parent" SortExpression="Parent">
                    <ItemStyle Width="50px" />
                </asp:BoundField>
            </Columns>
            <RowStyle CssClass="view" />
            <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" />
            <PagerStyle CssClass="viewpager" HorizontalAlign="Center" />
            <HeaderStyle CssClass="viewheader" Font-Bold="True" />
            <AlternatingRowStyle CssClass="view2" />
        </asp:GridView>
        <br />
                
    </asp:Panel>
<%-----------------------------------------------------------------------------------------------------------------------------------------------------------%>

</asp:Content>
