using System;
using Cinema.Models;

namespace Cinema.DataAccess.Repository.IRepository
{
	public interface IGenereRepository: IRepository<Genere>
    {
        void Update(Genere g);
    }
}

