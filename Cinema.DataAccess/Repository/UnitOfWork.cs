using System;
using Cinema.DataAccess.Repository.IRepository;

namespace Cinema.DataAccess.Repository
{
	public class UnitOfWork :IUnitOfWork
	{
        private readonly AppDbContext _db;
        public UnitOfWork(AppDbContext db)
		{
            _db = db;
            Sala = new SalaRepository(_db);
            Posto = new PostoRepository(_db);
            Genere = new GenereRepository(_db);
            Film = new FilmRepository(_db);
            Spettacolo = new SpettacoloRepository(_db);
            Utente = new UtenteRepository(_db);
            Valutazione = new ValutazioneRepository(_db);
        }

        public ISalaRepository Sala { get; private set; } = null!;
        public IPostoRepository Posto { get; private set; } = null!;
        public IGenereRepository Genere { get; private set; } = null!;
        public IFilmRepository Film { get; private set; } = null!;
        public ISpettacoloRepository Spettacolo { get; private set; } = null!;
        public IUtenteRepository Utente { get; private set; } = null!;
        public IValutazioneRepository Valutazione { get; private set; } = null!;

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}