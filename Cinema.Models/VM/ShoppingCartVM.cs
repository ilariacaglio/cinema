using System;
namespace Cinema.Models.VM
{
	public class ShoppingCartVM
	{
        public IEnumerable<ShoppingCart> ListCart { get; set; } = null!;
        public OrderHeader OrderHeader { get; set; } = null!;
    }
}