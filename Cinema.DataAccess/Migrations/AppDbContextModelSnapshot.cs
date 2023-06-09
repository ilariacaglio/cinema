﻿// <auto-generated />
using System;
using Cinema.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Cinema.DataAccess.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseCollation("utf8mb4_general_ci")
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.HasCharSet(modelBuilder, "utf8mb4");

            modelBuilder.Entity("Cinema.Models.Comprende", b =>
                {
                    b.Property<int>("IdPosto")
                        .HasColumnType("int(11)");

                    b.Property<int>("IdPrenotazione")
                        .HasColumnType("int(11)");

                    b.HasKey("IdPosto", "IdPrenotazione")
                        .HasName("PRIMARY")
                        .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                    b.HasIndex(new[] { "IdPrenotazione" }, "IdPrenotazione");

                    b.ToTable("Comprende", (string)null);
                });

            modelBuilder.Entity("Cinema.Models.Film", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int(11)");

                    b.Property<DateOnly>("Anno")
                        .HasColumnType("date");

                    b.Property<string>("Descrizione")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("varchar(500)");

                    b.Property<int>("Durata")
                        .HasColumnType("int(11)");

                    b.Property<int>("IdGenere")
                        .HasColumnType("int(11)");

                    b.Property<string>("Img")
                        .HasMaxLength(200)
                        .HasColumnType("varchar(200)");

                    b.Property<string>("Titolo")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.HasKey("Id")
                        .HasName("PRIMARY");

                    b.HasIndex(new[] { "IdGenere" }, "IdGenere");

                    b.ToTable("Film", (string)null);
                });

            modelBuilder.Entity("Cinema.Models.Genere", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int(11)");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("varchar(20)");

                    b.HasKey("Id")
                        .HasName("PRIMARY");

                    b.ToTable("Genere", (string)null);
                });

            modelBuilder.Entity("Cinema.Models.OrderDetails", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("OrderId")
                        .HasColumnType("int");

                    b.Property<int>("PrenotazioneId")
                        .HasColumnType("int(11)");

                    b.Property<double>("Price")
                        .HasColumnType("double");

                    b.HasKey("Id");

                    b.HasIndex("OrderId");

                    b.HasIndex("PrenotazioneId");

                    b.ToTable("OrderDetails");
                });

            modelBuilder.Entity("Cinema.Models.OrderHeader", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Carrier")
                        .HasColumnType("longtext");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("DataOrdine")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime?>("PaymentDate")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime?>("PaymentDueDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("PaymentIntentId")
                        .HasColumnType("longtext");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("PostalCode")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("SessionId")
                        .HasColumnType("longtext");

                    b.Property<string>("State")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("StatoOrdine")
                        .HasColumnType("longtext");

                    b.Property<string>("StatoPagamento")
                        .HasColumnType("longtext");

                    b.Property<string>("StreetAddress")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<double>("TotaleOrdine")
                        .HasColumnType("double");

                    b.Property<string>("TrackingNumber")
                        .HasColumnType("longtext");

                    b.Property<string>("UtenteId")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("UtenteId");

                    b.ToTable("OrderHeaders");
                });

            modelBuilder.Entity("Cinema.Models.Posto", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int(11)");

                    b.Property<double>("Costo")
                        .HasColumnType("double(4,2)");

                    b.Property<int>("Fila")
                        .HasColumnType("int(11)");

                    b.Property<int>("IdSala")
                        .HasColumnType("int(11)");

                    b.Property<int>("Numero")
                        .HasColumnType("int(11)");

                    b.HasKey("Id")
                        .HasName("PRIMARY");

                    b.HasIndex(new[] { "IdSala" }, "IdSala");

                    b.ToTable("Posto", (string)null);
                });

            modelBuilder.Entity("Cinema.Models.Prenotazione", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int(11)");

                    b.Property<DateOnly>("DataS")
                        .HasColumnType("date");

                    b.Property<int>("IdSala")
                        .HasColumnType("int(11)");

                    b.Property<string>("IdUtente")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<TimeOnly>("OraS")
                        .HasColumnType("time");

                    b.Property<bool>("Pagato")
                        .HasColumnType("boolean");

                    b.HasKey("Id")
                        .HasName("PRIMARY");

                    b.HasIndex(new[] { "DataS", "OraS", "IdSala" }, "DataS");

                    b.HasIndex(new[] { "IdUtente" }, "IdUtente");

                    b.ToTable("Prenotazione", null, t =>
                        {
                            t.ExcludeFromMigrations();
                        });
                });

            modelBuilder.Entity("Cinema.Models.Sala", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int(11)");

                    b.Property<bool>("Isense")
                        .HasColumnType("tinyint(1)");

                    b.Property<int>("Nfile")
                        .HasColumnType("int(11)");

                    b.Property<int>("Nposti")
                        .HasColumnType("int(11)");

                    b.HasKey("Id")
                        .HasName("PRIMARY");

                    b.ToTable("Sala", (string)null);
                });

            modelBuilder.Entity("Cinema.Models.ShoppingCart", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("PrenotazioneId")
                        .HasColumnType("int(11)");

                    b.Property<string>("UtenteId")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("PrenotazioneId");

                    b.HasIndex("UtenteId");

                    b.ToTable("ShoppingCarts");
                });

            modelBuilder.Entity("Cinema.Models.Spettacolo", b =>
                {
                    b.Property<DateOnly>("Data")
                        .HasColumnType("date");

                    b.Property<TimeOnly>("Ora")
                        .HasColumnType("time");

                    b.Property<int>("IdSala")
                        .HasColumnType("int(11)");

                    b.Property<int>("IdFilm")
                        .HasColumnType("int(11)");

                    b.HasKey("Data", "Ora", "IdSala")
                        .HasName("PRIMARY")
                        .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0, 0 });

                    b.HasIndex(new[] { "IdFilm" }, "IdFilm");

                    b.HasIndex(new[] { "IdSala" }, "IdSala")
                        .HasDatabaseName("IdSala1");

                    b.ToTable("Spettacolo", (string)null);
                });

            modelBuilder.Entity("Cinema.Models.Valutazione", b =>
                {
                    b.Property<string>("IdUtente")
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<int>("IdFilm")
                        .HasColumnType("int(11)");

                    b.Property<double>("Voto")
                        .HasColumnType("double(2,1)");

                    b.HasKey("IdUtente", "IdFilm")
                        .HasName("PRIMARY")
                        .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                    b.HasIndex(new[] { "IdFilm" }, "IdFilm")
                        .HasDatabaseName("IdFilm1");

                    b.ToTable("Valutazione", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("ClaimType")
                        .HasColumnType("longtext");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("longtext");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("longtext");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("longtext");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("longtext");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.ToTable("AspNetUsers", (string)null);

                    b.HasDiscriminator<string>("Discriminator").HasValue("IdentityUser");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("ClaimType")
                        .HasColumnType("longtext");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("longtext");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("longtext");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("RoleId")
                        .HasColumnType("varchar(255)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Name")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Value")
                        .HasColumnType("longtext");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("Cinema.Models.Utente", b =>
                {
                    b.HasBaseType("Microsoft.AspNetCore.Identity.IdentityUser");

                    b.Property<string>("Cognome")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("varchar(20)");

                    b.Property<DateOnly>("Nascita")
                        .HasColumnType("date");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("varchar(20)");

                    b.Property<string>("Residenza")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Sesso")
                        .IsRequired()
                        .HasMaxLength(1)
                        .HasColumnType("varchar(1)");

                    b.HasDiscriminator().HasValue("Utente");
                });

            modelBuilder.Entity("Cinema.Models.Comprende", b =>
                {
                    b.HasOne("Cinema.Models.Posto", "Id")
                        .WithMany("Comprendes")
                        .HasForeignKey("IdPosto")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("comprende_ibfk_1");

                    b.HasOne("Cinema.Models.Prenotazione", "Prenotazione")
                        .WithMany("Comprendes")
                        .HasForeignKey("IdPrenotazione")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("comprende_ibfk_2");

                    b.Navigation("Id");

                    b.Navigation("Prenotazione");
                });

            modelBuilder.Entity("Cinema.Models.Film", b =>
                {
                    b.HasOne("Cinema.Models.Genere", "IdGenereNavigation")
                        .WithMany("Films")
                        .HasForeignKey("IdGenere")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("film_ibfk_1");

                    b.Navigation("IdGenereNavigation");
                });

            modelBuilder.Entity("Cinema.Models.OrderDetails", b =>
                {
                    b.HasOne("Cinema.Models.OrderHeader", "OrderHeader")
                        .WithMany()
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Cinema.Models.Prenotazione", "Prenotazione")
                        .WithMany()
                        .HasForeignKey("PrenotazioneId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("OrderHeader");

                    b.Navigation("Prenotazione");
                });

            modelBuilder.Entity("Cinema.Models.OrderHeader", b =>
                {
                    b.HasOne("Cinema.Models.Utente", "Utente")
                        .WithMany()
                        .HasForeignKey("UtenteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Utente");
                });

            modelBuilder.Entity("Cinema.Models.Posto", b =>
                {
                    b.HasOne("Cinema.Models.Sala", "IdSalaNavigation")
                        .WithMany("Postos")
                        .HasForeignKey("IdSala")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("posto_ibfk_1");

                    b.Navigation("IdSalaNavigation");
                });

            modelBuilder.Entity("Cinema.Models.Prenotazione", b =>
                {
                    b.HasOne("Cinema.Models.Utente", "IdUtenteNavigation")
                        .WithMany("Prenotaziones")
                        .HasForeignKey("IdUtente")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("prenotazione_ibfk_2");

                    b.HasOne("Cinema.Models.Spettacolo", "Spettacolo")
                        .WithMany("Prenotaziones")
                        .HasForeignKey("DataS", "OraS", "IdSala")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("prenotazione_ibfk_1");

                    b.Navigation("IdUtenteNavigation");

                    b.Navigation("Spettacolo");
                });

            modelBuilder.Entity("Cinema.Models.ShoppingCart", b =>
                {
                    b.HasOne("Cinema.Models.Prenotazione", "prenotazione")
                        .WithMany()
                        .HasForeignKey("PrenotazioneId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Cinema.Models.Utente", "utente")
                        .WithMany()
                        .HasForeignKey("UtenteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("prenotazione");

                    b.Navigation("utente");
                });

            modelBuilder.Entity("Cinema.Models.Spettacolo", b =>
                {
                    b.HasOne("Cinema.Models.Film", "IdFilmNavigation")
                        .WithMany("Spettacolos")
                        .HasForeignKey("IdFilm")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("spettacolo_ibfk_2");

                    b.HasOne("Cinema.Models.Sala", "IdSalaNavigation")
                        .WithMany("Spettacolos")
                        .HasForeignKey("IdSala")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("spettacolo_ibfk_1");

                    b.Navigation("IdFilmNavigation");

                    b.Navigation("IdSalaNavigation");
                });

            modelBuilder.Entity("Cinema.Models.Valutazione", b =>
                {
                    b.HasOne("Cinema.Models.Film", "IdFilmNavigation")
                        .WithMany("Valutaziones")
                        .HasForeignKey("IdFilm")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("valutazione_ibfk_2");

                    b.HasOne("Cinema.Models.Utente", "IdUtenteNavigation")
                        .WithMany("Valutaziones")
                        .HasForeignKey("IdUtente")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("valutazione_ibfk_1");

                    b.Navigation("IdFilmNavigation");

                    b.Navigation("IdUtenteNavigation");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Cinema.Models.Film", b =>
                {
                    b.Navigation("Spettacolos");

                    b.Navigation("Valutaziones");
                });

            modelBuilder.Entity("Cinema.Models.Genere", b =>
                {
                    b.Navigation("Films");
                });

            modelBuilder.Entity("Cinema.Models.Posto", b =>
                {
                    b.Navigation("Comprendes");
                });

            modelBuilder.Entity("Cinema.Models.Prenotazione", b =>
                {
                    b.Navigation("Comprendes");
                });

            modelBuilder.Entity("Cinema.Models.Sala", b =>
                {
                    b.Navigation("Postos");

                    b.Navigation("Spettacolos");
                });

            modelBuilder.Entity("Cinema.Models.Spettacolo", b =>
                {
                    b.Navigation("Prenotaziones");
                });

            modelBuilder.Entity("Cinema.Models.Utente", b =>
                {
                    b.Navigation("Prenotaziones");

                    b.Navigation("Valutaziones");
                });
#pragma warning restore 612, 618
        }
    }
}
