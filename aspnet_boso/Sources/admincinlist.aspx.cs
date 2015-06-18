// Nom du fichier:   admincinlist.aspx.cs
// Nom du projet:    BOSO (Back-Office Service Object)
// Société:          Gladir.com
// Développeur:      Sylvain Maltais
// Date de création: 2008/02/28
// Version:          0.1
//
///////////////
// Description:      Cette page permet d'afficher la liste des CINs.


using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class admincinlist : System.Web.UI.Page
{
    string DeleteButtonText = "";
    string DeleteConfirmMessageText = "";

    // Cette procédure est chargée automatiquement avant de commencer la page.
    protected void Page_Load(object sender, EventArgs e)
    {
        new bosoauto().CheckAutorizationSystemAdministrator();
        bosotemplate myTemplate = new bosotemplate("templates\\adminheader.tpl");
        myTemplate.assign("SwitchLanguage", new bosolanguage().SwitchLanguage("admincinlist.aspx", Request.QueryString.Get("To")));
        List<string> label = myTemplate.switchTagsLanguage((string)Context.Session["LanguageMID"], new string[] { "cinmanagement", "newbutton", "deletebutton", "deleteconfirmcin", "name" });
        myTemplate.assign("Title", Title = label[0]);
        myTemplate.switchTagsSmarty();
        ButtonAdd.Text = label[1];
        DeleteButtonText = label[2];
        DeleteConfirmMessageText = label[3];
        CinDataGrid.Columns[2].HeaderText = label[4];//Nom
        CinDataGrid.Columns[2].FooterText = label[4];//Nom
        headertemplate.Text = myTemplate.getTemplate();

        if (!Page.IsPostBack) ShowListCin("CIN");
    }

    // Cette procédure est un événement appelé lorsqu'on changer le tri de la liste
    protected void CinDataGrid_EventHandler(Object sender, DataGridSortCommandEventArgs e)
    {
        ShowListCin(e.SortExpression);
    }

    // Cette procédure permet d'ajouter une question de confirmation avant de supprimer
    protected void CinDataGrid_OnItemDataBound(object sender, DataGridItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            Button btn = (Button)e.Item.Cells[3].Controls[0];
            btn.Attributes.Add("onclick", "return confirm('" + DeleteConfirmMessageText.Replace("'", "\\'") + "');");
            btn.Text = DeleteButtonText;
        }
    }

    // Cette procédure permet d'effectuer les actions
    protected void CinDataGrid_Command(Object sender, DataGridCommandEventArgs e)
    {
        switch (e.CommandName)
        {
            case "Delete":
                bosocin cin = new bosocin();
                if (cin.Delete(e.Item.Cells[0].Text))
                {
                    ErrorMsg.Text = new bosomessage().CIN_DELETE;
                    ShowListCin("CIN");
                }
                else
                {
                    ErrorMsg.Text = new bosomessage().CIN_CANT_DELETE;
                }
                break;
        }
    }

    public void ShowListCin(string sort)
    {
        bosocin cin = new bosocin();
        CinDataGrid.DataSource = cin.LoadList(new string[] { "CIN", "Name" }, sort);
        CinDataGrid.DataBind();
        for (int i = 0; i < CinDataGrid.Items.Count; i++)
        {
            if (CinDataGrid.Items[i].Cells[0].Text == Request.QueryString.Get("Id"))
            {
                CinDataGrid.SelectedIndex = i;
            }
        }
    }

    protected void AddButton(object sender, EventArgs e)
    {
        Server.Transfer("admincinedit.aspx", false);
    }
}
