using System;
using System.Collections.Generic;
using Cinema.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Cinema.DataAccess;

public partial class AppDbContext : IdentityDbContext<IdentityUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Comprende> Comprende { get; set; }

    public virtual DbSet<Film> Film { get; set; }

    public virtual DbSet<Genere> Generi { get; set; }

    public virtual DbSet<Posto> Posti { get; set; }

    public virtual DbSet<Prenotazione> Prenotazioni { get; set; }

    public virtual DbSet<Sala> Sale { get; set; }

    public virtual DbSet<Spettacolo> Spettacoli { get; set; }

    public virtual DbSet<Utente> Utenti { get; set; }

    public virtual DbSet<Valutazione> Valutazioni { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_general_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Comprende>(entity =>
        {
            entity.HasKey(e => new { e.IdPosto, e.IdPrenotazione})
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0});

            entity.ToTable("Comprende");

            entity.HasIndex(e => new { e.IdPrenotazione}, "IdPrenotazione");

            entity.Property(e => e.IdPosto).HasColumnType("int(11)");
            entity.Property(e => e.IdPrenotazione).HasColumnType("int(11)");

            entity.HasOne(d => d.Id).WithMany(p => p.Comprendes)
                .HasForeignKey(d => new { d.IdPosto})
                .HasConstraintName("comprende_ibfk_1");

            entity.HasOne(d => d.Prenotazione).WithMany(p => p.Comprendes)
                .HasForeignKey(d => new { d.IdPrenotazione })
                .HasConstraintName("comprende_ibfk_2");
        });

        modelBuilder.Entity<Film>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Film");

            entity.HasIndex(e => e.IdGenere, "IdGenere");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.Descrizione).HasMaxLength(500);
            entity.Property(e => e.Durata).HasColumnType("int(11)");
            entity.Property(e => e.IdGenere).HasColumnType("int(11)");
            entity.Property(e => e.Img).HasMaxLength(200);
            entity.Property(e => e.Titolo).HasMaxLength(50);

            entity.HasOne(d => d.IdGenereNavigation).WithMany(p => p.Films)
                .HasForeignKey(d => d.IdGenere)
                .HasConstraintName("film_ibfk_1");
        });

        modelBuilder.Entity<Genere>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Genere");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.Nome).HasMaxLength(20);
        });

        modelBuilder.Entity<Posto>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Posto");

            entity.HasIndex(e => e.IdSala, "IdSala");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.Costo).HasColumnType("double(4,2)");
            entity.Property(e => e.Fila).HasColumnType("int(11)");
            entity.Property(e => e.IdSala).HasColumnType("int(11)");
            entity.Property(e => e.Numero).HasColumnType("int(11)");

            entity.HasOne(d => d.IdSalaNavigation).WithMany(p => p.Postos)
                .HasForeignKey(d => d.IdSala)
                .HasConstraintName("posto_ibfk_1");
        });

        modelBuilder.Entity<Prenotazione>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Prenotazione");

            entity.HasIndex(e => new { e.DataS, e.OraS, e.IdSala }, "DataS");

            entity.HasIndex(e => e.IdUtente, "IdUtente");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.IdSala).HasColumnType("int(11)");
            entity.Property(e => e.IdUtente).HasMaxLength(50);
            entity.Property(e => e.OraS).HasColumnType("time");

            entity.HasOne(d => d.IdUtenteNavigation).WithMany(p => p.Prenotaziones)
                .HasForeignKey(d => d.IdUtente)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("prenotazione_ibfk_2");

            entity.HasOne(d => d.Spettacolo).WithMany(p => p.Prenotaziones)
                .HasForeignKey(d => new { d.DataS, d.OraS, d.IdSala })
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("prenotazione_ibfk_1");
        });

        modelBuilder.Entity<Sala>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Sala");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.Nfile).HasColumnType("int(11)");
            entity.Property(e => e.Nposti).HasColumnType("int(11)");
        });

        modelBuilder.Entity<Spettacolo>(entity =>
        {
            entity.HasKey(e => new { e.Data, e.Ora, e.IdSala })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0, 0 });

            entity.ToTable("Spettacolo");

            entity.HasIndex(e => e.IdFilm, "IdFilm");

            entity.HasIndex(e => e.IdSala, "IdSala");

            entity.Property(e => e.Ora).HasColumnType("time");
            entity.Property(e => e.IdSala).HasColumnType("int(11)");
            entity.Property(e => e.IdFilm).HasColumnType("int(11)");

            entity.HasOne(d => d.IdFilmNavigation).WithMany(p => p.Spettacolos)
                .HasForeignKey(d => d.IdFilm)
                .HasConstraintName("spettacolo_ibfk_2");

            entity.HasOne(d => d.IdSalaNavigation).WithMany(p => p.Spettacolos)
                .HasForeignKey(d => d.IdSala)
                .HasConstraintName("spettacolo_ibfk_1");
        });

        modelBuilder.Entity<Utente>(entity =>
        {
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.Cognome).HasMaxLength(20);
            entity.Property(e => e.Nome).HasMaxLength(20);
            entity.Property(e => e.PasswordHash).HasMaxLength(50);
            entity.Property(e => e.Residenza).HasMaxLength(50);
            entity.Property(e => e.Sesso).HasMaxLength(1);
        });

        modelBuilder.Entity<Valutazione>(entity =>
        {
            entity.HasKey(e => new { e.IdUtente, e.IdFilm })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("Valutazione");

            entity.HasIndex(e => e.IdFilm, "IdFilm");

            entity.Property(e => e.IdUtente).HasMaxLength(50);
            entity.Property(e => e.IdFilm).HasColumnType("int(11)");
            entity.Property(e => e.Voto).HasColumnType("double(2,1)");

            entity.HasOne(d => d.IdFilmNavigation).WithMany(p => p.Valutaziones)
                .HasForeignKey(d => d.IdFilm)
                .HasConstraintName("valutazione_ibfk_2");

            entity.HasOne(d => d.IdUtenteNavigation).WithMany(p => p.Valutaziones)
                .HasForeignKey(d => d.IdUtente)
                .HasConstraintName("valutazione_ibfk_1");
        });

        base.OnModelCreating(modelBuilder);
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
