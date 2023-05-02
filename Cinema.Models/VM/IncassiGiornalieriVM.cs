using System;
namespace Cinema.Models.VM
{
	public class IncassiGiornalieriVM
	{
		public string? titoloFilm {get; set;}
		public DateOnly data { get; set; }
		public double incasso { get; set; }
	}
}