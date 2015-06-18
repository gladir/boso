// Nom du fichier:   bosomessage.cs
// Nom du projet:    BOSO (Back-Office Service Object)
// Société:          Gladir.com
// Développeur:      Sylvain Maltais
// Date de création: 2008/03/03
// Version:          0.1
// Outils:           Visual Web Developer 2005 Express et Visual C# Express

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
/// La classe «bosomessage» permet de demander les messages textes appropriés
/// et de tous les retrouvés dans une même classe centralisé.
/// </summary>
public class bosomessage : System.Web.UI.Page
{
    public string LOGIN_USERID_REQUIRED = "Veuillez entrez un identificateur utilisateur pour effectuer une connexion";
    public string LOGIN_PASSWORD_REQUIRED = "Veuillez entrez un mot de passe pour effectuer une connexion";
    public string LOGIN_INVALID = "L'utilisateur ou le mot de passe est invalide !";
    public string PASSWORD_REQUIRED = "Veuillez entrez un mot de passe !";
    public string PASSWORD_CONFIRMATION_REQUIRED = "Veuillez entrez votre mot de passe de confirmation !";
    public string PASSWORD_MISMATCH = "Votre mot de passe ne correspond pas !";
    public string PASSWORD_EXPECTED = "Mot de passe attendu !";
    public string PASSWORD_TOOSHORT = "Mot de passe trop court ! Minimum 6 caractères.";
    public string PASSWORD_TOOLONG = "Mot de passe trop long ! Maximum 30 caractères.";
    public string WELCOME_SYSADMIN = "Bienvenue administrateur système!";
    public string DATA_NOTFOUND = "Les données sont introuvables !";
    public string CIN_ID_EXPECTED = "Le code d'identificateur (CIN) est attendu !";
    public string CIN_NAME_EXPECTED = "Un nom pour le CIN est attendu !";
    public string CIN_ID_ALREADYEXIST = "L'identificateur de CIN existe déjà dans la base de données";
    public string CIN_DELETE = "Un CIN a été supprimé.";
    public string CIN_CANT_DELETE = "Impossible de supprimé le CIN.";
    public string VALUE_EXPECTED = "La valeur est attendu !";
    public string NAME_EXPECTED = "Un nom est attendu !";
    public string NAME_EN_EXPECTED = "Un nom anglais du langage est attendu !";
    public string NAME_FR_EXPECTED = "Le nom français du langage est attendu !";
    public string VARIABLENAME_ALREADYEXIST = "Le nom de la variable existe déjà dans la base de données";
    public string CONFIGVAR_DELETE = "Une variable de configuration a été supprimé.";
    public string CONFIGVAR_CANT_DELETE = "Impossible de supprimé la variable de configuration.";
    public string MID_NOTFOUND = "Le MID est introuvable !";
    public string MID_EXPECTED = "Un modèle d'identificateur est attendu !";
    public string MID_ALREADYEXIST = "Le modèle d''identificateur existe déjà dans la base de données";
    public string CONTENTNAME_EXPECTED = "Un nom de contenu est attendu !";
    public string CONTENT_EXPECTED = "Le contenu est attendu !";
    public string CONTENT_DELETE = "Un contenu a été supprimé.";
    public string CONTENT_CANT_DELETE = "Impossible de supprimé le contenu.";
    public string READ_ERROR_TEMPLATE = "Erreur de lecture de la template!";
    public string FILE_TEMPLATE_NOTFOUND = "Le fichier original de la template est introuvable!";
    public string FIRSTNAME_EXPECTED = "Un prénom est attendu !";
    public string FIRSTNAME_TOOLONG = "Le prénom trop long. Maximum 75 caractères.";
    public string LASTNAME_EXPECTED = "Un nom de famille est attendu !";
    public string LASTNAME_TOOLONG = "Nom trop long. Maximum 75 caractères.";
    public string EMAIL_EXPECTED = "Un courriel est attendu !";
    public string EMAIL_INVALID = "Le courriel n'est pas valide !";
    public string EMAIL_ALREADYEEXIST = "Le courriel existe déjà dans la base de données !";
    public string EMAIL_SEND_ERROR = "Il y a eu un problème dans l'envoi du courrier électronique !";
    public string EMAIL_TOOSHORT = "La longueur de l'adresse de courriel est trop courte. Minimum 5 caractères.";
    public string EMAIL_TOOLONG = "La longueur de l'adresse de courriel est trop longue. Maximum 75 caractères.";
    public string USERMID_ALREADYEEXIST = "L'identificateur utilisateur existe déjà dans la base de données";
    public string USER_NOTINSCRIPTION = "L'utilisateur n'est plus inscrit dans la base de données !";
    public string USER_DELETE = "Un utilisateur a été supprimé.";
    public string USER_CANT_DELETE = "Impossible de supprimé l'utilisateur.";
    public string LANGUAGE_DELETE = "Un langage a été supprimé.";
    public string LANGUAGE_CANT_DELETE = "Impossible de supprimé le langage.";
    public string ROLE_DELETE = "Un rôle a été supprimé.";
    public string ROLE_CANT_DELETE = "Impossible de supprimé le rôle.";
    public string TYPEACTION_DELETE = "Un type d'action a été supprimé.";
    public string TYPEACTION_CANT_DELETE = "Impossible de supprimé le type d'action.";
    public string PHONE_EXPECTED = "Le numéro de téléphone est attendu !";
    public string PHONE_TOOSHORT = "Le numéro de téléphone est trop court ! Minimum 5 caractères.";
    public string PHONE_TOOLONG = "Le numéro de téléphone est trop long ! Maximum 75 caractères.";
    public string USERMID_EXPECTED = "Identificateur d'utilisateur attendu !";
    public string USERMID_TOOSHORT = "Identificateur d'utilisateur trop court ! Minimum 3 caractères.";
    public string USERMID_TOOLONG = "Identificateur d'utilisateur trop long ! Maximum 30 caractères.";

