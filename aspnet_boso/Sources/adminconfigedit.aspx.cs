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

public partial class adminconfigedit : System.Web.UI.Page
{
    string CurrId = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        new bosoauto().CheckAutorizationSystemAdministrator();
        bosotemplate myTemplate = new bosotemplate("templates\\adminheader.tpl");
        myTemplate.assign("SwitchLanguage", new bosolanguage().SwitchLanguage("adminconfigedit.aspx?Id=" + Request.QueryString.Get("Id"), Request.QueryString.Get("To")));
        List<string> label = myTemplate.switchTagsLanguage((string)Context.Session["LanguageMID"], new string[] { "configurationedit", "variablenamecolon", "variablevaluecolon", "savebutton", "cancelbutton" });
        myTemplate.assign("Title", Title = label[0]);
        myTemplate.switchTagsSmarty();
        headertemplate.Text = myTemplate.getTemplate();
        variablenamecolon.Text = label[1];
        variablevaluecolon.Text = label[2];
        Confirm.Text = label[3];
        Cancel.Text = label[4];
        TextBoxVariableName.Focus();

        if (!Page.IsPostBack)
        {
            try
            {
                if ("" != Request.QueryString.Get("Id")) CurrId = Request.QueryString.Get("Id");
            }
            catch
            {
                CurrId = null;
            }

            if (null != CurrId)
            {
                bosoconfig config = new bosoconfig(CurrId);
                TextBoxVariableName.Text = config.getVariableName();
                TextBoxVariableValue.Text = config.getVariableValue();
                TextBoxVariableName.ReadOnly = true;
            }
        }
        else
        {
            try
            {
                CurrId = Request.Form.Get("FormCurrId");
            }
            catch
            {
                CurrId = null;
            }
        }
        if (null != CurrId) FormCurrId.Value = CurrId;
    }

    protected void ReturnParent(object sender, EventArgs e)
    {
        Server.Transfer("adminconfiglist.aspx?Id=" + CurrId, false);
    }

    protected void SaveData(object sender, EventArgs e)
    {
        bosoconfig config = new bosoconfig();
        config.setNewVariableName(CurrId);
        config.setVariableName(TextBoxVariableName.Text);
        config.setVariableValue(TextBoxVariableValue.Text);
        if (config.SaveData())
        {
            Server.Transfer("adminconfiglist.aspx?Id=" + config.getVariableName(), false);
        }
        else
        {
            message.Text = config.getErrorMsg();
            message.BackColor = System.Drawing.Color.Red;
        }
    }
}
