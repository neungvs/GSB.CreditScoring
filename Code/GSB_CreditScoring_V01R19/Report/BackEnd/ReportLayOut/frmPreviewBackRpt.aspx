<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmPreviewBackRpt.aspx.cs" Inherits="GSB.Portfolio.Web.BackEnd.frmPreviewBackRpt" %>
<%@ Register assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <CR:CrystalReportViewer ID="rptView" runat="server" AutoDataBind="True" 
            Height="50px" ReportSourceID="CrystalReportSource1" Width="350px" />    
        <CR:CrystalReportSource ID="CrystalReportSource1" runat="server">
            <Report FileName="Reports\LogChonoRpt.rpt">
            </Report>
        </CR:CrystalReportSource>
        <input id="hdFS" type="hidden" runat="server" />
    </div>
    </form>
</body>
</html>
