<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.1//EN" "http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>BOSO (Back-Office Service Object)</title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" />
        <div>
        <div style="float: left; overflow:hidden;"><b>BOSO (Back-Office Service Object)</b></div>
        <div style="float: right; overflow:hidden;"><asp:HyperLink ID="toLanguage" runat="server" BackColor="Blue" ForeColor="White" /></div><br />
        <br />
        <br />
        <asp:label id="welcomeMsg" runat="server" />
        <br />
        <br />
        </div>
        <div>
            <div><asp:HyperLink ID="adminlink" runat="server" NavigateUrl="admin.aspx" /></div>
            <br />
            <div><asp:label id="titleDemo" runat="server" /></div>
            <div><asp:HyperLink ID="demotemplate" runat="server" NavigateUrl="demotemplate.aspx" /></div>
            <div><asp:HyperLink ID="demovalidate" runat="server" NavigateUrl="demovalidate.aspx" /></div>
        </div>
    </form>
</body>
</html>
