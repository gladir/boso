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

public partial class admin : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        new bosolanguage().SwitchLanguage(Request.QueryString.Get("To"), toLanguage);
        bosotemplate template = new bosotemplate();
        List<string> label = template.switchTagsLanguage((string)Session["LanguageMID"], new string[] { "login", "cincolon", "useridcolon", "passwordcolon" });
        if (label.Count > 0)
        {
            lblLogin.Text = label[0];
            lblCin.Text = label[1];
            lblUserId.Text = label[2];
            lblPassword.Text = label[3];
        }
        UserIdLogin.Focus();
    }

    protected void LoginSubmit(Object sender, System.Web.UI.ImageClickEventArgs e)
    {
        message.Text = ""; // Réinitialise les messages
        
        if ("" == UserIdLogin.Text)
        {
            message.Text = new bosomessage().LOGIN_USERID_REQUIRED;
            message.BackColor = System.Drawing.Color.Red;
            return;
        }

        if ("" == PasswordLogin.Text)
        {
            message.Text = new bosomessage().LOGIN_PASSWORD_REQUIRED;
            message.BackColor = System.Drawing.Color.Red;
            return;
        }
       
        // Teste les utilisateurs statiques
        if ((UserIdLogin.Text == System.Configuration.ConfigurationManager.AppSettings["SysAdminLogin"]) && (PasswordLogin.Text == System.Configuration.ConfigurationManager.AppSettings["SysAdminPassword"]))
        {
            Session["CIN"] = CinLogin.Text;
            Session["UserId"] = -1;
            Session["UserMid"] = "SYSADMIN";
            Session["Role"] = "SYSADMIN";

            message.Text = new bosomessage().WELCOME_SYSADMIN;
            message.BackColor = System.Drawing.Color.LightGreen;

            bosomaindata myConn = new bosomaindata();
            myConn.WriteAction(bosomaindata.typeAction.Login, "Connexion principal");

            Server.Transfer("homeservice.aspx",false);
        } else {
            bosouser user = new bosouser();
            if (user.Login(UserIdLogin.Text, PasswordLogin.Text,CinLogin.Text))
            {
                Session["CIN"] = user.getCIN();
                Session["UserId"] = user.getId().Value;
                Session["UserMid"] = user.getMID();
                Session["Role"] = user.getRoleMID();

                Server.Transfer("homeservice.aspx", false);
            }
            else
            {
                message.Text = user.getErrorMsg();
                message.BackColor = System.Drawing.Color.Red;
            }
        }
    }
}
