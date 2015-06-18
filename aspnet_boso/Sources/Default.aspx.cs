using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class _Default : System.Web.UI.Page 
{
    protected void Page_Load(object sender, EventArgs e)
    {
        new bosolanguage().SwitchLanguage(Request.QueryString.Get("To"), toLanguage);
        if ("en" == (string)Session["LanguageMID"])
        {
            welcomeMsg.Text = "Welcome on the projet prototype of <i>Back-Office Service Object</i>. A solution in C# (C Sharp) for universal management of back-office services for multiple customer.";
            adminlink.Text = "Click here for admin access";
            titleDemo.Text = "Internal demo:";
            demotemplate.Text = "Check template method";
            demovalidate.Text = "Check validate method";
        }
        else
        {
            welcomeMsg.Text = "Bienvenue sur le prototype du projet <i>Back-Office Service Object</i>. Une solution en C# (C Sharp) permettant d'effectuer la gestion universel de services d'arrière-plan de multiple client.";
            adminlink.Text = "Cliquez ici pour accéder à l'administration";
            titleDemo.Text = "Démonstrateur de principe:";
            demotemplate.Text = "Tester le mécanisme de template";
            demovalidate.Text = "Tester le mécanisme de validation";
        }

    }
}