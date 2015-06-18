// Nom du fichier:   bosoauto.cs
// Nom du projet:    BOSO (Back-Office Service Object)
// Société:          Gladir.com
// Développeur:      Sylvain Maltais
// Date de création: 2008/02/21
// Version:          0.1
// Outils:           Visual Web Developer 2005 Express et Visual C# Express
//
/// <summary>
/// Ce module est destiné est affectué l'authentification des utilisateurs d'un site Web.
/// </summary>

using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

/// <summary>
/// La classe «bosoauto» permet d'effectuer l'authentification d'un 
/// utilisateur sur le site Internet
/// </summary>
public class bosoauto : System.Web.UI.Page
{
	// Cette variable permet d'indiquer la page de redirection pour reconnexion
    string DefaultLoginPage = System.Configuration.ConfigurationManager.AppSettings["DefaultLoginPage"];
    string DefaultErrorPage = System.Configuration.ConfigurationManager.AppSettings["DefaultErrorPage"];

    /// <summary>
    /// Ce constructeur permet d'initialiser la classe d'authentification
    /// </summary>
    public bosoauto()
	{
	}

    /// <summary>
    /// Cette procédure permet d'effectuer la vérification qu'un
    /// utilisateutr de rôle administrateur est connecté et 
    /// d'effectuer une redirection à une connexion s'il n'est pas
    /// autorisé.
    /// </summary>
    public void CheckAutorizationAdministrator()
    {
        try
        {
            if ("ADMIN" != (string)Session["Role"])
            {
                Server.Transfer(DefaultLoginPage);
            }
        } catch {
            Server.Transfer(DefaultErrorPage);
        }
    }

    /// <summary>
    /// Cette procédure permet d'effectuer la vérification qu'un 
    /// utilisateur de rôle administrateur système est connecté et 
    /// d'effectuer une redirection à une connexion s'il n'est pas
    /// autorisé.
    /// </summary>
    public void CheckAutorizationSystemAdministrator()
    {
        if ("SYSADMIN" != (string)Session["Role"])
        {
            Server.Transfer(DefaultLoginPage);
        }
    }

    /// <summary>
    /// Cette procédure permet d'effectuer la vérification qu'un 
    /// utilisateur de rôle administrateur ou administrateur système est connecté et 
    /// d'effectuer une redirection à une connexion s'il n'est pas
    /// autorisé.
    /// </summary>
    public void CheckAutorizationAdministratorOrSystemAdministrator()
    {
        if (("SYSADMIN" != (string)Session["Role"]) && ("ADMIN" != (string)Session["Role"]))
        {
            Server.Transfer(DefaultLoginPage);
        }
    }

    /// <summary>
    /// Cette procédure permet d'effectuer la vérification qu'un 
    /// utilisateur de rôle administrateur système est connecté et 
    /// d'effectuer une redirection à une connexion s'il n'est pas
    /// autorisé.
    /// </summary>
    public void CheckAutorizationAnyUser()
    {
        if (null == Session["UserMid"]) Server.Transfer(DefaultLoginPage);
        if ("" == (string)Session["UserMid"]) Server.Transfer(DefaultLoginPage);
    }
}
