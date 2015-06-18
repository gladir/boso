using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class admintypeactionlist : System.Web.UI.Page
{
    string DeleteButtonText = "";
    string DeleteConfirmMessageText = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        new bosoauto().CheckAutorizationSystemAdministrator();
        bosotemplate myTemplate = new bosotemplate("templates\\adminheader.tpl");
        myTemplate.assign("SwitchLanguage", new bosolanguage().SwitchLanguage("admintypeactionlist.aspx", Request.QueryString.Get("To")));
        List<string> label = myTemplate.switchTagsLanguage((string)Context.Session["LanguageMID"], new string[] { "typeactionmanagement", "newbutton", "deletebutton", "deleteconfirmtypeaction", "mid", "nameen", "namefr" });
        myTemplate.assign("Title", Title = label[0]);
        myTemplate.switchTagsSmarty();
        ButtonAdd.Text = label[1];
        DeleteButtonText = label[2];
        DeleteConfirmMessageText = label[3];
        TypeActionDataGrid.Columns[1].HeaderText = label[4];//MID
        TypeActionDataGrid.Columns[1].FooterText = label[4];//MID
        TypeActionDataGrid.Columns[2].HeaderText = label[5];//Nom anglais
        TypeActionDataGrid.Columns[2].FooterText = label[5];//Nom anglais
        TypeActionDataGrid.Columns[3].HeaderText = label[6];//Nom français
        TypeActionDataGrid.Columns[3].FooterText = label[6];//Nom français
        headertemplate.Text = myTemplate.getTemplate();

        if (!Page.IsPostBack) ShowListTypeAction("TypeActionId");
    }

    protected void TypeActionDataGrid_EventHandler(Object sender, DataGridSortCommandEventArgs e)
    {
        ShowListTypeAction(e.SortExpression);
    }

    protected void TypeActionDataGrid_OnItemDataBound(object sender, DataGridItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            Button btn = (Button)e.Item.Cells[4].Controls[0];
            btn.Attributes.Add("onclick", "return confirm('" + DeleteConfirmMessageText.Replace("'", "\\'") + "');");
            btn.Text = DeleteButtonText;
        }
    }

    protected void TypeActionDataGrid_Command(Object sender, DataGridCommandEventArgs e)
    {
        switch (e.CommandName)
        {
            case "Delete":
                bosotypeaction typeaction = new bosotypeaction();
                if (typeaction.Delete(Convert.ToInt32(e.Item.Cells[0].Text)))
                {
                    ErrorMsg.Text = new bosomessage().TYPEACTION_DELETE;
                    ShowListTypeAction("MID");
                }
                else
                {
                    ErrorMsg.Text = new bosomessage().TYPEACTION_CANT_DELETE;
                }
                break;
        }
    }

    public void ShowListTypeAction(string sort)
    {
        bosotypeaction typeaction = new bosotypeaction();
        TypeActionDataGrid.DataSource = typeaction.LoadList(new string[] { "TypeActionId", "MID", "Name_EN As Name", "Name_FR" }, sort);
        TypeActionDataGrid.DataBind();
        for (int i = 0; i < TypeActionDataGrid.Items.Count; i++)
        {
            if (TypeActionDataGrid.Items[i].Cells[0].Text == Request.QueryString.Get("Id"))
            {
                TypeActionDataGrid.SelectedIndex = i;
            }
        }
    }

    protected void AddButton(object sender, EventArgs e)
    {
        Server.Transfer("admintypeactionedit.aspx", false);
    }
}
