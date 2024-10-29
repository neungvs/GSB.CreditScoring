<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LoadMaster.aspx.cs" Inherits="GSB.LoanSystem.LoadMaster" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
        <script language="javascript" type="text/javascript">
            function disableBtn(btnID, newText) {
                var btn = document.getElementById(btnID);
                setTimeout("EnableCtrl('" + btnID + "')", 30);
                btn.disabled = true;
                btn.value = newText;
            }

            function EnableCtrl(btnID) {
                var btn2 = document.getElementById(btnID);
                btn2.disable = false;
            }
            function Confirm(btnID) {

                var confirm_value = document.createElement("INPUT");
                confirm_value.type = "hidden";
                confirm_value.name = "confirm_value";
                if (confirm("เริ่มดำเนินการ")) {
                    confirm_value.value = "Yes";
                    //disableBtn(this.id, 'กำลังดำเนินการ');
                    disableBtn(btnID, 'กำลังดำเนินการ');
                    document.getElementById("Button1").disabled = "true";
                    document.getElementById("Button2").disabled = "true";
                    document.getElementById("Button3").disabled = "true";
                }
                else {
                    confirm_value.value = "No";
                }
                document.forms[0].appendChild(confirm_value);
            } 
    </script>
</head>
<body>

    <form id="form1" runat="server">
    <div>
    <center>
    <asp:ScriptManager EnablePartialRendering="true"  ID="ScriptManager1" runat="server" AsyncPostBackTimeout="3600"></asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>

        <asp:Label ID="Label1" runat="server" Text="Run Batch to Import Master Table"></asp:Label>
        <br />
        <br />
        <br />
        <asp:Button ID="Button1" runat="server" Text="ดำเนินการ Monthly Master" onclick = "Button1_Click" OnClientClick="Confirm(this.id)" UseSubmitBehavior="false" />
        <br />
        <br />
        <asp:Button ID="Button2" runat="server" Text="ดำเนินการ Daily Master" onclick = "Button2_Click" OnClientClick="Confirm(this.id)" UseSubmitBehavior="false" />
        <br />
        <br />
                <asp:Button ID="Button3" runat="server" onclick="Button3_Click" 
                    onclientclick="Confirm(this.id)" 
                    Text="ดำเนินการ Static Table" UseSubmitBehavior="False" />
                <br />
        <br />
        <asp:Label ID="Label2" runat="server" Text="กำลังดำเนินการ" Visible="false" ></asp:Label>
        <br />
        </ContentTemplate>
     <Triggers> <asp:AsyncPostBackTrigger ControlID="Button1" />
      </Triggers> </asp:UpdatePanel>
    </center>                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                          
    </div>
    </form>
</body>
</html>
                                                                                    