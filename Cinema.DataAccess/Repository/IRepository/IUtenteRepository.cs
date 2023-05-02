using System;
using Cinema.Models;
using Microsoft.AspNetCore.Identity;

namespace Cinema.DataAccess.Repository.IRepository
{
	public interface IUtenteRepository :IRepository<Utente>
	{
        public Utente? GetFirstOrDefault(string? id);
        public IdentityRole? GetUserRole(string userId);
        public IEnumerable<IdentityUser> GetAllUsers();
        public IdentityUser? GetFirstOrDefaultUser(string? id);
        public IEnumerable<IdentityRole> GetRoles();
        public void UpdateRuolo(string utenteId, string ruoloId);
    }
}

