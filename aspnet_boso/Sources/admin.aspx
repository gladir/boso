<%@ Page Language="C#" AutoEventWireup="true" CodeFile="admin.aspx.cs" Inherits="admin" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.1//EN" "http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>BOSO (Back-Office Service Object)</title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" />
        <div>
        <div style="float: left; overflow:hidden;"><b>BOSO (Back-Office Service Object)</b></div>
        <div style="float: right; overflow:hidden;"><asp:HyperLink ID="toLanguage" NavigateUrl="admin.aspx" runat="server" BackColor="Blue" ForeColor="White" /></div>
        <br />
        <br />
        <br />
        </div>
        <div>
            <div><b><asp:Label ID="lblLogin" runat="server"></asp:Label></b></div>
            <div><asp:Label ID="lblCin" runat="server"></asp:Label></div>
            <div><asp:TextBox ID="CinLogin" runat="server" Width="50px" MaxLength=5></asp:TextBox></div>
            <div><asp:Label ID="lblUserId" runat="server"></asp:Label></div>
            <div><asp:TextBox ID="UserIdLogin" runat="server" MaxLength=30></asp:TextBox></div>
            <div><asp:Label ID="lblPassword" runat="server"></asp:Label></div> 
            <div><asp:TextBox ID="PasswordLogin" TextMode="Password" runat="server" MaxLength=30></asp:TextBox></div>
            
            <asp:imagebutton id="ButtonLogin" OnClick="LoginSubmit" runat="server" ImageUrl="imgs/btnlogin.png">
	        </asp:imagebutton>
            <div>
            <asp:Label ID="message" runat="server"></asp:Label>
            </div>
        </div>
    </form>
</body>
</html>

