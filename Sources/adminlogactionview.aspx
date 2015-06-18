<%@ Page Language="C#" AutoEventWireup="true" CodeFile="adminlogactionview.aspx.cs" Inherits="adminlogactionview" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Consulter le journal de bord des actions</title>
    <link href="inc/admin.css" rel="stylesheet" type="text/css"/>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <asp:label id="headertemplate" runat="server" />
    
    <asp:datagrid id="LogDataGrid" runat="server" OnSortCommand="LogDataGrid_EventHandler" GridLines="None" 
		AllowSorting="True" PageSize="5" PagerStyle-Mode="NumericPages" CssClass="dataGrid"
		PagerStyle-PageButtonCount="5" PagerStyle-HorizontalAlign="Left"
		UseAccessibleHeader="True" AutoGenerateColumns="False" ShowFooter="true" CaptionAlign="Left" CellPadding="0" Width="100%">
		<SelectedItemStyle CssClass="dataGridSelected"></SelectedItemStyle>
		<AlternatingItemStyle CssClass="dataGridPaire"></AlternatingItemStyle>
		<ItemStyle CssClass="dataGridImpaire"></ItemStyle>
		<HeaderStyle HorizontalAlign="Left" CssClass="dataGridTitles"></HeaderStyle>
		<FooterStyle HorizontalAlign="Left" CssClass="dataGridTitles"></FooterStyle>
		<Columns>
		    <asp:BoundColumn DataField="LogId" Visible="False" />
		    <asp:BoundColumn DataField="CreateDate" HeaderText="Date" FooterText="Date"></asp:BoundColumn>
			<asp:BoundColumn DataField="UserMid" HeaderText="Utilisateur" FooterText="Utilisateur"></asp:BoundColumn>
			<asp:BoundColumn DataField="IPAddr" HeaderText="IP" FooterText="IP"></asp:BoundColumn>
			<asp:BoundColumn DataField="Message" HeaderText="Message" FooterText="Message"></asp:BoundColumn>
			<asp:BoundColumn DataField="TypeActionName" HeaderText="Action" FooterText="Action"></asp:BoundColumn>
		</Columns>
		<PagerStyle PageButtonCount="5" Mode="NumericPages" HorizontalAlign="Left"></PagerStyle>
	</asp:datagrid>
    
    </div>
    </form>
</body>
</html>
