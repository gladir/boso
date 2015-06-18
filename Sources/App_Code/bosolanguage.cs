// Nom du fichier:   bosolanguage.cs
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
/// La classe «bosolanguage» permet d'effectuer la gestion des langages 
/// humains (HAL).
/// </summary>
public class bosolanguage : System.Web.UI.Page
{
    int? CurrId = null;      // Identificateur de base de données
    string MID = "";         // MID du langage
    string Name_EN = "";     // Nom anglais
    string Name_FR = "";     // Nom français
    string Status = "A";     // État de publication

    string errorMsg = "";        // Message d'erreur

    /// <summary>
    /// Ce constructeur permet d'initialiser l'objet.
    /// </summary>
	public bosolanguage()
	{
	}

    /// <summary>
    /// Ce constructeur permet de charger immédiatement les données d'un langage
    /// en se basant sur son identificateur.
    /// </summary>
    /// <param name="currId">Ce paramètre permet d'indiquer l'identificateur du langage</param>
    public bosolanguage(int currId)
    {
        if (!LoadData(currId)) {
            errorMsg = new bosomessage().DATA_NOTFOUND;
        }
    }

    /// <summary>
    /// Cette fonction permet de connaitre le message correspondant à la dernière
    /// erreur survenu dans la classe de «language».
    /// </summary>
    /// <returns>Cette fonction retourne un message d'erreur s'il y a lieu.</returns>
    public string getErrorMsg()
    {
        return errorMsg;
    }

    /// <summary>
    /// Cette fonction permet d'effectuer le chargement des données concernant 
    /// le langage spécifié.
    /// </summary>
    /// <param name="currId">Ce paramètre permet d'indiquer l'identificateur du langage</param>
    /// <returns>Cette fonction retourne «true» si la lecture s'est correctement déroulé ou «false» si une erreur est survenu.</returns>
    public bool LoadData(int currId)
    {
        if (0 != currId)
        {
            bosomaindata myConn = new bosomaindata();
            SqlConnection Conn = myConn.getDBConnection();
            SqlCommand CmdEdit = new SqlCommand("select LanguageId,MID,Name_EN,Name_FR,Status from bosolanguage where LanguageId=@languageid", Conn);
            CmdEdit.Parameters.Add(new SqlParameter("@languageid", currId));
            Conn.Open();
            SqlDataReader drEdit = CmdEdit.ExecuteReader(CommandBehavior.CloseConnection);
            if (drEdit.Read())
            {
                CurrId = drEdit.GetInt32(0);
                MID = drEdit.GetString(1);
                Name_EN = drEdit.GetString(2);
                Name_FR = drEdit.GetString(3);
                Status = drEdit.GetString(4);
            }
            Conn.Close();
            return true;
        }
        return false;
    }

    /// <summary>
    /// Cette fonction permet d'effectuer le chargement des données concernant 
    /// le langage spécifié à partir de son MID.
    /// </summary>
    /// <param name="MID">Ce paramètre permet d'indiquer le modèle d'identificateur du langage</param>
    /// <returns>Cette fonction retourne «true» si la lecture s'est correctement déroulé ou «false» si une erreur est survenu.</returns>
    public bool LoadData(string MID)
    {
        if(LanguageMIDExist(MID)) {
            return LoadData(CurrId.Value);
        }
        return false;
    }

    /// <summary>
    /// Cette fonction permet d'effectuer la sauvegarde (ajout ou modification) 
    /// des données concernant le langage spécifié.
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
            if (LanguageMIDExist(MID))
            {
                errorMsg = new bosomessage().MID_ALREADYEXIST;
                return false;
            }

