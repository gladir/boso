// Nom du fichier:   bosoconfig.cs
// Nom du projet:    BOSO (Back-Office Service Office)
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
/// La classe «bosoconfig» permet d'effectuer la gestion des variables de 
/// configuation situé dans la base de données.
/// </summary>
public class bosoconfig : System.Web.UI.Page
{
    string NewVariableName = "";     // Nom de la variable
    string VariableName = "";        // Nom de la variable
    string VariableValue = "";       // Valeur de variable

    string errorMsg = "";        // Message d'erreur

    /// <summary>
    /// Ce constructeur permet d'initialiser l'objet.
    /// </summary>
    public bosoconfig()
	{
	}

    /// <summary>
    /// Ce constructeur permet de charger immédiatement les données d'une variable
    /// en se basant sur son nom.
    /// </summary>
    /// <param name="variableName">Ce paramètre permet d'indiquer le nom de la variable de configuration</param>
    public bosoconfig(string variableName)
    {
        if (!LoadData(variableName))
        {
            errorMsg = new bosomessage().DATA_NOTFOUND;
        }
    }

    /// <summary>
    /// Cette fonction permet de connaitre le message correspondant à la dernière
    /// erreur survenu dans la classe de «config».
    /// </summary>
    /// <returns>Cette fonction retourne un message d'erreur s'il y a lieu.</returns>
    public string getErrorMsg()
    {
        return errorMsg;
    }

    /// <summary>
    /// Cette fonction permet d'effectuer le chargement des données concernant 
    /// le config spécifié.
    /// </summary>
    /// <param name="variableName">Ce paramètre permet d'indiquer le nom de la variable de configuration</param>
    /// <returns>Cette fonction retourne «true» si la lecture s'est correctement déroulé ou «false» si une erreur est survenu.</returns>
    public bool LoadData(string variableName)
    {
        if ("" != variableName)
        {
            bosomaindata myConn = new bosomaindata();
            SqlConnection Conn = myConn.getDBConnection();
            SqlCommand CmdEdit = new SqlCommand("select VariableName,VariableValue from bosoconfig where VariableName=@variablename", Conn);
            CmdEdit.Parameters.Add(new SqlParameter("@variablename", variableName));
            Conn.Open();
            SqlDataReader drEdit = CmdEdit.ExecuteReader(CommandBehavior.CloseConnection);
            if (drEdit.Read())
            {
                NewVariableName = drEdit.GetString(0);
                VariableName = drEdit.GetString(0);
                VariableValue = drEdit.GetString(1);
            }
            Conn.Close();
            return true;
        }
        return false;
    }

    /// <summary>
    /// Cette fonction permet d'effectuer la sauvegarde (ajout ou modification) 
    /// des données concernant la variable spécifié.
    /// </summary>
    /// <returns>Cette fonction retourne «true» si la sauvegarde s'est correctement déroulé ou «false» si une erreur est survenu.</returns>
    public bool SaveData()
    {
        if ("" == VariableValue)
        {
            errorMsg = new bosomessage().VALUE_EXPECTED;
            return false;
        }

        if ("" == VariableName)
        {
            errorMsg = new bosomessage().NAME_EXPECTED;
            return false;
        }

        bosomaindata myConn = new bosomaindata();
        SqlConnection Conn = myConn.getDBConnection();
        SqlCommand Cmd;
        if (null == NewVariableName || "" == NewVariableName)
        {
            if (VariableNameExist(VariableName))
            {
                errorMsg = new bosomessage().VARIABLENAME_ALREADYEXIST;
                return false;
            }

            Cmd = new SqlCommand("INSERT INTO bosoconfig (" +
                                        "VariableName," +
                                        "VariableValue" +
                                  ") VALUES (" +
                                        "@variableName," +
                                        "@variableValue" +
                                  ")", Conn);
            Cmd.Parameters.Add(new SqlParameter("@variableName", VariableName));
            myConn.WriteAction(bosomaindata.typeAction.AddData, "Config");
        }
        else
        {
            Cmd = new SqlCommand("update bosoconfig set " +
                                    "VariableName = @newvariableName, " +
                                    "VariableValue = @variableValue " +
                               "where variableName = @variableName", Conn);
            Cmd.Parameters.Add(new SqlParameter("@variableName", VariableName));
            Cmd.Parameters.Add(new SqlParameter("@newvariableName", NewVariableName));
            myConn.WriteAction(bosomaindata.typeAction.ModifiedData, "Config");
        }
        Cmd.Parameters.Add(new SqlParameter("@variableValue", VariableValue));
        Conn.Open();
        Cmd.ExecuteNonQuery();
        Conn.Close();
        return true;
    }

