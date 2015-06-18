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

public partial class adminlanguagelist : System.Web.UI.Page
{
    string DeleteButtonText = "";
    string DeleteConfirmMessageText = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        new bosoauto().CheckAutorizationSystemAdministrator();
        bosotemplate myTemplate = new bosotemplate("templates\\adminheader.tpl");
        myTemplate.assign("SwitchLanguage", new bosolanguage().SwitchLanguage("adminlanguagelist.aspx", Request.QueryString.Get("To")));
        List<string> label = myTemplate.switchTagsLanguage((string)Context.Session["LanguageMID"], new string[] { "languagemanagement", "newbutton", "deletebutton", "deleteconfirmlanguage", "nameen", "namefr" });
        myTemplate.assign("Title", Title = label[0]);
        myTemplate.switchTagsSmarty();
        ButtonAdd.Text = label[1];
        DeleteButtonText = label[2];
        DeleteConfirmMessageText = label[3];
        LanguageDataGrid.Columns[2].HeaderText = label[4];//Nom anglais
        LanguageDataGrid.Columns[2].FooterText = label[4];//Nom anglais
        LanguageDataGrid.Columns[3].HeaderText = label[5];//Nom français
        LanguageDataGrid.Columns[3].FooterText = label[5];//Nom français
        headertemplate.Text = myTemplate.getTemplate();

        if (!Page.IsPostBack) ShowListLanguage("MID");
    }

    protected void LanguageDataGrid_EventHandler(Object sender, DataGridSortCommandEventArgs e)
    {
        ShowListLanguage(e.SortExpression);
    }

    protected void LanguageDataGrid_OnItemDataBound(object sender, DataGridItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            Button btn = (Button)e.Item.Cells[4].Controls[0];
            btn.Attributes.Add("onclick", "return confirm('" + DeleteConfirmMessageText.Replace("'", "\\'") + "');");
            btn.Text = DeleteButtonText;
        }
    }

    protected void LanguageDataGrid_Command(Object sender, DataGridCommandEventArgs e)
    {
        switch (e.CommandName)
        {
            case "Delete":
                bosolanguage language = new bosolanguage();
                if (language.Delete(Convert.ToInt32(e.Item.Cells[0].Text)))
                {
                    ErrorMsg.Text = new bosomessage().LANGUAGE_DELETE;
                    ShowListLanguage("MID");
                }
                else
                {
                    ErrorMsg.Text = new bosomessage().LANGUAGE_CANT_DELETE;
                }
                break;
        }
    }

    public void ShowListLanguage(string sort)
    {
        bosolanguage language = new bosolanguage();
        LanguageDataGrid.DataSource = language.LoadList(new string[] { "LanguageId", "MID", "Name_EN As Name", "Name_FR" }, sort);
        LanguageDataGrid.DataBind();
        for (int i = 0; i < LanguageDataGrid.Items.Count; i++)
        {
            if (LanguageDataGrid.Items[i].Cells[0].Text == Request.QueryString.Get("Id"))
            {
                LanguageDataGrid.SelectedIndex = i;
            }
        }
    }

    protected void AddButton(object sender, EventArgs e)
    {
        Server.Transfer("adminlanguageedit.aspx", false);
    }
}
