using System;
using Cinema.Models;
using Cinema.DataAccess.Repository.IRepository;

namespace Cinema.DataAccess.Repository
{
	public class PostoRepository: IRepository<Posto>, IPostoRepository
	{
		private readonly AppDbContext _db;

        public PostoRepository(AppDbContext db)
        {
            _db = db;
        }

        public void Add(Posto entity)
        {
            var sala = _db.Sale.Single(s => s.Id == entity.IdSala);
            if (sala.Nfile >= entity.Fila && sala.Nposti >= entity.Numero)
            {
                if (sala.Isense && entity.Fila >= 5)
                    entity.Costo += 2;
                _db.Posti.Add(entity);
            }
        }

        public IEnumerable<Posto> GetAll()
        {
            return _db.Posti.ToList();
        }

        public Posto? GetFirstOrDefault(int? id)
        {
            throw new NotImplementedException();
        }

        public Posto? GetFirstOrDefault(int? id, int? salaId)
        {
            return _db.Posti.Single(p => p.Id == id && p.IdSala == salaId);
        }

        public void Remove(Posto entity) 
        {
            var postoFromDb = GetFirstOrDefault(entity.Id, entity.IdSala);
            if (postoFromDb != null)
                _db.Posti.Remove(postoFromDb);
        }

        public void RemoveRange(IEnumerable<Posto> entities)
        {
            throw new NotImplementedException();
        }

        public void Update(Posto p)
        {
            var postoFromDb = GetFirstOrDefault(p.Id, p.IdSala);
            if (postoFromDb != null)
            {
                postoFromDb.Fila = p.Fila;
                postoFromDb.Numero = p.Numero;
                postoFromDb.Costo = p.Costo;
            }
        }
    }
}

