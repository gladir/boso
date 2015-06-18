<%@ Page Language="C#" AutoEventWireup="true" CodeFile="adminuseredit.aspx.cs" Inherits="adminuseredit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title runat="server" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <asp:label id="headertemplate" runat="server" />
    <input id="FormCurrId" type="hidden" runat="server" />
    <asp:Label ID="message" runat="server"></asp:Label>
    <div><asp:Label ID="cincolon" runat="server" /></div>
    <div><asp:dropdownlist id="TextBoxCIN" runat="server" Width="275px" /></div>
    <div><font color="red">*</font><asp:Label ID="useridcolon" runat="server" /></div>
    <div><asp:TextBox ID="TextBoxUserMID" runat="server" Width="275px" MaxLength="50" /></div>
    <div><asp:Label ID="primaryrolecolon" runat="server" /></div>
    <div><asp:dropdownlist id="TextboxUserRole" runat="server" Width="275px" /></div>
    <div><asp:Label ID="firstnamecolon" runat="server" /></div>
    <div><asp:TextBox ID="TextBoxFirstName" runat="server" Width="275px" MaxLength="100" /></div>
    <div><font color="red">*</font><asp:Label ID="lastnamecolon" runat="server" /></div>
    <div><asp:TextBox ID="TextBoxLastName" runat="server" Width="275px" MaxLength="100" /></div>
    <div><font color="red">*</font><asp:Label ID="emailcolon" runat="server" /></div>
    <div><asp:TextBox ID="TextBoxEmail" runat="server" Width="275px" MaxLength="100" /></div>
    <div><asp:button id="Confirm" onclick="SaveData" runat="server" />
    <asp:button id="Cancel" onclick="ReturnParent" runat="server" /></div>
    </div>
    </form>
</body>
</html>
