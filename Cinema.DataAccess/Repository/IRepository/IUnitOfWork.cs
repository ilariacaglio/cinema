using System;
namespace Cinema.DataAccess.Repository.IRepository
{
	public interface IUnitOfWork
	{
		ISalaRepository Sala { get; }
        IPostoRepository Posto { get; }
		IGenereRepository Genere { get; }
		IFilmRepository Film { get; }
		ISpettacoloRepository Spettacolo { get; }
		IUtenteRepository Utente { get; }
		IValutazioneRepository Valutazione { get; }
		IPrenotazioneRepository Prenotazione { get; }
		IComprendeRepository Comprende { get; }
        void Save();
	}
}

