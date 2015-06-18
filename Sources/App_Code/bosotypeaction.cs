// Nom du fichier:   bosotypeaction.cs
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
/// La classe «bosotypeaction» permet d'effectuer la gestion de type d'action.
/// </summary>
public class bosotypeaction : System.Web.UI.Page
{
    int? CurrId = null;      // Identificateur de base de données
    string MID = "";         // Identificateur modèle du type d'action
    string Name_EN = "";     // Nom en anglais
    string Name_FR = "";     // Nom en français

    string errorMsg = "";        // Message d'erreur

    /// <summary>
    /// Ce constructeur permet d'initialiser l'objet.
    /// </summary>
    public bosotypeaction()
	{
	}

    /// <summary>
    /// Ce constructeur permet de charger immédiatement les données d'un type d'action
    /// en se basant sur son identificateur.
    /// </summary>
    /// <param name="currId">Ce paramètre permet d'indiquer l'identificateur de type d'action.</param>
    public bosotypeaction(int currId)
    {
        if (!LoadData(currId)) {
            errorMsg = new bosomessage().DATA_NOTFOUND;
        }
    }

    /// <summary>
    /// Cette fonction permet de connaitre le message correspondant à la dernière
    /// erreur survenu dans la classe de «typeaction».
    /// </summary>
    /// <returns>Cette fonction retourne un message d'erreur s'il y a lieu.</returns>
    public string getErrorMsg()
    {
        return errorMsg;
    }

    /// <summary>
    /// Cette fonction permet d'effectuer le chargement des données concernant 
    /// le type d'action spécifié.
    /// </summary>
    /// <param name="currId">Ce paramètre permet d'indiquer l'identificateur du type d'action.</param>
    /// <returns>Cette fonction retourne «true» si la lecture s'est correctement déroulé ou «false» si une erreur est survenu.</returns>
    public bool LoadData(int currId)
    {
        if (0 != currId)
        {
            bosomaindata myConn = new bosomaindata();
            SqlConnection Conn = myConn.getDBConnection();
            SqlCommand CmdEdit = new SqlCommand("select TypeActionId,MID,Name_EN,Name_FR from bosotypeaction where TypeActionId=@typeactionid", Conn);
            CmdEdit.Parameters.Add(new SqlParameter("@typeactionid", currId));
            Conn.Open();
            SqlDataReader drEdit = CmdEdit.ExecuteReader(CommandBehavior.CloseConnection);
            if (drEdit.Read())
            {
                CurrId = drEdit.GetInt32(0);
                MID = drEdit.GetString(1);
                Name_EN = drEdit.GetString(2);
                Name_FR = drEdit.GetString(3);
            }
            Conn.Close();
            return true;
        }
        return false;
    }

