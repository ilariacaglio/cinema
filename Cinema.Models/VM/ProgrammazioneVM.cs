using System;
namespace Cinema.Models.VM
{
	public class ProgrammazioneVM
	{
		public int IdFilm { get; set; }
		public string? TitoloFilm { get; set; }
		public DateOnly DataInizio { get; set; }
		public DateOnly DataFine { get; set; }
		public string Genere { get; set; }
	}
}