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

public partial class adminconfiglist : System.Web.UI.Page
{
    string DeleteButtonText = "";
    string DeleteConfirmMessageText = "";
    
    protected void Page_Load(object sender, EventArgs e)
    {
        new bosoauto().CheckAutorizationSystemAdministrator();
        bosotemplate myTemplate = new bosotemplate("templates\\adminheader.tpl");
        myTemplate.assign("SwitchLanguage", new bosolanguage().SwitchLanguage("adminconfiglist.aspx", Request.QueryString.Get("To")));
        List<string> label = myTemplate.switchTagsLanguage((string)Context.Session["LanguageMID"], new string[] { "configurationmanagement", "newbutton", "deletebutton", "deleteconfirmconfig", "variablename", "variablevalue" });
        myTemplate.assign("Title", Title = label[0]);
        myTemplate.switchTagsSmarty();
        ButtonAdd.Text = label[1];
        DeleteButtonText = label[2];
        DeleteConfirmMessageText = label[3];
        ConfigDataGrid.Columns[1].HeaderText = label[4];//Nom de variable
        ConfigDataGrid.Columns[1].FooterText = label[4];//Nom de variable
        ConfigDataGrid.Columns[2].HeaderText = label[5];//Valeur de variable
        ConfigDataGrid.Columns[2].FooterText = label[5];//Valeur de variable
        headertemplate.Text = myTemplate.getTemplate();

        if (!Page.IsPostBack) ShowListConfig("VariableName");
    }

    protected void ConfigDataGrid_EventHandler(Object sender, DataGridSortCommandEventArgs e)
    {
        ShowListConfig(e.SortExpression);
    }

    protected void ConfigDataGrid_OnItemDataBound(object sender, DataGridItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            Button btn = (Button)e.Item.Cells[3].Controls[0];
            btn.Attributes.Add("onclick", "return confirm('" + DeleteConfirmMessageText.Replace("'", "\\'") + "');");
            btn.Text = DeleteButtonText;
        }
    }

    protected void ConfigDataGrid_Command(Object sender, DataGridCommandEventArgs e)
    {
        switch (e.CommandName)
        {
            case "Delete":
                bosoconfig config = new bosoconfig();
                if (config.Delete(e.Item.Cells[0].Text))
                {
                    ErrorMsg.Text = new bosomessage().CONFIGVAR_DELETE;
                    ShowListConfig("VariableName");
                }
                else
                {
                    ErrorMsg.Text = new bosomessage().CONFIGVAR_CANT_DELETE;
                }
                break;
        }
    }

    public void ShowListConfig(string sort)
    {
        bosoconfig config = new bosoconfig();
        ConfigDataGrid.DataSource = config.LoadList(new string[] { "VariableName", "VariableValue" }, sort);

        ConfigDataGrid.DataBind();
        for (int i = 0; i < ConfigDataGrid.Items.Count; i++)
        {
            if (ConfigDataGrid.Items[i].Cells[0].Text == Request.QueryString.Get("Id"))
            {
                ConfigDataGrid.SelectedIndex = i;
            }
        }
    }

    protected void AddButton(object sender, EventArgs e)
    {
        Server.Transfer("adminconfigedit.aspx", false);
    }
}
