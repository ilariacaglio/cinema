using System;
namespace Cinema.Models.VM
{
	public class HomePageVM
	{
		public int idFilm { get; set; }
		public string NomeFilm { get; set; }
		public DateOnly Data { get; set; }
		public string Img { get; set; }
		public bool uscito { get; set; }
		public string Descrizione { get; set; }
	}
}

