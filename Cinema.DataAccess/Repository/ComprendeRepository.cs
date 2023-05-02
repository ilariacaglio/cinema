using System;
using System.Linq.Expressions;
using Cinema.DataAccess.Repository.IRepository;
using Cinema.Models;

namespace Cinema.DataAccess.Repository
{
	public class ComprendeRepository: IRepository<Comprende>, IComprendeRepository
    {
        private readonly AppDbContext _db;

        public ComprendeRepository(AppDbContext db)
        {
            _db = db;
        }

        public void Add(Comprende entity)
        {
            _db.Comprende.Add(entity);
        }

        public IEnumerable<Comprende> GetAll()
        {
            return _db.Comprende.ToList();
        }

        public IEnumerable<Comprende> GetAll(Expression<Func<Comprende, bool>>? filter = null, string? includeProperties = null)
        {
            throw new NotImplementedException();
        }

        public Comprende? GetFirstOrDefault(int? idPosto, int? idPrenotazione)
        {
            if (idPosto is null || idPrenotazione is null)
                return null;
            return _db.Comprende.Single(c => c.IdPosto == idPosto && c.IdPrenotazione == idPrenotazione);
        }

        public Comprende? GetFirstOrDefault(int? id)
        {
            throw new NotImplementedException();
        }

        public void Remove(Comprende entity)
        {
            var comprendeFromDb = GetFirstOrDefault(entity.IdPosto, entity.IdPrenotazione);
            if (comprendeFromDb != null)
                _db.Comprende.Remove(comprendeFromDb);
        }

        public void RemoveRange(IEnumerable<Comprende> entities)
        {
            throw new NotImplementedException();
        }
    }
}

