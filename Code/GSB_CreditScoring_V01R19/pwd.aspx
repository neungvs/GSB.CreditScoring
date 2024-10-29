<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="pwd.aspx.cs" Inherits="GSB.pwd" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table>
            <tr>
                <td>User Id</td>
                <td>
                    <asp:TextBox ID="txtUser" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td>
                    <asp:Button ID="btnSave" runat="server" Text="Save" onclick="btnSave_Click" /></td>
                <td>
                    <asp:Button ID="btnSaveAll" runat="server" Text="Save All" 
                        onclick="btnSaveAll_Click" /></td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
