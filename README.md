Cadre d'application ASP .NET (C#) développé en 2008 pour .NET version 2.0 Ce cadre d'application 
est assez rudimentaire, il n'est plus d'actualité, cependant, le côté éducatif lui reste toujours pertinent.

<h3>Outils de développement</h3>h3

L’outil de développement pour le site Web est «Visual Web Developer 2005 Express» et l’outil permettant d’effectuer une compilation de « BOSO » en « DLL »  est «Visual C# Express ».

Si vous utilisez « SQL Server 2005 » comme dans le développement du projet, il serait souhaitable d’utiliser l’outil « SQL Server Management Studio Express ».

<h3>Objectif du projet</h3>

Fournir une base « MVC » ou un « Framework » en C# (ASP .NET).

<h3>Installation</h3>

<h4>Base de données</h4>

Pour pouvoir utiliser BOSO, il faut d’abord lancer le fichier contenant les scripts nécessaires à la création des tables de sa base de données à l’aide du fichier «boso.sql ». En théorie seule les tables sont indispensables et vous n’avez pas besoin d’effectuer les « INSERT » contenu dans le même fichier.

Ensuite, il faut ajuster la configuration de connexion à la base de données à l’aide «Web.Config» en site Web ou « boso.dll.Config» s’il s’agit d’une DLL.

Ainsi la clef «DBConnection» du chemin « configuration/ appSettings », permet d’indiquer les paramètres à la base de données. Par exemple, sur le serveur de teste, la base de données est accessible sur le serveur  « mssql.netc.net », avec le nom de base de données «mpedev », l’utilisateur est « mpedev » et le mots de passe « petdyps1 » et on retrouvera donc les balises suivantes :

<code>
&lt;appSettings&gt;
	&lt;add key="DBConnection" value="server=mssql.gladir.com;uid=boso;pwd=password;database=boso" /&gt;
&lt;/appSettings&gt;
</code>  

Courriel

Afin de pouvoir faire fonctionner l’envoi des courriels, vous devez modifier le fichier « web.config » situé dans le répertoire racine du projet Web. Ainsi la clef « MailSMTP » du chemin « configuration/ appSettings », contient l’adresse du serveur de courriel. Il doit naturellement correspondre avec le nom de domaine du site sur lequel il fonctionne. 

Par exemple, sur le serveur de teste, le nom « mpedev.netc.net », on retrouvera donc les balises suivantes :

<code>
  &lt;appSettings&gt;
    &lt;add key="MailSMTP" value="mail.gladir.com"/&gt;
&lt;/appSettings&gt;

</code>

Les services de courriel sont requis si l’utilisateur perd son mot de passe ou identificateur utilisateur. Ainsi que lorsqu’on effectue une confirmation d’un compte utilisateur.

<h3>Connexion</h3>

L’utilisateur pouvant se connecté peu importe le CIN dans le site Web : « sysadmin » et son mot de passe est « enterprise ». Il est possible de changer son mot de passe et l’identificateur utilisé dans le fichier « Web.config ».



<h3>Les concepts</h3>

<h4>Administration</h4>

Il existe deux rôles d’administrateurs : « sysadmin » et « admin ». Le premier est un administrateur système et il ne peut que modifier des informations de bas niveau et les utilisateurs. Le deuxième un administrateur ayant tous les droits à l’exception de la partie système de bas niveau dans un CIN en particulier.

<h4>CIN</h4>

Le CIN n’existe pas à proprement parler pour l’administrateur système mais seulement pour l’administrateur. Il s’agit d’un identificateur supplémentaire à l’utilisateur permettant de partager une même base de données avec plusieurs clients différents. Ce concept s’inspire des méga-systèmes comme SAP.

Lorsqu’on développera de nouvelle composante, il faudra donc ajouter un champ CIN dans la table afin d’être compatible avec se concept.

<h4>ID</h4>

Dans le système de base de données, un système est prévu afin qu’un identificateur unique peu importe la table soit fournit par le système. Dans la classe « bosomaindata », la méthode « getCurrId() » fournit l’identificateur suivant disponible. Cette technique s’inspire d’un identificateur ISO, laquelle permet de retrouver dans une base de données un information quelconqu’on en fonction de son numéro.
