using System;
using Cinema.DataAccess.Repository.IRepository;
using Cinema.Models;

namespace Cinema.DataAccess.Repository
{
	public class ShoppingCartRepository : Repository<ShoppingCart>, IShoppingCartRepository
    {
        private readonly AppDbContext _db;

        public ShoppingCartRepository(AppDbContext db):base(db)
        {
            _db = db;
        }

        public ShoppingCart? GetFirst(int? id) {
            if (id is null)
                return null;
            return _db.ShoppingCarts.Single(s => s.Id == id);
        }
    }
}

