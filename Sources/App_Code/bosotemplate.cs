// Nom du fichier:   bosotemplate.cs
// Nom du projet:    BOSO (Back-Office Service Object)
// Société:          Gladir.com
// Développeur:      Sylvain Maltais
// Date de création: 2008/02/21
// Version:          0.1
// Outils:           Visual Web Developer 2005 Express et Visual C# Express

using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml;

/// <summary>
/// La classe «bosotemplate» offre tous les services de traitement de base
/// pour une template de la manière de «Smarty» comme PHP. Il offre également
/// des services complémentaires comme les «meta tags» d'entête généraliste
/// dans une site.
/// </summary>
public class bosotemplate : System.Web.UI.Page
{
    Hashtable variableValue = new Hashtable(); // Contient la valeur de chacun des variables
    string header = "";                        // La chaine contenant l'entête HTML
    string template = "";                      // Le template est vide au départ
    string errorMsg = "";                      // Chaine de caractères contenant l'erreur
    string languageMID = "fr";                 // MID du langage par défaut
    // Definition des étiquettes pour les langages
    string fileLanguageName = "templates\\language.xml";

    /// <summary>
    /// Ce constructeur permet d'initialiser l'objet.
    /// </summary>
    public bosotemplate()
	{
	}

    /// <summary>
    /// Ce constructeur permet d'initialiser le contenu de la «template» 
    /// à partir d'un fichier.
    /// </summary>
    /// <param name="fileTemplateName">Ce paramètre permet d'indiquer le nom du fichier de la «template».</param>
    public bosotemplate(string fileTemplateName)
    {
        if (File.Exists(Server.MapPath(fileTemplateName)))
        {
            try
            {
                template = File.ReadAllText(Server.MapPath(fileTemplateName));
            }
            catch
            {
                errorMsg = new bosomessage().READ_ERROR_TEMPLATE;
            }
        }
        else
        {
            errorMsg = new bosomessage().FILE_TEMPLATE_NOTFOUND;
        }
    }

