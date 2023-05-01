using System;
using System.Linq.Expressions;
using Cinema.DataAccess.Repository.IRepository;
using Cinema.Models;
using Microsoft.AspNetCore.Identity;

namespace Cinema.DataAccess.Repository
{
	public class UtenteRepository: IRepository<Utente>, IUtenteRepository
    {
        private readonly AppDbContext _db;

        public UtenteRepository(AppDbContext db)
        {
            _db = db;
        }

        public void Add(Utente entity)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IdentityUser> GetAllUsers()
        {
            var utenti = _db.Users.ToList();
            return utenti;
        }

        public IdentityRole? GetUserRole(string userId)
        {
            try
            {
                var IdRuolo = _db.UserRoles.Single(a => a.UserId == userId).RoleId;
                return _db.Roles.Single(r => r.Id == IdRuolo);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public IEnumerable<IdentityRole> GetRoles()
        {
            return _db.Roles.ToList();
        }

        public void UpdateRuolo (string utenteId, string ruoloId)
        {
            var userRole = _db.UserRoles.Single(u => u.UserId == utenteId);

            //eliminazione ruolo
            if (userRole != null)
            {
                _db.UserRoles.Remove(userRole);
                _db.SaveChanges();
            }

            //aggiunta nuovo ruolo
            var u = new IdentityUserRole<string>()
            {
                RoleId = ruoloId,
                UserId = utenteId
            };
            _db.UserRoles.Add(u);
            _db.SaveChanges();
            
        }

        public IEnumerable<Utente> GetAll(Expression<Func<Utente, bool>>? filter = null, string? includeProperties = null)
        {
            throw new NotImplementedException();
        }

        public Utente? GetFirstOrDefault(int? id)
        {
            throw new NotImplementedException();
        }

        public IdentityUser? GetFirstOrDefaultUser(string? id)
        {
            if (id is null || id.Equals(String.Empty))
                return null;
            return _db.Users.Single(u => u.Id == id);
        }

        public void Remove(Utente entity)
        {
            throw new NotImplementedException();
        }

        public void RemoveRange(IEnumerable<Utente> entities)
        {
            throw new NotImplementedException();
        }

        IEnumerable<Utente> IRepository<Utente>.GetAll()
        {
            throw new NotImplementedException();
        }

        public Utente? GetFirstOrDefault(string? id)
        {
            throw new NotImplementedException();
        }
    }
}

