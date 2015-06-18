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

public partial class adminusersecurity : System.Web.UI.Page
{
    int? CurrId = null;
    
    protected void Page_Load(object sender, EventArgs e)
    {
        new bosoauto().CheckAutorizationSystemAdministrator();
        bosotemplate myTemplate = new bosotemplate("templates\\adminheader.tpl");
        myTemplate.assign("SwitchLanguage", new bosolanguage().SwitchLanguage("adminusersecurity.aspx?Id=" + Request.QueryString.Get("Id"), Request.QueryString.Get("To")));
        List<string> label = myTemplate.switchTagsLanguage((string)Context.Session["LanguageMID"], new string[] { "usersecurity", "useridcolon", "passwordcolon", "passwordconfirmcolon", "savebutton", "cancelbutton" });
        myTemplate.assign("Title", Title = label[0]);
        myTemplate.switchTagsSmarty();
        useridcolon.Text = label[1];
        passwordcolon.Text = label[2];
        passwordconfirmcolon.Text = label[3];
        Confirm.Text = label[4];
        Cancel.Text = label[5];
        headertemplate.Text = myTemplate.getTemplate();

        MotPass1.Focus();

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
                bosouser user = new bosouser(CurrId.Value);
                UserId.Text = user.getMID();
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
        Server.Transfer("adminuserlist.aspx?Id=" + CurrId, false);
    }

    protected void SaveData(object sender, EventArgs e)
    {
        bosouser user = new bosouser();
        user.setId(CurrId);
        if (user.SavePassword(MotPass1.Text, MotPass2.Text))
        {
            Server.Transfer("adminuserlist.aspx?Id=" + user.getId(), false);
        }
        else
        {
            message.Text = user.getErrorMsg();
            message.BackColor = System.Drawing.Color.Red;
        }
    }
}
