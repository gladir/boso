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

public partial class admincontentlist : System.Web.UI.Page
{
    string DeleteButtonText = "";
    string DeleteConfirmMessageText = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        new bosoauto().CheckAutorizationSystemAdministrator();
        bosotemplate myTemplate = new bosotemplate("templates\\adminheader.tpl");
        myTemplate.assign("SwitchLanguage", new bosolanguage().SwitchLanguage("admincontentlist.aspx", Request.QueryString.Get("To")));
        List<string> label = myTemplate.switchTagsLanguage((string)Context.Session["LanguageMID"], new string[] { "contentmanagement", "newbutton", "deletebutton", "deleteconfirmcontent", "name" });
        myTemplate.assign("Title", Title = label[0]);
        myTemplate.switchTagsSmarty();
        ButtonAdd.Text = label[1];
        DeleteButtonText = label[2];
        DeleteConfirmMessageText = label[3];
        ContentDataGrid.Columns[2].HeaderText = label[4];//Nom
        ContentDataGrid.Columns[2].FooterText = label[4];//Nom
        headertemplate.Text = myTemplate.getTemplate();

        if (!Page.IsPostBack) ShowListContent("ContentId");
    }

    protected void ContentDataGrid_EventHandler(Object sender, DataGridSortCommandEventArgs e)
    {
        ShowListContent(e.SortExpression);
    }

    protected void ContentDataGrid_OnItemDataBound(object sender, DataGridItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            Button btn = (Button)e.Item.Cells[3].Controls[0];
            btn.Attributes.Add("onclick", "return confirm('" + DeleteConfirmMessageText.Replace("'", "\\'") + "');");
            btn.Text = DeleteButtonText;
        }
    }

    protected void ContentDataGrid_Command(Object sender, DataGridCommandEventArgs e)
    {
        switch (e.CommandName)
        {
            case "Delete":
                bosocontent content = new bosocontent();
                if (content.Delete((string)Session["CIN"],Convert.ToInt32(e.Item.Cells[0].Text)))
                {
                    ErrorMsg.Text = new bosomessage().CONTENT_DELETE;
                    ShowListContent("MID");
                }
                else
                {
                    ErrorMsg.Text = new bosomessage().CONTENT_CANT_DELETE;
                }
                break;
        }
    }

    public void ShowListContent(string sort)
    {
        bosocontent content = new bosocontent();
        ContentDataGrid.DataSource = content.LoadList((string)Session["CIN"], new string[] {"ContentId", "MID", "Name", "Description"}, sort);

        ContentDataGrid.DataBind();
        for (int i = 0; i < ContentDataGrid.Items.Count; i++)
        {
            if (ContentDataGrid.Items[i].Cells[0].Text == Request.QueryString.Get("Id"))
            {
                ContentDataGrid.SelectedIndex = i;
            }
        }
    }

    protected void AddButton(object sender, EventArgs e)
    {
        Server.Transfer("admincontentedit.aspx", false);
    }
}
