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

public partial class adminmetatagslist : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        new bosoauto().CheckAutorizationSystemAdministrator();
        bosotemplate myTemplate = new bosotemplate("templates\\adminheader.tpl");
        myTemplate.assign("SwitchLanguage", new bosolanguage().SwitchLanguage("adminmetatagslist.aspx", Request.QueryString.Get("To")));
        List<string> label = myTemplate.switchTagsLanguage((string)Context.Session["LanguageMID"], new string[] { "metatagstemplatemanagement", "newbutton", "deletebutton", "deleteconfirmcin", "name" });
        myTemplate.assign("Title", Title = label[0]);
        myTemplate.switchTagsSmarty();
        headertemplate.Text = myTemplate.getTemplate();

        if (!Page.IsPostBack) ShowListMetaTags("RoleId");
    }

    protected void MetaTagsDataGrid_EventHandler(Object sender, DataGridSortCommandEventArgs e)
    {
        ShowListMetaTags(e.SortExpression);
    }

    protected void MetaTagsDataGrid_OnItemDataBound(object sender, DataGridItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            Button btn = (Button)e.Item.Cells[2].Controls[0];
            btn.Attributes.Add("onclick", "return confirm('Etes-vous certains de vouloir supprimer ce meta tags ?');");
        }
    }

    protected void MetaTagsDataGrid_Command(Object sender, DataGridCommandEventArgs e)
    {
        switch (e.CommandName)
        {
            case "Delete":
                bosorole role = new bosorole();
                if (role.Delete(Convert.ToInt32(e.Item.Cells[0].Text)))
                {
                    ErrorMsg.Text = "Un meta tags a été supprimé.";
                    ShowListMetaTags("MID");
                }
                else
                {
                    ErrorMsg.Text = "Impossible de supprimé le meta tags.";
                }
                break;
        }
    }

    public void ShowListMetaTags(string sort)
    {
        bosometatags metatags = new bosometatags();
        List<string> list = metatags.LoadListMetaTags();
        
        DataTable dt = new DataTable();
        DataRow dr;
        dt.Columns.Add(new DataColumn("MID", typeof(string)));

        for (int i = 0; i < list.Count; i++)
        {
            dr = dt.NewRow();
            dr[0] = list[i];
            dt.Rows.Add(dr);
        }

        DataView dv = new DataView(dt);
        MetaTagsDataGrid.DataSource = dv;
        MetaTagsDataGrid.DataBind();
        for (int i = 0; i < MetaTagsDataGrid.Items.Count; i++)
        {
            if (MetaTagsDataGrid.Items[i].Cells[0].Text == Request.QueryString.Get("Id"))
            {
                MetaTagsDataGrid.SelectedIndex = i;
            }
        }
    }

    protected void AddButton(object sender, EventArgs e)
    {
        Server.Transfer("adminmetatagsedit.aspx", false);
    }
}