    /// <summary>
    /// Cette fonction permet d'effectuer la suppression d'un config spécifié.
    /// </summary>
    /// <param name="variableName">Ce paramètre permet d'indiquer le nom de la variable de configuration</param>
    /// <returns>Cette fonction retourne «true» si la suppression s'est correctement déroulé ou «false» si une erreur est survenu.</returns>
    public bool Delete(string variableName)
    {
        bosomaindata myConn = new bosomaindata();
        SqlConnection Conn = myConn.getDBConnection();
        SqlCommand Cmd = new SqlCommand("delete from bosoconfig where VariableName = @variableName", Conn);
        Cmd.Parameters.Add(new SqlParameter("@variableName", variableName));
        Conn.Open();
        try
        {
            Cmd.ExecuteNonQuery();
            myConn.WriteAction(bosomaindata.typeAction.DeleteData, "Config");
            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Cette fonction permet d'indiquer si le nom
    /// de la variable spécifié existe dans la base de données.
    /// </summary>
    /// <param name="variableName">Ce paramètre permet d'indiquer le nom de la variable de configuration</param>
    /// <returns>Cette fonction retourne «true» si une variable a été trouver ou «false» si aucun n'a été trouvé.</returns>
    public bool VariableNameExist(string variableName)
    {
        if ("" == variableName) return false;
        bosomaindata myConn = new bosomaindata();
        SqlConnection Conn = myConn.getDBConnection();
        SqlCommand Cmd = new SqlCommand("select " +
                                            "bosoconfig.VariableValue " +
                                        "from bosoconfig " +
                                        "where VariableName=@variableName", Conn);
        Cmd.Parameters.Add(new SqlParameter("@variableName", variableName));
        Conn.Open();
        SqlDataReader drEdit = Cmd.ExecuteReader(CommandBehavior.CloseConnection);
        if (drEdit.Read())
        {
            VariableValue = drEdit.GetString(0);
            return true;
        }
        return false;
    }

    /// <summary>
    /// Cette fonction permet de demander la valeur actuel du champs de nouveau nom de variable dans l'objet.
    /// </summary>
    /// <returns>Cette fonction retourner une chaine de caractères contenant le nouveau nom de variable</returns>
    public string getNewVariableName()
    {
        return NewVariableName;
    }

    /// <summary>
    /// Cette procédure permet de demander la valeur actuel du champs de nouveau de nom de variable dans l'objet.
    /// </summary>
    /// <param name="newVariableName">Ce paramètre permet d'indiquer la nouvelle valeur que doit obtenir le nouveau nom de variable</param>
    public void setNewVariableName(string newVariableName)
    {
        NewVariableName = newVariableName;
    }

    /// <summary>
    /// Cette fonction permet de demander la valeur actuel du champs de nom de variable dans l'objet.
    /// </summary>
    /// <returns>Cette fonction retourner une chaine de caractères contenant le nom de variable</returns>
    public string getVariableName()
    {
        return VariableName;
    }

    /// <summary>
    /// Cette procédure permet de demander la valeur actuel du champs de nom de variable dans l'objet.
    /// </summary>
    /// <param name="variableName">Ce paramètre permet d'indiquer la nouvelle valeur que doit obtenir le nom de variable</param>
    public void setVariableName(string variableName)
    {
        VariableName = variableName;
    }

    /// <summary>
    /// Cette fonction permet de demander la valeur actuel du champs de valeur de variable dans l'objet.
    /// </summary>
    /// <returns>Cette fonction retourner une chaine de caractères contenant la valeur de variable</returns>
    public string getVariableValue()
    {
        return VariableValue;
    }

    /// <summary>
    /// Cette procédure permet de demander la valeur actuel du champs de valeur de variable dans l'objet.
    /// </summary>
    /// <param name="variableValue">Ce paramètre permet d'indiquer la nouvelle valeur que doit obtenir valeur de variable</param>
    public void setVariableValue(string variableValue)
    {
        VariableValue = variableValue;
    }

    /// <summary>
    /// Cette fonction permet de charger la liste des variables de configuration.
    /// </summary>
    /// <param name="field">Ce paramètre permet d'indiquer la liste des champs à retourner</param>
    /// <param name="sort">Ce paramètre permet d'indiquer l'ordre de tri</param>
    /// <returns>Cette fonction retourne objet «DataView» contenant la liste des variables de configurations.</returns>
    public DataView LoadList(string[] field, string sort)
    {
        bosomaindata myConn = new bosomaindata();
        SqlConnection Conn = myConn.getDBConnection();
        DataSet ds = new DataSet();
        SqlDataAdapter Cmd = new SqlDataAdapter("select " + String.Join(",", field) + " from bosoconfig order by " + sort, Conn);
        Cmd.Fill(ds, "mylist");
        return ds.Tables["mylist"].DefaultView;
    }
}
