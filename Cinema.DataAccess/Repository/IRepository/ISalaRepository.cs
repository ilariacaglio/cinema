using System;
using Cinema.Models;

namespace Cinema.DataAccess.Repository.IRepository
{
	public interface ISalaRepository:IRepository<Sala>
	{
        void Update(Sala s);
    }
}

