using System;
using Cinema.DataAccess.Repository.IRepository;
using Cinema.Models;

namespace Cinema.DataAccess.Repository
{
	public class OrderHeaderRepository : Repository<OrderHeader>, IOrderHeaderRepository
    {
        private readonly AppDbContext _db;
        public OrderHeaderRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }

        public OrderHeader? GetFirstOrDefault(int id)
        {
            if (id == 0)
                return null;
            return _db.OrderHeaders.Single(o => o.Id == id);
        }

        public void Update(OrderHeader orderHeader)
        {
            _db.OrderHeaders.Update(orderHeader);
        }

        public void UpdateStatus(int id, string orderStatus, string? paymentStatus = null)
        {
            var orderFromDb = _db.OrderHeaders.FirstOrDefault(u => u.Id == id);
            if (orderFromDb != null)
            {
                orderFromDb.StatoOrdine = orderStatus;
                if (paymentStatus != null)
                    orderFromDb.StatoPagamento = paymentStatus;
            }
        }

        public void UpdateStripeSessionId(int id, string sessionId)
        {
            var orderFromDb = _db.OrderHeaders.FirstOrDefault(u => u.Id == id);
            if (orderFromDb != null)
            {
                orderFromDb.SessionId = sessionId;
            }
        }
        public void UpdateStripePaymentIntentId(int id, string paymentIntentId)
        {
            var orderFromDb = _db.OrderHeaders.FirstOrDefault(u => u.Id == id);
            if (orderFromDb != null)
            {
                orderFromDb.PaymentIntentId = paymentIntentId;
            }
        }

    }
}