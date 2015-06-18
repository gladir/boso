<%@ Page Language="C#" AutoEventWireup="true" CodeFile="adminrolelist.aspx.cs" Inherits="adminrolelist" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Gestion des rôles</title>
    <link href="inc/admin.css" rel="stylesheet" type="text/css"/>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <asp:label id="headertemplate" runat="server" />
    <asp:Button id="ButtonAdd" runat="server" Text="Ajouter" OnClick="AddButton"></asp:Button>
    <asp:label id="ErrorMsg" runat="server" />
    <asp:datagrid id="RoleDataGrid" runat="server" OnSortCommand="RoleDataGrid_EventHandler" OnItemCommand="RoleDataGrid_Command" OnItemDataBound="RoleDataGrid_OnItemDataBound" GridLines="None" 
		AllowSorting="True" PageSize="5" PagerStyle-Mode="NumericPages" CssClass="dataGrid"
		PagerStyle-PageButtonCount="5" PagerStyle-HorizontalAlign="Left"
		UseAccessibleHeader="True" AutoGenerateColumns="False" ShowFooter="true" CaptionAlign="Left" CellPadding="0" Width="100%">
		<SelectedItemStyle CssClass="dataGridSelected"></SelectedItemStyle>
		<AlternatingItemStyle CssClass="dataGridPaire"></AlternatingItemStyle>
		<ItemStyle CssClass="dataGridImpaire"></ItemStyle>
		<HeaderStyle HorizontalAlign="Left" CssClass="dataGridTitles"></HeaderStyle>
		<FooterStyle HorizontalAlign="Left" CssClass="dataGridTitles"></FooterStyle>
		<Columns>
		    <asp:BoundColumn DataField="RoleId" Visible="False" />
			<asp:HyperLinkColumn DataTextField="MID" HeaderText="MID" FooterText="MID" DataNavigateUrlField="RoleId" DataNavigateUrlFormatString="adminroleedit.aspx?Id={0}" SortExpression="MID"></asp:HyperLinkColumn>
			<asp:BoundColumn DataField="Name" HeaderText="Nom" FooterText="Nom"></asp:BoundColumn>
			<asp:ButtonColumn ItemStyle-HorizontalAlign="Right" CommandName="Delete" Text="Supprimer" ButtonType=PushButton /> 
		</Columns>
		<PagerStyle PageButtonCount="5" Mode="NumericPages" HorizontalAlign="Left"></PagerStyle>
	</asp:datagrid>
    
    </div>
    </form>
</body>
</html>
