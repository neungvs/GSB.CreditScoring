<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/CreditScoring.Master" AutoEventWireup="true" CodeBehind="ExportLog.aspx.cs" Inherits="GSB.LoanSystem.ExportLog" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Header" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
    <p>
        <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
&nbsp;<asp:Button ID="btnShow" runat="server" onclick="btnShow_Click" 
            Text="Export Log" />
        <asp:Button ID="Button1" runat="server" onclick="Button1_Click" Text="Back" />
    </p>
</asp:Content>
