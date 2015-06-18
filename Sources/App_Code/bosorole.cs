// Nom du fichier:   bosorole.cs
// Nom du projet:    BOSO (Back-Office Service Object)
// Société:          Gladir.com
// Développeur:      Sylvain Maltais
// Date de création: 2008/02/27
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
/// La classe «bosorole» permet d'effectuer la gestion des rôles
/// </summary>
public class bosorole : System.Web.UI.Page
{
    int? CurrId = null;      // Identificateur de base de données
    string MID = "";         // Identificateur de modèle
    string Name = "";        // Nom du rôle
    string Description = ""; // Description du rôle

    string errorMsg = "";        // Message d'erreur

    /// <summary>
    /// Ce constructeur permet d'initialiser l'objet.
    /// </summary>
    public bosorole()
	{
	}

    /// <summary>
    /// Ce constructeur permet de charger immédiatement les données d'un rôle
    /// en se basant sur son identificateur.
    /// </summary>
    /// <param name="currId">Ce paramètre permet d'indiquer l'identificateur du rôle</param>
    public bosorole(int currId)
    {
        if (!LoadData(currId)) {
            errorMsg = new bosomessage().DATA_NOTFOUND;
        }
    }

    /// <summary>
    /// Cette fonction permet de connaitre le message correspondant à la dernière
    /// erreur survenu dans la classe de «role».
    /// </summary>
    /// <returns>Cette fonction retourne un message d'erreur s'il y a lieu.</returns>
    public string getErrorMsg()
    {
        return errorMsg;
    }

    /// <summary>
    /// Cette fonction permet d'effectuer le chargement des données concernant 
    /// le rôle spécifié.
    /// </summary>
    /// <param name="currId">Ce paramètre permet d'indiquer l'identificateur du rôle</param>
    /// <returns>Cette fonction retourne «true» si la lecture s'est correctement déroulé ou «false» si une erreur est survenu.</returns>
    public bool LoadData(int currId)
    {
        if (0 != currId)
        {
            bosomaindata myConn = new bosomaindata();
            SqlConnection Conn = myConn.getDBConnection();
            SqlCommand CmdEdit = new SqlCommand("select RoleId,MID,Name,Description from bosorole where RoleId=@RoleId", Conn);
            CmdEdit.Parameters.Add(new SqlParameter("@roleid", currId));
            Conn.Open();
            SqlDataReader drEdit = CmdEdit.ExecuteReader(CommandBehavior.CloseConnection);
            if (drEdit.Read())
            {
                CurrId = drEdit.GetInt32(0);
                MID = drEdit.GetString(1);
                Name = drEdit.GetString(2);
                Description = drEdit.GetString(3);
            }
            Conn.Close();
            return true;
        }
        return false;
    }

    /// <summary>
    /// Cette fonction permet d'effectuer la sauvegarde (ajout ou modification) 
    /// des données concernant le rôle spécifié.
    /// </summary>
    /// <returns>Cette fonction retourne «true» si la sauvegarde s'est correctement déroulé ou «false» si une erreur est survenu.</returns>
    public bool SaveData()
    {
        if ("" == MID)
        {
            errorMsg = new bosomessage().MID_EXPECTED;
            return false;
        }

        if ("" == Name)
        {
            errorMsg = new bosomessage().NAME_EXPECTED;
            return false;
        }

        bosomaindata myConn = new bosomaindata();
        SqlConnection Conn = myConn.getDBConnection();
        SqlCommand Cmd;
        if ((!CurrId.HasValue) || (CurrId == 0))
        {
            if (RoleMIDExist(MID))
            {
                errorMsg = new bosomessage().MID_ALREADYEXIST;
                return false;
            }

            Cmd = new SqlCommand("INSERT INTO bosorole (" +
                                        "RoleId, MID, Name, Description," +
                                        "CreateDate,CreateUserId, LastDate, LastUserId" +
                                  ") VALUES (" +
                                        "@roleid,@mid,@nom,@description," +
                                        "@createdate,@createuserid,@lastdate,@lastuserid" +
                                  ")", Conn);
            Cmd.Parameters.Add(new SqlParameter("@roleid", CurrId = myConn.getCurrId()));
            myConn.WriteAction(bosomaindata.typeAction.AddData, "Role");
        }
        else
        {
            Cmd = new SqlCommand("update bosorole set " +
                                    "MID = @mid, " +
                                    "Name = @nom, " +
                                    "Description = @description, " +
                                    "LastDate = @lastdate," +
                                    "LastUserId = @lastuserid " +
                               "where RoleId = @roleid", Conn);
            Cmd.Parameters.Add(new SqlParameter("@roleid", CurrId));
            myConn.WriteAction(bosomaindata.typeAction.ModifiedData, "Role");
        }
        Cmd.Parameters.Add(new SqlParameter("@mid", MID));
        Cmd.Parameters.Add(new SqlParameter("@nom", Name));
        Cmd.Parameters.Add(new SqlParameter("@description", Description));
        Cmd.Parameters.Add(new SqlParameter("@createdate", System.DateTime.Now));
        Cmd.Parameters.Add(new SqlParameter("@createuserid", Session["UserId"]));
        Cmd.Parameters.Add(new SqlParameter("@lastdate", System.DateTime.Now));
        Cmd.Parameters.Add(new SqlParameter("@lastuserid", Session["UserId"]));
        Conn.Open();
        Cmd.ExecuteNonQuery();
        Conn.Close();
        return true;
    }