    /// <summary>
    /// Cette fonction permet d'effectuer la sauvegarde (ajout ou modification) 
    /// des données concernant le type d'action spécifié.
    /// </summary>
    /// <returns>Cette fonction retourne «true» si la sauvegarde s'est correctement déroulé ou «false» si une erreur est survenu.</returns>
    public bool SaveData()
    {
        if ("" == MID)
        {
            errorMsg = new bosomessage().MID_EXPECTED;
            return false;
        }

        if ("" == Name_EN)
        {
            errorMsg = new bosomessage().NAME_EN_EXPECTED;
            return false;
        }

        if ("" == Name_FR)
        {
            errorMsg = new bosomessage().NAME_FR_EXPECTED;
            return false;
        }

        bosomaindata myConn = new bosomaindata();
        SqlConnection Conn = myConn.getDBConnection();
        SqlCommand Cmd;
        if ((!CurrId.HasValue) || (CurrId == 0))
        {
            if (TypeActionMIDExist(MID))
            {
                errorMsg = new bosomessage().MID_ALREADYEXIST;
                return false;
            }

            Cmd = new SqlCommand("INSERT INTO bosotypeaction (" +
                                        "TypeActionId, MID, Name_EN, Name_FR," +
                                        "CreateDate,CreateUserId, LastDate, LastUserId" +
                                  ") VALUES (" +
                                        "@typeactionid,@mid,@nameen,@namefr," +
                                        "@createdate,@createuserid,@lastdate,@lastuserid" +
                                  ")", Conn);
            Cmd.Parameters.Add(new SqlParameter("@typeactionid", CurrId = myConn.getCurrId()));
            myConn.WriteAction(bosomaindata.typeAction.AddData, "Type d'action");
        }
        else
        {
            Cmd = new SqlCommand("update bosotypeaction set " +
                                   "MID = @mid, " +
                                   "Name_EN = @nameen, " +
                                   "Name_FR = @namefr, " +
                                   "LastDate = @lastdate," +
                                   "LastUserId = @lastuserid " +
                              "where TypeActionId = @typeactionid", Conn);
            Cmd.Parameters.Add(new SqlParameter("@typeactionid", CurrId));
            myConn.WriteAction(bosomaindata.typeAction.ModifiedData, "Type d'action");
        }
        Cmd.Parameters.Add(new SqlParameter("@mid", MID));
        Cmd.Parameters.Add(new SqlParameter("@nameen", Name_EN));
        Cmd.Parameters.Add(new SqlParameter("@namefr", Name_FR));
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
    /// Cette fonction permet d'effectuer la suppression du type d'action spécifié.
    /// </summary>
    /// <param name="contentId">Ce paramètre permet d'indiquer l'identificateur du type d'action</param>
    /// <returns>Cette fonction retourne «true» si la suppression s'est correctement déroulé ou «false» si une erreur est survenu.</returns>
    public bool Delete(int contentId)
    {
        bosomaindata myConn = new bosomaindata();
        SqlConnection Conn = myConn.getDBConnection();
        SqlCommand Cmd = new SqlCommand("delete from bosotypeaction where TypeActionId = @typeactionid", Conn);
        Cmd.Parameters.Add(new SqlParameter("@typeactionid", contentId));
        Conn.Open();
        try
        {
            Cmd.ExecuteNonQuery();
            myConn.WriteAction(bosomaindata.typeAction.DeleteData, "Type d'action");
            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Cette fonction permet d'indiquer si le modèle d'identificateur 
    /// du type d'action spécifié existe dans la base de données.
    /// </summary>
    /// <param name="mid">Ce paramètre permet d'indiquer un modèle d'identificateur</param>
    /// <returns>Cette fonction retourne «true» si un type d'action a été trouver ou «false» si aucun n'a été trouvé.</returns>
    public bool TypeActionMIDExist(string mid)
    {
        if ("" == mid) return false;
        bosomaindata myConn = new bosomaindata();
        SqlConnection Conn = myConn.getDBConnection();
        SqlCommand Cmd = new SqlCommand("select " +
                                            "bosotypeaction.TypeActionId " +
                                        "from bosotypeaction " +
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
    /// Cette fonction permet de demander la valeur actuel du champs du nom anglais dans l'objet.
    /// </summary>
    /// <returns>Cette fonction retourner une chaine de caractères contenant le nom anglais</returns>
    public string getName_EN()
    {
        return Name_EN;
    }

    /// <summary>
    /// Cette procédure permet de demander la valeur actuel du champs du nom anglais dans l'objet.
    /// </summary>
    /// <param name="name_EN">Ce paramètre permet d'indiquer la nouvelle valeur que doit obtenir le nom anglais.
    /// </param>
    public void setName_EN(string name_EN)
    {
        Name_EN = name_EN;
    }

    /// <summary>
    /// Cette fonction permet de demander la valeur actuel du champs du nom français dans l'objet.
    /// </summary>
    /// <returns>Cette fonction retourner une chaine de caractères contenant le nom français</returns>
    public string getName_FR()
    {
        return Name_FR;
    }

    /// <summary>
    /// Cette procédure permet de demander la valeur actuel du champs du nom français dans l'objet.
    /// </summary>
    /// <param name="name_FR">Ce paramètre permet d'indiquer la nouvelle valeur que doit obtenir le nom français.
    /// </param>
    public void setName_FR(string name_FR)
    {
        Name_FR = name_FR;
    }

    /// <summary>
    /// Cette fonction permet de charger la liste des types d'action.
    /// </summary>
    /// <param name="field">Ce paramètre permet d'indiquer la liste des champs à retourner</param>
    /// <param name="sort">Ce paramètre permet d'indiquer l'ordre de tri</param>
    /// <returns>Cette fonction retourne objet «DataView» contenant la liste des types d'action.</returns>
    public DataView LoadList(string[] field, string sort)
    {
        bosomaindata myConn = new bosomaindata();
        SqlConnection Conn = myConn.getDBConnection();
        DataSet ds = new DataSet();
        SqlDataAdapter Cmd = new SqlDataAdapter("select " + String.Join(",", field) + " from bosotypeaction order by " + sort, Conn);
        Cmd.Fill(ds, "mylist");
        return ds.Tables["mylist"].DefaultView;
    }
}
