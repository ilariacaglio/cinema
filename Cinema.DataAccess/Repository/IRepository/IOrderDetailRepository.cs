using System;
using Cinema.Models;

namespace Cinema.DataAccess.Repository.IRepository
{
	public interface IOrderDetailRepository : IRepository<OrderDetails>
    {
        void Update(OrderDetails orderDetail);
    }
}