    /// <summary>
    /// Cette procédure permet de charger le fichier XML contenant l'entête HTML.
    /// </summary>
    /// <param name="fileXmlName">Ce paramètre permet d'indiquer le nom du ficher XML contenant les balises de «meta tags».</param>
    public void loadHeader(string fileXmlName)
    {
        XmlDocument xmlDoc = new XmlDocument();
        if (File.Exists(Server.MapPath(fileXmlName)))
        {
            xmlDoc.Load(Server.MapPath(fileXmlName));
            XmlElement elm = xmlDoc.DocumentElement;
            XmlNodeList lstHeaders = elm.ChildNodes;
            for(int i = 0; i < lstHeaders.Count; i++)
            {
                if (lstHeaders[i].Attributes["lang"].Value == languageMID)
                {
                    XmlNodeList lstCurrent = lstHeaders[i].ChildNodes;
                    for (int j = 0; j < lstCurrent.Count; j++)
                    {
                        string CurrName = lstCurrent[j].Name;
                        string CurrValue = lstCurrent[j].InnerText;
                        switch(CurrName) {
                            case "title":
                                header += "<title>" + CurrValue + "</title>\r\n";
                                break;
                            default:
                                header += "<meta name=\"" + CurrName + "\" content=\"" + CurrValue + "\" />\r\n";
                                break;
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// Cette procédure permet de fixer le titre de la page HTML.
    /// </summary>
    /// <param name="Title">Ce paramètre permet d'ajouter à l'entête de la page HTML un titre.</param>
    public void setTitle(string Title)
    {
        header += "<title>" + Title + "</title>";
    }

    /// <summary>
    /// Cette procédure permet de fixer l'identificateur MID du langage courant.
    /// </summary>
    /// <param name="MID">Cette fonction permet d'indiquer le modèle d'identificateur du langage.</param>
    public void setCurrentLanguageMID(string MID)
    {
        languageMID = MID;
    }

    /// <summary>
    /// Cette fonction permet de demander l'identificateur MID du langage courant.
    /// </summary>
    /// <returns>Cette fonction retourne la valeur du modèle d'identificateur de langage.</returns>
    public string getCurrentLanguageMID()
    {
        return languageMID;
    }

    /// <summary>
    /// Cette procédure permet d'initialiser le contenu du clef avec la
    /// valeur spécifié.
    /// </summary>
    /// <param name="key">Ce paramètre permet d'indiquer le nom d'une clef devant être remplacer dans la «template»</param>
    /// <param name="value">Ce paramètre permet d'indiquer la valeur que devra avoir une clef lorsqu'elle sera rencontré dans une «template».</param>
    public void assign(string key,string value) {
        variableValue.Add(key, value);
    }

    /// <summary>
    /// Cette procédure permet d'ajouter une template à la template déjà existante.
    /// </summary>
    /// <param name="newtemplate">Ce paramètre permet d'ajouter des informations à la «template».</param>
    public void addTemplate(string newtemplate)
    {
        template += newtemplate;
    }

    /// <summary>
    /// Cette procédure permet d'échanger seulement les variables de syntaxe «Smarty» 
    /// pour le contenu dans la «template».
    /// </summary>
    public void switchTagsSmarty()
    {
        foreach (DictionaryEntry currTags in variableValue)
        {
            template = template.Replace("{$" + currTags.Key + "}", (string)currTags.Value);
        }
    }

    /// <summary>
    /// Cette procédure permet d'échanger seulement les variables de syntaxe «ASP» 
    /// pour le contenu dans la «template».
    /// </summary>
    public void switchTagsASP()
    {
        foreach (DictionaryEntry currTags in variableValue)
        {
            template = template.Replace("<%=" + currTags.Key + "%>", (string)currTags.Value);
        }
    }

    /// <summary>
    /// Cette procédure permet d'échanger seulement les variables de syntaxe «PHP» 
    /// pour le contenu dans la «template».
    /// </summary>
    public void switchTagsPHP()
    {
        foreach (DictionaryEntry currTags in variableValue)
        {
            template = template.Replace("<?=" + currTags.Key + "?>", (string)currTags.Value);
        }
    }

    /// <summary>
    /// Cette procédure permet d'échanger seulement les variables de syntaxe «Gladir.com» 
    /// pour le contenu dans la «template».
    /// </summary>
    public void switchTagsGladirCom()
    {
        foreach (DictionaryEntry currTags in variableValue)
        {
            template = template.Replace("<var:" + currTags.Key + " />", (string)currTags.Value);
        }
    }

    /// <summary>
    /// Cette procédure permet d'échanger seulement les variables d'étiquettes de langage 
    /// dans le contenu dans la «template».
    /// </summary>
    /// <param name="LanguageMID">Ce paramètre permet d'indiquer le modèle d'identificateur du langage.</param>
    public void switchTagsLanguage(string LanguageMID)
    {
        if (null == LanguageMID || "" == LanguageMID) LanguageMID = "fr";
        XmlDocument xmlDoc = new XmlDocument();
        if (File.Exists(Server.MapPath(fileLanguageName)))
        {
            xmlDoc.Load(Server.MapPath(fileLanguageName));
            XmlElement elm = xmlDoc.DocumentElement;
            XmlNodeList lstHeaders = elm.ChildNodes;
            for (int i = 0; i < lstHeaders.Count; i++)
            {
                if (lstHeaders[i].Attributes["lang"].Value == LanguageMID)
                {
                    XmlNodeList lstCurrent = lstHeaders[i].ChildNodes;
                    for (int j = 0; j < lstCurrent.Count; j++)
                    {
                        string CurrName = lstCurrent[j].Name;
                        string CurrValue = lstCurrent[j].InnerText;
                        template = template.Replace("{$" + CurrName + "}", CurrValue);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Cette fonction permet d'échanger seulement les variables d'étiquettes de langage 
    /// dans le contenu dans la «template».
    /// </summary>
    /// <param name="LanguageMID">Ce paramètre permet d'indiquer le modèle d'identificateur du langage.</param>
    /// <param name="label">Ce paramètre permet d'indiquer la liste des étiquettes à retourner.</param>
    /// <returns>Cette fonction retourne une liste de chaine de caractères avec les messages correspondant respectivement aux étiquettes.</returns>
    public List<string> switchTagsLanguage(string LanguageMID, string[] label)
    {
        if (null == LanguageMID || "" == LanguageMID) LanguageMID = "fr";
        List<string> list = new List<string>();
        XmlDocument xmlDoc = new XmlDocument();
        if (File.Exists(Server.MapPath(fileLanguageName)))
        {
            xmlDoc.Load(Server.MapPath(fileLanguageName));
            XmlElement elm = xmlDoc.DocumentElement;
            XmlNodeList lstHeaders = elm.ChildNodes;
            for (int i = 0; i < lstHeaders.Count; i++)
            {
                if (lstHeaders[i].Attributes["lang"].Value == LanguageMID)
                {
                    for (int k = 0; k < label.Length; k++)
                    {
                        XmlNodeList lstCurrent = lstHeaders[i].ChildNodes;
                        for(int j = 0; j < lstCurrent.Count; j++)
                        {
                            string CurrName = lstCurrent[j].Name;
                            string CurrValue = lstCurrent[j].InnerText;
                            if (CurrName == label[k]) list.Add(CurrValue);
                        }
                    }
                }
            }
        }
        return list;
    }

    /// <summary>
    /// Cette fonction permet de lire le contenu de la «meta tags».
    /// </summary>
    /// <returns>Cette fonction retourne une chaine de caractères de l'entête de «meta tags»</returns>
    public string getMetaTags()
    {
        return header;
    }

    /// <summary>
    /// Cette fonction permet de lire le contenu de la «meta tags» avec la
    /// balise «head» si désiré.
    /// </summary>
    /// <param name="head">Ce paramètre s'il est à «true» retourne les «metas tags» dans la balise d'entête d'une page Web, sinon, seulement la liste des balises de «meta tags».</param>
    /// <returns>Cette fonction retourne une chaine de caractères des «metas tags» selon le traitement spécifié.</returns>
    public string getMetaTags(bool head)
    {
        if (head) return "<head>" + header + "</head>";
            else return header;
    }

    /// <summary>
    /// Cette fonction permet de lire le contenu de la «template».
    /// </summary>
    /// <returns>Cette fonction retorune une chaine de caractères du contenu de la «template».</returns>
    public string getTemplate()
    {
        return template;
    }

    /// <summary>
    /// Cette fonction permet de retourner une balise d'ouverture de code HTML.
    /// </summary>
    /// <returns>Cette fonction retourne une chaine de caractères de la balise d'ouverture d'un code HTML.</returns>
    public string getHeaderHTML() {
        return "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.1//EN\" \"http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd\">\r\n" +
               "<html xmlns=\"http://www.w3.org/1999/xhtml\">";
    }

    /// <summary>
    /// Cette fonction permet de retourner une balise de corps d'ouverture de la page HTML.
    /// </summary>
    /// <returns>Cette fonction retourne une chaine de caractères de la balise de corps d'ouverture de la page HTML.</returns>
    public string getHeaderBodyHTML()
    {
        return "<body>";
    }

    /// <summary>
    /// Cette fonction permet de retourner une balise de corps de fermeture de la page HTML.
    /// </summary>
    /// <returns>Cette fonction retourne une chaine de caractères de la balise de corps de fermeture de la page HTML.</returns>
    public string getBottomBodyHTML()
    {
        return "</body>";
    }

    /// <summary>
    /// Cette fonction permet de retourner une balise de fermeture de code HTML.
    /// </summary>
    /// <returns>Cette fonction retourne une chaine de caractères de la balise de fermeture de code HTML.</returns>
    public string getBottomHTML()
    {
        return "</html>";
    }

    /// <summary>
    /// Cette fonction permet de connaitre le message correspondant à la dernière
    /// erreur survenu dans la classe de «template».
    /// </summary>
    /// <returns>Cette fonction retourne un message d'erreur s'il y a lieu.</returns>
    public string getErrorMsg()
    {
        return errorMsg;
    }
}
