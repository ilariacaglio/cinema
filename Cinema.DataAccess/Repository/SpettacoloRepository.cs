﻿using System;
using System.Linq.Expressions;
using Cinema.DataAccess.Repository.IRepository;
using Cinema.Models;

namespace Cinema.DataAccess.Repository
{
	public class SpettacoloRepository: IRepository<Spettacolo>, ISpettacoloRepository
	{
        private readonly AppDbContext _db;

        public SpettacoloRepository(AppDbContext db)
        {
            _db = db;
        }

        public void Add(Spettacolo entity)
        {
            _db.Spettacoli.Add(entity);
        }

        public IEnumerable<Spettacolo> GetAll()
        {
            return _db.Spettacoli.ToList();
        }

        public IEnumerable<Spettacolo> GetAll(Expression<Func<Spettacolo, bool>>? filter = null, string? includeProperties = null)
        {
            return _db.Spettacoli.ToList();
        }

        public Spettacolo? GetFirstOrDefault(DateOnly data, TimeOnly ora, int? salaId)
        {
            try
            {
                var spettacolo = _db.Spettacoli.Single(s => s.IdSala == salaId && s.Data == data && s.Ora == ora);
                return spettacolo;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public Spettacolo? GetFirstOrDefault(int? id)
        {
            throw new NotImplementedException();
        }

        public void Remove(Spettacolo entity)
        {
            var spettacoloFromDb = GetFirstOrDefault(entity.Data, entity.Ora, entity.IdSala);
            if (spettacoloFromDb != null)
                _db.Spettacoli.Remove(spettacoloFromDb);
        }

        public void RemoveRange(IEnumerable<Spettacolo> entities)
        {
            throw new NotImplementedException();
        }

        public void Update(Spettacolo s)
        {
            var spettacoloFromDb = GetFirstOrDefault(s.Data, s.Ora, s.IdSala);
            if (spettacoloFromDb != null)
            {
                spettacoloFromDb.IdFilm = s.IdFilm;
                spettacoloFromDb.Data = s.Data;
                spettacoloFromDb.Ora = s.Ora;
                spettacoloFromDb.IdSala = s.IdSala;
            }
        }
    }
}

