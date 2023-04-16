using System;
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

        public IEnumerable<Utente> GetAll()
        {
            return _db.Utenti.ToList();
        }

        public Utente? GetFirstOrDefault(string? id)
        {
            if (id == null)
                return null;
            return _db.Utenti.Single(u => u.Id == id);
        }


        //già gestiti da Identity
        //non verranno quindi implementati
        public void Add(Utente entity)
        {
            throw new NotImplementedException();
        }

        public void Remove(Utente entity)
        {
            throw new NotImplementedException();
        }

        public void RemoveRange(IEnumerable<Utente> entities)
        {
            throw new NotImplementedException();
        }

        public Utente? GetFirstOrDefault(int? id)
        {
            throw new NotImplementedException();
        }
    }
}

