using System;
namespace Cinema.DataAccess.Repository.IRepository
{
	public interface IUnitOfWork
	{
		ISalaRepository Sala { get; }
        IPostoRepository Posto { get; }
		IGenereRepository Genere { get; }
		IFilmRepository Film { get; }
		ISpettacoloRepository Spettacolo { get; }
        void Save();
	}
}

