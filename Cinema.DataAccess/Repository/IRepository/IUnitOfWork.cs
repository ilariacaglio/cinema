using System;
namespace Cinema.DataAccess.Repository.IRepository
{
	public interface IUnitOfWork
	{
		ISalaRepository Sala { get; }
		void Save();
	}
}

