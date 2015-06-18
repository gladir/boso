// Nom du fichier:   bosocin.cs
// Nom du projet:    BOSO (Back-Office Service Object)
// Société:          Gladir.com
// Développeur:      Sylvain Maltais
// Date de création: 2008/02/28
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
/// La classe «bosocin» permet d'effectuer la gestion d'un CIN.
/// </summary>
public class bosocin : System.Web.UI.Page
{
    string CIN = "";        // Identificateur du CIN
    string Name = "";       // Nom du CIN
    int    LanguageId = 0;  // Langage du CIN
    string Status = "A";    // Indique l'état du client

    string errorMsg = "";          // Message d'erreur

    /// <summary>
    /// Ce constructeur permet d'initialiser l'objet.
    /// </summary>
    public bosocin()
	{
	}

    /// <summary>
    /// Ce constructeur permet de charger immédiatement les données d'un CIN
    /// en se basant sur son nom.
    /// </summary>
    /// <param name="cin">Ce paramètre permet d'indiquer un identificateur client</param>
    public bosocin(string cin)
    {
        if (!LoadData(cin)) errorMsg = new bosomessage().DATA_NOTFOUND;
    }

    /// <summary>
    /// Cette fonction permet de connaitre le message correspondant à la dernière
    /// erreur survenu dans la classe de «cin».
    /// </summary>
    /// <returns>Cette fonction retourne un message d'erreur s'il y a lieu.</returns>
    public string getErrorMsg()
    {
        return errorMsg;
    }

    /// <summary>
    /// Cette fonction permet d'effectuer le chargement des données concernant 
    /// le CIN spécifié.
    /// </summary>
    /// <param name="cin">Ce paramètre permet d'indiquer un identificateur client</param>
    /// <returns>Cette fonction retourne «true» si la lecture s'est correctement déroulé ou «false» si une erreur est survenu.</returns>
    public bool LoadData(string cin)
    {
        if ("" != cin)
        {
            bosomaindata myConn = new bosomaindata();
            SqlConnection Conn = myConn.getDBConnection();
            SqlCommand CmdEdit = new SqlCommand("select CIN,Name,LanguageId from bosocin where CIN=@cin", Conn);
            CmdEdit.Parameters.Add(new SqlParameter("@cin", cin));
            Conn.Open();
            SqlDataReader drEdit = CmdEdit.ExecuteReader(CommandBehavior.CloseConnection);
            if (drEdit.Read())
            {
                CIN = drEdit.GetString(0);
                Name = drEdit.GetString(1);
                LanguageId = drEdit.GetInt32(2);
            }
            Conn.Close();
            return true;
        }
        return false;
    }

