using System;
using Cinema.Models;

namespace Cinema.DataAccess.Repository.IRepository
{
	public interface IValutazioneRepository: IRepository<Valutazione>
	{
        void Update(Valutazione v);
        public Valutazione? GetFirstOrDefault(int? idFilm, string? idUtente);
    }
}