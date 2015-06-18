// Nom du fichier:   admincinedit.aspx.cs
// Nom du projet:    BOSO (Back-Office Service Object)
// Société:          Gladir.com
// Développeur:      Sylvain Maltais
// Date de création: 2008/02/28
// Version:          0.1
//
///////////////
// Description:      Cette page permet d'effectuer la gestion d'un CIN.

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

public partial class admincinedit : System.Web.UI.Page
{
    string CurrId = "";

    // Cette procédure est chargée automatiquement avant de commencer la page.
    protected void Page_Load(object sender, EventArgs e)
    {
        int CurrLanguage = 0;

        new bosoauto().CheckAutorizationSystemAdministrator();
        bosotemplate myTemplate = new bosotemplate("templates\\adminheader.tpl");
        myTemplate.assign("SwitchLanguage", new bosolanguage().SwitchLanguage("admincinedit.aspx?Id=" + Request.QueryString.Get("Id"), Request.QueryString.Get("To")));
        List<string> label = myTemplate.switchTagsLanguage((string)Context.Session["LanguageMID"], new string[] { "cinedit", "namecolon", "languagecolon", "listnone", "savebutton", "cancelbutton" });
        myTemplate.assign("Title", Title = label[0]);
        myTemplate.switchTagsSmarty();
        namecolon.Text = label[1];
        languagecolon.Text = label[2];
        Confirm.Text = label[4];
        Cancel.Text = label[5];
        headertemplate.Text = myTemplate.getTemplate();

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
                bosocin cin = new bosocin(CurrId);
                TextBoxCIN.Text = cin.getCIN();
                TextBoxName.Text = cin.getName();
                CurrLanguage = cin.getLanguageId();
            }

            this.TextboxLanguage.Items.Clear();
            this.TextboxLanguage.Items.Add(new ListItem(label[3], ""));
            try
            {
                int I = 1;
                bosomaindata myConn = new bosomaindata();
                SqlConnection ConnLanguage = myConn.getDBConnection();
                SqlCommand CmdLanguage = new SqlCommand("SELECT " +
                                                            "LanguageId, MID, " + ("en" == (string)Context.Session["LanguageMID"] ? "Name_EN" : "Name_FR") + " As Name " +
                                                        "FROM bosolanguage ORDER BY Name", ConnLanguage);
                ConnLanguage.Open();
                SqlDataReader drLanguage = CmdLanguage.ExecuteReader(CommandBehavior.CloseConnection);
                while (drLanguage.Read())
                {
                    this.TextboxLanguage.Items.Add(new ListItem(drLanguage.GetString(2) + " (" + drLanguage.GetString(1)+")", Convert.ToString(drLanguage.GetInt32(0))));
                    if (CurrLanguage == drLanguage.GetInt32(0))
                    {
                        this.TextboxLanguage.SelectedIndex = I;
                    }
                    I++;
                }
                ConnLanguage.Close();
            }
            catch
            {
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

    // Cette procédure permet de retourner à la page parente (Gestion des CINs).
    protected void ReturnParent(object sender, EventArgs e)
    {
        Server.Transfer("admincinlist.aspx?Id=" + CurrId, false);
    }

    // Cette procédure permet de sauvegarder le contenu de la page.
    protected void SaveData(object sender, EventArgs e)
    {
        bosocin cin = new bosocin();
        try
        {
            cin.setLanguageId(Convert.ToInt32(TextboxLanguage.SelectedItem.Value));
        }
        catch
        {
        }
        cin.setCIN(TextBoxCIN.Text);
        cin.setName(TextBoxName.Text);
        if (cin.SaveData(null == CurrId || "" == CurrId))
        {
            Server.Transfer("admincinlist.aspx?Id=" + cin.getCIN(), false);
        }
        else
        {
            message.Text = cin.getErrorMsg();
            message.BackColor = System.Drawing.Color.Red;
        }
    }
}
