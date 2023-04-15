using System;
using Cinema.DataAccess.Repository.IRepository;
using Cinema.Models;

namespace Cinema.DataAccess.Repository
{
    public class FilmRepository : IRepository<Film>, IFilmRepository
    {
        private readonly AppDbContext _db;

        public FilmRepository(AppDbContext db)
        {
            _db = db;
        }

        public void Add(Film entity)
        {
            _db.Film.Add(entity);
        }

        public IEnumerable<Film> GetAll()
        {
            return _db.Film.ToList();
        }

        public Film? GetFirstOrDefault(int? id)
        {
            if (id is null)
                return null;
            return _db.Film.Single(f => f.Id == id);
        }

        public void Remove(Film entity)
        {
            var filmFromDb = GetFirstOrDefault(entity.Id);
            if (filmFromDb != null)
                _db.Film.Remove(filmFromDb);
        }

        public void RemoveRange(IEnumerable<Film> entities)
        {
            throw new NotImplementedException();
        }

        public void Update(Film f)
        {
            var filmFromDb = GetFirstOrDefault(f.Id);
            if (filmFromDb != null)
            {
                filmFromDb.Titolo = f.Titolo;
                filmFromDb.Durata = f.Durata;
                filmFromDb.Anno = f.Anno;
                filmFromDb.Descrizione = f.Descrizione;
                filmFromDb.IdGenere = f.IdGenere;
                if (f.Img != null)
                    filmFromDb.Img = f.Img;
            }
        }
    }
}