    public bosomessage()
	{
        try
        {
            if ("en" == (string)Context.Session["LanguageMID"])
            {
                LOGIN_USERID_REQUIRED = "Login User Id required!";
                LOGIN_PASSWORD_REQUIRED = "Login password required!";
                LOGIN_INVALID = "User Id or password is invalid!";
                PASSWORD_REQUIRED = "Please enter password!";
                PASSWORD_CONFIRMATION_REQUIRED = "Please enter confirmation password!";
                PASSWORD_MISMATCH = "A password mismatch has been detected!";
                PASSWORD_EXPECTED = "Password expected!";
                PASSWORD_TOOSHORT = "Password too short! Minimum 6 characters.";
                PASSWORD_TOOLONG = "Password too long! Maximum 30 characters.";
                WELCOME_SYSADMIN = "Welcome SysAdmin!";
                DATA_NOTFOUND = "Data not found!";
                CIN_ID_EXPECTED = "Id for CIN not found!";
                CIN_NAME_EXPECTED = "CIN name expected!";
                CIN_ID_ALREADYEXIST = "CIN Id already exist!";
                CIN_DELETE = "CIN deleted.";
                CIN_CANT_DELETE = "Can't delete CIN.";
                VALUE_EXPECTED = "Value expected!";
                NAME_EXPECTED = "Name expected!";
                NAME_EN_EXPECTED = "Name english expected!";
                NAME_FR_EXPECTED = "Name french expected!";
                VARIABLENAME_ALREADYEXIST = "Name variable already exist!";
                CONFIGVAR_DELETE = "Config variable deleted.";
                CONFIGVAR_CANT_DELETE = "Can't delete config variable.";
                MID_NOTFOUND = "MID not found!";
                MID_EXPECTED = "MID expected!";
                MID_ALREADYEXIST = "MID already exist!";
                CONTENTNAME_EXPECTED = "Content name expected!";
                CONTENT_EXPECTED = "Content expected!";
                CONTENT_DELETE = "Content deleted.";
                CONTENT_CANT_DELETE = "Can't delete content.";
                READ_ERROR_TEMPLATE = "Read error template!";
                FILE_TEMPLATE_NOTFOUND = "File template not found!";
                FIRSTNAME_EXPECTED = "First name expected!";
                FIRSTNAME_TOOLONG = "First Name too long. Maximum 75 characters.";
                LASTNAME_EXPECTED = "Last name expected!";
                LASTNAME_TOOLONG = "Last Name too long. Maximum 75 characters.";
                EMAIL_EXPECTED = "Email expected!";
                EMAIL_INVALID = "Email is invalid!";
                EMAIL_ALREADYEEXIST = "Email already exist!";
                EMAIL_SEND_ERROR = "Email send error!";
                EMAIL_TOOSHORT = "Email length too short. Minimum 5 characters.";
                EMAIL_TOOLONG = "Email length too long. Maximum 75 characters.";
                USERMID_ALREADYEEXIST = "User MID already exist!";
                USER_NOTINSCRIPTION = "User not inscription in database!";
                USER_DELETE = "User deleted.";
                USER_CANT_DELETE = "Can't delete user.";
                LANGUAGE_DELETE = "Language deleted.";
                LANGUAGE_CANT_DELETE = "Can't delete language.";
                ROLE_DELETE = "Role deleted.";
                ROLE_CANT_DELETE = "Can't delete role.";
                TYPEACTION_DELETE = "Action type deleted.";
                TYPEACTION_CANT_DELETE = "Can't delete action type.";
                PHONE_EXPECTED = "Phone number expected!";
                PHONE_TOOSHORT = "Phone number too short! Minimum 5 characters.";
                PHONE_TOOLONG = "Phone number too long! Maximum 75 characters.";
                USERMID_EXPECTED = "User Id expected!";
                USERMID_TOOSHORT = "User Id too short! Minimum 3 characters.";
                USERMID_TOOLONG = "User Id too long! Maximum 30 characters.";
            }
        }
        catch
        {
        }
	}
}
