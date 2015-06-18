CREATE TABLE bosoconfig (
	VariableName varchar(15) NOT NULL PRIMARY KEY,
	VariableValue varchar(15) NULL
);

CREATE TABLE bosolog (
	LogId  		int IDENTITY(1,1) PRIMARY KEY,
	UserId 		int,
        RoleMid 	varchar(15),
	TypeAction      int,
	Message  	Text,
	IPAddr		varchar(15),
	CreateDate 	datetime
);

CREATE TABLE bosorole (
	RoleId  	int NOT NULL PRIMARY KEY,
	MID 		varchar(15),
        Name 		varchar(100),
	Description  	Text,
	CreateDate 	datetime,
	CreateUserId 	int,
	LastDate 	datetime,
	LastUserId 	int
);

CREATE TABLE bosocin (
	CIN 		varchar(5) NOT NULL PRIMARY KEY,
        Name 		varchar(100),
	LanguageId  	int,
	Status		char(1) DEFAULT 'A',
	CreateDate 	datetime,
	CreateUserId 	int,
	LastDate 	datetime,
	LastUserId 	int
);

CREATE TABLE bosouser (
	UserId  	int NOT NULL PRIMARY KEY,
	MID 		varchar(50),
	CIN 		varchar(5),
        RoleId  	int NOT NULL,
	FirstName 	varchar(100),
	LastName 	varchar(100),
	Email		varchar(100),
        Password	varchar(100) DEFAULT '',
	ConfirmSign     varchar(100) DEFAULT '',
        Status		char(1) DEFAULT 'A',
	LoginCount	int DEFAULT 0,
	CreateDate 	datetime,
	CreateUserId 	int,
	LastDate 	datetime,
	LastUserId 	int
);

CREATE TABLE bosotypeaction (
	TypeActionId  	int NOT NULL PRIMARY KEY,
	MID 		varchar(15),
        Name_EN		varchar(100),
	Name_FR		varchar(100),
	CreateDate 	datetime,
	CreateUserId 	int,
	LastDate 	datetime,
	LastUserId 	int
);

CREATE TABLE bosolanguage (
	LanguageId  	int NOT NULL PRIMARY KEY,
	MID 		varchar(15),
        Name_EN		varchar(100),
	Name_FR		varchar(100),
	Status		char(1) DEFAULT 'A',
	CreateDate 	datetime,
	CreateUserId 	int,
	LastDate 	datetime,
	LastUserId 	int
);

CREATE TABLE bosocontent (
	ContentId  	int NOT NULL PRIMARY KEY,
	MID 		varchar(15),
	CIN 		varchar(5) DEFAULT '',
        Name		varchar(100),
	Description	text,
	Status		char(1) DEFAULT 'A',
	CreateDate 	datetime,
	CreateUserId 	int,
	LastDate 	datetime,
	LastUserId 	int
);

