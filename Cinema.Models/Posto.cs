using System;
using System.Collections.Generic;

namespace Cinema.Models;

public partial class Posto
{
    public int Id { get; set; }

    public int Fila { get; set; }

    public int Numero { get; set; }

    public double Costo { get; set; }

    public int IdSala { get; set; }

    public virtual ICollection<Comprende> Comprendes { get; } = new List<Comprende>();

    public virtual Sala IdSalaNavigation { get; set; } = null!;
}
