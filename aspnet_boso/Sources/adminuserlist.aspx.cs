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

public partial class adminuserlist : System.Web.UI.Page
{
    string DeleteButtonText = "";
    string DeleteConfirmMessageText = "";
    string SecurityButtonText = "";
    
    protected void Page_Load(object sender, EventArgs e)
    {
        new bosoauto().CheckAutorizationAdministratorOrSystemAdministrator();
        bosotemplate myTemplate = new bosotemplate("templates\\adminheader.tpl");
        myTemplate.assign("SwitchLanguage", new bosolanguage().SwitchLanguage("adminuserlist.aspx", Request.QueryString.Get("To")));
        List<string> label = myTemplate.switchTagsLanguage((string)Context.Session["LanguageMID"], new string[] { "usermanagement", "newbutton", "deletebutton", "securitybutton", "deleteconfirmuser", "cin", "mid", "firstname", "lastname", "visit" });
        myTemplate.assign("Title", Title = label[0]);
        myTemplate.switchTagsSmarty();
        ButtonAdd.Text = label[1];
        DeleteButtonText = label[2];
        SecurityButtonText = label[3];
        DeleteConfirmMessageText = label[4];
        UserDataGrid.Columns[1].HeaderText = label[5];//CIN
        UserDataGrid.Columns[1].FooterText = label[5];//CIN
        UserDataGrid.Columns[2].HeaderText = label[6];//MID
        UserDataGrid.Columns[2].FooterText = label[6];//MID
        UserDataGrid.Columns[3].HeaderText = label[7];//Prénom
        UserDataGrid.Columns[3].FooterText = label[7];//Prénom
        UserDataGrid.Columns[4].HeaderText = label[8];//Nom
        UserDataGrid.Columns[4].FooterText = label[8];//Nom
        UserDataGrid.Columns[5].HeaderText = label[9];//Visite
        UserDataGrid.Columns[5].FooterText = label[9];//Visite
        headertemplate.Text = myTemplate.getTemplate();

        if (!Page.IsPostBack) ShowListUser("UserId");
    }

    protected void UserDataGrid_EventHandler(Object sender, DataGridSortCommandEventArgs e)
    {
        ShowListUser(e.SortExpression);
    }

    protected void UserDataGrid_OnItemDataBound(object sender, DataGridItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            Button btn = (Button)e.Item.Cells[6].Controls[0];
            btn.Attributes.Add("onclick", "return confirm('" + DeleteConfirmMessageText.Replace("'", "\\'") + "');");
            btn.Text = DeleteButtonText;
            Button btnSecurity = (Button)e.Item.Cells[7].Controls[0];
            btnSecurity.Text = SecurityButtonText;
        }
    }

    protected void UserDataGrid_Command(Object sender, DataGridCommandEventArgs e)
    {
        switch (e.CommandName)
        {
            case "Delete":
                bosouser user = new bosouser();
                if (user.Delete(Convert.ToInt32(e.Item.Cells[0].Text)))
                {
                    ErrorMsg.Text = new bosomessage().USER_DELETE;
                    ShowListUser("MID");
                }
                else
                {
                    ErrorMsg.Text = new bosomessage().USER_CANT_DELETE;
                }
                break;
            case "Security":
                Server.Transfer("adminusersecurity.aspx?Id=" + e.Item.Cells[0].Text);
                break;
        }
    }

    public void ShowListUser(string sort)
    {
        bosouser user = new bosouser();
        UserDataGrid.DataSource = user.LoadList((string)Session["CIN"], new string[] { "CIN", "UserId", "MID", "FirstName", "LastName", "LoginCount" }, sort);
        UserDataGrid.DataBind();
        for (int i = 0; i < UserDataGrid.Items.Count; i++)
        {
            if (UserDataGrid.Items[i].Cells[0].Text == Request.QueryString.Get("Id"))
            {
                UserDataGrid.SelectedIndex = i;
            }
        }
        if ("" != (string)Session["CIN"])
        {
            UserDataGrid.Columns[1].Visible = false;
        }
    }

    protected void AddButton(object sender, EventArgs e)
    {
        Server.Transfer("adminuseredit.aspx", false);
    }
}
