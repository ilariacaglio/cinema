using System;
using Cinema.Models;

namespace Cinema.DataAccess.Repository.IRepository
{
	public interface IComprendeRepository:IRepository<Comprende>
    {
        public Comprende? GetFirstOrDefault(int? idPosto, int? idPrenotazione);
    }
}

