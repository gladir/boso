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

public partial class admintypeactionedit : System.Web.UI.Page
{
    int? CurrId = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        new bosoauto().CheckAutorizationSystemAdministrator();
        bosotemplate myTemplate = new bosotemplate("templates\\adminheader.tpl");
        myTemplate.assign("SwitchLanguage", new bosolanguage().SwitchLanguage("admintypeactionedit.aspx?Id=" + Request.QueryString.Get("Id"), Request.QueryString.Get("To")));
        List<string> label = myTemplate.switchTagsLanguage((string)Context.Session["LanguageMID"], new string[] { "typeactionedit", "midcolon", "nameencolon", "namefrcolon", "savebutton", "cancelbutton" });
        myTemplate.assign("Title", Title = label[0]);
        myTemplate.switchTagsSmarty();
        midcolon.Text = label[1];
        nameencolon.Text = label[2];
        namefrcolon.Text = label[2];
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
                bosotypeaction typeaction = new bosotypeaction(CurrId.Value);
                TextBoxMID.Text = typeaction.getMID();
                TextBoxName_EN.Text = typeaction.getName_EN();
                TextBoxName_FR.Text = typeaction.getName_FR();
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
        Server.Transfer("admintypeactionlist.aspx?Id=" + CurrId, false);
    }

    protected void SaveData(object sender, EventArgs e)
    {
        bosotypeaction typeaction = new bosotypeaction();
        typeaction.setId(CurrId);
        typeaction.setMID(TextBoxMID.Text);
        typeaction.setName_EN(TextBoxName_EN.Text);
        typeaction.setName_FR(TextBoxName_FR.Text);
        if (typeaction.SaveData())
        {
            Server.Transfer("admintypeactionlist.aspx?Id=" + typeaction.getId(), false);
        }
        else
        {
            message.Text = typeaction.getErrorMsg();
            message.BackColor = System.Drawing.Color.Red;
        }
    }
}
