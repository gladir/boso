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

public partial class admincontentedit : System.Web.UI.Page
{
    int? CurrId = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        new bosoauto().CheckAutorizationSystemAdministrator();
        bosotemplate myTemplate = new bosotemplate("templates\\adminheader.tpl");
        myTemplate.assign("SwitchLanguage", new bosolanguage().SwitchLanguage("admincontentedit.aspx?Id=" + Request.QueryString.Get("Id"), Request.QueryString.Get("To")));
        List<string> label = myTemplate.switchTagsLanguage((string)Context.Session["LanguageMID"], new string[] { "contentedit", "titlecolon", "descriptioncolon", "savebutton", "cancelbutton" });
        myTemplate.assign("Title", Title = label[0]);
        myTemplate.switchTagsSmarty();
        titlecolon.Text = label[1];
        descriptioncolon.Text = label[2];
        Confirm.Text = label[3];
        Cancel.Text = label[4];
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
                bosocontent content = new bosocontent((string)Session["CIN"], CurrId.Value);
                TextBoxMID.Text = content.getMID();
                TextBoxName.Text = content.getName();
                TextBoxDescription.Text = content.getDescription();
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
        Server.Transfer("admincontentlist.aspx?Id=" + CurrId, false);
    }

    protected void SaveData(object sender, EventArgs e)
    {
        bosocontent content = new bosocontent();
        content.setCIN((string)Session["CIN"]);
        content.setId(CurrId);
        content.setMID(TextBoxMID.Text);
        content.setName(TextBoxName.Text);
        content.setDescription(TextBoxDescription.Text);
        if (content.SaveData())
        {
            Server.Transfer("admincontentlist.aspx?Id=" + content.getId(), false);
        }
        else
        {
            message.Text = content.getErrorMsg();
            message.BackColor = System.Drawing.Color.Red;
        }
    }

}
