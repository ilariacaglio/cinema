using System;
using Cinema.DataAccess.Repository.IRepository;
using Cinema.Models;
namespace Cinema.DataAccess.Repository
{
	public class OrderDetailRepository : Repository<OrderDetails>, IOrderDetailRepository
    {
        private readonly AppDbContext _db;
        public OrderDetailRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(OrderDetails orderDetail)
        {
            _db.OrderDetails.Update(orderDetail);
        }
    }
}