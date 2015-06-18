// Nom du fichier:   bosouser.cs
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
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Security.Cryptography;

/// <summary>
/// La classe «bosouser» permet d'effectuer la gestion des utilisateurs de 
/// façon centralisé.
/// </summary>
public class bosouser : System.Web.UI.Page
{
    int?   CurrId = null;   // Identificateur de base de données utilisateur
    int    RoleId = 0;      // Identificateur de rôle
    string CIN = "";        // Identificateur client
    string MID = "";        // Identificateur utilisateur
    string FirstName = "";  // Prénom
    string LastName = "";   // Nom de famille
    string Email = "";      // Courriel
    string Password = "";   // Mot de passe
    string Status = "A";    // État

    string RoleMID = "";    // MID du rôle

    string DefaultLostPasswordAdmin = "BOSO <info@gladir.com>";
    string DefaultLostPasswordSubject = "BOSO: Mot de passe oublié !";
    string DefaultLostPasswordBodyBegin = "Tu étais membre du site Web de BOSO.<br><br>";
    string DefaultLostPasswordBodyEnd = "L'équipe BOSO ";

    string ConfirmSign = "";       // Signature de confirmation
    string errorMsg = "";          // Message d'erreur

    /// <summary>
    /// Ce constructeur permet d'initialiser l'objet.
    /// </summary>
    public bosouser()
	{
	}

    /// <summary>
    /// Ce constructeur permet de charger immédiatement les données d'un utilisateur
    /// en se basant sur son identificateur numérique.
    /// </summary>
    /// <param name="currId">Ce paramètre permet d'indiquer l'identificateur d'utilisateur.</param>
    public bosouser(int currId)
    {
        if (!LoadData(currId)) {
            errorMsg = new bosomessage().DATA_NOTFOUND;
        }
    }

    /// <summary>
    /// Ce constructeur permet de charger immédiatement les données d'un utilisateur
    /// en se basant sur son identificateur numérique.
    /// </summary>
    /// <param name="cin">Ce paramètre permet d'indiquer un identificateur client</param>
    /// <param name="currId">Ce paramètre permet d'indiquer l'identificateur d'utilisateur.</param>
    public bosouser(string cin, int currId)
    {
        if (!LoadData(cin,currId))
        {
            errorMsg = new bosomessage().DATA_NOTFOUND;
        }
    }

    /// <summary>
    /// Cette fonction permet de connaitre le message correspondant à la dernière
    /// erreur survenu dans la classe de «user».
    /// </summary>
    /// <returns>Cette fonction retourne un message d'erreur s'il y a lieu.</returns>
    public string getErrorMsg()
    {
        return errorMsg;
    }

    /// <summary>
    /// Cette fonction permet d'effectuer le chargement des données concernant 
    /// l'utilisateur spécifié.
    /// </summary>
    /// <param name="currId">Ce paramètre permet d'indiquer l'identificateur d'un utilisateur</param>
    /// <returns>Cette fonction retourne «true» si la lecture s'est correctement déroulé ou «false» si une erreur est survenu.</returns>
    public bool LoadData(int currId) {
        return LoadData(null, currId);
    }

    /// <summary>
    /// Cette fonction permet d'effectuer le chargement des données concernant 
    /// l'utilisateur en fonction d'un CIN spécifié.
    /// </summary>
    /// <param name="cin">Ce paramètre permet d'indiquer un identificateur client</param>
    /// <param name="currId">Ce paramètre permet d'indiquer l'identificateur d'un utilisateur</param>
    /// <returns>Cette fonction retourne «true» si la lecture s'est correctement déroulé ou «false» si une erreur est survenu.</returns>
    public bool LoadData(string cin,int currId)
    {
        if (0 != currId)
        {
            if ("" == cin) cin = null;
            
            bosomaindata myConn = new bosomaindata();
            SqlConnection Conn = myConn.getDBConnection();
            SqlCommand CmdEdit = new SqlCommand("select UserId,MID,FirstName,LastName,Email,RoleId,CIN,Password,Status from bosouser where " + (null != cin ? "CIN=@cin and " : "") + "UserId=@userid", Conn);
            if (null != cin) CmdEdit.Parameters.Add(new SqlParameter("@cin", cin));
            CmdEdit.Parameters.Add(new SqlParameter("@userid", currId));
            Conn.Open();
            SqlDataReader drEdit = CmdEdit.ExecuteReader(CommandBehavior.CloseConnection);
            if (drEdit.Read())
            {
                CurrId = drEdit.GetInt32(0);
                MID = drEdit.GetString(1);
                FirstName = drEdit.GetString(2);
                LastName = drEdit.GetString(3);
                Email = drEdit.GetString(4);
                RoleId = drEdit.GetInt32(5);
                CIN = drEdit.GetString(6);
                Password = drEdit.GetString(7);
                Status = drEdit.GetString(8);
            }
            Conn.Close();
            return true;
        }
        return false;
    }

