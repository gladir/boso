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

public partial class adminuseredit : System.Web.UI.Page
{
    int? CurrId = null;
    
    protected void Page_Load(object sender, EventArgs e)
    {
        int CurrRole = 0;
        string CurrCin = "";

        new bosoauto().CheckAutorizationAdministratorOrSystemAdministrator();
        bosotemplate myTemplate = new bosotemplate("templates\\adminheader.tpl");
        myTemplate.assign("SwitchLanguage", new bosolanguage().SwitchLanguage("adminuseredit.aspx?Id=" + Request.QueryString.Get("Id"), Request.QueryString.Get("To")));
        List<string> label = myTemplate.switchTagsLanguage((string)Context.Session["LanguageMID"], new string[] { "useredit", "cincolon", "useridcolon", "primaryrolecolon", "firstnamecolon", "lastnamecolon", "emailcolon", "listnone", "savebutton", "cancelbutton" });
        myTemplate.assign("Title", Title = label[0]);
        myTemplate.switchTagsSmarty();
        cincolon.Text = label[1];
        useridcolon.Text = label[2];
        primaryrolecolon.Text = label[3];
        firstnamecolon.Text = label[4];
        lastnamecolon.Text = label[5];
        emailcolon.Text = label[6];
        Confirm.Text = label[8];
        Cancel.Text = label[9];
        headertemplate.Text = myTemplate.getTemplate();

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
                bosouser user = new bosouser((string)Session["CIN"], CurrId.Value);
                TextBoxUserMID.Text = user.getMID();
                TextBoxFirstName.Text = user.getFirstName();
                TextBoxLastName.Text = user.getLastName();
                TextBoxEmail.Text = user.getEmail();
                CurrRole = user.getRoleId();
                CurrCin = user.getCIN();
            }

            if ((null != (string)Session["CIN"]) && ("" != (string)Session["CIN"]))
            {
                CurrCin = (string)Session["CIN"];
                cincolon.Visible = false;
                TextBoxCIN.Visible = false;
            }

            this.TextboxUserRole.Items.Clear();
            this.TextboxUserRole.Items.Add(new ListItem(label[7], ""));
            try
            {
                int I = 1;
                bosomaindata myConn = new bosomaindata();
                SqlConnection ConnRole = myConn.getDBConnection();
                SqlCommand CmdRole = new SqlCommand("SELECT " +
                                                       "RoleId, MID, Name, Description " +
                                                    "FROM bosorole ORDER BY MID", ConnRole);
                ConnRole.Open();
                SqlDataReader drRole = CmdRole.ExecuteReader(CommandBehavior.CloseConnection);
                while (drRole.Read())
                {
                    this.TextboxUserRole.Items.Add(new ListItem(drRole.GetString(1) + ": " + drRole.GetString(2), Convert.ToString(drRole.GetInt32(0))));
                    if (CurrRole == drRole.GetInt32(0))
                    {
                        this.TextboxUserRole.SelectedIndex = I;
                    }
                    I += 1;
                }
                ConnRole.Close();
            }
            catch
            {
            }

            this.TextBoxCIN.Items.Clear();
            this.TextBoxCIN.Items.Add(new ListItem(label[7], ""));
            try
            {
                int I = 1;
                bosomaindata myConn = new bosomaindata();
                SqlConnection ConnCin = myConn.getDBConnection();
                SqlCommand CmdCin = new SqlCommand("SELECT " +
                                                       "CIN, Name " +
                                                    "FROM bosocin ORDER BY CIN", ConnCin);
                ConnCin.Open();
                SqlDataReader drCin = CmdCin.ExecuteReader(CommandBehavior.CloseConnection);
                while (drCin.Read())
                {
                    this.TextBoxCIN.Items.Add(new ListItem(drCin.GetString(0) + ": " + drCin.GetString(1), drCin.GetString(0)));
                    if (CurrCin == drCin.GetString(0))
                    {
                        this.TextBoxCIN.SelectedIndex = I;
                    }
                    I += 1;
                }
                ConnCin.Close();
            }
            catch
            {
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
        bosouser user   = new bosouser();
        user.setId(CurrId);
        try
        {
            user.setRoleId(Convert.ToInt32(TextboxUserRole.SelectedItem.Value));
        }
        catch
        {
        }
        user.setCIN(TextBoxCIN.SelectedItem.Value);
        user.setMID(TextBoxUserMID.Text);
        user.setFirstName(TextBoxFirstName.Text);
        user.setLastName(TextBoxLastName.Text);
        user.setEmail(TextBoxEmail.Text);
        if (user.SaveData())
        {
            Server.Transfer("adminuserlist.aspx?Id=" + user.getId(), false);
        } else {
            message.Text = user.getErrorMsg();
            message.BackColor = System.Drawing.Color.Red;
        }
    }
}
