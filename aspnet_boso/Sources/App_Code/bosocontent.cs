// Nom du fichier:   bosocontent.cs
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
/// La classe «bosocontent» permet d'effectuer la gestion d'un contenu au
/// niveau de la base de données.
/// </summary>
public class bosocontent : System.Web.UI.Page
{
    int? CurrId = null;      // Identificateur de base de données
    string CIN = "";         // CIN du contenu
    string MID = "";         // MID du contenu
    string Name = "";        // Nom du contenu
    string Description = ""; // Description dans le contenu
    string Status = "A";     // État de publication du contenu

    string errorMsg = "";        // Message d'erreur

    /// <summary>
    /// Ce constructeur permet d'initialiser l'objet.
    /// </summary>
    public bosocontent()
	{
	}

    /// <summary>
    /// Ce constructeur permet de charger immédiatement les données d'un contenu
    /// en se basant sur son identificateur.
    /// </summary>
    /// <param name="currId">Ce paramètre permet d'indiquer l'identificateur du contenu</param>
    public bosocontent(int currId)
    {
        if (!LoadData(currId)) {
            errorMsg = new bosomessage().DATA_NOTFOUND;
        }
    }

    /// <summary>
    /// Ce constructeur permet de charger immédiatement les données d'un contenu
    /// en se basant sur son identificateur.
    /// </summary>
    /// <param name="cin">Ce paramètre permet d'indiquer un identificateur client</param>
    /// <param name="currId">Ce paramètre permet d'indiquer l'identificateur du contenu</param>
    public bosocontent(string cin, int currId)
    {
        if (!LoadData(cin,currId))
        {
            errorMsg = new bosomessage().DATA_NOTFOUND;
        }
    }

    /// <summary>
    /// Ce constructeur permet de charger immédiatement les données d'un contenu
    /// en se basant sur son MID.
    /// </summary>
    /// <param name="cin">Ce paramètre permet d'indiquer un identificateur client</param>
    /// <param name="mid">Ce paramètre permet d'indiquer un modèle d'identificateur</param>
    public bosocontent(string cin, string mid)
    {
        if (ContentMIDExist(cin, mid))
        {
            if (!LoadData(cin, CurrId.Value))
            {
                errorMsg = new bosomessage().DATA_NOTFOUND;
            }
        }
        else
        {
            errorMsg = new bosomessage().MID_NOTFOUND;
        }
    }

    /// <summary>
    /// Cette fonction permet de connaitre le message correspondant à la dernière
    /// erreur survenu dans la classe de «content».
    /// </summary>
    /// <returns>Cette fonction retourne un message d'erreur s'il y a lieu.</returns>
    public string getErrorMsg()
    {
        return errorMsg;
    }

    /// <summary>
    /// Cette fonction permet d'effectuer le chargement des données concernant 
    /// le contenu spécifié.
    /// </summary>
    /// <param name="currId">Ce paramètre permet d'indiquer l'identificateur du contenu</param>
    /// <returns>Cette fonction retourne «true» si la lecture s'est correctement déroulé ou «false» si une erreur est survenu.</returns>
    public bool LoadData(int currId)
    {
        return LoadData(null, currId);
    }

    /// <summary>
    /// Cette fonction permet d'effectuer le chargement des données concernant 
    /// le contenu spécifié.
    /// </summary>
    /// <param name="cin">Ce paramètre permet d'indiquer un identificateur client</param>
    /// <param name="currId">Ce paramètre permet d'indiquer l'identificateur du contenu</param>
    /// <returns>Cette fonction retourne «true» si la lecture s'est correctement déroulé ou «false» si une erreur est survenu.</returns>
    public bool LoadData(string cin, int currId)
    {
        if (0 != currId)
        {
            if ("" == cin) cin = null;
            
            bosomaindata myConn = new bosomaindata();
            SqlConnection Conn = myConn.getDBConnection();
            SqlCommand CmdEdit = new SqlCommand("select ContentId,MID,Name,Description,Status,CIN from bosocontent where " + (null != cin ? "CIN=@cin and " : "") + " ContentId=@contentid", Conn);
            if (null != cin) CmdEdit.Parameters.Add(new SqlParameter("@cin", cin));
            CmdEdit.Parameters.Add(new SqlParameter("@contentid", currId));
            Conn.Open();
            SqlDataReader drEdit = CmdEdit.ExecuteReader(CommandBehavior.CloseConnection);
            if (drEdit.Read())
            {
                CurrId = drEdit.GetInt32(0);
                MID = drEdit.GetString(1);
                Name = drEdit.GetString(2);
                Description = drEdit.GetString(3);
                Status = drEdit.GetString(4);
                CIN = drEdit.GetString(5);
            }
            Conn.Close();
            return true;
        }
        return false;
    }

