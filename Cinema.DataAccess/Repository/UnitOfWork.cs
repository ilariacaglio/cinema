using System;
using Cinema.DataAccess.Repository.IRepository;

namespace Cinema.DataAccess.Repository
{
	public class UnitOfWork :IUnitOfWork
	{
        private readonly AppDbContext _db;
        public UnitOfWork(AppDbContext db)
		{
            _db = db;
            Sala = new SalaRepository(_db);
            Posto = new PostoRepository(_db);
        }

        public ISalaRepository Sala { get; private set; } = null!;
        public IPostoRepository Posto { get; private set; } = null!;

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}

