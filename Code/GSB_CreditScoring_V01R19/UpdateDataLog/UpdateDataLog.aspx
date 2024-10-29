<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/CreditScoring.Master" AutoEventWireup="true" CodeBehind="UpdateDataLog.aspx.cs" Inherits="GSB.LoanSystem.UpdateDataLog" %>
<%@ Register Assembly="MattBerseth.WebControls.AJAX" Namespace="MattBerseth.WebControls.AJAX.GridViewControl"
    TagPrefix="mb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Header" runat="server">
        <style type="text/css">
            .yui-datatable-theme
            {}
            .style1
            {
                height: 269px;
            }
        </style>
        <link rel="stylesheet" media="screen" href="<%=Page.ResolveUrl("~/")%>Styles/jquery-ui-1.7.2.custom.css" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
<script src="<%=Page.ResolveUrl("~/")%>Scripts/jquery-ui-1.8.10.offset.datepicker.min.js" type="text/javascript"></script>

<script type="text/javascript">
    function dpc() {
        var d = new Date();
        var toDay = d.getDate() + '/' + (d.getMonth() + 1) + '/' + (d.getFullYear() + 543);
        $("#<%=txtOpenDate.ClientID%>").datepicker({ changeMonth: true, changeYear: true, dateFormat: 'dd/mm/yy', isBuddhist: true, defaultDate: toDay, dayNames: ['อาทิตย์', 'จันทร์', 'อังคาร', 'พุธ', 'พฤหัสบดี', 'ศุกร์', 'เสาร์'],
            dayNamesMin: ['อา.', 'จ.', 'อ.', 'พ.', 'พฤ.', 'ศ.', 'ส.'],
            monthNames: ['มกราคม', 'กุมภาพันธ์', 'มีนาคม', 'เมษายน', 'พฤษภาคม', 'มิถุนายน', 'กรกฎาคม', 'สิงหาคม', 'กันยายน', 'ตุลาคม', 'พฤศจิกายน', 'ธันวาคม'],
            monthNamesShort: ['ม.ค.', 'ก.พ.', 'มี.ค.', 'เม.ย.', 'พ.ค.', 'มิ.ย.', 'ก.ค.', 'ส.ค.', 'ก.ย.', 'ต.ค.', 'พ.ย.', 'ธ.ค.']
        });
        $("#<%=txtCloseDate.ClientID%>").datepicker({ changeMonth: true, changeYear: true, dateFormat: 'dd/mm/yy', isBuddhist: true, defaultDate: toDay, dayNames: ['อาทิตย์', 'จันทร์', 'อังคาร', 'พุธ', 'พฤหัสบดี', 'ศุกร์', 'เสาร์'],
            dayNamesMin: ['อา.', 'จ.', 'อ.', 'พ.', 'พฤ.', 'ศ.', 'ส.'],
            monthNames: ['มกราคม', 'กุมภาพันธ์', 'มีนาคม', 'เมษายน', 'พฤษภาคม', 'มิถุนายน', 'กรกฎาคม', 'สิงหาคม', 'กันยายน', 'ตุลาคม', 'พฤศจิกายน', 'ธันวาคม'],
            monthNamesShort: ['ม.ค.', 'ก.พ.', 'มี.ค.', 'เม.ย.', 'พ.ค.', 'มิ.ย.', 'ก.ค.', 'ส.ค.', 'ก.ย.', 'ต.ค.', 'พ.ย.', 'ธ.ค.']
        });

    }
    $(document).ready(function () { dpc(); });
    prm.add_endRequest(function () { $(document).ready(function () { dpc(); }); });
