<%@ Page Language="C#" AutoEventWireup="true" CodeFile="homeservice.aspx.cs" Inherits="homeservice" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>BOSO (Back-Office Service Object)</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div style="float: left; overflow:hidden;"><asp:Label ID="mainmenuadmin" Font-Bold=true runat="server" /></div>
        <div style="float: right; overflow:hidden;"><asp:HyperLink ID="toLanguage" NavigateUrl="homeservice.aspx" runat="server" BackColor="Blue" ForeColor="White" /></div>
    </div>
    <div>
        <br />
        <div><asp:Label ID="menuservicesysadmin" runat="server" /></div>
        <div><asp:HyperLink ID="listcin" runat="server" NavigateUrl="admincinlist.aspx" /></div>
        <div><asp:HyperLink ID="listconfig" runat="server" NavigateUrl="adminconfiglist.aspx" /></div>
        <div><asp:HyperLink ID="listrole" runat="server" NavigateUrl="adminrolelist.aspx" /></div>
        <div><asp:HyperLink ID="listuser" runat="server" NavigateUrl="adminuserlist.aspx" /></div>
        <div><asp:HyperLink ID="listmetatags" runat="server" NavigateUrl="adminmetatagslist.aspx" /></div>
        <div><asp:HyperLink ID="logactionview" runat="server" NavigateUrl="adminlogactionview.aspx" /></div>
        <br />
        <div><asp:HyperLink ID="listlanguage" runat="server" NavigateUrl="adminlanguagelist.aspx" /></div>
        <div><asp:HyperLink ID="listtypeaction" runat="server" NavigateUrl="admintypeactionlist.aspx" /></div>
        <br />
        <div><asp:Label ID="menuserviceadmin" runat="server" /></div>
        <div><asp:HyperLink ID="listcontent" runat="server" NavigateUrl="admincontentlist.aspx" /></div>
        Cliquer ici "a faire..."
    </div>
    
    </form>
</body>
</html>