    /// <summary>
    /// Cette fonction permet d'effectuer la sauvegarde (ajout ou modification) 
    /// des données concernant le contenu spécifié.
    /// </summary>
    /// <returns>Cette fonction retourne «true» si la sauvegarde s'est correctement déroulé ou «false» si une erreur est survenu.</returns>
    public bool SaveData()
    {
        if ("" == CIN) CIN = null;
        
        if ("" == MID)
        {
            errorMsg = new bosomessage().MID_EXPECTED;
            return false;
        }

        if ("" == Name)
        {
            errorMsg = new bosomessage().CONTENTNAME_EXPECTED;
            return false;
        }

        if ("" == Description)
        {
            errorMsg = new bosomessage().CONTENT_EXPECTED;
            return false;
        }

        bosomaindata myConn = new bosomaindata();
        SqlConnection Conn = myConn.getDBConnection();
        SqlCommand Cmd;
        if ((!CurrId.HasValue) || (CurrId == 0))
        {
            if (ContentMIDExist(CIN,MID))
            {
                errorMsg = new bosomessage().MID_ALREADYEXIST;
                return false;
            }

            Cmd = new SqlCommand("INSERT INTO bosocontent (" +
                                        "ContentId, CIN, MID, Name, Description," +
                                        "Status, CreateDate,CreateUserId, LastDate, LastUserId" +
                                  ") VALUES (" +
                                        "@contentid,@cin,@mid,@name,@description," +
                                        "@status,@createdate,@createuserid,@lastdate,@lastuserid" +
                                  ")", Conn);
            Cmd.Parameters.Add(new SqlParameter("@contentid", CurrId = myConn.getCurrId()));
            myConn.WriteAction(bosomaindata.typeAction.AddData, "Content");
        }
        else
        {
            Cmd = new SqlCommand("update bosocontent set " +
                                    "MID = @mid, " +
                                    "CIN = @cin, " +
                                    "Name = @name, " +
                                    "Description = @description, " +
                                    "Status = @status, " +
                                    "LastDate = @lastdate," +
                                    "LastUserId = @lastuserid " +
                               "where " + (null != CIN ? "CIN=@cin and " : "") + " ContentId = @contentid", Conn);
            Cmd.Parameters.Add(new SqlParameter("@contentid", CurrId));
            myConn.WriteAction(bosomaindata.typeAction.ModifiedData, "Content");
        }
        Cmd.Parameters.Add(new SqlParameter("@mid", MID));
        if (null == CIN)
        {
            Cmd.Parameters.Add(new SqlParameter("@cin", ""));
        }
        else
        {
            Cmd.Parameters.Add(new SqlParameter("@cin", CIN));
        }
        Cmd.Parameters.Add(new SqlParameter("@name", Name));
        Cmd.Parameters.Add(new SqlParameter("@description", Description));
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
    /// Cette fonction permet d'effectuer la suppression de le contenu spécifié.
    /// </summary>
    /// <param name="cin">Ce paramètre permet d'indiquer un identificateur client</param>
    /// <param name="contentId">Ce paramètre permet d'indiquer l'identificateur du contenu</param>
    /// <returns>Cette fonction retourne «true» si la suppression s'est correctement déroulé ou «false» si une erreur est survenu.</returns>
    public bool Delete(string cin, int contentId)
    {
        if ("" == cin) cin = null;
        
        bosomaindata myConn = new bosomaindata();
        SqlConnection Conn = myConn.getDBConnection();
        SqlCommand Cmd = new SqlCommand("delete from bosocontent where " + (null != cin ? "CIN=@cin and " : "") + " ContentId = @contentid", Conn);
        Cmd.Parameters.Add(new SqlParameter("@contentid", contentId));
        if (null == cin)
        {
            Cmd.Parameters.Add(new SqlParameter("@cin", ""));
        }
        else
        {
            Cmd.Parameters.Add(new SqlParameter("@cin", cin));
        }
        Conn.Open();
        try
        {
            Cmd.ExecuteNonQuery();
            myConn.WriteAction(bosomaindata.typeAction.DeleteData, "Content");
            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Cette fonction permet d'effectuer la suppression de le contenu spécifié.
    /// </summary>
    /// <param name="contentId">Ce paramètre permet d'indiquer l'identificateur du contenu</param>
    /// <returns>Cette fonction retourne «true» si la suppression s'est correctement déroulé ou «false» si une erreur est survenu.</returns>
    public bool Delete(int contentId)
    {
        return Delete(null, contentId);
    }

    /// <summary>
    /// Cette fonction permet d'indiquer si le modèle d'identificateur 
    /// du contenu spécifié existe dans la base de données.
    /// </summary>
    /// <param name="cin">Ce paramètre permet d'indiquer un identificateur client</param>
    /// <param name="mid">Ce paramètre permet d'indiquer un modèle d'identificateur</param>
    /// <returns>Cette fonction retourne «true» si une contenu a été trouver ou «false» si aucun n'a été trouvé.</returns>
    public bool ContentMIDExist(string cin, string mid)
    {
        if ("" == cin) cin = null;
        if ("" == mid) return false;
        bosomaindata myConn = new bosomaindata();
        SqlConnection Conn = myConn.getDBConnection();
        SqlCommand Cmd = new SqlCommand("select " +
                                            "bosocontent.ContentId " +
                                        "from bosocontent " +
                                        "where " + (null != cin ? "CIN=@cin and " : "") + " MID=@mid", Conn);
        if (null == cin)
        {
            Cmd.Parameters.Add(new SqlParameter("@cin", ""));
        }
        else
        {
            Cmd.Parameters.Add(new SqlParameter("@cin", cin));
        }
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
    /// Cette fonction permet d'indiquer si le modèle d'identificateur 
    /// du contenu spécifié existe dans la base de données.
    /// </summary>
    /// <param name="mid">Ce paramètre permet d'indiquer un modèle d'identificateur</param>
    /// <returns>Cette fonction retourne «true» si une contenu a été trouver ou «false» si aucun n'a été trouvé.</returns>
    public bool ContentMIDExist(string mid)
    {
        return ContentMIDExist(null, mid);
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
    /// Cette fonction permet de charger la liste des contenus.
    /// </summary>
    /// <param name="cin">Ce paramètre permet d'indiquer un identificateur client</param>
    /// <param name="field">Ce paramètre permet d'indiquer la liste des champs à retourner</param>
    /// <param name="sort">Ce paramètre permet d'indiquer l'ordre de tri</param>
    /// <returns>Cette fonction retourne objet «DataView» contenant la liste des contenus.</returns>
    public DataView LoadList(string cin, string[] field, string sort)
    {
        if ("" == cin) cin = null;

        bosomaindata myConn = new bosomaindata();
        SqlConnection Conn = myConn.getDBConnection();
        DataSet ds = new DataSet();
        SqlDataAdapter Cmd = new SqlDataAdapter("select " + String.Join(",", field) + " from bosocontent " + (null != cin ? " where CIN='" + cin + "' " : "") + " order by " + sort, Conn);
        Cmd.Fill(ds, "mylist");
        return ds.Tables["mylist"].DefaultView;
    }
}