            Cmd = new SqlCommand("INSERT INTO bosolanguage (" +
                                        "LanguageId, MID, Name_EN, Name_FR," +
                                        "Status, CreateDate,CreateUserId, LastDate, LastUserId" +
                                  ") VALUES (" +
                                        "@languageid,@mid,@nameen,@namefr," +
                                        "@status,@createdate,@createuserid,@lastdate,@lastuserid" +
                                  ")", Conn);
            Cmd.Parameters.Add(new SqlParameter("@languageid", CurrId = myConn.getCurrId()));
            myConn.WriteAction(bosomaindata.typeAction.AddData, "Language");
        }
        else
        {
            Cmd = new SqlCommand("update bosolanguage set " +
                                    "MID = @mid, " +
                                    "Name_EN = @nameen, " +
                                    "Name_FR = @namefr, " +
                                    "Status = @status, " +
                                    "LastDate = @lastdate," +
                                    "LastUserId = @lastuserid " +
                               "where LanguageId = @languageid", Conn);
            Cmd.Parameters.Add(new SqlParameter("@languageid", CurrId));
            myConn.WriteAction(bosomaindata.typeAction.ModifiedData, "Language");
        }
        Cmd.Parameters.Add(new SqlParameter("@mid", MID));
        Cmd.Parameters.Add(new SqlParameter("@nameen", Name_EN));
        Cmd.Parameters.Add(new SqlParameter("@namefr", Name_FR));
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
    /// Cette fonction permet d'effectuer la suppression le langage spécifié.
    /// </summary>
    /// <param name="languageId">Ce paramètre permet d'indiquer l'identificateur du langage</param>
    /// <returns>Cette fonction retourne «true» si la lecture s'est correctement déroulé ou «false» si une erreur est survenu.</returns>
    public bool Delete(int languageId)
    {
        bosomaindata myConn = new bosomaindata();
        SqlConnection Conn = myConn.getDBConnection();
        SqlCommand Cmd = new SqlCommand("delete from bosolanguage where LanguageId = @languageid", Conn);
        Cmd.Parameters.Add(new SqlParameter("@languageid", languageId));
        Conn.Open();
        try
        {
            Cmd.ExecuteNonQuery();
            myConn.WriteAction(bosomaindata.typeAction.DeleteData, "Language");
            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Cette fonction permet d'indiquer si le modèle d'identificateur 
    /// du langage spécifié existe dans la base de données.
    /// </summary>
    /// <param name="mid">Ce paramètre permet d'indiquer un modèle d'identificateur</param>
    /// <returns>Cette fonction retourne «true» si un langage a été trouver ou «false» si aucun n'a été trouvé.</returns>
    public bool LanguageMIDExist(string mid)
    {
        if ("" == mid) return false;
        bosomaindata myConn = new bosomaindata();
        SqlConnection Conn = myConn.getDBConnection();
        SqlCommand Cmd = new SqlCommand("select " +
                                            "bosolanguage.LanguageId " +
                                        "from bosolanguage " +
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
    /// Cette fonction permet de passer de français à l'anglais à partir d'un simple lien.
    /// </summary>
    /// <param name="param">Ce paramètre permet d'indiquer un modèle d'identificateur de langage pour lequel il faut faire basculer le langage courant</param>
    public void SwitchLanguage(string param,HyperLink toLanguage) {
        if ("" != param && null != param)
        {
            Context.Session["LanguageMID"] = param;
        }
        switch ((string)Context.Session["LanguageMID"])
        {
            case "en":
                toLanguage.NavigateUrl += "?To=fr";
                if(LoadData("fr")) toLanguage.Text = Name_FR;
                break;
            default:
                toLanguage.NavigateUrl += "?To=en";
                if(LoadData("en")) toLanguage.Text = Name_EN;
                break;
        }
    }

    /// <summary>
    /// Cette fonction permet de retourner un lien pour de français à l'anglais à partir d'un simple lien.
    /// </summary>
    /// <param name="currentPageAspx">Ce paramètre permet le nom de la page ASP .NET courante</param>
    /// <param name="param">Ce paramètre permet d'indiquer un modèle d'identificateur de langage pour lequel il faut faire basculer le langage courant</param>
    /// <returns>Cette fonction retourne un balise de lien HTML permettant le changement de langage</returns>
    public string SwitchLanguage(string currentPageAspx,string param)
    {
        if ("" != param && null != param)
        {
            Context.Session["LanguageMID"] = param;
        }
        switch ((string)Context.Session["LanguageMID"])
        {
            case "en":
                LoadData("fr");
                return "<a style=\"background-color:Blue; color:White; \" href=\"" + currentPageAspx + (currentPageAspx.IndexOf('?') > 0 ? "&" : "?") + "To=fr\">" + Name_FR + "</a>";
                break;
            default:
                LoadData("en");
                return "<a style=\"background-color:Blue; color:White; \" href=\"" + currentPageAspx + (currentPageAspx.IndexOf('?') > 0 ? "&" : "?") + "To=en\">" + Name_EN + "</a>";
                break;
        }
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
    /// Cette fonction permet de charger la liste des langages.
    /// </summary>
    /// <param name="field">Ce paramètre permet d'indiquer la liste des champs à retourner</param>
    /// <param name="sort">Ce paramètre permet d'indiquer l'ordre de tri</param>
    /// <returns>Cette fonction retourne objet «DataView» contenant la liste des langages.</returns>
    public DataView LoadList(string[] field, string sort)
    {
        bosomaindata myConn = new bosomaindata();
        SqlConnection Conn = myConn.getDBConnection();
        DataSet ds = new DataSet();
        SqlDataAdapter Cmd = new SqlDataAdapter("select " + String.Join(",", field) + " from bosolanguage order by " + sort, Conn);
        Cmd.Fill(ds, "mylist");
        return ds.Tables["mylist"].DefaultView;
    }
}
