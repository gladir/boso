// Nom du fichier:   bosomaindata.cs
// Nom du projet:    BOSO (Back-Office Service Object)
// Société:          Gladir.com
// Développeur:      Sylvain Maltais
// Date de création: 2008/02/21
// Version:          0.1
// Outils:           Visual Web Developer 2005 Express et Visual C# Express

using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

/// <summary>
/// Cette classe est la classe primaire pour la création de tous objets de 
/// manipulation de données. Il peut ainsi être possible d'effectuer un
/// journal de bord (logging) centralisé de tous les actions effectués par
/// tous les utilisateurs, y compris les administrateurs.
/// </summary>
public class bosomaindata : System.Web.UI.Page
{
    /// <summary>
    /// Cette définition de type permet d'indiquer tous les actions enregistrable
    /// </summary>
    public enum typeAction
    {
        Login = 1,         // Action d'effectuer une connexion
        ReadData = 2,      // Lecture ou visualisation d'un contenu
        AddData = 3,       // Ajout de données
        ModifiedData = 4,  // Modification de données
        DeleteData = 5,    // Suppression de données
        ListViewData = 6,  // Visualisation d'une liste de données

        SearchData = 7,    // Action d'effectuer une recherche quelconque (utilisé en particulier par les gouvernements et les banques)
        SetStatusData = 8, // Modifier qu'une information d'état, de publication,...
        ErrorAction = 254, // Action d'une erreur inconnu
        OtherAction = 255  // Autres actions inclassable
    }

    /// <summary>
    /// Ce constructeur permet d'initialiser l'objet.
    /// </summary>
    public bosomaindata()
	{
	}

    /// <summary>
    /// Cette procédure permet l'écriture d'une action utilisateur dans un
    /// journal de bord.
    /// </summary>
    /// <param name="TypeAction">Ce paramètre désigne le type d'action à inscrire dans le journal de bord.</param>
    /// <param name="UserId">Ce paramètre permet d'indiquer l'identificateur d'utilisateur.</param>
    /// <param name="Role">Ce paramètre permet d'indiquer le modèle d'identificateur de rôle.</param>
    /// <param name="Message">Ce paramètre permet d'indiquer le message texte à écrire dans le journal de bord.</param>
    public void WriteAction(typeAction TypeAction, int UserId, string Role, string Message) {
        //string IPCustomer = Request.UserHostAddress;
        SqlConnection Conn = getDBConnection();
        SqlCommand Cmd = new SqlCommand("INSERT INTO bosolog (" +
                                        "UserId,RoleMid,TypeAction,Message," +
                                        "IPAddr,CreateDate" +
                                  ") VALUES (" +
                                        "@userid,@RoleMid,@typeaction,@message," +
                                        "@IPAddr,@CreateDate" +
                                  ")", Conn);
        Cmd.Parameters.Add(new SqlParameter("@userid", UserId));
        Cmd.Parameters.Add(new SqlParameter("@RoleMid", Role));
        Cmd.Parameters.Add(new SqlParameter("@typeaction", TypeAction));
        Cmd.Parameters.Add(new SqlParameter("@message", Message));
        try
        {
            Cmd.Parameters.Add(new SqlParameter("@IPAddr", Context.Request.UserHostAddress));
        }
        catch
        {
            Cmd.Parameters.Add(new SqlParameter("@IPAddr", ""));
        }
        Cmd.Parameters.Add(new SqlParameter("@CreateDate", System.DateTime.Now));
        Conn.Open();
        Cmd.ExecuteNonQuery();
        Conn.Close();
    }

    /// <summary>
    /// Cette procédure permet l'écriture d'une action utilisateur dans un
    /// journal de bord.
    /// </summary>
    /// <param name="TypeAction">Ce paramètre désigne le type d'action à inscrire dans le journal de bord.</param>
    /// <param name="Message">Ce paramètre permet d'indiquer le message texte à écrire dans le journal de bord.</param>
    public void WriteAction(typeAction TypeAction, string Message)
    {
        WriteAction(TypeAction, (int)Session["UserId"], (string)Session["Role"], Message);
    }

    /// <summary>
    /// Cette fonction permet de demander la chaine de connexion à la base de données.
    /// </summary>
    /// <returns>Cette fonction retourne une chaine de caractères correspondant à la connexion à la base de données.</returns>
    public string getStrDBConnection()
    {
        return System.Configuration.ConfigurationManager.AppSettings["DBConnection"];
    }

    /// <summary>
    /// Cette fonction permet de demander l'objet de connexion à la base de données.
    /// </summary>
    /// <returns>Cette fonction retourne l'objet préparé pour effectuer une connexion à la base de données.</returns>
    public SqlConnection getDBConnection()
    {
        return new SqlConnection(getStrDBConnection());
    }

    /// <summary>
    /// Cette fonction permet de demander l'identificateur global de la base de données.
    /// </summary>
    /// <returns>Cette fonction retourne un numéro unique du prochaine identificateur a utilisé pour un enregistrement.</returns>
    public int getCurrId()
    {
        try {
            SqlConnection ConnEdit = getDBConnection();
            SqlCommand CmdEdit = new SqlCommand("select " +
                                                    "VariableValue " +
                                                "from bosoconfig " +
                                                "where VariableName='ObjectId'", ConnEdit);
            try
            {
                ConnEdit.Open();
                SqlDataReader drEdit = CmdEdit.ExecuteReader(CommandBehavior.CloseConnection);
                if (drEdit.Read())
                {
                    int result = Convert.ToInt32(drEdit.GetValue(0));
                    SqlConnection ConnUpdate = getDBConnection();
                    SqlCommand CmdUpdate = new SqlCommand("update bosoconfig set " +
                                                            "VariableValue=@myvalue " +
                                                        "where VariableName='ObjectId'", ConnUpdate);
                    CmdUpdate.Parameters.Add(new SqlParameter("@myvalue", result + 1));
                    ConnUpdate.Open();
                    CmdUpdate.ExecuteNonQuery();
                    ConnUpdate.Close();
                    return result+1;
                }
                else
                {
                    SqlConnection ConnAdd = getDBConnection();
                    SqlCommand CmdAdd = new SqlCommand("INSERT INTO bosoconfig (" +
                                                    "VariableName, VariableValue" +
                                              ") VALUES (" +
                                                    "'ObjectId','1'" +
                                              ")", ConnAdd);
                    ConnAdd.Open();
                    CmdAdd.ExecuteNonQuery();
                    ConnAdd.Close();
                }
            }
            finally
            {
                ConnEdit.Close();
            }
            return 1;
        }
        catch {
            return 1;
        }
    }
}