    /// <summary>
    /// Cette fonction permet d'effectuer la suppression d'un rôle spécifié.
    /// </summary>
    /// <param name="roleId">Ce paramètre permet d'indiquer l'identificateur du rôle</param>
    /// <returns>Cette fonction retourne «true» si la suppression s'est correctement déroulé ou «false» si une erreur est survenu.</returns>
    public bool Delete(int roleId)
    {
        bosomaindata myConn = new bosomaindata();
        SqlConnection Conn = myConn.getDBConnection();
        SqlCommand Cmd = new SqlCommand("delete from bosorole where RoleId = @roleid", Conn);
        Cmd.Parameters.Add(new SqlParameter("@roleid", roleId));
        Conn.Open();
        try
        {
            Cmd.ExecuteNonQuery();
            myConn.WriteAction(bosomaindata.typeAction.DeleteData, "Role");
            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Cette fonction permet d'indiquer si le modèle d'identificateur 
    /// du rôle spécifié existe dans la base de données.
    /// </summary>
    /// <param name="mid">Ce paramètre permet d'indiquer un modèle d'identificateur</param>
    /// <returns>Cette fonction retourne «true» si un rôle a été trouver ou «false» si aucun n'a été trouvé.</returns>
    public bool RoleMIDExist(string mid)
    {
        if ("" == mid) return false;
        bosomaindata myConn = new bosomaindata();
        SqlConnection Conn = myConn.getDBConnection();
        SqlCommand Cmd = new SqlCommand("select " +
                                            "bosorole.RoleId " +
                                        "from bosorole " +
                                        "where MID=@mid", Conn);
        Cmd.Parameters.Add(new SqlParameter("@mid", mid));
        Conn.Open();
        SqlDataReader drEdit = Cmd.ExecuteReader(CommandBehavior.CloseConnection);
        if (drEdit.Read())
        {
            CurrId = drEdit.GetInt32(0);
            return true;
        }
        return false;
    }

    /// <summary>
    /// Cette fonction permet de demander la valeur actuel du champs de l'identificateur de l'objet.
    /// </summary>
    /// <returns>Cette fonction retourner une entier contenant l'identificateur</returns>
    public int? getId()
    {
        return CurrId;
    }

    /// <summary>
    /// Cette procédure permet de demander la valeur actuel du champs de l'identificateur de l'objet.
    /// </summary>
    /// <param name="currId">Ce paramètre permet d'indiquer la nouvelle valeur que doit obtenir l'identificateur.
    /// </param>
    public void setId(int? currId)
    {
        CurrId = currId;
    }

    /// <summary>
    /// Cette fonction permet de demander la valeur actuel du champs du modèle d'identificateur dans l'objet.
    /// </summary>
    /// <returns>Cette fonction retourner une chaine de caractères contenant le modèle d'identificateur</returns>
    public string getDescription()
    {
        return Description;
    }

    /// <summary>
    /// Cette procédure permet de demander la valeur actuel du champs de la description dans l'objet.
    /// </summary>
    /// <param name="name">Ce paramètre permet d'indiquer la nouvelle valeur que doit obtenir la description.
    /// </param>
    public void setDescription(string description)
    {
        Description = description;
    }

    /// <summary>
    /// Cette fonction permet de demander la valeur actuel du champs du modèle d'identificateur dans l'objet.
    /// </summary>
    /// <returns>Cette fonction retourner une chaine de caractères contenant le modèle d'identificateur</returns>
    public string getMID()
    {
        return MID;
    }

    /// <summary>
    /// Cette procédure permet de demander la valeur actuel du champs de le modèle d'identificateur dans l'objet.
    /// </summary>
    /// <param name="mid">Ce paramètre permet d'indiquer la nouvelle valeur que doit obtenir le modèle d'identificateur.
    /// </param>
    public void setMID(string mid)
    {
        MID = mid;
    }

    /// <summary>
    /// Cette fonction permet de demander la valeur actuel du champs du modèle d'identificateur dans l'objet.
    /// </summary>
    /// <returns>Cette fonction retourner une chaine de caractères contenant le modèle d'identificateur</returns>
    public string getName()
    {
        return Name;
    }

    /// <summary>
    /// Cette procédure permet de demander la valeur actuel du champs du nom dans l'objet.
    /// </summary>
    /// <param name="name">Ce paramètre permet d'indiquer la nouvelle valeur que doit obtenir le nom.
    /// </param>
    public void setName(string name)
    {
        Name = name;
    }

    /// <summary>
    /// Cette fonction permet de charger la liste des rôles.
    /// </summary>
    /// <param name="field">Ce paramètre permet d'indiquer la liste des champs à retourner</param>
    /// <param name="sort">Ce paramètre permet d'indiquer l'ordre de tri</param>
    /// <returns>Cette fonction retourne objet «DataView» contenant la liste des rôles.</returns>
    public DataView LoadList(string[] field, string sort)
    {
        bosomaindata myConn = new bosomaindata();
        SqlConnection Conn = myConn.getDBConnection();
        DataSet ds = new DataSet();
        SqlDataAdapter Cmd = new SqlDataAdapter("select " + String.Join(",", field) + " from bosorole order by " + sort, Conn);
        Cmd.Fill(ds, "mylist");
        return ds.Tables["mylist"].DefaultView;
    }
}
