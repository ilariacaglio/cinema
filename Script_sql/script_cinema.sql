create database cinema
CHARACTER SET utf8mb4
COLLATE utf8mb4_general_ci;

use cinema;

CREATE TABLE AspNetRoles (
  Id varchar(255) NOT NULL,
  Name varchar(256) DEFAULT NULL,
  NormalizedName varchar(256) DEFAULT NULL,
  ConcurrencyStamp longtext DEFAULT NULL,
  PRIMARY KEY (Id),
  UNIQUE KEY RoleNameIndex (NormalizedName)
);

CREATE TABLE AspNetUsers (
  Id varchar(255) NOT NULL,
  UserName varchar(256) DEFAULT NULL,
  NormalizedUserName varchar(256) DEFAULT NULL,
  Email varchar(256) DEFAULT NULL,
  NormalizedEmail varchar(256) DEFAULT NULL,
  EmailConfirmed tinyint(1) NOT NULL,
  PasswordHash varchar(200) DEFAULT NULL,
  SecurityStamp longtext DEFAULT NULL,
  ConcurrencyStamp longtext DEFAULT NULL,
  PhoneNumber longtext DEFAULT NULL,
  PhoneNumberConfirmed tinyint(1) NOT NULL,
  TwoFactorEnabled tinyint(1) NOT NULL,
  LockoutEnd datetime(6) DEFAULT NULL,
  LockoutEnabled tinyint(1) NOT NULL,
  AccessFailedCount int(11) NOT NULL,
  Cognome varchar(20) DEFAULT NULL,
  Discriminator longtext NOT NULL,
  Nascita date DEFAULT NULL,
  Nome varchar(20) DEFAULT NULL,
  Residenza varchar(50) DEFAULT NULL,
  Sesso varchar(1) DEFAULT NULL,
  PRIMARY KEY (Id),
  UNIQUE KEY UserNameIndex (NormalizedUserName),
  KEY EmailIndex (NormalizedEmail)
);

CREATE TABLE AspNetRoleClaims (
  Id int(11) NOT NULL AUTO_INCREMENT,
  RoleId varchar(255) NOT NULL,
  ClaimType longtext DEFAULT NULL,
  ClaimValue longtext DEFAULT NULL,
  PRIMARY KEY (Id),
  KEY IX_AspNetRoleClaims_RoleId (RoleId),
  CONSTRAINT FK_AspNetRoleClaims_AspNetRoles_RoleId FOREIGN KEY (RoleId) REFERENCES AspNetRoles (Id) ON DELETE CASCADE
);

CREATE TABLE AspNetUserClaims (
  Id int(11) NOT NULL AUTO_INCREMENT,
  UserId varchar(255) NOT NULL,
  ClaimType longtext DEFAULT NULL,
  ClaimValue longtext DEFAULT NULL,
  PRIMARY KEY (Id),
  KEY IX_AspNetUserClaims_UserId (UserId),
  CONSTRAINT FK_AspNetUserClaims_AspNetUsers_UserId FOREIGN KEY (UserId) REFERENCES AspNetUsers (Id) ON DELETE CASCADE
);

CREATE TABLE AspNetUserLogins (
  LoginProvider varchar(255) NOT NULL,
  ProviderKey varchar(255) NOT NULL,
  ProviderDisplayName longtext DEFAULT NULL,
  UserId varchar(255) NOT NULL,
  PRIMARY KEY (LoginProvider,ProviderKey),
  KEY IX_AspNetUserLogins_UserId (UserId),
  CONSTRAINT FK_AspNetUserLogins_AspNetUsers_UserId FOREIGN KEY (UserId) REFERENCES AspNetUsers (Id) ON DELETE CASCADE
);

CREATE TABLE AspNetUserRoles (
  UserId varchar(255) NOT NULL,
  RoleId varchar(255) NOT NULL,
  PRIMARY KEY (UserId,RoleId),
  KEY IX_AspNetUserRoles_RoleId (RoleId),
  CONSTRAINT FK_AspNetUserRoles_AspNetRoles_RoleId FOREIGN KEY (RoleId) REFERENCES AspNetRoles (Id) ON DELETE CASCADE,
  CONSTRAINT FK_AspNetUserRoles_AspNetUsers_UserId FOREIGN KEY (UserId) REFERENCES AspNetUsers (Id) ON DELETE CASCADE
);

CREATE TABLE AspNetUserTokens (
  UserId varchar(255) NOT NULL,
  LoginProvider varchar(255) NOT NULL,
  Name varchar(255) NOT NULL,
  Value longtext DEFAULT NULL,
  PRIMARY KEY (UserId,LoginProvider,Name),
  CONSTRAINT FK_AspNetUserTokens_AspNetUsers_UserId FOREIGN KEY (UserId) REFERENCES AspNetUsers (Id) ON DELETE CASCADE
);

CREATE TABLE __EFMigrationsHistory (
  MigrationId varchar(150) NOT NULL,
  ProductVersion varchar(32) NOT NULL,
  PRIMARY KEY (MigrationId)
);

CREATE TABLE Genere (
  Id int NOT NULL AUTO_INCREMENT,
  Nome varchar(20) NOT NULL,
  PRIMARY KEY (Id)
);

CREATE TABLE Film (
  Id int NOT NULL AUTO_INCREMENT,
  Titolo varchar(50) NOT NULL,
  Durata int NOT NULL,
  Anno date NOT NULL,
  Descrizione varchar(500) NOT NULL,
  Img varchar(200) DEFAULT NULL,
  IdGenere int NOT NULL,
  PRIMARY KEY (Id),
  KEY IdGenere (IdGenere),
  CONSTRAINT film_ibfk_1 FOREIGN KEY (IdGenere) REFERENCES Genere (Id) ON DELETE CASCADE
);

