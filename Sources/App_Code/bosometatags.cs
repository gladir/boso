// Nom du fichier:   bosometatags.cs
// Nom du projet:    BOSO (Back-Office Service Object)
// Société:          Gladir.com
// Développeur:      Sylvain Maltais
// Date de création: 2008/02/29
// Version:          0.1
// Outils:           Visual Web Developer 2005 Express et Visual C# Express

using System;
using System.Data;
using System.Data.SqlClient;
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
/// La classe «bosometatags» permet d'effectuer la gestion d'un fichier 
/// de «metatags».
/// </summary>
public class bosometatags : System.Web.UI.Page
{
    string fileMetaTags = "templates\\metatags.xml"; // Fichier de meta tags

    /// <summary>
    /// Ce constructeur permet d'initialiser l'objet.
    /// </summary>
    public bosometatags()
	{
	}

    /// <summary>
    /// Ce constructeur permet de charger immédiatement la classe avec un
    /// le contenu d'un fichier de «meta tags».
    /// </summary>
    /// <param name="newTagsFile">Ce paramètre permet d'indiquer le nom du fichier contenant les métas tags.</param>
    public bosometatags(string newTagsFile)
    {
        fileMetaTags = newTagsFile;
    }

    /// <summary>
    /// Cette fonction permet de retourner la liste des langues de «metas tags».
    /// </summary>
    /// <returns>Cette fonction retourne une liste de chaine de caractères contenant les noms de langues de «meta tags».</returns>
    public List<string> LoadListMetaTags()
    {
        List<string> list = new List<string>();

        XmlDocument xmlDoc = new XmlDocument();
        if (File.Exists(Server.MapPath(fileMetaTags)))
        {
            xmlDoc.Load(Server.MapPath(fileMetaTags));
            XmlElement elm = xmlDoc.DocumentElement;
            XmlNodeList lstHeaders = elm.ChildNodes;
            for (int i = 0; i < lstHeaders.Count; i++)
            {
                list.Add(lstHeaders[i].Attributes["lang"].Value);
            }
        }
        return list;
    }

    /// <summary>
    /// Cette fonction permet de retourner la liste des «metas tags» que contient une langue.
    /// </summary>
    /// <param name="name">Ce paramètre permet d'indiquer le modèle d'identificateur de langue</param>
    /// <returns>Cette fonction retourne un «DataView» préparer en deux colonnes: «Name» et «Value».</returns>
    public DataView LoadMetaTags(string name)
    {
        DataTable dt = new DataTable();
        DataRow dr;
        dt.Columns.Add(new DataColumn("Name", typeof(string)));
        dt.Columns.Add(new DataColumn("Value", typeof(string)));
        
        XmlDocument xmlDoc = new XmlDocument();
        if (File.Exists(Server.MapPath(fileMetaTags)))
        {
            xmlDoc.Load(Server.MapPath(fileMetaTags));
            XmlElement elm = xmlDoc.DocumentElement;
            XmlNodeList lstHeaders = elm.ChildNodes;
            for (int i = 0; i < lstHeaders.Count; i++)
            {
                if (lstHeaders[i].Attributes["lang"].Value == name)
                {
                    XmlNodeList lstCurrent = lstHeaders[i].ChildNodes;
                    for (int j = 0; j < lstCurrent.Count; j++)
                    {
                        dr = dt.NewRow();
                        dr[0] = lstCurrent[j].Name;
                        dr[1] = lstCurrent[j].InnerText;
                        dt.Rows.Add(dr);

                    }
                }
            }
        }

        DataView dv = new DataView(dt);
        return dv;
    }
}
