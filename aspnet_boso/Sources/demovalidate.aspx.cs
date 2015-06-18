using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class demovalidate : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        bosovalidate validate = new bosovalidate();

        Response.Write("Tester de validation de courriel:<br />");
        Response.Write("Courriel «abc» est valide: "+validate.IsEmail("abc")+"<br />");
        Response.Write("Courriel «@» est valide: " + validate.IsEmail("@") + "<br />");
        Response.Write("Courriel «@abc.abc» est valide: " + validate.IsEmail("@abc.abc") + "<br />");
        Response.Write("Courriel «abc@gladir.com» est valide: " + validate.IsEmail("abc@gladir.com") + "<br />");
        Response.Write("Courriel «abc@@gladir.com» est valide: " + validate.IsEmail("abc@@gladir.com") + "<br />");
        Response.Write("Courriel «abc@gl][adir.com» est valide: " + validate.IsEmail("abc@gl][adir.com") + "<br />");
    }
}
