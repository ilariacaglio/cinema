using System;
namespace Cinema.DataAccess.Repository.IRepository
{
	public interface IUnitOfWork
	{
		ISalaRepository Sala { get; }
        IPostoRepository Posto { get; }
        void Save();
	}
}

