<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MenuSearch.aspx.cs" Inherits="GSB.LoanSystem.MenuSearch" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Menu Search</title>
   <link href="../css/global.css" rel="stylesheet" type="text/css" />
</head>
<body  bgcolor="#e6f3eb" topmargin="0" leftmargin="0">
    <form id="form2" runat="server" >
    <div align=center>
<%--        <asp:TextBox ID="txtMenuSearch" runat="server" CssClass="txt" Width="300px"></asp:TextBox>&nbsp;
        <asp:Button ID="btnMenuSearch" runat="server" CssClass="btn" Text="Search" OnClick="btnMenuSearch_Click" /><br />
        <asp:RadioButton ID="rbByID" runat="server" Checked="True" CssClass="txt" GroupName="search"
            Text="By MenuID" />&nbsp;<asp:RadioButton ID="rbByName" runat="server" CssClass="txt"
                GroupName="search" Text="By MenuName" />&nbsp;<asp:RadioButton ID="rbByParent" runat="server"
                    CssClass="txt" GroupName="search" Text="By Parent" /><br />--%>
        <asp:Panel ID="pnlOWRMList" runat="server" Visible="true" Width="100%" 
        BorderWidth="0">
        <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
            <tr>
                <td>
                                                       <asp:GridView ID="grdView" runat="server" 
                                                            OnPageIndexChanging="grdView_PageIndexChanging"
                                                            OnSorting="grdView_Sorting" 
                                                            OnRowCommand="grdView_RowCommand"
                                                            OnRowCreated="grdView_OnRowCreated" 
                                                            OnRowDataBound="grdView_RowDataBound"
                                                            AllowSorting="True"
                                                            HeaderStyle-CssClass="viewheader" RowStyle-CssClass="view" AlternatingRowStyle-CssClass="view2"
                                                            Width="100%" AutoGenerateColumns="False" AllowPaging="True" CssClass="gridtxlist"
                                                            CellPadding="3" PagerSettings-Mode="NumericFirstLast" PagerSettings-NextPageText=">"
                                                            PagerSettings-PreviousPageText="<" PagerSettings-LastPageText="Last" PagerSettings-FirstPageText="First"
                                                            PagerSettings-PageButtonCount="10" PagerStyle-HorizontalAlign="Center" PagerStyle-CssClass="viewpager" PageSize="20">
                                                            <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast"
                                                                NextPageText="&gt;" PreviousPageText="&lt;" />
                                                            <Columns>
                                                                <asp:ButtonField DataTextField="MenuID" CommandName="link" HeaderText="Menu ID" SortExpression="MenuID" >
                                                                    <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                                </asp:ButtonField>
<%--                                                                 <asp:ButtonField DataTextField="MenuNameID" CommandName="link" HeaderText="MenuNameID" SortExpression="MenuNameID" Visible="False">
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:ButtonField>--%>
                                                                <asp:ButtonField DataTextField="MenuName" CommandName="link" HeaderText="Menu Name" SortExpression="MenuName">
                                                                    <ItemStyle HorizontalAlign="Center"/>
                                                                </asp:ButtonField>
                                                                <asp:ButtonField DataTextField="Parent" CommandName="link" HeaderText="Parent" SortExpression="Parent">
                                                                    <ItemStyle HorizontalAlign="Center" Width="20px" />
                                                                </asp:ButtonField>
                                                           </Columns>                                                            
                                                            <RowStyle CssClass="view" HorizontalAlign="Left" />
                                                            <SelectedRowStyle Font-Bold="True" HorizontalAlign="Left" />
                                                            <PagerStyle HorizontalAlign="Center" Font-Overline="False" CssClass="viewpager" />
                                                            <HeaderStyle Font-Bold="True" CssClass="viewheader" />
                                                            <AlternatingRowStyle CssClass="view2" />
                                                        </asp:GridView>
                </td>
            </tr>
        </table>
    </asp:Panel>

<table width="100%">
        <tr>
            <td align="left" class="windowbutton" style="text-align: center">
                <!-- INSERT BACK BUTTON HERE -->
                <asp:Button ID="btnBack" runat="server" CssClass="btn" Height="24px" Text="Close" Width="80px"/>
                <!-- END INSERT BACK BUTTON -->
                </td>
        </tr>
</table>

<%--   </table>--%>
    </div>
    </form>
</body>
</html>
