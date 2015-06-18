<%@ Page Language="C#" AutoEventWireup="true" CodeFile="admincontentedit.aspx.cs" Inherits="admincontentedit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title runat="server" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
    <input id="FormCurrId" type="hidden" runat="server" />
    <asp:label id="headertemplate" runat="server" />
    <asp:Label ID="message" runat="server" />
    <div><font color="red">*</font>MID:</div>
    <div><asp:TextBox ID="TextBoxMID" runat="server" Width="100px" MaxLength="15" /></div>
    <div><font color="red">*</font><asp:Label ID="titlecolon" runat="server" /></div>
    <div><asp:TextBox ID="TextBoxName" runat="server" Width="500px" MaxLength="100" /></div>
    <div><font color="red">*</font><asp:Label ID="descriptioncolon" runat="server" /></div>
    <div><asp:TextBox ID="TextBoxDescription" runat="server" Width="500px" Height="200px" MaxLength="100" TextMode="MultiLine" /></div>
    
    <div><asp:button id="Confirm" onclick="SaveData" runat="server" Text="Sauvegarder" />
    <asp:button id="Cancel" onclick="ReturnParent" runat="server" Text="Annuler" /></div>
    
    
    </div>
    </form>
</body>
</html>