INSERT [bosorole] ([RoleId], [MID], [Name], [Description], [CreateDate], [CreateUserId], [LastDate], [LastUserId]) VALUES (1, convert(text, N'ADMIN' collate SQL_Latin1_General_CP1_CI_AS), convert(text, N'Administrateur' collate SQL_Latin1_General_CP1_CI_AS), convert(text, N'Administrateur client' collate SQL_Latin1_General_CP1_CI_AS), CAST(0x00009A4D00F82327 AS DateTime), -1, CAST(0x00009A4E0092444D AS DateTime), -1)
INSERT [bosorole] ([RoleId], [MID], [Name], [Description], [CreateDate], [CreateUserId], [LastDate], [LastUserId]) VALUES (7, convert(text, N'SYSADMIN' collate SQL_Latin1_General_CP1_CI_AS), convert(text, N'Administrateur système' collate SQL_Latin1_General_CP1_CI_AS), convert(text, N'L''Administrateur du système' collate SQL_Latin1_General_CP1_CI_AS), CAST(0x00009A4E00871967 AS DateTime), -1, CAST(0x00009A5000B8FA19 AS DateTime), -1)
INSERT [bosorole] ([RoleId], [MID], [Name], [Description], [CreateDate], [CreateUserId], [LastDate], [LastUserId]) VALUES (8, convert(text, N'BLG' collate SQL_Latin1_General_CP1_CI_AS), convert(text, N'Blogueur' collate SQL_Latin1_General_CP1_CI_AS), convert(text, N'Sujet d''entrée de données blogueur' collate SQL_Latin1_General_CP1_CI_AS), CAST(0x00009A4E0087A28C AS DateTime), -1, CAST(0x00009A4E00BD1338 AS DateTime), -1)
INSERT [bosorole] ([RoleId], [MID], [Name], [Description], [CreateDate], [CreateUserId], [LastDate], [LastUserId]) VALUES (13, convert(text, N'GUEST' collate SQL_Latin1_General_CP1_CI_AS), convert(text, N'Invité' collate SQL_Latin1_General_CP1_CI_AS), convert(text, N'N''importe quel invité' collate SQL_Latin1_General_CP1_CI_AS), CAST(0x00009A4E0115650B AS DateTime), -1, CAST(0x00009A5800FCA054 AS DateTime), -1)

INSERT [bosotypeaction] ([TypeActionId], [MID], [Name_EN], [Name_FR], [CreateDate], [CreateUserId], [LastDate], [LastUserId]) VALUES (14, convert(text, N'1' collate SQL_Latin1_General_CP1_CI_AS), convert(text, N'Login' collate SQL_Latin1_General_CP1_CI_AS), convert(text, N'Connexion' collate SQL_Latin1_General_CP1_CI_AS), CAST(0x00009A4F0097A253 AS DateTime), -1, CAST(0x00009A5800FBB128 AS DateTime), -1)
INSERT [bosotypeaction] ([TypeActionId], [MID], [Name_EN], [Name_FR], [CreateDate], [CreateUserId], [LastDate], [LastUserId]) VALUES (15, convert(text, N'2' collate SQL_Latin1_General_CP1_CI_AS), convert(text, N'Read Data' collate SQL_Latin1_General_CP1_CI_AS), convert(text, N'Lecture de données' collate SQL_Latin1_General_CP1_CI_AS), CAST(0x00009A4F0097D2A5 AS DateTime), -1, CAST(0x00009A4F0097D2A5 AS DateTime), -1)
INSERT [bosotypeaction] ([TypeActionId], [MID], [Name_EN], [Name_FR], [CreateDate], [CreateUserId], [LastDate], [LastUserId]) VALUES (16, convert(text, N'3' collate SQL_Latin1_General_CP1_CI_AS), convert(text, N'Add Data' collate SQL_Latin1_General_CP1_CI_AS), convert(text, N'Ajout de données' collate SQL_Latin1_General_CP1_CI_AS), CAST(0x00009A4F00985C32 AS DateTime), -1, CAST(0x00009A4F00985C32 AS DateTime), -1)
INSERT [bosotypeaction] ([TypeActionId], [MID], [Name_EN], [Name_FR], [CreateDate], [CreateUserId], [LastDate], [LastUserId]) VALUES (17, convert(text, N'4' collate SQL_Latin1_General_CP1_CI_AS), convert(text, N'Modified Data' collate SQL_Latin1_General_CP1_CI_AS), convert(text, N'Modification de données' collate SQL_Latin1_General_CP1_CI_AS), CAST(0x00009A4F0098729B AS DateTime), -1, CAST(0x00009A4F0098729B AS DateTime), -1)
INSERT [bosotypeaction] ([TypeActionId], [MID], [Name_EN], [Name_FR], [CreateDate], [CreateUserId], [LastDate], [LastUserId]) VALUES (18, convert(text, N'5' collate SQL_Latin1_General_CP1_CI_AS), convert(text, N'Delete Data' collate SQL_Latin1_General_CP1_CI_AS), convert(text, N'Suppression de données' collate SQL_Latin1_General_CP1_CI_AS), CAST(0x00009A4F00989E00 AS DateTime), -1, CAST(0x00009A4F0098B610 AS DateTime), -1)
INSERT [bosotypeaction] ([TypeActionId], [MID], [Name_EN], [Name_FR], [CreateDate], [CreateUserId], [LastDate], [LastUserId]) VALUES (19, convert(text, N'6' collate SQL_Latin1_General_CP1_CI_AS), convert(text, N'List View Data' collate SQL_Latin1_General_CP1_CI_AS), convert(text, N'Visualisation d''une liste de données' collate SQL_Latin1_General_CP1_CI_AS), CAST(0x00009A4F0098CDA5 AS DateTime), -1, CAST(0x00009A4F0098CDA5 AS DateTime), -1)
INSERT [bosotypeaction] ([TypeActionId], [MID], [Name_EN], [Name_FR], [CreateDate], [CreateUserId], [LastDate], [LastUserId]) VALUES (20, convert(text, N'7' collate SQL_Latin1_General_CP1_CI_AS), convert(text, N'Search Data' collate SQL_Latin1_General_CP1_CI_AS), convert(text, N'Recherche de données' collate SQL_Latin1_General_CP1_CI_AS), CAST(0x00009A4F00991F83 AS DateTime), -1, CAST(0x00009A4F00991F83 AS DateTime), -1)
INSERT [bosotypeaction] ([TypeActionId], [MID], [Name_EN], [Name_FR], [CreateDate], [CreateUserId], [LastDate], [LastUserId]) VALUES (21, convert(text, N'8' collate SQL_Latin1_General_CP1_CI_AS), convert(text, N'Set Status Data' collate SQL_Latin1_General_CP1_CI_AS), convert(text, N'Fixe une données d''état' collate SQL_Latin1_General_CP1_CI_AS), CAST(0x00009A4F00994244 AS DateTime), -1, CAST(0x00009A4F00994244 AS DateTime), -1)
INSERT [bosotypeaction] ([TypeActionId], [MID], [Name_EN], [Name_FR], [CreateDate], [CreateUserId], [LastDate], [LastUserId]) VALUES (22, convert(text, N'254' collate SQL_Latin1_General_CP1_CI_AS), convert(text, N'Error Action' collate SQL_Latin1_General_CP1_CI_AS), convert(text, N'Action d''une erreur inconnu' collate SQL_Latin1_General_CP1_CI_AS), CAST(0x00009A4F0099651C AS DateTime), -1, CAST(0x00009A4F0099651C AS DateTime), -1)

