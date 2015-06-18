<%@ Page Language="C#" AutoEventWireup="true" CodeFile="adminconfigedit.aspx.cs" Inherits="adminconfigedit" %>

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
    <div><font color="red">*</font><asp:Label ID="variablenamecolon" runat="server" /></div>
    <div><asp:TextBox ID="TextBoxVariableName" runat="server" Width="100px" MaxLength="15"></asp:TextBox></div>
    <div><font color="red">*</font><asp:Label ID="variablevaluecolon" runat="server" /></div>
    <div><asp:TextBox ID="TextBoxVariableValue" runat="server" Width="500px" MaxLength="15"></asp:TextBox></div>
    
    <div><asp:button id="Confirm" onclick="SaveData" runat="server" />
    <asp:button id="Cancel" onclick="ReturnParent" runat="server" /></div>
    
    </div>
    </form>
</body>
</html>
