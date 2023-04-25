using System;
using Cinema.DataAccess.Repository.IRepository;
using Cinema.Models;

namespace Cinema.DataAccess.Repository
{
	public class ShoppingCartRepository : Repository<ShoppingCart>, IShoppingCartRepository
    {
		public ShoppingCartRepository(AppDbContext db):base(db)
        {
		}

        
    }
}

