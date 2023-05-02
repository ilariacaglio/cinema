using System;
using Cinema.Models;

namespace Cinema.DataAccess.Repository.IRepository
{
	public interface IOrderHeaderRepository: IRepository<OrderHeader>
	{
        OrderHeader? GetFirstOrDefault(int id);
        void Update(OrderHeader orderHeader);
        void UpdateStatus(int id, string orderStatus, string? paymentStatus = null);
        public void UpdateStripeSessionId(int id, string sessionId);
        public void UpdateStripePaymentIntentId(int id, string paymentIntentId);
    }
}