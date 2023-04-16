using System;
using Cinema.DataAccess.Repository.IRepository;
using Cinema.Models;

namespace Cinema.DataAccess.Repository
{
	public class PrenotazioneRepository : IRepository<Prenotazione>, IPrenotazioneRepository
    {
        private readonly AppDbContext _db;

        public PrenotazioneRepository(AppDbContext db)
        {
            _db = db;
        }

        public void Add(Prenotazione entity)
        {
            //gestisci i 4 posti a utente
            _db.Prenotazioni.Add(entity);
        }

        public IEnumerable<Prenotazione> GetAll()
        {
            return _db.Prenotazioni.ToList();
        }

        public Prenotazione? GetFirstOrDefault(int? id)
        {
            if (id is null)
                return null;
            return _db.Prenotazioni.Single(p => p.Id == id);
        }

        public void Remove(Prenotazione entity)
        {
            var prenotazioneFromDb = GetFirstOrDefault(entity.Id);
            if (prenotazioneFromDb != null)
                _db.Prenotazioni.Remove(prenotazioneFromDb);
        }

        public void RemoveRange(IEnumerable<Prenotazione> entities)
        {
            throw new NotImplementedException();
        }

        //non ci sono campi che ha senso aggiornare
        //necessaria una nuova prenotazione e la cancellazione della precedente
        public void Update(Prenotazione p)
        {
            throw new NotImplementedException();
        }
    }
}

