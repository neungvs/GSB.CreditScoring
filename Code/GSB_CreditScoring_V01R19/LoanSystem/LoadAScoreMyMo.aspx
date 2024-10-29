<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LoadAScoreMyMo.aspx.cs" Inherits="GSB.LoanSystem.LoadAScoreMyMo" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
        <script language="javascript" type="text/javascript">
            function disableBtn(btnID, newText) {
                var btn = document.getElementById(btnID);
                setTimeout("EnableCtrl('" + btnID + "')", 1000);
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
                }
                else {
                    confirm_value.value = "No";
                }
                document.forms[0].appendChild(confirm_value);

            }  
    </script></head>
<body>

    <form id="form1" runat="server">
    <div>
    <center>
    
        <asp:Label ID="Label1" runat="server" Text="Run Batch Digital loan Data"></asp:Label>
         <br />
        <br />
        <br />
           <asp:Label ID="Label3" runat="server" Text="Run แก้ไขข้อมูลวันที่" ></asp:Label>
        <br />
          <asp:DropDownList ID="ddlMNT" runat="server" AutoPostBack="true" />
        <br />
        <br />
        <br />
        <br />
         <asp:Button ID="Button1" runat="server" Text="ดำเนินการ Daily" onclick = "Button1_Click" OnClientClick="Confirm(this.id)" UseSubmitBehavior="false" />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <asp:Label ID="Label2" runat="server" Text="" ></asp:Label>
        <br />
    </center>
    </div>
    </form>

 
</body>
</html>
