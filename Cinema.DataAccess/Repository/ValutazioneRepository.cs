using System;
using Cinema.DataAccess.Migrations;
using Cinema.DataAccess.Repository.IRepository;
using Cinema.Models;

namespace Cinema.DataAccess.Repository
{
	public class ValutazioneRepository : IRepository<Valutazione>, IValutazioneRepository
	{
        private readonly AppDbContext _db;

		public ValutazioneRepository(AppDbContext db)
		{
            _db = db;
		}

        public void Add(Valutazione entity)
        {
            _db.Valutazioni.Add(entity);
        }

        public IEnumerable<Valutazione> GetAll()
        {
            return _db.Valutazioni.ToList();
        }

        public Valutazione? GetFirstOrDefault(int? idFilm, string? idUtente)
        {
            if (idFilm is null || idUtente is null)
                return null;
            return _db.Valutazioni.Single(v => v.IdFilm == idFilm && v.IdUtente == idUtente);
        }

        public Valutazione? GetFirstOrDefault(int? id)
        {
            throw new NotImplementedException();
        }

        public void Remove(Valutazione entity)
        {
            var valutazioneFromDb = GetFirstOrDefault(entity.IdFilm, entity.IdUtente);
            if (valutazioneFromDb != null)
                _db.Valutazioni.Remove(valutazioneFromDb);
        }

        public void RemoveRange(IEnumerable<Valutazione> entities)
        {
            throw new NotImplementedException();
        }

        public void Update(Valutazione v)
        {
            var valutazioneFromDb = GetFirstOrDefault(v.IdFilm, v.IdUtente);
            if (valutazioneFromDb != null)
            {
                valutazioneFromDb.Voto = v.Voto;
            }
        }
    }
}