INSERT [bosolanguage] ([LanguageId], [MID], [Name_EN], [Name_FR], [Status], [CreateDate], [CreateUserId], [LastDate], [LastUserId]) VALUES (33, convert(text, N'fr' collate SQL_Latin1_General_CP1_CI_AS), convert(text, N'French' collate SQL_Latin1_General_CP1_CI_AS), convert(text, N'Français' collate SQL_Latin1_General_CP1_CI_AS), convert(text, N'A' collate SQL_Latin1_General_CP1_CI_AS), CAST(0x00009A4F01171E59 AS DateTime), -1, CAST(0x00009A5600FD3A90 AS DateTime), -1)
INSERT [bosolanguage] ([LanguageId], [MID], [Name_EN], [Name_FR], [Status], [CreateDate], [CreateUserId], [LastDate], [LastUserId]) VALUES (34, convert(text, N'en' collate SQL_Latin1_General_CP1_CI_AS), convert(text, N'English' collate SQL_Latin1_General_CP1_CI_AS), convert(text, N'Anglais' collate SQL_Latin1_General_CP1_CI_AS), convert(text, N'A' collate SQL_Latin1_General_CP1_CI_AS), CAST(0x00009A4F0117C828 AS DateTime), -1, CAST(0x00009A5800FA6F91 AS DateTime), -1)

