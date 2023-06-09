﻿using System;
using System.Linq.Expressions;
using Cinema.DataAccess.Repository.IRepository;
using Cinema.Models;

namespace Cinema.DataAccess.Repository
{
	public class PrenotazioneRepository : IRepository<Prenotazione>, IPrenotazioneRepository
    {
        private readonly AppDbContext _db;

        public PrenotazioneRepository(AppDbContext db)
        {
            _db = db;
        }

        public void Add(Prenotazione entity)
        {
            _db.Prenotazioni.Add(entity);
        }

        public IEnumerable<Prenotazione> GetAll()
        {
            return _db.Prenotazioni.ToList();
        }

        public IEnumerable<Prenotazione> GetAll(Expression<Func<Prenotazione, bool>>? filter = null, string? includeProperties = null)
        {
            throw new NotImplementedException();
        }

        public Prenotazione? GetFirstOrDefault(int? id)
        {
            if (id is null)
                return null;
            return _db.Prenotazioni.Single(p => p.Id == id);
        }

        public void Remove(Prenotazione entity)
        {
            var prenotazioneFromDb = GetFirstOrDefault(entity.Id);
            if (prenotazioneFromDb != null)
                _db.Prenotazioni.Remove(prenotazioneFromDb);
        }

        public void RemoveRange(IEnumerable<Prenotazione> entities)
        {
            throw new NotImplementedException();
        }

        public void Update(Prenotazione p)
        {
            var prenotazioneFromDb = GetFirstOrDefault(p.Id);
            if (prenotazioneFromDb != null)
                prenotazioneFromDb.Pagato = p.Pagato;
        }
    }
}