<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Login.Master" AutoEventWireup="true" CodeBehind="SignIn.aspx.cs" Inherits="GSB.MasterPage.SignIn" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Header" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
<div class="login_template">
    <asp:TextBox ID="txtUsername" runat="server" CssClass="username" ClientIDMode="Static" />
    <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="password"
        ClientIDMode="Static" />
    <asp:ImageButton ID="imgbtnLogin" runat="server" ImageUrl="Images/Login/login_button.png"
        CssClass="btnLogin" OnClick="imgbtnLogin_Click" />
</div>
<asp:Label ID="message" runat="server" CssClass="login_error" Width="581" Height="100" />
</asp:Content>
