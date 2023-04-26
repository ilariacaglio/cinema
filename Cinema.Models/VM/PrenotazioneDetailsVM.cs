using System;
namespace Cinema.Models.VM
{
	public class PrenotazioneDetailsVM
	{
		public Prenotazione prenotazione { get; set; }
		public List<Posto> postiPrenotati { get; set; } = new List<Posto>();
		public double costo { get; set; }
		public string imgFilm { get; set; }
	}
}