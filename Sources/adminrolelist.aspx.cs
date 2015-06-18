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

public partial class adminrolelist : System.Web.UI.Page
{
    string DeleteButtonText = "";
    string DeleteConfirmMessageText = "";
    
    protected void Page_Load(object sender, EventArgs e)
    {
        new bosoauto().CheckAutorizationSystemAdministrator();
        bosotemplate myTemplate = new bosotemplate("templates\\adminheader.tpl");
        myTemplate.assign("SwitchLanguage", new bosolanguage().SwitchLanguage("adminrolelist.aspx", Request.QueryString.Get("To")));
        List<string> label = myTemplate.switchTagsLanguage((string)Context.Session["LanguageMID"], new string[] { "rolemanagement", "newbutton", "deletebutton", "deleteconfirmrole", "mid", "name" });
        myTemplate.assign("Title", Title = label[0]);
        myTemplate.switchTagsSmarty();
        ButtonAdd.Text = label[1];
        DeleteButtonText = label[2];
        DeleteConfirmMessageText = label[3];
        RoleDataGrid.Columns[1].HeaderText = label[4];//MID
        RoleDataGrid.Columns[1].FooterText = label[4];//MID
        RoleDataGrid.Columns[2].HeaderText = label[5];//Nom
        RoleDataGrid.Columns[2].FooterText = label[5];//Nom
        headertemplate.Text = myTemplate.getTemplate();

        if (!Page.IsPostBack)
        {
            ShowListRole("RoleId");
        }
    }

    protected void RoleDataGrid_EventHandler(Object sender, DataGridSortCommandEventArgs e)
    {
        ShowListRole(e.SortExpression);
    }

    protected void RoleDataGrid_OnItemDataBound(object sender, DataGridItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            Button btn = (Button)e.Item.Cells[3].Controls[0];
            btn.Attributes.Add("onclick", "return confirm('" + DeleteConfirmMessageText.Replace("'", "\\'") + "');");
            btn.Text = DeleteButtonText;
        }
    }

    protected void RoleDataGrid_Command(Object sender, DataGridCommandEventArgs e)
    {
        switch (e.CommandName)
        {
            case "Delete":
                bosorole role = new bosorole();
                if (role.Delete(Convert.ToInt32(e.Item.Cells[0].Text)))
                {
                    ErrorMsg.Text = new bosomessage().ROLE_DELETE;
                    ShowListRole("MID");
                }
                else
                {
                    ErrorMsg.Text = new bosomessage().ROLE_CANT_DELETE;
                }
                break;
        }
    }

    public void ShowListRole(string sort)
    {
        bosorole role = new bosorole();
        RoleDataGrid.DataSource = role.LoadList(new string[] { "RoleId", "MID", "Name"}, sort);
        RoleDataGrid.DataBind();
        for (int i = 0; i < RoleDataGrid.Items.Count; i++)
        {
            if (RoleDataGrid.Items[i].Cells[0].Text == Request.QueryString.Get("Id"))
            {
                RoleDataGrid.SelectedIndex = i;
            }
        }
    }

    protected void AddButton(object sender, EventArgs e)
    {
        Server.Transfer("adminroleedit.aspx",false);
    }
}
