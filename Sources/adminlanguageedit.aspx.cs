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

public partial class adminlanguageedit : System.Web.UI.Page
{
    int? CurrId = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        new bosoauto().CheckAutorizationSystemAdministrator();
        bosotemplate myTemplate = new bosotemplate("templates\\adminheader.tpl");
        myTemplate.assign("SwitchLanguage", new bosolanguage().SwitchLanguage("adminlanguageedit.aspx?Id=" + Request.QueryString.Get("Id"), Request.QueryString.Get("To")));
        List<string> label = myTemplate.switchTagsLanguage((string)Context.Session["LanguageMID"], new string[] { "languageedit", "nameencolon", "namefrcolon", "midcolon", "savebutton", "cancelbutton" });
        myTemplate.assign("Title", Title = label[0]);
        myTemplate.switchTagsSmarty();
        nameencolon.Text = label[1];
        namefrcolon.Text = label[2];
        midcolon.Text = label[3];
        Confirm.Text = label[4];
        Cancel.Text = label[5];
        headertemplate.Text = myTemplate.getTemplate();
        TextBoxMID.Focus();

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
                bosolanguage language = new bosolanguage(CurrId.Value);
                TextBoxMID.Text = language.getMID();
                TextBoxName_EN.Text = language.getName_EN();
                TextBoxName_FR.Text = language.getName_FR();
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
        Server.Transfer("adminlanguagelist.aspx?Id=" + CurrId, false);
    }

    protected void SaveData(object sender, EventArgs e)
    {
        bosolanguage language = new bosolanguage();
        language.setId(CurrId);
        language.setMID(TextBoxMID.Text);
        language.setName_EN(TextBoxName_EN.Text);
        language.setName_FR(TextBoxName_FR.Text);
        if (language.SaveData())
        {
            Server.Transfer("adminlanguagelist.aspx?Id=" + language.getId(), false);
        }
        else
        {
            message.Text = language.getErrorMsg();
            message.BackColor = System.Drawing.Color.Red;
        }
    }

}