    /// <summary>
    /// Cette fonction permet d'effectuer la sauvegarde (ajout ou modification) 
    /// des données concernant l'utilisateur spécifié.
    /// </summary>
    /// <returns>Cette fonction retourne «true» si la sauvegarde s'est correctement déroulé ou «false» si une erreur est survenu.</returns>
    public bool SaveData()
    {
        bosovalidate valid = new bosovalidate();
        if ("" == MID)
        {
            errorMsg = new bosomessage().MID_EXPECTED;
            return false;
        }

        if (!valid.UserMIDValid(MID))
        {
            errorMsg = valid.getErrorMsg();
            return false;
        }
        
        if(!valid.LastNameValid(LastName)) {
            errorMsg = valid.getErrorMsg();
            return false;
        }

        if (!valid.EmailValid(Email))
        {
            errorMsg = valid.getErrorMsg();
            return false;
        }

        bosomaindata myConn = new bosomaindata();
        SqlConnection Conn = myConn.getDBConnection();
        SqlCommand Cmd;
        if ((!CurrId.HasValue) || (CurrId == 0))
        {
            if (UserMIDExist(MID))
            {
                errorMsg = new bosomessage().USERMID_ALREADYEEXIST;
                return false;
            }
            
            if (EmailExist(Email))
            {
                errorMsg = new bosomessage().EMAIL_ALREADYEEXIST;
                return false;
            }
            Cmd = new SqlCommand("INSERT INTO bosouser (" +
                                        "UserId, CIN, RoleId, MID, FirstName, LastName, Email," +
                                        "ConfirmSign, Status, CreateDate,CreateUserId, LastDate, LastUserId" +
                                  ") VALUES (" +
                                        "@userid,@cin,@roleid,@mid,@firstname,@lastname,@email," +
                                        "@confirmsign,@status,@createdate,@createuserid,@lastdate,@lastuserid" +
                                  ")", Conn);
            Cmd.Parameters.Add(new SqlParameter("@userid", CurrId = myConn.getCurrId()));
            Cmd.Parameters.Add(new SqlParameter("@confirmsign", ConfirmSign));
            myConn.WriteAction(bosomaindata.typeAction.AddData, "User");
        }
        else
        {
            Cmd = new SqlCommand("update bosouser set " +
                                    "MID = @mid, " +
                                    "CIN = @cin, " +
                                    "RoleId = @roleid, " +
                                    "FirstName = @firstname, " +
                                    "LastName = @lastname, " +
                                    "Email = @email, " +
                                    "Status = @status, " +
                                    "LastDate = @lastdate," +
                                    "LastUserId = @lastuserid " +
                               "where UserId = @userid", Conn);
            Cmd.Parameters.Add(new SqlParameter("@userid", CurrId));
            myConn.WriteAction(bosomaindata.typeAction.ModifiedData, "User");
        }
        Cmd.Parameters.Add(new SqlParameter("@mid", MID));
        Cmd.Parameters.Add(new SqlParameter("@cin", CIN));
        Cmd.Parameters.Add(new SqlParameter("@roleid", RoleId));
        Cmd.Parameters.Add(new SqlParameter("@firstname", FirstName));
        Cmd.Parameters.Add(new SqlParameter("@lastname", LastName));
        Cmd.Parameters.Add(new SqlParameter("@email", Email));
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
    /// Cette fonction permet d'effectuer la suppression de l'utilisateur spécifié.
    /// </summary>
    /// <param name="userId">Ce paramètre permet d'indiquer l'identificateur d'un utilisateur</param>
    /// <returns>Cette fonction retourne «true» si la suppression s'est correctement déroulé ou «false» si une erreur est survenu.</returns>
    public bool Delete(int userId) {
        bosomaindata myConn = new bosomaindata();
        SqlConnection Conn = myConn.getDBConnection();
        SqlCommand Cmd = new SqlCommand("delete from bosouser where UserId = @userid", Conn);
        Cmd.Parameters.Add(new SqlParameter("@userid", userId));
        Conn.Open();
        try
        {
            Cmd.ExecuteNonQuery();
            myConn.WriteAction(bosomaindata.typeAction.DeleteData, "User");
            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Cette fonction permet d'indiquer si le courriel spécifié existe dans la 
    /// base de données des utilisateurs.
    /// </summary>
    /// <param name="email">Ce paramètre permet d'indiquer l'adresse de courriel.</param>
    /// <returns>Cette fonction retourne «true» si l'adresse de courriel a été trouvé dans la base de données ou «false» s'il n'a pas été trouvé.</returns>
    public bool EmailExist(string email)
    {
        if ("" == Email) return false;
        bosomaindata myConn = new bosomaindata();
        SqlConnection Conn = myConn.getDBConnection();
        SqlCommand Cmd = new SqlCommand("select " +
                                            "bosouser.UserId " +
                                        "from bosouser " +
                                        "where Email=@courriel", Conn);
        Cmd.Parameters.Add(new SqlParameter("@courriel", email));
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
    /// Cette fonction permet d'indiquer si l'identificateur utilisateur spécifié existe 
    /// dans la base de données des utilisateurs.
    /// </summary>
    /// <param name="mid">Ce paramètre permet d'indiquer un modèle d'identificateur</param>
    /// <returns>Cette fonction retourne «true» si un utilisateur a été trouver ou «false» si aucun n'a été trouvé.</returns>
    public bool UserMIDExist(string mid)
    {
        if ("" == mid) return false;
        bosomaindata myConn = new bosomaindata();
        SqlConnection Conn = myConn.getDBConnection();
        SqlCommand Cmd = new SqlCommand("select " +
                                            "bosouser.UserId " +
                                        "from bosouser " +
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
    /// Cette fonction permet d'indiquer si la connexion d'un utilisateur et de son 
    /// mot de passe est valide.
    /// </summary>
    /// <param name="userId">Ce paramètre permet d'indiquer l'identificateur de l'utilisateur.</param>
    /// <param name="password">Ce paramètre permet d'indiquer le mot de passe de l'utilisateur.</param>
    /// <param name="CIN">Ce paramètre d'indiquer l'identificateur client s'il y a lieu.</param>
    public bool Login(string userId, string password, string CIN)
    {
        if ("" == userId)
        {
            errorMsg = new bosomessage().LOGIN_USERID_REQUIRED;
            return false;
        }

        if ("" == password)
        {
            errorMsg = new bosomessage().LOGIN_PASSWORD_REQUIRED;
            return false;
        }

        bosomaindata myConn = new bosomaindata();
        SqlConnection Conn = myConn.getDBConnection();
        SqlCommand Cmd = new SqlCommand("select " +
                                            "bosouser.UserId," +
                                            "bosouser.RoleId," +
                                            "bosorole.MID, " +
                                            "bosouser.MID, " +
                                            "bosouser.CIN " +
                                        "from bosouser " +
                                        "left join bosorole on bosorole.RoleId=bosouser.RoleId " +
                                        "where bosouser.CIN=@cin and bosouser.MID=@mid and Password=@motpasse", Conn);
        Cmd.Parameters.Add(new SqlParameter("@cin", CIN));
        Cmd.Parameters.Add(new SqlParameter("@mid", userId));
        Cmd.Parameters.Add(new SqlParameter("@motpasse", EncodePassword(password)));
        Conn.Open();
        SqlDataReader drEdit = Cmd.ExecuteReader(CommandBehavior.CloseConnection);
        if (drEdit.Read())
        {
            CurrId = drEdit.GetInt32(0);
            RoleId = drEdit.GetInt32(1);
            RoleMID = drEdit.GetString(2);
            MID = drEdit.GetString(3);
            this.CIN = drEdit.GetString(4);
            myConn.WriteAction(bosomaindata.typeAction.Login, CurrId.Value, RoleMID, "Connexion");
            LoginInc();
            return true;
        }
        errorMsg = new bosomessage().LOGIN_INVALID;
        return false;
    }

    /// <summary>
    /// Cette fonction permet d'encrypter un mot de passe.
    /// </summary>
    /// <param name="originalPassword">Ce paramètre permet d'indiquer le mot de passe à encrypter</param>
    /// <returns>Cette fonction retourne une chaine de caractères encodé en MD5</returns>
    private string EncodePassword(string originalPassword)
    {
        return BitConverter.ToString(new MD5CryptoServiceProvider().ComputeHash(ASCIIEncoding.Default.GetBytes(originalPassword)));
    }

    /// <summary>
    /// Cette procédure permet d'incrémenter le décompte du nombre de connexion d'un
    /// utilisateur.
    /// </summary>
    public void LoginInc() {
        bosomaindata myConn = new bosomaindata();
        SqlConnection Conn = myConn.getDBConnection();
        SqlCommand Cmd = new SqlCommand("update bosouser set " +
                                            "LoginCount = LoginCount + 1 " +
                                        "where UserId = @userid", Conn);
        Cmd.Parameters.Add(new SqlParameter("@userid", CurrId));
        Conn.Open();
        Cmd.ExecuteNonQuery();
        Conn.Close();
    }

    /// <summary>
    /// Cette fonction permet de fixer le mot de passe d'un utilisateur.
    /// </summary>
    /// <param name="passwordA">Ce paramètre permet d'indiquer le nouveau mot de passe</param>
    /// <param name="passwordB">Ce paramètre permet d'indiquer le mot de passe de confirmation.</param>
    /// <returns>Cette fonction retourne «true» si le mot de passe a été changé, sinon «false» s'il n'a pas été possible de changer le mot de passe.</returns>
    public bool SavePassword(string passwordA, string passwordB) {
        if ("" == passwordA)
        {
            errorMsg = new bosomessage().PASSWORD_REQUIRED;
            return false;
        }

        if ("" == passwordB)
        {
            errorMsg = new bosomessage().PASSWORD_CONFIRMATION_REQUIRED;
            return false;
        }

        if (passwordA != passwordB)
        {
            errorMsg = new bosomessage().PASSWORD_MISMATCH;
            return false;
        }

        bosovalidate valid = new bosovalidate();
        if (!valid.PasswordValid(passwordA))
        {
            errorMsg = valid.getErrorMsg();
            return false;
        }

        bosomaindata myConn = new bosomaindata();
        SqlConnection Conn = myConn.getDBConnection();
        SqlCommand Cmd = new SqlCommand("update bosouser set " +
                                    "Password = @password, " +
                                    "LastDate = @lastdate," +
                                    "LastUserId = @lastuserid " +
                               "where UserId = @userid", Conn);
        Cmd.Parameters.Add(new SqlParameter("@userid", CurrId));
        Cmd.Parameters.Add(new SqlParameter("@password", EncodePassword(passwordA)));
        Cmd.Parameters.Add(new SqlParameter("@lastdate", System.DateTime.Now));
        Cmd.Parameters.Add(new SqlParameter("@lastuserid", Session["UserId"]));
        Conn.Open();
        Cmd.ExecuteNonQuery();
        Conn.Close();
        myConn.WriteAction(bosomaindata.typeAction.ModifiedData, "User Password");
        return true;
    }

    /// <summary>
    /// Cette fonction permet d'envoyer un courriel quand les informations de compte de 
    /// connexion sont oubliés.
    /// </summary>
    /// <param name="Email">Ce paramètre permet d'indiquer l'adresse de courriel.</param>
    /// <returns>Cette fonction retourne «true» si l'opération s'est correctement déroulé, sinon «false» pour indiquer qu'un problème est survenu.</returns>
    public bool LostMyAccount(string Email) {
        if (EmailExist(Email))
        {
            if (LoadData(CurrId.Value))
            {
                MailMessage objEmail = new MailMessage();
                objEmail.From = new MailAddress(DefaultLostPasswordAdmin);
                objEmail.To.Add("<" + Email + ">,");
                objEmail.Priority = MailPriority.Normal;
                objEmail.IsBodyHtml = true;
                objEmail.Subject = DefaultLostPasswordSubject;
                objEmail.Body = "<html>" +
                          "Bonjour " + LastName + ",<br><br>" +
                          DefaultLostPasswordBodyBegin +
                          "Les informations que tu as besoin pour te reconnecter sont les suivantes:<br><br>" +
                          "Nom utilisateur : " + MID + "<br>" +
                          "Mot de passe : " + Password + "<br><br>" +
                          DefaultLostPasswordBodyEnd +
                          "</html>";
                objEmail.SubjectEncoding = System.Text.Encoding.GetEncoding("ISO-8859-1");
                objEmail.BodyEncoding = System.Text.Encoding.GetEncoding("ISO-8859-1");
                SmtpClient objSmtp = new SmtpClient();
                objSmtp.Host = System.Configuration.ConfigurationManager.AppSettings["MailSMTP"];
                try
                {
                    objSmtp.Send(objEmail);
                }
                catch
                {
                    errorMsg = new bosomessage().EMAIL_SEND_ERROR;
                    return false;
                }
                objEmail.Dispose();
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Cette fonction permet de vérifier la signature de confirmation 
    /// pour une inscription et si elle est valide, son état devient
    /// autorisé.
    /// </summary>
    /// <param name="confirmSign">Ce paramètre permet d'indiquer la signature à vérifié avec celle existant dans la base de données.</param>
    public bool CheckConfirmSign(string confirmSign) {
        bosomaindata myConn = new bosomaindata();
        SqlConnection Conn = myConn.getDBConnection();
        SqlCommand Cmd = new SqlCommand("select " +
                                            "UserId " +
                                        "from bosouser " +
                                        "where ConfirmSign=@confirm and Status='C'", Conn);
        Cmd.Parameters.Add(new SqlParameter("@confirm", confirmSign));
        Conn.Open();
        SqlDataReader drEdit = Cmd.ExecuteReader(CommandBehavior.CloseConnection);
        if (drEdit.Read())
        {
            CurrId = drEdit.GetInt32(0);
            SaveStatus("A");
            return true;
        }
        errorMsg = new bosomessage().USER_NOTINSCRIPTION;
        return false;
    }

    /// <summary>
    /// Cette procédure permet de fixer l'état d'autorisation de l'utilisateur dans la Base de données.
    /// </summary>
    /// <param name="Status">Ce paramètre permet d'indiquer la lettre correspondant à l'état de l'utilisateur: A=Autorisé, C=A confirmé, I=Inactif</param>
    public void SaveStatus(string Status)
    {
        bosomaindata myConn = new bosomaindata();
        SqlConnection Conn = myConn.getDBConnection();
        SqlCommand Cmd = new SqlCommand("update bosouser set " +
                                            "Status = @status " +
                                        "where UserId = @userid", Conn);
        Cmd.Parameters.Add(new SqlParameter("@userid", CurrId));
        Cmd.Parameters.Add(new SqlParameter("@status", Status));
        Conn.Open();
        Cmd.ExecuteNonQuery();
        Conn.Close();
        myConn.WriteAction(bosomaindata.typeAction.SetStatusData, Status);
    }

    /// <summary>
    /// Cette procédure permet de créer une signature de confirmation 
    /// a générer pour une inscription.
    /// </summary>
    public void MakeConfirmSign() {
        Status = "C";
        ConfirmSign = GetRecordKey();
    }

    /// <summary>
    /// Cette fonction permet de retourner une signature de confirmation 
    /// a générer pour une inscription.
    /// </summary>
    /// <returns>Cette fonction retourne la chaine de caractères d'une clef d'enregistrement générer de façon aléatoire.</returns>
    public string GetRecordKey() {
	    string result = "";
        string matrix = "abcdefhijkmnopqrstuvxwyz123456789";
        char[] palette = matrix.ToCharArray();

        Random rand = new Random(DateTime.Now.Millisecond);
        for (int i = 1; i <= 2; i++)
        {
            for (int j = 1; j < palette.Length; j++)
            {
                int p1 = rand.Next(0, palette.Length);
                int p2 = rand.Next(0, palette.Length);
                char t = palette[p1];
                palette[p1] = palette[p2];
                palette[p2] = t;
            }
        }

        for (int i = 0; i <= 32; i++) result += palette[i];
        return result;
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
    /// Cette fonction permet de demander la valeur actuel du champs d'adresse de courriel dans l'objet.
    /// </summary>
    /// <returns>Cette fonction retourner une chaine de caractères contenant l'adresse de courriel</returns>
    public string getEmail()
    {
        return Email;
    }

    /// <summary>
    /// Cette procédure permet de demander la valeur actuel du champs de l'adresse de courriel dans l'objet.
    /// </summary>
    /// <param name="email">Ce paramètre permet d'indiquer la nouvelle valeur que doit obtenir l'adresse de courriel.
    /// </param>
    public void setEmail(string email)
    {
        Email = email;
    }

    /// <summary>
    /// Cette fonction permet de demander la valeur actuel du champs du prénom dans l'objet.
    /// </summary>
    /// <returns>Cette fonction retourner une chaine de caractères contenant le prénom</returns>
    public string getFirstName()
    {
        return FirstName;
    }

    /// <summary>
    /// Cette procédure permet de demander la valeur actuel du champs du prénom dans l'objet.
    /// </summary>
    /// <param name="firstName">Ce paramètre permet d'indiquer la nouvelle valeur que doit obtenir le prénom.
    /// </param>
    public void setFirstName(string firstName)
    {
        FirstName = firstName;
    }

    /// <summary>
    /// Cette fonction permet de demander la valeur actuel du champs du nom de famille dans l'objet.
    /// </summary>
    /// <returns>Cette fonction retourner une chaine de caractères contenant le nom de famille</returns>
    public string getLastName()
    {
        return LastName;
    }

    /// <summary>
    /// Cette procédure permet de demander la valeur actuel du champs du nom de famille dans l'objet.
    /// </summary>
    /// <param name="lastName">Ce paramètre permet d'indiquer la nouvelle valeur que doit obtenir le nom de famille.
    /// </param>
    public void setLastName(string lastName)
    {
        LastName = lastName;
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
    /// Cette fonction permet de demander la valeur actuel du champs de mot de passe dans l'objet.
    /// </summary>
    /// <returns>Cette fonction retourner une chaine de caractères contenant mot de passe</returns>
    public string getPassword()
    {
        return Password;
    }

    /// <summary>
    /// Cette procédure permet de demander la valeur actuel du champs de mot de passe dans l'objet.
    /// </summary>
    /// <param name="password">Ce paramètre permet d'indiquer la nouvelle valeur que doit obtenir le mot de passe
    /// </param>
    public void setPassword(string password)
    {
        Password = password;
    }

    /// <summary>
    /// Cette fonction permet de demander la valeur actuel du champs d'identificateur du rôle dans l'objet.
    /// </summary>
    /// <returns>Cette fonction retourner une entier contenant l'identificateur du rôle</returns>
    public int getRoleId()
    {
        return RoleId;
    }

    /// <summary>
    /// Cette procédure permet de demander la valeur actuel du champs d'identificateur du rôle dans l'objet.
    /// </summary>
    /// <param name="roleId">Ce paramètre permet d'indiquer la nouvelle valeur que doit obtenir l'identificateur du rôle
    /// </param>
    public void setRoleId(int roleId)
    {
        RoleId = roleId;
    }
    

    /// <summary>
    /// Cette fonction permet de demander la valeur actuel du champs de modèle d'identificateur du rôle dans l'objet.
    /// </summary>
    /// <returns>Cette fonction retourner une chaine de caractères contenant le modèle d'identificateur du rôle</returns>
    public string getRoleMID()
    {
        return RoleMID;
    }

    /// <summary>
    /// Cette procédure permet de demander la valeur actuel du champs de modèle d'identificateur du rôle dans l'objet.
    /// </summary>
    /// <param name="roleMID">Ce paramètre permet d'indiquer la nouvelle valeur que doit obtenir le modèle d'identificateur du rôle
    /// </param>
    public void setRoleMID(string roleMID)
    {
        RoleMID = roleMID;
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
    /// Cette fonction permet de demander la valeur actuel du champs d'administration de mots de passe perdu dans l'objet.
    /// </summary>
    /// <returns>Cette fonction retourner une chaine de caractères contenant l'administration de mots de passe perdu</returns>
    public string getDefaultLostPasswordAdmin()
    {
        return DefaultLostPasswordAdmin;
    }

    /// <summary>
    /// Cette procédure permet de demander la valeur actuel du champs d'administration de mots de passe perdu dans l'objet.
    /// </summary>
    /// <param name="defaultLostPasswordAdmin">Ce paramètre permet d'indiquer la nouvelle valeur que doit obtenir l'administration de mots de passe perdu
    /// </param>
    public void setDefaultLostPasswordAdmin(string defaultLostPasswordAdmin)
    {
        DefaultLostPasswordAdmin = defaultLostPasswordAdmin;
    }

    /// <summary>
    /// Cette fonction permet de demander la valeur actuel du champs du sujet de mots de passe perdu dans l'objet.
    /// </summary>
    /// <returns>Cette fonction retourner une chaine de caractères contenant le sujet de mots de passe perdu</returns>
    public string getDefaultLostPasswordSubject()
    {
        return DefaultLostPasswordSubject;
    }

    /// <summary>
    /// Cette procédure permet de demander la valeur actuel du champs du sujet de mots de passe perdu dans l'objet.
    /// </summary>
    /// <param name="defaultLostPasswordSubject">Ce paramètre permet d'indiquer la nouvelle valeur que doit obtenir le sujet de mots de passe perdu
    /// </param>
    public void setDefaultLostPasswordSubject(string defaultLostPasswordSubject)
    {
        DefaultLostPasswordSubject = defaultLostPasswordSubject;
    }
     
    /// <summary>
    /// Cette fonction permet de demander la valeur actuel du champs du début du corps de mots de passe perdu dans l'objet.
    /// </summary>
    /// <returns>Cette fonction retourner une chaine de caractères contenant le début du corps de mots de passe perdu</returns>
    public string getDefaultLostPasswordBodyBegin()
    {
        return DefaultLostPasswordBodyBegin;
    }

    /// <summary>
    /// Cette procédure permet de demander la valeur actuel du champs du début du corps de mots de passe perdu dans l'objet.
    /// </summary>
    /// <param name="defaultLostPasswordBodyBegin">Ce paramètre permet d'indiquer la nouvelle valeur que doit obtenir le début du corps de mots de passe perdu
    /// </param>
    public void setDefaultLostPasswordBodyBegin(string defaultLostPasswordBodyBegin)
    {
        DefaultLostPasswordBodyBegin = defaultLostPasswordBodyBegin;
    }
     
    /// <summary>
    /// Cette fonction permet de demander la valeur actuel du champs de la fin du corps de mots de passe perdu dans l'objet.
    /// </summary>
    /// <returns>Cette fonction retourner une chaine de caractères contenant la fin du corps de mots de passe perdu</returns>
    public string getDefaultLostPasswordBodyEnd()
    {
        return DefaultLostPasswordBodyEnd;
    }

    /// <summary>
    /// Cette procédure permet de demander la valeur actuel du champs de la fin du corps de mots de passe perdu dans l'objet.
    /// </summary>
    /// <param name="defaultLostPasswordBodyEnd">Ce paramètre permet d'indiquer la nouvelle valeur que doit obtenir la fin du corps de mots de passe perdu
    /// </param>
    public void setDefaultLostPasswordBodyEnd(string defaultLostPasswordBodyEnd)
    {
        DefaultLostPasswordBodyEnd = defaultLostPasswordBodyEnd;
    }

    /// <summary>
    /// Cette fonction permet de charger la liste des utilisateurs.
    /// </summary>
    /// <param name="cin">Ce paramètre permet d'indiquer un identificateur client</param>
    /// <param name="field">Ce paramètre permet d'indiquer la liste des champs à retourner</param>
    /// <param name="sort">Ce paramètre permet d'indiquer l'ordre de tri</param>
    /// <returns>Cette fonction retourne objet «DataView» contenant la liste des utilisateurs.</returns>
    public DataView LoadList(string cin, string[] field, string sort)
    {
        if ("" == cin) cin = null;

        bosomaindata myConn = new bosomaindata();
        SqlConnection Conn = myConn.getDBConnection();
        DataSet ds = new DataSet();
        SqlDataAdapter Cmd = new SqlDataAdapter("select " + String.Join(",", field) + " from bosouser where UserId<>-1 " + (null != cin ? " and CIN='" + cin + "' " : "") + " order by " + sort, Conn);
        Cmd.Fill(ds, "mylist");
        return ds.Tables["mylist"].DefaultView;
    }
}
