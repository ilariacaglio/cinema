create database Cinema_Caglio_Ilaria;

use Cinema_Caglio_Ilaria;

create table if not exists Sala (
	Id int auto_increment primary key,
    Nposti int not null,
    Nfile int not null,
    Isense boolean not null
);

create table if not exists Posto (
	Id int auto_increment primary key,
    Fila int not null,
    Numero int not null,
    Costo double(4,2) not null,
    IdSala int not null,
    foreign key (IdSala) references Sala (Id) on update cascade on delete cascade
);

create table if not exists Utente (
	Cognome varchar(20) not null,
    Nome varchar(20) not null,
    Mail varchar(50) primary key,
    Password varchar(50) not null,
    Sesso varchar(1) not null,
    Nascita Date not null,
    Residenza varchar(50) not null
);

create table if not exists Genere (
	Id int auto_increment primary key,
    Nome varchar(20) not null
);

create table if not exists Film (
	Id int auto_increment primary key,
    Titolo varchar(50) not null,
    Durata int not null,
    Anno date not null,
    Descrizione varchar(500),
    Img varchar(200),
    IdGenere int not null,
    foreign key (IdGenere) references Genere (Id) on update cascade on delete cascade
);

create table if not exists Spettacolo (
	Data date,
    Ora time, 
    IdFilm int not null,
    IdSala int,
    primary key (Data, Ora, IdSala),
    foreign key (IdSala) references Sala (Id) on update cascade on delete cascade,
    foreign key (IdFilm) references FIlm (Id) on update cascade on delete cascade
);

create table if not exists Prenotazione (
	Id int auto_increment primary key,
    DataS date,
    OraS time,
    IdSala int,
    IdUtente varchar(50),
    foreign key (DataS, OraS, IdSala) references Spettacolo (Data, Ora, IdSala) on update cascade on delete cascade,
    foreign key (IdUtente) references Utente (Mail) on update cascade on delete cascade
);

create table if not exists Comprende (
	IdPosto int,
    IdPrenotazione int,
    primary key (IdPosto, IdPrenotazione),
    foreign key (IdPosto) references Posto (Id) on update cascade on delete cascade,
    foreign key (IdPrenotazione) references Prenotazione (Id) on update cascade on delete cascade
);

create table if not exists Valutazione (
	IdUtente varchar(50),
    IdFilm int,
    Voto double(2,1) not null,
    primary key (IdUtente, IdFilm),
    foreign key (IdUtente) references Utente(Mail) on update cascade on delete cascade,
    foreign key (IdFilm) references Film (Id) on update cascade on delete cascade
);

CREATE TABLE if not exists OrderHeaders (
    Id INT auto_increment PRIMARY KEY,
    UtenteId VARCHAR(50) NOT NULL,
    DataOrdine DATETIME NOT NULL,
    TotaleOrdine FLOAT NOT NULL,
    StatoOrdine VARCHAR(500),
    StatoPagamento VARCHAR(500),
    TrackingNumber VARCHAR(500),
    Carrier VARCHAR(500),
    PaymentDate DATETIME,
    PaymentDueDate DATETIME,
    SessionId VARCHAR(500),
    PaymentIntentId VARCHAR(500),
    PhoneNumber VARCHAR(500) NOT NULL,
    StreetAddress VARCHAR(500) NOT NULL,
    City VARCHAR(500),
    State VARCHAR(500),
    PostalCode VARCHAR(500),
    Name VARCHAR(500) NOT NULL,
    FOREIGN KEY (UtenteId) REFERENCES AspNetUsers(Id) on update cascade on delete cascade
);

CREATE TABLE if not exists OrderDetails (
   Id INT auto_increment PRIMARY KEY,
    OrderId INT NOT NULL,
    PrenotazioneId INT NOT NULL,
    Price FLOAT,
    FOREIGN KEY (OrderId) REFERENCES OrderHeaders(Id) on update cascade on delete cascade,
    FOREIGN KEY (PrenotazioneId) REFERENCES Prenotazione(Id) on update cascade on delete cascade
);