</script>
<table style="border:0">
    <tr>
        <td>
            <asp:GridView ID="GridViewSum" runat="server" 
                OnRowDataBound="GridViewSum_RowDataBound"  AutoGenerateColumns="False" 
                CssClass="yui-datatable-theme" Width="656px">
                <Columns>
                    <asp:TemplateField HeaderText="ลำดับ" HeaderStyle-Width="35">
                        <ItemTemplate>
                            <center>
                                <b>
                                    <%# Eval("NO") %></b></center>
                        </ItemTemplate>
                        <HeaderStyle Width="35px" />
                    </asp:TemplateField>
                    <asp:BoundField DataField="TABLE_NAME" HeaderText="ชื่อตาราง" 
                        HeaderStyle-Width="210" >
                    <HeaderStyle Width="210px" />
                    <ItemStyle HorizontalAlign = "Center"></ItemStyle>
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="วันที่ RUN ล่าสุด" HeaderStyle-Width="130">
                        <ItemTemplate>
                            <center>
                                <%# (Eval("CLEANSING_DATE") == DBNull.Value ? "" : Convert.ToDateTime(Eval("CLEANSING_DATE")).ToString("dd/MM/yyyy"))%></center>
                        </ItemTemplate>
                        <HeaderStyle Width="130px" />
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </td>
    </tr>
    <tr>
        <td class="style1">            
            <asp:Panel ID="Panel1" runat="server" BorderColor="#FF66FF" BorderStyle="Solid" 
                BorderWidth="2px">
            <table style="border:0">
                <tr>
                    <td valign="middle" align="right">   
                        ช่วงวันที่                     
                    </td>
                    <td colspan="5">
                        <asp:TextBox ID="txtOpenDate" runat="server" CssClass="Cal" />&nbsp;ถึง&nbsp;<asp:TextBox
                            ID="txtCloseDate" runat="server" CssClass="Cal" />
                    </td>
                </tr>
                <tr>
                    <td valign="middle" align="right">
                        ชื่อตาราง
                    </td>                    
                    <td>
                        <asp:CheckBox ID="chkCBS_LN_APP" runat="server" Text ="CBS_LN_APP" GroupName="gpTable_Name"/>
                    </td>
                    <td>
                        <asp:CheckBox ID="chkLN_APP" runat="server" Text ="LN_APP" GroupName="gpTable_Name"/>
                    </td>
                    <td>
                        <asp:CheckBox ID="chkLN_GRADE" runat="server" Text ="LN_GRADE" GroupName="gpTable_Name"/>
                    </td>
                    <td>
                        <asp:CheckBox ID="chkLN_CHAR" runat="server" Text ="LN_CHAR" GroupName="gpTable_Name"/>
                    </td>
                    <td>
                        <asp:CheckBox ID="chkDWH_BTFILE" runat="server" Text ="DWH_BTFILE" GroupName="gpTable_Name"/>
                    </td>
                </tr>
                <tr>
                    <td colspan="6" valign="middle" align="center">                        
                        <asp:Button ID="btnSearch" runat="server" Text="ค้นหา" 
                            onclick="btnSearch_Click" />&nbsp;&nbsp;
                        <asp:Button ID="btnExport" runat="server" Text="Export Summary Log" 
                            onclick="btnExport_Click" />&nbsp;&nbsp;
                        <asp:Button ID="BtnBack" runat="server" Text="Back" 
                            onclick="btnBack_Click" />&nbsp;&nbsp;
                    </td>
                </tr>
                <tr>
                    <td colspan="6" valign="middle" align="left">
                    <asp:Label ID="lblErrorMessage" runat="server" Text="lblError" Visible="false" ForeColor="Red"></asp:Label>
                    </td>
                </tr>
            </table>
            </asp:Panel>            
            <asp:Panel ID="Panel2" runat="server" BorderColor="#FF66FF" BorderStyle="Solid" 
                BorderWidth="2px">
                <table style="border:0">
                    <tr>
                        <td valign="middle" align="right">Export Log&nbsp;</td>
                        <td colspan="5"  valign="middle" align="center">
                            <asp:Button ID="btnExport_LOG_CBS_LN_APP" runat="server" Text="CBS_LN_APP" 
                                onclick="btnExport_LOG_CBS_LN_APP_Click"/>&nbsp;&nbsp;
                            <asp:Button ID="btnExport_LOG_LN_APP" runat="server" Text="LN_APP" 
                                onclick="btnExport_LOG_LN_APP_Click"/>&nbsp;&nbsp;
                            <asp:Button ID="btnExport_LOG_LN_GRADE" runat="server" Text="LN_GRADE" 
                                onclick="btnExport_LOG_LN_GRADE_Click"/>&nbsp;&nbsp;
                            <asp:Button ID="btnExport_LOG_LN_CHAR" runat="server" Text="LN_CHAR" 
                                onclick="btnExport_LOG_LN_CHAR_Click"/>&nbsp;&nbsp;
                            <asp:Button ID="btnExport_LOG_DWH_BTFILE" runat="server" Text="DWH_BTFILE" 
                                onclick="btnExport_LOG_DWH_BTFILE_Click"/>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </td>
    </tr>
</table>
<table style="width:200px;border:0px">
    <tr>
        <td>
            &nbsp;</td>
        <td>
            <asp:GridView ID="GridViewLog" runat="server" 
                OnRowDataBound="GridViewSum_RowDataBound" 
                CssClass="yui-datatable-theme" Width="900px" Height = "300px" AutoGenerateColumns="False" >
                <Columns>
                    <asp:BoundField DataField="ROWNUM" HeaderText="ลำดับที่">
                    <ItemStyle HorizontalAlign="Center" Width="50px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="table_name" HeaderText="ชื่อตาราง">
                    <ItemStyle HorizontalAlign="left" Width="100px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="CLEAN_REMARK" HeaderText="ขั้นตอน">
                    <ItemStyle HorizontalAlign="left" Width="200px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="CLEANSING_DATE" HeaderText="วันที่เริ่มดำเนินการ">
                    <ItemStyle HorizontalAlign="Center" Width="120px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="END_DATE" HeaderText="วันที่ดำเนินการสิ้นสุด">
                    <ItemStyle HorizontalAlign="Center" Width="120px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="COUNT" HeaderText="จำนวน">
                    <ItemStyle HorizontalAlign="Right" Width="60px" />
                    </asp:BoundField>
                </Columns>
            </asp:GridView>
        </td>
    </tr>
</table>
</asp:Content>