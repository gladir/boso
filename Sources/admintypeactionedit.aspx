﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="admintypeactionedit.aspx.cs" Inherits="admintypeactionedit" %>

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
    <div><font color="red">*</font><asp:Label ID="midcolon" runat="server" /></div>
    <div><asp:TextBox ID="TextBoxMID" runat="server" Width="50px" MaxLength="15" /></div>
    <div><font color="red">*</font><asp:Label ID="nameencolon" runat="server" /></div>
    <div><asp:TextBox ID="TextBoxName_EN" runat="server" Width="275px" MaxLength="100" /></div>
    <div><font color="red">*</font><asp:Label ID="namefrcolon" runat="server" /></div>
    <div><asp:TextBox ID="TextBoxName_FR" runat="server" Width="275px" MaxLength="100" /></div>
    
    <div><asp:button id="Confirm" onclick="SaveData" runat="server" />
    <asp:button id="Cancel" onclick="ReturnParent" runat="server" /></div>
    
    </div>
    </form>
</body>
</html>
