<%@ Page Language="C#" AutoEventWireup="true" CodeFile="adminusersecurity.aspx.cs" Inherits="adminusersecurity" %>

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
    <asp:Label ID="message" runat="server" />
    <div><asp:Label ID="useridcolon" runat="server" /></div>
    <div><asp:TextBox ID="UserId" runat="server" Width="275px" ReadOnly=true MaxLength="15" /></div>
    <div><asp:Label ID="passwordcolon" runat="server" /></div>
    <div><asp:TextBox ID="MotPass1" runat="server" Width="275px" TextMode="Password" MaxLength=50 /></div>
    <div><asp:Label ID="passwordconfirmcolon" runat="server" /></div>
    <div><asp:TextBox ID="MotPass2" runat="server" Width="275px" TextMode="Password" MaxLength=50 /></div>
    
    <div><asp:button id="Confirm" onclick="SaveData" runat="server" Text="Sauvegarder"></asp:button>
    <asp:button id="Cancel" onclick="ReturnParent" runat="server" Text="Annuler"></asp:button></div>
    
    </div>
    </form>
</body>
</html>
