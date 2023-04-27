using System;
using System.Linq.Expressions;
using Cinema.DataAccess.Repository.IRepository;
using Cinema.Models;

namespace Cinema.DataAccess.Repository
{
	public class UtenteRepository: IRepository<Utente>, IUtenteRepository
    {
        private readonly AppDbContext _db;

        public UtenteRepository(AppDbContext db)
        {
            _db = db;
        }

        public void Add(Utente entity)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Utente> GetAll()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Utente> GetAll(Expression<Func<Utente, bool>>? filter = null, string? includeProperties = null)
        {
            throw new NotImplementedException();
        }

        public Utente? GetFirstOrDefault(int? id)
        {
            throw new NotImplementedException();
        }

        public Utente? GetFirstOrDefault(string? id)
        {
            if (id is null || id.Equals(String.Empty))
                return null;
            return _db.Utenti.Single(u => u.Id == id);
        }

        public void Remove(Utente entity)
        {
            throw new NotImplementedException();
        }

        public void RemoveRange(IEnumerable<Utente> entities)
        {
            throw new NotImplementedException();
        }
    }
}

