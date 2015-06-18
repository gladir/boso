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

public partial class adminroleedit : System.Web.UI.Page
{
    int? CurrId = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        new bosoauto().CheckAutorizationSystemAdministrator();
        bosotemplate myTemplate = new bosotemplate("templates\\adminheader.tpl");
        myTemplate.assign("SwitchLanguage", new bosolanguage().SwitchLanguage("adminroleedit.aspx?Id=" + Request.QueryString.Get("Id"), Request.QueryString.Get("To")));
        List<string> label = myTemplate.switchTagsLanguage((string)Context.Session["LanguageMID"], new string[] { "roleedit", "midcolon", "namecolon", "descriptioncolon", "savebutton", "cancelbutton" });
        myTemplate.assign("Title", Title = label[0]);
        myTemplate.switchTagsSmarty();
        midcolon.Text = label[1];
        namecolon.Text = label[2];
        descriptioncolon.Text = label[3];
        Confirm.Text = label[4];
        Cancel.Text = label[5];
        headertemplate.Text = myTemplate.getTemplate();
        TextBoxRoleMID.Focus();

        if (!Page.IsPostBack)
        {
            try
            {
                if ("" != Request.QueryString.Get("Id")) CurrId = Convert.ToInt32(Request.QueryString.Get("Id"));
            }
            catch
            {
                CurrId = null;
            }

            if (null != CurrId)
            {
                bosorole role = new bosorole(CurrId.Value);
                TextBoxRoleMID.Text = role.getMID();
                TextBoxRoleName.Text = role.getName();
                TextBoxRoleDescription.Text = role.getDescription();
            }
        }
        else
        {
            try
            {
                CurrId = Convert.ToInt32(Request.Form.Get("FormCurrId"));
            }
            catch
            {
                CurrId = null;
            } 
        }
        if (CurrId.HasValue) FormCurrId.Value = Convert.ToString(CurrId);
    }

    protected void ReturnParent(object sender, EventArgs e)
    {
        Server.Transfer("adminrolelist.aspx?Id=" + CurrId, false);
    }

    protected void SaveData(object sender, EventArgs e)
    {
        bosorole role = new bosorole();
        role.setId(CurrId);
        role.setMID(TextBoxRoleMID.Text);
        role.setName(TextBoxRoleName.Text);
        role.setDescription(TextBoxRoleDescription.Text);
        if (role.SaveData())
        {
            Server.Transfer("adminrolelist.aspx?Id=" + role.getId(), false);
        }
        else
        {
            message.Text = role.getErrorMsg();
            message.BackColor = System.Drawing.Color.Red;
        }
    }
}
