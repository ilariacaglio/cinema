using System;
using Cinema.DataAccess.Repository.IRepository;
using Cinema.Models;

namespace Cinema.DataAccess.Repository
{
	public class UtenteRepository: IRepository<Utente>, IUtenteRepository
    {
        public UtenteRepository(AppDbContext db) 
        {

        }

        public void Add(Utente entity)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Utente> GetAll()
        {
            throw new NotImplementedException();
        }

        public Utente? GetFirstOrDefault(int? id)
        {
            throw new NotImplementedException();
        }

        public Utente? GetFirstOrDefault(string? id)
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
    }
}

