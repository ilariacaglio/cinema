using System;
namespace Cinema.Models.VM
{
	public class FilmVM
	{
		public Film film { get; set; }
		public IEnumerable<Spettacolo>? s { get; set; } 
		public double valutazione { get; set; }
	}
}