using System;
using System.Linq.Expressions;
using Cinema.DataAccess.Repository.IRepository;
using Cinema.Models;

namespace Cinema.DataAccess.Repository
{
	public class GenereRepository: IRepository<Genere>, IGenereRepository
    {
        private readonly AppDbContext _db;

        public GenereRepository(AppDbContext db)
        {
            _db = db;
        }

        public void Add(Genere entity)
        {
            _db.Generi.Add(entity);
        }

        public IEnumerable<Genere> GetAll()
        {
            return _db.Generi.ToList();
        }

        public IEnumerable<Genere> GetAll(Expression<Func<Genere, bool>>? filter = null, string? includeProperties = null)
        {
            throw new NotImplementedException();
        }

        public Genere? GetFirstOrDefault(int? id)
        {
            if (id is null)
                return null;
            return _db.Generi.Single(g => g.Id == id);
        }

        public void Remove(Genere entity)
        {
            var genereFromDb = GetFirstOrDefault(entity.Id);
            if (genereFromDb != null)
                _db.Generi.Remove(genereFromDb);
        }

        public void RemoveRange(IEnumerable<Genere> entities)
        {
            throw new NotImplementedException();
        }

        public void Update(Genere g)
        {
            var genereFromDb = GetFirstOrDefault(g.Id);
            if (genereFromDb != null)
            {
                genereFromDb.Nome = g.Nome;
            }
        }
    }
}

