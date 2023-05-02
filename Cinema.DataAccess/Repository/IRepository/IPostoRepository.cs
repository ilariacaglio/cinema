using System;
using Cinema.Models;

namespace Cinema.DataAccess.Repository.IRepository
{
	public interface IPostoRepository : IRepository<Posto>
    {
        void Update(Posto p);
    }
}

