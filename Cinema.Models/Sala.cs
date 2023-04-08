using System;
using System.Collections.Generic;

namespace Cinema.Models;

public partial class Sala
{
    public int Id { get; set; }

    public int Nposti { get; set; }

    public int Nfile { get; set; }

    public bool Isense { get; set; }

    public virtual ICollection<Posto> Postos { get; } = new List<Posto>();

    public virtual ICollection<Spettacolo> Spettacolos { get; } = new List<Spettacolo>();
}
