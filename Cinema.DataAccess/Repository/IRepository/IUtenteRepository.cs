using System;
using Cinema.Models;

namespace Cinema.DataAccess.Repository.IRepository
{
	public interface IUtenteRepository :IRepository<Utente>
	{
        public Utente? GetFirstOrDefault(string? id);
    }
}

