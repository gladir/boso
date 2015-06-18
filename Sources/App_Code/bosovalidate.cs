// Nom du fichier:   bosovalidate.cs
// Nom du projet:    BOSO (Back-Office Service Object)
// Société:          Gladir.com
// Développeur:      Sylvain Maltais
// Date de création: 2008/02/27
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
using System.Text.RegularExpressions;

/// <summary>
/// La classe «bosovalidate» permet d'effectuer des validations de toutes sortes.
/// </summary>
public class bosovalidate
{
    string errorMsg = "";        // Message d'erreur
    
    /// <summary>
    /// Ce constructeur permet d'initialiser l'objet.
    /// </summary>
    public bosovalidate()
	{
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
    /// Cette fonction permet de tester la validité d'un courriel.
    /// </summary>
    /// <param name="email">Ce paramètre permet d'indiquer l'adresse de courriel.</param>
    /// <returns>Cette fonction retourne «true» si le courriel est valide sinon «false» s'il n'est pas valide.</returns>
    public bool IsEmail(string email)
    {
        Regex validation = new Regex(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*",RegexOptions.IgnoreCase);
        return validation.IsMatch(email);
    }

    /// <summary>
    /// Cette fonction permet de tester la validité d'un identificateur d'utilisateur.
    /// </summary>
    /// <param name="UserMID">Ce paramètre permet d'indiquer l'identificateur d'utilisateur.</param>
    /// <returns>Cette fonction retourne «true» si l'utilisateur est valide sinon «false» s'il n'est pas valide.</returns>
    public bool UserMIDValid(string UserMID)
    {
        if ("" == UserMID)
        {
            errorMsg = new bosomessage().USERMID_EXPECTED;
            return false;
        }

        if (UserMID.Length < 3)
        {
            errorMsg = new bosomessage().USERMID_TOOSHORT;
            return false;
        }

        if (UserMID.Length > 30)
        {
            errorMsg = new bosomessage().USERMID_TOOLONG;
            return false;
        }
        return true;
    }

    /// <summary>
    /// Cette fonction permet de tester la validité d'un prénom.
    /// </summary>
    /// <param name="FirstName">Ce paramètre permet d'indiquer le prénom.</param>
    /// <returns>Cette fonction retourne «true» si le prénom est valide sinon «false» s'il n'est pas valide.</returns>
    public bool FirstNameValid(string FirstName)
    {
        if ("" == FirstName)
        {
            errorMsg = new bosomessage().FIRSTNAME_EXPECTED;
            return false;
        }

        if (FirstName.Length > 75)
        {
            errorMsg = new bosomessage().FIRSTNAME_TOOLONG;
            return false;
        }
        return true;
    }

    /// <summary>
    /// Cette fonction permet de tester la validité d'un nom de famille.
    /// </summary>
    /// <param name="LastName">Ce paramètre permet d'indiquer le nom de famille.</param>
    /// <returns>Cette fonction retourne «true» si le nom est valide sinon «false» s'il n'est pas valide.</returns>
    public bool LastNameValid(string LastName)
    {
        if ("" == LastName)
        {
            errorMsg = new bosomessage().LASTNAME_EXPECTED;
            return false;
        }

        if (LastName.Length > 75)
        {
            errorMsg = new bosomessage().LASTNAME_TOOLONG;
            return false;
        }
        return true;
    }

    /// <summary>
    /// Cette fonction permet de tester la validité d'un mot de passe.
    /// </summary>
    /// <param name="Password">Ce paramètre permet d'indiquer le mot de passe.</param>
    /// <returns>Cette fonction retourne «true» si le mot de passe est valide sinon «false» s'il n'est pas valide.</returns>
    public bool PasswordValid(string Password)
    {
        if ("" == Password)
        {
            errorMsg = new bosomessage().PASSWORD_EXPECTED;
            return false;
        }

        if (Password.Length < 6)
        {
            errorMsg = new bosomessage().PASSWORD_TOOSHORT;
            return false;
        }

        if (Password.Length > 30)
        {
            errorMsg = new bosomessage().PASSWORD_TOOLONG;
            return false;
        }
        return true;
    }

    /// <summary>
    /// Cette fonction permet de tester la validité d'un numéro de téléphone.
    /// </summary>
    /// <param name="phone">Ce paramètre permet d'indiquer le numéro de téléphone.</param>
    /// <returns>Cette fonction retourne «true» si le numéro de téléphone est valide sinon «false» s'il n'est pas valide.</returns>
    public bool PhoneValid(string phone)
    {
        if ("" == phone)
        {
            errorMsg = new bosomessage().PHONE_EXPECTED;
            return false;
        }

        if (phone.Length < 5)
        {
            errorMsg = new bosomessage().PHONE_TOOSHORT;
            return false;
        }

        if (phone.Length > 75)
        {
            errorMsg = new bosomessage().PHONE_TOOLONG;
            return false;
        }
        return true;
    }

    /// <summary>
    /// Cette fonction permet de tester la validité d'un courriel.
    /// </summary>
    /// <param name="phone">Ce paramètre permet d'indiquer le courriel.</param>
    /// <returns>Cette fonction retourne «true» si le courriel est valide sinon «false» s'il n'est pas valide.</returns>
    public bool EmailValid(string Email)
    {
        if ("" == Email)
        {
            errorMsg = new bosomessage().EMAIL_EXPECTED;
            return false;
        }

        if (Email.Length < 5)
        {
            errorMsg = new bosomessage().EMAIL_TOOSHORT;
            return false;
        }

        if (Email.Length > 75)
        {
            errorMsg = new bosomessage().EMAIL_TOOLONG;
            return false;
        }

        if (!IsEmail(Email))
        {
            errorMsg = new bosomessage().EMAIL_INVALID;
            return false;
        }

        return true;
    }
}
