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

public partial class adminlogactionview : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        new bosoauto().CheckAutorizationSystemAdministrator();
        bosotemplate myTemplate = new bosotemplate("templates\\adminheader.tpl");
        myTemplate.assign("SwitchLanguage", new bosolanguage().SwitchLanguage("adminlogactionview.aspx", Request.QueryString.Get("To")));
        List<string> label = myTemplate.switchTagsLanguage((string)Context.Session["LanguageMID"], new string[] { "logaction", "date", "user", "ip", "message", "action" });
        myTemplate.assign("Title", Title = label[0]);
        myTemplate.switchTagsSmarty();
        LogDataGrid.Columns[1].HeaderText = label[1];//Date
        LogDataGrid.Columns[1].FooterText = label[1];//Date
        LogDataGrid.Columns[2].HeaderText = label[2];//Utilisateur
        LogDataGrid.Columns[2].FooterText = label[2];//Utilisateur
        LogDataGrid.Columns[3].HeaderText = label[3];//IP
        LogDataGrid.Columns[3].FooterText = label[3];//IP
        LogDataGrid.Columns[4].HeaderText = label[4];//Message
        LogDataGrid.Columns[4].FooterText = label[4];//Message
        LogDataGrid.Columns[5].HeaderText = label[5];//Action
        LogDataGrid.Columns[5].FooterText = label[5];//Action
        headertemplate.Text = myTemplate.getTemplate();

        if (!Page.IsPostBack) ShowListLog("bosolog.LogId desc");
    }

    protected void LogDataGrid_EventHandler(Object sender, DataGridSortCommandEventArgs e)
    {
        ShowListLog(e.SortExpression);
    }

    public void ShowListLog(string sort)
    {
        bosomaindata myConn = new bosomaindata();
        SqlConnection Conn = myConn.getDBConnection();
        DataSet ds = new DataSet();
        SqlDataAdapter Cmd = new SqlDataAdapter("select " +
                                                    "bosolog.LogId," +
                                                    "bosolog.Message, " +
                                                    "bosouser.MID As UserMid, " +
                                                    "bosolog.CreateDate, " +
                                                    "bosotypeaction.Name_FR As TypeActionName, " +
                                                    "bosolog.IPAddr " +
                                                "from bosolog " +
                                                "left join bosouser on bosolog.UserId=bosouser.UserId " +
                                                "left join bosotypeaction on bosolog.TypeAction=bosotypeaction.MID " +
                                                "order by " + sort, Conn);
        Cmd.Fill(ds, "contrib");
        LogDataGrid.DataSource = ds.Tables["contrib"].DefaultView;
        LogDataGrid.DataBind();
        for (int i = 0; i < LogDataGrid.Items.Count; i++)
        {
            if (LogDataGrid.Items[i].Cells[0].Text == Request.QueryString.Get("Id"))
            {
                LogDataGrid.SelectedIndex = i;
            }
        }
        Conn.Close();
    }
}