    /// <summary>
    /// Cette fonction permet d'effectuer la sauvegarde (ajout ou modification) 
    /// des données concernant le CIN spécifié.
    /// </summary>
    /// <param name="newData">Ce paramètre d'indiquer s'il faut effectuer un ajout des données</param>
    /// <returns>Cette fonction retourne «true» si la sauvegarde s'est correctement déroulé ou «false» si une erreur est survenu.</returns>
    public bool SaveData(bool newData)
    {
        if ("" == CIN)
        {
            errorMsg = new bosomessage().CIN_ID_EXPECTED;
            return false;
        }

        if ("" == Name)
        {
            errorMsg = new bosomessage().CIN_NAME_EXPECTED;
            return false;
        }

        bosomaindata myConn = new bosomaindata();
        SqlConnection Conn = myConn.getDBConnection();
        SqlCommand Cmd;
        if (newData)
        {
            if (CINExist(CIN))
            {
                errorMsg = new bosomessage().CIN_ID_ALREADYEXIST;
                return false;
            }

            Cmd = new SqlCommand("INSERT INTO bosocin (" +
                                        "CIN, Name, LanguageId," +
                                        "Status, CreateDate,CreateUserId, LastDate, LastUserId" +
                                  ") VALUES (" +
                                        "@cin,@name,@languageid," +
                                        "@status,@createdate,@createuserid,@lastdate,@lastuserid" +
                                  ")", Conn);
            myConn.WriteAction(bosomaindata.typeAction.AddData, "CIN");
        }
        else
        {
            Cmd = new SqlCommand("update bosocin set " +
                                    "Name = @name, " +
                                    "LanguageId = @languageid, " +
                                    "Status = @status, " +
                                    "LastDate = @lastdate," +
                                    "LastUserId = @lastuserid " +
                               "where CIN = @cin", Conn);
            myConn.WriteAction(bosomaindata.typeAction.ModifiedData, "CIN");
        }
        Cmd.Parameters.Add(new SqlParameter("@cin", CIN));
        Cmd.Parameters.Add(new SqlParameter("@name", Name));
        Cmd.Parameters.Add(new SqlParameter("@languageid", LanguageId));
        Cmd.Parameters.Add(new SqlParameter("@status", Status));
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
    /// Cette fonction permet d'effectuer la suppression d'un CIN spécifié.
    /// </summary>
    /// <param name="cin">Ce paramètre permet d'indiquer un identificateur client</param>
    /// <returns>Cette fonction retourne «true» si la suppression s'est correctement déroulé ou «false» si une erreur est survenu.</returns>
    public bool Delete(string cin)
    {
        bosomaindata myConn = new bosomaindata();
        SqlConnection Conn = myConn.getDBConnection();
        SqlCommand Cmd = new SqlCommand("delete from bosocin where CIN = @cin", Conn);
        Cmd.Parameters.Add(new SqlParameter("@cin", cin));
        Conn.Open();
        try
        {
            Cmd.ExecuteNonQuery();
            myConn.WriteAction(bosomaindata.typeAction.DeleteData, "CIN");
            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Cette fonction permet d'indiquer si le CIN spécifié existe dans 
    /// la base de données.
    /// </summary>
    /// <param name="cin">Ce paramètre permet d'indiquer un identificateur client</param>
    /// <returns>Cette fonction retourne «true» si un CIN a été trouver ou «false» si aucun n'a été trouvé.</returns>
    public bool CINExist(string cin)
    {
        if ("" == cin) return false;
        bosomaindata myConn = new bosomaindata();
        SqlConnection Conn = myConn.getDBConnection();
        SqlCommand Cmd = new SqlCommand("select " +
                                            "bosocin.CIN " +
                                        "from bosocin " +
                                        "where CIN=@cin", Conn);
        Cmd.Parameters.Add(new SqlParameter("@cin", cin));
        Conn.Open();
        SqlDataReader drEdit = Cmd.ExecuteReader(CommandBehavior.CloseConnection);
        if (drEdit.Read())
        {
            CIN = drEdit.GetString(0);
            return true;
        }
        return false;
    }

    /// <summary>
    /// Cette fonction permet de demander la valeur actuel du champs de l'identificateur client dans l'objet.
    /// </summary>
    /// <returns>Cette fonction retourner une chaine de caractères contenant l'identificateur client</returns>
    public string getCIN()
    {
        return CIN;
    }

    /// <summary>
    /// Cette procédure permet de demander la valeur actuel du champs de l'identificateur client dans l'objet.
    /// </summary>
    /// <param name="cin">Ce paramètre permet d'indiquer la nouvelle valeur que doit obtenir l'identificateur client.
    /// </param>
    public void setCIN(string cin)
    {
        CIN = cin;
    }

    /// <summary>
    /// Cette fonction permet de demander la valeur actuel du champs d'identificateur de langage dans l'objet.
    /// </summary>
    /// <returns>Cette fonction retourner un entier contenant l'identificateur de langage</returns>
    public int getLanguageId()
    {
        return LanguageId;
    }

    /// <summary>
    /// Cette procédure permet de demander la valeur actuel du champs de l'identificateur de langage dans l'objet.
    /// </summary>
    /// <param name="languageId">Ce paramètre permet d'indiquer la nouvelle valeur que doit obtenir l'identificateur de langage.
    /// </param>
    public void setLanguageId(int languageId)
    {
        LanguageId = languageId;
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
    /// Cette fonction permet de demander la valeur actuel du champs d'état dans l'objet.
    /// </summary>
    /// <returns>Cette fonction retourner une chaine de caractères contenant l'état</returns>
    public string getStatus()
    {
        return Status;
    }

    /// <summary>
    /// Cette procédure permet de demander la valeur actuel du champs d'état dans l'objet.
    /// </summary>
    /// <param name="status">Ce paramètre permet d'indiquer la nouvelle valeur que doit obtenir l'état: A=Autorisé, C=A confirmé, I=Inactif
    /// </param>
    public void setStatus(string status)
    {
        Status = status;
    }

    /// <summary>
    /// Cette fonction permet de charger la liste des CINs.
    /// </summary>
    /// <param name="field">Ce paramètre permet d'indiquer la liste des champs à retourner</param>
    /// <param name="sort">Ce paramètre permet d'indiquer l'ordre de tri</param>
    /// <returns>Cette fonction retourne objet «DataView» contenant la liste des CINs.</returns>
    public DataView LoadList(string[] field, string sort)
    {
        bosomaindata myConn = new bosomaindata();
        SqlConnection Conn = myConn.getDBConnection();
        DataSet ds = new DataSet();
        SqlDataAdapter Cmd = new SqlDataAdapter("select " + String.Join(",", field) + " from bosocin order by " + sort, Conn);
        Cmd.Fill(ds, "mylist");
        return ds.Tables["mylist"].DefaultView;
    }
}
