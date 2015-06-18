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

public partial class homeservice : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        new bosoauto().CheckAutorizationAnyUser();

        new bosolanguage().SwitchLanguage(Request.QueryString.Get("To"), toLanguage);
        List<string> label = new bosotemplate().switchTagsLanguage((string)Session["LanguageMID"], new string[] { "mainmenuadmin", "systemadministratorservice", "administratorservice", "cinmanagement", "configurationmanagement", "rolemanagement", "usermanagement", "metatagstemplatemanagement", "viewlogaction", "languagemanagement", "typeactionmanagement", "contentmanagement" });
        mainmenuadmin.Text = label[0];
        menuservicesysadmin.Text = label[1];
        menuserviceadmin.Text = label[2];
        listcin.Text = label[3];
        listconfig.Text = label[4];
        listrole.Text = label[5];
        listuser.Text = label[6];
        listmetatags.Text = label[7];
        logactionview.Text = label[8];
        listlanguage.Text = label[9];
        listtypeaction.Text = label[10];
        listcontent.Text = label[11]; 

        if ("" != (string)Session["CIN"])
        {
            listcin.Visible = false;
            listrole.Visible = false;

            listlanguage.Visible = false;
            listtypeaction.Visible = false;
        }
    }
}
