using System;
using Cinema.Models;

namespace Cinema.DataAccess.Repository.IRepository
{
	public interface ISpettacoloRepository:IRepository<Spettacolo>
	{
        void Update(Spettacolo s);
        Spettacolo? GetFirstOrDefault(DateOnly data, TimeOnly ora, int? salaId);
    }
}