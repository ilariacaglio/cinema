using System;
using Cinema.Models;

namespace Cinema.DataAccess.Repository.IRepository
{
	public interface IFilmRepository: IRepository<Film>
	{
        void Update(Film f);
    }
}