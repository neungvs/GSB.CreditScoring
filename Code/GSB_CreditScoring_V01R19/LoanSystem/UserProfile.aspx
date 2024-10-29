<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/CreditScoring.Master" AutoEventWireup="true" CodeBehind="UserProfile.aspx.cs" Inherits="GSB.LoanSystem.UserProfile" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Header" runat="server">
<style type="text/css">
        .txt
        {
            text-align: left;
            
        }
        .txt2
        {
            font-size: 12px;
            font-family: "Helvetica Neue", Helvetica, Arial, sans-serif;
            font-weight: normal;
            padding-left: 8px;
            padding-top: 6px;
            margin-top: 6px;
            height: 26px;
            /* width: 300px; */
            color: #d32d8e;
            background: url('../Images/Body/sprite.png') repeat-x -0px -230px;
            border: 0;
            outline: 0;
   
        }
       
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
    <asp:Panel ID="Panel_UserMain" runat="server" Width="100%" Visible = "true">
    <table border="0" cellpadding="5" cellspacing="3" style="width: 95%">
        <tr>
            <td class="searchbox" colspan="3" style="width: 100%; text-align: center" valign="top">
                <table width="100%">
                    <tr>
                        <td class="tdsearch">
                        </td>
                        <td class="tdsearch">
                            User ID</td>
                        <td class="tdsearch">
                            Password</td>
                      <td class="tdsearch">
                            Prefix</td>
                      <td class="tdsearch">
                            Name</td>
                      <td class="tdsearch">
                            SurName</td>
                       <td class="tdsearch">
                            Description</td>
                        <td class="tdsearch">
                            Group</td>
                        <td class="tdsearch">
                            Lock/UnLock</td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;</td>
                        <td>
                            <asp:TextBox ID="txtUserID" runat="server"  Width="50px" MaxLength="15"></asp:TextBox></td>
                        <td>
                            <%--<asp:TextBox ID="txtUserpwd"  runat="server" CssClass="txt2" Width="50px" MaxLength="15"></asp:TextBox>--%>
                             <opp:PasswordTextBox id="txtUserpwd" runat="server" CssClass="txt2" Width="50px" MaxLength="15"/> </td>
                        <td>
                            <asp:TextBox ID="txtUserpName" runat="server"  Width="25" MaxLength="15"  Enabled="false"></asp:TextBox></td>
                        <td>
                            <asp:TextBox ID="txtUserName" runat="server" CssClass="txt" Width="100" MaxLength="150"  Enabled="false"></asp:TextBox></td>
                        <td>
                            <asp:TextBox ID="txtUserSurName" runat="server" CssClass="txt" Width="100" MaxLength="150"  Enabled="false"></asp:TextBox></td>
                     <td>
                            <asp:TextBox ID="txtUserDesc" runat="server" CssClass="txt" Width="100" MaxLength="150"  Enabled="false"></asp:TextBox></td>
                        <td>
                            <asp:DropDownList ID="ddlGroup" runat="server" CssClass="ddl" DataTextField="Group_name" DataValueField="Group_id" Width="100px">
                            </asp:DropDownList></td>
                        <td>
                            <asp:CheckBox ID="cbLock" runat="server" CssClass="cbsearch" Width="80px"/></td>
                    </tr>
                    <tr>
                        <td colspan="1">
                        </td>
                        <td colspan="10">
                        <asp:Button ID="btnAdd" runat="server" CssClass="btnsearch" OnClick="btnAdd_Click" Text="ADD" Width="50px" TabIndex="0"/>&nbsp;&nbsp;&nbsp;
                        <asp:Button ID="btnUpdate" runat="server" CssClass="btnsearch" OnClick="btnUpdate_Click" Text="Update" Enabled="False" TabIndex="1"/>&nbsp;&nbsp;&nbsp;
                        <asp:Button ID="btnCancel" runat="server" CssClass="btnsearch" OnClick="btnCancel_Click" Text="Cancel" TabIndex="2"/>&nbsp;
                         </td>
                    </tr>
                </table>
             </td>
        </tr>
    </table>
    <asp:Label ID="lblstr" runat="server" ForeColor="Red" Width="400px"></asp:Label>&nbsp;
     <asp:GridView ID="gvUser" runat="server" AllowPaging="True" AutoGenerateColumns="False"
        CaptionAlign="Top" EnableTheming="True" OnSorting="gvUser_Sorting" AllowSorting="true" 
        OnPageIndexChanging="gvUser_OnPageIndexChanging" OnRowCommand="gvUser_OnRowCommand"
        OnRowDataBound="gvUser_OnRowDataBound" OnRowDeleting="gvUser_OnRowDeleting" 
        OnSelectedIndexChanged="gvUser_SelectedIndexChanged"
        PageSize="16" ShowFooter="True" Width="95%" BorderColor="#999999" BorderStyle="None" 
        BorderWidth="1px" CellPadding="3"
        CssClass="gridtxlist" PagerSettings-Mode="NumericFirstLast" PagerSettings-NextPageText=">"
        PagerSettings-PreviousPageText="<" PagerSettings-LastPageText="Last" PagerSettings-FirstPageText="First"
        PagerSettings-PageButtonCount="10" PagerStyle-HorizontalAlign="Center" PagerStyle-CssClass="viewpager">
        <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast"
            NextPageText="&gt;" PreviousPageText="&lt;" />
        <Columns>
            <asp:TemplateField HeaderText="Edit">
                <ItemTemplate>
                    <asp:ImageButton ID="ImageButton_Select" runat="server" CommandArgument='<%# Eval("User_ID") %>'
                        CommandName="Select" ImageUrl="~/Images/pen.gif" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Delete">
                <ItemTemplate>
                    <asp:ImageButton ID="ImageButton_Delete" runat="server" CommandArgument='<%# Eval("User_ID") %>'
                        CommandName="Delete" ImageUrl="~/Images/cancel.gif" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="User_ID" HeaderText="User ID"  SortExpression="User_ID"/>
            <asp:BoundField DataField="User_pName" HeaderText="Pre" SortExpression="User_pName"/>
            <asp:BoundField DataField="User_fName" HeaderText="Name" SortExpression="User_fName"/>
            <asp:BoundField DataField="User_sName" HeaderText="Surname" SortExpression="User_sName"/>
            <asp:BoundField DataField="User_desc" HeaderText="Description" SortExpression="User_desc"/>
            <asp:BoundField DataField="ACTIVE_FLAG" HeaderText="Active" SortExpression="ACTIVE_FLAG" />
           <asp:BoundField DataField="Groups" HeaderText="Groups" SortExpression="Groups"/>

        </Columns>
        <RowStyle CssClass="view" />
        <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" />
        <PagerStyle HorizontalAlign="Center" CssClass="viewpager" />
        <HeaderStyle Font-Bold="True" CssClass="viewheader" />
        <AlternatingRowStyle CssClass="view2" />
    </asp:GridView>
    </asp:Panel>

</asp:Content>