CREATE TABLE Sala (
  Id int NOT NULL AUTO_INCREMENT,
  Nposti int NOT NULL,
  Nfile int NOT NULL,
  Isense boolean NOT NULL,
  PRIMARY KEY (Id)
);

CREATE TABLE Posto (
  Id int NOT NULL AUTO_INCREMENT,
  Fila int NOT NULL,
  Numero int NOT NULL,
  Costo double(4,2) NOT NULL,
  IdSala int NOT NULL,
  PRIMARY KEY (Id),
  KEY IdSala (IdSala),
  CONSTRAINT posto_ibfk_1 FOREIGN KEY (IdSala) REFERENCES Sala (Id) ON DELETE CASCADE
);

CREATE TABLE Spettacolo (
  Data date NOT NULL,
  Ora time NOT NULL,
  IdSala int NOT NULL,
  IdFilm int NOT NULL,
  PRIMARY KEY (Data,Ora,IdSala),
  KEY IdFilm (IdFilm),
  KEY IdSala1 (IdSala),
  CONSTRAINT spettacolo_ibfk_1 FOREIGN KEY (IdSala) REFERENCES Sala (Id) ON DELETE CASCADE,
  CONSTRAINT spettacolo_ibfk_2 FOREIGN KEY (IdFilm) REFERENCES Film (Id) ON DELETE CASCADE
);

CREATE TABLE if not exists Prenotazione (
  Id int primary key AUTO_INCREMENT,
  DataS date NOT NULL,
  OraS time NOT NULL,
  IdSala int NOT NULL,
  IdUtente varchar(255) NOT NULL,
  Pagato boolean NOT NULL,
  FOREIGN KEY (DataS, OraS, IdSala) REFERENCES Spettacolo (Data, Ora, IdSala) ON DELETE CASCADE,
  FOREIGN KEY (IdUtente) REFERENCES AspNetUsers (Id) ON DELETE CASCADE
);

CREATE TABLE if not exists  Comprende (
  IdPosto int NOT NULL,
  IdPrenotazione int  NOT NULL,
  PRIMARY KEY (IdPosto,IdPrenotazione),
  FOREIGN KEY (IdPosto) REFERENCES Posto (Id) ON DELETE CASCADE,
  FOREIGN KEY (IdPrenotazione) REFERENCES Prenotazione (Id) ON DELETE CASCADE
);

CREATE TABLE if not exists Valutazione (
  IdUtente varchar(255) NOT NULL,
  IdFilm int NOT NULL,
  Voto double(2,1) NOT NULL,
  PRIMARY KEY (IdUtente,IdFilm),
  KEY IdFilm1 (IdFilm),
  CONSTRAINT valutazione_ibfk_1 FOREIGN KEY (IdUtente) REFERENCES AspNetUsers (Id) ON DELETE CASCADE,
  CONSTRAINT valutazione_ibfk_2 FOREIGN KEY (IdFilm) REFERENCES Film (Id) ON DELETE CASCADE
);

CREATE TABLE if not exists ShoppingCarts (
  Id int NOT NULL AUTO_INCREMENT,
  PrenotazioneId int NOT NULL,
  UtenteId varchar(255) NOT NULL,
  PRIMARY KEY (Id),
  KEY IX_ShoppingCarts_PrenotazioneId (PrenotazioneId),
  KEY IX_ShoppingCarts_UtenteId (UtenteId),
  CONSTRAINT FK_ShoppingCarts_AspNetUsers_UtenteId FOREIGN KEY (UtenteId) REFERENCES AspNetUsers (Id) ON DELETE CASCADE,
  CONSTRAINT FK_ShoppingCarts_Prenotazione_PrenotazioneId FOREIGN KEY (PrenotazioneId) REFERENCES Prenotazione (Id) ON DELETE CASCADE
);

CREATE TABLE if not exists OrderHeaders (
  Id int NOT NULL AUTO_INCREMENT,
  UtenteId varchar(50) NOT NULL,
  DataOrdine datetime NOT NULL,
  TotaleOrdine float NOT NULL,
  StatoOrdine varchar(500) DEFAULT NULL,
  StatoPagamento varchar(500) DEFAULT NULL,
  TrackingNumber varchar(500) DEFAULT NULL,
  Carrier varchar(500) DEFAULT NULL,
  PaymentDate datetime DEFAULT NULL,
  PaymentDueDate datetime DEFAULT NULL,
  SessionId varchar(500) DEFAULT NULL,
  PaymentIntentId varchar(500) DEFAULT NULL,
  PhoneNumber varchar(500) NOT NULL,
  StreetAddress varchar(500) NOT NULL,
  City varchar(500) DEFAULT NULL,
  State varchar(500) DEFAULT NULL,
  PostalCode varchar(500) DEFAULT NULL,
  Name varchar(500) NOT NULL,
  PRIMARY KEY (Id),
  KEY UtenteId (UtenteId),
  CONSTRAINT orderheaders_ibfk_1 FOREIGN KEY (UtenteId) REFERENCES AspNetUsers (Id) ON DELETE CASCADE ON UPDATE CASCADE
);

CREATE TABLE if not exists OrderDetails (
  Id int NOT NULL AUTO_INCREMENT,
  OrderId int NOT NULL,
  PrenotazioneId int NOT NULL,
  Price float DEFAULT NULL,
  PRIMARY KEY (Id),
  KEY OrderId (OrderId),
  KEY PrenotazioneId (PrenotazioneId),
  CONSTRAINT orderdetails_ibfk_1 FOREIGN KEY (OrderId) REFERENCES OrderHeaders (Id) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT orderdetails_ibfk_2 FOREIGN KEY (PrenotazioneId) REFERENCES Prenotazione (Id) ON DELETE CASCADE ON UPDATE CASCADE
);