﻿using System;
using System.Linq.Expressions;
using Cinema.DataAccess.Repository.IRepository;
using Cinema.Models;

namespace Cinema.DataAccess.Repository
{
	public class SalaRepository: IRepository<Sala>, ISalaRepository
    {
        private readonly AppDbContext _db;

        public SalaRepository(AppDbContext db)
        {
            _db = db;
        }

        public void Add(Sala entity)
        {
            _db.Sale.Add(entity);
        }

        public IEnumerable<Sala> GetAll()
        {
            return _db.Sale.ToList();
        }

        public Sala? GetFirstOrDefault(int? id)
        {
            return _db.Sale.Single(s => s.Id == id);
        }

        public void Remove(Sala entity)
        {
            var salaFromDb = GetFirstOrDefault(entity.Id);
            if (salaFromDb != null)
                _db.Sale.Remove(salaFromDb);
        }

        public void RemoveRange(IEnumerable<Sala> entities)
        {
            throw new NotImplementedException();
        }

        public void Update(Sala s)
        {
            var salaFromDb = GetFirstOrDefault(s.Id);
            if (salaFromDb != null)
            {
                salaFromDb.Isense = s.Isense;
                salaFromDb.Nfile = s.Nfile;
                salaFromDb.Nposti = s.Nposti;
            }    
        }
    }
}

