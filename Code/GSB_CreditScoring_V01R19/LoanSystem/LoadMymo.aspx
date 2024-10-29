<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LoadMymo.aspx.cs" Inherits="GSB.LoanSystem.LoadMymo" %>

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
    <form id="formMenu" runat="server">
        <div>
            <center>
                <asp:Label ID="Label4" runat="server" Text="Run Batch Mymo Data"></asp:Label>
                <br />
                <br />
                <br />
                <br />
                <br />
                <br />
                <asp:Button ID="Button2" runat="server" Text="ดำเนินการ Daily" onclick = "Button2_Click" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Button ID="Button3" runat="server" Text="ดำเนินการ Monthly" onclick = "Button3_Click"  />

            </center>
        </div>
    </form>


    <form id="form1" runat="server" visible="false">
    <div>
    <center>
    
        <asp:Label ID="Label1" runat="server" Text="Run Batch Mymo Data"></asp:Label>
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

    <form id="form2" runat="server" visible="false">
    <div>
    <center>
    
        <asp:Label ID="Label5" runat="server" Text="Run Batch Mymo Data"></asp:Label>
         <br />
        <br />
        <br />
           <asp:Label ID="Label6" runat="server" Text="Run แก้ไขข้อมูล เดือน" ></asp:Label>
        <br />
          <asp:DropDownList ID="ddlMNTMonthly" runat="server" AutoPostBack="true" />
        <br />
        <br />
        <br />
        <br />
         <asp:Button ID="Button4" runat="server" Text="ดำเนินการ Monthly" onclick = "Button4_Click" OnClientClick="Confirm(this.id)" UseSubmitBehavior="false" />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <asp:Label ID="Label7" runat="server" Text="" ></asp:Label>
        <br />
    </center>
    </div>
    </form>
</body>
</html>
