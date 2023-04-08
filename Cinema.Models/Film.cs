using System;
using System.Collections.Generic;

namespace Cinema.Models;

public partial class Film
{
    public int Id { get; set; }

    public string Titolo { get; set; } = null!;

    public int Durata { get; set; }

    public DateOnly Anno { get; set; }

    public string? Descrizione { get; set; }

    public string? Img { get; set; }

    public int IdGenere { get; set; }

    public virtual Genere IdGenereNavigation { get; set; } = null!;

    public virtual ICollection<Spettacolo> Spettacolos { get; } = new List<Spettacolo>();

    public virtual ICollection<Valutazione> Valutaziones { get; } = new List<Valutazione>();
}
