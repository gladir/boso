<%@ Page Language="C#" AutoEventWireup="true" CodeFile="admincinedit.aspx.cs" Inherits="admincinedit" %>

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
    <div><font color="red">*</font>CIN:</div>
    <div><asp:TextBox ID="TextBoxCIN" runat="server" Width="50px" MaxLength="5" /></div>
    <div><font color="red">*</font><asp:Label ID="namecolon" runat="server" /></div>
    <div><asp:TextBox ID="TextBoxName" runat="server" Width="275px" MaxLength="100" /></div>
    <div><asp:Label ID="languagecolon" runat="server" /></div>
    <div>
    <asp:dropdownlist id="TextboxLanguage" runat="server" Width="275px">
	</asp:dropdownlist>
	
	<div><asp:button id="Confirm" onclick="SaveData" runat="server" />
    <asp:button id="Cancel" onclick="ReturnParent" runat="server" /></div>
	
    </div>
    
    </div>
    </form>
</body>
</html>
