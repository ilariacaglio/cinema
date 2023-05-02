using System;
using Cinema.Models;

namespace Cinema.DataAccess.Repository.IRepository
{
	public interface IPrenotazioneRepository : IRepository<Prenotazione>
    {
        void Update(Prenotazione p);
    }
}